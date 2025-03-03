// 
// CTFontTrait.cs: CTFont Traits
//
// Authors: Mono Team
//          Rolf Bjarne Kvinge <rolf@xamarin.com>
//     
// Copyright 2010 Novell, Inc
// Copyright 2011 - 2014 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace CoreText {

#if !NET
	public static class CTFontTraitKey {
		public static readonly NSString? Symbolic;
		public static readonly NSString? Weight;
		public static readonly NSString? Width;
		public static readonly NSString? Slant;

		static CTFontTraitKey ()
		{
			var handle = Libraries.CoreText.Handle;
			Symbolic = Dlfcn.GetStringConstant (handle, "kCTFontSymbolicTrait");
			Weight = Dlfcn.GetStringConstant (handle, "kCTFontWeightTrait");
			Width = Dlfcn.GetStringConstant (handle, "kCTFontWidthTrait");
			Slant = Dlfcn.GetStringConstant (handle, "kCTFontSlantTrait");
		}
	}
#endif

	[Flags]
	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontTraits.h
	public enum CTFontSymbolicTraits : uint {
		/// <summary>Used when no values are set.</summary>
		None = 0,
		/// <summary>This is an italic font.  If querying <see cref="T:CoreText.CTFontTraits" /> you can get more
		/// 	  information about the slant from the <see cref="P:CoreText.CTFontTraits.Slant" /> property.</summary>
		Italic = (1 << 0),
		/// <summary>
		/// 	  This is a bold font.  If querying <see cref="T:CoreText.CTFontTraits" /> you can get more
		/// 	  information about the weight from the <see cref="P:CoreText.CTFontTraits.Weight" /> property.
		/// 	</summary>
		Bold = (1 << 1),
		/// <summary>This is an expanded font.</summary>
		Expanded = (1 << 5),
		/// <summary>This is a condensed font.  If querying <see cref="T:CoreText.CTFontTraits" /> you can get more
		/// 	  information about the width from the <see cref="P:CoreText.CTFontTraits.Width" /> property.</summary>
		Condensed = (1 << 6),
		/// <summary>Use fixed fonts if available.</summary>
		MonoSpace = (1 << 10),
		/// <summary>The font has vertical information for rendering.</summary>
		Vertical = (1 << 11),
		/// <summary>The font is optimized for UI rendering.</summary>
		UIOptimized = (1 << 12),
		/// <summary>This font contains glyphs with colors (for example, some Emoji fonts).</summary>
		ColorGlyphs = (1 << 13),
		/// <summary>The font is a composite font reference, and a cascade list is expected for the font.</summary>
		Composite = (1 << 14),

		/// <summary>Mask used to extract trait values.</summary>
		Mask = ((uint) 15 << CTFontTraits.ClassMaskShift),
	}

	// defined as uint32_t - /System/Library/Frameworks/CoreText.framework/Headers/CTFontTraits.h
	public enum CTFontStylisticClass : uint {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Unknown = ((uint) 0 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		OldStyleSerifs = ((uint) 1 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		TransitionalSerifs = ((uint) 2 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		ModernSerifs = ((uint) 3 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		ClarendonSerifs = ((uint) 4 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		SlabSerifs = ((uint) 5 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		FreeformSerifs = ((uint) 7 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		SansSerif = ((uint) 8 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		Ornamentals = ((uint) 9 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		Scripts = ((uint) 10 << CTFontTraits.ClassMaskShift),
		/// <summary>To be added.</summary>
		Symbolic = ((uint) 12 << CTFontTraits.ClassMaskShift),
	}

#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
#endif
	public class CTFontTraits {

		public CTFontTraits ()
			: this (new NSMutableDictionary ())
		{
		}

		public CTFontTraits (NSDictionary dictionary)
		{
			if (dictionary is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (dictionary));
			Dictionary = dictionary;
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDictionary Dictionary { get; private set; }

		// CFNumber
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint? Symbolic {
			get { return Adapter.GetUInt32Value (Dictionary, CTFontTraitKey.Symbolic); }
			set { Adapter.SetValue (Dictionary, CTFontTraitKey.Symbolic!, value); }
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CTFontSymbolicTraits? SymbolicTraits {
			get {
				var v = Symbolic;
				return !v.HasValue ? null : (CTFontSymbolicTraits?) (v.Value & ~StylisticClassMask);
			}
			set {
				var c = StylisticClass;
				Symbolic = Adapter.BitwiseOr (
						c.HasValue ? (uint?) c.Value : null,
						value.HasValue ? (uint?) value.Value : null);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CTFontStylisticClass? StylisticClass {
			get {
				var v = Symbolic;
				return !v.HasValue ? null : (CTFontStylisticClass?) (v.Value & StylisticClassMask);
			}
			set {
				var t = SymbolicTraits;
				Symbolic = Adapter.BitwiseOr (
						t.HasValue ? (uint?) t.Value : null,
						value.HasValue ? (uint?) value.Value : null);
			}
		}

		// CFNumber representing a float value 
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? Weight {
			get { return Adapter.GetSingleValue (Dictionary, CTFontTraitKey.Weight); }
			set { Adapter.SetValue (Dictionary, CTFontTraitKey.Weight!, value); }
		}

		// CFNumber representing a float value 
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? Width {
			get { return Adapter.GetSingleValue (Dictionary, CTFontTraitKey.Width); }
			set { Adapter.SetValue (Dictionary, CTFontTraitKey.Width!, value); }
		}

		// CFNumber representing a float value 
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? Slant {
			get { return Adapter.GetSingleValue (Dictionary, CTFontTraitKey.Slant); }
			set { Adapter.SetValue (Dictionary, CTFontTraitKey.Slant!, value); }
		}

		internal const int ClassMaskShift = 28;
		internal const uint StylisticClassMask = ((uint) 15 << ClassMaskShift);
	}
}
