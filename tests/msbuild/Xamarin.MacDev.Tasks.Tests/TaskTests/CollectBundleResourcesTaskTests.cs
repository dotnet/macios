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

		// Ref: https://github.com/dotnet/macios/issues/23898
		// Items defined by an SDK (not default items) where the defining
		// project is in a completely different directory tree than the
		// project should still resolve their LogicalName correctly
		// when ResolveResourceItemsRelativeToProject is enabled.
		[TestCase (true)]  // .NET 12+ default: resolve relative to project
		[TestCase (false)] // < .NET 12 default: resolve relative to defining project (legacy behavior)
		public void ContentDefinedBySdkFarFromProject (bool resolveRelativeToProject)
		{
			var currentDirectory = Environment.CurrentDirectory;
			try {
				var tmpdir = Cache.CreateTemporaryDirectory ();

				// Simulate the project directory
				var projDir = Path.Combine (tmpdir, "src", "MyProject");
				Directory.CreateDirectory (projDir);

				// Simulate an SDK directory far from the project (like a Razor/Blazor SDK)
				var sdkDir = Path.Combine (tmpdir, "sdk", "packs", "Microsoft.NET.Sdk.Razor", "10.0.0", "build");
				Directory.CreateDirectory (sdkDir);

				Environment.CurrentDirectory = projDir;

				// Create several content files in the project directory
				var files = new [] { "wwwroot/background.png", "wwwroot/script.js", "Component1.razor" };
				var items = new List<ITaskItem> ();
				foreach (var file in files) {
					var fullPath = Path.Combine (projDir, file);
					Directory.CreateDirectory (Path.GetDirectoryName (fullPath)!);
					File.WriteAllText (fullPath, "content");

					var item = new TaskItem (file);
					// The defining project is the SDK file, not a default item
					item.SetMetadata ("LocalDefiningProjectFullPath", Path.Combine (sdkDir, "Microsoft.NET.Sdk.Razor.DefaultItems.props"));
					item.SetMetadata ("LocalMSBuildProjectFullPath", Path.Combine (projDir, "MyProject.csproj"));
					items.Add (item);
				}

				var task = CreateTask<CollectBundleResources> ();
				task.ProjectDir = projDir + Path.DirectorySeparatorChar;
				task.ResourcePrefix = "";
				task.ResolveResourceItemsRelativeToProject = resolveRelativeToProject;
				task.BundleResources = items.ToArray ();
				ExecuteTask (task);

				if (resolveRelativeToProject) {
					// With ResolveResourceItemsRelativeToProject enabled, the
					// LogicalName is computed relative to the project directory,
					// so items are correctly included.
					Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (0), $"Warnings: {string.Join (", ", Engine.Logger.WarningsEvents.Select (e => e.Message))}");
					Assert.That (Engine.Logger.ErrorEvents.Count, Is.EqualTo (0), $"Errors: {string.Join (", ", Engine.Logger.ErrorEvents.Select (e => e.Message))}");
					Assert.That (task.BundleResourcesWithLogicalNames.Length, Is.EqualTo (3), "BundleResourcesWithLogicalNames count");

					var logicalNames = task.BundleResourcesWithLogicalNames
						.Select (i => i.GetMetadata ("LogicalName"))
						.OrderBy (n => n)
						.ToArray ();
					Assert.That (logicalNames [0], Is.EqualTo ("Component1.razor"), "LogicalName[0]");
					Assert.That (logicalNames [1], Is.EqualTo ("wwwroot/background.png"), "LogicalName[1]");
					Assert.That (logicalNames [2], Is.EqualTo ("wwwroot/script.js"), "LogicalName[2]");
				} else {
					// Without ResolveResourceItemsRelativeToProject, the LogicalName
					// is computed relative to the defining project (SDK) directory,
					// which produces paths outside the app bundle.
					Assert.That (Engine.Logger.WarningsEvents.Count, Is.EqualTo (3), $"Warnings: {string.Join (", ", Engine.Logger.WarningsEvents.Select (e => e.Message))}");
					Assert.That (task.BundleResourcesWithLogicalNames.Length, Is.EqualTo (0), "BundleResourcesWithLogicalNames count");
				}
			} finally {
				Environment.CurrentDirectory = currentDirectory;
			}
		}
	}
}
