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
	public void TypeMapOutputsOnlyChangeWithRegistration ()
	{
		static string Code (int value, string extraMethod = "") => $@"
		using Foundation;
		using ObjCRuntime;

		public class MyClass : NSObject {{
			[Export (""answer"")]
			public int Answer () => {value};
			{extraMethod}
		}}
		";

		var tempDir = Xamarin.Cache.CreateTemporaryDirectory ();
		var typeMapDir = Path.Combine (tempDir, "typemaps");
		var config = $@"
		AssemblyName=Microsoft.iOS.dll
		PrepareAssemblies=true
		TypeMapAssemblyName=_TypeMap
		TypeMapOutputDirectory={typeMapDir}
		UnmanagedCallersOnlyMapPath={Path.Combine (tempDir, "uco.txt")}
		";
		var rootPath = Path.Combine (typeMapDir, "_TypeMap.dll");
		var companionPath = Path.Combine (typeMapDir, "_Test.TypeMap.dll");
		void AssertTypeMapMvids (AssemblyPreparer preparer)
		{
			foreach (var added in preparer.AddedAssemblies.Where (v => v.Path == rootPath || v.Path == companionPath)) {
				using var onDisk = AssemblyDefinition.ReadAssembly (added.Path);
				Assert.That (added.Assembly.MainModule.Mvid, Is.EqualTo (onDisk.MainModule.Mvid), $"MVID of {added.Path}");
			}
		}

		AssemblyPreparerInfo [] infos;
		string projectDir;
		string userAssemblyPath;

		using (var preparer = CreatePreparer (ApplePlatform.iOS, false, p => p.Registrar = RegistrarMode.TrimmableStatic,
			Code (1), out var testInfo, extraConfig: config, hotReloadCompatibleBuild: true, testAssemblyTrimMode: "copy")) {
			infos = preparer.Assemblies.Select (v => new AssemblyPreparerInfo (v.InputPath, v.OutputPath, v.OriginalInputPath, v.IsTrimmable, v.TrimMode)).ToArray ();
			projectDir = Path.GetFullPath (Path.Combine (preparer.IntermediateOutputPath, ".."));
			userAssemblyPath = testInfo.InputPath;
			AssertPrepare (preparer);
			AssertTypeMapMvids (preparer);
		}

		Assert.That (File.Exists (rootPath), Is.True, "Root type map exists");
		Assert.That (File.Exists (companionPath), Is.True, "Companion type map exists");
		var originalRoot = File.ReadAllBytes (rootPath);
		var originalCompanion = File.ReadAllBytes (companionPath);
		var originalUserAssembly = File.ReadAllBytes (userAssemblyPath);
		var timestamp = new DateTime (2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		File.SetLastWriteTimeUtc (rootPath, timestamp);
		File.SetLastWriteTimeUtc (companionPath, timestamp);

		void Reprepare (string code)
		{
			File.WriteAllText (Path.Combine (projectDir, "Test.cs"), code);
			DotNet.AssertBuild (Path.Combine (projectDir, "Test.csproj"), new Dictionary<string, string> { ["TreatWarningsAsErrors"] = "false" });
			var logger = new TestLogger { Platform = ApplePlatform.iOS };
			var inputInfos = infos.Select (v => new AssemblyPreparerInfo (v.InputPath, v.OutputPath, v.OriginalInputPath, v.IsTrimmable, v.TrimMode)).ToArray ();
			using var preparer = new AssemblyPreparer (logger, inputInfos, Path.Combine (projectDir, "config.txt")) { Registrar = RegistrarMode.TrimmableStatic };
			AssertPrepare (preparer);
			AssertTypeMapMvids (preparer);
		}

		Reprepare (Code (2));
		Assert.That (File.ReadAllBytes (userAssemblyPath), Is.Not.EqualTo (originalUserAssembly), "User assembly changed");
		Assert.That (File.ReadAllBytes (rootPath), Is.EqualTo (originalRoot), "Root type map contents after a body edit");
		Assert.That (File.ReadAllBytes (companionPath), Is.EqualTo (originalCompanion), "Companion contents after a body edit");
		Assert.That (File.GetLastWriteTimeUtc (rootPath), Is.EqualTo (timestamp), "Root type map timestamp after a body edit");
		Assert.That (File.GetLastWriteTimeUtc (companionPath), Is.EqualTo (timestamp), "Companion timestamp after a body edit");

		Reprepare (Code (2, @"[Export (""other"")] public void Other () { }"));
		Assert.That (File.ReadAllBytes (companionPath), Is.Not.EqualTo (originalCompanion), "Companion contents after a new export");
		Assert.That (File.GetLastWriteTimeUtc (companionPath), Is.GreaterThan (timestamp), "Companion timestamp after a new export");
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
}
