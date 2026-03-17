// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;
using Mono.Cecil.Cil;
using Sharpie.Bind.Massagers;
using Xamarin.MacDev;
using Xamarin.Utils;

namespace Sharpie.Bind;

public abstract partial class ObjectiveCBinder : IDisposable {
	TranslationUnit? translationUnit;

	BindingResult bindingResult = new ();

	public abstract string Tool { get; }

	public int Verbosity { get => bindingResult.Verbosity; set => bindingResult.Verbosity = value; }
	public string Namespace { get; set; } = "";

	string? sdk;
	public string Sdk {
		get => sdk ?? "macosx";
		set => sdk = value;
	}

	public List<(string Massager, bool Enable)> Massagers { get; } = new List<(string, bool)> ();
	public string SourceFile { get; set; } = "";
	public string SourceFramework { get; set; } = string.Empty;
	public List<string> DirectoriesInScope { get; } = new List<string> ();
	public string OutputDirectory { get; set; } = "";
	public string ClangResourceDirectory { get; set; } = "";
	public List<string> ClangArguments = new List<string> ();
	public bool? EnableModules { get; set; }
	public bool SplitDocuments { get; set; } = true;

	public string ApiDefinitionName { get; set; } = "ApiDefinition.cs";
	public string StructsAndEnumsName { get; set; } = "StructsAndEnums.cs";

	public string OutputApiDefinitionPath => Path.Combine (OutputDirectory, ApiDefinitionName);
	public string OutputStructsAndEnumsPath => Path.Combine (OutputDirectory, StructsAndEnumsName);

	public BindingResult BindingResult => bindingResult;

	public ApplePlatform Platform {
		get {
			if (Sdk.StartsWith ("macosx", StringComparison.OrdinalIgnoreCase))
				return ApplePlatform.MacOSX;
			else if (Sdk.StartsWith ("iphoneos", StringComparison.OrdinalIgnoreCase) ||
					 Sdk.StartsWith ("iphonesimulator", StringComparison.OrdinalIgnoreCase))
				return ApplePlatform.iOS;
			else if (Sdk.StartsWith ("appletvos", StringComparison.OrdinalIgnoreCase) ||
					 Sdk.StartsWith ("appletvsimulator", StringComparison.OrdinalIgnoreCase))
				return ApplePlatform.TVOS;
			else if (Sdk.StartsWith ("ios", StringComparison.OrdinalIgnoreCase) && Sdk.EndsWith ("-macabi", StringComparison.OrdinalIgnoreCase))
				return ApplePlatform.MacCatalyst;
			else
				return ApplePlatform.None;
		}
	}

	string? platformAssembly;
	public string PlatformAssembly {
		get {
			if (string.IsNullOrEmpty (platformAssembly))
				platformAssembly = GuessPlatformAssemblyLocation (Platform);
			return platformAssembly;
		}
		set => platformAssembly = value;
	}

	public static string GuessPlatformAssemblyLocation (ApplePlatform platform)
	{
		var tfm = $"net$(BundledNETCoreAppTargetFrameworkVersion)-{platform.AsString ().ToLowerInvariant ()}";
		var csproj = $"""
		<Project Sdk="Microsoft.NET.Sdk">
			<PropertyGroup>
			<TargetFramework>{tfm}</TargetFramework>
			<OutputType>Library</OutputType>
			</PropertyGroup>
			<Target Name="GetPlatformAssemblyPath" DependsOnTargets="ResolveTargetingPackAssets" Returns="$(ComputedPlatformAssemblyPath)">
				<ItemGroup>
					<PlatformAssemblyReference Include="@(Reference)" Condition="'%(Filename)' == 'Microsoft.$(_PlatformName)'" />
				</ItemGroup>
				<PropertyGroup>
					<ComputedPlatformAssemblyPath>@(PlatformAssemblyReference->'%(FullPath)')</ComputedPlatformAssemblyPath>
				</PropertyGroup>
			</Target>
		</Project>
		""";
		var tmpdir = Path.Combine (Path.GetTempPath (), "sharpie", "compute-platform-assembly", platform.AsString ());
		if (Directory.Exists (tmpdir))
			Directory.Delete (tmpdir, true);
		Directory.CreateDirectory (tmpdir);
		var csprojPath = Path.Combine (tmpdir, "ComputePlatformAssembly.csproj");
		var outputPath = Path.Combine (tmpdir, "output.txt");
		var binlogPath = Path.Combine (tmpdir, "msbuild.binlog");
		File.WriteAllText (csprojPath, csproj);

		var arguments = new string [] {
			"build",
			"-getProperty:ComputedPlatformAssemblyPath",
			"-t:GetPlatformAssemblyPath",
			$"-getResultOutputFile:{outputPath}",
			$"-bl:{binlogPath}",
			"-verbosity:quiet",
			"--nologo",
		};

		var rv = Execution.RunAsync ("dotnet", arguments, workingDirectory: tmpdir).Result;
		if (rv.ExitCode != 0)
			throw new BindingException (new BindingMessage (6 /* Failed to compute platform assembly location for the platform {0}: {1} */, new object [] { platform.AsString (), rv.Output.MergedOutput }, true));
		var assemblyPath = File.ReadAllText (outputPath).Trim ();
		if (!File.Exists (assemblyPath))
			throw new BindingException (new BindingMessage (7 /* Computed platform assembly path does not exist for the platform {0}: {1} */, new object [] { platform.AsString (), assemblyPath }, true));
		return assemblyPath;
	}

