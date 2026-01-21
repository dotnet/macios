using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using NUnit.Framework;
using Xamarin.MacDev;

using Xamarin.Utils;

namespace Xamarin.Tests {
	public abstract class TestBase {
		public string Platform;
		public string Config = "Debug";

		public TestBase ()
		{
		}

		public TestBase (string platform)
		{
			Platform = platform;
		}

		public TestBase (string platform, string config)
		{
			Platform = platform;
			Config = config;
		}

		protected static class TargetName {
			public static string Build = "Build";
			public static string Clean = "Clean";
			public static string CollectBundleResources = "_CollectBundleResources";
			public static string CompileImageAssets = "_CompileImageAssets";
			public static string CompileInterfaceDefinitions = "_CompileInterfaceDefinitions";
			public static string CopyResourcesToBundle = "_CopyResourcesToBundle";
			public static string DetectAppManifest = "_DetectAppManifest";
			public static string GenerateBundleName = "_GenerateBundleName";
			public static string PackLibraryResources = "_PackLibraryResources";
			public static string ResolveReferences = "ResolveReferences";
		}

		public string [] ExpectedAppFiles = { };
		public string [] UnexpectedAppFiles = { "monotouch.dll" };

		public string [] GetCoreAppFiles (string managedExe, string nativeExe)
		{
			var coreFiles = new List<string> ();

			coreFiles.Add ("mscorlib.dll");
			if (Config == "Debug")
				coreFiles.Add ("mscorlib.pdb");

			coreFiles.Add (managedExe);
			if (Config == "Debug")
				coreFiles.Add (Path.ChangeExtension (managedExe, ".pdb"));

			coreFiles.Add (nativeExe);

			return coreFiles.ToArray ();
		}

		public BuildEngine Engine {
			get; private set;
		}

		public ProjectPaths LibraryProject {
			get; private set;
		}

		public ProjectPaths MonoTouchProject {
			get; protected set;
		}

		public MSBuildProject MonoTouchProjectInstance {
			get; private set;
		}

		public MSBuildProject LibraryProjectInstance {
			get; private set;
		}

		public string LibraryProjectBinPath => LibraryProject.ProjectBinPath;
		public string LibraryProjectObjPath => LibraryProject.ProjectObjPath;
		public string LibraryProjectPath => LibraryProject.ProjectPath;

		public string MonoTouchProjectBinPath => MonoTouchProject.ProjectBinPath;
		public string MonoTouchProjectObjPath => MonoTouchProject.ProjectObjPath;
		public string MonoTouchProjectPath => MonoTouchProject.ProjectPath;
		public string AppBundlePath => MonoTouchProject.AppBundlePath;

		[SetUp]
		public virtual void Setup ()
		{
			//MonoTouchProjectInstance = new MSBuildProject (MonoTouchProject, this);
			//LibraryProjectInstance = new MSBuildProject (LibraryProject, this);

			Engine = new BuildEngine ();
		}

		public ApplePlatform ApplePlatform {
			get {
				return new TargetFramework (TargetFrameworkIdentifier, null).Platform;
			}
		}

		public virtual string TargetFrameworkIdentifier {
			get {
				return "Xamarin.iOS";
			}
		}

		public bool IsTVOS {
			get { return TargetFrameworkIdentifier == "Xamarin.TVOS"; }
		}

		public void TestFilesDoNotExist (string baseDir, IEnumerable<string> files)
		{
			foreach (var v in files.Select (s => Path.Combine (baseDir, s)))
				Assert.IsFalse (File.Exists (v) || Directory.Exists (v), "Unexpected file: {0} exists", v);
		}

		public void TestFilesExists (string baseDir, string [] files)
		{
			foreach (var v in files.Select (s => Path.Combine (baseDir, s)))
				Assert.IsTrue (File.Exists (v) || Directory.Exists (v), "Expected file: {0} does not exist", v);
		}

		public void TestFilesExists (string [] baseDirs, string [] files)
		{
			if (baseDirs.Length == 1) {
				TestFilesExists (baseDirs [0], files);
			} else {
				foreach (var file in files)
					Assert.IsTrue (baseDirs.Select (s => File.Exists (Path.Combine (s, file))).Any (v => v), $"Expected file: {file} does not exist in any of the directories: {string.Join (", ", baseDirs)}");
			}
		}

		public void TestStoryboardC (string path)
		{
			Assert.IsTrue (Directory.Exists (path), "Storyboard {0} does not exist", path);
			Assert.IsTrue (File.Exists (Path.Combine (path, "Info.plist")));
			TestPList (path, new string [] { "CFBundleVersion", "CFBundleExecutable" });
		}

