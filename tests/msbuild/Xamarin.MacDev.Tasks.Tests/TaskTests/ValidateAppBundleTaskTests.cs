// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.IO;

using NUnit.Framework;

using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ValidateAppBundleTaskTests : TestBase {
		[TestCase ("valid", 0, ApplePlatform.iOS)]
		[TestCase ("missing-identifier", 1, ApplePlatform.iOS)]
		[TestCase ("both", 1, ApplePlatform.iOS)]
		[TestCase ("valid", 0, ApplePlatform.MacCatalyst)]
		[TestCase ("missing-identifier", 1, ApplePlatform.MacCatalyst)]
		[TestCase ("both", 1, ApplePlatform.MacCatalyst)]
		public void MixedExtensions (string kind, int expectedErrors, ApplePlatform platform)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var app = Path.Combine (directory, "Container.app");
			var contents = platform == ApplePlatform.MacCatalyst ? "Contents" : "";
			Directory.CreateDirectory (Path.Combine (app, contents));
			var plist = CreateManifest ("com.example.container", "Container");
			plist ["CFBundleSupportedPlatforms"] = new PArray { new PString ("iPhoneSimulator") };
			plist ["UIDeviceFamily"] = new PArray { new PNumber (1), new PNumber (2) };
			plist.Save (Path.Combine (app, contents, "Info.plist"));

			var legacy = Path.Combine (app, contents, "PlugIns", "Legacy.appex");
			Directory.CreateDirectory (Path.Combine (legacy, contents));
			plist = CreateManifest ("com.example.container.legacy", "Legacy");
			plist ["NSExtension"] = new PDictionary {
				{ "NSExtensionPointIdentifier", new PString ("com.apple.intents-service") },
			};
			plist.Save (Path.Combine (legacy, contents, "Info.plist"));

			var modern = Path.Combine (app, contents, "Extensions", "Modern.appex");
			Directory.CreateDirectory (Path.Combine (modern, contents));
			plist = CreateManifest ("com.example.container.modern", "Modern");
			var attributes = new PDictionary ();
			if (kind != "missing-identifier")
				attributes ["EXExtensionPointIdentifier"] = "com.example.future-extension-point";
			plist ["EXAppExtensionAttributes"] = attributes;
			if (kind == "both")
				plist ["NSExtension"] = new PDictionary ();
			plist.Save (Path.Combine (modern, contents, "Info.plist"));

			var task = CreateTask<ValidateAppBundleTask> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();
			task.AppBundlePath = app;
			task.SdkIsSimulator = true;
			ExecuteTask (task, expectedErrors);
			Assert.That (Engine.Logger.WarningsEvents, Is.Empty, "ExtensionKit identifiers do not use the legacy allowlist");
			if (expectedErrors > 0) {
				Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain (Path.Combine (modern, contents, "Info.plist")), "Error identifies the manifest");
				Assert.That (Engine.Logger.ErrorEvents [0].Code, Is.EqualTo (kind == "both" ? "MT7236" : "MT7237"), "Error code");
			}
		}

		static PDictionary CreateManifest (string identifier, string executable)
		{
			return new PDictionary {
				{ "CFBundleIdentifier", new PString (identifier) },
				{ "CFBundleExecutable", new PString (executable) },
				{ "CFBundleShortVersionString", new PString ("1.0") },
				{ "CFBundleVersion", new PString ("1") },
			};
		}
	}
}
