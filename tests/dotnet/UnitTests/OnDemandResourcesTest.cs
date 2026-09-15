// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Xamarin.Tests {

	[TestFixture]
	public class OnDemandResourcesTest : TestBaseClass {
		const string project = "AppWithOnDemandResources";

		// The bundle identifier is defined in the test project's shared.csproj (ApplicationId),
		// and the asset pack directory name is "<bundle identifier>.<tags>.assetpack".
		const string assetPackName = "com.xamarin.appwithondemandresources.MusicTag.assetpack";

		[TestCase (ApplePlatform.iOS, "iossimulator-arm64")]
		[TestCase (ApplePlatform.TVOS, "tvossimulator-arm64")]
		public void OnDemandResourcesAreEmbeddedForSimulator (ApplePlatform platform, string runtimeIdentifiers)
		{
			Configuration.AssertRuntimeIdentifiersAvailable (platform, runtimeIdentifiers);
			Configuration.IgnoreIfIgnoredPlatform (platform);

			var projectPath = GetProjectPath (project, runtimeIdentifiers: runtimeIdentifiers, platform: platform, out var appPath);
			Clean (projectPath);

			DotNet.AssertBuild (projectPath, verbosity);

			// The asset pack must be embedded inside the app bundle so the on-demand resources APIs can find it
			// on the simulator (there's no App Store nor a local hosting server to stream it from).
			var assetPackDir = Path.Combine (appPath, "OnDemandResources", assetPackName);
			Assert.That (assetPackDir, Does.Exist, "The asset pack directory must be embedded in the app bundle.");
			Assert.That (Path.Combine (assetPackDir, "SoundBank.bin"), Does.Exist, "The tagged resource must be inside the asset pack.");
			Assert.That (Path.Combine (assetPackDir, "Info.plist"), Does.Exist, "The asset pack must have an Info.plist.");

			// The OnDemandResources.plist maps tags to asset packs, and must reference our tag and pack.
			var odrPlistPath = Path.Combine (appPath, "OnDemandResources.plist");
			Assert.That (odrPlistPath, Does.Exist, "OnDemandResources.plist must exist in the app bundle.");

			var odrPlist = PDictionary.OpenFile (odrPlistPath);
			var odr = AssertNotNull (odrPlist, "Failed to load OnDemandResources.plist.");

			var assetPacks = AssertNotNull (odr.Get<PDictionary> ("NSBundleResourceRequestAssetPacks"), "NSBundleResourceRequestAssetPacks");
			Assert.That (assetPacks, Is.Not.Empty, "NSBundleResourceRequestAssetPacks");

			var requestTags = AssertNotNull (odr.Get<PDictionary> ("NSBundleResourceRequestTags"), "NSBundleResourceRequestTags");
			var tag = AssertNotNull (requestTags.Get<PDictionary> ("MusicTag"), "The 'MusicTag' tag must be listed in NSBundleResourceRequestTags.");

			// The tag must reference an asset pack that's actually present in the manifest.
			var tagAssetPacks = AssertNotNull (tag.GetArray ("NSAssetPacks"), "The 'MusicTag' tag must reference at least one asset pack.");
			Assert.That (tagAssetPacks, Is.Not.Empty, "The 'MusicTag' tag must reference at least one asset pack.");
			foreach (var packId in tagAssetPacks.OfType<PString> ())
				Assert.That (assetPacks.ContainsKey (packId.Value), Is.True, $"The asset pack '{packId.Value}' referenced by the tag must be listed in NSBundleResourceRequestAssetPacks.");

			// The streaming manifest template points at http://127.0.0.1 URLs, which don't work on the simulator.
			// It must be replaced by an AssetPackManifest.plist that points at the embedded asset packs.
			var manifestPath = Path.Combine (appPath, "AssetPackManifest.plist");
			Assert.That (manifestPath, Does.Exist, "AssetPackManifest.plist must exist in the app bundle.");
			Assert.That (Path.Combine (appPath, "AssetPackManifestTemplate.plist"), Does.Not.Exist, "The streaming AssetPackManifestTemplate.plist must not be present in the app bundle.");

			var manifest = AssertNotNull (PDictionary.OpenFile (manifestPath), "Failed to load AssetPackManifest.plist.");
			var resources = AssertNotNull (manifest.GetArray ("resources"), "AssetPackManifest.plist must contain resources.");
			Assert.That (resources, Is.Not.Empty, "AssetPackManifest.plist must contain resources.");
			foreach (var resource in resources.OfType<PDictionary> ()) {
				var url = resource.Get<PString> ("URL")?.Value;
				Assert.That (url, Does.StartWith ("OnDemandResources/"), "The asset pack URL must be a relative path to the embedded asset pack.");
				var isStreamable = resource.Get<PNumber> ("isStreamable");
				Assert.That (isStreamable?.Value, Is.EqualTo (0), "Embedded asset packs must not be streamable.");
			}
		}

		static T AssertNotNull<T> (T? value, string message) where T : class
		{
			Assert.That (value, Is.Not.Null, message);
			return value ?? throw new InvalidOperationException (message);
		}
	}
}
