//
// UITypes.cs: Various types defined in UIKit
//
// Authors:
//   Miguel de Icaza
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2009, Novell, Inc.
// Copyright 2011-2016 Xamarin Inc.
//

using CoreGraphics;

#nullable enable

namespace UIKit {

	/// <summary>Edge insets, used to reduce or expand rectangles.</summary>
	[StructLayout (LayoutKind.Sequential)]
	public struct UIEdgeInsets {

		// API match for UIEdgeInsetsZero field/constant
		/// <summary>An instance with all of the <see cref="UIEdgeInsets" /> parameters set to zero.</summary>
		[Field ("UIEdgeInsetsZero")] // fake (but helps testing and could also help documentation)
		public static readonly UIEdgeInsets Zero;

		/// <summary>The top inset value.</summary>
		public nfloat Top;
		/// <summary>The left inset value.</summary>
		public nfloat Left;
		/// <summary>The bottom inset value.</summary>
		public nfloat Bottom;
		/// <summary>The right inset value.</summary>
		public nfloat Right;

#if !COREBUILD
		/// <summary>Initializes a new <see cref="UIEdgeInsets" /> with the specified inset values.</summary>
		/// <param name="top">The top inset value.</param>
		/// <param name="left">The left inset value.</param>
		/// <param name="bottom">The bottom inset value.</param>
		/// <param name="right">The right inset value.</param>
		public UIEdgeInsets (nfloat top, nfloat left, nfloat bottom, nfloat right)
		{
			Top = top;
			Left = left;
			Bottom = bottom;
			Right = right;
		}

		// note: UIEdgeInsetsInsetRect (UIGeometry.h) is a macro
		/// <summary>Adjusts a rectangle by the given edge insets.</summary>
		/// <param name="rect">The rectangle to adjust.</param>
		/// <returns>A new rectangle adjusted by the edge insets.</returns>
		public CGRect InsetRect (CGRect rect)
		{
			return new CGRect (rect.X + Left,
								   rect.Y + Top,
								   rect.Width - Left - Right,
								   rect.Height - Top - Bottom);
		}

		// note: UIEdgeInsetsEqualToEdgeInsets (UIGeometry.h) is a macro
		/// <summary>Determines whether the specified <see cref="UIEdgeInsets" /> is equal to this instance.</summary>
		/// <param name="other">The <see cref="UIEdgeInsets" /> to compare with this instance.</param>
		/// <returns><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
		public bool Equals (UIEdgeInsets other)
		{
			if (Left != other.Left)
				return false;
			if (Right != other.Right)
				return false;
			if (Top != other.Top)
				return false;
			return (Bottom == other.Bottom);
		}

		/// <summary>Determines whether the specified object is equal to this instance.</summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if the specified object is an equal <see cref="UIEdgeInsets" />; otherwise, <see langword="false" />.</returns>
		public override bool Equals (object? obj)
		{
			if (obj is UIEdgeInsets other)
				return Equals (other);
			return false;
		}

		/// <summary>Determines whether two <see cref="UIEdgeInsets" /> instances are equal.</summary>
		/// <param name="insets1">The first instance to compare.</param>
		/// <param name="insets2">The second instance to compare.</param>
		/// <returns><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
		public static bool operator == (UIEdgeInsets insets1, UIEdgeInsets insets2)
		{
			return insets1.Equals (insets2);
		}

		/// <summary>Determines whether two <see cref="UIEdgeInsets" /> instances are not equal.</summary>
		/// <param name="insets1">The first instance to compare.</param>
		/// <param name="insets2">The second instance to compare.</param>
		/// <returns><see langword="true" /> if the two instances are not equal; otherwise, <see langword="false" />.</returns>
		public static bool operator != (UIEdgeInsets insets1, UIEdgeInsets insets2)
		{
			return !insets1.Equals (insets2);
		}

		/// <summary>Returns the hash code for this <see cref="UIEdgeInsets" />.</summary>
		/// <returns>A hash code for this instance.</returns>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Top, Left, Right, Bottom);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static UIEdgeInsets UIEdgeInsetsFromString (IntPtr /* NSString */ s);

		/// <summary>Creates a <see cref="UIEdgeInsets" /> from a string representation.</summary>
		/// <param name="s">The string representation of the edge insets.</param>
		/// <returns>A new <see cref="UIEdgeInsets" /> parsed from the string.</returns>
		static public UIEdgeInsets FromString (string? s)
		{
			// note: null is allowed
			var ptr = NSString.CreateNative (s);
			var value = UIEdgeInsetsFromString (ptr);
			NSString.ReleaseNative (ptr);
			return value;
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static IntPtr /* NSString */ NSStringFromUIEdgeInsets (UIEdgeInsets insets);

		// note: ensure we can roundtrip ToString into FromString
		/// <summary>Returns a string representation of the edge insets.</summary>
		/// <returns>A string representation of this <see cref="UIEdgeInsets" />.</returns>
		public override string ToString ()
		{
			using (var ns = new NSString (NSStringFromUIEdgeInsets (this)))
				return ns.ToString ();
		}
#endif
	}

	/// <summary>A range of floating-point numbers.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct UIFloatRange : IEquatable<UIFloatRange> {

		/// <summary>The minimum value in the range.</summary>
		public nfloat Minimum;
		/// <summary>The maximum value in the range.</summary>
		public nfloat Maximum;

