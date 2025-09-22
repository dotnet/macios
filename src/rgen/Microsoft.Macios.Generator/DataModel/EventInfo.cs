// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Represents information about an event.
/// </summary>
readonly record struct EventInfo {

	/// <summary>
	/// The namespace of the type that contains the event.
	/// </summary>
	public string Namespace { get; init; }
	/// <summary>
	/// The name of the event.
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// The usings required for the event.
	/// </summary>
	public ImmutableArray<string> Usings { get; init; }

	/// <summary>
	/// The type name of the event arguments class.
	/// </summary>
	public string? EventArgsType { get; init; }

	/// <summary>
	/// A boolean value indicating whether the event arguments are a tuple.
	/// </summary>
	public bool EventArgsIsTuple { get; init; }

	/// <summary>
	/// The signature of the method that this event is based on.
	/// </summary>
	public string MethodSignature { get; init; }

	/// <summary>
	/// The selector of the method that this event is based on.
	/// </summary>
	public string MethodSelector { get; init; }

	/// <summary>
	/// The parameters of the method that this event is based on.
	/// </summary>
	public ImmutableArray<(string Name, string Type)> MethodParameters { get; init; }

	/// <summary>
	/// The parameters for the event handler delegate.
	/// </summary>
	public ImmutableArray<(string Name, string Type)> EventArgParameters => [.. MethodParameters.Skip (1)];

	/// <summary>
	/// A boolean value indicating whether the event arguments class should be generated.
	/// </summary>
	[MemberNotNullWhen (true, nameof (EventArgsType))]
	public bool ToGenerate { get; init; }

	/// <summary>
	/// The fully qualified name of the EventArgs for the event.
	/// </summary>
	public string EventArgsFullyQualifiedName {
		get {
			if (EventArgsType is null)
				return string.Empty;
			return string.IsNullOrEmpty (Namespace)
				? EventArgsType : $"{Namespace}.{EventArgsType}";
		}
	}

	/// <inheritdoc />
	public override string ToString ()
	{
		return $"{{ Namespace = {Namespace}, Name = {Name}, EventArgsType = {EventArgsType ?? "null"}, MethodParameters = {MethodParameters}, ToGenerate = {ToGenerate} }}";
	}

	/// <inheritdoc />
	public override int GetHashCode ()
	{
		var hashCode = new HashCode ();
		hashCode.Add (Namespace);
		hashCode.Add (Name);
		foreach (var u in Usings)
			hashCode.Add (u);
		hashCode.Add (EventArgsType);
		foreach (var p in MethodParameters)
			hashCode.Add (p);
		hashCode.Add (ToGenerate);
		return hashCode.ToHashCode ();
	}

	/// <inheritdoc />
	public bool Equals (EventInfo other)
	{
		return Namespace == other.Namespace &&
			   Name == other.Name &&
			   Usings.SequenceEqual (other.Usings) &&
			   EventArgsType == other.EventArgsType &&
			   EventArgsIsTuple == other.EventArgsIsTuple &&
			   MethodParameters.SequenceEqual (other.MethodParameters) &&
			   ToGenerate == other.ToGenerate;
	}
}
