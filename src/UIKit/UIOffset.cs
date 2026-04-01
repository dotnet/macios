//
// UIEnums.cs:
//
// Copyright 2009-2011 Novell, Inc.
// Copyright 2011-2012, Xamarin Inc.
//
// Author:
//  Miguel de Icaza
//

#nullable enable

namespace UIKit {

	/// <summary>A position offset.</summary>
	/// <remarks>Represents a position offset. Positive values are to the right and down.</remarks>
	public struct UIOffset {

		// API match for UIOffsetZero field/constant
		/// <summary>A static identity offset of (0, 0).</summary>
		[Field ("UIOffsetZero")] // fake (but helps testing and could also help documentation)
		public static readonly UIOffset Zero;

		/// <summary>Creates a new <see cref="UIOffset" /> with the specified horizontal and vertical offsets.</summary>
		/// <param name="horizontal">The horizontal offset.</param>
		/// <param name="vertical">The vertical offset.</param>
		public UIOffset (nfloat horizontal, nfloat vertical)
		{
			Horizontal = horizontal;
			Vertical = vertical;
		}
		/// <summary>The horizontal offset.</summary>
		public /* CGFloat */ nfloat Horizontal;
		/// <summary>The vertical offset.</summary>
		public /* CGFloat */ nfloat Vertical;

		/// <summary>Determines whether this <see cref="UIOffset" /> is equal to the specified object.</summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if <paramref name="obj" /> is a <see cref="UIOffset" /> with the same horizontal and vertical values; otherwise, <see langword="false" />.</returns>
		public override bool Equals (object? obj)
		{
			if (!(obj is UIOffset))
				return false;
			var other = (UIOffset) obj;
			return other.Horizontal == Horizontal && other.Vertical == Vertical;
		}

		/// <summary>Returns a hash code for this <see cref="UIOffset" />.</summary>
		/// <returns>A hash code for this instance.</returns>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Horizontal, Vertical);
		}

		/// <summary>Determines whether two <see cref="UIOffset" /> instances are equal.</summary>
		/// <param name="left">The first offset to compare.</param>
		/// <param name="right">The second offset to compare.</param>
		/// <returns><see langword="true" /> if the two offsets are equal; otherwise, <see langword="false" />.</returns>
		public static bool operator == (UIOffset left, UIOffset right)
		{
			return (left.Horizontal == right.Horizontal) && (left.Vertical == right.Vertical);
		}

		/// <summary>Determines whether two <see cref="UIOffset" /> instances are not equal.</summary>
		/// <param name="left">The first offset to compare.</param>
		/// <param name="right">The second offset to compare.</param>
		/// <returns><see langword="true" /> if the two offsets are not equal; otherwise, <see langword="false" />.</returns>
		public static bool operator != (UIOffset left, UIOffset right)
		{
			return (left.Horizontal != right.Horizontal) || (left.Vertical != right.Vertical);
		}
	}
}
