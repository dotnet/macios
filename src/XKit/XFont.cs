// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// Copyright 2009 Novell, Inc.
// Copyright 2012-2014 Xamarin Inc. All rights reserved.

#nullable enable

using CoreGraphics;
using CoreText;
using CGGlyph = System.UInt16;

#if __MACOS__
using XFont = AppKit.NSFont;
namespace AppKit {
#else
using XFont = UIKit.UIFont;
namespace UIKit {
#endif

#if !__MACOS__
	/// <summary>Provides extension methods for converting <see cref="UIFontWeight" /> values to their native weight values.</summary>
	public static class UIFontWeightExtensions {
		/// <summary>Gets the native weight value for the specified <see cref="UIFontWeight" />.</summary>
		/// <param name="weight">The font weight to convert.</param>
		/// <returns>The native weight value corresponding to the specified <paramref name="weight" />.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="weight" /> is not a valid <see cref="UIFontWeight" /> value.</exception>
		public static nfloat GetWeight (this UIFontWeight weight)
		{
			switch (weight) {
			case UIFontWeight.UltraLight:
				return UIFontWeightConstants.UltraLight;
			case UIFontWeight.Thin:
				return UIFontWeightConstants.Thin;
			case UIFontWeight.Light:
				return UIFontWeightConstants.Light;
			case UIFontWeight.Regular:
				return UIFontWeightConstants.Regular;
			case UIFontWeight.Medium:
				return UIFontWeightConstants.Medium;
			case UIFontWeight.Semibold:
				return UIFontWeightConstants.Semibold;
			case UIFontWeight.Bold:
				return UIFontWeightConstants.Bold;
			case UIFontWeight.Heavy:
				return UIFontWeightConstants.Heavy;
			case UIFontWeight.Black:
				return UIFontWeightConstants.Black;
			default:
				throw new ArgumentException (weight.ToString ());
			}
		}
	}

