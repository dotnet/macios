//
// UIEnums.cs:
//
// Copyright 2009-2011 Novell, Inc.
// Copyright 2011-2012, Xamarin Inc.
//
// Author:
//  Miguel de Icaza
//

using System;
using Foundation;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	// UIGeometry.h
	/// <summary>A position offset.</summary>
	///     <remarks>Represents a position offset. Positive values are to the right and down.</remarks>
	public struct UIOffset {

		// API match for UIOffsetZero field/constant
		/// <summary>A static identity offset of 0,0.</summary>
		///         <remarks>To be added.</remarks>
		[Field ("UIOffsetZero")] // fake (but helps testing and could also help documentation)
		public static readonly UIOffset Zero;

		public UIOffset (nfloat horizontal, nfloat vertical)
		{
			Horizontal = horizontal;
			Vertical = vertical;
		}
		/// <summary>The horizontal offset.</summary>
		///         <remarks>To be added.</remarks>
		public /* CGFloat */ nfloat Horizontal;
		/// <summary>The vertical offset.</summary>
		///         <remarks>To be added.</remarks>
		public /* CGFloat */ nfloat Vertical;

		/// <param name="obj">To be added.</param>
		///         <summary>Whether this has the same value as <paramref name="obj" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object obj)
		{
			if (!(obj is UIOffset))
				return false;
			var other = (UIOffset) obj;
			return other.Horizontal == Horizontal && other.Vertical == Vertical;
		}

		/// <summary>The hash code for this <see cref="T:UIKit.UIOffset" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Horizontal, Vertical);
		}

		public static bool operator == (UIOffset left, UIOffset right)
		{
			return (left.Horizontal == right.Horizontal) && (left.Vertical == right.Vertical);
		}

		public static bool operator != (UIOffset left, UIOffset right)
		{
			return (left.Horizontal != right.Horizontal) || (left.Vertical != right.Vertical);
		}
	}
}
