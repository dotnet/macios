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
using XFontDescriptor = AppKit.NSFontDescriptor;
namespace AppKit {
#else
using XFont = UIKit.UIFont;
using XFontDescriptor = UIKit.UIFontDescriptor;
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

		/// <inheritdoc cref="CreateMonospacedDigitSystemFont(nfloat, nfloat)" />
#if __MACOS__
		public static XFont MonospacedDigitSystemFontOfSize (nfloat fontSize, nfloat weight) => CreateMonospacedDigitSystemFont (fontSize, weight);
#else
		public static XFont MonospacedDigitSystemFontOfSize (nfloat size, nfloat weight) => CreateMonospacedDigitSystemFont (size, weight);
#endif

		/// <summary>Creates a font using <see cref="GetMonospacedSystemFont(nfloat, nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="size">The font size in points.</param>
		/// <param name="weight">The font weight.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static XFont CreateMonospacedSystemFont (nfloat size, nfloat weight) => GetMonospacedSystemFont (size, weight);

		/// <summary>Creates a font matching a font descriptor.</summary>
		/// <param name="descriptor">The descriptor to match.</param>
		/// <param name="pointSize">The font size in points, or zero to use the descriptor's size.</param>
		/// <returns>A new managed font wrapper, or <see langword="null" /> if no matching font exists.</returns>
		public static XFont? CreateFromDescriptor (XFontDescriptor descriptor, nfloat pointSize) => FromDescriptor (descriptor, pointSize);

		/// <summary>Creates a font from the specified font descriptor.</summary>
		/// <param name="descriptor">The font descriptor describing the font to create.</param>
		/// <param name="pointSize">The point size, or zero to use the descriptor's size.</param>
		/// <returns>A new font based on the descriptor, or <see langword="null" /> if no matching font exists.</returns>
		/// <remarks>The nullable return type is shared across platforms, although UIKit always supplies a matching font.</remarks>
		public static XFont? FromDescriptor (XFontDescriptor descriptor, nfloat pointSize) => _FromDescriptor (descriptor, pointSize);

		/// <summary>Creates a font using <see cref="FromName(string, nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="name">The font name.</param>
		/// <param name="size">The font size in points.</param>
		/// <returns>A new managed font wrapper, or <see langword="null" /> if the font could not be created.</returns>
		public static XFont? CreateFromName (string name, nfloat size) => FromName (name, size);

		/// <inheritdoc cref="CreateSystemFont(nfloat)" />
#if __MACOS__
		public static XFont SystemFontOfSize (nfloat fontSize) => CreateSystemFont (fontSize);
#else
		public static XFont SystemFontOfSize (nfloat size) => CreateSystemFont (size);
#endif

		/// <inheritdoc cref="CreateSystemFont(nfloat, nfloat)" />
#if __MACOS__
		public static XFont SystemFontOfSize (nfloat fontSize, nfloat weight) => CreateSystemFont (fontSize, weight);
#else
		public static XFont SystemFontOfSize (nfloat size, nfloat weight) => CreateSystemFont (size, weight);
#endif

		/// <inheritdoc cref="CreateBoldSystemFont(nfloat)" />
#if __MACOS__
		public static XFont BoldSystemFontOfSize (nfloat fontSize) => CreateBoldSystemFont (fontSize);
#else
		public static XFont BoldSystemFontOfSize (nfloat size) => CreateBoldSystemFont (size);
#endif

		/// <summary>Determines whether the specified object is equal to the current <see cref="XFont" />.</summary>
		/// <param name="obj">The object to compare with the current font.</param>
		/// <returns><see langword="true" /> if the specified object is equal to the current font; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public override bool Equals (object? obj)
		{
#if __MACOS__
			return base.Equals (obj);
#else
			var font = obj as XFont;
			return this == font;
#endif
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>A hash code for this instance.</returns>
		/// <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return GetNativeHash ().GetHashCode ();
		}

		/// <summary>Compares two <see cref="XFont" /> objects for value equality.</summary>
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
#if __MACOS__
			return f1.Equals ((object) f2);
#else
			return f1.Handle == f2.Handle;
#endif
		}

		/// <summary>Compares two <see cref="XFont" /> objects for value inequality.</summary>
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

		/// <summary>Creates a font using <see cref="SystemFontOfSize(nfloat, nfloat, nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <param name="weight">The font weight.</param>
		/// <param name="width">The font width.</param>
		/// <returns>A new managed font wrapper.</returns>
#if __MACOS__
		[SupportedOSPlatform ("macos")]
