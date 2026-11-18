using System;
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
			if (app.IsExtension && app.AppExtensionPointIdentifier == "com.apple.fskit.fsmodule")
				swiftSources.Add (GenerateFSKitExtension ());
			Configuration.WriteOutputForMSBuild ("_GeneratedSwiftSource", swiftSources);

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

		// FSKit extensions are started by the Swift-only 'UnaryFileSystemExtension' protocol (with '@main'),
		// so generate a Swift file that implements this protocol using the app's FSUnaryFileSystem subclass.
		// At runtime NSExtensionMain (which we call once .NET has been initialized) finds and calls the
		// Swift '@main' entry point (see the CompileSwift task for how we compile Swift's '@main').
		MSBuildItem GenerateFSKitExtension ()
		{
			var app = Configuration.Application;
			var usesStaticRegistrar = app.Registrar == RegistrarMode.Static || app.Registrar == RegistrarMode.ManagedStatic || app.Registrar == RegistrarMode.TrimmableStatic;

			var fileSystemClasses = usesStaticRegistrar ? FindFileSystemClassesFromRegistrar () : FindFileSystemClassesFromAssemblies ();
			if (fileSystemClasses.Count == 0)
				throw ErrorHelper.CreateError (2325, Errors.MX2325);
			if (fileSystemClasses.Count > 1)
				throw ErrorHelper.CreateError (2326, Errors.MX2326, string.Join (", ", fileSystemClasses.Select (v => v.Type.FullName).OrderBy (v => v)));

			var fileSystemClass = fileSystemClasses [0];
			if (!ImplementsFileSystemOperations (fileSystemClass.Type))
				throw ErrorHelper.CreateError (2327, Errors.MX2327, fileSystemClass.Type.FullName);
			var objcClassName = fileSystemClass.ObjCClassName;
			var moduleName = "XamarinFSKitExtension";
			var directory = Configuration.CacheDirectory;
			var swiftPath = Path.Combine (directory, "fskit-extension.swift");
			var headerPath = Path.Combine (directory, "fskit-extension.h");

			// FSKit was introduced in macOS 15.4, so if the app can run on earlier macOS versions, we must tell the compilers
			// that this code only runs on macOS 15.4+ (it won't run at all on earlier versions, because FSKit extensions don't exist there).
			var needsAvailability = app.DeploymentTarget is not null && app.DeploymentTarget < new Version (15, 4);

			var sb = new StringBuilder ();
			sb.AppendLine ("// This file was generated by the build, to start an FSKit extension.");
			sb.AppendLine ("import ExtensionFoundation");
			sb.AppendLine ("import Foundation");
			sb.AppendLine ("import FSKit");
			sb.AppendLine ();
			if (needsAvailability)
				sb.AppendLine ("@available(macOS 15.4, *)");
			sb.AppendLine ("@main");
			sb.AppendLine ("struct XamarinFSKitExtension: UnaryFileSystemExtension {");
			sb.AppendLine ("\tvar fileSystem: FSUnaryFileSystem & FSUnaryFileSystemOperations {");
			if (usesStaticRegistrar) {
				// The static registrar creates the Objective-C class at build time, so we can reference it directly (the generated header declares it).
				sb.AppendLine ($"\t\t{objcClassName}()");
			} else {
				// The dynamic registrar creates the Objective-C class at runtime, so we have to look it up at runtime.
				sb.AppendLine ($"\t\tlet type = NSClassFromString(\"{objcClassName}\") as! NSObject.Type");
				sb.AppendLine ("\t\treturn type.init() as! FSUnaryFileSystem & FSUnaryFileSystemOperations");
			}
			sb.AppendLine ("\t}");
			sb.AppendLine ("}");
			Driver.WriteIfDifferent (app, swiftPath, sb.ToString (), true);

			var item = new MSBuildItem (swiftPath, new Dictionary<string, string> {
				{ "ModuleName", moduleName },
			});

			if (usesStaticRegistrar) {
				sb.Clear ();
				sb.AppendLine ("// This file was generated by the build, to start an FSKit extension.");
				sb.AppendLine ("#import <FSKit/FSKit.h>");
				sb.AppendLine ();
				if (needsAvailability)
					sb.AppendLine ("API_AVAILABLE(macos(15.4))");
				sb.AppendLine ($"@interface {objcClassName} : FSUnaryFileSystem <FSUnaryFileSystemOperations>");
				sb.AppendLine ("@end");
				Driver.WriteIfDifferent (app, headerPath, sb.ToString (), true);
				item.Metadata ["BridgingHeader"] = headerPath;
			}

			return item;
		}

		// The static registrar has already found all the Objective-C classes in the app, so use those instead of looking through all the types again.
		List<(TypeDefinition Type, string ObjCClassName)> FindFileSystemClassesFromRegistrar ()
		{
			var rv = new List<(TypeDefinition Type, string ObjCClassName)> ();
			foreach (var objcType in Configuration.Application.StaticRegistrar.Types.Values) {
				if (objcType.IsWrapper || objcType.IsProtocol || objcType.IsCategory || objcType.IsModel)
					continue;
				var type = objcType.Type.Resolve ();
				if (type is null || type.IsAbstract)
					continue;
				// A class without a registered base class (such as NSObject) is its own base type, so stop there.
				var baseType = objcType.BaseType;
				while (baseType is not null) {
					if (baseType.Type.Is ("FSKit", "FSUnaryFileSystem")) {
						rv.Add ((type, objcType.ExportedName));
						break;
					}
					if (baseType == baseType.BaseType)
						break;
					baseType = baseType.BaseType;
				}
			}
			return rv;
		}

		List<(TypeDefinition Type, string ObjCClassName)> FindFileSystemClassesFromAssemblies ()
		{
			var registrar = Configuration.Application.StaticRegistrar;
			var rv = new List<(TypeDefinition Type, string ObjCClassName)> ();
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
					rv.Add ((type, registrar.GetExportedTypeName (type, registrar.GetRegisterAttribute (type))));
				}
			}
			return rv;
		}

		static bool ImplementsFileSystemOperations (TypeDefinition? type)
		{
			while (type is not null) {
				if (type.HasInterfaces && type.Interfaces.Any (v => v.InterfaceType.Is ("FSKit", "IFSUnaryFileSystemOperations")))
					return true;
				type = type.BaseType?.Resolve ();
			}
			return false;
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
