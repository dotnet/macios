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
				if (!assembly.IsCILAssembly)
					continue;

				var assemblyDefinition = assembly.Assembly;
				if (assemblyDefinition is null) {
					exceptions.Add (ErrorHelper.CreateError (99, $"Assembly definition is null for {assembly.InputPath}"));
					return;
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
	}
}
