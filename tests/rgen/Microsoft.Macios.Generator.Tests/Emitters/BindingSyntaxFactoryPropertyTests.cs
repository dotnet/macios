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
				"ret = global::CoreFoundation.CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)!",
				"ret = global::CoreFoundation.CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)!"
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
				"ret = global::CoreFoundation.CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)",
				"ret = global::CoreFoundation.CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)"
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
				"ret = global::CoreFoundation.CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)!",
				"ret = global::CoreFoundation.CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)!"
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
				"ret = global::CoreFoundation.CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)",
				"ret = global::CoreFoundation.CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), false)"
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
				"ret = global::ObjCRuntime.Messaging.int_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\"))",
				"ret = global::ObjCRuntime.Messaging.int_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\"))"
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
				"ret = global::ObjCRuntime.Messaging.uint_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\"))",
				"ret = global::ObjCRuntime.Messaging.uint_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\"))"
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
				"ret = global::ObjCRuntime.Messaging.bool_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")) != 0",
				"ret = global::ObjCRuntime.Messaging.bool_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")) != 0"
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
				"ret = global::ObjCRuntime.Runtime.GetNSObject<Foundation.NSObject> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))!",
				"ret = global::ObjCRuntime.Runtime.GetNSObject<Foundation.NSObject> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))!"
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
				"ret = global::ObjCRuntime.Runtime.GetNSObject<Foundation.NSObject> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
				"ret = global::ObjCRuntime.Runtime.GetNSObject<Foundation.NSObject> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))"
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
				"ret = global::CoreFoundation.CFArray.ArrayFromHandle<Foundation.NSObject> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))!",
				"ret = global::CoreFoundation.CFArray.ArrayFromHandle<Foundation.NSObject> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))!"
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
				"ret = NSNumber.ToInt32 (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
				"ret = NSNumber.ToInt32 (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
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
				"ret = NSNumber.ToInt64 (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
				"ret = NSNumber.ToInt64 (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
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
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<int> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSNumber.ToInt32, false)",
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<int> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSNumber.ToInt32, false)",
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
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<uint> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSNumber.ToUInt32, false)",
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<uint> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSNumber.ToUInt32, false)",
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
				"ret = global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetValue (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
				"ret = global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetValue (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
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
				"ret = NSValue.ToCATransform3D (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
				"ret = NSValue.ToCATransform3D (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
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
				"ret = NSValue.ToCGPoint (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
				"ret = NSValue.ToCGPoint (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")))",
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
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<CoreAnimation.CATransform3D> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSValue.ToCATransform3D, false)",
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<CoreAnimation.CATransform3D> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSValue.ToCATransform3D, false)",
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
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<CoreGraphics.CGPoint> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSValue.ToCGPoint, false)",
				"ret = global::Foundation.NSArray.ArrayFromHandleFunc<CoreGraphics.CGPoint> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"myProperty\")), NSValue.ToCGPoint, false)",
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