		public void TestPList (string path, string [] keys)
		{
			var plist = PDictionary.FromFile (Path.Combine (path, "Info.plist"));
			foreach (var x in keys) {
				Assert.IsTrue (plist.ContainsKey (x), "Key {0} is not present in {1} Info.plist", x, path);
				Assert.IsNotEmpty (((PString) plist [x]).Value, "Key {0} is empty in {1} Info.plist", x, path);
			}
		}

		protected string CreateTempFile (string path)
		{
			var dir = Cache.CreateTemporaryDirectory ();
			path = Path.Combine (dir, path);
			using (new FileStream (path, FileMode.CreateNew)) { }
			return path;
		}

		protected DateTime GetLastModified (string file)
		{
			if (Path.GetExtension (file) == ".nib" && !File.Exists (file))
				file = Path.Combine (file, "runtime.nib");

			if (!File.Exists (file))
				Assert.Fail ("Expected file '{0}' did not exist", file);

			return File.GetLastWriteTimeUtc (file);
		}

		protected void Touch (string file)
		{
			if (!File.Exists (file))
				Assert.Fail ("Expected file '{0}' did not exist", file);
			EnsureFilestampChange ();
			File.SetLastWriteTimeUtc (file, DateTime.UtcNow);
			EnsureFilestampChange ();
		}

		static bool? is_apfs;
		public static bool IsAPFS {
			get {
				if (!is_apfs.HasValue) {
					if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
						is_apfs = false;
					} else {
						var exit_code = ExecutionHelper.Execute ("/bin/df", new string [] { "-t", "apfs", "/" }, out var output, TimeSpan.FromSeconds (10));
						is_apfs = exit_code == 0 && output.Trim ().Split ('\n').Length >= 2;
					}
				}
				return is_apfs.Value;
			}
		}

		public static void EnsureFilestampChange ()
		{
			if (IsAPFS)
				return;
			Thread.Sleep (1000);
		}

		public void RunTarget (ProjectPaths paths, string target, int expectedErrorCount = 0, Dictionary<string, string> properties = null)
		{
			var rv = Engine.RunTarget (ApplePlatform, paths.ProjectCSProjPath, target, properties);
			if (expectedErrorCount != Engine.ErrorEvents.Count) {
				foreach (var e in Engine.ErrorEvents)
					Console.WriteLine (e.ToString ());
				Assert.AreEqual (expectedErrorCount, Engine.ErrorEvents.Count, $"Unexpected number of errors when executing target '{target}'");
			}
			if (expectedErrorCount > 0) {
				Assert.AreEqual (1, rv.ExitCode, "ExitCode (failure)");
			} else {
				Assert.AreEqual (0, rv.ExitCode, "ExitCode (success)");
			}
		}

		public static void NugetRestore (string project)
		{
			var rv = ExecutionHelper.Execute ("nuget", new string [] { "restore", project }, out var output);
			if (rv != 0) {
				Console.WriteLine ("nuget restore failed:");
				Console.WriteLine (output);
				Assert.Fail ($"'nuget restore' failed for {project}");
			}
		}

		/// <summary>
		/// Returns true if a target was skipped.
		/// Originally from: https://github.com/xamarin/xamarin-android/blob/320cb0f66730e7107cc17310b99cd540605a234d/src/Xamarin.Android.Build.Tasks/Tests/Xamarin.ProjectTools/Common/BuildOutput.cs#L48-L62
		/// </summary>
		public bool IsTargetSkipped (string target)
		{
			foreach (var line in Engine.Logger.MessageEvents.Select (m => m.Message)) {
				if (line.Contains ($"Building target \"{target}\" completely")
					|| line.Contains ($"Done building target \"{target}\""))
					return false;
				if (line.Contains ($"Target {target} skipped due to ")
					|| line.Contains ($"Skipping target \"{target}\" because it has no ") //NOTE: message can say `inputs` or `outputs`
					|| line.Contains ($"Target \"{target}\" skipped, due to")
					|| line.Contains ($"Skipping target \"{target}\" because its outputs are up-to-date")
					|| line.Contains ($"target {target}, skipping")
					|| line.Contains ($"Skipping target \"{target}\" because all output files are up-to-date"))
					return true;
			}
			return false;
		}
	}

	public class ProjectPaths {
		public string ProjectPath { get; set; }
		public string ProjectBinPath { get; set; }
		public string ProjectObjPath { get; set; }
		public string ProjectCSProjPath { get; set; }
		public string AppBundlePath { get; set; }
	}
}
