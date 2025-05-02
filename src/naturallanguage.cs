// Copyright 2018-2019 Microsoft, Corp.
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

using System;
using System.ComponentModel;
using Foundation;
using CoreML;
using ObjCRuntime;

namespace NaturalLanguage {

	/// <summary>Determines the most likely language in which a text is written.</summary>
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // designated
	[BaseType (typeof (NSObject))]
	interface NLLanguageRecognizer {

		/// <summary>Creates a new <see cref="NaturalLanguage.NLLanguageRecognizer" /> with default values.</summary>
		/// <remarks />
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		[Static]
		[Internal]
		[Export ("dominantLanguageForString:")]
		[return: NullAllowed]
		NSString _GetDominantLanguage (IntPtr @string);

		/// <param name="string">The text to process.</param>
		///         <summary>Evaluates <paramref name="string" /> to determine the language it was most likely to have been written in.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("processString:")]
		void Process (string @string);

		/// <summary>Resets the recognizer, discarding recognition results and any text supplied to <see cref="NaturalLanguage.NLLanguageRecognizer.Process(System.String)" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reset")]
		void Reset ();

		[Internal]
		[NullAllowed, Export ("dominantLanguage")]
		NSString _DominantLanguage { get; }

		/// <summary>Gets the language in which the text that was analyzed with <see cref="NaturalLanguage.NLLanguageRecognizer.Process(System.String)" /> was most likely written.</summary>
		///         <value>The the language in which the text was most likely written.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("NLLanguageExtensions.GetValue (_DominantLanguage)")]
		NLLanguage DominantLanguage { get; }

		// left in case the user does not want to get a c# dict
		/// <param name="maxHypotheses">The maximum number of hypotheses to return.</param>
		/// <summary>Returns a dictionary of probabilities, keyed by language, that describes the most likely languages in which the text that was analyzed with <see cref="NaturalLanguage.NLLanguageRecognizer.Process(System.String)" /> was written.</summary>
		/// <returns>A dictionary of probabilities, keyed by language, that describes the most likely languages in which the text that was analyzed with <see cref="NaturalLanguage.NLLanguageRecognizer.Process(System.String)" /> was written.</returns>
		/// <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("languageHypothesesWithMaximum:")]
		NSDictionary<NSString, NSNumber> GetNativeLanguageHypotheses (nuint maxHypotheses);

		// left in case the user does not want to get a c# dict
		/// <summary>Gets or sets a list of language hints that use native types.</summary>
		///         <value>A list of language hints that use native types.</value>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("languageHints", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> NativeLanguageHints { get; set; }

		[Internal]
		[Export ("languageConstraints", ArgumentSemantic.Copy)]
		NSString [] _LanguageConstraints { get; set; }

		/// <summary>Gets or sets the array of languages from which the recognizer must choose.</summary>
		///         <value>The array of languages from which the recognizer must choose..</value>
		///         <remarks>To be added.</remarks>
		NLLanguage [] LanguageConstraints {
			[Wrap ("Array.ConvertAll (_LanguageConstraints, e => NLLanguageExtensions.GetValue (e))")]
			get;
			[Wrap ("_LanguageConstraints = Array.ConvertAll (value, e => NLLanguageExtensions.GetConstant (e)!)")]
			set;
		}
	}

	/// <summary>Contains a configuration for a <see cref="NaturalLanguage.NLModel" />.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface NLModelConfiguration : NSCopying, NSSecureCoding {
		/// <summary>Gets the model type.</summary>
		///         <value>A value that tells whether the model tags at the token level, or at a higher level.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type")]
		NLModelType Type { get; }

		[Internal]
		[NullAllowed, Export ("language")]
		NSString _Language { get; }

		/// <summary>Gets the language that the model supports.</summary>
		///         <value>The language that the model supports..</value>
		///         <remarks>To be added.</remarks>
		NLLanguage Language { [Wrap ("(_Language is not null)? NLLanguageExtensions.GetValue (_Language) : NLLanguage.Undetermined")] get; }

		/// <summary>Gets the framework version on which the model was trained.</summary>
		///         <value>The framework version on which the model was trained.</value>
		///         <remarks>To be added.</remarks>
		[Export ("revision")]
		nuint Revision { get; }

