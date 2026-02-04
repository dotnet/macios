// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind.Attributes;
using Sharpie.Bind.Types;

namespace Sharpie.Bind.Massagers;

public sealed class MethodToPropertyMassager : Massager<MethodToPropertyMassager> {
	public MethodToPropertyMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitMethodDeclaration (MethodDeclaration methodDeclaration)
	{
		if (HasVisited (methodDeclaration))
			return;

		MarkVisited (methodDeclaration);

		if (methodDeclaration is ObjCConstructorMethodDeclaration)
			return;

		var nativeDecl = methodDeclaration.Annotation<ObjCMethodDecl> ();
		if (nativeDecl is null)
			return;

		if (methodDeclaration.Parameters.Count > 0 || methodDeclaration.ReturnsVoid ())
			return;

		var returnType = methodDeclaration.ReturnType;
		if (returnType is ObjCInstanceTypeType)
			return;

		returnType.Remove ();

		var propertyDeclaration = new PropertyDeclaration {
			Name = methodDeclaration.Name,
			ReturnType = returnType,
			Getter = new Accessor ()
		};

		methodDeclaration.Attributes.MoveTo (propertyDeclaration.Attributes);

		propertyDeclaration.AddLinkAnnotation (nativeDecl);

		if (propertyDeclaration.Name.StartsWith ("Get") &&
			propertyDeclaration.Name.Length >= 4 &&
			Char.IsUpper (propertyDeclaration.Name [3]))
			propertyDeclaration.Name = propertyDeclaration.Name.Substring (3);

		propertyDeclaration.AddAttribute (new VerifyAttribute (VerifyHint.MethodToProperty));

		methodDeclaration.ReplaceWith (propertyDeclaration);

		var parentType = propertyDeclaration.Parent as TypeDeclaration;
		if (parentType is null)
			return;

		// try to match a compatible setter method
		foreach (var setterMethodDeclaration in parentType.Members.OfType<MethodDeclaration> ()) {
			var nativeSetterDecl = setterMethodDeclaration.Annotation<ObjCMethodDecl> ();
			if (nativeSetterDecl is null)
				continue;

			if (setterMethodDeclaration.Parameters.Count == 1 &&
				setterMethodDeclaration.ReturnsVoid () &&
				nativeSetterDecl.Name == "set" + nativeDecl.Name.UCFirst () + ":" &&
				setterMethodDeclaration.Parameters.First ().Type.ToString () == returnType.ToString ()) {
				setterMethodDeclaration.Remove ();
				propertyDeclaration.Setter = new Accessor ();
				propertyDeclaration.AddLinkAnnotation (nativeSetterDecl);
				break;
			}
		}
	}
}
