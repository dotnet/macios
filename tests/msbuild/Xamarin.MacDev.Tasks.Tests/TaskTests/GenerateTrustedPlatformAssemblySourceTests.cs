// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;

using Microsoft.Build.Utilities;
using NUnit.Framework;

namespace Xamarin.MacDev.Tasks {
	[TestFixture]
	public class GenerateTrustedPlatformAssemblySourceTests : TestBase {
		[Test]
		public void GenerateSource ()
		{
			var outputDirectory = Cache.CreateTemporaryDirectory ("trusted-platform-assemblies");
			var task = CreateTask<GenerateTrustedPlatformAssemblySource> ();
			task.Assemblies = [
				CreatePublishItem ("source-z.exe", @"MyApp.app\Z.exe"),
				CreatePublishItem ("source-b.dll", @"MyApp.app\B.dll"),
				CreatePublishItem ("source-a.dll", @"MyApp.app\A.dll"),
				CreatePublishItem ("source-a-copy.dll", @"MyApp.app\A.dll"),
				CreatePublishItem ("source-quoted.dll", "MyApp.app/Assembly\"Name.DLL"),
				CreatePublishItem ("source-satellite.dll", @"MyApp.app\fr\Satellite.dll"),
			];
			task.AssemblyDirectory = @"MyApp.app\";
			task.IsMultiRidBuild = true;
			task.MainFiles = [
				new TaskItem ("main.arm64.mm", new System.Collections.Generic.Dictionary<string, string> { { "Arch", "arm64" } }),
				new TaskItem ("main.x86_64.mm", new System.Collections.Generic.Dictionary<string, string> { { "Arch", "x86_64" } }),
			];
			task.OutputDirectory = outputDirectory;

			ExecuteTask (task);

			Assert.That (task.NativeSourceFiles.Select (v => v.GetMetadata ("Arch")), Is.EquivalentTo (new [] { "arm64", "x86_64" }), "Architectures");
			var source = File.ReadAllText (task.NativeSourceFiles.First ().ItemSpec);
			Assert.That (source, Does.Contain ("const char *xamarin_trusted_platform_assemblies = \"A.dll:Assembly\\\"Name.DLL:B.dll:Z.exe\";"), "Assemblies");
			Assert.That (source, Does.Contain ("const size_t xamarin_trusted_platform_assembly_count = 4;"), "Assembly count");
			Assert.That (source, Does.Not.Contain ("Satellite.dll"), "Satellite assemblies");
			Assert.That (source, Does.Contain ("const bool xamarin_is_multi_rid_build = true;"), "Multi-RID");
		}

		static TaskItem CreatePublishItem (string itemSpec, string relativePath)
		{
			var item = new TaskItem (itemSpec);
			item.SetMetadata ("RelativePath", relativePath);
			return item;
		}
	}
}
