using System.Collections.Generic;

using Mono.Linker;
using Xamarin.Bundler;
using Xamarin.Tuner;

using Mono.Cecil;

namespace MonoMac.Tuner {

	public class LinkerOptions {
		public AssemblyDefinition MainAssembly { get; set; }
		public string OutputDirectory { get; set; }
		public bool LinkSymbols { get; set; }
		public LinkMode LinkMode { get; set; }
		public IEnumerable<string> SkippedAssemblies { get; set; }
		public IList<string> ExtraDefinitions { get; set; }
		public bool SkipExportedSymbolsInSdkAssemblies { get; set; }
		public DerivedLinkContext LinkContext { get; set; }
		public Target Target { get; set; }
		public Application Application { get { return Target.App; } }
		public List<string> WarnOnTypeRef { get; set; }
	}
}
