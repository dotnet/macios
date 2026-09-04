// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

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
