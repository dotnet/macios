//
// UIFontFeatuers.cs: 
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2013, Xamarin Inc
//

#nullable enable

using System;

using Foundation;
using ObjCRuntime;
using CoreText;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace UIKit {
	/// <include file="../../docs/api/UIKit/UIFontFeature.xml" path="/Documentation/Docs[@DocId='T:UIKit.UIFontFeature']/*" />
	public class UIFontFeature : INativeObject {
		static NSObject [] keys = new NSObject [] { UIFontDescriptor.UIFontFeatureTypeIdentifierKey, UIFontDescriptor.UIFontFeatureSelectorIdentifierKey };

		NSDictionary dictionary;

		// To easily implement ToString
		FontFeatureGroup fontFeature;
		object? fontFeatureValue;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NativeHandle INativeObject.Handle {
			get {
				return dictionary.Handle;
			}
		}

		// internal ctor
		UIFontFeature (FontFeatureGroup fontFeature, int fontFeatureSelector, object value)
		{
			this.dictionary = NSDictionary.FromObjectsAndKeys (new NSObject [] { new NSNumber ((int) fontFeature), new NSNumber (fontFeatureSelector) }, keys);
			this.fontFeature = fontFeature;
			this.fontFeatureValue = value;
		}

		internal UIFontFeature (NSDictionary dict)
		{
			dictionary = dict;
			var v = dict [UIFontDescriptor.UIFontFeatureTypeIdentifierKey] as NSNumber;
			fontFeature = (FontFeatureGroup) (v is null ? -1 : v.Int32Value);

			v = dict [UIFontDescriptor.UIFontFeatureSelectorIdentifierKey] as NSNumber;
			int n = v is null ? 0 : v.Int32Value;

			switch (fontFeature) {
			case FontFeatureGroup.AllTypographicFeatures:
				fontFeatureValue = (CTFontFeatureAllTypographicFeatures.Selector) n;
				break;
			case FontFeatureGroup.Ligatures:
				fontFeatureValue = (CTFontFeatureLigatures.Selector) n;
				break;
			case FontFeatureGroup.CursiveConnection:
				fontFeatureValue = (CTFontFeatureCursiveConnection.Selector) n;
				break;
			case FontFeatureGroup.LetterCase:
				fontFeatureValue = (CTFontFeatureLetterCase.Selector) n;
				break;
			case FontFeatureGroup.VerticalSubstitution:
				fontFeatureValue = (CTFontFeatureVerticalSubstitutionConnection.Selector) n;
				break;
			case FontFeatureGroup.LinguisticRearrangement:
				fontFeatureValue = (CTFontFeatureLinguisticRearrangementConnection.Selector) n;
				break;
			case FontFeatureGroup.NumberSpacing:
				fontFeatureValue = (CTFontFeatureNumberSpacing.Selector) n;
				break;
			case FontFeatureGroup.SmartSwash:
				fontFeatureValue = (CTFontFeatureSmartSwash.Selector) n;
				break;
			case FontFeatureGroup.Diacritics:
				fontFeatureValue = (CTFontFeatureDiacritics.Selector) n;
				break;
			case FontFeatureGroup.VerticalPosition:
				fontFeatureValue = (CTFontFeatureVerticalPosition.Selector) n;
				break;
			case FontFeatureGroup.Fractions:
				fontFeatureValue = (CTFontFeatureFractions.Selector) n;
				break;
			case FontFeatureGroup.OverlappingCharacters:
				fontFeatureValue = (CTFontFeatureOverlappingCharacters.Selector) n;
				break;
			case FontFeatureGroup.TypographicExtras:
				fontFeatureValue = (CTFontFeatureTypographicExtras.Selector) n;
				break;
			case FontFeatureGroup.MathematicalExtras:
				fontFeatureValue = (CTFontFeatureMathematicalExtras.Selector) n;
				break;
			case FontFeatureGroup.OrnamentSets:
				fontFeatureValue = (CTFontFeatureOrnamentSets.Selector) n;
				break;
			case FontFeatureGroup.CharacterAlternatives:
				fontFeatureValue = (CTFontFeatureCharacterAlternatives.Selector) n;
				break;
			case FontFeatureGroup.DesignComplexity:
				fontFeatureValue = (CTFontFeatureDesignComplexity.Selector) n;
				break;
			case FontFeatureGroup.StyleOptions:
				fontFeatureValue = (CTFontFeatureStyleOptions.Selector) n;
				break;
			case FontFeatureGroup.CharacterShape:
				fontFeatureValue = (CTFontFeatureCharacterShape.Selector) n;
				break;
			case FontFeatureGroup.NumberCase:
				fontFeatureValue = (CTFontFeatureNumberCase.Selector) n;
				break;
			case FontFeatureGroup.TextSpacing:
				fontFeatureValue = (CTFontFeatureTextSpacing.Selector) n;
				break;
			case FontFeatureGroup.Transliteration:
				fontFeatureValue = (CTFontFeatureTransliteration.Selector) n;
				break;
			case FontFeatureGroup.Annotation:
				fontFeatureValue = (CTFontFeatureAnnotation.Selector) n;
				break;
			case FontFeatureGroup.KanaSpacing:
				fontFeatureValue = (CTFontFeatureKanaSpacing.Selector) n;
				break;
			case FontFeatureGroup.IdeographicSpacing:
				fontFeatureValue = (CTFontFeatureIdeographicSpacing.Selector) n;
				break;
			case FontFeatureGroup.UnicodeDecomposition:
				fontFeatureValue = (CTFontFeatureUnicodeDecomposition.Selector) n;
				break;
			case FontFeatureGroup.RubyKana:
				fontFeatureValue = (CTFontFeatureRubyKana.Selector) n;
				break;
			case FontFeatureGroup.CJKSymbolAlternatives:
				fontFeatureValue = (CTFontFeatureCJKSymbolAlternatives.Selector) n;
				break;
			case FontFeatureGroup.IdeographicAlternatives:
				fontFeatureValue = (CTFontFeatureIdeographicAlternatives.Selector) n;
				break;
			case FontFeatureGroup.CJKVerticalRomanPlacement:
				fontFeatureValue = (CTFontFeatureCJKVerticalRomanPlacement.Selector) n;
				break;
			case FontFeatureGroup.ItalicCJKRoman:
				fontFeatureValue = (CTFontFeatureItalicCJKRoman.Selector) n;
				break;
			case FontFeatureGroup.CaseSensitiveLayout:
				fontFeatureValue = (CTFontFeatureCaseSensitiveLayout.Selector) n;
				break;
			case FontFeatureGroup.AlternateKana:
				fontFeatureValue = (CTFontFeatureAlternateKana.Selector) n;
				break;
			case FontFeatureGroup.StylisticAlternatives:
				fontFeatureValue = (CTFontFeatureStylisticAlternatives.Selector) n;
				break;
			case FontFeatureGroup.ContextualAlternates:
				fontFeatureValue = (CTFontFeatureContextualAlternates.Selector) n;
				break;
			case FontFeatureGroup.LowerCase:
				fontFeatureValue = (CTFontFeatureLowerCase.Selector) n;
				break;
			case FontFeatureGroup.UpperCase:
				fontFeatureValue = (CTFontFeatureUpperCase.Selector) n;
				break;
			case FontFeatureGroup.CJKRomanSpacing:
				fontFeatureValue = (CTFontFeatureCJKRomanSpacing.Selector) n;
				break;
			}
			if (v is null)
				fontFeatureValue = "InvalidKeyFound";
		}

		/// <summary>The FontFeatureGroup that this UIFontFeature object encapsulates.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public FontFeatureGroup FontFeature {
			get {
				return fontFeature;
			}
		}

		/// <summary>The value for the FontFeature.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public object? FontFeatureValue {
			get {
				return fontFeatureValue;
			}
		}

		/// <summary>Renders a human readable representation of this object.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			return String.Format ("{0}={1}", FontFeature == ((FontFeatureGroup) (-1)) ? "Invalid" : FontFeature.ToString (), FontFeatureValue);
		}

		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureAllTypographicFeatures with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureAllTypographicFeatures type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureAllTypographicFeatures.Selector featureSelector) : this (FontFeatureGroup.AllTypographicFeatures, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureLigatures with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureLigatures type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureLigatures.Selector featureSelector) : this (FontFeatureGroup.Ligatures, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureCursiveConnection with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureCursiveConnection type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureCursiveConnection.Selector featureSelector) : this (FontFeatureGroup.CursiveConnection, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureLetterCase with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureLetterCase type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureLetterCase.Selector featureSelector) : this (FontFeatureGroup.LetterCase, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureVerticalSubstitutionConnection with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureVerticalSubstitutionConnection type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureVerticalSubstitutionConnection.Selector featureSelector) : this (FontFeatureGroup.VerticalSubstitution, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureLinguisticRearrangementConnection with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureLinguisticRearrangementConnection type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureLinguisticRearrangementConnection.Selector featureSelector) : this (FontFeatureGroup.LinguisticRearrangement, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureNumberSpacing with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureNumberSpacing type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureNumberSpacing.Selector featureSelector) : this (FontFeatureGroup.NumberSpacing, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureSmartSwash with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureSmartSwash type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureSmartSwash.Selector featureSelector) : this (FontFeatureGroup.SmartSwash, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureDiacritics with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureDiacritics type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureDiacritics.Selector featureSelector) : this (FontFeatureGroup.Diacritics, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureVerticalPosition with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureVerticalPosition type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureVerticalPosition.Selector featureSelector) : this (FontFeatureGroup.VerticalPosition, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureFractions with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureFractions type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureFractions.Selector featureSelector) : this (FontFeatureGroup.Fractions, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureOverlappingCharacters with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureOverlappingCharacters type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureOverlappingCharacters.Selector featureSelector) : this (FontFeatureGroup.OverlappingCharacters, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureTypographicExtras with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureTypographicExtras type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureTypographicExtras.Selector featureSelector) : this (FontFeatureGroup.TypographicExtras, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureMathematicalExtras with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureMathematicalExtras type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureMathematicalExtras.Selector featureSelector) : this (FontFeatureGroup.MathematicalExtras, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureOrnamentSets with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureOrnamentSets type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureOrnamentSets.Selector featureSelector) : this (FontFeatureGroup.OrnamentSets, (int) featureSelector, featureSelector) { }
		/// <include file="../../docs/api/UIKit/UIFontFeature.xml" path="/Documentation/Docs[@DocId='M:UIKit.UIFontFeature.#ctor(CoreText.CTFontFeatureCharacterAlternatives.Selector)']/*" />
		public UIFontFeature (CTFontFeatureCharacterAlternatives.Selector featureSelector) : this (FontFeatureGroup.CharacterAlternatives, (int) featureSelector, featureSelector) { }
		/// <param name="characterAlternatives">Requests that the specified item be used as a Character Alternative. </param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureCharacterAlternatives with the given value.</summary>
		///         <remarks>
		///
		/// 	  Character alternatives are font specific, and the only
		/// 	  strongly typed value is the NoAlternatives value.  Any other
		/// 	  integer above 0 is used to specify the specific character
		/// 	  alternative that you want to use.
		///
		/// 	</remarks>
		public UIFontFeature (int characterAlternatives) : this (FontFeatureGroup.CharacterAlternatives, (int) characterAlternatives, characterAlternatives) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureDesignComplexity with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureDesignComplexity type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureDesignComplexity.Selector featureSelector) : this (FontFeatureGroup.DesignComplexity, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureStyleOptions with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureStyleOptions type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureStyleOptions.Selector featureSelector) : this (FontFeatureGroup.StyleOptions, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureCharacterShape with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureCharacterShape type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureCharacterShape.Selector featureSelector) : this (FontFeatureGroup.CharacterShape, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureNumberCase with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureNumberCase type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureNumberCase.Selector featureSelector) : this (FontFeatureGroup.NumberCase, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureTextSpacing with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureTextSpacing type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureTextSpacing.Selector featureSelector) : this (FontFeatureGroup.TextSpacing, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureTransliteration with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureTransliteration type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureTransliteration.Selector featureSelector) : this (FontFeatureGroup.Transliteration, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureAnnotation with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureAnnotation type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureAnnotation.Selector featureSelector) : this (FontFeatureGroup.Annotation, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureKanaSpacing with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureKanaSpacing type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureKanaSpacing.Selector featureSelector) : this (FontFeatureGroup.KanaSpacing, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureIdeographicSpacing with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureIdeographicSpacing type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureIdeographicSpacing.Selector featureSelector) : this (FontFeatureGroup.IdeographicSpacing, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureUnicodeDecomposition with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureUnicodeDecomposition type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureUnicodeDecomposition.Selector featureSelector) : this (FontFeatureGroup.UnicodeDecomposition, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureRubyKana with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureRubyKana type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureRubyKana.Selector featureSelector) : this (FontFeatureGroup.RubyKana, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureCJKSymbolAlternatives with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureCJKSymbolAlternatives type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureCJKSymbolAlternatives.Selector featureSelector) : this (FontFeatureGroup.CJKSymbolAlternatives, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureIdeographicAlternatives with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureIdeographicAlternatives type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureIdeographicAlternatives.Selector featureSelector) : this (FontFeatureGroup.IdeographicAlternatives, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureCJKVerticalRomanPlacement with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureCJKVerticalRomanPlacement type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureCJKVerticalRomanPlacement.Selector featureSelector) : this (FontFeatureGroup.CJKVerticalRomanPlacement, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureItalicCJKRoman with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureItalicCJKRoman type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureItalicCJKRoman.Selector featureSelector) : this (FontFeatureGroup.ItalicCJKRoman, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureCaseSensitiveLayout with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureCaseSensitiveLayout type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureCaseSensitiveLayout.Selector featureSelector) : this (FontFeatureGroup.CaseSensitiveLayout, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureAlternateKana with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureAlternateKana type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureAlternateKana.Selector featureSelector) : this (FontFeatureGroup.AlternateKana, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureStylisticAlternatives with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureStylisticAlternatives type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureStylisticAlternatives.Selector featureSelector) : this (FontFeatureGroup.StylisticAlternatives, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureContextualAlternates with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureContextualAlternates type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureContextualAlternates.Selector featureSelector) : this (FontFeatureGroup.ContextualAlternates, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureLowerCase with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureLowerCase type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureLowerCase.Selector featureSelector) : this (FontFeatureGroup.LowerCase, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureUpperCase with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureUpperCase type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureUpperCase.Selector featureSelector) : this (FontFeatureGroup.UpperCase, (int) featureSelector, featureSelector) { }
		/// <param name="featureSelector">The value for this setting.</param>
		///         <summary>Creates a new UIFontFeature that describes a CoreText CTFontFeatureCJKRomanSpacing with the given value.</summary>
		///         <remarks>
		/// 	  This creates a new instance of the UIFontFeature with the CoreText's CTFontFeatureCJKRomanSpacing type using the <paramref name="featureSelector" /> as its parameter.
		/// 	</remarks>
		public UIFontFeature (CTFontFeatureCJKRomanSpacing.Selector featureSelector) : this (FontFeatureGroup.CJKRomanSpacing, (int) featureSelector, featureSelector) { }
	}
}
