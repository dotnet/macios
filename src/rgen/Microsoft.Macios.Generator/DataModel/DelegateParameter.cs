// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Readonly structure that describes a parameter in a delegate. This class contains less information
/// than Parameter since some of the extra fields make no sense in delegates.
/// </summary>
[StructLayout (LayoutKind.Auto)]
readonly partial struct DelegateParameter : IEquatable<DelegateParameter> {

	/// <summary>
	/// Parameter position in the method.
	/// </summary>
	public int Position { get; }

	/// <summary>
	/// Type of the parameter.
	/// </summary>
	public TypeInfo Type { get; }

	/// <summary>
	/// Parameter name
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// True if the parameter is optional
	/// </summary>
	public bool IsOptional { get; init; }

	/// <summary>
	/// True if a parameter is using the 'params' modifier.
	/// </summary>
	public bool IsParams { get; init; }

	/// <summary>
	/// True if the parameter represents the 'this' pointer.
	/// </summary>
	public bool IsThis { get; init; }

	/// <summary>
	/// The reference type used.
	/// </summary>
	public ReferenceKind ReferenceKind { get; init; }

	/// <summary>
	/// The parameter is passed by reference. This means any possible reference mode: in, out, ref.
	/// </summary>
	public bool IsByRef => ReferenceKind != ReferenceKind.None;

	/// <summary>
	/// True if the delegate was decorated with the BlockCallbackAttribute.
	/// </summary>
	public bool IsBlockCallback { get; init; }

	/// <summary>
	/// True if the delegate was decorated with the CCallbackAttribute.
	/// </summary>
	public bool IsCCallback { get; init; }

	public DelegateParameter (int position, TypeInfo type, string name)
	{
		Position = position;
		Name = name;
		Type = type;
	}

	/// <inheritdoc/>
	public bool Equals (DelegateParameter other)
	{
		if (Position != other.Position)
			return false;
		if (Type != other.Type)
			return false;
		if (Name != other.Name)
			return false;
		if (IsOptional != other.IsOptional)
			return false;
		if (IsParams != other.IsParams)
			return false;
		if (IsThis != other.IsThis)
			return false;
		if (ForcedType != other.ForcedType)
			return false;
		if (BindAs != other.BindAs)
			return false;
		return ReferenceKind == other.ReferenceKind;
	}

	/// <inheritdoc/>
	public override bool Equals (object? obj)
	{
		return obj is DelegateParameter other && Equals (other);
	}

	/// <inheritdoc/>
	public override int GetHashCode ()
	{
		var hashCode = new HashCode ();
		hashCode.Add (Position);
		hashCode.Add (Type);
		hashCode.Add (Name);
		hashCode.Add (IsOptional);
		hashCode.Add (IsParams);
		hashCode.Add (IsThis);
		hashCode.Add ((int) ReferenceKind);
		return hashCode.ToHashCode ();
	}

	public static bool operator == (DelegateParameter left, DelegateParameter right)
	{
		return left.Equals (right);
	}

	public static bool operator != (DelegateParameter left, DelegateParameter right)
	{
		return !left.Equals (right);
	}

	/// <summary>
	/// Converts this <see cref="DelegateParameter"/> to a <see cref="Parameter"/>.
	/// </summary>
	/// <returns>A new <see cref="Parameter"/> instance.</returns>
	public Parameter ToParameter () => new (Position, Type, Name);

	/// <inheritdoc/>
	public override string ToString ()
	{
		var sb = new StringBuilder ("{");
		sb.Append ($"Position: {Position}, ");
		sb.Append ($"Type: {Type}, ");
		sb.Append ($"Name: {Name}, ");
		sb.Append ($"IsOptional: {IsOptional}, ");
		sb.Append ($"IsParams: {IsParams}, ");
		sb.Append ($"IsThis: {IsThis}, ");
		sb.Append ($"ReferenceKind: {ReferenceKind}, ");
		sb.Append ($"ForcedType: {ForcedType?.ToString () ?? "null"} ");
		sb.Append ('}');
		return sb.ToString ();
	}
}
