// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;

namespace CoreGraphics;

/// <summary>This class represents a <see cref="CGRect" /> stored in a dictionary; that is a dictionary with "X", "Y", Width" and "Height" keys for the corresponding <see cref="CGRect" /> fields.</summary>
public class CGRectDictionary : DictionaryContainer {
#if !COREBUILD
	/// <summary>Creates a new <see cref="CGRectDictionary" /> with default (empty) values.</summary>
	[Preserve (Conditional = true)]
	public CGRectDictionary () : base (new NSMutableDictionary ()) { }

	/// <summary>Creates a new <see cref="CGRectDictionary" /> from the values that are specified in <paramref name="dictionary" />.</summary>
	/// <param name="dictionary">The dictionary to use to populate the properties of this type.</param>
	[Preserve (Conditional = true)]
	public CGRectDictionary (NSDictionary? dictionary) : base (dictionary) { }

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

	/// <summary>Get the <see cref="CGRect" /> stored in this dictionary.</summary>
	/// <returns>The <see cref="CGRect" /> stored in this dictionary.</returns>
	public CGRect ToRect ()
	{
		if (CGRect.TryParse (Dictionary, out var rv))
			return rv;
		return default;
	}
#endif // !COREBUILD
}
