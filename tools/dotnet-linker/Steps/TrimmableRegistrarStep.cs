// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using Xamarin.Bundler;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Linker;
using Mono.Tuner;

using Mono.Cecil.Rocks;
using Registrar;

#nullable enable

namespace Xamarin.Linker {
	public class TrimmableRegistrarStep : ConfigurationAwareStep {
		protected override string Name { get; } = "TrimmableRegistrar";
		protected override int ErrorCode { get; } = 2470;

		AppBundleRewriter abr { get { return Configuration.AppBundleRewriter; } }
		List<AssemblyDefinition> addedAssemblies = new List<AssemblyDefinition> ();
		List<Exception> exceptions = new List<Exception> ();

		void AddException (Exception exception)
		{
			if (exceptions is null)
				exceptions = new List<Exception> ();
			exceptions.Add (exception);
		}

		protected override void TryProcess ()
		{
			base.TryProcess ();

			if (App.Registrar != RegistrarMode.TrimmableStatic)
				return;

			Configuration.Application.StaticRegistrar.Register (Configuration.GetNonDeletedAssemblies (this));
		}

		AssemblyDefinition CreateTypeMapRootAssembly (ModuleParameters moduleParameters, IEnumerable<AssemblyDefinition> assemblies)
		{
			AssemblyDefinition rootTypeMapAssembly;

			// .NET 10 doesn't support a separate root type map assembly, so we have to add these attributes to the entry assembly instead.
			var useEntryAssemblyAsRootTypeMapAssembly = Driver.TargetFramework.Version.Major <= 10;

			if (useEntryAssemblyAsRootTypeMapAssembly) {
				rootTypeMapAssembly = Configuration.EntryAssembly;
			} else {
				var rootTypeMapAssemblyName = new AssemblyNameDefinition (App.TypeMapAssemblyName, new Version (1, 0, 0, 0));
				rootTypeMapAssembly = AssemblyDefinition.CreateAssembly (rootTypeMapAssemblyName, rootTypeMapAssemblyName.Name, moduleParameters);
				Annotations.SetAction (rootTypeMapAssembly, AssemblyAction.Link);
				addedAssemblies.Add (rootTypeMapAssembly);

				// We're running from inside the linker, but the TypeMapEntryAssembly property can only be set using a command-line
				// argument, so we need to cheat a bit here and use reflection to set it. This will go away once we're not running
				// as a custom linker step anymore.
				var typeMapEntryAssemblyProperty = this.Context.GetType ().GetProperty ("TypeMapEntryAssembly");
				if (typeMapEntryAssemblyProperty is null)
					throw ErrorHelper.CreateError (99, "Could not find the 'TypeMapEntryAssembly' property on the linker context.");
				typeMapEntryAssemblyProperty.SetValue (this.Context, App.TypeMapAssemblyName);
			}

			abr.SetCurrentAssembly (rootTypeMapAssembly);

			foreach (var assembly in assemblies.OrderBy (v => v.FullName)) {
				/*
				 * [assembly: TypeMapAssemblyTarget<NSObject> ("...")]
				 */
				var attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssemblyTargetAttribute_1_Constructor_String_Type_Type, abr.Foundation_NSObject));
				attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, "_" + assembly.Name.Name + ".TypeMap"));
				rootTypeMapAssembly.CustomAttributes.Add (attribute);

				/*
				 * [assembly: TypeMapAssemblyTarget<SkippedObjectiveCTypeUniverse> ("...")]
				 */
				attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssemblyTargetAttribute_1_Constructor_String_Type_Type, abr.ObjCRuntime_SkippedObjectiveCTypeUniverse));
				attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, "_" + assembly.Name.Name + ".TypeMap"));
				rootTypeMapAssembly.CustomAttributes.Add (attribute);

				/*
				 * [assembly: TypeMapAssemblyTarget<INativeObject> ("...")]
				 */
				attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssemblyTargetAttribute_1_Constructor_String_Type_Type, abr.ObjCRuntime_INativeObject));
				attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, "_" + assembly.Name.Name + ".TypeMap"));
				rootTypeMapAssembly.CustomAttributes.Add (attribute);

				/*
				 * [assembly: TypeMapAssemblyTarget<ProtocolProxyAttribute> ("...")]
				 */
				attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssemblyTargetAttribute_1_Constructor_String_Type_Type, abr.ObjCRuntime_ProtocolProxyAttribute));
				attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, "_" + assembly.Name.Name + ".TypeMap"));
				rootTypeMapAssembly.CustomAttributes.Add (attribute);

				/*
				 * [assembly: TypeMapAssemblyTarget<ProtocolAttribute> ("...")]
				 */
				attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssemblyTargetAttribute_1_Constructor_String_Type_Type, abr.Foundation_ProtocolAttribute));
				attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, "_" + assembly.Name.Name + ".TypeMap"));
				rootTypeMapAssembly.CustomAttributes.Add (attribute);
			}
			abr.SaveCurrentAssembly ();
			abr.ClearCurrentAssembly ();

			// We write the assembly here even if it hasn't changed, because otherwise we'll just end up re-creating
			// it again during the next incremental build.
			if (!useEntryAssemblyAsRootTypeMapAssembly) {
				rootTypeMapAssembly.Write (Path.Combine (App.TypeMapOutputDirectory, rootTypeMapAssembly.Name.Name + ".dll"));
			}
			return rootTypeMapAssembly;
		}

		MethodReference CreateMethodReference (MethodReference methodReference, params TypeReference [] declaringTypeGenericArguments)
		{
			var methodDeclaringType = methodReference.DeclaringType;
			if (methodDeclaringType.HasGenericParameters) {
				if (declaringTypeGenericArguments.Length != methodDeclaringType.GenericParameters.Count)
					throw new ArgumentException ($"The number of generic arguments provided ({declaringTypeGenericArguments.Length}) does not match the number of generic parameters of the method's declaring type ({methodDeclaringType.GenericParameters.Count}).", nameof (declaringTypeGenericArguments));

				methodDeclaringType = methodDeclaringType.MakeGenericInstanceType (declaringTypeGenericArguments);
			}

			var method = new MethodReference (methodReference.Name, methodReference.ReturnType, methodDeclaringType) {
				HasThis = methodReference.HasThis,
				ExplicitThis = methodReference.ExplicitThis,
				CallingConvention = methodReference.CallingConvention,
			};

			foreach (var parameter in methodReference.Parameters)
				method.Parameters.Add (new ParameterDefinition (parameter.ParameterType));

			return abr.CurrentAssembly.MainModule.ImportReference (method);
		}

		static string GetNamespace (TypeReference tr)
		{
			return tr.FullName.Length == tr.Name.Length ? "" : tr.FullName.Substring (0, tr.FullName.Length - tr.Name.Length - 1).Replace (".", "__").Replace ("/", "__");
		}

		protected override void TryEndProcess (out List<Exception>? exceptions)
		{
			CustomAttribute attribute;
			ILProcessor il;

			base.TryEndProcess ();

			if (App.Registrar != RegistrarMode.TrimmableStatic) {
				exceptions = null;
				return;
			}

			abr.SetCurrentAssembly (abr.PlatformAssembly);
			abr.ObjCRuntime_NSObjectProxyAttribute.Resolve ().IsPublic = true;
			abr.ObjCRuntime_ProtocolProxyAttribute.Resolve ().IsPublic = true;
			abr.ObjCRuntime_INativeObjectProxyAttribute.Resolve ().IsPublic = true;
			abr.ObjCRuntime_SkippedObjectiveCTypeUniverse.Resolve ().IsPublic = true;
			abr.SaveCurrentAssembly ();
			abr.ClearCurrentAssembly ();

			Directory.CreateDirectory (App.TypeMapOutputDirectory);

			var typesByAssembly = App.StaticRegistrar.Types.GroupBy (v => v.Key.Module.Assembly);
			var skippedTypesByAssembly = App.StaticRegistrar.SkippedTypes.GroupBy (v => v.Skipped.Module.Assembly).ToDictionary (v => v.Key, v => v.ToList ());
			// Workaround for https://github.com/dotnet/runtime/issues/127504
			// Tracking issue: https://github.com/dotnet/macios/issues/25275
			//
			// Build a set of types that are the "actual" (non-generic) target of skipped type associations.
			// These are types like NSOrderedSet, NSArray, NSDictionary etc. that have generic variants
			// (NSOrderedSet<T>, NSArray<T>, NSDictionary<TKey,TValue>) mapping to the same ObjC class.
			//
			// These types need unconditional (2-arg) TypeMap entries instead of conditional (3-arg) ones
			// because of a bug in the linker's TypeMapHandler: when it processes the
			// TypeMapAssociationAttribute<SkippedObjectiveCTypeUniverse> for these types, it calls
			// MarkInstantiated directly (bypassing MarkRequirementsForInstantiatedTypes), which poisons
			// the IsInstantiated flag and prevents ProcessType from ever being called. This means their
			// conditional TypeMapAttribute entries (which require ProcessType to be promoted from
			// _unmarkedExternalTypeMapEntries) are silently trimmed by the linker.
			//
			// The workaround consists of two parts:
			//   1. This HashSet identifying the affected types.
			//   2. The conditional block below (lines starting with 'if (skippedActualTypes.Contains (td))')
			//      that uses the 2-arg TypeMapAttribute constructor for these types instead of the 3-arg one.
			//
			// To remove this workaround once the issue is fixed:
			//   1. Delete this HashSet and its comment.
			//   2. Remove the 'if (skippedActualTypes.Contains (td))' branch below, keeping only the 'else' branch.
			//   3. Verify by running: make build run-bare TEST_VARIATION='release|trimmable-static-registrar-all-optimizations-linkall' \
			//        RUN_ARGUMENTS="--test MonoTouchFixtures.Foundation.NSOrderedSetTest"
			//      in tests/monotouch-test/dotnet/MacCatalyst (the MakeNSOrderedSet_WithNull test is a good canary).
			var skippedActualTypes = new HashSet<TypeDefinition> (App.StaticRegistrar.SkippedTypes.Select (v => v.Actual.Type.Resolve ()));

			var copyAssemblyParametersFrom = abr.PlatformAssembly.MainModule;
			var assemblyParameters = new ModuleParameters {
				Kind = copyAssemblyParametersFrom.Kind,
				Runtime = copyAssemblyParametersFrom.Runtime,
				Architecture = copyAssemblyParametersFrom.Architecture,
				AssemblyResolver = copyAssemblyParametersFrom.AssemblyResolver,
				MetadataResolver = copyAssemblyParametersFrom.MetadataResolver,
			};

			var rootTypeMapAssembly = CreateTypeMapRootAssembly (assemblyParameters, typesByAssembly.Select (v => v.Key));

			var categoryMethodsByType = App.StaticRegistrar.Types
				.Where (v => v.Value.IsCategory)
				.SelectMany (v => v.Value.Methods!.Select (m => (Type: v.Value.BaseType!.Type, Method: m)))
				.GroupBy (v => v.Type)
				.ToDictionary (v => v.Key, v => v.Select (m => m.Method).ToList ());

			var trampolinesByMethod = Configuration.AssemblyTrampolineInfos
				.SelectMany (v => v.Value.Select (t => (Assembly: v.Key, TrampolineInfo: t)))
				.ToDictionary (v => v.TrampolineInfo.Target, v => v.TrampolineInfo);

			var trampolinesByType = Configuration.AssemblyTrampolineInfos
				.SelectMany (v => v.Value.Select (t => (Assembly: v.Key, TrampolineInfo: t)))
				.GroupBy (v => v.TrampolineInfo.Target.DeclaringType)
				.ToDictionary (v => v.Key, v => v.Select (t => t.TrampolineInfo).ToList ());


			// If we need to modify an assembly that's not the typemap assembly, do it after we've finished writing the typemap assembly,
			// to avoid having to switch between assemblies (we cache a lot of stuff, and those caches will have to be re-created for every switch).
			var postActionsByAssembly = new Dictionary<AssemblyDefinition, List<Action<AssemblyDefinition>>> ();
			void addPostAction (AssemblyDefinition assembly, Action<AssemblyDefinition> action)
			{
				if (!postActionsByAssembly.TryGetValue (assembly, out var actions)) {
					actions = new List<Action<AssemblyDefinition>> ();
					postActionsByAssembly.Add (assembly, actions);
				}
				actions.Add (action);
			}

			foreach (var typesInAssembly in typesByAssembly.OrderBy (v => v.Key.FullName)) {
				var assembly = typesInAssembly.Key;
				var types = typesInAssembly.ToList ();

				var typeMapAssemblyName = new AssemblyNameDefinition ("_" + assembly.Name.Name + ".TypeMap", new Version (1, 0, 0, 0));
				var typeMapAssembly = AssemblyDefinition.CreateAssembly (typeMapAssemblyName, typeMapAssemblyName.Name, assemblyParameters);
				var existingAction = Annotations.GetAction (assembly);
				Annotations.SetAction (typeMapAssembly, existingAction);
				addedAssemblies.Add (typeMapAssembly);

				var accessesAssemblies = new HashSet<AssemblyDefinition> ();
				accessesAssemblies.Add (assembly);

				abr.SetCurrentAssembly (typeMapAssembly);

				/*
				 * [assembly: IgnoresAccessChecksTo ("...")]
				 */
				var ignoredAccessChecks = new TypeDefinition ("System.Runtime.CompilerServices", "IgnoresAccessChecksToAttribute", TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, abr.System_Attribute);
				var ignoredAccessChecksCtor = new MethodDefinition (".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, abr.System_Void);
				ignoredAccessChecksCtor.AddParameter ("assemblyName", abr.System_String);
				il = ignoredAccessChecksCtor.Body.GetILProcessor ();
				il.Append (il.Create (OpCodes.Ldarg_0));
				il.Append (il.Create (OpCodes.Call, abr.System_Attribute__ctor));
				il.Append (il.Create (OpCodes.Ret));
				ignoredAccessChecks.Methods.Add (ignoredAccessChecksCtor);
				typeMapAssembly.MainModule.Types.Add (ignoredAccessChecks);

				// INativeObject subclasses
				var inativeObjectTypes = StaticRegistrar.GetAllTypes (assembly).Where (t => !t.IsInterface && !t.IsAbstract && t.IsNativeObject ());
				foreach (var tr in inativeObjectTypes.OrderBy (v => v.FullName)) {
					var inativeObjCtor = ManagedRegistrarLookupTablesStep.FindINativeObjectConstructor (tr);
					if (inativeObjCtor is null)
						continue;

					var trImported = typeMapAssembly.MainModule.ImportReference (tr);
					var trNamespace = GetNamespace (tr);

					/*
					* [..._Proxy]
					* sealed class ..._Proxy : INativeObjectProxyAttribute {
					* }
					*/
					var proxyType = new TypeDefinition (trNamespace, tr.Name + "_Proxy", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, abr.ObjCRuntime_INativeObjectProxyAttribute);
					typeMapAssembly.MainModule.Types.Add (proxyType);

					/* default ctor */
					var ctor = proxyType.AddMethod (".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, abr.System_Void);
					il = ctor.Body.GetILProcessor ();
					il.Append (il.Create (OpCodes.Ldarg_0));
					il.Append (il.Create (OpCodes.Call, abr.ObjCRuntime_INativeObjectProxyAttribute__ctor));
					il.Append (il.Create (OpCodes.Ret));

					/*
					* public override INativeObject? CreateObject (IntPtr handle, bool owns)
					* {
					*     return new ... (handle, owns);
					* }	
					*/
					var createObjectMethod = proxyType.AddMethod ("CreateObject", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, abr.ObjCRuntime_INativeObject);
					createObjectMethod.AddParameter ("handle", abr.System_IntPtr);
					createObjectMethod.AddParameter ("owns", abr.System_Boolean);
					il = createObjectMethod.Body.GetILProcessor ();
					il.Append (il.Create (OpCodes.Ldarg_1));
					if (inativeObjCtor.Parameters [0].ParameterType.Is ("ObjCRuntime", "NativeHandle"))
						il.Append (il.Create (OpCodes.Call, abr.NativeObject_op_Implicit_NativeHandle));
					il.Append (il.Create (OpCodes.Ldarg_2));
					il.Append (il.Create (OpCodes.Newobj, abr.CurrentAssembly.MainModule.ImportReference (inativeObjCtor)));
					il.Append (il.Create (OpCodes.Ret));

					// We add the proxy type as an attribute to itself
					attribute = abr.CreateAttribute (ctor);
					proxyType.CustomAttributes.Add (attribute);

					/*
					 * Add the [TypeMapAssociation] attribute for the protocol wrapper type as well
					 *
					 * [assembly: TypeMapAssociation<INativeObject> (typeof (...), typeof (...))]
					 */
					attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssociationAttribute_1_Constructor_Type_Type, abr.ObjCRuntime_INativeObject));
					attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, trImported));
					attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, proxyType));
					typeMapAssembly.CustomAttributes.Add (attribute);
				}

				foreach (var kvp in typesInAssembly.OrderBy (v => v.Key.FullName)) {
					var tr = kvp.Key;
					var trNamespace = GetNamespace (tr);
					var trImported = typeMapAssembly.MainModule.ImportReference (tr);
					var td = tr.Resolve ();
					var objcType = kvp.Value;
					var objcClassName = objcType.ExportedName;
					var isCustomType = App.StaticRegistrar.IsCustomType (objcType);

					if (!objcType.IsProtocol && !objcType.IsCategory) {
						if (skippedActualTypes.Contains (td)) {
							// Workaround for https://github.com/dotnet/runtime/issues/127504
							// Tracking issue: https://github.com/dotnet/macios/issues/25275
							// Use the 2-arg (unconditional) TypeMap constructor for types that are
							// the target of a SkippedObjectiveCTypeUniverse association, because
							// their conditional (3-arg) entries would be incorrectly trimmed.
							// See the comment where skippedActualTypes is created for full details.
							attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAttribute_1_Constructor_String_Type, abr.Foundation_NSObject));
							attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, objcClassName));
							attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, trImported));
						} else {
							/*
							 * [assembly: TypeMap<NSObject> ("Objective-C class name", typeof (...), typeof (...))]
							 */
							attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAttribute_1_Constructor_String_Type_Type, abr.Foundation_NSObject));
							attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, objcClassName));
							attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, trImported));
							attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, trImported));
						}
						typeMapAssembly.CustomAttributes.Add (attribute);

						/*
						 * [..._Proxy]
						 * sealed class ..._Proxy : NSObjectProxy {
						 * }
						 */
						var proxyType = new TypeDefinition (trNamespace, tr.Name + "_Proxy", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, abr.ObjCRuntime_NSObjectProxyAttribute);
						typeMapAssembly.MainModule.Types.Add (proxyType);

						/* default ctor */
						var ctor = proxyType.AddMethod (".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, abr.System_Void);
						il = ctor.Body.GetILProcessor ();
						il.Append (il.Create (OpCodes.Ldarg_0));
						il.Append (il.Create (OpCodes.Call, abr.ObjCRuntime_NSObjectProxy__ctor));
						il.Append (il.Create (OpCodes.Ret));

						/*
						 * public override NSObject? CreateObject (IntPtr handle)
						 * {
						 *     return new ... (handle);
						 * }	
						 */
						var createObjectMethod = proxyType.AddMethod ("CreateObject", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, abr.Foundation_NSObject);
						createObjectMethod.AddParameter ("handle", abr.System_IntPtr);
						il = createObjectMethod.Body.GetILProcessor ();
						var nativeHandleCtor = ManagedRegistrarLookupTablesStep.FindNSObjectConstructor (td);
						if (nativeHandleCtor is not null) {
							il.Append (il.Create (OpCodes.Ldarg_1));
							if (nativeHandleCtor.Parameters [0].ParameterType.Is ("ObjCRuntime", "NativeHandle"))
								il.Append (il.Create (OpCodes.Call, abr.NativeObject_op_Implicit_NativeHandle));
							il.Append (il.Create (OpCodes.Newobj, abr.CurrentAssembly.MainModule.ImportReference (nativeHandleCtor)));
						} else {
							il.Append (il.Create (OpCodes.Ldnull));
						}
						il.Append (il.Create (OpCodes.Ret));

						/*
						 * public override IntPtr GetClassHandle (out bool is_custom_type)
						 * {
						 * 	   is_custom_type = ...;
						 * 	   return Class.GetHandle ("...");
						 * }
						 */
						var getClassHandleMethod = proxyType.AddMethod ("GetClassHandle", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, abr.System_IntPtr);
						getClassHandleMethod.AddParameter ("is_custom_type", abr.System_Boolean.MakeByReferenceType ());
						il = getClassHandleMethod.Body.GetILProcessor ();
						il.Append (il.Create (OpCodes.Ldarg_1));
						il.Append (il.Create (isCustomType ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0));
						il.Append (il.Create (OpCodes.Stind_I1));
						il.Append (il.Create (OpCodes.Ldstr, objcClassName));
						il.Append (il.Create (OpCodes.Call, abr.Class_GetHandle__System_String));
						il.Append (il.Create (OpCodes.Call, abr.NativeObject_op_Implicit_IntPtr));
						il.Append (il.Create (OpCodes.Ret));

						/*
						 * public override IntPtr LookupUnmanagedFunction (string name)
						 * {
						 *     if (name == "funcA")
						 *         return &funcA;
						 *     if (name == "funcB")
						 *         return &funcB;
						 *     return IntPtr.Zero;
						 * }
						 */
						var lookupUnmanagedFunctionMethod = proxyType.AddMethod ("LookupUnmanagedFunction", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, abr.System_IntPtr);
						lookupUnmanagedFunctionMethod.AddParameter ("name", abr.System_String);
						il = lookupUnmanagedFunctionMethod.Body.GetILProcessor ();

						// Get all the UnmanagedCallersOnly methods we need to be able to find for the current type, which includes:
						// - methods from the type itself
						// - methods from categories on the type
						var uco = new List<TrampolineInfo> ();
						if (categoryMethodsByType.Remove (td, out var categoryMethods)) {
							foreach (var m in categoryMethods.OrderBy (v => v.FullName)) {
								if (!trampolinesByMethod.Remove (m.Method!, out var info)) {
									AddException (ErrorHelper.CreateWarning (4191, Errors.MX4191 /* Could not find the trampoline for the category method {0}. */, m.Method?.FullName));
									continue;
								}
								trampolinesByType.Remove (m.CategoryType!.Type.Resolve ());
								uco.Add (info);
								accessesAssemblies.Add (info.Trampoline.Module.Assembly);
							}
						}
						if (trampolinesByType.Remove (td, out var trampolines)) {
							uco.AddRange (trampolines);
							foreach (var info in trampolines) {
								trampolinesByMethod.Remove (info.Target);
							}
						}

						var ucos = uco.OrderBy (v => v.UnmanagedCallersOnlyEntryPoint).ToList ();
						var ldcI4 = il.Create (OpCodes.Ldc_I4_0);
						for (var i = 0; i < ucos.Count; i++) {
							var info = ucos [i];
							var isLast = i == ucos.Count - 1;
							var falseTarget = isLast ? ldcI4 : il.Create (OpCodes.Nop);
							il.Append (il.Create (OpCodes.Ldarg_1));
							il.Append (il.Create (OpCodes.Ldstr, info.UnmanagedCallersOnlyEntryPoint));
							il.Append (il.Create (OpCodes.Call, abr.System_String__op_Equality_String_String));
							il.Append (il.Create (OpCodes.Brfalse_S, falseTarget));
							//     return &Method;
							il.Append (il.Create (OpCodes.Ldftn, abr.CurrentAssembly.MainModule.ImportReference (info.Trampoline)));
							il.Append (il.Create (OpCodes.Ret));
							if (!isLast)
								il.Append (falseTarget);
						}
						// CWL
						// il.Append (il.Create (OpCodes.Ldstr, $"{proxyType.FullName}.LookupUnmanagedFunction ({{0}}): did not find this UCO method, among: {string.Join (", ", uco.Select (v => v.UnmanagedCallersOnlyEntryPoint))}"));
						// il.Append (il.Create (OpCodes.Ldarg_1));
						// il.Append (il.Create (OpCodes.Call, abr.System_Console__WriteLine_String_Object));
						//
						// return IntPtr.Zero
						il.Append (ldcI4);
						il.Append (il.Create (OpCodes.Conv_I));
						il.Append (il.Create (OpCodes.Ret));

						// We add the proxy type as an attribute to itself
						attribute = abr.CreateAttribute (ctor);
						proxyType.CustomAttributes.Add (attribute);

						// We also add the proxy type as an attribute to the type, as a workaround for https://github.com/dotnet/runtime/issues/127004
						// Tracking issue: https://github.com/dotnet/macios/issues/25276
						addPostAction (td.Module.Assembly, assembly => {
							var attribute = new CustomAttribute (assembly.MainModule.ImportReference (ctor)); // don't use abr.CreateAttribute here, because the ctor has already been marked
							td.CustomAttributes.Add (attribute);
						});

						/*
						 * Add the [TypeMapAssociation] attribute for this type and its proxy
						 *
						 * [assembly: TypeMapAssociation<NSObject> (typeof (...), typeof (...))]
						 */
						attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssociationAttribute_1_Constructor_Type_Type, abr.Foundation_NSObject));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, trImported));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, proxyType));
						typeMapAssembly.CustomAttributes.Add (attribute);
					}

					if (objcType.IsProtocol && objcType.ProtocolWrapperType is not null) {
						/*
						 * [..._Proxy]
						 * sealed class ..._Proxy : ProtocolProxyAttribute {
						 * }
						 */
						var proxyType = new TypeDefinition (trNamespace, tr.Name + "_Proxy", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, abr.ObjCRuntime_ProtocolProxyAttribute);
						typeMapAssembly.MainModule.Types.Add (proxyType);

						/* default ctor */
						var ctor = proxyType.AddMethod (".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, abr.System_Void);
						il = ctor.Body.GetILProcessor ();
						il.Append (il.Create (OpCodes.Ldarg_0));
						il.Append (il.Create (OpCodes.Call, abr.ObjCRuntime_ProtocolProxy__ctor));
						il.Append (il.Create (OpCodes.Ret));

						/*
						 * public override INativeObject? CreateObject (IntPtr handle, bool owns)
						 * {
						 *     return new ... (handle, owns);
						 * }	
						 */
						var createObjectMethod = proxyType.AddMethod ("CreateObject", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, abr.ObjCRuntime_INativeObject);
						createObjectMethod.AddParameter ("handle", abr.System_IntPtr);
						createObjectMethod.AddParameter ("owns", abr.System_Boolean);
						createObjectMethod.CreateBody (out il);
						var nativeHandleCtor = ManagedRegistrarLookupTablesStep.FindINativeObjectConstructor (objcType.ProtocolWrapperType.Resolve ());
						if (nativeHandleCtor is not null) {
							il.Append (il.Create (OpCodes.Ldarg_1));
							if (nativeHandleCtor.Parameters [0].ParameterType.Is ("ObjCRuntime", "NativeHandle"))
								il.Append (il.Create (OpCodes.Call, abr.NativeObject_op_Implicit_NativeHandle));
							il.Append (il.Create (OpCodes.Ldarg_2));
							il.Append (il.Create (OpCodes.Newobj, abr.CurrentAssembly.MainModule.ImportReference (nativeHandleCtor)));
						} else {
							il.Append (il.Create (OpCodes.Ldnull));
						}
						il.Append (il.Create (OpCodes.Ret));

						/*
						 * public override string GetName ()
						 * {
						 * 	   return "";
						 * }
						 */
						var getProtocolNameMethod = new MethodDefinition ("GetProtocolName", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, abr.System_String);
						il = getProtocolNameMethod.Body.GetILProcessor ();
						il.Append (il.Create (OpCodes.Ldstr, objcType.ProtocolName));
						il.Append (il.Create (OpCodes.Ret));
						proxyType.Methods.Add (getProtocolNameMethod);

						// We add the proxy type as an attribute to itself
						attribute = abr.CreateAttribute (ctor);
						proxyType.CustomAttributes.Add (attribute);

						/*
						 * Add the [TypeMapAssociation] attribute for the protocol wrapper type as well
						 *
						 * [assembly: TypeMapAssociation<ProtocolProxyAttribute> (typeof (...), typeof (...))]
						 */
						attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssociationAttribute_1_Constructor_Type_Type, abr.ObjCRuntime_ProtocolProxyAttribute));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, trImported));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, proxyType));
						typeMapAssembly.CustomAttributes.Add (attribute);

						/*
						 * Add the [TypeMapAssociation] attribute for the protocol wrapper type as well
						 *
						 * [assembly: TypeMapAssociation<ProtocolAttribute> (typeof (...), typeof (...))]
						 */
						attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssociationAttribute_1_Constructor_Type_Type, abr.Foundation_ProtocolAttribute));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, trImported));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, abr.CurrentAssembly.MainModule.ImportReference (objcType.ProtocolWrapperType)));
						typeMapAssembly.CustomAttributes.Add (attribute);

						// We also add the proxy type as an attribute to the type, as a workaround for https://github.com/dotnet/runtime/issues/127004
						// Tracking issue: https://github.com/dotnet/macios/issues/25276
						addPostAction (td.Module.Assembly, assembly => {
							var attribute = new CustomAttribute (assembly.MainModule.ImportReference (ctor)); // don't use abr.CreateAttribute here, because the ctor has already been marked
							td.CustomAttributes.Add (attribute);
						});
					}
				}

				foreach (var accessesAssembly in accessesAssemblies.OrderBy (v => v.FullName)) {
					var attrib = abr.CreateAttribute (ignoredAccessChecksCtor);
					attrib.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_String, accessesAssembly.Name.Name));
					typeMapAssembly.CustomAttributes.Add (attrib);
				}

				if (skippedTypesByAssembly.Remove (assembly, out var skippedTypes)) {
					foreach (var skipped in skippedTypes.OrderBy (v => v.Skipped.FullName)) {
						/*
						 * [assembly: TypeMapAssociation<SkippedObjectiveCTypeUniverse> (typeof (...), typeof (...))]
						 */
						attribute = abr.CreateAttribute (CreateMethodReference (abr.TypeMapAssociationAttribute_1_Constructor_Type_Type, abr.ObjCRuntime_SkippedObjectiveCTypeUniverse));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, typeMapAssembly.MainModule.ImportReference (skipped.Skipped)));
						attribute.ConstructorArguments.Add (new CustomAttributeArgument (abr.System_Type, typeMapAssembly.MainModule.ImportReference (skipped.Actual.Type)));
						typeMapAssembly.CustomAttributes.Add (attribute);
					}
				}

				abr.ClearCurrentAssembly ();

				// We write the assembly here even if it hasn't changed, because otherwise we'll just end up re-creating
				// it again during the next incremental build.
				typeMapAssembly.Write (Path.Combine (App.TypeMapOutputDirectory, typeMapAssembly.Name.Name + ".dll"));
			}

			foreach (var kvp in postActionsByAssembly) {
				var assembly = kvp.Key;
				var actions = kvp.Value;
				abr.SetCurrentAssembly (assembly);
				foreach (var action in actions) {
					action (assembly);
				}
				abr.ClearCurrentAssembly ();
			}

			// Since we're running inside the trimmer, we need to make sure the trimmer knows about the assemblies we've created.
			// This will go away once we're running outside of the trimmer.
			var managedAssemblyToLinkItems = new List<MSBuildItem> ();
			var resolver = abr.PlatformAssembly.MainModule.AssemblyResolver;
			var getAssembly = resolver.GetType ().GetMethod ("GetAssembly", new Type [] { typeof (string) })!;
			var cacheAssembly = resolver.GetType ().GetMethod ("CacheAssembly", new Type [] { typeof (AssemblyDefinition) })!;
			foreach (var asm in addedAssemblies) {
				var fn = Path.Combine (App.TypeMapOutputDirectory, asm.Name.Name + ".dll");
				var asmDef = (AssemblyDefinition) getAssembly.Invoke (resolver, [fn])!;
				cacheAssembly.Invoke (resolver, [asmDef]);
				var action = Annotations.GetAction (asm);
				Annotations.SetAction (asmDef, action);

				var linkedPath = Path.Combine (Configuration.IntermediateLinkDir, asm.Name.Name + ".dll");
				managedAssemblyToLinkItems.Add (new MSBuildItem (linkedPath, new Dictionary<string, string> {
					{ "TrimMode", "link" },
				}));
			}

			Configuration.WriteOutputForMSBuild ("ManagedAssemblyToLink", managedAssemblyToLinkItems);

			// Report back any exceptions that occurred during the processing.
			exceptions = this.exceptions;
		}
	}
}
