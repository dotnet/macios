// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;

using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class XcodeBuildTaskTests : TestBase {
		class ArchiveTask : CreateXcArchive {
			public static string RemotePath (string path) => GetRemotePath (path);

			public string [] Arguments () => GenerateCommandLineCommands ().ToArray ();
		}

		[Test]
		[TestCase ("")]
		[TestCase ("Xcode project/TemplateFx.xcodeproj")]
		[TestCase ("obj/Debug/xcode")]
		public void RemotePaths (string path)
		{
			Assert.That (ArchiveTask.RemotePath (path), Is.EqualTo (path), "Relative path");
			if (path.Length > 0)
				Assert.That (ArchiveTask.RemotePath (Path.GetFullPath (path)), Is.EqualTo (path), "Absolute path");
		}

		[Test]
		public void CopiesEntireProjectDirectory ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var files = new [] {
				Path.Combine ("TemplateFx.xcodeproj", "project.pbxproj"),
				Path.Combine ("TemplateFx.xcodeproj", "xcshareddata", "xcschemes", "TemplateFx.xcscheme"),
				Path.Combine ("TemplateFx", "Source.swift"),
				Path.Combine ("TemplateFx", "Header.h"),
				Path.Combine ("TemplateFx", "Assets.xcassets", "Contents.json"),
				Path.Combine ("object-source", "Source.swift"),
			};
			var generatedFiles = new [] {
				Path.Combine ("obj", "Debug", "DerivedData", "build.db"),
				Path.Combine ("bin", "Debug", "Binding.dll"),
			};
			foreach (var file in files.Concat (generatedFiles)) {
				var path = Path.Combine (directory, file);
				Directory.CreateDirectory (Path.GetDirectoryName (path) ?? directory);
				File.WriteAllText (path, "input");
			}

			var task = CreateTask<CreateXcArchive> ();
			task.WorkingDirectory = directory;
			task.InputExcludeDirectories = new [] { Path.Combine (directory, "obj"), Path.Combine (directory, "bin") };
			var inputs = task.GetAdditionalItemsToBeCopied ().ToArray ();
			Assert.That (inputs.Select (v => v.ItemSpec), Is.EquivalentTo (files.Select (v => Path.Combine (directory, v))), "Files copied to the Mac");
		}

		[Test]
		public void ProjectPathIsAbsolute ()
		{
			var task = new ArchiveTask {
				ProjectPath = Path.Combine ("Xcode project", "TemplateFx.xcodeproj"),
				WorkingDirectory = "Xcode project",
			};
			var args = task.Arguments ();
			Assert.That (args [0], Is.EqualTo ("-project"), "Project option");
			Assert.That (args [1], Is.EqualTo (Path.GetFullPath (task.ProjectPath)), "Project path must not be relative to the Xcode working directory");
		}
	}
}
