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

	/// <summary>
	/// All binding types should be partial.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0001 = new (
		"RBI0001",
		new LocalizableResourceString (nameof (Resources.RBI0001Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0001MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0001Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// BindingType&lt;Class&gt; can only decorate partial classes.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0002 = new (
		"RBI0002",
		new LocalizableResourceString (nameof (Resources.RBI0002Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0002MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0002Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// BindingType&lt;Category&gt; can only decorate partial classes.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0003 = new (
		"RBI0003",
		new LocalizableResourceString (nameof (Resources.RBI0003Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0003MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0003Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// BindingType&lt;Category&gt; can only decorate static classes.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0004 = new (
		"RBI0004",
		new LocalizableResourceString (nameof (Resources.RBI0004Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0004MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0004Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// BindingType&lt;Protocol&gt; can only decorate interfaces.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0005 = new (
		"RBI0005",
		new LocalizableResourceString (nameof (Resources.RBI0005Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0005MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0005Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// BindingType can only decorate enumerators.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0006 = new (
		"RBI0006",
		new LocalizableResourceString (nameof (Resources.RBI0006Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0006MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0006Description), Resources.ResourceManager,
			typeof (Resources))
	);

	internal static readonly DiagnosticDescriptor RBI0007 = new (
		"RBI0007",
		new LocalizableResourceString (nameof (Resources.RBI0007Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0007MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0007Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// All enum values must have a Field attribute
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0008 = new (
		"RBI0008",
		new LocalizableResourceString (nameof (Resources.RBI0008Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0008MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0008Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Do not allow duplicated backing fields
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0009 = new (
		"RBI0009",
		new LocalizableResourceString (nameof (Resources.RBI0009Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0009MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0009Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Fields must be a valid identifier
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0010 = new (
		"RBI0010",
		new LocalizableResourceString (nameof (Resources.RBI0010Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0010MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0010Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// If not an apple framework, we should provide the library path
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0011 = new (
		"RBI0011",
		new LocalizableResourceString (nameof (Resources.RBI0011Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0011MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0011Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// if apple framework, the library path should be empty
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0012 = new (
		"RBI0012",
		new LocalizableResourceString (nameof (Resources.RBI0012Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0012MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0012Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// User used the wrong flag for the attribute
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0013 = new (
		"RBI0013",
		new LocalizableResourceString (nameof (Resources.RBI0013Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0013MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0013Description), Resources.ResourceManager,
			typeof (Resources))
	);

	internal static readonly DiagnosticDescriptor RBI0014 = new (
		"RBI0014",
		new LocalizableResourceString (nameof (Resources.RBI0014Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0014MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0014Description), Resources.ResourceManager,
			typeof (Resources))
	);

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

	/// <summary>
	/// Diagnostic descriptor for when a property export is missing a selector.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0018 = new (
		"RBI0018",
		new LocalizableResourceString (nameof (Resources.RBI0018Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0018MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0018Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a property export selector contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0019 = new (
		"RBI0019",
		new LocalizableResourceString (nameof (Resources.RBI0019Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0019MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0019Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a named parameter is used with an incorrect flag.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0020 = new (
		"RBI0020",
		new LocalizableResourceString (nameof (Resources.RBI0020Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0020MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0020Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when an invalid combination of flags is used.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0021 = new (
		"RBI0021",
		new LocalizableResourceString (nameof (Resources.RBI0021Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0021MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0021Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a method export is missing a selector.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0022 = new (
		"RBI0022",
		new LocalizableResourceString (nameof (Resources.RBI0022Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0022MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0022Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a method export selector contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0023 = new (
		"RBI0023",
		new LocalizableResourceString (nameof (Resources.RBI0023Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0023MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0023Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a native prefix or suffix contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0024 = new (
		"RBI0024",
		new LocalizableResourceString (nameof (Resources.RBI0024Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0024MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0024Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a type name contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0025 = new (
		"RBI0025",
		new LocalizableResourceString (nameof (Resources.RBI0025Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0025MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0025Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when an async method name contains whitespace.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0026 = new (
		"RBI0026",
		new LocalizableResourceString (nameof (Resources.RBI0026Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0026MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0026Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when an symbol can be reached in an unsupported platform.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0027 = new (
		"RBI0027",
		new LocalizableResourceString (nameof (Resources.RBI0027Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0027MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0027Description), Resources.ResourceManager,
			typeof (Resources))
	);

	/// <summary>
	/// Diagnostic descriptor for when a flag is ignored because the attribute is not used in the right context.
	/// </summary>
	internal static readonly DiagnosticDescriptor RBI0028 = new (
		"RBI0028",
		new LocalizableResourceString (nameof (Resources.RBI0028Title), Resources.ResourceManager, typeof (Resources)),
		new LocalizableResourceString (nameof (Resources.RBI0028MessageFormat), Resources.ResourceManager,
			typeof (Resources)),
		"Usage",
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true,
		description: new LocalizableResourceString (nameof (Resources.RBI0028Description), Resources.ResourceManager,
			typeof (Resources))
	);
}
