// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Availability;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Extensions;
using Microsoft.Macios.Generator.Formatters;
using Microsoft.Macios.Generator.IO;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Emitters;

class AsyncResultEmitter (
	TabbedStringBuilder builder) {

	static HashSet<string> DefaultNamespaces { get; } = [
		"System",
		"System.Threading.Tasks",
		"System.Runtime.CompilerServices"
	];

	/// <summary>
	/// Writes the necessary 'using' statements for the generated async result type.
	/// </summary>
	/// <param name="asyncResult">The information about the async result type being generated.</param>
	/// <param name="typeNamespace">The namespace of the generated type.</param>
	void WriteNamespaces (AsyncResultInfo asyncResult, string typeNamespace)
	{
		if (asyncResult.CompletionHandler.Type.Delegate is null)
			return;

		// add the default namespaces
		foreach (var ns in DefaultNamespaces.OrderBy (x => x)) {
			builder.WriteLine ($"using {ns};");
		}

		// collect the namespaces of the parameters of the delegate, use a set to avoid duplicates
		var namespaces = new HashSet<string> ();
		// add missing namespaces
		foreach (var parameter in asyncResult.CompletionHandler.Type.Delegate.Parameters) {
			var ns = string.Join ('.', parameter.Type.Namespace);
			if (!string.IsNullOrEmpty (ns)
				&& ns != typeNamespace // ensure that we do not add a using statement for the same namespace
				&& !DefaultNamespaces.Contains (ns))
				namespaces.Add (ns);
		}

		// sort the namespaces to be consistent and add them
		foreach (var ns in namespaces.OrderBy (x => x)) {
			builder.WriteLine ($"using {ns};");
		}

		// add space for readability
		if (namespaces.Count > 0)
			builder.WriteLine ();
	}

	public bool TryEmit (AsyncResultInfo asyncResult, [NotNullWhen (false)] out ImmutableArray<Diagnostic>? diagnostics)
	{
		diagnostics = null;

		// get the parameters based on the completion handler
		if (asyncResult.CompletionHandler.Type.Delegate is null) {
			diagnostics = [Diagnostic.Create (
				Diagnostics
					.RBI0000, // An unexpected error occurred while processing '{0}'. Please fill a bug report at https://github.com/dotnet/macios/issues/new.
				null,
				asyncResult.FullyQualifiedName)];
			return false;
		}
		// check if we should include the last parameter or not
		var parameters = asyncResult.CompletionHandler.Type.Delegate.Parameters
			.Select (p => p.ToParameter ())
			.ToImmutableArray ();
		if (parameters.Length > 0 && parameters [^1].Type.Name.Contains ("NSError")) {
			parameters = parameters [..^1];
		}

		var typeNamespace = string.Join ('.', asyncResult.Namespace);

		WriteNamespaces (asyncResult, typeNamespace);

		// namespace declaration
		builder.WriteLine ($"namespace {typeNamespace};");
		builder.WriteLine ();

		using (var classBlock =
			   builder.CreateBlock ($"public partial class {asyncResult.Name}", true)) {

			foreach (var parameter in parameters) {
				classBlock.WriteLine ();
				classBlock.WriteLine ($"public {parameter.Type.GetIdentifierSyntax ().ToString ()} {parameter.Name.Capitalize ()} {{ get; set; }}");
			}

			// get the parameter from the delegate type
			classBlock.WriteLine ();
			classBlock.WriteLine ("partial void Initialize ();");

			// create a constructor for the result type 
			var constructor = new Constructor (
				type: asyncResult.Name,
				symbolAvailability: new SymbolAvailability (),
				attributes: [],
				modifiers: [Token (SyntaxKind.PublicKeyword)],
				parameters: parameters);
			classBlock.WriteLine ();
			using (var constructorBlock = classBlock.CreateBlock ($"{constructor.ToDeclaration ()}", true)) {
				foreach (var parameter in parameters) {
					constructorBlock.WriteLine ($"this.{parameter.Name.Capitalize ()} = {parameter.Name};");
				}
				constructorBlock.WriteLine ("Initialize ();");
			}
		}

		return true;
	}
}
