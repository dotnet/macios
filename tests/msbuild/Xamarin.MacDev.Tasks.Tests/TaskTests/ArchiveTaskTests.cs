// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ArchiveTaskTests : TestBase {
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacOSX)]
		[TestCase (ApplePlatform.MacCatalyst)]
		public void AppExtension (ApplePlatform platform)
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var appBundle = Path.Combine (tmp, "Test.app");
			var contents = platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst ? "Contents" : "";
			var appContents = Path.Combine (appBundle, contents);
			Directory.CreateDirectory (appContents);
			if (contents.Length > 0)
				Directory.CreateDirectory (Path.Combine (appContents, "MonoBundle"));

			var appManifest = new PDictionary {
				{ "CFBundleIdentifier", new PString ("com.microsoft.archive-test") },
				{ "CFBundleName", new PString ("Test") },
			};
			appManifest.Save (Path.Combine (appContents, "Info.plist"));

			var extensionBundle = Path.Combine (appContents, "PlugIns", "ShareExtension Mac.appex");
			var extensionContents = Path.Combine (extensionBundle, contents);
			Directory.CreateDirectory (extensionContents);
			var extensionManifest = new PDictionary {
				{ "CFBundleIdentifier", new PString ("com.microsoft.archive-test.share") },
			};
			extensionManifest.Save (Path.Combine (extensionContents, "Info.plist"));

			var dsymName = Path.GetFileName (extensionBundle) + ".dSYM";
			var dsymDir = Path.Combine (tmp, dsymName);
			Directory.CreateDirectory (dsymDir);
			File.WriteAllText (Path.Combine (dsymDir, "symbols"), "dSYM contents");

			var msymDir = extensionBundle + ".mSYM";
			Directory.CreateDirectory (msymDir);
			File.WriteAllText (Path.Combine (msymDir, "symbols"), "mSYM contents");

			var task = CreateTask<Archive> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();
			task.AppBundleDir = new TaskItem (appBundle);
			task.AppExtensionReferences = [new TaskItem (extensionBundle)];
			task.CustomBundleName = "MonoBundle";
			task.OutputPath = tmp;
			task.ProjectName = "Test";
			task.ArchiveDir = Path.Combine (tmp, "Test.xcarchive");

			ExecuteTask (task);

			var archivedApp = Path.Combine (task.ArchiveDir, "Products", "Applications", "Test.app", contents);
			Assert.That (Path.Combine (task.ArchiveDir, "Info.plist"), Does.Exist, "Archive manifest");
			Assert.That (Path.Combine (archivedApp, "PlugIns", Path.GetFileName (extensionBundle), contents, "Info.plist"), Does.Exist, "Archived extension manifest");
			Assert.That (File.ReadAllText (Path.Combine (task.ArchiveDir, "dSYMs", dsymName, "symbols")), Is.EqualTo ("dSYM contents"), "Extension dSYM");
			Assert.That (File.ReadAllText (Path.Combine (task.ArchiveDir, "mSYMs", Path.GetFileName (msymDir), "symbols")), Is.EqualTo ("mSYM contents"), "Extension mSYM");
		}
	}
}
