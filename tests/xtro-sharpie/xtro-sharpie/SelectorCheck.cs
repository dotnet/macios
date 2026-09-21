//
// The rule reports
//
// !missing-selector!
//             if headers defines a selector for which we have no bindings
//
// !unknown-selector!
//             if we have a managed [Export] selector for which the current SDK headers have no
//             matching native declaration (e.g. Apple removed or privatized the selector)
//

using System.Diagnostics;
using System.Runtime.Serialization;

namespace Extrospection {

	public class SelectorCheck : BaseVisitor {

		const int FactoryMethodBindingImplOption = 4;

		HashSet<string> qualified_selectors = new HashSet<string> ();
		Dictionary<string, List<Tuple<MethodDefinition, Helpers.ArgumentSemantic>>> qualified_properties = new Dictionary<string, List<Tuple<MethodDefinition, Helpers.ArgumentSemantic>>> ();

		// Reverse check (managed -> native): report managed [Export] selectors that no longer have
		// a native declaration in the current SDK headers (e.g. Apple removed or privatized the
		// selector) so the stale binding can be removed.
		//
		// A managed selector 'Type::selector' is considered valid if 'selector' is part of the
		// *effective* set of selectors for the native class 'Type', which is:
		//   * the selectors declared directly on the class (and its categories);
		//   * plus the selectors declared on every base class (and their categories);
		//   * plus the selectors of every protocol adopted by the class or its base classes -
		//     both the protocols declared in the headers and the protocols the managed binding
		//     declares conformance to (Apple often implements a protocol, e.g. NSCoding/NSCopying,
		//     without declaring the conformance on the class) - transitively through protocol
		//     inheritance.
		// This mirrors how managed bindings inline category and protocol members into a class, so
		// those aren't mis-reported as extra, while still catching a selector that Apple removed
		// from a specific class even if the same selector name still exists on some other class.

		// native class (or category base) name -> selectors declared on it (with '+' for class selectors)
		Dictionary<string, HashSet<string>> type_selectors = new (StringComparer.Ordinal);
		// native protocol name -> selectors declared on it (with '+' for class selectors)
		Dictionary<string, HashSet<string>> protocol_selectors = new (StringComparer.Ordinal);
		// native class name -> its superclass name
		Dictionary<string, string> type_super = new (StringComparer.Ordinal);
		// native class name -> protocols it adopts (from the interface and its categories)
		Dictionary<string, HashSet<string>> type_protocols = new (StringComparer.Ordinal);
		// native protocol name -> protocols it inherits from
		Dictionary<string, HashSet<string>> protocol_bases = new (StringComparer.Ordinal);
		// native class name -> protocols the *managed* binding declares it conforms to. Apple often
		// omits a protocol conformance (e.g. NSCoding, NSCopying) from a class' header even though
		// the class implements it, so we trust the managed binding for the set of adopted protocols.
		Dictionary<string, HashSet<string>> managed_type_protocols = new (StringComparer.Ordinal);
		// native class names whose definition we've seen (so we can tell a removed selector apart
		// from a removed type - the latter is reported by the '!unknown-type!' check instead)
		HashSet<string> known_types = new (StringComparer.Ordinal);
		// memoized effective selector sets, keyed by native class name
		Dictionary<string, HashSet<string>> effective_selectors = new (StringComparer.Ordinal);
		// Frameworks that were actually parsed natively. Managed selectors from frameworks that
		// weren't parsed (e.g. excluded from the sharpie run) must not be reported.
		HashSet<string> parsed_frameworks = new HashSet<string> (StringComparer.Ordinal);
		List<(string Framework, string QualifiedName, string Type, string Selector)> managed_selectors = new List<(string, string, string, string)> ();

		public SelectorCheck (BindingResult bindingResult)
			: base (bindingResult)
		{
		}

		static HashSet<string> GetOrCreate (Dictionary<string, HashSet<string>> map, string key)
		{
			if (!map.TryGetValue (key, out var set))
				map [key] = set = new HashSet<string> (StringComparer.Ordinal);
			return set;
		}

		void AddNativeSelector (Decl? container, string? selector, bool isClassMember)
		{
			if (container is null || string.IsNullOrEmpty (selector))
				return;

			var value = isClassMember ? "+" + selector : selector;
			switch (container) {
			case ObjCProtocolDecl protocol:
				GetOrCreate (protocol_selectors, protocol.Name).Add (value);
				break;
			case ObjCCategoryDecl category:
				GetOrCreate (type_selectors, category.ClassInterface.Name).Add (value);
				break;
			case ObjCInterfaceDecl @interface:
				GetOrCreate (type_selectors, @interface.Name).Add (value);
				break;
			}

			var framework = Helpers.GetFramework (container);
			if (framework is not null)
				parsed_frameworks.Add (framework);
		}

