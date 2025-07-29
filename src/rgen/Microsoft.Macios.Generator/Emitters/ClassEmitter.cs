// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Formatters;
using Microsoft.Macios.Generator.IO;
using ObjCBindings;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Constructor = ObjCBindings.Constructor;
using Property = Microsoft.Macios.Generator.DataModel.Property;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emitter for Objective-C classes.
/// </summary>
class ClassEmitter : IClassEmitter {
	/// <inheritdoc />
	public string GetSymbolName (in Binding binding)
	{
		var outerClasses = binding.OuterClasses.Select (x => x.Name);
		var prefix = string.Join ('.', outerClasses);
		return string.IsNullOrEmpty (prefix)
			? binding.Name
			: $"{prefix}.{binding.Name}";
	}

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements => [
		"System",
		"System.Drawing",
		"System.Diagnostics",
		"System.ComponentModel",
		"System.Runtime.Versioning",
		"System.Runtime.InteropServices",
		"System.Diagnostics.CodeAnalysis",
		"ObjCRuntime",
	];


	/// <summary>
	/// Emit the default constructors for the class.
	/// </summary>
	/// <param name="bindingContext">The current binding context.</param>
	/// <param name="classBlock">Current class block.</param>
	/// <param name="disableDefaultCtor">A value indicating whether to disable the default constructor.</param>
	void EmitDefaultConstructors (in BindingContext bindingContext, TabbedWriter<StringWriter> classBlock, bool disableDefaultCtor)
	{

		if (!disableDefaultCtor) {
			classBlock.WriteDocumentation (Documentation.Class.DefaultInit (bindingContext.Changes.Name));
			classBlock.AppendGeneratedCodeAttribute ();
			classBlock.AppendDesignatedInitializer ();
			classBlock.WriteRaw (
$@"[Export (""init"")]
public {bindingContext.Changes.Name} () : base ({NSObjectFlag}.Empty)
{{
	if (IsDirectBinding)
		InitializeHandle (global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (""init"")), ""init"");
	else
		InitializeHandle (global::ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (""init"")), ""init"");
}}
");
			classBlock.WriteLine ();
		}

		classBlock.WriteDocumentation (Documentation.Class.DefaultInitWithFlag (bindingContext.Changes.Name));
		classBlock.AppendGeneratedCodeAttribute ();
		classBlock.AppendEditorBrowsableAttribute (EditorBrowsableState.Advanced);
		classBlock.WriteLine ($"protected {bindingContext.Changes.Name} ({NSObjectFlag} t) : base (t) {{}}");

