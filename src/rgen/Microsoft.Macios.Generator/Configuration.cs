// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Macios.Generator;

public static class GeneratorConfiguration {

	/// <summary>
	/// Use the global namespace for the generated code.
	/// </summary>
	public const bool UseGlobalNamespace = true;

	/// <summary>
	/// Generate code compatible with bgen output for backward compatibility.
	/// </summary>
	public const bool BGenCompatible = true;
}
