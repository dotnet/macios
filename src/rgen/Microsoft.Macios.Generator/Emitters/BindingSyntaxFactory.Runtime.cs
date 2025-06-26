// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {
	public const string ClassPtr = "class_ptr";

	/// <summary>
	/// Generates a call to the Runtime.GetNSObject&lt;T&gt; method to create a nsobject from a handle.
	/// </summary>
	/// <param name="nsObjectType">The type of object to use as T</param>
	/// <param name="args">The arguments to pass to the GetNSObject method.</param>
	/// <param name="suppressNullableWarning">If we should suppress the nullable warning.</param>
	/// <returns>The expression that calls GetNSObject method.</returns>
	public static ExpressionSyntax GetNSObject (TypeSyntax nsObjectType, ImmutableArray<ArgumentSyntax> args,
		bool suppressNullableWarning = false)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression (Runtime, "GetNSObject",
			nsObjectType, argsList, suppressNullableWarning);
	}

	/// <summary>
	/// Generates a call to the RuntimeGetINativeObject.&lt;T&gt; method to create a INativeObject from a handle.
	/// </summary>
	/// <param name="nsObjectType">The type of object to use as T</param>
	/// <param name="args">The arguments to pass to the GetNSObject method.</param>
	/// <param name="suppressNullableWarning">If we should suppress the nullable warning.</param>
	/// <returns>The expression that calls GetNSObject method.</returns>
	public static ExpressionSyntax GetINativeObject (TypeSyntax nsObjectType, ImmutableArray<ArgumentSyntax> args,
		bool suppressNullableWarning = false)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression (Runtime, "GetINativeObject",
			nsObjectType, argsList, suppressNullableWarning);
	}

	/// <summary>
	/// Generates a call to the method CFArray.ArrayFromHandle&lt;T&gt; to create a collection of NSObjects.
	/// </summary>
	/// <param name="nsObjectType">The type of the object to use as T</param>
	/// <param name="args">The arguments to bass to the ArrayFromHandle method.</param>
	/// <param name="suppressNullableWarning">If we should suppress the nullable warning.</param>
	/// <returns>The expression that calls ArrayFromHandle method.</returns>
	public static ExpressionSyntax GetCFArrayFromHandle (TypeSyntax nsObjectType, ImmutableArray<ArgumentSyntax> args,
		bool suppressNullableWarning = false)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression (CFArray, "ArrayFromHandle",
			nsObjectType, argsList, suppressNullableWarning);
	}

	/// <summary>
	/// Generates a call to the method NSArray.ArrayFromHandle&lt;T&gt; to create a collection of NSObjects.
	/// </summary>
	/// <param name="nsObjectType">The type of the object to use as T</param>
	/// <param name="args">The arguments to pass to the ArrayFromHandle method.</param>
	/// <param name="suppressNullableWarning">If we should suppress the nullable warning.</param>
	/// <returns>The expression that calls ArrayFromHandle method.</returns>
	public static ExpressionSyntax GetNSArrayFromHandle (TypeSyntax nsObjectType, ImmutableArray<ArgumentSyntax> args,
		bool suppressNullableWarning = false)
	{
		var argsList = ArgumentList (SeparatedList<ArgumentSyntax> (args.ToSyntaxNodeOrTokenArray ()));
		return StaticInvocationGenericExpression (NSArray, "ArrayFromHandle",
			nsObjectType, argsList, suppressNullableWarning);
	}

	/// <summary>
	/// Returns the expression to get the handle of a selector.
	/// </summary>
	/// <param name="selector">The selector whose handle we want to retrieve.</param>
	/// <returns>The expression to retrieve a selector handle.</returns>
	public static InvocationExpressionSyntax SelectorGetHandle (string selector)
	{
		// (selector)
		var args = ArgumentList (SingletonSeparatedList (
			Argument (LiteralExpression (
				SyntaxKind.StringLiteralExpression,
				Literal (selector))))).NormalizeWhitespace ();

		// Selector.GetHandle (selector)
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					Selector,
					IdentifierName ("GetHandle").WithTrailingTrivia (Space)))
			.WithArgumentList (args);
	}

	/// <summary>
	/// Generates the "this.Handle" expression.
	/// </summary>
	/// <returns></returns>
	public static MemberAccessExpressionSyntax ThisHandle ()
	{
		return MemberAccessExpression (
			SyntaxKind.SimpleMemberAccessExpression,
			ThisExpression (),
			IdentifierName ("Handle"));
	}

	/// <summary>
	/// Generates the expression to call the objc_msgSend method.
	/// </summary>
	/// <param name="objcMsgSendMethod">The name of the method in the messaging namespace.</param>
	/// <param name="selector">The selector.</param>
	/// <param name="parameters">An optional argument list.</param>
	/// <returns>The expression needed to call a specific messaging method.</returns>
	public static InvocationExpressionSyntax MessagingInvocation (string objcMsgSendMethod, string selector,
		ImmutableArray<ArgumentSyntax> parameters)
	{
		// the size of the arguments is 2 + the optional arguments
		// [0] = the handle
		// [1] = the selector
		// [2] = the arguments
		// but to be able to use the SeparatedList we need to add a comma for each argument
		// except for the last one, so we need to add parametersCount - 1 commas
		var parametersCount = 2 + parameters.Length;
		var args = new SyntaxNodeOrToken [(2 * parametersCount) - 1];
		// the first two arguments are the selector and the handle, we add those by hand
		args [0] = Argument (ThisHandle ());
		args [1] = Token (SyntaxKind.CommaToken).WithTrailingTrivia (Space);
		args [2] = Argument (SelectorGetHandle (selector));

		// we need to add the commas and the arguments provided by the user of the api
		if (parameters.Length > 0) {
			// add a comma because we know we added the selector
			args [3] = Token (SyntaxKind.CommaToken).WithTrailingTrivia (Space);
			var argsIndex = 4; // start at 4 because we added the first 2 parameters + 2 separators
			var parametersIndex = 0;
			while (argsIndex < args.Length) {
				var currentParameter = parameters [parametersIndex++];
				args [argsIndex++] = currentParameter;
				if (argsIndex < args.Length - 1) {
					args [argsIndex++] = Token (SyntaxKind.CommaToken).WithTrailingTrivia (Space);
				}
			}
		}

		// generates: (this.Handle, Selector.GetHandle (selector), args)
		var argumentList = ArgumentList (SeparatedList<ArgumentSyntax> (args));
		// generates: ObjCRuntime.Messaging.objc_msgSend (this.Handle, Selector.GetHandle (selector), args)
		var invocation = InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					MemberAccessExpression (
						SyntaxKind.SimpleMemberAccessExpression,
						AliasQualifiedName (
							IdentifierName (
								Token (SyntaxKind.GlobalKeyword)),
							IdentifierName ("ObjCRuntime")),
						IdentifierName ("Messaging")),
					IdentifierName (objcMsgSendMethod).WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
		return invocation;
	}

	/// <summary>
	/// Generates the expression to call the CFArray.StringArrayFromHandle method.
	/// </summary>
	/// <param name="arguments">The argument list for the invocation.</param>
	/// <returns>The expression to call CFArray.StringArrayFromHandle method with the provided args.</returns>
	internal static InvocationExpressionSyntax StringArrayFromHandle (ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		// generate: CFArray.StringArrayFromHandle (arg1, arg2, arg3)
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					CFArray,
					IdentifierName ("StringArrayFromHandle").WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates the expression to call the CFString.FromHandle method.
	/// </summary>
	/// <param name="arguments">The argument list for the invocation.</param>
	/// <returns>The expression to call the CFString.FromHandle method with the provided args.</returns>
	internal static InvocationExpressionSyntax StringFromHandle (ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		// generate: CFString.FromHandle (arg1, arg2, arg3)
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					CFString,
					IdentifierName ("FromHandle").WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates the expression to call the CFString.CreateNative method.
	/// </summary>
	/// <param name="arguments">The argument list for the invocation.</param>
	/// <returns>The expression to call the CFString.CreateNative method with the provided args.</returns>
	internal static InvocationExpressionSyntax StringCreateNative (ImmutableArray<ArgumentSyntax> arguments)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					CFString,
					IdentifierName ("CreateNative").WithTrailingTrivia (Space))
			).WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates the expression to call the CFString.ReleaseNative method.
	/// </summary>
	/// <param name="arguments">The argument list for the invocation.</param>
	/// <returns>The expression to call the CFString.ReleaseNative method with the provided args.</returns>
	internal static InvocationExpressionSyntax StringReleaseNative (ImmutableArray<ArgumentSyntax> arguments)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				CFString,
				IdentifierName ("ReleaseNative").WithTrailingTrivia (Space))
		).WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates the expression to call the NSString.CreateNative method.
	/// </summary>
	/// <param name="arguments">The argument list for the invocation.</param>
	/// <returns>The expression to call the NSString.CreateNative method with the provided args.</returns>
	internal static InvocationExpressionSyntax NStringCreateNative (ImmutableArray<ArgumentSyntax> arguments)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					NSString,
					IdentifierName ("CreateNative").WithTrailingTrivia (Space))
			).WithArgumentList (argumentList);
	}

	/// <summary>
	/// Returns the method group needed to get a NSValue from a handle.
	/// </summary>
	/// <param name="returnType">The type info of the return type.</param>
	/// <returns>The member access to the correct NSValue method.</returns>
	internal static MemberAccessExpressionSyntax? NSValueFromHandle (in TypeInfo returnType)
	{
#pragma warning disable format
		var memberName = returnType switch {
			// CoreAnimation
			{ FullyQualifiedName: "CoreAnimation.CATransform3D" } => "ToCATransform3D",

			// CoreGraphics
			{ FullyQualifiedName: "CoreGraphics.CGAffineTransform" } => "ToCGAffineTransform",
			{ FullyQualifiedName: "CoreGraphics.CGPoint" } => "ToCGPoint",
			{ FullyQualifiedName: "CoreGraphics.CGRect" } => "ToCGRect",
			{ FullyQualifiedName: "CoreGraphics.CGSize" } => "ToCGSize",
			{ FullyQualifiedName: "CoreGraphics.CGVector" } => "ToCGVector",

			// CoreMedia
			{ FullyQualifiedName: "CoreMedia.CMTime" } => "ToCMTime",
			{ FullyQualifiedName: "CoreMedia.CMTimeMapping" } => "ToCMTimeMapping",
			{ FullyQualifiedName: "CoreMedia.CMTimeRange" } => "ToCMTimeRange",
			{ FullyQualifiedName: "CoreMedia.CMVideoDimensions" } => "ToCMVideoDimensions",

			// CoreLocation
			{ FullyQualifiedName: "CoreLocation.CLLocationCoordinate2D" } => "ToCLLocationCoordinate2D",

			// Foundation
			{ FullyQualifiedName: "Foundation.NSRange" } => "ToNSRange",

			// MapKit
			{ FullyQualifiedName: "MapKit.MKCoordinateSpan" } => "ToMKCoordinateSpan",

			// SceneKit
			{ FullyQualifiedName: "SceneKit.SCNMatrix4" } => "ToSCNMatrix4",
			{ FullyQualifiedName: "SceneKit.SCNVector3" } => "ToSCNVector3",
			{ FullyQualifiedName: "SceneKit.SCNVector4" } => "ToSCNVector4",

			// UIKit
			{ FullyQualifiedName: "UIKit.NSDirectionalEdgeInsets" } => "ToNSDirectionalEdgeInsets",
			{ FullyQualifiedName: "UIKit.UIEdgeInsets" } => "ToUIEdgeInsets",
			{ FullyQualifiedName: "UIKit.UIOffset" } => "ToUIOffset",

			_ => null,
		};
#pragma warning restore format

		if (memberName is null)
			return null;
		return MemberAccessExpression (
			SyntaxKind.SimpleMemberAccessExpression,
			NSValue,
			IdentifierName (memberName));
	}

	/// <summary>
	/// Generates the correct call to the To* methods in the NSValue class that converts a NativeHandle
	/// to the return type of the method/property.
	/// </summary>
	/// <param name="returnType">The return method of the method/property.</param>
	/// <param name="arguments">The arguments to pass to the NSValue method.</param>
	/// <returns>The expression needed to call the NSNumber method with the given args.</returns>
	internal static InvocationExpressionSyntax? NSValueFromHandle (in TypeInfo returnType,
		ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		// generate: NSNumber.ToInt (arg1, arg2, arg3)
		var nsValueFromHandle = NSValueFromHandle (returnType);
		if (nsValueFromHandle is null)
			return null;
		return InvocationExpression (
				nsValueFromHandle.WithTrailingTrivia (Space))
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Returns the method group needed to get a NSNumber from a handle.
	/// </summary>
	/// <param name="returnType">The type info of the return type.</param>
	/// <returns>The member access to the correct NSNumber method.</returns>
	internal static MemberAccessExpressionSyntax? NSNumberFromHandle (in TypeInfo returnType)
	{
		// create a tuple to store the name and special type depending if it is an array 
		// or a non array type
		var info = returnType.IsArray
			? (Name: returnType.Name, SpecialType: returnType.ArrayElementType)
			: (Name: returnType.Name, SpecialType: returnType.SpecialType);

#pragma warning disable format
		var memberName = info switch {
			// name must be before SpecialType or you'll get them wrong values because
			// the type we want by name also have a valid special type, the tests should catch
			// mistakes here
			{ Name: "NFloat" or "nfloat" } => "ToNFloat",
			{ Name: "nint" } => "ToNInt",
			{ Name: "nuint" } => "ToNUInt",
			{ SpecialType: SpecialType.System_Boolean } => "ToBool",
			{ SpecialType: SpecialType.System_Byte } => "ToByte",
			{ SpecialType: SpecialType.System_Double } => "ToDouble",
			{ SpecialType: SpecialType.System_Single } => "ToFloat",
			{ SpecialType: SpecialType.System_Int16 } => "ToInt16",
			{ SpecialType: SpecialType.System_Int32 } => "ToInt32",
			{ SpecialType: SpecialType.System_Int64 } => "ToInt64",
			{ SpecialType: SpecialType.System_SByte } => "ToSByte",
			{ SpecialType: SpecialType.System_UInt16 } => "ToUInt16",
			{ SpecialType: SpecialType.System_UInt32 } => "ToUInt32",
			{ SpecialType: SpecialType.System_UInt64 } => "ToUInt64",
			_ => null,
		};
#pragma warning restore format
		if (memberName is null)
			return null;
		return MemberAccessExpression (
			SyntaxKind.SimpleMemberAccessExpression,
			NSNumber,
			IdentifierName (memberName));
	}

	/// <summary>
	/// Generates the correct call to the To* methods in the NSNumber class that converts a NativeHandle
	/// to the return type of the method/property.
	/// </summary>
	/// <param name="returnType">The return method of the method/property.</param>
	/// <param name="arguments">The arguments to pass to the NSNumber method.</param>
	/// <returns>The expression needed to call the NSNumber method iwth the given args.</returns>
	internal static InvocationExpressionSyntax? NSNumberFromHandle (in TypeInfo returnType,
		ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		// generate: NSNumber.ToInt (arg1, arg2, arg3)
		var nsNumberFromHandle = NSNumberFromHandle (returnType);
		if (nsNumberFromHandle is null)
			return null;
		return InvocationExpression (
				nsNumberFromHandle.WithTrailingTrivia (Space))
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates a call to the NSArray.ArrayFromHandleFunc with the given arguments.
	/// </summary>
	/// <param name="returnType">The generic return type of the call.</param>
	/// <param name="arguments">An immutable array of arguments.</param>
	/// <returns>The invocation syntax for the method.</returns>
	internal static InvocationExpressionSyntax NSArrayFromHandleFunc (TypeSyntax returnType,
		ImmutableArray<ArgumentSyntax> arguments)
	{
		// generate: (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		// generate <returnType>
		var genericsList = TypeArgumentList (
			SingletonSeparatedList (returnType));

		// generate NSArray.ArrayFromHandleFunc<returnType> (arg1, arg2, arg3)
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					NSArray,
					GenericName ("ArrayFromHandleFunc")
						.WithTypeArgumentList (genericsList)
						.WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Factory method that returns the expression for the NSArray.FromNSObjects invocation.
	/// </summary>
	/// <param name="arguments">The arguments to be used with the invocation.</param>
	/// <returns>The NSArray.FromNSObjects invocation.</returns>
	internal static InvocationExpressionSyntax NSArrayFromNSObjects (ImmutableArray<ArgumentSyntax> arguments)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		return InvocationExpression (MemberAccessExpression (
			SyntaxKind.SimpleMemberAccessExpression,
			NSArray,
			IdentifierName ("FromNSObjects").WithTrailingTrivia (Space)))
			.WithArgumentList (argumentList);
	}

	internal static ExpressionSyntax SmartEnumGetValue (in TypeInfo enumType)
	{
		var extensionClassName = Nomenclator.GetSmartEnumExtensionClassName (enumType.FullyQualifiedName);
		var getValueMethod = enumType.IsNullable ? "GetNullableValue" : "GetValue";

		return MemberAccessExpression (SyntaxKind.SimpleMemberAccessExpression,
			AliasQualifiedName (
				IdentifierName (Token (SyntaxKind.GlobalKeyword)),
				IdentifierName (extensionClassName)),
			IdentifierName (getValueMethod));
	}

	/// <summary>
	/// Returns the enum extension method needed to get the value of the enum from a NativeHandle.
	/// </summary>
	/// <param name="enumType">The type info of the enum type.</param>
	/// <param name="arguments">The arguments to pass to the method invocation.</param>
	/// method does not use the data in the TypeInfo to allow it to be overriden. This is because
	/// the BindAsAttribute might need to override the call. Use the overload when the type info is all
	/// we care about.</param>
	/// <returns>The extension method invocation syntax.</returns>
	internal static InvocationExpressionSyntax SmartEnumGetValue (in TypeInfo enumType,
		ImmutableArray<ArgumentSyntax> arguments)
	{

		// generate (arg1, arg2, arg3)
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));

		// generate: global::extensionNamespace.extensionClassName.GetValue
		var memberAccess = SmartEnumGetValue (enumType).WithTrailingTrivia (Space);

		// generate the invocation with the given params
		return InvocationExpression (memberAccess)
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generates the expression GetHandle () for a given expression syntax. For example:
	/// NSArray.FromNSObjects(retval).GetHandle ();
	/// </summary>
	/// <param name="nativeObject"></param>
	/// <returns></returns>
	internal static InvocationExpressionSyntax GetHandle (ExpressionSyntax nativeObject)
		=> InvocationExpression (
			MemberAccessExpression (
				SyntaxKind.SimpleMemberAccessExpression,
				nativeObject,
				IdentifierName ("GetHandle").WithTrailingTrivia (Space)
			)
		);

	/// <summary>
	/// Generate an object creation expressing for the given type info using the provided arguments.
	/// </summary>
	/// <param name="type">The information of the type of object to be created.</param>
	/// <param name="arguments">The argument list for the object creation expression.</param>
	/// <returns>An object creation expression.</returns>
	internal static ObjectCreationExpressionSyntax New (TypeSyntax type, ImmutableArray<ArgumentSyntax> arguments)
	{
		var argumentList = ArgumentList (
			SeparatedList<ArgumentSyntax> (arguments.ToSyntaxNodeOrTokenArray ()));
		return ObjectCreationExpression (type.WithLeadingTrivia (Space).WithTrailingTrivia (Space))
			.WithArgumentList (argumentList);
	}

	/// <summary>
	/// Generate an object creation expression for the given type info using the provided arguments.
	/// </summary>
	/// <param name="type">The information of the type of object to be created.</param>
	/// <param name="arguments">The argument list for the object creation expression.</param>
	/// <returns>An object creation expression.</returns>
	internal static ObjectCreationExpressionSyntax New (in TypeInfo type, ImmutableArray<ArgumentSyntax> arguments)
		=> New (type.GetIdentifierSyntax (), arguments);

	/// <summary>
	/// Generates a nameof(variableName) expression.
	/// </summary>
	/// <param name="variableName">The name of the variable to use in the nameof expression.</param>
	/// <returns>An <see cref="InvocationExpressionSyntax"/> representing the nameof call.</returns>
	internal static InvocationExpressionSyntax NameOf (string variableName)
	{
		return InvocationExpression (
				IdentifierName (
					Identifier (TriviaList (),
						SyntaxKind.NameOfKeyword,
						"nameof", "nameof",
						TriviaList (Space))))
			.WithArgumentList (
				ArgumentList (
					SingletonSeparatedList (
						Argument (
							IdentifierName (variableName)))));
	}

	/// <summary>
	/// Generate a ternary expression that checks if the variable is IntPtr.Zero and returns null or the expression
	/// </summary>
	/// <param name="variableName">The variable to check against IntPtr.Zero.</param>
	/// <param name="expressionSyntax">The expression to use on false.</param>
	/// <param name="suppressNullableWarning">If we should suppress the nullable warning if the true case.</param>
	/// <returns>The ternary expression.</returns>
	internal static ExpressionSyntax IntPtrZeroCheck (string variableName, ExpressionSyntax expressionSyntax,
		bool suppressNullableWarning = false)
	{
		// generate: null or null! depending if we want to suppress the nullable warning
		ExpressionSyntax nullExpression = suppressNullableWarning
			? PostfixUnaryExpression (
				SyntaxKind.SuppressNullableWarningExpression,
				LiteralExpression (
					SyntaxKind.NullLiteralExpression))
			: LiteralExpression (
				SyntaxKind.NullLiteralExpression);

		// generate: (variableName == IntPtr.Zero) ? null : expressionSyntax
		return ConditionalExpression (
			BinaryExpression (
				SyntaxKind.EqualsExpression,
				IdentifierName (variableName).WithTrailingTrivia (Space),
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName ("IntPtr").WithLeadingTrivia (Space),
					IdentifierName ("Zero").WithTrailingTrivia (Space))),
			nullExpression.WithLeadingTrivia (Space).WithTrailingTrivia (Space),
			expressionSyntax.WithLeadingTrivia (Space));
	}

	/// <summary>
	/// Generate a call to Runtime.RetainAndAutoreleaseNSObject (args) statement.
	/// </summary>
	/// <param name="arguments">The arguments to use to call Runtime.RetainAndAutoreleaseNSObject.</param>
	/// <returns>The C# expression for the call.</returns>
	internal static ExpressionSyntax RetainAndAutoreleaseNSObject (ImmutableArray<ArgumentSyntax> arguments)
		=> StaticInvocationExpression (Runtime, "RetainAndAutoreleaseNSObject", arguments);

	/// <summary>
	/// Generate a call to Runtime.RetainAndAutoreleaseNativeObject (args) statement.
	/// </summary>
	/// <param name="arguments">The arguments to use to call Runtime.RetainAndAutoreleaseNSObject.</param>
	/// <returns>The C# expression for the call.</returns>
	internal static ExpressionSyntax RetainAndAutoreleaseNativeObject (ImmutableArray<ArgumentSyntax> arguments)
		=> StaticInvocationExpression (Runtime, "RetainAndAutoreleaseNativeObject", arguments);

	/// <summary>
	/// Generates a call to System.GC.KeepAlive(variableName).
	/// </summary>
	/// <param name="variableName">The name of the variable to keep alive.</param>
	/// <returns>An <see cref="InvocationExpressionSyntax"/> representing the call to GC.KeepAlive.</returns>
	internal static InvocationExpressionSyntax KeepAlive (string variableName)
	{
		return InvocationExpression (
				MemberAccessExpression (
					SyntaxKind.SimpleMemberAccessExpression,
					GC,
					IdentifierName ("KeepAlive").WithTrailingTrivia (Space)))
			.WithArgumentList (
				ArgumentList (
					SingletonSeparatedList (
						Argument (IdentifierName (variableName)))));
	}
}
