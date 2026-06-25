using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.MacDev;
using Xamarin.MacDev.Tasks;
using Xamarin.Tests;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ACToolTaskTests : TestBase {
		ACTool CreateACToolTask (ApplePlatform platform, string projectDir, out string intermediateOutputPath, params string [] imageAssets)
		{
			Configuration.IgnoreIfIgnoredPlatform (platform);

			intermediateOutputPath = Cache.CreateTemporaryDirectory ();

			var task = CreateTask<ACTool> ();

			var version = AppleSdkVersion.UseDefault.ToString ();
			string sdkPlatform;
			var uiDeviceFamily = "";

			switch (platform) {
			case ApplePlatform.TVOS:
				sdkPlatform = "AppleTVOS";
				uiDeviceFamily = "TV";
				break;
			case ApplePlatform.iOS:
				sdkPlatform = "iPhoneOS";
				uiDeviceFamily = "IPhone, IPad";
				break;
			case ApplePlatform.MacOSX:
				sdkPlatform = "MacOSX";
				break;
			case ApplePlatform.MacCatalyst:
				sdkPlatform = "MacCatalyst";
				break;
			default:
				throw new NotImplementedException (platform.ToString ());
			}

			task.ImageAssets = imageAssets
				.Select (v => {
					var spl = v.Split ('|');
					var rv = new TaskItem (spl [0]);
					rv.SetMetadata ("Link", spl [1]);
					return rv;
				})
				.Cast<ITaskItem> ()
				.ToArray ();
			task.IntermediateOutputPath = intermediateOutputPath;
			task.MinimumOSVersion = Xamarin.SdkVersions.GetMinVersion (platform).ToString ();
			task.OutputPath = Path.Combine (intermediateOutputPath, "OutputPath");
			task.ProjectDir = projectDir;
			task.SdkDevPath = Configuration.XcodeLocation;
			task.SdkPlatform = sdkPlatform;
			task.SdkVersion = version.ToString ();
			task.TargetFrameworkMoniker = TargetFramework.GetTargetFramework (platform).ToString ();
			task.UIDeviceFamily = uiDeviceFamily;
			return task;
		}

		ACTool CreateACToolTaskWithResources (ApplePlatform platform)
		{
			var projectDir = Path.Combine (Configuration.SourceRoot, "tests", "dotnet", "AppWithXCAssets", platform.AsString ());
			var files = Directory.GetFiles (Path.Combine (projectDir, "Resources", "Images.xcassets"), "*", SearchOption.AllDirectories);
			var imageAssets = files.Select (v => v + "|" + v.Substring (projectDir.Length + 1)).ToArray ();
			return CreateACToolTask (
				platform,
				projectDir,
				out var _,
				imageAssets
			);
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void DefaultAppIcons (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			ExecuteTask (actool);

			Assert.That (actool.PartialAppManifest, Is.Not.Null, "PartialAppManifest");
			var appIconsManifestPath = actool.PartialAppManifest?.ItemSpec ?? "";
			var appIconsManifest = PDictionary.OpenFile (appIconsManifestPath);
			Assert.That (appIconsManifest.Count, Is.EqualTo (0), $"Partial plist contents: {actool.PartialAppManifest?.ItemSpec}");
			var expectedXml =
				"""
				<?xml version="1.0" encoding="UTF-8"?>
				<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
				<plist version="1.0">
				<dict>
				</dict>
				</plist>
				""";
			PListAsserts.AreStringsEqual (expectedXml, File.ReadAllText (appIconsManifestPath), "Partial plist contents");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void AllAppIcons (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.IncludeAllAppIcons = true;

			ExecuteTask (actool);

			Assert.That (actool.PartialAppManifest, Is.Not.Null, "PartialAppManifest");

			var appIconsManifestPath = actool.PartialAppManifest?.ItemSpec ?? "";
			string expectedXml;
			if (platform == ApplePlatform.TVOS) {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIcons</key>
						<dict>
							<key>CFBundleAlternateIcons</key>
							<dict>
								<key>AlternateAppIcons</key>
								<dict>
									<key>CFBundleIconName</key>
									<string>AlternateAppIcons</string>
								</dict>
							</dict>
						</dict>
					</dict>
					</plist>
					""";
			} else {
				expectedXml = "";
			}
			PListAsserts.AreStringsEqual (expectedXml, File.ReadAllText (appIconsManifestPath), "Partial plist contents");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void AllAppIconsWithAppIcon (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.IncludeAllAppIcons = true;
			if (platform == ApplePlatform.TVOS) {
				actool.AppIcon = "AlternateBrandAssets";
			} else {
				actool.AppIcon = "AlternateAppIcons";
			}

			ExecuteTask (actool);

			Assert.That (actool.PartialAppManifest, Is.Not.Null, "PartialAppManifest");

			var appIconsManifestPath = actool.PartialAppManifest?.ItemSpec!;
			string expectedXml;
			if (platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst) {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIconFile</key>
						<string>AlternateAppIcons</string>
						<key>CFBundleIconName</key>
						<string>AlternateAppIcons</string>
					</dict>
					</plist>
					""";
			} else if (platform == ApplePlatform.TVOS) {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIcons</key>
						<dict>
							<key>CFBundleAlternateIcons</key>
							<dict>
								<key>AlternateAppIcons</key>
								<dict>
									<key>CFBundleIconName</key>
									<string>AlternateAppIcons</string>
								</dict>
							</dict>
							<key>CFBundlePrimaryIcon</key>
							<string>AppIcon</string>
						</dict>
						<key>TVTopShelfImage</key>
						<dict>
							<key>TVTopShelfPrimaryImage</key>
							<string>TopShelfImage</string>
							<key>TVTopShelfPrimaryImageWide</key>
							<string>TopShelfImageWide</string>
						</dict>
					</dict>
					</plist>
					""";
			} else {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIcons</key>
						<dict>
							<key>CFBundleAlternateIcons</key>
							<dict>
								<key>AppIcons</key>
								<dict>
									<key>CFBundleIconFiles</key>
									<array>
										<string>AppIcons60x60</string>
										<string>AppIcons76x76</string>
									</array>
									<key>CFBundleIconName</key>
									<string>AppIcons</string>
								</dict>
							</dict>
							<key>CFBundlePrimaryIcon</key>
							<dict>
								<key>CFBundleIconFiles</key>
								<array>
									<string>AlternateAppIcons60x60</string>
								</array>
								<key>CFBundleIconName</key>
								<string>AlternateAppIcons</string>
							</dict>
						</dict>
						<key>CFBundleIcons~ipad</key>
						<dict>
							<key>CFBundleAlternateIcons</key>
							<dict>
								<key>AppIcons</key>
								<dict>
									<key>CFBundleIconFiles</key>
									<array>
										<string>AppIcons60x60</string>
										<string>AppIcons76x76</string>
									</array>
									<key>CFBundleIconName</key>
									<string>AppIcons</string>
								</dict>
							</dict>
							<key>CFBundlePrimaryIcon</key>
							<dict>
								<key>CFBundleIconFiles</key>
								<array>
									<string>AlternateAppIcons60x60</string>
									<string>AlternateAppIcons76x76</string>
								</array>
								<key>CFBundleIconName</key>
								<string>AlternateAppIcons</string>
							</dict>
						</dict>
					</dict>
					</plist>
					""";
			}
			PListAsserts.AreStringsEqual (expectedXml, File.ReadAllText (appIconsManifestPath), "Partial plist contents");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void AppIcon (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.AppIcon = "AppIcons";

			ExecuteTask (actool);

			Assert.That (actool.PartialAppManifest, Is.Not.Null, "PartialAppManifest");

			var appIconsManifestPath = actool.PartialAppManifest?.ItemSpec ?? "";
			string expectedXml;
			if (platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst) {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIconFile</key>
						<string>AppIcons</string>
						<key>CFBundleIconName</key>
						<string>AppIcons</string>
					</dict>
					</plist>
					""";
			} else if (platform == ApplePlatform.TVOS) {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIcons</key>
						<dict>
							<key>CFBundlePrimaryIcon</key>
							<string>AppIcons</string>
						</dict>
						<key>TVTopShelfImage</key>
						<dict>
							<key>TVTopShelfPrimaryImage</key>
							<string>TopShelfImage</string>
							<key>TVTopShelfPrimaryImageWide</key>
							<string>TopShelfImageWide</string>
						</dict>
					</dict>
					</plist>
					""";
			} else {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIcons</key>
						<dict>
							<key>CFBundlePrimaryIcon</key>
							<dict>
								<key>CFBundleIconFiles</key>
								<array>
									<string>AppIcons60x60</string>
								</array>
								<key>CFBundleIconName</key>
								<string>AppIcons</string>
							</dict>
						</dict>
						<key>CFBundleIcons~ipad</key>
						<dict>
							<key>CFBundlePrimaryIcon</key>
							<dict>
								<key>CFBundleIconFiles</key>
								<array>
									<string>AppIcons60x60</string>
									<string>AppIcons76x76</string>
								</array>
								<key>CFBundleIconName</key>
								<string>AppIcons</string>
							</dict>
						</dict>
					</dict>
					</plist>
					""";
			}
			PListAsserts.AreStringsEqual (expectedXml, File.ReadAllText (appIconsManifestPath), "Partial plist contents");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void AppIconAndAlternateIcons (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			if (platform == ApplePlatform.TVOS) {
				actool.AppIcon = "AppIcons";
				actool.AlternateAppIcons = new ITaskItem [] { new TaskItem ("AlternateAppIcons") };
			} else {
				actool.AppIcon = "AppIcons";
				actool.AlternateAppIcons = new ITaskItem [] { new TaskItem ("AlternateAppIcons") };
			}

			ExecuteTask (actool);

			Assert.That (actool.PartialAppManifest, Is.Not.Null, "PartialAppManifest");

			var appIconsManifestPath = actool.PartialAppManifest?.ItemSpec ?? "";
			string expectedXml;
			if (platform == ApplePlatform.MacOSX || platform == ApplePlatform.MacCatalyst) {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIconFile</key>
						<string>AppIcons</string>
						<key>CFBundleIconName</key>
						<string>AppIcons</string>
					</dict>
					</plist>
					""";
			} else if (platform == ApplePlatform.TVOS) {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIcons</key>
						<dict>
							<key>CFBundleAlternateIcons</key>
							<dict>
								<key>AlternateAppIcons</key>
								<dict>
									<key>CFBundleIconName</key>
									<string>AlternateAppIcons</string>
								</dict>
							</dict>
							<key>CFBundlePrimaryIcon</key>
							<string>AppIcons</string>
						</dict>
						<key>TVTopShelfImage</key>
						<dict>
							<key>TVTopShelfPrimaryImage</key>
							<string>TopShelfImage</string>
							<key>TVTopShelfPrimaryImageWide</key>
							<string>TopShelfImageWide</string>
						</dict>
					</dict>
					</plist>
					""";
			} else {
				expectedXml =
					"""
					<?xml version="1.0" encoding="UTF-8"?>
					<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
					<plist version="1.0">
					<dict>
						<key>CFBundleIcons</key>
						<dict>
							<key>CFBundleAlternateIcons</key>
							<dict>
								<key>AlternateAppIcons</key>
								<dict>
									<key>CFBundleIconFiles</key>
									<array>
										<string>AlternateAppIcons60x60</string>
										<string>AlternateAppIcons76x76</string>
									</array>
									<key>CFBundleIconName</key>
									<string>AlternateAppIcons</string>
								</dict>
							</dict>
							<key>CFBundlePrimaryIcon</key>
							<dict>
								<key>CFBundleIconFiles</key>
								<array>
									<string>AppIcons60x60</string>
								</array>
								<key>CFBundleIconName</key>
								<string>AppIcons</string>
							</dict>
						</dict>
						<key>CFBundleIcons~ipad</key>
						<dict>
							<key>CFBundleAlternateIcons</key>
							<dict>
								<key>AlternateAppIcons</key>
								<dict>
									<key>CFBundleIconFiles</key>
									<array>
										<string>AlternateAppIcons60x60</string>
										<string>AlternateAppIcons76x76</string>
									</array>
									<key>CFBundleIconName</key>
									<string>AlternateAppIcons</string>
								</dict>
							</dict>
							<key>CFBundlePrimaryIcon</key>
							<dict>
								<key>CFBundleIconFiles</key>
								<array>
									<string>AppIcons60x60</string>
									<string>AppIcons76x76</string>
								</array>
								<key>CFBundleIconName</key>
								<string>AppIcons</string>
							</dict>
						</dict>
					</dict>
					</plist>
					""";
			}
			PListAsserts.AreStringsEqual (expectedXml, File.ReadAllText (appIconsManifestPath), "Partial plist contents");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void AlternateIcons (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.AlternateAppIcons = new ITaskItem [] { new TaskItem ("AlternateAppIcons") };

			ExecuteTask (actool);

			string expectedXml;
			switch (platform) {
			case ApplePlatform.TVOS:
				expectedXml = """
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
	<key>CFBundleIcons</key>
	<dict>
		<key>CFBundleAlternateIcons</key>
		<dict>
			<key>AlternateAppIcons</key>
			<dict>
				<key>CFBundleIconName</key>
				<string>AlternateAppIcons</string>
			</dict>
		</dict>
	</dict>
</dict>
</plist>
""";
				break;
			case ApplePlatform.iOS:
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				expectedXml = "";
				break;
			default:
				throw new NotImplementedException (platform.ToString ());
			}

			var appIconsManifestPath = actool.PartialAppManifest?.ItemSpec ?? "";
			PListAsserts.AreStringsEqual (expectedXml, File.ReadAllText (appIconsManifestPath), "Partial plist contents");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void InexistentAppIcon (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.AppIcon = "InexistentAppIcons";

			ExecuteTask (actool, 1);
			string expectedErrorMessage;
			switch (platform) {
			case ApplePlatform.TVOS:
				expectedErrorMessage = "Can't find the AppIcon 'InexistentAppIcons' among the image resources. There are 2 app icons in the image resources: AlternateBrandAssets, AppIcons.";
				break;
			case ApplePlatform.iOS:
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				expectedErrorMessage = "Can't find the AppIcon 'InexistentAppIcons' among the image resources. There are 2 app icons in the image resources: AlternateAppIcons, AppIcons.";
				break;
			default:
				throw new NotImplementedException (platform.ToString ());
			}
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo (expectedErrorMessage), "Error message");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void InexistentAlternateIcons (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.AlternateAppIcons = new ITaskItem [] { new TaskItem ("InexistentAlternateAppIcons") };

			ExecuteTask (actool, 1);
			string expectedErrorMessage;
			switch (platform) {
			case ApplePlatform.TVOS:
				expectedErrorMessage = "Can't find the AlternateAppIcon 'InexistentAlternateAppIcons' among the image resources. There are 5 app icons in the image resources: AlternateAppIcons, AppIcon, AppIcon-AppStore, AppIcons, AppIcons-AppStore.";
				break;
			case ApplePlatform.iOS:
			case ApplePlatform.MacOSX:
			case ApplePlatform.MacCatalyst:
				expectedErrorMessage = "Can't find the AlternateAppIcon 'InexistentAlternateAppIcons' among the image resources. There are 2 app icons in the image resources: AlternateAppIcons, AppIcons.";
				break;
			default:
				throw new NotImplementedException (platform.ToString ());
			}
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo (expectedErrorMessage), "Error message");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void BothAlternateAndMainIcon (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.AlternateAppIcons = new ITaskItem [] { new TaskItem ("AppIcons") };
			actool.AppIcon = "AppIcons";

			ExecuteTask (actool, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo ($"The image resource '{actool.AppIcon}' is specified as both 'AppIcon' and 'AlternateAppIcon'."), "Error message");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void XSAppIconAssetsAndAppIcon (ApplePlatform platform)
		{
			var actool = CreateACToolTaskWithResources (platform);
			actool.AppIcon = "AppIcons";
			actool.XSAppIconAssets = "Resources/Images.xcassets/AppIcons.appiconset";

			ExecuteTask (actool, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Is.EqualTo ("Can't specify both 'XSAppIconAssets' in the Info.plist and 'AppIcon' in the project file. Please select one or the other."), "Error message");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void IconFileSupport (ApplePlatform platform)
		{
			// Test that .icon folders (Icon Composer format) are recognized as app icons
			var projectDir = Cache.CreateTemporaryDirectory ();
			var iconFolderPath = Path.Combine (projectDir, "Resources", "AppIcon.icon");
			var assetsPath = Path.Combine (iconFolderPath, "Assets");
			Directory.CreateDirectory (assetsPath);

			// Create a placeholder icon.json file (simplified structure for testing)
			var iconJsonPath = Path.Combine (iconFolderPath, "icon.json");
			File.WriteAllText (iconJsonPath, @"{""groups"":[{""layers"":[{""image-name"":""icon_512x512.png"",""name"":""icon""}]}]}");

			// Create a placeholder image file in the Assets folder
			var imagePath = Path.Combine (assetsPath, "icon_512x512.png");
			File.WriteAllText (imagePath, "placeholder image");

			var actool = CreateACToolTask (
				platform,
				projectDir,
				out var _,
				iconJsonPath + "|Resources/AppIcon.icon/icon.json",
				imagePath + "|Resources/AppIcon.icon/Assets/icon_512x512.png"
			);
			actool.AppIcon = "AppIcon";

			// actool may fail on the placeholder .icon content, but the validation phase should pass
			actool.Execute ();

			// Verify that no icon validation errors were logged
			AssertNoIconValidationErrors ();
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void IconFileSupportWithIncludeAllAppIcons (ApplePlatform platform)
		{
			// Test that .icon folders work with IncludeAllAppIcons
			var projectDir = Cache.CreateTemporaryDirectory ();
			var iconFolderPath = Path.Combine (projectDir, "Resources", "AppIcon.icon");
			var assetsPath = Path.Combine (iconFolderPath, "Assets");
			Directory.CreateDirectory (assetsPath);

			var iconJsonPath = Path.Combine (iconFolderPath, "icon.json");
			File.WriteAllText (iconJsonPath, @"{""groups"":[{""layers"":[{""image-name"":""icon_512x512.png"",""name"":""icon""}]}]}");

			var imagePath = Path.Combine (assetsPath, "icon_512x512.png");
			File.WriteAllText (imagePath, "placeholder image");

			var actool = CreateACToolTask (
				platform,
				projectDir,
				out var _,
				iconJsonPath + "|Resources/AppIcon.icon/icon.json",
				imagePath + "|Resources/AppIcon.icon/Assets/icon_512x512.png"
			);
			actool.AppIcon = "AppIcon";
			actool.IncludeAllAppIcons = true;

			// actool may fail on the placeholder .icon content, but the validation phase should pass
			actool.Execute ();

			// Verify that no icon validation errors were logged
			AssertNoIconValidationErrors ();
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void IconFileSupportAsAlternateIcon (ApplePlatform platform)
		{
			// Test that .icon folders work as alternate app icons
			var projectDir = Cache.CreateTemporaryDirectory ();
			var iconFolderPath = Path.Combine (projectDir, "Resources", "AlternateIcon.icon");
			var assetsPath = Path.Combine (iconFolderPath, "Assets");
			Directory.CreateDirectory (assetsPath);

			var iconJsonPath = Path.Combine (iconFolderPath, "icon.json");
			File.WriteAllText (iconJsonPath, @"{""groups"":[{""layers"":[{""image-name"":""icon_512x512.png"",""name"":""icon""}]}]}");

			var imagePath = Path.Combine (assetsPath, "icon_512x512.png");
			File.WriteAllText (imagePath, "placeholder image");

			// Also need a primary icon for the alternate icon test to make sense
			var primaryIconPath = Path.Combine (projectDir, "Resources", "AppIcon.icon");
			var primaryAssetsPath = Path.Combine (primaryIconPath, "Assets");
			Directory.CreateDirectory (primaryAssetsPath);

			var primaryIconJsonPath = Path.Combine (primaryIconPath, "icon.json");
			File.WriteAllText (primaryIconJsonPath, @"{""groups"":[{""layers"":[{""image-name"":""icon_512x512.png"",""name"":""icon""}]}]}");

			var primaryImagePath = Path.Combine (primaryAssetsPath, "icon_512x512.png");
			File.WriteAllText (primaryImagePath, "placeholder image");

			var actool = CreateACToolTask (
				platform,
				projectDir,
				out var _,
				iconJsonPath + "|Resources/AlternateIcon.icon/icon.json",
				imagePath + "|Resources/AlternateIcon.icon/Assets/icon_512x512.png",
				primaryIconJsonPath + "|Resources/AppIcon.icon/icon.json",
				primaryImagePath + "|Resources/AppIcon.icon/Assets/icon_512x512.png"
			);
			actool.AppIcon = "AppIcon";
			actool.AlternateAppIcons = new ITaskItem [] { new TaskItem ("AlternateIcon") };

			// actool may fail on the placeholder .icon content, but the validation phase should pass
			actool.Execute ();

			// Verify that no icon validation errors were logged
			AssertNoIconValidationErrors ();
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.TVOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void InexistentIconFile (ApplePlatform platform)
		{
			// Test that an inexistent .icon-based app icon is correctly reported
			var projectDir = Cache.CreateTemporaryDirectory ();
			var iconFolderPath = Path.Combine (projectDir, "Resources", "AppIcon.icon");
			var assetsPath = Path.Combine (iconFolderPath, "Assets");
			Directory.CreateDirectory (assetsPath);

			var iconJsonPath = Path.Combine (iconFolderPath, "icon.json");
			File.WriteAllText (iconJsonPath, @"{""groups"":[{""layers"":[{""image-name"":""icon_512x512.png"",""name"":""icon""}]}]}");

			var imagePath = Path.Combine (assetsPath, "icon_512x512.png");
			File.WriteAllText (imagePath, "placeholder image");

			var actool = CreateACToolTask (
				platform,
				projectDir,
				out var _,
				iconJsonPath + "|Resources/AppIcon.icon/icon.json",
				imagePath + "|Resources/AppIcon.icon/Assets/icon_512x512.png"
			);
			actool.AppIcon = "InexistentIcon";

			ExecuteTask (actool, 1);

			var errorMessages = Engine.Logger.ErrorEvents.Select (e => e.Message).ToList ();
			Assert.That (errorMessages.Any (m => m?.Contains ("Can't find the AppIcon 'InexistentIcon'") == true), Is.True, "Should report that InexistentIcon is not found among image resources");
		}

		[Test]
		[TestCase (ApplePlatform.iOS)]
		[TestCase (ApplePlatform.MacCatalyst)]
		[TestCase (ApplePlatform.MacOSX)]
		public void MixedXCAssetsAndIconFile (ApplePlatform platform)
		{
			// Test that .icon folders and .xcassets can coexist in the validation phase
			var projectDir = Path.Combine (Configuration.SourceRoot, "tests", "dotnet", "AppWithXCAssets", platform.AsString ());
			var files = Directory.GetFiles (Path.Combine (projectDir, "Resources", "Images.xcassets"), "*", SearchOption.AllDirectories);
			var imageAssets = files.Select (v => v + "|" + v.Substring (projectDir.Length + 1)).ToList ();

			// Add a .icon folder alongside the existing .xcassets
			var tmpDir = Cache.CreateTemporaryDirectory ();
			var iconFolderPath = Path.Combine (tmpDir, "ComposerIcon.icon");
			var assetsPath = Path.Combine (iconFolderPath, "Assets");
			Directory.CreateDirectory (assetsPath);

			var iconJsonPath = Path.Combine (iconFolderPath, "icon.json");
			File.WriteAllText (iconJsonPath, @"{""groups"":[{""layers"":[{""image-name"":""icon_512x512.png"",""name"":""icon""}]}]}");

			var imagePath = Path.Combine (assetsPath, "icon_512x512.png");
			File.WriteAllText (imagePath, "placeholder image");

			imageAssets.Add (iconJsonPath + "|Resources/ComposerIcon.icon/icon.json");
			imageAssets.Add (imagePath + "|Resources/ComposerIcon.icon/Assets/icon_512x512.png");

			var actool = CreateACToolTask (
				platform,
				projectDir,
				out var _,
				imageAssets.ToArray ()
			);
			actool.AppIcon = "AppIcons";

			// actool may fail on the placeholder .icon content, but the validation phase should pass
			actool.Execute ();

			// Verify that no icon validation errors were logged
			AssertNoIconValidationErrors ();
		}

		void AssertNoIconValidationErrors ()
		{
			var errorMessages = Engine.Logger.ErrorEvents.Select (e => e.Message).ToList ();
			Assert.That (errorMessages, Has.None.Contain ("Can't find the AppIcon"),
				"Should not report that AppIcon is not found among image resources");
			Assert.That (errorMessages, Has.None.Contain ("Can't find the AlternateAppIcon"),
				"Should not report that AlternateAppIcon is not found among image resources");
			Assert.That (errorMessages, Has.None.Contain ("is specified as both 'AppIcon' and 'AlternateAppIcon'"),
				"Should not report icon conflict between AppIcon and AlternateAppIcon");
			Assert.That (errorMessages, Has.None.Contain ("Can't specify both 'XSAppIconAssets'"),
				"Should not report XSAppIconAssets conflict");
		}
	}
}
