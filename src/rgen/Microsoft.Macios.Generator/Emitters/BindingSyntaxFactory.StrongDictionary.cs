// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Formatters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	/// <summary>
	/// Gets the getter and setter invocations for a strong dictionary property based on its type.
	/// </summary>
	/// <param name="property">The property to generate invocations for.</param>
	/// <returns>A tuple containing the getter and setter expressions.</returns>
	internal static (ExpressionSyntax Getter, ExpressionSyntax Setter) GetStrongDictionaryInvocations (in Property property)
	{
		// decide what methods to use based on the type of the property
#pragma warning disable format
		var dictionaryKey = property.StrongDictionaryKey!;
		var propertyType = property.ReturnType;
		var propertyTypeSyntax = property.ReturnType.WithNullable (isNullable: false).GetIdentifierSyntax ();
		return property.ReturnType switch {
			{ IsArray: false, SpecialType: SpecialType.System_Boolean } => (
				Getter ("GetBoolValue"), 
				Setter ("SetBooleanValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_Int32 } => (
				Getter ("GetInt32Value"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_IntPtr } => (
				Getter ("GetNIntValue"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_Int64 } => (
				Getter ("GetLongValue"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_UInt64 } => (
				Getter ("GetULongValue"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_Single } => (
				Getter ("GetFloatValue"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_Double } => (
				Getter ("GetDoubleValue"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_UInt32 } => (
				Getter ("GetUInt32Value"),
				Setter ("SetNumberValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_UIntPtr } => (
				Getter ("GetNUIntValue"),
				Setter ("SetNumberValue")
				),
			{ IsArray: false, FullyQualifiedName: "CoreGraphics.CGRect" } => (
				Getter ("GetCGRectValue"), 
				Setter ("SetCGRectValue")
				),
			{ IsArray: false, FullyQualifiedName: "CoreGraphics.CGSize" } => (
				Getter ("GetCGSizeValue"), 
				Setter ("SetCGSizeValue")
				),
			{ IsArray: false, FullyQualifiedName: "CoreGraphics.CGPoint" } => (
				Getter ("GetCGPointValue"), 
				Setter ("SetCGPointValue")
				), 
			{ IsArray: false, FullyQualifiedName: "CoreMedia.CMTime" } => (
				Getter ("GetCMTimeValue"), 
				Setter ("SetCMTimeValue")
				),
			{ IsArray: false, IsEnum: true, IsNativeEnum: true, EnumUnderlyingType: SpecialType.System_Int64 } => (
				Getter ("GetNIntValue"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, IsEnum: true, IsNativeEnum: true, EnumUnderlyingType: SpecialType.System_UInt64 } => (
				Getter ("GetNUIntValue"), 
				Setter ("SetNumberValue")
				),
			{ IsArray: false, FullyQualifiedName: "UIKit.UIEdgeInsets" } => (
				Getter ("GetUIEdgeInsets"), 
				Setter ("SetUIEdgeInsets")
				),
			{ IsArray: false, FullyQualifiedName: "Foundation.NSString" } => (
				Getter ("GetNSStringValue"), 
				Setter ("SetStringValue")
				),
			{ IsArray: false, SpecialType: SpecialType.System_String } => (
				Getter ("GetStringValue"), 
				Setter ("SetStringValue")
				),
			{ IsArray: false, FullyQualifiedName: "Foundation.NSDictionary", IsGenericType: false} => (
				Getter ("GetNSDictionary"), 
				Setter ("SetNativeValue")
				),
			{ IsArray: false, FullyQualifiedName: var fqn, IsGenericType: true} when fqn.StartsWith ("Foundation.NSDictionary") => (
				GenericGetter ("GetNSDictionary",
					types: [..propertyType.TypeArguments.Select (TypeSyntax (t) => IdentifierName (t))]
					),
				Setter ("SetNativeValue")
				),
			{ IsArray: false, IsStrongDictionary: true} => (
				GenericGetter ("GetStrongDictionary", [StrongDictionaryLambda ()]), 
				Setter ("SetNativeValue")
				),
			{ IsArray: false, IsWrapped: true } => (
				DictionaryIndex (),
				Setter ("SetNativeValue")
				),
			{ IsArray: false, FullyQualifiedName: "CoreGraphics.CGColorSpace" } => (
				GenericGetter ("GetNativeValue"),
				Setter ("SetNativeValue")
				),
			{ IsArray: false, FullyQualifiedName: "ImageIO.CGImageSource" } => (
				GenericGetter ("GetNativeValue"),
				Setter ("SetNativeValue")
			),
			{ IsArray: false, FullyQualifiedName: "CoreText.CTFontDescriptor" } => (
				GenericGetter ("GetNativeValue"),
				Setter ("SetNativeValue")
				),
			{ IsArray: true, ArrayElementTypeIsWrapped: true } => (
				GenericGetter ("GetArray", 
					types: [propertyType.ToArrayElementType ().GetIdentifierSyntax ()]),
				Setter ("SetArrayValue")
				),
			{ IsArray: true, ArrayElementType: SpecialType.System_String } => (
				GenericGetter ("GetArray", 
					arguments: [StringLambda ()],
					types: [IdentifierName ("string")]
					),
				Setter ("SetArrayValue")
				),
			{ IsArray: true, FullyQualifiedName: "CoreText.CTFontDescriptor" } => (
				GenericGetter ("GetArray",
					types:[propertyType.ToArrayElementType ().GetIdentifierSyntax ()],
					arguments: [CTFontDescriptorLambda ()]),
				Setter ("SetArrayValue")
				),
			{ IsArray: true, ArrayElementTypeIsEnum: true } => (
				GenericGetter ("GetArray", 
					types: [propertyType.ToArrayElementType ().GetIdentifierSyntax ()], 
					arguments: [EnumLambda ()]),
				GenericSetter ("SetArrayValue")
				),
			_ => (ThrowNotImplementedException (), ThrowNotImplementedException ())
#pragma warning restore format
		};

		// Creates a getter method invocation expression.
		// method: The name of the getter method.
		// arguments: Optional additional arguments for the method.
		// Returns: A method invocation expression for the getter.
		ExpressionSyntax Getter (string method, ImmutableArray<ArgumentSyntax> arguments = default)
			=> MemberInvocationExpression (
				methodName: method,
				arguments: arguments.IsDefaultOrEmpty
					? [Argument (IdentifierName (dictionaryKey))]
					: arguments.Insert (0, Argument (IdentifierName (dictionaryKey)))
			);

		// Creates a generic getter method invocation expression.
		// method: The name of the getter method.
		// arguments: Optional additional arguments for the method.
		// type: Optional type argument for the generic method.
		// Returns: A generic method invocation expression for the getter.
		ExpressionSyntax GenericGetter (string method, ImmutableArray<ArgumentSyntax> arguments = default,
			ImmutableArray<TypeSyntax> types = default)
		{
			if (arguments.IsDefaultOrEmpty) {
				arguments = [];
			}

			if (types.IsDefaultOrEmpty) {
				types = [propertyTypeSyntax];
			}
			return GenericMemberInvocationExpression (
				methodName: method,
				typeArguments: types,
				arguments: arguments.Insert (0, Argument (IdentifierName (dictionaryKey)))
			);
		}

		// Creates a setter method invocation expression.
		// method: The name of the setter method.
		// Returns: A method invocation expression for the setter.
		ExpressionSyntax Setter (string method)
		{
			var arguments = ImmutableArray.Create (
				Argument (IdentifierName (dictionaryKey)),
				Argument (IdentifierName ("value")) // TODO: do the appropriate conversion
			);
			return MemberInvocationExpression (method, arguments);
		}

		// Creates a generic setter method invocation expression.
		// method: The name of the setter method.
		// type: Optional type argument for the generic method.
		// Returns: A generic method invocation expression for the setter.
		ExpressionSyntax GenericSetter (string method, TypeSyntax? type = null)
		{
			var arguments = ImmutableArray.Create (
				Argument (IdentifierName (dictionaryKey)),
				Argument (IdentifierName ("value")) // TODO: do the appropriate conversion
			);
			return GenericMemberInvocationExpression (
				methodName: method,
				typeArguments: [
					type ?? propertyTypeSyntax
				],
				arguments: arguments);
		}

		// Creates a lambda expression argument for strong dictionary instantiation.
		// Returns: An argument containing a lambda expression that creates a strong dictionary instance.
		ArgumentSyntax StrongDictionaryLambda ()
		{
			var lambdaVariable = "dict";
			return Argument (
				ParenthesizedLambdaExpression ()
					.WithParameterList (
						ParameterList (
							SingletonSeparatedList (Parameter (Identifier (lambdaVariable))))).NormalizeWhitespace ()
					.WithExpressionBody (
						New (
							propertyTypeSyntax,
							[Argument (IdentifierName (lambdaVariable))]
						).WithLeadingTrivia (Space)
					));
		}

		// Creates a lambda expression argument for string conversion from native handle.
		// Returns: An argument containing a lambda expression that converts a native handle to a string.
		ArgumentSyntax StringLambda ()
		{
			var lambdaVariable = "ptr";
			return Argument (ParenthesizedLambdaExpression ()
				.WithParameterList (
					ParameterList (
						SingletonSeparatedList (
							Parameter (Identifier (lambdaVariable))))).NormalizeWhitespace ()
				.WithExpressionBody (
					PostfixUnaryExpression (
						SyntaxKind.SuppressNullableWarningExpression,
						StringFromHandle ([Argument (IdentifierName (lambdaVariable))])
						).WithLeadingTrivia (Space)));
		}

		// Creates a lambda expression argument for CTFontDescriptor conversion from native handle.
		// Returns: An argument containing a lambda expression that creates a CTFontDescriptor from a native handle.
		ArgumentSyntax CTFontDescriptorLambda ()
		{
			var lambdaVariable = "ptr";
			return Argument (ParenthesizedLambdaExpression ()
				.WithParameterList (
					ParameterList (
						SingletonSeparatedList (
							Parameter (Identifier (lambdaVariable))))).NormalizeWhitespace ()
				.WithExpressionBody (
					New (
						CTFontDescriptor,
						[Argument (IdentifierName (lambdaVariable)), BoolArgument (false)]
						).WithLeadingTrivia (Space)
				)
			);
		}

		// Creates a lambda expression argument for enum conversion from native handle.
		// Returns: An argument containing a lambda expression that converts a native handle to an enum value.
		ArgumentSyntax EnumLambda ()
		{
			var lambdaVariable = "ptr";
			// using var num = Runtime.GetNSObject<NSNumber> (ptr)!);
			var declaration = Using (
				VariableInitialization ("num",
					GetNSObject (NSNumber, [Argument (IdentifierName (lambdaVariable))], true)
					)
				);
			// return statement
			var returnVariable = ReturnStatement (
				CastExpression (
					propertyType.ToArrayElementType ().GetIdentifierSyntax (),
					MemberAccessExpression (
						SyntaxKind.SimpleMemberAccessExpression,
						IdentifierName ("num"),
						IdentifierName (GetNSNumberValue (propertyType.ArrayElementEnumUnderlyingType!.Value))).WithLeadingTrivia (Space)
					).WithLeadingTrivia (Space)
				);

			return Argument (ParenthesizedLambdaExpression ()
				.WithParameterList (
					ParameterList (
						SingletonSeparatedList (
							Parameter (Identifier (lambdaVariable))))).NormalizeWhitespace ()
				.WithBlock (Block (
					declaration.WithLeadingTrivia (Space),
					returnVariable.WithLeadingTrivia (Space).WithTrailingTrivia (Space)
					).WithLeadingTrivia (Space)
				));
		}

		// Creates a dictionary index access expression for wrapped types.
		// Returns: An expression that accesses a dictionary value by key and casts it to the property type.
		ExpressionSyntax DictionaryIndex ()
		{
			return BinaryExpression (
				SyntaxKind.AsExpression,
				ElementAccessExpression (IdentifierName ("Dictionary").WithTrailingTrivia (Space))
					.WithArgumentList (
						BracketedArgumentList (
							SingletonSeparatedList (
								Argument (IdentifierName (dictionaryKey))
								))).WithTrailingTrivia (Space),
				propertyTypeSyntax.WithLeadingTrivia (Space));
		}
	}
}
