// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

public class GroupedAttributeMassager : Massager<GroupedAttributeMassager> {
	public GroupedAttributeMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitMethodDeclaration (MethodDeclaration methodDeclaration)
	{
		VisitEntityDeclaration (methodDeclaration);
	}

	public override void VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration)
	{
		VisitEntityDeclaration (propertyDeclaration);
		base.VisitPropertyDeclaration (propertyDeclaration);
	}

	public override void VisitAccessor (Accessor accessor)
	{
		VisitEntityDeclaration (accessor);
	}

	public override void VisitDelegateDeclaration (DelegateDeclaration delegateDeclaration)
	{
		VisitEntityDeclaration (delegateDeclaration);
	}

	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		VisitEntityDeclaration (typeDeclaration);
		base.VisitTypeDeclaration (typeDeclaration);
	}

	void VisitEntityDeclaration (EntityDeclaration entityDeclaration)
	{
		if (HasVisited (entityDeclaration))
			return;

		MarkVisited (entityDeclaration);

		foreach (var attrGroup in
			from section in entityDeclaration.Attributes
			from attr in section.Attributes
			let groupNumber = GetGroupNumber (attr)
			where groupNumber >= 0
			group attr by groupNumber into section
			orderby section.Key
			select section.ToList ()) {
			var section = new AttributeSection ();
			foreach (var attr in attrGroup.OrderByDescending (attr => attr.Type.ToString ())) {
				section.AttributeTarget = attr.GetParent<AttributeSection> ().AttributeTarget;
				attr.Remove ();
				section.Attributes.Add (attr);
			}
			entityDeclaration.Attributes.Add (section);
		}

		foreach (var attrGroup in entityDeclaration.Attributes) {
			if (attrGroup.Attributes.Count == 0)
				attrGroup.Remove ();
		}
	}

	int GetGroupNumber (ICSharpCode.NRefactory.CSharp.Attribute attr)
	{
		var groupedAttribute = attr as GroupedAttribute;
		if (groupedAttribute is not null)
			return groupedAttribute.GroupNumber;

		var availabilityAttr = attr as AvailabilityBaseAttribute;
		if (availabilityAttr is not null && availabilityAttr.IsShorthand)
			return 0;

		switch (attr.Type.ToString ()) {
		case "Abstract":
		case "Internal":
		case "Model":
		case "Protocol":
		case "Category":
		case "Native":
		case "Static":
			return 1;
		case "BaseType":
		case "Export":
		case "NullAllowed":
			return 2;
		case "DisableDefaultCtor":
		case "DesignatedInitializer":
			return 3;
		default:
			return -1;
		}
	}
}
