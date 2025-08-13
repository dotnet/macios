// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.IO;

namespace Microsoft.Macios.Generator.Emitters;

/// <summary>
/// Emits event argument types for weak delegates.
/// </summary>
/// <param name="builder">The tabbed string builder to use.</param>
class EventTypeEmitter (
	TabbedStringBuilder builder) {

	/// <summary>
	/// Tries to emit the event argument type.
	/// </summary>
	/// <param name="eventInfo">The event info.</param>
	/// <param name="diagnostics">A list of diagnostics if the emission fails.</param>
	/// <returns>True if the event argument type was emitted, false otherwise.</returns>
	public bool TryEmit (EventInfo eventInfo, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;

		// get the parameters based on the completion handler
		if (eventInfo.EventArgsType is null) {
			diagnostics = [
				Diagnostic.Create (
					Diagnostics
						.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
					null,
					eventInfo.Name)
			];
			return false;
		}

		// emit the using statements
		foreach (var ns in eventInfo.Usings.OrderBy (x => x)) {
			builder.WriteLine ($"using {ns};");
		}
		// readability
		if (eventInfo.Usings.Length > 0)
			builder.WriteLine ();

		builder.WriteLine ($"namespace {eventInfo.Namespace};");
		builder.WriteLine ();

		var argsClassName = eventInfo.EventArgsType.EndsWith ("EventArgs", StringComparison.Ordinal)
			? eventInfo.EventArgsType
			: $"{eventInfo.EventArgsType}EventArgs";

		using (var classBlock =
			   builder.CreateBlock ($"public partial class {argsClassName}", true)) {

			// emit a property per parameter
			foreach (var (name, type) in eventInfo.MethodParameters) {
				classBlock.WriteLine ($"public {type} {name.Capitalize ()} {{ get; set; }}");
				classBlock.WriteLine ();
			}

			// emit a constructor that takes all parameters
			using (var constructorBlock = classBlock.CreateBlock (
					   $"public {argsClassName} ({string.Join (", ", eventInfo.MethodParameters.Select (p => $"{p.Type} {p.Name}"))})",
					   true)) {

				// for each parameter, assign it to the property
				foreach (var (name, type) in eventInfo.MethodParameters) {
					constructorBlock.WriteLine ($"this.{name.Capitalize ()} = {name};");
				}
			}
		}

		return true;
	}
}