		/// <param name="type">The model type for which to inquire about the versions of support.</param>
		///         <summary>Gets the revisions of natural language support that work with models of the specified type.</summary>
		///         <returns>The revisions of natural language support that work with models of the specified type.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("supportedRevisionsForType:")]
		NSIndexSet GetSupportedRevisions (NLModelType type);

		/// <param name="type">The model type for which to inquire about the version of support.</param>
		///         <summary>Gets the revision of the current natural language support for models of the specified type.</summary>
		///         <returns>The revision of the current natural language support for models of the specified type.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("currentRevisionForType:")]
		nuint GetCurrentRevision (NLModelType type);
	}

	/// <summary>Imports custom classification and tagging models into the application.</summary>
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface NLModel {
		/// <param name="url">The location of the custom tagging or language recognition model.</param>
		///         <param name="error">A location in which to write any errors that occur.</param>
		///         <summary>Creates and returns a new NLModel from the custom tagging or language recognition model at the specified <paramref name="url" />.</summary>
		///         <returns>The new model.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("modelWithContentsOfURL:error:")]
		[return: NullAllowed]
		NLModel Create (NSUrl url, [NullAllowed] out NSError error);

		/// <param name="mlModel">The model to import.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Creates and returns a new NLModel from the provided custom tagging or language recognition model.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("modelWithMLModel:error:")]
		[return: NullAllowed]
		NLModel Create (MLModel mlModel, [NullAllowed] out NSError error);

		/// <summary>Gets the configuration information, such as which language the model supports, or the current version of the model.</summary>
		///         <value>The configuration information, such as which language the model supports, or the current version of the model.</value>
		///         <remarks>To be added.</remarks>
		[Export ("configuration", ArgumentSemantic.Copy)]
		NLModelConfiguration Configuration { get; }

		/// <param name="string">The string for which to get a prediction.</param>
		///         <summary>Returns the prediction for the string.</summary>
		///         <returns>The prediction for the string.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("predictedLabelForString:")]
		[return: NullAllowed]
		string GetPredictedLabel (string @string);

		/// <param name="tokens">The strings for which to get a prediction.</param>
		///         <summary>Returns the prediction for the strings.</summary>
		///         <returns>The prediction for the strings.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("predictedLabelsForTokens:")]
		string [] GetPredictedLabels (string [] tokens);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("predictedLabelHypothesesForString:maximumCount:")]
		// `Native` added (like existing API) because we provide a better API with manual bindings (to avoid NSNumber)
		NSDictionary<NSString, NSNumber> GetNativePredictedLabelHypotheses (string @string, nuint maximumCount);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("predictedLabelHypothesesForTokens:maximumCount:")]
		// `Native` added (like existing API) because we provide a better API with manual bindings (to avoid NSNumber)
		NSDictionary<NSString, NSNumber> [] GetNativePredictedLabelHypotheses (string [] tokens, nuint maximumCount);
	}

	/// <param name="tokenRange">The range of tokens to which to apply the delegate.</param>
	///     <param name="flags">Tokenizer hints.</param>
	///     <param name="stop">
	///       <see langword="false" /> to stop enumerating.</param>
	///     <summary>Delegate to apply to tokens as they are enumerated by <see cref="NaturalLanguage.NLTokenizer.EnumerateTokens(Foundation.NSRange,NaturalLanguage.NLTokenizerEnumerateContinuationHandler)" />.</summary>
	delegate void NLTokenizerEnumerateContinuationHandler (NSRange tokenRange, NLTokenizerAttributes flags, out bool stop);

	/// <summary>Breaks a text up into semantic units.</summary>
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface NLTokenizer {
		/// <param name="unit">The unit into which the tokenizer will separate text.</param>
		/// <summary>Creates a new tokenizer that breaks text up into the specified semantic <paramref name="unit" />s.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithUnit:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NLTokenUnit unit);

		/// <summary>Gets the semantic unit of the tokens that this tokenizer returns.</summary>
		///         <value>The semantic unit of the tokens that this tokenizer returns.</value>
		///         <remarks>To be added.</remarks>
		[Export ("unit")]
		NLTokenUnit Unit { get; }

		/// <summary>Gets or sets the string to process.</summary>
		///         <value>The string to process.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("string", ArgumentSemantic.Retain)]
		string String { get; set; }

		[Internal]
		[Export ("setLanguage:")]
		void _SetLanguage (NSString language);

		/// <param name="language">The language value to set.</param>
		///         <summary>Sets the language that the tokenizer will use when processing the string.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("_SetLanguage (language.GetConstant ()!)")]
		void SetLanguage (NLLanguage language);

		/// <param name="characterIndex">The index of a character that is covered by a token.</param>
		/// <summary>Gets the range of the token that covers the specified character index.</summary>
		/// <returns>The range of the token that covers the specified character index.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("tokenRangeAtIndex:")]
		NSRange GetTokenRange (nuint characterIndex);

		/// <param name="range">The range for which to return all tokens.</param>
		///         <summary>Tokenizes the specified range of text.</summary>
		///         <returns>Tokens for the specified range.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("tokensForRange:")]
		NSValue [] GetTokens (NSRange range);

		/// <param name="range">The lexical range over which to get tokens.</param>
		///         <param name="handler">A handler to run on each token.</param>
		///         <summary>Enumerates tokens for the specified range in the text.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("enumerateTokensInRange:usingBlock:")]
		void EnumerateTokens (NSRange range, NLTokenizerEnumerateContinuationHandler handler);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("tokenRangeForRange:")]
		NSRange GetTokenRange (NSRange range);
	}

	/// <param name="tag">The tag on which to operate.</param>
	///     <param name="tokenRange">The token range to process.</param>
	///     <param name="stop">
	///       <see langword="false" /> to stop enumerating.</param>
	///     <summary>Delegate to apply to tokens as they are enumerated by <see cref="NaturalLanguage.NLTagger.EnumerateTags(Foundation.NSRange,NaturalLanguage.NLTokenUnit,NaturalLanguage.NLTagScheme,NaturalLanguage.NLTaggerOptions,NaturalLanguage.NLTaggerEnumerateTagsContinuationHandler)" />.</summary>
	delegate void NLTaggerEnumerateTagsContinuationHandler (NSString tag, NSRange tokenRange, out bool stop);

	/// <summary>Analyzes text and produces an enumerable list of tags drawn from a specified set of tag schemas.</summary>
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface NLTagger {
		/// <param name="tagSchemes">The taggging schemes that detail the classifications to return.</param>
		/// <summary>Initializes a tagger that classifies tokens according the the identified tagging schemes.</summary>
		/// <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("initWithTagSchemes:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([Params] NSString [] tagSchemes);

		/// <param name="tagSchemes">The taggging schemes that detail the classifications to return.</param>
		/// <summary>Initializes a tagger that classifies tokens according the the provided tagging schemes.</summary>
		/// <remarks>To be added.</remarks>
		[Wrap ("this (Array.ConvertAll (tagSchemes, e => e.GetConstant ()!))")]
		NativeHandle Constructor ([Params] NLTagScheme [] tagSchemes);

		[Internal]
		[Export ("tagSchemes", ArgumentSemantic.Copy)]
		NSString [] _TagSchemes { get; }

		/// <summary>Gets the tagging schemes that are supported by this tagger.</summary>
		///         <value>The tagging schemes that are supported by this tagger.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("Array.ConvertAll (_TagSchemes, e => NLTagSchemeExtensions.GetValue (e))")]
		NLTagScheme [] TagSchemes { get; }

		/// <summary>Gets or sets the string to tag.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("string", ArgumentSemantic.Retain)]
		string String { get; set; }

		/// <param name="unit">The unit for which to get the available tag schemes.</param>
		///         <param name="language">The language that constrains the tags available for the <paramref name="unit" />.</param>
		///         <summary>Returns the available tag schemes for <paramref name="unit" /> and <paramref name="language" />.</summary>
		///         <returns>The available tag schemes for <paramref name="unit" /> and <paramref name="language" />.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("availableTagSchemesForUnit:language:")]
		NSString [] GetAvailableTagSchemes (NLTokenUnit unit, NSString language);

		/// <param name="unit">The unit for which to get the available tag schemes.</param>
		///         <param name="language">The language that constrains the tags available for the <paramref name="unit" />.</param>
		///         <summary>Returns the available tag schemes for <paramref name="unit" /> and <paramref name="language" />.</summary>
		///         <returns>The available tag schemes for <paramref name="unit" /> and <paramref name="language" />.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("Array.ConvertAll (GetAvailableTagSchemes (unit, language.GetConstant()!), e => NLTagSchemeExtensions.GetValue (e))")]
		NLTagScheme [] GetAvailableTagSchemes (NLTokenUnit unit, NLLanguage language);

		/// <param name="characterIndex">A character index for the desired range.</param>
		/// <param name="unit">The unit, which covers the <paramref name="characterIndex" />, whose range to get.</param>
		/// <summary>Returns the lexical range of the <paramref name="unit" /> that contains the spcified <paramref name="characterIndex" />.</summary>
		/// <returns>The lexical range of the <paramref name="unit" /> that contains the spcified <paramref name="characterIndex" />.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("tokenRangeAtIndex:unit:")]
		NSRange GetTokenRange (nuint characterIndex, NSString unit);

		[Internal]
		[NullAllowed, Export ("dominantLanguage")]
		NSString _DominantLanguage { get; }

		/// <summary>Gets the dominant language for the tagged text.</summary>
		///         <value>The dominant language for the tagged text.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("NLLanguageExtensions.GetValue (_DominantLanguage)")]
		NLLanguage DominantLanguage { get; }

		/// <param name="range">The range of the tag.</param>
		///         <param name="unit">The lexical unit of the tag.</param>
		///         <param name="scheme">The schemes for which to enumerate the corresponding tags.</param>
		///         <param name="options">Options that control preprocessing done to tags.</param>
		///         <param name="handler">A handler to run on enumerated tags.</param>
		///         <summary>Enumerates over a filtered list of tags and applies a handler.</summary>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("enumerateTagsInRange:unit:scheme:options:usingBlock:")]
		void EnumerateTags (NSRange range, NLTokenUnit unit, NSString scheme, NLTaggerOptions options, NLTaggerEnumerateTagsContinuationHandler handler);

		/// <param name="range">The range of the tag.</param>
		///         <param name="unit">The lexical unit of the tag.</param>
		///         <param name="scheme">The schemes for which to enumerate the corresponding tags.</param>
		///         <param name="options">Options that control preprocessing done to tags.</param>
		///         <param name="handler">A handler to run on enumerated tags.</param>
		///         <summary>Enumerates over a filtered list of tags and applies a handler.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("EnumerateTags (range, unit, scheme.GetConstant ()!, options, handler)")]
		void EnumerateTags (NSRange range, NLTokenUnit unit, NLTagScheme scheme, NLTaggerOptions options, NLTaggerEnumerateTagsContinuationHandler handler);

		/// <param name="characterIndex">To be added.</param>
		/// <param name="unit">To be added.</param>
		/// <param name="scheme">To be added.</param>
		/// <param name="tokenRange">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("tagAtIndex:unit:scheme:tokenRange:")]
		[return: NullAllowed]
		NSString GetTag (nuint characterIndex, NLTokenUnit unit, NSString scheme, out NSRange tokenRange);

		/// <param name="characterIndex">To be added.</param>
		/// <param name="unit">To be added.</param>
		/// <param name="scheme">To be added.</param>
		/// <param name="tokenRange">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Wrap ("GetTag (characterIndex, unit, scheme.GetConstant ()!, out tokenRange)")]
		NSString GetTag (nuint characterIndex, NLTokenUnit unit, NLTagScheme scheme, out NSRange tokenRange);

		/// <param name="range">The index range of the characters from which to get tags.</param>
		///         <param name="unit">The token unit for the tags to retrieve.</param>
		///         <param name="scheme">The tag scheme for the tags to retrieve.</param>
		///         <param name="options">Options that control preprocessing done to tags.</param>
		///         <param name="tokenRanges">Location to store the ranges of the tokens for the returned tags.</param>
		///         <summary>Returns the tags and ranges for a string range and unit.</summary>
		///         <returns>The tags and ranges for the string range and unit.</returns>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("tagsInRange:unit:scheme:options:tokenRanges:")]
		NSString [] GetTags (NSRange range, NLTokenUnit unit, NSString scheme, NLTaggerOptions options, [NullAllowed] out NSValue [] tokenRanges);

		/// <param name="range">To be added.</param>
		///         <param name="unit">To be added.</param>
		///         <param name="scheme">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="tokenRanges">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("GetTags (range, unit, scheme.GetConstant ()!, options, out tokenRanges)")]
		NSString [] GetTags (NSRange range, NLTokenUnit unit, NLTagScheme scheme, NLTaggerOptions options, [NullAllowed] out NSValue [] tokenRanges);

		/// <param name="language">The new language value.</param>
		///         <param name="range">The range to which to apply the change.</param>
		///         <summary>Sets the language for the specified range.</summary>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("setLanguage:range:")]
		void SetLanguage (NSString language, NSRange range);

		/// <param name="language">The new language value.</param>
		///         <param name="range">The range to which to apply the change.</param>
		///         <summary>Sets the language for the specified range.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("SetLanguage (language.GetConstant ()!, range)")]
		void SetLanguage (NLLanguage language, NSRange range);

		/// <param name="orthography">The orthography to set for the range.</param>
		///         <param name="range">The range for which to assign an orthography.</param>
		///         <summary>Assigns an orthography to a range.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setOrthography:range:")]
		void SetOrthography (NSOrthography orthography, NSRange range);

		/// <param name="models">The models to assign to the tag schemes.</param>
		///         <param name="tagScheme">The tag scheme for which to assign the models.</param>
		///         <summary>Assigns models to a tag scheme.</summary>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("setModels:forTagScheme:")]
		void SetModels (NLModel [] models, NSString tagScheme);

		/// <param name="models">The models to assign to the tag schemes.</param>
		///         <param name="tagScheme">The tag scheme for which to assign the models.</param>
		///         <summary>Assigns models to a tag scheme.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("SetModels (models, tagScheme.GetConstant ()!)")]
		void SetModels (NLModel [] models, NLTagScheme tagScheme);

		/// <param name="tagScheme">The tag scheme for which to get corresponding models.</param>
		///         <summary>Returns the models that generate tags from the specified scheme.</summary>
		///         <returns>The models that generate tags from the specified scheme.</returns>
		///         <remarks>To be added.</remarks>
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("modelsForTagScheme:")]
		NLModel [] GetModels (NSString tagScheme);

		/// <param name="tagScheme">The tag scheme for which to get corresponding models.</param>
		///         <summary>Returns the models that generate tags from the specified scheme.</summary>
		///         <returns>The models that generate tags from the specified scheme.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("GetModels (tagScheme.GetConstant ()!)")]
		NLModel [] GetModels (NLTagScheme tagScheme);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("setGazetteers:forTagScheme:")]
		void SetGazetteers (NLGazetteer [] gazetteers, NSString tagScheme);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Wrap ("SetGazetteers (gazetteers, tagScheme.GetConstant ()!)")]
		void SetGazetteers (NLGazetteer [] gazetteers, NLTagScheme tagScheme);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("gazetteersForTagScheme:")]
		NLGazetteer [] GetGazetteers (NSString tagScheme);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Wrap ("GetGazetteers (tagScheme.GetConstant ()!)")]
		NLGazetteer [] GetGazetteers (NLTagScheme tagScheme);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Static]
		[Async]
		[Export ("requestAssetsForLanguage:tagScheme:completionHandler:")]
		void RequestAssets (NSString language, NSString tagScheme, Action<NLTaggerAssetsResult, NSError> completionHandler);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Static]
		[Async]
		[Wrap ("RequestAssets (language.GetConstant ()!, tagScheme.GetConstant ()!, completionHandler)")]
		void RequestAssets (NLLanguage language, NLTagScheme tagScheme, Action<NLTaggerAssetsResult, NSError> completionHandler);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("tagHypothesesAtIndex:unit:scheme:maximumCount:tokenRange:")]
		// `Native` added (like existing API) because we provide a better API with manual bindings (to avoid NSNumber)
		NSDictionary<NSString, NSNumber> GetNativeTagHypotheses (nuint characterIndex, NLTokenUnit unit, NSString scheme, nuint maximumCount, out NSRange tokenRange);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Internal]
		[Sealed]
		[Export ("tagHypothesesAtIndex:unit:scheme:maximumCount:tokenRange:")]
		NSDictionary<NSString, NSNumber> GetTagHypotheses (nuint characterIndex, NLTokenUnit unit, NSString scheme, nuint maximumCount, IntPtr tokenRange);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Wrap ("GetTagHypotheses (characterIndex, unit, scheme, maximumCount, IntPtr.Zero)")]
		// `Native` added (like existing API) because we provide a better API with manual bindings (to avoid NSNumber)
		NSDictionary<NSString, NSNumber> GetNativeTagHypotheses (nuint characterIndex, NLTokenUnit unit, NSString scheme, nuint maximumCount);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("tokenRangeForRange:unit:")]
		NSRange GetTokenRange (NSRange range, NLTokenUnit unit);
	}

	/// <summary>Enumerates token types.</summary>
	[MacCatalyst (13, 1)]
	[Static] // only used to compare with NSString not as input/output
	interface NLTag {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagWord")]
		NSString Word { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagPunctuation")]
		NSString Punctuation { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagWhitespace")]
		NSString Whitespace { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagOther")]
		NSString Other { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagNoun")]
		NSString Noun { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagVerb")]
		NSString Verb { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagAdjective")]
		NSString Adjective { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagAdverb")]
		NSString Adverb { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagPronoun")]
		NSString Pronoun { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagDeterminer")]
		NSString Determiner { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagParticle")]
		NSString Particle { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagPreposition")]
		NSString Preposition { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagNumber")]
		NSString Number { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagConjunction")]
		NSString Conjunction { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagInterjection")]
		NSString Interjection { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagClassifier")]
		NSString Classifier { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagIdiom")]
		NSString Idiom { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagOtherWord")]
		NSString OtherWord { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagSentenceTerminator")]
		NSString SentenceTerminator { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagOpenQuote")]
		NSString OpenQuote { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagCloseQuote")]
		NSString CloseQuote { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagOpenParenthesis")]
		NSString OpenParenthesis { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagCloseParenthesis")]
		NSString CloseParenthesis { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagWordJoiner")]
		NSString WordJoiner { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagDash")]
		NSString Dash { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagOtherPunctuation")]
		NSString OtherPunctuation { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagParagraphBreak")]
		NSString ParagraphBreak { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagOtherWhitespace")]
		NSString OtherWhitespace { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagPersonalName")]
		NSString PersonalName { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagPlaceName")]
		NSString PlaceName { get; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NLTagOrganizationName")]
		NSString OrganizationName { get; }
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NLDistanceType : long {
		Cosine,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	enum NLTaggerAssetsResult : long {
		Available,
		NotAvailable,
		Error,
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	delegate void NLEnumerateNeighborsHandler (string neighbor, /* NLDistance */ double distance, ref bool stop);

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NLEmbedding {

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Static]
		[Export ("wordEmbeddingForLanguage:")]
		[return: NullAllowed]
		NLEmbedding GetWordEmbedding (NSString language);

		[Static]
		[Wrap ("GetWordEmbedding (language.GetConstant ()!)")]
		[return: NullAllowed]
		NLEmbedding GetWordEmbedding (NLLanguage language);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Static]
		[Export ("wordEmbeddingForLanguage:revision:")]
		[return: NullAllowed]
		NLEmbedding GetWordEmbedding (NSString language, nuint revision);

		[Static]
		[Wrap ("GetWordEmbedding (language.GetConstant ()!)")]
		[return: NullAllowed]
		NLEmbedding GetWordEmbedding (NLLanguage language, nuint revision);

		[Static]
		[Export ("embeddingWithContentsOfURL:error:")]
		[return: NullAllowed]
		NLEmbedding GetEmbedding (NSUrl url, [NullAllowed] out NSError error);

		[Export ("containsString:")]
		bool Contains (string @string);

		[Export ("distanceBetweenString:andString:distanceType:")]
		double GetDistance (string firstString, string secondString, NLDistanceType distanceType);

		[Export ("enumerateNeighborsForString:maximumCount:distanceType:usingBlock:")]
		void EnumerateNeighbors (string @string, nuint maxCount, NLDistanceType distanceType, NLEnumerateNeighborsHandler handler);

		[Export ("enumerateNeighborsForString:maximumCount:maximumDistance:distanceType:usingBlock:")]
		void EnumerateNeighbors (string @string, nuint maxCount, double maxDistance, NLDistanceType distanceType, NLEnumerateNeighborsHandler handler);

		[Export ("neighborsForString:maximumCount:distanceType:")]
		[return: NullAllowed]
		string [] GetNeighbors (string @string, nuint maxCount, NLDistanceType distanceType);

		[Export ("neighborsForString:maximumCount:maximumDistance:distanceType:")]
		[return: NullAllowed]
		string [] GetNeighbors (string @string, nuint maxCount, double maxDistance, NLDistanceType distanceType);

		[Export ("vectorForString:")]
		[return: NullAllowed]
		[return: BindAs (typeof (float []))]
		// doc says "array of double" but other API uses float ?!?
		NSNumber [] GetVector (string @string);

		[Internal] // can't bind float[] without NSArray but it will be better bound using .net pattern `bool TryGetVector (string, out float[] vector)`
		[Export ("getVector:forString:")]
		bool GetVector (IntPtr /* float[] */ vector, string @string);

		[Export ("enumerateNeighborsForVector:maximumCount:distanceType:usingBlock:")]
		void EnumerateNeighbors ([BindAs (typeof (float []))] NSNumber [] vector, nuint maxCount, NLDistanceType distanceType, NLEnumerateNeighborsHandler handler);

		[Export ("enumerateNeighborsForVector:maximumCount:maximumDistance:distanceType:usingBlock:")]
		void EnumerateNeighbors ([BindAs (typeof (float []))] NSNumber [] vector, nuint maxCount, double maxDistance, NLDistanceType distanceType, NLEnumerateNeighborsHandler handler);

		[Export ("neighborsForVector:maximumCount:distanceType:")]
		string [] GetNeighbors ([BindAs (typeof (float []))] NSNumber [] vector, nuint maxCount, NLDistanceType distanceType);

		[Export ("neighborsForVector:maximumCount:maximumDistance:distanceType:")]
		string [] GetNeighbors ([BindAs (typeof (float []))] NSNumber [] vector, nuint maxCount, double maxDistance, NLDistanceType distanceType);

		[Export ("dimension")]
		nuint Dimension { get; }

		[Export ("vocabularySize")]
		nuint VocabularySize { get; }

		[NullAllowed, Export ("language")]
		[BindAs (typeof (NLLanguage?))]
		NSString Language { get; }

		[Export ("revision")]
		nuint Revision { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Static]
		[Export ("supportedRevisionsForLanguage:")]
		NSIndexSet GetSupportedRevisions (NSString language);

		[Static]
		[Wrap ("GetSupportedRevisions (language.GetConstant ()!)")]
		NSIndexSet GetSupportedRevisions (NLLanguage language);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Static]
		[Export ("currentRevisionForLanguage:")]
		nuint GetCurrentRevision (NSString language);

		[Static]
		[Wrap ("GetCurrentRevision (language.GetConstant ()!)")]
		nuint GetCurrentRevision (NLLanguage language);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Static]
		[Export ("writeEmbeddingForDictionary:language:revision:toURL:error:")]
		bool Write (NSDictionary dictionary, [NullAllowed] NSString language, nuint revision, NSUrl url, [NullAllowed] out NSError error);

		[Static]
		[Wrap ("Write (dictionary.GetDictionary ()!, language.HasValue ? language.Value.GetConstant () : null, revision, url, out error)")]
		bool Write (NLVectorDictionary dictionary, NLLanguage? language, nuint revision, NSUrl url, [NullAllowed] out NSError error);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Export ("currentSentenceEmbeddingRevisionForLanguage:")]
		nuint GetCurrentSentenceEmbeddingRevision (NSString language);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Wrap ("GetCurrentSentenceEmbeddingRevision (language.GetConstant ()!)")]
		nuint GetCurrentSentenceEmbeddingRevision (NLLanguage language);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Export ("sentenceEmbeddingForLanguage:")]
		[return: NullAllowed]
		NLEmbedding GetSentenceEmbedding (NSString language);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Wrap ("GetSentenceEmbedding (language.GetConstant ()!)")]
		[return: NullAllowed]
		NLEmbedding GetSentenceEmbedding (NLLanguage language);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Export ("sentenceEmbeddingForLanguage:revision:")]
		[return: NullAllowed]
		NLEmbedding GetSentenceEmbedding (NSString language, nuint revision);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Wrap ("GetSentenceEmbedding (language.GetConstant ()!, revision)")]
		[return: NullAllowed]
		NLEmbedding GetSentenceEmbedding (NLLanguage language, nuint revision);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Export ("supportedSentenceEmbeddingRevisionsForLanguage:")]
		NSIndexSet GetSupportedSentenceEmbeddingRevisions (NSString language);

		[TV (14, 0), iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Static]
		[Wrap ("GetSupportedSentenceEmbeddingRevisions (language.GetConstant ()!)")]
		NSIndexSet GetSupportedSentenceEmbeddingRevisions (NLLanguage language);
	}

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NLGazetteer {

		[Static]
		[Export ("gazetteerWithContentsOfURL:error:")]
		[return: NullAllowed]
		NLGazetteer Create (NSUrl url, [NullAllowed] out NSError error);

		[Export ("initWithContentsOfURL:error:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl url, [NullAllowed] out NSError error);

		[Export ("initWithData:error:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSData data, [NullAllowed] out NSError error);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("initWithDictionary:language:error:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary dictionary, [NullAllowed] NSString language, [NullAllowed] out NSError error);

		// sadly `language?.GetConstant ()` does not cut it :(
		// error CS1929: 'NLLanguage?' does not contain a definition for 'GetConstant' and the best extension method overload 'NLLanguageExtensions.GetConstant(NLLanguage)' requires a receiver of type 'NLLanguage'
		[Wrap ("this (dictionary.GetDictionary ()!, language.HasValue ? language.Value.GetConstant () : null, out error)")]
		NativeHandle Constructor (NLStrongDictionary dictionary, NLLanguage? language, [NullAllowed] out NSError error);

		[Export ("labelForString:")]
		[return: NullAllowed]
		string GetLabel (string @string);

		[NullAllowed, Export ("language")]
		[BindAs (typeof (NLLanguage?))]
		NSString Language { get; }

		[Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; }

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Static]
		[Export ("writeGazetteerForDictionary:language:toURL:error:")]
		bool Write (NSDictionary dictionary, [NullAllowed] NSString language, NSUrl url, [NullAllowed] out NSError error);

		[Static]
		[Wrap ("Write (dictionary.GetDictionary ()!, language.HasValue ? language.Value.GetConstant () : null, url, out error)")]
		bool Write (NLStrongDictionary dictionary, NLLanguage? language, NSUrl url, [NullAllowed] out NSError error);
	}

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NLContextualEmbedding {
		[Static]
		[Export ("contextualEmbeddingWithModelIdentifier:")]
		[return: NullAllowed]
		NLContextualEmbedding CreateWithModelIdentifier (string modelIdentifier);

		[Static]
		[Export ("contextualEmbeddingsForValues:")]
		NLContextualEmbedding [] Create (NSDictionary<NSString, NSObject> values);

		[Static]
		[Export ("contextualEmbeddingWithLanguage:")]
		[return: NullAllowed]
		NLContextualEmbedding CreateWithLanguage (string language);

		[Static]
		[Export ("contextualEmbeddingWithScript:")]
		[return: NullAllowed]
		NLContextualEmbedding CreateWithScript (string script);

		[Export ("modelIdentifier")]
		string ModelIdentifier { get; }

		[Export ("languages", ArgumentSemantic.Copy)]
		string [] Languages { get; }

		[Export ("scripts", ArgumentSemantic.Copy)]
		string [] Scripts { get; }

		[Export ("revision")]
		nuint Revision { get; }

		[Export ("dimension")]
		nuint Dimension { get; }

		[Export ("maximumSequenceLength")]
		nuint MaximumSequenceLength { get; }

		[Export ("loadWithError:")]
		bool Load ([NullAllowed] out NSError error);

		[Export ("unload")]
		void Unload ();

		[Export ("embeddingResultForString:language:error:")]
		[return: NullAllowed]
		NLContextualEmbeddingResult GetEmbeddingResult (string @string, [NullAllowed] string language, [NullAllowed] out NSError error);

		[Export ("hasAvailableAssets")]
		bool HasAvailableAssets { get; }

		[Export ("requestEmbeddingAssetsWithCompletionHandler:")]
		[Async]
		void RequestAssets (Action<NLContextualEmbeddingAssetsResult, NSError> completionHandler);
	}

	delegate void TokenVectorEnumeratorHandler (NSArray<NSNumber> tokenVector, NSRange tokenRange, out bool stop);

	[TV (17, 0), Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NLContextualEmbeddingResult {
		[Export ("string")]
		string String { get; }

		[Export ("language")]
		string Language { get; }

		[Export ("sequenceLength")]
		nuint SequenceLength { get; }

		[Export ("enumerateTokenVectorsInRange:usingBlock:")]
		void EnumerateTokenVectors (NSRange range, TokenVectorEnumeratorHandler enumerationHandler);

		[return: BindAs (typeof (nuint []))]
		[Export ("tokenVectorAtIndex:tokenRange:")]
		[return: NullAllowed]
		NSNumber [] GetVector (nuint characterIndex, ref NSRange tokenRange);
	}
}
