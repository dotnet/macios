// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;
using Parameter = Microsoft.Macios.Generator.DataModel.Parameter;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {
	readonly static string objc_msgSend = "objc_msgSend";
	readonly static string objc_msgSendSuper = "objc_msgSendSuper";

	/// <summary>
	/// Returns the expression needed to cast a parameter to its native type.
	/// </summary>
	/// <param name="parameter">The parameter whose casting we need to generate. The type info has to be
	/// and enum and be marked as native. If it is not, the method returns null</param>
	/// <returns>The cast C# expression.</returns>
	internal static CastExpressionSyntax? CastEnumToNative (in Parameter parameter)
		=> CastEnumToNative (parameter.Name, parameter.Type);

	/// <summary>
	/// Returns the expression needed to cast a parameter to its native type.
	/// </summary>
	/// <param name="parameter">The parameter whose casting we need to generate. The type info has to be
	/// and enum and be marked as native. If it is not, the method returns null</param>
	/// <returns>The cast C# expression.</returns>
	internal static CastExpressionSyntax? CastEnumToNative (in DelegateParameter parameter)
		=> CastEnumToNative (parameter.Name, parameter.Type);

	/// <summary>
	/// Returns the expression needed to cast a varuable to its native type.
	/// </summary>
	/// <param name="variableName">The variable whose casting we need to generate.</param>
	/// <param name="typeInfo">The type information of the variable.  The type info has to be
	/// and enum and be marked as native. If it is not, the method returns null</param>
	/// <returns>The cast C# expression.</returns>
	internal static CastExpressionSyntax? CastEnumToNative (string variableName, in TypeInfo typeInfo)
	{
		// not an enum and not a native value. we cannot calculate the casting expression.
		if (!typeInfo.IsEnum || !typeInfo.IsNativeEnum)
			return null;

		// build a casting expression based on the marshall type of the typeinfo
		var marshalType = typeInfo.ToMarshallType ();
		if (marshalType is null)
			// cannot calculate the marshal, return null
			return null;

		var enumBackingValue = typeInfo.EnumUnderlyingType.Value.GetKeyword ();
		var castExpression = CastExpression (IdentifierName (marshalType), // (IntPtr/UIntPtr) cast
			CastExpression (
					IdentifierName (enumBackingValue),
					IdentifierName (variableName)
						.WithLeadingTrivia (Space))
				.WithLeadingTrivia (Space)); // (backingfield) (variable) cast
		return castExpression;
	}

	/// <summary>
	/// Returns the expression needed to cast a native representation of an enum back to its enum type.
	/// </summary>
	/// <param name="parameter">The parameter whose casting we need to generate. The type info has to be
	/// an enum and be marked as native. If it is not, the method returns null.</param>
	/// <returns>The cast C# expression, or null if the parameter is not a native enum.</returns>
	internal static CastExpressionSyntax? CastNativeToEnum (in Parameter parameter)
		=> CastNativeToEnum (IdentifierName (parameter.Name), parameter.Type);

	/// <summary>
	/// Returns the expression needed to cast a native representation of an enum back to its enum type.
	/// </summary>
	/// <param name="parameter">The parameter whose casting we need to generate. The type info has to be
	/// an enum and be marked as native. If it is not, the method returns null.</param>
	/// <returns>The cast C# expression, or null if the parameter is not a native enum.</returns>
	internal static CastExpressionSyntax? CastNativeToEnum (in DelegateParameter parameter)
		=> CastNativeToEnum (IdentifierName (parameter.Name), parameter.Type);

	/// <summary>
	/// Returns the expression needed to cast a native representation of an enum back to its enum type.
	/// </summary>
	/// <param name="variableName">The name of the variable holding the native enum value.</param>
	/// <param name="typeInfo">The type information of the enum. The type info has to be
	/// an enum and be marked as native. If it is not, the method returns null.</param>
	/// <returns>The cast C# expression, or null if the typeInfo is not a native enum.</returns>
	internal static CastExpressionSyntax? CastNativeToEnum (ExpressionSyntax variableName, in TypeInfo typeInfo)
	{
		// not an enum and not a native value. we cannot calculate the casting expression.
		if (!typeInfo.IsEnum || !typeInfo.IsNativeEnum)
			return null;

		var enumBackingValue = typeInfo.EnumUnderlyingType.Value.GetKeyword ();
		var castExpression = CastExpression (typeInfo.GetIdentifierSyntax (), // (IntPtr/UIntPtr) cast
			CastExpression (
					IdentifierName (enumBackingValue),
					variableName
						.WithLeadingTrivia (Space))
				.WithLeadingTrivia (Space)); // (backingfield) (variable) cast
		return castExpression;
	}

	/// <summary>
	/// Returns the expression needed to cast an enum parameter to its primitive type to be used in marshaling.
	/// </summary>
	/// <param name="parameter">The parameter for which we need to generate the casting. The type info has to be
	/// an enumerator. If it is not, the method returns null.</param>
	/// <returns>The cast C# expression.</returns>
	internal static CastExpressionSyntax? CastToPrimitive (in Parameter parameter)
	{
		if (!parameter.Type.IsEnum) {
			return null;
		}

		if (parameter.Type.IsNativeEnum) {
			// return the native casting
			return CastEnumToNative (parameter);
		}

		// returns the enum primitive to be used
		var marshalType = parameter.Type.ToMarshallType ();
		if (marshalType is null)
			return null;

		// (byte) parameter
		var castExpression = CastExpression (
			type: IdentifierName (marshalType),
			expression: IdentifierName (parameter.Name).WithLeadingTrivia (Space));
		return castExpression;
	}

	/// <summary>
	/// Returns the expression needed to cast a bool to a byte to be used in a native call. 
	/// </summary>
	/// <param name="parameter">The parameter to cast.</param>
	/// <returns>A conditional expression that casts a bool to a byte.</returns>
	internal static ConditionalExpressionSyntax? CastToByte (in Parameter parameter)
		=> CastToByte (parameter.Name, parameter.Type);

	/// <summary>
	/// Returns the expression needed to cast a bool to a byte to be used in a native call. 
	/// </summary>
	/// <param name="variableName">The variable to cast.</param>
	/// <param name="typeInfo">The type information of the variable.</param>
	/// <returns>A conditional expression that casts a bool to a byte.</returns>
	internal static ConditionalExpressionSyntax? CastToByte (string variableName, in TypeInfo typeInfo)
	{
		if (typeInfo.SpecialType != SpecialType.System_Boolean)
			return null;
		// (byte) 1
		var castOne = CastExpression (
			PredefinedType (Token (SyntaxKind.ByteKeyword)),
			LiteralExpression (SyntaxKind.NumericLiteralExpression, Literal (1)).WithLeadingTrivia (Space)
				.WithTrailingTrivia (Space)
		);
		// (byte) 0
		var castZero = CastExpression (
			PredefinedType (Token (SyntaxKind.ByteKeyword)),
			LiteralExpression (SyntaxKind.NumericLiteralExpression, Literal (0)).WithLeadingTrivia (Space)
		).WithLeadingTrivia (Space);

		// with this exact space count
		// foo ? (byte) 1 : (byte) 0
		return ConditionalExpression (
			condition: IdentifierName (variableName).WithTrailingTrivia (Space),
			whenTrue: castOne.WithLeadingTrivia (Space),
			whenFalse: castZero);
	}

	/// <summary>
	/// Return the expression needed to cast an invocation that returns a byte to a bool.
	/// </summary>
	/// <param name="expression">The byte returning invocation expression.</param>
	/// <returns>The expression need to cast the invocation to a byte.</returns>
	internal static BinaryExpressionSyntax CastToBool (ExpressionSyntax expression)
	{
		// generates: invocation != 0
		return BinaryExpression (
			SyntaxKind.NotEqualsExpression,
			expression.WithTrailingTrivia (Space),
			LiteralExpression (SyntaxKind.NumericLiteralExpression, Literal (0)).WithLeadingTrivia (Space));
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary NSArray.
	/// This is used when a C# array needs to be passed to an Objective-C method expecting an NSArray.
	/// The method handles nullable arrays by assigning null to the auxiliary variable if the input array is null.
	/// </summary>
	/// <param name="parameterName">The name of the C# array parameter.</param>
	/// <param name="parameterType">The <see cref="TypeInfo"/> of the C# array parameter.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary NSArray variable, or null if the input is not an array or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetNSArrayAuxVariable (string parameterName,
		in TypeInfo parameterType)
	{
		if (!parameterType.IsArray)
			return null;
		var nsArrayFactoryMethod = parameterType.ArrayElementType switch {
			SpecialType.System_String => "FromStrings",
			_ => "FromNSObjects" // the general assumption is that we are working with nsobjects unless we have a bind form
		};
		// syntax that calls the NSArray factory method using the parameter: NSArray.FromNSObjects (targetTensors);
		var factoryInvocation = InvocationExpression (MemberAccessExpression (SyntaxKind.SimpleMemberAccessExpression,
				NSArray, IdentifierName (nsArrayFactoryMethod).WithTrailingTrivia (Space)))
			.WithArgumentList (
				ArgumentList (SingletonSeparatedList (
					Argument (IdentifierName (parameterName)))));

		// variable name
		var variableName = Nomenclator.GetNameForVariableType (parameterName, Nomenclator.VariableType.NSArray);
		if (variableName is null)
			return null;
		var declarator = VariableDeclarator (Identifier (variableName));
		// we have the basic constructs, now depending on if the variable is nullable or not, we need to write the initializer 	
		if (parameterType.IsNullable) {
			// writes the param ? null : NSArray expression
			var nullCheck = ConditionalExpression (
				IsPatternExpression (
					IdentifierName (parameterName).WithLeadingTrivia (Space).WithTrailingTrivia (Space),
					ConstantPattern (LiteralExpression (SyntaxKind.NullLiteralExpression).WithLeadingTrivia (Space)
						.WithTrailingTrivia (Space))),
				LiteralExpression (
					SyntaxKind.NullLiteralExpression).WithLeadingTrivia (Space).WithTrailingTrivia (Space),
				factoryInvocation.WithLeadingTrivia (Space));

			// translates to = x ? null : NSArray.FromNSObject (parameterName), notice we added the '=' here
			declarator = declarator.WithInitializer (EqualsValueClause (nullCheck).WithLeadingTrivia (Space));
		} else {
			// translates to = NSArray.FromNSObject (parameterName);
			declarator = declarator.WithInitializer (EqualsValueClause (factoryInvocation.WithLeadingTrivia (Space))
				.WithLeadingTrivia (Space));
		}

		// complicated way to write 'var auxVariableName = '
		var variableDeclaration = VariableDeclaration (IdentifierName (
				Identifier (TriviaList (), SyntaxKind.VarKeyword, "var", "var", TriviaList ())))
			.WithTrailingTrivia (Space)
			.WithVariables (SingletonSeparatedList (declarator));
		// add using if requested
		return LocalDeclarationStatement (variableDeclaration);
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary NSArray.
	/// This is a convenience overload for <see cref="GetNSArrayAuxVariable(string, in TypeInfo)"/>.
	/// </summary>
	/// <param name="argumentInfo">The <see cref="ArgumentInfo"/> representing the C# array.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary NSArray variable, or null if the input is not an array or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetNSArrayAuxVariable (in ArgumentInfo argumentInfo)
		=> GetNSArrayAuxVariable (argumentInfo.Name, argumentInfo.Type);

	/// <summary>
	/// Generates a local variable declaration for an auxiliary handle (IntPtr).
	/// This is used when a C# object (NSObject or INativeObject) needs to be passed as its handle to an Objective-C method.
	/// The method handles nullable objects by calling `GetNonNullHandle` if the object is nullable, otherwise `GetHandle`.
	/// </summary>
	/// <param name="parameterName">The name of the C# object parameter.</param>
	/// <param name="parameterType">The <see cref="TypeInfo"/> of the C# object parameter. Must be an NSObject or INativeObject.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary handle variable, or null if the input is not an NSObject or INativeObject, or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetHandleAuxVariable (string parameterName, in TypeInfo parameterType)
	{
		if (!parameterType.IsNSObject && !parameterType.IsINativeObject)
			return null;

		var variableName = Nomenclator.GetNameForVariableType (parameterName, Nomenclator.VariableType.Handle);
		if (variableName is null)
			return null;
		// decide about the factory based on the need of a null check 
		ExpressionSyntax factoryInvocation;
		if (parameterType.IsNullable) {
			// generates: zone?.GetHandle ();
			factoryInvocation = ConditionalAccessExpression (
					IdentifierName (parameterName),
					InvocationExpression (
						MemberBindingExpression (
							IdentifierName ("GetHandle").WithTrailingTrivia (Space))));
		} else {
			// generates: zone!.GetNonNullHandle (nameof (zone));
			factoryInvocation = InvocationExpression (
					MemberAccessExpression (SyntaxKind.SimpleMemberAccessExpression,
						PostfixUnaryExpression (
							SyntaxKind.SuppressNullableWarningExpression,
							IdentifierName (parameterName)),
						IdentifierName ("GetNonNullHandle").WithTrailingTrivia (Space)))
				.WithArgumentList (ArgumentList (
					SingletonSeparatedList (Argument (NameOf (parameterName)))));
		}

		// generates: variable = {FactoryCall}
		var declarator = VariableDeclarator (Identifier (variableName))
			.WithInitializer (EqualsValueClause (factoryInvocation.WithLeadingTrivia (Space))
				.WithLeadingTrivia (Space));
		// generates the final statement: 
		// var x = zone.GetHandle ();
		// or 
		// var x = zone!.GetNonNullHandle (nameof (constantValues));
		return LocalDeclarationStatement (
			VariableDeclaration (
				IdentifierName (
					Identifier (
						TriviaList (),
						SyntaxKind.VarKeyword,
						"var",
						"var",
						TriviaList ()))).WithVariables (
				SingletonSeparatedList (declarator.WithLeadingTrivia (Space))
			));
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary handle (IntPtr).
	/// This is a convenience overload for <see cref="GetHandleAuxVariable(string, in TypeInfo)"/>.
	/// </summary>
	/// <param name="argumentInfo">The <see cref="ArgumentInfo"/> representing the C# object.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary handle variable, or null if the input is not an NSObject or INativeObject, or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetHandleAuxVariable (in ArgumentInfo argumentInfo)
		=> GetHandleAuxVariable (argumentInfo.Name, argumentInfo.Type);

	/// <summary>
	/// Generates a local variable declaration for an auxiliary handle (IntPtr) initialized to IntPtr.Zero.
	/// This is typically used to declare a default handle variable before assigning it a valid native handle.
	/// </summary>
	/// <param name="variableName">The name of the handle variable to declare.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> representing the declaration of the handle variable initialized to IntPtr.Zero.</returns>
	internal static LocalDeclarationStatementSyntax? GetHandleDefaultVariable (string variableName)
	{
		// generates: var handle = IntPtr.Zero;
		var declarator = VariableDeclarator (Identifier (variableName))
			.WithInitializer (EqualsValueClause (
					MemberAccessExpression (
						SyntaxKind.SimpleMemberAccessExpression,
						IntPtr,
						IdentifierName ("Zero")))
				.WithLeadingTrivia (Space).WithTrailingTrivia (Space));

		var variableDeclaration = VariableDeclaration (NativeHandle)
			.WithVariables (SingletonSeparatedList (declarator));
		return LocalDeclarationStatement (variableDeclaration).NormalizeWhitespace ();
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary NSString.
	/// This is used when a C# string needs to be passed to an Objective-C method expecting an NSString.
	/// The method uses <c>CFString.CreateNative</c> to create the native string.
	/// </summary>
	/// <param name="parameterName">The name of the C# string parameter.</param>
	/// <param name="parameterType">The <see cref="TypeInfo"/> of the C# string parameter. Must be a string type.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary NSString variable, or null if the input is not a string or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetStringAuxVariable (string parameterName,
		in TypeInfo parameterType)
	{
		if (parameterType.Name != "string")
			return null;

		var variableName = Nomenclator.GetNameForVariableType (parameterName, Nomenclator.VariableType.NSString);
		if (variableName is null)
			return null;

		// generates: CFString.CreateNative ({parameter.Name});
		var cfstringFactoryInvocation = StringCreateNative ([
			Argument (IdentifierName (parameterName)),
		]);

		// generates {var} = CFString.CreateNative ({parameter.Name});
		var declarator =
			VariableDeclarator (Identifier (variableName).WithLeadingTrivia (Space).WithTrailingTrivia (Space))
				.WithInitializer (EqualsValueClause (cfstringFactoryInvocation.WithLeadingTrivia (Space)));


		// put everythign together
		var declaration = VariableDeclaration (IdentifierName (Identifier (
				TriviaList (), SyntaxKind.VarKeyword, "var", "var", TriviaList ())))
			.WithVariables (SingletonSeparatedList (declarator));

		return LocalDeclarationStatement (declaration);
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary NSString.
	/// This is a convenience overload for <see cref="GetStringAuxVariable(string, in TypeInfo)"/>.
	/// </summary>
	/// <param name="argumentInfo">The <see cref="ArgumentInfo"/> representing the C# string.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary NSString variable, or null if the input is not a string or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetStringAuxVariable (in ArgumentInfo argumentInfo)
		=> GetStringAuxVariable (argumentInfo.Name, argumentInfo.Type);

	internal static LocalDeclarationStatementSyntax? GetNSNumberAuxVariable (in Parameter parameter)
	{
		// the BindFrom attribute with a nsnumber supports the following types:
		// - bool
		// - byte
		// - double
		// - float
		// - short
		// - int
		// - long
		// - sbyte
		// - ushort
		// - uint
		// - ulong
		// - nfloat
		// - nint
		// - nuint
		// - Enums: this are simply casted to their backing representation
		// if we do not match the expected type, return null

		// make sure that the parameter type is valid and return the required method for the nsnumber variable
#pragma warning disable format
		var factoryMethod = parameter.Type switch {
			{ Name: "nint" } => "FromNInt",
			{ Name: "nuint" } => "FromNUInt",
			{ Name: "nfloat" or "NFloat" } => "FromNFloat",
			{ IsEnum: true, IsSmartEnum: true } => null, // we do not support smart enums, there is a special case for them 
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_SByte } => "FromSByte",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Byte } => "FromByte",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Int16 } => "FromInt16",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UInt16 } => "FromUInt16",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Int32 } => "FromInt32",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UInt32 } => "FromUInt32",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Int64 } => "FromInt64",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UInt64 } => "FromUInt64",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_IntPtr } => "FromNint",
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UIntPtr } => "FromNUint",
			{ SpecialType: SpecialType.System_Boolean } => "FromBoolean",
			{ SpecialType: SpecialType.System_Byte } => "FromByte",
			{ SpecialType: SpecialType.System_Double } => "FromDouble",
			{ SpecialType: SpecialType.System_Single } => "FromFloat",
			{ SpecialType: SpecialType.System_Int16 } => "FromInt16",
			{ SpecialType: SpecialType.System_Int32 } => "FromInt32",
			{ SpecialType: SpecialType.System_Int64 } => "FromInt64",
			{ SpecialType: SpecialType.System_SByte } => "FromSByte",
			{ SpecialType: SpecialType.System_UInt16 } => "FromUInt16",
			{ SpecialType: SpecialType.System_UInt32 } => "FromUInt32",
			{ SpecialType: SpecialType.System_UInt64 } => "FromUInt64",
			{ SpecialType: SpecialType.System_IntPtr } => "FromNint",
			{ SpecialType: SpecialType.System_UIntPtr } => "FromNUint",
			_ => null,
		};