	Sdk? _sdk;
	public Sdk GetSdk ()
	{
		if (_sdk is null)
			_sdk = new Sdk (bindingResult, Sdk);
		return _sdk;
	}

	public void AddMassager (string value)
	{
		if (string.IsNullOrEmpty (value))
			return;

		if (value [0] == '+') {
			Massagers.Add ((value.Substring (1), true));
		} else if (value [0] == '-') {
			Massagers.Add ((value.Substring (1), false));
		} else {
			Massagers.Add ((value, true));
		}
	}

	bool IsInScope (Decl? decl)
	{
		if (decl is null)
			return true;

		if (DirectoriesInScope.Count == 0)
			return true;

		if (!decl.TryGetPresumedLoc (out var presumedLoc))
			return true;

		var fn = presumedLoc.Value.FileName;
		if (string.IsNullOrEmpty (fn))
			return true;
		foreach (var dir in DirectoriesInScope) {
			// Ensure the scope directory ends with a directory separator so that
			// a scope of "/foo/bar" doesn't falsely match "/foo/barbaz/header.h".
			var normalizedDir = dir.EndsWith (Path.DirectorySeparatorChar) || dir.EndsWith (Path.AltDirectorySeparatorChar)
				? dir
				: dir + Path.DirectorySeparatorChar;
			if (fn.StartsWith (normalizedDir, StringComparison.Ordinal))
				return true;
		}

		return false;
	}

	public string SdkPath => GetSdk ().Location;

	// At the moment this is only used when running unit tests, so that we don't abort a test run if clang crashes for some reason
	// (in which case running out-of-process is better).
	static string? sharpieToolPath;
	static string SharpieToolPath {
		get {
			if (sharpieToolPath is null) {
				var candidatePath = Path.GetDirectoryName (typeof (ObjectiveCBinder).Assembly.Location)!;
				while (candidatePath.Length > 2 && sharpieToolPath is null) {
					var candidate = Path.Combine (candidatePath, "tools", "sharpie", "Sharpie.Bind.Tool", "bin", "Debug", "osx-arm64", "Sharpie.Bind.Tool")!;
					if (File.Exists (candidate)) {
						sharpieToolPath = candidate;
						break;
					}
					candidatePath = Path.GetDirectoryName (candidatePath)!;
				}
				if (sharpieToolPath is null)
					throw new InvalidOperationException ("Failed to find the path to the Sharpie.Bind.Tool binary.");
			}
			return sharpieToolPath;
		}
	}

