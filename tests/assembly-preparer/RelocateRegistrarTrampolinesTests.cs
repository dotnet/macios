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

	// When HotReloadCompatibleBuild is enabled with the TrimmableStatic registrar, the registrar
	// trampolines for methods declared in a *generic* NSObject subclass must also be relocated into
	// the companion assembly, without modifying the user assembly. Historically these were handled
	// by adding a proxy interface (and its implementation) onto the user type, which modified the
	// user assembly; instead they must now be dispatched via a generic helper + reflection emitted
	// entirely into the companion.
	[Test]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void GenericTrampolinesAreRelocated (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		public class MyGeneric<T> : NSObject where T : NSObject {
			[Export (""doSomething:"")]
			public void DoSomething (T value)
			{
			}

			[Export (""roundtrip:"")]
			public T Roundtrip (T value)
			{
				return value;
			}

			[Export (""computeValue:result:"")]
			public void ComputeValue (int value, out T result)
			{
				result = null;
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

		// The user assembly must not contain any generated proxy interface type.
		var proxyInterfaceTypes = AllTypes (userAssembly).Where (v => v.Name.StartsWith ("__IRegistrarGenericTypeProxy__", StringComparison.Ordinal)).ToList ();
		Assert.That (proxyInterfaceTypes, Is.Empty, "The user assembly should not contain any generated proxy interface type");

		var myGeneric = AllTypes (userAssembly).Single (v => v.Name == "MyGeneric`1");

		// The user type must not implement any generated proxy interface.
		var proxyInterfaceImpls = myGeneric.Interfaces.Where (v => v.InterfaceType.Name.StartsWith ("__IRegistrarGenericTypeProxy__", StringComparison.Ordinal)).ToList ();
		Assert.That (proxyInterfaceImpls, Is.Empty, "The user type should not implement any generated proxy interface");

		// The user type must not have a generated proxy implementation method.
		var proxyImplMethods = myGeneric.Methods.Where (v => v.Name.StartsWith ("__IRegistrarGenericTypeProxy__", StringComparison.Ordinal)).ToList ();
		Assert.That (proxyImplMethods, Is.Empty, "The user type should not have a generated proxy implementation method");

		// The user methods must not have [DynamicDependency] attributes pointing at the trampolines.
		foreach (var method in myGeneric.Methods) {
			var dda = method.CustomAttributes.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute" && v.ConstructorArguments.Any (a => a.Value is string s && s.StartsWith ("callback_", StringComparison.Ordinal))).ToList ();
			Assert.That (dda, Is.Empty, $"The user method {method.Name} should not have a [DynamicDependency] attribute pointing at a trampoline");
		}

		// The user type's static constructor must not have been given a [DynamicDependency] attribute
		// (that's how the proxy interface used to be kept alive - it modified the user assembly).
		var staticCtor = myGeneric.Methods.SingleOrDefault (v => v.IsConstructor && v.IsStatic);
		if (staticCtor is not null) {
			var dda = staticCtor.CustomAttributes.Where (v => v.AttributeType.Name == "DynamicDependencyAttribute").ToList ();
			Assert.That (dda, Is.Empty, "The user type's static constructor should not have a [DynamicDependency] attribute");
		}

		// The companion assembly must contain a top-level __Registrar_Callbacks__ type.
		var companionCallbackType = companionAssembly.MainModule.Types.SingleOrDefault (v => v.Name == "__Registrar_Callbacks__" && v.DeclaringType is null);
		Assert.That (companionCallbackType, Is.Not.Null, "The companion assembly should contain a top-level __Registrar_Callbacks__ type");

		// The companion must contain the UnmanagedCallersOnly trampolines...
		var trampolines = companionCallbackType.Methods.Where (v => v.CustomAttributes.Any (a => a.AttributeType.Name == "UnmanagedCallersOnlyAttribute")).ToList ();
		Assert.That (trampolines.Count, Is.GreaterThanOrEqualTo (1), "The companion should contain at least one [UnmanagedCallersOnly] trampoline");

		// ...as well as the generic helper methods the trampolines dispatch to via reflection.
		var genericHelpers = companionCallbackType.Methods.Where (v => v.HasGenericParameters).ToList ();
		Assert.That (genericHelpers.Count, Is.GreaterThanOrEqualTo (1), "The companion should contain at least one generic dispatch helper method");

		// The generic helper's first parameter is the instance (self), typed as the user generic type
		// closed over the helper's own generic parameters.
		var doSomethingHelper = genericHelpers.Single (v => v.Name.Contains ("DoSomething"));
		Assert.That (doSomethingHelper.Parameters.Count, Is.GreaterThanOrEqualTo (1), "The generic helper should have at least a 'self' parameter");
		Assert.That (doSomethingHelper.Parameters [0].Name, Is.EqualTo ("self"), "The generic helper's first parameter should be 'self'");

		// A generic helper for a method with an out/ref parameter represents that parameter as a
		// plain IntPtr: a pointer can't round-trip through reflection's object[] (it isn't boxable
		// and MethodInfo.Invoke can't marshal it), so the out/ref (native pointer) parameter is
		// passed as an IntPtr and the write-back happens through that pointer inside the helper body.
		var computeHelper = genericHelpers.Single (v => v.Name.Contains ("ComputeValue"));
		var outParameter = computeHelper.Parameters.Single (v => v.Name == "p1");
		Assert.That (outParameter.ParameterType.FullName, Is.EqualTo ("System.IntPtr"), "The generic helper's out parameter should be typed IntPtr");
		// A normal by-value parameter is unaffected: it keeps its native value type.
		var valueParameter = computeHelper.Parameters.Single (v => v.Name == "p0");
		Assert.That (valueParameter.ParameterType.FullName, Is.EqualTo ("System.Int32"), "The generic helper's by-value parameter should keep its native value type");

		// The companion must be granted access to the user assembly.
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
