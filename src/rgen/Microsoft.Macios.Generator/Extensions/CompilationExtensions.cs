// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.Macios.Generator.Extensions;

static class CompilationExtensions {
	/// <summary>
	/// Return the current platform that the compilation is targeting.
	/// </summary>
	/// <param name="self">The current compilation.</param>
	/// <returns>The target platform of the current compilation.</returns>
	public static PlatformName GetCurrentPlatform (this Compilation self)
	{
		// use the reference assembly to determine what platform we are binding
		foreach (var referenceAssembly in self.ReferencedAssemblyNames) {
			switch (referenceAssembly.Name) {
			case "Microsoft.iOS":
				return PlatformName.iOS;
			case "Microsoft.MacCatalyst":
				return PlatformName.MacCatalyst;
			case "Microsoft.macOS":
				return PlatformName.MacOSX;
			case "Microsoft.tvOS":
				return PlatformName.TvOS;
			}
		}

		// possible special case when we are compiling the Microsoft.* assembly
		switch (self.AssemblyName) {
		case "Microsoft.iOS":
			return PlatformName.iOS;
		case "Microsoft.MacCatalyst":
			return PlatformName.MacCatalyst;
		case "Microsoft.macOS":
			return PlatformName.MacOSX;
		case "Microsoft.tvOS":
			return PlatformName.TvOS;
		}

		// we cannot identify the platform we are working on
		return PlatformName.None;
	}

	public static ImmutableArray<string> GetPreprocessorSymbols (this Compilation self)
	{
		CSharpSyntaxTree? firstTree = (CSharpSyntaxTree?) self.SyntaxTrees.FirstOrDefault ();

		return firstTree is null ? ImmutableArray<string>.Empty : [.. firstTree.Options.PreprocessorSymbolNames];
	}


	// list of known ui namespaces
	static readonly HashSet<string> uiNamespaces = new HashSet<string> {
		"AppKit",
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
	};

	readonly static ConditionalWeakTable<Compilation, IReadOnlySet<string>> uiNamespacesCache = new ();

	/// <summary>
	/// Calculates the UI namespaces for the current compilation.
	/// </summary>
	/// <param name="self">The compilation under qury.</param>
	/// <param name="force">For the namespaces to be recalculated.</param>
	/// <returns>The currently configured UI namespaces.</returns>
	public static IReadOnlySet<string> GetUINamespaces (this Compilation self, bool force = false)
	{
		// The cache is keyed on the compilation, so that concurrent compilations targeting different
		// platforms (e.g. an iOS and a macOS compilation running in parallel) do not race and overwrite
		// each other's results.
		if (force)
			uiNamespacesCache.Remove (self);

		return uiNamespacesCache.GetValue (self, static compilation => {
			var result = new HashSet<string> ();
			// build the hash set based on the current definitions used in the compilation
			var preprocessorSymbols = compilation.GetPreprocessorSymbols ();
			foreach (var ns in uiNamespaces) {
				var define = $"HAS_{ns.ToUpper ()}";
				if (preprocessorSymbols.Contains (define)) {
					result.Add (ns);
				}
			}
			return result;
		});
	}
}
