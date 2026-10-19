// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Xml.Linq;

using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Localization.MSBuild;
using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class WarningCodeTests : TestBase {
		[TestCase (51, "MT0051", null, 0)]
		[TestCase (108, "MT0108", "Entitlements.plist", 12)]
		[TestCase (179, "MT0179", null, 0)]
		[TestCase (7171, "MT7171", "Project.csproj", 0)]
		public void LoggingHelper (int code, string expectedCode, string? file, int line)
		{
			var task = CreateTask<CompileAppManifest> ();
			task.Log.LogWarning (code, file, MSBStrings.E0179, "TestTask");
			task.Log.LogWarning (code, file, line, MSBStrings.E0179, "TestTask");

			Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (2), "Warning count");
			for (var i = 0; i < 2; i++) {
				var warning = Engine.Logger.WarningsEvents [i];
				Assert.That (warning.Code, Is.EqualTo (expectedCode), "Code");
				Assert.That (warning.File, Is.EqualTo (file ?? "MSBuild"), "File");
				Assert.That (warning.LineNumber, Is.EqualTo (i == 0 ? 0 : line), "Line");
				Assert.That (warning.Message, Is.EqualTo (string.Format (MSBStrings.E0179, "TestTask")), "Message");
			}
		}

		[Test]
		public void NativeReferenceWarning ()
		{
			var task = CreateTask<CompileAppManifest> ();
			new LinkerOptions ().BuildNativeReferenceFlags (task.Log, [new TaskItem ("library.a")]);

			Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (1), "Warning count");
			Assert.That (Engine.Logger.WarningsEvents [0].Code, Is.EqualTo ("MT0051"), "Code");
			Assert.That (Engine.Logger.WarningsEvents [0].Message, Is.EqualTo (string.Format (MSBStrings.W0051, "library.a")), "Message");
		}

		[TestCase (null)]
		[TestCase ("Info.plist")]
		public void AppManifestWarning (string? file)
		{
			var task = CreateTask<ManifestWarningTask> ();
			task.AppManifest = file is null ? null : new TaskItem (file);
			ExecuteTask (task);

			Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (1), "Warning count");
			var warning = Engine.Logger.WarningsEvents [0];
			Assert.That (warning.Code, Is.EqualTo ("MT0106"), "Code");
			Assert.That (warning.File, Is.EqualTo (file ?? "MSBuild"), "File");
			Assert.That (warning.Message, Is.EqualTo (string.Format (MSBStrings.W0106, "CFBundleIdentifier")), "Message");
		}

		public class ManifestWarningTask : CompileAppManifest {
			public override bool Execute ()
			{
				LogAppManifestWarning (106, MSBStrings.W0106, "CFBundleIdentifier");
				return !Log.HasLoggedErrors;
			}
		}

		[Test]
		public void ProductDefinitionWarning ()
		{
			var task = CreateTask<ProductDefinitionWarningTask> ();
			task.ProductDefinition = "Product.plist";
			ExecuteTask (task);

			Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (1), "Warning count");
			var warning = Engine.Logger.WarningsEvents [0];
			Assert.That (warning.Code, Is.EqualTo ("MT7072"), "Code");
			Assert.That (warning.File, Is.EqualTo ("Product.plist"), "File");
			Assert.That (warning.Message, Is.EqualTo (string.Format (MSBStrings.E7072, "x86_64", "arm64")), "Message");
		}

		public class ProductDefinitionWarningTask : CompileProductDefinition {
			public override bool Execute ()
			{
				LogProductDefinitionWarning (7072, MSBStrings.E7072, "x86_64", "arm64");
				return !Log.HasLoggedErrors;
			}
		}

		[TestCase ("MSBuildWarningsAsMessages", "MT7194", 0)]
		[TestCase ("MSBuildWarningsNotAsErrors", "MT7194", 0)]
		[TestCase ("MSBuildWarningsAsMessages", "MT0000", 1)]
		[TestCase ("MSBuildWarningsNotAsErrors", "MT0000", 1)]
		public void WarningControls (string property, string code, int expectedExitCode)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var project = Path.Combine (directory, "Warnings.proj");
			var input = Path.Combine (directory, "input.dll");
			var cache = Path.Combine (directory, "cache.json");
			File.WriteAllText (input, "input");
			File.WriteAllText (cache, "invalid json");
			new XDocument (
				new XElement ("Project",
					new XElement ("UsingTask",
						new XAttribute ("TaskName", typeof (GetCachedFileHash).FullName ?? nameof (GetCachedFileHash)),
						new XAttribute ("AssemblyFile", typeof (GetCachedFileHash).Assembly.Location)),
					new XElement ("Target", new XAttribute ("Name", "Build"),
						new XElement (nameof (GetCachedFileHash),
							new XAttribute ("Files", input),
							new XAttribute ("CacheFile", cache)))))
				.Save (project);

			var exitCode = ExecutionHelper.Execute (DotNet.Executable,
				["msbuild", project, "-warnaserror", $"-p:{property}={code}", "-nologo", "-v:normal"],
				out string? output, TimeSpan.FromMinutes (1));
			Assert.That (exitCode, Is.EqualTo (expectedExitCode), output);
			if (expectedExitCode != 0)
				Assert.That (output, Does.Contain ("error MT7194"), "Warning promoted to error");
			else if (property == "MSBuildWarningsNotAsErrors")
				Assert.That (output, Does.Contain ("warning MT7194"), "Warning retained");
			else
				Assert.That (output, Does.Not.Contain ("warning MT7194"), "Warning demoted to message");
		}
	}
}
