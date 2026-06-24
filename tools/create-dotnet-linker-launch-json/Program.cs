using System.IO;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Build.Logging.StructuredLogger;

using Mono.Options;

class Program {
	static int Main (string [] args)
	{
		var printHelp = false;
		var binlog = string.Empty;
		var rootDirectory = string.Empty;
		var skippedLinkerCommands = 0;
		var options = new OptionSet {
			{ "h|?|help", "Print this help message", (v) => printHelp = true },
			{ "r|root=", "The root directory", (v) => rootDirectory = v },
			{ "bl|binlog=", "The binlog", (v) => binlog = v },
			{ "s|skip", "Task invocations to skip", (v) => skippedLinkerCommands++ },
		};

		if (printHelp) {
			options.WriteOptionDescriptions (Console.Out);
			return 0;
		}

		var others = options.Parse (args);
		if (others.Any ()) {
			Console.WriteLine ("Unexpected arguments:");
			foreach (var arg in others)
				Console.WriteLine ("\t{0}", arg);
			Console.WriteLine ("Expected arguments are:");
			options.WriteOptionDescriptions (Console.Out);
			return 1;
		}

		if (string.IsNullOrEmpty (binlog)) {
			Console.Error.WriteLine ("A binlog is required");
			Console.WriteLine ("Expected arguments are:");
			options.WriteOptionDescriptions (Console.Out);
			return 1;
		}

		var path = Path.GetFullPath (binlog);

		if (string.IsNullOrEmpty (rootDirectory))
			rootDirectory = Path.GetDirectoryName (path)!;

		Console.WriteLine ($"Processing {path} with root directory {rootDirectory}...");

		// Copy the binlog to "/tmp/assembly-preparer.binlog", to easily debug the PrepareAssemblies task.
		var unitTestLocation = "/tmp/assembly-preparer.binlog";
		try {
			File.Copy (path, unitTestLocation, true);
			Console.WriteLine ($"Copied {path} to {unitTestLocation}");
		} catch (Exception ex) {
			Console.WriteLine ($"Failed to copy {path} to {unitTestLocation}: {ex.Message}");
		}

		var reader = new BinLogReader ();
		var records = reader.ReadRecords (path).ToArray ();

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

			if (record.Args is not TaskStartedEventArgs tsea)
				continue;

			switch (tsea.TaskName) {
			case "PrepareAssemblies": {
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

				string GetAssemblyInfo (ITaskItem item)
				{
					var inputPath = Path.GetFullPath (item.ItemSpec, originalBinlogDirectory);
					var outputPath = Path.Combine (outputDirectory, Path.GetFileName (inputPath));
					var metadataNames = item.MetadataNames.Cast<string> ().Select (v => v.ToLowerInvariant ()).ToHashSet ();
					var isTrimmableString = item.GetMetadata ("IsTrimmable");
					var isTrimmable = string.IsNullOrEmpty (isTrimmableString) ? (bool?) null : string.Equals (isTrimmableString, "true", StringComparison.OrdinalIgnoreCase);
					var trimMode = item.GetMetadata ("TrimMode");

					return $"InputPath={inputPath}|OutputPath={outputPath}|IsTrimmable={isTrimmable}|TrimMode={trimMode}";
				}

				var launcherArgs = new List<string> ();
				launcherArgs.Add ("${workspaceFolder}/bin/Debug/ap-launcher.dll");
				if (!string.IsNullOrEmpty (makeReproPath))
					launcherArgs.Add ("--make-repro=" + makeReproPath);
				if (!string.IsNullOrEmpty (optionsFile))
					launcherArgs.Add ("--options-file=" + optionsFile);

				foreach (var ia in inputAssemblies) {
					launcherArgs.Add ("--input-assembly=" + GetAssemblyInfo (ia));
				}

				WriteApLauncherLaunchJson (CreateLaunchJson (rootDirectory, launcherArgs.ToArray ()));

				break;
			}
			case "ILLink": {
				if (skippedLinkerCommands > 0) {
					Console.WriteLine ($"Skipped an ILLink task invocation, {skippedLinkerCommands} left to skip...");
					skippedLinkerCommands--;
					continue;
				}

				var relevantRecords = records.Where (v => v?.Args?.BuildEventContext?.TaskId == tsea.BuildEventContext?.TaskId).Select (v => v.Args).ToArray ();
				var cla = relevantRecords.Where (v => v is BuildMessageEventArgs).Cast<BuildMessageEventArgs> ().Where (v => v?.ToString ()?.Contains ("CommandLineArguments") == true).ToArray ();
				foreach (var rr in relevantRecords) {
					if (rr is TaskCommandLineEventArgs tclea) {
						if (!Xamarin.Utils.StringUtils.TryParseArguments (tclea.CommandLine.Replace ('\n', ' '), out var arguments, out var ex)) {
							Console.WriteLine ($"Failed to parse command line arguments: {ex.Message}");
							return 1;
						}

						WriteDotNetLinkerLaunchJson (CreateLaunchJson (rootDirectory, arguments));
						return 0;
					}
				}
				break;
			}
			}
		}

