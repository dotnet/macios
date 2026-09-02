// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Massagers;

/// <summary>
/// Splits the generated binding into one .cs file per source header file.
/// Struct/enum declarations go into a separate StructsAndEnums.cs file.
/// </summary>
[RegisterBefore (typeof (GenerateUsingStatementsMassager))]
public sealed class DeepSplitMassager : Massager<DeepSplitMassager> {
	readonly Dictionary<string, DocumentSyntaxTree> documentsByHeader = new (StringComparer.OrdinalIgnoreCase);
	readonly DocumentSyntaxTree structsAndEnums = new DocumentSyntaxTree ("StructsAndEnums.cs");

	public DeepSplitMassager (ObjectiveCBinder binder)
		: base (binder)
	{
	}

	DocumentSyntaxTree GetOrCreateDocument (string headerFileName)
	{
		var baseName = Path.GetFileNameWithoutExtension (headerFileName);
		var key = baseName.ToLowerInvariant ();

		if (!documentsByHeader.TryGetValue (key, out var doc)) {
			doc = new DocumentSyntaxTree (baseName + ".cs");
			documentsByHeader [key] = doc;
		}
		return doc;
	}

	string? GetSourceHeaderName (AstNode node)
	{
		// Walk annotations to find the linked Clang declaration and its source location
		foreach (var annotation in node.Annotations) {
			if (annotation is Cursor cursor) {
				if (cursor.TryGetPresumedLoc (out var loc) && loc.HasValue && !string.IsNullOrEmpty (loc.Value.FileName))
					return Path.GetFileName (loc.Value.FileName);
			}
		}
		return null;
	}

	public override void VisitTypeDeclaration (TypeDeclaration typeDeclaration)
	{
		if (HasVisited (typeDeclaration))
			return;

		MarkVisited (typeDeclaration);
		typeDeclaration.Remove ();

		if (typeDeclaration.ClassType == ClassType.Interface) {
			var headerName = GetSourceHeaderName (typeDeclaration);
			if (headerName is not null) {
				GetOrCreateDocument (headerName).Members.Add (typeDeclaration);
			} else {
				GetOrCreateDocument ("ApiDefinition").Members.Add (typeDeclaration);
			}
		} else {
			structsAndEnums.Members.Add (typeDeclaration);
		}
	}

	public override void VisitDelegateDeclaration (DelegateDeclaration delegateDeclaration)
	{
		if (HasVisited (delegateDeclaration))
			return;

		MarkVisited (delegateDeclaration);
		delegateDeclaration.Remove ();

		var headerName = GetSourceHeaderName (delegateDeclaration);
		if (headerName is not null) {
			GetOrCreateDocument (headerName).Members.Add (delegateDeclaration);
		} else {
			GetOrCreateDocument ("ApiDefinition").Members.Add (delegateDeclaration);
		}
	}

	public override void VisitSyntaxTree (SyntaxTree syntaxTree)
	{
		base.VisitSyntaxTree (syntaxTree);

		if (syntaxTree.Members.Count > 0) {
			Console.Error.WriteLine (syntaxTree);
			throw new Exception ("original SyntaxTree should be empty");
		}

		// Add documents sorted by filename for deterministic output
		foreach (var doc in documentsByHeader.OrderBy (kv => kv.Key)) {
			if (doc.Value.Members.Count > 0)
				syntaxTree.Members.Add (doc.Value);
		}

		if (structsAndEnums.Members.Count > 0)
			syntaxTree.Members.Add (structsAndEnums);
	}
}
