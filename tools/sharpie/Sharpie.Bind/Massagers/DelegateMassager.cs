// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Wraps strongly typed delegate properties with weak NSObject delegates.
/// </summary>
public sealed class DelegateMassager : Massager<DelegateMassager> {
	public DelegateMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitPropertyDeclaration (PropertyDeclaration strongDelegate)
	{
		if (HasVisited (strongDelegate))
			return;

		MarkVisited (strongDelegate);

		var parent = strongDelegate.Parent as TypeDeclaration;

		if (parent is null ||
			strongDelegate?.Name is null ||
			!(strongDelegate.Name.EndsWith ("Delegate", StringComparison.OrdinalIgnoreCase) ||
			strongDelegate.Name.EndsWith ("DataSource", StringComparison.OrdinalIgnoreCase)))
			return;

		var weakDelegate = (PropertyDeclaration) strongDelegate.Clone ();

		weakDelegate.Name = "Weak" + weakDelegate.Name;
		weakDelegate.ReturnType = AstType.Create ("Foundation.NSObject");
		weakDelegate.AddAttribute (new NullAllowedAttribute ());

		strongDelegate.RemoveAnnotations<Decl> ();
		strongDelegate.RemoveAttributesOfType<ExportAttribute> ();
		strongDelegate.AddAttribute (new WrapAttribute (weakDelegate.Name));

		MarkVisited (weakDelegate);
		parent.Members.InsertAfter (strongDelegate, weakDelegate);
	}
}
