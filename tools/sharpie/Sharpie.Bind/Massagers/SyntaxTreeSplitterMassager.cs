// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

[RegisterBefore (typeof (GenerateUsingStatementsMassager))]
public sealed class SyntaxTreeSplitterMassager : Massager<SyntaxTreeSplitterMassager> {
	readonly DocumentSyntaxTree api = new DocumentSyntaxTree ("ApiDefinition.cs");
	readonly DocumentSyntaxTree structsAndEnums = new DocumentSyntaxTree ("StructsAndEnums.cs");

	public SyntaxTreeSplitterMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		if (HasVisited (typeDeclaration))
			return;

		MarkVisited (typeDeclaration);

		typeDeclaration.Remove ();

		if (typeDeclaration.ClassType == ClassType.Interface)
			api.Members.Add (typeDeclaration);
		else
			structsAndEnums.Members.Add (typeDeclaration);
	}

	public override void VisitDelegateDeclaration (DelegateDeclaration delegateDeclaration)
	{
		if (HasVisited (delegateDeclaration))
			return;

		MarkVisited (delegateDeclaration);

		delegateDeclaration.Remove ();
		api.Members.Add (delegateDeclaration);
	}

	public override void VisitSyntaxTree (SyntaxTree syntaxTree)
	{
		base.VisitSyntaxTree (syntaxTree);

		if (syntaxTree.Members.Count > 0) {
			Console.Error.WriteLine (syntaxTree);
			throw new Exception ("original SyntaxTree should be empty");
		}

		if (api.Members.Count > 0)
			syntaxTree.Members.Add (api);

		if (structsAndEnums.Members.Count > 0)
			syntaxTree.Members.Add (structsAndEnums);
	}
}
