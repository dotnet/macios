//
// Resource Bundling Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.IO;
using System.Linq;

namespace BundledResources {

	// Unused sentinel type used to detect whether this assembly was linked/trimmed: the linker removes it
	// when this assembly is linked, so its absence at runtime means the assembly was linked.
	class LinkerSentinel { }

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ResourcesTest {

		[Test]
		[System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This test checks whether the assembly was linked by checking if an unused type survived trimming; the behavioral difference when the trimmer is enabled is exactly what it's looking for.")]
		public void Bundled ()
		{
			// files are extracted (by MonoDevelop) so we can see them in the file system
			// that's true for simulator or devices and whatever the linker settings are
			var dir = NSBundle.MainBundle.ResourcePath!;
			Assert.That (File.Exists (Path.Combine (dir, "basn3p08.png")), Is.True, "file-basn3p08.png");
			Assert.That (File.Exists (Path.Combine (dir, "basn3p08_with_loc.png")), Is.True, "file-basn3p08_with_loc.png");
			Assert.That (File.Exists (Path.Combine (dir, "xamvideotest.mp4")), Is.True, "xamvideotest.mp4");

			// resources are removed by the linker or an extra step (e.g. "link sdk" or "don't link") but that
			// extra step is done only on device (to keep the simulator builds as fast as possible)
			// Note: Also we need to remove the shared-dotnet.plist file from the list of resources if present
			// since normally it has been embedded in the app bundle manifest (and not as a resource)
			// unless we are running under the context of (bundle original resources).
			var resources = typeof (ResourcesTest).Assembly.GetManifestResourceNames ()
				.Where (r => !r.Contains ("shared-dotnet.plist"))
				.ToArray ();

			// When this assembly is linked its resources are always stripped. When it's not linked, the
			// resources are stripped by an extra step that runs on device and desktop (but not simulator) -
			// except in a Hot Reload compatible build, where that extra step is skipped for reloadable
			// (non-linked) assemblies to leave them byte-identical (so the resources are kept).
			var assemblyLinked = typeof (ResourcesTest).Assembly.GetType ("BundledResources.LinkerSentinel") is null;
#if HOTRELOAD_COMPATIBLE_BUILD
			var hotReloadCompatibleBuild = true;
#else
			var hotReloadCompatibleBuild = false;
#endif

#if __MACOS__ || __MACCATALYST__
			var simulator = false;
#else
			var simulator = Runtime.Arch != Arch.DEVICE;
#endif

			var hasResources = simulator || (hotReloadCompatibleBuild && !assemblyLinked);
			if (!hasResources) {
				Assert.That (resources.Length, Is.EqualTo (0), "No resources");
			} else {
				var expectedResources = new string [] {
					"basn3p08.png",
					"basn3p08__with__loc.png",
					"xamvideotest.mp4",
				};
				var oldPrefixed = expectedResources.Select (v => $"__monotouch_content_{v}").ToArray ();
				var newPrefixed = expectedResources.Select (v => $"__monotouch_item_BundleResource_{v}").ToArray ();
				Assert.That (resources, Is.EquivalentTo (oldPrefixed).Or.EquivalentTo (newPrefixed), "Resources");
			}
		}
	}
}
