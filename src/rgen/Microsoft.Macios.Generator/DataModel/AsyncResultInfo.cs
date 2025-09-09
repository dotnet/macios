// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;

namespace Microsoft.Macios.Generator.DataModel;

/// <summary>
/// Represents information about the result of an asynchronous operation.
/// </summary>
readonly record struct AsyncResultInfo {

	/// <summary>
	/// Gets the namespace of the result type.
	/// </summary>
	public ImmutableArray<string> Namespace { get; init; }

	/// <summary>
	/// Gets the name of the result type.
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// Gets the completion handler parameter.
	/// </summary>
	public Parameter CompletionHandler { get; init; }

	/// <summary>
	/// Gets the fully qualified name of the result type.
	/// </summary>
	public string FullyQualifiedName {
		get {
			if (Namespace.IsDefaultOrEmpty)
				return Name;
			return string.Join (".", Namespace) + "." + Name;
		}
	}
}