	protected virtual bool AddArguments (List<string> args)
	{
		if (!string.IsNullOrEmpty (sdk)) {
			args.Add ("--sdk");
			args.Add (sdk);
		}

		if (!string.IsNullOrEmpty (OutputDirectory)) {
			args.Add ("--output");
			args.Add (OutputDirectory);
		}

		if (!string.IsNullOrEmpty (Namespace)) {
			args.Add ("--namespace");
			args.Add (Namespace);
		}

		if (Verbosity > 0) {
			for (var i = 0; i < Verbosity; i++)
				args.Add ("--verbose");
		} else if (Verbosity < 0) {
			for (var i = 0; i < -Verbosity; i++)
				args.Add ("--quiet");
		}

		if (!string.IsNullOrEmpty (SourceFile)) {
			args.Add ("--header");
			args.Add (SourceFile);
		}

		if (!string.IsNullOrEmpty (SourceFramework)) {
			args.Add ("--framework");
			args.Add (SourceFramework);
		}

		foreach (var scope in DirectoriesInScope) {
			args.Add ("--scope");
			args.Add (scope);
		}

		foreach (var massage in Massagers) {
			var prefix = massage.Enable ? "+" : "-";
			args.Add ("--massage=" + prefix + massage.Massager);
		}

		if (!string.IsNullOrEmpty (ClangResourceDirectory)) {
			args.Add ("--clang-resource-dir");
			args.Add (ClangResourceDirectory);
		}

		if (!string.IsNullOrEmpty (platformAssembly)) {
			args.Add ("--platform-assembly");
			args.Add (platformAssembly);
		}

		if (!SplitDocuments) {
			args.Add ("--nosplit");
		}

		if (EnableModules.HasValue) {
			if (EnableModules.Value)
				args.Add ("--modules=true");
			else
				args.Add ("--modules=false");
		}

		// --clang must come last
		if (ClangArguments.Any ()) {
			args.Add ("--clang");
			foreach (var cArg in ClangArguments)
				args.Add (cArg);
		}
		return true;
	}

	public BindingResult ExecuteOutOfProcess ()
	{
		var args = new List<string> ();

		if (string.IsNullOrEmpty (Tool))
			throw new InvalidOperationException ("The Tool property must be set before calling ExecuteOutOfProcess.");
		args.Add (Tool);

		if (!AddArguments (args))
			return bindingResult;

		var stdout = new List<string> ();
		var stderr = new List<string> ();
		var process = Execution.RunWithCallbacksAsync (SharpieToolPath, args, log: Console.Out, standardOutput: stdout.Add, standardError: stderr.Add).Result;

		bindingResult.ExitCode = process.ExitCode;
		if (bindingResult.ExitCode == 0) {
			if (SplitDocuments) {
				foreach (var fn in Directory.GetFiles (OutputDirectory, "*.cs")) {
					var content = File.ReadAllText (fn);
					bindingResult.AdditionalFiles [Path.GetFileName (fn)] = content;
				}
			} else {
				bindingResult.BindingCode = File.ReadAllText (OutputApiDefinitionPath);
			}
		} else if (bindingResult.ExitCode == 139) {
			stdout.ForEach (Console.WriteLine);
			stderr.ForEach (Console.Error.WriteLine);
		}
		var errors = stderr.Select (BindingMessage.Parse).Where (m => m is not null && m.IsError).Select (v => v!);
		var warnings = stdout.Select (BindingMessage.Parse).Where (m => m is not null && !m.IsError).Select (v => v!);
		bindingResult.Messages = errors.Concat (warnings).ToList ();

		return bindingResult;
	}

	public virtual BindingResult Bind ()
	{
		try {
			return BindImpl ();
		} catch (Exception e) {
			bindingResult.ReportException (e);
			return bindingResult;
		}
	}

	[DllImport ("/usr/lib/libSystem.dylib")]
	static extern int dladdr (IntPtr addr, out Dl_info info);

	struct Dl_info {
		internal IntPtr dli_fname; /* Pathname of shared object */
		internal IntPtr dli_fbase; /* Base address of shared object */
		internal IntPtr dli_sname; /* Name of nearest symbol */
		internal IntPtr dli_saddr; /* Address of nearest symbol */
	}

	[LibraryImport ("/usr/lib/libSystem.dylib")]
	private static partial IntPtr dlsym (IntPtr handle, ReadOnlySpan<byte> symbol);

	static string GetLibraryPathOfSymbol (ReadOnlySpan<byte> symbol)
	{
		var addr = dlsym (-2 /* RTLD_DEFAULT */, symbol);

		if (addr == IntPtr.Zero)
			return $"<could not find the library for the symbol {Encoding.UTF8.GetString (symbol)}>";

		if (dladdr (addr, out var info) == 0)
			return $"<failed to lookup info for symbol {Encoding.UTF8.GetString (symbol)}>";

		return Marshal.PtrToStringUTF8 (info.dli_fname) ?? "";
	}

