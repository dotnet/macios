// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Build.Framework;
using Microsoft.Build.Logging.StructuredLogger;

namespace AssemblyPreparerTests;

public class ReproTest : BaseClass {
	[TestCase (ApplePlatform.iOS, false)]
	public void RoundTrip (ApplePlatform platform, bool isCoreCLR)
	{
		Configuration.IgnoreIfIgnoredPlatform (platform);

		// build once with a repro path
		// load everything from the repro path, prepare again (with a different repro path this time),
		// and verify that the arguments.txt files from each preparation are identical
		//
		// this test can also be repurposed to run an existing repro by setting the _PrepareAssembliesMakeReproPath variable

		var reproPath = Environment.GetEnvironmentVariable ("_PrepareAssembliesMakeReproPath");
		var referenceAssemblies = Configuration.GetReferenceAssemblies (platform, false);
		if (string.IsNullOrEmpty (reproPath)) {
			var code = @"public class SomeLibrary {}";

			reproPath = Xamarin.Cache.CreateTemporaryDirectory ();
			Directory.Delete (reproPath); // the repro path can't exist prior to Prepare
			AssertPrepareCode (platform, isCoreCLR, (preparer) => {
				preparer.MakeReproPath = reproPath;
				preparer.Registrar = RegistrarMode.Dynamic;
			}, code, out string _);
		}

		var lines = File.ReadAllLines (Path.Combine (reproPath, "arguments.txt"));

		var ap = AssemblyPreparer.LoadFromReproPath (reproPath);
		ap.MakeReproPath = Xamarin.Cache.CreateTemporaryDirectory ();
		Directory.Delete (ap.MakeReproPath); // the repro path can't exist prior to Prepare
		AssertPrepare (ap);

		var lines2 = File.ReadAllLines (Path.Combine (ap.MakeReproPath, "arguments.txt"));

		// Normalize repro paths before comparison, since they will always differ
		var normalizedLines = lines.Select (l => l.Replace (reproPath, "<REPRO>")).ToArray ();
		var normalizedLines2 = lines2.Select (l => l.Replace (ap.MakeReproPath, "<REPRO>")).ToArray ();
		Assert.That (normalizedLines, Is.EqualTo (normalizedLines2), "Repro arguments match");
	}

	[TestCase (ApplePlatform.iOS, false, false)]
	[TestCase (ApplePlatform.iOS, false, true)]
	public void BindingResourcePackageRoundTrip (ApplePlatform platform, bool isCoreCLR, bool zipped)
	{
		var code = @"public class SomeLibrary {}";
		using var preparer = CreatePreparer (platform, isCoreCLR, p => p.Registrar = RegistrarMode.Dynamic, code, out var testInfo, testAssemblyHasOriginalInputPath: true);
		var originalInputPath = testInfo.OriginalInputPath;
		if (originalInputPath is null) {
			Assert.Fail ("Original input path is null.");
			return;
		}
		Assert.That (originalInputPath, Is.Not.EqualTo (testInfo.InputPath), "The original input path must differ from the assembly-preparer input");
		var resourceDirectory = Path.ChangeExtension (originalInputPath, ".resources");
		if (zipped) {
			File.WriteAllText (resourceDirectory + ".zip", "");
		} else {
			Directory.CreateDirectory (resourceDirectory);
			File.WriteAllText (Path.Combine (resourceDirectory, "resource"), "");
		}

		var reproPath = Xamarin.Cache.CreateTemporaryDirectory ();
		Directory.Delete (reproPath);
		preparer.MakeReproPath = reproPath;
		AssertPrepare (preparer);

		var copiedResourceDirectory = Path.ChangeExtension (Path.Combine (reproPath, Path.GetFileName (testInfo.OutputPath)), ".resources");
		if (zipped) {
			Assert.That (File.Exists (copiedResourceDirectory + ".zip"), Is.True, "The zipped binding resource package was copied");
		} else {
			Assert.That (File.Exists (Path.Combine (copiedResourceDirectory, "resource")), Is.True, "The binding resource package directory was copied");
		}

		using var replay = AssemblyPreparer.LoadFromReproPath (reproPath);
		replay.Registrar = RegistrarMode.Dynamic;
		var replayTestInfo = replay.Assemblies.Single (v => Path.GetFileName (v.InputPath) == "Test.dll");
		Assert.That (replayTestInfo.OriginalInputPath, Is.EqualTo (Path.Combine (reproPath, "Test.dll")), "Original input path");
		AssertPrepare (replay);
	}

