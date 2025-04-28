//
// searchkit.cs: Definitions for AppKit
//

using System;
using Foundation;
using ObjCRuntime;

namespace SearchKit {
	[Static]
	interface SKTextAnalysisKeys {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKMinTermLength")]
		NSString MinTermLengthKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKStopWords")]
		NSString StopWordsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKSubstitutions")]
		NSString SubstitutionsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKMaximumTerms")]
		NSString MaximumTermsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKProximityIndexing")]
		NSString ProximityIndexingKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKTermChars")]
		NSString TermCharsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKStartTermChars")]
		NSString StartTermCharsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kSKEndTermChars")]
		NSString EndTermCharsKey { get; }
	}

	[StrongDictionary ("SKTextAnalysisKeys")]
	interface SKTextAnalysis {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		int MinTermLength { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSSet StopWords { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDictionary Substitutions { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSNumber MaximumTerms { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		bool ProximityIndexing { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string TermChars { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string StartTermChars { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string EndTermChars { get; set; }
	}
}
