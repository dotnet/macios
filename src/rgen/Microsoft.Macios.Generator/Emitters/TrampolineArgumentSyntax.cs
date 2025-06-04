// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Represents the conversions needed for a trampoline argument.
/// </summary>
readonly record struct TrampolineArgumentSyntax (ArgumentSyntax ArgumentSyntax) {
	/// <summary>
	/// The syntax to be used for the argument in the delegate call.
	/// </summary>
	public ArgumentSyntax ArgumentSyntax { get; init; } = ArgumentSyntax;

	/// <summary>
	/// Collection of initializers that need to be executed before the delegate call and the
	/// conversion of the argument syntax.
	/// </summary>
	public ImmutableArray<SyntaxNode> Initializers { get; init; } = [];

	/// <summary>
	/// Collection of expressions that need to be called before the delegate call.
	/// </summary>
	public ImmutableArray<SyntaxNode> PreDelegateCallConversion { get; init; } = [];

	/// <summary>
	/// Collection of expressions that need to be called after the delegate call.
	/// </summary>
	public ImmutableArray<SyntaxNode> PostDelegateCallConversion { get; init; } = [];
}