		// Computes (and memoizes) the effective set of selectors reachable from a native class:
		// its own selectors plus those of all its base classes and all adopted protocols.
		HashSet<string> GetEffectiveSelectors (string typeName)
		{
			if (effective_selectors.TryGetValue (typeName, out var cached))
				return cached;

			var result = new HashSet<string> (StringComparer.Ordinal);
			var protocols = new HashSet<string> (StringComparer.Ordinal);

			// walk the class hierarchy, collecting selectors and adopted protocols
			var seenTypes = new HashSet<string> (StringComparer.Ordinal);
			var current = typeName;
			while (current is not null && seenTypes.Add (current)) {
				if (type_selectors.TryGetValue (current, out var sels))
					result.UnionWith (sels);
				if (type_protocols.TryGetValue (current, out var protos))
					protocols.UnionWith (protos);
				if (managed_type_protocols.TryGetValue (current, out var mprotos))
					protocols.UnionWith (mprotos);
				type_super.TryGetValue (current, out current);
			}

			// expand the adopted protocols transitively and collect their selectors
			var pending = new Queue<string> (protocols);
			var seenProtocols = new HashSet<string> (StringComparer.Ordinal);
			while (pending.Count > 0) {
				var protocol = pending.Dequeue ();
				if (!seenProtocols.Add (protocol))
					continue;
				if (protocol_selectors.TryGetValue (protocol, out var sels))
					result.UnionWith (sels);
				if (protocol_bases.TryGetValue (protocol, out var bases)) {
					foreach (var b in bases)
						pending.Enqueue (b);
				}
			}

			effective_selectors [typeName] = result;
			return result;
		}

		// We only report extra selectors for publicly visible types: internal helper types (e.g. the
		// various managed dispatcher/proxy types) use fabricated selectors that never map to a native
		// declaration and would be permanent noise. This mirrors how the '!unknown-type!' check skips
		// internal types.
		static bool IsPubliclyVisible (TypeDefinition type)
		{
			if (type.IsNested)
				return (type.IsNestedPublic || type.IsNestedFamily || type.IsNestedFamilyOrAssembly) && IsPubliclyVisible (type.DeclaringType);
			return type.IsPublic;
		}

		static bool IsFactoryMethod (MethodDefinition method)
		{
			foreach (var ca in method.CustomAttributes) {
				if (ca.Constructor.DeclaringType.Name != "BindingImplAttribute")
					continue;
				var options = (int) ca.ConstructorArguments [0].Value;
				return (options & FactoryMethodBindingImplOption) == FactoryMethodBindingImplOption;
			}
			return false;
		}

		static bool IsInitializerSelector (string selector)
		{
			if (selector == "init")
				return true;
			return selector.Length > 4 && selector.StartsWith ("init", StringComparison.Ordinal) && char.IsUpper (selector [4]);
		}

		// splits '[+]Type::selector' into the native type name and the selector (keeping the leading
		// '+' on the selector for class selectors)
		static bool TrySplitQualifiedName (string qualifiedName, out string type, out string selector)
		{
			type = selector = "";
			var isStatic = qualifiedName.Length > 0 && qualifiedName [0] == '+';
			var body = isStatic ? qualifiedName.Substring (1) : qualifiedName;
			var idx = body.IndexOf ("::", StringComparison.Ordinal);
			if (idx < 0)
				return false;
			type = body.Substring (0, idx);
			selector = (isStatic ? "+" : "") + body.Substring (idx + 2);
			return true;
		}

		// collect the protocols the managed binding declares each type conforms to
		public override void VisitManagedType (TypeDefinition type)
		{
			if (!type.HasInterfaces)
				return;

			// skip protocols themselves - we only care about concrete classes here
			if (type.IsProtocol ())
				return;

			var nativeName = type.GetName ();
			if (string.IsNullOrEmpty (nativeName))
				return;

			foreach (var iface in type.Interfaces) {
				var ifaceType = iface.InterfaceType.Resolve ();
				if (ifaceType is null)
					continue;
				var protocolName = ObjCInterfaceCheck.GetProtocolName (ifaceType);
				if (!string.IsNullOrEmpty (protocolName))
					GetOrCreate (managed_type_protocols, nativeName).Add (protocolName);
			}
		}

