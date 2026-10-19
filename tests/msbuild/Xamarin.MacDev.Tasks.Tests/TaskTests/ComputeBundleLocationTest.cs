// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class ComputeBundleLocationTest : TestBase {
		ComputeBundleLocation CreateManifestTask (string library, string? manifestContents, out string manifest)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var path = Path.Combine (directory, library);
			var manifestLibrary = path.EndsWith (".zip") ? Path.ChangeExtension (path, null) : path;
			manifest = Path.ChangeExtension (manifestLibrary, ".manifest");
			if (library.EndsWith (".framework") || library.EndsWith (".xcframework")) {
				Directory.CreateDirectory (path);
				path = Path.Combine (path, "Info.plist");
			}
			File.WriteAllText (path, "");
			if (manifestContents is not null)
				File.WriteAllText (manifest, manifestContents);

			var task = CreateTask<ComputeBundleLocation> ();
			task.FrameworksDirectory = "Frameworks";
			task.AssemblyDirectory = "MonoBundle";
			task.ProjectDir = directory;
			task.ResolvedFileToPublish = new ITaskItem [] { new TaskItem (path), new TaskItem (manifest) };
			return task;
		}

		[TestCase ("libtest.a", "StaticLibrary")]
		[TestCase ("libtest.dylib", "DynamicLibrary")]
		[TestCase ("libtest.so", "PluginLibrary")]
		[TestCase ("Test.framework", "AppleFramework")]
		[TestCase ("Test.xcframework", "AppleFramework")]
		[TestCase ("Test.framework.zip", "CompressedAppleFramework")]
		[TestCase ("Test.xcframework.zip", "CompressedAppleFramework")]
		public void NativeLibraryManifest (string library, string publishFolderType)
		{
			var task = CreateManifestTask (library, $@"<BindingAssembly>
	<NativeReference Name=""{library}"">
		<ForceLoad>True</ForceLoad>
		<Frameworks>CoreLocation Foundation ModelIO</Frameworks>
		<WeakFrameworks>Metal</WeakFrameworks>
		<IsCxx>True</IsCxx>
		<LinkerFlags>-lz -lsqlite3</LinkerFlags>
		<NeedsGccExceptionHandling>True</NeedsGccExceptionHandling>
		<SmartLink>False</SmartLink>
	</NativeReference>
</BindingAssembly>", out var manifest);

			ExecuteTask (task);

			var items = task.UpdatedResolvedFileToPublish ?? throw new AssertionException ("Missing output items.");
			var native = items.Single (v => v.ItemSpec != manifest);
			Assert.That (native.GetMetadata ("PublishFolderType"), Is.EqualTo (publishFolderType), "PublishFolderType");
			Assert.That (native.GetMetadata ("ForceLoad"), Is.EqualTo ("True"), "ForceLoad");
			Assert.That (native.GetMetadata ("Frameworks"), Is.EqualTo ("CoreLocation Foundation ModelIO"), "Frameworks");
			Assert.That (native.GetMetadata ("WeakFrameworks"), Is.EqualTo ("Metal"), "WeakFrameworks");
			Assert.That (native.GetMetadata ("IsCxx"), Is.EqualTo ("True"), "IsCxx");
			Assert.That (native.GetMetadata ("LinkerFlags"), Is.EqualTo ("-lz -lsqlite3"), "LinkerFlags");
			Assert.That (native.GetMetadata ("NeedsGccExceptionHandling"), Is.EqualTo ("True"), "NeedsGccExceptionHandling");
			Assert.That (native.GetMetadata ("SmartLink"), Is.EqualTo ("False"), "SmartLink");
			Assert.That (items.Single (v => v.ItemSpec == manifest).GetMetadata ("PublishFolderType"), Is.EqualTo ("None"), "manifest is not bundled");
			Assert.That (Engine.Logger.WarningsEvents, Is.Empty, "warnings");
		}

		[Test]
		public void NoManifest ()
		{
			var task = CreateManifestTask ("libtest.a", null, out _);
			task.ResolvedFileToPublish = task.ResolvedFileToPublish?.Take (1).ToArray ();
			ExecuteTask (task);
			var items = task.UpdatedResolvedFileToPublish ?? throw new AssertionException ("Missing output items.");
			Assert.That (items.Single ().GetMetadata ("LinkerFlags"), Is.Empty, "no linker flags");
			Assert.That (Engine.Logger.WarningsEvents, Is.Empty, "warnings");
		}

		[TestCase ("<BindingAssembly>")]
		[TestCase ("<Other />")]
		[TestCase ("<BindingAssembly><NativeReference Name=\"other.a\" /></BindingAssembly>")]
		[TestCase ("<BindingAssembly><NativeReference /><NativeReference Name=\"other.a\" /></BindingAssembly>")]
		[TestCase ("<BindingAssembly><NativeReference Name=\"libtest.a\" /><NativeReference Name=\"libtest.a\" /></BindingAssembly>")]
		public void InvalidManifest (string contents)
		{
			var task = CreateManifestTask ("libtest.a", contents, out _);
			ExecuteTask (task, 1);
		}

		[Test]
		public void ManifestCannotRedirectOutput ()
		{
			var task = CreateManifestTask ("libtest.dylib", @"<BindingAssembly>
	<NativeReference Name=""libtest.dylib"">
		<LinkerFlags>-lz</LinkerFlags>
		<PublishFolderType>RootDirectory</PublishFolderType>
		<RelativePath>../../escape</RelativePath>
		<TargetPath>../../escape</TargetPath>
		<Link>../../escape</Link>
		<DestinationSubDirectory>../../escape</DestinationSubDirectory>
		<Identity>evil</Identity>
	</NativeReference>
</BindingAssembly>", out var manifest);
			ExecuteTask (task);
			var items = task.UpdatedResolvedFileToPublish ?? throw new AssertionException ("Missing output items.");
			var native = items.Single (v => v.ItemSpec != manifest);
			Assert.That (native.GetMetadata ("LinkerFlags"), Is.EqualTo ("-lz"), "LinkerFlags");
			Assert.That (native.GetMetadata ("PublishFolderType"), Is.EqualTo ("DynamicLibrary"), "PublishFolderType");
			Assert.That (native.GetMetadata ("RelativePath"), Is.EqualTo (Path.Combine ("MonoBundle", "libtest.dylib")), "RelativePath");
			Assert.That (native.GetMetadata ("Identity"), Is.EqualTo (native.ItemSpec), "Identity");
			foreach (var metadata in new [] { "TargetPath", "Link", "DestinationSubDirectory" })
				Assert.That (native.GetMetadata (metadata), Is.Empty, metadata);
			Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (6), "warnings");
		}

		[Test]
		public void StaticLibraryLinkerFlags ()
		{
			var task = CreateManifestTask ("libtest.a", @"<BindingAssembly>
	<NativeReference Name=""other.a""><LinkerFlags>-lignored</LinkerFlags></NativeReference>
	<NativeReference Name=""libtest.a"">
		<ForceLoad>True</ForceLoad>
		<Frameworks>Foundation ModelIO</Frameworks>
		<WeakFrameworks>Metal</WeakFrameworks>
		<IsCxx>True</IsCxx>
		<LinkerFlags>-lz -L&quot;path with spaces&quot;</LinkerFlags>
		<NeedsGccExceptionHandling>True</NeedsGccExceptionHandling>
	</NativeReference>
</BindingAssembly>", out var manifest);
			ExecuteTask (task);
			var items = task.UpdatedResolvedFileToPublish ?? throw new AssertionException ("Missing output items.");
			var native = items.Single (v => v.ItemSpec != manifest);
			native.SetMetadata ("Kind", "Static");

			var options = new LinkerOptions ();
			options.BuildNativeReferenceFlags (new TaskLoggingHelper (task), new [] { native });
			Assert.That (options.Arguments, Is.EqualTo (new [] { "-force_load", native.ItemSpec, "-lgcc_eh", "-lz", "-Lpath with spaces" }), "linker arguments");
			Assert.That (options.Frameworks, Is.EquivalentTo (new [] { "Foundation", "ModelIO" }), "frameworks");
			Assert.That (options.WeakFrameworks, Is.EquivalentTo (new [] { "Metal" }), "weak frameworks");
			Assert.That (options.Cxx, Is.True, "C++ linker");
			Assert.That (Engine.Logger.WarningsEvents, Is.Empty, "warnings");
		}

		[TestCase ("Test.framework")]
		[TestCase ("Test.xcframework")]
		public void ResolvedFrameworkRetainsManifestFlags (string library)
		{
			var task = CreateManifestTask (library, $@"<BindingAssembly>
	<NativeReference Name=""{library}""><LinkerFlags>-lz</LinkerFlags></NativeReference>
</BindingAssembly>", out var manifest);
			var framework = Path.Combine (task.ProjectDir, library);
			if (library.EndsWith (".xcframework")) {
				var plist = Path.Combine (TestContext.CurrentContext.TestDirectory, "Resources", "xcf-xcode12.2.plist");
				File.Copy (plist, Path.Combine (framework, "Info.plist"), true);
				var binary = Path.Combine (framework, "macos-arm64_x86_64", "Universal.framework", "Universal");
				Directory.CreateDirectory (Path.GetDirectoryName (binary) ?? throw new AssertionException ("Missing binary directory."));
				File.WriteAllText (binary, "");
			}
			task.ResolvedFileToPublish = task.ResolvedFileToPublish?.Reverse ().ToArray ();
			ExecuteTask (task);

			var items = task.UpdatedResolvedFileToPublish ?? throw new AssertionException ("Missing output items.");
			var resolver = CreateTask<ResolveNativeReferences> ();
			resolver.NativeReferences = items.Where (v => v.ItemSpec != manifest).ToArray ();
			resolver.Architectures = "arm64";
			resolver.FrameworksDirectory = "Frameworks";
			resolver.IntermediateOutputPath = Path.Combine (task.ProjectDir, "obj");
			resolver.TargetFrameworkMoniker = "net11.0-macos";
			ExecuteTask (resolver);

			var resolved = resolver.NativeFrameworks ?? throw new AssertionException ("Missing native references.");
			Assert.That (resolved.Single ().GetMetadata ("LinkerFlags"), Is.EqualTo ("-lz"), "resolved linker flags");
			var options = new LinkerOptions ();
			options.BuildNativeReferenceFlags (new TaskLoggingHelper (resolver), resolved);
			Assert.That (options.Arguments, Does.Contain ("-lz"), "linker arguments");
			Assert.That (Engine.Logger.WarningsEvents, Is.Empty, "warnings");
		}
	}
}
