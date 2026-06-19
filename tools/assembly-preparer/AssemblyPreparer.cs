// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

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
		var steps = new ConfigurationAwareStep [] {
			// All the same steps as the custom trimmer steps that are run before MarkStep in Xamarin.Shared.Sdk.targets (and in the same order).
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
			new RegistrarRemovalTrackingStep (),
			// PreMarkDispatcher: I don't think we need this one
			new ManagedRegistrarStep (),
			new TrimmableRegistrarStep (),
			new ManagedRegistrarLookupTablesStep (),
			new SaveAssembliesStep (),
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

		foreach (var step in steps) {
			step.Process (linkContext);
		}

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