#pragma warning restore format

		if (factoryMethod is null)
			return null;

		var variableName = Nomenclator.GetNameForVariableType (parameter.Name, Nomenclator.VariableType.BindFrom);
		if (variableName is null)
			return null;

		// generates: NSNumber.FromDouble
		var factoryInvocation = InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				NSNumber,
				IdentifierName (factoryMethod).WithTrailingTrivia (Space))
		);

		// the arguments of the factory information depends on if we are dealing with a enum, in which case we cast
		// or not, in which case we just add the arguments
		if (parameter.Type.IsEnum) {
			// generates: NSNumber.FromDouble ((int)value);
			factoryInvocation = factoryInvocation
				.WithArgumentList (ArgumentList (SingletonSeparatedList (Argument (
					CastExpression (
						IdentifierName (parameter.Type.EnumUnderlyingType.GetKeyword () ?? ""),
						IdentifierName (parameter.Name).WithLeadingTrivia (Space))))));
		} else {
			// generates: NSNumber.FromDouble (value);
			factoryInvocation = factoryInvocation
				.WithArgumentList (ArgumentList (SingletonSeparatedList (
					Argument (IdentifierName (parameter.Name)))));
		}

		var declarator =
			VariableDeclarator (Identifier (variableName).WithLeadingTrivia (Space).WithTrailingTrivia (Space))
				.WithInitializer (EqualsValueClause (factoryInvocation.WithLeadingTrivia (Space)));

		// generats: var nba_variable = NSNumber.FromDouble(value);
		var declaration = VariableDeclaration (IdentifierName (Identifier (
				TriviaList (), SyntaxKind.VarKeyword, "var", "var", TriviaList ())))
			.WithVariables (SingletonSeparatedList (declarator));

		return LocalDeclarationStatement (declaration);
	}

	internal static LocalDeclarationStatementSyntax? GetNSValueAuxVariable (in Parameter parameter)
	{

		// the BindFrom attribute with a nsvalue supports the following types:
		// - CGAffineTransform
		// - NSRange
		// - CGVector
		// - SCNMatrix4
		// - CLLocationCoordinate2D
		// - SCNVector3
		// - SCNVector4
		// - CGPoint / PointF
		// - CGRect / RectangleF
		// - CGSize / SizeF
		// - UIEdgeInsets
		// - UIOffset
		// - MKCoordinateSpan
		// - CMTimeRange
		// - CMTime
		// - CMTimeMapping
		// - CATransform3D
		
#pragma warning disable format
		// get the factory method based on the parameter type, if it is not found, return null
		var factoryMethod = parameter.Type switch { 
			{ FullyQualifiedName: "CoreGraphics.CGAffineTransform" } => "FromCGAffineTransform", 
			{ FullyQualifiedName: "Foundation.NSRange" } => "FromRange", 
			{ FullyQualifiedName: "CoreGraphics.CGVector" } => "FromCGVector", 
			{ FullyQualifiedName: "SceneKit.SCNMatrix4" } => "FromSCNMatrix4", 
			{ FullyQualifiedName: "CoreLocation.CLLocationCoordinate2D" } => "FromMKCoordinate", 
			{ FullyQualifiedName: "SceneKit.SCNVector3" } => "FromVector", 
			{ FullyQualifiedName: "SceneKit.SCNVector4" } => "FromVector", 
			{ FullyQualifiedName: "CoreGraphics.CGPoint" } => "FromCGPoint", 
			{ FullyQualifiedName: "CoreGraphics.CGRect" } => "FromCGRect", 
			{ FullyQualifiedName: "CoreGraphics.CGSize" } => "FromCGSize", 
			{ FullyQualifiedName: "UIKit.UIEdgeInsets" } => "FromUIEdgeInsets", 
			{ FullyQualifiedName: "UIKit.UIOffset" } => "FromUIOffset", 
			{ FullyQualifiedName: "MapKit.MKCoordinateSpan" } => "FromMKCoordinateSpan", 
			{ FullyQualifiedName: "CoreMedia.CMTimeRange" } => "FromCMTimeRange", 
			{ FullyQualifiedName: "CoreMedia.CMTime" } => "FromCMTime", 
			{ FullyQualifiedName: "CoreMedia.CMTimeMapping" } => "FromCMTimeMapping", 
			{ FullyQualifiedName: "CoreAnimation.CATransform3D" } => "FromCATransform3D",
			_ => null,
		};
#pragma warning restore format

		if (factoryMethod is null)
			return null;

		var variableName = Nomenclator.GetNameForVariableType (parameter.Name, Nomenclator.VariableType.BindFrom);
		if (variableName is null)
			return null;

		// generates: NSValue.FromCMTime 
		var factoryInvocation = InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				NSValue,
				IdentifierName (factoryMethod).WithTrailingTrivia (Space))
		).WithArgumentList (ArgumentList (SingletonSeparatedList (
			Argument (IdentifierName (parameter.Name)))));

		var declarator =
			VariableDeclarator (Identifier (variableName).WithLeadingTrivia (Space).WithTrailingTrivia (Space))
				.WithInitializer (EqualsValueClause (factoryInvocation.WithLeadingTrivia (Space)));

		// generats: var nba_variable = NSNumber.FromDouble(value);
		var declaration = VariableDeclaration (IdentifierName (Identifier (
				TriviaList (), SyntaxKind.VarKeyword, "var", "var", TriviaList ())))
			.WithVariables (SingletonSeparatedList (declarator));

		return LocalDeclarationStatement (declaration);
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary NSString for a smart enum.
	/// This is used when a C# smart enum needs to be passed to an Objective-C method expecting an NSString constant.
	/// The method calls the `GetConstant()` extension method on the smart enum.
	/// </summary>
	/// <param name="parameterName">The name of the C# smart enum parameter.</param>
	/// <param name="parameterType">The <see cref="TypeInfo"/> of the C# smart enum parameter.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary NSString variable, or null if the input is not a smart enum or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetNSStringSmartEnumAuxVariable (string parameterName,
		in TypeInfo parameterType)
	{
		if (!parameterType.IsSmartEnum)
			return null;

		var variableName = Nomenclator.GetNameForVariableType (parameterName, Nomenclator.VariableType.BindFrom);
		if (variableName is null)
			return null;

		// smart enums are very simple to do, we need to call the GetConstant that was generated as an extension
		// method
		var factoryInvocation = InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				IdentifierName (parameterName),
				IdentifierName ("GetConstant").WithTrailingTrivia (Space))
		);

		var declarator =
			VariableDeclarator (Identifier (variableName).WithLeadingTrivia (Space).WithTrailingTrivia (Space))
				.WithInitializer (EqualsValueClause (factoryInvocation.WithLeadingTrivia (Space)));

		// generats: var nba_variable = NSNumber.FromDouble(value);
		var declaration = VariableDeclaration (IdentifierName (Identifier (
				TriviaList (), SyntaxKind.VarKeyword, "var", "var", TriviaList ())))
			.WithVariables (SingletonSeparatedList (declarator));

		return LocalDeclarationStatement (declaration);
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary NSString for a smart enum.
	/// This is a convenience overload for <see cref="GetNSStringSmartEnumAuxVariable(string, in TypeInfo)"/>.
	/// </summary>
	/// <param name="argumentInfo">The <see cref="ArgumentInfo"/> representing the C# smart enum.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary NSString variable, or null if the input is not a smart enum or if a variable name cannot be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetNSStringSmartEnumAuxVariable (in ArgumentInfo argumentInfo)
		=> GetNSStringSmartEnumAuxVariable (argumentInfo.Name, argumentInfo.Type);

	/// <summary>
	/// Generates an invocation expression to convert an NSString (represented by an argument syntax) back to its corresponding smart enum value.
	/// This method relies on a generated extension class for the smart enum that provides a `GetValue(string)` method.
	/// </summary>
	/// <param name="typeInfo">The <see cref="TypeInfo"/> of the target smart enum. Must be a smart enum type.</param>
	/// <param name="argument">The <see cref="ArgumentSyntax"/> representing the NSString value.</param>
	/// <returns>An <see cref="InvocationExpressionSyntax"/> that calls the `GetValue` extension method to perform the conversion.</returns>
	internal static InvocationExpressionSyntax GetSmartEnumFromNSString (in TypeInfo typeInfo, ArgumentSyntax argument)
	{
		var extensionClass = Nomenclator.GetSmartEnumExtensionClassName (typeInfo.GetIdentifierSyntax ().ToString ());
		// generates: SmartEnum.GetValue (variableName);
		return InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				IdentifierName (extensionClass),
				IdentifierName ("GetValue").WithTrailingTrivia (Space)))
			.WithArgumentList (ArgumentList (SingletonSeparatedList (argument)));
	}

	internal static LocalDeclarationStatementSyntax? GetNSArrayBindFromAuxVariable (in Parameter parameter)
	{
		// we can only work with parameters that are an array
		if (!parameter.Type.IsArray)
			return null;

		var variableName = Nomenclator.GetNameForVariableType (parameter.Name, Nomenclator.VariableType.BindFrom);
		if (variableName is null)
			return null;

		// use a switch to decide which of the constructors we are going to use to build the array.
		var lambdaFunctionVariable = "obj";
		var nsNumberExpr = ObjectCreationExpression (
				NSNumber.WithLeadingTrivia (Space).WithTrailingTrivia (Space))
			.WithArgumentList (ArgumentList (SingletonSeparatedList (
				Argument (IdentifierName (lambdaFunctionVariable)))));
		var nsValueExpr = ObjectCreationExpression (
				NSValue.WithLeadingTrivia (Space).WithTrailingTrivia (Space))
			.WithArgumentList (ArgumentList (SingletonSeparatedList (
				Argument (IdentifierName (lambdaFunctionVariable)))));
		var smartEnumExpr = InvocationExpression (MemberAccessExpression (
			SyntaxKind.SimpleMemberAccessExpression,
			IdentifierName (lambdaFunctionVariable).WithLeadingTrivia (Space),
			IdentifierName ("GetConstant")));
			
#pragma warning disable format
		ExpressionSyntax? constructor = parameter.Type switch {
			// smart enums
			{ IsEnum: true, IsSmartEnum: true } => smartEnumExpr,
			// NSNumber types:
			{ Name: "nint" } => nsNumberExpr,
			{ Name: "nuint" } => nsNumberExpr,
			{ Name: "nfloat" or "NFloat" } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_SByte } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Byte } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Int16 } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UInt16 } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Int32 } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UInt32 } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_Int64 } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UInt64 } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_IntPtr } => nsNumberExpr,
			{ IsEnum: true, EnumUnderlyingType: SpecialType.System_UIntPtr } => nsNumberExpr,
			{ SpecialType: SpecialType.System_Boolean } => nsNumberExpr,
			{ SpecialType: SpecialType.System_Byte } => nsNumberExpr,
			{ SpecialType: SpecialType.System_Double } => nsNumberExpr,
			{ SpecialType: SpecialType.System_Single } => nsNumberExpr,
			{ SpecialType: SpecialType.System_Int16 } => nsNumberExpr,
			{ SpecialType: SpecialType.System_Int32 } => nsNumberExpr,
			{ SpecialType: SpecialType.System_Int64 } => nsNumberExpr,
			{ SpecialType: SpecialType.System_SByte } => nsNumberExpr,
			{ SpecialType: SpecialType.System_UInt16 } => nsNumberExpr,
			{ SpecialType: SpecialType.System_UInt32 } => nsNumberExpr,
			{ SpecialType: SpecialType.System_UInt64 } => nsNumberExpr,
			{ SpecialType: SpecialType.System_IntPtr } => nsNumberExpr,
			{ SpecialType: SpecialType.System_UIntPtr } => nsNumberExpr,
			// NSValue related types:
			{ FullyQualifiedName: "CoreGraphics.CGAffineTransform" } => nsValueExpr, 
			{ FullyQualifiedName: "Foundation.NSRange" } =>  nsValueExpr, 
			{ FullyQualifiedName: "CoreGraphics.CGVector" } =>  nsValueExpr, 
			{ FullyQualifiedName: "SceneKit.SCNMatrix4" } => nsValueExpr, 
			{ FullyQualifiedName: "CoreLocation.CLLocationCoordinate2D" } => nsValueExpr, 
			{ FullyQualifiedName: "SceneKit.SCNVector3" } => nsValueExpr,
			{ FullyQualifiedName: "SceneKit.SCNVector4" } => nsValueExpr,
			{ FullyQualifiedName: "CoreGraphics.CGPoint" } => nsValueExpr,
			{ FullyQualifiedName: "CoreGraphics.CGRect" } => nsValueExpr,
			{ FullyQualifiedName: "CoreGraphics.CGSize" } => nsValueExpr,
			{ FullyQualifiedName: "UIKit.UIEdgeInsets" } => nsValueExpr,
			{ FullyQualifiedName: "UIKit.UIOffset" } => nsValueExpr,
			{ FullyQualifiedName: "MapKit.MKCoordinateSpan" } => nsNumberExpr,
			{ FullyQualifiedName: "CoreMedia.CMTimeRange" } => nsNumberExpr,
			{ FullyQualifiedName: "CoreMedia.CMTime" } => nsValueExpr, 
			{ FullyQualifiedName: "CoreMedia.CMTimeMapping" } => nsValueExpr, 
			{ FullyQualifiedName: "CoreAnimation.CATransform3D" } => nsValueExpr, 
			_ => null
		};
