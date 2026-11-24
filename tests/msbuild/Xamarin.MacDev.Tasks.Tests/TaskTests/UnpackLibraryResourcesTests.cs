// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;

using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Mono.Cecil;

using NUnit.Framework;

using Xamarin.Tests;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class UnpackLibraryResourcesTests : TestBase {

		class ResourceLogger : Microsoft.Build.Utilities.Logger {
			public int UnpackExecutions { get; private set; }
			public string Errors { get; private set; } = "";

			public override void Initialize (IEventSource eventSource)
			{
				eventSource.ErrorRaised += (sender, e) => Errors += e.Message + "\n";
				eventSource.TaskStarted += (sender, e) => {
					if (e.TaskName == "UnpackLibraryResources")
						UnpackExecutions++;
				};
			}
		}

		static string CreateAssemblyWithResource (string directory, string assemblyName, string resourceName, byte [] content)
		{
			var asmPath = Path.Combine (directory, assemblyName + ".dll");
			var assemblyDef = AssemblyDefinition.CreateAssembly (
				new AssemblyNameDefinition (assemblyName, new Version (1, 0, 0, 0)),
				assemblyName,
				ModuleKind.Dll);

			var resource = new EmbeddedResource (resourceName, ManifestResourceAttributes.Public, content);
			assemblyDef.MainModule.Resources.Add (resource);
			assemblyDef.Write (asmPath);

			return asmPath;
		}

		[Test]
		public void PathTraversal_IsRejected ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			// Resource name "__monotouch_content_.._sEvil.txt" unmangles to "../Evil.txt" (path traversal)
			var assemblyPath = CreateAssemblyWithResource (tmpdir, "TestTraversal", "__monotouch_content_.._sEvil.txt", new byte [] { 0x41 });

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 1);

			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("would extract to"));
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("outside"));

			// Verify the file was NOT written outside the target directory
			var escapedPath = Path.Combine (tmpdir, "intermediate", "unpack", "TestTraversal", "Evil.txt");
			Assert.That (File.Exists (escapedPath), Is.False, "File should not have been extracted outside target directory");
		}

		[Test]
		public void ValidResource_IsExtracted ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			// Resource name "__monotouch_content_sub_sfile.txt" unmangles to "sub/file.txt" (valid path)
			var assemblyPath = CreateAssemblyWithResource (tmpdir, "TestValid", "__monotouch_content_sub_sfile.txt", new byte [] { 0x41 });

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = new [] { new TaskItem (assemblyPath) };
			task.TargetFrameworkDirectory = Array.Empty<TaskItem> ();

			ExecuteTask (task, expectedErrorCount: 0);

			var extractedPath = Path.Combine (tmpdir, "intermediate", "unpack", "TestValid", "content", "sub", "file.txt");
			Assert.That (File.Exists (extractedPath), Is.True, $"File should have been extracted to {extractedPath}");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void BundleResources_AreCached (bool original)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var resourceName = original ? "__monotouch_item_BundleResource_sub_sfile.txt" : "__monotouch_content_sub_sfile.txt";
			var assemblyPath = CreateAssemblyWithResource (tmpdir, "TestCache", resourceName, new byte [] { 0x41 });
			var itemsFile = Path.Combine (tmpdir, "resourcestamps", "TestCache.items");
			var assembly = new TaskItem (assemblyPath);
			assembly.SetMetadata ("ItemsFile", itemsFile);

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = [assembly];
			task.TargetFrameworkDirectory = [];
			ExecuteTask (task, expectedErrorCount: 0);

			Assert.That (task.ItemsFiles, Has.Length.EqualTo (1), "Cache file count");
			Assert.That (task.ItemsFiles [0].ItemSpec, Is.EqualTo (itemsFile), "Cache file");

			var read = CreateTask<ReadItemsFromFile> ();
			read.File = [new TaskItem (itemsFile)];
			ExecuteTask (read, expectedErrorCount: 0);

			Assert.That (read.Items, Has.Length.EqualTo (1), "Cached resource count");
			Assert.That (read.Items [0].ItemSpec, Is.EqualTo (task.BundleResourcesWithLogicalNames? [0].ItemSpec), "Resource path");
			Assert.That (read.Items [0].GetMetadata ("LogicalName"), Is.EqualTo (Path.Combine ("sub", "file.txt")), "LogicalName");
			Assert.That (read.Items [0].GetMetadata ("BundledInAssembly"), Is.EqualTo (assemblyPath), "BundledInAssembly");
			Assert.That (read.Items [0].GetMetadata ("Optimize"), Is.EqualTo ("false"), "Optimize");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void EmptyBundleResourceCache_IsWritten (bool frameworkAssembly)
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var assemblyPath = CreateAssemblyWithResource (tmpdir, "TestEmptyCache", "__monotouch_item_ImageAsset_Images.xcassets_sContents.json", new byte [] { 0x41 });
			var itemsFile = Path.Combine (tmpdir, "resourcestamps", "TestEmptyCache.items");
			var assembly = new TaskItem (assemblyPath);
			assembly.SetMetadata ("ItemsFile", itemsFile);

			var task = CreateTask<UnpackLibraryResources> ();
			task.Prefix = "monotouch";
			task.IntermediateOutputPath = Path.Combine (tmpdir, "intermediate");
			task.ReferencedLibraries = [assembly];
			task.TargetFrameworkDirectory = frameworkAssembly ? [new TaskItem (tmpdir)] : [];
			ExecuteTask (task, expectedErrorCount: 0);

			Assert.That (task.ItemsFiles, Has.Length.EqualTo (1), "Cache file count");
			Assert.That (task.ImageAssets, Has.Length.EqualTo (frameworkAssembly ? 0 : 1), "Image asset count");

			var read = CreateTask<ReadItemsFromFile> ();
			read.File = [new TaskItem (itemsFile)];
			ExecuteTask (read, expectedErrorCount: 0);
			Assert.That (read.Items, Is.Empty, "No bundle resources should be cached");
		}

		[Test]
		public void MissingCache_PreservesOtherLibrariesResources ()
		{
			var tmpdir = Cache.CreateTemporaryDirectory ();
			var intermediate = Path.Combine (tmpdir, "obj") + Path.DirectorySeparatorChar;
			var appBundle = Path.Combine (tmpdir, "Test.app");
			var root = ProjectRootElement.Create (Engine.ProjectCollection);
			var properties = root.AddPropertyGroup ();
			properties.AddProperty ("_TaskAssemblyName", typeof (UnpackLibraryResources).Assembly.Location);
			properties.AddProperty ("DeviceSpecificIntermediateOutputPath", intermediate);
			properties.AddProperty ("_CanOutputAppBundle", "true");
			properties.AddProperty ("IsMacEnabled", "true");
			properties.AddProperty ("_EmbeddedResourcePrefix", "monotouch");
			foreach (var type in new [] { "PartialAppManifest", "AtlasTexture", "ImageAsset", "InterfaceDefinition", "Collada", "CoreMLModel", "SceneKitAsset" })
				properties.AddProperty ($"_Unpacked{type}Cache", Path.Combine (intermediate, type + ".items"));

			foreach (var name in new [] { "A", "B" }) {
				var path = Path.Combine (tmpdir, name + ".dll");
				using var assembly = AssemblyDefinition.CreateAssembly (new AssemblyNameDefinition (name, new Version (1, 0)), name, ModuleKind.Dll);
				foreach (var resource in new [] { $"BundleResource_{name}.txt", $"ImageAsset_{name}.xcassets_sContents.json", $"PartialAppManifest_{name}.plist" })
					assembly.MainModule.Resources.Add (new EmbeddedResource ("__monotouch_item_" + resource, ManifestResourceAttributes.Public, new byte [] { 0x41 }));
				assembly.Write (path);
				root.AddItem ("ReferencePath", path);
			}

			root.AddImport (Path.Combine (Configuration.RootPath, "msbuild", "Xamarin.Shared", "Xamarin.Shared.targets"));
			root.AddTarget ("BuildOnlySettings");
			root.AddTarget ("ResolveReferences");
			root.AddTarget ("_DetectBuildType");
			var capture = root.AddTarget ("CaptureResources");
			capture.DependsOnTargets = "_UnpackLibraryResources;_LoadUnpackedLibraryResources;_LoadUnpackedPartialAppManifests";
			var copy = capture.AddTask ("Copy");
			copy.SetParameter ("SourceFiles", "@(_UnpackedBundleResourceWithLogicalName)");
			copy.SetParameter ("DestinationFolder", appBundle);
			var project = new Project (root, null, null, Engine.ProjectCollection);

			BuildAndAssertResources (true);

			var stamp = Path.Combine (intermediate, "resourcestamps", "A.stamp");
			Assert.That (stamp, Does.Exist, "Stamp retained when deleting its cache");
			File.Delete (Path.Combine (intermediate, "resourcestamps", "A.items"));
			Directory.Delete (appBundle, true);
			BuildAndAssertResources (true);
			Assert.That (Path.Combine (intermediate, "resourcestamps", "A.items"), Does.Exist, "Missing cache regenerated");

			Directory.Delete (appBundle, true);
			BuildAndAssertResources (false);

			void BuildAndAssertResources (bool unpackExpected)
			{
				var instance = project.CreateProjectInstance ();
				var logger = new ResourceLogger ();
				Assert.That (instance.Build ("CaptureResources", new [] { logger }), Is.True, logger.Errors);
				Assert.That (logger.UnpackExecutions, Is.EqualTo (unpackExpected ? 1 : 0), "Unpacking only runs when a cache is missing");
				Assert.That (instance.GetItems ("ImageAsset").Select (v => v.GetMetadataValue ("LogicalName")), Is.EquivalentTo (new [] { Path.Combine ("A.xcassets", "Contents.json"), Path.Combine ("B.xcassets", "Contents.json") }), "Both libraries' image assets");
				Assert.That (instance.GetItems ("PartialAppManifest").Select (v => v.GetMetadataValue ("LogicalName")), Is.EquivalentTo (new [] { "A.plist", "B.plist" }), "Both libraries' partial manifests");
				Assert.That (File.ReadAllBytes (Path.Combine (appBundle, "A.txt")), Is.EqualTo (new byte [] { 0x41 }), "Library A bundle resource");
				Assert.That (File.ReadAllBytes (Path.Combine (appBundle, "B.txt")), Is.EqualTo (new byte [] { 0x41 }), "Library B bundle resource");
			}
		}
	}
}
