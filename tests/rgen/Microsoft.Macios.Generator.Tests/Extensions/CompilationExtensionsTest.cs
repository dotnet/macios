// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Macios.Generator.Extensions;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Extensions;

public class CompilationExtensionsTest : BaseGeneratorTestClass {

	[Theory]
	[PlatformInlineData (ApplePlatform.iOS, PlatformName.iOS)]
	[PlatformInlineData (ApplePlatform.TVOS, PlatformName.TvOS)]
	[PlatformInlineData (ApplePlatform.MacOSX, PlatformName.MacOSX)]
	[PlatformInlineData (ApplePlatform.MacCatalyst, PlatformName.MacCatalyst)]
	public void GetCurrentPlatformTests (ApplePlatform platform, PlatformName expectedPlatform)
	{
		// get the current compilation for the platform and assert we return the correct one from
		// the compilation
		var (compilation, _) = CreateCompilation (platform);
		Assert.Equal (expectedPlatform, compilation.GetCurrentPlatform ());
	}

	class TestDataGetUINamespaces : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			if (Configuration.IsEnabled (ApplePlatform.iOS)) {
				yield return [
					ApplePlatform.iOS,
					new [] {
						"UIKit",
						"Twitter",
						"NewsstandKit",
						"QuickLook",
						"EventKitUI",
						"AddressBookUI",
						"MessageUI",
						"PhotosUI",
						"HealthKitUI",
						"GameKit",
						"MapKit"
					}
				];
			}

			if (Configuration.IsEnabled (ApplePlatform.TVOS)) {
				yield return [
					ApplePlatform.TVOS,
					new [] {
						"UIKit",
						"PhotosUI",
						"GameKit",
						"MapKit",
					}];
			}

			if (Configuration.IsEnabled (ApplePlatform.MacOSX)) {
				yield return [
					ApplePlatform.MacOSX,
					new [] {
						"AppKit",
						"QuickLook",
						"PhotosUI",
						"GameKit",
						"MapKit",
					}];
			}

			if (Configuration.IsEnabled (ApplePlatform.MacCatalyst)) {
				yield return [
					ApplePlatform.MacCatalyst,
					new [] {
						"AppKit",
						"UIKit",
						"QuickLook",
						"EventKitUI",
						"MessageUI",
						"PhotosUI",
						"HealthKitUI",
						"GameKit",
						"MapKit",
					}];
			}
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetUINamespaces))]
	public void GetUINamespacesTests (ApplePlatform platform, string [] expectedNamespaces)
	{
		var collectionComparer = new CollectionComparer<string> (StringComparer.Ordinal);
		// we need a source to trigger the parsing. Otherwise the compilation will have no syntax
		// trees and therefore no preprocessor symbols
		const string dummyClass = @"
using System;

public class DummyClass {}
";
		var (compilation, _) = CreateCompilation (platform, sources: dummyClass);
		Assert.True (collectionComparer.Equals (expectedNamespaces, compilation.GetUINamespaces (force: true)));
	}

	[Fact]
	public void GetUINamespacesIsCompilationSpecific ()
	{
		// Regression test for https://github.com/dotnet/macios/issues/26184
		// The UI namespaces are cached per-compilation, so interleaving compilations that target
		// different platforms must not race and overwrite each other's cached result. Before the fix
		// a process-global cache could return e.g. the macOS namespaces (which don't contain UIKit)
		// while querying an iOS compilation.
		if (!Configuration.IsEnabled (ApplePlatform.iOS) || !Configuration.IsEnabled (ApplePlatform.MacOSX))
			return;

		const string dummyClass = @"
using System;

public class DummyClass {}
";
		var (iosCompilation, _) = CreateCompilation (ApplePlatform.iOS, sources: dummyClass);
		var (macCompilation, _) = CreateCompilation (ApplePlatform.MacOSX, sources: dummyClass);

		// prime and then interleave the queries; the results must remain stable regardless of order.
		for (var i = 0; i < 10; i++) {
			var iosNamespaces = iosCompilation.GetUINamespaces ();
			var macNamespaces = macCompilation.GetUINamespaces ();

			Assert.Contains ("UIKit", iosNamespaces);
			Assert.DoesNotContain ("UIKit", macNamespaces);
			Assert.Contains ("AppKit", macNamespaces);
			Assert.DoesNotContain ("AppKit", iosNamespaces);
		}
	}
}
