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

		// For enums, the availability attributes may be attached to the typedef that names
		// the enum (e.g. `typedef NS_ENUM(NSInteger, Foo) { ... } API_UNAVAILABLE(maccatalyst);`)
		// rather than to the enum declaration itself. BindingGenerator.VisitTypedefDecl links
		// that typedef to the enum, so include its attributes here as well.
		IEnumerable<Attr> attrs = decl.Attrs;
		var typedef = decl.GetAnnotations<TypedefDecl> ().FirstOrDefault ();
		if (typedef is not null)
			attrs = attrs.Concat (typedef.Attrs);

		if (attrs.IsUnavailableAttr ()) {
			entity.Remove ();
			return;
		}

		foreach (var attr in attrs.GetAvailabilityAttributes ().SelectMany (AvailabilityBaseAttribute.FromClang))
			entity.AddAttribute (attr);
	}
}
