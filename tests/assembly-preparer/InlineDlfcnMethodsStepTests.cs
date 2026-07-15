// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace AssemblyPreparerTests;

public class InlineDlfcnMethodsStepTests : BaseClass {
	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void MarkedTest (ApplePlatform platform, bool isCoreCLR)
	{
		var code = @"
		using System;
		using CoreAnimation;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			void GetIntPtr ()
			{
				Console.WriteLine (Dlfcn.GetIntPtr (0, ""NativeSymbol""));
			}
		}";

		AssertPrepare (platform, isCoreCLR, code, out var assemblyDefinition);

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");
		var platformReference = assemblyDefinition.MainModule.AssemblyReferences.Single (v => v.Name == $"Microsoft.{platform.AsString ()}");
		var platformAssembly = assemblyDefinition.MainModule.AssemblyResolver.Resolve (platformReference);
		var dlfcn = platformAssembly.MainModule.Types.Single (v => v.Name == "Dlfcn");

		var cctor = type.GetStaticConstructor ();
		Assert.That (cctor, Is.Null, "No static constructor should be needed.");

		void AssertHasDlfcnPInvokeCall (MethodDefinition method)
		{
			var instructions = method.Body.Instructions;
			var call = instructions.FirstOrDefault (v => v.OpCode == OpCodes.Call && v.Operand is MethodReference mr && mr.DeclaringType.FullName == dlfcn.FullName);
			Assert.That (call, Is.Not.Null, $"Expected a call to Dlfcn in {method}");
			var resolvedMethod = ((MethodReference) call.Operand).Resolve ();
			Assert.That (resolvedMethod, Is.Not.Null, $"Expected the call to resolve to a method in Dlfcn for {method}");
			Assert.That (resolvedMethod.PInvokeInfo, Is.Null, $"Expected the method to not be a PInvoke method for {method}");
		}

		Assert.Multiple (() => {
			AssertHasDlfcnPInvokeCall (type.Methods.Single (v => v.Name == "GetIntPtr"));
		});
	}
}
