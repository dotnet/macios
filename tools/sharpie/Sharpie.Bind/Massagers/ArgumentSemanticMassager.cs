// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Removes ArgumentSemantic.Copy from property [Export] attributes when the
/// property type is a string; string properties imply ArgumentSemantic.Copy.
/// Also removes UnsafeUnretained/Assign from primitive/builtin types.
/// </summary>
public sealed class ArgumentSemanticMassager : Massager<ArgumentSemanticMassager> {
	public ArgumentSemanticMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitPropertyDeclaration (PropertyDeclaration propertyDeclaration)
	{
		if (HasVisited (propertyDeclaration))
			return;

		MarkVisited (propertyDeclaration);

		var type = propertyDeclaration.ReturnType as PrimitiveType;
		if (type is null)
			return;

		var exportAttr = propertyDeclaration.GetAttributes<ExportAttribute> ().FirstOrDefault ();
		if (exportAttr is null)
			return;

		if (exportAttr.ArgumentSemantic == ArgumentSemantic.UnsafeUnretained ||
			exportAttr.ArgumentSemantic == ArgumentSemantic.Assign ||
			(exportAttr.ArgumentSemantic == ArgumentSemantic.Copy && type.Keyword == "string"))
			exportAttr.RemoveArgumentSemantic ();
	}
}
