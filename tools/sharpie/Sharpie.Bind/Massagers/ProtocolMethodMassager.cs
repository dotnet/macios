// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// A common convention with protocols is that all of the first selector slots
/// have the same name; detect this pattern and rename the bound CIL method to
/// the second selector slot.
/// </summary>
public sealed class ProtocolMethodMassager : Massager<ProtocolMethodMassager> {
	public ProtocolMethodMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		if (HasVisited (typeDeclaration))
			return;

		MarkVisited (typeDeclaration);

		if (!typeDeclaration.HasAttribute<ProtocolAttribute> ())
			return;

		var firstMethod = typeDeclaration.Members.OfType<MethodDeclaration> ().FirstOrDefault ();
		if (!typeDeclaration.Members.OfType<MethodDeclaration> ().All (
			m => m.Name == firstMethod?.Name && m.ReturnType.ToString () == firstMethod?.ReturnType?.ToString ()))
			return;

		foreach (var method in typeDeclaration.Members.OfType<MethodDeclaration> ()) {
			var nativeDecl = method.Annotation<ObjCMethodDecl> ();
			if (nativeDecl is not null && nativeDecl.GetSelector ().ArgumentCount > 1)
				method.Name = nativeDecl.GetSelector ().GetNameForSlot (1).UCFirst ();
		}
	}
}
