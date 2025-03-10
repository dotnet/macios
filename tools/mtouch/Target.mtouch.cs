// Copyright 2013--2014 Xamarin Inc. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.IO;

using MonoTouch.Tuner;

using Mono.Cecil;

namespace Xamarin.Bundler {
	public partial class Target {
		public string TargetDirectory;
		public string AppTargetDirectory;

		public MonoTouchManifestResolver ManifestResolver = new MonoTouchManifestResolver ();
		public AssemblyDefinition ProductAssembly;

		public void Initialize ()
		{
			// we want to load our own mscorlib[-runtime].dll, not something else we're being feeded
			// (e.g. bug #6612) since it might not match the libmono[-sgen].a library we'll link with,
			// so load the corlib we want first.

			var corlib_path = Path.Combine (Resolver.FrameworkDirectory, "mscorlib.dll");
			var corlib = ManifestResolver.Load (corlib_path);
			if (corlib is null)
				throw new ProductException (2006, true, Errors.MT2006, corlib_path);

			var roots = new List<AssemblyDefinition> ();
			foreach (var root_assembly in App.RootAssemblies) {
				var root = ManifestResolver.Load (root_assembly);
				if (root is null) {
					// We check elsewhere that the path exists, so I'm not sure how we can get into this.
					throw ErrorHelper.CreateError (2019, Errors.MT2019, root_assembly);
				}
				roots.Add (root);
			}

			foreach (var reference in App.References) {
				var ad = ManifestResolver.Load (reference);
				if (ad is null)
					throw new ProductException (2002, true, Errors.MT2002, reference);

				var root_assembly = roots.FirstOrDefault ((v) => v.MainModule.FileName == ad.MainModule.FileName);
				if (root_assembly is not null) {
					// If we asked the manifest resolver for assembly X and got back a root assembly, it means the requested assembly has the same identity as the root assembly, which is not allowed.
					throw ErrorHelper.CreateError (23, Errors.MT0023, root_assembly.MainModule.FileName, reference);
				}

				if (ad.MainModule.Runtime > TargetRuntime.Net_4_0)
					ErrorHelper.Show (new ProductException (11, false, Errors.MT0011, Path.GetFileName (reference), ad.MainModule.Runtime));

				// Figure out if we're referencing Xamarin.iOS or monotouch.dll
				var filename = ad.MainModule.FileName;
				if (Path.GetFileNameWithoutExtension (filename) == Driver.GetProductAssembly (App))
					ProductAssembly = ad;
			}
		}
	}
}
