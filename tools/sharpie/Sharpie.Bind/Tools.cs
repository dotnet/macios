// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Options;

namespace Sharpie.Bind;

public static class Tools {
	public static int Bind (string [] arguments)
	{
		using var binder = CreateBindTool (arguments);
		if (binder is null)
			return 0;
		var rv = binder.Bind ();
		return rv.PrintMessages ();
	}

	static BindTool? CreateBindTool (string [] args)
	{
		var showHelp = false;

		var binder = new BindTool ();

		var os = new OptionSet {
			{ "Options:" },
			{ "h|help", "Show detailed help information.", v => showHelp = true },
			{ "v|verbose", "Be verbose with output.", v => binder.Verbosity++ },
			{ "q|quiet", "Suppress fluffy status messages.", v => binder.Verbosity-- },
			new ResponseFileSource (),

			{ "" },
			{ "Parse options:" },
			{ "s|sdk=", "Target SDK.", v => binder.Sdk = v },
			{ "f|framework=", "The input framework to bind. Implies setting the scope (--scope) to the framework, setting the namespace (--namespace) to the name of the framework, and no other sources/headers can be specified. If the framework provides an 'Info.plist' with SDK information (DTSDKName), the '-sdk' option will be implied as well (if not manually specified).", v => binder.SourceFramework = v },
			{ "header=", "The input header file to bind. This can also be a .framework directory.", v => binder.SourceFile = v },
			{   "scope=", "Restrict following #include and #import directives declared in header files to within the specified DIR directory.", v => binder.DirectoriesInScope.Add (Path.GetFullPath (v)) },
			{ "c|clang", "All arguments after this argument are not processed by Objective Sharpie and are proxied directly to Clang.", v => { } },
			{ "clang-resource-dir=", "Specify the Clang resource directory.", v => binder.ClangResourceDirectory = v },
			{ "platform-assembly=", "Specify the platform assembly to use for binding.", v => binder.PlatformAssembly = v },

			{ "" },
			{ "Bind options:" },
			{ "o|output=", "Output directory for generated binding files.", v => binder.OutputDirectory = v },
			{ "n|namespace=", "Namespace under which to generate the binding. The default is to use the framework's name as the namespace.", v => binder.Namespace = v },
			{ "m|massage=", "Register (+ prefix) or exclude (- prefix) a massager by name.", v => binder.AddMassager (v) },
			{ "nosplit", "Do not split the generated binding into multiple files.", v => binder.SplitDocuments = false },
			{ "deepsplit", "Split the generated binding into one file per source header.", v => binder.DeepSplit = true },
			{ "custom-delegates", "Generate custom named delegates instead of Func<>/Action<>.", v => binder.UseCustomDelegates = true },
		};

		os.EndOfParsingArguments.Clear ();
		os.EndOfParsingArguments.AddRange (["-c", "--clang"]);

		var clangArgs = os.Parse (args);

		if (showHelp) {
			os.WriteOptionDescriptions (Console.Out);
			return null;
		}

		if (string.IsNullOrEmpty (binder.ClangResourceDirectory))
			binder.ClangResourceDirectory = GetClangResourceDirectory ();

		binder.ClangArguments.AddRange (clangArgs);
		return binder;
	}

	public static int SdkDb (string [] args)
	{
		using var binder = CreateSdkDbTool (args);
		if (binder is null)
			return 0;
		var rv2 = binder.Bind ();
		return rv2.PrintMessages ();
	}

	public static bool Visit<T> (string [] arguments, Func<BindingResult, T> createVisitor, out BindingResult? bindingResult) where T : AstVisitor
	{
		bindingResult = null;

		using var binder = CreateSdkDbTool (arguments);
		if (binder is null)
			return false;

		bindingResult = binder.BindingResult;
		if (bindingResult.ExitCode != 0)
			return false;

		binder.Visit (createVisitor);
		return true;
	}

	public static SdkDbTool? CreateSdkDbTool (string [] args)
	{
		var binder = new SdkDbTool ();
		var showHelp = false;
		var arch = "";

		var os = new OptionSet {
			{ "Options:" },
			{ "h|help", "Show detailed help information.", v => showHelp = true },
			{ "v|verbose", "Be verbose with output.", v => binder.Verbosity++ },
			{ "q|quiet", "Suppress fluffy status messages.", v => binder.Verbosity-- },
			new ResponseFileSource (),

			{ "" },
			{ "Options:" },
			{ "a|arch=", "Specify which architecture(s) to build for.", v => arch = v },
			{ "o|output=", "Output directory for generated binding files.", v => binder.OutputDirectory = v },
			{ "s|sdk=", "Target SDK.", v => binder.Sdk = v },
			{ "x|exclude=", "Exclude a framework by name from the SDK.", v => binder.ExcludedFrameworks.Add (v) },
			{ "i|extra-hash-import=", " Framework-relative header for which to generate an #import; use this option whena framework's module map or umbrella header is known to be broken, such aswith AVFoundation in Xcode 9 Beta 1: '-i AVFoundation/AVAssetDownloadStorageManager.h'", v => binder.ExtraHashImports.Add (v) },
			{ "modules=", "Enable/use modules (-fmodules). Defaults to 'true'.", v => binder.EnableModules = bool.Parse (v) },
			{ "c|clang", "All arguments after this argument are not processed by Objective Sharpie and are proxied directly to Clang.", v => { } },
			{ "clang-resource-dir=", "Specify the Clang resource directory.", v => binder.ClangResourceDirectory = v },
			{ "platform-assembly=", "Specify the platform assembly to use for binding.", v => binder.PlatformAssembly = v },
			{ "nosplit", "Do not split the generated binding into multiple files.", v => binder.SplitDocuments = false },
			{ "custom-delegates", "Generate custom named delegates instead of Func<>/Action<>.", v => binder.UseCustomDelegates = true },
		};

		os.EndOfParsingArguments.Clear ();
		os.EndOfParsingArguments.AddRange (["-c", "--clang"]);

		var clangArgs = os.Parse (args);

		if (showHelp) {
			os.WriteOptionDescriptions (Console.Out);
			return null;
		}

		if (string.IsNullOrEmpty (binder.ClangResourceDirectory))
			binder.ClangResourceDirectory = GetClangResourceDirectory ();

		binder.ClangArguments.AddRange (clangArgs);
		binder.PrepareArguments ();
		return binder;
	}

	static string GetClangResourceDirectory ()
	{
		var executableLocation = AppContext.BaseDirectory;
		var rootNuGetDirectory = Path.GetFullPath (Path.Combine (executableLocation, "..", "..", ".."));
		if (!File.Exists (Path.Combine (rootNuGetDirectory, "Sharpie.Bind.Tool.nuspec"))) {
			var msg = new BindingMessage (12 /* Could not find the clang resource directory, because it doesn't look like we're executing from inside a NuGet. */, Array.Empty<object> (), false);
			msg.PrintMessage ();
			return "";
		}
		var clangResourceDirectory = Path.GetFullPath (Path.Combine (rootNuGetDirectory, "content", "clang"));
		if (!Directory.Exists (clangResourceDirectory)) {
			var msg = new BindingMessage (13 /* Could not find the clang resource directory, because the directory {0} doesn't exist. */, [clangResourceDirectory], false);
			msg.PrintMessage ();
			return "";
		}
		return clangResourceDirectory;
	}
}
