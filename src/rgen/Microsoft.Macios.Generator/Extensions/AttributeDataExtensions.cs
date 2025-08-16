using System;
using Microsoft.CodeAnalysis;

namespace Microsoft.Macios.Generator.Extensions;

/// <summary>
/// Extension methods for <see cref="AttributeData"/>.
/// </summary>
public static class AttributeDataExtensions {

	/// <summary>
	/// Gets the location of the attribute in source code.
	/// </summary>
	/// <param name="attributeData">The attribute data.</param>
	/// <returns>The location of the attribute, or <c>null</c> if it can't be determined.</returns>
	public static Location? GetLocation (this AttributeData attributeData)
	{
		var syntaxReference = attributeData.ApplicationSyntaxReference;
		var syntaxNode = syntaxReference?.GetSyntax ();
		return syntaxNode?.GetLocation ();

	}
}
