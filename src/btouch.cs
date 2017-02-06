//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011-2014 Xamarin Inc.
// Copyright 2009-2010 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mono.Options;
using System.Runtime.InteropServices;

using XamCore.ObjCRuntime;
using XamCore.Foundation;
using Xamarin.Utils;

class BindingTouch {
	static Type CoreObject = typeof (XamCore.Foundation.NSObject);

	static TargetFramework? target_framework;
#if MONOMAC
	public static PlatformName CurrentPlatform = PlatformName.MacOSX;
#if XAMCORE_2_0
	public static bool Unified = true;
	public static bool skipSystemDrawing /* full: yes, mobile: no */;
#else
	public static bool Unified = false;
	public static bool skipSystemDrawing = false;
#endif
#elif WATCH
	public static PlatformName CurrentPlatform = PlatformName.WatchOS;
	public static bool Unified = true;
	public static bool skipSystemDrawing = false;
#elif TVOS
	public static PlatformName CurrentPlatform = PlatformName.TvOS;
	public static bool Unified = true;
	public static bool skipSystemDrawing = false;
#elif IOS
	public static PlatformName CurrentPlatform = PlatformName.iOS;
	public static bool skipSystemDrawing = false;
#if XAMCORE_2_0
	public static bool Unified = true;
#else
	public static bool Unified = false;
#endif
#else
	#error Invalid platform.
#endif

#if MONOMAC
	static string baselibdll = "MonoMac.dll";
	static string tool_name = "bmac";
	static string compiler = "/Library/Frameworks/Mono.framework/Commands/mcs";
	static string net_sdk = "4";
#elif WATCH
	static string baselibdll = Path.Combine (GetSDKRoot (), "lib/mono/Xamarin.WatchOS/Xamarin.WatchOS.dll");
	static string tool_name = "bwatch";
	static string compiler = "/Library/Frameworks/Mono.framework/Commands/mcs";
	static string net_sdk = null;
#elif TVOS
	static string baselibdll = Path.Combine (GetSDKRoot (), "lib/mono/Xamarin.TVOS/Xamarin.TVOS.dll");
	static string tool_name = "btv";
	static string compiler = "/Library/Frameworks/Mono.framework/Commands/mcs";
	static string net_sdk = null;
#elif IOS
#if XAMCORE_2_0
	static string baselibdll = Path.Combine (GetSDKRoot (), "lib/mono/Xamarin.iOS/Xamarin.iOS.dll");
#else
	static string baselibdll = Path.Combine (GetSDKRoot (), "lib/mono/2.1/monotouch.dll");
#endif
	static string tool_name = "btouch";
	static string compiler = Path.Combine (GetSDKRoot (), "bin/smcs");
	static string net_sdk = null;
#else
#error Unknown platform
#endif
	static char shellQuoteChar;

	public static string ToolName {
		get { return tool_name; }
	}

	static void ShowHelp (OptionSet os)
	{
		Console.WriteLine ("{0} - Mono Objective-C API binder", tool_name);
		Console.WriteLine ("Usage is:\n {0} [options] apifile1.cs [--api=apifile2.cs [--api=apifile3.cs]] [-s=core1.cs [-s=core2.cs]] [core1.cs [core2.cs]] [-x=extra1.cs [-x=extra2.cs]]", tool_name);
		
		os.WriteOptionDescriptions (Console.Out);
	}
	
	static int Main (string [] args)
	{
#if !MONOMAC

		// for monotouch.dll we're using a the iOS specific mscorlib.dll, which re-routes CWL to NSLog
               // but that's not what we want for tooling, like the binding generator, so we provide our own
		var sw = new UnexceptionalStreamWriter (Console.OpenStandardOutput ()) { AutoFlush = true };
		Console.SetOut (sw);
#endif
		try {
			return Main2 (args);
		} catch (Exception ex) {
			ErrorHelper.Show (ex);
			return 1;
		}
	}

	static string GetSDKRoot ()
	{
		switch (CurrentPlatform) {
		case PlatformName.iOS:
		case PlatformName.WatchOS:
		case PlatformName.TvOS:
			var sdkRoot = Environment.GetEnvironmentVariable ("MD_MTOUCH_SDK_ROOT");
			if (string.IsNullOrEmpty (sdkRoot))
				sdkRoot = "/Library/Frameworks/Xamarin.iOS.framework/Versions/Current";
			return sdkRoot;
		default:
			throw new BindingException (1047, "Unsupported platform: {0}. Please file a bug report (http://bugzilla.xamarin.com) with a test case.", CurrentPlatform);
		}
	}

