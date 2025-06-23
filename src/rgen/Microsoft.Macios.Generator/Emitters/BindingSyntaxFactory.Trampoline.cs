// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {
	/// <summary>
	/// Returns the statement with the return type of the invoke method for the given type info delegate.
	/// </summary>
	/// <param name="typeInfo"></param>
	/// <param name="auxVariableName"></param>
	/// <returns></returns>
	internal static ExpressionSyntax? GetTrampolineInvokeReturnType (TypeInfo typeInfo, string auxVariableName)
	{
		// ignore those types that are not delegates or that are a delegate with a void return type
		if (!typeInfo.IsDelegate || typeInfo.Delegate.ReturnType.IsVoid)
			return null;

#pragma warning disable format
		// based on the return type of the delegate we build a statement that will return the expected value
		return typeInfo.Delegate.ReturnType switch {

			//  Runtime.RetainAndAutoreleaseNSObject (NSArray.FromNSObjects(auxVariable))
			{ IsArray: true, ArrayElementTypeIsWrapped: true }
				=> RetainAndAutoreleaseNSObject ([Argument(NSArrayFromNSObjects ([Argument (IdentifierName (auxVariableName))]))]),
			
			// Runtime.RetainAndAutoreleaseNativeObject (auxVariable)
			{ IsArray: false, IsINativeObject: true, IsNSObject: false, IsInterface: false}
				=> RetainAndAutoreleaseNativeObject ([Argument(IdentifierName (auxVariableName))]),
			
			// Runtime.RetainAndAutoreleaseNSObject (auxVariable)
			{ IsArray: false, IsWrapped: true }
				=> RetainAndAutoreleaseNSObject ([Argument (IdentifierName(auxVariableName))]),
			
			//  NSString.CreateNative (auxVariable, true);
			{ SpecialType: SpecialType.System_String }
				=> NStringCreateNative ([Argument (IdentifierName(auxVariableName)), BoolArgument (true)]),
			
			// Runtime.RetainAndAutoreleaseNativeObject (auxVariable) of the native NSString that represents a smart enum
			{ IsSmartEnum: true, IsNativeEnum: false }
				=> RetainAndAutoreleaseNSObject ([Argument (IdentifierName(auxVariableName))]),

			// (UIntPtr) (ulong) myParam
			{ IsNativeEnum: true }
				=> CastEnumToNative (auxVariableName, typeInfo.Delegate.ReturnType),
			
			// auxVariable ? (byte) 1 : (byte) 0; 
			{ SpecialType: SpecialType.System_Boolean } 
				=> CastToByte (auxVariableName, typeInfo.Delegate.ReturnType),
			
			// default case, return the value as is
			_ => IdentifierName (auxVariableName),

		};
#pragma warning restore format
	}

	/// <summary>
	/// Returns an expression syntax representing the conversion of a native return value to its corresponding managed type
	/// after a native delegate (block) invocation within a trampoline.
	/// This method handles various conversions, such as:
	/// - Converting a native byte (0 or 1) to a C# bool.
	/// - Converting a native integer to a C# enum (both regular and smart enums from NSString).
	/// - Creating a C# string from a native string handle (CFStringRef).
	/// - Creating a C# string array from a native array handle (CFArrayRef of CFStringRef).
	/// - Creating specific managed objects like CMSampleBuffer or AudioBuffers from their native handles.
	/// - Getting an NSObject or INativeObject instance from a native handle.
	/// - Getting a C# array of NSObject or INativeObject from a native array handle.
	/// If no conversion is needed, it returns an identifier for the auxiliary variable holding the native return value.
	/// </summary>
	/// <param name="typeInfo">The <see cref="TypeInfo"/> of the delegate, used to determine the return type and its properties.</param>
	/// <param name="auxVariableName">The name of the auxiliary variable holding the native return value.</param>
	/// <returns>An <see cref="ExpressionSyntax"/> for the converted managed return value, or null if the delegate returns void.</returns>
	internal static ExpressionSyntax? GetTrampolineNativeInvokeReturnType (TypeInfo typeInfo, string auxVariableName)
	{
		// ignore those types that are not delegates or that are a delegate with a void return type
		if (!typeInfo.IsDelegate || typeInfo.Delegate.ReturnType.IsVoid)
			return null;
		var auxIdentifier = IdentifierName (auxVariableName);
		return ConvertToManaged (typeInfo.Delegate, auxIdentifier);
	}

	/// <summary>
	/// Returns the expression for the creation of the NativeInvocationClass for a given trampoline.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline whose class we want to create.</param>
	/// <param name="arguments">The arguments for pass to the create method.</param>
	/// <returns>The expression needed to create the native invocation class.</returns>
	internal static ExpressionSyntax CreateTrampolineNativeInvocationClass (string trampolineName, ImmutableArray<ArgumentSyntax> arguments)
	{
		var className = Nomenclator.GetTrampolineClassName (trampolineName, Nomenclator.TrampolineClassType.NativeInvocationClass);
		var staticClassName = IdentifierName (className);
		return StaticInvocationExpression (staticClassName, "Create", arguments, suppressNullableWarning: true);
	}

	/// <summary>
	/// Gets the low-level native type syntax for a given native enum type.
	/// This is used when generating code that interacts with native representations of enums.
	/// </summary>
	/// <param name="typeInfo">The <see cref="TypeInfo"/> for the native enum.</param>
	/// <returns>A <see cref="TypeSyntax"/> representing the low-level native type. 
	/// For <see cref="SpecialType.System_Int64"/> it returns <c>IntPtr</c>,
	/// for <see cref="SpecialType.System_UInt64"/> it returns <c>UIntPtr</c>,
	/// and for other underlying types, it returns the corresponding keyword.
	/// </returns>
	/// <exception cref="InvalidOperationException">Thrown if the <paramref name="typeInfo"/> does not have an underlying type for the native enum.</exception>
	internal static TypeSyntax GetNativeEnumLowLevel (in TypeInfo typeInfo)
	{
		// based on the underlying type, the only ones in which we have to be careful are
		// * System_Int64 -> IntPtr
		// * System_UInt64 -> UIntPtr
		// in all other cases, we can use the underlying type as is
		if (typeInfo.EnumUnderlyingType is null)
			throw new InvalidOperationException ("The type info does not have an underlying type for the native enum.");

		return typeInfo.EnumUnderlyingType.Value switch {
			SpecialType.System_Int64 => IntPtr,
			SpecialType.System_UInt64 => UIntPtr,
			_ => IdentifierName (typeInfo.EnumUnderlyingType.Value.GetKeyword ()),
		};

	}

	/// <summary>
	/// Gets the low-level native type syntax for a given <see cref="TypeInfo"/>.
	/// This method is used to determine the appropriate C# type to represent a native type in contexts
	/// like P/Invoke signatures or trampoline function parameters.
	/// </summary>
	/// <param name="typeInfo">The <see cref="TypeInfo"/> for which to get the low-level native type.</param>
	/// <returns>A <see cref="TypeSyntax"/> representing the low-level native type.
	/// For example, pointers return their original syntax, delegates and most Objective-C objects return <c>IntPtr</c>,
	/// native enums return their underlying native type (e.g., <c>int</c>, <c>long</c>, or <c>IntPtr</c>/<c>UIntPtr</c> for 64-bit enums),
	/// booleans return <c>byte</c>, and arrays or strings return <c>NativeHandle</c>.
	/// Other types generally return their direct C# identifier syntax.
	/// </returns>
	internal static TypeSyntax GetLowLevelType (in TypeInfo typeInfo)
	{
#pragma warning disable format
		return typeInfo switch {
			// pointer parameter 
			{ IsPointer: true } => typeInfo.GetIdentifierSyntax (),
			
			// delegate parameter is a NativeHandle
			{ IsDelegate: true } => NativeHandle,
			
			// native enum, return the conversion expression to the native type
			{ IsNativeEnum: true} =>  GetNativeEnumLowLevel (typeInfo),

			// boolean, convert it to byte
			{ SpecialType: SpecialType.System_Boolean } => PredefinedType (Token(SyntaxKind.ByteKeyword)),

			{ IsArray: true } => NativeHandle,

			{ SpecialType: SpecialType.System_String } =>  NativeHandle,

			{ IsProtocol: true } => NativeHandle,

			// special types

			// CoreMedia.CMSampleBuffer
			{ FullyQualifiedName: "CoreMedia.CMSampleBuffer" } => NativeHandle,

			// AudioToolbox.AudioBuffers
			{ FullyQualifiedName: "AudioToolbox.AudioBuffers" } => NativeHandle,

			// general NSObject/INativeObject, has to be after the special types otherwise the special types will
			// fall into the NSObject/INativeObject case

			// same name, native handle
			{ IsNSObject: true } => NativeHandle,

			// same name, native handle
			{ IsINativeObject: true } => NativeHandle,
			
			// by default, we will use the parameter name as is and the type of the parameter
			_ => typeInfo.GetIdentifierSyntax (),
		};
#pragma warning restore format
	}

	/// <summary>
	/// Returns the needed data to build the parameter syntax for the native trampoline delegate.
	/// </summary>
	/// <param name="parameter">The parameter we want to generate for the lower invoke method.</param>
	/// <returns>The parameter syntax needed for the parameter.</returns>
	internal static (SyntaxToken ParameterName, TypeSyntax ParameterType) GetTrampolineInvokeParameter (in DelegateParameter parameter)
	{
		// in the general case we will return the low level type conversion of the parameter type but we 
		// need to handle in a special case those parameters that are passed by reference. We also need to ensure that
		// if the parameter has been decorated with the BindFrom attribute, we will use the type specified in the attribute
		// instead of the type of the parameter.
		var parameterIdentifier = Identifier (parameter.Name);
		TypeInfo parameterType = parameter.BindAs?.Type ?? parameter.Type; 
#pragma warning disable format
		(SyntaxToken ParameterName, TypeSyntax ParameterType) parameterInfo = (IsByRef: parameter.IsByRef, Type: parameterType) switch {
			// parameters that are passed by reference, depend on the type that is referenced
			{ IsByRef: true, Type.IsReferenceType: false, Type.IsNullable: true} 
				=> (parameterIdentifier, 
					PointerType (GetLowLevelType (parameterType.ToNonNullable ()))),
			
			{ IsByRef: true, Type.IsReferenceType: false, Type.IsNullable: false} 
				=> (parameterIdentifier, 
					PointerType (GetLowLevelType (parameterType))),
			
			{ IsByRef: true, Type.IsReferenceType: true, Type.IsNullable: false} 
				=> (parameterIdentifier,
					PointerType (NativeHandle)),
			
			// by default, we will use the parameter name as is and the type of the parameter
			_ => (parameterIdentifier, GetLowLevelType (parameterType)),
		};
#pragma warning restore format
		return parameterInfo;
	}

	/// <summary>
	/// Returns the argument syntax of a parameter to be used for the trampoliner to invoke a delegate.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline whose parameter we are generating.</param>
	/// <param name="parameter">The parameter whose argument syntax has to be calculated.</param>
	/// <returns>The argument syntax for the given parameter.</returns>
	internal static ArgumentSyntax GetTrampolineInvokeArgument (string trampolineName, in DelegateParameter parameter)
	{
		// build the needed expression based on the information of the parameter and its type, taking into account
		// that the type of the parameter might be different from the type specified in the BindAs attribute.
		TypeInfo parameterType = parameter.BindAs?.Type ?? parameter.Type;
		var parameterIdentifier = IdentifierName (parameter.Name);
#pragma warning disable format
		var expression = (Type: parameterType, Parameter: parameter) switch {
			// pointer parameter 
			{ Type.IsPointer: true } => parameterIdentifier,
			
			// parameters that are passed by reference, the nomenclator will return the name of the
			// temporary variable to use for the trampoline, there is no need for us to do anything
			{ Parameter.IsByRef: true, Type.IsReferenceType: false, Type.IsNullable: true} => 
				IdentifierName (Nomenclator.GetNameForTempTrampolineVariable (parameter) ?? parameter.Name),
			
			{ Parameter.IsByRef: true, Type.IsReferenceType: true } => 
				IdentifierName (Nomenclator.GetNameForTempTrampolineVariable (parameter) ?? parameter.Name),
			
			{ Parameter.IsByRef: true, Type.SpecialType: SpecialType.System_Boolean } => 
				IdentifierName (Nomenclator.GetNameForTempTrampolineVariable (parameter) ?? parameter.Name),
			
			// other cases in which we will use AsRef for the pointed type
			{ Parameter.IsByRef: true } 
				=> AsRef (parameterType.ToPointedAtType ().GetIdentifierSyntax (), 
					[Argument (IdentifierName (parameter.Name))]),
			
			// delegate parameter, c callback
			// System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<ParameterType> (ParameterName)
			{ Type.IsDelegate: true, Parameter.IsCCallback: true } => 
				GetDelegateForFunctionPointer (parameterType.GetIdentifierSyntax (), [Argument (parameterIdentifier)]),
			
			// delegate parameter, block callback
			// TrampolineNativeInvocationClass.Create (ParameterName)!
			{ Type.IsDelegate: true, Parameter.IsBlockCallback: true } 
				=> CreateTrampolineNativeInvocationClass (trampolineName, [Argument (parameterIdentifier)]),
			
			// native enum, return the conversion expression to the native type
			{ Type.IsNativeEnum: true} 
				=> CastNativeToEnum (parameter)!,
			
			// boolean, convert it to byte
			{ Type.SpecialType: SpecialType.System_Boolean } 
				=> CastToBool (IdentifierName (parameter.Name))!,
			
			// array types
			
			// CFArray.ArrayFromHandle<{0}> ({1})!
			{ Type.IsArray: true, Type.ArrayElementTypeIsWrapped: true } 
				=> GetCFArrayFromHandle (parameterType.ToArrayElementType ().GetIdentifierSyntax (), [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true), 
			
			// NSArray.ArrayFromHandle<{0}> ({1})!
			{ Type.IsArray: true, Type.ArrayElementIsINativeObject: true } 
				=> GetNSArrayFromHandle (parameterType.ToArrayElementType ().GetIdentifierSyntax (), [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true),
			
			// string[]
			// CFArray.StringArrayFromHandle (ParameterName)!
			{ Type.IsArray: true, Type.ArrayElementType: SpecialType.System_String }
				=> SuppressNullableWarning (StringArrayFromHandle ([Argument (parameterIdentifier)])),
			
			// string
			// CFString.FromHandle (ParameterName)!
			{ Type.SpecialType: SpecialType.System_String }
				=> SuppressNullableWarning (StringFromHandle ([Argument (parameterIdentifier)])),
			
			// Runtime.GetINativeObject<ParameterType> (ParameterName, false)!
			{ Type.IsProtocol: true } => 
				GetINativeObject (parameterType.GetIdentifierSyntax (), [
						Argument (parameterIdentifier), 
						BoolArgument (false)
					], suppressNullableWarning: true),
			// Runtime.GetINativeObject<ParameterType> (ParameterName, true, Forced.Owns)!
			{ Parameter.ForcedType: not null } => GetINativeObject (parameterType.GetIdentifierSyntax (), 
				[
					Argument (parameterIdentifier),
					BoolArgument (true),
					BoolArgument (parameter.ForcedType!.Value.Owns)
				], suppressNullableWarning: true),
			
			// special types
			
			// CoreMedia.CMSampleBuffer
			// {0} == IntPtr.Zero ? null! : new global::CoreMedia.CMSampleBuffer ({0}, false)
			{ Type.FullyQualifiedName: "CoreMedia.CMSampleBuffer" } =>
				IntPtrZeroCheck (parameter.Name, 
					expressionSyntax: New (parameterType, [Argument (parameterIdentifier), BoolArgument (false)]), 
					suppressNullableWarning: true),
			
			// AudioToolbox.AudioBuffers
			// new global::AudioToolbox.AudioBuffers ({0})
			{ Type.FullyQualifiedName: "AudioToolbox.AudioBuffers" } =>
				New (parameterType, [Argument (parameterIdentifier)]),
			
			// general NSObject/INativeObject, has to be after the special types otherwise the special types will
			// fall into the NSObject/INativeObject case
			
			// Runtime.GetNSObject<ParameterType> (ParameterName) 
			{ Type.IsNSObject: true, Type.IsNullable: true} =>
				GetNSObject (parameterType.ToNonNullable ().GetIdentifierSyntax (), [
					Argument (parameterIdentifier)
				], suppressNullableWarning: false),
			
			// Runtime.GetNSObject<ParameterType> (ParameterName)! 
			{ Type.IsNSObject: true } =>
				GetNSObject (parameterType.GetIdentifierSyntax (), [
					Argument (parameterIdentifier)
				], suppressNullableWarning: true),
			
			// Runtime.GetINativeObject<ParameterType> (ParameterName, false)!
			{ Type.IsINativeObject: true, Type.IsNullable: true } =>
				GetINativeObject (parameterType.ToNonNullable ().GetIdentifierSyntax (), [
					Argument (parameterIdentifier), 
					BoolArgument (false)
				], suppressNullableWarning: false),
			
			// Runtime.GetINativeObject<ParameterType> (ParameterName, false)!
			{ Type.IsINativeObject: true } =>
				GetINativeObject (parameterType.GetIdentifierSyntax (), [
					Argument (parameterIdentifier), 
					BoolArgument (false)
				], suppressNullableWarning: true),
			
			// by default, we will use the parameter name as is
			_ => parameterIdentifier
		};
#pragma warning restore format
		
		// at this point we have the native type to the manage type conversion done BUT if we are using a BindFrom
		// attribute, we need get that expression and convert the NSValue/NSNumber to the expected type.
		if (parameter.BindAs is not null) {
#pragma warning disable format
			expression = (BindAsType: parameter.BindAs.Value.Type, ParameterType: parameter.Type) switch {
				{ BindAsType.FullyQualifiedName: "Foundation.NSValue", ParameterType.IsArray: false } =>
					MemberAccessExpression (
						kind: SyntaxKind.SimpleMemberAccessExpression, 
						expression: expression, 
						name: IdentifierName (GetNSValueValue (parameter.Type))),
				{ BindAsType.FullyQualifiedName: "Foundation.NSNumber", ParameterType.IsArray: false } =>
					MemberAccessExpression (
						kind: SyntaxKind.SimpleMemberAccessExpression, 
						expression: expression, 
						name: IdentifierName (GetNSNumberValue (parameter.Type))),
				{ BindAsType.FullyQualifiedName: "Foundation.NSString", ParameterType.IsArray: false }
					=> InvocationExpression(
						MemberAccessExpression(
							SyntaxKind.SimpleMemberAccessExpression,
							parameter.Type.GetIdentifierSyntax (), // smart enum name
							IdentifierName ("GetValue").WithTrailingTrivia (Space)))
						.WithArgumentList (
							ArgumentList (SingletonSeparatedList(Argument (expression)))), // pass the nsstring expression
				// array support: NSArray.ArrayFromHandleFunc<parameterType> (parameterName, NSValue.FromHandle, false)!
				{ BindAsType.FullyQualifiedName: "Foundation.NSValue", ParameterType.IsArray: true } =>
					SuppressNullableWarning (NSArrayFromHandleFunc (
						returnType: parameter.Type.ToArrayElementType ().GetIdentifierSyntax (), 
						arguments: [
							Argument (parameterIdentifier),
							Argument (NSValueFromHandle (parameter.Type)!), 
							BoolArgument (false)
						]
					)),
				{ BindAsType.FullyQualifiedName: "Foundation.NSNumber", ParameterType.IsArray: true } =>
					SuppressNullableWarning (NSArrayFromHandleFunc (
						returnType: parameter.Type.ToArrayElementType ().GetIdentifierSyntax (), 
						arguments: [
							Argument (parameterIdentifier),
							Argument (NSNumberFromHandle (parameter.Type)!), 
							BoolArgument (false)
						]
					)),
				{ BindAsType.FullyQualifiedName: "Foundation.NSString", ParameterType.IsArray: true } =>
					SuppressNullableWarning (NSArrayFromHandleFunc (
						returnType: parameter.Type.ToArrayElementType ().GetIdentifierSyntax (), 
						arguments: [
							Argument (parameterIdentifier),
							Argument (SmartEnumGetValue (parameter.Type)), 
							BoolArgument (false)
						]
					)),
				_ => expression
			};
#pragma warning restore format
		}
		// Argument syntax is the same as the expression syntax, but we need to add the ref kind keyword if needed
		var argument = Argument (expression);
		if (parameter.IsByRef)
			argument = argument.WithRefKindKeyword (
				Token (parameter.ReferenceKind.ToSyntaxKind ()) // match the correct syntax kind
				.WithTrailingTrivia (Space));
		return argument;
	}

	/// <summary>
	/// Generates any necessary pre-invocation statements for a by-ref trampoline argument.
	/// This is used to handle special cases for by-ref parameters, such as creating temporary variables
	/// for nullable or boolean types that require conversion before the trampoline is invoked.
	/// Returns an empty array if no special handling is required.
	/// </summary>
	/// <param name="parameter">The delegate parameter to process for pre-invocation by-ref handling.</param>
	/// <returns>An immutable array of syntax nodes representing the required pre-invocation statements for the by-ref argument.</returns>
	internal static ImmutableArray<SyntaxNode> GetTrampolinePreInvokeByRefArgument (in DelegateParameter parameter)
	{
		// there are two cases in which we need to do something with the byref parameters:
		// 1. the parameter is by ref and nullable, we need to create a temporary variable to hold the value
		// 2. the parameter is a boolean type and we need a conversion
		// any other case we can just use the parameter as is and we will return an empty array
		var tempVariableName = Nomenclator.GetNameForTempTrampolineVariable (parameter);
		if (tempVariableName is null)
			return [];

		if (parameter.Type.IsNullable) {
			// declare a new variable to hold the temp var
			// ParameterType? tempVariable = null;
			var declarationNode = LocalDeclarationStatement (
				VariableDeclaration (parameter.Type.GetIdentifierSyntax ())
					.WithVariables (
						SingletonSeparatedList (
							VariableDeclarator (
									Identifier (tempVariableName))
								.WithInitializer (
									EqualsValueClause (
										LiteralExpression (
											SyntaxKind.NullLiteralExpression))))));
			// check for the parameter being null and assign the value if needed.
			// if (parameterName is not null)
			//     tempVariable =  *parameterName;
			var ifNode = IfStatement (
				IsPatternExpression (
					IdentifierName (parameter.Name),
					UnaryPattern (
						ConstantPattern (
							LiteralExpression (
								SyntaxKind.NullLiteralExpression)))),
				ExpressionStatement (
					AssignmentExpression (
						SyntaxKind.SimpleAssignmentExpression,
						IdentifierName (tempVariableName),
						PrefixUnaryExpression (
							SyntaxKind.PointerIndirectionExpression,
							IdentifierName (parameter.Name)))));
			return [
				declarationNode,
				ifNode,
			];
		}

		if (parameter.Type.SpecialType == SpecialType.System_Boolean) {
			// generates the following:
			// bool {tempVariable} = *{parameterName} != 0;
			var variableDeclaration = LocalDeclarationStatement (
				VariableDeclaration (
						PredefinedType (
							Token (SyntaxKind.BoolKeyword)))
					.WithVariables (
						SingletonSeparatedList (
							VariableDeclarator (
									Identifier (tempVariableName))
								.WithInitializer (
									EqualsValueClause (
										BinaryExpression (
											SyntaxKind.NotEqualsExpression,
											PrefixUnaryExpression (
												SyntaxKind.PointerIndirectionExpression,
												IdentifierName (parameter.Name)),
											LiteralExpression (
												SyntaxKind.NumericLiteralExpression,
												Literal (0))))))));
			return [variableDeclaration];
		}

		// default case, we do not need to do anything
		return [];
	}

	/// <summary>
	/// Returns an array of syntax nodes representing initializations required for a 'byref' parameter before invoking the trampoline.
	/// This is primarily used for handling 'byref' parameters, which may require temporary variables or conversions.
	/// </summary>
	/// <param name="parameter">The delegate parameter, which is expected to be 'byref'.</param>
	/// <returns>An immutable array of syntax nodes representing the initialization statements for the 'byref' parameter.</returns>
	internal static ImmutableArray<SyntaxNode> GetTrampolineInitializationByRefArgument (in DelegateParameter parameter)
	{
		// create the pointer variable and assign it to its default value
		// generates the following:
		// *{ParameterName} = default;
		var expr = ExpressionStatement (
			AssignmentExpression (
				SyntaxKind.SimpleAssignmentExpression,
				PrefixUnaryExpression (
					SyntaxKind.PointerIndirectionExpression,
					IdentifierName (parameter.Name)),
				LiteralExpression (
					SyntaxKind.DefaultLiteralExpression,
					Token (SyntaxKind.DefaultKeyword)))).NormalizeWhitespace ();
		return [expr];
	}

	/// <summary>
	/// Generates any necessary post-invocation statements for a by-ref trampoline argument.
	/// This is used to handle special cases for by-ref parameters, such as assigning back values
	/// from temporary variables for nullable types or converting boolean values back to their native
	/// representation after the trampoline is invoked.
	/// Returns an empty array if no special handling is required.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline. This parameter is not directly used in the current implementation but is kept for consistency with related methods.</param>
	/// <param name="parameter">The delegate parameter to process for post-invocation by-ref handling.</param>
	/// <returns>An immutable array of syntax nodes representing the required post-invocation statements for the by-ref argument.</returns>
	internal static ImmutableArray<SyntaxNode> GetTrampolinePostInvokeByRefArgument (string trampolineName,
		in DelegateParameter parameter)
	{
		// similar to the pre invoke case, we need to do something with the byref parameters:
		// 1. the parameter is by ref and nullable we need to assign the value
		// 2. the parameter is a boolean type we need to convert back the value from a byte
		var tempVariableName = Nomenclator.GetNameForTempTrampolineVariable (parameter);
		if (tempVariableName is null)
			return [];

		if (parameter.Type.IsNullable) {
			// check if the temp variable has a value and assign it to the parameter
			// if (ParameterName is not null && TempVariable.HasValue)
			//     *ParameterName = TempVariable.Value;
			var ifNode = IfStatement (
				BinaryExpression (
					SyntaxKind.LogicalAndExpression,
					IsPatternExpression (
						IdentifierName (parameter.Name),
						UnaryPattern (
							ConstantPattern (
								LiteralExpression (
									SyntaxKind.NullLiteralExpression)))),
					MemberAccessExpression (
						SyntaxKind.SimpleMemberAccessExpression,
						IdentifierName (tempVariableName),
						IdentifierName ("HasValue"))),
				ExpressionStatement (
					AssignmentExpression (
						SyntaxKind.SimpleAssignmentExpression,
						PrefixUnaryExpression (
							SyntaxKind.PointerIndirectionExpression,
							IdentifierName (parameter.Name)),
						MemberAccessExpression (
							SyntaxKind.SimpleMemberAccessExpression,
							IdentifierName (tempVariableName),
							IdentifierName ("Value")))));
			return [ifNode];
		}

		if (parameter.Type.SpecialType == SpecialType.System_Boolean) {
			// set the boolean value from a byte
			// *ParameterName = TempVariable ? (byte) 1 : (byte) 0;
			var assignment = AssignmentExpression (
				SyntaxKind.SimpleAssignmentExpression,
				PrefixUnaryExpression (
					SyntaxKind.PointerIndirectionExpression,
					IdentifierName (parameter.Name)),
				ConditionalExpression (
					IdentifierName (tempVariableName),
					CastExpression (
						PredefinedType (
							Token (SyntaxKind.ByteKeyword)),
						LiteralExpression (
							SyntaxKind.NumericLiteralExpression,
							Literal (1))).WithTrailingTrivia (Space),
					CastExpression (
						PredefinedType (
							Token (SyntaxKind.ByteKeyword)),
						LiteralExpression (
							SyntaxKind.NumericLiteralExpression,
							Literal (0).WithLeadingTrivia (Space)))));
			return [ExpressionStatement (assignment)];
		}

		if (parameter.Type.IsReferenceType) {
			// assign the value of the temp variable to the parameter	
			var assignment = IfStatement (
				IsPatternExpression (
					IdentifierName (parameter.Name),
					UnaryPattern (
						ConstantPattern (
							LiteralExpression (
								SyntaxKind.NullLiteralExpression)))),
				ExpressionStatement (
					AssignmentExpression (
						SyntaxKind.SimpleAssignmentExpression,
						PrefixUnaryExpression (
							SyntaxKind.PointerIndirectionExpression,
							IdentifierName (parameter.Name)),
						InvocationExpression (
								MemberAccessExpression (
									SyntaxKind.SimpleMemberAccessExpression,
									IdentifierName ("Runtime"),
									IdentifierName ("RetainAndAutoreleaseNativeObject").WithTrailingTrivia (Space)))
							.WithArgumentList (
								ArgumentList (
									SingletonSeparatedList (
										Argument (
											IdentifierName (tempVariableName))))))));
			return [assignment];
		}
		return [];
	}

	/// <summary>
	/// Returns a list of syntax nodes representing the necessary initializations for a trampoline argument before the delegate is invoked.
	/// This is primarily used for handling 'byref' parameters, which may require temporary variables or conversions.
	/// </summary>
	/// <param name="parameter">The delegate parameter for which initializations might be needed.</param>
	/// <returns>An immutable array of syntax nodes for the initializations. Returns an empty array if no special initialization is required.</returns>
	internal static ImmutableArray<SyntaxNode> GetTrampolineInvokeArgumentInitializations (in DelegateParameter parameter)
	{
		// decide the type of conversion we need to do based on the type of the parameter
		return parameter switch { { IsByRef: true } => GetTrampolineInitializationByRefArgument (parameter),
			_ => []
		};
	}

	/// <summary>
	/// Returns the list of expressions that need to be executed before the trampoline is invoked. This allows to
	/// help the trampoline to convert the parameters to the expected types.
	/// </summary>
	/// <param name="parameter">The parameters whose conversions we need.</param>
	/// <returns>An immutable array with the needed conversion expressions. Empty is return if no conversion
	/// is needed.</returns>
	internal static ImmutableArray<SyntaxNode> GetTrampolinePreInvokeArgumentConversions (in DelegateParameter parameter)
	{
		// decide the type of conversion we need to do based on the type of the parameter
		return parameter switch { { IsByRef: true } => GetTrampolinePreInvokeByRefArgument (parameter),
			_ => []
		};
	}

	/// <summary>
	/// Returns the list of expressions that need to be executed after the trampoline delegate has been invoked.
	/// This is used to handle conversions for 'byref' parameters after the delegate call.
	/// </summary>
	/// <param name="trampolineName">The name of the trampoline.</param>
	/// <param name="parameter">The parameter for which post-invoke conversions might be needed.</param>
	/// <returns>An immutable array with the needed conversion expressions. Returns an empty array if no conversion is needed.</returns>
	internal static ImmutableArray<SyntaxNode> GetTrampolinePostInvokeArgumentConversions (string trampolineName,
		in DelegateParameter parameter)
	{
		// decide the type of conversion we need to do based on the type of the parameter
		return parameter switch { { IsByRef: true } => GetTrampolinePostInvokeByRefArgument (trampolineName, parameter),
			_ => []
		};
	}

	/// <summary>
	/// Return a immutable array of arguments to be used for the trampoline invoke method. The arguments are all
	/// the different expressions needed to pass the parameters to the trampoline.
	/// </summary>
	/// <param name="trampolineName">The trampoline whose parameters we are generating.</param>
	/// <param name="delegateInfo">The delegate info of the trampoline we are generating.</param>
	/// <returns>An immutable array with the argument expressions needed to invoke the trampoline delegate.</returns>
	internal static ImmutableArray<TrampolineArgumentSyntax> GetTrampolineInvokeArguments (string trampolineName,
		in DelegateInfo delegateInfo)
	{
		// create the builder for the arguments, we already know the size of the array
		var bucket = ImmutableArray.CreateBuilder<TrampolineArgumentSyntax> (delegateInfo.Parameters.Length);
		foreach (var parameter in delegateInfo.Parameters) {
			var argument = new TrampolineArgumentSyntax (GetTrampolineInvokeArgument (trampolineName, parameter)) {
				Initializers = GetTrampolineInvokeArgumentInitializations (parameter),
				PreDelegateCallConversion = GetTrampolinePreInvokeArgumentConversions (parameter),
				PostDelegateCallConversion = GetTrampolinePostInvokeArgumentConversions (trampolineName, parameter),
			};
			bucket.Add (argument);
		}
		return bucket.ToImmutable ();
	}

	/// <summary>
	/// Generates the call to the trampoline delegate.
	/// </summary>
	/// <param name="delegateInfo">The information of the delegate.</param>
	/// <param name="argumentSyntax">The argument syntax for the parameters of the delegate.</param>
	/// <returns>The needed statement to call the delegate with the parameters of the trampoline.</returns>
	internal static StatementSyntax CallTrampolineDelegate (in DelegateInfo delegateInfo,
		in ImmutableArray<TrampolineArgumentSyntax> argumentSyntax)
	{
		// we always need to create a block that performs the call to the trampoline variable with the argument syntax
		// these arguments already have all the needed conversions
		var args = argumentSyntax
			.Select (x => x.ArgumentSyntax)
			.ToImmutableArray ();
		var invocation = InvocationExpression (
				IdentifierName (Nomenclator.GetTrampolineDelegateVariableName ()).WithTrailingTrivia (Space))
			.WithArgumentList (ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ())));

		// return the invocation expression if the delegate return type is a void type
		if (delegateInfo.ReturnType.IsVoid)
			return ExpressionStatement (invocation);

		// perform an assigment to the return variable
		var declaration = VariableDeclaration (
				IdentifierName (
					Identifier (
						TriviaList (),
						SyntaxKind.VarKeyword,
						"var",
						"var",
						TriviaList (Space))))
			.WithVariables (
				SingletonSeparatedList (
					VariableDeclarator (
							Identifier (Nomenclator.GetReturnVariableName ()))
						.WithInitializer (
							EqualsValueClause (invocation.WithLeadingTrivia (Space)).WithLeadingTrivia (Space))));
		return LocalDeclarationStatement (declaration);
	}

	/// <summary>
	/// Helper method to get the parameters of the trampoline delegate and its invoke implementation.
	/// </summary>
	/// <param name="delegateTypeInfo">The delegate type info.</param>
	/// <returns>The parameter list for the delegate to be used in the trampoline.</returns>
	static ParameterListSyntax GetBlockDelegateParameters (in TypeInfo delegateTypeInfo)
	{
		// build the arguments for the delegate, but add a IntPtr parameter at the start of the list 
		var parameterBucket = ImmutableArray.CreateBuilder<ParameterSyntax> (delegateTypeInfo.Delegate!.Parameters.Length + 1);
		// block parameter needed for the trampoline
		parameterBucket.Add (
			Parameter (Identifier (Nomenclator.GetTrampolineBlockParameterName (delegateTypeInfo.Delegate!.Parameters)))
				.WithType (IntPtr));
		// calculate the rest of the parameters  
		foreach (var currentParameter in delegateTypeInfo.Delegate!.Parameters) {
			// build the parameter
			var parameterInfo = GetTrampolineInvokeParameter (currentParameter);
			var parameter = Parameter (parameterInfo.ParameterName)
				.WithType (parameterInfo.ParameterType)
				.NormalizeWhitespace ();
			parameterBucket.Add (parameter);
		}

		var parametersSyntax = ParameterList (
			SeparatedList<ParameterSyntax> (
				parameterBucket.ToImmutableArray ().ToSyntaxNodeOrTokenArray ())).NormalizeWhitespace ();
		return parametersSyntax;
	}

	/// <summary>
	/// Return the delegate declaration for the trampoline delegate. The trampoline delegate is a delegate that
	/// takes as a first parameter a IntPtr that represents the block to be called. The rest of the parameters are
	/// the same as the original delegate.
	/// </summary>
	/// <param name="delegateTypeInfo">The delegate type information.</param>
	/// <param name="delegateName">The name of the delegate.</param>
	/// <returns>The syntax of the delegate.</returns>
	internal static SyntaxNode GetTrampolineDelegateDeclaration (in TypeInfo delegateTypeInfo, string delegateName)
	{
		// generate a new delegate type with the addition of the IntPtr parameter for block
		var modifiers = TokenList (Token (SyntaxKind.UnsafeKeyword), Token (SyntaxKind.InternalKeyword));
		var parametersSyntax = GetBlockDelegateParameters (delegateTypeInfo);
		// delegate declaration
		var declaration = DelegateDeclaration (
				GetLowLevelType (delegateTypeInfo.Delegate!.ReturnType), // return the low level type, not the manged version
				Identifier (delegateName))
			.WithModifiers (modifiers).NormalizeWhitespace ()
			.WithParameterList (parametersSyntax.WithLeadingTrivia (Space));

		return declaration;
	}

	/// <summary>
	/// Generatees the delegate pointer variable for the trampoline. The delegate pointer is a function pointer that
	/// takes the same parameters as the delegate but with the addition of a IntPtr parameter that represents
	/// the native block.
	/// </summary>
	/// <param name="delegateTypeInfo">The information of the delegate.</param>
	/// <returns>The expression that defines a variable that holds the function pointer.</returns>
	internal static StatementSyntax GetTrampolineDelegatePointer (in TypeInfo delegateTypeInfo)
	{
		// build the function parameter list
		var parameterBucket = ImmutableArray.CreateBuilder<FunctionPointerParameterSyntax> (delegateTypeInfo.Delegate!.Parameters.Length + 1);
		// block parameter needed for the trampoline
		parameterBucket.Add (FunctionPointerParameter (IntPtr));
		// calculate the rest of the parameters  
		foreach (var currentParameter in delegateTypeInfo.Delegate!.Parameters) {
			// build the parameter
			var (_, parameterType) = GetTrampolineInvokeParameter (currentParameter);
			parameterBucket.Add (FunctionPointerParameter (parameterType));
		}
		// we need to add the return type of the delegate
		parameterBucket.Add (FunctionPointerParameter (GetLowLevelType (delegateTypeInfo.Delegate!.ReturnType)));

		// generates the function parameter list:
		// example:
		// <IntPtr, int, int, int>
		// that is, the block ptr, the parameters and the return type
		var parametersSyntax = FunctionPointerParameterList (
			SeparatedList<FunctionPointerParameterSyntax> (
				parameterBucket.ToImmutableArray ().ToSyntaxNodeOrTokenArray ())).NormalizeWhitespace ();

		// function pointer type
		var pointerType = FunctionPointerType ()
			.WithCallingConvention (
				FunctionPointerCallingConvention (
					Token (SyntaxKind.UnmanagedKeyword)))
			.WithParameterList (parametersSyntax.WithLeadingTrivia (Space));
		// declare the delegate pointer variable:
		var declaration = VariableDeclaration (pointerType)
			.WithVariables (SingletonSeparatedList (
				VariableDeclarator (
						Identifier (Nomenclator.GetTrampolineDelegatePointerVariableName ()))
					.WithInitializer (
						EqualsValueClause (
							PrefixUnaryExpression (
								SyntaxKind.AddressOfExpression,
								IdentifierName (Nomenclator.GetTrampolineInvokeMethodName ()))))));

		return LocalDeclarationStatement (declaration.NormalizeWhitespace ());
	}

	/// <summary>
	/// Returns the method declaration for the trampoline invoke method. The trampoline invoke method, this is the
	/// method that will be invoked by the native code.
	/// </summary>
	/// <param name="delegateTypeInfo">The delegate whose signature we want to declare.</param>
	/// <returns>The invoke member delcaration.</returns>
	internal static MemberDeclarationSyntax GetTrampolineInvokeSignature (in TypeInfo delegateTypeInfo)
	{
		var modifiers = TokenList (
			Token (SyntaxKind.InternalKeyword),
			Token (SyntaxKind.StaticKeyword),
			Token (SyntaxKind.UnsafeKeyword));
		var parametersSyntax = GetBlockDelegateParameters (delegateTypeInfo);

		var method = MethodDeclaration (
				GetLowLevelType (delegateTypeInfo.Delegate!.ReturnType), // return the low level type, not the manged version
				Identifier (Nomenclator.GetTrampolineInvokeMethodName ()))
			.WithModifiers (modifiers).NormalizeWhitespace ()
			.WithParameterList (parametersSyntax.WithLeadingTrivia (Space));
		return method;
	}

	/// <summary>
	/// Returns the method declaration signature for the native trampoline invoke method.
	/// This is the method that will be directly called from the native side (e.g., by a block invocation).
	/// The parameters and return type match the original managed delegate's signature, without low-level conversions
	/// applied at this stage, as this method is part of the managed-to-native transition.
	/// </summary>
	/// <param name="delegateTypeInfo">The <see cref="TypeInfo"/> of the delegate for which to generate the native invoke signature.</param>
	/// <returns>A <see cref="MemberDeclarationSyntax"/> representing the signature of the native invoke method.</returns>
	internal static MemberDeclarationSyntax GetTrampolineNativeInvokeSignature (in TypeInfo delegateTypeInfo)
	{
		var modifiers = TokenList (
			Token (SyntaxKind.UnsafeKeyword));
		// the parameters are the exact same as the delegate parameters, we are not going to low level them
		var parameterBucket = ImmutableArray.CreateBuilder<ParameterSyntax> (delegateTypeInfo.Delegate!.Parameters.Length);
		foreach (var currentParameter in delegateTypeInfo.Delegate!.Parameters) {
			// build the parameter
			var parameter = Parameter (Identifier (currentParameter.Name))
				.WithType (currentParameter.Type.GetIdentifierSyntax ())
				.NormalizeWhitespace ();
			if (currentParameter.IsByRef)
				parameter = parameter.WithModifiers (TokenList (Token (currentParameter.ReferenceKind.ToSyntaxKind ())));
			parameterBucket.Add (parameter);
		}

		var parametersSyntax = ParameterList (
			SeparatedList<ParameterSyntax> (
				parameterBucket.ToImmutableArray ().ToSyntaxNodeOrTokenArray ())).NormalizeWhitespace ();

		var returnType = delegateTypeInfo.Delegate!.ReturnType.IsVoid
			? PredefinedType (Token (SyntaxKind.VoidKeyword))
			: delegateTypeInfo.Delegate!.ReturnType.GetIdentifierSyntax ();

		var method = MethodDeclaration (
				returnType, // return the low level type, not the managed version
				Identifier (Nomenclator.GetTrampolineInvokeMethodName ()))
			.WithModifiers (modifiers).NormalizeWhitespace ()
			.WithParameterList (parametersSyntax.WithLeadingTrivia (Space));
		return method;
	}

	/// <summary>
	/// Generates a list of <see cref="TrampolineArgumentSyntax"/> objects for invoking a native delegate (block) from a trampoline.
	/// Each <see cref="TrampolineArgumentSyntax"/> encapsulates the argument itself, along with any necessary
	/// pre-invocation initializations, pre-invocation conversions (managed to native), and post-invocation
	/// conversions or cleanup actions (e.g., GC.KeepAlive).
	/// </summary>
	/// <param name="delegateInfo">The <see cref="DelegateInfo"/> describing the delegate whose arguments are being generated.</param>
	/// <returns>An immutable array of <see cref="TrampolineArgumentSyntax"/> for the native delegate invocation.</returns>
	internal static ImmutableArray<TrampolineArgumentSyntax> GetTrampolineNativeInvokeArguments (in DelegateInfo delegateInfo)
	{
		// create the builder for the arguments, we already know the size of the array
		var bucket = ImmutableArray.CreateBuilder<TrampolineArgumentSyntax> (delegateInfo.Parameters.Length);

		// add the first parameter to be the BlockPointer of the class.
		bucket.Add (new TrampolineArgumentSyntax (Argument (IdentifierName (Nomenclator.GetBlockLiteralName ()))));

		// add all the mising parameters to the bucket.
		foreach (var parameter in delegateInfo.Parameters) {
			var argument = new TrampolineArgumentSyntax (GetNativeInvokeArgument (parameter)) {
				Initializers = GetNativeInvokeArgumentInitializations (parameter),
				PreDelegateCallConversion = GetPreNativeInvokeArgumentConversions (parameter),
				PostDelegateCallConversion = GetPostNativeInvokeArgumentConversions (parameter),
			};
			bucket.Add (argument);
		}
		return bucket.ToImmutable ();
	}

	/// <summary>
	/// Generates the statement syntax for calling the native invoker delegate.
	/// This method constructs the invocation of the delegate using the provided arguments,
	/// and handles whether the delegate returns a value or is void.
	/// </summary>
	/// <param name="delegateInfo">The information about the delegate being called.</param>
	/// <param name="argumentSyntax">The immutable array of argument syntax for the delegate call.</param>
	/// <returns>A <see cref="StatementSyntax"/> representing the call to the native invoker delegate.</returns>
	internal static StatementSyntax CallNativeInvokerDelegate (in DelegateInfo delegateInfo,
		in ImmutableArray<TrampolineArgumentSyntax> argumentSyntax)
	{
		// we always need to create a block that performs the call to the trampoline variable with the argument syntax
		// these arguments already have all the needed conversions
		var args = argumentSyntax
			.Select (x => x.ArgumentSyntax)
			.ToImmutableArray ();
		var invocation = InvocationExpression (
				IdentifierName (Nomenclator.GetNativeInvokerVariableName ()).WithTrailingTrivia (Space))
			.WithArgumentList (ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ())));

		// return the invocation expression if the delegate return type is a void type
		if (delegateInfo.ReturnType.IsVoid)
			return ExpressionStatement (invocation);

		// perform an assigment to the return variable
		var declaration = VariableDeclaration (
				IdentifierName (
					Identifier (
						TriviaList (),
						SyntaxKind.VarKeyword,
						"var",
						"var",
						TriviaList (Space))))
			.WithVariables (
				SingletonSeparatedList (
					VariableDeclarator (
							Identifier (Nomenclator.GetReturnVariableName ()))
						.WithInitializer (
							EqualsValueClause (invocation.WithLeadingTrivia (Space)).WithLeadingTrivia (Space))));
		return LocalDeclarationStatement (declaration);
	}

	/// <summary>
	/// Generates an expression to create an instance of a native invocation class for a given trampoline type.
	/// This is used to create a native block from a C# delegate. The generated expression calls the static `Create`
	/// method on the appropriate `NativeInvocationClass` within the `ObjCRuntime.Trampolines` namespace.
	/// </summary>
	/// <param name="trampolineType">The <see cref="TypeInfo"/> of the delegate for which to create the native invocation class.</param>
	/// <param name="arguments">The arguments to pass to the `Create` method.</param>
	/// <returns>An <see cref="ExpressionSyntax"/> representing the call to create the native invocation class instance.</returns>
	internal static ExpressionSyntax TrampolineNativeInvocationClassCreate (in TypeInfo trampolineType, ImmutableArray<ArgumentSyntax> arguments)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		// get the name of the native class to be used to call the create method
		var className =
			Nomenclator.GetTrampolineClassName (trampolineType, Nomenclator.TrampolineClassType.NativeInvocationClass);

		// generate the needed invocation expression for the Create method with the passed arguments
		var invocation = InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					MemberAccessExpression (
						SyntaxKind.SimpleMemberAccessExpression,
						Trampolines,
						IdentifierName (className)),
					IdentifierName ("Create").WithTrailingTrivia (Space))).
			WithArgumentList (argumentList);

		// null ignore
		return PostfixUnaryExpression (
			SyntaxKind.SuppressNullableWarningExpression,
			invocation);
	}
}
