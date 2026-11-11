// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using MonoTouch.Tuner;
using Xamarin.Linker;

namespace AssemblyPreparerTests;

public class SpecializeGenericRegistrationsTests : BaseClass {
	static string? Selector (MethodDefinition method) => (string?) method.CustomAttributes
		.SingleOrDefault (v => v.AttributeType.FullName == "Foundation.ExportAttribute")?.ConstructorArguments [0].Value;

	const string Code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		[Protocol]
		public interface IProtocolValue {
			[Export (""protocolValue:"")]
			int ProtocolValue ([BindAs (typeof (int), OriginalType = typeof (NSNumber))] int value);
		}

		public class Generic<T> : NSObject, IProtocolValue where T : NSObject {
			[Export (""hash"")]
			public override nuint GetNativeHash () => 42;

			[Export (""echo:"")]
			public virtual T Echo (T value) => value;

			[Export (""privateValue"")]
			int PrivateValue () => 17;

			[Export (""value"")]
			public virtual T Value { get; set; }

			[Export (""number"")]
			[BindAs (typeof (int), OriginalType = typeof (NSNumber))]
			public int Number { get; set; }

			public int ProtocolValue (int value) => value;
		}

		public class Intermediate<T> : Generic<NSArray<T>> where T : NSObject { }
		public class Closed : Intermediate<NSString> { }
		public sealed class Recursive : Generic<Recursive> { }
		public class Override : Generic<NSString> {
			public override NSString Echo (NSString value) => value;
		}
		public class Derived : Closed {
			public override NSArray<NSString> Echo (NSArray<NSString> value) => value;
		}
		";

