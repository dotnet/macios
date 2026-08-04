// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;

namespace AssemblyPreparerTests;

public class RelocateRegistrarTrampolinesTests : BaseClass {
	// When HotReloadCompatibleBuild is enabled with the TrimmableStatic registrar, the registrar
	// trampolines (and the constructor helpers) must be emitted into the companion assembly
	// (_Test.TypeMap.dll) instead of the user (Test) assembly, so that the user assembly isn't
	// modified with registrar artifacts.
	[Test]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void TrampolinesAreRelocated (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		public class MyClass : NSObject {
			[Export (""initWithFoo:"")]
			public MyClass (int foo)
			{
			}

			[Export (""doSomething:"")]
			public void DoSomething (int value)
			{
			}

			[Export (""name"")]
			public string Name {
				get => string.Empty;
			}
		}
		";

		AssertPrepareHotReloadTrimmableStatic (platform, isCoreCLR, code, out var userAssemblyWasSaved, out var userAssembly, out var companionAssembly);

		// The whole point of relocating the trampolines is to keep the user assembly
		// byte-unmodified, so the assembly-preparer must not re-save it.
		Assert.That (userAssemblyWasSaved, Is.False, "The user assembly should not have been modified/re-saved");

		// The user assembly must not contain any registrar callback trampolines.
		var userCallbackTypes = AllTypes (userAssembly).Where (v => v.Name == "__Registrar_Callbacks__").ToList ();
		Assert.That (userCallbackTypes, Is.Empty, "The user assembly should not contain any __Registrar_Callbacks__ type");

		// The user type must not have a cloned constructor with a native handle parameter.
		var myClass = AllTypes (userAssembly).Single (v => v.Name == "MyClass");
		var clonedCtors = myClass.Methods.Where (v => v.IsConstructor && v.Parameters.Any (p => p.Name == "nativeHandle")).ToList ();
		Assert.That (clonedCtors, Is.Empty, "The user type should not have a cloned constructor with a nativeHandle parameter");

		// The user methods must not have [DynamicDependency] attributes pointing at the trampolines.
		foreach (var method in myClass.Methods) {
			var dda = method.CustomAttributes.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute" && v.ConstructorArguments.Any (a => a.Value is string s && s.StartsWith ("callback_", StringComparison.Ordinal))).ToList ();
			Assert.That (dda, Is.Empty, $"The user method {method.Name} should not have a [DynamicDependency] attribute pointing at a trampoline");
		}

		// The companion assembly must contain a top-level __Registrar_Callbacks__ type with the trampolines.
		var companionCallbackType = companionAssembly.MainModule.Types.SingleOrDefault (v => v.Name == "__Registrar_Callbacks__" && v.DeclaringType is null);
		Assert.That (companionCallbackType, Is.Not.Null, "The companion assembly should contain a top-level __Registrar_Callbacks__ type");

		var trampolines = companionCallbackType.Methods.Where (v => v.CustomAttributes.Any (a => a.AttributeType.Name == "UnmanagedCallersOnlyAttribute")).ToList ();
		Assert.That (trampolines.Count, Is.GreaterThanOrEqualTo (1), "The companion should contain at least one [UnmanagedCallersOnly] trampoline");

		// The companion must be granted access to the user assembly (and the platform assembly for the ctor).
		var ignoresAccessChecksTo = companionAssembly.CustomAttributes
			.Where (v => v.AttributeType.Name == "IgnoresAccessChecksToAttribute")
			.Select (v => (string) v.ConstructorArguments [0].Value)
			.ToList ();
		Assert.That (ignoresAccessChecksTo, Does.Contain ("Test"), "The companion should have [IgnoresAccessChecksTo(\"Test\")]");
	}

	static IEnumerable<TypeDefinition> AllTypes (AssemblyDefinition assembly)
	{
		foreach (var type in assembly.MainModule.Types) {
			yield return type;
			foreach (var nested in AllNestedTypes (type))
				yield return nested;
		}
	}

	static IEnumerable<TypeDefinition> AllNestedTypes (TypeDefinition type)
	{
		foreach (var nested in type.NestedTypes) {
			yield return nested;
			foreach (var deeper in AllNestedTypes (nested))
				yield return deeper;
		}
	}
}
