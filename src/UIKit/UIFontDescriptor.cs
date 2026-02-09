//
// UIFontDescriptor.cs: Supporting classes for UIFontDescriptor
//
// Author:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2013-2014 Xamarin Inc
//
using CoreGraphics;

#nullable enable

namespace UIKit {

	/// <summary>Attributes used to describe a font, used by <see cref="UIFontDescriptor" />.</summary>
	/// <remarks>
	///   <para>
	///     This type defines the attributes used to describe a font, like
	///     the font family, the font name, the character set, typographic features, glyph
	///     advancement, advanced typesetting features and others.
	///   </para>
	///   <para>
	///     Typically you create objects of this instance to create a <see cref="UIFontDescriptor" />.
	///   </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	/// var attributes = new UIFontAttributes () {
	///     Size = 14,
	///     Matrix = MyTransform
	/// };
	/// ]]></code>
	///   </example>
	/// </remarks>
	public class UIFontAttributes : DictionaryContainer {
		/// <summary>Creates an empty <see cref="UIFontAttributes" />.</summary>
		public UIFontAttributes () { }

#if !COREBUILD
		/// <summary>Creates a <see cref="UIFontAttributes" /> from a weakly typed <see cref="NSDictionary" />.</summary>
		/// <param name="dictionary">Dictionary containing <see cref="UIFontAttributes" />.</param>
		public UIFontAttributes (NSDictionary dictionary) : base (dictionary) { }

		/// <summary>Creates a <see cref="UIFontAttributes" /> with the specified font feature settings.</summary>
		/// <param name="features">The font features to apply.</param>
		public UIFontAttributes (params UIFontFeature [] features)
		{
			FeatureSettings = features;
		}

		/// <summary>The font family.</summary>
		/// <value>The font family name, or <see langword="null" /> if not set.</value>
		public string? Family {
			get {
				return GetStringValue (UIFontDescriptor.FamilyAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.FamilyAttribute, value);
			}
		}

		/// <summary>The font name.</summary>
		/// <value>The font name, or <see langword="null" /> if not set.</value>
		public string? Name {
			get {
				return GetStringValue (UIFontDescriptor.NameAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.NameAttribute, value);
			}
		}

		/// <summary>The font face.</summary>
		/// <value>The font face name, or <see langword="null" /> if not set.</value>
		public string? Face {
			get {
				return GetStringValue (UIFontDescriptor.FaceAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.FaceAttribute, value);
			}
		}

		// Size is encoded as a string, but should contain a float
		/// <summary>The font size.</summary>
		/// <value>The font size, or <see langword="null" /> if not set.</value>
		public float? Size {
			get {
				return GetFloatValue (UIFontDescriptor.SizeAttribute);
			}
			set {
				SetNumberValue (UIFontDescriptor.SizeAttribute, value);
			}
		}

		/// <summary>The font's visible name.</summary>
		/// <value>The visible name, or <see langword="null" /> if not set.</value>
		public string? VisibleName {
			get {
				return GetStringValue (UIFontDescriptor.VisibleNameAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.VisibleNameAttribute, value);
			}
		}

		/// <summary>The text style.</summary>
		/// <value>The text style, or <see langword="null" /> if not set.</value>
		public NSString? TextStyle {
			get {
				return Dictionary [UIFontDescriptor.TextStyleAttribute] as NSString;
			}

			set {
				if (value is null) {
					RemoveValue (UIFontDescriptor.TextStyleAttribute);
					return;
				}
				Dictionary [UIFontDescriptor.TextStyleAttribute] = value;
			}
		}

		/// <summary>The affine transformation matrix for the font.</summary>
		/// <value>The transformation matrix. The default is the identity matrix, or <see langword="null" /> if not set.</value>
		public CGAffineTransform? Matrix {
			get {
				if (!Dictionary.TryGetValue (UIFontDescriptor.MatrixAttribute, out var value))
					return null;

				return ((NSValue) value).CGAffineTransformValue;
			}
			set {
				if (!value.HasValue) {
					RemoveValue (UIFontDescriptor.MatrixAttribute);
					return;
				}
				Dictionary [UIFontDescriptor.MatrixAttribute] = NSValue.FromCGAffineTransform (value.Value);
			}
		}

