// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Microsoft.Macios.Generator;

/// <summary>
/// Nomenclator: A person, generally a public official, who announces the names of guests at a party or other
/// social gathering or ceremony.
///
/// In this case, the Nomenclator is used to generate the names of the bindings.
/// </summary>
static class Nomenclator {
	/// <summary>
	/// Returns the name to be used by the extension classes for smart enumerators.
	/// </summary>
	/// <param name="enumName">The name of the smart enum.</param>
	/// <returns>The name of the extension class to be generated for the given smart enum.</returns>
	public static string GetSmartEnumExtensionClassName (string enumName) => $"{enumName}Extensions";
}
