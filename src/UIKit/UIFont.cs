// 
// UIFont.cs: Implements the managed UIFont
//
// Authors:
//   Geoff Norton.
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2009 Novell, Inc
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace UIKit {

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
	///     Use the static <see cref="UIKit.UIFont.FromName(System.String,nfloat)" /> method to create new fonts of a particular font.
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
	///     <see cref="UIKit.UIFont.SystemFontOfSize(nfloat,UIKit.UIFontWeight)" />,
	///     <see cref="UIKit.UIFont.ItalicSystemFontOfSize(nfloat)" /> and
	///     <see cref="UIKit.UIFont.BoldSystemFontOfSize(nfloat)" />.
	///   </para>
	///   <format type="text/html">
	///     <h2>Activating Typographic Features</h2>
	///   </format>
	///   <para>
	///     You can retrieve a new font based on an existing font instance by using font descriptors.
	///     To do this, you fetch the immutable <see cref="UIKit.UIFont.FontDescriptor" /> property from
	///	    your font and use it to create a new font descriptor with your
	///	    desired changes, which then you use to can pass to the <see cref="UIKit.UIFont.FromDescriptor(UIKit.UIFontDescriptor,nfloat)" />
	///     method to create the altered font.
	///   </para>
	///   <para>
	///     The following example shows how to alter the existing
	///	    font to enable the CoreText typographic features for
	///	    proportional numbers and to use the character alternative:
	///	  </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	/// UIFont CustomizeFont (UIFont font)
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
	///     return UIFont.FromDescriptor (newDesc, 80);
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
	///     example would remove the use of the Melon Snowman character
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
	/// label.Font = UIFont.FromDescriptor (removedSnowmanDescriptor, 0);
	///	]]></code>
	///   </example>
	///   <para tool="threads">The members of this class can be used from a background thread.</para>
	/// </remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/UIKit/Reference/UIFont_Class/index.html">Apple documentation for <c>UIFont</c></related>
	public partial class UIFont {
		/// <summary>Returns a string representation of the value of the current instance.</summary>
		/// <returns>A string containing the font name and point size.</returns>
		public override string ToString ()
		{
			return String.Format ("{0} {1}", Name, PointSize);
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Headline text style.</summary>
		/// <value>The preferred font for headline text.</value>
		/// <remarks>
		///   <para>To implement dynamic type, application developers must act in reaction to <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" /> (see remarks and example at <see cref="GetPreferredFontForTextStyle(UIFontTextStyle, UITraitCollection)" />).</para>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? PreferredHeadline {
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
		public static UIFont? PreferredBody {
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
		public static UIFont? PreferredSubheadline {
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
		public static UIFont? PreferredFootnote {
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
		public static UIFont? PreferredCaption1 {
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
		public static UIFont? PreferredCaption2 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Caption2);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Title1 text style.</summary>
		/// <value>The preferred font for primary title text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? PreferredTitle1 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title1);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Title2 text style.</summary>
		/// <value>The preferred font for secondary title text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? PreferredTitle2 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title2);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Title3 text style.</summary>
		/// <value>The preferred font for tertiary title text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? PreferredTitle3 {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Title3);
			}
		}

		/// <summary>The preferred <see cref="UIFont" /> for the Callout text style.</summary>
		/// <value>The preferred font for callout text.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? PreferredCallout {
			get {
				return GetPreferredFontForTextStyle (UIFontTextStyle.Callout);
			}
		}

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
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

		/// <summary>Gets the system font for the specified <paramref name="size" /> and <paramref name="weight" />.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The system font with the specified size and weight.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? SystemFontOfSize (nfloat size, UIFontWeight weight)
		{
			return SystemFontOfSize (size, weight.GetWeight ());
		}

		/// <summary>Returns the system monospaced font specialized for digits, in the specified size and weight.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The monospaced digit system font, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? MonospacedDigitSystemFontOfSize (nfloat size, nfloat weight)
		{
			var ptr = _MonospacedDigitSystemFontOfSize (size, weight);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Gets the monospaced <see cref="UIFont" /> preferred by the system for displaying digits, of the specified <paramref name="fontSize" /> and <paramref name="weight" />.</summary>
		/// <param name="fontSize">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The monospaced digit system font, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? MonospacedDigitSystemFontOfSize (nfloat fontSize, UIFontWeight weight)
		{
			return MonospacedDigitSystemFontOfSize (fontSize, weight.GetWeight ());
		}

		/// <summary>Gets the monospaced system font of the specified size and weight.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The monospaced system font, or <see langword="null" /> if the font could not be created.</returns>
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static UIFont? GetMonospacedSystemFont (nfloat size, nfloat weight)
		{
			var ptr = _MonospacedSystemFontOfSize (size, weight);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Gets the monospaced system font of the specified size and weight.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>The monospaced system font, or <see langword="null" /> if the font could not be created.</returns>
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static UIFont? GetMonospacedSystemFont (nfloat size, UIFontWeight weight) => GetMonospacedSystemFont (size, weight.GetWeight ());

		// In this case we want to _always_ return a different managed instance
		// so one can be disposed without affecting others
		// ref: https://bugzilla.xamarin.com/show_bug.cgi?id=25511

		/// <summary>Gets the preferred font for the specified text style.</summary>
		/// <param name="uiFontTextStyle">Name of one of the built-in system text styles.</param>
		/// <returns>The preferred <see cref="UIFont" /> for the specified text style, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para>You can instead use the <see cref="PreferredBody" />, <see cref="PreferredCaption1" />, <see cref="PreferredCaption2" />, <see cref="PreferredHeadline" />, <see cref="PreferredSubheadline" /> properties to get this information.</para>
		///   <para>Using these methods to obtain an initial font during view initialization is not sufficient to implement dynamic type. After the application user has set the "Text Size Property" in Settings, the application will receive a notification via <see cref="UIApplication.Notifications.ObserveContentSizeCategoryChanged(EventHandler{UIContentSizeCategoryChangedEventArgs})" />. It is the application developer's responsibility, at that point, to invalidate the layout in all view elements that should be resized. The simplest way to do that is to have a method that re-sets the font in all components that support Dynamic Type:</para>
		///   <example>
		///     <code lang="csharp lang-csharp"><![CDATA[
		/// UIApplication.Notifications.ObserveContentSizeCategoryChanged((s,e) => {
		/// 	SetDynamicTypeFonts();
		/// });
		///
		/// //Call this when initializing, and also in response to ObserveContentSizeCategoryChanged notifications
		/// private void SetDynamicTypeFonts()
		/// {
		/// 	headlineLabel.Font = UIFont.PreferredFontForTextStyle(UIFontTextStyle.Headline);
		/// 	bodyText.Font = UIFont.PreferredFontForTextStyle(UIFontTextStyle.Body);
		///   //...etc...
		/// }
		///     ]]></code>
		///   </example>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? GetPreferredFontForTextStyle (NSString uiFontTextStyle)
		{
			var ptr = _GetPreferredFontForTextStyle (uiFontTextStyle);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Gets the preferred font for the specified text style.</summary>
		/// <param name="uiFontTextStyle">The style for which to get the preferred font.</param>
		/// <returns>The preferred <see cref="UIFont" /> for the specified text style.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? GetPreferredFontForTextStyle (UIFontTextStyle uiFontTextStyle)
		{
			return GetPreferredFontForTextStyle (uiFontTextStyle.GetConstant ()!);
		}

		/// <summary>Gets the preferred font for the specified text style and trait collection.</summary>
		/// <param name="uiFontTextStyle">The style for which to get the preferred font.</param>
		/// <param name="traitCollection">The trait collection for which to get the preferred font.</param>
		/// <returns>The preferred <see cref="UIFont" /> for the specified text style and trait collection, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? GetPreferredFontForTextStyle (NSString uiFontTextStyle, UITraitCollection traitCollection)
		{
			var ptr = _GetPreferredFontForTextStyle (uiFontTextStyle, traitCollection);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Gets the <see cref="UIFont" /> that is preferred by the system for the specified <paramref name="uiFontTextStyle" /> and <paramref name="traitCollection" />.</summary>
		/// <param name="uiFontTextStyle">The style for which to get the preferred font.</param>
		/// <param name="traitCollection">The trait collection for which to get the preferred font.</param>
		/// <returns>The preferred <see cref="UIFont" /> for the specified text style and trait collection.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? GetPreferredFontForTextStyle (UIFontTextStyle uiFontTextStyle, UITraitCollection traitCollection)
		{
			return GetPreferredFontForTextStyle (uiFontTextStyle.GetConstant ()!, traitCollection);
		}

		/// <summary>Creates a <see cref="UIFont" /> from the specified font descriptor.</summary>
		/// <param name="descriptor">The font descriptor describing the font to create.</param>
		/// <param name="pointSize">The point size of the font to create. If 0, the size from the descriptor is used.</param>
		/// <returns>A new <see cref="UIFont" /> based on the descriptor, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? FromDescriptor (UIFontDescriptor descriptor, nfloat pointSize)
		{
			var ptr = _FromDescriptor (descriptor, pointSize);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Creates a font with the specified name and size.</summary>
		/// <param name="name">The name of the font to create.</param>
		/// <param name="size">The size of the font in points.</param>
		/// <returns>A new <see cref="UIFont" /> with the specified name and size, or <see langword="null" /> if the font could not be found.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? FromName (string name, nfloat size)
		{
			var ptr = _FromName (name, size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Creates a system font of the specified size.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <returns>A new system <see cref="UIFont" /> of the specified size, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? SystemFontOfSize (nfloat size)
		{
			var ptr = _SystemFontOfSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Creates a system font of the specified size, weight, and width.</summary>
		/// <param name="fontSize">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <param name="width">The width of the font.</param>
		/// <returns>A new system <see cref="UIFont" /> of the specified size, weight, and width, or <see langword="null" /> if the font could not be created.</returns>
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[SupportedOSPlatform ("tvos16.0")]
		public static UIFont? SystemFontOfSize (nfloat fontSize, UIFontWeight weight, UIFontWidth width)
		{
			var ptr = _SystemFontOfSize (fontSize, weight.GetWeight (), GetFontWidth (width));
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Returns the default system font in the specified <paramref name="size" /> and <paramref name="weight" />.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <param name="weight">The weight of the font.</param>
		/// <returns>A new system <see cref="UIFont" /> of the specified size and weight, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? SystemFontOfSize (nfloat size, nfloat weight)
		{
			var ptr = _SystemFontOfSize (size, weight);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Returns a bold system font of the specified size.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <returns>A new bold system <see cref="UIFont" /> of the specified size, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? BoldSystemFontOfSize (nfloat size)
		{
			var ptr = _BoldSystemFontOfSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Creates an italicized system font of the specified size.</summary>
		/// <param name="size">The size of the font in points.</param>
		/// <returns>A new italic system <see cref="UIFont" /> of the specified size, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFont? ItalicSystemFontOfSize (nfloat size)
		{
			var ptr = _ItalicSystemFontOfSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Returns a new font based on the current one, with the specified size.</summary>
		/// <param name="size">The new font size in points.</param>
		/// <returns>A new <see cref="UIFont" /> with the same characteristics as this font but with the specified size, or <see langword="null" /> if the font could not be created.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public virtual UIFont? WithSize (nfloat size)
		{
			var ptr = _WithSize (size);
			return ptr == IntPtr.Zero ? null : new UIFont (ptr);
		}

		/// <summary>Compares two <see cref="UIFont" /> objects for value equality.</summary>
		/// <param name="f1">The first font to compare.</param>
		/// <param name="f2">The second font to compare.</param>
		/// <returns><see langword="true" /> if the two fonts are equal; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static bool operator == (UIFont? f1, UIFont? f2)
		{
			if (f1 is null)
				return f2 is null;
			else if (f2 is null)
				return false;
			return f1.Handle == f2.Handle;
		}

		/// <summary>Compares two <see cref="UIFont" /> objects for value inequality.</summary>
		/// <param name="f1">The first font to compare.</param>
		/// <param name="f2">The second font to compare.</param>
		/// <returns><see langword="true" /> if the two fonts are not equal; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static bool operator != (UIFont? f1, UIFont? f2)
		{
			return !(f1 == f2);
		}

		/// <summary>Determines whether the specified object is equal to the current <see cref="UIFont" />.</summary>
		/// <param name="obj">The object to compare with the current font.</param>
		/// <returns><see langword="true" /> if the specified object is equal to the current font; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public override bool Equals (object? obj)
		{
			var font = obj as UIFont;
			return this == font;
		}

		/// <summary>Generates a hash code for the current instance.</summary>
		/// <returns>A hash code for this instance.</returns>
		/// <remarks>The algorithm used to generate the hash code is unspecified.</remarks>
		public override int GetHashCode ()
		{
			return GetNativeHash ().GetHashCode ();
		}
	}
}