		classBlock.WriteLine ();
		classBlock.WriteDocumentation (Documentation.Class.DefaultInitWithHandle (bindingContext.Changes.Name));
		classBlock.AppendGeneratedCodeAttribute ();
		classBlock.AppendEditorBrowsableAttribute (EditorBrowsableState.Advanced);
		classBlock.WriteLine ($"protected internal {bindingContext.Changes.Name} ({NativeHandle} handle) : base (handle) {{}}");
	}

	/// <summary>
	/// Emit the code for all the constructors in the class.
	/// </summary>
	/// <param name="context">The current binding context.</param>
	/// <param name="classBlock">Current class block.</param>
	void EmitConstructors (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		// When dealing with constructors we cannot sort them by name because the name is always the same as the class
		// instead we will sort them by the selector name so that we will always generate the constructors in the same order
		foreach (var constructor in context.Changes.Constructors.OrderBy (c => c.ExportMethodData.Selector)) {
			classBlock.WriteLine ();
			classBlock.AppendMemberAvailability (constructor.SymbolAvailability);
			classBlock.AppendGeneratedCodeAttribute (optimizable: true);
			if (constructor.ExportMethodData.Flags.HasFlag (Constructor.DesignatedInitializer)) {
				classBlock.AppendDesignatedInitializer ();
			}

			using (var constructorBlock = classBlock.CreateBlock (constructor.ToDeclaration (withBaseNSFlag: true).ToString (), block: true)) {
				// retrieve the method invocation via the factory, this will generate the necessary arguments
				// transformations and the invocation
				var invocations = GetInvocations (constructor);

				// init the needed temp variables
				foreach (var argument in invocations.Arguments) {
					constructorBlock.Write (argument.Validations, verifyTrivia: false);
					constructorBlock.Write (argument.Initializers, verifyTrivia: false);
					constructorBlock.Write (argument.PreCallConversion, verifyTrivia: false);
				}

				// simply call the send or sendSuper accordingly
				constructorBlock.WriteRaw (
$@"if (IsDirectBinding) {{
	{ExpressionStatement (invocations.Send)}
}} else {{
	{ExpressionStatement (invocations.SendSuper)}
}}
");

				// before we leave the methods, do any post operations
				foreach (var argument in invocations.Arguments) {
					constructorBlock.Write (argument.PostCallConversion, verifyTrivia: false);
				}
			}
		}
	}

	/// <summary>
	/// Emit the code for all the properties in the class.
	/// </summary>
	/// <param name="context">The current binding context.</param>
	/// <param name="classBlock">Current class block.</param>
	void EmitProperties (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{

		// use the binding context to decide if we need to insert the ui thread check
		var uiThreadCheck = (context.NeedsThreadChecks)
			? EnsureUiThread (context.RootContext.CurrentPlatform) : null;

		foreach (var property in context.Changes.Properties.OrderBy (p => p.Name)) {
			if (property.IsField)
				// ignore fields
				continue;
			// use the factory to generate all the needed invocations for the current 
			var invocations = GetInvocations (property);

			// we expect to always at least have a getter
			var getter = property.GetAccessor (AccessorKind.Getter);
			if (getter.IsNullOrDefault)
				continue;

			// add backing variable for the property if it is needed
			if (property.NeedsBackingField) {
				classBlock.WriteLine ();
				classBlock.AppendGeneratedCodeAttribute (optimizable: true);
				classBlock.WriteLine ($"object? {property.BackingField} = null;");
			}

			classBlock.WriteLine ();
			classBlock.AppendMemberAvailability (property.SymbolAvailability);
			classBlock.AppendGeneratedCodeAttribute (optimizable: true);

			using (var propertyBlock = classBlock.CreateBlock (property.ToDeclaration ().ToString (), block: true)) {
				// be very verbose with the availability, makes the life easier to the dotnet analyzer
				propertyBlock.AppendMemberAvailability (getter.SymbolAvailability);
				// if we deal with a delegate, include the attr:
				// [return: DelegateProxy (typeof ({staticBridge}))]
				if (property.ReturnType.IsDelegate)
					propertyBlock.AppendDelegateProxyReturn (property.ReturnType);
				using (var getterBlock = propertyBlock.CreateBlock ("get", block: true)) {
					if (uiThreadCheck is not null) {
						getterBlock.WriteLine (uiThreadCheck.ToString ());
						getterBlock.WriteLine ();
					}
					// depending on the property definition, we might need a temp variable to store
					// the return value
					var (tempVar, tempDeclaration) = GetReturnValueAuxVariable (property.ReturnType);
					getterBlock.WriteRaw (
$@"{tempDeclaration}
if (IsDirectBinding) {{
	{ExpressionStatement (invocations.Getter.Send)}
}} else {{
	{ExpressionStatement (invocations.Getter.SendSuper)}
}}
{ExpressionStatement (KeepAlive ("this"))}
");
					if (property.RequiresDirtyCheck || property.IsWeakDelegate) {
						getterBlock.WriteLine ("MarkDirty ();");
					}

					if (property.NeedsBackingField) {
						getterBlock.WriteLine ($"{property.BackingField} = {tempVar};");
					}

					getterBlock.WriteLine ($"return {tempVar};");
				}

				var setter = property.GetAccessor (AccessorKind.Setter);
				if (setter.IsNullOrDefault || invocations.Setter is null)
					// we are done with the current property
					continue;

				propertyBlock.WriteLine (); // add space between getter and setter since we have the attrs
				propertyBlock.AppendMemberAvailability (setter.SymbolAvailability);
				// if we deal with a delegate, include the attr:
				// [param: BlockProxy (typeof ({nativeInvoker}))]
				if (property.ReturnType.IsDelegate)
					propertyBlock.AppendDelegateParameter (property.ReturnType);
				using (var setterBlock = propertyBlock.CreateBlock ("set", block: true)) {
					if (uiThreadCheck is not null) {
						setterBlock.WriteLine (uiThreadCheck.ToString ());
						setterBlock.WriteLine ();
					}
					// init the needed temp variables
					setterBlock.Write (invocations.Setter.Value.Argument.Initializers, verifyTrivia: false);
					setterBlock.Write (invocations.Setter.Value.Argument.Validations, verifyTrivia: false);
					setterBlock.Write (invocations.Setter.Value.Argument.PreCallConversion, verifyTrivia: false);

					// perform the invocation
					setterBlock.WriteRaw (
$@"if (IsDirectBinding) {{
	{ExpressionStatement (invocations.Setter.Value.Send)}
}} else {{
	{ExpressionStatement (invocations.Setter.Value.SendSuper)}
}}
{ExpressionStatement (KeepAlive ("this"))}
");
					// perform the post delegate call conversion, this might include the GC.KeepAlive calls to keep
					// the native object alive
					setterBlock.Write (invocations.Setter.Value.Argument.PostCallConversion, verifyTrivia: false);
					// mark property as dirty if needed
					if (property.RequiresDirtyCheck || property.IsWeakDelegate) {
						setterBlock.WriteLine ("MarkDirty ();");
					}

					if (property.NeedsBackingField) {
						setterBlock.WriteLine ($"{property.BackingField} = value;");
					}
				}
			}

			// if the property is a weak delegate and has the strong delegate type set, we need to emit the
			// strong delegate property
			if (property is { IsProperty: true, IsWeakDelegate: true }
				&& !property.ExportPropertyData.Value.StrongDelegateType.IsNullOrDefault) {
				classBlock.WriteLine ();
				var strongDelegate = property.ToStrongDelegate ();
				using (var propertyBlock =
					   classBlock.CreateBlock (strongDelegate.ToDeclaration ().ToString (), block: true)) {
					using (var getterBlock =
						   propertyBlock.CreateBlock ("get", block: true)) {
						getterBlock.WriteLine (
							$"return {property.Name} as {strongDelegate.ReturnType.WithNullable (isNullable: false).GetIdentifierSyntax ()};");
					}

					using (var setterBlock =
						   propertyBlock.CreateBlock ("set", block: true)) {
						setterBlock.WriteRaw (
$@"var rvalue = value as NSObject;
if (!(value is null) && rvalue is null) {{
	throw new ArgumentException ($""The object passed of type {{value.GetType ()}} does not derive from NSObject"");
}}
{property.Name} = rvalue;
");
					}
				}
			}
		}
	}

	/// <summary>
	/// Emit the code for all the notifications in the class.
	/// </summary>
	/// <param name="properties">All properties of the class, the method will filter those that are notifications.</param>
	/// <param name="classBlock">Current class block.</param>
	void EmitNotifications (in ImmutableArray<Property> properties, TabbedWriter<StringWriter> classBlock)
	{
		if (properties.Length == 0)
			return;

		// add a space just to make it nicer to read
		classBlock.WriteLine ();

		// create a nested static class with the notification helpers
		using (var notificationClass = classBlock.CreateBlock ("public static partial class Notifications", true)) {
			notificationClass.WriteLine ();
			// generate two methods per notification
			foreach (var notification in properties) {
				var count = 12; // == "Notification".Length;
				var name = $"Observe{notification.Name [..^count]}";
				var notificationCenter = notification.ExportFieldData?.FieldData.NotificationCenter ?? $"{NotificationCenter}.DefaultCenter";
				var eventType = notification.ExportFieldData?.FieldData.Type ?? NSNotificationEventArgs.ToString ();
				// use the raw writer which makes it easier to read in this case
				notificationClass.WriteRaw (
@$"public static {NSObject} {name} ({EventHandler}<{eventType}> handler)
{{
	return {notificationCenter}.AddObserver ({notification.Name}, notification => handler (null, new {eventType} (notification)));
}}

public static NSObject {name} ({NSObject} objectToObserve, {EventHandler}<{eventType}> handler)
{{
	return {notificationCenter}.AddObserver ({notification.Name}, notification => handler (null, new {eventType} (notification)), objectToObserve);
}}

"
);
			}
		}
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.Class) {
			diagnostics = [Diagnostic.Create (
					Diagnostics
						.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
					null,
					bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}

		// namespace declaration
		this.EmitNamespace (bindingContext);

		using (var _ = this.EmitOuterClasses (bindingContext, out var builder)) {

			// register the class only if we are not dealing with a static class
			var bindingData = (BindingTypeData<Class>) bindingContext.Changes.BindingInfo;
			// Registration depends on the class name. If the binding data contains a name, we use that one, else
			// we use the name of the class
			var registrationName = bindingData.Name ?? bindingContext.Changes.Name;

			// append the class availability, this will add the necessary attributes to the class
			bindingContext.Builder.AppendMemberAvailability (bindingContext.Changes.SymbolAvailability);
			if (!bindingContext.Changes.IsStatic) {
				builder.WriteLine ($"[Register (\"{registrationName}\", true)]");
			}

			var modifiers = $"{string.Join (' ', bindingContext.Changes.Modifiers)} ";
			using (var classBlock =
				   builder.CreateBlock (
					   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {bindingContext.Changes.Name}",
					   true)) {
				// emit the fields for the selectors before we register the class or anything
				this.EmitSelectorFields (bindingContext, classBlock);

				if (!bindingContext.Changes.IsStatic) {
					classBlock.AppendGeneratedCodeAttribute (optimizable: true);
					classBlock.WriteLine (
						$"static readonly {NativeHandle} {ClassPtr} = {BindingSyntaxFactory.Class}.GetHandle (\"{registrationName}\");");
					classBlock.WriteLine ();
					classBlock.WriteDocumentation (Documentation.Class.ClassHandle (bindingContext.Changes.Name));
					classBlock.WriteLine ($"public override {NativeHandle} ClassHandle => {ClassPtr};");
					classBlock.WriteLine ();

					EmitDefaultConstructors (bindingContext: bindingContext,
						classBlock: classBlock,
						disableDefaultCtor: bindingData.Flags.HasFlag (ObjCBindings.Class.DisableDefaultCtor));

					// emit any other constructor that is not the default one
					EmitConstructors (bindingContext, classBlock);
				}

				this.EmitFields (bindingContext.Changes.Name, bindingContext.Changes.Properties, classBlock,
					out var notificationProperties);
				EmitProperties (bindingContext, classBlock);
				this.EmitMethods (bindingContext, classBlock);

				// emit the notification helper classes, leave this for the very bottom of the class
				EmitNotifications (notificationProperties, classBlock);
			}

			return true;
		}
	}
}
