// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Clang;

[StructLayout (LayoutKind.Sequential)]
public struct PresumedLoc {
	readonly CXString filename;
	readonly uint line;
	readonly uint column;

	public PresumedLoc (CXString filename, uint line, uint column)
	{
		this.filename = filename;
		this.line = line;
		this.column = column;
	}

	public unsafe bool IsInvalid {
		get { return filename.data is null; }
	}

	public bool IsValid {
		get { return !IsInvalid; }
	}

	public string FileName {
		get { return filename.ToString (); }
	}

	public uint Line {
		get { return line; }
	}

	public uint Column {
		get { return column; }
	}

	public override string? ToString ()
	{
		return IsValid ? String.Format ("{0}:{1}:{2}", FileName, Line, Column) : null;
	}
}
