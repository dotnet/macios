// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using Mono.Linker;

using Xamarin.Linker;
using Xamarin.Linker.Steps;
using Xamarin.Tuner;

namespace AssemblyPreparerTests;

public class NativeObjectFactoryTests {
	static TypeDefinition AddType (ModuleDefinition module, string ns, string name, TypeAttributes attributes = TypeAttributes.Public)
	{
		var type = new TypeDefinition (ns, name, attributes);
		module.Types.Add (type);
		return type;
	}

	static MethodDefinition AddFactory (TypeDefinition type, TypeReference self, GenericInstanceType iface, bool explicitImplementation)
	{
		var module = type.Module;
		var nativeHandle = module.Types.Single (v => v.Name == "NativeHandle");
		var method = new MethodDefinition (explicitImplementation ? iface.FullName + ".Create" : "Create",
			MethodAttributes.Static | (explicitImplementation ? MethodAttributes.Private : MethodAttributes.Public), self);
		method.Parameters.Add (new ParameterDefinition (nativeHandle));
		method.Parameters.Add (new ParameterDefinition (module.TypeSystem.Boolean));
		type.Methods.Add (method);
		if (explicitImplementation) {
			var declaration = new MethodReference ("Create", self, iface);
			declaration.Parameters.Add (new ParameterDefinition (nativeHandle));
			declaration.Parameters.Add (new ParameterDefinition (module.TypeSystem.Boolean));
			method.Overrides.Add (declaration);
		}
		return method;
	}

	static GenericInstanceType AddInterface (TypeDefinition type, TypeReference self)
	{
		var iface = new GenericInstanceType (type.Module.Types.Single (v => v.Name == "ICreatableNativeObject`1"));
		iface.GenericArguments.Add (self);
		type.Interfaces.Add (new InterfaceImplementation (iface));
		return iface;
	}

