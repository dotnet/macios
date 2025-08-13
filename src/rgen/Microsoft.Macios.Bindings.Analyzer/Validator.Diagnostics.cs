// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Bindings.Analyzer;

public partial class Validator<T> {

	internal static readonly DiagnosticDescriptor RBI0015 = new (
		"RBI0015",
		new LocalizableResourceString (nameof (Resources.RBI0015Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0015MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0015Description), Resources.ResourceManager,
			typeof (Resources))
	);

	internal static readonly DiagnosticDescriptor RBI0016 = new (
		"RBI0016",
		new LocalizableResourceString (nameof (Resources.RBI0016Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0016MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0016Description), Resources.ResourceManager,
			typeof (Resources))
	);
}