	/// <summary>Font representation for UIKit classes.</summary>
	/// <remarks>
	///   <para>
	///     <see cref="UIFont" /> objects are not instantiated directly, but instead are created from static methods on this class.
	///     You can use the
	///        <see cref="UIKit.UIFont.PreferredBody" />,
	///        <see cref="UIKit.UIFont.PreferredCaption1" />,
	///        <see cref="UIKit.UIFont.PreferredCaption2" />,
	///        <see cref="UIKit.UIFont.PreferredHeadline" />,
	///        <see cref="UIKit.UIFont.PreferredSubheadline" /> properties to get the preferred system fonts for the corresponding use case.
	///   </para>
	///   <para>
	///     Use the static <see cref="UIKit.UIFont.CreateFromName(System.String,nfloat)" /> method to create a named font.
	///     Font factories return independent managed wrappers, even when the system caches the native font, so each wrapper can be disposed independently.
	///   </para>
	///   <para>
	///     You can use the <see cref="UIKit.UIFont.FamilyNames" /> property to get a
	///     list of all family names available and then the <see cref="UIKit.UIFont.FontNamesForFamilyName(System.String)" />
	///     method to get a list of the fonts available in a particular font family.
	///   </para>
	///   <format type="text/html">
	///     <h2>System Information and Fonts</h2>
	///   </format>
	///   <para>
	///     To get a normal, italic or bold font representing the system font, use the static methods
	///     <see cref="UIKit.UIFont.CreateSystemFont(nfloat,UIKit.UIFontWeight)" />,
	///     <see cref="UIKit.UIFont.CreateItalicSystemFont(nfloat)" /> and
	///     <see cref="UIKit.UIFont.CreateBoldSystemFont(nfloat)" />.
	///   </para>
	///   <format type="text/html">
	///     <h2>Activating Typographic Features</h2>
	///   </format>
	///   <para>
	///     You can retrieve a new font based on an existing font instance by using font descriptors.
	///     To do this, you fetch the immutable <see cref="UIKit.UIFont.FontDescriptor" /> property from
	///	    your font and use it to create a new font descriptor with your
	///	    desired changes, which you can then pass to the <see cref="UIKit.UIFont.CreateFromDescriptor(UIKit.UIFontDescriptor,nfloat)" />
	///     method to create the altered font.
	///   </para>
	///   <para>
	///     The following example shows how to alter the existing
	///	    font to enable the CoreText typographic features for
	///	    proportional numbers and to use the character alternative:
	///	  </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	/// UIFont? CustomizeFont (UIFont font)
	/// {
	///     var originalDescriptor = font.FontDescriptor;
	///
	///     // This font has a character alternative, at index 1, use that:
	///     const int characterAlternative = 1;
	///
	///     var attributes = new UIFontAttributes (
	///	        new UIFontFeature (CTFontFeatureNumberSpacing.Selector.ProportionalNumbers),
	///	        new UIFontFeature (characterAlternative));
	///     var newDesc = originalDescriptor.CreateWithAttributes (attributes);
	///     return UIFont.CreateFromDescriptor (newDesc, 80);
	/// }
	///        ]]></code>
	///   </example>
	///   <format type="text/html">
	///     <h2>Limiting Character Coverage of a Font</h2>
	///   </format>
	///   <para>
	///     The following example alters a font descriptor by altering the
	///     character set supported by the font and forces the system to
	///     use a glyph from the list of fallback fonts in the system.
	///     For example, if you were to use the Menlo font, the following
	///     example would remove the use of the Menlo snowman character
	///     (At Unicode 0x2603), and fall back to the system snowman:
	///   </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	/// var label = new UILabel (new CGRect (0, 300, 200, 100)) {
	///	    Text = "Hello \u2603 there!",
	///	    TextColor = UIColor.White,
	///	    Font = MyFonts.Menlo
	/// };
	///
	/// var originalDescriptor = label.Font.FontDescriptor;
	/// var originalCharSet = originalDescriptor.CharacterSet;
	///
	/// // Make a copy of the character set to modify
	/// var charSetCopy = originalCharSet.MutableCopy () as NSMutableCharacterSet;
	///
	/// // Remove the snowman
	/// charSetCopy.RemoveCharacters (new NSRange (0x2603, 1));
	///
	/// // Create new descriptor
	/// var removedSnowmanDescriptor = originalDescriptor.CreateWithAttributes (new UIFontAttributes () {
	///	    CharacterSet =  originalCharSet
	/// });
	///
	/// // Create the font and set it on our label
	/// label.Font = UIFont.CreateFromDescriptor (removedSnowmanDescriptor, 0);
	///	]]></code>
	///   </example>
	///   <para tool="threads">The members of this class can be used from a background thread.</para>
	/// </remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/UIKit/Reference/UIFont_Class/index.html">Apple documentation for <c>UIFont</c></related>
	public partial class UIFont {
#else
	/// <summary>Font representation for AppKit classes.</summary>
	/// <remarks>
	///   <para>
	///     <see cref="NSFont" /> objects are not instantiated directly, but instead are created from static methods on this class.
	///     Use <see cref="CreateFromName(string, nfloat)" /> to create a named font, or
	///     <see cref="CreateFromDescriptor(NSFontDescriptor, nfloat)" /> to create a font matching a font descriptor.
	///     These methods return <see langword="null" /> when no matching font exists.
	///   </para>
	///   <para>
	///     Use <see cref="CreateSystemFont(nfloat)" />, <see cref="CreateBoldSystemFont(nfloat)" />, or
	///     <see cref="CreateMonospacedSystemFont(nfloat, nfloat)" /> to create system fonts.
	///     <see cref="FontDescriptor" /> describes an existing font, and <see cref="CreateWithSize(nfloat)" />
	///     creates a font with the same characteristics at a different point size.
	///   </para>
	///   <para>
	///     Font factories return independent managed wrappers, even when the system caches the native font.
	///     Each wrapper can be disposed independently.
	///   </para>
	///   <para tool="threads">The members of this class can be used from a background thread.</para>
	/// </remarks>
	public partial class NSFont {
#endif
		// In this case we want to _always_ return a different managed instance
		// so one can be disposed without affecting others
		// ref: https://bugzilla.xamarin.com/show_bug.cgi?id=25511

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		/// <returns>A string containing the font name and point size.</returns>
		public override string ToString ()
		{
			return String.Format ("{0} {1}", FontName, PointSize);
		}

		/// <summary>Determines whether the specified object is equal to the current <see cref="XFont" />.</summary>
		/// <param name="obj">The object to compare with the current font.</param>
		/// <returns><see langword="true" /> if the specified object is equal to the current font; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public override bool Equals (object? obj)
		{
			return base.Equals (obj);
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>A hash code for this instance.</returns>
		/// <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return GetNativeHash ().GetHashCode ();
		}

		/// <summary>Compares two <see cref="XFont" /> objects for equality.</summary>
		/// <param name="f1">The first font to compare.</param>
		/// <param name="f2">The second font to compare.</param>
		/// <returns><see langword="true" /> if the two fonts are equal; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static bool operator == (XFont? f1, XFont? f2)
		{
			if (f1 is null)
				return f2 is null;
			else if (f2 is null)
				return false;
			return f1.Equals (f2);
		}

		/// <summary>Compares two <see cref="XFont" /> objects for inequality.</summary>
		/// <param name="f1">The first font to compare.</param>
		/// <param name="f2">The second font to compare.</param>
		/// <returns><see langword="true" /> if the two fonts are not equal; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static bool operator != (XFont? f1, XFont? f2)
		{
			return !(f1 == f2);
		}

#if __MACOS__
		/// <summary>Creates a font with the specified name and transformation matrix.</summary>
		/// <param name="name">The PostScript font name.</param>
		/// <param name="matrix">The six matrix elements in the order a, b, c, d, tx, ty.</param>
		/// <returns>An independently disposable font wrapper, or <see langword="null" /> if the font is unavailable.</returns>
		/// <exception cref="ArgumentNullException">The name or matrix is null.</exception>
		/// <exception cref="ArgumentException">The matrix does not contain exactly six elements.</exception>
		public static unsafe NSFont? CreateFromName (string name, nfloat [] matrix)
		{
			ArgumentNullException.ThrowIfNull (name);
			ArgumentNullException.ThrowIfNull (matrix);
			if (matrix.Length != 6)
				throw new ArgumentException ("A font matrix must contain exactly six elements.", nameof (matrix));
			fixed (nfloat* pointer = matrix)
				return _CreateFromName (name, (IntPtr) pointer);
		}