		// most selectors will be found in [Export] attribtues
		public override void VisitManagedMethod (MethodDefinition method)
		{
			if (!method.HasCustomAttributes)
				return;

			var type = method.DeclaringType;
			// we do not process protocols here
			if (type.IsProtocol ())
				return;

			foreach (var ca in method.CustomAttributes) {
				switch (ca.Constructor.DeclaringType.Name) {
				case "ExportAttribute":
					var methodDefinition = method.GetName ();
					if (!string.IsNullOrEmpty (methodDefinition)) {
						var argumentSemantic = Helpers.ArgumentSemantic.Assign; // Default
						if (ca.ConstructorArguments.Count > 1) {
							argumentSemantic = (Helpers.ArgumentSemantic) ca.ConstructorArguments [1].Value;
							if (!qualified_properties.TryGetValue (methodDefinition, out var list))
								qualified_properties [methodDefinition] = list = new List<Tuple<MethodDefinition, Helpers.ArgumentSemantic>> ();
							list.Add (new Tuple<MethodDefinition, Helpers.ArgumentSemantic> (method, argumentSemantic));
						}

						qualified_selectors.Add (methodDefinition);

						// record it for the reverse (managed -> native) check (public API only)
						if (IsPubliclyVisible (type) && TrySplitQualifiedName (methodDefinition, out var selectorType, out var selector))
							managed_selectors.Add ((Helpers.GetFramework (type), methodDefinition, selectorType, selector));
					}

					break;
				}
			}

			if (!IsFactoryMethod (method) || !method.HasBody)
				return;

			var typeName = type.GetName ();
			if (string.IsNullOrEmpty (typeName))
				return;

			foreach (var instruction in method.Body.Instructions) {
				if (instruction.OpCode.Code != Mono.Cecil.Cil.Code.Ldstr || instruction.Operand is not string selector || !IsInitializerSelector (selector))
					continue;

				var methodDefinition = $"{typeName}::{selector}";
				qualified_selectors.Add (methodDefinition);
				if (IsPubliclyVisible (type))
					managed_selectors.Add ((Helpers.GetFramework (type), methodDefinition, typeName, selector));
			}
		}

		public override void VisitObjCPropertyDecl (ObjCPropertyDecl decl)
		{
			// collect the native property accessor selectors for the reverse (managed -> native)
			// check (including protocol members, so inlined protocol properties aren't mis-reported)
			AddNativeSelector (decl.DeclContext as Decl, decl.GetterMethodDecl?.GetSelector (), decl.GetterMethodDecl?.IsClassMethod == true);
			AddNativeSelector (decl.DeclContext as Decl, decl.SetterMethodDecl?.GetSelector (), decl.SetterMethodDecl?.IsClassMethod == true);

			// protocol members are checked in ObjCProtocolCheck
			if (decl.DeclContext is ObjCProtocolDecl)
				return;

			// check availability macros to see if the API is available on the OS and not deprecated
			if (!decl.IsAvailable ())
				return;

			var framework = Helpers.GetFramework (decl);
			if (framework is null)
				return;

			var nativeArgumentSemantic = decl.GetPropertyAttributes ().ToArgumentSemantic ();
			var nativeMethodDefinition = decl.QualifiedName;

			if (qualified_properties.TryGetValue (nativeMethodDefinition, out var managedArgumentSemanticList)) {
				foreach (var entry in managedArgumentSemanticList) {
					var method = entry.Item1;
					var managedArgumentSemantic = entry.Item2;

					if (managedArgumentSemantic != nativeArgumentSemantic) {
						// FIXME: only Copy mistakes are reported now
						if (managedArgumentSemantic == Helpers.ArgumentSemantic.Copy || nativeArgumentSemantic == Helpers.ArgumentSemantic.Copy) {
							// FIXME: rule disactivated for now
							// Log.On (framework).Add ($"!incorrect-argument-semantic! Native '{nativeMethodDefinition}' is declared as ({nativeArgumentSemantic.ToUsableString ().ToLowerInvariant ()}) but mapped to 'ArgumentSemantic.{managedArgumentSemantic.ToUsableString ()}' in '{method}'");
						}
					}
				}
			}
		}

