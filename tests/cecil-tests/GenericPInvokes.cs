using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xamarin.Utils;

#nullable enable

namespace Cecil.Tests {
	[TestClass]
	public class GenericPInvokesTest {
		[TestMethod]
		[DynamicData (nameof (Helper.NetPlatformImplementationAssemblyTestData), typeof (Helper))]
		public void CheckSetupBlockUnsafeUsage (string assemblyPath)
		{
			var info = Helper.GetAssemblyInfo (assemblyPath);
			// We should not call BlockLiteral.SetupBlockUnsafe in our code at all.
			// All our code should use the function pointer syntax for block creation:
			//     var block = new BlockLiteral (&function, nameof (<type where function is defined), nameof (function))

			var assembly = info.Assembly;
			var callsToSetupBlock = AllSetupBlocks (assembly).Select (v => v.FullName).ToArray ();
			Assert.IsEmpty (callsToSetupBlock, $"No calls at all to BlockLiteral.SetupBlockUnsafe\n{string.Join ("\n", callsToSetupBlock)}");
		}

		[TestMethod]
		[DynamicData (nameof (Helper.NetPlatformImplementationAssemblyTestData), typeof (Helper))]
		public void CheckAllPInvokes (string assemblyPath)
		{
			var info = Helper.GetAssemblyInfo (assemblyPath);
			var assembly = info.Assembly;
			var pinvokes = AllPInvokes (assembly).Where (IsPInvokeOK);
			Assert.IsGreaterThan (0, pinvokes.Count ());

			var failures = pinvokes.Where (ContainsGenerics).ToList ();
			var failingMethods = ListOfFailingMethods (failures);

			Assert.AreEqual (0, failures.Count, $"There are {failures.Count} pinvoke methods that contain generics. This will not work in .NET 7 and above (see https://github.com/dotnet/macios/issues/11771 ):{failingMethods}");
		}

		string ListOfFailingMethods (IEnumerable<MethodDefinition> methods)
		{
			var list = new StringBuilder ();
			foreach (var method in methods) {
				list.Append ('\n').Append (method.FullName);
			}
			return list.ToString ();
		}

		static bool ContainsGenerics (MethodDefinition method)
		{
			return method.ContainsGenericParameter;
		}

		IEnumerable<MethodDefinition> AllPInvokes (AssemblyDefinition assembly)
		{
			return assembly.EnumerateMethods (method =>
				(method.Attributes & MethodAttributes.PInvokeImpl) != 0);
		}

		static bool IsPInvokeOK (MethodDefinition method)
		{
			var fullName = method.FullName;
			switch (fullName) {
			default:
				return true;
			}
		}

		IEnumerable<MethodDefinition> AllSetupBlocks (AssemblyDefinition assembly)
		{
			return assembly.EnumerateMethods (method => {
				if (!method.HasBody)
					return false;
				return method.Body.Instructions.Any (IsCallToSetupBlockUnsafe);
			});
		}

		static bool IsCallToSetupBlockUnsafe (Instruction instr)
		{
			if (!IsCall (instr))
				return false;

			var operand = instr.Operand;
			if (!(operand is MethodReference mr))
				return false;

			if (!mr.DeclaringType.Is ("ObjCRuntime", "BlockLiteral"))
				return false;

			if (mr.Name != "SetupBlockUnsafe")
				return false;

			if (!mr.ReturnType.Is ("System", "Void"))
				return false;

			if (!mr.HasParameters || mr.Parameters.Count != 2)
				return false;

			if (!mr.Parameters [0].ParameterType.Is ("System", "Delegate") || !mr.Parameters [1].ParameterType.Is ("System", "Delegate"))
				return false;

			return true;
		}

		static bool IsCall (Instruction instr)
		{
			return instr.OpCode == OpCodes.Call ||
				instr.OpCode == OpCodes.Calli;
		}
	}
}
