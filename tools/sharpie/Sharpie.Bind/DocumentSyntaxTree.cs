// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind;

public class DocumentSyntaxTree : SyntaxTree {
	public DocumentSyntaxTree (string? fileName = null)
	{
		FileName = fileName;
	}

	public IEnumerable<DocumentSyntaxTree> AllDocuments {
		get {
			if (Members.All (child => child is DocumentSyntaxTree)) {
				foreach (var child in Members)
					yield return (DocumentSyntaxTree) child;
			} else {
				yield return this;
			}
		}
	}

	public override void AcceptVisitor (IAstVisitor visitor)
	{
		foreach (var document in AllDocuments)
			visitor.VisitSyntaxTree (document);
	}
}
