// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.DataModel;

readonly partial struct TypeInfo {

	/// <summary>
	/// True if the type needs to use a stret call.
	/// </summary>
	public bool NeedsStret { get; init; }

	internal TypeInfo (ITypeSymbol symbol, Compilation compilation) : this (symbol)
	{
		NeedsStret = symbol.NeedsStret (compilation);
	}

	/// <summary>
	/// Creates a <see cref="TypeInfo"/> for a delegate proxy (trampoline).
	/// </summary>
	/// <param name="delegateType">The <see cref="TypeInfo"/> of the delegate.</param>
	/// <returns>A new <see cref="TypeInfo"/> representing the delegate proxy, or the original <paramref name="delegateType"/> if it's not a delegate.</returns>
	public static TypeInfo CreateDelegateProxy (TypeInfo delegateType)
	{
		if (!delegateType.IsDelegate)
			return delegateType;

		// build a new type info that is a delegate proxy
		var trampolineName = Nomenclator.GetTrampolineName (delegateType);
		var proxyName = Nomenclator.GetTrampolineClassName (
			trampolineName, Nomenclator.TrampolineClassType.StaticBridgeClass);

		var trampolineInfo = new TypeInfo (
			name: $"ObjCRuntime.Trampolines.{proxyName}",
			specialType: SpecialType.None,
			isNullable: false,
			isBlittable: false,
			isSmartEnum: false,
			isArray: false,
			isReferenceType: true,
			isStruct: false
		) {
			Delegate = null,
			EnumUnderlyingType = null,
			IsGenericType = false,
			IsTask = false,
			TypeArguments = []
		};
		return trampolineInfo;
	}

}
