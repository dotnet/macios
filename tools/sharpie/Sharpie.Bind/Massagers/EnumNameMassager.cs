// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;
using Sharpie.Bind.Attributes;

namespace Sharpie.Bind.Massagers;

public class EnumNameMassager : Massager<EnumNameMassager> {
	readonly Dictionary<string, string> typeRenames = new Dictionary<string, string> ();
	readonly Dictionary<string, string> memberRenames = new Dictionary<string, string> ();

	public EnumNameMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	int counter;
	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		if (HasVisited (typeDeclaration))
			return;

		MarkVisited (typeDeclaration);

		if (typeDeclaration.ClassType != ClassType.Enum) {
			base.VisitTypeDeclaration (typeDeclaration);
			return;
		}

		// Don't massage C++11 scoped enums since the language enforces sensible naming like C#
		if (typeDeclaration.Annotation<EnumDecl> ().IsScoped)
			return;

		int memberCount = 0;
		string? memberPrefix = null;

		memberRenames.Clear ();

		memberCount = typeDeclaration.Children.OfType<EnumMemberDeclaration> ().Count ();

		// find the shared prefix of all member names
		if (memberCount > 1)
			memberPrefix = typeDeclaration
				.Children
				.OfType<EnumMemberDeclaration> ()
				.Select (m => m.Name)
				.SharedPrefix ();

		// if we have an anonymous enum with members having a
		// shared prefix, use that prefix as the enum name
		if (String.IsNullOrEmpty (typeDeclaration.Name) && !String.IsNullOrEmpty (memberPrefix)) {
			typeDeclaration.Name = memberPrefix;
			typeDeclaration.AddAttribute (new VerifyAttribute (VerifyHint.InferredFromMemberPrefix));
		}

		foreach (var member in typeDeclaration.Children.OfType<EnumMemberDeclaration> ()) {
			if (String.IsNullOrEmpty (member.Name))
				continue;

			var origName = member.Name;

			if (!String.IsNullOrEmpty (memberPrefix))
				member.Name = member.Name.Substring (memberPrefix.Length);

			if (member.Name.Length > 1 && member.Name [0] == 'k' && Char.IsUpper (member.Name [1]))
				member.Name = member.Name.Substring (1);

			member.Name = member.Name.ToCamelCaseFromUnderscoreUpperCase ();

			if (member.Name.Length > 0 && member.Name [0] == '_')
				member.Name = member.Name.Substring (1);

			if (member.Name.Length == 0 || !Char.IsLetter (member.Name [0]))
				member.Name = origName.ToCamelCaseFromUnderscoreUpperCase ();

			if (member.Name != origName)
				memberRenames.Add (origName, member.Name);
		}

		if (!String.IsNullOrEmpty (typeDeclaration.Name)) {
			var origName = typeDeclaration.Name;
			typeDeclaration.Name = typeDeclaration.Name.ToCamelCaseFromUnderscoreUpperCase ();
			if (typeDeclaration.Name != origName)
				typeRenames [origName] = typeDeclaration.Name;
		}


		if (string.IsNullOrEmpty (typeDeclaration.Name))
			typeDeclaration.Name = $"AnonymousEnum{++counter}";

		base.VisitTypeDeclaration (typeDeclaration);
		memberRenames.Clear ();
	}

	public override void VisitSimpleType (SimpleType simpleType)
	{
		if (typeRenames.TryGetValue (simpleType.Identifier, out var rename))
			simpleType.Identifier = rename;
	}

	public override void VisitIdentifier (Identifier identifier)
	{
		if (memberRenames.TryGetValue (identifier.Name, out var rename))
			identifier.Name = rename;
	}
}
