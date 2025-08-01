// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using Microsoft.Macios.Generator.IO;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Marker interface to ensure that the EmitSelectorFields and EmitMethods are used for emitters that generate classes.
/// </summary>
interface IClassEmitter : ICodeEmitter { }

static class ClassEmitterExtensions {
	/// <summary>
	/// Emit the selector fields for the current class. The method will add the fields to the binding context so that
	/// they can be used later.
	/// </summary>
	/// <param name="self"></param>
	/// <param name="bindingContext">The current binding context.</param>
	/// <param name="classBlock">The current class block.</param>
	public static void EmitSelectorFields (this IClassEmitter self, in BindingContext bindingContext, TabbedWriter<StringWriter> classBlock)
	{
		// we will use the binding context to store the name of the selectors so that later other methods can
		// access them
		foreach (var method in bindingContext.Changes.Methods) {
			if (method.ExportMethodData.Selector is null)
				continue;
			var selectorName = method.ExportMethodData.GetSelectorFieldName ()!;
			if (bindingContext.SelectorNames.TryAdd (method.ExportMethodData.Selector, selectorName)) {
				EmitField (method.ExportMethodData.Selector, selectorName);
			}
		}

		// Similar to methods, but with properties is hard because we have a selector for the different 
		// accessors.
		//
		// The accessor.GetSelector method helps to simplify the logic by returning the 
		// correct selector for the accessor taking the export data from the property into account
		foreach (var property in bindingContext.Changes.Properties) {
			if (!property.IsProperty)
				// ignore fields
				continue;
			var getter = property.GetAccessor (AccessorKind.Getter);
			if (!getter.IsNullOrDefault) {
				var selector = getter.GetSelector (property)!;
				var selectorName = selector.GetSelectorFieldName ();
				if (bindingContext.SelectorNames.TryAdd (selector, selectorName)) {
					EmitField (selector, selectorName);
				}
			}

			var setter = property.GetAccessor (AccessorKind.Setter);
			if (!setter.IsNullOrDefault) {
				var selector = setter.GetSelector (property)!;
				var selectorName = selector.GetSelectorFieldName ();
				if (bindingContext.SelectorNames.TryAdd (selector, selectorName)) {
					EmitField (selector, selectorName);
				}
			}
		}
		// helper function that simply writes the necessary fields to the class block.
		void EmitField (string selector, string selectorName)
		{
			classBlock.AppendGeneratedCodeAttribute (optimizable: true);
			classBlock.WriteLine (GetSelectorStringField (selector, selectorName).ToString ());
			classBlock.WriteLine (GetSelectorHandleField (selector, selectorName).ToString ());
			// reading generated code should not be painful, add a space
			classBlock.WriteLine ();
		}
	}

	/// <summary>
	/// Emits the body for a method that does not return a value.
	/// </summary>
	/// <param name="context">Current binding context.</param>
	/// <param name="method">The method for which to generate the body.</param>
	/// <param name="invocations">The method invocations and argument transformations.</param>
	/// <param name="methodBlock">The writer for the method block.</param>
	static void EmitVoidMethodBody (in BindingContext context, in Method method, in MethodInvocations invocations, TabbedWriter<StringWriter> methodBlock)
	{
		// validate and init the needed temp variables
		foreach (var argument in invocations.Arguments) {
			methodBlock.Write (argument.Validations, verifyTrivia: false);
			methodBlock.Write (argument.Initializers, verifyTrivia: false);
		}

		// do any pre-call conversions that might be needed, for example string to NSString
		foreach (var argument in invocations.Arguments) {
			methodBlock.Write (argument.PreCallConversion, verifyTrivia: false);
		}

		// if we are dealing with a protocol or an extension method, we need to call send directly
		if (context.Changes.BindingType == BindingType.Protocol || method.IsExtension) {
			methodBlock.WriteRaw (
$@"{ExpressionStatement (invocations.Send)}
{ExpressionStatement (KeepAlive (method.This))}
");
		} else {
			// simply call the send or sendSuper accordingly
			methodBlock.WriteRaw (
$@"if (IsDirectBinding) {{
	{ExpressionStatement (invocations.Send)}
}} else {{
	{ExpressionStatement (invocations.SendSuper)}
}}
{ExpressionStatement (KeepAlive (method.This))}
");
		}

		// before we leave the methods, do any post operations
		foreach (var argument in invocations.Arguments) {
			methodBlock.Write (argument.PostCallConversion, verifyTrivia: false);
		}
	}

