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
		configuration = new LinkerConfiguration (log, lines, linker_file, GetConfigurator (null, assemblies.Length == 0 ? null : (input, output) => assemblies.Single (a => a.InputPath == input && a.OutputPath == output)));
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
		exceptions = configuration.Exceptions;

		if (Registrar == RegistrarMode.Default) {
			exceptions.Add (ErrorHelper.CreateError (99, "RegistrarMode must be explicitly set."));
			return false;
		}

		if (!string.IsNullOrEmpty (MakeReproPath) && !SaveToReproPath (exceptions))
			return false;

		var steps = new ConfigurationAwareStep [] {
			// All the same steps as the custom trimmer steps that are run before MarkStep in Xamarin.Shared.Sdk.targets (and in the same order).
			// CollectAssembliesStep
			new CoreTypeMapStep (),
			// ProcessExportedFields
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
		};

		var linkContext = configuration.DerivedLinkContext;

		var parameters = new ReaderParameters {
			AssemblyResolver = configuration.AssemblyResolver,
			MetadataResolver = configuration.MetadataResolver,
			ReadSymbols = true,
			SymbolReaderProvider = new DefaultSymbolReaderProvider (throwIfNoSymbol: false),
		};

		var skippedAssemblies = new List<AssemblyPreparerInfo> ();
		foreach (var assembly in Assemblies) {
			AssemblyDefinition assemblyDefinition;
			try {
				assemblyDefinition = AssemblyDefinition.ReadAssembly (assembly.InputPath, parameters);
			} catch (BadImageFormatException) {
				// Not a managed assembly, skip it (pass it through unchanged).
				log.Log ($"Skipping non-managed assembly: {assembly.InputPath}");
				assembly.OutputPath = assembly.InputPath;
				skippedAssemblies.Add (assembly);
				continue;
			}
			linkContext.Assemblies.Add (assemblyDefinition);
			assembly.Assembly = assemblyDefinition;
			configuration.Context.Annotations.SetAction (assemblyDefinition, ComputeAssemblyAction (assemblyDefinition, assembly));
			var assemblyName = assemblyDefinition.Name.Name;
			if (configuration.AssemblyResolver.ResolverCache.ContainsKey (assemblyName))
				exceptions.Add (ErrorHelper.CreateWarning (99, $"Duplicate assembly name '{assemblyName}' in the list of assemblies to prepare (new: {assembly.InputPath})."));
			configuration.AssemblyResolver.ResolverCache [assemblyName] = assemblyDefinition;
		}

		configuration.Context.Annotations.CollectOverrides (linkContext.Assemblies, linkContext);

		// Populate FieldSymbols for InlineDlfcnMethodsStep's compatibility mode.
		// This is equivalent to what ProcessExportedFields does in the ILLink pipeline.
		if (configuration.InlineDlfcnMethodsEnabled) {
			foreach (var assembly in linkContext.Assemblies) {
				if (!assembly.MainModule.HasTypeReference (Namespaces.Foundation + ".FieldAttribute"))
					continue;
				foreach (var type in assembly.MainModule.Types)
					CollectFieldSymbols (configuration, type);
			}
		}

		foreach (var step in steps) {
			step.Process (linkContext);
		}

		// save assemblies

		foreach (var assembly in Assemblies) {
			if (skippedAssemblies.Contains (assembly))
				continue;

			var assemblyDefinition = assembly.Assembly;
			if (assemblyDefinition is null) {
				exceptions.Add (ErrorHelper.CreateError (99, $"Assembly definition is null for {assembly.InputPath}"));
				return false;
			}

			var action = configuration.Context.Annotations.GetAction (assemblyDefinition);
			switch (action) {
			case AssemblyAction.Copy:
			case AssemblyAction.CopyUsed:
				assembly.OutputPath = assembly.InputPath;
				continue;
			case AssemblyAction.Link:
			case AssemblyAction.Save:
				log.Log ($"Saving {assembly.InputPath} to {assembly.OutputPath}");
				break;
			default:
				exceptions.Add (ErrorHelper.CreateError (99, $"Unknown link action: {action} for assembly {assemblyDefinition.Name}"));
				return false;
			}

			PathUtils.CreateDirectoryForFile (assembly.OutputPath);
			var writerParameters = new WriterParameters ();
			if (assemblyDefinition.MainModule.HasSymbols) {
				var provider = new CustomSymbolWriterProvider ();
				try {
					using (var tmp = provider.GetSymbolWriter (assemblyDefinition.MainModule, Path.ChangeExtension (assembly.OutputPath, ".pdb"))) { }
					File.Delete (Path.ChangeExtension (assembly.OutputPath, ".pdb"));
					writerParameters.WriteSymbols = true;
					writerParameters.SymbolWriterProvider = provider;
				} catch (Exception e) {
					log.Log ($"Failed to create symbol writer for {assembly.OutputPath}, not writing symbols: {e.Message}");
				}
			}

			RemoveCrossGen (assemblyDefinition);

			try {
				assemblyDefinition.Write (assembly.OutputPath, writerParameters);
				ModuleAttributes m = assemblyDefinition.MainModule.Attributes;
			} catch (Exception e) {
				exceptions.Add (ErrorHelper.CreateError (99, e, $"Failed to write {assembly.OutputPath}: {e.Message}"));
				log.Log ($"Failed to write {assembly.OutputPath}: {e}");
				return false;
			}
		}

		return exceptions.Count == 0;
	}

	void RemoveCrossGen (AssemblyDefinition assemblyDefinition)
	{
		// Drop crossgened code from the assembly
		// Ref: https://github.com/dotnet/runtime/blob/b86458593223f866effa63122b05bec37f83015e/src/tools/illink/src/linker/Linker.Steps/OutputStep.cs#L95-L105
		foreach (var module in assemblyDefinition.Modules) {
			var moduleAttributes = module.Attributes;
			var isCrossGened = (moduleAttributes & ModuleAttributes.ILOnly) == 0 && (moduleAttributes & ModuleAttributes.ILLibrary) == ModuleAttributes.ILLibrary;
			if (isCrossGened) {
				moduleAttributes |= ModuleAttributes.ILOnly;
				moduleAttributes &= ~ModuleAttributes.ILLibrary;
				module.Attributes = moduleAttributes;
				module.Architecture = TargetArchitecture.I386;
				module.Characteristics |= ModuleCharacteristics.NoSEH;
			}
		}
	}

	// Figure out if an assembly is trimmed or not.
	// This must be identical to how it's done for ILLink/ILC.
	AssemblyAction ComputeAssemblyAction (AssemblyDefinition assembly, AssemblyPreparerInfo info)
	{
		// Unless 'PublishTrimmed=true', nothing is trimmed, because we won't run the trimmer.
		if (!configuration.PublishTrimmed)
			return AssemblyAction.Copy;

		// Then if 'TrimMode' is set on the assembly, then that takes precedence
		switch (info.TrimMode?.ToLowerInvariant () ?? "") {
		case "link":
			return AssemblyAction.Link;
		case "copy":
			return AssemblyAction.Copy;
		case "":
			break;
		default:
			throw new ArgumentException ($"Unknown trim mode: {info.TrimMode} for assembly {assembly.Name}");
		}

		// Then if 'IsTrimmable' is set on the assembly, that takes precedence over the default for the platform.
		if (info.IsTrimmable == false)
			return AssemblyAction.CopyUsed;
		else if (info.IsTrimmable == true)
			return AssemblyAction.Link;

		// Check the global 'TrimMode' property, if it's not 'link', 'partial' or 'full', then we're not trimming anything
		var globalTrimMode = configuration.TrimMode.ToLowerInvariant ();
		switch (globalTrimMode) {
		case "copy":
		case "":
			return AssemblyAction.Copy;
		case "partial":
		case "full":
		case "link":
			break;
		default:
			throw new ArgumentException ($"Unknown global trim mode: {configuration.TrimMode}");
		}

		// Check the [AssemblyMetadata] attribute
		var isTrimmableAttribute = assembly.CustomAttributes
			.Where (v => v.AttributeType.FullName == "System.Reflection.AssemblyMetadataAttribute")
			.Where (v => v.HasConstructorArguments && v.ConstructorArguments.Count == 2 && v.ConstructorArguments [0].Type.Is ("System", "String") && v.ConstructorArguments [1].Type.Is ("System", "String"))
			.Where (v => (v.ConstructorArguments [0].Value as string) == "IsTrimmable" && string.Equals (v.ConstructorArguments [1].Value as string, "true", StringComparison.OrdinalIgnoreCase))
			.SingleOrDefault ();

		if (isTrimmableAttribute is null) {
			// If the attribute is not present, then we trim if the global 'TrimMode' is 'full'
			return globalTrimMode switch {
				"link" => AssemblyAction.Copy,
				"partial" => AssemblyAction.Copy,
				"full" => AssemblyAction.Link,
				_ => throw new ArgumentException ($"Unknown global trim mode: {configuration.TrimMode}"),
			};
		}

		// if the attribute is present, then we trim if the global 'TrimMode' is 'partial', 'full' or 'link', which are the only values it should have at this point
		switch (globalTrimMode) {
		case "partial":
		case "full":
		case "link":
			break;
		default:
			// we shouldn't get here for any other trim mode value
			throw new ArgumentException ($"Unexpected global trim mode: {configuration.TrimMode}");
		}

		return AssemblyAction.Link;
	}

	public void Dispose ()
	{
		foreach (var assembly in Assemblies)
			assembly.Assembly?.Dispose ();
		configuration.AssemblyResolver.ResolverCache.Clear ();
		configuration.DerivedLinkContext.Assemblies.Clear ();
	}

	static void CollectFieldSymbols (LinkerConfiguration configuration, TypeDefinition type)
	{
		if (type.HasNestedTypes) {
			foreach (var nested in type.NestedTypes)
				CollectFieldSymbols (configuration, nested);
		}

		if (!type.HasProperties)
			return;

		foreach (var property in type.Properties) {
			if (!property.HasCustomAttributes)
				continue;

			foreach (var attrib in property.CustomAttributes) {
				var declaringType = attrib.Constructor.DeclaringType.Resolve ();
				if (!declaringType.Is (Namespaces.Foundation, "FieldAttribute"))
					continue;
				if (attrib.ConstructorArguments.Count < 1)
					continue;
				configuration.FieldSymbols.Add ((string) attrib.ConstructorArguments [0].Value);
				break;
			}
		}
	}
}

public class AssemblyPreparerInfo {
	internal AssemblyDefinition? Assembly { get; set; }

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
