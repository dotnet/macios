// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.CompilerServices.SymbolWriter;
using Mono.Linker;
using Mono.Linker.Steps;
using Mono.Tuner;
using MonoTouch.Tuner;
using Xamarin.Bundler;
using Xamarin.Linker;
using Xamarin.Linker.Steps;
using Xamarin.Tuner;
using Xamarin.Utils;

namespace Xamarin.Build;

public class AssemblyPreparer : IDisposable {
	AggregateLog log = new AggregateLog ();

	LinkerConfiguration configuration;

	public LinkerConfiguration Configuration => configuration;

	public string MakeReproPath { get; set; } = "";

	public RegistrarMode Registrar {
		get => configuration.Application.Registrar;
		set => configuration.Application.Registrar = value;
	}

	public string IntermediateOutputPath {
		get => configuration.IntermediateOutputPath;
	}

	public Optimizations Optimizations => configuration.Application.Optimizations;

	public List<AssemblyPreparerInfo> Assemblies { get; set; } = new List<AssemblyPreparerInfo> ();

	// The list of steps that were executed, along with how long each step took and whether it modified any assemblies.
	public List<StepExecution> StepExecutions { get; } = new List<StepExecution> ();

	// Set to true (via the AppBundleRewriter.AssemblySaved callback) whenever the currently executing step modifies an assembly.
	bool currentStepModifiedAssemblies;

	public IList<(string Path, AssemblyDefinition Assembly, string? OriginatingAssembly)> AddedAssemblies => configuration.AddedAssemblies;

	LinkerConfiguration.Configurator GetConfigurator (string? reproPath = null, Func<string, string, AssemblyPreparerInfo>? assemblyPreparerInfoFactory = null)
	{
		var dict = new LinkerConfiguration.Configurator () {
			{ "AssemblyPreparer", (
				new LinkerConfiguration.LoadValue ((key, value) => {
					var split = value.Split ('|');
					var input = split[0];
					var output = split[1];
					var isTrimmableString = split[2];
					var isTrimmable = string.IsNullOrEmpty (isTrimmableString) ? (bool?) null : string.Equals (isTrimmableString, "true", StringComparison.OrdinalIgnoreCase);
					var trimMode = split[3];
					var apinfo = assemblyPreparerInfoFactory is not null ? assemblyPreparerInfoFactory (input, output) : new AssemblyPreparerInfo (input, output, isTrimmable, trimMode);
					Assemblies.Add (apinfo);
				}),
				new LinkerConfiguration.SaveValue ((key, storage) => SaveAssemblies (key, storage, reproPath, Assemblies))
			)},
		};
		return dict;
	}

	static void SaveAssemblies (string key, List<string> storage, string? reproPath, IList<AssemblyPreparerInfo> assemblies)
	{
		foreach (var assembly in assemblies) {
			var input = assembly.InputPath;
			var output = assembly.OutputPath;
			if (!string.IsNullOrEmpty (reproPath)) {
				output = Path.Combine (reproPath, Path.GetFileName (output));
				File.Copy (input, output);
			}
			storage.Add ($"{key}={input}|{output}|{(assembly.IsTrimmable.HasValue ? (assembly.IsTrimmable.Value ? "true" : "false") : "")}|{assembly.TrimMode}");
		}
	}

	public AssemblyPreparer (IToolLog log, AssemblyPreparerInfo [] assemblies, string linker_file)
	{
		var lines = File.ReadAllLines (linker_file).ToList ();
		SaveAssemblies ("AssemblyPreparer", lines, null, assemblies);
		configuration = new LinkerConfiguration (log, lines, linker_file, GetConfigurator (null, assemblies.Length == 0 ? null : (input, output) => assemblies.Single (a => a.InputPath == input && a.OutputPath == output))) {
			AssemblyInfos = Assemblies,
		};
	}

	public void AddLog (IAssemblyPreparerLog log)
	{
		if (log is null)
			throw new ArgumentNullException (nameof (log));
		this.log.Add (log);
	}

