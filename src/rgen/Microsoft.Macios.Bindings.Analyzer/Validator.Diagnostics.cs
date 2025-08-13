// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Bindings.Analyzer;

public partial class Validator<T> {

	/// <summary>
	/// Diagnostic descriptor for fields that are required when certain flags are present.
	/// </summary>
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

	/// <summary>
	/// Diagnostic descriptor for fields that must be mutually exclusive.
	/// </summary>
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

	/// <summary>
	/// Diagnostic descriptor for fields that are restricted to specific flag types.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0017 = new (
		"RBI0017",
		new LocalizableResourceString (nameof (Resources.RBI0017Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0017MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0017Description), Resources.ResourceManager,
			typeof (Resources))
	);
}