#else
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public static XFont CreateSystemFont (nfloat fontSize, nfloat weight, nfloat width) => SystemFontOfSize (fontSize, weight, width);

		/// <summary>Creates a font with the same characteristics and a different size.</summary>
		/// <param name="size">The new font size in points.</param>
		/// <returns>A new, independently disposable managed font wrapper.</returns>
		public XFont CreateWithSize (nfloat size) => WithSize (size);

#if __MACOS__
		/// <summary>Returns a new font based on the current one, with the specified size.</summary>
		/// <param name="size">The new font size in points.</param>
		/// <returns>A new <see cref="XFont" /> with the same characteristics as this font but with the specified size.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public virtual XFont WithSize (nfloat size)
		{
			return GetFont (size);
		}

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
				return _FromName (name, (IntPtr) pointer);
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

		/// <summary>Creates a font using <see cref="FromCTFont(CTFont)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="font">The Core Text font to wrap.</param>
		/// <returns>A new managed font wrapper, or <see langword="null" /> if the font could not be created.</returns>
		public static NSFont? CreateFromCTFont (CTFont? font) => FromCTFont (font);

		/// <summary>Creates an independently disposable wrapper for a Core Text font.</summary>
		/// <param name="font">The font to wrap, or <see langword="null" />.</param>
		/// <returns>A new managed wrapper, or <see langword="null" /> if <paramref name="font" /> is null.</returns>
		public static NSFont? FromCTFont (CTFont? font)
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
			if (glyphs is null)
				throw new ArgumentNullException ("glyphs");
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
			if (glyphs is null)
				throw new ArgumentNullException ("glyphs");
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

		/// <inheritdoc cref="FromName(string, nfloat)" />
		public static NSFont? FromFontName (string fontName, nfloat fontSize)
		{
			ArgumentNullException.ThrowIfNull (fontName);
			return FromName (fontName, fontSize);
		}

		/// <inheritdoc cref="FromDescriptor(NSFontDescriptor, nfloat)" />
		public static NSFont? FromDescription (NSFontDescriptor fontDescriptor, nfloat fontSize)
		{
			ArgumentNullException.ThrowIfNull (fontDescriptor);
			return FromDescriptor (fontDescriptor, fontSize);
		}

		/// <inheritdoc cref="CreateFromDescriptor(NSFontDescriptor, NSAffineTransform)" />
		public static NSFont? FromDescription (NSFontDescriptor fontDescriptor, NSAffineTransform? textTransform) => CreateFromDescriptor (fontDescriptor, textTransform);

		/// <summary>Creates a font using <see cref="UserFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper, or <see langword="null" /> if the font could not be created.</returns>
		public static NSFont? CreateUserFont (nfloat fontSize) => UserFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="UserFixedPitchFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper, or <see langword="null" /> if the font could not be created.</returns>
		public static NSFont? CreateUserFixedPitchFont (nfloat fontSize) => UserFixedPitchFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="LabelFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreateLabelFont (nfloat fontSize) => LabelFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="TitleBarFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreateTitleBarFont (nfloat fontSize) => TitleBarFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="MenuFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreateMenuFont (nfloat fontSize) => MenuFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="MenuBarFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreateMenuBarFont (nfloat fontSize) => MenuBarFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="MessageFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreateMessageFont (nfloat fontSize) => MessageFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="PaletteFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreatePaletteFont (nfloat fontSize) => PaletteFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="ToolTipsFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreateToolTipsFont (nfloat fontSize) => ToolTipsFontOfSize (fontSize);

		/// <summary>Creates a font using <see cref="ControlContentFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreateControlContentFont (nfloat fontSize) => ControlContentFontOfSize (fontSize);

		/// <summary>Creates a vertical version of this font.</summary>
		/// <returns>A new, independently disposable managed font wrapper.</returns>
		public NSFont CreateVerticalFont () => GetVerticalFont ();

		/// <summary>Gets the system font size for a control size.</summary>
		/// <param name="controlSize">The control size.</param>
		/// <returns>The system font size in points.</returns>
		public static nfloat GetSystemFontSize (NSControlSize controlSize) => SystemFontSizeForControlSize (controlSize);

		/// <inheritdoc cref="GetMonospacedSystemFont(nfloat, nfloat)" />
		public static NSFont MonospacedSystemFont (nfloat fontSize, nfloat weight)
		{
			return GetMonospacedSystemFont (fontSize, weight);
		}

		/// <summary>Creates a font using <see cref="GetPreferredFont(string, NSDictionary)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="textStyle">The text style.</param>
		/// <param name="options">The font selection options.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static NSFont CreatePreferredFont (string textStyle, NSDictionary options) => GetPreferredFont (textStyle, options);

		/// <summary>Gets the font name.</summary>
		public string Name => FontName;
