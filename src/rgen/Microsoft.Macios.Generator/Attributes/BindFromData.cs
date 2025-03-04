// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Attributes;

readonly struct BindFromData : IEquatable<BindFromData> {

	public TypeInfo Type { get; }
	public TypeInfo? OriginalType { get; }

	public BindFromData (TypeInfo type)
	{
		Type = type;
	}

	public BindFromData (TypeInfo type, TypeInfo? originalType)
	{
		Type = type;
		OriginalType = originalType;
	}


	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out BindFromData? data)
	{
		data = null;
		var count = attributeData.ConstructorArguments.Length;
		TypeInfo type;
		TypeInfo? originalType = null;

		switch (count) {
		case 1:
			type = new ((INamedTypeSymbol) attributeData.ConstructorArguments [0].Value!);
			break;
		default:
			// no other constructors are available
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			data = new (type);
			return true;
		}

		foreach (var (name, value) in attributeData.NamedArguments) {
			switch (name) {
			case "Type":
				type = new ((INamedTypeSymbol) value.Value!);
				break;
			case "OriginalType":
				originalType = new ((INamedTypeSymbol) value.Value!);
				break;
			default:
				data = null;
				return false;
			}
		}
		data = new (type, originalType);
		return true;
	}

	/// <inheritdoc />
	public bool Equals (BindFromData other)
	{
		return Type.FullyQualifiedName == other.Type.FullyQualifiedName
			   && OriginalType?.FullyQualifiedName == other.OriginalType?.FullyQualifiedName;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is BindFromData other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (Type, OriginalType);
	}

	public static bool operator == (BindFromData x, BindFromData y)
	{
		return x.Equals (y);
	}

	public static bool operator != (BindFromData x, BindFromData y)
	{
		return !(x == y);
	}

	public override string ToString ()
	{
		return $"{{ Type: '{Type.FullyQualifiedName}', OriginalType: '{OriginalType?.FullyQualifiedName ?? "null"}' }}";
	}
}
