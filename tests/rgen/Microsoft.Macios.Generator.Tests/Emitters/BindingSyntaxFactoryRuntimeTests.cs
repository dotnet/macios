// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Xunit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;
using static Microsoft.Macios.Generator.Tests.BaseGeneratorTestClass;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryRuntimeTests {

	[Theory]
	[InlineData ("Test", "ObjCRuntime.Selector.GetHandle (\"Test\")")]
	[InlineData ("name", "ObjCRuntime.Selector.GetHandle (\"name\")")]
	[InlineData ("setName:", "ObjCRuntime.Selector.GetHandle (\"setName:\")")]
	void SelectorGetHandleTests (string selector, string expectedDeclaration)
	{
		var declaration = SelectorGetHandle (selector);
		Assert.Equal (Global (expectedDeclaration), declaration.ToFullString ());
	}

	[Fact]
	void ThisHandleTests ()
	{
		var declaration = ThisHandle ();
		Assert.Equal ("this.Handle", declaration.ToFullString ());
	}

	[Theory]
	[InlineData ("self", "self.Handle")]
	[InlineData ("instance", "instance.Handle")]
	[InlineData ("myObject", "myObject.Handle")]
	void ThisHandleWithParameterTests (string parameterName, string expected)
	{
		var declaration = ThisHandle (parameterName);
		Assert.Equal (expected, declaration.ToFullString ());
	}

	[Fact]
	void ThisSuperHandleTests ()
	{
		var declaration = ThisSuperHandle ();
		Assert.Equal ("this.SuperHandle", declaration.ToFullString ());
	}

	[Theory]
	[InlineData ("self", "self.SuperHandle")]
	[InlineData ("instance", "instance.SuperHandle")]
	[InlineData ("myObject", "myObject.SuperHandle")]
	void ThisSuperHandleWithParameterTests (string parameterName, string expected)
	{
		var declaration = ThisSuperHandle (parameterName);
		Assert.Equal (expected, declaration.ToFullString ());
	}

	class TestDataMessagingInvocationTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// no extra params
			yield return [
				"IntPtr_objc_msgSend",
				"string",
				ImmutableArray<ArgumentSyntax>.Empty,
				false,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"))"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				ImmutableArray<ArgumentSyntax>.Empty,
				false,
				"self",
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (self.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"))"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				ImmutableArray<ArgumentSyntax>.Empty,
				true,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"))"
			];

			// one param extra
			ImmutableArray<ArgumentSyntax> args = ImmutableArray.Create (
				Argument (IdentifierName ("arg1"))
			);
			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				false,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				false,
				"example",
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (example.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				true,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				false,
				"example",
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (example.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				true,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1)"
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
				false,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1, arg2, arg3)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				false,
				"multi",
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (multi.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1, arg2, arg3)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				true,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1, arg2, arg3)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				false,
				"multi",
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (multi.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1, arg2, arg3)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				true,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), arg1, arg2, arg3)"
			];

			// out parameter
			args = ImmutableArray.Create (
				Argument (PrefixUnaryExpression (SyntaxKind.AddressOfExpression, IdentifierName ("errorValue")))
			);

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				false,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), &errorValue)"
			];

			yield return [
				"IntPtr_objc_msgSend",
				"string",
				args,
				true,
				null!,
				$"{Global ("ObjCRuntime.Messaging")}.IntPtr_objc_msgSend (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"string\"), &errorValue)"
			];

		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataMessagingInvocationTests))]
	void MessagingInvocationTests (string objcMsgSendMethod, string selector, ImmutableArray<ArgumentSyntax> parameters,
		bool isSuper, string? thisParameter, string? expectedDeclaration)
	{
		var declaration = MessagingInvocation (objcMsgSendMethod, selector, parameters, isSuper, thisParameter);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}


	class TestDataStringArrayFromHandleTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("CoreFoundation.CFArray")}.StringArrayFromHandle (arg1)"
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				$"{Global ("CoreFoundation.CFArray")}.StringArrayFromHandle (arg1, arg2, arg3)"
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
				$"{Global ("CoreFoundation.CFString")}.FromHandle (arg1)"
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				$"{Global ("CoreFoundation.CFString")}.FromHandle (arg1, arg2, arg3)"
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
				$"{Global ("Foundation.NSValue")}.ToCATransform3D"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGAffineTransform"),
				$"{Global ("Foundation.NSValue")}.ToCGAffineTransform"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGPoint"),
				$"{Global ("Foundation.NSValue")}.ToCGPoint"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGRect"),
				$"{Global ("Foundation.NSValue")}.ToCGRect"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGSize"),
				$"{Global ("Foundation.NSValue")}.ToCGSize"
			];

			yield return [
				ReturnTypeForNSObject ("CoreGraphics.CGVector"),
				$"{Global ("Foundation.NSValue")}.ToCGVector"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMTime"),
				$"{Global ("Foundation.NSValue")}.ToCMTime"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMTimeMapping"),
				$"{Global ("Foundation.NSValue")}.ToCMTimeMapping"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMTimeRange"),
				$"{Global ("Foundation.NSValue")}.ToCMTimeRange"
			];

			yield return [
				ReturnTypeForNSObject ("CoreMedia.CMVideoDimensions"),
				$"{Global ("Foundation.NSValue")}.ToCMVideoDimensions"
			];

			yield return [
				ReturnTypeForNSObject ("CoreLocation.CLLocationCoordinate2D"),
				$"{Global ("Foundation.NSValue")}.ToCLLocationCoordinate2D"
			];

			yield return [
				ReturnTypeForNSObject ("Foundation.NSRange"),
				$"{Global ("Foundation.NSValue")}.ToNSRange"
			];

			yield return [
				ReturnTypeForNSObject ("MapKit.MKCoordinateSpan"),
				$"{Global ("Foundation.NSValue")}.ToMKCoordinateSpan"
			];

			yield return [
				ReturnTypeForNSObject ("SceneKit.SCNMatrix4"),
				$"{Global ("Foundation.NSValue")}.ToSCNMatrix4"
			];

			yield return [
				ReturnTypeForNSObject ("SceneKit.SCNVector3"),
				$"{Global ("Foundation.NSValue")}.ToSCNVector3"
			];

			yield return [
				ReturnTypeForNSObject ("SceneKit.SCNVector4"),
				$"{Global ("Foundation.NSValue")}.ToSCNVector4"
			];

			yield return [
				ReturnTypeForNSObject ("UIKit.NSDirectionalEdgeInsets"),
				$"{Global ("Foundation.NSValue")}.ToNSDirectionalEdgeInsets"
			];

			yield return [
				ReturnTypeForNSObject ("UIKit.UIEdgeInsets"),
				$"{Global ("Foundation.NSValue")}.ToUIEdgeInsets"
			];

			yield return [
				ReturnTypeForNSObject ("UIKit.UIOffset"),
				$"{Global ("Foundation.NSValue")}.ToUIOffset"
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
				$"{Global ("Foundation.NSNumber")}.ToBool"
			];

			yield return [
				ReturnTypeForInt (),
				$"{Global ("Foundation.NSNumber")}.ToInt32"
			];

			yield return [
				ReturnTypeForInt (isUnsigned: true),
				$"{Global ("Foundation.NSNumber")}.ToUInt32"
			];

			yield return [
				ReturnTypeForShort (),
				$"{Global ("Foundation.NSNumber")}.ToInt16"
			];

			yield return [
				ReturnTypeForShort (isUnsigned: true),
				$"{Global ("Foundation.NSNumber")}.ToUInt16"
			];

			yield return [
				ReturnTypeForLong (),
				$"{Global ("Foundation.NSNumber")}.ToInt64"
			];

			yield return [
				ReturnTypeForLong (isUnsigned: true),
				$"{Global ("Foundation.NSNumber")}.ToUInt64"
			];

			yield return [
				ReturnTypeForNInt (),
				$"{Global ("Foundation.NSNumber")}.ToNInt"
			];

			yield return [
				ReturnTypeForNInt (isUnsigned: true),
				$"{Global ("Foundation.NSNumber")}.ToNUInt"
			];

			yield return [
				ReturnTypeForDouble (),
				$"{Global ("Foundation.NSNumber")}.ToDouble"
			];

			yield return [
				ReturnTypeForFloat (),
				$"{Global ("Foundation.NSNumber")}.ToFloat"
			];

			yield return [
				ReturnTypeForArray ("int", underlyingType: SpecialType.System_Int32),
				$"{Global ("Foundation.NSNumber")}.ToInt32"
			];

			yield return [
				ReturnTypeForArray ("uint", underlyingType: SpecialType.System_UInt32),
				$"{Global ("Foundation.NSNumber")}.ToUInt32"
			];

			yield return [
				ReturnTypeForArray ("nint", underlyingType: SpecialType.System_IntPtr),
				$"{Global ("Foundation.NSNumber")}.ToNInt"
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
				"int".GetIdentifierName ([]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("Foundation.NSArray")}.ArrayFromHandleFunc<int> (arg1)"
			];

			yield return [
				"string".GetIdentifierName ([]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				$"{Global ("Foundation.NSArray")}.ArrayFromHandleFunc<string> (arg1, arg2, arg3)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNSArrayFromHandleFunc))]
	void NSArrayFromHandleFuncTests (TypeSyntax returnType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
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
				$"{Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue (arg1)"
			];

			yield return [
				ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true, isNullable: true),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetNullableValue (arg1)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataSmartEnumGetValue))]
	void SmartEnumGetValueTests (TypeInfo enumType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = SmartEnumGetValue (enumType, arguments);
		var str = declaration.ToString ();
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataNSArrayFromNSObjects : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("Foundation.NSArray")}.FromNSObjects (arg1)"
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				$"{Global ("Foundation.NSArray")}.FromNSObjects (arg1, arg2, arg3)"
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
					[Argument (IdentifierName ("arg1"))]),
				$"{Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue (arg1).GetHandle ()"
			];

			yield return [
				SmartEnumGetValue (
					ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true, isNullable: true),
					[Argument (IdentifierName ("arg1"))]),
				$"{Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetNullableValue (arg1).GetHandle ()"
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


	class TestDataNewWithArgumentsTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// empty constructor
			yield return [
				ImmutableArray<ArgumentSyntax>.Empty,
				"new ()"
			];

			// single param
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				"new (arg1)"
			];

			// several params
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2"))
				),
				"new (arg1, arg2)"
			];

			// out params
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2"))
						.WithRefOrOutKeyword (Token (SyntaxKind.OutKeyword))
						.NormalizeWhitespace ()
				),
				"new (arg1, out arg2)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNewWithArgumentsTests))]
	void NewWithArgumentsTests (ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = New (arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataNew : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// empty constructor
			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray<ArgumentSyntax>.Empty,
				$"new {Global ("AudioToolbox.AudioBuffers")} ()"
			];

			// single param
			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				$"new {Global ("AudioToolbox.AudioBuffers")} (arg1)"
			];

			// several params
			yield return [
				ReturnTypeForNSObject ("AudioToolbox.AudioBuffers"),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2"))
				),
				$"new {Global ("AudioToolbox.AudioBuffers")} (arg1, arg2)"
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
				$"new {Global ("AudioToolbox.AudioBuffers")} (arg1, out arg2)"
			];

			// named tuples
			yield return [
				ReturnTypeForNamedTuple (
					new ("Name", ReturnTypeForString ()),
					new ("Surname", ReturnTypeForString ())),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
						.WithNameColon (NameColon (IdentifierName ("Name"))).NormalizeWhitespace (),
					Argument (IdentifierName ("arg2"))
						.WithNameColon (NameColon (IdentifierName ("Surname"))).NormalizeWhitespace ()
				),
				"(Name: arg1, Surname: arg2)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataNew))]
	void NewTests (TypeInfo typeInfo, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = New (typeInfo, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	[Fact]
	void NewTestsKnownType ()
	{
		var expected = $"new {Global ("AudioToolbox.AudioBuffers")} (arg1, arg2)";
		var declaration = New (AudioBuffers, [
			Argument (IdentifierName ("arg1")),
			Argument (IdentifierName ("arg2"))
		]);
		Assert.Equal (expected, declaration.ToFullString ());
	}

	class TestDataGetNSObject : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"NSString".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				false,
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSString")}> (arg1)"
			];

			yield return [
				"NSString".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				true,
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSString")}> (arg1)!"
			];

			yield return [
				"NSNumber".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				false,
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSNumber")}> (arg1, arg2, arg3)"
			];

			yield return [
				"NSNumber".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				true,
				$"{Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSNumber")}> (arg1, arg2, arg3)!"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetNSObject))]
	void GetNSObjectTests (TypeSyntax nsObjecttype, ImmutableArray<ArgumentSyntax> arguments, bool suppressNullable, string expectedDeclaration)
	{
		var declaration = GetNSObject (nsObjecttype, arguments, suppressNullable);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestGetINativeObject : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"NSString".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				false,
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Foundation.NSString")}> (arg1)"
			];

			yield return [
				"NSString".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				true,
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Foundation.NSString")}> (arg1)!"
			];

			yield return [
				"NSNumber".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				false,
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Foundation.NSNumber")}> (arg1, arg2, arg3)"
			];

			yield return [
				"NSNumber".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				true,
				$"{Global ("ObjCRuntime.Runtime")}.GetINativeObject<{Global ("Foundation.NSNumber")}> (arg1, arg2, arg3)!"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestGetINativeObject))]
	void GetINativeObjectTests (TypeSyntax iNativeObject, ImmutableArray<ArgumentSyntax> arguments, bool suppressNullable, string expectedDeclaration)
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
				$"enumPtr == IntPtr.Zero ? null : {Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue (enumPtr)"
			];

			yield return [
				"enumPtr",
				SmartEnumGetValue (
					ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true),
					[Argument (IdentifierName ("enumPtr"))]
					),
				true,
				$"enumPtr == IntPtr.Zero ? null! : {Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue (enumPtr)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataIntPtrZeroCheck))]
	void IntPtrZeroCheckTests (string variableName, ExpressionSyntax falseExpression, bool suppressNullableWarning, string expectedDeclaration)
	{
		var declaration = IntPtrZeroCheck (variableName, falseExpression, suppressNullableWarning);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataRetainAndAutoreleaseNSObject : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject (arg1)",
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNSObject (arg1, arg2, arg3)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataRetainAndAutoreleaseNSObject))]
	void RetainAndAutoreleaseNSObjectTests (ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = RetainAndAutoreleaseNSObject (arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataRetainAndAutoreleaseNativeObject : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNativeObject (arg1)",
			];

			yield return [
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				$"{Global ("ObjCRuntime.Runtime")}.RetainAndAutoreleaseNativeObject (arg1, arg2, arg3)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataRetainAndAutoreleaseNativeObject))]
	void RetainAndAutoreleaseNativeObjectTests (ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = RetainAndAutoreleaseNativeObject (arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataGetCFArrayFromHandle : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"NSObject".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSObject")}> (arg1)",
			];

			yield return [
				"NSString".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				$"{Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSString")}> (arg1, arg2, arg3)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetCFArrayFromHandle))]
	void GetCFArrayFromHandleTests (TypeSyntax objectType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = GetCFArrayFromHandle (objectType, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataGetNSArrayFromHandle : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"NSObject".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))),
				$"{Global ("Foundation.NSArray")}.ArrayFromHandle<{Global ("Foundation.NSObject")}> (arg1)",
			];

			yield return [
				"NSString".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))
				),
				$"{Global ("Foundation.NSArray")}.ArrayFromHandle<{Global ("Foundation.NSString")}> (arg1, arg2, arg3)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetNSArrayFromHandle))]
	void GetNSArrayFromHandleTests (TypeSyntax objectType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = GetNSArrayFromHandle (objectType, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataGetIdentifierNameTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// no namespace
			yield return [
				Array.Empty<string> (),
				"NSObject",
				false,
				"NSObject",
			];

			yield return [
				Array.Empty<string> (),
				"NSObject",
				true,
				"NSObject",
			];

			// single namespace
			yield return [
				new [] { "Foundation" },
				"NSObject",
				false,
				"Foundation.NSObject",
			];

			// global single namespace
			yield return [
				new [] { "Foundation" },
				"NSObject",
				true,
				Global ("Foundation.NSObject"),
			];

			// multiple namespaces
			yield return [
				new [] {
					"System",
					"Runtime",
					"CompilerServices",
				},
				"Unsafe",
				false,
				"System.Runtime.CompilerServices.Unsafe"
			];

			// global multiple namespaces
			yield return [
				new [] {
					"System",
					"Runtime",
					"CompilerServices",
				},
				"Unsafe",
				true,
				Global ("System.Runtime.CompilerServices.Unsafe")
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetIdentifierNameTests))]
	void GetIdentifierNameTests (string [] @namespace, string @class, bool isGlobal, string expectedDeclaration)
	{
		var declaration = @class.GetIdentifierName (@namespace, isGlobal);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataAsRefTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"NSObject".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				$"{Global ("System.Runtime.CompilerServices.Unsafe")}.AsRef<{Global ("Foundation.NSObject")}> (arg1)"
			];

			yield return [
				"NSString".GetIdentifierName (["Foundation"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				$"{Global ("System.Runtime.CompilerServices.Unsafe")}.AsRef<{Global ("Foundation.NSString")}> (arg1, arg2, arg3)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataAsRefTests))]
	void AsRefTests (TypeSyntax objectType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = AsRef (objectType, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataAsPointerTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				IdentifierName ("int"),
				null!,
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				$"(int*) {Global ("System.Runtime.CompilerServices.Unsafe")}.AsPointer<int> (arg1)"
			];

			yield return [
				IdentifierName ("uint"),
				null!,
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				$"(uint*) {Global ("System.Runtime.CompilerServices.Unsafe")}.AsPointer<uint> (arg1, arg2, arg3)"
			];

			// test case with explicit castType
			yield return [
				IdentifierName ("bool"),
				PredefinedType (Token (SyntaxKind.ByteKeyword)),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				$"(byte*) {Global ("System.Runtime")}.CompilerServices.Unsafe.AsPointer<bool> (arg1)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataAsPointerTests))]
	void AsPointerTests (TypeSyntax objectType, TypeSyntax? castType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = AsPointer (objectType, arguments, castType);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataGetDelegateForFunctionPointer : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"MyDelegateType".GetIdentifierName (["NS"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1"))
				),
				$"{Global ("System.Runtime.InteropServices.Marshal")}.GetDelegateForFunctionPointer<{Global ("NS.MyDelegateType")}> (arg1)"
			];

			yield return [
				"Action<string>".GetIdentifierName (["System"]),
				ImmutableArray.Create (
					Argument (IdentifierName ("arg1")),
					Argument (IdentifierName ("arg2")),
					Argument (IdentifierName ("arg3"))),
				$"{Global ("System.Runtime.InteropServices.Marshal")}.GetDelegateForFunctionPointer<{Global ("System.Action<string>")}> (arg1, arg2, arg3)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataGetDelegateForFunctionPointer))]
	void GetDelegateForFunctionPointerTests (TypeSyntax objectType, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = GetDelegateForFunctionPointer (objectType, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	[Fact]
	void ThrowIfNullTests ()
	{
		var variableName = "markers";
		var declaration = ThrowIfNull (variableName);
		var expected = $"if (markers is null)\n\t{Global ("ObjCRuntime.ThrowHelper")}.ThrowArgumentNullException (nameof (markers));";
		Assert.Equal (expected, declaration.ToFullString ());
	}

	class TestDataArgumentSyntaxForParameterTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return ["arg1", ReferenceKind.None, "arg1"];
			yield return ["arg2", ReferenceKind.In, "in arg2"];
			yield return ["arg3", ReferenceKind.Out, "out arg3"];
			yield return ["arg4", ReferenceKind.Ref, "ref arg4"];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataArgumentSyntaxForParameterTests))]
	void ArgumentSyntaxForParameterTests (string argumentName, ReferenceKind referenceKind, string expectedSyntax)
	{
		var argumentSyntax = ArgumentForParameter (argumentName, referenceKind);
		Assert.Equal (expectedSyntax, argumentSyntax.ToFullString ());
	}

	class TestDataTcsSetExceptionTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"tcs",
				ImmutableArray.Create (
					Argument (IdentifierName ("ex"))),
				"tcs.SetException (ex)"
			];

			yield return [
				"myTcs",
				ImmutableArray.Create (
					Argument (IdentifierName ("exception")),
					Argument (IdentifierName ("additionalArg"))),
				"myTcs.SetException (exception, additionalArg)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataTcsSetExceptionTests))]
	void TcsSetExceptionTests (string tcsVariableName, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = TcsSetException (tcsVariableName, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataTcsSetResultTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				"tcs",
				ImmutableArray.Create (
					Argument (IdentifierName ("result"))),
				"tcs.SetResult (result)"
			];

			yield return [
				"myTcs",
				ImmutableArray.Create (
					Argument (IdentifierName ("value")),
					Argument (IdentifierName ("additionalArg"))),
				"myTcs.SetResult (value, additionalArg)"
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataTcsSetResultTests))]
	void TcsSetResultTests (string tcsVariableName, ImmutableArray<ArgumentSyntax> arguments, string expectedDeclaration)
	{
		var declaration = TcsSetResult (tcsVariableName, arguments);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}

	class TestDataIsNotNullTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return ["variable1", "variable1 is not null"];
			yield return ["another_variable", "another_variable is not null"];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataIsNotNullTests))]
	void IsNotNullTests (string variableName, string expectedDeclaration)
	{
		var declaration = IsNotNull (variableName);
		Assert.Equal (expectedDeclaration, declaration.ToFullString ());
	}
}
