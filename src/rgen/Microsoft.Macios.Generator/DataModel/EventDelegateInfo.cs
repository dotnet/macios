// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Represents information about an event delegate.
/// </summary>
readonly record struct EventDelegateInfo {

	/// <summary>
	/// The usings required for the event delegate.
	/// </summary>
	public ImmutableArray<string> Usings { get; init; }

	/// <summary>
	/// The namespace of the event delegate.
	/// </summary>
	public string Namespace { get; init; }

	/// <summary>
	/// The name of the outer class that contains the event delegate.
	/// </summary>
	public string OuterClassName { get; init; }

	/// <summary>
	/// The modifiers for the outer class.
	/// </summary>
	public ImmutableArray<SyntaxToken> OuterClassModifiers { get; init; }

	/// <summary>
	/// The type information for the delegate.
	/// </summary>
	public TypeInfo DelegateType { get; init; }

	/// <summary>
	/// The fully qualified name of the delegate type.
	/// </summary>
	public string FullyQualifiedDelegateTypeName
		=> $"{Namespace}.{OuterClassName}.{Nomenclator.GetInternalDelegateForEventName (DelegateType)}";


	/// <inheritdoc />
	public override string ToString ()
	{
		return $"{{ Namespace = {Namespace}, OuterClassName = {OuterClassName}, DelegateType = {DelegateType} }}";
	}
}
