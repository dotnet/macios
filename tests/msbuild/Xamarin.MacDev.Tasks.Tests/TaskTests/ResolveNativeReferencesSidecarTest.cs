using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {

	// Regression tests for https://github.com/dotnet/macios - a binding resource package's 'manifest' is
	// passive data that may come from a restored (and potentially untrusted) package, so it must not be
	// able to inject path/layout/identity metadata that could redirect this task's (or a downstream
	// task's) output outside the intended output directory.
	[TestFixture]
	public class ResolveNativeReferencesSidecarTest : TestBase {

		ResolveNativeReferences CreateSidecarTask (string manifestContents, out string tmpdir)
		{
			tmpdir = Cache.CreateTemporaryDirectory ();
			var resources = Path.Combine (tmpdir, "Binding.resources");
			Directory.CreateDirectory (resources);
			File.WriteAllText (Path.Combine (resources, "manifest"), manifestContents);
			// Create the (fake) native library referenced by the manifest, for realism.
			File.WriteAllText (Path.Combine (resources, "libpayload.dylib"), "fake dylib");

			var task = CreateTask<ResolveNativeReferences> ();
			task.Architectures = "arm64";
			task.FrameworksDirectory = "Frameworks";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "obj");
			task.SdkIsSimulator = false;
			task.TargetFrameworkMoniker = "net9.0-macos";
			task.References = new ITaskItem [] { new TaskItem (Path.Combine (tmpdir, "Binding.dll")) };
			return task;
		}

		[Test]
		public void StripsUnsafeManifestMetadata ()
		{
			var manifest = @"<BindingAssembly>
	<NativeReference Name=""libpayload.dylib"">
		<Kind>Dynamic</Kind>
		<ForceLoad>True</ForceLoad>
		<Frameworks>CoreFoundation</Frameworks>
		<RelativePath>../../../../../../tmp/escape/libpayload.dylib</RelativePath>
		<ReidentifiedPath>/tmp/escape/libpayload.dylib</ReidentifiedPath>
		<DynamicLibraryId>@executable_path/../../../../escape</DynamicLibraryId>
	</NativeReference>
</BindingAssembly>";
			var task = CreateSidecarTask (manifest, out var _);

			ExecuteTask (task, 0);

			var item = task.NativeFrameworks!.Single (v => v.GetMetadata ("Kind") == "Dynamic");

			// Allowed (non-path) metadata is preserved (overriding the defaults).
			Assert.That (item.GetMetadata ("ForceLoad"), Is.EqualTo ("True"), "ForceLoad");
			Assert.That (item.GetMetadata ("Frameworks"), Is.EqualTo ("CoreFoundation"), "Frameworks");

			// Path/layout/identity metadata is NOT copied from the manifest.
			Assert.That (item.GetMetadata ("RelativePath"), Is.Empty, "RelativePath");
			Assert.That (item.GetMetadata ("ReidentifiedPath"), Is.Empty, "ReidentifiedPath");
			Assert.That (item.GetMetadata ("DynamicLibraryId"), Is.Empty, "DynamicLibraryId");

			// A warning is emitted for each ignored metadata.
			var warnings = Engine.Logger.WarningsEvents.Select (v => v.Message ?? "").ToArray ();
			Assert.That (warnings.Count (v => v.Contains ("RelativePath")), Is.EqualTo (1), "RelativePath warning");
			Assert.That (warnings.Count (v => v.Contains ("ReidentifiedPath")), Is.EqualTo (1), "ReidentifiedPath warning");
			Assert.That (warnings.Count (v => v.Contains ("DynamicLibraryId")), Is.EqualTo (1), "DynamicLibraryId warning");
		}

		[Test]
		public void RejectsTraversalInName ()
		{
			var manifest = @"<BindingAssembly>
	<NativeReference Name=""../../../../../../tmp/escape/evil.dylib"">
		<Kind>Dynamic</Kind>
	</NativeReference>
</BindingAssembly>";
			var task = CreateSidecarTask (manifest, out var _);

			ExecuteTask (task, 1);

			Assert.That (Engine.Logger.ErrorEvents.Single ().Message, Does.Contain ("'..'"), "error mentions traversal");
		}
	}
}