#pragma warning restore format
		
		if (constructor is null)
			return null;
		// generates:
		// obj => new NSNumber (obj);
		// obj => new NSValue (obj);
		// obj => obj.GetValue ();
		var lambdaExpression = SimpleLambdaExpression (Parameter (Identifier (lambdaFunctionVariable).WithTrailingTrivia (Space)))
			.WithExpressionBody (constructor.WithLeadingTrivia (Space));

		// generate: NSArray.FromNSObjects (o => new NSNumber (o), shape);
		var factoryInvocation = NSArrayFromNSObjects ([
			Argument (lambdaExpression),
			Argument (IdentifierName (parameter.Name))
		]);

		var declarator =
			VariableDeclarator (Identifier (variableName).WithLeadingTrivia (Space).WithTrailingTrivia (Space))
				.WithInitializer (EqualsValueClause (factoryInvocation.WithLeadingTrivia (Space)));

		var declaration = VariableDeclaration (IdentifierName (Identifier (
				TriviaList (), SyntaxKind.VarKeyword, "var", "var", TriviaList ())))
			.WithVariables (SingletonSeparatedList (declarator));

		return LocalDeclarationStatement (declaration);
	}

	/// <summary>
	/// Returns the aux variable declaration needed when a parameter has the BindFrom attribute.
	/// </summary>
	/// <param name="parameter">The parameter whose aux variable we want to declare.</param>
	/// <returns>The syntax declaration of the aux variable or null if it could not be generated.</returns>
	internal static LocalDeclarationStatementSyntax? GetBindFromAuxVariable (in Parameter parameter)
	{
		if (parameter.BindAs is null)
			return null;

#pragma warning disable format
		// based on the bindas type call one of the helper factory methods
		return (Type: parameter.BindAs.Value.Type, IsArray: parameter.Type.IsArray) switch {
			{ IsArray: true } => GetNSArrayBindFromAuxVariable (parameter),
			{ Type.FullyQualifiedName: "Foundation.NSNumber" } => GetNSNumberAuxVariable (parameter),
			{ Type.FullyQualifiedName: "Foundation.NSValue" } => GetNSValueAuxVariable (parameter),
			{ Type.FullyQualifiedName: "Foundation.NSString" } => GetNSStringSmartEnumAuxVariable (parameter),
			_ => null,
		};
#pragma warning restore format
	}

	/// <summary>
	/// Generate the definition for an autorelease pool to wrap a method/property call.
	/// </summary>
	/// <returns>The variable declaration for a auto release pool.</returns>
	internal static LocalDeclarationStatementSyntax GetAutoreleasePoolVariable ()
	{
		const string poolVariableName = "autorelease_pool";
		//  object creation
		var create =
			ObjectCreationExpression (
					IdentifierName ("NSAutoreleasePool").WithLeadingTrivia (Space).WithTrailingTrivia (Space))
				.WithArgumentList (ArgumentList ());

		// return the autorelease pool definition 
		var declarator = VariableDeclarator (Identifier (poolVariableName));
		declarator = declarator.WithInitializer (EqualsValueClause (create.WithLeadingTrivia (Space))
				.WithLeadingTrivia (Space));

		var variableDeclaration = VariableDeclaration (IdentifierName (
				Identifier (TriviaList (), SyntaxKind.VarKeyword, "var", "var", TriviaList ())))
			.WithTrailingTrivia (Space)
			.WithVariables (SingletonSeparatedList (declarator));

		return LocalDeclarationStatement (variableDeclaration);
	}

	/// <summary>
	/// Generate the method that will check the ui thread based on the platform.
	/// </summary>
	/// <param name="platform">The platform targeted in the compilation.</param>
	/// <returns>The correct expression to check if the code is executing in the UI thread.</returns>
	internal static ExpressionStatementSyntax? EnsureUiThread (PlatformName platform)
	{
		(string Namespace, string Classname)? caller = platform switch {
			PlatformName.iOS => ("UIKit", "UIApplication"),
			PlatformName.TvOS => ("UIKit", "UIApplication"),
			PlatformName.MacCatalyst => ("UIKit", "UIApplication"),
			PlatformName.MacOSX => ("AppKit", "NSApplication"),
			_ => null
		};
		if (caller is null)
			return null;

		return ExpressionStatement (InvocationExpression (MemberAccessExpression (
			SyntaxKind.SimpleMemberAccessExpression,
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				IdentifierName (caller.Value.Namespace),
				IdentifierName (caller.Value.Classname)),
			IdentifierName ("EnsureUIThread").WithTrailingTrivia (Space))));
	}

	/// <summary>
	/// Generate the declaration needed for the exception marhsaling.
	/// </summary>
	/// <returns>The local declaration needed for the exception marshaling.</returns>
	internal static LocalDeclarationStatementSyntax GetExceptionHandleAuxVariable ()
	{
		const string handleVariableName = "exception_gchandle";
		const string intPtr = "IntPtr";
		//  object creation
		var zeroPtr = MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				IdentifierName (intPtr),
				IdentifierName ("Zero"));

		var declarator = VariableDeclarator (Identifier (handleVariableName))
				.WithInitializer (EqualsValueClause (zeroPtr));

		return LocalDeclarationStatement (
			VariableDeclaration (IdentifierName (intPtr))
				.WithVariables (SingletonSeparatedList (declarator)))
			.NormalizeWhitespace (); // no special mono style
	}

	internal static (string Name, LocalDeclarationStatementSyntax Declaration) GetReturnValueAuxVariable (in TypeInfo returnType)
	{
		var typeSyntax = returnType.GetIdentifierSyntax ();
		var variableName = Nomenclator.GetReturnVariableName ();
		// generates Type ret; The GetIdentifierSyntax will ensure that the correct type and nullable annotation is used
		var declaration = LocalDeclarationStatement (
			VariableDeclaration (typeSyntax.WithTrailingTrivia (Space))
				.WithVariables (SingletonSeparatedList (VariableDeclarator (Identifier (variableName)))));
		return (Name: variableName, Declaration: declaration);
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary variable that holds a native block created from a nullable C# delegate.
	/// This method is used to handle block parameters that can be null. It generates a call to a static `CreateNullableBlock`
	/// method on a trampoline-specific static bridge class. This helper method is responsible for creating the native block
	/// if the delegate is not null, or returning `IntPtr.Zero` if it is.
	/// </summary>
	/// <param name="variableName">The name of the C# delegate variable.</param>
	/// <param name="blockTypeInfo">The <see cref="TypeInfo"/> of the delegate.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary nullable block variable.</returns>
	internal static LocalDeclarationStatementSyntax GetNullableBlockAuxVariable (string variableName, in TypeInfo blockTypeInfo)
	{
		var staticBridgeClassName =
			Nomenclator.GetTrampolineClassName (blockTypeInfo, Nomenclator.TrampolineClassType.StaticBridgeClass);
		// generates the call to create the nullable block
		var invocation = InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					MemberAccessExpression (
						SyntaxKind.SimpleMemberAccessExpression,
						Trampolines,
						IdentifierName (staticBridgeClassName)),
					IdentifierName ("CreateNullableBlock").WithTrailingTrivia (Space)))
			.WithArgumentList (
				ArgumentList (
					SingletonSeparatedList (
						Argument (IdentifierName (variableName)))));
		// variable declarator 'name = invocation'
		var declarator = VariableDeclarator (
				Identifier (Nomenclator.GetNameForVariableType (variableName, Nomenclator.VariableType.NullableBlock)!).WithTrailingTrivia (Space))
			.WithInitializer (
				EqualsValueClause (invocation.WithLeadingTrivia (Space)));
		// var declaration
		return LocalDeclarationStatement (
			VariableDeclaration (
					IdentifierName (
						Identifier (
							TriviaList (),
							SyntaxKind.VarKeyword,
							"var",
							"var",
							TriviaList (Space))))
				.WithVariables (
					SingletonSeparatedList (declarator)));
	}

	/// <summary>
	/// Generates a local variable declaration for an auxiliary variable that holds a native block created from a nullable C# delegate.
	/// This is a convenience overload for <see cref="GetNullableBlockAuxVariable(string, string, in TypeInfo)"/>.
	/// </summary>
	/// <param name="argumentInfo">The <see cref="ArgumentInfo"/> representing the C# delegate.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the auxiliary nullable block variable.</returns>
	internal static LocalDeclarationStatementSyntax GetNullableBlockAuxVariable (in ArgumentInfo argumentInfo)
		=> GetNullableBlockAuxVariable (argumentInfo.Name, argumentInfo.Type);

	/// <summary>
	/// Generates a local variable declaration for a pointer to a BlockLiteral structure.
	/// This is used in trampolines to manage the lifecycle of a native block.
	/// If the corresponding C# delegate is not null, the variable is initialized with the address of the block literal struct; otherwise, it's initialized to null.
	/// </summary>
	/// <param name="variableName">The name of the C# delegate variable, used to derive the name of the block literal pointer variable.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the block literal pointer variable.</returns>
	internal static LocalDeclarationStatementSyntax GetBlockLiteralAuxVariable (string variableName)
	{
		var blockLiteralPointerName = Nomenclator.GetNameForVariableType (variableName, Nomenclator.VariableType.BlockLiteral);
		var blockVariableName = Nomenclator.GetNameForVariableType (variableName, Nomenclator.VariableType.NullableBlock);
		// generates parameterName is not null ? &blockVariableName : null;
		var conditional = ConditionalExpression (
			IsPatternExpression (
				IdentifierName (variableName),
				UnaryPattern (
					ConstantPattern (
						LiteralExpression (
							SyntaxKind.NullLiteralExpression)))),
			PrefixUnaryExpression (
				SyntaxKind.AddressOfExpression,
				IdentifierName (blockVariableName!)),
			LiteralExpression (
				SyntaxKind.NullLiteralExpression));

		return LocalDeclarationStatement (
			VariableDeclaration (PointerType (BlockLiteral))
				.WithVariables (
					SingletonSeparatedList (
						VariableDeclarator (Identifier (blockLiteralPointerName!))
							.WithInitializer (EqualsValueClause (conditional))))).NormalizeWhitespace ();
	}

	/// <summary>
	/// Generates a local variable declaration for a pointer to a BlockLiteral structure.
	/// This is a convenience overload for <see cref="GetBlockLiteralAuxVariable(string)"/>.
	/// </summary>
	/// <param name="argumentInfo">The <see cref="ArgumentInfo"/> representing the C# delegate.</param>
	/// <returns>A <see cref="LocalDeclarationStatementSyntax"/> for the block literal pointer variable.</returns>
	internal static LocalDeclarationStatementSyntax GetBlockLiteralAuxVariable (in ArgumentInfo argumentInfo)
		=> GetBlockLiteralAuxVariable (argumentInfo.Name);

	/// <summary>
	/// Returns the declaration needed for the string field of a given selector.
	/// </summary>
	/// <param name="selector">The selector to be store in the field.</param>
	/// <param name="selectorName">The selector handle name. This will be used to calculate the field name.</param>
	/// <returns>The declaration statement for the variable.</returns>
	internal static LocalDeclarationStatementSyntax GetSelectorStringField (string selector, string selectorName)
	{
		var fieldName = selectorName.Substring (0, selectorName.Length - 6 /* Handle */);
		var variableDeclaration =
			VariableDeclaration (PredefinedType (Token (SyntaxKind.StringKeyword)))
				.WithVariables (
					SingletonSeparatedList (
						VariableDeclarator (Identifier (fieldName))
							.WithInitializer (EqualsValueClause (
								LiteralExpression (SyntaxKind.StringLiteralExpression, Literal (selector))))));
		return LocalDeclarationStatement (variableDeclaration)
			.WithModifiers (TokenList (Token (SyntaxKind.ConstKeyword))).NormalizeWhitespace ();
	}

	internal static LocalDeclarationStatementSyntax GetSelectorHandleField (string selector, string selectorName)
	{
		// list of the modifiers
		var modifiers = TokenList (
			Token (SyntaxKind.StaticKeyword).WithTrailingTrivia (Space),
			Token (SyntaxKind.ReadOnlyKeyword).WithTrailingTrivia (Space));
		// generates: Selector.GetHandle (selector);
		var getHandleInvocation = InvocationExpression (MemberAccessExpression (SyntaxKind.SimpleMemberAccessExpression,
					Selector, IdentifierName ("GetHandle").WithTrailingTrivia (Space)))
			.WithArgumentList (
				ArgumentList (
					SingletonSeparatedList (
						Argument (
							LiteralExpression (SyntaxKind.StringLiteralExpression, Literal (selector))))));

		// generates: NativeHandler selectorName = Selector.GetHandle (selector);
		return LocalDeclarationStatement (
			VariableDeclaration (NativeHandle.WithTrailingTrivia (Space))
				.WithVariables (
					SingletonSeparatedList (
						VariableDeclarator (Identifier (selectorName).WithTrailingTrivia (Space))
							.WithInitializer (EqualsValueClause (getHandleInvocation.WithLeadingTrivia (Space)))))
		).WithModifiers (modifiers);
	}

	static string? GetObjCMessageSendMethodName<T> (ExportData<T> exportData,
		in TypeInfo returnType, ImmutableArray<Parameter> parameters, bool isSuper = false, bool isStret = false)
		where T : Enum
	{
		var flags = exportData.Flags;
		if (flags is null)
			// flags are not set, should be a bug, but we will return null
			return null;

		// the name of the objcSend method is calculated in the following way	
		// {CustomMarshallPrefix}_{MarshallTypeOfReturnType}_{objcSendMsg}{stret?_stret}_{string.Join('_', MarshallTypeArgs)}{nativeException?_exception}{CustomMarsahllPostfix}
		// we will use a sb to make things easy to follow
		var sb = new StringBuilder ();

		// first, decide if the user created a custom marshalling by checking the flags of the export data
		CustomMarshalDirective? customMarshalDirective = null;
		if (flags.HasCustomMarshalDirective ()) {
			customMarshalDirective = exportData.ToCustomMarshalDirective ();
		}

		if (customMarshalDirective?.NativePrefix is not null) {
			sb.Append (customMarshalDirective.NativePrefix);
		} else if (flags.HasMarshalNativeExceptions ()) {
			sb.Append ("xamarin_");
		}

		// return types do not have a reference kind
		sb.Append (returnType.ToMarshallType ());
		sb.Append ('_');
		// append the msg method based if it is for super or not, do not append '_' intimidatingly, since if we do
		// not have parameters, we are done
		sb.Append (isSuper ? objc_msgSendSuper : objc_msgSend);
		if (isStret) {
			sb.Append ("_stret");
		}

		// loop over params and get their native handler name
		if (parameters.Length > 0) {
			sb.Append ('_');
			sb.AppendJoin ('_', parameters.Select (p => p.Type.ToMarshallType ()));
		}

		// check if we do have a custom marshall exception set for the export

		// check any possible custom postfix naming
		if (customMarshalDirective?.NativeSuffix is not null) {
			sb.Append (customMarshalDirective.NativeSuffix);
		} else if (flags.HasMarshalNativeExceptions ()) {
			sb.Append ("_exception");
		}

		return sb.ToString ();
	}

	internal static string? GetObjCMessageSendMethod (in Method method, bool isSuper = false, bool isStret = false)
		=> GetObjCMessageSendMethodName (method.ExportMethodData, method.ReturnType, method.Parameters, isSuper,
			isStret);

	/// <summary>
	/// Gets the name of the NSValue instance method used to retrieve its underlying value, based on the provided type.
	/// For example, if the type is CGPoint, this returns "CGPointValue".
	/// </summary>
	/// <param name="type">The <see cref="TypeInfo"/> representing the expected underlying type of the NSValue.</param>
	/// <returns>A string representing the name of the NSValue method to call (e.g., "CGPointValue", "CGRectValue"),
	/// or an empty string if the type is not a supported NSValue underlying type.</returns>
	internal static string GetNSValueValue (in TypeInfo type)
	{
		// return the method needed to retrieve the value from the NSValue object based on the type
#pragma warning disable format
		// get the factory method based on the parameter type, if it is not found, return null
		return type switch { 
			{ FullyQualifiedName: "CoreGraphics.CGAffineTransform" } => "CGAffineTransformValue", 
			{ FullyQualifiedName: "Foundation.NSRange" } => "RangeValue", 
			{ FullyQualifiedName: "CoreGraphics.CGVector" } => "CGVectorValue", 
			{ FullyQualifiedName: "SceneKit.SCNMatrix4" } => "SCNMatrix4Value", 
			{ FullyQualifiedName: "CoreLocation.CLLocationCoordinate2D" } => "MKCoordinateValue", 
			{ FullyQualifiedName: "SceneKit.SCNVector3" } => "VectorValue", 
			{ FullyQualifiedName: "SceneKit.SCNVector4" } => "VectorValue", 
			{ FullyQualifiedName: "CoreGraphics.CGPoint" } => "CGPointValue", 
			{ FullyQualifiedName: "CoreGraphics.CGRect" } => "CGRectValue", 
			{ FullyQualifiedName: "CoreGraphics.CGSize" } => "CGSizeValue", 
			{ FullyQualifiedName: "UIKit.UIEdgeInsets" } => "UIEdgeInsetsValue", 
			{ FullyQualifiedName: "UIKit.UIOffset" } => "UIOffsetValue", 
			{ FullyQualifiedName: "MapKit.MKCoordinateSpan" } => "MKCoordinateSpanValue", 
			{ FullyQualifiedName: "CoreMedia.CMTimeRange" } => "CMTimeRangeValue", 
			{ FullyQualifiedName: "CoreMedia.CMTime" } => "CMTimeValue", 
			{ FullyQualifiedName: "CoreMedia.CMTimeMapping" } => "CMTimeMappingValue", 
			{ FullyQualifiedName: "CoreAnimation.CATransform3D" } => "CATransform3DValue",
			_ => string.Empty,
		};
#pragma warning restore format
	}

	/// <summary>
	/// Gets the name of the NSNumber instance method used to retrieve its underlying value, based on the provided type.
	/// For example, if the type is `int`, this returns "Int32Value".
	/// </summary>
	/// <param name="type">The <see cref="TypeInfo"/> representing the expected underlying type of the NSNumber.</param>
	/// <returns>A string representing the name of the NSNumber method to call (e.g., "Int32Value", "DoubleValue"),
	/// or an empty string if the type is not a supported NSNumber underlying type.</returns>
	internal static string GetNSNumberValue (in TypeInfo type)
	{
#pragma warning disable format
		return type switch {
			{ Name: "nint" } => "NIntValue",
			{ Name: "nuint" } => "NUIntValue",
			{ Name: "nfloat" or "NFloat" } => "NFloatValue",
			{ SpecialType: SpecialType.System_Boolean } => "BooleanValue",
			{ SpecialType: SpecialType.System_Byte } => "ByteValue",
			{ SpecialType: SpecialType.System_Double } => "DoubleValue",
			{ SpecialType: SpecialType.System_Single } => "FloatValue",
			{ SpecialType: SpecialType.System_Int16 } => "Int16Value",
			{ SpecialType: SpecialType.System_Int32 } => "Int32Value",
			{ SpecialType: SpecialType.System_Int64 } => "Int64Value",
			{ SpecialType: SpecialType.System_SByte } => "SByteValue",
			{ SpecialType: SpecialType.System_UInt16 } => "UInt16Value",
			{ SpecialType: SpecialType.System_UInt32 } => "UInt32Value",
			{ SpecialType: SpecialType.System_UInt64 } => "UInt64Value",
			{ SpecialType: SpecialType.System_IntPtr } => "NIntValue",
			{ SpecialType: SpecialType.System_UIntPtr } => "NUIntValue",
			_ => string.Empty,
		};
#pragma warning restore format
	}

	/// <summary>
	/// Generates an if-statement that throws an <see cref="ArgumentNullException"/> if the specified variable is null.
	/// </summary>
	/// <param name="variableName">The name of the variable to check for null.</param>
	/// <returns>An <see cref="IfStatementSyntax"/> that performs the null check and throws if necessary.</returns>
	internal static SyntaxNode ThrowIfNull (string variableName)
	{
		var isExpression = IsPatternExpression (
			IdentifierName (variableName),
			ConstantPattern (LiteralExpression (SyntaxKind.NullLiteralExpression))).NormalizeWhitespace ();

		var throwHelper = ExpressionStatement (InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					ThrowHelper,
					IdentifierName ("ThrowArgumentNullException").WithTrailingTrivia (Space)))
			.WithArgumentList (
				ArgumentList (
					SingletonSeparatedList (Argument (NameOf (variableName))))));

		// if + throw using the mono style, other methods will remove the spaces with added before the ()
		return IfStatement (
			attributeLists: default,
			ifKeyword: Token (SyntaxKind.IfKeyword).WithTrailingTrivia (Space),
			openParenToken: Token (SyntaxKind.OpenParenToken),
			condition: isExpression,
			closeParenToken: Token (SyntaxKind.CloseParenToken),
			statement: throwHelper.WithLeadingTrivia (LineFeed, Tab),
			@else: default);
	}

	/// <summary>
	/// Generates an expression to convert a native value (represented by an expression) to its corresponding managed type.
	/// This method handles various conversions required when receiving data from a native call, such as:
	/// - Casting native integer values to managed enums (including native and smart enums).
	/// - Creating managed wrapper objects like <c>CMSampleBuffer</c> and <c>AudioBuffers</c> from native handles.
	/// - Converting native objects returned via a <c>[BindAs]</c> attribute (e.g., NSNumber, NSValue, NSString) to the corresponding managed type.
	/// - Creating managed delegates from native block pointers.
	/// - Converting native arrays (e.g., CFArray) to managed arrays (e.g., string[], INativeObject[]).
	/// - Creating managed <c>INativeObject</c> or <c>NSObject</c> instances from native handles, with optional handle release semantics.
	/// - Converting native strings (e.g., CFString) to managed strings.
	/// - Converting native booleans (bytes) to managed booleans.
	/// </summary>
	/// <param name="info">The <see cref="ReturnInfo"/> describing the expected managed return type and its attributes.</param>
	/// <param name="expression">The <see cref="ExpressionSyntax"/> representing the native value to be converted.</param>
	/// <returns>An <see cref="ExpressionSyntax"/> representing the converted managed value, or the original expression if no conversion is needed.</returns>
