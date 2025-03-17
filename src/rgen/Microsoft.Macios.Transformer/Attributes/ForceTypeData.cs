// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Transformer.Attributes;

readonly struct ForcedTypeData : IEquatable<ForcedTypeData> {

	public bool Owns { get; } = false;

	public ForcedTypeData (bool owns)
	{
		Owns = owns;
	}

	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out ForcedTypeData? data)
	{
		data = null;
		var count = attributeData.ConstructorArguments.Length;
		bool owns;

		switch (count) {
		case 1:
			owns = (bool) attributeData.ConstructorArguments [0].Value!;
			break;
		default:
			// no other constructors are available
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			data = new (owns);
			return true;
		}

		foreach (var (name, value) in attributeData.NamedArguments) {
			switch (name) {
			case "Owns":
				owns = (bool) value.Value!;
				break;
			default:
				data = null;
				return false;
			}
		}
		data = new (owns);
		return true;
	}

	/// <inheritdoc />
	public bool Equals (ForcedTypeData other)
	{
		return Owns == other.Owns;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is ForcedTypeData other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Owns);
	}

	public static bool operator == (ForcedTypeData x, ForcedTypeData y)
	{
		return x.Equals (y);
	}

	public static bool operator != (ForcedTypeData x, ForcedTypeData y)
	{
		return !(x == y);
	}

	public override string ToString ()
	{
		return $"{{ Owns: {Owns} }}";
	}
}
