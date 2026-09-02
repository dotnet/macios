// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Moves all members to a namespace
/// </summary>
public sealed class NamespaceMassager : Massager<NamespaceMassager> {
	readonly string @namespace;

	public NamespaceMassager (ObjectiveCBinder binder, string @namespace)
		: base (binder)
	{
		if (@namespace is null)
			throw new ArgumentNullException ("namespace");

		if (String.IsNullOrWhiteSpace (@namespace))
			throw new ArgumentException ("empty or white space", "namespace");

		this.@namespace = @namespace;
	}

	public override void VisitSyntaxTree (SyntaxTree syntaxTree)
	{
		if (HasVisited (syntaxTree))
			return;

		MarkVisited (syntaxTree);

		var namespaceDeclaration = new NamespaceDeclaration (@namespace);

		var usings = new List<UsingDeclaration> ();
		foreach (var usingDecl in syntaxTree.Members.OfType<UsingDeclaration> ()) {
			usingDecl.Remove ();
			usings.Add (usingDecl);
		}

		syntaxTree.Members.MoveTo (namespaceDeclaration.Members);

		foreach (var usingDecl in usings)
			syntaxTree.Members.Add (usingDecl);

		syntaxTree.Members.Add (namespaceDeclaration);
	}
}
