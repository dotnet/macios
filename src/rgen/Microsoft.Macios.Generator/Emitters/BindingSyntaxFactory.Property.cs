// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
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

		var getterSend = ConvertToManaged (property, MessagingInvocation (sendMethod, selector, []));
		var getterSuperSend = ConvertToManaged (property, MessagingInvocation (superSendMethod, selector, []));
		// if we cannot get the methods, throw a runtime exception 
		if (getterSend is null || getterSuperSend is null) {
			return (ThrowNotImplementedException (), ThrowNotImplementedException ());
		}

		// get the getter invocation and assign it to the return variable 
		return (
			Send: AssignVariable (Nomenclator.GetReturnVariableName (), getterSend),
			SendSuper: AssignVariable (Nomenclator.GetReturnVariableName (), getterSuperSend)
		);
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
