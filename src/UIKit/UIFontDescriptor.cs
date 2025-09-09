//
// UIFontDescriptor.cs: Supporting classes for UIFontDescriptor
//
// Author:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2013-2014 Xamarin Inc
//
using System;
using ObjCRuntime;
using CoreGraphics;
using Foundation;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	/// <summary>Attributes used to describe a font, used by <see cref="UIKit.UIFontDescriptor" /></summary>
	///     <remarks>
	///       <para>
	/// 	This type defines the attributes used to describe a font, like
	/// 	the font family, the font name, the character set, typographic features, glyph
	/// 	advancement, advanced typesetting features and others.
	///       </para>
	///       <para>
	/// 	Typically you create objects of this instance to create a <see cref="UIKit.UIFontDescriptor" />. 
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// var attributes = new UIFontAttributes () {
	///     Size = 14,
	///     Matrix = MyTransform
	/// };
	/// ]]></code>
	///       </example>
	///     </remarks>
	public class UIFontAttributes : DictionaryContainer {
		/// <summary>Creates an empty UIFontAttributes.</summary>
		///         <remarks>
		///         </remarks>
		public UIFontAttributes () { }

#if !COREBUILD
		/// <param name="dictionary">Dictionary containing UIFontAttributes.</param>
		///         <summary>Creates a UIFontAttributes from a weakly typed NSDictionary.</summary>
		///         <remarks>
		///         </remarks>
		public UIFontAttributes (NSDictionary dictionary) : base (dictionary) { }

		/// <param name="features">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIFontAttributes (params UIFontFeature [] features)
		{
			FeatureSettings = features;
		}

		/// <summary>The font family.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string Family {
			get {
				return GetStringValue (UIFontDescriptor.FamilyAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.FamilyAttribute, value);
			}
		}

		/// <summary>Font name.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string Name {
			get {
				return GetStringValue (UIFontDescriptor.NameAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.NameAttribute, value);
			}
		}

		/// <summary>The font face.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string Face {
			get {
				return GetStringValue (UIFontDescriptor.FaceAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.FaceAttribute, value);
			}
		}

		// Size is encoded as a string, but should contain a float
		/// <summary>Font size.</summary>
		///         <value>Null if not set.</value>
		///         <remarks>
		///         </remarks>
		public float? Size {
			get {
				return GetFloatValue (UIFontDescriptor.SizeAttribute);
			}
			set {
				SetNumberValue (UIFontDescriptor.SizeAttribute, value);
			}
		}

		/// <summary>Font visible name.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public string VisibleName {
			get {
				return GetStringValue (UIFontDescriptor.VisibleNameAttribute);
			}
			set {
				SetStringValue (UIFontDescriptor.VisibleNameAttribute, value);
			}
		}

		/// <summary>Text style.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSString TextStyle {
			get {
				return Dictionary [UIFontDescriptor.TextStyleAttribute] as NSString;
			}

			set {
				Dictionary [UIFontDescriptor.TextStyleAttribute] = value;
			}
		}

		/// <summary>Affine transformation matrix for the font.</summary>
		///         <value>The default is the identity matrix;   Null if not set.</value>
		///         <remarks>
		///         </remarks>
		public CGAffineTransform? Matrix {
			get {
				NSObject value;
				if (!Dictionary.TryGetValue (UIFontDescriptor.MatrixAttribute, out value))
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

		/// <summary>Describes the character set supported by the specified font.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSCharacterSet CharacterSet {
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

		/// <summary>Specifies the list of sub descriptors to be used before the system default fallbacks.</summary>
		///         <value>The default is the list of descriptors based on the user locale.</value>
		///         <remarks>
		///
		/// 	  By setting this property, developers can override the list
		/// 	  of UIFontDescriptors to be used before using the system
		/// 	  defaults.
		///
		/// 	</remarks>
		public UIFontDescriptor [] CascadeList {
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

		/// <summary>Font traits.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public UIFontTraits Traits {
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

		/// <summary>Override for font's fixed advance for individual glyphs.</summary>
		///         <value>Default value is 0, null if not set.</value>
		///         <remarks>
		///         </remarks>
		public float? FixedAdvance {
			get {
				return GetFloatValue (UIFontDescriptor.FixedAdvanceAttribute);
			}
			set {
				SetNumberValue (UIFontDescriptor.FixedAdvanceAttribute, value);
			}
		}

		/// <summary>Weakly-typed version of the FeatureSettings properties</summary>
		///         <value>
		///         </value>
		///         <remarks>This uses the raw, untyped version of the FeatureSettings.</remarks>
		public NSDictionary [] WeakFeatureSettings {
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

		/// <include file="../../docs/api/UIKit/UIFontAttributes.xml" path="/Documentation/Docs[@DocId='P:UIKit.UIFontAttributes.FeatureSettings']/*" />
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
		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for headlines.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIFontDescriptor PreferredHeadline {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Headline);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for body text.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIFontDescriptor PreferredBody {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Body);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for subheadings.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIFontDescriptor PreferredSubheadline {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Subheadline);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for footnotes.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIFontDescriptor PreferredFootnote {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Footnote);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for captions.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIFontDescriptor PreferredCaption1 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Caption1);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for 'caption 2'.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIFontDescriptor PreferredCaption2 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Caption2);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for titles ('Title 1').</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static UIFontDescriptor PreferredTitle1 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Title1);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for level 2 titles ('Title 2').</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static UIFontDescriptor PreferredTitle2 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Title2);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for level 3 titles ('Title 3').</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static UIFontDescriptor PreferredTitle3 {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Title3);
			}
		}

		/// <summary>Gets the <see cref="UIKit.UIFontDescriptor" /> describing the preferred font for callouts.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static UIFontDescriptor PreferredCallout {
			get {
				return GetPreferredDescriptorForTextStyle (UIFontTextStyle.Callout);
			}
		}

		/// <param name="mandatoryKeys">The list of mandatory keys that you desire on the font descriptor.</param>
		///         <summary>Retrieve a UIFontDescriptor with an explicit set of features.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public UIFontDescriptor [] GetMatchingFontDescriptors (params UIFontDescriptorAttribute [] mandatoryKeys)
		{
			var n = mandatoryKeys.Length;
			if (n == 0)
				return GetMatchingFontDescriptors ((NSSet) null);
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

		NSString GetStringValue (NSString key)
		{
			return (NSString) GetObject (key);
		}

		float? GetFloatValue (NSString key)
		{
			var n = (NSNumber) GetObject (key);
			if (n is not null)
				return n.FloatValue;
			return null;
		}

		/// <summary>The font family.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public string Family {
			get {
				return GetStringValue (UIFontDescriptor.FamilyAttribute);
			}
		}

		/// <summary>Name of the descriptor.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public string Name {
			get {
				return GetStringValue (UIFontDescriptor.NameAttribute);
			}
		}

		/// <summary>The font face.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public string Face {
			get {
				return GetStringValue (UIFontDescriptor.FaceAttribute);
			}
		}

		/// <summary>Font size (if set).</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public float? Size {
			get {
				return GetFloatValue (UIFontDescriptor.SizeAttribute);
			}
		}

		/// <summary>The visible name for this descriptor.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public string VisibleName {
			get {
				return GetStringValue (UIFontDescriptor.VisibleNameAttribute);
			}
		}

		/// <summary>Name of the style to use for text.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public NSString TextStyle {
			get {
				return GetStringValue (UIFontDescriptor.TextStyleAttribute);
			}
		}

		/// <summary>Describes the character set supported by the specified font.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public NSCharacterSet CharacterSet {
			get {
				return GetObject (UIFontDescriptor.CharacterSetAttribute) as NSCharacterSet;
			}
		}

		/// <summary>Specifies the list of sub descriptors to be used before the system default fallbacks.</summary>
		///         <value>The default is the list of descriptors based on the user locale.</value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public UIFontDescriptor [] CascadeList {
			get {
				var o = GetObject (UIFontDescriptor.CascadeListAttribute) as NSArray;
				if (o is null)
					return new UIFontDescriptor [0];
				return NSArray.FromArray<UIFontDescriptor> (o);
			}
		}

		/// <summary>Font's traits.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public UIFontTraits Traits {
			get {
				var traits = GetObject (UIFontDescriptor.TraitsAttribute) as NSDictionary;
				if (traits is null)
					return null;
				return new UIFontTraits (traits);
			}
		}

		/// <summary>Override for font's fixed advance for individual glyphs.</summary>
		///         <value>Default value is 0, null if not set.</value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public float? FixedAdvance {
			get {
				return GetFloatValue (UIFontDescriptor.FixedAdvanceAttribute);
			}
		}

		/// <summary>Weakly typed version of the feature settings.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public NSDictionary [] WeakFeatureSettings {
			get {
				var wf = GetObject (UIFontDescriptor.FeatureSettingsAttribute) as NSArray;
				if (wf is null)
					return null;
				return NSArray.FromArray<NSDictionary> (wf);
			}
		}

		/// <include file="../../docs/api/UIKit/UIFontDescriptor.xml" path="/Documentation/Docs[@DocId='P:UIKit.UIFontDescriptor.FeatureSettings']/*" />
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

	// that's a convenience enum that maps to UIFontDescriptorXXX which are internal (hidden) NSString
	/// <summary>An enumeration whose values can be passed to <see cref="UIKit.UIFontDescriptor.GetMatchingFontDescriptors(UIKit.UIFontDescriptorAttribute[])" /> to specify which keys must be matched.</summary>
	///     <remarks>To be added.</remarks>
	public enum UIFontDescriptorAttribute {
		/// <summary>Key to specify that font family must be matched.</summary>
		Family,
		/// <summary>Key to specify that font face must be matched.</summary>
		Face,
		/// <summary>Key to specify that the font name must be matched.</summary>
		Name,
		/// <summary>Key to specify that the font size must be matched.</summary>
		Size,
		/// <summary>Key to specify the font's visible name.</summary>
		VisibleName,
		/// <summary>Key to retrieve the font transformation matrix.</summary>
		Matrix,
		/// <summary>Key to specify that character sets must be matched.</summary>
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

	/// <summary>A <see cref="Foundation.DictionaryContainer" /> that describes the symbolic traits of a <see cref="UIKit.UIFont" />. Returned by <see cref="UIKit.UIFontAttributes.Traits" />.</summary>
	///     <remarks>To be added.</remarks>
	public class UIFontTraits : DictionaryContainer {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIFontTraits () { }
		/// <param name="dictionary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public UIFontTraits (NSDictionary dictionary) : base (dictionary) { }

		/// <summary>The symbolic traits, if any, of the UIFont.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public UIFontDescriptorSymbolicTraits? SymbolicTrait {
			get {
				return (UIFontDescriptorSymbolicTraits?) GetInt32Value (UIFontDescriptor.SymbolicTrait);
			}
			set {
				SetNumberValue (UIFontDescriptor.SymbolicTrait, (int) value);
			}
		}

		/// <summary>The weight of the UIFont, if set.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? Weight {
			get {
				return GetInt32Value (UIFontDescriptor.WeightTrait);
			}
			set {
				SetNumberValue (UIFontDescriptor.WeightTrait, value);
			}
		}

		/// <summary>The width of the UIFont, if set.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public float? Width {
			get {
				return GetInt32Value (UIFontDescriptor.WidthTrait);
			}
			set {
				SetNumberValue (UIFontDescriptor.WidthTrait, value);
			}
		}

		/// <summary>The slant, if any, of the UIFont.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
