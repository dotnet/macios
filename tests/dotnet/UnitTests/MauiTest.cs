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
				properties ["DeviceSpecificBuild"] = "true";
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
			var infoPlist = PDictionary.OpenFile (infoPlistPath);
			Assert.That (infoPlist.GetString ("CFBundleIdentifier").Value, Is.EqualTo ("com.xamarin.mymauiapp"), "CFBundleIdentifier");
			Assert.That (infoPlist.GetString ("CFBundleDisplayName").Value, Is.EqualTo ("MyMauiApp"), "CFBundleDisplayName");
			Assert.That (infoPlist.GetString ("CFBundleVersion").Value, Is.EqualTo ("1"), "CFBundleVersion");
			Assert.That (infoPlist.GetString ("CFBundleShortVersionString").Value, Is.EqualTo ("1.0"), "CFBundleShortVersionString");

			Assert.That (BinLog.TryFindPropertyValue (rv.BinLogPath, "TrimMode", out var trimModeValue), Is.True, "Could not find the property 'TrimMode' in the binlog.");
			Assert.That (BinLog.TryFindPropertyValue (rv.BinLogPath, "_LinkMode", out var linkModeValue), Is.True, "Could not find the property '_LinkMode' in the binlog.");
			Assert.That (BinLog.TryFindPropertyValue (rv.BinLogPath, "MtouchLink", out var mtouchLinkValue), Is.True, "Could not find the property 'MtouchLink' in the binlog.");

			Assert.That (trimModeValue, Is.EqualTo ("copy"), "TrimMode");
			Assert.That (linkModeValue, Is.EqualTo ("None"), "LinkMode");
			Assert.That (mtouchLinkValue, Is.EqualTo ("None"), "MtouchLink");
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
