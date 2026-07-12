// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;

using Mono.Cecil;
using Mono.Linker;

using Xamarin.Bundler;
using Xamarin.Utils;

namespace MonoTouch.Tuner {
	// Save the assemblies after processing.
	public class SaveAssembliesStep : ConfigurationAwareStep {
		protected override string Name { get; } = "SaveAssemblies";
		protected override int ErrorCode { get; } = 2500;

		protected override void TryProcess ()
		{
			var configuration = Configuration;
			var exceptions = Configuration.Exceptions;
			var log = Configuration;

			foreach (var assembly in configuration.AssemblyInfos) {
				if (!assembly.IsCILAssembly) {
					// Non-managed assembly, already handled by LoadAssembliesStep (OutputPath = InputPath).
					continue;
				}

				var assemblyDefinition = assembly.Assembly;
				if (assemblyDefinition is null) {
					exceptions.Add (ErrorHelper.CreateError (99, $"Assembly definition is null for {assembly.InputPath}"));
					return;
				}

				var action = configuration.Context.Annotations.GetAction (assemblyDefinition);
				switch (action) {
				case AssemblyAction.Copy:
				case AssemblyAction.CopyUsed:
					OutputWithoutRewriting (assembly);
					continue;
				case AssemblyAction.Link:
				case AssemblyAction.Save:
					if (!configuration.ModifiedAssemblies.Contains (assemblyDefinition)) {
						// The assembly is marked to be saved (e.g. it's part of the set of assemblies to
						// trim), but the assembly-preparer didn't actually modify it, so there's no need to
						// re-serialize it - just output the original assembly.
						OutputWithoutRewriting (assembly);
						continue;
					}
					log.Log ($"Saving {assembly.InputPath} to {assembly.OutputPath}");
					break;
				default:
					exceptions.Add (ErrorHelper.CreateError (99, $"Unknown link action: {action} for assembly {assemblyDefinition.Name}"));
					return;
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
				} catch (Exception e) {
					exceptions.Add (ErrorHelper.CreateError (99, e, $"Failed to write {assembly.OutputPath}: {e.Message}"));
					log.Log ($"Failed to write {assembly.OutputPath}: {e}");
					return;
				}
			}
		}

		void OutputWithoutRewriting (Xamarin.Build.AssemblyPreparerInfo assembly)
		{
			if (Configuration.Application.IsPostProcessingAssemblies && assembly.InputPath != assembly.OutputPath) {
				// During post-processing, copy unchanged assemblies to the output directory
				// so all assemblies are in the same directory (required for AOT compilation).
				CopyAssemblyToOutput (assembly.InputPath, assembly.OutputPath);
			} else {
				assembly.OutputPath = assembly.InputPath;
			}
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

		void CopyAssemblyToOutput (string source, string target)
		{
			PathUtils.CreateDirectoryForFile (target);

			CopyIfNeeded (source, target);
			CopyIfNeeded (Path.ChangeExtension (source, ".pdb"), Path.ChangeExtension (target, ".pdb"));
			CopyIfNeeded (source + ".config", target + ".config");
		}

		void CopyIfNeeded (string source, string target)
		{
			if (!File.Exists (source))
				return;

			// Skip if target is already up-to-date.
			if (File.Exists (target) && File.GetLastWriteTimeUtc (source) <= File.GetLastWriteTimeUtc (target)) {
				Configuration.Log ($"Not copying '{source}' to '{target}' because it's already up-to-date.");
				return;
			}

			Configuration.Log ($"Copying '{source}' to '{target}'.");
			File.Copy (source, target, true);
		}
	}
}
