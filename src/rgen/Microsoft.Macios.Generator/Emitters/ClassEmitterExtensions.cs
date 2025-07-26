// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Linq;
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
			if (getter is not null) {
				var selector = getter.Value.GetSelector (property)!;
				var selectorName = selector.GetSelectorFieldName ();
				if (bindingContext.SelectorNames.TryAdd (selector, selectorName)) {
					EmitField (selector, selectorName);
				}
			}

			var setter = property.GetAccessor (AccessorKind.Setter);
			if (setter is not null) {
				var selector = setter.Value.GetSelector (property)!;
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
	/// <param name="method">The method for which to generate the body.</param>
	/// <param name="invocations">The method invocations and argument transformations.</param>
	/// <param name="methodBlock">The writer for the method block.</param>
	static void EmitVoidMethodBody (in Method method, in MethodInvocations invocations, TabbedWriter<StringWriter> methodBlock)
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

		if (method.IsExtension) {
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
	static void EmitReturnMethodBody (in Method method, in MethodInvocations invocations, TabbedWriter<StringWriter> methodBlock)
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

		if (method.IsExtension) {
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
	/// Emit the code for all the methods in the class.
	/// </summary>
	/// <param name="context">The current binding context.</param>
	/// <param name="classBlock">Current class block.</param>
	public static void EmitMethods (this IClassEmitter self, in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		var uiThreadCheck = (context.NeedsThreadChecks)
			? EnsureUiThread (context.RootContext.CurrentPlatform) : null;
		foreach (var method in context.Changes.Methods.OrderBy (m => m.Name)) {
			classBlock.WriteLine ();
			classBlock.AppendMemberAvailability (method.SymbolAvailability);
			classBlock.AppendGeneratedCodeAttribute (optimizable: true);

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
					EmitVoidMethodBody (method, invocations, methodBlock);
				} else {
					EmitReturnMethodBody (method, invocations, methodBlock);
				}
			}

			if (!method.IsAsync)
				continue;

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
	}

}
