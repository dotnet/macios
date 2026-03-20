// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

public sealed class ExplicitBaseTypeNameMassager : Massager<ExplicitBaseTypeNameMassager> {
	public ExplicitBaseTypeNameMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitAttribute (ICSharpCode.NRefactory.CSharp.Attribute attribute)
	{
		if (HasVisited (attribute))
			return;

		MarkVisited (attribute);

		if (attribute is BaseTypeAttribute baseTypeAttribute) {
			var nativeDecl = attribute.Annotation<ObjCContainerDecl> ();
			if (nativeDecl is not null)
				baseTypeAttribute.Arguments.Add (
					new NamedExpression (
						"Name",
						new PrimitiveExpression (nativeDecl.Name)));
		}
	}
}
