// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
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
			task.Log.LogWarning (code, file, MSBStrings.E0179, "TestTask"); // There is no available connection to the Mac. Task '{0}' will not be executed
			task.Log.LogWarning (code, file, line, MSBStrings.E0179, "TestTask"); // There is no available connection to the Mac. Task '{0}' will not be executed

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
		public void ErrorLoggingHelper ()
		{
			var task = CreateTask<CompileAppManifest> ();
			task.Log.LogError (10, "Info.plist", MSBStrings.E0010, "Info.plist", "Invalid plist"); // Error loading '{0}': {1}
			task.Log.LogError (10, "Info.plist", 12, MSBStrings.E0010, "Info.plist", "Invalid plist"); // Error loading '{0}': {1}
			task.Log.LogError (10, "Info.plist", 12, 2, 15, 8, MSBStrings.E0010, "Info.plist", "Invalid plist"); // Error loading '{0}': {1}

			Assert.That (Engine.Logger.ErrorEvents.Count, Is.EqualTo (3), "Error count");
			for (var i = 0; i < 3; i++) {
				var error = Engine.Logger.ErrorEvents [i];
				Assert.That (error.Code, Is.EqualTo ("MT0010"), "Code");
				Assert.That (error.File, Is.EqualTo ("Info.plist"), "File");
				Assert.That (error.LineNumber, Is.EqualTo (i == 0 ? 0 : 12), "Line");
				Assert.That (error.ColumnNumber, Is.EqualTo (i == 2 ? 2 : 0), "Column");
				Assert.That (error.EndLineNumber, Is.EqualTo (i == 2 ? 15 : 0), "End line");
				Assert.That (error.EndColumnNumber, Is.EqualTo (i == 2 ? 8 : 0), "End column");
				Assert.That (error.Message, Is.EqualTo (string.Format (MSBStrings.E0010, "Info.plist", "Invalid plist")), "Message");
			}
		}

		[TestCase ("E0179", true, "MT0179", new [] { "TestTask" })]
		[TestCase ("W0108b", false, "MT0108", new [] { "key", "value" })]
		[TestCase ("W7225", false, "MT7225", new string [0])]
		[TestCase ("E7152_InvalidMtouchHttpClientHandler", true, "MT7152", new [] { "invalid" })]
		public void ResourceDiagnostic (string resource, bool error, string expectedCode, string [] arguments)
		{
			var task = CreateTask<MacDevMessage> ();
			task.ResourceName = resource;
			task.Error = error;
			task.Warning = !error;
			task.FormatArguments = Array.ConvertAll (arguments, value => new TaskItem (value));
			ExecuteTask (task, error ? 1 : 0);

			if (error) {
				Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo (expectedCode), "Error code");
				Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo (string.Format (MSBStrings.ResourceManager.GetString (resource) ?? "", arguments)), "Error message");
			} else {
				Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (1), "Warning count");
				Assert.That (Engine.Logger.WarningsEvents [0].Code, Is.EqualTo (expectedCode), "Warning code");
				Assert.That (Engine.Logger.WarningsEvents [0].Message, Is.EqualTo (string.Format (MSBStrings.ResourceManager.GetString (resource) ?? "", arguments)), "Warning message");
			}
		}

		[TestCase ("InvalidPlatform", true)]
		[TestCase ("InvalidPlatform", false)]
		[TestCase ("E", true)]
		[TestCase ("Eabcd", true)]
		public void UnnumberedResourceDiagnostic (string resource, bool error)
		{
			var task = CreateTask<MacDevMessage> ();
			task.ResourceName = resource;
			task.Error = error;
			task.Warning = !error;
			task.FormatArguments = [new TaskItem ("invalid")];
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo ("MT7223"), "Code");
		}

		[Test]
		public void AppManifestError ()
		{
			var task = CreateTask<ManifestErrorTask> ();
			task.AppManifest = new TaskItem ("Info.plist");
			ExecuteTask (task, 1);

			Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo ("MT0010"), "Code");
			Assert.That (Engine.Logger.ErrorEvents [0].File, Is.EqualTo ("Info.plist"), "File");
		}

		public class ManifestErrorTask : CompileAppManifest {
			public override bool Execute ()
			{
				LogAppManifestError (10, MSBStrings.E0010, "Info.plist", "Invalid plist"); // Error loading '{0}': {1}
				return !Log.HasLoggedErrors;
			}
		}

		[Test]
		public void MissingAppManifest ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var file = Path.Combine (directory, "missing.plist");
			var task = CreateTask<WriteAppManifest> ();
			task.AppManifests = [new TaskItem (file)];
			task.AppBundleManifest = new TaskItem (Path.Combine (directory, "Info.plist"));
			ExecuteTask (task, 1);

			var error = Engine.Logger.ErrorEvents [0];
			Assert.That (error.Code, Is.EqualTo ("MT0010"), "Code");
			Assert.That (error.File, Is.EqualTo (file), "File");
			Assert.That (error.Message, Does.StartWith (string.Format (MSBStrings.E0010, file, "")), "Message");
		}

		[Test]
		public void DebugConfigurationWriteError ()
		{
			var task = CreateTask<CreateDebugConfiguration> ();
			task.AppBundleDir = Path.Combine (Cache.CreateTemporaryDirectory (), "missing");
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo ("MT7226"), "Code");
			Assert.That (Engine.Logger.ErrorEvents [0].File, Is.EqualTo (Path.Combine (task.AppBundleDir, "MonoTouchDebugConfiguration.txt")), "File");
		}

		[Test]
		public void CopyError ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<SmartCopy> ();
			task.SourceFiles = [new TaskItem (Path.Combine (directory, "missing"))];
			task.DestinationFiles = [new TaskItem (Path.Combine (directory, "output"))];
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo ("MT7228"), "Code");
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
				LogAppManifestWarning (106, MSBStrings.W0106, "CFBundleIdentifier"); // Could not determine value for manifest key '{0}'
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
				LogProductDefinitionWarning (7072, MSBStrings.E7072, "x86_64", "arm64"); // Package product requirement file contains architectures ({0}) which mismatches with target architectures ({1})
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

		[Test]
		public void NativeHttpHandlerWarning ()
		{
			var targets = XDocument.Load (Path.Combine (Configuration.RootPath, "dotnet", "targets", "Xamarin.Shared.Sdk.targets"));
			var ns = targets.Root?.Name.Namespace ?? XNamespace.None;
			var target = targets.Root?.Elements (ns + "Target").Single (v => (string?) v.Attribute ("Name") == "_VerifyValidProperties");
			Assert.That (target, Is.Not.Null, "Validation target");
			var project = Path.Combine (Cache.CreateTemporaryDirectory (), "Warnings.proj");
			new XDocument (
				new XElement (ns + "Project",
					new XElement (ns + "UsingTask",
						new XAttribute ("TaskName", typeof (MacDevMessage).FullName ?? nameof (MacDevMessage)),
						new XAttribute ("AssemblyFile", typeof (MacDevMessage).Assembly.Location)),
					target))
				.Save (project);
			var exitCode = ExecutionHelper.Execute (DotNet.Executable,
				["msbuild", project, "-t:_VerifyValidProperties", "-p:_PlatformName=macOS", "-p:UseNativeHttpHandler=true", "-nologo", "-v:normal"],
				out string? output, TimeSpan.FromMinutes (1));
			Assert.That (exitCode, Is.EqualTo (0), output);
			Assert.That (output, Does.Contain ("warning MT7225"), "Warning code");
			Assert.That (output, Does.Contain (MSBStrings.W7225), "Warning message");
		}
	}
}
