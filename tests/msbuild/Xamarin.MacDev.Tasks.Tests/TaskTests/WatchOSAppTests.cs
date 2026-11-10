// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

using Microsoft.Build.Utilities;
using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class WatchOSAppTests : TestBase {
		[TestCase ("Watch.app/Info.plist", "", true)]
		[TestCase ("ios-arm64/Watch.app/Info.plist", "ios-arm64", true)]
		[TestCase ("iossimulator-arm64/Watch.app/Info.plist", "", false)]
		[TestCase ("ios-arm64/Watch.app/../outside", "", false)]
		[TestCase ("ios-arm64/Watch.app/../../outside", "", false)]
		[TestCase ("ios-arm64/Watch.app\\outside", "", false)]
		[TestCase ("ios-arm64/Watch.app/C:/outside", "", false)]
		[TestCase ("/ios-arm64/Watch.app/Info.plist", "", false)]
		[TestCase ("ios-arm64/unexpected.txt", "", false)]
		public void ZipSelection (string entryName, string expectedResource, bool expectedResult)
		{
			using var stream = new MemoryStream ();
			using var archive = new ZipArchive (stream, ZipArchiveMode.Create, true);
			archive.CreateEntry (entryName);
			archive.Dispose ();
			stream.Position = 0;
			using var input = new ZipArchive (stream, ZipArchiveMode.Read);
			var result = PrepareWatchOSApp.TrySelectZipResource (input, "ios-arm64", out var resource);
			Assert.That (result, Is.EqualTo (expectedResult), "selection");
			if (expectedResult)
				Assert.That (resource, Is.EqualTo (expectedResource), "selected resource");
		}

		[Test]
		public void SelectiveExtractionWithoutDirectoryEntries ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var path = Path.Combine (directory, "watch.zip");
			using (var zip = ZipFile.Open (path, ZipArchiveMode.Create)) {
				foreach (var rid in new [] { "ios-arm64", "iossimulator-arm64" }) {
					using var writer = new StreamWriter (zip.CreateEntry (rid + "/Watch.app/Info.plist").Open ());
					writer.Write (rid);
				}
			}
			var task = CreateTask<PrepareWatchOSApp> ();
			var files = new System.Collections.Generic.List<string> ();
			Assert.That (CompressionHelper.TryDecompress (task, path, "iossimulator-arm64", Path.Combine (directory, "out"), files, CancellationToken.None, out var extracted), Is.True, "extraction");
			var selectedDirectory = Path.Combine (directory, "out", "iossimulator-arm64");
			Assert.That (extracted, Is.EqualTo (selectedDirectory), "selected directory");
			Assert.That (File.ReadAllText (Path.Combine (selectedDirectory, "Watch.app", "Info.plist")), Is.EqualTo ("iossimulator-arm64"), "variant");
			Assert.That (Directory.Exists (Path.Combine (directory, "out", "ios-arm64")), Is.False, "unselected variant");
		}

		[TestCase ("../../outside")]
		[TestCase ("/outside")]
		public void RejectEscapingZipSymlink (string target)
		{
			using var stream = new MemoryStream ();
			using (var archive = new ZipArchive (stream, ZipArchiveMode.Create, true)) {
				var entry = archive.CreateEntry ("ios-arm64/Watch.app/link");
				entry.ExternalAttributes = unchecked ((int) 0xa1ff0000);
				using var writer = new StreamWriter (entry.Open ());
				writer.Write (target);
			}
			stream.Position = 0;
			using var input = new ZipArchive (stream, ZipArchiveMode.Read);
			Assert.That (PrepareWatchOSApp.TrySelectZipResource (input, "ios-arm64", out _), Is.False);
		}

		[Test]
		public void RemovingWatchItemRemovesPreviousBundle ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<PrepareWatchOSApp> ();
			task.AppBundleDir = Path.Combine (directory, "Host.app");
			task.IntermediateOutputPath = Path.Combine (directory, "obj");
			task.RuntimeIdentifier = "iossimulator-arm64";
			var embedded = Path.Combine (task.AppBundleDir, "Watch", "Old.app");
			Directory.CreateDirectory (embedded);
			Directory.CreateDirectory (task.IntermediateOutputPath);
			File.WriteAllText (Path.Combine (task.IntermediateOutputPath, "embedded-app.txt"), "Old.app");
			var previousTimestamp = DateTime.UtcNow.AddMinutes (-1);
			Directory.SetLastWriteTimeUtc (task.AppBundleDir, previousTimestamp);
			ExecuteTask (task);
			Assert.That (Directory.Exists (embedded), Is.False, "removed bundle");
			Assert.That (Directory.GetLastWriteTimeUtc (task.AppBundleDir), Is.GreaterThan (previousTimestamp), "host signing invalidated");
		}

		[Test]
		[TestCase (false)]
		[TestCase (true)]
		public void InvalidSigningEntries (bool duplicate)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<PrepareWatchOSApp> ();
			task.AppBundleDir = Path.Combine (directory, "Host.app");
			task.IntermediateOutputPath = Path.Combine (directory, "obj");
			task.RuntimeIdentifier = "iossimulator-arm64";
			task.Signing = Enumerable.Range (0, duplicate ? 2 : 1).Select (_ => new TaskItem ("com.example.missing")).ToArray ();
			ExecuteTask (task, expectedErrorCount: 1);
			Assert.That (Engine.Logger.ErrorEvents.Single ().Code, Is.EqualTo (duplicate ? "MT7213" : "MT7212"), "diagnostic code");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void RejectX64Simulator (bool zip)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			if (zip) {
				var task = CreateTask<CreateWatchOSAppZip> ();
				task.RuntimeIdentifiers = new [] { "iossimulator-x64" };
				task.ProjectPath = Path.Combine (directory, "WatchApp.xcodeproj");
				task.IntermediateOutputPath = Path.Combine (directory, "obj");
				task.ZipPath = Path.Combine (directory, "watch.zip");
				ExecuteTask (task, expectedErrorCount: 1);
			} else {
				var task = CreateTask<BuildWatchOSApp> ();
				task.RuntimeIdentifier = "iossimulator-x64";
				task.ProjectPath = Path.Combine (directory, "WatchApp.xcodeproj");
				ExecuteTask (task, expectedErrorCount: 1);
			}
			Assert.That (Engine.Logger.ErrorEvents.Single ().Code, Is.EqualTo ("MT7205"), "unsupported runtime diagnostic");
		}

		[Test]
		public void NativeZipBuildAndEmbedding ()
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var create = CreateTask<CreateWatchOSAppZip> ();
			create.ProjectPath = Path.Combine (Configuration.TestProjectsDirectory, "Xcode", "WatchApp", "WatchApp.xcodeproj");
			create.IntermediateOutputPath = Path.Combine (directory, "build");
			create.ZipPath = Path.Combine (directory, "watch.zip");
			Assert.That (create.RuntimeIdentifiers, Is.EquivalentTo (new [] { "ios-arm64", "iossimulator-arm64" }), "default runtime identifiers");
			ExecuteTask (create);
			foreach (var rid in create.RuntimeIdentifiers) {
				var prepare = CreateTask<PrepareWatchOSApp> ();
				prepare.Apps = new [] { new TaskItem (create.ZipPath) };
				prepare.AppBundleDir = Path.Combine (directory, rid, "Host.app");
				prepare.IntermediateOutputPath = Path.Combine (directory, rid, "obj");
				prepare.RuntimeIdentifier = rid;
				prepare.BundleIdentifier = "com.xamarin.NativeWatchApp";
				prepare.EnableCodeSigning = false;
				ExecuteTask (prepare);
				var embedded = Path.Combine (prepare.AppBundleDir, "Watch", "WatchApp.app");
				Assert.That (File.Exists (Path.Combine (embedded, "WatchApp")), Is.True, rid + ": executable");
				Assert.That (prepare.CodesignBundles.Select (v => v.ItemSpec), Does.Contain (Path.Combine ("Host.app", "Watch", "WatchApp.app")), rid + ": signing registration");
				Assert.That (Directory.GetDirectories (Path.Combine (prepare.IntermediateOutputPath, "extracted")).Select (Path.GetFileName), Is.EquivalentTo (new [] { rid }), rid + ": extracted variants");
				var stale = Path.Combine (embedded, "stale.txt");
				File.WriteAllText (stale, "");
				ExecuteTask (prepare);
				Assert.That (File.Exists (stale), Is.True, rid + ": unchanged input preserves the staged bundle");
				File.SetLastWriteTimeUtc (create.ZipPath, File.GetLastWriteTimeUtc (create.ZipPath).AddSeconds (1));
				ExecuteTask (prepare);
				Assert.That (File.Exists (stale), Is.False, rid + ": stale content removed");
				Assert.That (File.Exists (Path.Combine (create.IntermediateOutputPath, rid, "artifact", "WatchApp.app", "WatchApp")), Is.True, rid + ": source preserved");
				if (rid.StartsWith ("iossimulator-", StringComparison.Ordinal)) {
					prepare.EnableCodeSigning = true;
					prepare.SigningKey = "-";
					prepare.CodesignAllocate = Path.Combine (Configuration.XcodeLocation, "Toolchains", "XcodeDefault.xctoolchain", "usr", "bin", "codesign_allocate");
					ExecuteTask (prepare);
					var compute = CreateTask<ComputeCodesignItems> ();
					compute.AppBundleDir = prepare.AppBundleDir;
					compute.CodesignBundle = prepare.CodesignBundles;
					compute.CodesignStampPath = Path.Combine (prepare.IntermediateOutputPath, "codesign");
					compute.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
					ExecuteTask (compute);
					var sign = CreateTask<Codesign> ();
					sign.TargetFrameworkMoniker = TargetFramework.DotNet_iOS_String;
					sign.Resources = compute.OutputCodesignItems;
					ExecuteTask (sign);
					var stamp = prepare.CodesignBundles.Single ().GetMetadata ("CodesignStampFile");
					Assert.That (File.Exists (stamp), Is.True, rid + ": signing stamp");
					Assert.That (Directory.Exists (Path.Combine (embedded, "_CodeSignature")), Is.True, rid + ": signed bundle");
					ExecuteTask (prepare);
					Assert.That (File.Exists (stamp), Is.True, rid + ": unchanged input preserves signing");
					File.SetLastWriteTimeUtc (create.ZipPath, File.GetLastWriteTimeUtc (create.ZipPath).AddSeconds (1));
					ExecuteTask (prepare);
					Assert.That (File.Exists (stamp), Is.False, rid + ": replacing the signed bundle invalidates signing");
					Assert.That (Directory.Exists (Path.Combine (create.IntermediateOutputPath, rid, "artifact", "WatchApp.app", "_CodeSignature")), Is.False, rid + ": source remains unsigned");
				}
			}
		}
	}
}
