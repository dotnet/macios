// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.IO;

using NUnit.Framework;

using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ValidateAppBundleTaskTests : TestBase {
		[TestCase ("valid", 0)]
		[TestCase ("missing-identifier", 1)]
		[TestCase ("both", 1)]
		public void MixedExtensions (string kind, int expectedErrors)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var app = Path.Combine (directory, "Container.app");
			Directory.CreateDirectory (app);
			var plist = CreateManifest ("com.example.container", "Container");
			plist ["CFBundleSupportedPlatforms"] = new PArray { new PString ("iPhoneSimulator") };
			plist ["UIDeviceFamily"] = new PArray { new PNumber (1), new PNumber (2) };
			plist.Save (Path.Combine (app, "Info.plist"));

			var legacy = Path.Combine (app, "PlugIns", "Legacy.appex");
			Directory.CreateDirectory (legacy);
			plist = CreateManifest ("com.example.container.legacy", "Legacy");
			plist ["NSExtension"] = new PDictionary {
				{ "NSExtensionPointIdentifier", new PString ("com.apple.intents-service") },
			};
			plist.Save (Path.Combine (legacy, "Info.plist"));

			var modern = Path.Combine (app, "Extensions", "Modern.appex");
			Directory.CreateDirectory (modern);
			plist = CreateManifest ("com.example.container.modern", "Modern");
			var attributes = new PDictionary ();
			if (kind != "missing-identifier")
				attributes ["EXExtensionPointIdentifier"] = "com.example.future-extension-point";
			plist ["EXAppExtensionAttributes"] = attributes;
			if (kind == "both")
				plist ["NSExtension"] = new PDictionary ();
			plist.Save (Path.Combine (modern, "Info.plist"));

			var task = CreateTask<ValidateAppBundleTask> ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (ApplePlatform.iOS).ToString ();
			task.AppBundlePath = app;
			task.SdkIsSimulator = true;
			ExecuteTask (task, expectedErrors);
			Assert.That (Engine.Logger.WarningsEvents, Is.Empty, "ExtensionKit identifiers do not use the legacy allowlist");
			if (expectedErrors > 0)
				Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain (Path.Combine (modern, "Info.plist")), "Error identifies the manifest");
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
