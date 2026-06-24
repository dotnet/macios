using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {

	// Tests for the InstallNameTool task: a reidentified native library must never be written outside the
	// intended intermediate output directory, even if the 'ReidentifiedPath' was influenced by metadata
	// that originates from a (passive) binding resource package manifest.
	[TestFixture]
	public class InstallNameToolTaskTest : TestBase {

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

		[Test]
		public void CleansUpTemporaryFileWhenInstallNameToolFails ()
		{
			// When install_name_tool fails (here: the input isn't a valid Mach-O file) the temporary copy
			// must not be left behind.
			var tmp = Cache.CreateTemporaryDirectory ();
			var root = Path.Combine (tmp, "obj", "nativelibraries");
			Directory.CreateDirectory (root);
			var src = Path.Combine (tmp, "libpayload.dylib");
			File.WriteAllText (src, "this is not a valid Mach-O file");
			var target = Path.Combine (root, "Contents", "MonoBundle", "libpayload.dylib");

			var task = CreateTask<InstallNameTool> ();
			task.IntermediateNativeLibraryDir = root;
			var item = new TaskItem (src);
			item.SetMetadata ("ReidentifiedPath", target);
			item.SetMetadata ("DynamicLibraryId", "@executable_path/libpayload.dylib");
			task.DynamicLibrary = new ITaskItem [] { item };

			ExecuteTask (task, 1);

			Assert.That (target + ".tmp", Does.Not.Exist, "temporary file cleaned up");
			Assert.That (target, Does.Not.Exist, "target not created on failure");
		}
	}
}