#pragma warning disable format
	internal static ExpressionSyntax? ConvertToManaged (in ReturnInfo info, ExpressionSyntax expression) 
		=> info switch { 
			
			// enum values
			
			{ Type.IsEnum: true, Type.IsNativeEnum: true } 
				=> CastNativeToEnum (expression, info.Type), 
			
			// smart enum, get type from string
			{ Type.IsEnum: true, Type.IsSmartEnum: true, Type.IsNativeEnum: false } 
				=> GetSmartEnumFromNSString (info.Type, Argument (expression)),
			
			// normal enum casting
			{ Type.IsEnum: true, Type.IsSmartEnum: false, Type.IsNativeEnum: false } 
				=> CastExpression (
					info.Type.GetIdentifierSyntax (), 
					expression.WithLeadingTrivia (Space)),
			
			// CoreMedia.CMSampleBuffer
			// new global::CoreMedia.CMSampleBuffer ({0})
			{ Type.FullyQualifiedName: "CoreMedia.CMSampleBuffer" } => 
				New (CMSampleBuffer, [Argument (expression), BoolArgument (false)]), 
			
			// AudioToolbox.AudioBuffers
			// new global::AudioToolbox.AudioBuffers ({0})
			{ Type.FullyQualifiedName: "AudioToolbox.AudioBuffers" } =>
				New (AudioBuffers, [Argument (expression), BoolArgument (false)]),
			
			// bind from NSNumber: NSNumber.ToInt32 (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector"));
			{ BindAs.Type.FullyQualifiedName: "Foundation.NSNumber", Type.IsArray: false } => 
				NSNumberFromHandle (info.Type, [Argument (expression)]),
			
			// bind from NSNumber array: NSArray.ArrayFromHandleFunc <int> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector"), NSNumber.ToInt32, false))
			{ BindAs.Type.FullyQualifiedName: "Foundation.NSNumber", Type.IsArray: true } => 
				NSArrayFromHandleFunc (info.Type.ToArrayElementType ().GetIdentifierSyntax (), [Argument (expression), Argument(NSNumberFromHandle (info.Type)!), BoolArgument (false)]),
			
			// bind from NSValue: NSValue.ToCGPoint (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle (\"myProperty\")))
			{ BindAs.Type.FullyQualifiedName: "Foundation.NSValue", Type.IsArray: false } => 
				NSValueFromHandle (info.Type, [Argument (expression)]),
			
			// bind from NSValue array: NSArray.ArrayFromHandleFunc<CoreGraphics.CGPoint> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle (\"myProperty\")), NSValue.ToCGPoint, false)
			{ BindAs.Type.FullyQualifiedName: "Foundation.NSValue", Type.IsArray: true } => 
				NSArrayFromHandleFunc (info.Type.ToArrayElementType ().GetIdentifierSyntax (), [Argument (expression), Argument(NSValueFromHandle (info.Type)!), BoolArgument (false)]),

			// bind from NSString to a SmartEnum: "global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetNullableValue (arg1)
			{ BindAs.Type.FullyQualifiedName: "Foundation.NSString", Type.IsSmartEnum: true} =>
				SmartEnumGetValue (info.Type, [Argument (expression)]),
			
			// block callback
			// global::ObjCRuntime.Trampolines.{TrampolineNativeClass}.Create (ret)!;
			{ Type.IsDelegate: true } 
				=> TrampolineNativeInvocationClassCreate (info.Type, [Argument (expression)]), 
			
			// Arrays 
			
			// string[]? => CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector")), false);
			{ Type.IsArray: true, Type.ArrayElementType: SpecialType.System_String, Type.IsNullable: true } =>
				StringArrayFromHandle ([Argument (expression), BoolArgument (false)]),

			// string[] => CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector")), false)!;
			{ Type.IsArray: true, Type.ArrayElementType: SpecialType.System_String, Type.IsNullable: false } =>
				SuppressNullableWarning (StringArrayFromHandle ([Argument (expression), BoolArgument (false)])),
			
			// NSObject[] => CFArray.ArrayFromHandle<Foundation.NSMetadataItem> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle ("results")))!;
			{ Type.IsArray: true, Type.ArrayElementTypeIsWrapped: true }
				=> GetCFArrayFromHandle (info.Type.ToArrayElementType ().ToNonNullable ().GetIdentifierSyntax (), [
					Argument (expression)
				], suppressNullableWarning: !info.Type.IsNullable), 
			
			{ Type.IsArray: true, Type.ArrayElementIsINativeObject: true }
				=> GetCFArrayFromHandle (info.Type.ToArrayElementType ().ToNonNullable ().GetIdentifierSyntax (), [
					Argument (expression)
				], suppressNullableWarning: !info.Type.IsNullable), 
			
			// Native objects
			
			// INativeObject from a native handle
			// Runtime.GetINativeObject<NSString> (auxVariable, false)!;
			{ Type.IsINativeObject: true, Type.IsNSObject: false, ReleaseHandle: not null}
				=> GetINativeObject (
					nsObjectType: info.Type.ToNonNullable ().GetIdentifierSyntax (), 
					args: [
						Argument (expression),
						BoolArgument (info.ReleaseHandle.Value)
					], 
					suppressNullableWarning: !info.Type.IsNullable),
			
			{ Type.IsINativeObject: true, Type.IsNSObject: false, ReleaseHandle: null}
				=> GetINativeObject (
					nsObjectType: info.Type.ToNonNullable ().GetIdentifierSyntax (), 
					args: [
						Argument (expression),
					], 
					suppressNullableWarning: !info.Type.IsNullable),
			
			// NSObject from a native handle
			// Runtime.GetNSObject<NSString> (auxVariable, false)!;
			{ Type.IsNSObject: true, ReleaseHandle: not null} 
				=> GetNSObject (
					nsObjectType: info.Type.ToNonNullable ().GetIdentifierSyntax (), 
					args: [
						Argument (expression),
						BoolArgument (false)
					],
					suppressNullableWarning: !info.Type.IsNullable),
			
			{ Type.IsNSObject: true, ReleaseHandle: null} 
				=> GetNSObject (
					nsObjectType: info.Type.ToNonNullable ().GetIdentifierSyntax (), 
					args: [
						Argument (expression),
					],
					suppressNullableWarning: !info.Type.IsNullable),
			
			// Strings

			// string => CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle ("tunnelRemoteAddress")), false);
			{ Type.SpecialType: SpecialType.System_String, Type.IsNullable: true } =>
				StringFromHandle ([Argument (expression), BoolArgument (false)]),

			// string => CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle ("tunnelRemoteAddress")), false)!;
			{ Type.SpecialType: SpecialType.System_String, Type.IsNullable: false } =>
				SuppressNullableWarning (StringFromHandle ([Argument (expression), BoolArgument (false)])),
			
			// Booleans

			// bool => global::ObjCRuntime.Messaging.bool_objc_msgSend (this.Handle, Selector.GetHandle ("canDraw")) != 0;
			{ Type.SpecialType: SpecialType.System_Boolean } => CastToBool (expression),
			
			// general case, just return the result of the send message
			_ => expression,
		}; 
#pragma warning restore format
}
