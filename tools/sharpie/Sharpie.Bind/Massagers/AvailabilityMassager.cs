// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

public sealed class AvailabilityMassager : Massager<AvailabilityMassager> {
	public AvailabilityMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		Massage (typeDeclaration);
		base.VisitTypeDeclaration (typeDeclaration);
	}

	public override void VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration)
	{
		Massage (propertyDeclaration);
		base.VisitPropertyDeclaration (propertyDeclaration);
	}

	public override void VisitMethodDeclaration (MethodDeclaration methodDeclaration)
	{
		Massage (methodDeclaration);
		base.VisitMethodDeclaration (methodDeclaration);
	}

	void Massage (EntityDeclaration entity)
	{
		if (HasVisited (entity))
			return;

		MarkVisited (entity);

		var decl = entity.Annotation<Decl> ();
		if (decl is null)
			return;

		if (decl.Attrs.IsUnavailableAttr ()) {
			entity.Remove ();
			return;
		}

		foreach (var attr in decl.Attrs.GetAvailabilityAttributes ().SelectMany (AvailabilityBaseAttribute.FromClang))
			entity.AddAttribute (attr);
	}
}
