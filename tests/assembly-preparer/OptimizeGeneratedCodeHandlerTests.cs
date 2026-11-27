// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AssemblyPreparerTests;

public class OptimizeGeneratedCodeHandlerTests : BaseClass {
	[TestCase ("CoreText.CTLine", "EnumerateCaretOffsets", "v@?dqB^B")]
	[TestCase ("ImageIO.CGImageAnimation", "AnimateImage", "v@?Q@^B")]
	[TestCase ("MetalPerformanceShaders.MPSImageBatch", "Iterate", "q@?@Q")]
	[TestCase ("Network.NWFramer", "set_InputHandler", "Q@?@")]
	public void NativeIntegerBlockSignatures (string typeName, string methodName, string expectedSignature)
	{
		using var preparer = CreatePreparer (ApplePlatform.iOS, true, p => {
			p.Registrar = RegistrarMode.Static;
			p.Optimizations.OptimizeBlockLiteralSetupBlock = true;
		}, "class Test { }", out _);
		var context = preparer.Configuration.DerivedLinkContext;
		new MonoTouch.Tuner.LoadAssembliesStep ().Process (context);
		new Xamarin.Linker.Steps.OptimizeGeneratedCodeStep ().Process (context);
		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Microsoft.iOS");
		var methods = assembly.MainModule.GetType (typeName).Methods.Where (v => v.Name == methodName).ToArray ();
		Assert.That (methods, Is.Not.Empty, "Block constructors");
		foreach (var method in methods) {
			var signatures = method.Body.Instructions.Where (v => v.OpCode == OpCodes.Ldstr).Select (v => (string) v.Operand);
			Assert.That (signatures, Does.Contain (expectedSignature), method.FullName);
		}
	}

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

	[TestCase ("link", true)]
	[TestCase ("copy", false)]
	public void SetBeforeFieldInitForTrimmedProtocolInterfaces (string trimMode, bool expectedBeforeFieldInit)
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

		AssertPrepareCode (ApplePlatform.MacOSX, true, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.RegisterProtocols = true;
		}, code, out var outputPath, testAssemblyTrimMode: trimMode);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "IMyProtocol");
		Assert.That (type.IsBeforeFieldInit, Is.EqualTo (expectedBeforeFieldInit), "BeforeFieldInit");
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
		// the optimizer must leave it untouched: the IsDirectBinding condition must not be inlined
		// and the dead 'return 2' must not be eliminated.
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Export (""myMethod"")]
			public int MyMethod () {
				if (!IsDirectBinding) {
					return 1;
				}
				return 2;
			}
		}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.DeadCodeElimination = true;
			preparer.Optimizations.InlineIsDirectBinding = true;
		}, code, out var outputPath, extraCsproj: "<PropertyGroup><Optimize>true</Optimize></PropertyGroup>");

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
		// MyClass is a user type, so IsDirectBinding is inlined to a constant (false), after which the
		// unreachable 'return 2' branch is eliminated (the method is optimizable via [BindingImpl]).
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[BindingImpl (BindingImplOptions.Optimizable)]
			[Export (""myMethod"")]
			public int MyMethod () {
				if (!IsDirectBinding) {
					return 1;
				}
				return 2;
			}
		}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.DeadCodeElimination = true;
			preparer.Optimizations.InlineIsDirectBinding = true;
		}, code, out var outputPath, extraCsproj: "<PropertyGroup><Optimize>true</Optimize></PropertyGroup>");

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "MyMethod");

		// After dead code elimination, there should be no ldc.i4.2 (the unreachable return 2)
		var hasDeadCode = method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Ldc_I4_2);
		Assert.That (hasDeadCode, Is.False, "Dead code (return 2) should be eliminated");
	}
}