	/// <summary>
	/// Emits the body for a method that returns a value.
	/// </summary>
	/// <param name="method">The method for which to generate the body.</param>
	/// <param name="invocations">The method invocations and argument transformations.</param>
	/// <param name="methodBlock">The writer for the method block.</param>
	static void EmitReturnMethodBody (in BindingContext context, in Method method, in MethodInvocations invocations, TabbedWriter<StringWriter> methodBlock)
	{
		// similar to the void method but we need to create a temp variable to store the return value
		// and do any conversions that might be needed for the return value, for example byte to bool
		var (tempVar, tempDeclaration) = GetReturnValueAuxVariable (method.ReturnType);

		// init and validate the needed temp variables
		foreach (var argument in invocations.Arguments) {
			methodBlock.Write (argument.Validations, verifyTrivia: false);
			methodBlock.Write (argument.Initializers, verifyTrivia: false);
		}

		// perform any pre-call conversions that might be needed, for example string to NSString
		foreach (var argument in invocations.Arguments) {
			methodBlock.Write (argument.PreCallConversion, verifyTrivia: false);
		}

		// if we are dealing with a protocol or an extension method, we need to call send directly
		if (context.Changes.BindingType == BindingType.Protocol || method.IsExtension) {
			methodBlock.WriteRaw (
$@"{tempDeclaration}
{ExpressionStatement (invocations.Send)}
{ExpressionStatement (KeepAlive (method.This))}
");
		} else {
			methodBlock.WriteRaw (
$@"{tempDeclaration}
if (IsDirectBinding) {{
	{ExpressionStatement (invocations.Send)}
}} else {{
	{ExpressionStatement (invocations.SendSuper)}
}}
{ExpressionStatement (KeepAlive (method.This))}
");
		}
		// before returning the value, we need to do the post operations for the temp vars
		foreach (var argument in invocations.Arguments) {
			methodBlock.Write (argument.PostCallConversion, verifyTrivia: false);
		}
		methodBlock.WriteLine ($"return {tempVar};");
	}

	/// <summary>
	/// Emits the code for a given method, including its async version if applicable.
	/// </summary>
	/// <param name="self">The class emitter.</param>
	/// <param name="context">The current binding context.</param>
	/// <param name="method">The method to emit.</param>
	/// <param name="classBlock">The current class block writer.</param>
	/// <param name="uiThreadCheck">An optional UI thread check expression. If not provided, it will be created based on the context.</param>
	public static void EmitMethod (this IClassEmitter self, in BindingContext context, in Method method,
		TabbedWriter<StringWriter> classBlock, ExpressionStatementSyntax? uiThreadCheck = null)
	{

		// if not passed as an argument, we will create the ui thread check based on the context
		if (uiThreadCheck is null) {
			uiThreadCheck = (context.NeedsThreadChecks)
				? EnsureUiThread (context.RootContext.CurrentPlatform)
				: null;
		}

		classBlock.WriteLine ();
		classBlock.AppendMemberAvailability (method.SymbolAvailability);
		classBlock.AppendGeneratedCodeAttribute (optimizable: true);

		// append the export attribute to the method just in case it is a protocol method in a wrapper class,
		// that is when the method is not an extension method and the binding type is protocol.
		if (context.Changes.BindingType == BindingType.Protocol && !method.IsExtension) {
			classBlock.AppendExportAttribute (method.ExportMethodData);
		}

		using (var methodBlock = classBlock.CreateBlock (method.ToDeclaration ().ToString (), block: true)) {
			// write any possible thread check at the beginning of the method
			if (uiThreadCheck is not null) {
				methodBlock.WriteLine (uiThreadCheck.ToString ());
				methodBlock.WriteLine ();
			}

			// retrieve the method invocation via the factory, this will generate the necessary arguments
			// transformations and the invocation
			var invocations = GetInvocations (method);

			if (method.ReturnType.IsVoid) {
				EmitVoidMethodBody (context, method, invocations, methodBlock);
			} else {
				EmitReturnMethodBody (context, method, invocations, methodBlock);
			}
		}

		if (!method.IsAsync)
			return;

		// if the method is an async method, generate its async version
		classBlock.WriteLine ();
		classBlock.AppendMemberAvailability (method.SymbolAvailability);
		classBlock.AppendGeneratedCodeAttribute (optimizable: true);

		var asyncMethod = method.ToAsync ();
		using (var methodBlock = classBlock.CreateBlock (asyncMethod.ToDeclaration ().ToString (), block: true)) {
			// we need to create the tcs for the the async method
			var tcsType = asyncMethod.ReturnType.ToTaskCompletionSource ();
			var tcsName = Nomenclator.GetTaskCompletionSourceName ();
			methodBlock.WriteRaw (
$@"{tcsType.GetIdentifierSyntax ()} {tcsName} = new ();
{ExpressionStatement (ExecuteSyncCall (method))}
return {tcsName}.Task;
");
		}
	}

	/// <summary>
	/// Emit the code for all the methods in the class.
	/// </summary>
	/// <param name="context">The current binding context.</param>
	/// <param name="classBlock">Current class block.</param>
	public static void EmitMethods (this IClassEmitter self, in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		var uiThreadCheck = (context.NeedsThreadChecks)
			? EnsureUiThread (context.RootContext.CurrentPlatform) : null;
		foreach (var method in context.Changes.Methods.OrderBy (m => m.Name)) {
			EmitMethod (self, context, method, classBlock, uiThreadCheck);
		}
	}

	/// <summary>
	/// Emit the code for all the field properties in the class. The code will add any necessary backing fields and
	/// will return all properties that are notifications.
	/// </summary>
	/// <param name="self"></param>
	/// <param name="className">The current class name.</param>
	/// <param name="properties">All properties of the class, the method will filter those that are fields.</param>
	/// <param name="classBlock">Current class block.</param>
	/// <param name="notificationProperties">An immutable array with all the properties that are marked as notifications
	/// and that need a helper class to be generated.</param>
	public static void EmitFields (this IClassEmitter self, string className, in ImmutableArray<Property> properties, TabbedWriter<StringWriter> classBlock,
		out ImmutableArray<Property> notificationProperties)
	{
		var notificationsBuilder = ImmutableArray.CreateBuilder<Property> ();
		foreach (var property in properties.OrderBy (p => p.Name)) {
			if (!property.IsField)
				continue;

			classBlock.WriteLine ();
			// a field should always have a getter, if it does not, we do not generate the property
			var getter = property.GetAccessor (AccessorKind.Getter);
			if (getter.IsNullOrDefault)
				continue;

			// provide a backing variable for the property if and only if we are dealing with a reference type
			if (property.IsReferenceType) {
				classBlock.WriteLine (FieldPropertyBackingVariable (property).ToString ());
			}

			classBlock.WriteLine ();
			classBlock.AppendMemberAvailability (property.SymbolAvailability);
			classBlock.AppendGeneratedCodeAttribute (optimizable: true);
			if (property.IsNotification) {
				// add it to the bucket so that we can later generate the necessary partial class for the 
				// notifications
				notificationsBuilder.Add (property);
				classBlock.AppendNotificationAdvice (className, property.Name);
			}

			using (var propertyBlock = classBlock.CreateBlock (property.ToDeclaration ().ToString (), block: true)) {
				// generate the accessors, we will always have a get, a set is optional depending on the type
				// if the symbol availability of the accessor is different of the one from the property, write it
				var backingField = property.BackingField;

				// be very verbose with the availability, makes the life easier to the dotnet analyzer
				propertyBlock.AppendMemberAvailability (getter.SymbolAvailability);
				using (var getterBlock = propertyBlock.CreateBlock ("get", block: true)) {
					// fields with a reference type have a backing fields, while value types do not
					if (property.IsReferenceType) {
						getterBlock.WriteRaw (
$@"if ({backingField} is null)
	{backingField} = {ExpressionStatement (FieldConstantGetter (property))}
return {backingField};
");
					} else {
						// directly return the call from the getter
						getterBlock.WriteLine ($"return {ExpressionStatement (FieldConstantGetter (property))}");
					}
				}

				var setter = property.GetAccessor (AccessorKind.Setter);
				if (setter.IsNullOrDefault)
					// we are done with the current property
					continue;

				propertyBlock.WriteLine (); // add space between getter and setter since we have the attrs
				propertyBlock.AppendMemberAvailability (setter.SymbolAvailability);
				using (var setterBlock = propertyBlock.CreateBlock ("set", block: true)) {
					if (property.IsReferenceType) {
						// set the backing field
						setterBlock.WriteLine ($"{backingField} = value;");
					}
					// call the native code
					setterBlock.WriteLine ($"{ExpressionStatement (FieldConstantSetter (property, "value"))}");
				}
			}
		}
		notificationProperties = notificationsBuilder.ToImmutable ();
	}
}
