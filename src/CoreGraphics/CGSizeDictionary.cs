// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace CoreGraphics;

/// <summary>This class represents a <see cref="CGSize" /> stored in a dictionary; that is a dictionary with "Width" and "Height" keys for the corresponding <see cref="CGSize" /> fields.</summary>
public class CGSizeDictionary : DictionaryContainer {
#if !COREBUILD
	/// <summary>Creates a new <see cref="CGSizeDictionary" /> with default (empty) values.</summary>
	[Preserve (Conditional = true)]
	public CGSizeDictionary () : base (new NSMutableDictionary ()) { }

	/// <summary>Creates a new <see cref="CGSizeDictionary" /> from the values that are specified in <paramref name="dictionary" />.</summary>
	/// <param name="dictionary">The dictionary to use to populate the properties of this type.</param>
	[Preserve (Conditional = true)]
	public CGSizeDictionary (NSDictionary? dictionary) : base (dictionary) { }

	/// <summary>The width component of the <see cref="CGSize" />.</summary>
	public nfloat? Width {
		get => GetNFloatValue ((NSString) "Width");
		set => SetNumberValue ((NSString) "Width", value);
	}

	/// <summary>The height component of the <see cref="CGSize" />.</summary>
	public nfloat? Height {
		get => GetNFloatValue ((NSString) "Height");
		set => SetNumberValue ((NSString) "Height", value);
	}

	/// <summary>Get the <see cref="CGSize" /> stored in this dictionary.</summary>
	/// <returns>The <see cref="CGSize" /> stored in this dictionary.</returns>
	public CGSize ToSize ()
	{
		if (CGSize.TryParse (Dictionary, out var rv))
			return rv;
		return default;
	}
#endif // !COREBUILD
}
