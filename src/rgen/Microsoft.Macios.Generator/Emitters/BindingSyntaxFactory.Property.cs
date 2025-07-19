// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using static Microsoft.Macios.Generator.Nomenclator;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Emitters;

static partial class BindingSyntaxFactory {

	/// <summary>
	/// Gets the names of the `objc_msgSend` P/Invoke methods for a property's getter and setter.
	/// </summary>
	/// <param name="property">The property for which to get the message send method names.</param>
	/// <param name="isSuper">A value indicating whether to call the superclass implementation.</param>
	/// <param name="isStret">A value indicating whether the return value requires a struct-return mechanism.</param>
	/// <returns>A tuple containing the names of the P/Invoke methods for the getter and setter. Either can be <c>null</c> if the corresponding accessor does not exist.</returns>
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

	/// <summary>
	/// Gets the expressions for invoking a property's getter.
	/// </summary>
	/// <param name="property">The property for which to get the invocations.</param>
	/// <param name="selector">The selector for the getter.</param>
	/// <param name="sendMethod">The name of the `objc_msgSend` method for the getter.</param>
	/// <param name="superSendMethod">The name of the `objc_msgSend` method for the superclass getter.</param>
	/// <returns>A tuple containing the expressions for the normal and superclass getter invocations.</returns>
	internal static (ExpressionSyntax Send, ExpressionSyntax SendSuper) GetGetterInvocations (in Property property,
		string? selector, string? sendMethod, string? superSendMethod)
	{
		// if any of the methods is null, return a throw statement for both
		if (selector is null || sendMethod is null || superSendMethod is null) {
			return (ThrowNotImplementedException (), ThrowNotImplementedException ());
		}

		var getterSend = ConvertToManaged (property, MessagingInvocation (sendMethod, selector, [], isSuper: false));
		var getterSuperSend = ConvertToManaged (property, MessagingInvocation (superSendMethod, selector, [], isSuper: true));
		// if we cannot get the methods, throw a runtime exception 
		if (getterSend is null || getterSuperSend is null) {
			return (ThrowNotImplementedException (), ThrowNotImplementedException ());
		}

		// get the getter invocation and assign it to the return variable 
		return (
			Send: AssignVariable (GetReturnVariableName (), getterSend),
			SendSuper: AssignVariable (GetReturnVariableName (), getterSuperSend)
		);
	}

	/// <summary>
	/// Gets the argument and expressions for invoking a property's setter.
	/// </summary>
	/// <param name="property">The property for which to get the invocations.</param>
	/// <param name="selector">The selector for the setter.</param>
	/// <param name="sendMethod">The name of the `objc_msgSend` method for the setter.</param>
	/// <param name="superSendMethod">The name of the `objc_msgSend` method for the superclass setter.</param>
	/// <returns>A tuple containing the argument syntax, and the expressions for the normal and superclass setter invocations.</returns>
	internal static (ArgumentConversions Argument, ExpressionSyntax Send, ExpressionSyntax SendSuper) GetSetterInvocations (
		in Property property, string? selector, string? sendMethod, string? superSendMethod)
	{
		var syntax = GetNativeInvokeArgument (property);
		var argument = new ArgumentConversions {
			Initializers = GetNativeInvokeArgumentInitializations (property),
			Validations = GetNativeInvokeArgumentValidations (property),
			PreCallConversion = GetPreNativeInvokeArgumentConversions (property),
			PostCallConversion = GetPostNativeInvokeArgumentConversions (property),
		};
		// if any of the methods is null, return a throw statement for both
		if (selector is null || sendMethod is null || superSendMethod is null) {
			return (argument, ThrowNotImplementedException (), ThrowNotImplementedException ());
		}

		var setterSend = MessagingInvocation (sendMethod, selector, [syntax], isSuper: false);
		var setterSuperSend = MessagingInvocation (superSendMethod, selector, [syntax], isSuper: true);

		return (
			Argument: argument,
			Send: setterSend,
			SendSuper: setterSuperSend
		);
	}

	/// <summary>
	/// Gets the invocations for a property's getter and setter.
	/// </summary>
	/// <param name="property">The property for which to get the invocations.</param>
	/// <returns>A <see cref="PropertyInvocations"/> instance containing the getter and setter invocations.</returns>
	internal static PropertyInvocations GetInvocations (in Property property)
	{
		// retrieve the objc_msgSend methods
		var (getter, setter) = GetObjCMessageSendMethods (property, isStret: property.ReturnType.NeedsStret);
		var (superGetter, superSetter) = GetObjCMessageSendMethods (property, isSuper: true, isStret: property.ReturnType.NeedsStret);
		var getterSelector = property.GetAccessor (AccessorKind.Getter)?.GetSelector (property);
		var getterInvocations = GetGetterInvocations (property, getterSelector, getter, superGetter);

		(ArgumentConversions Argument, ExpressionSyntax Send, ExpressionSyntax SendSuper)? setterInvocations = null;
		var setterAccessor = property.GetAccessor (AccessorKind.Setter);
		if (setterAccessor is not null) {
			var setterSelector = setterAccessor.Value.GetSelector (property);
			setterInvocations = GetSetterInvocations (property, setterSelector, setter, superSetter);
		}

		return new () {
			Getter = getterInvocations,
			Setter = setterInvocations,
		};
	}
}
