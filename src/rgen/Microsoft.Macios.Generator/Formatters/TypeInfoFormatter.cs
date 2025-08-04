// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Formatters;

static class TypeInfoFormatter {

	/// <summary>
	/// Converts a TypeInfo to a TypeSyntax, handling arrays, named tuples, generic types, pointers, and nullable types.
	/// </summary>
	/// <param name="typeInfo">The TypeInfo to convert to syntax.</param>
	/// <returns>A TypeSyntax representing the type with proper namespace qualification and nullability.</returns>
	public static TypeSyntax GetIdentifierSyntax (this in TypeInfo typeInfo)
	{
		TypeSyntax classSyntax;
		// the type info already provides the correct name, but we need to build the actual class for arrays and 
		// generic types
		if (typeInfo.IsVoid) {
			return IdentifierName ("void");
		}
		if (typeInfo.IsArray) {
			// could be a params array or simply an array
			classSyntax = ArrayType (IdentifierName (typeInfo.Name))
				.WithRankSpecifiers (SingletonList (
					ArrayRankSpecifier (
						SingletonSeparatedList<ExpressionSyntax> (OmittedArraySizeExpression ()))));
		} else if (typeInfo.IsNamedTuple) {
			// create the tuple elements
			var tupleElements = ImmutableArray.CreateBuilder<TupleElementSyntax> (typeInfo.NamedTupleFields.Length);
			foreach (var (name, type) in typeInfo.NamedTupleFields) {
				var element = TupleElement (IdentifierName (type))
					.WithIdentifier (Identifier (name).WithLeadingTrivia (Space));
				tupleElements.Add (element);
			}

			classSyntax = TupleType (
				SeparatedList<TupleElementSyntax> (
				tupleElements.ToSyntaxNodeOrTokenArray ()
			));
		} else if (typeInfo.IsGenericType) {
			// build the argument list
			var parameterBucket = ImmutableArray.CreateBuilder<TypeSyntax> (typeInfo.TypeArguments.Length);
			foreach (var currentGenericType in typeInfo.TypeArguments) {
				// build the parameter
				parameterBucket.Add (IdentifierName (currentGenericType));
			}

			// generates the function parameter list:
			// example:
			// <IntPtr, int, int, int>
			// that is, the block ptr, the parameters and the return type
			var parametersSyntax = TypeArgumentList (
				SeparatedList<TypeSyntax> (
					parameterBucket.ToSyntaxNodeOrTokenArray ())).NormalizeWhitespace ();
			classSyntax = GenericName (Identifier (typeInfo.Name))
				.WithTypeArgumentList (parametersSyntax);
		} else if (typeInfo.IsPointer) {
			classSyntax = PointerType (IdentifierName (typeInfo.Name));
		} else {
			// dealing with a non-array or generic type
			classSyntax = IdentifierName (typeInfo.Name);
		}

		// build the full type name using the namespace and the class name
		if (!typeInfo.IsNamedTuple)
			classSyntax = classSyntax.ToString ().GetIdentifierName (typeInfo.Namespace);
		// we still need to check if the type is nullable
		return typeInfo.IsNullable ? NullableType (classSyntax) : classSyntax;
	}
}
