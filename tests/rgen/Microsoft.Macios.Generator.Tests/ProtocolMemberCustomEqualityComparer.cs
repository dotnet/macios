// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Macios.Generator.Attributes;

namespace Microsoft.Macios.Generator.Tests;

/// <summary>
/// Custom equality comparer for <see cref="ProtocolMemberData"/> used in tests.
/// Performs comparison based on the fully qualified names of type infos rather than reference equality.
/// </summary>
class ProtocolMemberCustomEqualityComparer : IEqualityComparer<ProtocolMemberData> {
	TypeInfoCustomEqualityComparer typeComparer = new ();
	/// <summary>
	/// Determines whether the specified <see cref="ProtocolMemberData"/> objects are equal.
	/// </summary>
	/// <param name="x">The first <see cref="ProtocolMemberData"/> to compare.</param>
	/// <param name="y">The second <see cref="ProtocolMemberData"/> to compare.</param>
	/// <returns><c>true</c> if the specified objects are equal; otherwise, <c>false</c>.</returns>
	public bool Equals (ProtocolMemberData x, ProtocolMemberData y)
	{
		if (x.IsRequired != y.IsRequired)
			return false;
		if (x.IsProperty != y.IsProperty)
			return false;
		if (x.IsStatic != y.IsStatic)
			return false;
		if (x.Name != y.Name)
			return false;
		if (x.Selector != y.Selector)
			return false;
		if (!typeComparer.Equals (x.ReturnType, y.ReturnType))
			return false;
		if (x.ReturnTypeDelegateProxy?.FullyQualifiedName != y.ReturnTypeDelegateProxy?.FullyQualifiedName)
			return false;
		if (!x.ParameterType.SequenceEqual (y.ParameterType, (l, r) => typeComparer.Equals (l, r)))
			return false;
		if (!x.ParameterByRef.SequenceEqual (y.ParameterByRef))
			return false;
		if (!x.ParameterBlockProxy.SequenceEqual (y.ParameterBlockProxy, (l, r) => typeComparer.Equals (l, r)))
			return false;
		if (x.IsVariadic != y.IsVariadic)
			return false;
		if (!typeComparer.Equals (x.PropertyType, y.PropertyType))
			return false;
		if (x.GetterSelector != y.GetterSelector)
			return false;
		if (x.SetterSelector != y.SetterSelector)
			return false;
		if (x.ArgumentSemantic != y.ArgumentSemantic)
			return false;
		return true;
	}

	/// <summary>
	/// Returns a hash code for the specified <see cref="ProtocolMemberData"/>.
	/// </summary>
	/// <param name="obj">The <see cref="ProtocolMemberData"/> for which a hash code is to be returned.</param>
	/// <returns>A hash code for the specified object.</returns>
	public int GetHashCode (ProtocolMemberData obj)
	{
		var hashCode = new HashCode ();
		hashCode.Add (obj.IsRequired);
		hashCode.Add (obj.IsProperty);
		hashCode.Add (obj.IsStatic);
		hashCode.Add (obj.Name);
		hashCode.Add (obj.Selector);
		hashCode.Add (obj.ReturnType);
		hashCode.Add (obj.ReturnTypeDelegateProxy);
		hashCode.Add (obj.ParameterType);
		hashCode.Add (obj.ParameterByRef);
		hashCode.Add (obj.ParameterBlockProxy);
		hashCode.Add (obj.IsVariadic);
		hashCode.Add (obj.PropertyType);
		hashCode.Add (obj.GetterSelector);
		hashCode.Add (obj.SetterSelector);
		hashCode.Add ((int) obj.ArgumentSemantic);
		return hashCode.ToHashCode ();
	}
}