	public TranslationUnit? CreateTranslationUnit ()
	{
		if (string.IsNullOrEmpty (ClangResourceDirectory)) {
			BindingResult.ReportError (10 /* The path to clang's resource directory is required. */);
			return null;
		} else if (!Directory.Exists (ClangResourceDirectory)) {
			BindingResult.ReportError (11 /* The specified clang resource directory does not exist: {0} */, ClangResourceDirectory);
			return null;
		}

		if (translationUnit is not null)
			throw new InvalidOperationException ("A translation unit has already been created.");

		var clangVersion = clang.getClangVersion ().ToString ();
		bindingResult.Log (1, $"Clang version: {clangVersion} - loaded from {GetLibraryPathOfSymbol ("clang_getClangVersion"u8)}");

		var clangSharpVersion = clangsharp.getVersion ().ToString ();
		bindingResult.Log (1, $"ClangSharp version: {clangSharpVersion} - loaded from {GetLibraryPathOfSymbol ("clangsharp_getVersion"u8)}");

		var index = CXIndex.Create (displayDiagnostics: Verbosity > 0);
		var translationFlags = CXTranslationUnit_Flags.CXTranslationUnit_None;
		translationFlags |= CXTranslationUnit_Flags.CXTranslationUnit_IncludeAttributedTypes; // Include attributed types in CXType
		translationFlags |= CXTranslationUnit_Flags.CXTranslationUnit_VisitImplicitAttributes; // Implicit attributes should be visited

		// Can't use detailed preprocessing records when using modules: https://github.com/llvm/llvm-project/issues/170491
		// translationFlags |= CXTranslationUnit_Flags.CXTranslationUnit_DetailedPreprocessingRecord;

		if (string.IsNullOrEmpty (OutputDirectory)) {
			bindingResult.ReportError (2 /* The output directory must be specified. */);
			return null;
		}

		if (!ResolveFramework ())
			return null;

		var clangArgs = new List<string> (ClangArguments);
		clangArgs.AddRange (GetSdk ().GetCflags ());

		if (Verbosity > 0)
			clangArgs.Add ("-v");

		FixupClangArguments (clangArgs, SourceFile);

		if (EnableModules == true && !clangArgs.Any (v => v == "-fmodules"))
			clangArgs.Add ("-fmodules");

		// We need to use the headers shipped with clang, because they need to match the clang version we're using
		// https://github.com/llvm/llvm-project/issues/158312#issuecomment-3285866899
		clangArgs.Add ("-I");
		clangArgs.Add ($"{ClangResourceDirectory}/include");
		clangArgs.Add ($"-resource-dir");
		clangArgs.Add (ClangResourceDirectory);

		bindingResult.Log (1, $"Parsing {SourceFile} with arguments:");
		foreach (var arg in clangArgs)
			bindingResult.Log (1, $"    {arg}");

		var translationUnitError = CXTranslationUnit.TryParse (index, SourceFile, clangArgs.ToArray (), [], translationFlags, out var handle);
		if (translationUnitError != CXErrorCode.CXError_Success) {
			bindingResult.ReportError (3 /* Failed to parse translation unit (error: {0}). */, translationUnitError);
			return null;
		}

		translationUnit = TranslationUnit.GetOrCreate (handle);
		if (translationUnit is null) {
			bindingResult.ReportError (4 /* Failed to create translation unit for unknown reasons. Please file an issue at https://github.com/dotnet/macios/issues. */);
			return null;
		}

		bindingResult.Log (1, $"Processing '{SourceFile}'");

		if (translationUnit.Handle.NumDiagnostics != 0) {
			for (uint i = 0; i < translationUnit.Handle.NumDiagnostics; i++) {
				using CXDiagnostic diagnostic = translationUnit.Handle.GetDiagnostic (i);
				var severity = diagnostic.Severity;
				var spelling = diagnostic.Spelling;
				var msg = spelling.ToString ();
				switch (severity) {
				case CXDiagnosticSeverity.CXDiagnostic_Error:
				case CXDiagnosticSeverity.CXDiagnostic_Fatal:
					bindingResult.ReportError (5, diagnostic.Location, /* Compilation failed with error: {0} */ msg);
					break;
				case CXDiagnosticSeverity.CXDiagnostic_Warning:
					bindingResult.ReportWarning (1000, diagnostic.Location /* A warning occurred during compilation: {0} */, msg);
					break;
				case CXDiagnosticSeverity.CXDiagnostic_Ignored:
				case CXDiagnosticSeverity.CXDiagnostic_Note:
				default:
					bindingResult.Log (0, $"{severity}: {msg}");
					break;
				}
			}
		}
		if (bindingResult.ExitCode != 0)
			return null;

		return translationUnit;
	}

