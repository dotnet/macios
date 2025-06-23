// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.DataModel;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator;

/// <summary>
/// Nomenclator: A person, generally a public official, who announces the names of guests at a party or other
/// social gathering or ceremony.
///
/// In this case, the Nomenclator is used to generate the names of the bindings.
/// </summary>
static class Nomenclator {

	const string globalPrefix = "global::";
	public enum VariableType {
		BlockLiteral,
		Handle,
		NSArray,
		NSString,
		NSStringStruct,
		NullableBlock,
		PrimitivePointer,
		StringPointer,
		BindFrom,
	}

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
	public static string GetTrampolineName (TypeInfo typeInfo)
	{
		// trampoline name will the name of the type + the arity + the length of the generic types
		// else it will be the trampoline name. We will replace any . with _ to ensure that the name is valid
		// when working with nested classes.
		var typeName = typeInfo.Name.Replace ('.', '_');
		var trampolineName = typeInfo.IsGenericType
			? $"{typeName}Arity{typeInfo.TypeArguments.Length}"
			: typeName;

		if (!typeInfo.IsGenericType)
			return trampolineName;

		// trampoline names have to be deterministic. We are going to use the name of the argyment types 
		// to calculate the name of the trampoline.
		var sb = new StringBuilder (trampolineName);
		foreach (var typeArgument in typeInfo.TypeArguments) {
			// we will use the name of the type argument to generate the name of the trampoline
			// we will replace any . with _ to ensure that the name is valid when working with nested classes.
			var argumentName = typeArgument.Replace ('.', '_');
			if (GeneratorConfiguration.UseGlobalNamespace) {
				// remove the global alias if it is present
				argumentName = argumentName.StartsWith (globalPrefix)
					? argumentName.Substring (globalPrefix.Length)
					: argumentName;
			}
			sb.Append (argumentName);
		}
		return sb.ToString ();
	}

	/// <summary>
	/// Enumeration of the trampoline class types that we will generate for the trampolines.
	/// </summary>
	public enum TrampolineClassType {
		/// <summary>
		/// Get name for the C# delegate type (the function signature).
		/// </summary>
		DelegateType,
		/// <summary>
		/// The static helper class bridging the native block invocation to the C# delegate.
		/// </summary>
		StaticBridgeClass,
		/// <summary>
		/// The native invocation class that wraps the block and calls the C# delegate.
		/// </summary>
		NativeInvocationClass,
	}

	/// <summary>
	/// Return the name of the trampoline class to be used for the given type info.
	/// </summary>
	/// <param name="trampolineName">The namne of the trampoline.</param>
	/// <param name="trampolineClassType">The type of class to be generated.</param>
	/// <returns>The name to be used by the generated class.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the trampolineClassType value is out
	/// of range.</exception>
	public static string GetTrampolineClassName (string trampolineName, TrampolineClassType trampolineClassType)
	{
		// get the name of the trampoline a based on the class type we will return one of the other
		return trampolineClassType switch {
			TrampolineClassType.DelegateType => $"D{trampolineName}",
			TrampolineClassType.StaticBridgeClass => $"SD{trampolineName}",
			TrampolineClassType.NativeInvocationClass => $"NID{trampolineName}",
			_ => throw new ArgumentOutOfRangeException (nameof (trampolineClassType), trampolineClassType, null)
		};
	}

	/// <summary>
	/// Return the name of the trampoline class to be used for the given type info.
	/// This is a convenience overload for <see cref="GetTrampolineClassName(string, TrampolineClassType)"/>.
	/// </summary>
	/// <param name="typeInfo">The type info for which to get the trampoline class name.</param>
	/// <param name="trampolineClassType">The type of class to be generated.</param>
	/// <returns>The name to be used by the generated class.</returns>
	public static string GetTrampolineClassName (in TypeInfo typeInfo, TrampolineClassType trampolineClassType)
		=> GetTrampolineClassName (GetTrampolineName (typeInfo), trampolineClassType);

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
			VariableType.NullableBlock => $"block_{cleanedName}",
			VariableType.PrimitivePointer => $"converted_{cleanedName}",
			VariableType.StringPointer => $"_p{cleanedName}",
			VariableType.BindFrom => $"nsb_{cleanedName}",
			_ => null
		};
	}

	/// <summary>
	/// Get the name of the variable for the type when it is used as a return value.
	/// </summary>
	public static string GetReturnVariableName () => "ret"; // nothing fancy for now

	/// <summary>
	/// Returns the name of the trampoline variable for the given parameter info. This variables are used as
	/// temporary variables to hold the value of the parameter before passing it to the trampoline.
	/// </summary>
	/// <param name="parameterInfo">The parameter information for the trampoline.</param>
	/// <returns>The name to be used for the temporary variable or null if it was unknown.</returns>
	public static string? GetNameForTempTrampolineVariable (in DelegateParameter parameterInfo)
	{
#pragma warning disable format
		return parameterInfo switch {
			{ Type.IsReferenceType: false, Type.IsNullable: true } => $"__xamarin_nullified__{parameterInfo.Position}",
			{ Type.SpecialType: SpecialType.System_Boolean } => $"__xamarin_bool__{parameterInfo.Position}",
			{ Type.IsReferenceType: true } => $"__xamarin_pref{parameterInfo.Position}",
			_ => null,
		};
#pragma warning restore format
	}

	/// <summary>
	/// Returns the name of the trampoline delegate variable.
	/// </summary>
	/// <returns>The name of the variable used to store delegates in trampolines.</returns>
	public static string GetTrampolineDelegateVariableName () => "del";

	/// <summary>
	/// Returns the name of the variable used to store the native invoker in trampolines.
	/// </summary>
	/// <returns>The name of the native invoker variable.</returns>
	public static string GetNativeInvokerVariableName () => "invoker";

	/// <summary>
	/// Return the name of the trampoline block parameter. This is the name of the parameter that will be containing the
	/// IntPtr to the trampoline block.
	///
	/// The default value to used is 'block' but we need to ensure that the name is not already used by
	/// the delegate, so we will use block_ptr_ + the name of the delegate if it is already used.
	/// </summary>
	/// <returns>The name to be used in the delegate for the block pointer.</returns>
	public static string GetTrampolineBlockParameterName (in ImmutableArray<DelegateParameter> parameters)
	{
		// the default value of the block pointer is block, yet we need to ensure that the name is not
		// already used by the delegate, so we will use block_ptr_ + the name of the delegate
		var parameterNames = new HashSet<string> (StringComparer.InvariantCulture);
		foreach (var parameter in parameters) {
			parameterNames.Add (parameter.Name);
		}
		// default value
		if (parameterNames.Add ("block_ptr"))
			return "block_ptr";
		// perform a loop until we find a name that is not used	
		for (var i = 0; ; i++) {
			var name = $"block_ptr_{i}";
			if (parameterNames.Add (name))
				return name;
		}
	}

	/// <summary>
	/// Return the name of the invoke method of the trampoline.
	/// </summary>
	/// <returns>The method name to be used.</returns>
	public static string GetTrampolineInvokeMethodName () => "Invoke";

	/// <summary>
	/// Return the name of the invoke method of the trampoline.
	/// </summary>
	/// <returns>The method name to be used.</returns>
	public static string GetTrampolineDelegatePointerVariableName () => "trampoline";

	/// <summary>
	/// Returns the name used for the block literal type.
	/// </summary>
	public static string GetBlockLiteralName () => "BlockLiteral";
}
