// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.IO;
using Method = ObjCBindings.Method;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emits event delegates for weak delegates.
/// </summary>
/// <param name="builder">The tabbed string builder to use.</param>
class EventDelegateEmitter (
	TabbedStringBuilder builder) {

	/// <summary>
	/// Emits the default constructor for the delegate class.
	/// </summary>
	/// <param name="classBlock">The current class block.</param>
	/// <param name="delegateClassName">The name of the delegate class.</param>
	static void EmitDefaultConstructor (TabbedWriter<StringWriter> classBlock, string delegateClassName)
	{
		classBlock.WriteLine ($"public {delegateClassName} () {{ IsDirectBinding = false; }}");
	}

	/// <summary>
	/// Emits the methods for the delegate class.
	/// </summary>
	/// <param name="delegateInfo">The event delegate info.</param>
	/// <param name="classBlock">The current class block.</param>
	static void EmitMethods (in EventDelegateInfo delegateInfo, TabbedWriter<StringWriter> classBlock)
	{
		foreach (var eventInfo in delegateInfo.DelegateType.Events) {
			var argsClassName = eventInfo.EventArgsType;
			var handlerName = eventInfo.Name.Decapitalize ();
			if (argsClassName is null) {
				classBlock.WriteLine ($"internal {EventHandler}? {handlerName};");
			} else {
				classBlock.WriteLine ($"internal {EventHandler}<{argsClassName}>? {handlerName};");
			}
			classBlock.AppendPreserveAttribute ();
			classBlock.AppendExportAttribute (new ExportData<Method> (eventInfo.MethodSelector));
			using (var methodBlock = classBlock.CreateBlock (eventInfo.MethodSignature, true)) {
				if (eventInfo.EventArgsType is null) {
					methodBlock.WriteRaw (
$@"var handler = {handlerName};
if (handler is not null) {{
	handler ({eventInfo.MethodParameters [0].Name}, {EventHandler}.Empty);
}}
");
				} else {
					// handle tuple types differently
					var argsConstructor = eventInfo.EventArgsIsTuple
						? $"var args = ({string.Join (", ", eventInfo.EventArgParameters.Select (p => p.Name))});"
						: $"{argsClassName} args = new ({string.Join (", ", eventInfo.EventArgParameters.Select (p => p.Name))});";

					methodBlock.WriteRaw (
$@"var handler = {handlerName};
if (handler is not null) {{
	{argsConstructor}
	handler ({eventInfo.MethodParameters [0].Name}, args);
}}
");
				}
			}

			classBlock.WriteLine ();
		}
	}

	/// <summary>
	/// Tries to emit the event delegate.
	/// </summary>
	/// <param name="delegateInfo">The event delegate info.</param>
	/// <param name="diagnostics">A list of diagnostics if the emission fails.</param>
	/// <returns>True if the event delegate was emitted, false otherwise.</returns>
	public bool TryEmit (EventDelegateInfo delegateInfo, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;

		// emit the using statements
		foreach (var ns in delegateInfo.Usings.OrderBy (x => x)) {
			builder.WriteLine ($"using {ns};");
		}
		// readability
		if (delegateInfo.Usings.Length > 0)
			builder.WriteLine ();

		builder.WriteLine ($"namespace {delegateInfo.Namespace};");
		builder.WriteLine ();

		var delegateClassName = Nomenclator.GetInternalDelegateForEventName (delegateInfo.DelegateType);

		// because the delegate is internal and has an other class, we need to nest it
		var modifiers = $"{string.Join (' ', delegateInfo.OuterClassModifiers)} ";
		using (var outerClass =
			   builder.CreateBlock (
				   $"{(string.IsNullOrWhiteSpace (modifiers) ? string.Empty : modifiers)}class {delegateInfo.OuterClassName}",
				   true)) {
			outerClass.AppendRegisterAttribute ();
			// actual delegate class to be used for event internally
			using (var classBlock = outerClass.CreateBlock ($"internal class {delegateClassName}: NSObject, {delegateInfo.DelegateType.Name}", true)) {
				// emit the constructors
				EmitDefaultConstructor (classBlock, delegateClassName);
				classBlock.WriteLine ();

				EmitMethods (delegateInfo, classBlock);
			}
		}

		return true;
	}
}