		/// <summary>Gets a copy of the font's six-element transformation matrix.</summary>
		public unsafe nfloat [] Matrix {
			get {
				var matrix = new ReadOnlySpan<nfloat> ((void*) _Matrix, 6).ToArray ();
				GC.KeepAlive (this);
				return matrix;
			}
		}

		/// <summary>Gets a copy of the six-element identity font matrix.</summary>
		public static unsafe nfloat [] IdentityMatrix {
			get {
				var matrix = _IdentityMatrix;
				// AppKit uses a null pointer to represent the identity matrix.
				if (matrix == IntPtr.Zero)
					return [1, 0, 0, 1, 0, 0];
				return new ReadOnlySpan<nfloat> ((void*) matrix, 6).ToArray ();
			}
		}

		/// <summary>Creates an independently disposable wrapper for a Core Text font.</summary>
		/// <param name="font">The font to wrap, or <see langword="null" />.</param>
		/// <returns>A new managed wrapper, or <see langword="null" /> if <paramref name="font" /> is null.</returns>
		public static NSFont? CreateFromCTFont (CTFont? font)
		{
			if (font is null)
				return null;
			var result = new NSFont (font.Handle);
			GC.KeepAlive (font);
			return result;
		}

		/// <summary>Gets the bounding rectangle of each glyph.</summary>
		/// <param name="glyphs">The glyph identifiers to measure.</param>
		/// <returns>The glyph bounding rectangles, in the same order as <paramref name="glyphs" />.</returns>
		public unsafe CGRect [] GetBoundingRects (CGGlyph [] glyphs)
		{
			ArgumentNullException.ThrowIfNull (glyphs);
			if (glyphs.Length < 1)
				throw new ArgumentException ("glyphs array is empty");

			CGRect [] bounds = new CGRect [glyphs.Length];
			fixed (CGRect* boundsPtr = bounds) {
				fixed (CGGlyph* glyphsPtr = glyphs) {
					_GetBoundingRects ((IntPtr) boundsPtr, (IntPtr) glyphsPtr, (nuint) glyphs.Length);
				}
			}
			return bounds;
		}

		/// <summary>Gets the advancement of each glyph.</summary>
		/// <param name="glyphs">The glyph identifiers to measure.</param>
		/// <returns>The glyph advancements, in the same order as <paramref name="glyphs" />.</returns>
		public unsafe CGSize [] GetAdvancements (CGGlyph [] glyphs)
		{
			ArgumentNullException.ThrowIfNull (glyphs);
			if (glyphs.Length < 1)
				throw new ArgumentException ("glyphs array is empty");

			CGSize [] advancements = new CGSize [glyphs.Length];
			fixed (CGSize* advancementsPtr = advancements) {
				fixed (CGGlyph* glyphsPtr = glyphs) {
					_GetAdvancements ((IntPtr) advancementsPtr, (IntPtr) glyphsPtr, (nuint) glyphs.Length);
				}
			}
			return advancements;
		}

		/// <summary>Gets the bounding rectangles of legacy glyphs.</summary>
		/// <param name="glyphs">The legacy glyph identifiers.</param>
		/// <returns>The bounding rectangles in the same order as the glyphs.</returns>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.13", "Use 'GetBoundingRects' with Core Graphics glyphs instead.")]
		public unsafe CGRect [] GetBoundingRects (uint [] glyphs)
		{
			ArgumentNullException.ThrowIfNull (glyphs);
			var bounds = new CGRect [glyphs.Length];
			if (glyphs.Length == 0)
				return bounds;
			fixed (CGRect* boundsPointer = bounds)
			fixed (uint* glyphsPointer = glyphs)
				_GetBoundingRectsForGlyphs ((IntPtr) boundsPointer, (IntPtr) glyphsPointer, (nuint) glyphs.Length);
			return bounds;
		}

		/// <summary>Gets the advancements of legacy glyphs.</summary>
		/// <param name="glyphs">The legacy glyph identifiers.</param>
		/// <returns>The advancements in the same order as the glyphs.</returns>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.13", "Use 'GetAdvancements' with Core Graphics glyphs instead.")]
		public unsafe CGSize [] GetAdvancements (uint [] glyphs)
		{
			ArgumentNullException.ThrowIfNull (glyphs);
			var advancements = new CGSize [glyphs.Length];
			if (glyphs.Length == 0)
				return advancements;
			fixed (CGSize* advancementsPointer = advancements)
			fixed (uint* glyphsPointer = glyphs)
				_GetAdvancementsForGlyphs ((IntPtr) advancementsPointer, (IntPtr) glyphsPointer, (nuint) glyphs.Length);
			return advancements;
		}

#endif
	}
}