		Console.Error.WriteLine ($"Unable to find command line arguments for ILLink in {path}");
		return 1;
	}

	static void WriteApLauncherLaunchJson (string contents)
	{
		WriteLaunchJson ("ap-launcher", contents);
	}

	static void WriteDotNetLinkerLaunchJson (string contents)
	{
		WriteLaunchJson ("dotnet-linker", contents);
	}

	static void WriteLaunchJson (string toolName, string contents)
	{
		var dir = Environment.CurrentDirectory!;
		while (!Directory.Exists (Path.Combine (dir, "tools", toolName)))
			dir = Path.GetDirectoryName (dir)!;
		var path = Path.Combine (dir, "tools", toolName, ".vscode", "launch.json");
		File.WriteAllText (path, contents);
		Console.WriteLine ($"Created {path}");
	}

	static string CreateLaunchJson (string workingDirectory, string [] arguments)
	{
		var dotnet = arguments [0];
		var sb = new StringBuilder ();
		sb.AppendLine ("{");
		sb.AppendLine ("    // Use IntelliSense to learn about possible attributes.");
		sb.AppendLine ("    // Hover to view descriptions of existing attributes.");
		sb.AppendLine ("    // For more information, visit: https://go.microsoft.com/fwlink/?linkid=830387");
		sb.AppendLine ("    \"version\": \"0.2.0\",");
		sb.AppendLine ("    \"configurations\": [");
		sb.AppendLine ("        {");
		sb.AppendLine ("            \"justMyCode\": false,");
		sb.AppendLine ("            \"preLaunchTask\": \"make\",");
		sb.AppendLine ("            \"name\": \".NET Core Launch (console)\",");
		sb.AppendLine ("            \"type\": \"coreclr\",");
		sb.AppendLine ("            \"request\": \"launch\",");
		sb.AppendLine ($"            \"program\": \"{dotnet}\",");
		sb.AppendLine ("            \"args\": [");
		for (var i = 1; i < arguments.Length; i++) {
			sb.AppendLine ($"                \"{arguments [i]}\"{(i < arguments.Length - 1 ? "," : "")}");
		}
		sb.AppendLine ("            ],");
		sb.AppendLine ($"            \"cwd\": \"{Path.GetFullPath (workingDirectory)}\",");
		sb.AppendLine ("            \"console\": \"internalConsole\",");
		sb.AppendLine ("            \"stopAtEntry\": false");
		sb.AppendLine ("        },");
		sb.AppendLine ("        {");
		sb.AppendLine ("            \"name\": \".NET Core Attach\",");
		sb.AppendLine ("            \"type\": \"coreclr\",");
		sb.AppendLine ("            \"request\": \"attach\"");
		sb.AppendLine ("        }");
		sb.AppendLine ("    ]");
		sb.AppendLine ("}");
		return sb.ToString ();
	}
}
