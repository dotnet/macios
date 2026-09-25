// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

using MonoTouch.Tuner;

using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class ManagedRegistrarStepTests : BaseClass {
	[Test]
	public void FactoryMethodsNotAddedToReloadableAssemblies ()
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			protected MyClass (NativeHandle handle)
				: base (handle)
			{
			}
		}
		";

		using var preparer = CreatePreparer (ApplePlatform.iOS, false, p => p.Registrar = RegistrarMode.TrimmableStatic, code, out _, hotReloadCompatibleBuild: true, testAssemblyTrimMode: "copy");
		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new ManagedRegistrarStep ().Process (context);
		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		var type = assembly.MainModule.Types.Single (v => v.Name == "MyClass");

		Assert.That (type.Methods.Select (v => v.Name), Does.Not.Contain ("_Xamarin_ConstructNSObject"), "NSObject factory");
		Assert.That (type.Methods.Select (v => v.Name), Does.Not.Contain ("_Xamarin_ConstructINativeObject"), "INativeObject factory");
		Assert.That (preparer.Configuration.ModifiedAssemblies, Does.Not.Contain (assembly), "Modified assemblies");
	}

	[Test]
	public void CollectsRelocatedProtocolCallbacksAfterPreparation ()
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		[Protocol]
		public interface IMyProtocol {
			[Export (""first"")]
			void First ();

			[Export (""second"")]
			void Second ();
		}

		public class MyClass : NSObject, IMyProtocol {
			public void First () { }
			public virtual void Second () { }
		}
		";
		var tempDir = Xamarin.Cache.CreateTemporaryDirectory ();
		var config = $@"
		AssemblyName=Microsoft.iOS.dll
		PrepareAssemblies=true
		TypeMapAssemblyName=_TypeMap
		TypeMapOutputDirectory={Path.Combine (tempDir, "typemaps")}
		UnmanagedCallersOnlyMapPath={Path.Combine (tempDir, "uco.txt")}
		";

		using var preparer = CreatePreparer (ApplePlatform.iOS, false, p => p.Registrar = RegistrarMode.TrimmableStatic,
			code, out _, extraConfig: config, hotReloadCompatibleBuild: true, testAssemblyTrimMode: "copy");
		AssertPrepare (preparer);

		var postDir = Path.Combine (tempDir, "post");
		var infos = preparer.Assemblies.Select (v => new AssemblyPreparerInfo (v.OutputPath, Path.Combine (postDir, Path.GetFileName (v.OutputPath)), v.OriginalInputPath, v.IsTrimmable, v.TrimMode))
			.Concat (preparer.AddedAssemblies.Select (v => new AssemblyPreparerInfo (v.Path, Path.Combine (postDir, Path.GetFileName (v.Path)), true, "link")))
			.ToArray ();
		var configPath = Path.Combine (Path.GetFullPath (Path.Combine (preparer.IntermediateOutputPath, "..")), "config.txt");
		var logger = new TestLogger { Platform = ApplePlatform.iOS };
		using var postprocessor = new AssemblyPreparer (logger, infos, configPath) { Registrar = RegistrarMode.TrimmableStatic };
		postprocessor.Configuration.Application.IsPostProcessingAssemblies = true;
		var resolver = new PreTrimTestResolver ();
		resolver.AddPaths (preparer.Assemblies.Select (v => v.InputPath));
		postprocessor.Configuration.Application.PreTrimAssemblyResolver = resolver;

		var context = postprocessor.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		var protocol = assembly.MainModule.Types.Single (v => v.Name == "IMyProtocol");
		// The pre-trim resolver still exposes this method after it is removed from the loaded assembly.
		protocol.Methods.Remove (protocol.Methods.Single (v => v.Name == "Second"));
		new ManagedRegistrarStep ().Process (context);

		var callbacks = postprocessor.Configuration.AssemblyTrampolineInfos [assembly];
		Assert.That (callbacks.Count (v => v.Target.Name == "First"), Is.EqualTo (1), "First callback (present in both protocol snapshots)");
		Assert.That (callbacks.Count (v => v.Target.Name == "Second"), Is.EqualTo (1), "Second callback (removed from the post-trim protocol)");
		Assert.That (resolver.TestResolveCount, Is.GreaterThan (0), "Pre-trim protocol lookup");
		Assert.That (logger.Errors, Is.Empty, "Postprocessing errors");
	}

	[Test]
	public void NSObjectFactory ()
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class NonGenericClass : NSObject {
			protected NonGenericClass (NativeHandle handle)
				: base (handle)
			{
			}
		}

		class GenericClass<T> : NSObject {
			protected GenericClass (NativeHandle handle)
				: base (handle)
			{
			}
		}
		";

		using var preparer = CreatePreparer (ApplePlatform.MacCatalyst, true, p => p.Registrar = RegistrarMode.TrimmableStatic, code, out _);
		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new ManagedRegistrarStep ().Process (context);
		var assemblyDefinition = context.GetAssemblies ().Single (v => v.Name.Name == "Test");

		var nonGenericType = assemblyDefinition.MainModule.Types.Single (v => v.Name == "NonGenericClass");
		Assert.That (nonGenericType.Interfaces.Select (v => v.InterfaceType.FullName), Does.Not.Contain ("Foundation.INSObjectFactory"), "Non-generic interfaces");
		Assert.That (nonGenericType.Methods.Select (v => v.Name), Does.Not.Contain ("_Xamarin_ConstructNSObject"), "Non-generic methods");
		Assert.That (nonGenericType.Interfaces.Select (v => v.InterfaceType.FullName), Does.Contain ("ObjCRuntime.INativeObject"), "Non-generic INativeObject interface");
		Assert.That (nonGenericType.Methods.Select (v => v.Name), Does.Contain ("_Xamarin_ConstructINativeObject"), "Non-generic INativeObject methods");

		var genericType = assemblyDefinition.MainModule.Types.Single (v => v.Name == "GenericClass`1");
		Assert.That (genericType.Interfaces.Select (v => v.InterfaceType.FullName), Does.Contain ("Foundation.INSObjectFactory"), "Generic interfaces");
		var factoryMethod = genericType.Methods.Single (v => v.Name == "_Xamarin_ConstructNSObject");
		Assert.That (factoryMethod.Overrides.Select (v => v.DeclaringType.FullName), Does.Contain ("Foundation.INSObjectFactory"), "Generic method overrides");

		Assert.That (GetInterfaceDependencies (nonGenericType), Is.Empty, "Non-generic interface dependencies");
		Assert.That (GetInterfaceDependencies (genericType), Is.Empty, "Generic interface dependencies");

		static IEnumerable<CustomAttribute> GetInterfaceDependencies (TypeDefinition type)
		{
			var cctor = type.Methods.SingleOrDefault (v => v.IsConstructor && v.IsStatic);
			if (cctor is null)
				return [];

			return cctor.CustomAttributes.Where (v =>
				v.AttributeType.FullName == "System.Diagnostics.CodeAnalysis.DynamicDependencyAttribute"
					&& v.ConstructorArguments.Count == 2
					&& v.ConstructorArguments [0].Type.FullName == "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes"
					&& (int) v.ConstructorArguments [0].Value == (int) DynamicallyAccessedMemberTypes.Interfaces);
		}
	}

	[TestCase (XamarinRuntime.CoreCLR, false)]
	[TestCase (XamarinRuntime.MonoVM, true)]
	public void UnmanagedCallersOnlyEntryPoint (XamarinRuntime runtime, bool expectedEntryPoint)
	{
		var code = @"
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Export (""myMethod"")]
			public void MyMethod ()
			{
			}
		}
		";

		// The runtime is configured independently of the reference assembly set used to compile the test code.
		AssertPrepare (ApplePlatform.iOS, false, RegistrarMode.ManagedStatic, code, out var assemblyDefinition, extraConfig: $"XamarinRuntime={runtime}");

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var callbackType = type.NestedTypes.Single (v => v.Name == "__Registrar_Callbacks__");
		var callback = callbackType.Methods.Single (v => v.Name.EndsWith ("_MyMethod", StringComparison.Ordinal));
		var attribute = callback.CustomAttributes.Single (v => v.AttributeType.FullName == "System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute");
		var entryPointFields = attribute.Fields.Where (v => v.Name == "EntryPoint").ToArray ();

		if (expectedEntryPoint) {
			Assert.That (entryPointFields, Has.Exactly (1).Items, "EntryPoint fields");
			Assert.That (entryPointFields [0].Argument.Value, Is.EqualTo (callback.Name), "EntryPoint");
		} else {
			Assert.That (entryPointFields, Is.Empty, "EntryPoint fields");
		}
	}

	sealed class PreTrimTestResolver : CoreResolver {
		readonly Dictionary<string, string> paths = new (StringComparer.OrdinalIgnoreCase);

		public int TestResolveCount { get; private set; }

		public void AddPaths (IEnumerable<string> assemblies)
		{
			foreach (var path in assemblies)
				paths [Path.GetFileNameWithoutExtension (path)] = path;
		}

		public override AssemblyDefinition Resolve (AssemblyNameReference name, ReaderParameters parameters)
		{
			if (name.Name == "Test")
				TestResolveCount++;
			if (ResolverCache.TryGetValue (name.Name, out var assembly))
				return assembly;
			if (paths.TryGetValue (name.Name, out var path))
				return CacheAssembly (AssemblyDefinition.ReadAssembly (path, parameters));
			throw new AssemblyResolutionException (name);
		}
	}
}
