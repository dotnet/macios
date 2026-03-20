// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

public struct Selector {
	public static readonly Selector Empty = new Selector ();

	public string Name;

	public Selector (string name)
	{
		Name = name;
	}

	public uint ArgumentCount {
		get => (uint) (Name.Count (c => c == ':'));
	}

	public string GetNameForSlot (uint argIndex)
	{
		return Name.Split (':') [argIndex];
	}

	public override string ToString ()
	{
		return Name;
	}
}