	public T? Visit<T> (Func<BindingResult, T> createVisitor) where T : AstVisitor
	{
		var translationUnit = CreateTranslationUnit ();

		if (translationUnit is null)
			return null;

		var generator = createVisitor (BindingResult);
		generator.IsIncluded = IsInScope;

		generator.Visit (translationUnit.TranslationUnitDecl);
		generator.EndVisit ();

		return generator;
	}

	BindingResult BindImpl ()
	{
		var generator = Visit ((bindingResult) => {
			var generator = new BindingGenerator (bindingResult);
			generator.IsIncluded = IsInScope;
			return generator;
		});

		if (generator is null)
			return bindingResult;

		var massager = new BindingMassager (this);
		if (!string.IsNullOrEmpty (Namespace)) {
			var massagerNs = new NamespaceMassager (this, Namespace);
			massager.RegisterMassager (massagerNs);
		}
		if (SplitDocuments)
			massager.RegisterMassager (new SyntaxTreeSplitterMassager (this));
		foreach (var m in Massagers) {
			if (m.Enable) {
				massager.RegisterMassager (m.Massager);
			} else {
				massager.ExcludeMassager (m.Massager);
			}
		}
		massager.Massage (generator.SyntaxTree);

		Directory.CreateDirectory (OutputDirectory);

		var unnamedOutput = new StringBuilder ();
		foreach (var document in generator.SyntaxTree.AllDocuments) {
			var writer = new StringWriter ();
			var bindingTokenWriter = new BindingTokenWriter (this, writer);
			var formatting = FormattingOptionsFactory.CreateMono ();
			formatting.NamespaceBraceStyle = BraceStyle.EndOfLine;
			formatting.InterfaceBraceStyle = BraceStyle.EndOfLine;
			formatting.ClassBraceStyle = BraceStyle.EndOfLine;
			formatting.EnumBraceStyle = BraceStyle.EndOfLine;
			formatting.StructBraceStyle = BraceStyle.EndOfLine;
			formatting.SpaceAfterTypecast = true;
			var v = new CSharpOutputVisitor (bindingTokenWriter, formatting);
			document.AcceptVisitor (v);

			var bindingCode = writer.ToString ();
			if (document.FileName is null) {
				unnamedOutput.AppendLine (bindingCode);
			} else {
				var outputPath = Path.Combine (OutputDirectory, document.FileName);
				File.WriteAllText (outputPath, bindingCode);
				bindingResult.AdditionalFiles [document.FileName] = outputPath;
			}
		}

		bindingResult.BindingCode = unnamedOutput.ToString ();
		bindingResult.ExitCode = 0;

		if (!string.IsNullOrEmpty (bindingResult.BindingCode))
			File.WriteAllText (OutputApiDefinitionPath, bindingResult.BindingCode);

		bindingResult.Log (0, $"Bindings generated successfully.");

		return bindingResult;
	}

