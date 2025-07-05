// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Represents the various syntax components required to invoke a method.
/// </summary>
readonly record struct MethodInvocations {

	/// <summary>
	/// Gets the transformations and initializations for the method arguments.
	/// </summary>
	public ImmutableArray<TrampolineArgumentSyntax> Arguments { get; init; }

	/// <summary>
	/// Gets the expression syntax for a standard message send invocation.
	/// </summary>
	public ExpressionSyntax Send { get; init; }

	/// <summary>
	/// Gets the expression syntax for a message send invocation to the superclass implementation.
	/// </summary>
	public ExpressionSyntax SendSuper { get; init; }
}
