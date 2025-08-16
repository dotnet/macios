// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Extensions;

namespace Microsoft.Macios.Generator.Attributes;

readonly struct FieldData<T> : IEquatable<FieldData<T>> where T : Enum {

	public enum ParsingError {
		None = 0,
		NotIdentifier,
		UnknownConstructor,
		UnknownNamedArgument,
	}
	public string SymbolName { get; }
	public string? LibraryName { get; }

	/// <summary>
	/// Gets and set the type to be used. This is a property that can be used on notifications.
	/// </summary>
	public string? Type { get; init; }

	/// <summary>
	/// The notification center to be used, if null, the default one will be used.
	/// </summary>
	public string? NotificationCenter { get; init; }

	public T? Flags { get; } = default;

	/// <summary>
	/// The location of the attribute in source code.
	/// </summary>
	public Location? Location { get; init; }

	internal FieldData (string symbolName, string? libraryName, T? flags)
	{
		SymbolName = symbolName;
		LibraryName = libraryName;
		Flags = flags;
	}

	internal FieldData (string symbolName, T? flags) : this (symbolName, null, flags) { }

	internal FieldData (string symbolName) : this (symbolName, null, default) { }

	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out FieldData<T>? data)
		=> TryParse (attributeData, out data, out _);

	public static bool TryParse (AttributeData attributeData,
		[NotNullWhen (true)] out FieldData<T>? data, out AttributeParsingError<ParsingError> error)
	{
		data = default;
		error = new (ParsingError.None, null);

		var count = attributeData.ConstructorArguments.Length;
		string? symbolName;
		string? libraryName = null;
		T? flags = default;

		// notifications customizations
		string? notificationType = null;
		string? notificationCenter = null;

		switch (count) {
		case 1:
			if (!attributeData.ConstructorArguments [0].TryGetIdentifier (out symbolName)) {
				error = new (ParsingError.NotIdentifier, attributeData.ConstructorArguments [0].Value);
				return false;
			}
			break;
		case 2:
			if (!attributeData.ConstructorArguments [0].TryGetIdentifier (out symbolName)) {
				return false;
			}

			if (attributeData.ConstructorArguments [1].Value is string) {
				libraryName = (string?) attributeData.ConstructorArguments [1].Value!;
			} else {
				flags = (T) attributeData.ConstructorArguments [1].Value!;
			}
			break;
		case 3:
			if (!attributeData.ConstructorArguments [0].TryGetIdentifier (out symbolName)) {
				return false;
			}
			libraryName = (string?) attributeData.ConstructorArguments [1].Value!;
			flags = (T) attributeData.ConstructorArguments [2].Value!;
			break;
		default:
			// 0 should not be an option.
			return false;
		}

		if (attributeData.NamedArguments.Length == 0) {
			data = new (symbolName, libraryName, flags) {
				Location = attributeData.GetLocation (),
			};
			return true;
		}

		// LibraryName can be a param value
		foreach (var (name, value) in attributeData.NamedArguments) {
			switch (name) {
			case "LibraryName":
				libraryName = (string?) value.Value!;
				break;
			case "Flags":
				flags = (T) value.Value!;
				break;
			case "Type":
				notificationType = ((INamedTypeSymbol) value.Value!).ToDisplayString ();
				break;
			case "NotificationCenter":
				notificationCenter = (string?) value.Value!;
				break;
			default:
				data = null;
				error = new (ParsingError.UnknownNamedArgument, name);
				return false;
			}
		}

		data = new (symbolName, libraryName, flags) {
			Type = notificationType,
			NotificationCenter = notificationCenter,
			Location = attributeData.GetLocation (),
		};
		return true;
	}

	/// <inheritdoc />
	public bool Equals (FieldData<T> other)
	{
		if (SymbolName != other.SymbolName)
			return false;
		if (LibraryName != other.LibraryName)
			return false;
		if (Type != other.Type)
			return false;
		if (NotificationCenter != other.NotificationCenter)
			return false;
		if (Flags is not null && other.Flags is not null) {
			return Flags.Equals (other.Flags);
		}
		return false;
	}

	/// <inheritdoc />
	public override bool Equals (object? obj)
	{
		return obj is FieldData<T> other && Equals (other);
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		return HashCode.Combine (SymbolName, LibraryName, Flags);
	}

	public static bool operator == (FieldData<T> x, FieldData<T> y)
	{
		return x.Equals (y);
	}

	public static bool operator != (FieldData<T> x, FieldData<T> y)
	{
		return !(x == y);
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		var sb = new StringBuilder ($"{{ SymbolName: '{SymbolName}', ");
		sb.Append ($"LibraryName: '{LibraryName ?? "null"}', ");
		sb.Append ($"Type: '{Type ?? "null"}', ");
		sb.Append ($"NotificationCenter: '{NotificationCenter ?? "null"}', ");
		sb.Append ($"Flags: '{Flags}' }}");
		return sb.ToString ();
	}
}
