//
// coretext.cs: Definitions for CoreText
//
// Authors: 
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc.
//

using System;

using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace CoreText {

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTFontFeatures" />.</summary>
	[Static]
	interface CTFontFeatureKey {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureTypeIdentifierKey")]
		NSString Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureTypeNameKey")]
		NSString Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureTypeExclusiveKey")]
		NSString Exclusive { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureTypeSelectorsKey")]
		NSString Selectors { get; }
	}

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTFontFeatureSelectors" />.</summary>
	[Static]
	interface CTFontFeatureSelectorKey {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureSelectorIdentifierKey")]
		NSString Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureSelectorNameKey")]
		NSString Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureSelectorDefaultKey")]
		NSString Default { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureSelectorSettingKey")]
		NSString Setting { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCTFontFeatureSampleTextKey")]
		NSString SampleText { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCTFontFeatureTooltipTextKey")]
		NSString TooltipText { get; }
	}

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTFontVariationAxes" />.</summary>
	[Static]
	interface CTFontVariationAxisKey {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontVariationAxisIdentifierKey")]
		NSString Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontVariationAxisMinimumValueKey")]
		NSString MinimumValue { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontVariationAxisMaximumValueKey")]
		NSString MaximumValue { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontVariationAxisDefaultValueKey")]
		NSString DefaultValue { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kCTFontVariationAxisNameKey")]
		NSString Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCTFontVariationAxisHiddenKey")]
		NSString Hidden { get; }
	}

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTTypesetterOptions" />.</summary>
	[Static]
	interface CTTypesetterOptionKey {

		/// <summary>Developers should not use this deprecated field. </summary>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 6, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Deprecated (PlatformName.MacOSX, 10, 8)]
		[Deprecated (PlatformName.TvOS, 9, 0)]
		[Field ("kCTTypesetterOptionDisableBidiProcessing")]
		NSString DisableBidiProcessing { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTTypesetterOptionForcedEmbeddingLevel")]
		NSString ForceEmbeddingLevel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kCTTypesetterOptionAllowUnboundedLayout")]
		NSString AllowUnboundedLayout { get; }
	}

	[Static]
	interface CTFontManagerErrorKeys {
		[Field ("kCTFontManagerErrorFontURLsKey")]
		NSString FontUrlsKey { get; }

		[NoTV, NoMac, iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCTFontManagerErrorFontDescriptorsKey")]
		NSString FontDescriptorsKey { get; }

		[NoTV, NoMac, iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCTFontManagerErrorFontAssetNameKey")]
		NSString FontAssetNameKey { get; }
	}

	[Internal]
	[Static]
	[Partial]
	interface CTBaselineClassID {
		[Field ("kCTBaselineClassRoman")]
		NSString Roman { get; }

		[Field ("kCTBaselineClassIdeographicCentered")]
		NSString IdeographicCentered { get; }

		[Field ("kCTBaselineClassIdeographicLow")]
		NSString IdeographicLow { get; }

		[Field ("kCTBaselineClassIdeographicHigh")]
		NSString IdeographicHigh { get; }

		[Field ("kCTBaselineClassHanging")]
		NSString Hanging { get; }

		[Field ("kCTBaselineClassMath")]
		NSString Math { get; }
	}

	[Internal]
	[Static]
	[Partial]
	interface CTBaselineFontID {
		[Field ("kCTBaselineReferenceFont")]
		NSString Reference { get; }

		[Field ("kCTBaselineOriginalFont")]
		NSString Original { get; }
	}

	/// <summary>A valid key for use with <see cref="CoreText.CTFontDescriptor" /> attribute properties.</summary>
	[Static]
	interface CTFontDescriptorAttributeKey {
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontURLAttribute")]
		NSString Url { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontNameAttribute")]
		NSString Name { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontDisplayNameAttribute")]
		NSString DisplayName { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontFamilyNameAttribute")]
		NSString FamilyName { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontStyleNameAttribute")]
		NSString StyleName { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontTraitsAttribute")]
		NSString Traits { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontVariationAttribute")]
		NSString Variation { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontSizeAttribute")]
		NSString Size { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontMatrixAttribute")]
		NSString Matrix { get; }

		/// <summary>Augment the list of cascading fonts to try out when a font is needed.</summary>
		/// <remarks>
		///           <para>
		/// 	    Since fonts do not cover the entire Unicode space, you can
		/// 	    provide a list of fallback fonts that will be tried for
		/// 	    glyphs that are not available for a certain codepoint in
		/// 	    the main selected font.
		/// 
		/// 	  </para>
		///           <para>
		/// 	    By default the system has a built-in cascade list that the
		/// 	    system uses to satisfy the font.  By setting this property
		/// 	    is to set a list that is consulted before the system
		/// 	    cascade list is looked up.
		/// 
		/// 	  </para>
		///           <para>
		/// 	    If you want to prevent the system cascade list from being
		/// 	    consulted, you can use the special font name "LastResort",
		/// 	    this is a special font that contains glyphs for every
		/// 	    unicode code point.  You can learn more about it at the
		/// 	    Unicode web site:
		/// 	    https://unicode.org/policies/lastresortfont_eula.html
		/// 
		/// 	  </para>
		///         </remarks>
		[Field ("kCTFontCascadeListAttribute")]
		NSString CascadeList { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontCharacterSetAttribute")]
		NSString CharacterSet { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontLanguagesAttribute")]
		NSString Languages { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontBaselineAdjustAttribute")]
		NSString BaselineAdjust { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontMacintoshEncodingsAttribute")]
		NSString MacintoshEncodings { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontFeaturesAttribute")]
		NSString Features { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontFeatureSettingsAttribute")]
		NSString FeatureSettings { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontFixedAdvanceAttribute")]
		NSString FixedAdvance { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontOrientationAttribute")]
		NSString FontOrientation { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontFormatAttribute")]
		NSString FontFormat { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontRegistrationScopeAttribute")]
		NSString RegistrationScope { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontPriorityAttribute")]
		NSString Priority { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontEnabledAttribute")]
		NSString Enabled { get; }

		[iOS (13, 0), NoTV, MacCatalyst (13, 1), NoMac]
		[Field ("kCTFontRegistrationUserInfoAttribute")]
		NSString RegistrationUserInfo { get; }
	}

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTTextTabOptions" />.</summary>
	[Static]
	interface CTTextTabOptionKey {
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTTabColumnTerminatorsAttributeName")]
		NSString ColumnTerminators { get; }
	}

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTFrameAttributes" />.</summary>
	[Static]
	interface CTFrameAttributeKey {
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFrameProgressionAttributeName")]
		NSString Progression { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFramePathFillRuleAttributeName")]
		NSString PathFillRule { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFramePathWidthAttributeName")]
		NSString PathWidth { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFrameClippingPathsAttributeName")]
		NSString ClippingPaths { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFramePathClippingPathAttributeName")]
		NSString PathClippingPath { get; }
	}

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTFontTraits" />.</summary>
	[Static]
	interface CTFontTraitKey {
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontSymbolicTrait")]
		NSString Symbolic { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontWeightTrait")]
		NSString Weight { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontWidthTrait")]
		NSString Width { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontSlantTrait")]
		NSString Slant { get; }
	}

	[Internal]
	[Static]
	[Partial]
	interface CTFontNameKeyId {
		[Field ("kCTFontCopyrightNameKey")]
		NSString Copyright { get; }

		[Field ("kCTFontFamilyNameKey")]
		NSString Family { get; }

		[Field ("kCTFontSubFamilyNameKey")]
		NSString SubFamily { get; }

		[Field ("kCTFontStyleNameKey")]
		NSString Style { get; }

		[Field ("kCTFontUniqueNameKey")]
		NSString Unique { get; }

		[Field ("kCTFontFullNameKey")]
		NSString Full { get; }

		[Field ("kCTFontVersionNameKey")]
		NSString Version { get; }

		[Field ("kCTFontPostScriptNameKey")]
		NSString PostScript { get; }

		[Field ("kCTFontTrademarkNameKey")]
		NSString Trademark { get; }

		[Field ("kCTFontManufacturerNameKey")]
		NSString Manufacturer { get; }

		[Field ("kCTFontDesignerNameKey")]
		NSString Designer { get; }

		[Field ("kCTFontDescriptionNameKey")]
		NSString Description { get; }

		[Field ("kCTFontVendorURLNameKey")]
		NSString VendorUrl { get; }

		[Field ("kCTFontDesignerURLNameKey")]
		NSString DesignerUrl { get; }

		[Field ("kCTFontLicenseNameKey")]
		NSString License { get; }

		[Field ("kCTFontLicenseURLNameKey")]
		NSString LicenseUrl { get; }

		[Field ("kCTFontSampleTextNameKey")]
		NSString SampleText { get; }

		[Field ("kCTFontPostScriptCIDNameKey")]
		NSString PostscriptCid { get; }
	}

	/// <summary>A class whose static property can be used as a key for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTFontCollectionOptions" />.</summary>
	[Static]
	interface CTFontCollectionOptionKey {
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontCollectionRemoveDuplicatesOption")]
		NSString RemoveDuplicates { get; }
	}

	[Internal]
	[Static]
	interface CTFontDescriptorMatchingKeys {
		[Field ("kCTFontDescriptorMatchingSourceDescriptor")]
		NSString SourceDescriptorKey { get; }

		[Field ("kCTFontDescriptorMatchingDescriptors")]
		NSString DescriptorsKey { get; }

		[Field ("kCTFontDescriptorMatchingResult")]
		NSString ResultKey { get; }

		[Field ("kCTFontDescriptorMatchingPercentage")]
		NSString PercentageKey { get; }

		[Field ("kCTFontDescriptorMatchingCurrentAssetSize")]
		NSString CurrentAssetSizeKey { get; }

		[Field ("kCTFontDescriptorMatchingTotalDownloadedSize")]
		NSString TotalDownloadedSizeKey { get; }

		[Field ("kCTFontDescriptorMatchingTotalAssetSize")]
		NSString TotalAssetSizeKey { get; }

		[Field ("kCTFontDescriptorMatchingError")]
		NSString ErrorKey { get; }
	}

	[StrongDictionary ("CTFontDescriptorMatchingKeys")]
	interface CTFontDescriptorMatchingProgress {
		CTFontDescriptor SourceDescriptor { get; }
		CTFontDescriptor [] Descriptors { get; }
		CTFontDescriptor [] Result { get; }
		double Percentage { get; }
		long CurrentAssetSize { get; }
		long TotalDownloadedSize { get; }
		long TotalAssetSize { get; }
		NSError Error { get; }
	}

	/// <summary>A class whose static properties can be used as keys for the <see cref="Foundation.NSDictionary" /> used by <see cref="CoreText.CTStringAttributes" />.</summary>
	[Static]
	[Partial]
	interface CTStringAttributeKey {
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTFontAttributeName")]
		NSString Font { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTForegroundColorFromContextAttributeName")]
		NSString ForegroundColorFromContext { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTKernAttributeName")]
		NSString KerningAdjustment { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTLigatureAttributeName")]
		NSString LigatureFormation { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTForegroundColorAttributeName")]
		NSString ForegroundColor { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTBackgroundColorAttributeName")]
		NSString BackgroundColor { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTParagraphStyleAttributeName")]
		NSString ParagraphStyle { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTStrokeWidthAttributeName")]
		NSString StrokeWidth { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTStrokeColorAttributeName")]
		NSString StrokeColor { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTUnderlineStyleAttributeName")]
		NSString UnderlineStyle { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTSuperscriptAttributeName")]
		NSString Superscript { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTUnderlineColorAttributeName")]
		NSString UnderlineColor { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTVerticalFormsAttributeName")]
		NSString VerticalForms { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTHorizontalInVerticalFormsAttributeName")]
		NSString HorizontalInVerticalForms { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTGlyphInfoAttributeName")]
		NSString GlyphInfo { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTCharacterShapeAttributeName")]
		NSString CharacterShape { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Field ("kCTRunDelegateAttributeName")]
		NSString RunDelegate { get; }

		[Field ("kCTBaselineOffsetAttributeName")]
		NSString BaselineOffset { get; }

		[Field ("kCTBaselineClassAttributeName")]
		NSString BaselineClass { get; }

		[Field ("kCTBaselineInfoAttributeName")]
		NSString BaselineInfo { get; }

		[Field ("kCTBaselineReferenceInfoAttributeName")]
		NSString BaselineReferenceInfo { get; }

		[Field ("kCTWritingDirectionAttributeName")]
		NSString WritingDirection { get; }

		[Field ("kCTRubyAnnotationAttributeName")]
		NSString RubyAnnotation { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Field ("kCTAdaptiveImageProviderAttributeName")]
		NSString AdaptiveImageProvider { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("kCTTrackingAttributeName")]
		NSString TrackingAttributeName { get; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface CTAdaptiveImageProviding {
		[Abstract]
		[Export ("imageForProposedSize:scaleFactor:imageOffset:imageSize:")]
		[return: NullAllowed]
		CGImage GetImage (CGSize proposedSize, nfloat scaleFactor, out CGPoint imageOffset, out CGSize imageSize);
	}
}
