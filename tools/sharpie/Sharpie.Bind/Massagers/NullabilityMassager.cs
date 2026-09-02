// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

public sealed class NullabilityMassager : Massager<NullabilityMassager> {
	public NullabilityMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitMethodDeclaration (MethodDeclaration methodDeclaration)
	{
		if (HasVisited (methodDeclaration))
			return;

		MarkVisited (methodDeclaration);

		if (IsNullAllowed (methodDeclaration.ReturnType, methodDeclaration.Annotation<NamedDecl> ()))
			methodDeclaration.Attributes.Add (new AttributeSection (
				// override the group number so GroupedAttributeMassager
				// hoists [return: NullAllowed] out of default group 2
				// and puts it closest to the method signature
				new NullAllowedAttribute { GroupNumber = 10000 }
			) {
				AttributeTarget = "return"
			});

		// for VisitParameterDeclaration below to be invoked
		base.VisitMethodDeclaration (methodDeclaration);
	}

	public override void VisitParameterDeclaration (ParameterDeclaration parameterDeclaration)
	{
		if (HasVisited (parameterDeclaration))
			return;

		MarkVisited (parameterDeclaration);

		if (IsNullAllowed (parameterDeclaration.Type, parameterDeclaration.Annotation<NamedDecl> ()))
			parameterDeclaration.Attributes.Add (new AttributeSection (new NullAllowedAttribute ()));
	}

	public override void VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration)
	{
		if (HasVisited (propertyDeclaration))
			return;

		MarkVisited (propertyDeclaration);

		if (IsNullAllowed (propertyDeclaration.ReturnType, propertyDeclaration.Annotation<NamedDecl> ()))
			propertyDeclaration.AddAttribute (new NullAllowedAttribute ());
	}

	static bool IsNullAllowed (AstType type, NamedDecl decl)
	{
		var attributedTypes = type?.Annotations.OfType<AttributedType> ();

		if (attributedTypes?.Any (a => a.AttrKind == CX_AttrKind.CX_AttrKind_ObjCKindOf) == true) {
			// When __kindof is present we need to dig for nullability information using the AstContext
			return decl switch {
				ObjCMethodDecl metDecl => metDecl.ReturnType.Handle.Nullability == CXTypeNullabilityKind.CXTypeNullability_Nullable,
				ObjCPropertyDecl pDecl => pDecl.Type.Handle.Nullability == CXTypeNullabilityKind.CXTypeNullability_Nullable,
				ParmVarDecl paramvDecl => paramvDecl.Type.Handle.Nullability == CXTypeNullabilityKind.CXTypeNullability_Nullable,
				_ => false
			};
		}

		return attributedTypes?.Any (a => a.AttrKind == CX_AttrKind.CX_AttrKind_TypeNullable) == true;
	}
}
