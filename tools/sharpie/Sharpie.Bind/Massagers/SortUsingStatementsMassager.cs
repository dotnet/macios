// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Sorts using statements alphabetically within two groups: "System*" and "everything else"
/// </summary>
public class SortUsingStatementsMassager : Massager<SortUsingStatementsMassager> {
	public SortUsingStatementsMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitSyntaxTree (SyntaxTree syntaxTree)
	{
		if (HasVisited (syntaxTree))
			return;

		MarkVisited (syntaxTree);

		base.VisitSyntaxTree (syntaxTree);

		// collect and detach all using statements from this tree
		var usings = syntaxTree.Members.OfType<UsingDeclaration> ().ToList ();
		usings.ForEach (u => u.Remove ());

		// descending sort
		usings.Sort ((a, b) => {
			if (a.IsSystem () && b.IsSystem ())
				return b.Namespace.Length.CompareTo (a.Namespace.Length);
			if (a.IsSystem () == b.IsSystem ())
				return String.CompareOrdinal (b.Namespace, a.Namespace);
			return a.IsSystem () ? 1 : -1;
		});

		// insert nodes in reverse, separate "System" usings from everything else with a newline
		for (var i = 0; i < usings.Count; i++) {
			syntaxTree.Members.InsertBefore (syntaxTree.FirstChild, usings [i]);
			if (!usings [i].IsSystem () && i < usings.Count - 1 && usings [i + 1].IsSystem ())
				syntaxTree.Members.InsertBefore (syntaxTree.FirstChild, new NewLineNode ());
		}
	}
}
