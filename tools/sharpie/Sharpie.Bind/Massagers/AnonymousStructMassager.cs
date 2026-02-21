// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Fix up names on anonymous structs and unions where defined like 'struct { int foo; } name;'.
/// </summary>
public class AnonymousStructMassager : Massager<AnonymousStructMassager> {
	int counter;

	public AnonymousStructMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		if (HasVisited (typeDeclaration))
			return;

		MarkVisited (typeDeclaration);

		base.VisitTypeDeclaration (typeDeclaration);


		FieldDeclaration? field;
		if (String.IsNullOrEmpty (typeDeclaration.Name) &&
			(field = typeDeclaration.NextSibling as FieldDeclaration) is not null) {
			var nativeDecl = typeDeclaration.Annotation<RecordDecl> ();
			var nativeType = field.ReturnType.Annotation<RecordType> ();

			if (nativeType?.Decl == nativeDecl) {
				if (nativeDecl.CursorKind == CXCursorKind.CXCursor_UnionDecl) {
					typeDeclaration.Name = "AnonymousUnion" + ++counter;
				} else {
					var fieldName = field.Variables.First ().Name;

					var structName = fieldName.UCFirst ();
					if (structName == fieldName)
						structName = "_" + structName;

					typeDeclaration.Name = structName;

					field.ReturnType = AstType.Create (structName);
					field.ReturnType.AddAnnotation (nativeType);
				}
			}
		}

		if (typeDeclaration.ClassType != ClassType.Enum && string.IsNullOrEmpty (typeDeclaration.Name))
			typeDeclaration.Name = $"AnonymousType{++counter}";

		if (typeDeclaration.Name.Contains (' ')) {
			var nativeDecl = typeDeclaration.Annotation<RecordDecl> ();
			Binder.BindingResult.ReportUnsupportedConstruct (nativeDecl.Location, $"Anonymous struct/union with space in its name: {typeDeclaration.Name}");
		}
	}
}