	bool SaveToReproPath (List<ProductException> exceptions)
	{
		if (File.Exists (MakeReproPath) || Directory.Exists (MakeReproPath)) {
			exceptions.Add (ErrorHelper.CreateError (99, $"Repro location already exists: {MakeReproPath}"));
			return false;
		}
		Directory.CreateDirectory (MakeReproPath);
		var lines = new List<string> ();
		configuration.Save (lines, GetConfigurator (MakeReproPath));
		File.WriteAllLines (Path.Combine (MakeReproPath, "arguments.txt"), lines);
		log.Log ($"Created repro in {MakeReproPath}");

		return true;
	}

	public static AssemblyPreparer LoadFromReproPath (string reproPath)
	{
		var file = Path.Combine (reproPath, "arguments.txt");
		if (!File.Exists (file))
			throw new FileNotFoundException ($"Repro arguments file not found: {file}");
		return new AssemblyPreparer (ConsoleLog.Instance, [], file);
	}

	public bool Prepare (out List<ProductException> exceptions)
	{
		// All the same steps as the custom trimmer steps that are run before MarkStep in Xamarin.Shared.Sdk.targets (and in the same order).
		var steps = new List<ConfigurationAwareStep> {
			// CollectAssembliesStep
			new LoadAssembliesStep (),
			new ComputeMethodOverridesStep (),
			new CoreTypeMapStep (),
			new CollectFieldsStep (), // ProcessExportedFields
			new PreserveProtocolsStep (),
			new PreserveSmartEnumConversionsStep (),
			new PreserveBlockCodeStep (),
			new OptimizeGeneratedCodeStep (),
			new ApplyPreserveAttributeStep (),
			new MarkForStaticRegistrarStep (),
			new MarkNSObjectsStep (),
			new InlineDlfcnMethodsStep (),
		};

		// Only add RegistrarRemovalTrackingStep if it's needed:
		// * If the user explicitly set $(DynamicRegistrationSupported), we don't need to compute the value (it's
		//   passed straight through to the trimmer feature switch).
		// * If nothing is being trimmed, the dynamic registrar (which lives in the platform assembly, an SDK
		//   assembly that's only trimmed when trimming is enabled) can't be removed, so there's nothing to compute.
		if (!configuration.DynamicRegistrationSupported.HasValue && configuration.Application.AreAnyAssembliesTrimmed)
			steps.Add (new RegistrarRemovalTrackingStep ());

		// PreMarkDispatcher: I don't think we need this one
		steps.Add (new ManagedRegistrarStep ());
		steps.Add (new TrimmableRegistrarStep ());
		steps.Add (new ManagedRegistrarLookupTablesStep ());
		steps.Add (new InlineClassGetHandleStep ());
		steps.Add (new SaveAssembliesStep ());

		return RunSteps (steps, out exceptions);
	}

	public bool PostProcess (out List<ProductException> exceptions)
	{
		configuration.Application.IsPostProcessingAssemblies = true;

		var steps = new ConfigurationAwareStep [] {
			// All the same steps as the custom trimmer steps that are run after sweeping in Xamarin.Shared.Sdk.targets (and in the same order).
			new LoadAssembliesStep (), // LoadNonSkippedAssembliesStep

			// post-sweep
			new CollectFieldsStep (), // Must run before ListExportedSymbols to populate ExportedFields annotation
			new ExtractBindingLibrariesStep (),
			// The ListExportedSymbols must run after ExtractBindingLibrariesStep, otherwise we won't properly list exported Objective-C classes from binding libraries
			new ListExportedSymbols (),
			new RemoveUserResourcesSubStep (), // from PreOutputDispatcher.
			// We're not doing ClassHandleRewriterStep, that's replaced by InlineClassGetHandleStep, which is run in Prepare().

			// ManagedRegistrarStep/TrimmableRegistrarStep/ManagedRegistrarLookupTablesStep
			// must run before SaveAssembliesStep because they modify assemblies (adding
			// lookup tables and ldtoken instructions). SaveAssembliesStep writes the final
			// versions to disk with correct metadata tokens.
			new ManagedRegistrarStep (),
			new TrimmableRegistrarStep (),
			new ManagedRegistrarLookupTablesStep (),

			new SaveAssembliesStep (),

			// PopulateApplicationAssembliesStep must run after SaveAssembliesStep so that
			// OutputPath is set correctly (used by ComputeAOTArguments and GatherFrameworksStep).
			new PopulateApplicationAssembliesStep (),

			// post-output

			new RegistrarStep (),

			new GenerateMainStep (),
			new GenerateReferencesStep (),
			new GatherFrameworksStep (),
			new ComputeNativeBuildFlagsStep (),
			new ComputeAOTArguments (),

			// Must be the last step.
			new DoneStep (),
		};

		return RunSteps (steps, out exceptions);
	}