		/// <summary>Initializes a new <see cref="UIFloatRange" /> with the specified minimum and maximum values.</summary>
		/// <param name="minimum">The minimum value in the range.</param>
		/// <param name="maximum">The maximum value in the range.</param>
		public UIFloatRange (nfloat minimum, nfloat maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static byte UIFloatRangeIsInfinite (UIFloatRange range);

		/// <summary>Gets whether the range is infinitely large.</summary>
		/// <value><see langword="true" /> if the range is infinite; otherwise, <see langword="false" />.</value>
		public bool IsInfinite {
			get {
				return UIFloatRangeIsInfinite (this) != 0;
			}
		}

		// Got replaced by a macro in iOS 13 (Xcode 11)...
		// [DllImport (Constants.UIKitLibrary)]
		// static extern bool UIFloatRangeIsEqualToRange (UIFloatRange range, UIFloatRange otherRange);

		/// <summary>Determines whether the specified <see cref="UIFloatRange" /> is equal to this instance.</summary>
		/// <param name="other">The <see cref="UIFloatRange" /> to compare with this instance.</param>
		/// <returns><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
		public bool Equals (UIFloatRange other) => this.Minimum == other.Minimum && this.Maximum == other.Maximum;

		/// <summary>Determines whether the specified object is equal to this instance.</summary>
		/// <param name="other">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if the specified object is an equal <see cref="UIFloatRange" />; otherwise, <see langword="false" />.</returns>
		public override bool Equals (object? other)
		{
			if (other is UIFloatRange range)
				return Equals (range);
			return false;
		}

		/// <summary>Returns the hash code for this <see cref="UIFloatRange" />.</summary>
		/// <returns>A hash code for this instance.</returns>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Minimum, Maximum);
		}

		/// <summary>A <see cref="UIFloatRange" /> with zero magnitude.</summary>
		[Field ("UIFloatRangeZero")] // fake (but helps testing and could also help documentation)
		public static UIFloatRange Zero;

		/// <summary>A <see cref="UIFloatRange" /> that spans from negative infinity to positive infinity.</summary>
		[Field ("UIFloatRangeInfinite")] // fake (but helps testing and could also help documentation)
		public static UIFloatRange Infinite = new UIFloatRange (nfloat.NegativeInfinity, nfloat.PositiveInfinity);
	}

#if IOS || __MACCATALYST__
	/// <summary>Represents the position of a pointer accessory relative to the pointer.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct UIPointerAccessoryPosition {
		/// <summary>The offset of the accessory from its default position.</summary>
		public nfloat Offset;
		/// <summary>The angle of the accessory, in radians.</summary>
		public nfloat Angle;

		/// <summary>Initializes a new <see cref="UIPointerAccessoryPosition" /> with the specified offset and angle.</summary>
		/// <param name="offset">The offset of the accessory from its default position.</param>
		/// <param name="angle">The angle of the accessory, in radians.</param>
		public UIPointerAccessoryPosition (nfloat offset, nfloat angle)
		{
			Offset = offset;
			Angle = angle;
		}

#if !COREBUILD
		/// <summary>The accessory position at the top.</summary>
		[Field ("UIPointerAccessoryPositionTop", "UIKit")]
		public static UIPointerAccessoryPosition Top => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTop"))!;

		/// <summary>The accessory position at the top-right.</summary>
		[Field ("UIPointerAccessoryPositionTopRight", "UIKit")]
		public static UIPointerAccessoryPosition TopRight => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTopRight"))!;

		/// <summary>The accessory position at the right.</summary>
		[Field ("UIPointerAccessoryPositionRight", "UIKit")]
		public static UIPointerAccessoryPosition Right => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionRight"))!;

		/// <summary>The accessory position at the bottom-right.</summary>
		[Field ("UIPointerAccessoryPositionBottomRight", "UIKit")]
		public static UIPointerAccessoryPosition BottomRight => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottomRight"))!;

		/// <summary>The accessory position at the bottom.</summary>
		[Field ("UIPointerAccessoryPositionBottom", "UIKit")]
		public static UIPointerAccessoryPosition Bottom => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottom"))!;

		/// <summary>The accessory position at the bottom-left.</summary>
		[Field ("UIPointerAccessoryPositionBottomLeft", "UIKit")]
		public static UIPointerAccessoryPosition BottomLeft => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottomLeft"))!;

		/// <summary>The accessory position at the left.</summary>
		[Field ("UIPointerAccessoryPositionLeft", "UIKit")]
		public static UIPointerAccessoryPosition Left => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionLeft"))!;

		/// <summary>The accessory position at the top-left.</summary>
		[Field ("UIPointerAccessoryPositionTopLeft", "UIKit")]
		public static UIPointerAccessoryPosition TopLeft => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTopLeft"))!;
#endif
	}
#endif

#if false
	[Protocol]
	public interface IUITextInputTraits {
		[Export ("autocapitalizationType")]
		UITextAutocapitalizationType AutocapitalizationType { get; set; }

		[Export ("autocorrectionType")]
		UITextAutocorrectionType AutocorrectionType { get; set; }

		[Export ("keyboardType")]
		UIKeyboardType KeyboardType { get; set; }

		[Export ("keyboardAppearance")]
		UIKeyboardAppearance KeyboardAppearance { get; set; }

		[Export ("returnKeyType")]
		UIReturnKeyType ReturnKeyType { get; set; }

		[Export ("enablesReturnKeyAutomatically")]
		bool EnablesReturnKeyAutomatically { get; set; } 

		[Export ("secureTextEntry")]
		bool SecureTextEntry { get; set; } 
	}
#endif
}
