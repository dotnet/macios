// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.IO;

using NUnit.Framework;

using Xamarin.MacDev.Tasks;
using Xamarin.Utils;

#nullable enable

namespace Xamarin.MacDev.Tasks.Tests {
	[TestFixture]
	public class StripFrameworkHeadersTaskTests : TestBase {
		[Test]
		public void StripFrameworkDirectories ()
		{
			var appBundle = Path.Combine (Cache.CreateTemporaryDirectory (), "Test.app");
			var flatFramework = Path.Combine (appBundle, "Frameworks", "Flat.framework");
			var versionedFramework = Path.Combine (appBundle, "Frameworks", "Versioned.framework");
			var nestedFramework = Path.Combine (appBundle, "PlugIns", "Extension.appex", "Frameworks", "Nested.framework");
			var unrelatedHeaders = Path.Combine (appBundle, "Headers");
			var resourceHeaders = Path.Combine (versionedFramework, "Versions", "A", "Resources", "Headers");
			var symbolicLinks = new [] {
				Path.Combine (versionedFramework, "Headers"),
				Path.Combine (versionedFramework, "Modules"),
				Path.Combine (versionedFramework, "PrivateHeaders"),
			};
			var directoriesToStrip = new [] {
				Path.Combine (flatFramework, "Headers"),
				Path.Combine (flatFramework, "Modules"),
				Path.Combine (flatFramework, "PrivateHeaders"),
				Path.Combine (versionedFramework, "Versions", "A", "Headers"),
				Path.Combine (versionedFramework, "Versions", "A", "Modules"),
				Path.Combine (versionedFramework, "Versions", "A", "PrivateHeaders"),
				Path.Combine (nestedFramework, "Headers"),
			};

			foreach (var directory in directoriesToStrip) {
				Directory.CreateDirectory (directory);
				File.WriteAllText (Path.Combine (directory, "content"), "content");
			}
			Directory.CreateDirectory (unrelatedHeaders);
			Directory.CreateDirectory (resourceHeaders);

			if (!OperatingSystem.IsWindows ()) {
				foreach (var symbolicLink in symbolicLinks)
					Directory.CreateSymbolicLink (symbolicLink, Path.Combine ("Versions", "A", Path.GetFileName (symbolicLink)));
			}

			var task = CreateTask<StripFrameworkHeaders> ();
			task.AppBundleDir = appBundle;
			ExecuteTask (task);

			foreach (var directory in directoriesToStrip)
				Assert.That (directory, Does.Not.Exist);
			foreach (var symbolicLink in symbolicLinks) {
				Assert.That (symbolicLink, Does.Not.Exist);
				Assert.That (new DirectoryInfo (symbolicLink).LinkTarget, Is.Null);
			}
			Assert.That (unrelatedHeaders, Does.Exist);
			Assert.That (resourceHeaders, Does.Exist);
		}
	}
}
