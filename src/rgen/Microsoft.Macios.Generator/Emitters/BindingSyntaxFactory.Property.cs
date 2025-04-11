// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	internal static (string? Getter, string? Setter) GetObjCMessageSendMethods (in Property property,
		bool isSuper = false, bool isStret = false)
	{
		if (property.IsProperty) {
			// The getter and the setter depend on the accessors that have been set for the property, we do not want
			// to calculate things that we won't use. The export data used will also depend on if the getter/setter has a
			// export attribute attached
			var getter = property.GetAccessor (AccessorKind.Getter);
			string? getterMsgSend = null;
			if (getter is not null) {
				var getterExportData = getter.Value.ExportPropertyData ?? property.ExportPropertyData;
				if (getterExportData is not null) {
					getterMsgSend = GetObjCMessageSendMethodName (getterExportData.Value, property.BindAs?.Type ?? property.ReturnType, [],
						isSuper, isStret);
				}
			}

			var setter = property.GetAccessor (AccessorKind.Setter);
			string? setterMsgSend = null;
			if (setter is not null) {
				// the setter also depends on if we have a bindas attribute or not. If present, the parameter of the
				// setter will be that indicated by the bind as attribute
				var valueParameter = property.BindAs is null
					? property.ValueParameter
					: new Parameter (0, property.BindAs.Value.Type, "value");
				var setterExportData = setter.Value.ExportPropertyData ?? property.ExportPropertyData;
				if (setterExportData is not null) {
					setterMsgSend = GetObjCMessageSendMethodName (setterExportData.Value, TypeInfo.Void,
						[valueParameter], isSuper, isStret);
				}
			}

			return (Getter: getterMsgSend, Setter: setterMsgSend);
		}

		return default;
	}

	internal static (ExpressionSyntax Send, ExpressionSyntax SendSuper) GetGetterInvocations (in Property property,
		string? selector, string? sendMethod, string? superSendMethod)
	{
		// if any of the methods is null, return a throw statement for both
		if (selector is null || sendMethod is null || superSendMethod is null) {
			return (ThrowNotImplementedException (), ThrowNotImplementedException ());
		}

		var getterSend = GetterInvocation (MessagingInvocation (sendMethod, selector, []), property);
		var getterSuperSend = GetterInvocation (MessagingInvocation (superSendMethod, selector, []), property);
		// if we cannot get the methods, throw a runtime exception 
		if (getterSend is null || getterSuperSend is null) {
			return (ThrowNotImplementedException (), ThrowNotImplementedException ());
		}

		// get the getter invocation and assign it to the return variable 
		return (
			Send: AssignVariable (Nomenclator.GetReturnVariableName (property.ReturnType), getterSend),
			SendSuper: AssignVariable (Nomenclator.GetReturnVariableName (property.ReturnType), getterSuperSend)
		);

#pragma warning disable format
		// helper internal function that returns the expression based on the property return type and uses the passed
		// message send expression
		ExpressionSyntax? GetterInvocation (InvocationExpressionSyntax objMsgSend, in Property property)
			=> property switch {
				// bind from NSNumber: NSNumber.ToInt32 (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector"));
				{ BindAs.Type.FullyQualifiedName: "Foundation.NSNumber", ReturnType.IsArray: false } => 
					NSNumberFromHandle (property.ReturnType, [Argument (objMsgSend)]),
				
				// bind from NSNumber array: NSArray.ArrayFromHandleFunc <int> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector"), NSNumber.ToInt32, false))
				{ BindAs.Type.FullyQualifiedName: "Foundation.NSNumber", ReturnType.IsArray: true } => 
					NSArrayFromHandleFunc (property.ReturnType.FullyQualifiedName, [Argument (objMsgSend), Argument(NSNumberFromHandle (property.ReturnType)!), BoolArgument (false)]),
				
				// bind from NSValue: NSValue.ToCGPoint (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle (\"myProperty\")))
				{ BindAs.Type.FullyQualifiedName: "Foundation.NSValue", ReturnType.IsArray: false } => 
					NSValueFromHandle (property.ReturnType, [Argument (objMsgSend)]),
				
				// bind from NSValue array: NSArray.ArrayFromHandleFunc<CoreGraphics.CGPoint> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle (\"myProperty\")), NSValue.ToCGPoint, false)
				{ BindAs.Type.FullyQualifiedName: "Foundation.NSValue", ReturnType.IsArray: true } => 
					NSArrayFromHandleFunc (property.ReturnType.FullyQualifiedName, [Argument (objMsgSend), Argument(NSValueFromHandle (property.ReturnType)!), BoolArgument (false)]),

				// bind from NSString to a SmartEnum: "global::AVFoundation.AVCaptureSystemPressureLevelExtensions.GetNullableValue (arg1)
				{ BindAs.Type.FullyQualifiedName: "Foundation.NSString", ReturnType.IsSmartEnum: true} =>
					SmartEnumGetValue (property.ReturnType, [Argument (objMsgSend)]),
				
				// string[]? => CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector")), false);
				{ ReturnType.IsArray: true, ReturnType.Name: "string", ReturnType.IsNullable: true } =>
					StringArrayFromHandle ([Argument (objMsgSend), BoolArgument (false)]),

				// string[] => CFArray.StringArrayFromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (class_ptr, Selector.GetHandle ("selector")), false)!;
				{ ReturnType.IsArray: true, ReturnType.Name: "string", ReturnType.IsNullable: false } =>
					SuppressNullableWarning (StringArrayFromHandle ([Argument (objMsgSend), BoolArgument (false)])),
				
				// NSObject[] => CFArray.ArrayFromHandle<Foundation.NSMetadataItem> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle ("results")))!;
				{ ReturnType.IsArray: true, ReturnType.ArrayElementTypeIsWrapped: true } => 
					GetCFArrayFromHandle (property.ReturnType.FullyQualifiedName, [Argument (objMsgSend)], suppressNullableWarning: !property.ReturnType.IsNullable),
				
				// Runtime.GetNSObject<Foundation.NSObject> (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle ("delegate")));
				{ ReturnType.IsArray: false, ReturnType.IsNSObject: true } => 
					GetNSObject (property.ReturnType.FullyQualifiedName, [Argument (objMsgSend)], suppressNullableWarning: !property.ReturnType.IsNullable),

				// string => CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle ("tunnelRemoteAddress")), false);
				{ ReturnType.SpecialType: SpecialType.System_String, ReturnType.IsNullable: true } =>
					StringFromHandle ([Argument (objMsgSend), BoolArgument (false)]),

				// string => CFString.FromHandle (global::ObjCRuntime.Messaging.NativeHandle_objc_msgSend (this.Handle, Selector.GetHandle ("tunnelRemoteAddress")), false)!;
				{ ReturnType.SpecialType: SpecialType.System_String, ReturnType.IsNullable: false } =>
					SuppressNullableWarning (StringFromHandle ([Argument (objMsgSend), BoolArgument (false)])),

				// bool => global::ObjCRuntime.Messaging.bool_objc_msgSend (this.Handle, Selector.GetHandle ("canDraw")) != 0;
				{ ReturnType.SpecialType: SpecialType.System_Boolean } => ByteToBool (objMsgSend),
				
				// general case, just return the result of the send message
				_ => objMsgSend,
			}; 
#pragma warning restore format
	}

	internal static PropertyInvocations GetInvocations (in Property property)
	{
		// retrieve the objc_msgSend methods
		var (getter, setter) = GetObjCMessageSendMethods (property, isStret: property.ReturnType.NeedsStret);
		var (superGetter, supperSetter) = GetObjCMessageSendMethods (property, isSuper: true, isStret: property.ReturnType.NeedsStret);
		var getterSelector = property.GetAccessor (AccessorKind.Getter)?.GetSelector (property);
		var setterSelector = property.GetAccessor (AccessorKind.Getter)?.GetSelector (property);

		return new () {
			Getter = GetGetterInvocations (property, getterSelector, getter, superGetter),
			Setter = (ThrowNotImplementedException (), ThrowNotImplementedException ()),
		};
	}
}
