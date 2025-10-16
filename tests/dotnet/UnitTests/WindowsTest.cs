// #define TRACE

using System.IO;
using System.IO.Compression;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class WindowsTest : TestBaseClass {

		[Category ("Windows")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-x64")]
		public void BundleStructureNonRemotablePlatforms (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.IgnoreIfNotOnWindows ();

			var project = "BundleStructure";
			var configuration = "Debug";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			var project_dir = Path.GetDirectoryName (Path.GetDirectoryName (project_path))!;
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);

			// For any platform other than iOS, all we care about is that project builds, we don't care about the built app.
			// So assert that the project builds.
			DotNet.AssertBuild (project_path, properties);
		}

		[Category ("Windows")]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-x64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-x64")]
		public void BuildAppWithXCFrameworkWithSymlinks (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.IgnoreIfNotOnWindows ();

			var project = "AppWithXCFrameworkWithSymlinks";
			var configuration = "Debug";
			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			var project_dir = Path.GetDirectoryName (Path.GetDirectoryName (project_path))!;
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);

			// For any platform other than iOS, all we care about is that project builds, we don't care about the built app.
			// So assert that the project builds.
			DotNet.AssertBuild (project_path, properties);
		}

		class FileData {
			public required string FullPath;
			public required string RelativePath;
		}

		void AssertMaxFileLengthInBinAndObjDirectories (ApplePlatform platform, string project_path, string runtimeIdentifiers, string configuration, int maxLength = 110)
		{
			var binDir = GetBinDir (project_path, platform, runtimeIdentifiers, configuration);
			var objDir = GetObjDir (project_path, platform, runtimeIdentifiers, configuration);

			var allFiles = new List<FileData> ();
			foreach (var entry in new [] { new { Dir = binDir, Distinguisher = "bin" }, new { Dir = objDir, Distinguisher = "obj" } }) {
				var dir = entry.Dir;
				allFiles.AddRange (Directory.GetFileSystemEntries (dir, "*", SearchOption.AllDirectories).Select (v => new FileData { FullPath = v, RelativePath = $"{entry.Distinguisher}:{v.Substring (dir.Length + 1)}" }));
			}

			// Console.WriteLine ($"Found {allFiles.Count} files in bin and obj:");
			// foreach (var f in allFiles.OrderBy (v => v.RelativePath.Length)) {
			// 	Console.WriteLine ($"    Length={f.RelativePath.Length} {f.RelativePath} ({f.FullPath})");
			// }

			var longerThanMax = allFiles.Where (v => v.RelativePath.Length > maxLength).Select (v => $"{v.RelativePath} (length: {v.RelativePath.Length})").ToArray ();
			Assert.That (longerThanMax, Is.Empty, $"Relative paths longer than max ({maxLength})");

			var invalidPaths = new string []
			{
				// 'full-paths-exceeding-two-hundred-and-sixty-characters' is a subdirectory inside the FrameworkWithLongFileNames framework
				"full-paths-exceeding-two-hundred-and-sixty-characters",
				// 'especially-when-contained-in-other-directories.h' is a file inside the FrameworkWithLongFileNames framework
				"especially-when-contained-in-other-directories.h",
			};
			foreach (var ip in invalidPaths) {
				var withLongFilenames = allFiles.Where (v => v.RelativePath.Contains (ip)).ToArray ();
				Assert.That (longerThanMax, Is.Empty, $"No paths with '{ip}'");
			}
		}

		[Category ("RemoteWindows")]
		[TestCase (ApplePlatform.iOS, "ios-arm64", BundleStructureTest.CodeSignature.All, "Debug")]
		public void BundleStructureWithRemoteMac (ApplePlatform platform, string runtimeIdentifiers, BundleStructureTest.CodeSignature signature, string configuration)
		{
			var project = "BundleStructure";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);
			Configuration.IgnoreIfNotOnWindows ();

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath, configuration: configuration);
			var project_dir = Path.GetDirectoryName (Path.GetDirectoryName (project_path))!;
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			properties ["_IsAppSigned"] = signature != BundleStructureTest.CodeSignature.None ? "true" : "false";
			if (!string.IsNullOrWhiteSpace (configuration))
				properties ["Configuration"] = configuration;

			// Copy the app bundle to Windows so that we can inspect the results.
			properties ["CopyAppBundleToWindows"] = "true";

			var rv = DotNet.AssertBuild (project_path, properties);
			var warnings = BinLog.GetBuildLogWarnings (rv.BinLogPath).ToArray ();
			var warningMessages = BundleStructureTest.FilterWarnings (warnings, canonicalizePaths: true);

			var isReleaseBuild = string.Equals (configuration, "Release", StringComparison.OrdinalIgnoreCase);
			var platformString = platform.AsString ();
			var tfm = platform.ToFramework ();
			var testsDirectory = Path.GetDirectoryName (Path.GetDirectoryName (project_dir))!;
			var expectedWarnings = new List<string> {
				$"The 'PublishFolderType' metadata value 'Unknown' on the item '{Path.Combine (project_dir, platformString, "SomewhatUnknownI.bin")}' is not recognized. The file will not be copied to the app bundle. If the file is not supposed to be copied to the app bundle, remove the 'CopyToOutputDirectory' metadata on the item.",
				$"The 'PublishFolderType' metadata value 'Unknown' on the item '{Path.Combine (project_dir, platformString, "UnknownI.bin")}' is not recognized. The file will not be copied to the app bundle. If the file is not supposed to be copied to the app bundle, remove the 'CopyToOutputDirectory' metadata on the item.",
				$"The file '{Path.Combine (project_dir, platformString, "NoneA.txt")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
				$"The file '{Path.Combine (project_dir, platformString, "NoneI.txt")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
				$"The file '{Path.Combine (project_dir, platformString, "NoneJ.txt")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
				$"The file '{Path.Combine (project_dir, platformString, "NoneK.txt")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
				$"The file '{Path.Combine (project_dir, platformString, "NoneM.unknown")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
				$"The file '{Path.Combine (project_dir, platformString, "Sub", "NoneG.txt")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
				$"The file '{Path.Combine (project_dir, "NoneH.txt")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
				$"The file '{Path.Combine (project_dir, "NoneO.xml")}' does not specify a 'PublishFolderType' metadata, and a default value could not be calculated. The file will not be copied to the app bundle.",
			};

			var rids = runtimeIdentifiers.Split (';');
			if (rids.Length > 1) {
				// All warnings show up twice if we're building for multiple architectures
				expectedWarnings.AddRange (expectedWarnings);
			}

			if (signature == BundleStructureTest.CodeSignature.None && (platform == ApplePlatform.MacCatalyst || platform == ApplePlatform.MacOSX)) {
				expectedWarnings.Add ($"Found files in the root directory of the app bundle. This will likely cause codesign to fail. Files:\n{Path.Combine ("bin", configuration, tfm, runtimeIdentifiers.IndexOf (';') >= 0 ? string.Empty : runtimeIdentifiers, "BundleStructure.app", "UnknownJ.bin")}");
			}

			// Sort the messages so that comparison against the expected array is faster
			expectedWarnings = expectedWarnings
				.Select (v => v.Replace (Path.DirectorySeparatorChar, '/')) // warnings we get are from macOS, so make sure we expect macOS-style paths.
				.OrderBy (v => v)
				.ToList ();

			var appExecutable = GetNativeExecutable (platform, appPath);

			var objDir = GetObjDir (project_path, platform, runtimeIdentifiers, configuration);
			var zippedAppBundlePath = Path.Combine (objDir, "AppBundle.zip");
			Assert.That (zippedAppBundlePath, Does.Exist, "AppBundle.zip");

			BundleStructureTest.CheckZippedAppBundleContents (platform, zippedAppBundlePath, rids, signature, isReleaseBuild);
			AssertWarningsEqual (expectedWarnings, warningMessages, "Warnings");
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Verify that we don't create files with long paths inside bin/obj
			AssertMaxFileLengthInBinAndObjDirectories (platform, project_path, runtimeIdentifiers, configuration);

			// touch AppDelegate.cs, and rebuild should succeed and do the right thing
			var appDelegatePath = Path.Combine (project_dir, "AppDelegate.cs");
			Configuration.Touch (appDelegatePath);

			rv = DotNet.AssertBuild (project_path, properties);
			var allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			warnings = BinLog.GetBuildLogWarnings (rv.BinLogPath).ToArray ();
			warningMessages = BundleStructureTest.FilterWarnings (warnings, canonicalizePaths: true);

			BundleStructureTest.CheckZippedAppBundleContents (platform, zippedAppBundlePath, rids, signature, isReleaseBuild);
			AssertWarningsEqual (expectedWarnings, warningMessages, "Warnings Rebuild 1");
			AssertTargetNotExecuted (allTargets, "_CompileAppManifest", "_CompileAppManifest Rebuild 1");
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Verify that we don't create files with long paths inside bin/obj
			AssertMaxFileLengthInBinAndObjDirectories (platform, project_path, runtimeIdentifiers, configuration);

			// remove the bin directory, and rebuild should succeed and do the right thing
			var binDirectory = Path.Combine (Path.GetDirectoryName (project_path)!, "bin");
			Directory.Delete (binDirectory, true);

			rv = DotNet.AssertBuild (project_path, properties);
			allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			warnings = BinLog.GetBuildLogWarnings (rv.BinLogPath).ToArray ();
			warningMessages = BundleStructureTest.FilterWarnings (warnings, canonicalizePaths: true);

			BundleStructureTest.CheckZippedAppBundleContents (platform, zippedAppBundlePath, rids, signature, isReleaseBuild);
			AssertWarningsEqual (expectedWarnings, warningMessages, "Warnings Rebuild 2");
			AssertTargetNotExecuted (allTargets, "_CompileAppManifest", "_CompileAppManifest Rebuild 2");
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Verify that we don't create files with long paths inside bin/obj
			AssertMaxFileLengthInBinAndObjDirectories (platform, project_path, runtimeIdentifiers, configuration);

			// a simple rebuild should succeed
			rv = DotNet.AssertBuild (project_path, properties);
			allTargets = BinLog.GetAllTargets (rv.BinLogPath);
			warnings = BinLog.GetBuildLogWarnings (rv.BinLogPath).ToArray ();
			warningMessages = BundleStructureTest.FilterWarnings (warnings, canonicalizePaths: true);

			BundleStructureTest.CheckZippedAppBundleContents (platform, zippedAppBundlePath, rids, signature, isReleaseBuild);
			AssertWarningsEqual (expectedWarnings, warningMessages, "Warnings Rebuild 3");
			AssertTargetNotExecuted (allTargets, "_CompileAppManifest", "_CompileAppManifest Rebuild 3");
			ExecuteWithMagicWordAndAssert (platform, runtimeIdentifiers, appExecutable);

			// Verify that we don't create files with long paths inside bin/obj
			AssertMaxFileLengthInBinAndObjDirectories (platform, project_path, runtimeIdentifiers, configuration);
		}

		[Category ("RemoteWindows")]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.iOS, "iossimulator-arm64;iossimulator-x64")]
		public void PluralRuntimeIdentifiersWithRemoteMac (ApplePlatform platform, string runtimeIdentifiers)
		{
			var properties = AddRemoteProperties ();
			DotNetProjectTest.PluralRuntimeIdentifiersImpl (platform, runtimeIdentifiers, properties);
		}

		static void AssertWarningsEqual (IList<string> expected, IList<string> actual, string message)
		{
			if (expected.Count == actual.Count) {
				var equal = true;
				for (var i = 0; i < actual.Count; i++) {
					if (expected [i] != actual [i]) {
						equal = false;
						break;
					}
				}
				if (equal)
					return;
			}

			var sb = new StringBuilder ();
			sb.AppendLine ($"Incorrect warnings: {message}");
			sb.AppendLine ($"Expected {expected.Count} warnings:");
			for (var i = 0; i < expected.Count; i++)
				sb.AppendLine ($"\t#{i + 1}: {expected [i]}");
			sb.AppendLine ($"Got {actual.Count} warnings:");
			for (var i = 0; i < actual.Count; i++) {
				if (i < expected.Count && actual [i] == expected [i]) {
					sb.AppendLine ($"\t#{i + 1}: {actual [i]}");
				} else {
					sb.AppendLine ($"\t!{i + 1}: {actual [i]}");
				}
			}

			Console.WriteLine (sb);
			Assert.Fail (sb.ToString ());
		}

		[Category ("RemoteWindows")]
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		public void RemoteTest (ApplePlatform platform, string runtimeIdentifiers)
		{
			var project = "MySimpleApp";
			var configuration = "Debug";

			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);
			Configuration.IgnoreIfNotOnWindows ();

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			var project_dir = Path.GetDirectoryName (project_path)!;
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);

			// Copy the app bundle to Windows so that we can inspect the results.
			properties ["CopyAppBundleToWindows"] = "true";
			// Check for updated files on the remote output and update them locally so the app is ready for debug
			properties ["KeepLocalOutputUpToDate"] = "true";
			// Don't clean the zip file with the updated files from the remote side so they can be asserted
			properties ["CleanChangedOutputFilesZipFile"] = "false";

			var result = DotNet.AssertBuild (project_path, properties, timeout: TimeSpan.FromMinutes (15));
			AssertThatLinkerExecuted (result);

			var objDir = GetObjDir (project_path, platform, runtimeIdentifiers, configuration);

			var zippedAppBundlePath = Path.Combine (objDir, "AppBundle.zip");
			Assert.That (zippedAppBundlePath, Does.Exist, "AppBundle.zip");

			// Open the zipped app bundle and get the Info.plist
			using var zip = ZipFile.OpenRead (zippedAppBundlePath);
			ZipHelpers.DumpZipFile (zip, zippedAppBundlePath);
			var infoPlistEntry = zip.Entries.SingleOrDefault (v => v.Name == "Info.plist")!;
			Assert.NotNull (infoPlistEntry, "Info.plist");

			// Parse the Info.plist
			// PDictionary.FromStream requires a seekable stream, but the zip stream isn't seekable, so copy to a
			// MemoryStream and use that. Info.plist files aren't big, so this shouldn't become a memory consumption problem.
			using var memoryStream = new MemoryStream ((int) infoPlistEntry.Length);
			using var plistStream = infoPlistEntry.Open ();
			plistStream.CopyTo (memoryStream);

			var infoPlist = (PDictionary) PDictionary.FromStream (memoryStream)!;
			Assert.AreEqual ("com.xamarin.mysimpleapp", infoPlist.GetString ("CFBundleIdentifier").Value, "CFBundleIdentifier");
			Assert.AreEqual ("MySimpleApp", infoPlist.GetString ("CFBundleDisplayName").Value, "CFBundleDisplayName");
			Assert.AreEqual ("3.14", infoPlist.GetString ("CFBundleVersion").Value, "CFBundleVersion");
			Assert.AreEqual ("3.14", infoPlist.GetString ("CFBundleShortVersionString").Value, "CFBundleShortVersionString");

			//Validate that the output assemblies report file with the list of local assemblies, lengths and MVIDs has been created
			var outputAssembliesReportFileName = "OutputAssembliesReport.txt";
			var outputAssembliesReportFile = Path.Combine (objDir, outputAssembliesReportFileName);
			Assert.That (outputAssembliesReportFile, Does.Exist, outputAssembliesReportFileName);

			//Validate that the file with the updated assemblies to replace locally has been created
			var zippedChangedOutputFilesFileName = "ChangedOutputFiles.zip";
			var zippedChangedOutputFiles = Path.Combine (objDir, zippedChangedOutputFilesFileName);
			Assert.That (zippedChangedOutputFiles, Does.Exist, zippedChangedOutputFilesFileName);

			//Create a directory in the obj to extract the updated assemblies
			var changedOutputFilesDirectory = Path.Combine (objDir, "ChangedOutputFiles");
			Directory.CreateDirectory (changedOutputFilesDirectory);

			//Extract the updated assemblies from the zip file
			using var changedOutputFilesZip = ZipFile.OpenRead (zippedChangedOutputFiles);
			ZipHelpers.DumpZipFile (changedOutputFilesZip, zippedChangedOutputFiles);
			changedOutputFilesZip.ExtractToDirectory (changedOutputFilesDirectory, overwriteFiles: true);

			//Reads the output assemblies report file
			var outputAssembliesReportFileList = GetOutputAssembliesReportFileList (outputAssembliesReportFile);
			var changedOutputAssemblies = Directory.GetFiles (changedOutputFilesDirectory, "*.dll", SearchOption.TopDirectoryOnly);

			foreach (var file in changedOutputAssemblies) {
				var fileName = Path.GetFileName (file);
				var fileInReport = outputAssembliesReportFileList.TryGetValue (fileName, out (long length, Guid mvid) localInfo);

				if (fileInReport) {
					var fileInfo = new FileInfo (file);
					using Stream stream = fileInfo.OpenRead ();
					using var peReader = new PEReader (stream);
					MetadataReader metadataReader = peReader.GetMetadataReader ();
					Guid mvid = metadataReader.GetGuid (metadataReader.GetModuleDefinition ().Mvid);
					var fileWasUpdated = fileInfo.Length != localInfo.length || mvid != localInfo.mvid;

					Assert.IsTrue (fileWasUpdated, $"The file '{fileName}' is identical to the one present in the output assemblies report file '{outputAssembliesReportFile}'");
				}
			}
		}

		IDictionary<string, (long length, Guid mvid)> GetOutputAssembliesReportFileList (string reportFile)
		{
			var reportFileList = new Dictionary<string, (long length, Guid mvid)> ();

			//Expected format of the report file lines (defined in the CalculateAssembliesReport task): Foo.dll/23189/768C814C-05C3-4563-9B53-35FEF571968E
			foreach (var line in File.ReadLines (reportFile)) {
				string [] lineParts = line.Split (["/"], StringSplitOptions.RemoveEmptyEntries);

				// Skip lines that don't match the expected format
				if (lineParts.Length == 3 && long.TryParse (lineParts [1], out long fileLength) && Guid.TryParse (lineParts [2], out Guid mvid)) {
					// Adds file name, length and MVID to the dictionary
					reportFileList.Add (lineParts [0], (fileLength, mvid));
				}
			}

			return reportFileList;
		}
	}

	public class AppBundleInfo {
		public readonly bool IsRemoteBuild;
		public readonly string AppPath;
		public readonly string ProjectPath;
		public readonly ApplePlatform Platform;
		public readonly string Configuration;
		public readonly string RuntimeIdentifiers;

		string? zippedAppBundlePath;
		string ZippedAppBundlePath {
			get {
				if (zippedAppBundlePath is null) {
					if (!IsRemoteBuild)
						throw new InvalidOperationException ($"Can't get the zipped app bundle path unless it's for a remote build.");
					var objDir = TestBaseClass.GetObjDir (ProjectPath, Platform, RuntimeIdentifiers, Configuration);
					zippedAppBundlePath = Path.Combine (objDir, "AppBundle.zip");
					Assert.That (zippedAppBundlePath, Does.Exist, "AppBundle.zip");
				}
				return zippedAppBundlePath;
			}
		}

		public AppBundleInfo (ApplePlatform platform, string appPath, string projectPath, bool isRemoteBuild, string runtimeIdentifiers, string configuration)
		{
			Platform = platform;
			AppPath = appPath;
			ProjectPath = projectPath;
			IsRemoteBuild = isRemoteBuild;
			Configuration = configuration;
			RuntimeIdentifiers = runtimeIdentifiers;
		}

		public byte [] GetFile (string appBundleRelativePath)
		{
			Assert.That (GetAppBundleFiles (), Does.Contain (appBundleRelativePath), "File does not exist in app bundle");
			if (IsRemoteBuild) {
				Console.WriteLine ($"Opening {ZippedAppBundlePath}");
				using var zip = ZipFile.OpenRead (ZippedAppBundlePath);
				var entry = zip.GetEntry (appBundleRelativePath.Replace (Path.DirectorySeparatorChar, '/'))!;
				using var stream = entry.Open ();
				using var memoryStream = new MemoryStream (stream.CanSeek ? (int) stream.Length : 4096);
				stream.CopyTo (memoryStream);
				return memoryStream.ToArray ();
			} else {
				return File.ReadAllBytes (Path.Combine (AppPath, appBundleRelativePath));
			}
		}

		public IEnumerable<string> GetAppBundleFiles (bool merged = false)
		{
			if (IsRemoteBuild) {
				return ZipHelpers.List (ZippedAppBundlePath);
			} else {
				var rv = new HashSet<string> ();

				rv.UnionWith (GetAllFilesInDirectory (AppPath));

				return rv;
			}
		}

		static IEnumerable<string> GetAllFilesInDirectory (string? directory, string subdir = "")
		{
			if (string.IsNullOrEmpty (directory))
				return Enumerable.Empty<string> ();

			if (!string.IsNullOrEmpty (subdir)) {
				var subdirs = Directory.GetDirectories (directory, subdir);
				if (subdirs.Length != 1)
					throw new InvalidOperationException ($"Found {subdirs.Length} (expected 1) subdirs for glob '{subdir}' in '{directory}': {string.Join (", ", subdirs)}");
				directory = subdirs [0];
			}

			if (!Directory.Exists (directory))
				return Array.Empty<string> ();

			return Directory
					.GetFileSystemEntries (directory, "*", SearchOption.AllDirectories)
					.Select (v => v.Substring (directory.Length + 1));
		}

		public void DumpAppBundleContents ()
		{
			Console.WriteLine ($"App bundle info:");
			Console.WriteLine ($"    IsRemoteBuild: {IsRemoteBuild}");
			Console.WriteLine ($"    AppPath: {AppPath}");
			Console.WriteLine ($"    Platform: {Platform}");
			Console.WriteLine ($"    Configuration: {Configuration}");
			Console.WriteLine ($"    RuntimeIdentifiers: {RuntimeIdentifiers}");

			var appBundleContents = GetAppBundleFiles ().OrderBy (v => v).ToArray ();
			Console.WriteLine ($"    App bundle files ({appBundleContents.Length}):");
			foreach (var abc in appBundleContents)
				Console.WriteLine ($"        {abc}");
		}
	}
}
