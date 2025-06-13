// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.DataModel;
using ObjCRuntime;
using Xunit;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;
using static Microsoft.Macios.Generator.Tests.BaseGeneratorTestClass;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryPropertyTests {


	class TestDataPropertyInvocationsGetterTests : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			var property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForString (),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("CoreFoundation.CFString")}.FromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)!",
				$"ret = {Global ("CoreFoundation.CFString")}.FromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)!"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForString (isNullable: true),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("CoreFoundation.CFString")}.FromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)",
				$"ret = {Global ("CoreFoundation.CFString")}.FromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("string"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("CoreFoundation.CFArray")}.StringArrayFromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)!",
				$"ret = {Global ("CoreFoundation.CFArray")}.StringArrayFromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)!"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("string", isNullable: true),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("CoreFoundation.CFArray")}.StringArrayFromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)",
				$"ret = {Global ("CoreFoundation.CFArray")}.StringArrayFromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForInt (),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("ObjCRuntime.Messaging")}.int_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\"))",
				$"ret = {Global ("ObjCRuntime.Messaging")}.int_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\"))"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForInt (isUnsigned: true),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("ObjCRuntime.Messaging")}.uint_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\"))",
				$"ret = {Global ("ObjCRuntime.Messaging")}.uint_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\"))"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForBool (),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("ObjCRuntime.Messaging")}.bool_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")) != 0",
				$"ret = {Global ("ObjCRuntime.Messaging")}.bool_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")) != 0"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForNSObject (),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))!",
				$"ret = {Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))!"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForNSObject (isNullable: true),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("Foundation.NSObject", isNSObject: true),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
			};

			yield return [
				property,
				$"ret = {Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))!",
				$"ret = {Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))!"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForInt (),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSNumber")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSNumber")}.ToInt32 ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("Foundation.NSNumber")}.ToInt32 ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForLong (),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSNumber")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSNumber")}.ToInt64 ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("Foundation.NSNumber")}.ToInt64 ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("int", underlyingType: SpecialType.System_Int32),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSNumber")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<int> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSNumber")}.ToInt32, false)",
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<int> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSNumber")}.ToInt32, false)",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("uint", underlyingType: SpecialType.System_UInt32),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSNumber")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<uint> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSNumber")}.ToUInt32, false)",
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<uint> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSNumber")}.ToUInt32, false)",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForEnum ("AVFoundation.AVCaptureSystemPressureLevel", isSmartEnum: true, isNullable: false),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSString")),
			};

			yield return [
				property,
				$"ret = {Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForNSObject ("CoreAnimation.CATransform3D"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSValue")}.ToCATransform3D ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("Foundation.NSValue")}.ToCATransform3D ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForNSObject ("CoreGraphics.CGPoint"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSValue")}.ToCGPoint ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("Foundation.NSValue")}.ToCGPoint ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("CoreAnimation.CATransform3D"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreAnimation.CATransform3D")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCATransform3D, false)",
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreAnimation.CATransform3D")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCATransform3D, false)",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("CoreGraphics.CGPoint"),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: null,
						attributes: [],
						modifiers: []
					)
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreGraphics.CGPoint")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCGPoint, false)",
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreGraphics.CGPoint")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCGPoint, false)",
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataPropertyInvocationsGetterTests))]
	void PropertyInvocationsGetterTests (Property property, string getter, string superGetter)
	{
		var invocations = GetInvocations (property);
		Assert.Equal (getter, invocations.Getter.Send.ToString ());
		Assert.Equal (superGetter, invocations.Getter.SendSuper.ToString ());
	}
}

#pragma warning restore APL0003
