// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using Microsoft.Build.Utilities;

using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class PrepareNativeReferencesTaskTests : TestBase {
		[TestCase ("/System/Library/Frameworks/ScriptingBridge.framework")]
		[TestCase ("/System/Library/Frameworks/ScriptingBridge.framework/")]
		[TestCase (@"\System\Library\Frameworks\ScriptingBridge.framework\")]
		public void SystemFramework (string path)
		{
			var framework = new TaskItem (path);
			framework.SetMetadata ("Kind", "Framework");
			framework.SetMetadata ("Frameworks", "Foundation");
			framework.SetMetadata ("WeakFrameworks", "AppKit");
			framework.SetMetadata ("LinkerFlags", "-lz");
			var task = CreateTask<PrepareNativeReferences> ();
			task.IntermediateOutputPath = Cache.CreateTemporaryDirectory ();
			task.NativeReferences = [framework];

			ExecuteTask (task);

			Assert.That (task.NativeFrameworks, Is.Empty, "Frameworks to embed");
			Assert.That (task.EmbeddedResources, Is.Empty, "Embedded resources");
			Assert.That (task.ShouldCopyToBuildServer (framework), Is.False, "Copy to Mac");
			Assert.That (task.GetAdditionalItemsToBeCopied (), Is.Empty, "Additional files to copy");
			var attributes = task.LinkWithAttributes ?? throw new AssertionException ("No generated attributes");
			Assert.That (File.ReadAllText (attributes.ItemSpec), Is.EqualTo (
				"[assembly: ObjCRuntime.LinkWith (Frameworks = \"Foundation ScriptingBridge\", SmartLink = true, WeakFrameworks = \"AppKit\", LinkerFlags = \"-lz\")]" + System.Environment.NewLine), "LinkWith");
		}

		[TestCase ("Custom.framework", "Framework")]
		[TestCase ("/Library/Frameworks/Custom.framework", "Framework")]
		[TestCase ("/System/Library/FrameworksExtra/Custom.framework", "Framework")]
		[TestCase ("/System/Library/Frameworks/Custom.a", "Static")]
		public void NonSystemReference (string path, string kind)
		{
			var reference = new TaskItem (path);
			reference.SetMetadata ("Kind", kind);
			var task = CreateTask<PrepareNativeReferences> ();
			task.IntermediateOutputPath = Cache.CreateTemporaryDirectory ();
			task.NativeReferences = [reference];

			ExecuteTask (task);

			Assert.That (task.NativeFrameworks.Concat (task.EmbeddedResources).Select (v => v.ItemSpec), Is.EqualTo (new [] { path }), "References to embed");
			Assert.That (task.ShouldCopyToBuildServer (reference), Is.True, "Copy to Mac");
			var attributes = task.LinkWithAttributes ?? throw new AssertionException ("No generated attributes");
			Assert.That (File.ReadAllText (attributes.ItemSpec), Is.EqualTo (
				$"[assembly: ObjCRuntime.LinkWith (\"{Path.GetFileName (path)}\", SmartLink = true)]" + System.Environment.NewLine), "LinkWith");
		}
	}
}
