// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003

using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("CoreFoundation.CFString")}.FromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)!",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsvalue)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsvalue)",
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("CoreFoundation.CFString")}.FromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsvalue)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsvalue)"
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("string", underlyingType: SpecialType.System_String),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("CoreFoundation.CFArray")}.StringArrayFromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)!",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value.Handle)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value.Handle)",
			];

			property = new Property (
				name: "MyProperty",
				returnType: ReturnTypeForArray ("string", isNullable: true, underlyingType: SpecialType.System_String),
				symbolAvailability: new (),
				attributes: [],
				modifiers: [],
				accessors: [
					new (
						accessorKind: AccessorKind.Getter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("CoreFoundation.CFArray")}.StringArrayFromHandle ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), false)",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value.Handle)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value.Handle)",
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("ObjCRuntime.Messaging")}.int_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\"))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_int (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_int (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value)",
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("ObjCRuntime.Messaging")}.uint_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\"))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_uint (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_uint (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("ObjCRuntime.Messaging")}.bool_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")) != 0",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_bool (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value ? (byte) 1 : (byte) 0)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_bool (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value ? (byte) 1 : (byte) 0)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))!",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value__handle__)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("ObjCRuntime.Runtime")}.GetNSObject<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), value__handle__)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("CoreFoundation.CFArray")}.ArrayFromHandle<{Global ("Foundation.NSObject")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))!",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value.Handle)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value.Handle)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("Foundation.NSNumber")}.ToInt32 ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("Foundation.NSNumber")}.ToInt64 ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<int> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSNumber")}.ToInt32, false)",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
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
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<uint> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSNumber")}.ToUInt32, false)",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSString")),
			};

			yield return [
				property,
				$"ret = {Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("AVFoundation.AVCaptureSystemPressureLevelExtensions")}.GetValue ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSValue")}.ToCATransform3D ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("Foundation.NSValue")}.ToCATransform3D ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSValue")}.ToCGPoint ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				$"ret = {Global ("Foundation.NSValue")}.ToCGPoint ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")))",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsb_value__handle__)",
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreAnimation.CATransform3D")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCATransform3D, false)",
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreAnimation.CATransform3D")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCATransform3D, false)",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)"
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
					new (
						accessorKind: AccessorKind.Setter,
						symbolAvailability: new (),
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreGraphics.CGPoint")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCGPoint, false)",
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreGraphics.CGPoint")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCGPoint, false)",
				"global::ObjCRuntime.Messaging.void_objc_msgSend_NativeHandle (this.Handle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)",
				"global::ObjCRuntime.Messaging.void_objc_msgSendSuper_NativeHandle (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle (\"setMyProperty:\"), nsa_value__handle__)"
			];

			// test property with no setter
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
						exportPropertyData: ExportData<ObjCBindings.Property>.Default,
						attributes: [],
						modifiers: []
					),
				]
			) {
				ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe),
				BindAs = new (ReturnTypeForNSObject ("Foundation.NSValue")),
			};

			yield return [
				property,
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreGraphics.CGPoint")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSend (this.Handle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCGPoint, false)",
				$"ret = {Global ("Foundation.NSArray")}.ArrayFromHandleFunc<{Global ("CoreGraphics.CGPoint")}> ({Global ("ObjCRuntime.Messaging")}.NativeHandle_objc_msgSendSuper (this.SuperHandle, {Global ("ObjCRuntime.Selector")}.GetHandle (\"myProperty\")), {Global ("Foundation.NSValue")}.ToCGPoint, false)",
				null!,
				null!,
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataPropertyInvocationsGetterTests))]
	void PropertyInvocationsGetterTests (Property property, string getter, string superGetter, string? setter, string? superSetter)
	{
		var invocations = GetInvocations (property);
		Assert.Equal (getter, invocations.Getter.Send.ToString ());
		Assert.Equal (superGetter, invocations.Getter.SendSuper.ToString ());
		if (setter is not null && superSetter is not null) {
			Assert.NotNull (invocations.Setter);
			// testing the message send syntax already tests the arguments
			Assert.Equal (setter, invocations.Setter.Value.Send.ToString ());
			Assert.Equal (superSetter, invocations.Setter.Value.SendSuper.ToString ());
		} else {
			Assert.Null (invocations.Setter);
		}
	}
}

#pragma warning restore APL0003
