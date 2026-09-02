// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class BaseTypeAttribute : GroupedAttribute {
	public AstType? BaseType {
		get {
			var typeofExpr = Arguments.FirstOrNullObject () as TypeOfExpression;
			if (typeofExpr is not null)
				return typeofExpr.Type;
			return null;
		}
	}

	public BaseTypeAttribute (string type) : this (AstType.Create (type))
	{
	}

	public BaseTypeAttribute (AstType type) : base (2)
	{
		// NOTE: this is in global namespace :-/
		Type = AstType.Create ("BaseTypeAttribute");
		Arguments.Add (new TypeOfExpression (type));
	}
}
