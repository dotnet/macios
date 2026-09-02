// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;

using Microsoft.Build.Utilities;

using Mono.Cecil;

using NUnit.Framework;

using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class UnpackLibraryResourcesTests : TestBase {

		static string CreateAssemblyWithResource (string directory, string assemblyName, string resourceName, byte [] content)
		{
			var asmPath = Path.Combine (directory, assemblyName + ".dll");
			var assemblyDef = AssemblyDefinition.CreateAssembly (
				new AssemblyNameDefinition (assemblyName, new Version (1, 0, 0, 0)),
				assemblyName,
				ModuleKind.Dll);

			var resource = new EmbeddedResource (resourceName, ManifestResourceAttributes.Public, content);
			assemblyDef.MainModule.Resources.Add (resource);
			assemblyDef.Write (asmPath);

			return asmPath;
		}

		[Test]
		public void PathTraversal_IsRejected ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			// Resource name "__monotouch_content_.._sEvil.txt" unmangles to "../Evil.txt" (path traversal)
			var assemblyPath = CreateAssemblyWithResource (tmpdir, "TestTraversal", "__monotouch_content_.._sEvil.txt", new byte [] { 0x41 });

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("would extract to"));
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("outside"));

			// Verify the file was NOT written outside the target directory
			var escapedPath = Path.Combine (tmpdir, "intermediate", "unpack", "TestTraversal", "Evil.txt");
			Assert.That (File.Exists (escapedPath), Is.False, "File should not have been extracted outside target directory");
		}

		[Test]
		public void ValidResource_IsExtracted ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			// Resource name "__monotouch_content_sub_sfile.txt" unmangles to "sub/file.txt" (valid path)
			var assemblyPath = CreateAssemblyWithResource (tmpdir, "TestValid", "__monotouch_content_sub_sfile.txt", new byte [] { 0x41 });

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 0);

			var extractedPath = Path.Combine (tmpdir, "intermediate", "unpack", "TestValid", "content", "sub", "file.txt");
			Assert.That (File.Exists (extractedPath), Is.True, $"File should have been extracted to {extractedPath}");
		}
	}
}
