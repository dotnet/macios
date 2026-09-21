// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

using NUnit.Framework;

#nullable enable

namespace Xamarin.MacDev.Tasks {

	// Note: we can't use System.Text.Json types here to inspect the output, because the
	// Xamarin.MacDev.Tasks assembly under test is ILMerged and also exposes System.Text.Json,
	// which causes ambiguous type references. We assert on the raw JSON text instead.
	[TestFixture]
	public class MergeRuntimeConfigFilesTaskTest : TestBase {

		string RunMerge (string mainJson, string? devJson)
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var mainFile = Path.Combine (tmp, "app.runtimeconfig.json");
			File.WriteAllText (mainFile, mainJson);

			string? devFile = null;
			if (devJson is not null) {
				devFile = Path.Combine (tmp, "app.runtimeconfig.dev.json");
				File.WriteAllText (devFile, devJson);
			}

			var outputFile = Path.Combine (tmp, "obj", "runtimeconfig.merged.json");

			var task = CreateTask<MergeRuntimeConfigFiles> ();
			task.RuntimeConfigFile = mainFile;
			task.RuntimeConfigDevFile = devFile;
			task.OutputFile = outputFile;

			ExecuteTask (task);

			Assert.That (outputFile, Does.Exist, "output file created");
			return File.ReadAllText (outputFile);
		}

		[Test]
		public void MergesDevPropertiesOntoMain ()
		{
			var mainJson = @"{
	""runtimeOptions"": {
		""tfm"": ""net10.0"",
		""configProperties"": {
			""OnlyInMain"": true,
			""InBoth"": ""main-value""
		}
	}
}";
			var devJson = @"{
	""runtimeOptions"": {
		""configProperties"": {
			""OnlyInDev"": ""dev-only"",
			""InBoth"": ""dev-value""
		}
	}
}";

			var merged = RunMerge (mainJson, devJson);

			// (a) properties only in main are preserved
			Assert.That (merged, Does.Contain ("\"OnlyInMain\": true"), "OnlyInMain preserved");
			// (b) properties only in dev are added
			Assert.That (merged, Does.Contain ("\"OnlyInDev\": \"dev-only\""), "OnlyInDev added");
			// (c) properties in both are taken from dev (dev wins)
			Assert.That (merged, Does.Contain ("\"InBoth\": \"dev-value\""), "dev wins for InBoth");
			Assert.That (merged, Does.Not.Contain ("main-value"), "main value for InBoth is gone");
		}

		[Test]
		public void NoDevFile ()
		{
			var mainJson = @"{
	""runtimeOptions"": {
		""configProperties"": {
			""OnlyInMain"": true
		}
	}
}";

			var merged = RunMerge (mainJson, null);

			Assert.That (merged, Does.Contain ("\"OnlyInMain\": true"), "OnlyInMain preserved");
		}

		[Test]
		public void DevFileWithoutConfigProperties ()
		{
			var mainJson = @"{
	""runtimeOptions"": {
		""configProperties"": {
			""OnlyInMain"": true
		}
	}
}";
			var devJson = @"{
	""runtimeOptions"": {
		""additionalProbingPaths"": [ ""/some/path"" ]
	}
}";

			var merged = RunMerge (mainJson, devJson);

			Assert.That (merged, Does.Contain ("\"OnlyInMain\": true"), "OnlyInMain preserved");
			// The dev file had no configProperties, so nothing from it should have leaked in.
			Assert.That (merged, Does.Not.Contain ("additionalProbingPaths"), "dev-only non-configProperties content is not merged");
		}

		[Test]
		public void InvalidMainJsonLogsError ()
		{
			var tmp = Cache.CreateTemporaryDirectory ();
			var mainFile = Path.Combine (tmp, "app.runtimeconfig.json");
			File.WriteAllText (mainFile, "this is not json {");

			var task = CreateTask<MergeRuntimeConfigFiles> ();
			task.RuntimeConfigFile = mainFile;
			task.OutputFile = Path.Combine (tmp, "obj", "runtimeconfig.merged.json");

			// The task must report an MSBuild error rather than throwing.
			ExecuteTask (task, 1);
		}
	}
}
