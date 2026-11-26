// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

using Mono.Cecil;
using Mono.Cecil.Cil;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Cecil.Tests {

	// MSBuild may run tasks marked with [MSBuildMultiThreadableTask] in parallel in the same process.
	// This test finds all the code such a task can reach, and fails if any of it:
	// * Reads or writes a static field in our own code (static state is shared between all task instances).
	// * Calls an API that affects or depends on process-wide state (such as Console, the current directory or environment variables).
	//
	// The Microsoft.Build.TaskAuthoring.Analyzer analyzer checks some of this at build time, but it can't see into
	// other assemblies (such as Xamarin.MacDev), and it doesn't check static state.
	[TestFixture]
	public class MultiThreadableTaskTest {
		const string MultiThreadableTaskAttribute = "Microsoft.Build.Framework.MSBuildMultiThreadableTaskAttribute";

		// We look into these assemblies, and treat any other assembly as a boundary.
		static readonly HashSet<string> OurAssemblies = new () {
			"Xamarin.MacDev.Tasks",
			"Xamarin.Apple.Tools.MaciOS",
			"assembly-preparer",
		};

		// Type name -> member names (an empty array means every member).
		static readonly Dictionary<string, string []> BannedApis = new () {
			{ "System.Console", [] },
			{ "System.Environment", [
				"Exit",
				"FailFast",
				"GetEnvironmentVariable",
				"GetEnvironmentVariables",
				"SetEnvironmentVariable",
				"ExpandEnvironmentVariables",
				"get_CurrentDirectory",
				"set_CurrentDirectory",
			] },
			{ "System.IO.Directory", [
				"GetCurrentDirectory",
				"SetCurrentDirectory",
			] },
			{ "System.IO.Path", [
				"GetFullPath",
				"GetTempPath",
				"GetTempFileName",
			] },
			{ "System.Diagnostics.Process", [
				"Start",
				"Kill",
			] },
			{ "System.Diagnostics.ProcessStartInfo", [".ctor"] },
			{ "System.Threading.ThreadPool", [
				"SetMinThreads",
				"SetMaxThreads",
			] },
			{ "System.Globalization.CultureInfo", [
				"set_CurrentCulture",
				"set_CurrentUICulture",
				"set_DefaultThreadCurrentCulture",
				"set_DefaultThreadCurrentUICulture",
			] },
			// This method resolves relative paths against the current directory (the analyzer warning is suppressed in the method).
			{ "Xamarin.MacDev.Tasks.XamarinTask", ["CreateItemsForAllFilesRecursively"] },
		};

		static string GetTaskAssembly ()
		{
			// The netstandard2.0 build merges its dependencies into a single assembly, so look at the .NET build instead.
			var platform = Configuration.GetIncludedPlatforms ().First ();
			return Path.Combine (Configuration.GetSdkRoot (platform), "msbuild", Configuration.DotNetTfm, "Xamarin.MacDev.Tasks.dll");
		}

		[Test]
		public void NoSharedStateOrUnsafeApis ()
		{
			var path = GetTaskAssembly ();
			using var resolver = new DefaultAssemblyResolver ();
			foreach (var directory in resolver.GetSearchDirectories ())
				resolver.RemoveSearchDirectory (directory);
			resolver.AddSearchDirectory (Path.GetDirectoryName (path));
			foreach (var sdk in Directory.EnumerateDirectories (Path.Combine (Configuration.DOTNET_DIR, "sdk")))
				resolver.AddSearchDirectory (sdk);
			resolver.AddSearchDirectory (AppContext.BaseDirectory);
			resolver.AddSearchDirectory (Path.GetDirectoryName (typeof (object).Assembly.Location));
			using var assembly = AssemblyDefinition.ReadAssembly (path, new ReaderParameters { AssemblyResolver = resolver });

			var tasks = assembly.MainModule.Types
				.Where (t => !t.IsAbstract && t.CustomAttributes.Any (ca => ca.AttributeType.FullName == MultiThreadableTaskAttribute))
				.ToArray ();
			Assert.That (tasks, Is.Not.Empty, "No multithreadable tasks found");

			var walker = new Walker (assembly.MainModule);
			var failures = new List<string> ();
			foreach (var task in tasks)
				failures.AddRange (walker.Check (task));

			Assert.That (failures, Is.Empty, "Multithreadable tasks must not use static state or unsafe APIs. Either fix the code, or remove [MSBuildMultiThreadableTask] from the task.\n\t" + string.Join ("\n\t", failures));
		}

		[Test]
		public void FindsInheritedInterfaceImplementation ()
		{
			using var assembly = AssemblyDefinition.CreateAssembly (
				new AssemblyNameDefinition ("InterfaceDispatchFixture", new Version (1, 0)),
				"InterfaceDispatchFixture",
				ModuleKind.Dll);
			var module = assembly.MainModule;
			var interfaceType = new TypeDefinition ("Fixture", "IRunnable", Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Interface | Mono.Cecil.TypeAttributes.Abstract, null);
			var interfaceMethod = new MethodDefinition ("Run", Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Abstract | Mono.Cecil.MethodAttributes.Virtual | Mono.Cecil.MethodAttributes.NewSlot, module.TypeSystem.Void);
			interfaceType.Methods.Add (interfaceMethod);
			module.Types.Add (interfaceType);

			var baseType = new TypeDefinition ("Fixture", "Base", Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Class, null);
			var implementation = new MethodDefinition ("Run", Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Virtual, module.TypeSystem.Void);
			baseType.Methods.Add (implementation);
			module.Types.Add (baseType);

			var derivedType = new TypeDefinition ("Fixture", "Derived", Mono.Cecil.TypeAttributes.Public | Mono.Cecil.TypeAttributes.Class, baseType);
			derivedType.Interfaces.Add (new InterfaceImplementation (interfaceType));
			module.Types.Add (derivedType);

			Assert.That (Walker.GetInterfaceImplementations (derivedType, interfaceMethod), Does.Contain (implementation));
		}

		class Walker {
			// Base method -> the methods in our assemblies that override or implement it.
			readonly Dictionary<MethodDefinition, List<MethodDefinition>> overrides = new ();

			public Walker (ModuleDefinition module)
			{
				var modules = new Dictionary<string, ModuleDefinition> { { module.Assembly.Name.Name, module } };
				var pending = new Queue<ModuleDefinition> ();
				pending.Enqueue (module);
				while (pending.TryDequeue (out var current)) {
					foreach (var reference in current.AssemblyReferences.Where (r => OurAssemblies.Contains (r.Name))) {
						if (modules.ContainsKey (reference.Name))
							continue;
						var dependency = current.AssemblyResolver.Resolve (reference);
						Assert.That (dependency, Is.Not.Null, $"Could not resolve {reference.FullName}");
						modules.Add (reference.Name, dependency.MainModule);
						pending.Enqueue (dependency.MainModule);
					}
				}
				Assert.That (modules.Keys, Is.EquivalentTo (OurAssemblies), "Missing task dependency assemblies");
				var types = modules.Values.SelectMany (m => m.Types);
				foreach (var type in types.SelectMany (EnumerateNestedTypes)) {
					foreach (var method in type.Methods.Where (m => m.IsVirtual)) {
						foreach (var baseMethod in GetBaseMethods (method)) {
							if (!overrides.TryGetValue (baseMethod, out var list))
								overrides [baseMethod] = list = new List<MethodDefinition> ();
							list.Add (method);
						}
					}
				}
			}

			static IEnumerable<TypeDefinition> EnumerateNestedTypes (TypeDefinition type)
			{
				yield return type;
				foreach (var nested in type.NestedTypes.SelectMany (EnumerateNestedTypes))
					yield return nested;
			}

			// The methods (in any assembly) the specified method overrides or implements.
			static IEnumerable<MethodDefinition> GetBaseMethods (MethodDefinition method)
			{
				foreach (var o in method.Overrides) {
					if (o.Resolve () is MethodDefinition md)
						yield return md;
				}
				var type = method.DeclaringType;
				for (var baseType = type.BaseType?.Resolve (); baseType is not null; baseType = baseType.BaseType?.Resolve ()) {
					foreach (var candidate in baseType.Methods.Where (m => m.IsVirtual && SameSignature (m, method)))
						yield return candidate;
				}
				var interfaces = new Queue<TypeReference> ();
				for (var t = type; t is not null; t = t.BaseType?.Resolve ()) {
					foreach (var iface in t.Interfaces)
						interfaces.Enqueue (iface.InterfaceType);
				}
				var visited = new HashSet<TypeDefinition> ();
				while (interfaces.TryDequeue (out var iface)) {
					var ifaceType = iface.Resolve ();
					Assert.That (ifaceType, Is.Not.Null, $"Could not resolve interface {iface.FullName}");
					if (!visited.Add (ifaceType))
						continue;
					foreach (var candidate in ifaceType.Methods.Where (m => SameSignature (m, method)))
						yield return candidate;
					foreach (var parent in ifaceType.Interfaces)
						interfaces.Enqueue (parent.InterfaceType);
				}
			}

			static bool ImplementsInterface (TypeDefinition type, TypeDefinition interfaceType)
			{
				var pending = new Queue<TypeReference> ();
				for (var current = type; current is not null; current = current.BaseType?.Resolve ()) {
					foreach (var iface in current.Interfaces)
						pending.Enqueue (iface.InterfaceType);
				}

				var visited = new HashSet<TypeDefinition> ();
				while (pending.TryDequeue (out var iface)) {
					var definition = iface.Resolve ();
					Assert.That (definition, Is.Not.Null, $"Could not resolve interface {iface.FullName}");
					if (definition == interfaceType)
						return true;
					if (!visited.Add (definition))
						continue;
					foreach (var parent in definition.Interfaces)
						pending.Enqueue (parent.InterfaceType);
				}
				return false;
			}

			public static IEnumerable<MethodDefinition> GetInterfaceImplementations (TypeDefinition receiver, MethodDefinition interfaceMethod)
			{
				if (!ImplementsInterface (receiver, interfaceMethod.DeclaringType))
					yield break;

				for (var current = receiver; current is not null; current = current.BaseType?.Resolve ()) {
					foreach (var method in current.Methods) {
						if (method.IsStatic || method.IsConstructor)
							continue;
						if (method.Overrides.Any (o => o.Resolve () == interfaceMethod) ||
							(method.IsPublic && SameSignature (method, interfaceMethod)))
							yield return method;
					}
				}
			}

			static bool SameSignature (MethodDefinition a, MethodDefinition b)
			{
				if (a.Name != b.Name || a.Parameters.Count != b.Parameters.Count || a.GenericParameters.Count != b.GenericParameters.Count)
					return false;
				for (var i = 0; i < a.Parameters.Count; i++) {
					var left = a.Parameters [i].ParameterType;
					var right = b.Parameters [i].ParameterType;
					// Conservatively match generic signatures, including closed implementations such as IFoo<string>.
					if (!left.ContainsGenericParameter && !right.ContainsGenericParameter && left.FullName != right.FullName)
						return false;
				}
				return true;
			}

			static bool IsOurs (IMemberDefinition member)
			{
				var type = member as TypeDefinition ?? member.DeclaringType;
				return OurAssemblies.Contains (type.Module.Assembly.Name.Name);
			}

			// Static fields in compiler-generated types (such as cached lambdas) aren't shared state we need to worry about.
			// Note that the backing fields for static auto-properties are compiler-generated too, but they're declared in our own types, so we report them.
			static bool IsInCompilerGeneratedType (FieldDefinition field)
			{
				for (var t = field.DeclaringType; t is not null; t = t.DeclaringType) {
					if (t.Name.StartsWith ('<'))
						return true;
				}
				return false;
			}

			// Classes generated from *.resx files store a ResourceManager in a static field. This is safe, because
			// ResourceManager is thread-safe, and creating it twice (if two threads race) is harmless. Changing the
			// culture would affect every task though, so we don't allow calling the Culture setter.
			static bool IsResourceClass (TypeDefinition type)
			{
				return type.CustomAttributes.Any (ca =>
					ca.AttributeType.FullName == "System.CodeDom.Compiler.GeneratedCodeAttribute" &&
					ca.ConstructorArguments [0].Value is string tool &&
					tool.EndsWith (".StronglyTypedResourceBuilder", StringComparison.Ordinal));
			}

			static bool IsBanned (MethodReference method)
			{
				if (method.Name == "set_Culture" && method.DeclaringType.Resolve () is TypeDefinition type && IsResourceClass (type))
					return true;
				if (!BannedApis.TryGetValue (method.DeclaringType.FullName, out var members))
					return false;
				return members.Length == 0 || members.Contains (method.Name);
			}

			public IEnumerable<string> Check (TypeDefinition task)
			{
				// The method -> the method that made it reachable, so that we can show how we got there.
				var callers = new Dictionary<MethodDefinition, MethodDefinition?> ();
				var queue = new Queue<MethodDefinition> ();
				var failures = new List<string> ();

				void Add (MethodDefinition? method, MethodDefinition? caller)
				{
					if (method is null || !IsOurs (method) || callers.ContainsKey (method))
						return;
					callers [method] = caller;
					queue.Enqueue (method);
				}

				// This is a rapid type analysis: a virtual call only reaches the overrides in types we've created an instance of.
				// Instantiated type -> the method that created it.
				var instantiated = new Dictionary<TypeDefinition, MethodDefinition?> ();
				// Method called virtually -> the method that called it.
				var virtualCalls = new Dictionary<MethodDefinition, MethodDefinition> ();

				IEnumerable<TypeDefinition> GetTypeAndBaseTypes (TypeDefinition type)
				{
					for (var t = type; t is not null && IsOurs (t); t = t.BaseType?.Resolve ())
						yield return t;
				}

				bool IsInstantiated (TypeDefinition type)
				{
					return instantiated.Keys.Any (t => GetTypeAndBaseTypes (t).Contains (type));
				}

				void AddVirtualCall (MethodDefinition method, MethodDefinition caller)
				{
					Add (method, caller);
					if (!virtualCalls.TryAdd (method, caller))
						return;
					if (method.DeclaringType.IsInterface) {
						foreach (var type in instantiated.Keys) {
							foreach (var implementation in GetInterfaceImplementations (type, method))
								Add (implementation, caller);
						}
						return;
					}
					if (overrides.TryGetValue (method, out var list)) {
						foreach (var o in list.Where (o => IsInstantiated (o.DeclaringType)))
							Add (o, caller);
					}
				}

				void Instantiate (TypeDefinition type, MethodDefinition? caller)
				{
					if (!IsOurs (type) || !instantiated.TryAdd (type, caller))
						return;
					foreach (var m in GetTypeAndBaseTypes (type).SelectMany (t => t.Methods).Where (m => m.IsVirtual)) {
						var baseMethods = GetBaseMethods (m).ToArray ();
						if (virtualCalls.TryGetValue (m, out var virtualCaller)) {
							Add (m, virtualCaller);
						} else if (baseMethods.FirstOrDefault (b => virtualCalls.ContainsKey (b)) is MethodDefinition calledBase) {
							Add (m, virtualCalls [calledBase]);
						} else if (baseMethods.Any (b => !IsOurs (b))) {
							// Code outside our assemblies (such as MSBuild, or the BCL) may call this method.
							Add (m, caller);
						}
					}
					foreach (var call in virtualCalls.Where (v => v.Key.DeclaringType.IsInterface)) {
						foreach (var implementation in GetInterfaceImplementations (type, call.Key))
							Add (implementation, call.Value);
					}
				}

				void InstantiateTypeArguments (TypeReference type, MethodDefinition caller)
				{
					while (type is TypeSpecification specification && type is not GenericInstanceType)
						type = specification.ElementType;
					if (type is not GenericInstanceType generic)
						return;
					foreach (var argument in generic.GenericArguments)
						InstantiateTypeArgument (argument, caller);
				}

				void InstantiateTypeArgument (TypeReference type, MethodDefinition caller)
				{
					var element = type.GetElementType ();
					if (element is GenericParameter)
						return;
					var definition = element.Resolve ();
					Assert.That (definition, Is.Not.Null, $"Could not resolve {type.FullName}");
					Instantiate (definition, caller);
					InstantiateTypeArguments (type, caller);
				}

				void Report (MethodDefinition method, string problem)
				{
					var chain = new List<string> ();
					for (var m = method; m is not null; m = callers [m])
						chain.Add (m.FullName);
					chain.Reverse ();
					failures.Add ($"{task.FullName}: {problem}\n\t\t" + string.Join ("\n\t\t-> ", chain));
				}

				// MSBuild creates the task, sets its public properties, calls Execute (and maybe Cancel), and reads its output properties.
				Instantiate (task, null);
				var taskMethods = GetTypeAndBaseTypes (task).SelectMany (t => t.Methods).ToArray ();
				var execute = taskMethods.FirstOrDefault (m => m.Name == "Execute" && !m.IsStatic && m.Parameters.Count == 0 && m.ReturnType.FullName == "System.Boolean");
				Assert.That (execute, Is.Not.Null, $"{task.FullName} has no Execute entry point");
				Add (execute, null);
				foreach (var cancel in taskMethods.Where (m => (m.Name == "Cancel" || m.Name.EndsWith (".Cancel", StringComparison.Ordinal)) && !m.IsStatic && m.Parameters.Count == 0))
					Add (cancel, null);
				foreach (var ctor in task.Methods.Where (m => m.IsConstructor && !m.IsStatic))
					Add (ctor, null);
				foreach (var property in GetTypeAndBaseTypes (task).SelectMany (t => t.Properties)) {
					if (property.SetMethod?.IsPublic == true)
						Add (property.SetMethod, null);
					if (property.GetMethod is not null && property.CustomAttributes.Any (ca => ca.AttributeType.FullName == "Microsoft.Build.Framework.OutputAttribute"))
						Add (property.GetMethod, null);
				}

				while (queue.TryDequeue (out var method)) {
					// Calling any member of a type may run its static constructor.
					Add (method.DeclaringType.Methods.FirstOrDefault (m => m.IsConstructor && m.IsStatic), method);

					// The compiler moves the code for async and iterator methods to a nested state machine type.
					foreach (var ca in method.CustomAttributes) {
						if (ca.AttributeType.Name is "AsyncStateMachineAttribute" or "IteratorStateMachineAttribute" && ca.ConstructorArguments [0].Value is TypeReference smType && smType.Resolve () is TypeDefinition sm)
							Instantiate (sm, method);
					}

					if (!method.HasBody)
						continue;

					foreach (var ins in method.Body.Instructions) {
						switch (ins.Operand) {
						case TypeReference tr when ins.OpCode == OpCodes.Initobj || ins.OpCode == OpCodes.Constrained || ins.OpCode == OpCodes.Box:
							// Generic parameters have no definition; their concrete arguments are recorded at the call site.
							InstantiateTypeArgument (tr, method);
							break;
						case FieldReference fr when ins.OpCode == OpCodes.Ldsfld || ins.OpCode == OpCodes.Stsfld || ins.OpCode == OpCodes.Ldsflda:
							var field = fr.Resolve ();
							Assert.That (field, Is.Not.Null, $"Could not resolve {fr.FullName}");
							if (IsOurs (field) && !IsInCompilerGeneratedType (field) && !IsResourceClass (field.DeclaringType))
								Report (method, $"accesses the static field {field.FullName}");
							break;
						case MethodReference mr:
							InstantiateTypeArguments (mr.DeclaringType, method);
							if (mr is GenericInstanceMethod genericMethod) {
								foreach (var argument in genericMethod.GenericArguments)
									InstantiateTypeArgument (argument, method);
							}
							if (IsBanned (mr)) {
								Report (method, $"calls {mr.FullName}");
								break;
							}
							// Array constructors and Get/Set pseudo-methods have no IL body or MethodDefinition.
							if (mr.DeclaringType is ArrayType)
								break;
							var md = mr.Resolve ();
							Assert.That (md, Is.Not.Null, $"Could not resolve {mr.FullName}");
							if (ins.OpCode == OpCodes.Newobj)
								Instantiate (md.DeclaringType, method);
							if (md.IsVirtual && (ins.OpCode == OpCodes.Callvirt || ins.OpCode == OpCodes.Ldvirtftn))
								AddVirtualCall (md, method);
							else
								Add (md, method);
							break;
						}
					}
				}

				return failures;
			}
		}
	}
}
