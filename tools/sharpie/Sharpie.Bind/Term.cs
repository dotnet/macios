// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Writing;

public enum Term {
	None,
	Keyword,
	ObjCKeyword,
	Operator,
	TypeName,
	TypeReferenceName,
	TypeDefinitionName,
	Attribute,
	MethodName,
	StringLiteral,
	ArgumentName,
	PropertyName,
	Comment,
	NumberLiteral,
	CharLiteral,
	PreprocessingDirective,
}
