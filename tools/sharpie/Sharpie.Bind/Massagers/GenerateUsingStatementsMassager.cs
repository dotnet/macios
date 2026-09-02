// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Types;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Generates using statements based on types referenced in the tree and
/// rewrites the referenced types without the namespace part.
/// </summary>
[RegisterAfterAttribute (typeof (ObjCTypeMassager))]
public class GenerateUsingStatementsMassager : Massager<GenerateUsingStatementsMassager> {
	public GenerateUsingStatementsMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitPrimitiveType (PrimitiveType primitiveType)
	{
		if (HasVisited (primitiveType))
			return;

		MarkVisited (primitiveType);

		var tree = primitiveType.GetParent<SyntaxTree> ();
		if (tree is null)
			return;

		if (primitiveType is NIntType || primitiveType is NUIntType || primitiveType is NFloatType)
			InsertUsingStatement (tree, primitiveType, "System");
	}

	public override void VisitMemberType (MemberType memberType)
	{
		if (HasVisited (memberType))
			return;

		MarkVisited (memberType);

		if (memberType.Parent is NamespaceDeclaration)
			return;

		var tree = memberType.GetParent<SyntaxTree> ();
		if (tree is null)
			return;

		// it seems generic type arguments are not visited automatically
		foreach (var typeArg in memberType.TypeArguments.OfType<MemberType> ())
			typeArg.AcceptVisitor (this);

		var newType = new SimpleType (memberType.MemberName);
		memberType.CopyAnnotationsTo (newType);
		memberType.TypeArguments.MoveTo (newType.TypeArguments);
		memberType.ReplaceWith (newType);

		InsertUsingStatement (tree, newType, memberType.Target.ToString ());
	}

	void InsertUsingStatement (SyntaxTree tree, AstType type, string ns)
	{
		var parentNs = type.GetParent<NamespaceDeclaration> ();

		if (tree.Members.OfType<UsingDeclaration> ().All (u => u.Namespace != ns) &&
			parentNs?.FullName != ns)
			tree.Members.InsertBefore (tree.FirstChild, new UsingDeclaration (ns));
	}
}
