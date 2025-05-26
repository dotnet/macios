using System.Diagnostics;
using System.Xml;

using Mono.Cecil;

#nullable enable

namespace Xamarin.Tests {
	[TestFixture]
	public class MauiTest : TestBaseClass {
		[TestCase (ApplePlatform.iOS, "ios-arm64")]
		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		public void BuildMauiApp (ApplePlatform platform, string runtimeIdentifiers)
		{
			BuildMauiAppImpl (platform, runtimeIdentifiers);
		}

		void BuildMauiAppImpl (ApplePlatform platform, string runtimeIdentifiers, bool deviceSpecificBuild = false)
		{
			var project = "MyMauiApp";
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);

			DotNet.InstallWorkload ("maui-tizen");

			var properties = GetDefaultProperties (runtimeIdentifiers);
			if (deviceSpecificBuild) {
				properties ["file:TargetiOSDevice"] =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>device</key>
						<dict>
							<key>architecture</key>
							<string>ARM64</string>
							<key>os</key>
							<string>ios</string>
							<key>model</key>
							<string>iphone</string>
							<key>os-version</key>
							<string>18.0</string>
						</dict>
					</dict>
					</plist>
					""";
			}
			var rv = DotNet.AssertBuild (project_path, properties);
			AssertThatLinkerExecuted (rv);
			var infoPlistPath = GetInfoPListPath (platform, appPath);
			var infoPlist = PDictionary.FromFile (infoPlistPath)!;
			Assert.AreEqual ("com.xamarin.mymauiapp", infoPlist.GetString ("CFBundleIdentifier").Value, "CFBundleIdentifier");
			Assert.AreEqual ("MyMauiApp", infoPlist.GetString ("CFBundleDisplayName").Value, "CFBundleDisplayName");
			Assert.AreEqual ("1", infoPlist.GetString ("CFBundleVersion").Value, "CFBundleVersion");
			Assert.AreEqual ("1.0", infoPlist.GetString ("CFBundleShortVersionString").Value, "CFBundleShortVersionString");


			Assert.IsTrue (BinLog.TryFindPropertyValue (rv.BinLogPath, "TrimMode", out var trimModeValue), "Could not find the property 'TrimMode' in the binlog.");
			Assert.IsTrue (BinLog.TryFindPropertyValue (rv.BinLogPath, "_LinkMode", out var linkModeValue), "Could not find the property '_LinkMode' in the binlog.");
			Assert.IsTrue (BinLog.TryFindPropertyValue (rv.BinLogPath, "MtouchLink", out var mtouchLinkValue), "Could not find the property 'MtouchLink' in the binlog.");

			Assert.AreEqual ("copy", trimModeValue, "TrimMode");
			Assert.AreEqual ("None", linkModeValue, "LinkMode");
			Assert.AreEqual ("None", mtouchLinkValue, "MtouchLink");
		}

		[TestCase (ApplePlatform.MacCatalyst, "maccatalyst-arm64")]
		// [Category ("RemoteWindows")]
		public void BuildMauiAppWithDeviceSpecificBuilds (ApplePlatform platform, string runtimeIdentifiers)
		{
			// Configuration.IgnoreIfNotOnWindows ();
			BuildMauiAppImpl (platform, runtimeIdentifiers, deviceSpecificBuild: true);
		}
	}
}
