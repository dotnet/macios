using System.Collections.Generic;
using System.IO;
using System.Linq;

using Mono.Cecil;

using Xamarin.Linker;
using Xamarin.Utils;

#nullable enable

namespace Xamarin {

	public class GatherFrameworksStep : ConfigurationAwareStep {
		protected override string Name { get; } = "Gather Frameworks";
		protected override int ErrorCode { get; } = 2310;

		HashSet<string> Frameworks = new HashSet<string> ();
		HashSet<string> WeakFrameworks = new HashSet<string> ();

		protected override void TryProcessAssembly (AssemblyDefinition assembly)
		{
			base.TryProcessAssembly (assembly);

			var app = Configuration.Application;
			if (app.XamarinRuntime == Xamarin.Bundler.XamarinRuntime.NativeAOT && app.NativeAOTSurvivingPInvokeSymbols is not null)
				return;

			global::Frameworks.Gather (app, [assembly], Frameworks, WeakFrameworks);
		}

		protected override void TryEndProcess ()
		{
			var app = Configuration.Application;
			if (app.XamarinRuntime == Xamarin.Bundler.XamarinRuntime.NativeAOT && app.NativeAOTSurvivingPInvokeSymbols is not null)
				global::Frameworks.GatherNativeAOT (app, Configuration.Assemblies, Frameworks, WeakFrameworks);

			Configuration.Application.ComputeLinkerFlags ();

			foreach (var asm in Configuration.Application.Assemblies) {
				Frameworks.UnionWith (asm.Frameworks);
				WeakFrameworks.UnionWith (asm.WeakFrameworks);
			}

			// Remove duplicates. WeakFrameworks takes precedence
			Frameworks.ExceptWith (WeakFrameworks);

			// Write out the frameworks we found and pass them to the MSBuild tasks
			var items = new List<MSBuildItem> ();
			foreach (var fw in Frameworks.OrderBy (v => v)) {
				items.Add (new MSBuildItem (
					GetRelativePathIfFullPath (fw),
					new Dictionary<string, string> {
						{ "IsWeak", "false" },
					}
				));
			}
			foreach (var fw in WeakFrameworks.OrderBy (v => v)) {
				items.Add (new MSBuildItem (
					GetRelativePathIfFullPath (fw),
					new Dictionary<string, string> {
						{ "IsWeak", "true" },
					}
				));
			}

			Configuration.WriteOutputForMSBuild ("_LinkerFrameworks", items);
		}

		string GetRelativePathIfFullPath (string path)
		{
			if (!Path.IsPathRooted (path))
				return path;
			return PathUtils.AbsoluteToRelative (Environment.CurrentDirectory, path);
		}
	}
}
