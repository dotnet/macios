// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Xamarin.Tests {
	[TestFixture]
	public class ContentWithPublishFolderTypeTest : TestBaseClass {

		[Test]
		[TestCase (ApplePlatform.MacOSX, "osx-arm64")]
		public void ContentFilesWithSdkAssemblyNamesAreIncluded (ApplePlatform platform, string runtimeIdentifiers)
		{
			// Regression test for https://github.com/dotnet/macios/issues/25497
			// When Content items have PublishFolderType=RootDirectory and filenames matching
			// SDK assemblies (e.g., Microsoft.macOS.dll) or runtime files (e.g., libclrgc.dylib),
			// they should still be copied to the app bundle.
			var project = "ContentWithPublishFolderType";
			Configuration.IgnoreIfIgnoredPlatform (platform);
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);

			var project_path = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (project_path);

			var properties = GetDefaultProperties (runtimeIdentifiers);
			DotNet.AssertBuild (project_path, properties);

			// These are files in our helper directory that have the same names as SDK assemblies / runtime files.
			// They should all be present in the app bundle's SharedSupport directory.
			var expectedFiles = new string [] {
				// Files that match SDK assembly names (these are the ones that go missing per the bug report)
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/.xamarin/osx-arm64/Microsoft.macOS.dll",
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/.xamarin/osx-arm64/Microsoft.CSharp.dll",
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/.xamarin/osx-arm64/Microsoft.VisualBasic.dll",
				// A runtime dylib that matches a known runtime file name
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/libclrgc.dylib",
				// Files that don't match SDK assembly names (these work fine)
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/.xamarin/osx-arm64/HeartBeatHandlerMac.dll",
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/AsyncIO.dll",
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/HeartbeatHandlerLib.dll",
				"Contents/SharedSupport/SubApp.app/Contents/MonoBundle/System.ServiceModel.Security.dll",
			};

			foreach (var expectedFile in expectedFiles) {
				var fullPath = Path.Combine (appPath, expectedFile);
				Assert.That (fullPath, Does.Exist, $"Expected file '{expectedFile}' to be in the app bundle");
			}
		}
	}
}