	bool RunSteps (IList<ConfigurationAwareStep> steps, out List<ProductException> exceptions)
	{
		exceptions = configuration.Exceptions;

		if (Registrar == RegistrarMode.Default) {
			exceptions.Add (ErrorHelper.CreateError (99, "RegistrarMode must be explicitly set."));
			return false;
		}

		if (!string.IsNullOrEmpty (MakeReproPath) && !SaveToReproPath (exceptions))
			return false;

		var linkContext = configuration.DerivedLinkContext;

		// We detect whether a step modified any assemblies by subscribing to the AppBundleRewriter's
		// AssemblySaved callback, which is called whenever an assembly is modified. All the steps that
		// modify assemblies go through the AppBundleRewriter to do so.
		Action<AssemblyDefinition>? assemblySavedHandler = null;
		try {
			foreach (var step in steps) {
				// Subscribe once the assemblies have been loaded: accessing the AppBundleRewriter before
				// that point would create it without finding the corlib and platform assemblies.
				if (assemblySavedHandler is null && configuration.Assemblies.Count > 0) {
					assemblySavedHandler = (_) => currentStepModifiedAssemblies = true;
					configuration.AppBundleRewriter.AssemblySaved += assemblySavedHandler;
				}

				currentStepModifiedAssemblies = false;
				var watch = Stopwatch.StartNew ();
				step.Process (linkContext);
				watch.Stop ();
				StepExecutions.Add (new StepExecution (step.GetType ().Name, watch.Elapsed, currentStepModifiedAssemblies));
			}
		} finally {
			if (assemblySavedHandler is not null)
				configuration.AppBundleRewriter.AssemblySaved -= assemblySavedHandler;
		}

		// The post-processing pass flushes its MSBuild output as its last step (DoneStep). The preparation
		// pass has no DoneStep, so flush here so that its MSBuild output properties are written.
		if (!configuration.Application.IsPostProcessingAssemblies)
			configuration.FlushOutputForMSBuild ();

		return exceptions.Count == 0;
	}

	public void Dispose ()
	{
		foreach (var assembly in Assemblies)
			assembly.Assembly?.Dispose ();
		configuration.AssemblyResolver.ResolverCache.Clear ();
		configuration.DerivedLinkContext.Assemblies.Clear ();
	}
}

// The result of executing a single step: its name, how long it took, and whether it modified any assemblies.
public record struct StepExecution (string Name, TimeSpan Duration, bool ModifiedAssemblies);

public class AssemblyPreparerInfo {
	internal AssemblyDefinition? Assembly { get; set; }
	internal bool IsCILAssembly { get; set; }

	public string InputPath { get; private set; }
	public bool? IsTrimmable { get; set; }
	public string TrimMode { get; set; }
	public string OutputPath { get; set; }

	public AssemblyPreparerInfo (string inputPath, string outputPath, bool? isTrimmable, string trimMode)
	{
		InputPath = inputPath;
		OutputPath = outputPath;
		IsTrimmable = isTrimmable;
		TrimMode = trimMode;
	}
}
