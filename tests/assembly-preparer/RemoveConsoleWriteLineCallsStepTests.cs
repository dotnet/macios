// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Linq;

using Mono.Cecil.Cil;

namespace AssemblyPreparerTests;

public class RemoveConsoleWriteLineCallsStepTests : BaseClass {
	const string code = @"
	using System;
	using Foundation;

	class MyClass : NSObject {
		public static int Counter;

		// A Console.WriteLine with a constant string argument - the whole call should be removed.
		public void WithConstantArgument ()
		{
			Console.WriteLine (""a constant message"");
		}

		// A parameterless Console.WriteLine - the call should be removed.
		public void WithoutArguments ()
		{
			Console.WriteLine ();
		}

		// A Console.WriteLine whose argument has a side effect (a method call): the side effect
		// (the call to SideEffect) must be preserved, and only the write to the console removed.
		public void WithSideEffectingArgument ()
		{
			Console.WriteLine (SideEffect ());
		}

		// A multi-argument Console.WriteLine overload: every argument must be popped so the
		// evaluation stack stays balanced after the call is removed.
		public void WithMultipleArguments ()
		{
			Console.WriteLine (""{0} {1}"", 1, 2);
		}

		static string SideEffect ()
		{
			Counter++;
			return ""value"";
		}
	}";

	static bool HasConsoleWriteLineCall (MethodDefinition method)
	{
		return method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Call &&
			i.Operand is MethodReference mr &&
			mr.Name == "WriteLine" &&
			mr.DeclaringType.FullName == "System.Console");
	}

	static bool HasCall (MethodDefinition method, string name)
	{
		return method.Body.Instructions.Any (i =>
			i.OpCode.Code == Code.Call &&
			i.Operand is MethodReference mr &&
			mr.Name == name);
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void RemovesConsoleWriteLineCalls (ApplePlatform platform, bool isCoreCLR)
	{
		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			preparer.Optimizations.RemoveConsoleWriteLineCalls = true;
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");

		Assert.Multiple (() => {
			Assert.That (HasConsoleWriteLineCall (type.Methods.Single (v => v.Name == "WithConstantArgument")), Is.False, "WithConstantArgument");
			Assert.That (HasConsoleWriteLineCall (type.Methods.Single (v => v.Name == "WithoutArguments")), Is.False, "WithoutArguments");

			// The Console.WriteLine call is removed, but the side effect of evaluating its argument is preserved.
			var sideEffecting = type.Methods.Single (v => v.Name == "WithSideEffectingArgument");
			Assert.That (HasConsoleWriteLineCall (sideEffecting), Is.False, "WithSideEffectingArgument: WriteLine removed");
			Assert.That (HasCall (sideEffecting, "SideEffect"), Is.True, "WithSideEffectingArgument: side effect preserved");
			Assert.That (sideEffecting.Body.Instructions.Any (i => i.OpCode.Code == Code.Pop), Is.True, "WithSideEffectingArgument: argument popped");

			// A multi-argument overload must get one 'pop' per argument (here: format string + 2 boxed ints).
			var multiArg = type.Methods.Single (v => v.Name == "WithMultipleArguments");
			Assert.That (HasConsoleWriteLineCall (multiArg), Is.False, "WithMultipleArguments: WriteLine removed");
			Assert.That (multiArg.Body.Instructions.Count (i => i.OpCode.Code == Code.Pop), Is.EqualTo (3), "WithMultipleArguments: one pop per argument");
		});
	}

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void KeepsConsoleWriteLineCallsWhenOptimizationDisabled (ApplePlatform platform, bool isCoreCLR)
	{
		AssertPrepareCode (platform, isCoreCLR, preparer => {
			preparer.Registrar = RegistrarMode.Dynamic;
			// The optimization is opt-in, so leave it disabled here.
		}, code, out var outputPath);

		using var assemblyDefinition = AssemblyDefinition.ReadAssembly (outputPath);
		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");

		Assert.Multiple (() => {
			Assert.That (HasConsoleWriteLineCall (type.Methods.Single (v => v.Name == "WithConstantArgument")), Is.True, "WithConstantArgument");
			Assert.That (HasConsoleWriteLineCall (type.Methods.Single (v => v.Name == "WithoutArguments")), Is.True, "WithoutArguments");
			Assert.That (HasConsoleWriteLineCall (type.Methods.Single (v => v.Name == "WithSideEffectingArgument")), Is.True, "WithSideEffectingArgument");
			Assert.That (HasConsoleWriteLineCall (type.Methods.Single (v => v.Name == "WithMultipleArguments")), Is.True, "WithMultipleArguments");
		});
	}
}
