// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Remove the 'Attribute' suffix on type names in an Attribute context.
/// </summary>
public sealed class AttributeMassager : Massager<AttributeMassager> {
	public AttributeMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	[return: NotNullIfNotNull (nameof (name))]
	static string? RemoveSuffix (string? name)
	{
		const string attr = "Attribute";
		if (name is not null &&
			name.Length > attr.Length &&
			name.EndsWith (attr, System.StringComparison.Ordinal))
			return name.Substring (0, name.Length - attr.Length);
		return name;
	}

	public override void VisitAttribute (ICSharpCode.NRefactory.CSharp.Attribute attribute)
	{
		if (HasVisited (attribute))
			return;

		MarkVisited (attribute);

		var simpleType = attribute.Type as SimpleType;
		if (simpleType is not null) {
			simpleType.Identifier = RemoveSuffix (simpleType.Identifier);
			return;
		}

		var memberType = attribute.Type as MemberType;
		if (memberType is not null) {
			memberType.MemberName = RemoveSuffix (memberType.MemberName);
			return;
		}
	}
}
