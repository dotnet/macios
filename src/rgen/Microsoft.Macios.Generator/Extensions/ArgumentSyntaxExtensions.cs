// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Extensions;

static class ArgumentSyntaxExtensions {

	/// <summary>
	/// Converts an immutable array of syntax nodes to an array of syntax nodes and tokens,
	/// inserting commas between each node for use in syntax separated lists.
	/// </summary>
	/// <typeparam name="T">The type of syntax nodes in the array.</typeparam>
	/// <param name="arguments">The immutable array of syntax nodes to convert.</param>
	/// <returns>An array of syntax nodes and tokens with commas inserted between nodes.</returns>
	public static SyntaxNodeOrToken [] ToSyntaxNodeOrTokenArray<T> (this ImmutableArray<T> arguments) where T : CSharpSyntaxNode
	{
		// the size of the array is simple to calculate, we need space for all parameters
		// and for a comma for each parameter except for the last one
		// length = parameters.Length + parameters.Length - 1
		// length = (2 * parameters.Length) - 1
		if (arguments.Length == 0)
			return [];
		var nodes = new SyntaxNodeOrToken [(2 * arguments.Length) - 1];
		var argsIndex = 0;
		var parametersIndex = 0;
		while (argsIndex < nodes.Length) {
			var currentarg = arguments [parametersIndex++];
			nodes [argsIndex++] = currentarg;
			if (argsIndex < nodes.Length - 1) {
				nodes [argsIndex++] = Token (SyntaxKind.CommaToken).WithTrailingTrivia (Space);
			}
		}

		return nodes;
	}

	/// <summary>
	/// Converts an immutable array builder of syntax nodes to an array of syntax nodes and tokens,
	/// inserting commas between each node for use in syntax separated lists.
	/// </summary>
	/// <typeparam name="T">The type of syntax nodes in the array builder.</typeparam>
	/// <param name="arguments">The immutable array builder of syntax nodes to convert.</param>
	/// <returns>An array of syntax nodes and tokens with commas inserted between nodes.</returns>
	public static SyntaxNodeOrToken [] ToSyntaxNodeOrTokenArray<T> (this ImmutableArray<T>.Builder arguments)
		where T : CSharpSyntaxNode
		=> arguments.ToImmutable ().ToSyntaxNodeOrTokenArray ();

}
