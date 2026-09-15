using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using NUnit.Framework;

using Xamarin.MacDev;
using Xamarin.MacDev.Tasks;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class UtilityTests {
		[Test]
		public void TestAbsoluteToRelativePath ()
		{
			string rpath;

			rpath = PathUtils.AbsoluteToRelative ("/Users/user/source/Project", "/Users/user/Source/Project/Info.plist");
			Assert.That (rpath, Is.EqualTo ("Info.plist"), "#1");
		}

		[Test]
		public void IsPathContained_Contained ()
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var root = Path.Combine (tmp, "obj", "nativelibraries");
			Directory.CreateDirectory (root);

			Assert.That (PathUtils.IsPathContained (root, Path.Combine (root, "Contents", "MonoBundle", "lib.dylib")), Is.True, "subdir");
			Assert.That (PathUtils.IsPathContained (root, Path.Combine (root, "lib.dylib")), Is.True, "direct child");
			// A trailing separator on the root must not change the result.
			Assert.That (PathUtils.IsPathContained (root + Path.DirectorySeparatorChar, Path.Combine (root, "lib.dylib")), Is.True, "trailing separator root");
			// A Windows-style (backslash) root must be normalized to match a slash-normalized target
			// (this happens on remote Windows -> Mac builds).
			Assert.That (PathUtils.IsPathContained (root.Replace (Path.DirectorySeparatorChar, '\\'), Path.Combine (root, "lib.dylib")), Is.True, "backslash root");
		}

		[Test]
		public void IsPathContained_NotContained ()
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var root = Path.Combine (tmp, "obj", "nativelibraries");
			Directory.CreateDirectory (root);

			// '..' traversal that escapes the root.
			Assert.That (PathUtils.IsPathContained (root, Path.Combine (root, "..", "..", "escape.dylib")), Is.False, "traversal");
			// An absolute path outside the root.
			Assert.That (PathUtils.IsPathContained (root, Path.Combine (tmp, "escape.dylib")), Is.False, "outside");
			// A sibling directory that merely shares the root as a string prefix must not be considered contained.
			Assert.That (PathUtils.IsPathContained (root, root + "EVIL" + Path.DirectorySeparatorChar + "lib.dylib"), Is.False, "sibling prefix");
			// The root itself isn't a contained target (it's a directory, not a file under the root).
			Assert.That (PathUtils.IsPathContained (root, root), Is.False, "root itself");
			// The root itself with trailing separator(s) is still not a contained target.
			Assert.That (PathUtils.IsPathContained (root, root + Path.DirectorySeparatorChar), Is.False, "root itself + separator");
			Assert.That (PathUtils.IsPathContained (root + Path.DirectorySeparatorChar, root + Path.DirectorySeparatorChar), Is.False, "root+sep vs root+sep");
			// Empty inputs are never contained.
			Assert.That (PathUtils.IsPathContained ("", Path.Combine (root, "lib.dylib")), Is.False, "empty root");
			Assert.That (PathUtils.IsPathContained (root, ""), Is.False, "empty target");
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

			Assert.That (PathUtils.IsPathContained (root, Path.Combine (link, "evil.dylib")), Is.False, "symlink escape");
		}
	}
}
