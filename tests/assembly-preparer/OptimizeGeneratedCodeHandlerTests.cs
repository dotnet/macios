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
	public void RemoveEnsureUIThread (ApplePlatform platform, bool isCoreCLR)
	{
		var ensureUIThreadCall = platform == ApplePlatform.MacOSX
			? "AppKit.NSApplication.EnsureUIThread ();"
			: "UIKit.UIApplication.EnsureUIThread ();";

		var usingDirective = platform == ApplePlatform.MacOSX
			? "using AppKit;"
			: "using UIKit;";

		var code = $@"
		using System;
		using Foundation;
		using ObjCRuntime;
		{usingDirective}

		class MyClass : NSObject {{
			[BindingImpl (BindingImplOptions.Optimizable)]
			[Export (""myMethod"")]
			public void MyMethod () {{
				{ensureUIThreadCall}
			}}
		}}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.RemoveUIThreadChecks = true;
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "MyMethod");

		var hasEnsureUIThread = method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Call &&
			(i.Operand as MethodReference)?.Name == "EnsureUIThread");
		Assert.That (hasEnsureUIThread, Is.False, "EnsureUIThread call should be removed");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void KeepEnsureUIThreadWhenOptimizationDisabled (ApplePlatform platform, bool isCoreCLR)
	{
		var ensureUIThreadCall = platform == ApplePlatform.MacOSX
			? "AppKit.NSApplication.EnsureUIThread ();"
			: "UIKit.UIApplication.EnsureUIThread ();";

		var usingDirective = platform == ApplePlatform.MacOSX
			? "using AppKit;"
			: "using UIKit;";

		var code = $@"
		using System;
		using Foundation;
		using ObjCRuntime;
		{usingDirective}

		class MyClass : NSObject {{
			[BindingImpl (BindingImplOptions.Optimizable)]
			[Export (""myMethod"")]
			public void MyMethod () {{
				{ensureUIThreadCall}
			}}
		}}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.RemoveUIThreadChecks = false;
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "MyMethod");

		var hasEnsureUIThread = method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Call &&
			(i.Operand as MethodReference)?.Name == "EnsureUIThread");
		Assert.That (hasEnsureUIThread, Is.True, "EnsureUIThread call should be preserved when optimization is disabled");
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
		var ensureUIThreadCall = platform == ApplePlatform.MacOSX
			? "AppKit.NSApplication.EnsureUIThread ();"
			: "UIKit.UIApplication.EnsureUIThread ();";

		var usingDirective = platform == ApplePlatform.MacOSX
			? "using AppKit;"
			: "using UIKit;";

		var code = $@"
		using System;
		using Foundation;
		using ObjCRuntime;
		{usingDirective}

		class MyClass : NSObject {{
			[Export (""myMethod"")]
			public void MyMethod () {{
				{ensureUIThreadCall}
			}}
		}}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.RemoveUIThreadChecks = true;
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "MyMethod");

		var hasEnsureUIThread = method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Call &&
			(i.Operand as MethodReference)?.Name == "EnsureUIThread");
		Assert.That (hasEnsureUIThread, Is.True, "EnsureUIThread call should be preserved without [BindingImpl(Optimizable)]");
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void DeadCodeElimination (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[BindingImpl (BindingImplOptions.Optimizable)]
			[Export (""myMethod"")]
			public int MyMethod () {
				if (true) {
					return 1;
				}
				return 2;
			}
		}";

		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.DeadCodeElimination = true;
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var method = type.Methods.Single (v => v.Name == "MyMethod");

		// After dead code elimination, there should be no ldc.i4.2 (the unreachable return 2)
		var hasDeadCode = method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Ldc_I4_2);
		Assert.That (hasDeadCode, Is.False, "Dead code (return 2) should be eliminated");
	}
}
