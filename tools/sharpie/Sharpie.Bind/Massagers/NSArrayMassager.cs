// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Translate NSArray types to NSObject[] types.
/// </summary>
public sealed class NSArrayMassager : Massager<NSArrayMassager> {
	public NSArrayMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitSimpleType (SimpleType simpleType)
	{
		Massage (simpleType);
	}

	public override void VisitMemberType (MemberType memberType)
	{
		Massage (memberType);
	}

	void Massage (AstType type)
	{
		if (HasVisited (type))
			return;

		MarkVisited (type);

		var nsarray = type.Annotation<ObjCObjectType> ();
		if (nsarray?.Interface?.Name == "NSArray") {
			var typeArgs = nsarray.TypeArgs.ToArray () ?? Array.Empty<ClangSharp.Type> ();

			var arrayType = (typeArgs.Length == 1
				? typeArgs [0].Bind (Binder.BindingResult)!
				: AstType.Create ("Foundation.NSObject")).MakeArrayType ();

			type.CopyAnnotationsTo (arrayType);
			type.ReplaceWith (arrayType);

			if (typeArgs.Length == 1)
				return;

			var parentEntity =
				arrayType.GetParent<MethodDeclaration> () ??
				arrayType.GetParent<PropertyDeclaration> () as EntityDeclaration;
			if (parentEntity is not null)
				parentEntity.AddAttribute (new VerifyAttribute (VerifyHint.StronglyTypedNSArray));
		}
	}
}