		/// <summary>The character set supported by the specified font.</summary>
		/// <value>The character set, or <see langword="null" /> if not set.</value>
		public NSCharacterSet? CharacterSet {
			get {
				return Dictionary [UIFontDescriptor.CharacterSetAttribute] as NSCharacterSet;
			}
			set {
				if (value is null) {
					RemoveValue (UIFontDescriptor.CharacterSetAttribute);
					return;
				}
				Dictionary [UIFontDescriptor.CharacterSetAttribute] = value;
			}
		}

		/// <summary>The list of sub descriptors to be used before the system default fallbacks.</summary>
		/// <value>The cascade list of font descriptors. The default is the list of descriptors based on the user locale.</value>
		/// <remarks>
		///   By setting this property, developers can override the list
		///   of <see cref="UIFontDescriptor" /> instances to be used before using the system
		///   defaults.
		/// </remarks>
		public UIFontDescriptor []? CascadeList {
			get {
				return GetArray<UIFontDescriptor> (UIFontDescriptor.CascadeListAttribute);

			}
			set {
				if (value is null) {
					RemoveValue (UIFontDescriptor.CascadeListAttribute);
					return;
				}
				Dictionary [UIFontDescriptor.CascadeListAttribute] = NSArray.FromNSObjects (value);
			}
		}

		/// <summary>The font traits.</summary>
		/// <value>The font traits, or <see langword="null" /> if not set.</value>
		public UIFontTraits? Traits {
			get {
				var traits = GetNSDictionary (UIFontDescriptor.TraitsAttribute);
				if (traits is null)
					return null;
				return new UIFontTraits (traits);
			}
			set {
				if (value is null) {
					RemoveValue (UIFontDescriptor.TraitsAttribute);
					return;
				}

				Dictionary [UIFontDescriptor.TraitsAttribute] = value.Dictionary;
			}
		}

		/// <summary>The override for the font's fixed advance for individual glyphs.</summary>
		/// <value>The fixed advance value. Default value is 0, or <see langword="null" /> if not set.</value>
		public float? FixedAdvance {
			get {
				return GetFloatValue (UIFontDescriptor.FixedAdvanceAttribute);
			}
			set {
				SetNumberValue (UIFontDescriptor.FixedAdvanceAttribute, value);
			}
		}

		/// <summary>The weakly-typed version of the <see cref="FeatureSettings" /> property.</summary>
		/// <value>An array of dictionaries representing the feature settings, or <see langword="null" /> if not set.</value>
		/// <remarks>This uses the raw, untyped version of the feature settings.</remarks>
		public NSDictionary []? WeakFeatureSettings {
			get {
				return GetArray<NSDictionary> (UIFontDescriptor.FeatureSettingsAttribute);
			}
			set {
				if (value is null) {
					RemoveValue (UIFontDescriptor.FeatureSettingsAttribute);
					return;
				}
				Dictionary [UIFontDescriptor.FeatureSettingsAttribute] = NSArray.FromNSObjects (value);
			}
		}

