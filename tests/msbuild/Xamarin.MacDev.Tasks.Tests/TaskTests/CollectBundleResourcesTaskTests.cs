using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using NUnit.Framework;

using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class CollectBundleResourcesTaskTests : TestBase {
		[Test]
		public void LogicalNameOutsideAppBundle ()
		{
			var currentDirectory = Environment.CurrentDirectory;
			try {
				var tmpdir = Cache.CreateTemporaryDirectory ();
				var task = CreateTask<CollectBundleResources> ();
				var item = new TaskItem ("image.png");
				var projDir = Path.Combine (tmpdir, "B");
				Directory.CreateDirectory (projDir);
				Environment.CurrentDirectory = projDir;
				File.WriteAllText (Path.Combine (projDir, item.ItemSpec), "image!");
				item.SetMetadata ("LocalDefiningProjectFullPath", Path.Combine (tmpdir, "A", "SDK.csproj"));
				item.SetMetadata ("LocalMSBuildProjectFullPath", Path.Combine (projDir, "Project.csproj"));
				task.BundleResources = [item];
				ExecuteTask (task);
				Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (1), "Warnings");
				Assert.That (Engine.Logger.WarningsEvents [0].Message, Is.EqualTo ("The path '../B/image.png' would result in a file outside of the app bundle and cannot be used."), "Warning Message");
			} finally {
				Environment.CurrentDirectory = currentDirectory;
			}
		}
	}
}
