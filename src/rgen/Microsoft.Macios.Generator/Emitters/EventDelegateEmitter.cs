// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.IO;

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
			}
		}

		return true;
	}

}
