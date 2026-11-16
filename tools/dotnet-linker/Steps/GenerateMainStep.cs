using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

using Mono.Cecil;
using Mono.Tuner;

using Xamarin.Bundler;
using Xamarin.Linker;

#nullable enable

namespace Xamarin {

	public class GenerateMainStep : ConfigurationAwareStep {
		protected override string Name { get; } = "Generate Main";
		protected override int ErrorCode { get; } = 2320;

		// These properties are set by the runtime (in xamarin_vm_initialize / xamarin_bridge_vm_initialize)
		// and passed to coreclr_initialize, so they must not also come from the runtimeconfig.json (otherwise
		// we'd pass duplicate properties to coreclr_initialize). Keep in sync with runtime/runtime.m and the
		// _RuntimeConfigReservedProperties item group in Xamarin.Shared.Sdk.targets.
		static readonly string [] reservedRuntimeConfigProperties = new [] {
			"APP_CONTEXT_BASE_DIRECTORY",
			"APP_PATHS",
			"PINVOKE_OVERRIDE",
			"TRUSTED_PLATFORM_ASSEMBLIES",
			"NATIVE_DLL_SEARCH_DIRECTORIES",
			"RUNTIME_IDENTIFIER",
			"SYSTEM_CORELIB_DIRECTORY",
			"STARTUP_HOOKS",
		};

		protected override void TryEndProcess ()
		{
			base.TryEndProcess ();

			var registration_methods = new List<string> (Configuration.RegistrationMethods);
			var items = new List<MSBuildItem> ();

			var app = Configuration.Application;

			// For CoreCLR we bake the runtimeconfig.json 'configProperties' directly into the app (as C arrays
			// in the generated main), instead of shipping the binary runtimeconfig format and decoding it at
			// startup. MonoVM and NativeAOT are unaffected (they don't use xamarin_bridge_compute_properties).
			if (app.XamarinRuntime == XamarinRuntime.CoreCLR)
				app.RuntimeConfigProperties = LoadRuntimeConfigProperties ();

			if (app.GenerateTrustedPlatformAssemblies) {
				app.TrustedPlatformAssemblies.AddRange (app.Assemblies.Select (v => v.FileName));
				app.TrustedPlatformAssemblies.AddRange (Configuration.AddedAssemblies.Select (v => Path.GetFileName (v.Path)));
			}

			var swiftSources = new List<MSBuildItem> ();
			if (app.IsExtension && string.IsNullOrEmpty (app.AppExtensionMainFunction) && app.AppExtensionPointIdentifier == "com.apple.fskit.fsmodule")
				swiftSources.Add (new MSBuildItem (GenerateFSKitExtension ()));
			Configuration.WriteOutputForMSBuild ("_GeneratedSwiftSource", swiftSources);

			if (app.AppExtensionMainFunction is string mainFunction && mainFunction.Length > 0 && !IsValidCIdentifier (mainFunction))
				throw ErrorHelper.CreateError (2324, Errors.MX2324, mainFunction);

			// We want this called before any other initialization methods.
			registration_methods.Insert (0, "xamarin_initialize_dotnet");

			var abi = Configuration.Abi;
			var file = Path.Combine (Configuration.CacheDirectory, $"main.{abi.AsArchString ()}.mm");
			var contents = new StringBuilder ();

			contents.AppendLine ("#include <stdlib.h>");
			contents.AppendLine ();
			contents.AppendLine ("static void xamarin_initialize_dotnet ()");
			contents.AppendLine ("{");
			if (Configuration.Application.PackageManagedDebugSymbols && (Configuration.Application.UseInterpreter || Configuration.Application.XamarinRuntime == XamarinRuntime.CoreCLR))
				contents.AppendLine ($"\tsetenv (\"DOTNET_MODIFIABLE_ASSEMBLIES\", \"debug\", 1);");
			contents.AppendLine ("}");
			contents.AppendLine ();

			Configuration.Application.GenerateMain (contents, app.Platform, abi, file, registration_methods);

			var item = new MSBuildItem (
				file,
				new Dictionary<string, string> {
					{ "Arch", abi.AsArchString () },
				}
			);
			if (app.EnableDebug)
				item.Metadata.Add ("Arguments", "-DDEBUG");
			items.Add (item);

			if (app.RequiresPInvokeWrappers) {
				var state = Configuration.PInvokeWrapperGenerationState!;
				item = new MSBuildItem (
					state.SourcePath,
					new Dictionary<string, string> {
						{ "Arch", abi.AsArchString () },
					}
				);
				if (app.EnableDebug)
					item.Metadata.Add ("Arguments", "-DDEBUG");
				items.Add (item);
			}

			Configuration.WriteOutputForMSBuild ("_MainFile", items);

			var linkWith = new List<MSBuildItem> ();
			if (Configuration.CompilerFlags.LinkWithLibraries is not null) {
				foreach (var lib in Configuration.CompilerFlags.LinkWithLibraries) {
					linkWith.Add (new MSBuildItem (lib));
				}
			}
			if (Configuration.CompilerFlags.ForceLoadLibraries is not null) {
				foreach (var lib in Configuration.CompilerFlags.ForceLoadLibraries) {
					linkWith.Add (new MSBuildItem (
						lib,
						new Dictionary<string, string> {
							{ "ForceLoad", "true" },
						}
					));
				}
			}

			string? extensionlib = null;
			if (app.IsTVExtension) {
				extensionlib = "libtvextension-dotnet-coreclr.a";
			} else if (app.IsExtension) {
				extensionlib = "libextension-dotnet-coreclr.a";
			}
			if (!string.IsNullOrEmpty (extensionlib)) {
				linkWith.Add (new MSBuildItem (
					Path.Combine (Configuration.XamarinNativeLibraryDirectory, extensionlib),
					new Dictionary<string, string> {
						{ "ForceLoad", "true" },
					}
				));
			}

			Configuration.WriteOutputForMSBuild ("_MainLinkWith", linkWith);
		}