	// This test is here only to easily debug the PrepareAssemblies task from a build that produced a binlog.
	// Just copy the binlog to /tmp/assembly-preparer.binlog and run this test. It will find the PrepareAssemblies
	// task in the binlog, extract the relevant parameters, and run the preparation logic with those parameters.
	[Test]
	public void FromBinlog ()
	{
		var binlogPath = "/tmp/assembly-preparer.binlog";
		if (!File.Exists (binlogPath))
			Assert.Ignore (); // The binlog doesn't exist, so nothing to do

		var reader = new BinLogReader ();
		var records = reader.ReadRecords (binlogPath).ToArray ();
		var originalBinlogPath = records
			.Select (r => r.Args)
			.OfType<BuildMessageEventArgs> ()
			.Where (r => r.SenderName == "BinaryLogger" && r.Message?.StartsWith ("BinLogFilePath=") == true)
			.Select (v => v.Message?.Substring ("BinLogFilePath=".Length) ?? "")
			.SingleOrDefault ();
		var originalBinlogDirectory = Path.GetDirectoryName (originalBinlogPath)!;
		foreach (var record in records) {
			if (record is null)
				continue;

			if (record.Args is null)
				continue;

			if (record.Args is TaskStartedEventArgs tsea && tsea.TaskName == "PrepareAssemblies") {
				var taskId = tsea.BuildEventContext?.TaskId;
				if (taskId is null)
					continue;
				var relevantRecords = records.
					Select (v => v.Args).
					Where (v => v is not null).
					Where (v => v.BuildEventContext?.TaskId == taskId).
					ToArray ();

				var taskParameters = relevantRecords.Where (v => v is TaskParameterEventArgs).Cast<TaskParameterEventArgs> ().ToArray ();

				string? getProperty (string name)
				{
					var param = taskParameters.SingleOrDefault (v => v.ItemType == name);
					if (param is null)
						return null;
					if (param.Items is null)
						return null;
					if (param.Items.Count != 1)
						return null;
					var item = param.Items [0];
					if (item is null)
						return null;
					return ((ITaskItem) item).ItemSpec;
				}
				ITaskItem []? getItems (string name)
				{
					var param = taskParameters.SingleOrDefault (v => v.ItemType == name);
					if (param is null)
						return null;
					if (param.Items is null)
						return null;
					return param.Items.Cast<ITaskItem> ().ToArray ();
				}
				var outputDirectory = getProperty ("OutputDirectory");
				var optionsFile = getProperty ("OptionsFile");
				var targetFrameworkMoniker = getProperty ("TargetFrameworkMoniker");
				var makeReproPath = getProperty ("MakeReproPath");
				var inputAssemblies = getItems ("InputAssemblies");

				if (string.IsNullOrEmpty (outputDirectory))
					throw new InvalidOperationException ("OutputDirectory is required");
				outputDirectory = Path.GetFullPath (outputDirectory, originalBinlogDirectory);

				if (string.IsNullOrEmpty (optionsFile))
					throw new InvalidOperationException ("OptionsFile is required");
				optionsFile = Path.GetFullPath (optionsFile, originalBinlogDirectory);

				if (string.IsNullOrEmpty (targetFrameworkMoniker))
					throw new InvalidOperationException ("TargetFrameworkMoniker is required");

				if (inputAssemblies is null || inputAssemblies.Length == 0)
					throw new InvalidOperationException ("InputAssemblies is required");

				AssemblyPreparerInfo GetAssemblyInfo (ITaskItem item)
				{
					var inputPath = Path.GetFullPath (item.ItemSpec, originalBinlogDirectory);
					var outputPath = Path.Combine (outputDirectory, Path.GetFileName (inputPath));
					var metadataNames = item.MetadataNames.Cast<string> ().Select (v => v.ToLowerInvariant ()).ToHashSet ();
					var isTrimmableString = item.GetMetadata ("IsTrimmable");
					var isTrimmable = string.IsNullOrEmpty (isTrimmableString) ? (bool?) null : string.Equals (isTrimmableString, "true", StringComparison.OrdinalIgnoreCase);
					var trimMode = item.GetMetadata ("TrimMode");
					var originalInputPath = item.GetMetadata ("OriginalItemSpec");
					if (!string.IsNullOrEmpty (originalInputPath))
						originalInputPath = Path.GetFullPath (originalInputPath, originalBinlogDirectory);

					var rv = new AssemblyPreparerInfo (inputPath, outputPath, originalInputPath, isTrimmable, trimMode);
					return rv;
				}

				var platformString = File.ReadAllLines (optionsFile).Single (v => v.StartsWith ("Platform=")).Substring ("Platform=".Length);
				var platform = ApplePlatformExtensions.Parse (platformString);

				var infos = inputAssemblies.Select (GetAssemblyInfo).ToArray ();
				var logger = new TestLogger () { Platform = platform };
				using var preparer = new AssemblyPreparer (logger, infos, optionsFile);
				preparer.MakeReproPath = makeReproPath ?? "";
				var rv = preparer.Prepare (out var exceptions);
				return;
			}
		}

		Assert.Fail ("The task 'PrepareAssemblies' was not found in the provided binlog.");
	}
}


class TestLogger : IToolLog {
	public int Verbosity => 0;
	public required ApplePlatform Platform { get; set; }
	public List<ProductException> Errors { get; } = [];

	public void Log (string value)
	{
		Console.WriteLine (value);
	}

	public void Log (string format, params object? [] args)
	{
		Console.WriteLine (format, args);
	}

	public void LogException (Exception ex)
	{
		Console.WriteLine (ex.ToString ());
	}

	public void LogError (ProductException ex)
	{
		Errors.Add (ex);
		Console.WriteLine (ex.ToString ());
	}

	public void LogError (Exception ex)
	{
		Console.WriteLine (ex.ToString ());
	}

	public void LogError (string value)
	{
		Console.WriteLine (value);
	}

	public void LogWarning (ProductException ex)
	{
		Console.WriteLine (ex.ToString ());
	}

	public void LogWarning (Exception ex)
	{
		Console.WriteLine (ex.ToString ());
	}
}
