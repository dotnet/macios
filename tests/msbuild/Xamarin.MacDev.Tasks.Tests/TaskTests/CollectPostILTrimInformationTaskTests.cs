// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.IO;

using Microsoft.Build.Utilities;

using Mono.Cecil;

using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class CollectPostILTrimInformationTaskTests : TestBase {
		static void WriteAssemblyWithInternalPInvoke (string path, string symbolName, DateTime lastWriteTimeUtc)
		{
			using (var assembly = AssemblyDefinition.CreateAssembly (
				new AssemblyNameDefinition (Path.GetFileNameWithoutExtension (path), new Version (1, 0, 0, 0)),
				Path.GetFileNameWithoutExtension (path),
				ModuleKind.Dll)) {
				var moduleRef = new ModuleReference ("__Internal");
				assembly.MainModule.ModuleReferences.Add (moduleRef);

				var type = new TypeDefinition ("TestNamespace", "TestType", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed, assembly.MainModule.TypeSystem.Object);
				var method = new MethodDefinition (
					symbolName,
					MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PInvokeImpl | MethodAttributes.HideBySig,
					assembly.MainModule.TypeSystem.IntPtr) {
					PInvokeInfo = new PInvokeInfo (PInvokeAttributes.CallConvWinapi, symbolName, moduleRef),
				};
				type.Methods.Add (method);
				assembly.MainModule.Types.Add (type);
				assembly.Write (path);
			}
			File.SetLastWriteTimeUtc (path, lastWriteTimeUtc);
		}

		[Test]
		public void DowngradedAssemblyWithOlderTimestampInvalidatesCache ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var assemblyPath = Path.Combine (directory, "Dep.dll");
			var cacheDir = Path.Combine (directory, "cache");
			var nativeSymbolsFile = Path.Combine (directory, "surviving-native-symbols.txt");
			var classesFile = Path.Combine (directory, "surviving-classes.txt");

			var newerTimestamp = new DateTime (2026, 9, 16, 0, 0, 0, DateTimeKind.Utc);
			var olderTimestamp = new DateTime (2021, 3, 17, 0, 0, 0, DateTimeKind.Utc);

			// First build with a newer package version
			WriteAssemblyWithInternalPInvoke (assemblyPath, "xamarin_Dlfcn_V2Symbol_Native", newerTimestamp);
			RunTask (assemblyPath, cacheDir, nativeSymbolsFile, classesFile);
			Assert.That (File.ReadAllLines (nativeSymbolsFile), Is.EqualTo (new [] { "V2Symbol" }), "First build symbols");

			// Second build with an unchanged assembly should hit the cache
			Engine.Logger.Clear ();
			RunTask (assemblyPath, cacheDir, nativeSymbolsFile, classesFile);
			Assert.That (File.ReadAllLines (nativeSymbolsFile), Is.EqualTo (new [] { "V2Symbol" }), "Cached build symbols");

			// Downgrade the assembly to an older version with an older timestamp (older than the cache file)
			WriteAssemblyWithInternalPInvoke (assemblyPath, "xamarin_Dlfcn_V1SymbolWithDifferentLength_Native", olderTimestamp);
			RunTask (assemblyPath, cacheDir, nativeSymbolsFile, classesFile);
			Assert.That (File.ReadAllLines (nativeSymbolsFile), Is.EqualTo (new [] { "V1SymbolWithDifferentLength" }), "Downgraded build symbols");
		}

		void RunTask (string assemblyPath, string cacheDir, string nativeSymbolsFile, string classesFile)
		{
			var task = CreateTask<CollectPostILTrimInformation> ();
			task.TrimmedAssemblies = [new TaskItem (assemblyPath)];
			task.CacheDirectory = cacheDir;
			task.SurvivingNativeSymbolsFile = nativeSymbolsFile;
			task.SurvivingClassesFile = classesFile;
			ExecuteTask (task);
		}
	}
}
