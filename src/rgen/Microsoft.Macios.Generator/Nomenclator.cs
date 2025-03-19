// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.Macios.Generator.DataModel;

namespace Microsoft.Macios.Generator;

/// <summary>
/// Nomenclator: A person, generally a public official, who announces the names of guests at a party or other
/// social gathering or ceremony.
///
/// In this case, the Nomenclator is used to generate the names of the bindings.
/// </summary>
class Nomenclator {

	public enum VariableType {
		BlockLiteral,
		Handle,
		NSArray,
		NSString,
		NSStringStruct,
		PrimitivePointer,
		StringPointer,
		BindFrom,
	}

	// keep track of the generic versions of a trampoline, we will use the fully qualified name
	// of the type to keep track of the generic versions.
	readonly Dictionary<string, int> trampolinesGenericVersions = new ();

	/// <summary>
	/// Returns the name to be used by the extension classes for smart enumerators.
	/// </summary>
	/// <param name="enumName">The name of the smart enum.</param>
	/// <returns>The name of the extension class to be generated for the given smart enum.</returns>
	public static string GetSmartEnumExtensionClassName (string enumName) => $"{enumName}Extensions";

	/// <summary>
	/// Returns the name to be used for a trampoline.
	/// </summary>
	/// <param name="typeInfo">The type info whose trampoline name we require.</param>
	/// <returns>The name of the trampoline to be used for the given type.</returns>
	public string GetTrampolineName (TypeInfo typeInfo)
	{
		// The following algo is used to generate the same trampoline name that bgen used to generate.
		// for that we need to understand how bgen generates the name:
		// old code:
		//
		// var trampolineName = typeInfo.Name.Replace ("`", "Arity");
		// if (typeInfo.IsGenericType) {
		// 	var gdef = typeInfo.GetGenericTypeDefinition ();
		//
		// 	if (!trampolinesGenericVersions.ContainsKey (gdef))
		// 		trampolinesGenericVersions.Add (gdef, 0);
		//
		// 	trampolineName = trampolineName + "V" + trampolinesGenericVersions [gdef]++;
		// }
		// return trampolineName;

		// trampoline name will the the name of the type + the arity + the length of the generic types
		// else it will be the trampoline name 
		var trampolineName = typeInfo.IsGenericType
			? $"{typeInfo.Name [..typeInfo.Name.IndexOf ('<')]}Arity{typeInfo.TypeArguments.Length}"
			: typeInfo.Name;

		if (!typeInfo.IsGenericType)
			return trampolineName;

		// TryAdd will only insert 0 if it is not already present, reduces the dict access to a single operation
		// rather than a contain + add
		trampolinesGenericVersions.TryAdd (typeInfo.Name, 0);
		trampolineName = trampolineName + "V" + trampolinesGenericVersions [typeInfo.Name]++;

		return trampolineName;
	}

	/// <summary>
	/// Returns the name of the aux variable that would have needed for the given parameter. Use the
	/// variable type to name it.
	/// </summary>
	/// <param name="nameHint">Hint of the name for the variable.</param>
	/// <param name="variableType">The type of aux variable.</param>
	/// <returns>The name of the aux variable to use.</returns>
	public static string? GetNameForVariableType (string nameHint, VariableType variableType)
	{
		var cleanedName = nameHint.Replace ("@", "");
		return variableType switch {
			VariableType.BlockLiteral => $"block_ptr_{cleanedName}",
			VariableType.Handle => $"{cleanedName}__handle__",
			VariableType.NSArray => $"nsa_{cleanedName}",
			VariableType.NSString => $"ns{cleanedName}",
			VariableType.NSStringStruct => $"_s{cleanedName}",
			VariableType.PrimitivePointer => $"converted_{cleanedName}",
			VariableType.StringPointer => $"_p{cleanedName}",
			VariableType.BindFrom => $"nsb_{cleanedName}",
			_ => null
		};
	}

	/// <summary>
	/// Get the name of the variable for the type when it is used as a return value.
	/// </summary>
	/// <param name="typeInfo">The type info whose name we want for the return type.</param>
	public static string GetReturnVariableName (in TypeInfo typeInfo) => "ret"; // nothing fancy for now
}
