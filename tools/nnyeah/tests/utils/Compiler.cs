using System;
using System.IO;
using System.Threading.Tasks;

using Xamarin;
using Xamarin.Utils;
using System.Collections.Generic;

namespace Microsoft.MaciOS.Nnyeah.Tests {

	public enum PlatformName {
		None, // desktop managed executable
		macOS, // Xamarin.Mac app
		iOS,
		watchOS,
		tvOS,
	}

	public class Compiler {
		const string MonoCompiler = "/Library/Frameworks/Mono.framework/Versions/Current/Commands/csc";

		public static async Task<string> CompileText (string text, string outputFile, PlatformName platformName, bool isLibrary)
		{
			var dir = Cache.CreateTemporaryDirectory ();
			var outputCSFile = Path.Combine (dir, "LibraryFile.cs");
			File.WriteAllText (outputCSFile, text);
			return await Compile (outputFile, platformName, isLibrary, dir, outputCSFile);
		}

		public static async Task<string> Compile (string outputFile, PlatformName platformName, bool isLibrary, string workingDirectory, params string[] sourceFiles)
		{
			var compilerArgs = BuildCompilerArgs (sourceFiles, outputFile, platformName, isLibrary);
			Execution execution = await Execution.RunAsync(MonoCompiler, compilerArgs, mergeOutput: true, workingDirectory: workingDirectory);
			return execution!.StandardOutput?.ToString()!;
		}

		static List<string> BuildCompilerArgs (string[] sourceFiles, string outputFile, PlatformName platformName,
			bool isLibrary)
		{
			var args = new List<string>();

			args.Add ("/unsafe");
			args.Add ("/nostdlib+");
			AppendPlatformReference (args, platformName, "mscorlib");
			AppendPlatformReference (args, platformName, XamarinLibName (platformName));
			args.Add ("/debug+");
			args.Add ("/debug:full");
			args.Add ("/optimize-");
			args.Add ("/out:" + outputFile);
			args.Add ("/target:" + (isLibrary ? "library" : "exe"));

			foreach (var file in sourceFiles) {
				args.Add (file);
			}

			return args;
		}

		static void AppendPlatformReference (List<string> args, PlatformName platformName, string libName)
		{
			args.Add("/reference:" + PlatformLibPath (platformName, libName));
		}

		static string PlatformLibPath (PlatformName platformName, string libName)
		{
			return Path.Combine (PlatformLibDirectory (platformName), $"{libName}.dll");
		}

		static string PlatformLibDirectory (PlatformName platformName) =>
			platformName switch {
				PlatformName.macOS => "/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/Xamarin.Mac/",
				PlatformName.iOS => "/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/lib/mono/Xamarin.iOS",
				PlatformName.tvOS => "/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/lib/mono/Xamarin.TVOS",
				PlatformName.watchOS => "/Library/Frameworks/Xamarin.iOS.framework/Versions/Current/lib/mono/Xamarin.WatchOS",
				_ => throw new NotImplementedException (),
			};

		static string XamarinLibName (PlatformName platformName) =>
			platformName switch {
				PlatformName.macOS => "Xamarin.Mac",
				PlatformName.iOS => "Xamarin.iOS",
				PlatformName.tvOS => "Xamarin.TVOS",
				PlatformName.watchOS => "Xamarin.WatchOS",
				_ => throw new NotImplementedException (),
			};
	}
}
