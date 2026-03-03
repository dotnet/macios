// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Writing;

public interface ICodeWriter {
	void WriteIndent (int indentLevel);

	void Write (string str);
	void Write (Term term, string str);

	void WriteLine ();
	void WriteLine (string str);
	void WriteLine (Term term, string str);

	void PushTag (object tag);
	void PopTag ();
}