#else

		/// <summary>Gets the names of the fonts in a font family.</summary>
		/// <param name="familyName">The font family name.</param>
		/// <returns>The font names available in the specified family.</returns>
		public static string [] GetFontNames (string familyName) => FontNamesForFamilyName (familyName);

		/// <summary>The preferred <see cref="UIFont" /> for the Headline text style.</summary>
		/// <value>The preferred font for headline text.</value>
		/// <remarks>
		///   <para>To implement dynamic type, application developers must act in reaction to <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" /> (see remarks and example at <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />).</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredHeadline {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Headline);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Body text style.</summary>
		/// <value>The preferred font for body text.</value>
		/// <remarks>
		///   <para>To implement dynamic type, application developers must act in reaction to <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" /> (see remarks and example at <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />).</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredBody {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Body);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Subheadline text style.</summary>
		/// <value>The preferred font for subheadline text.</value>
		/// <remarks>
		///   <para>To implement dynamic type, application developers must act in reaction to <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" /> (see remarks and example at <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />).</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredSubheadline {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Subheadline);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Footnote text style.</summary>
		/// <value>The preferred font for footnote text.</value>
		/// <remarks>
		///   <para>To implement dynamic type, application developers must act in reaction to <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" /> (see remarks and example at <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />).</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredFootnote {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Footnote);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Caption1 text style.</summary>
		/// <value>The preferred font for primary caption text.</value>
		/// <remarks>
		///   <para>To implement dynamic type, application developers must act in reaction to <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" /> (see remarks and example at <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />).</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredCaption1 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Caption1);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Caption2 text style.</summary>
		/// <value>The preferred font for secondary caption text.</value>
		/// <remarks>
		///   <para>To implement dynamic type, application developers must act in reaction to <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" /> (see remarks and example at <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />).</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredCaption2 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Caption2);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Title1 text style.</summary>
		/// <value>The preferred font for primary title text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredTitle1 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title1);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Title2 text style.</summary>
		/// <value>The preferred font for secondary title text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredTitle2 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title2);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Title3 text style.</summary>
		/// <value>The preferred font for tertiary title text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredTitle3 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title3);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Callout text style.</summary>
		/// <value>The preferred font for callout text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont PreferredCallout {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Callout);
			}
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos16.0")]
		static nfloat GetFontWidth (UIFontWidth width)
		{
			switch (width) {
			case UIFontWidth.Condensed:
				return UIFontWidthConstants.Condensed;
			case UIFontWidth.Standard:
				return UIFontWidthConstants.Standard;
			case UIFontWidth.Expanded:
				return UIFontWidthConstants.Expanded;
			case UIFontWidth.Compressed:
				return UIFontWidthConstants.Compressed;
			default:
				throw new ArgumentException (width.ToString ());
			}
		}

		/// <summary>Creates a font using <see cref="SystemFontOfSize(nfloat, UIFontWeight)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="size">The font size in points.</param>
		/// <param name="weight">The font weight.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static UIFont CreateSystemFont (nfloat size, UIFontWeight weight) => SystemFontOfSize (size, weight);

		/// <summary>Gets the system font for the specified <paramref name="size" /> and <paramref name="weight" />.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The system font with the specified size and weight.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont SystemFontOfSize (nfloat size, UIFontWeight weight)
		{
			return SystemFontOfSize (size, weight.GetWeight ());
		}

		/// <summary>Creates a font using <see cref="MonospacedDigitSystemFontOfSize(nfloat, UIFontWeight)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <param name="weight">The font weight.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static UIFont CreateMonospacedDigitSystemFont (nfloat fontSize, UIFontWeight weight) => MonospacedDigitSystemFontOfSize (fontSize, weight);

		/// <summary>Gets the monospaced <see cref="UIFont" /> preferred by the system for displaying digits, of the specified <paramref name="fontSize" /> and <paramref name="weight" />.</summary>
		/// <param name="fontSize">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The monospaced digit system font.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont MonospacedDigitSystemFontOfSize (nfloat fontSize, UIFontWeight weight)
		{
			return MonospacedDigitSystemFontOfSize (fontSize, weight.GetWeight ());
		}

		/// <summary>Creates a font using <see cref="GetMonospacedSystemFont(nfloat, UIFontWeight)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="size">The font size in points.</param>
		/// <param name="weight">The font weight.</param>
		/// <returns>A new managed font wrapper.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static UIFont CreateMonospacedSystemFont (nfloat size, UIFontWeight weight) => GetMonospacedSystemFont (size, weight);

		/// <summary>Gets the monospaced system font of the specified size and weight.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The monospaced system font.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static UIFont GetMonospacedSystemFont (nfloat size, UIFontWeight weight) => GetMonospacedSystemFont (size, weight.GetWeight ());

		/// <summary>Creates a font using <see cref="GetPreferredFontForTextStyle(NSString)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="uiFontTextStyle">The text style.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static UIFont CreatePreferredFont (NSString uiFontTextStyle) => GetPreferredFontForTextStyle (uiFontTextStyle);

		/// <summary>Creates a font using <see cref="GetPreferredFontForTextStyle(UIFontTextStyle)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="uiFontTextStyle">The text style.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static UIFont CreatePreferredFont (UIFontTextStyle uiFontTextStyle) => GetPreferredFontForTextStyle (uiFontTextStyle);

		/// <summary>Gets the preferred font for the specified text style.</summary>
		/// <param name="uiFontTextStyle">The style for which to get the preferred font.</param>
		/// <returns>The preferred <see cref="UIFont" /> for the specified text style.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont GetPreferredFontForTextStyle (UIFontTextStyle uiFontTextStyle)
		{
			return GetPreferredFontForTextStyle (uiFontTextStyle.GetConstant () ?? throw new ArgumentOutOfRangeException (nameof (uiFontTextStyle)));
		}

		/// <summary>Creates a font using <see cref="GetPreferredFontForTextStyle(NSString, UITraitCollection)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="uiFontTextStyle">The text style.</param>
		/// <param name="traitCollection">The trait collection, or null for the current traits.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static UIFont CreatePreferredFont (NSString uiFontTextStyle, UITraitCollection? traitCollection) => GetPreferredFontForTextStyle (uiFontTextStyle, traitCollection);

		/// <summary>Creates a font using <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="uiFontTextStyle">The text style.</param>
		/// <param name="traitCollection">The trait collection, or null for the current traits.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static UIFont CreatePreferredFont (UIFontTextStyle uiFontTextStyle, UITraitCollection? traitCollection) => GetPreferredFontForTextStyle (uiFontTextStyle, traitCollection);

		/// <summary>Gets the <see cref="UIFont" /> that is preferred by the system for the specified <paramref name="uiFontTextStyle" /> and <paramref name="traitCollection" />.</summary>
		/// <param name="uiFontTextStyle">The style for which to get the preferred font.</param>
		/// <param name="traitCollection">The trait collection for which to get the preferred font.</param>
		/// <returns>The preferred <see cref="UIFont" /> for the specified text style and trait collection.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont GetPreferredFontForTextStyle (UIFontTextStyle uiFontTextStyle, UITraitCollection? traitCollection)
		{
			return GetPreferredFontForTextStyle (uiFontTextStyle.GetConstant () ?? throw new ArgumentOutOfRangeException (nameof (uiFontTextStyle)), traitCollection);
		}

		/// <summary>Creates a font using <see cref="SystemFontOfSize(nfloat, UIFontWeight, UIFontWidth)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="fontSize">The font size in points.</param>
		/// <param name="weight">The font weight.</param>
		/// <param name="width">The font width.</param>
		/// <returns>A new managed font wrapper.</returns>
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos16.0")]
		public static UIFont CreateSystemFont (nfloat fontSize, UIFontWeight weight, UIFontWidth width) => SystemFontOfSize (fontSize, weight, width);

		/// <summary>Creates a system font of the specified size, weight, and width.</summary>
		/// <param name="fontSize">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <param name="width">The width of the font.</param>
		/// <returns>A new system <see cref="UIFont" /> of the specified size, weight, and width.</returns>
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos16.0")]
		public static UIFont SystemFontOfSize (nfloat fontSize, UIFontWeight weight, UIFontWidth width)
		{
			return SystemFontOfSize (fontSize, weight.GetWeight (), GetFontWidth (width));
		}

		/// <summary>Creates a font using <see cref="ItalicSystemFontOfSize(nfloat)" />. The returned wrapper can be disposed independently of other fonts.</summary>
		/// <param name="size">The font size in points.</param>
		/// <returns>A new managed font wrapper.</returns>
		public static UIFont CreateItalicSystemFont (nfloat size) => ItalicSystemFontOfSize (size);

		/// <summary>Gets the font name.</summary>
		public string FontName => Name;
#endif
	}
}