	static ModuleDefinition CreateModule ()
	{
		var resolver = new TestAssemblyResolver ();
		var module = ModuleDefinition.CreateModule ("NativeObjectFactories", new ModuleParameters { Kind = ModuleKind.Dll, AssemblyResolver = resolver });
		resolver.Assemblies.Add (module.Assembly);
		AddType (module, "ObjCRuntime", "NativeHandle");
		var nativeObject = AddType (module, "ObjCRuntime", "INativeObject", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
		var iface = AddType (module, "ObjCRuntime", "ICreatableNativeObject`1", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
		iface.GenericParameters.Add (new GenericParameter ("T", iface));
		iface.Interfaces.Add (new InterfaceImplementation (nativeObject));
		return module;
	}

	static LinkerConfiguration CreateConfiguration (ModuleDefinition module, ModuleDefinition corlib)
	{
		var config = new LinkerConfiguration (ConsoleLog.Instance, [
			"Platform=iOS",
			"AreAnyAssembliesTrimmed=true",
			$"TargetFramework={TargetFramework.GetTargetFramework (ApplePlatform.iOS)}",
			$"PlatformAssembly={module.Assembly.Name.Name}.dll",
		], "NativeObjectFactories.config") { AssemblyInfos = [] };
		config.Application.Registrar = RegistrarMode.ManagedStatic;
		config.Assemblies.Add (module.Assembly);
		config.Assemblies.Add (corlib.Assembly);
		config.AssemblyResolver.CacheAssembly (module.Assembly);
		config.AssemblyResolver.CacheAssembly (corlib.Assembly);
		config.Context.Annotations.SetAction (module.Assembly, AssemblyAction.Link);
		config.Context.Annotations.SetAction (corlib.Assembly, AssemblyAction.Copy);
		config.DerivedLinkContext.CachedIsNSObject = [];
		return config;
	}

	static MethodDefinition AddMethod (TypeDefinition type, string name, TypeReference returnType, MethodAttributes attributes, params TypeReference [] parameters)
	{
		var method = new MethodDefinition (name, attributes, returnType);
		foreach (var parameter in parameters)
			method.Parameters.Add (new ParameterDefinition (parameter));
		type.Methods.Add (method);
		return method;
	}

	static ModuleDefinition CreateCorlib (IAssemblyResolver resolver)
	{
		using var definition = ModuleDefinition.CreateModule ("System.Private.CoreLib", ModuleKind.Dll);
		var boolean = AddType (definition, "System", "Boolean");
		var voidType = AddType (definition, "System", "Void");
		var stringType = AddType (definition, "System", "String");
		var objectType = AddType (definition, "System", "Object");
		AddMethod (AddType (definition, "System", "GC"), "KeepAlive", voidType, MethodAttributes.Public | MethodAttributes.Static, objectType);
		var attribute = AddType (definition, "System", "Attribute");
		AddMethod (attribute, ".ctor", voidType, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
		var dependency = AddType (definition, "System.Diagnostics.CodeAnalysis", "DynamicDependencyAttribute");
		AddMethod (dependency, ".ctor", voidType, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, stringType);
		var runtimeTypeHandle = AddType (definition, "System", "RuntimeTypeHandle");
		AddMethod (runtimeTypeHandle, "Equals", boolean, MethodAttributes.Public, runtimeTypeHandle);
		using var stream = new MemoryStream ();
		definition.Write (stream);
		stream.Position = 0;
		var module = ModuleDefinition.ReadModule (stream, new ReaderParameters { InMemory = true, ReadingMode = ReadingMode.Immediate, AssemblyResolver = resolver });
		((TestAssemblyResolver) resolver).Assemblies.Add (module.Assembly);
		return module;
	}

	sealed class TestAssemblyResolver : IAssemblyResolver {
		public List<AssemblyDefinition> Assemblies { get; } = [];
		public AssemblyDefinition Resolve (AssemblyNameReference name) => Assemblies.Single (v => v.Name.Name == name.Name);
		public AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters) => Resolve (name);
		public void Dispose ()
		{
		}
	}

	[Test]
	public void SourceFactoryOnlyAssemblyGetsTypeMap ()
	{
		using var module = CreateModule ();
		var type = AddType (module, "Test", "Wrapper");
		var iface = AddInterface (type, type);
		AddFactory (type, type, iface, true);

		Assert.That (AppBundleRewriter.FindINativeObjectConstructor (type), Is.Null, "Factory-only wrapper");
		Assert.That (TrimmableRegistrarStep.GetAssembliesWithTypeMaps ([], [module.Assembly]), Is.EqualTo (new [] { module.Assembly }), "Assembly without Objective-C registrations");
	}

	[Test]
	public void ProxyCanCallPrivateSourceFactoryWithoutConstructor ()
	{
		using var module = CreateModule ();
		using var corlib = CreateCorlib (module.AssemblyResolver);
		var nativeHandle = module.Types.Single (v => v.Name == "NativeHandle");
		AddMethod (nativeHandle, "op_Implicit", nativeHandle, MethodAttributes.Public | MethodAttributes.Static, module.TypeSystem.IntPtr);
		var type = AddType (module, "Test", "Wrapper");
		var iface = AddInterface (type, type);
		var factory = AddFactory (type, type, iface, true);
		var config = CreateConfiguration (module, corlib);
		var companion = RegistrarCompanionAssembly.GetOrCreate (config, module.Assembly);
		using var companionAssembly = companion.Assembly;
		var proxy = AddType (companionAssembly.MainModule, "Test", "Wrapper_Proxy");
		var create = AddMethod (proxy, "CreateObject", companionAssembly.MainModule.ImportReference (module.Types.Single (v => v.Name == "INativeObject")), MethodAttributes.Public,
			companionAssembly.MainModule.TypeSystem.IntPtr, companionAssembly.MainModule.TypeSystem.Boolean);
		var rewriter = config.AppBundleRewriter;
		rewriter.SetCurrentAssembly (companionAssembly);
		rewriter.EmitINativeObjectProxyConstruction (create.Body.GetILProcessor (), type);
		companion.AddAccessCheckAttributes (rewriter);

		Assert.That (AppBundleRewriter.FindINativeObjectConstructor (type), Is.Null, "Factory-only wrapper");
		Assert.That (factory.IsPrivate, Is.True, "Source visibility is unchanged");
		Assert.That (create.Body.Instructions.Select (v => v.OpCode), Does.Not.Contain (OpCodes.Newobj), "No constructor reference");
		var call = create.Body.Instructions.Where (v => v.OpCode == OpCodes.Call).Select (v => (MethodReference) v.Operand).Last ();
		Assert.That (call.Name, Is.EqualTo (factory.Name), "Proxy calls the explicit source factory");
		Assert.That (companionAssembly.CustomAttributes.Where (v => v.AttributeType.FullName == "System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute")
			.Select (v => (string) v.ConstructorArguments [0].Value), Does.Contain (module.Assembly.Name.Name), "Companion can access the private source method");
	}

	[Test]
	public void LookupTableIncludesSourceFactoryWithoutConstructor ()
	{
		using var module = CreateModule ();
		using var corlib = CreateCorlib (module.AssemblyResolver);
		var nativeObject = module.Types.Single (v => v.Name == "INativeObject");
		var nativeHandle = module.Types.Single (v => v.Name == "NativeHandle");
		AddMethod (nativeObject, "_Xamarin_ConstructINativeObject", nativeObject, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.Virtual, nativeHandle, module.TypeSystem.Boolean);
		var registrarInterface = AddType (module, "ObjCRuntime", "IManagedRegistrar", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
		AddMethod (registrarInterface, "ConstructINativeObject", nativeObject, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract,
			corlib.Types.Single (v => v.Name == "RuntimeTypeHandle"), nativeHandle, module.TypeSystem.Boolean);
		var type = AddType (module, "Test", "Wrapper");
		var iface = AddInterface (type, type);
		var factory = AddFactory (type, type, iface, true);
		var registrar = AddType (module, "ObjCRuntime", "__Registrar__");
		var config = CreateConfiguration (module, corlib);
		var step = new LookupTablesTestStep ();
		step.Process (config.DerivedLinkContext);
		config.AppBundleRewriter.SetCurrentAssembly (module.Assembly);
		step.GenerateConstructINativeObject (registrar);
		var lookup = registrar.Methods.Single (v => v.Name == "ConstructINativeObject");

		Assert.That (AppBundleRewriter.FindINativeObjectConstructor (type), Is.Null, "Factory-only wrapper");
		Assert.That (lookup.Body.Instructions.Where (v => v.OpCode == OpCodes.Ldtoken).Select (v => ((TypeReference) v.Operand).FullName), Does.Contain (type.FullName), "Wrapper registered in lookup");
		Assert.That (lookup.Body.Instructions.Where (v => v.OpCode == OpCodes.Call).Select (v => ((MethodReference) v.Operand).Name), Does.Contain (factory.Name), "Lookup calls the source factory");
		Assert.That (lookup.Body.Instructions.Select (v => v.OpCode), Does.Not.Contain (OpCodes.Newobj), "No constructor reference");
	}

	sealed class LookupTablesTestStep : ManagedRegistrarLookupTablesStep {
		protected override void TryProcessAssembly (AssemblyDefinition assembly)
		{
		}
	}

	[TestCase (RegistrarMode.Dynamic)]
	[TestCase (RegistrarMode.Static)]
	[TestCase (RegistrarMode.ManagedStatic)]
	[TestCase (RegistrarMode.TrimmableStatic)]
	public void FactoryDependenciesAreConditionalOnWrapperType (RegistrarMode registrar)
	{
		using var module = CreateModule ();
		using var corlib = CreateCorlib (module.AssemblyResolver);
		var factories = new List<MethodDefinition> ();
		foreach (var name in new [] { "UsedWrapper", "UnusedWrapper" }) {
			var type = AddType (module, "Test", name);
			factories.Add (AddFactory (type, type, AddInterface (type, type), true));
			AddMethod (type, ".ctor", module.TypeSystem.Void, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
				module.Types.Single (v => v.Name == "NativeHandle"), module.TypeSystem.Boolean);
		}
		var config = CreateConfiguration (module, corlib);
		config.Application.Registrar = registrar;
		new MarkNSObjectsStep ().Process (config.DerivedLinkContext);

		Assert.That (config.Exceptions, Is.Empty, "Marking errors");
		Assert.That (module.Types.Single (v => v.Name == "<Module>").Methods, Is.Empty, "No module initializer roots unused wrappers");
		foreach (var factory in factories) {
			var cctor = factory.DeclaringType.Methods.Single (v => v.Name == ".cctor");
			Assert.That (cctor.CustomAttributes.Select (v => v.AttributeType.FullName), Does.Contain ("System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute"), "Factory dependency attribute");
			var dependency = cctor.CustomAttributes.Single (v => v.AttributeType.Name == "DynamicDependencyAttribute");
			Assert.That (dependency.ConstructorArguments.Select (v => v.Value), Is.EqualTo (new [] { DocumentationComments.GetSignature (factory) }), "Only the factory is kept, conditional on its own type");
		}
	}

	[TestCase (false)]
	[TestCase (true)]
	public void GeneratedBridgeCallsSourceFactory (bool generic)
	{
		using var module = CreateModule ();
		using var corlib = CreateCorlib (module.AssemblyResolver);
		var nativeObject = module.Types.Single (v => v.Name == "INativeObject");
		var contract = new MethodDefinition ("_Xamarin_ConstructINativeObject", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.Virtual, nativeObject);
		contract.Parameters.Add (new ParameterDefinition (module.Types.Single (v => v.Name == "NativeHandle")));
		contract.Parameters.Add (new ParameterDefinition (module.TypeSystem.Boolean));
		nativeObject.Methods.Add (contract);
		var type = AddType (module, "Test", generic ? "Wrapper`1" : "Wrapper");
		TypeReference self = type;
		if (generic) {
			type.GenericParameters.Add (new GenericParameter ("T", type));
			var instance = new GenericInstanceType (type);
			instance.GenericArguments.Add (type.GenericParameters [0]);
			self = instance;
		}
		var iface = AddInterface (type, self);
		var factory = AddFactory (type, self, iface, true);
		var config = CreateConfiguration (module, corlib);
		var rewriter = new AppBundleRewriter (config);
		rewriter.SetCurrentAssembly (module.Assembly);

		Assert.That (rewriter.ImplementConstructINativeObjectFactoryMethod (config.DerivedLinkContext, type, null), Is.True, "Generated");
		var bridge = type.Methods.Single (v => v.Name == "_Xamarin_ConstructINativeObject");
		Assert.That (bridge.Body.Instructions.Select (v => v.OpCode), Does.Not.Contain (OpCodes.Newobj), "No duplicate constructor reference");
		var call = (MethodReference) bridge.Body.Instructions.Single (v => v.OpCode == OpCodes.Call).Operand;
		Assert.That (call.Name, Is.EqualTo (factory.Name), "Source factory call");
		Assert.That (call.DeclaringType.FullName, Is.EqualTo (self.FullName), "Generic self arguments");
		Assert.That (call.Resolve (), Is.SameAs (factory), "Resolved source factory");
		bridge.Name = "ObjCRuntime.INativeObject._Xamarin_ConstructINativeObject";
		bridge.IsPrivate = true;
		Assert.That (rewriter.ImplementConstructINativeObjectFactoryMethod (config.DerivedLinkContext, type, null), Is.False, "Existing override is retained");
		Assert.That (type.Methods.Count (v => v.Overrides.Any (o => o.Name == contract.Name)), Is.EqualTo (1), "Single bridge");
	}

	[TestCase (RegistrarMode.Dynamic)]
	[TestCase (RegistrarMode.Static)]
	[TestCase (RegistrarMode.ManagedStatic)]
	[TestCase (RegistrarMode.TrimmableStatic)]
	public void PreservesSourceFactoryInsteadOfConstructor (RegistrarMode registrar)
	{
		using var module = CreateModule ();
		using var corlib = ModuleDefinition.CreateModule ("System.Private.CoreLib", ModuleKind.Dll);
		var type = AddType (module, "Test", "Wrapper");
		var iface = AddInterface (type, type);
		var factory = AddFactory (type, type, iface, true);
		var ctor = new MethodDefinition (".ctor", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, module.TypeSystem.Void);
		ctor.Parameters.Add (new ParameterDefinition (module.Types.Single (v => v.Name == "NativeHandle")));
		ctor.Parameters.Add (new ParameterDefinition (module.TypeSystem.Boolean));
		type.Methods.Add (ctor);
		var config = CreateConfiguration (module, corlib);
		config.Application.Registrar = registrar;
		var marker = new RecordingMarker (config);

		Assert.That (MarkNSObjectsImpl.ProcessType (marker, type), Is.True, "Factory preservation");
		Assert.That (marker.Methods, Is.EqualTo (new [] { (type, factory) }), "Only the source factory is preserved, conditional on its declaring type");
		Assert.That (marker.Types, Is.Empty, "No new type roots");
	}

	sealed class RecordingMarker (LinkerConfiguration config) : IMarkNSObjects {
		public List<(TypeDefinition Type, MethodDefinition Method)> Methods { get; } = [];
		public List<TypeDefinition> Types { get; } = [];
		public AnnotationStore Annotations => config.Context.Annotations;
		public Application App => config.Application;
		public DerivedLinkContext Context => config.DerivedLinkContext;

		public bool PreserveMethod (TypeDefinition onType, MethodDefinition method)
		{
			Methods.Add ((onType, method));
			return true;
		}

		public bool PreserveType (TypeDefinition type, bool allMembers)
		{
			Types.Add (type);
			return true;
		}

		public bool PreserveType (TypeDefinition onType, TypeDefinition type)
		{
			Types.Add (type);
			return true;
		}
	}

	[TestCase (false, false)]
	[TestCase (false, true)]
	[TestCase (true, false)]
	[TestCase (true, true)]
	public void FindsExactSelfFactory (bool generic, bool explicitImplementation)
	{
		using var module = CreateModule ();
		var type = AddType (module, "Test", generic ? "Wrapper`1" : "Wrapper");
		TypeReference self = type;
		if (generic) {
			type.GenericParameters.Add (new GenericParameter ("T", type));
			var instance = new GenericInstanceType (type);
			instance.GenericArguments.Add (type.GenericParameters [0]);
			self = instance;
		}
		var iface = AddInterface (type, self);
		var factory = AddFactory (type, self, iface, explicitImplementation);

		Assert.That (AppBundleRewriter.FindINativeObjectFactory (type), Is.SameAs (factory), "Self factory");
	}

	[Test]
	public void DoesNotUseInheritedSelfFactory ()
	{
		using var module = CreateModule ();
		var baseType = AddType (module, "Test", "Base");
		var baseInterface = AddInterface (baseType, baseType);
		var baseFactory = AddFactory (baseType, baseType, baseInterface, true);
		var derived = AddType (module, "Test", "Derived");
		derived.BaseType = baseType;

		Assert.That (AppBundleRewriter.FindINativeObjectFactory (baseType), Is.SameAs (baseFactory), "Base factory");
		Assert.That (AppBundleRewriter.FindINativeObjectFactory (derived), Is.Null, "Inherited base factory is not a derived factory");

		var derivedInterface = AddInterface (derived, derived);
		var derivedFactory = AddFactory (derived, derived, derivedInterface, true);
		Assert.That (AppBundleRewriter.FindINativeObjectFactory (derived), Is.SameAs (derivedFactory), "Derived factory");
	}

	[Test]
	public void DoesNotMatchDifferentClosedSelfType ()
	{
		using var module = CreateModule ();
		var type = AddType (module, "Test", "Wrapper`1");
		type.GenericParameters.Add (new GenericParameter ("T", type));
		var closedSelf = new GenericInstanceType (type);
		closedSelf.GenericArguments.Add (module.TypeSystem.Int32);
		var iface = AddInterface (type, closedSelf);
		AddFactory (type, closedSelf, iface, true);

		Assert.That (AppBundleRewriter.FindINativeObjectFactory (type), Is.Null, "Closed Wrapper<int> does not implement the open self contract");
	}

	[Test]
	public void DoesNotMatchFactoryForOtherType ()
	{
		using var module = CreateModule ();
		var type = AddType (module, "Test", "Wrapper");
		var other = AddType (module, "Test", "Other");
		var iface = AddInterface (type, other);
		AddFactory (type, other, iface, true);

		Assert.That (AppBundleRewriter.FindINativeObjectFactory (type), Is.Null, "Other self type");
	}

	[Test]
	public void DoesNotMatchWrongClosedOverride ()
	{
		using var module = CreateModule ();
		var type = AddType (module, "Test", "Wrapper");
		AddInterface (type, type);
		var other = AddType (module, "Test", "Other");
		var otherInterface = AddInterface (type, other);
		AddFactory (type, type, otherInterface, true);

		Assert.That (AppBundleRewriter.FindINativeObjectFactory (type), Is.Null, "Override must target ICreatableNativeObject<Wrapper>, not ICreatableNativeObject<Other>");
	}
}
