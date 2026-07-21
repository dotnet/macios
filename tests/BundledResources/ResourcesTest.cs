//
// Resource Bundling Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace BundledResources {

	// An unused type used to detect whether this assembly was linked (trimmed): the trimmer removes
	// it when the assembly is linked, but it's kept when the assembly is only copied (e.g. "dont link"
	// or "link sdk"). This mirrors the approach in TestRuntime.IsLinkAll.
	class LinkerSentinel { }

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ResourcesTest {

		[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This property checks whether the trimmer is enabled by checking if a type survived trimming; it's thus trimmer safe in that any behavioral difference when the trimmer is enabled is exactly what it's looking for.")]
		static bool AssemblyWasLinked {
			get {
				// Reference the sentinel by name (not typeof) so we don't root it and prevent it from
				// being trimmed - that would defeat the detection.
				return typeof (ResourcesTest).Assembly.GetType ("BundledResources.LinkerSentinel") is null;
			}
		}

		[Test]
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

#if __MACOS__ || __MACCATALYST__
			var isSimulator = false;
#else
			var isSimulator = Runtime.Arch != Arch.DEVICE;
#endif

#if HOTRELOAD_COMPATIBLE_BUILD
			// In a hot-reload-compatible build, resource stripping is skipped for reloadable
			// (non-linked) user assemblies, because stripping would re-serialize the assembly and
			// break Hot Reload. So the bundled resources remain embedded unless this assembly was
			// linked (in which case it's re-saved regardless, and stripping still applies).
			var hasResources = isSimulator || !AssemblyWasLinked;
#else
			// Without Hot Reload the resources are always stripped, except on the simulator (where
			// stripping is skipped to keep simulator builds fast).
			var hasResources = isSimulator;
#endif
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
