// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind;

static class NRefactoryExtensions {
	public static void CopyAnnotationsTo (this AstNode sourceNode, AstNode targetNode)
	{
		foreach (var annotation in sourceNode.Annotations)
			targetNode.AddAnnotation (annotation);
	}

	public static void AddAttribute (this EntityDeclaration entity, ICSharpCode.NRefactory.CSharp.Attribute attribute)
	{
		entity.Attributes.Add (new AttributeSection (attribute));
	}

	public static void RemoveAttributesOfType<T> (this EntityDeclaration entity) where T : ICSharpCode.NRefactory.CSharp.Attribute
	{
		foreach (var attrSection in entity.Attributes.ToList ()) {
			foreach (var attr in attrSection.Attributes.OfType<T> ().ToList ())
				attrSection.Attributes.Remove (attr);

			if (attrSection.Attributes.Count == 0)
				entity.Attributes.Remove (attrSection);
		}
	}

	public static bool HasAttribute<T> (this EntityDeclaration entity) where T : ICSharpCode.NRefactory.CSharp.Attribute
	{
		return (
			from section in entity.Attributes
			from attr in section.Attributes
			where attr is T
			select attr
		).Any ();
	}

	public static IEnumerable<T> GetAttributes<T> (this EntityDeclaration entity) where T : ICSharpCode.NRefactory.CSharp.Attribute
	{
		return (from section in entity.Attributes from attr in section.Attributes select attr).OfType<T> ();
	}

	public static AstType ToAstType (this System.Type type)
	{
		return AstType.Create (type.FullName);
	}

	public static bool IsSystem (this UsingDeclaration usingDeclaration)
	{
		var name = usingDeclaration.Namespace ?? usingDeclaration.Import.ToString ();
		if (string.IsNullOrEmpty (name))
			return false;

		return name == "System" || name.StartsWith ("System.", System.StringComparison.Ordinal);
	}

	public static void AddLinkAnnotation (this AstNode node, object clangNode)
	{
		node.AddAnnotation ((object) clangNode);
		clangNode.AddAnnotation (node);
	}

	public static bool ReturnsVoid (this MethodDeclaration method)
	{
		return (method?.ReturnType as PrimitiveType)?.Keyword == "void";
	}
}
