using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;
using System.Text;

using Mono.Linker;

using Mono.Cecil;
using Mono.Tuner;

using Xamarin.Bundler;
using Xamarin.Linker;
using Xamarin.Tuner;

namespace MonoTouch.Tuner {

	public class LinkerOptions {
		public IEnumerable<AssemblyDefinition> MainAssemblies { get; set; }
		public string OutputDirectory { get; set; }
		public LinkMode LinkMode { get; set; }
		// public AssemblyResolver Resolver { get; set; }
		public IEnumerable<string> SkippedAssemblies { get; set; }
		// public I18nAssemblies I18nAssemblies { get; set; }
		public bool LinkSymbols { get; set; }
		public bool LinkAway { get; set; }
		public bool Device { get; set; }
		public IList<string> ExtraDefinitions { get; set; }
		public bool DebugBuild { get; set; }
		public bool DumpDependencies { get; set; }
		public List<string> WarnOnTypeRef { get; set; }
		public bool RemoveRejectedTypes { get; set; }

		public DerivedLinkContext LinkContext { get; set; }
		public Target Target { get; set; }
		public Application Application { get { return Target.App; } }
	}
}
