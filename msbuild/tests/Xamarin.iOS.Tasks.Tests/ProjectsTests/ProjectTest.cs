﻿using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Xamarin.iOS.Tasks
{
	public class ProjectTest : TestBase {
		public string BundlePath;
		public string Platform;

		public ProjectTest (string platform)
		{
			Platform = platform;
		}

		public ProjectTest (string bundlePath, string platform)
		{
			BundlePath = bundlePath;
			Platform = platform;
		}

		//public void SetupPaths (string appName, string platform) 
		//{
		//	var paths = SetupProjectPaths (appName, "../", true, platform);
		//	MonoTouchProjectPath = paths ["project_path"];
		//	AppBundlePath = paths ["app_bundlepath"];
		//}

		[SetUp]
		public override void Setup () 
		{
			AssertValidDeviceBuild (Platform);
			SetupEngine ();
		}

		public string BuildProject (string appName, string platform, string config, int expectedErrorCount = 0, bool clean = true)
		{
			var mtouchPaths = SetupProjectPaths (appName, "../", true, platform, config);
			var csproj = mtouchPaths["project_csprojpath"];

			var proj = SetupProject (Engine, csproj);

			AppBundlePath = mtouchPaths ["app_bundlepath"];
			Engine.GlobalProperties.SetProperty("Platform", platform);
			Engine.GlobalProperties.SetProperty("Configuration", config);

			if (clean) {
				RunTarget (proj, "Clean");
				Assert.IsFalse (Directory.Exists (AppBundlePath), "App bundle exists after cleanup: {0} ", AppBundlePath);
				Assert.IsFalse (Directory.Exists (AppBundlePath + ".dSYM"), "App bundle .dSYM exists after cleanup: {0} ", AppBundlePath + ".dSYM");
				Assert.IsFalse (Directory.Exists (AppBundlePath + ".mSYM"), "App bundle .mSYM exists after cleanup: {0} ", AppBundlePath + ".mSYM");
			}

			proj = SetupProject (Engine, mtouchPaths.ProjectCSProjPath);
			RunTarget (proj, "Build", expectedErrorCount);

			if (expectedErrorCount > 0)
				return csproj;

			Assert.IsTrue (Directory.Exists (AppBundlePath), "App Bundle does not exist: {0} ", AppBundlePath);

			TestFilesExists (AppBundlePath, ExpectedAppFiles);
			TestFilesDoNotExist (AppBundlePath, UnexpectedAppFiles);

			var coreFiles = GetCoreAppFiles (platform, config, appName + ".exe", appName);
			if (IsTVOS) {
				TestFilesExists (platform == "iPhone" ? Path.Combine (AppBundlePath, ".monotouch-64") : AppBundlePath, coreFiles);
			} else {
				bool exists = false;

				var baseDir = Path.Combine (AppBundlePath, ".monotouch-32");
				if (Directory.Exists (baseDir)) {
					TestFilesExists (baseDir, coreFiles);
					exists = true;
				}

				baseDir = Path.Combine (AppBundlePath, ".monotouch-64");
				if (Directory.Exists (baseDir)) {
					TestFilesExists (baseDir, coreFiles);
					exists = true;
				}

				if (!exists)
					TestFilesExists (AppBundlePath, coreFiles);
			}

			if (platform == "iPhone") {
				var dSYMInfoPlist = Path.Combine (AppBundlePath + ".dSYM", "Contents", "Info.plist");
				var nativeExecutable = Path.Combine (AppBundlePath, appName);

				Assert.IsTrue (File.Exists (dSYMInfoPlist), "dSYM Info.plist file does not exist");
				Assert.IsTrue (File.GetLastWriteTimeUtc (dSYMInfoPlist) >= File.GetLastWriteTimeUtc (nativeExecutable), "dSYM Info.plist should be newer than the native executable");
			}

			return csproj;
		}
	}
}