		// Reads the (already merged with the *.runtimeconfig.dev.json) runtimeconfig.json file and extracts
		// the 'runtimeOptions.configProperties' as a string->string dictionary, matching the value conversion
		// done by the shared RuntimeConfigParserTask (from dotnet/runtime) that MonoVM uses.
		Dictionary<string, string>? LoadRuntimeConfigProperties ()
		{
			var path = Configuration.RuntimeConfigurationFilePath;
			if (string.IsNullOrEmpty (path) || !File.Exists (path))
				return null;

			var options = new JsonDocumentOptions {
				AllowTrailingCommas = true,
				CommentHandling = JsonCommentHandling.Skip,
			};

			using var document = JsonDocument.Parse (File.ReadAllText (path), options);
			if (!document.RootElement.TryGetProperty ("runtimeOptions", out var runtimeOptions))
				return null;
			if (!runtimeOptions.TryGetProperty ("configProperties", out var configProperties))
				return null;
			if (configProperties.ValueKind != JsonValueKind.Object)
				throw ErrorHelper.CreateError (2323, $"The 'runtimeOptions.configProperties' value in '{path}' must be a JSON object, but it's a {configProperties.ValueKind}.");

			var result = new Dictionary<string, string> ();
			foreach (var property in configProperties.EnumerateObject ()) {
				result [property.Name] = property.Value.ValueKind switch {
					JsonValueKind.String => property.Value.GetString () ?? "",
					JsonValueKind.True => "true",
					JsonValueKind.False => "false",
					JsonValueKind.Number => property.Value.GetRawText (),
					_ => throw ErrorHelper.CreateError (2321, $"Unsupported value for the runtime configuration property '{property.Name}' in '{path}'."),
				};
			}

			foreach (var reserved in reservedRuntimeConfigProperties) {
				if (result.ContainsKey (reserved))
					throw ErrorHelper.CreateError (2322, $"The runtime configuration property '{reserved}' can't be set by the user, it's reserved for the runtime.");
			}

			return result;
		}

