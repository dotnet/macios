// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.Context;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Formatters;
using Microsoft.Macios.Generator.IO;
using ObjCBindings;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Constructor = ObjCBindings.Constructor;
using Property = Microsoft.Macios.Generator.DataModel.Property;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emitter for Objective-C classes.
/// </summary>
class ClassEmitter : IClassEmitter {
	/// <inheritdoc />
	public string GetSymbolName (in Binding binding)
	{
		var outerClasses = binding.OuterClasses.Select (x => x.Name);
		var prefix = string.Join ('.', outerClasses);
		return string.IsNullOrEmpty (prefix)
			? binding.Name
			: $"{prefix}.{binding.Name}";
	}

	/// <inheritdoc />
	public IEnumerable<string> UsingStatements => [
		"System",
		"System.Drawing",
		"System.Diagnostics",
		"System.ComponentModel",
		"System.Runtime.Versioning",
		"System.Runtime.InteropServices",
		"System.Diagnostics.CodeAnalysis",
		"ObjCRuntime",
	];

	/// <summary>
	/// Emit the code for all the constructors in the class.
	/// </summary>
	/// <param name="context">The current binding context.</param>
	/// <param name="classBlock">Current class block.</param>
	void EmitConstructors (in BindingContext context, TabbedWriter<StringWriter> classBlock)
	{
		// When dealing with constructors we cannot sort them by name because the name is always the same as the class
		// instead we will sort them by the selector name so that we will always generate the constructors in the same order
		foreach (var constructor in context.Changes.Constructors.OrderBy (c => c.ExportMethodData.Selector)) {
			classBlock.AppendMemberAvailability (constructor.SymbolAvailability);
			classBlock.AppendGeneratedCodeAttribute (optimizable: true);
			if (constructor.ExportMethodData.Flags.HasFlag (Constructor.DesignatedInitializer)) {
				classBlock.AppendDesignatedInitializer ();
			}

			using (var constructorBlock = classBlock.CreateBlock (constructor.ToDeclaration (withBaseNSFlag: true).ToString (), block: true)) {
				// retrieve the method invocation via the factory, this will generate the necessary arguments
				// transformations and the invocation
				var invocations = GetInvocations (constructor);

				// init the needed temp variables
				foreach (var argument in invocations.Arguments) {
					constructorBlock.Write (argument.Validations, verifyTrivia: false);
					constructorBlock.Write (argument.Initializers, verifyTrivia: false);
					constructorBlock.Write (argument.PreCallConversion, verifyTrivia: false);
				}

				// simply call the send or sendSuper accordingly
				constructorBlock.WriteRaw (
$@"if (IsDirectBinding) {{
	{ExpressionStatement (invocations.Send)}
}} else {{
	{ExpressionStatement (invocations.SendSuper)}
}}
");

				// before we leave the methods, do any post operations
				foreach (var argument in invocations.Arguments) {
					constructorBlock.Write (argument.PostCallConversion, verifyTrivia: false);
				}
			}
			classBlock.WriteLine ();
		}
	}

	/// <summary>
	/// Emit the code for all the notifications in the class.
	/// </summary>
	/// <param name="properties">All properties of the class, the method will filter those that are notifications.</param>
	/// <param name="classBlock">Current class block.</param>
	void EmitNotifications (in ImmutableArray<Property> properties, TabbedWriter<StringWriter> classBlock)
	{
		if (properties.Length == 0)
			return;

		// create a nested static class with the notification helpers
		using (var notificationClass = classBlock.CreateBlock ("public static partial class Notifications", true)) {
			notificationClass.WriteLine ();
			// generate two methods per notification
			foreach (var notification in properties) {
				var count = 12; // == "Notification".Length;
				var name = $"Observe{notification.Name [..^count]}";
				var notificationCenter = notification.ExportFieldData.IsNullOrDefault || notification.ExportFieldData.FieldData.NotificationCenter is null
					? $"{NotificationCenter}.DefaultCenter"
					: notification.ExportFieldData.FieldData.NotificationCenter;
				var eventType = notification.ExportFieldData.IsNullOrDefault || notification.ExportFieldData.FieldData.Type is null
					? NSNotificationEventArgs.ToString ()
					: notification.ExportFieldData.FieldData.Type;
				// use the raw writer which makes it easier to read in this case
				notificationClass.WriteRaw (
@$"public static {NSObject} {name} ({EventHandler}<{eventType}> handler)
{{
	return {notificationCenter}.AddObserver ({notification.Name}, notification => handler (null, new {eventType} (notification)));
}}

public static NSObject {name} ({NSObject} objectToObserve, {EventHandler}<{eventType}> handler)
{{
	return {notificationCenter}.AddObserver ({notification.Name}, notification => handler (null, new {eventType} (notification)), objectToObserve);
}}

"
);
			}
		}
	}

	/// <summary>
	/// Emit the events for the given delegates.
	/// </summary>
	/// <param name="delegates">The delegate properties.</param>
	/// <param name="classBlock">Current class block.</param>
	void EmitEvents (in ImmutableArray<Property> delegates, TabbedWriter<StringWriter> classBlock)
	{
		// loop over the delegates
		foreach (var property in delegates) {
			// see if we have a strong type
			if (property.ExportPropertyData.StrongDelegateType.IsNullOrDefault)
				continue;

			// loop over the events, those should be present in the property for the delegate
			foreach (var eventInfo in property.ExportPropertyData.StrongDelegateType.Events) {
				// create the event args type name
				var eventHandler = eventInfo.EventArgsType is null
					? EventHandler.ToString ()
					: $"{EventHandler}<{eventInfo.EventArgsType}>";
				classBlock.WriteLine ($"// Generate event for delegate: {eventInfo.Name} with args: {eventHandler}");
			}
		}
	}

	/// <inheritdoc />
	public bool TryEmit (in BindingContext bindingContext, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;
		if (bindingContext.Changes.BindingType != BindingType.Class) {
			diagnostics = [Diagnostic.Create (
					Diagnostics
						.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
					null,
					bindingContext.Changes.FullyQualifiedSymbol)];
			return false;
		}

		// namespace declaration
		this.EmitNamespace (bindingContext);

		using (var _ = this.EmitOuterClasses (bindingContext, out var builder)) {

			// register the class only if we are not dealing with a static class
			var bindingData = (BindingTypeData<Class>) bindingContext.Changes.BindingInfo;
			// Registration depends on the class name. If the binding data contains a name, we use that one, else
			// we use the name of the class
			var registrationName = bindingData.Name ?? bindingContext.Changes.Name;

			// append the class availability, this will add the necessary attributes to the class
			bindingContext.Builder.AppendMemberAvailability (bindingContext.Changes.SymbolAvailability);
			if (!bindingContext.Changes.IsStatic) {
				builder.WriteLine ($"[Register (\"{registrationName}\", true)]");
			}

			var modifiers = $"{string.Join (' ', bindingContext.Changes.Modifiers)} ";
			using (var classBlock =
				   builder.CreateBlock (
					   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {bindingContext.Changes.Name}",
					   true)) {
				// emit the fields for the selectors before we register the class or anything
				this.EmitSelectorFields (bindingContext, classBlock);

				if (!bindingContext.Changes.IsStatic) {
					classBlock.AppendGeneratedCodeAttribute (optimizable: true);
					classBlock.WriteLine (
						$"static readonly {NativeHandle} {ClassPtr} = {BindingSyntaxFactory.Class}.GetHandle (\"{registrationName}\");");
					classBlock.WriteLine ();
					classBlock.WriteDocumentation (Documentation.Class.ClassHandle (bindingContext.Changes.Name));
					classBlock.WriteLine ($"public override {NativeHandle} ClassHandle => {ClassPtr};");
					classBlock.WriteLine ();

					this.EmitDefaultNSObjectConstructors (className: bindingContext.Changes.Name,
						classBlock: classBlock,
						disableDefaultCtor: bindingData.Flags.HasFlag (ObjCBindings.Class.DisableDefaultCtor));

					// emit any other constructor that is not the default one
					EmitConstructors (bindingContext, classBlock);
				}

				this.EmitFields (bindingContext.Changes.Name, bindingContext.Changes.Properties, classBlock,
					out var notificationProperties);
				this.EmitProperties (bindingContext, classBlock, out var strongDelegates);
				this.EmitMethods (bindingContext, classBlock);

				// emit the events for the delegates
				EmitEvents (strongDelegates, classBlock);

				// emit the notification helper classes, leave this for the very bottom of the class
				EmitNotifications (notificationProperties, classBlock);
			}

			return true;
		}
	}
}
