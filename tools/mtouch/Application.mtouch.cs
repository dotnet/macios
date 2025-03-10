// Copyright 2013 Xamarin Inc. All rights reserved.
using System.Collections.Generic;
using System.IO;

using MonoTouch.Tuner;


using Mono.Cecil;
using Mono.Tuner;

using Xamarin.Utils;
using System.Collections;

namespace Xamarin.Bundler {

	public partial class Application {
		public string ProductName = "Xamarin.iOS";

		void BuildInitialize ()
		{
			ValidateAbi ();
			Initialize ();
		}

		void Initialize ()
		{
			var FrameworkDirectory = Driver.GetPlatformFrameworkDirectory (this);

			var appContentDirectory = ContentDirectory;

			{
				var target = new Target (this);

				target.TargetDirectory = appContentDirectory;
				target.AppTargetDirectory = IsSimulatorBuild ? appContentDirectory : Path.Combine (appContentDirectory, Is64Build ? ".monotouch-64" : ".monotouch-32");
				// target.ArchDirectory = Cache.Location;
				// target.Resolver.ArchDirectory = Driver.GetArchDirectory (this, Is64Build);
				target.Abis = abis;

				Targets.Add (target);

				// Make sure there aren't any lingering .monotouch-* directories.
				if (IsSimulatorBuild) {
					var dir = Path.Combine (appContentDirectory, ".monotouch-32");
					if (Directory.Exists (dir))
						Directory.Delete (dir, true);
					dir = Path.Combine (appContentDirectory, ".monotouch-64");
					if (Directory.Exists (dir))
						Directory.Delete (dir, true);
				}
			}

			var RootDirectory = Path.GetDirectoryName (Path.GetFullPath (RootAssemblies [0]));
			foreach (var target in Targets) {
				target.Resolver.FrameworkDirectory = FrameworkDirectory;
				target.Resolver.RootDirectory = RootDirectory;
				target.ManifestResolver.FrameworkDirectory = target.Resolver.FrameworkDirectory;
				target.ManifestResolver.RootDirectory = target.Resolver.RootDirectory;
				target.ManifestResolver.ArchDirectory = target.Resolver.ArchDirectory;
				target.Initialize ();

				if (!Directory.Exists (target.TargetDirectory))
					Directory.CreateDirectory (target.TargetDirectory);
			}

			InitializeCommon ();

			Driver.Watch ("Resolve References", 1);
		}
	}
}