		/// <summary>Typographic and layout feature settings.</summary>
		/// <value>An array of <see cref="UIFontFeature" /> objects representing the feature settings.</value>
		/// <remarks>
		///   <para>
		///     This property is an array of <see cref="UIFontFeature" /> objects, each
		///     representing a font-specific typographic or layout feature
		///     that can be enabled in the font. For background on font
		///     features, you can read
		///     https://developer.apple.com/fonts/registry
		///   </para>
		///   <para>
		///     The following example shows how to configure the font to
		///     use proportional numbers as well as informing the
		///     rendering engine to use the first character alternatives available in this font.
		///   </para>
		///   <example>
		///     <code lang="csharp lang-csharp"><![CDATA[
		/// UIFont CustomizeFont (UIFont font)
		/// {
		///     var originalDescriptor = font.FontDescriptor;
		///     var attributes = new UIFontAttributes (
		///         new UIFontFeature (CTFontFeatureNumberSpacing.Selector.ProportionalNumbers),
		///         new UIFontFeature ((CTFontFeatureCharacterAlternatives.Selector) 1));
		///
		///     var newDesc = originalDescriptor.CreateWithAttributes (attributes);
		///     return UIFont.FromDescriptor (newDesc, 80);
		/// }
		/// ]]></code>
		///   </example>
		/// </remarks>
		public UIFontFeature [] FeatureSettings {
			get {
				var dictArray = WeakFeatureSettings;
				if (dictArray is null)
					return new UIFontFeature [0];

				var strong = new UIFontFeature [dictArray.Length];
				for (int i = 0; i < dictArray.Length; i++)
					strong [i] = new UIFontFeature (dictArray [i]);
				return strong;
			}
			set {
				Dictionary [UIFontDescriptor.FeatureSettingsAttribute] = NSArray.FromNativeObjects (value);
			}
		}
#endif
	}

#if !COREBUILD
	public partial class UIFontDescriptor {
		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for headlines.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred headline font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredHeadline {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Headline);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for body text.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred body font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredBody {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Body);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for subheadings.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred subheadline font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredSubheadline {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Subheadline);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for footnotes.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred footnote font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredFootnote {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Footnote);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for captions.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred caption 1 font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredCaption1 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Caption1);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for caption 2.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred caption 2 font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredCaption2 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Caption2);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for title 1.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred title 1 font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredTitle1 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Title1);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for title 2.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred title 2 font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredTitle2 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Title2);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for title 3.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred title 3 font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredTitle3 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Title3);
			}
		}

		/// <summary>Gets the <see cref="UIFontDescriptor" /> describing the preferred font for callouts.</summary>
		/// <value>A <see cref="UIFontDescriptor" /> for the preferred callout font.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public static UIFontDescriptor PreferredCallout {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Callout);
			}
		}

		/// <summary>Retrieves an array of <see cref="UIFontDescriptor" /> instances with an explicit set of mandatory keys.</summary>
		/// <param name="mandatoryKeys">The list of mandatory keys that you desire on the font descriptor.</param>
		/// <returns>An array of <see cref="UIFontDescriptor" /> instances matching the specified mandatory keys.</returns>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public UIFontDescriptor [] GetMatchingFontDescriptors (params UIFontDescriptorAttribute [] mandatoryKeys)
		{
			var n = mandatoryKeys.Length;
			if (n == 0)
				return GetMatchingFontDescriptors ((NSSet?) null);
			var all = new NSString [n];
			for (int i = 0; i < n; i++) {
				switch (mandatoryKeys [i]) {
				case UIFontDescriptorAttribute.Family:
					all [i] = FamilyAttribute;
					break;
				case UIFontDescriptorAttribute.Face:
					all [i] = FaceAttribute;
					break;
				case UIFontDescriptorAttribute.Name:
					all [i] = NameAttribute;
					break;
				case UIFontDescriptorAttribute.Size:
					all [i] = SizeAttribute;
					break;
				case UIFontDescriptorAttribute.VisibleName:
					all [i] = VisibleNameAttribute;
					break;
				case UIFontDescriptorAttribute.Matrix:
					all [i] = MatrixAttribute;
					break;
				case UIFontDescriptorAttribute.CharacterSet:
					all [i] = CharacterSetAttribute;
					break;
				case UIFontDescriptorAttribute.CascadeList:
					all [i] = CascadeListAttribute;
					break;
				case UIFontDescriptorAttribute.Traits:
					all [i] = TraitsAttribute;
					break;
				case UIFontDescriptorAttribute.FixedAdvance:
					all [i] = FixedAdvanceAttribute;
					break;
				case UIFontDescriptorAttribute.FeatureSettings:
					all [i] = FeatureSettingsAttribute;
					break;
				case UIFontDescriptorAttribute.TextStyle:
					all [i] = TextStyleAttribute;
					break;
				}
			}
			return GetMatchingFontDescriptors (new NSSet (all));
		}

		NSString? GetStringValue (NSString key)
		{
			return (NSString?) GetObject (key);
		}

		float? GetFloatValue (NSString key)
		{
			var n = (NSNumber?) GetObject (key);
			if (n is not null)
				return n.FloatValue;
			return null;
		}

		/// <summary>The font family.</summary>
		/// <value>The font family name, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public string? Family {
			get {
				return GetStringValue (UIFontDescriptor.FamilyAttribute);
			}
		}

		/// <summary>The name of the descriptor.</summary>
		/// <value>The font name, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public string? Name {
			get {
				return GetStringValue (UIFontDescriptor.NameAttribute);
			}
		}

		/// <summary>The font face.</summary>
		/// <value>The font face name, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public string? Face {
			get {
				return GetStringValue (UIFontDescriptor.FaceAttribute);
			}
		}

		/// <summary>The font size, if set.</summary>
		/// <value>The font size, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public float? Size {
			get {
				return GetFloatValue (UIFontDescriptor.SizeAttribute);
			}
		}

		/// <summary>The visible name for this descriptor.</summary>
		/// <value>The visible font name, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public string? VisibleName {
			get {
				return GetStringValue (UIFontDescriptor.VisibleNameAttribute);
			}
		}

		/// <summary>The name of the style to use for text.</summary>
		/// <value>The text style, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public NSString? TextStyle {
			get {
				return GetStringValue (UIFontDescriptor.TextStyleAttribute);
			}
		}

		/// <summary>The character set supported by the specified font.</summary>
		/// <value>The character set, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public NSCharacterSet? CharacterSet {
			get {
				return GetObject (UIFontDescriptor.CharacterSetAttribute) as NSCharacterSet;
			}
		}

		/// <summary>The list of sub descriptors to be used before the system default fallbacks.</summary>
		/// <value>The cascade list of font descriptors. The default is the list of descriptors based on the user locale.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public UIFontDescriptor [] CascadeList {
			get {
				var o = GetObject (UIFontDescriptor.CascadeListAttribute) as NSArray;
				if (o is null)
					return new UIFontDescriptor [0];
				return NSArray.FromArray<UIFontDescriptor> (o);
			}
		}

		/// <summary>The font's traits.</summary>
		/// <value>The font traits, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public UIFontTraits? Traits {
			get {
				var traits = GetObject (UIFontDescriptor.TraitsAttribute) as NSDictionary;
				if (traits is null)
					return null;
				return new UIFontTraits (traits);
			}
		}

		/// <summary>The override for the font's fixed advance for individual glyphs.</summary>
		/// <value>The fixed advance value. Default value is 0, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public float? FixedAdvance {
			get {
				return GetFloatValue (UIFontDescriptor.FixedAdvanceAttribute);
			}
		}

		/// <summary>The weakly typed version of the feature settings.</summary>
		/// <value>An array of dictionaries representing the feature settings, or <see langword="null" /> if not set.</value>
		/// <remarks>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public NSDictionary []? WeakFeatureSettings {
			get {
				var wf = GetObject (UIFontDescriptor.FeatureSettingsAttribute) as NSArray;
				if (wf is null)
					return null;
				return NSArray.FromArray<NSDictionary> (wf);
			}
		}

		/// <summary>Typographic and layout feature settings.</summary>
		/// <value>An array of <see cref="UIFontFeature" /> objects representing the feature settings.</value>
		/// <remarks>
		///   <para>
		///     This property is an array of <see cref="UIFontFeature" /> objects, each
		///     representing a font-specific typographic or layout feature
		///     that can be enabled in the font. For background on font
		///     features, you can read
		///     https://developer.apple.com/fonts/registry
		///   </para>
		///   <para>
		///     The following example shows how to configure the font to
		///     use proportional numbers as well as informing the
		///     rendering engine to use the first character alternatives available in this font.
		///   </para>
		///   <example>
		///     <code lang="csharp lang-csharp"><![CDATA[
		/// UIFont CustomizeFont (UIFont font)
		/// {
		///     var originalDescriptor = font.FontDescriptor;
		///     var attributes = new UIFontAttributes (
		///         new UIFontFeature (CTFontFeatureNumberSpacing.Selector.ProportionalNumbers),
		///         new UIFontFeature ((CTFontFeatureCharacterAlternatives.Selector) 1));
		///
		///     var newDesc = originalDescriptor.CreateWithAttributes (attributes);
		///     return UIFont.FromDescriptor (newDesc, 80);
		/// }
		/// ]]></code>
		///   </example>
		///   <para tool="threads">This can be used from a background thread.</para>
		/// </remarks>
		public UIFontFeature [] FeatureSettings {
			get {
				var dictArray = WeakFeatureSettings;
				if (dictArray is null)
					return new UIFontFeature [0];

				var strong = new UIFontFeature [dictArray.Length];
				for (int i = 0; i < dictArray.Length; i++)
					strong [i] = new UIFontFeature (dictArray [i]);
				return strong;
			}
		}
	}

	/// <summary>An enumeration whose values can be passed to <see cref="UIFontDescriptor.GetMatchingFontDescriptors(UIFontDescriptorAttribute[])" /> to specify which keys must be matched.</summary>
	public enum UIFontDescriptorAttribute {
		/// <summary>Key to specify that the font family must be matched.</summary>
		Family,
		/// <summary>Key to specify that the font face must be matched.</summary>
		Face,
		/// <summary>Key to specify that the font name must be matched.</summary>
		Name,
		/// <summary>Key to specify that the font size must be matched.</summary>
		Size,
		/// <summary>Key to specify the font's visible name.</summary>
		VisibleName,
		/// <summary>Key to retrieve the font transformation matrix.</summary>
		Matrix,
		/// <summary>Key to specify that the character set must be matched.</summary>
		CharacterSet,
		/// <summary>Key to specify a list of sub-descriptors.</summary>
		CascadeList,
		/// <summary>Key to specify a dictionary fully describing the font's traits.</summary>
		Traits,
		/// <summary>Key to represent a value that overrides the glyph advancement specified by the font.</summary>
		FixedAdvance,
		/// <summary>Key to represent an array of dictionaries containing non-default font attributes.</summary>
		FeatureSettings,
		/// <summary>Key to specify that the text style must be matched.</summary>
		TextStyle,
	}

	/// <summary>A <see cref="DictionaryContainer" /> that describes the symbolic traits of a <see cref="UIFont" />. Returned by <see cref="UIFontAttributes.Traits" />.</summary>
	public class UIFontTraits : DictionaryContainer {
		/// <summary>Creates an empty <see cref="UIFontTraits" /> instance.</summary>
		public UIFontTraits () { }

		/// <summary>Creates a <see cref="UIFontTraits" /> instance from the specified dictionary.</summary>
		/// <param name="dictionary">The dictionary containing the font traits.</param>
		public UIFontTraits (NSDictionary dictionary) : base (dictionary) { }

		/// <summary>The symbolic traits, if any, of the <see cref="UIFont" />.</summary>
		/// <value>The symbolic traits, or <see langword="null" /> if not set.</value>
		public UIFontDescriptorSymbolicTraits? SymbolicTrait {
			get {
				return (UIFontDescriptorSymbolicTraits?) GetInt32Value (UIFontDescriptor.SymbolicTrait);
			}
			set {
				if (value.HasValue)
					SetNumberValue (UIFontDescriptor.SymbolicTrait, (int) value.Value);
				else
					SetNumberValue (UIFontDescriptor.SymbolicTrait, (int?) null);
			}
		}

		/// <summary>The weight of the <see cref="UIFont" />, if set.</summary>
		/// <value>The font weight, or <see langword="null" /> if not set.</value>
		public float? Weight {
			get {
				return GetInt32Value (UIFontDescriptor.WeightTrait);
			}
			set {
				SetNumberValue (UIFontDescriptor.WeightTrait, value);
			}
		}

		/// <summary>The width of the <see cref="UIFont" />, if set.</summary>
		/// <value>The font width, or <see langword="null" /> if not set.</value>
		public float? Width {
			get {
				return GetInt32Value (UIFontDescriptor.WidthTrait);
			}
			set {
				SetNumberValue (UIFontDescriptor.WidthTrait, value);
			}
		}

		/// <summary>The slant, if any, of the <see cref="UIFont" />.</summary>
		/// <value>The font slant, or <see langword="null" /> if not set.</value>
		public float? Slant {
			get {
				return GetInt32Value (UIFontDescriptor.SlantTrait);
			}
			set {
				SetNumberValue (UIFontDescriptor.SlantTrait, value);
			}
		}
	}
#endif

}
