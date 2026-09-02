// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.IO;
using System.Text;

namespace Sharpie.Writing;

public class CodeWriter : ICodeWriter {
	public TextWriter BaseWriter { get; private set; }
	public string IndentString { get; set; }

	public CodeWriter (TextWriter baseWriter)
	{
		if (baseWriter is null)
			throw new ArgumentNullException (nameof (baseWriter));

		this.BaseWriter = baseWriter;

		IndentString = "\t";
	}

	public virtual void WriteIndent (int indentLevel)
	{
		for (int i = 0; i < indentLevel; i++) {
			Write (IndentString);
		}
	}

	public virtual void WriteLine ()
	{
		BaseWriter.WriteLine ();
	}

	public virtual void Write (Term term, string? str)
	{
		BaseWriter.Write (str);
	}

	public virtual void WriteLine (Term term, string? str)
	{
		BaseWriter.WriteLine (str);
	}

	public void Write (string? str)
	{
		Write (Term.None, str);
	}

	public void WriteLine (string? str)
	{
		WriteLine (Term.None, str);
	}

	public void WriteStringLiteral (string literal, string nullToken)
	{
		if (literal is null) {
			Write (Term.TypeReferenceName, nullToken);
			return;
		}

		var builder = new StringBuilder ();
		builder.Append ("\"");
		foreach (var c in literal) {
			switch (c) {
			case '\\':
				builder.Append ("\\\\");
				break;
			case '\0':
				builder.Append ("\\0");
				break;
			case '\a':
				builder.Append ("\\a");
				break;
			case '\b':
				builder.Append ("\\b");
				break;
			case '\f':
				builder.Append ("\\f");
				break;
			case '\n':
				builder.Append ("\\n");
				break;
			case '\r':
				builder.Append ("\\r");
				break;
			case '\t':
				builder.Append ("\\t");
				break;
			case '\v':
				builder.Append ("\\v");
				break;
			default:
				builder.Append (c);
				break;
			}
		}
		builder.Append ("\"");
		Write (Term.StringLiteral, builder.ToString ());
	}

	public virtual void PushTag (object tag)
	{
	}

	public virtual void PopTag ()
	{
	}
}