	static void SetTargetFramework (string fx)
	{
		TargetFramework tf;
		if (!TargetFramework.TryParse (fx, out tf))
			throw ErrorHelper.CreateError (68, "Invalid value for target framework: {0}.", fx);
		target_framework = tf;

		if (Array.IndexOf (TargetFramework.ValidFrameworks, target_framework.Value) == -1)
			throw ErrorHelper.CreateError (70, "Invalid target framework: {0}. Valid target frameworks are: {1}.", target_framework.Value, string.Join (" ", TargetFramework.ValidFrameworks.Select ((v) => v.ToString ()).ToArray ()));
	}

	static int Main2 (string [] args)
	{
		bool show_help = false;
		bool zero_copy = false;
		string basedir = null;
		string tmpdir = null;
		string ns = null;
		string outfile = null;
		bool delete_temp = true, debug = false;
		bool verbose = false;
		bool unsafef = true;
		bool external = false;
		bool public_mode = true;
		bool nostdlib = false;
		bool inline_selectors = Unified && CurrentPlatform != PlatformName.MacOSX;
		List<string> sources;
		var resources = new List<string> ();
		var linkwith = new List<string> ();
		var references = new List<string> ();
		var libs = new List<string> ();
		var api_sources = new List<string> ();
		var core_sources = new List<string> ();
		var extra_sources = new List<string> ();
		var defines = new List<string> ();
		bool binding_third_party = true;
		string generate_file_list = null;
		bool process_enums = false;

		// .NET treats ' as part of the command name when running an app so we must use " on Windows
		PlatformID pid = Environment.OSVersion.Platform;
		if (((int)pid != 128 && pid != PlatformID.Unix && pid != PlatformID.MacOSX))
			shellQuoteChar = '"'; // Windows
		else
			shellQuoteChar = '\''; // !Windows

		var os = new OptionSet () {
			{ "h|?|help", "Displays the help", v => show_help = true },
			{ "a", "Include alpha bindings (Obsolete).", v => {}, true },
			{ "outdir=", "Sets the output directory for the temporary binding files", v => { basedir = v; }},
			{ "o|out=", "Sets the name of the output library", v => outfile = v },
			{ "tmpdir=", "Sets the working directory for temp files", v => { tmpdir = v; delete_temp = false; }},
			{ "debug", "Generates a debugging build of the binding", v => debug = true },
			{ "sourceonly=", "Only generates the source", v => generate_file_list = v },
			{ "ns=", "Sets the namespace for storing helper classes", v => ns = v },
			{ "unsafe", "Sets the unsafe flag for the build", v=> unsafef = true },
			{ "core", "Use this to build product assemblies", v => binding_third_party = false },
			{ "r=", "Adds a reference", v => references.Add (v) },
			{ "lib=", "Adds the directory to the search path for the compiler", v => libs.Add (Quote (v)) },
			{ "compiler=", "Sets the compiler to use", v => compiler = v },
			{ "sdk=", "Sets the .NET SDK to use", v => net_sdk = v },
			{ "new-style", "Build for Unified (Obsolete).", v => { Console.WriteLine ("The --new-style option is obsolete and ignored."); }, true},
			{ "d=", "Defines a symbol", v => defines.Add (v) },
			{ "api=", "Adds a API definition source file", v => api_sources.Add (Quote (v)) },
			{ "s=", "Adds a source file required to build the API", v => core_sources.Add (Quote (v)) },
			{ "v", "Sets verbose mode", v => verbose = true },
			{ "x=", "Adds the specified file to the build, used after the core files are compiled", v => extra_sources.Add (Quote (v)) },
			{ "e", "Generates smaller classes that can not be subclassed (previously called 'external mode')", v => external = true },
			{ "p", "Sets private mode", v => public_mode = false },
			{ "baselib=", "Sets the base library", v => baselibdll = v },
			{ "use-zero-copy", v=> zero_copy = true },
			{ "nostdlib", "Does not reference mscorlib.dll library", l => nostdlib = true },
			{ "no-mono-path", "Launches compiler with empty MONO_PATH", l => { } },
			{ "native-exception-marshalling", "Enable the marshalling support for Objective-C exceptions", (v) => { /* no-op */} },
			{ "inline-selectors:", "If Selector.GetHandle is inlined and does not need to be cached (enabled by default in Xamarin.iOS, disabled in Xamarin.Mac)", 
				v => inline_selectors = string.Equals ("true", v, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty (v)
			},
			{ "process-enums", "Process enums as bindings, not external, types.", v => process_enums = true },
			{ "link-with=,", "Link with a native library {0:FILE} to the binding, embedded as a resource named {1:ID}",
				(path, id) => {
					if (path == null || path.Length == 0)
						throw new Exception ("-link-with=FILE,ID requires a filename.");
					
					if (id == null || id.Length == 0)
						id = Path.GetFileName (path);
					
					if (linkwith.Contains (id))
						throw new Exception ("-link-with=FILE,ID cannot assign the same resource id to multiple libraries.");
					
					resources.Add (string.Format ("-res:{0},{1}", path, id));
					linkwith.Add (id);
				}
			},
			{ "unified-full-profile", "Launches compiler pointing to XM Full Profile", l => { /* no-op*/ }, true },
			{ "unified-mobile-profile", "Launches compiler pointing to XM Mobile Profile", l => { /* no-op*/ }, true },
			{ "target-framework=", "Specify target framework to use. Only applicable to Xamarin.Mac, and the currently supported values are: 'Xamarin.Mac,Version=v2.0,Profile=Mobile', 'Xamarin.Mac,Version=v4.5,Profile=Full' and 'Xamarin.Mac,Version=v4.5,Profile=System')", v => SetTargetFramework (v) },
		};

		try {
			sources = os.Parse (args);
		} catch (Exception e){
			Console.Error.WriteLine ("{0}: {1}", tool_name, e.Message);
			Console.Error.WriteLine ("see {0} --help for more information", tool_name);
			return 1;
		}

		if (show_help) {
			ShowHelp (os);
			return 0;
		}

		if (sources.Count > 0) {
			api_sources.Insert (0, Quote (sources [0]));
			for (int i = 1; i < sources.Count; i++)
				core_sources.Insert (i - 1, Quote (sources [i]));
		}

		if (api_sources.Count == 0) {
			Console.WriteLine ("Error: no api file provided");
			ShowHelp (os);
			return 1;
		}

		if (tmpdir == null)
			tmpdir = GetWorkDir ();

		string firstApiDefinitionName = Path.GetFileNameWithoutExtension (Unquote (api_sources [0]));
		firstApiDefinitionName = firstApiDefinitionName.Replace ('-', '_'); // This is not exhaustive, but common.
		if (outfile == null)
			outfile = firstApiDefinitionName + ".dll";

		string refs = string.Empty;
		foreach (var r in references) {
			if (refs != string.Empty)
				refs += " ";
			refs += "-r:" + Quote (r);
		}
		string paths = (libs.Count > 0 ? "-lib:" + String.Join (" -lib:", libs.ToArray ()) : "");

		try {
			var tmpass = Path.Combine (tmpdir, "temp.dll");

			// -nowarn:436 is to avoid conflicts in definitions between core.dll and the sources
			// Keep source files at the end of the command line - csc will create TWO assemblies if any sources preceed the -out parameter
			var cargs = new StringBuilder ();

			if (CurrentPlatform == PlatformName.MacOSX) {
				if (!string.IsNullOrEmpty (net_sdk) && net_sdk != "mobile")
					cargs.Append ("-sdk:").Append (net_sdk).Append (' ');
			} else {
				if (!string.IsNullOrEmpty (net_sdk))
					cargs.Append ("-sdk:").Append (net_sdk).Append (' ');
			}
			cargs.Append ("-debug -unsafe -target:library -nowarn:436").Append (' ');
			cargs.Append ("-out:").Append (Quote (tmpass)).Append (' ');
			cargs.Append ("-r:").Append (Environment.GetCommandLineArgs ()[0]).Append (' ');
			cargs.Append (refs).Append (' ');
			if (unsafef)
				cargs.Append ("-unsafe ");
			cargs.Append ("-r:").Append (Quote (baselibdll)).Append (' ');
			foreach (var def in defines)
				cargs.Append ("-define:").Append (def).Append (' ');
			cargs.Append (paths).Append (' ');
			if (nostdlib)
				cargs.Append ("-nostdlib ");
			foreach (var qs in api_sources)
				cargs.Append (qs).Append (' ');
			foreach (var cs in core_sources)
				cargs.Append (cs).Append (' ');
			if (!string.IsNullOrEmpty (Path.GetDirectoryName (baselibdll)))
				cargs.Append ("-lib:").Append (Path.GetDirectoryName (baselibdll)).Append (' ');
			

			var si = new ProcessStartInfo (compiler, cargs.ToString ()) {
				UseShellExecute = false,
			};
				
			// HACK: We are calling btouch with forced 2.1 path but we need working mono for compiler
			si.EnvironmentVariables.Remove ("MONO_PATH");

			if (verbose)
				Console.WriteLine ("{0} {1}", si.FileName, si.Arguments);
			
			var p = Process.Start (si);
			p.WaitForExit ();
			if (p.ExitCode != 0){
				Console.WriteLine ("{0}: API binding contains errors.", tool_name);
				return 1;
			}

			Assembly api;
			try {
				api = Assembly.LoadFrom (tmpass);
			} catch (Exception e) {
				if (verbose)
					Console.WriteLine (e);
				
				Console.Error.WriteLine ("Error loading API definition from {0}", tmpass);
				return 1;
			}

			Assembly baselib;
			try {
				baselib = Assembly.LoadFrom (baselibdll);
			} catch (Exception e){
				if (verbose)
					Console.WriteLine (e);

				Console.Error.WriteLine ("Error loading base library {0}", baselibdll);
				return 1;
			}
			GC.KeepAlive (baselib); // Fixes a compiler warning (unused variable).
				
			foreach (object attr in api.GetCustomAttributes (typeof (LinkWithAttribute), true)) {
				LinkWithAttribute linkWith = (LinkWithAttribute) attr;
				
				if (!linkwith.Contains (linkWith.LibraryName)) {
					Console.Error.WriteLine ("Missing native library {0}, please use `--link-with' to specify the path to this library.", linkWith.LibraryName);
					return 1;
				}
			}

			foreach (var r in references) {
				if (File.Exists (r)) {
					try {
						Assembly.LoadFrom (r);
					} catch (Exception ex) {
						ErrorHelper.Show (new BindingException (1104, false, "Could not load the referenced library '{0}': {1}.", r, ex.Message));
					}
				}
			}

			var types = new List<Type> ();
			var  strong_dictionaries = new List<Type> ();
			foreach (var t in api.GetTypes ()){
				if ((process_enums && t.IsEnum) ||
				    t.GetCustomAttributes (typeof (BaseTypeAttribute), true).Length > 0 ||
				    t.GetCustomAttributes (typeof (ProtocolAttribute), true).Length > 0 ||
				    t.GetCustomAttributes (typeof (StaticAttribute), true).Length > 0 ||
				    t.GetCustomAttributes (typeof (PartialAttribute), true).Length > 0)
					types.Add (t);
				if (t.GetCustomAttributes (typeof (StrongDictionaryAttribute), true).Length > 0)
					strong_dictionaries.Add (t);
			}

			string nsManagerPrefix;
			switch (CurrentPlatform) {
			case PlatformName.MacOSX:
				nsManagerPrefix = Unified ? null : "MonoMac";
				break;
			case PlatformName.iOS:
				nsManagerPrefix = Unified ? null : "MonoTouch";
				break;
			default:
				nsManagerPrefix = null;
				break;
			}

			if (CurrentPlatform == PlatformName.MacOSX && Unified) {
				if (!target_framework.HasValue)
					throw ErrorHelper.CreateError (86, "A target framework (--target-framework) must be specified when building for Xamarin.Mac.");
				skipSystemDrawing = target_framework == TargetFramework.Xamarin_Mac_4_5_Full;
			}

			var nsManager = new NamespaceManager (
				nsManagerPrefix,
				ns == null ? firstApiDefinitionName : ns,
				skipSystemDrawing
			);

			var g = new Generator (nsManager, public_mode, external, debug, types.ToArray (), strong_dictionaries.ToArray ()){
				BindThirdPartyLibrary = binding_third_party,
				CoreNSObject = CoreObject,
				BaseDir = basedir != null ? basedir : tmpdir,
				ZeroCopyStrings = zero_copy,
				InlineSelectors = inline_selectors,
			};

			if (!Unified && !binding_third_party) {
				foreach (var mi in baselib.GetType (nsManager.CoreObjCRuntime + ".Messaging").GetMethods ()){
					if (mi.Name.IndexOf ("_objc_msgSend") != -1)
						g.RegisterMethodName (mi.Name);
				}
			}

			g.Go ();

			if (generate_file_list != null){
				using (var f = File.CreateText (generate_file_list)){
					foreach (var x in g.GeneratedFiles.OrderBy ((v) => v))
						f.WriteLine (x);
				}
				return 0;
			}

			cargs.Clear ();
			if (unsafef)
				cargs.Append ("-unsafe ");
			cargs.Append ("-target:library ");
			cargs.Append ("-out:").Append (Quote (outfile)).Append (' ');
			foreach (var def in defines)
				cargs.Append ("-define:").Append (def).Append (' ');
			foreach (var gf in g.GeneratedFiles)
				cargs.Append (gf).Append (' ');
			foreach (var cs in core_sources)
				cargs.Append (cs).Append (' ');
			foreach (var es in extra_sources)
				cargs.Append (es).Append (' ');
			cargs.Append (refs).Append (' ');
			cargs.Append ("-r:").Append (Quote (baselibdll)).Append (' ');
			foreach (var res in resources)
				cargs.Append (res).Append (' ');
			if (nostdlib)
				cargs.Append ("-nostdlib ");
			if (!string.IsNullOrEmpty (Path.GetDirectoryName (baselibdll)))
				cargs.Append ("-lib:").Append (Path.GetDirectoryName (baselibdll)).Append (' ');
				
			si = new ProcessStartInfo (compiler, cargs.ToString ()) {
				UseShellExecute = false,
			};

			// HACK: We are calling btouch with forced 2.1 path but we need working mono for compiler
			si.EnvironmentVariables.Remove ("MONO_PATH");

			if (verbose)
				Console.WriteLine ("{0} {1}", si.FileName, si.Arguments);

			p = Process.Start (si);
			p.WaitForExit ();
			if (p.ExitCode != 0){
				Console.WriteLine ("{0}: API binding contains errors.", tool_name);
				return 1;
			}
		} finally {
			if (delete_temp)
				Directory.Delete (tmpdir, true);
		}
		return 0;
	}

	static string GetWorkDir ()
	{
		while (true){
			string p = Path.Combine (Path.GetTempPath(), Path.GetRandomFileName());
			if (Directory.Exists (p))
				continue;
			
			var di = Directory.CreateDirectory (p);
			return di.FullName;
		}
	}

	static string Unquote (string input)
	{
		if (input == null || input.Length == 0 || input [0] != shellQuoteChar)
			return input;

		var builder = new StringBuilder ();
		for (int i = 1; i < input.Length - 1; i++) {
			char c = input [i];
			if (c == '\\') {
				builder.Append (input [i + 1]);
				i++;
				continue;
			}
			builder.Append (input [i]);
		}
		return builder.ToString ();
	}

	static string Quote (string input)
	{
		if (String.IsNullOrEmpty (input))
			return input ?? String.Empty;

		var builder = new StringBuilder ();
		builder.Append (shellQuoteChar);
		foreach (var c in input) {
			if (c == '\\')
				builder.Append ('\\');

			builder.Append (c);
		}
		builder.Append (shellQuoteChar);

		return builder.ToString ();
	}
}

#if !MONOMAC
internal class UnexceptionalStreamWriter : StreamWriter
{
	public UnexceptionalStreamWriter (Stream stream)
		: base (stream)
	{
	}

