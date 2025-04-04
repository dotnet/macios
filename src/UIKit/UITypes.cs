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
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreGraphics;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	[StructLayout (LayoutKind.Sequential)]
	public struct UIEdgeInsets {

		// API match for UIEdgeInsetsZero field/constant
		/// <summary>An instance with all of the UIEdgeInsets parameters set to zero.</summary>
		///         <remarks>
		///         </remarks>
		[Field ("UIEdgeInsetsZero")] // fake (but helps testing and could also help documentation)
		public static readonly UIEdgeInsets Zero;

		/// <summary>Top value.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Top;
		/// <summary>Left value.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Left;
		/// <summary>Bottom value.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Bottom;
		/// <summary>Right value.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Right;

#if !COREBUILD
		public UIEdgeInsets (nfloat top, nfloat left, nfloat bottom, nfloat right)
		{
			Top = top;
			Left = left;
			Bottom = bottom;
			Right = right;
		}

		// note: UIEdgeInsetsInsetRect (UIGeometry.h) is a macro
		/// <param name="rect">To be added.</param>
		///         <summary>Adjusts a rectangle by the given edge insets.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGRect InsetRect (CGRect rect)
		{
			return new CGRect (rect.X + Left,
								   rect.Y + Top,
								   rect.Width - Left - Right,
								   rect.Height - Top - Bottom);
		}

		// note: UIEdgeInsetsEqualToEdgeInsets (UIGeometry.h) is a macro
		/// <param name="other">To be added.</param>
		///         <summary>Whether this is equivalent to <paramref name="other" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
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

		/// <param name="obj">To be added.</param>
		///         <summary>Whether this is equivalent to the <paramref name="obj" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object obj)
		{
			if (obj is UIEdgeInsets)
				return Equals ((UIEdgeInsets) obj);
			return false;
		}

		public static bool operator == (UIEdgeInsets insets1, UIEdgeInsets insets2)
		{
			return insets1.Equals (insets2);
		}

		public static bool operator != (UIEdgeInsets insets1, UIEdgeInsets insets2)
		{
			return !insets1.Equals (insets2);
		}

		/// <summary>The hash for this <see cref="T:UIKit.UIEdgeInsets" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Top, Left, Right, Bottom);
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static UIEdgeInsets UIEdgeInsetsFromString (IntPtr /* NSString */ s);

		/// <param name="s">To be added.</param>
		///         <summary>Creates an edge inset from a string representation.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		static public UIEdgeInsets FromString (string s)
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
		/// <summary>Returns a human-readable version of the UIEdgeInset properties, for debugging.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			using (var ns = new NSString (NSStringFromUIEdgeInsets (this)))
				return ns.ToString ();
		}
#endif
	}

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct UIFloatRange : IEquatable<UIFloatRange> {

		/// <summary>The minimum value in the interval.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Minimum;
		/// <summary>The maximum value in the interval.</summary>
		///         <remarks>To be added.</remarks>
		public nfloat Maximum;

		public UIFloatRange (nfloat minimum, nfloat maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		[DllImport (Constants.UIKitLibrary)]
		extern static byte UIFloatRangeIsInfinite (UIFloatRange range);

		/// <summary>Gets whether the range is infinitely large.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsInfinite {
			get {
				return UIFloatRangeIsInfinite (this) != 0;
			}
		}

		// Got replaced by a macro in iOS 13 (Xcode 11)...
		// [DllImport (Constants.UIKitLibrary)]
		// static extern bool UIFloatRangeIsEqualToRange (UIFloatRange range, UIFloatRange otherRange);

		/// <param name="other">To be added.</param>
		///         <summary>Whether two <see cref="T:UIKit.UIFloatRange" /> objects have equal values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool Equals (UIFloatRange other) => this.Minimum == other.Minimum && this.Maximum == other.Maximum;

		/// <param name="other">To be added.</param>
		///         <summary>Whether two <see cref="T:UIKit.UIFloatRange" /> objects have equal values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override bool Equals (object other)
		{
			if (other is UIFloatRange)
				return Equals ((UIFloatRange) other);
			return false;
		}

		/// <summary>A hash for the interval.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override int GetHashCode ()
		{
			return HashCode.Combine (Minimum, Maximum);
		}

		/// <summary>Gets a <see cref="T:UIKit.UIFloatRange" /> that has no magnitude.</summary>
		///         <remarks>To be added.</remarks>
		[Field ("UIFloatRangeZero")] // fake (but helps testing and could also help documentation)
		public static UIFloatRange Zero;

		/// <summary>An infinitely large range.</summary>
		///         <remarks>To be added.</remarks>
		[Field ("UIFloatRangeInfinite")] // fake (but helps testing and could also help documentation)
		public static UIFloatRange Infinite = new UIFloatRange (nfloat.NegativeInfinity, nfloat.PositiveInfinity);
	}

#if IOS || __MACCATALYST__
#if NET
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("maccatalyst")]
#else
	[Introduced (PlatformName.iOS, 15,0)]
	[Introduced (PlatformName.MacCatalyst, 15,0)]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct UIPointerAccessoryPosition {
		public nfloat Offset;
		public nfloat Angle;

		public UIPointerAccessoryPosition (nfloat offset, nfloat angle)
		{
			Offset = offset;
			Angle = angle;
		}

#if !COREBUILD
		[Field ("UIPointerAccessoryPositionTop", "UIKit")]
		public static UIPointerAccessoryPosition Top => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTop"))!;

		[Field ("UIPointerAccessoryPositionTopRight", "UIKit")]
		public static UIPointerAccessoryPosition TopRight => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionTopRight"))!;

		[Field ("UIPointerAccessoryPositionRight", "UIKit")]
		public static UIPointerAccessoryPosition Right => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionRight"))!;

		[Field ("UIPointerAccessoryPositionBottomRight", "UIKit")]
		public static UIPointerAccessoryPosition BottomRight => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottomRight"))!;

		[Field ("UIPointerAccessoryPositionBottom", "UIKit")]
		public static UIPointerAccessoryPosition Bottom => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottom"))!;

		[Field ("UIPointerAccessoryPositionBottomLeft", "UIKit")]
		public static UIPointerAccessoryPosition BottomLeft => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionBottomLeft"))!;

		[Field ("UIPointerAccessoryPositionLeft", "UIKit")]
		public static UIPointerAccessoryPosition Left => Marshal.PtrToStructure<UIPointerAccessoryPosition> (Dlfcn.GetIndirect (Libraries.UIKit.Handle, "UIPointerAccessoryPositionLeft"))!;

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
