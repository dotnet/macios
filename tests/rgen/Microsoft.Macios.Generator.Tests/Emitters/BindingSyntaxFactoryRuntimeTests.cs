// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryRuntimeTests {

	[Theory]
	[InlineData ("Test", "Selector.GetHandle (\"Test\")")]
	[InlineData ("name", "Selector.GetHandle (\"name\")")]
	[InlineData ("setName:", "Selector.GetHandle (\"setName:\")")]
	void SelectorGetHandleTests (string selector, string expectedDeclaration)
	{
		var declaration = SelectorGetHandle (selector);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataMessagingInvocationTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// no extra params
			yield return [
				"IntPtr_objc_msgSend",
				"string",
				ImmutableArray<ArgumentSyntax>.Empty,
				"global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle (\"string\"))"
			];

			// one param extra
			ImmutableArray<ArgumentSyntax> args = ImmutableArray.Create (
				Argument (IdentifierName ("arg1"))
			);
			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				"global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle (\"string\"), arg1)"
			];

			// several params
			args = ImmutableArray.Create (
				Argument (IdentifierName ("arg1")),
				Argument (IdentifierName ("arg2")),
				Argument (IdentifierName ("arg3"))
			);
			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				"global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle (\"string\"), arg1, arg2, arg3)"
			];

			// out parameter
			args = ImmutableArray.Create (
				Argument (PrefixUnaryExpression (SyntaxKind.AddressOfExpression, IdentifierName ("errorValue")))
			);

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				"global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, Selector.GetHandle (\"string\"), &errorValue)"
			];

		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataMessagingInvocationTests))]
	void MessagingInvocationTests (string objcMsgSendMethod, string selector, ImmutableArray<ArgumentSyntax> parameters,
		string expectedDeclaration)
	{
		var declaration = MessagingInvocation (objcMsgSendMethod, selector, parameters);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}


	class TestDataStringArrayFromHandleTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				"CFArray.StringArrayFromHandle (arg1)"
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				"CFArray.StringArrayFromHandle (arg1, arg2, arg3)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}


	[Theory]
	[ClassData (typeof (TestDataStringArrayFromHandleTests))]
	void StringArrayFromHandleTests (ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = StringArrayFromHandle (arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataStringFromHandleTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				"CFString.FromHandle (arg1)"
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				"CFString.FromHandle (arg1, arg2, arg3)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataStringFromHandleTests))]
	void StringFromHandleTests (ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = StringFromHandle (arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataNSValueFromHandleTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				ReturnTypeForNSObject ("CoreAnimation.CATransform3D"),
				"NSValue.ToCATransform3D"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGAffineTransform"),
				"NSValue.ToCGAffineTransform"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGPoint"),
				"NSValue.ToCGPoint"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGRect"),
				"NSValue.ToCGRect"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGSize"),
				"NSValue.ToCGSize"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGVector"),
				"NSValue.ToCGVector"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMTime"),
				"NSValue.ToCMTime"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMTimeMapping"),
				"NSValue.ToCMTimeMapping"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMTimeRange"),
				"NSValue.ToCMTimeRange"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMVideoDimensions"),
				"NSValue.ToCMVideoDimensions"
			];

			yield return [
				ReturnTypeForNSObject ("CoreLocation.CLLocationCoordinate2D"),
				"NSValue.ToCLLocationCoordinate2D"
			];

			yield return [
				ReturnTypeForNSObject ("Foundation.NSRange"),
				"NSValue.ToNSRange"
			];

			yield return [
				ReturnTypeForNSObject ("MapKit.MKCoordinateSpan"),
				"NSValue.ToMKCoordinateSpan"
			];

			yield return [
				ReturnTypeForNSObject ("SceneKit.SCNMatrix4"),
				"NSValue.ToSCNMatrix4"
			];

			yield return [
				ReturnTypeForNSObject ("SceneKit.SCNVector3"),
				"NSValue.ToSCNVector3"
			];

			yield return [
				ReturnTypeForNSObject ("SceneKit.SCNVector4"),
				"NSValue.ToSCNVector4"
			];

			yield return [
				ReturnTypeForNSObject ("UIKit.NSDirectionalEdgeInsets"),
				"NSValue.ToNSDirectionalEdgeInsets"
			];

			yield return [
				ReturnTypeForNSObject ("UIKit.UIEdgeInsets"),
				"NSValue.ToUIEdgeInsets"
			];

			yield return [
				ReturnTypeForNSObject ("UIKit.UIOffset"),
				"NSValue.ToUIOffset"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNSValueFromHandleTests))]
	void NSValueFromHandleTests (TypeInfo returnType, string expectedDeclaration)
	{
		var declaration = NSValueFromHandle (returnType);
		Assert.Equal (expectedDeclaration, declaration?.ToFullString ());
	}

	class TestDataNSNumberFromHandleTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{

			yield return [
				ReturnTypeForBool (),
				"NSNumber.ToBool"
			];

			yield return [
				ReturnTypeForInt (),
				"NSNumber.ToInt32"
			];

			yield return [
				ReturnTypeForInt (isUnsigned: true),
				"NSNumber.ToUInt32"
			];

			yield return [
				ReturnTypeForShort (),
				"NSNumber.ToInt16"
			];

			yield return [
				ReturnTypeForShort (isUnsigned: true),
				"NSNumber.ToUInt16"
			];

			yield return [
				ReturnTypeForLong (),
				"NSNumber.ToInt64"
			];

			yield return [
				ReturnTypeForLong (isUnsigned: true),
				"NSNumber.ToUInt64"
			];

			yield return [
				ReturnTypeForNInt (),
				"NSNumber.ToNInt"
			];

			yield return [
				ReturnTypeForNInt (isUnsigned: true),
				"NSNumber.ToNUInt"
			];

			yield return [
				ReturnTypeForDouble (),
				"NSNumber.ToDouble"
			];

			yield return [
				ReturnTypeForFloat (),
				"NSNumber.ToFloat"
			];

			yield return [
				ReturnTypeForArray ("int", underlyingType: SpecialType.System_Int32),
				"NSNumber.ToInt32"
			];

			yield return [
				ReturnTypeForArray ("uint", underlyingType: SpecialType.System_UInt32),
				"NSNumber.ToUInt32"
			];

			yield return [
				ReturnTypeForArray ("nint", underlyingType: SpecialType.System_IntPtr),
				"NSNumber.ToNInt"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNSNumberFromHandleTests))]
	void NSNumberFromHandleTests (TypeInfo returnType, string expectedDeclaration)
	{
		var declaration = NSNumberFromHandle (returnType);
		Assert.Equal (expectedDeclaration, declaration?.ToFullString ());
	}

	class TestDataNSArrayFromHandleFunc : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"int",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				"NSArray.ArrayFromHandleFunc<int> (arg1)"
			];

			yield return [
				"string",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				"NSArray.ArrayFromHandleFunc<string> (arg1, arg2, arg3)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNSArrayFromHandleFunc))]
	void NSArrayFromHandleFuncTests (string returnType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = NSArrayFromHandleFunc (returnType, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataSmartEnumGetValue : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				false,
				"global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetValue (arg1)"
			];

			yield return [
				ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				true,
				"global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetNullableValue (arg1)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataSmartEnumGetValue))]
	void SmartEnumGetValueTests (TypeInfo enumType, ImmutableArray<ArgumentSyntax> arguments, bool isNullable, string expectedDeclaration)
	{
		var declaration = SmartEnumGetValue (enumType, arguments, isNullable);
		var str = declaration.ToString ();
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataNSArrayFromNSObjects : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				"NSArray.FromNSObjects (arg1)"
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				"NSArray.FromNSObjects (arg1, arg2, arg3)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNSArrayFromNSObjects))]
	void NSArrayFromNSObjectsTests (ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = NSArrayFromNSObjects (arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataGetHandle : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				SmartEnumGetValue (
					ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true),
					[Argument (IdentifierName ("arg1"))],
					false),
				"global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetValue (arg1).GetHandle ()"
			];

			yield return [
				SmartEnumGetValue (
					ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true),
					[Argument (IdentifierName ("arg1"))],
					true),
				"global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetNullableValue (arg1).GetHandle ()"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetHandle))]
	void GetHandleTests (ExpressionSyntax expression, string expectedDeclaration)
	{
		var declaration = GetHandle (expression);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}


	class TestDataNew : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// empty constructor
			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray<ArgumentSyntax>.Empty,
				false,
				"new AudioToolbox.AudioBuffers ()"
			];

			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray<ArgumentSyntax>.Empty,
				true,
				"new global::AudioToolbox.AudioBuffers ()"
			];

			// single param
			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				false,
				"new AudioToolbox.AudioBuffers (arg1)"
			];

			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				true,
				"new global::AudioToolbox.AudioBuffers (arg1)"
			];

			// several params

			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2"))
				),
				false,
				"new AudioToolbox.AudioBuffers (arg1, arg2)"
			];

			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2"))
				),
				true,
				"new global::AudioToolbox.AudioBuffers (arg1, arg2)"
			];

			// out params
			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2"))
						.WithRefOrOutKeyword (Token (SyntaxKind.OutKeyword))
						.NormalizeWhitespace ()
				),
				false,
				"new AudioToolbox.AudioBuffers (arg1, out arg2)"
			];

			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2"))
						.WithRefOrOutKeyword (Token (SyntaxKind.OutKeyword))
						.NormalizeWhitespace ()
				),
				true,
				"new global::AudioToolbox.AudioBuffers (arg1, out arg2)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNew))]
	void NewTests (TypeInfo typeInfo, ImmutableArray<ArgumentSyntax> arguments, bool global, string expectedDeclaration)
	{
		var declaration = New (typeInfo, arguments, global);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataGetNSObject : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"NSString",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				false,
				"Runtime.GetNSObject<NSString> (arg1)"
			];

			yield return [
				"NSString",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				true,
				"Runtime.GetNSObject<NSString> (arg1)!"
			];

			yield return [
				"NSNumber",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				false,
				"Runtime.GetNSObject<NSNumber> (arg1, arg2, arg3)"
			];

			yield return [
				"NSNumber",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				true,
				"Runtime.GetNSObject<NSNumber> (arg1, arg2, arg3)!"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetNSObject))]
	void GetNSObjectTests (string nsObjecttype, ImmutableArray<ArgumentSyntax> arguments, bool suppressNullable, string expectedDeclaration)
	{
		var declaration = GetNSObject (nsObjecttype, arguments, suppressNullable);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestGetINativeObject : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"NSString",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				false,
				"Runtime.GetINativeObject<NSString> (arg1)"
			];

			yield return [
				"NSString",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				true,
				"Runtime.GetINativeObject<NSString> (arg1)!"
			];

			yield return [
				"NSNumber",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				false,
				"Runtime.GetINativeObject<NSNumber> (arg1, arg2, arg3)"
			];

			yield return [
				"NSNumber",
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				true,
				"Runtime.GetINativeObject<NSNumber> (arg1, arg2, arg3)!"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestGetINativeObject))]
	void GetINativeObjectTests (string iNativeObject, ImmutableArray<ArgumentSyntax> arguments, bool suppressNullable, string expectedDeclaration)
	{
		var declaration = GetINativeObject (iNativeObject, arguments, suppressNullable);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataIntPtrZeroCheck : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"enumPtr",
				SmartEnumGetValue (
					ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true),
					[Argument (IdentifierName ("enumPtr"))]
					),
				false,
				"enumPtr == IntPtr.Zero ? null : global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetValue (enumPtr)"
			];

			yield return [
				"enumPtr",
				SmartEnumGetValue (
					ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true),
					[Argument (IdentifierName ("enumPtr"))]
					),
				true,
				"enumPtr == IntPtr.Zero ? null! : global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetValue (enumPtr)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataIntPtrZeroCheck))]
	void IntPtrZeroCheckTests (string variableName, ExpressionSyntax falseExpression, bool suppressNullableWarning, string expectedDeclaration)
	{
		var declaration = IntPtrZeroCheck (variableName, falseExpression, suppressNullableWarning);
		var str = declaration.ToString ();
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

}
