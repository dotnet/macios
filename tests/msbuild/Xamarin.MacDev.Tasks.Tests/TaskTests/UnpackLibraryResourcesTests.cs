// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;

using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class UnpackLibraryResourcesTests : TestBase {

		// Test assemblies were compiled with:
		//   csc -target:library -out:UnpackLibraryResources-Traversal.dll -resource:traversal.txt,__monotouch_content_.._sEvil.txt empty.cs
		//   csc -target:library -out:UnpackLibraryResources-Valid.dll -resource:valid.txt,__monotouch_content_sub_sfile.txt empty.cs

		static string GetTestDataPath (string filename)
		{
			var path = Path.Combine (TestContext.CurrentContext.TestDirectory, "TestData", filename);
			if (!File.Exists (path))
				Assert.Ignore ($"Test data file not found: {path}");
			return path;
		}

		[Test]
		public void PathTraversal_IsRejected ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var assemblyPath = GetTestDataPath ("UnpackLibraryResources-Traversal.dll");

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("would extract to"));
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("outside"));

			// Verify the file was NOT written outside the target directory
			var escapedPath = Path.Combine (tmpdir, "intermediate", "unpack", "UnpackLibraryResources-Traversal", "Evil.txt");
			Assert.That (File.Exists (escapedPath), Is.False, "File should not have been extracted outside target directory");
		}

		[Test]
		public void ValidResource_IsExtracted ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var assemblyPath = GetTestDataPath ("UnpackLibraryResources-Valid.dll");

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 0);

			var extractedPath = Path.Combine (tmpdir, "intermediate", "unpack", "UnpackLibraryResources-Valid", "content", "sub", "file.txt");
			Assert.That (File.Exists (extractedPath), Is.True, $"File should have been extracted to {extractedPath}");
		}
	}
}
