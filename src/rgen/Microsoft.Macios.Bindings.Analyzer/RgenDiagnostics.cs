// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.Macios.Bindings.Analyzer;

namespace Microsoft.Macios.Generator;

public static class RgenDiagnostics {

	/// <summary>
	/// An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0000 = new (
		"RBI0000",
		new LocalizableResourceString (nameof (Bindings.Analyzer.Resources.RBI0000Title), Bindings.Analyzer.Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Bindings.Analyzer.Resources.RBI0000MessageFormat), Bindings.Analyzer.Resources.ResourceManager,
			typeof (Bindings.Analyzer.Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Bindings.Analyzer.Resources.RBI0000Description), Bindings.Analyzer.Resources.ResourceManager,
			typeof (Bindings.Analyzer.Resources))
	);
}