	[TestCase (RegistrarMode.Static, null)]
	[TestCase (RegistrarMode.Static, "-specialize-generic-registrations")]
	[TestCase (RegistrarMode.Static, "+specialize-generic-registrations")]
	[TestCase (RegistrarMode.ManagedStatic, null)]
	[TestCase (RegistrarMode.ManagedStatic, "+specialize-generic-registrations")]
	[TestCase (RegistrarMode.TrimmableStatic, null)]
	[TestCase (RegistrarMode.TrimmableStatic, "+specialize-generic-registrations")]
	[TestCase (RegistrarMode.Dynamic, "+specialize-generic-registrations")]
	public void ClosedForwarders (RegistrarMode mode, string? optimization)
	{
		using var preparer = CreatePreparer (ApplePlatform.MacOSX, true, p => p.Registrar = mode, Code, out _,
			extraConfig: $"Registrar={mode}\nOptimize={optimization}\nTypeMapAssemblyName=_TypeMap\nTypeMapOutputDirectory={Xamarin.Cache.CreateTemporaryDirectory ()}");
		AssertPrepare (preparer);
		Assert.That (((TestLogger) preparer.Configuration.Logger).Errors, Is.Empty, "Registrar errors");
		var context = preparer.Configuration.DerivedLinkContext;
		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		var closed = assembly.MainModule.Types.Single (v => v.Name == "Closed");
		var forwarders = closed.Methods.Where (v => v.Name.StartsWith ("__Registrar_Specialized_", StringComparison.Ordinal)).ToArray ();
		var enabled = optimization == "+specialize-generic-registrations" && mode != RegistrarMode.Dynamic;
		Assert.That (forwarders.Select (Selector), Is.EquivalentTo (enabled ? new [] { "hash", "echo:", "privateValue", "value", "setValue:", "number", "setNumber:", "protocolValue:" } : []), "Closed exports");
		if (!enabled)
			return;

		var echo = forwarders.Single (v => Selector (v) == "echo:");
		Assert.That (echo.ReturnType.FullName, Is.EqualTo ("Foundation.NSArray`1<Foundation.NSString>"), "Closed return type");
		Assert.That (echo.Parameters.Single ().ParameterType.FullName, Is.EqualTo (echo.ReturnType.FullName), "Closed parameter type");
		var call = echo.Body.Instructions.Single (v => v.OpCode == OpCodes.Callvirt);
		var target = (MethodReference) call.Operand;
		Assert.That (target.Name, Is.EqualTo ("Echo"), "Virtual target");
		Assert.That (target.DeclaringType.FullName, Is.EqualTo ("Generic`1<Foundation.NSArray`1<Foundation.NSString>>"), "Closed target");
		Assert.That (target.ReturnType, Is.InstanceOf<GenericParameter> (), "MemberRef generic signature");
		Assert.That (target.Resolve ().Name, Is.EqualTo ("Echo"), "Resolvable MemberRef");

		var overridden = assembly.MainModule.Types.Single (v => v.Name == "Override");
		Assert.That (overridden.Methods.Count (v => Selector (v) == "echo:"), Is.Zero, "Existing override is not duplicated");
		var derived = assembly.MainModule.Types.Single (v => v.Name == "Derived");
		Assert.That (derived.Methods.Any (v => v.Name.StartsWith ("__Registrar_Specialized_", StringComparison.Ordinal)), Is.False, "Closed base forwarders are inherited");
		var recursive = assembly.MainModule.Types.Single (v => v.Name == "Recursive");
		var recursiveEcho = recursive.Methods.Single (v => Selector (v) == "echo:");
		Assert.That (recursiveEcho.ReturnType.FullName, Is.EqualTo ("Recursive"), "Recursive generic argument");
		Assert.That (assembly.MainModule.Types.Where (v => v.HasGenericParameters).SelectMany (v => v.Methods).Any (v => Selector (v) == "echo:" && v.Name.StartsWith ("__Registrar_", StringComparison.Ordinal)), Is.False, "Open types are not specialized");

		var privateForwarder = forwarders.Single (v => Selector (v) == "privateValue");
		var helper = ((MethodReference) privateForwarder.Body.Instructions.Single (v => v.OpCode == OpCodes.Call).Operand).Resolve ();
		Assert.That (helper.IsPublic, Is.True, "Private export helper accessibility");
		Assert.That (((MethodReference) helper.Body.Instructions.Single (v => v.OpCode == OpCodes.Call).Operand).Resolve ().Name, Is.EqualTo ("PrivateValue"), "Private export helper target");

		var number = forwarders.Single (v => Selector (v) == "number");
		Assert.That (number.MethodReturnType.CustomAttributes.Select (v => v.AttributeType.FullName), Does.Contain ("ObjCRuntime.BindAsAttribute"), "Property BindAs");
		var setNumber = forwarders.Single (v => Selector (v) == "setNumber:");
		Assert.That (setNumber.Parameters.Single ().CustomAttributes.Select (v => v.AttributeType.FullName), Does.Contain ("ObjCRuntime.BindAsAttribute"), "Setter BindAs");
		var protocolValue = forwarders.Single (v => Selector (v) == "protocolValue:");
		Assert.That (protocolValue.Parameters.Single ().CustomAttributes.Select (v => v.AttributeType.FullName), Does.Contain ("ObjCRuntime.BindAsAttribute"), "Protocol parameter BindAs");

		if (mode != RegistrarMode.Static) {
			var info = preparer.Configuration.AssemblyTrampolineInfos [assembly].Single (v => v.Target == echo);
			Assert.That (info.Trampoline.Body.Instructions.Where (v => v.OpCode == OpCodes.Call || v.OpCode == OpCodes.Callvirt)
				.Select (v => ((MethodReference) v.Operand).FullName), Does.Contain (echo.FullName), "Direct specialized callback");
		}
	}

	[Test]
	public void ReloadableAssemblyIsNotModified ()
	{
		using var preparer = CreatePreparer (ApplePlatform.MacOSX, true, p => p.Registrar = RegistrarMode.TrimmableStatic, Code, out _,
			extraConfig: "Registrar=trimmable-static\nOptimize=+specialize-generic-registrations", hotReloadCompatibleBuild: true, testAssemblyTrimMode: "copy");
		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new SpecializeGenericRegistrationsStep ().Process (context);
		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		Assert.That (preparer.Configuration.ModifiedAssemblies, Does.Not.Contain (assembly), "Reloadable assembly");
		Assert.That (assembly.MainModule.Types.SelectMany (v => v.Methods).Any (v => v.Name.StartsWith ("__Registrar_Specialized_", StringComparison.Ordinal)), Is.False, "No forwarders");
	}
}
