// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Xamarin.Build;
using Xamarin.Bundler;
using Xamarin.Utils;

class Program {
	public static int Main (string [] args)
	{
		var optionsFile = args.Single (v => v.StartsWith ("--options-file=")).Substring ("--options-file=".Length);
		var makeReproPath = args.SingleOrDefault (v => v.StartsWith ("--make-repro-path="))?.Substring ("--make-repro-path=".Length) ?? "";

		var api = new List<AssemblyPreparerInfo> ();
		foreach (var inputArgs in args.Where (v => v.StartsWith ("--input-assembly=")).Select (v => v.Substring ("--input-assembly=".Length))) {
			var ia = inputArgs.Split ('|');
			var inputPath = ia.Single (v => v.StartsWith ("InputPath="))?.Substring ("InputPath=".Length) ?? throw new InvalidOperationException ("InputPath is required");
			var outputPath = ia.Single (v => v.StartsWith ("OutputPath="))?.Substring ("OutputPath=".Length) ?? throw new InvalidOperationException ("OutputPath is required");
			var isTrimmableString = ia.Single (v => v.StartsWith ("IsTrimmable="))?.Substring ("IsTrimmable=".Length);
			var isTrimmable = string.IsNullOrEmpty (isTrimmableString) ? (bool?) null : string.Equals (isTrimmableString, "true", StringComparison.OrdinalIgnoreCase);
			var trimMode = ia.Single (v => v.StartsWith ("TrimMode="))?.Substring ("TrimMode=".Length) ?? "";

			api.Add (new AssemblyPreparerInfo (inputPath, outputPath, isTrimmable, trimMode));
		}

		var platformString = File.ReadAllLines (optionsFile).Single (v => v.StartsWith ("Platform=")).Substring ("Platform=".Length);
		var platform = ApplePlatformExtensions.Parse (platformString);

		var infos = api.ToArray ();
		var logger = new TestLogger () {
			Platform = platform,
		};
		using var preparer = new AssemblyPreparer (logger, infos, optionsFile);
		preparer.MakeReproPath = makeReproPath ?? "";
		var rv = preparer.Prepare (out var exceptions);

		return rv && exceptions.Count == 0 ? 0 : 1;
	}
}

class TestLogger : IToolLog {
	public int Verbosity => 0;
	public required ApplePlatform Platform { get; set; }

	public void Log (string value)
	{
		Console.WriteLine (value);
	}

	public void LogError (string value)
	{
		Console.WriteLine (value);
	}

	public void Log (string format, params object? [] args)
	{
		Console.WriteLine (format, args);
	}

	public void LogException (Exception ex)
	{
		Console.Error.WriteLine (ex.ToString ());
	}

	public void LogError (ProductException ex)
	{
		Console.Error.WriteLine (ex.ToString ());
	}

	public void LogWarning (ProductException ex)
	{
		Console.WriteLine (ex.ToString ());
	}
}
