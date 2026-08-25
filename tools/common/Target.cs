// Copyright 2013--2014 Xamarin Inc. All rights reserved.

using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;

using Mono.Cecil;
using Mono.Tuner;
using Xamarin.Linker;

using Xamarin.Utils;
using Registrar;

#if LEGACY_TOOLS
using MonoTouch.Tuner;
using PlatformResolver = MonoTouch.Tuner.MonoTouchResolver;
using PlatformLinkContext = Xamarin.Tuner.DerivedLinkContext;
#else
using PlatformLinkContext = Xamarin.Tuner.DerivedLinkContext;
using PlatformResolver = Xamarin.Linker.DotNetResolver;
#endif

#nullable enable

namespace Xamarin.Bundler {
	public partial class Application {
		public Application App => this;
		public AssemblyCollection Assemblies = new AssemblyCollection (); // The root assembly is not in this list.

#if LEGACY_TOOLS
		public PlatformLinkContext? LinkContext;
#else
		public PlatformLinkContext LinkContext;
#endif
		public PlatformResolver Resolver;

		internal StaticRegistrar StaticRegistrar { get; set; }

#if !LEGACY_TOOLS
		public Assembly AddAssembly (AssemblyDefinition assembly)
		{
			var asm = new Assembly (this, assembly);
			Assemblies.Add (asm);
			return asm;
		}
#endif // !LEGACY_TOOLS

		public PlatformLinkContext? GetLinkContext ()
		{
			return LinkContext;
		}

#if !LEGACY_TOOLS
		public void ExtractNativeLinkInfo (List<Exception> exceptions)
		{
			foreach (var a in Assemblies) {
				try {
					a.ExtractNativeLinkInfo ();
				} catch (Exception e) {
					exceptions.Add (e);
				}
			}
		}
#endif // !LEGACY_TOOLS

		[DllImport ("libc", SetLastError = true)]
		static extern string realpath (string path, IntPtr zero);

		public static string GetRealPath (IToolLog log, string path, bool warnIfNoSuchPathExists = true)
		{
			// For some reason realpath doesn't always like filenames only, and will randomly fail.
			// Prepend the current directory if there's no directory specified.
			if (string.IsNullOrEmpty (Path.GetDirectoryName (path)))
				path = Path.Combine (Environment.CurrentDirectory, path);

			var rv = realpath (path, IntPtr.Zero);
			if (rv is not null)
				return rv;

			var errno = Marshal.GetLastWin32Error ();
			if (warnIfNoSuchPathExists || (errno != 2))
				ErrorHelper.Warning (log, 54, Errors.MT0054, path, FileCopier.strerror (errno), errno);
			return path;
		}

#if !LEGACY_TOOLS
		public void ComputeLinkerFlags ()
		{
			foreach (var a in Assemblies)
				a.ComputeLinkerFlags ();
		}
#endif // !LEGACY_TOOLS

#if !LEGACY_TOOLS
		internal string? GenerateReferencingSource (string reference_m, IEnumerable<Symbol> symbols)
		{
			if (!symbols.Any ()) {
				if (File.Exists (reference_m))
					File.Delete (reference_m);
				return null;
			}
			var sb = new StringBuilder ();
			sb.AppendLine ("#import <Foundation/Foundation.h>");
			foreach (var symbol in symbols) {
				switch (symbol.Type) {
				case SymbolType.Function:
				case SymbolType.Field:
					sb.Append ("extern void * ").Append (symbol.Name).AppendLine (";");
					break;
				case SymbolType.ObjectiveCClass:
					sb.AppendLine ($"@interface {symbol.ObjectiveCName} : NSObject @end");
					break;
				default:
					throw ErrorHelper.CreateError (99, Errors.MX0099, $"invalid symbol type {symbol.Type} for symbol {symbol.Name}");
				}
			}
			sb.AppendLine ("static void __xamarin_symbol_referencer () __attribute__ ((used)) __attribute__ ((optnone));");
			sb.AppendLine ("void __xamarin_symbol_referencer ()");
			sb.AppendLine ("{");
			sb.AppendLine ("\tvoid *value;");
			foreach (var symbol in symbols) {
				switch (symbol.Type) {
				case SymbolType.Function:
				case SymbolType.Field:
					sb.AppendLine ($"\tvalue = {symbol.Name};");
					break;
				case SymbolType.ObjectiveCClass:
					sb.AppendLine ($"\tvalue = [{symbol.ObjectiveCName} class];");
					break;
				default:
					throw ErrorHelper.CreateError (99, Errors.MX0099, $"invalid symbol type {symbol.Type} for symbol {symbol.Name}");
				}
			}
			sb.AppendLine ("}");
			sb.AppendLine ();

			Driver.WriteIfDifferent (App, reference_m, sb.ToString (), true);

			return reference_m;
		}
#endif // !LEGACY_TOOLS

