// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

[RegisterBefore (typeof (ArgumentSemanticMassager))]
public sealed class NSStringMassager : Massager<NSStringMassager> {
	public NSStringMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitMemberType (MemberType memberType)
	{
		if (HasVisited (memberType))
			return;

		MarkVisited (memberType);
		Massage (memberType);
	}

	public override void VisitSimpleType (SimpleType simpleType)
	{

		if (HasVisited (simpleType))
			return;

		MarkVisited (simpleType);
		Massage (simpleType);
	}

	void Massage (AstType type)
	{
		if (type.Annotation<ObjCInterfaceType> ()?.Interface?.Name == "NSString" &&
			!type.GetParent<EntityDeclaration> ().HasAttribute<FieldAttribute> () &&
			!type.GetParent<EntityDeclaration> ().HasAttribute<Attributes.DllImportAttribute> ())
			type.ReplaceWith (new PrimitiveType ("string"));
	}
}
