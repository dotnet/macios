// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AssemblyPreparerTests;

public class OptimizeGeneratedCodeHandlerTests : BaseClass {
	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void OptimizeProtocolInterfaceStaticConstructor (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		[Protocol]
		interface IMyProtocol {
			[BindingImpl (BindingImplOptions.Optimizable)]
			static IMyProtocol () {
				GC.KeepAlive (null);
			}
		}

		class MyClass : NSObject, IMyProtocol {
		}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.RegisterProtocols = true;
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "IMyProtocol");
		var cctor = type.GetStaticConstructor ();

		Assert.That (cctor, Is.Not.Null, "Static constructor should still exist");
		Assert.That (cctor.Body.Instructions.Count, Is.EqualTo (1), "Static constructor should have only a ret instruction");
		Assert.That (cctor.Body.Instructions [0].OpCode.Code, Is.EqualTo (Code.Ret), "Static constructor should only contain ret");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void KeepProtocolStaticConstructorWhenOptimizationDisabled (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		[Protocol]
		interface IMyProtocol {
			[BindingImpl (BindingImplOptions.Optimizable)]
			static IMyProtocol () {
				GC.KeepAlive (null);
			}
		}

		class MyClass : NSObject, IMyProtocol {
		}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.RegisterProtocols = false;
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "IMyProtocol");
		var cctor = type.GetStaticConstructor ();

		Assert.That (cctor, Is.Not.Null, "Static constructor should still exist");
		Assert.That (cctor.Body.Instructions.Count, Is.GreaterThan (1), "Static constructor should not be optimized");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void NoOptimizationWithoutBindingAttributes (ApplePlatform platform, bool isCoreCLR)
	{
		// The method deliberately has no [BindingImpl (BindingImplOptions.Optimizable)] attribute, so
		// the optimizer must leave it untouched: the Runtime.IsARM64CallingConvention condition must not
		// be inlined and the dead 'return 2' must not be eliminated.
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Export (""myMethod"")]
			public int MyMethod () {
				if (Runtime.IsARM64CallingConvention) {
					return 1;
				}
				return 2;
			}
		}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.DeadCodeElimination = true;
			preparer.Optimizations.InlineIsARM64CallingConvention = true;
		}, code, out var outputPath, extraCsproj: "<PropertyGroup><Optimize>true</Optimize></PropertyGroup>", extraConfig: "TargetArchitectures=ARM64");

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "MyMethod");

		var hasDeadCode = method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Ldc_I4_2);
		Assert.That (hasDeadCode, Is.True, "Dead code (return 2) should be preserved without [BindingImpl(Optimizable)]");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void DeadCodeElimination (ApplePlatform platform, bool isCoreCLR)
	{
		// The Runtime.IsARM64CallingConvention condition is inlined to a constant, after which the
		// unreachable 'return 2' branch is eliminated (the method is optimizable via [BindingImpl]).
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[BindingImpl (BindingImplOptions.Optimizable)]
			[Export (""myMethod"")]
			public int MyMethod () {
				if (Runtime.IsARM64CallingConvention) {
					return 1;
				}
				return 2;
			}
		}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.DeadCodeElimination = true;
			preparer.Optimizations.InlineIsARM64CallingConvention = true;
		}, code, out var outputPath, extraCsproj: "<PropertyGroup><Optimize>true</Optimize></PropertyGroup>", extraConfig: "TargetArchitectures=ARM64");

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "MyMethod");

		// After dead code elimination, there should be no ldc.i4.2 (the unreachable return 2)
		var hasDeadCode = method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Ldc_I4_2);
		Assert.That (hasDeadCode, Is.False, "Dead code (return 2) should be eliminated");
	}
}
