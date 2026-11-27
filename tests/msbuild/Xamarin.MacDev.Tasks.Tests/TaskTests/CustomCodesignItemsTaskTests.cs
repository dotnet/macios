// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class CustomCodesignItemsTaskTests : TestBase {
		ComputeCodesignItems CreateComputeTask (params ITaskItem [] items)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var task = CreateTask<ComputeCodesignItems> ();
			task.AppBundleDir = Path.Combine (directory, "bin", "App.app");
			Directory.CreateDirectory (task.AppBundleDir);
			task.CodesignStampPath = Path.Combine (directory, "obj", "codesign");
			task.CodesignItems = items;
			task.TargetFrameworkMoniker = TargetFramework.DotNet_MacCatalyst_String;
			return task;
		}

		static TaskItem CustomItem (string path, string pattern = "", string owner = "App.app")
		{
			var item = new TaskItem (owner + "/" + path);
			item.SetMetadata ("CodesignItemPath", path);
			item.SetMetadata ("CodesignItemAppBundle", owner);
			item.SetMetadata ("FilePattern", pattern);
			item.SetMetadata ("CodesignSigningKey", "-");
			return item;
		}

		static string WriteMachO (string directory, string relativePath, uint fileType = 2)
		{
			var path = Path.Combine (directory, relativePath);
			Directory.CreateDirectory (Path.GetDirectoryName (path) ?? throw new InvalidOperationException ());
			using var writer = new BinaryWriter (File.Create (path));
			writer.Write (0xfeedfacfu);
			writer.Write (0x01000007);
			writer.Write (3);
			writer.Write (fileType);
			writer.Write (new byte [16]);
			return path;
		}

		[TestCase ("Contents/MacOS/ffmpeg")]
		[TestCase (@"Contents\MacOS\ffmpeg")]
		public void ExplicitFile (string include)
		{
			var item = CustomItem (include);
			item.SetMetadata ("CodesignEntitlements", "Helper.entitlements");
			var task = CreateComputeTask (item);
			var path = WriteMachO (task.AppBundleDir, "Contents/MacOS/ffmpeg");
			ExecuteTask (task);
			Assert.That (task.OutputCodesignItems.Select (v => v.ItemSpec), Is.EqualTo (new [] { PathUtils.ResolveSymbolicLinks (path) }), "File to sign");
			Assert.That (task.OutputCodesignItems [0].GetMetadata ("CodesignEntitlements"), Is.EqualTo ("Helper.entitlements"), "Helper entitlements");
			Assert.That (task.OutputCodesignItems [0].GetMetadata ("CodesignStampFile"), Is.EqualTo (Path.Combine (task.CodesignStampPath, "App.app", "Contents/MacOS/ffmpeg")), "Stamp");
		}

		[TestCase ("*", new [] { "ffmpeg", "libcodec.dylib", "plugin.so" })]
		[TestCase ("**/*", new [] { "ffmpeg", "libcodec.dylib", "plugin.so", "sub/ffprobe" })]
		[TestCase ("**/ff*", new [] { "ffmpeg", "sub/ffprobe" })]
		[TestCase ("sub/ffprob?", new [] { "sub/ffprobe" })]
		[TestCase ("*", new [] { "ffmpeg", "libcodec.dylib", "plugin.so" }, true)]
		[TestCase ("**/*", new [] { "ffmpeg", "libcodec.dylib", "plugin.so", "sub/ffprobe" }, true)]
		[TestCase ("**/ff*", new [] { "ffmpeg", "sub/ffprobe" }, true)]
		[TestCase ("sub/ffprob?", new [] { "sub/ffprobe" }, true)]
		public void Discovery (string pattern, string [] expected, bool relativeBundlePath = false)
		{
			var task = CreateComputeTask (CustomItem ("Contents/Helpers", pattern));
			if (relativeBundlePath)
				task.AppBundleDir = PathUtils.AbsoluteToRelative (Environment.CurrentDirectory, task.AppBundleDir);
			var directory = Path.Combine (task.AppBundleDir, "Contents/Helpers");
			WriteMachO (directory, "ffmpeg");
			WriteMachO (directory, "sub/ffprobe");
			WriteMachO (directory, "libcodec.dylib", 6);
			WriteMachO (directory, "plugin.so", 8);
			WriteMachO (directory, "object.o", 1);
			WriteMachO (directory, "debug.dSYM", 10);
			File.WriteAllText (Path.Combine (directory, "README"), "not code");
			File.WriteAllText (Path.Combine (directory, "archive.a"), "!<arch>\n");
			File.WriteAllBytes (Path.Combine (directory, "Java.class"), new byte [] { 0xca, 0xfe, 0xba, 0xbe, 0, 0, 0, 61, 0, 1 });
			File.WriteAllBytes (Path.Combine (directory, "JavaPreview.class"), new byte [] { 0xca, 0xfe, 0xba, 0xbe, 0xff, 0xff, 0, 61, 0, 1 });
			foreach (var file in new [] { "Java.class", "JavaPreview.class" }) {
				using var stream = File.OpenWrite (Path.Combine (directory, file));
				stream.SetLength (4096);
			}
			ExecuteTask (task);
			Assert.That (task.OutputCodesignItems.Select (v => v.ItemSpec), Is.EquivalentTo (expected.Select (v => PathUtils.ResolveSymbolicLinks (Path.GetFullPath (Path.Combine (directory, v))))), "Discovered code");
		}

		[Test]
		public void UniversalExecutable ()
		{
			var task = CreateComputeTask (CustomItem ("ffmpeg"));
			var thin = WriteMachO (task.AppBundleDir, "thin");
			var header = File.ReadAllBytes (thin);
			using (var writer = new BinaryWriter (File.Create (Path.Combine (task.AppBundleDir, "ffmpeg")))) {
				writer.Write (0xcafebabeu);
				writer.Write (2);
				for (var i = 0; i < 2; i++) {
					writer.Write (0x01000007);
					writer.Write (3);
					writer.Write (48 + i * header.Length);
					writer.Write (header.Length);
					writer.Write (0);
				}
				writer.Write (header);
				writer.Write (header);
			}
			ExecuteTask (task);
			Assert.That (task.OutputCodesignItems, Has.Length.EqualTo (1), "Universal file");
		}

		[TestCase ("../outside", "")]
		[TestCase (@"..\outside", "")]
		[TestCase ("/bin/echo", "")]
		[TestCase (@"C:\outside", "")]
		[TestCase ("Contents/Helpers", "../*")]
		[TestCase ("Contents/Helpers", @"..\*")]
		[TestCase ("Contents/Helpers", "/bin/*")]
		[TestCase ("Contents/Helpers", "sub/../../*")]
		public void RejectsTraversal (string path, string pattern)
		{
			var task = CreateComputeTask (CustomItem (path, pattern));
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("must be relative paths without '..' segments"), "Diagnostic");
			Assert.That (task.OutputCodesignItems, Is.Empty, "No signing output");
		}

		[TestCase ("missing", "", "file")]
		[TestCase ("missing", "**/*", "search directory")]
		[TestCase ("Contents", "", "file")]
		public void RejectsMissingFiles (string path, string pattern, string diagnostic)
		{
			var task = CreateComputeTask (CustomItem (path, pattern));
			Directory.CreateDirectory (Path.Combine (task.AppBundleDir, "Contents"));
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain (diagnostic), "Diagnostic");
		}

		[TestCase (1u)]
		[TestCase (10u)]
		public void RejectsUnsupportedMachO (uint type)
		{
			var task = CreateComputeTask (CustomItem ("file"));
			WriteMachO (task.AppBundleDir, "file", type);
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("not a Mach-O executable"), "Diagnostic");
		}

		[Test]
		public void RejectsNonCode ()
		{
			var task = CreateComputeTask (CustomItem ("README"));
			File.WriteAllText (Path.Combine (task.AppBundleDir, "README"), "not code");
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("not a Mach-O executable"), "Diagnostic");
		}

		[TestCase ("")]
		[TestCase ("**/*")]
		public void RejectsMalformedMachO (string pattern)
		{
			var task = CreateComputeTask (CustomItem (string.IsNullOrEmpty (pattern) ? "Contents/tool" : "Contents", pattern));
			var path = WriteMachO (task.AppBundleDir, "Contents/tool");
			using (var stream = File.OpenWrite (path))
				stream.SetLength (4);
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("Could not read the CodesignItem file"), "Diagnostic");
			Assert.That (task.OutputCodesignItems, Is.Empty, "No signing output");
		}

		[Test]
		public void RejectsInvalidPattern ()
		{
			var task = CreateComputeTask (CustomItem ("Contents", "sub/**suffix"));
			Directory.CreateDirectory (Path.Combine (task.AppBundleDir, "Contents"));
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("not a valid MSBuild wildcard pattern"), "Diagnostic");
		}

		[TestCase (false, false)]
		[TestCase (true, false)]
		[TestCase (true, true)]
		public void RejectsSymlinkEscape (bool directoryLink, bool discovery)
		{
			Configuration.IgnoreIfNotOnMacOS ();
			var task = CreateComputeTask (CustomItem (discovery ? "Contents" : directoryLink ? "Contents/link/ffmpeg" : "Contents/link", discovery ? "**/*" : ""));
			var outside = Path.Combine (Path.GetDirectoryName (task.AppBundleDir) ?? throw new InvalidOperationException (), "App.app-other");
			var executable = WriteMachO (outside, "ffmpeg");
			Directory.CreateDirectory (Path.Combine (task.AppBundleDir, "Contents"));
			PathUtils.CreateSymlink (Path.Combine (task.AppBundleDir, "Contents/link"), directoryLink ? outside : executable);
			ExecuteTask (task, 1);
			Assert.That (Engine.Logger.ErrorEvents [0].Message, Does.Contain ("outside its app bundle"), "Diagnostic");
			Assert.That (task.OutputCodesignItems, Is.Empty, "No signing output");
		}

		[TestCase ("App.app/PlugIns/Helper.appex")]
		[TestCase ("App.app/Watch/Helper.app")]
		public void NestedOwner (string owner)
		{
			var item = CustomItem ("Contents/MacOS/tool", owner: owner);
			var task = CreateComputeTask (item);
			var path = WriteMachO (Path.GetDirectoryName (task.AppBundleDir) ?? throw new InvalidOperationException (), owner + "/Contents/MacOS/tool");
			ExecuteTask (task);
			Assert.That (task.OutputCodesignItems.Select (v => v.ItemSpec), Is.EqualTo (new [] { PathUtils.ResolveSymbolicLinks (path) }), "Nested tool");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void RejectsEscapingNestedOwner (bool symlink)
		{
			var owner = "App.app/PlugIns/Helper.appex";
			var task = CreateComputeTask (CustomItem (symlink ? "tool" : "../sibling", owner: owner));
			if (symlink) {
				Configuration.IgnoreIfNotOnMacOS ();
				var outside = Path.Combine (Cache.CreateTemporaryDirectory (), "Helper.appex");
				WriteMachO (outside, "tool");
				Directory.CreateDirectory (Path.Combine (task.AppBundleDir, "PlugIns"));
				PathUtils.CreateSymlink (Path.Combine (Path.GetDirectoryName (task.AppBundleDir) ?? throw new InvalidOperationException (), owner), outside);
			}
			ExecuteTask (task, 1);
			Assert.That (task.OutputCodesignItems, Is.Empty, "No signing output");
		}

		[Test]
		public void ExplicitOverridesDiscovery ()
		{
			Configuration.IgnoreIfNotOnMacOS ();
			var explicitItem = CustomItem ("Contents/Helpers/libcodec.dylib");
			explicitItem.SetMetadata ("CodesignEntitlements", "Helper.entitlements");
			var task = CreateComputeTask (CustomItem ("Contents/Helpers", "**/*"), explicitItem, CustomItem ("Contents/Helpers/alias"));
			var file = WriteMachO (task.AppBundleDir, "Contents/Helpers/libcodec.dylib", 6);
			PathUtils.CreateSymlink (Path.Combine (task.AppBundleDir, "Contents/Helpers/alias"), "libcodec.dylib");
			task.CodesignItems [2].SetMetadata ("CodesignEntitlements", "Helper.entitlements");
			task.CodesignBundle = new [] { new TaskItem ("App.app", new Dictionary<string, string> { { "CodesignSigningKey", "-" }, { "CodesignEntitlements", "App.entitlements" } }) };
			ExecuteTask (task);
			var items = task.OutputCodesignItems.Where (v => v.ItemSpec == PathUtils.ResolveSymbolicLinks (file)).ToArray ();
			Assert.That (items, Has.Length.EqualTo (1), "Sign library only once");
			Assert.That (items [0].GetMetadata ("CodesignEntitlements"), Is.EqualTo ("Helper.entitlements"), "Explicit entitlements");
			Assert.That (Engine.Logger.WarningsEvents, Is.Empty, "Identical declarations");
		}

		[Test]
		public void ConflictingExplicitMetadata ()
		{
			var first = CustomItem ("ffmpeg");
			var second = CustomItem ("ffmpeg");
			first.SetMetadata ("CodesignEntitlements", "First.entitlements");
			second.SetMetadata ("CodesignEntitlements", "Second.entitlements");
			var task = CreateComputeTask (first, second);
			WriteMachO (task.AppBundleDir, "ffmpeg");
			ExecuteTask (task);
			Assert.That (task.OutputCodesignItems, Has.Length.EqualTo (1), "Deduplicated");
			Assert.That (Engine.Logger.WarningsEvents, Has.Count.EqualTo (1), "Conflicting metadata");
		}

		[Test]
		public void SkipCustomFile ()
		{
			var task = CreateComputeTask (CustomItem ("Contents/Helpers", "**/*"));
			WriteMachO (task.AppBundleDir, "Contents/Helpers/ffmpeg");
			task.SkipCodesignItems = new [] { new TaskItem ("Contents/Helpers") };
			ExecuteTask (task);
			Assert.That (task.OutputCodesignItems, Is.Empty, "Excluded directory");
		}

		[Test]
		public void SignsHelperBeforeApp ()
		{
			Configuration.IgnoreIfNotOnMacOS ();
			var item = CustomItem ("Contents/MacOS/ffmpeg");
			item.SetMetadata ("CodesignExtraArgs", "--identifier=com.example.helper");
			item.SetMetadata ("CodesignUseHardenedRuntime", "true");
			var task = CreateComputeTask (item);
			var macOS = Path.Combine (task.AppBundleDir, "Contents/MacOS");
			Directory.CreateDirectory (macOS);
			var helper = Path.Combine (macOS, "ffmpeg");
			File.Copy ("/bin/echo", helper);
			File.Copy ("/usr/bin/true", Path.Combine (macOS, "App"));
			new PDictionary {
				{ "CFBundleExecutable", new PString ("App") },
				{ "CFBundleIdentifier", new PString ("com.example.app") },
				{ "CFBundlePackageType", new PString ("APPL") },
			}.Save (Path.Combine (task.AppBundleDir, "Contents/Info.plist"));
			var entitlements = Path.Combine (Path.GetDirectoryName (task.AppBundleDir) ?? throw new InvalidOperationException (), "Helper.entitlements");
			var plist = new PDictionary { { "com.apple.security.cs.allow-jit", new PBoolean (false) } };
			plist.Save (entitlements);
			item.SetMetadata ("SourceProjectPath", Path.GetDirectoryName (entitlements) ?? throw new InvalidOperationException ());
			item.SetMetadata ("CodesignEntitlements", Path.GetFileName (entitlements));
			task.CodesignBundle = new [] { new TaskItem ("App.app", new Dictionary<string, string> { { "CodesignSigningKey", "-" } }) };
			ExecuteTask (task);

			var sign = CreateTask<Codesign> ();
			sign.CodesignAllocate = Path.Combine (Configuration.XcodeLocation, "Toolchains/XcodeDefault.xctoolchain/usr/bin/codesign_allocate");
			sign.Resources = task.OutputCodesignItems;
			sign.TargetFrameworkMoniker = task.TargetFrameworkMoniker;
			ExecuteTask (sign);
			Assert.That (ExecutionHelper.Execute ("/usr/bin/codesign", new [] { "--verify", "--deep", "--strict", task.AppBundleDir }, out var verifyOutput), Is.EqualTo (0), verifyOutput.ToString ());
			Assert.That (ExecutionHelper.Execute (helper, new [] { "ready" }, out var helperOutput), Is.EqualTo (0), helperOutput.ToString ());
			Assert.That (helperOutput.ToString ().Trim (), Is.EqualTo ("ready"), "Signed helper runs");

			var stamps = task.OutputCodesignItems.Select (v => v.GetMetadata ("CodesignStampFile")).ToArray ();
			var timestamps = stamps.Select (File.GetLastWriteTimeUtc).ToArray ();
			ExecuteTask (sign);
			Assert.That (stamps.Select (File.GetLastWriteTimeUtc), Is.EqualTo (timestamps), "No-op signing");
			plist ["com.apple.security.cs.allow-jit"] = new PBoolean (true);
			plist.Save (entitlements);
			ExecuteTask (sign);
			for (var i = 0; i < stamps.Length; i++)
				Assert.That (File.GetLastWriteTimeUtc (stamps [i]), Is.GreaterThan (timestamps [i]), "Updated helper entitlements: " + stamps [i]);
			Assert.That (ExecutionHelper.Execute ("/usr/bin/codesign", new [] { "--verify", "--deep", "--strict", task.AppBundleDir }, out verifyOutput), Is.EqualTo (0), verifyOutput.ToString ());
		}

		[TestCase (false, "")]
		[TestCase (true, "")]
		[TestCase (true, "Contents/PlugIns/")]
		[TestCase (true, "Watch/")]
		public void CollectsPublicItems (bool customSettings, string parentContainer)
		{
			var directory = Cache.CreateTemporaryDirectory ();
			var projectPath = Path.Combine (directory, "Test.proj");
			var item = new XElement ("CodesignItem", new XAttribute ("Include", "Contents/MacOS/ffmpeg"));
			if (customSettings) {
				item.Add (new XElement ("CodesignEntitlements", "Helper.entitlements"));
				item.Add (new XElement ("CodesignSigningKey", "helper-key"));
				item.Add (new XElement ("CodesignUseHardenedRuntime", "false"));
			}
			new XDocument (new XElement ("Project",
				new XElement ("PropertyGroup",
					new XElement ("_TaskAssemblyName", typeof (ComputeCodesignItems).Assembly.Location),
					new XElement ("_PlatformName", "macOS"),
					new XElement ("_AppBundleName", "App"),
					new XElement ("AppBundleExtension", ".app"),
					new XElement ("IsAppExtension", parentContainer == "Contents/PlugIns/" ? "true" : "false"),
					new XElement ("_AppPlugInsRelativePath", "Contents/PlugIns/"),
					new XElement ("_CodeSigningKey", "app-key"),
					new XElement ("_CodesignAllocate", "/codesign_allocate"),
					new XElement ("_CompiledCodesignEntitlements", "App.entitlements"),
					new XElement ("UseHardenedRuntime", "true"),
					new XElement ("DeviceSpecificOutputPath", directory + "/")),
				new XElement ("ItemGroup", item),
				new XElement ("Import", new XAttribute ("Project", Path.Combine (Configuration.RootPath, "msbuild/Xamarin.Shared/Xamarin.Shared.targets"))),
				new XElement ("Target", new XAttribute ("Name", "_PlaceAppExtensions"))
			)).Save (projectPath);
			using var collection = new ProjectCollection ();
			var project = new ProjectInstance (projectPath, null, null, collection);
			Assert.That (project.Build ("_StoreCodesigningData", new ILogger [] { Engine.Logger }), Is.True, "Collect and store");
			var reader = CreateTask<ReadItemsFromFile> ();
			reader.File = new [] { new TaskItem (Path.Combine (directory, "codesign.items")) };
			ExecuteTask (reader);
			var result = reader.Items.Single ();
			Assert.That (result.ItemSpec, Is.EqualTo ("App.app/Contents/MacOS/ffmpeg"), "Bundle path");
			Assert.That (result.GetMetadata ("CodesignItemPath"), Is.EqualTo ("Contents/MacOS/ffmpeg"), "Original include");
			Assert.That (result.GetMetadata ("CodesignItemAppBundle"), Is.EqualTo ("App.app"), "Owner");
			Assert.That (result.GetMetadata ("CodesignAllocate"), Is.EqualTo ("/codesign_allocate"), "Signing tool");
			Assert.That (result.GetMetadata ("CodesignSigningKey"), Is.EqualTo (customSettings ? "helper-key" : "app-key"), "Signing key");
			Assert.That (result.GetMetadata ("CodesignUseHardenedRuntime"), Is.EqualTo (customSettings ? "false" : "true"), "Hardened runtime");
			Assert.That (result.GetMetadata ("CodesignEntitlements"), Is.EqualTo (customSettings ? "Helper.entitlements" : ""), "Do not inherit app entitlements");
			if (string.IsNullOrEmpty (parentContainer))
				return;

			var parentDirectory = Path.Combine (directory, "parent");
			Directory.CreateDirectory (parentDirectory);
			var parentProjectPath = Path.Combine (parentDirectory, "Parent.proj");
			new XDocument (new XElement ("Project",
				new XElement ("PropertyGroup",
					new XElement ("_TaskAssemblyName", typeof (ComputeCodesignItems).Assembly.Location),
					new XElement ("_PlatformName", "macOS"),
					new XElement ("_AppBundleName", "Main"),
					new XElement ("AppBundleExtension", ".app"),
					new XElement ("DeviceSpecificOutputPath", parentDirectory + "/")),
				new XElement ("ItemGroup",
					new XElement (parentContainer == "Watch/" ? "_ResolvedWatchAppReferences" : "_ResolvedAppExtensionReferences", new XAttribute ("Include", Path.Combine (directory, "App.app")))),
				new XElement ("Import", new XAttribute ("Project", Path.Combine (Configuration.RootPath, "msbuild/Xamarin.Shared/Xamarin.Shared.targets"))),
				new XElement ("Target", new XAttribute ("Name", "_PlaceAppExtensions"))
			)).Save (parentProjectPath);
			var parentProject = new ProjectInstance (parentProjectPath, null, null, collection);
			Assert.That (parentProject.Build ("_StoreCodesigningData", new ILogger [] { Engine.Logger }), Is.True, "Collect nested signing requests");
			reader = CreateTask<ReadItemsFromFile> ();
			reader.File = new [] { new TaskItem (Path.Combine (parentDirectory, "codesign.items")) };
			ExecuteTask (reader);
			var nested = reader.Items.Single ();
			var owner = "Main.app/" + parentContainer + "App.app";
			Assert.That (nested.ItemSpec, Is.EqualTo (owner + "/Contents/MacOS/ffmpeg"), "Nested item path");
			Assert.That (nested.GetMetadata ("CodesignItemAppBundle"), Is.EqualTo (owner), "Nested owner");
			Assert.That (nested.GetMetadata ("SourceProjectPath"), Is.EqualTo (directory), "Original entitlement directory");

			var compute = CreateComputeTask (reader.Items);
			compute.AppBundleDir = Path.Combine (parentDirectory, "Main.app");
			var helper = WriteMachO (parentDirectory, nested.ItemSpec);
			ExecuteTask (compute);
			Assert.That (compute.OutputCodesignItems.Select (v => v.ItemSpec), Is.EqualTo (new [] { PathUtils.ResolveSymbolicLinks (helper) }), "Nested signing destination");
		}
	}
}
