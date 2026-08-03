// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Cecil.Cil;

using Mono.Linker;
using Mono.Tuner;

#nullable enable

namespace Xamarin.Linker.Steps {
	// Removes calls to System.Console.WriteLine from application code, in order to reduce the size of the
	// shipped app (see https://github.com/dotnet/macios/issues/16781): these calls (and any strings/formatting
	// logic feeding them) are typically dead weight once an app has shipped, since there's usually nobody
	// around to read the standard output of a mobile app.
	//
	// This step intentionally only targets System.Console.WriteLine (and not the other Console.Write* siblings,
	// such as Console.Write, Console.Out.Write, etc.), to keep the initial implementation's scope and risk small.
	// It can be broadened to cover more of the Console.Write* family later if that turns out to be useful.
	//
	// This optimization is opt-in (it must be explicitly enabled with '--optimize=+remove-console-writeline'),
	// because - unlike most of the other optimizations in this assembly - it changes the observable behavior of
	// the app (any console output produced by these calls is removed).
	public class RemoveConsoleWriteLineCallsStep : AssemblyModifierStep {
		protected override string Name { get; } = "Remove Console.WriteLine calls";
		protected override int ErrorCode { get; } = 2540;

		protected override bool IsActiveFor (AssemblyDefinition assembly)
		{
			// Only do any work if the optimization has been explicitly enabled.
			if (App.Optimizations.RemoveConsoleWriteLineCalls != true)
				return false;

			// We only care about assemblies that are actually being trimmed/linked - if an assembly isn't
			// linked, its unused members (including calls to Console.WriteLine) can't be swept away anyway,
			// and there's no point in modifying its IL.
			return DerivedLinkContext.Annotations.GetAction (assembly) == AssemblyAction.Link;
		}

		protected override bool ProcessType (TypeDefinition type)
		{
			return ProcessMethods (type);
		}

		protected override bool ProcessMethod (MethodDefinition method)
		{
			if (!method.HasBody)
				return false;

			var modified = false;
			var instructions = method.Body.Instructions;
			for (var i = 0; i < instructions.Count; i++) {
				var ins = instructions [i];

				// Console.WriteLine is a static method, so any call to it is a 'call' instruction (never 'callvirt').
				if (ins.OpCode.Code != Code.Call)
					continue;

				if (ins.Operand is not MethodReference mr)
					continue;

				if (mr.Name != "WriteLine")
					continue;

				if (!mr.DeclaringType.Is ("System", "Console"))
					continue;

				i += RemoveCall (method, ins, mr);
				modified = true;
			}

			return modified;
		}

		// Removes a call to Console.WriteLine, while preserving any side effects from evaluating its arguments.
		// Returns the number of instructions inserted into the method body, so the caller can adjust its
		// iteration index accordingly.
		static int RemoveCall (MethodDefinition method, Instruction callInstruction, MethodReference targetMethod)
		{
			var instructions = method.Body.Instructions;
			var index = instructions.IndexOf (callInstruction);

			// Console.WriteLine is a static method that doesn't return a value, so the only values left on the
			// stack right before the call executes are its arguments (if any). Instead of just nop-ing out the
			// call (which would unbalance the stack), insert a 'pop' instruction for each argument. This keeps
			// the stack balanced, and preserves any side effects from evaluating the arguments (e.g. a method
			// call used as an argument to Console.WriteLine) - only the actual write to the console is removed.
			var parameterCount = targetMethod.Parameters.Count;
			for (var p = 0; p < parameterCount; p++)
				instructions.Insert (index, Instruction.Create (OpCodes.Pop));

			// Nop out the call instruction itself (as opposed to removing it), so that any branches or exception
			// handlers that reference this exact instruction remain valid.
			callInstruction.OpCode = OpCodes.Nop;
			callInstruction.Operand = null;

			return parameterCount;
		}
	}
}
