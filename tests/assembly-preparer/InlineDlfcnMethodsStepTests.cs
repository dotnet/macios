// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using MonoTouch.Tuner;

using Xamarin.Linker.Steps;

namespace AssemblyPreparerTests;

public class InlineDlfcnMethodsStepTests : BaseClass {
	[TestCase (true)]
	[TestCase (false)]
	public void FindingsCache (bool cacheIsCurrent)
	{
		var code = @"
		using System;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			void GetIntPtr ()
			{
				Console.WriteLine (Dlfcn.GetIntPtr (0, ""NativeSymbol""));
			}
		}";

		using var preparer = CreatePreparer (ApplePlatform.iOS, false, null, code, out var testInfo, inlineDlfcnMethods: "strict");
		var cacheDirectory = Path.Combine (preparer.IntermediateOutputPath, "inline-dlfcn-cache");
		var cacheFile = Path.Combine (cacheDirectory, "Test.txt");
		Directory.CreateDirectory (cacheDirectory);
		File.WriteAllText (cacheFile, "");
		File.SetLastWriteTimeUtc (cacheFile, cacheIsCurrent ? DateTime.UtcNow.AddMinutes (1) : DateTime.UtcNow.AddYears (-1));
		preparer.InlineDlfcnCacheDirectory = cacheDirectory;

		var context = preparer.Configuration.DerivedLinkContext;
		new LoadAssembliesStep ().Process (context);
		new CollectFieldsStep ().Process (context);
		new InlineDlfcnMethodsStep ().Process (context);

		var assembly = context.GetAssemblies ().Single (v => v.Name.Name == "Test");
		var method = assembly.MainModule.GetType ("MyClass").Methods.Single (v => v.Name == "GetIntPtr");
		var callsDlfcn = method.Body.Instructions.Any (v => v.Operand is MethodReference mr && mr.DeclaringType.FullName == "ObjCRuntime.Dlfcn");
		Assert.That (callsDlfcn, Is.EqualTo (cacheIsCurrent), "Dlfcn call");
		Assert.That (preparer.InlineDlfcnCacheHits, Is.EqualTo (cacheIsCurrent ? 1 : 0), "Cache hits");

		if (!cacheIsCurrent) {
			Assert.That (File.GetLastWriteTimeUtc (cacheFile), Is.GreaterThanOrEqualTo (File.GetLastWriteTimeUtc (testInfo.InputPath)), "Cache timestamp");
			var findings = File.ReadAllLines (cacheFile);
			Assert.That (findings.All (v => v.Length == 8 && v.All (Uri.IsHexDigit)), Is.True, $"Cache findings: {string.Join (", ", findings)}");
		}
	}

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

	[Test]
	[TestCase (ApplePlatform.MacCatalyst, false)]
	[TestCase (ApplePlatform.iOS, false)]
	[TestCase (ApplePlatform.TVOS, false)]
	[TestCase (ApplePlatform.MacOSX, true)]
	public void HotReloadCompatibleBuildLeavesReloadableAssemblyUnmodified (ApplePlatform platform, bool isCoreCLR)
	{
		// A [Field] symbol referenced via Dlfcn, exactly the shape that compatibility-mode inlining
		// (the default for Debug builds) would normally collect as a native symbol.
		var code = @"
		using System;
		using CoreAnimation;
		using Foundation;
		using ObjCRuntime;

		class MyClass : NSObject {
			[Field (""NativeSymbol"", ""__Internal"")]
			static IntPtr MyField {
				get { return Dlfcn.GetIntPtr (0, ""NativeSymbol""); }
			}
		}";

		// The test assembly is a reloadable (Copy) assembly, and we're building for Hot Reload compatibility,
		// so the assembly must be left byte-unmodified: no inlining, no generated P/Invokes or helper members.
		// Use compatibility mode (the Debug default) so the referenced [Field] symbol is eligible for collection.
		var modified = AssertPrepare (platform, isCoreCLR, RegistrarMode.Dynamic, code, out var assemblyDefinition, out var preparer, hotReloadCompatibleBuild: true, testAssemblyTrimMode: "copy", inlineDlfcnMethods: "compatibility");
		Assert.That (modified, Is.False, "The reloadable assembly must not be modified.");

		var type = assemblyDefinition.MainModule.Types.Single (v => v.Name == "MyClass");

		// The Dlfcn call must remain an ordinary call into the platform assembly's Dlfcn type (not inlined into a
		// generated P/Invoke in the user assembly).
		var call = type.Methods
			.Where (m => m.HasBody)
			.SelectMany (m => m.Body.Instructions)
			.FirstOrDefault (v => v.OpCode == OpCodes.Call && v.Operand is MethodReference mr && mr.DeclaringType.Name == "Dlfcn" && mr.Name == "GetIntPtr");
		Assert.That (call, Is.Not.Null, "Expected the original call to Dlfcn.GetIntPtr to be preserved.");

		// No generated Dlfcn helper type should have been added to the user assembly.
		var generatedDlfcn = assemblyDefinition.MainModule.GetTypes ().FirstOrDefault (v => v.Name == "Dlfcn");
		Assert.That (generatedDlfcn, Is.Null, "No Dlfcn helper type should have been generated in the reloadable assembly.");

		// Even though we didn't inline the call, the referenced native symbol must still be collected so the
		// native linker keeps it alive (GenerateReferencesStep turns RequiredSymbols into native references).
		var requiredSymbol = preparer.Configuration.DerivedLinkContext.RequiredSymbols.Find ("NativeSymbol");
		Assert.That (requiredSymbol, Is.Not.Null, "The referenced native symbol must still be collected for native linking.");
	}
}
