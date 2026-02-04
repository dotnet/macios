// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Annotate union structures with [StructLayout] and their members with [FieldOffset].
/// </summary>
public class UnionStructMassager : Massager<UnionStructMassager> {
	public UnionStructMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		if (HasVisited (typeDeclaration))
			return;

		MarkVisited (typeDeclaration);

		base.VisitTypeDeclaration (typeDeclaration);

		var nativeDecl = typeDeclaration.Annotation<RecordDecl> ();
		if (nativeDecl is null || nativeDecl.CursorKind != CXCursorKind.CXCursor_UnionDecl)
			return;

		typeDeclaration.AddAttribute (new Attributes.StructLayoutAttribute (LayoutKind.Explicit));

		foreach (var field in typeDeclaration.Members.OfType<FieldDeclaration> ())
			field.AddAttribute (new Attributes.FieldOffsetAttribute (0));
	}
}
