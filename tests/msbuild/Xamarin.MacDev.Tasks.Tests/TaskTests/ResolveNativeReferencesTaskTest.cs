using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Enumeration;
using System.Linq;
using System.Threading;
using Microsoft.Build.Experimental.ProjectCache;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks.Tests {

	[TestFixture]
	public class ResolveNativeReferencesTaskTest : TestBase {

		TaskLoggingHelper log = new TaskLoggingHelper (new TestEngine (), "ResolveNativeReferences");

		// single arch request (subset are fine)
		[TestCase (TargetFramework.DotNet_iOS_String, false, "arm64", "ios-arm64/Universal.framework/Universal", "ios-arm64/Universal.framework")]
		[TestCase (TargetFramework.DotNet_iOS_String, true, "x86_64", "ios-arm64_x86_64-simulator/Universal.framework/Universal", "ios-arm64_x86_64-simulator/Universal.framework")] // subset
		[TestCase (TargetFramework.DotNet_MacCatalyst_String, false, "x86_64", "ios-arm64_x86_64-maccatalyst/Universal.framework/Universal", "ios-arm64_x86_64-maccatalyst/Universal.framework")] // subset
		[TestCase (TargetFramework.DotNet_tvOS_String, false, "arm64", "tvos-arm64/Universal.framework/Universal", "tvos-arm64/Universal.framework")]
		[TestCase (TargetFramework.DotNet_tvOS_String, true, "x86_64", "tvos-arm64_x86_64-simulator/Universal.framework/Universal", "tvos-arm64_x86_64-simulator/Universal.framework")] // subset
		[TestCase (TargetFramework.DotNet_macOS_String, false, "x86_64", "macos-arm64_x86_64/Universal.framework/Universal", "macos-arm64_x86_64/Universal.framework")] // subset

		// multiple arch request (all must be present)
		[TestCase (TargetFramework.DotNet_macOS_String, false, "x86_64, arm64", "macos-arm64_x86_64/Universal.framework/Universal", "macos-arm64_x86_64/Universal.framework")]

		// failure to resolve requested architecture
		[TestCase (TargetFramework.DotNet_iOS_String, true, "i386, x86_64", null, null)] // i386 not available

		// failure to resolve mismatched variant
		[TestCase (TargetFramework.DotNet_macOS_String, true, "x86_64", null, null)] // simulator not available on macOS
		public void Xcode12_x (string targetFrameworkMoniker, bool isSimulator, string architecture, string expected, string expectedNativeRelativePath)
		{
			// on Xcode 12.2+ you get arm64 for all (iOS, tvOS) simulators
			var path = Path.Combine (Path.GetDirectoryName (GetType ().Assembly.Location)!, "Resources", "xcf-xcode12.2.plist");
			var plist = PDictionary.FromFile (path)!;
			var result = ResolveNativeReferences.TryResolveXCFramework (log, plist, "N/A", targetFrameworkMoniker, isSimulator, architecture, null, out var frameworkPath, out var nativeRelativePath);
			Assert.AreEqual (result, !string.IsNullOrEmpty (expected), "result");
			Assert.That (frameworkPath, Is.EqualTo (expected), "frameworkPath");
			Assert.That (nativeRelativePath, Is.EqualTo (expectedNativeRelativePath), "frameworkPath");
		}

		[TestCase (TargetFramework.DotNet_iOS_String, false, "ARMv7", "ios-arm64_armv7_armv7s/XTest.framework/XTest", "ios-arm64_armv7_armv7s/XTest.framework")]
		public void PreXcode12 (string targetFrameworkMoniker, bool isSimulator, string architecture, string expected, string expectedNativeRelativePath)
		{
			var path = Path.Combine (Path.GetDirectoryName (GetType ().Assembly.Location)!, "Resources", "xcf-prexcode12.plist");
			var plist = PDictionary.FromFile (path)!;
			var result = ResolveNativeReferences.TryResolveXCFramework (log, plist, "N/A", targetFrameworkMoniker, isSimulator, architecture, null, out var frameworkPath, out var nativeRelativePath);
			Assert.AreEqual (result, !string.IsNullOrEmpty (expected), "result");
			Assert.That (frameworkPath, Is.EqualTo (expected), "frameworkPath");
			Assert.That (nativeRelativePath, Is.EqualTo (expectedNativeRelativePath), "frameworkPath");
		}

		[Test]
		public void BadInfoPlist ()
		{
			var plist = new PDictionary ();
			var result = ResolveNativeReferences.TryResolveXCFramework (log, plist, "N/A", TargetFramework.DotNet_iOS_String, false, "x86_64", null, out var frameworkPath, out var nativeRelativePath);
			Assert.IsFalse (result, "Invalid Info.plist");
		}

		[TestCase (ApplePlatform.iOS, false)]
		[TestCase (ApplePlatform.iOS, true)]
		[TestCase (ApplePlatform.MacOSX, false)]
		[TestCase (ApplePlatform.TVOS, false)]
		[TestCase (ApplePlatform.TVOS, true)]
		[TestCase (ApplePlatform.MacCatalyst, false)]
		public void ExtractedPath (ApplePlatform platform, bool useSystemIOCompression)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var tmpdir = Cache.CreateTemporaryDirectory ();

			var item = new TaskItem (Path.Combine (Configuration.RootPath, "tests", "test-libraries", ".libs", "XTest.xcframework.zip"));
			item.SetMetadata ("Kind", "Framework");

			var task = CreateTask<ResolveNativeReferences> ();
			task.Architectures = "ARM64";
			switch (platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				task.FrameworksDirectory = "";
				break;
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				task.FrameworksDirectory = "Contents/Frameworks/";
				break;
			default:
				throw new NotSupportedException ($"Unsupported platform: {platform}");
			}
			task.IntermediateOutputPath = tmpdir;
			task.NativeReferences = new TaskItem [] {
				item,
			};
			task.SdkIsSimulator = false;
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();

			var originalSystemIOCompression = Environment.GetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION");
			if (useSystemIOCompression)
				Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", "1");

			try {
				Assert.IsTrue (task.Execute (), "Execute");

				var expectedFiles = new List<string> () {
					Path.Combine ("XTest.xcframework.zip"),
					Path.Combine ("XTest.xcframework.zip", "XTest.framework"),
					Path.Combine ("XTest.xcframework.zip", "XTest.framework", "XTest"),
					Path.Combine ("XTest.xcframework.zip", "XTest.framework.stamp"),
				};
				switch (platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Info.plist"));
					break;
				case ApplePlatform.MacCatalyst:
				case ApplePlatform.MacOSX:
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Resources"));
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Versions"));
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Versions", "A"));
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Versions", "A", "Resources"));
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Versions", "A", "Resources", "Info.plist"));
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Versions", "A", "XTest"));
					expectedFiles.Add (Path.Combine ("XTest.xcframework.zip", "XTest.framework", "Versions", "Current"));
					break;
				default:
					throw new NotSupportedException ($"Unsupported platform: {platform}");
				}

				var files = new FileSystemEnumerable<string> (
					directory: tmpdir,
					transform: (ref FileSystemEntry entry) => entry.ToFullPath (),
					options: new EnumerationOptions {
						RecurseSubdirectories = true,
					}) {
					ShouldRecursePredicate = (ref FileSystemEntry entry) => {
						return entry.ToFileSystemInfo ().LinkTarget is null;
					}
				}
				.Select (v => v [(tmpdir.Length + 1)..])
				.OrderBy (v => v)
				.ToArray ();

				var expectedFilesSorted = expectedFiles.OrderBy (v => v).ToArray ();

				Assert.That (files, Is.EqualTo (expectedFilesSorted), "Unzipped files");
			} finally {
				if (useSystemIOCompression)
					Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", originalSystemIOCompression);
			}
		}

		[TestCase (ApplePlatform.iOS, false)]
		[TestCase (ApplePlatform.iOS, true)]
		[TestCase (ApplePlatform.MacOSX, false)]
		[TestCase (ApplePlatform.TVOS, false)]
		[TestCase (ApplePlatform.TVOS, true)]
		[TestCase (ApplePlatform.MacCatalyst, false)]
		public void ExtractedPath2 (ApplePlatform platform, bool useSystemIOCompression)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var tmpdir = Cache.CreateTemporaryDirectory ();
			var inputdir = Path.Combine (tmpdir, "input");
			var outputdir = Path.Combine (tmpdir, "output");

			var dll = Path.Combine (inputdir, "BindingWithCompressedXCFramework.dll");
			var sidecar = Path.Combine (inputdir, "BindingWithCompressedXCFramework.resources");
			Directory.CreateDirectory (sidecar);
			var manifest =
			$"""
			<BindingAssembly>
				<NativeReference Name="XTest.xcframework.zip">
					<ForceLoad></ForceLoad>
					<Frameworks></Frameworks>
					<IdentityWithoutPathSeparatorSuffix>../../../test-libraries/.libs/XTest.xcframework.zip</IdentityWithoutPathSeparatorSuffix>
					<IsCxx></IsCxx>
					<Kind>Framework</Kind>
					<LinkerFlags></LinkerFlags>
					<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
					<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
					<SmartLink></SmartLink>
					<WeakFrameworks></WeakFrameworks>
				</NativeReference>
				<NativeReference Name="XStaticArTest.xcframework.zip">
					<ForceLoad></ForceLoad>
					<Frameworks></Frameworks>
					<IdentityWithoutPathSeparatorSuffix>../../../test-libraries/.libs/XStaticArTest.xcframework.zip</IdentityWithoutPathSeparatorSuffix>
					<IsCxx></IsCxx>
					<Kind>Static</Kind>
					<LinkerFlags></LinkerFlags>
					<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
					<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
					<SmartLink></SmartLink>
					<WeakFrameworks></WeakFrameworks>
				</NativeReference>
				<NativeReference Name="XStaticObjectTest.xcframework.zip">
					<ForceLoad></ForceLoad>
					<Frameworks></Frameworks>
					<IdentityWithoutPathSeparatorSuffix>../../../test-libraries/.libs/XStaticObjectTest.xcframework.zip</IdentityWithoutPathSeparatorSuffix>
					<IsCxx></IsCxx>
					<Kind>Static</Kind>
					<LinkerFlags></LinkerFlags>
					<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
					<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
					<SmartLink></SmartLink>
					<WeakFrameworks></WeakFrameworks>
				</NativeReference>
			</BindingAssembly>
			""";
			File.WriteAllText (Path.Combine (sidecar, "manifest"), manifest);
			File.Copy (Path.Combine (Configuration.RootPath, "tests", "test-libraries", ".libs", "XTest.xcframework.zip"), Path.Combine (sidecar, "XTest.xcframework.zip"));
			File.Copy (Path.Combine (Configuration.RootPath, "tests", "test-libraries", ".libs", "XStaticArTest.xcframework.zip"), Path.Combine (sidecar, "XStaticArTest.xcframework.zip"));
			File.Copy (Path.Combine (Configuration.RootPath, "tests", "test-libraries", ".libs", "XStaticObjectTest.xcframework.zip"), Path.Combine (sidecar, "XStaticObjectTest.xcframework.zip"));

			var item = new TaskItem (dll);

			var task = CreateTask<ResolveNativeReferences> ();
			task.Architectures = "ARM64";
			switch (platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				task.FrameworksDirectory = "";
				break;
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				task.FrameworksDirectory = "Contents/Frameworks/";
				break;
			default:
				throw new NotSupportedException ($"Unsupported platform: {platform}");
			}
			task.IntermediateOutputPath = outputdir;
			task.References = new TaskItem [] {
				item,
			};
			task.SdkIsSimulator = false;
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();

			var originalSystemIOCompression = Environment.GetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION");
			if (useSystemIOCompression)
				Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", "1");

			try {
				Assert.IsTrue (task.Execute (), "Execute");

				var expectedFiles = new List<string> ();
				switch (platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "XStaticArTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "XStaticObjectTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Info.plist"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "XTest"));
					break;
				case ApplePlatform.MacCatalyst:
				case ApplePlatform.MacOSX:
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "XStaticArTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "XStaticObjectTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A", "Resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A", "Resources", "Info.plist"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A", "XTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "Current"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "XTest"));
					break;
				default:
					throw new NotSupportedException ($"Unsupported platform: {platform}");
				}

				var files = new FileSystemEnumerable<string> (
					directory: task.IntermediateOutputPath,
					transform: (ref FileSystemEntry entry) => entry.ToFullPath (),
					options: new EnumerationOptions {
						RecurseSubdirectories = true,
					}) {
					ShouldRecursePredicate = (ref FileSystemEntry entry) => {
						return entry.ToFileSystemInfo ().LinkTarget is null;
					}
				}
				.Select (v => v [(task.IntermediateOutputPath.Length + 1)..])
				.OrderBy (v => v)
				.ToArray ();

				var expectedFilesSorted = expectedFiles.OrderBy (v => v).ToArray ();

				Assert.That (files, Is.EqualTo (expectedFilesSorted), "Unzipped files");
			} finally {
				if (useSystemIOCompression)
					Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", originalSystemIOCompression);
			}
		}

		static void AddFileToZip (ZipArchive archive, string pathInZip, string contents)
		{
			var entry = archive.CreateEntry (pathInZip);
			using var entryStream = entry.Open ();
			using var writer = new StreamWriter (entryStream);
			writer.Write (contents);
		}

		static void StuffZipWithFiles (string zipFile)
		{
			using var stream = File.Open (zipFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			using var archive = new ZipArchive (stream, ZipArchiveMode.Update);
			var manifests = archive.Entries.Where (v => {
				if (v.Name != "Info.plist")
					return false;
				var dir = Path.GetDirectoryName (v.FullName);
				if (dir?.EndsWith (".xcframework", StringComparison.Ordinal) != true)
					return false;

				if (Path.GetFileName (dir) != dir)
					return false; // inside an unexpected subdirectory somewhere

				return true;
			}).ToArray ();

			var frameworks = new List<(string Path, string Platform)> ();
			foreach (var manifest in manifests) {
				using var manifestStream = manifest.Open ();
				var dict = (PDictionary) PDictionary.FromStream (manifestStream)!;
				var availableLibraries = dict.Get<PArray> ("AvailableLibraries")!;
				foreach (PDictionary lib in availableLibraries) {
					var libraryIdentifier = (string) lib.GetString ("LibraryIdentifier")!;
					var libraryPath = (string) lib.GetString ("LibraryPath")!;
					var platform = (string) lib.GetString ("SupportedPlatform")!;
					var platformVariant = (string) lib.GetString ("SupportedPlatformVariant")!;

					if (platformVariant == "maccatalyst")
						platform = platformVariant;

					frameworks.Add ((Path.Combine (Path.GetDirectoryName (manifest.FullName)!, libraryIdentifier, libraryPath), platform));
				}
			}

			foreach (var (path, platform) in frameworks) {
				var isDesktop = platform == "macos" || platform == "maccatalyst";
				var fwName = Path.GetFileNameWithoutExtension (path);
				var architectures = AfterFirst (Path.GetFileName (Path.GetDirectoryName (path)!)!, '-').Split ('_');
				string infix = "";
				var resourceInfix = "";
				if (isDesktop) {
					infix = Path.Combine ("Versions", "A");
					resourceInfix = "Resources";
				}
				AddFileToZip (archive, Path.Combine (path, infix, "Headers", "MyHeader.h"), "// myheader");
				AddFileToZip (archive, Path.Combine (path, infix, "PrivateHeaders", "MyPrivateHeader.h"), "// myprivateheader");
				AddFileToZip (archive, Path.Combine (path, infix, resourceInfix, "PrivacyInfo.xcprivacy"), "<!-- my privacy info -->");
				AddFileToZip (archive, Path.Combine (path, infix, "Modules", "module.modulemap"), "// modulemap");
				foreach (var arch in architectures)
					AddFileToZip (archive, Path.Combine (path, infix, "Modules", fwName + ".swiftmodule", $"{arch}-{platform}.swiftinterface"), "// swiftinterface");
				AddFileToZip (archive, Path.Combine (path, infix, "dSYMs", fwName + ".dSYM", "Contents", "Resources", "DWARF", fwName), "// dsym");
			}
		}

		static string AfterFirst (string value, char needle)
		{
			var idx = value.IndexOf (needle);
			if (idx == -1)
				return value;
			return value [(idx + 1)..];
		}
	
		[TestCase (ApplePlatform.iOS, false)]
		[TestCase (ApplePlatform.iOS, true)]
		[TestCase (ApplePlatform.MacOSX, false)]
		[TestCase (ApplePlatform.TVOS, false)]
		[TestCase (ApplePlatform.TVOS, true)]
		[TestCase (ApplePlatform.MacCatalyst, false)]
		public void FilteredExtraction (ApplePlatform platform, bool useSystemIOCompression)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var tmpdir = Cache.CreateTemporaryDirectory ();
			var inputdir = Path.Combine (tmpdir, "input");
			var outputdir = Path.Combine (tmpdir, "output");

			var dll = Path.Combine (inputdir, "BindingWithCompressedXCFramework.dll");
			var sidecar = Path.Combine (inputdir, "BindingWithCompressedXCFramework.resources");
			Directory.CreateDirectory (sidecar);
			var manifest =
			$"""
			<BindingAssembly>
				<NativeReference Name="XTest.xcframework.zip">
					<ForceLoad></ForceLoad>
					<Frameworks></Frameworks>
					<IdentityWithoutPathSeparatorSuffix>../../../test-libraries/.libs/XTest.xcframework.zip</IdentityWithoutPathSeparatorSuffix>
					<IsCxx></IsCxx>
					<Kind>Framework</Kind>
					<LinkerFlags></LinkerFlags>
					<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
					<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
					<SmartLink></SmartLink>
					<WeakFrameworks></WeakFrameworks>
				</NativeReference>
				<NativeReference Name="XStaticArTest.xcframework.zip">
					<ForceLoad></ForceLoad>
					<Frameworks></Frameworks>
					<IdentityWithoutPathSeparatorSuffix>../../../test-libraries/.libs/XStaticArTest.xcframework.zip</IdentityWithoutPathSeparatorSuffix>
					<IsCxx></IsCxx>
					<Kind>Static</Kind>
					<LinkerFlags></LinkerFlags>
					<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
					<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
					<SmartLink></SmartLink>
					<WeakFrameworks></WeakFrameworks>
				</NativeReference>
				<NativeReference Name="XStaticObjectTest.xcframework.zip">
					<ForceLoad></ForceLoad>
					<Frameworks></Frameworks>
					<IdentityWithoutPathSeparatorSuffix>../../../test-libraries/.libs/XStaticObjectTest.xcframework.zip</IdentityWithoutPathSeparatorSuffix>
					<IsCxx></IsCxx>
					<Kind>Static</Kind>
					<LinkerFlags></LinkerFlags>
					<LinkWithSwiftSystemLibraries></LinkWithSwiftSystemLibraries>
					<NeedsGccExceptionHandling></NeedsGccExceptionHandling>
					<SmartLink></SmartLink>
					<WeakFrameworks></WeakFrameworks>
				</NativeReference>
			</BindingAssembly>
			""";
			File.WriteAllText (Path.Combine (sidecar, "manifest"), manifest);

			var XTestFrameworkZipPath = Path.Combine (sidecar, "XTest.xcframework.zip");
			File.Copy (Path.Combine (Configuration.RootPath, "tests", "test-libraries", ".libs", "XTest.xcframework.zip"), XTestFrameworkZipPath);
			StuffZipWithFiles (XTestFrameworkZipPath);

			var XStaticArTestFrameworkZipPath = Path.Combine (sidecar, "XStaticArTest.xcframework.zip");
			File.Copy (Path.Combine (Configuration.RootPath, "tests", "test-libraries", ".libs", "XStaticArTest.xcframework.zip"), XStaticArTestFrameworkZipPath);
			StuffZipWithFiles (XStaticArTestFrameworkZipPath);

			var XStaticObjectTestFrameworkZipPath = Path.Combine (sidecar, "XStaticObjectTest.xcframework.zip");
			File.Copy (Path.Combine (Configuration.RootPath, "tests", "test-libraries", ".libs", "XStaticObjectTest.xcframework.zip"), XStaticObjectTestFrameworkZipPath);
			StuffZipWithFiles (XStaticObjectTestFrameworkZipPath);

			var item = new TaskItem (dll);

			var task = CreateTask<ResolveNativeReferences> ();
			task.Architectures = "ARM64";
			switch (platform) {
			case ApplePlatform.iOS:
			case ApplePlatform.TVOS:
				task.FrameworksDirectory = "";
				break;
			case ApplePlatform.MacCatalyst:
			case ApplePlatform.MacOSX:
				task.FrameworksDirectory = "Contents/Frameworks/";
				break;
			default:
				throw new NotSupportedException ($"Unsupported platform: {platform}");
			}
			task.IntermediateOutputPath = outputdir;
			task.References = new TaskItem [] {
				item,
			};
			task.SdkIsSimulator = false;
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();
			task.ExtractionFilters = new [] {
				new TaskItem (".*/Headers/.*"),
				new TaskItem ("Modules/.*"),
				new TaskItem ("dSYMs/.*"),
				new TaskItem ("PrivateHeaders/.*"),
				new TaskItem ("PrivateHeaders/.*"),
			};

			var originalSystemIOCompression = Environment.GetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION");
			if (useSystemIOCompression)
				Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", "1");

			try {
				Assert.IsTrue (task.Execute (), "Execute");

				var expectedFiles = new List<string> ();
				switch (platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "XStaticArTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "PrivacyInfo.xcprivacy"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "XStaticObjectTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "PrivacyInfo.xcprivacy"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Info.plist"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "PrivacyInfo.xcprivacy"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "XTest"));
					break;
				case ApplePlatform.MacCatalyst:
				case ApplePlatform.MacOSX:
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "XStaticArTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "Versions"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "Versions", "A"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "Versions", "A", "Resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticArTest.framework", "Versions", "A", "Resources", "PrivacyInfo.xcprivacy"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "XStaticObjectTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "Versions"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "Versions", "A"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "Versions", "A", "Resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XStaticObjectTest.framework", "Versions", "A", "Resources", "PrivacyInfo.xcprivacy"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework.stamp"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A", "Resources"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A", "Resources", "Info.plist"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A", "Resources", "PrivacyInfo.xcprivacy"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "A", "XTest"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "Versions", "Current"));
					expectedFiles.Add (Path.Combine ("BindingWithCompressedXCFramework.resources", "XTest.framework", "XTest"));
					break;
				default:
					throw new NotSupportedException ($"Unsupported platform: {platform}");
				}

				// Get all extracted files, but don't recurse into directories that are symlinks
				var files = new FileSystemEnumerable<string> (
					directory: task.IntermediateOutputPath,
					transform: (ref FileSystemEntry entry) => entry.ToFullPath (),
					options: new EnumerationOptions {
						RecurseSubdirectories = true,
					}) {
					ShouldRecursePredicate = (ref FileSystemEntry entry) => {
						return entry.ToFileSystemInfo ().LinkTarget is null;
					}
				}
				.Select (v => v [(task.IntermediateOutputPath.Length + 1)..])
				.OrderBy (v => v)
				.ToArray ();

				var expectedFilesSorted = expectedFiles.OrderBy (v => v).ToArray ();

				Assert.That (files, Is.EqualTo (expectedFilesSorted), "Unzipped files");
			} finally {
				if (useSystemIOCompression)
					Environment.SetEnvironmentVariable ("XAMARIN_USE_SYSTEM_IO_COMPRESSION", originalSystemIOCompression);
			}
		}

	}
}