	public override void Flush ()
	{
		try {
			base.Flush ();
		} catch (Exception) {
		}
	}

	public override void Write (char[] buffer, int index,
				    int count)
	{
		try {
			base.Write (buffer, index, count);
		} catch (Exception) {
			NSLog (new string (buffer, index, count));
		}
	}

	public override void Write (char value)
	{
		try {
			base.Write (value);
		} catch (Exception) {
			NSLog (value.ToString ());
		}
	}

	public override void Write (char[] value)
	{
		try {
			base.Write (value);
		} catch (Exception) {
			NSLog (new string (value));
		}
	}

	public override void Write (string value)
	{
		try {
			base.Write (value);
		} catch (Exception) {
			NSLog (value);
		}
	}

	// NSLog support

	[DllImport ("/usr/lib/libobjc.dylib", EntryPoint="objc_msgSend")]
	extern static IntPtr objc_msgSend (IntPtr receiver, IntPtr selector, [MarshalAs (UnmanagedType.LPWStr)] string arg1, IntPtr arg2);

	[DllImport ("/System/Library/Frameworks/Foundation.framework/Foundation")]
	extern static void NSLog (IntPtr format, [MarshalAs (UnmanagedType.LPStr)] string s);

	[DllImport ("/usr/lib/libobjc.dylib")]
	internal static extern IntPtr objc_getClass (string name);

	[DllImport ("/usr/lib/libobjc.dylib", EntryPoint="sel_registerName")]
	public extern static IntPtr sel_registerName (string name);

	internal static void NSLog (string format, params object[] args)
	{
		var val = (args == null || args.Length == 0) ? format : string.Format (format, args);
		const string str = "%s";
		var fmt = objc_msgSend (objc_getClass ("NSString"), sel_registerName ("stringWithCharacters:length:"), str, new IntPtr (str.Length));
		NSLog (fmt, val);
	}
}
#endif // !MONOMAC
