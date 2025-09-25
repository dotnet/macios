using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;
using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Cecil.Tests {
	[TestFixture]
	public partial class HandleSafetyTest {
		[Test]
		public void Check ()
		{
			Configuration.IgnoreIfAnyIgnoredPlatforms ();

			var failures = new Dictionary<string, string> ();

			foreach (var info in Helper.NetPlatformImplementationAssemblyDefinitions) {
				var assembly = info.Assembly;
				var methodsToCheck = Helper.EnumerateMethods (assembly!, Filter).ToArray ();

				Assert.That (methodsToCheck.Length, Is.GreaterThan (0), "There must be something to check!");

				foreach (var m in methodsToCheck) {
					var state = new MethodState (m);
					var methodFullName = AttributeTest.GetMemberLookupKeyInternal (state.Method);
					try {
						state.Process ();
						if (state.Entries.Any (v => v.StackBeforeInstruction is null)) {
							failures [methodFullName] = $"Error while processing {m}: found instructions without stack state information after processing completed";
							continue;
						}

						var rv = CheckMethod (state, out var failure);
						if (!rv) {
							var sb = new StringBuilder ();
							sb.AppendLine ($"{state.Method.DeclaringType.FullName}.{state.Method.Name}: {failure!.Message}");
							sb.AppendLine ($"    Call instruction: {failure.CallInstruction}");
							sb.AppendLine ($"        {GetLocation (m, failure.CallInstruction!)}");
							sb.AppendLine ($"    Load instruction: {failure.LoadInstruction}");
							sb.AppendLine ($"        {GetLocation (m, failure.LoadInstruction!)}");
							var msg = sb.ToString ();
							failures [methodFullName] = msg;
						}
					} catch (Exception e) {
						failures [methodFullName] = $"Failed to process {m}: {e}";
					}
				}
			}

			Helper.AssertFailures (failures, knownFailuresHandleSafety, nameof (knownFailuresHandleSafety), "Members with unsafe handle usage");
		}

		static bool Filter (MethodDefinition method)
		{
			if (!method.HasBody)
				return false;

			if (!method.Body.Instructions.Any (v => IsUnsafeMethodCall (v.Operand as MethodReference)))
				return false;

			return true;
		}

		static void DumpState (MethodState state, Exception? e = null)
		{
			var m = state.Method;
			Console.WriteLine ($"Failed to compute stack state for {m}: {e}");
			Console.WriteLine (DumpIL (m));
			Console.WriteLine ($"Current state:");
			for (var i = 0; i < state.Entries.Length; i++) {
				var instr = m.Body.Instructions [i];
				var entry = state.Entries [i];
				var size = entry.StackBeforeInstruction?.Count.ToString () ?? "N/A";
				var sources = entry.StackBeforeInstruction is null ? "N/A" : string.Join (", ", entry.StackBeforeInstruction.Select (v => v.Source.ToString ()));
				Console.WriteLine (instr.ToString ());
				Console.WriteLine ($"    Stack size before instruction: {size}");
				if (entry.StackBeforeInstruction is not null) {
					for (var s = 0; s < entry.StackBeforeInstruction.Count; s++) {
						var source = entry.StackBeforeInstruction [s];
						Console.WriteLine ($"        #{s}: Kind: {source.Source}");
						if (source.LoadInstructions?.Length > 0) {
							Console.WriteLine ($"              Load Instructions:");
							foreach (var li in source.LoadInstructions!)
								Console.WriteLine ($"                  {li}");
						} else {
							Console.WriteLine ($"              Load Instructions: {source.LoadInstructions?.FirstOrDefault ()}");
						}
					}
				}
			}
		}

		static string GetLocation (MethodDefinition method, Instruction instr)
		{
			if (method is null)
				return "<no location> ";

			if (method.DebugInformation.HasSequencePoints) {
				var seq = method.DebugInformation.GetSequencePoint (instr);
				if (seq is null)
					seq = method.DebugInformation.SequencePoints.Where (v => v.Offset < instr.Offset).LastOrDefault ();
				if (seq is null)
					seq = method.DebugInformation.SequencePoints [0];
				return seq.Document.Url + ":" + seq.StartLine + " -> " + seq.EndLine + " ";
			}
			return string.Empty;
		}

		// Decide whether a method call needs to be inspected. We're looking for methods that return the handle
		// of one of the arguments, in which case the caller must make sure the object used to get the handle
		// is not collected by the GC before the use of the handle itself has completed.
		// For a given method, we say it's unsafe if any kind of native cleanup is done in the Dispose (or destructor)
		// implementation for the class where the method is defined.
		static bool IsUnsafeMethodCall (MethodReference? target)
		{
			if (target is null)
				return false;

			switch (target.DeclaringType.FullName) {
			case "CoreText.CTFrameAttributesExtensions":
			case "CoreText.CTFontCollectionOptionsExtensions":
			case "CoreText.CTTextTabOptionsExtensions":
			case "Foundation.DictionaryContainerHelper":
			case "CoreText.CTTypesetterOptionsExtensions":
			case "Foundation.NSMetadataItem":
			case "CoreImage.CIFilter":
				switch (target.Name) {
				case "GetHandle":
					return true;
				}
				break;
			case "AudioToolbox.AudioBuffers":
			case "AudioToolbox.AudioQueueProcessingTap":
			case "CoreFoundation.CFDataBuffer":
			case "CoreMidi.MidiObject":
			case "ObjCRuntime.DisposableObject":
			case "ObjCRuntime.INativeObject":
			case "ObjCRuntime.NativeHandle":
			case "Foundation.NSObject":
			case "Foundation.NSZone":
			case "CoreMidi.MidiThruConnection":
				switch (target.Name) {
				case "get_Handle":
					return true;
				}
				break;
			case "ObjCRuntime.NativeObjectExtensions":
				switch (target.Name) {
				case "GetHandle":
				case "GetNonNullHandle":
				case "GetCheckedHandle":
					return true;
				}
				break;
			// The following GetHandle / get_Handle methods don't have any safety problems.
			case "ObjCRuntime.Class":
			case "ObjCRuntime.Selector":
			case "ObjCRuntime.Protocol":
				switch (target.Name) {
				case "get_Handle":
				case "GetHandle":
					return false;
				}
				break;
			case "AudioUnit.AURenderEventEnumerator": // just a wrapper around a pointer, doesn't free anything in its destructor
			case "CoreGraphics.CGPDFObject": // just a wrapper around a pointer, doesn't free anything in its destructor
			case "CoreText.CTRunDelegateOperations": // The Handle property is a GCHandle (converted to IntPtr)
			case "CoreGraphics.CGEvent/TapData": // The Handle property is a GCHandle (converted to IntPtr)
				switch (target.Name) {
				case "get_Handle":
					return false;
				}
				break;
			}

			// Check if there are any other GetHandle calls we should process.
			switch (target.Name) {
			case "GetHandle":
			case "get_Handle":
				throw new NotImplementedException ($"Must decide whether the call to {target} is safe or not.");
			}

			return false;
		}

		[Flags]
		enum StackSource {
			None = 0,
			Argument = 1,
			Local = 2,
			Call = 4,
			Constant = 8,
			BuiltIn = 16, // catch handlers
			Field = 32,
			Other = 64,
		}

		class StackEntry {
			public StackSource Source;
			public Instruction []? LoadInstructions;
		}

		class StackState {
			public Instruction Instruction;
			public List<StackEntry>? StackBeforeInstruction;
			public List<Instruction> CallingInstructions = new List<Instruction> ();

			public StackState (Instruction instr)
			{
				Instruction = instr;
			}
		}

		class MethodState {
			public MethodDefinition Method;
			public StackState [] Entries;

			public MethodState (MethodDefinition md)
			{
				Method = md;
				Entries = new StackState [md.Body.Instructions.Count];
				for (var i = 0; i < Entries.Length; i++)
					Entries [i] = new StackState (md.Body.Instructions [i]);

			}

			public void Process ()
			{
				ComputeMap (Method.Body.Instructions [0]);
				if (!Method.Body.HasExceptionHandlers)
					return;

				foreach (var eh in Method.Body.ExceptionHandlers) {
					List<StackEntry>? startStack = null;
					switch (eh.HandlerType) {
					case ExceptionHandlerType.Catch:
						startStack = new List<StackEntry> ();
						startStack.Add (new StackEntry { Source = StackSource.BuiltIn });
						break;
					case ExceptionHandlerType.Finally:
						break;
					case ExceptionHandlerType.Fault:
					case ExceptionHandlerType.Filter:
					default:
						throw new NotImplementedException (eh.HandlerType.ToString ());
					}
					if (eh.HandlerStart is not null)
						ComputeMap (eh.HandlerStart, startStack);
					if (eh.FilterStart is not null)
						ComputeMap (eh.FilterStart, startStack);
				}
			}

			void ComputeMap (Instruction startInstruction, List<StackEntry>? initialStack = null)
			{
				var iterations = 0;
				var queue = new Queue<(Instruction, Instruction?, List<StackEntry>?)> ();
				var processedInstructions = new HashSet<Instruction> ();
				queue.Enqueue ((startInstruction, null, initialStack));

				do {
					var entry = queue.Dequeue ();
					var instruction = entry.Item1;
					var previousInstruction = entry.Item2;
					var postStack = entry.Item3;

					if (processedInstructions.Contains (instruction))
						continue;
					processedInstructions.Add (instruction);

					ComputeMap (instruction, postStack, out var nextInstructions, out var postInstructionStack);
					if (nextInstructions is not null) {
						foreach (var n in nextInstructions) {
							queue.Enqueue ((n, instruction, postInstructionStack));
						}
					}
					iterations++;
					if (iterations > 10000)
						throw new NotImplementedException ("Too many iterations");
				} while (queue.Count > 0);
			}

			void ComputeMap (Instruction instruction, List<StackEntry>? callingStack, out IEnumerable<Instruction>? nextInstructions, out List<StackEntry> postInstructionStack)
			{
				var instructions = Method.Body.Instructions;
				var index = instructions.IndexOf (instruction);
				var entry = Entries [index];

				if (entry.StackBeforeInstruction is null) {
					// First time processing this method.
					entry.StackBeforeInstruction = callingStack is null ? new List<StackEntry> () : new List<StackEntry> (callingStack);
				} else {
					// Reached this instruction from multiple places
					if (callingStack?.Count > 0) {
						if (callingStack.Count != entry.StackBeforeInstruction.Count)
							throw new InvalidOperationException ($"Stack difference between branch locations");
						for (var i = 0; i < callingStack.Count; i++) {
							entry.StackBeforeInstruction [i].Source |= callingStack [i].Source;
							var mergedLoadInstructions = new List<Instruction> ();
							mergedLoadInstructions.AddRange (callingStack [i].LoadInstructions!);
							mergedLoadInstructions.AddRange (entry.StackBeforeInstruction [i].LoadInstructions!);
							entry.StackBeforeInstruction [i].LoadInstructions = mergedLoadInstructions.ToArray ();
						}
					} else {
						if (entry.StackBeforeInstruction.Count != 0)
							throw new InvalidOperationException ($"Stack difference between branch locations");
					}
				}

				StackSource src;
				int popCount = 0;
				int pushCount = 0;
				nextInstructions = null;
				Instruction? loadInstruction = null;
				Instruction []? loadInstructions = null;
				switch (instruction.OpCode.Code) {
				case Code.Ldarg:
				case Code.Ldarga:
				case Code.Ldarga_S:
				case Code.Ldarg_0:
				case Code.Ldarg_1:
				case Code.Ldarg_2:
				case Code.Ldarg_3:
				case Code.Ldarg_S:
					pushCount = 1;
					src = StackSource.Argument;
					loadInstruction = instruction;
					break;
				case Code.Ldfld:
				case Code.Ldflda:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Field;
					loadInstruction = instruction;
					break;
				case Code.Ldsfld:
				case Code.Ldsflda:
					pushCount = 1;
					src = StackSource.Field;
					loadInstruction = instruction;
					break;
				case Code.Ldloc:
				case Code.Ldloca:
				case Code.Ldloca_S:
				case Code.Ldloc_0:
				case Code.Ldloc_1:
				case Code.Ldloc_2:
				case Code.Ldloc_3:
				case Code.Ldloc_S:
					pushCount = 1;
					src = StackSource.Local;
					loadInstruction = instruction;
					break;
				case Code.Call:
				case Code.Calli:
				case Code.Callvirt:
					if (instruction.Operand is MethodReference calledMethod) {
						popCount += calledMethod.Parameters.Count;
						if (calledMethod.HasThis)
							popCount++;
						if (!calledMethod.ReturnType.Is ("System", "Void")) {
							pushCount++;
							loadInstruction = instruction;
						}
						src = StackSource.Call;
					} else {
						throw new NotImplementedException ($"Unable to understand operand of type {instruction.Operand!.GetType ()} => {instruction.Operand}");
					}
					break;
				case Code.Newobj:
					popCount = ((MethodReference) instruction.Operand).Parameters.Count;
					pushCount = 1;
					src = StackSource.Call;
					loadInstruction = instruction;
					break;
				case Code.Box: {
					var lastEntryOnStack = entry.StackBeforeInstruction.Last ();
					popCount = 1;
					pushCount = 1;
					src = lastEntryOnStack.Source;
					loadInstructions = lastEntryOnStack.LoadInstructions;
					break;
				}
				case Code.Rethrow:
					src = StackSource.None;
					nextInstructions = Array.Empty<Instruction> ();
					break;
				case Code.Throw:
					src = StackSource.None;
					popCount = 1;
					nextInstructions = Array.Empty<Instruction> ();
					break;
				case Code.Br:
				case Code.Br_S:
					src = StackSource.None;
					nextInstructions = new Instruction [] { (Instruction) instruction.Operand };
					break;
				case Code.Brfalse:
				case Code.Brfalse_S:
				case Code.Brtrue:
				case Code.Brtrue_S:
					src = StackSource.None;
					popCount = 1;
					nextInstructions = new Instruction [] { instruction.Next, (Instruction) instruction.Operand };
					break;
				case Code.Beq:
				case Code.Beq_S:
				case Code.Bge:
				case Code.Bge_S:
				case Code.Bge_Un:
				case Code.Bge_Un_S:
				case Code.Bgt:
				case Code.Bgt_S:
				case Code.Bgt_Un:
				case Code.Bgt_Un_S:
				case Code.Ble:
				case Code.Ble_S:
				case Code.Ble_Un:
				case Code.Ble_Un_S:
				case Code.Blt:
				case Code.Blt_S:
				case Code.Blt_Un:
				case Code.Blt_Un_S:
				case Code.Bne_Un:
				case Code.Bne_Un_S:
					src = StackSource.None;
					popCount = 2;
					nextInstructions = new Instruction [] { instruction.Next, (Instruction) instruction.Operand };
					break;
				case Code.Ret:
					src = StackSource.None;
					popCount = Method.ReturnType.Is ("System", "Void") ? 0 : 1;
					nextInstructions = Array.Empty<Instruction> ();
					break;
				case Code.Ldc_I4_0:
				case Code.Ldc_I4_1:
				case Code.Ldc_I4_2:
				case Code.Ldc_I4_3:
				case Code.Ldc_I4_4:
				case Code.Ldc_I4_5:
				case Code.Ldc_I4_6:
				case Code.Ldc_I4_7:
				case Code.Ldc_I4_8:
				case Code.Ldc_I4_M1:
				case Code.Ldc_I4:
				case Code.Ldc_I4_S:
				case Code.Ldc_I8:
				case Code.Ldc_R4:
				case Code.Ldc_R8:
					pushCount = 1;
					src = StackSource.Constant;
					loadInstruction = instruction;
					break;
				case Code.Ldelema:
				case Code.Ldelem_Any:
				case Code.Ldelem_I:
				case Code.Ldelem_I1:
				case Code.Ldelem_I2:
				case Code.Ldelem_I4:
				case Code.Ldelem_I8:
				case Code.Ldelem_R4:
				case Code.Ldelem_R8:
				case Code.Ldelem_Ref:
				case Code.Ldelem_U1:
				case Code.Ldelem_U2:
				case Code.Ldelem_U4: {
					var lastEntryOnStack = entry.StackBeforeInstruction [entry.StackBeforeInstruction.Count - 2];
					src = lastEntryOnStack.Source;
					popCount = 2;
					pushCount = 1;
					loadInstructions = lastEntryOnStack.LoadInstructions;
					break;
				}
				case Code.Newarr:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Stelem_Any:
				case Code.Stelem_I:
				case Code.Stelem_I2:
				case Code.Stelem_I1:
				case Code.Stelem_I4:
				case Code.Stelem_I8:
				case Code.Stelem_R4:
				case Code.Stelem_R8:
				case Code.Stelem_Ref:
					popCount = 3;
					src = StackSource.None;
					break;
				case Code.Stloc:
					popCount = 2;
					src = StackSource.None;
					break;
				case Code.Stloc_0:
				case Code.Stloc_1:
				case Code.Stloc_2:
				case Code.Stloc_3:
				case Code.Stloc_S:
					popCount = 1;
					src = StackSource.None;
					break;
				case Code.Stfld:
					popCount = 2;
					src = StackSource.None;
					break;
				case Code.Stsfld:
					popCount = 1;
					src = StackSource.None;
					break;
				case Code.Stind_I:
				case Code.Stind_I1:
				case Code.Stind_I2:
				case Code.Stind_I4:
				case Code.Stind_I8:
				case Code.Stind_R4:
				case Code.Stind_R8:
				case Code.Stind_Ref:
					popCount = 2;
					src = StackSource.Other;
					break;
				case Code.Dup: {
					var lastEntryOnStack = entry.StackBeforeInstruction.Last ();
					src = lastEntryOnStack.Source;
					pushCount = 1;
					loadInstructions = lastEntryOnStack.LoadInstructions;
					break;
				}
				case Code.Castclass:
					src = StackSource.Other; // or should this be the same as Dup?
					popCount = 1;
					pushCount = 1;
					loadInstructions = entry.StackBeforeInstruction.Last ().LoadInstructions;
					break;
				case Code.Ldstr:
					src = StackSource.Other;
					pushCount = 1;
					loadInstruction = instruction;
					break;
				case Code.Isinst:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other; // boolean?
					loadInstruction = instruction;
					break;
				case Code.Pop:
					popCount = 1;
					src = StackSource.None;
					break;
				case Code.Ldnull:
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Initobj:
					popCount = 1;
					src = StackSource.Other;
					break;
				case Code.Conv_I:
				case Code.Conv_I1:
				case Code.Conv_I2:
				case Code.Conv_I4:
				case Code.Conv_I8:
				case Code.Conv_Ovf_I:
				case Code.Conv_Ovf_I1:
				case Code.Conv_Ovf_I1_Un:
				case Code.Conv_Ovf_I2:
				case Code.Conv_Ovf_I2_Un:
				case Code.Conv_Ovf_I4:
				case Code.Conv_Ovf_I4_Un:
				case Code.Conv_Ovf_I8:
				case Code.Conv_Ovf_I8_Un:
				case Code.Conv_Ovf_I_Un:
				case Code.Conv_Ovf_U:
				case Code.Conv_Ovf_U1:
				case Code.Conv_Ovf_U1_Un:
				case Code.Conv_Ovf_U2:
				case Code.Conv_Ovf_U2_Un:
				case Code.Conv_Ovf_U4:
				case Code.Conv_Ovf_U4_Un:
				case Code.Conv_Ovf_U8:
				case Code.Conv_Ovf_U8_Un:
				case Code.Conv_Ovf_U_Un:
				case Code.Conv_R4:
				case Code.Conv_R8:
				case Code.Conv_R_Un:
				case Code.Conv_U:
				case Code.Conv_U1:
				case Code.Conv_U2:
				case Code.Conv_U4:
				case Code.Conv_U8:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Ldftn:
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Switch:
					popCount = 1;
					var next = new List<Instruction> ((Instruction []) (instruction.Operand));
					next.Add (instruction.Next);
					nextInstructions = next;
					src = StackSource.Other;
					break;
				case Code.Ldtoken:
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Ldind_I:
				case Code.Ldind_I1:
				case Code.Ldind_I2:
				case Code.Ldind_I4:
				case Code.Ldind_I8:
				case Code.Ldind_R4:
				case Code.Ldind_R8:
				case Code.Ldind_U1:
				case Code.Ldind_U2:
				case Code.Ldind_U4:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Ldind_Ref: {
					var lastEntryOnStack = entry.StackBeforeInstruction.Last ();
					src = lastEntryOnStack.Source;
					popCount = 1;
					pushCount = 1;
					loadInstructions = lastEntryOnStack.LoadInstructions;
					break;
				}
				case Code.Ldlen:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Add:
				case Code.Add_Ovf:
				case Code.Add_Ovf_Un:
				case Code.Sub:
				case Code.Sub_Ovf:
				case Code.Sub_Ovf_Un:
				case Code.Mul:
				case Code.Mul_Ovf:
				case Code.Mul_Ovf_Un:
				case Code.Div:
				case Code.Div_Un:
					popCount = 2;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Sizeof:
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Ldobj:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Unbox:
				case Code.Unbox_Any:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Leave:
				case Code.Leave_S:
					src = StackSource.None;
					nextInstructions = new Instruction [] { (Instruction) instruction.Operand };
					break;
				case Code.Cgt:
				case Code.Cgt_Un:
				case Code.Ceq:
				case Code.Clt:
				case Code.Clt_Un:
				case Code.And:
				case Code.Or:
				case Code.Xor:
				case Code.Shr:
				case Code.Shr_Un:
				case Code.Shl:
				case Code.Rem:
				case Code.Rem_Un:
					popCount = 2;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Neg:
				case Code.Not:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Constrained:
					src = StackSource.None;
					break;
				case Code.Stobj:
					popCount = 2;
					src = StackSource.None;
					break;
				case Code.Starg:
				case Code.Starg_S:
					popCount = 1;
					src = StackSource.None;
					break;
				case Code.Nop:
					src = StackSource.None;
					break;
				case Code.Localloc:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				// case Code.Endfilter:
				case Code.Endfinally:
					src = StackSource.None;
					nextInstructions = Array.Empty<Instruction> ();
					break;
				case Code.Ldvirtftn:
					popCount = 1;
					pushCount = 1;
					src = StackSource.Other;
					loadInstruction = instruction;
					break;
				case Code.Volatile:
				case Code.Readonly:
					src = StackSource.None;
					break;
				default:
					throw new NotImplementedException ($"Don't know how to handle this instruction when computing stack behavior: {instruction}");
				}

				if (loadInstruction is not null && loadInstructions is not null)
					throw new InvalidOperationException ($"Can't set both 'loadInstruction' and 'loadInstructions'.");
				if (loadInstruction is not null)
					loadInstructions = new Instruction [] { loadInstruction };

				var hasLoadInstructions = loadInstructions is not null;

				if (callingStack is not null) {
					postInstructionStack = new List<StackEntry> (callingStack);
				} else {
					postInstructionStack = new List<StackEntry> ();
				}

				if (popCount > 0) {
					if (popCount > postInstructionStack.Count)
						throw new Exception ($"Stack too small (popping {popCount}, but stack contains {postInstructionStack.Count} entries) at: {instruction}");
					postInstructionStack.RemoveRange (postInstructionStack.Count - popCount, popCount);
				}
				if (pushCount > 0) {
					if (pushCount > 1)
						throw new NotImplementedException ($"Invalid push count: {pushCount}");
					postInstructionStack.Add (new StackEntry () { Source = src, LoadInstructions = loadInstructions });
					if (!hasLoadInstructions)
						throw new InvalidOperationException ($"Null load instruction but pushed something to the stack: {instruction}");
				} else {
					if (hasLoadInstructions)
						throw new InvalidOperationException ($"Non-null load instruction but not pushing anything onto the stack: {instruction}");
				}

				if (nextInstructions?.Count () == 0 && postInstructionStack.Count != 0) {
					switch (instruction.OpCode.Code) {
					case Code.Throw:
						break; // leftover entries on the stack seems to be OK when throwing exceptions.
					default:
						throw new InvalidOperationException ($"Reached end of block but stack isn't empty? {instruction}");
					}
				}

				if (nextInstructions is null)
					nextInstructions = new Instruction [] { instruction.Next };
			}
		}

		IEnumerable<Instruction> GetLoadInstructionForParameterAtCall (MethodState state, Instruction call, int parameter /* 1-based */)
		{
			// only handle methods with one argument for now
			var md = state.Method;
			var entry = state.Entries [md.Body.Instructions.IndexOf (call)];
			var target = ((MethodReference) call.Operand);
			var targetArguments = target.Parameters.Count + (target.HasThis ? 1 : 0);
			var desiredParameterIndex = targetArguments - parameter;
			var preStack = entry.StackBeforeInstruction;

			var idx = entry.StackBeforeInstruction!.Count - desiredParameterIndex - 1;
			if (idx < 0 || idx >= entry.StackBeforeInstruction.Count)
				throw new InvalidOperationException ($"Trying to get parameter #{parameter} from the stack for a method call with {md.Parameters.Count} parameters, but there are only {entry.StackBeforeInstruction.Count} entries on the stack.");
			var stackEntry = entry.StackBeforeInstruction [entry.StackBeforeInstruction.Count - desiredParameterIndex - 1];

			if (stackEntry?.LoadInstructions is null)
				throw new InvalidOperationException ($"No load instructions for parameter {parameter} in call to {call}");

			return stackEntry.LoadInstructions;
		}

		static string DumpIL (MethodDefinition md)
		{
			var sb = new StringBuilder ();
			sb.AppendLine (md.FullName);
			sb.AppendLine (GetLocation (md, md.Body.Instructions.First ()));
			foreach (var instr in md.Body.Instructions)
				sb.AppendLine (instr.ToString ());
			if (md.Body.HasExceptionHandlers) {
				foreach (var eh in md.Body.ExceptionHandlers) {
					sb.AppendLine ($"Exception handler: {eh.HandlerType} Catch type: {eh.CatchType}");
					if (eh.TryStart is not null) {
						sb.AppendLine ($"    TryStart: {eh.TryStart}");
						sb.AppendLine ($"    TryEnd:   {eh.TryEnd}");
					}
					if (eh.HandlerStart is not null) {
						sb.AppendLine ($"    HandlerStart: {eh.HandlerStart}");
						sb.AppendLine ($"    HandlerEnd:   {eh.HandlerEnd}");

					}
					if (eh.FilterStart is not null) {
						sb.AppendLine ($"    FilterStart: {eh.FilterStart}");
					}
				}
			}
			return sb.ToString ();
		}

		bool IsReferencedLater (MethodDefinition md, Instruction value, Instruction after)
		{
			var originalAfter = after;

			while (after.Next is not null) {
				after = after.Next;
				if (value.OpCode == after.OpCode && value.Operand == after.Operand) {
					return true;
				}
			}
			return false;
		}

		class Failure {
			public Instruction? LoadInstruction;
			public Instruction? CallInstruction;
			public string? Message;
		}

		bool CheckMethod (MethodState state, out Failure? failure)
		{
			failure = null;

			var md = state.Method;
			foreach (var instr in md.Body.Instructions) {
				switch (instr.OpCode.Code) {
				case Code.Call:
				case Code.Calli:
				case Code.Callvirt:
					if (!IsUnsafeMethodCall (instr.Operand as MethodReference))
						continue;

					var values = GetLoadInstructionForParameterAtCall (state, instr, 1).ToList ();

					foreach (var value in values) {
						switch (value.OpCode.Code) {
						case Code.Call:
						case Code.Calli:
						case Code.Callvirt:
							failure = new Failure {
								Message = $"Fetched the handle of an object, but a reference to the object was never stored anywhere. This means the object could be collected by the GC while the handle is in use.",
								CallInstruction = instr,
								LoadInstruction = value,
							};
							return false;
						}

						// Assume that calling [get_]Handle from within an instance method (on the self instance) is safe.
						if (value.OpCode.Code == Code.Ldarg_0 && state.Method.HasThis)
							continue;

						if (!IsReferencedLater (md, value, instr)) {
							failure = new Failure {
								Message = $"Fetched the handle of an object, but the object was never referenced again in this method. This means the object could be collected by the GC while the handle is in use.",
								CallInstruction = instr,
								LoadInstruction = value,
							};
							return false;
						}
					}

					break;
				}
			}

			return true;
		}
	}
}