		// This is to load the symbols for all assemblies, so that we can give better error messages
		// (with file name / line number information).
		public void LoadSymbols ()
		{
			foreach (var a in Assemblies)
				a.LoadSymbols ();
		}

#if !LEGACY_TOOLS
		public void GenerateMain (ApplePlatform platform, Abi abi, string main_source, IList<string> registration_methods)
		{
			var sb = new StringBuilder ();
			GenerateMain (sb, platform, abi, main_source, registration_methods);
		}

		public void GenerateMain (StringBuilder sb, ApplePlatform platform, Abi abi, string main_source, IList<string> registration_methods)
		{
			try {
				using (var sw = new StringWriter (sb)) {

					if (registration_methods is not null) {
						foreach (var method in registration_methods) {
							sw.Write ("extern \"C\" void ");
							sw.Write (method);
							sw.WriteLine ("();");
						}
						sw.WriteLine ();
					}

					sw.WriteLine ("static void xamarin_invoke_registration_methods ()");
					sw.WriteLine ("{");
					if (registration_methods is not null) {
						for (int i = 0; i < registration_methods.Count; i++) {
							sw.Write ("\t");
							sw.Write (registration_methods [i]);
							sw.WriteLine ("();");
						}
					}
					sw.WriteLine ("}");
					sw.WriteLine ();

					switch (platform) {
					case ApplePlatform.iOS:
					case ApplePlatform.TVOS:
					case ApplePlatform.MacCatalyst:
					case ApplePlatform.MacOSX:
						GenerateMainImpl (sw, abi);
						break;
					default:
						throw ErrorHelper.CreateError (71, Errors.MX0071, platform, App.ProductName);
					}
				}
				Driver.WriteIfDifferent (App, main_source, sb.ToString (), true);
			} catch (ProductException) {
				throw;
			} catch (Exception e) {
				throw new ProductException (4001, true, e, Errors.MT4001, main_source);
			}
		}