	bool ResolveFramework ()
	{
		if (string.IsNullOrEmpty (SourceFramework))
			return true;

		if (!string.IsNullOrEmpty (SourceFile)) {
			bindingResult.ReportError (16 /* Cannot specify both a source framework and a source file. */);
			return false;
		}

		if (!Directory.Exists (SourceFramework)) {
			bindingResult.ReportError (17 /* The framework '{0}' doesn't exist. */, SourceFramework);
			return false;
		}

		SourceFramework = Path.GetFullPath (SourceFramework);
		SourceFramework = SourceFramework.TrimEnd ('\\', '/');

		var frameworkName = Path.GetFileNameWithoutExtension (SourceFramework);
		if (string.IsNullOrEmpty (Namespace))
			Namespace = frameworkName;

		DirectoriesInScope.Add (SourceFramework);

		// Add the parent directory as a framework search path so clang can resolve framework imports.
		var parentDir = Path.GetDirectoryName (SourceFramework);
		if (!string.IsNullOrEmpty (parentDir))
			ClangArguments.AddRange (["-F", parentDir]);

		// Find the umbrella header or module map for the framework and set SourceFile.
		var headersDir = Path.Combine (SourceFramework, "Headers");
		var umbrellaHeader = Path.Combine (headersDir, frameworkName + ".h");
		var moduleMapFile = Path.Combine (SourceFramework, "Modules", "module.modulemap");

		if (File.Exists (moduleMapFile)) {
			// Generate an import file that imports the module.
			var importDir = Path.Combine (Path.GetTempPath (), "sharpie");
			Directory.CreateDirectory (importDir);
			var importFile = Path.Combine (importDir, $"framework-{frameworkName}.h");
			File.WriteAllText (importFile, $"@import {frameworkName};\n");
			SourceFile = importFile;
		} else if (File.Exists (umbrellaHeader)) {
			SourceFile = umbrellaHeader;
		} else {
			bindingResult.ReportError (18 /* The framework '{0}' does not have an umbrella header or module map. */, SourceFramework);
			return false;
		}

		if (sdk is null) {
			var infoPlistPaths = new string [] {
				Path.Combine (SourceFramework, "Info.plist"),
				Path.Combine (SourceFramework, "Versions", "A", "Resources", "Info.plist"),
			};
			foreach (var infoPlistPath in infoPlistPaths) {
				if (!File.Exists (infoPlistPath))
					continue;

				var plist = PDictionary.FromFile (infoPlistPath)!;
				if (plist is null)
					continue;

				if (!plist.TryGetValue ("DTSDKName", out PString? sdkNameValue))
					continue;

				var sdkName = sdkNameValue.Value;
				if (string.IsNullOrEmpty (sdkName))
					continue;

				// Strip the version number from the SDK name (e.g. "macosx11.0" → "macosx")
				// since the exact version from the framework's build may not be installed.
				var firstDigit = sdkName.IndexOfAny (['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']);
				if (firstDigit > 0)
					sdkName = sdkName [0..firstDigit];

				Sdk = sdkName;
				break;
			}
		}

		return true;
	}

	void FixupClangArguments (List<string> clangArguments, string sourceFile)
	{
		var languageSpecified = false;

		for (int i = 0; i < clangArguments.Count; i++) {
			var arg = clangArguments [i];
			switch (arg) {
			case "-ObjC":
				languageSpecified = true;
				break;
			case "-x":
				languageSpecified = true;
				i++;
				break;
			case string s when s.StartsWith ("-x", StringComparison.Ordinal):
				languageSpecified = true;
				break;
			}
		}

		if (!languageSpecified)
			DetectObjectiveC (clangArguments, sourceFile);
	}

	readonly static string [] objcHints = {
		"#import <",
		"#import \"",
		"@interface ",
		"@protocol ",
		"@import "
	};

	void DetectObjectiveC (List<string> clangArguments, string sourceFile)
	{
		var fmodules = false;
		var xobjectivec = false;

		if (!File.Exists (sourceFile))
			return;

		using var reader = new StreamReader (sourceFile);
		string? line;
		while ((line = reader.ReadLine ()) is not null) {
			foreach (var hint in objcHints) {
				if (line.Contains (hint)) {
					if (!xobjectivec) {
						xobjectivec = true;
						clangArguments.Add ("-xobjective-c");
					}
					if (!fmodules && hint == "@import ") {
						fmodules = true;
						clangArguments.Add ("-fmodules");
						clangArguments.Add ($"-fmodules-cache-path={ModuleCachePath}");
					}
					break;
				}
			}

			if (fmodules && xobjectivec && EnableModules != false)
				return;
		}
	}

	public void Dispose ()
	{
		translationUnit?.Dispose ();
	}

	static string ModuleCachePath => $"{Path.Combine (Path.GetTempPath (), "sharpie", "ModuleCache")}";
}