		public override void VisitObjCMethodDecl (ObjCMethodDecl decl)
		{
			// collect the native selector for the reverse (managed -> native) check (including
			// protocol and category members, so inlined selectors aren't mis-reported as extra)
			AddNativeSelector (decl.DeclContext as Decl, decl.GetSelector (), decl.IsClassMethod);

			// protocol members are checked in ObjCProtocolCheck
			if (decl.DeclContext is ObjCProtocolDecl)
				return;

			// don't process methods (or types) that are unavailable for the current platform
			if (!decl.IsAvailable () || !((Decl) decl.DeclContext!).IsAvailable ())
				return;

			// don't process deprecated methods (or types)
			if (decl.IsDeprecated () || ((Decl) decl.DeclContext!).IsDeprecated ())
				return;

			var framework = Helpers.GetFramework (decl);
			if (framework is null)
				return;

			string selector = decl.GetSelector ();
			if (String.IsNullOrEmpty (selector))
				return;

			var name = decl.QualifiedName;
			if (decl.IsClassMethod) {
				// we do not bind `+{type}:new` just instance `init`
				if (selector == "new")
					return;
				name = "+" + name;
			}
			bool found = qualified_selectors.Contains (name);
			if (!found) {
				// a category could be inlined into the type it extend
				var category = decl.DeclContext as ObjCCategoryDecl;
				if (category is not null) {
					var cname = category.Name;
					if (cname is null)
						name = GetCategoryBase (category) + name;
					else
						name = name.ReplaceFirstInstance (cname, GetCategoryBase (category));
					found = qualified_selectors.Contains (name);
				}
			}
			if (!found)
				Log.On (framework).Add ($"!missing-selector! {name} not bound");
		}

		public override void VisitObjCInterfaceDecl (ObjCInterfaceDecl decl)
		{
			// collect the native class hierarchy for the reverse (managed -> native) check
			if (!decl.IsThisDeclarationADefinition)
				return;

			var name = decl.Name;
			known_types.Add (name);

			var super = decl.SuperClass?.Name;
			if (!string.IsNullOrEmpty (super))
				type_super [name] = super;

			foreach (var protocol in decl.Protocols)
				GetOrCreate (type_protocols, name).Add (protocol.Name);

			var framework = Helpers.GetFramework (decl);
			if (framework is not null)
				parsed_frameworks.Add (framework);
		}

		public override void VisitObjCCategoryDecl (ObjCCategoryDecl decl)
		{
			// a category can add protocol conformances to the class it extends
			var name = decl.ClassInterface.Name;
			foreach (var protocol in decl.Protocols)
				GetOrCreate (type_protocols, name).Add (protocol.Name);
		}

		public override void VisitObjCProtocolDecl (ObjCProtocolDecl decl)
		{
			// collect the native protocol inheritance for the reverse (managed -> native) check
			if (!decl.IsThisDeclarationADefinition)
				return;

			foreach (var protocol in decl.Protocols)
				GetOrCreate (protocol_bases, decl.Name).Add (protocol.Name);
		}

		static string GetCategoryBase (ObjCCategoryDecl category)
		{
			// I really dislike doing this
			switch (category.Name) {
			case "UIResponderStandardEditActions":
				// we inlined this protocol in UIResponder but Apple has it on NSObject
				return "UIResponder";
			case "UIAccessibility":
				// we inlined this protocol in UIView... but Apple has it on NSObject
				return "UIView";
			case "UIAccessibilityAction":
				// we inlined this protocol in UIResponder but Apple has it on NSObject
				return "UIResponder";
			default:
				return Helpers.GetManagedName (category.ClassInterface.Name);
			}
		}

		public override void EndVisit ()
		{
			// At this stage we've collected the full native class/protocol hierarchy. A managed
			// [Export] selector is stale if it's not part of the effective set of selectors for its
			// native class (the class' own selectors plus those of all its base classes and adopted
			// protocols) - report it so the binding can be removed.
			foreach (var entry in managed_selectors) {
				// skip frameworks that weren't parsed natively (e.g. excluded from the sharpie run)
				if (!parsed_frameworks.Contains (entry.Framework))
					continue;
				// if the whole native type is gone that's reported by the '!unknown-type!' check
				if (!known_types.Contains (entry.Type))
					continue;
				if (GetEffectiveSelectors (entry.Type).Contains (entry.Selector))
					continue;
				Log.On (entry.Framework).Add ($"!unknown-selector! {entry.QualifiedName} not found");
			}
		}
	}
}
