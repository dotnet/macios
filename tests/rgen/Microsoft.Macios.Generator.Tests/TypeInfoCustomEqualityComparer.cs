// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Tests;

class TypeInfoCustomEqualityComparer : IEqualityComparer<TypeInfo?>{
	
	public bool Equals (TypeInfo? x, TypeInfo? y)
	{
		if (x is null)
			return y is null;
		if (y is null)
			return false;
		if (x.Value.SpecialType == SpecialType.System_Void)
			return y.Value.SpecialType == SpecialType.System_Void;
		
		return x?.FullyQualifiedName == y?.FullyQualifiedName;
	}

	public int GetHashCode ([DisallowNull] TypeInfo? obj)
	{
		throw new System.NotImplementedException();
	}
}
