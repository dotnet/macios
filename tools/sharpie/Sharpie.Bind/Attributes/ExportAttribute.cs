// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ICSharpCode.NRefactory.CSharp;

namespace Sharpie.Bind.Attributes;

public sealed class ExportAttribute : GroupedAttribute {
	public string Name { get; private set; }
	public ArgumentSemantic ArgumentSemantic { get; private set; }
	public bool IsVariadic { get; private set; }

	public ExportAttribute (string name,
		ArgumentSemantic argumentSemantic = ArgumentSemantic.None,
		bool isVariadic = false) : base (2)
	{
		Type = AstType.Create ("Foundation.ExportAttribute");
		Name = name;
		ArgumentSemantic = argumentSemantic;

		Arguments.Add (new PrimitiveExpression (name));

		if (argumentSemantic != ArgumentSemantic.None)
			Arguments.Add (new MemberReferenceExpression (
				new TypeReferenceExpression (AstType.Create ("ObjCRuntime.ArgumentSemantic")),
				argumentSemantic.ToString ()
			));

		if (isVariadic)
			Arguments.Add (new NamedExpression ("IsVariadic", new PrimitiveExpression (true)));
	}

	public void RemoveArgumentSemantic ()
	{
		if (ArgumentSemantic != ArgumentSemantic.None) {
			ArgumentSemantic = ArgumentSemantic.None;
			Arguments.OfType<MemberReferenceExpression> ().FirstOrDefault ()?.Remove ();
		}
	}
}
