// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace CoreGraphics;

/// <summary>This class represents a <see cref="CGPoint" /> stored in a dictionary; that is a dictionary with "X" and "Y" keys for the corresponding <see cref="CGPoint" /> fields.</summary>
public class CGPointDictionary : DictionaryContainer {
#if !COREBUILD
	/// <summary>Creates a new <see cref="CGPointDictionary" /> with default (empty) values.</summary>
	[Preserve (Conditional = true)]
	public CGPointDictionary () : base (new NSMutableDictionary ()) { }

	/// <summary>Creates a new <see cref="CGPointDictionary" /> from the values that are specified in <paramref name="dictionary" />.</summary>
	/// <param name="dictionary">The dictionary to use to populate the properties of this type.</param>
	[Preserve (Conditional = true)]
	public CGPointDictionary (NSDictionary? dictionary) : base (dictionary) { }

	/// <summary>The X component of the <see cref="CGPoint" />.</summary>
	public nfloat? X {
		get => GetNFloatValue ((NSString) "X");
		set => SetNumberValue ((NSString) "X", value);
	}

	/// <summary>The Y component of the <see cref="CGPoint" />.</summary>
	public nfloat? Y {
		get => GetNFloatValue ((NSString) "Y");
		set => SetNumberValue ((NSString) "Y", value);
	}

	/// <summary>Get the <see cref="CGPoint" /> stored in this dictionary.</summary>
	/// <returns>The <see cref="CGPoint" /> stored in this dictionary.</returns>
	public CGPoint ToPoint ()
	{
		if (CGPoint.TryParse (Dictionary, out var rv))
			return rv;
		return default;
	}
#endif // !COREBUILD
}