		static bool IsValidCIdentifier (string name)
		{
			if (name.Length == 0 || char.IsDigit (name [0]))
				return false;
			return name.All (c => c == '_' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'));
		}

		// FSKit extensions are started by the Swift-only 'UnaryFileSystemExtension' protocol (normally with '@main'),
		// so generate a Swift file that implements this protocol using the app's FSUnaryFileSystem subclass,
		// and start the extension from that file once .NET has been initialized.
		string GenerateFSKitExtension ()
		{
			var app = Configuration.Application;
			var fileSystemTypes = new List<TypeDefinition> ();
			foreach (var assembly in Configuration.Assemblies) {
				// Only look in assemblies that reference the platform assembly.
				if (Configuration.Profile.IsSdkAssembly (assembly))
					continue;
				if (!assembly.MainModule.AssemblyReferences.Any (v => Configuration.Profile.IsProductAssembly (v.Name)))
					continue;
				foreach (var type in GetAllTypes (assembly.MainModule.Types)) {
					if (type.IsAbstract || type.Is ("FSKit", "FSUnaryFileSystem"))
						continue;
					if (!type.Inherits ("FSKit", "FSUnaryFileSystem",
#if ASSEMBLY_PREPARER
						Configuration.MetadataResolver
#else
						Configuration.Context
#endif
					))
						continue;
					fileSystemTypes.Add (type);
				}
			}

			if (fileSystemTypes.Count == 0)
				throw ErrorHelper.CreateError (2325, Errors.MX2325);
			if (fileSystemTypes.Count > 1)
				throw ErrorHelper.CreateError (2326, Errors.MX2326, string.Join (", ", fileSystemTypes.Select (v => v.FullName).OrderBy (v => v)));

			var registrar = app.StaticRegistrar;
			var fileSystemType = fileSystemTypes [0];
			var objcClassName = registrar.GetExportedTypeName (fileSystemType, registrar.GetRegisterAttribute (fileSystemType));

			app.AppExtensionMainFunction = "xamarin_fskit_extension_main";

			var sb = new StringBuilder ();
			sb.AppendLine ("// This file was generated by the build, to start an FSKit extension.");
			sb.AppendLine ("import ExtensionFoundation");
			sb.AppendLine ("import Foundation");
			sb.AppendLine ("import FSKit");
			sb.AppendLine ();
			sb.AppendLine ("struct XamarinFSKitExtension: UnaryFileSystemExtension {");
			sb.AppendLine ("\tvar fileSystem: FSUnaryFileSystem & FSUnaryFileSystemOperations {");
			// Look up the class at runtime, because it might not exist at build time (if it's registered by the dynamic registrar).
			sb.AppendLine ($"\t\tlet type = NSClassFromString(\"{objcClassName}\") as! NSObject.Type");
			sb.AppendLine ("\t\treturn type.init() as! FSUnaryFileSystem & FSUnaryFileSystemOperations");
			sb.AppendLine ("\t}");
			sb.AppendLine ("}");
			sb.AppendLine ();
			sb.AppendLine ($"@_cdecl(\"{app.AppExtensionMainFunction}\")");
			sb.AppendLine ("func xamarinFSKitExtensionMain(_ argc: Int32, _ argv: UnsafeMutablePointer<UnsafeMutablePointer<CChar>?>?) -> Int32 {");
			sb.AppendLine ("\tdo {");
			sb.AppendLine ("\t\ttry MainActor.assumeIsolated {");
			sb.AppendLine ("\t\t\ttry XamarinFSKitExtension.main()");
			sb.AppendLine ("\t\t}");
			sb.AppendLine ("\t} catch {");
			sb.AppendLine ("\t\tNSLog(\"%@\", \"Failed to start the FSKit extension: \\(error)\" as NSString)");
			sb.AppendLine ("\t\treturn 1");
			sb.AppendLine ("\t}");
			sb.AppendLine ("\treturn 0");
			sb.AppendLine ("}");

			var path = Path.Combine (Configuration.CacheDirectory, "fskit-extension.swift");
			Driver.WriteIfDifferent (app, path, sb.ToString (), true);
			return path;
		}

		static IEnumerable<TypeDefinition> GetAllTypes (IEnumerable<TypeDefinition> types)
		{
			foreach (var type in types) {
				yield return type;
				if (type.HasNestedTypes) {
					foreach (var nested in GetAllTypes (type.NestedTypes))
						yield return nested;
				}
			}
		}
	}
}
