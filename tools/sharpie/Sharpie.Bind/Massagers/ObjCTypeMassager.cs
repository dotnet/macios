// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind.Attributes;
using Sharpie.Bind.Types;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Massage Objective-C special types 'instancetype', 'id', 'Class', and 'SEL' to Xamarin API types.
/// </summary>
public class ObjCTypeMassager : Massager<ObjCTypeMassager> {
	public ObjCTypeMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitPrimitiveType (PrimitiveType primitiveType)
	{
		if (HasVisited (primitiveType))
			return;

		MarkVisited (primitiveType);

		AnnotateGenerics (primitiveType);

		AstType? newType = null;
		var typeDeclaration = primitiveType.GetParent<TypeDeclaration> ()?.Annotation<object> ();

		if (primitiveType is ObjCInstanceTypeType) {
			var method = primitiveType.Parent as MethodDeclaration;
			if (method is not null) {
				var type = method.Parent as TypeDeclaration;
				if (type is not null) {
					var baseType = type.GetAttributes<BaseTypeAttribute> ().FirstOrDefault ();
					if (baseType is not null && type.HasAttribute<CategoryAttribute> ())
						primitiveType.ReplaceWith (newType = baseType.BaseType!.Clone ());
					else
						primitiveType.ReplaceWith (newType = new SimpleType (type.Name));
				}
			}

			// the native instancetype node is just a typedef that's checked
			// during Sema by clang and replaces the node with the defining type,
			// so we need to do the same thing here that Clang does so we can later
			// navigate from the instancetype to the defining decl
			if (newType is not null && typeDeclaration is not null) {
				newType.RemoveAnnotations<object> ();
				newType.AddAnnotation (typeDeclaration);
			}
		}
	}

	public override void VisitSimpleType (SimpleType simpleType)
	{
		if (HasVisited (simpleType))
			return;

		MarkVisited (simpleType);

		AnnotateGenerics (simpleType);

		AstType newType;

		var idType = simpleType as ObjCIdType;
		if (idType is not null)
			newType = AstType.Create ("Foundation.NSObject");
		else if (simpleType is ObjCClassType)
			newType = AstType.Create ("ObjCRuntime.Class");
		else if (simpleType is ObjCSelectorType)
			newType = AstType.Create ("ObjCRuntime.Selector");
		else
			return;

		if (simpleType.TypeArguments.Count == 1)
			newType = simpleType.TypeArguments.FirstOrNullObject ();
		else
			simpleType.TypeArguments.MoveTo (((MemberType) newType).TypeArguments);

		simpleType.CopyAnnotationsTo (newType);
		simpleType.ReplaceWith (newType);
	}

	void AnnotateGenerics (AstType type)
	{
		var tpDecl = type.Annotation<ObjCTypeParamDecl> ();
		if (tpDecl is null)
			return;

		var parentDecl = type.GetParent<EntityDeclaration> ();
		if (parentDecl is null)
			return;

		if ((from section in parentDecl.Attributes
			 from attr in section.Attributes.OfType<VerifyAttribute> ()
			 where attr.Annotation<ObjCTypeParamDecl> () == tpDecl
			 select attr).FirstOrDefault () is not null)
			return;

		var writer = new StringWriter ();
		var generator = new Clang.NativeCodeGenerator (Binder, writer);
		tpDecl.Accept (generator);

		var verifyAttr = new VerifyAttribute (VerifyHint.GenericTypeParameter, writer.ToString ().Trim ());
		verifyAttr.AddAnnotation (tpDecl);
		parentDecl.AddAttribute (verifyAttr);
	}
}
