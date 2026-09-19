// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.IO;
using System.Linq;

using Microsoft.Build.Utilities;

using Mono.Cecil;

using NUnit.Framework;

using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class AOTCompileTests : TestBase {
		static string CreateAssembly (string directory, string assemblyName, params string [] references)
		{
			var assemblyPath = Path.Combine (directory, assemblyName + ".dll");
			using var assembly = AssemblyDefinition.CreateAssembly (
				new AssemblyNameDefinition (assemblyName, new Version (1, 0, 0, 0)),
				assemblyName,
				ModuleKind.Dll);
			foreach (var reference in references)
				assembly.MainModule.AssemblyReferences.Add (new AssemblyNameReference (reference, new Version (1, 0, 0, 0)));
			assembly.Write (assemblyPath);
			return assemblyPath;
		}

		static TaskItem CreateAssemblyItem (string assemblyPath, bool isUpToDate)
		{
			var objectFile = assemblyPath + ".o";
			if (isUpToDate) {
				File.WriteAllText (objectFile, "");
				File.SetLastWriteTimeUtc (objectFile, File.GetLastWriteTimeUtc (assemblyPath).AddMinutes (1));
			}

			var item = new TaskItem (assemblyPath);
			item.SetMetadata ("ObjectFile", objectFile);
			item.SetMetadata ("AOTAssembly", assemblyPath + ".s");
			item.SetMetadata ("Arch", "arm64");
			return item;
		}

		[Test]
		public void OutOfDateReferenceAlreadyVisited ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var assemblyA = CreateAssembly (directory, "A", "C");
			var assemblyB = CreateAssembly (directory, "B", "C");
			var assemblyC = CreateAssembly (directory, "C");
			var itemA = CreateAssemblyItem (assemblyA, isUpToDate: true);
			var itemB = CreateAssemblyItem (assemblyB, isUpToDate: true);
			var itemC = CreateAssemblyItem (assemblyC, isUpToDate: false);

			var task = CreateTask<AOTCompile> ();
			task.AOTCompilerPath = "/usr/bin/true";
			task.Assemblies = [itemA, itemB, itemC];
			task.MinimumOSVersion = "1.0";
			task.OutputDirectory = directory;

			ExecuteTask (task);

			var fileWrites = task.FileWrites?.Select (v => v.ItemSpec).ToArray ();
			Assert.That (fileWrites, Is.EquivalentTo (new [] {
				itemA.GetMetadata ("ObjectFile"),
				itemB.GetMetadata ("ObjectFile"),
				itemC.GetMetadata ("ObjectFile"),
			}));
		}
	}
}
