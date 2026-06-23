using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks {

	// Regression tests for the containment check that makes sure a reidentified native library is never
	// written outside the intended intermediate output directory, even if the 'ReidentifiedPath' was
	// influenced by metadata that originates from a (passive, potentially untrusted) binding resource
	// package manifest.
	[TestFixture]
	public class InstallNameToolTaskTest : TestBase {

		[Test]
		public void IsPathContained_Contained ()
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var root = Path.Combine (tmp, "obj", "nativelibraries");
			Directory.CreateDirectory (root);

			Assert.That (InstallNameTool.IsPathContained (root, Path.Combine (root, "Contents", "MonoBundle", "lib.dylib")), Is.True, "subdir");
			Assert.That (InstallNameTool.IsPathContained (root, Path.Combine (root, "lib.dylib")), Is.True, "direct child");
			// A trailing separator on the root must not change the result.
			Assert.That (InstallNameTool.IsPathContained (root + Path.DirectorySeparatorChar, Path.Combine (root, "lib.dylib")), Is.True, "trailing separator root");
			// A Windows-style (backslash) root must be normalized to match a slash-normalized target
			// (this happens on remote Windows -> Mac builds).
			Assert.That (InstallNameTool.IsPathContained (root.Replace (Path.DirectorySeparatorChar, '\\'), Path.Combine (root, "lib.dylib")), Is.True, "backslash root");
		}

		[Test]
		public void IsPathContained_NotContained ()
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var root = Path.Combine (tmp, "obj", "nativelibraries");
			Directory.CreateDirectory (root);

			// '..' traversal that escapes the root.
			Assert.That (InstallNameTool.IsPathContained (root, Path.Combine (root, "..", "..", "escape.dylib")), Is.False, "traversal");
			// An absolute path outside the root.
			Assert.That (InstallNameTool.IsPathContained (root, Path.Combine (tmp, "escape.dylib")), Is.False, "outside");
			// A sibling directory that merely shares the root as a string prefix must not be considered contained.
			Assert.That (InstallNameTool.IsPathContained (root, root + "EVIL" + Path.DirectorySeparatorChar + "lib.dylib"), Is.False, "sibling prefix");
			// The root itself isn't a contained target (it's a directory, not a file under the root).
			Assert.That (InstallNameTool.IsPathContained (root, root), Is.False, "root itself");
			// Empty inputs are never contained.
			Assert.That (InstallNameTool.IsPathContained ("", Path.Combine (root, "lib.dylib")), Is.False, "empty root");
			Assert.That (InstallNameTool.IsPathContained (root, ""), Is.False, "empty target");
		}

		[Test]
		public void IsPathContained_SymlinkEscape ()
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var root = Path.Combine (tmp, "obj", "nativelibraries");
			Directory.CreateDirectory (root);
			var outside = Path.Combine (tmp, "outside");
			Directory.CreateDirectory (outside);

			// A symlink inside the root that points outside the root must not be usable to escape.
			var link = Path.Combine (root, "link");
			PathUtils.CreateSymlink (link, outside);

			Assert.That (InstallNameTool.IsPathContained (root, Path.Combine (link, "evil.dylib")), Is.False, "symlink escape");
		}

		[TestCase ("traversal")]
		[TestCase ("absolute")]
		[TestCase ("mixedseparators")]
		public void RefusesToWriteOutOfRoot (string kind)
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var root = Path.Combine (tmp, "obj", "nativelibraries");
			Directory.CreateDirectory (root);
			var src = Path.Combine (tmp, "libpayload.dylib");
			File.WriteAllText (src, "fake dylib");
			var escapeTarget = Path.Combine (tmp, "ESCAPED", "libpayload.dylib");

			string reidentifiedPath;
			switch (kind) {
			case "traversal":
				reidentifiedPath = Path.Combine (root, "..", "..", "ESCAPED", "libpayload.dylib");
				break;
			case "absolute":
				reidentifiedPath = escapeTarget;
				break;
			case "mixedseparators":
				reidentifiedPath = root + @"\..\..\ESCAPED\libpayload.dylib";
				break;
			default:
				throw new System.NotImplementedException (kind);
			}

			var task = CreateTask<InstallNameTool> ();
			task.IntermediateNativeLibraryDir = root;
			var item = new TaskItem (src);
			item.SetMetadata ("ReidentifiedPath", reidentifiedPath);
			item.SetMetadata ("DynamicLibraryId", "@executable_path/libpayload.dylib");
			task.DynamicLibrary = new ITaskItem [] { item };

			ExecuteTask (task, 1);

			// Nothing was created outside the intended directory (not even the temporary file).
			Assert.That (Path.Combine (tmp, "ESCAPED"), Does.Not.Exist, "no escaped directory");
			Assert.That (escapeTarget, Does.Not.Exist, "no escaped file");
			Assert.That (escapeTarget + ".tmp", Does.Not.Exist, "no escaped temp file");
		}
	}
}
