// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Linker;
using Mono.Tuner;

using Xamarin.Build;
using Xamarin.Bundler;

namespace MonoTouch.Tuner {
	// Load the assemblies before processing.
	public class LoadAssembliesStep : ConfigurationAwareStep {
		protected override string Name { get; } = "LoadAssemblies";
		protected override int ErrorCode { get; } = 2510;

		protected override void TryProcess ()
		{
			var configuration = Configuration;
			var exceptions = Configuration.Exceptions;
			var log = Configuration;

			var linkContext = configuration.DerivedLinkContext;

			var parameters = new ReaderParameters {
				AssemblyResolver = configuration.AssemblyResolver,
				MetadataResolver = configuration.MetadataResolver,
				ReadSymbols = true,
				SymbolReaderProvider = new DefaultSymbolReaderProvider (throwIfNoSymbol: false),
			};

			foreach (var assembly in configuration.AssemblyInfos) {
				AssemblyDefinition assemblyDefinition;
				try {
					assemblyDefinition = AssemblyDefinition.ReadAssembly (assembly.InputPath, parameters);
					assembly.IsCILAssembly = true;
				} catch (BadImageFormatException) {
					// Not a managed assembly, skip it (pass it through unchanged).
					log.Log ($"Skipping non-managed assembly: {assembly.InputPath}");
					assembly.OutputPath = assembly.InputPath;
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
		}

		// Figure out if an assembly is trimmed or not.
		// This must be identical to how it's done for ILLink/ILC.
		AssemblyAction ComputeAssemblyAction (AssemblyDefinition assembly, AssemblyPreparerInfo info)
		{
			// Unless 'PublishTrimmed=true', nothing is trimmed, because we won't run the trimmer.
			if (!Configuration.PublishTrimmed)
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
			var globalTrimMode = Configuration.TrimMode.ToLowerInvariant ();
			switch (globalTrimMode) {
			case "copy":
			case "":
				return AssemblyAction.Copy;
			case "partial":
			case "full":
			case "link":
				break;
			default:
				throw new ArgumentException ($"Unknown global trim mode: {Configuration.TrimMode}");
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
					_ => throw new ArgumentException ($"Unknown global trim mode: {Configuration.TrimMode}"),
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
				throw new ArgumentException ($"Unexpected global trim mode: {Configuration.TrimMode}");
			}

			return AssemblyAction.Link;
		}
	}
}