		void GenerateMainImpl (StringWriter sw, Abi abi)
		{
			var app = App;
			var assemblies = Assemblies;
			var assembly_name = App.AssemblyName;
			var assembly_externs = new StringBuilder ();
			var assembly_aot_modules = new StringBuilder ();
			var register_assemblies = new StringBuilder ();
			var assembly_location = new StringBuilder ();
			var assembly_location_count = 0;
			var enable_llvm = (abi & Abi.LLVM) != 0;

			if (app.XamarinRuntime == XamarinRuntime.MonoVM) {
				register_assemblies.AppendLine ("\tGCHandle exception_gchandle = INVALID_GCHANDLE;");
				foreach (var s in assemblies) {
					if (!s.IsAOTCompiled)
						continue;

					var info = s.AssemblyDefinition.Name.Name;
					info = EncodeAotSymbol (info);
					assembly_externs.Append ("extern void *mono_aot_module_").Append (info).AppendLine ("_info;");
					assembly_aot_modules.Append ("\tmono_aot_register_module (mono_aot_module_").Append (info).AppendLine ("_info);");

					string sname = s.FileName;
					if (assembly_name != sname && IsBoundAssembly (s)) {
						register_assemblies.Append ("\txamarin_open_and_register (\"").Append (sname).Append ("\", &exception_gchandle);").AppendLine ();
						register_assemblies.AppendLine ("\txamarin_process_managed_exception_gchandle (exception_gchandle);");
					}
				}
			}

			sw.WriteLine ("#include \"xamarin/xamarin.h\"");

			if (assembly_location.Length > 0) {
				sw.WriteLine ();
				sw.WriteLine ("struct AssemblyLocation assembly_location_entries [] = {");
				sw.WriteLine (assembly_location);
				sw.WriteLine ("};");

				sw.WriteLine ();
				sw.WriteLine ("struct AssemblyLocations assembly_locations = {{ {0}, assembly_location_entries }};", assembly_location_count);
			}

			sw.WriteLine ();
			sw.WriteLine (assembly_externs);

			sw.WriteLine ("void xamarin_register_modules_impl ()");
			sw.WriteLine ("{");
			sw.WriteLine (assembly_aot_modules);
			sw.WriteLine ("}");
			sw.WriteLine ();

			sw.WriteLine ("void xamarin_register_assemblies_impl ()");
			sw.WriteLine ("{");
			sw.WriteLine (register_assemblies);
			sw.WriteLine ("}");
			sw.WriteLine ();

			if (app.UseInterpreter) {
				sw.WriteLine ("extern \"C\" { void mono_ee_interp_init (const char *); }");
				sw.WriteLine ("extern \"C\" { void mono_icall_table_init (void); }");
				sw.WriteLine ("extern \"C\" { void mono_marshal_ilgen_init (void); }");
				sw.WriteLine ("extern \"C\" { void mono_method_builder_ilgen_init (void); }");
				sw.WriteLine ("extern \"C\" { void mono_sgen_mono_ilgen_init (void); }");
			}

			sw.WriteLine ("static const char *xamarin_runtime_libraries_array[] = {");
			foreach (var lib in app.MonoLibraries)
				sw.WriteLine ($"\t\"{Path.GetFileNameWithoutExtension (lib)}\",");
			sw.WriteLine ($"\tNULL");
			sw.WriteLine ("};");

			// For CoreCLR we bake the runtimeconfig.json 'configProperties' directly into the app as C arrays
			// (assigned to the xamarin_runtime_config_property_* globals in xamarin_setup_impl below, and
			// consumed by xamarin_bridge_compute_properties). This avoids shipping the binary runtimeconfig
			// format and decoding it at startup.
			var runtimeConfigProperties = app.XamarinRuntime == XamarinRuntime.CoreCLR ? app.RuntimeConfigProperties : null;
			if (runtimeConfigProperties is not null && runtimeConfigProperties.Count > 0) {
				// Sort by key so the generated main is stable regardless of the dictionary's enumeration order.
				var sortedRuntimeConfigProperties = runtimeConfigProperties.OrderBy (v => v.Key, StringComparer.Ordinal).ToArray ();
				sw.WriteLine ();
				sw.WriteLine ("static const char *xamarin_runtime_config_property_keys_array [] = {");
				foreach (var property in sortedRuntimeConfigProperties)
					sw.WriteLine ($"\t\"{EscapeCString (property.Key)}\",");
				sw.WriteLine ("};");
				sw.WriteLine ("static const char *xamarin_runtime_config_property_values_array [] = {");
				foreach (var property in sortedRuntimeConfigProperties)
					sw.WriteLine ($"\t\"{EscapeCString (property.Value)}\",");
				sw.WriteLine ("};");
			}

			var trusted_platform_assembly_names = app.TrustedPlatformAssemblies
				.Distinct (StringComparer.Ordinal)
				// Any .exe files must be at the end, due to https://github.com/dotnet/runtime/issues/62735
				.OrderBy (v => Path.GetExtension (v).Equals (".exe", StringComparison.OrdinalIgnoreCase))
				.ThenBy (v => v, StringComparer.Ordinal)
				.ToArray ();
			if (app.GenerateTrustedPlatformAssemblies && trusted_platform_assembly_names.Length > 0) {
				sw.WriteLine ();
				sw.WriteLine ("static const char * const xamarin_trusted_platform_assembly_names_array[] = {");
				foreach (var name in trusted_platform_assembly_names)
					sw.WriteLine ("\t\"{0}\",", EscapeCString (name));
				sw.WriteLine ("\tNULL");
				sw.WriteLine ("};");
			}

			sw.WriteLine ("void xamarin_setup_impl ()");
			sw.WriteLine ("{");

			if (app.GenerateTrustedPlatformAssemblies && trusted_platform_assembly_names.Length > 0) {
				sw.WriteLine ("\txamarin_trusted_platform_assembly_names = xamarin_trusted_platform_assembly_names_array;");
				if (app.IsMultiRidBuild) {
					sw.WriteLine ("#if defined (SUPPORTS_UNIVERSAL_BUILDS)");
					sw.WriteLine ("\txamarin_is_multi_rid_build = true;");
					sw.WriteLine ("#endif");
				}
			}

			if (app.UseInterpreter) {
				sw.WriteLine ("\tmono_icall_table_init ();");
				sw.WriteLine ("\tmono_marshal_ilgen_init ();");
				sw.WriteLine ("\tmono_method_builder_ilgen_init ();");
				sw.WriteLine ("\tmono_sgen_mono_ilgen_init ();");
				if ((abi & Abi.x86_64) == Abi.x86_64) {
					sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_INTERP_ONLY);");
				} else {
					sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_INTERP);");
				}
			} else if (app.XamarinRuntime == XamarinRuntime.NativeAOT) {
				// don't call mono_jit_set_aot_mode
			} else if (app.XamarinRuntime == XamarinRuntime.CoreCLR) {
				// don't call mono_jit_set_aot_mode
			} else if (app.IsDeviceBuild) {
				sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_FULL);");
			} else if (app.Platform == ApplePlatform.MacCatalyst && ((abi & Abi.ARM64) == Abi.ARM64)) {
				sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_FULL);");
			} else if (app.IsSimulatorBuild && ((abi & Abi.ARM64) == Abi.ARM64)) {
				sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_FULL);");
			}

			if (assembly_location.Length > 0)
				sw.WriteLine ("\txamarin_set_assembly_directories (&assembly_locations);");

			sw.WriteLine ("\txamarin_invoke_registration_methods ();");

			// Mono doesn't support dllmaps for Mac Catalyst / macOS in .NET, so we're using an alternative:
			// the PINVOKE_OVERRIDE runtime option. Since we have to use it for Mac Catalyst + macOS, let's
			// just use it everywhere to simplify code. This means that at runtime we need to know how we
			// linked to mono, so store that in the xamarin_libmono_native_link_mode variable.
			// Ref: https://github.com/dotnet/runtime/issues/43204 (macOS) https://github.com/dotnet/runtime/issues/48110 (Mac Catalyst)
			sw.WriteLine ($"\txamarin_libmono_native_link_mode = XamarinNativeLinkMode{app.LibMonoNativeLinkMode};");
			sw.WriteLine ($"\txamarin_runtime_libraries = xamarin_runtime_libraries_array;");

			sw.WriteLine ("\txamarin_init_mono_debug = {0};", app.PackageManagedDebugSymbols ? "TRUE" : "FALSE");
			sw.WriteLine ("\txamarin_executable_name = \"{0}\";", assembly_name);
			if (app.XamarinRuntime == XamarinRuntime.MonoVM)
				sw.WriteLine ("\tmono_use_llvm = {0};", enable_llvm ? "TRUE" : "FALSE");
			sw.WriteLine ("\txamarin_log_level = {0};", Verbosity.ToString (CultureInfo.InvariantCulture));
			sw.WriteLine ("\txamarin_arch_name = \"{0}\";", abi.AsArchString ());
			if (!app.IsDefaultMarshalManagedExceptionMode)
				sw.WriteLine ("\txamarin_marshal_managed_exception_mode = MarshalManagedExceptionMode{0};", app.MarshalManagedExceptions);
			sw.WriteLine ("\txamarin_marshal_objectivec_exception_mode = MarshalObjectiveCExceptionMode{0};", app.MarshalObjectiveCExceptions);
			if (app.EnableDebug)
				sw.WriteLine ("\txamarin_debug_mode = TRUE;");
			if (!string.IsNullOrEmpty (app.MonoGCParams) && app.XamarinRuntime == XamarinRuntime.MonoVM)
				sw.WriteLine ("\tsetenv (\"MONO_GC_PARAMS\", \"{0}\", 1);", app.MonoGCParams);
			// Do this last, so that the app developer can override any other environment variable we set.
			foreach (var kvp in app.EnvironmentVariables) {
				var name = kvp.Key;
				var value = kvp.Value.Value;
				var overwrite = kvp.Value.Overwrite;
				sw.WriteLine ("\tsetenv (\"{0}\", \"{1}\", {2});", name.Replace ("\"", "\\\""), value.Replace ("\"", "\\\""), overwrite ? 1 : 0);
			}
			if (app.XamarinRuntime != XamarinRuntime.NativeAOT) {
				if (app.DynamicRegistrationSupported)
					sw.WriteLine ("\txamarin_initialize_dynamic_registrar ();");
				sw.WriteLine ("\txamarin_supports_dynamic_registration = {0};", app.DynamicRegistrationSupported ? "TRUE" : "FALSE");
			}
			sw.WriteLine ("\txamarin_runtime_configuration_name = {0};", string.IsNullOrEmpty (app.RuntimeConfigurationFile) ? "NULL" : $"\"{app.RuntimeConfigurationFile}\"");
			if (runtimeConfigProperties is not null && runtimeConfigProperties.Count > 0) {
				sw.WriteLine ("\txamarin_runtime_config_property_count = {0};", runtimeConfigProperties.Count);
				sw.WriteLine ("\txamarin_runtime_config_property_keys = xamarin_runtime_config_property_keys_array;");
				sw.WriteLine ("\txamarin_runtime_config_property_values = xamarin_runtime_config_property_values_array;");
			}
			if (app.Registrar == RegistrarMode.TrimmableStatic)
				sw.WriteLine ("\txamarin_set_is_trimmable_static_registrar (true);");
			if (app.Registrar == RegistrarMode.ManagedStatic)
				sw.WriteLine ("\txamarin_set_is_managed_static_registrar (true);");
			sw.WriteLine ("}");
			sw.WriteLine ();
			sw.Write ("int main");
			sw.WriteLine (" (int argc, char **argv)");
			sw.WriteLine ("{");
			sw.WriteLine ("\tNSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];");
			if (app.IsExtension) {
				// the name of the executable must be the bundle id (reverse dns notation)
				// but we do not want to impose that (ugly) restriction to the managed .exe / project name / ...
				sw.WriteLine ("\targv [0] = (char *) \"{0}\";", Path.GetFileNameWithoutExtension (app.RootAssemblies [0]));
				sw.WriteLine ("\tint rv = xamarin_main (argc, argv, XamarinLaunchModeExtension);");
			} else {
				sw.WriteLine ("\tint rv = xamarin_main (argc, argv, XamarinLaunchModeApp);");
			}
			sw.WriteLine ("\t[pool drain];");
			sw.WriteLine ("\treturn rv;");
			sw.WriteLine ("}");

			// Add an empty __managed__Main function when building class lib app extensions with NativeAOT to workaround static reference to this symbol from nativeaot-bridge.m
			if (app.IsExtension && app.XamarinRuntime == XamarinRuntime.NativeAOT) {
				sw.WriteLine ();
				sw.Write ("extern \"C\" int __managed__Main (int argc, const char** argv) { return 0; } ");
				sw.WriteLine ();
			}

			sw.WriteLine ();
			sw.WriteLine ("void xamarin_initialize_callbacks () __attribute__ ((constructor));");
			sw.WriteLine ("void xamarin_initialize_callbacks ()");
			sw.WriteLine ("{");
			sw.WriteLine ("\txamarin_setup = xamarin_setup_impl;");
			sw.WriteLine ("\txamarin_register_assemblies = xamarin_register_assemblies_impl;");
			sw.WriteLine ("\txamarin_register_modules = xamarin_register_modules_impl;");
			sw.WriteLine ("}");
		}

		static string EscapeCString (string value)
		{
			var sb = new StringBuilder ();
			foreach (var b in Encoding.UTF8.GetBytes (value)) {
				switch (b) {
				case (byte) '\\':
					sb.Append ("\\\\");
					break;
				case (byte) '"':
					sb.Append ("\\\"");
					break;
				default:
					if (b >= 0x20 && b <= 0x7e) {
						sb.Append ((char) b);
					} else {
						sb.Append ('\\').Append (Convert.ToString (b, 8).PadLeft (3, '0'));
					}
					break;
				}
			}
			return sb.ToString ();
		}

		static readonly char [] charsToReplaceAot = new [] { '.', '-', '+', '<', '>' };
		static string EncodeAotSymbol (string symbol)
		{
			var sb = new StringBuilder ();
			/* This mimics what the aot-compiler does */
			// https://github.com/dotnet/runtime/blob/2f08fcbfece0c09319f237a6aee6f74c4a9e14e8/src/mono/mono/metadata/native-library.c#L1265-L1284
			// https://github.com/dotnet/runtime/blob/2f08fcbfece0c09319f237a6aee6f74c4a9e14e8/src/tasks/Common/Utils.cs#L419-L445
			foreach (var b in System.Text.Encoding.UTF8.GetBytes (symbol)) {
				char c = (char) b;
				if ((c >= '0' && c <= '9') ||
					(c >= 'a' && c <= 'z') ||
					(c >= 'A' && c <= 'Z') ||
					(c == '_')) {
					sb.Append (c);
					continue;
				} else if (charsToReplaceAot.Contains (c)) {
					sb.Append ('_');
				} else {
					// Append the hex representation of b between underscores
					sb.Append ($"_{b:X}_");
				}
			}
			return sb.ToString ();
		}

		static bool IsBoundAssembly (Assembly s)
		{
			if (s.IsFrameworkAssembly == true)
				return false;

			AssemblyDefinition ad = s.AssemblyDefinition;

			foreach (ModuleDefinition md in ad.Modules)
				foreach (TypeDefinition td in md.Types)
					if (td.IsNSObject (s.App.LinkContext))
						return true;

			return false;
		}

#endif // !LEGACY_TOOLS
	}
}
