// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;

using MonoTouch.Tuner;

using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class ManagedRegistrarStepTests : BaseClass {
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
			var cctor = type.Methods.Single (v => v.IsConstructor && v.IsStatic);
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
