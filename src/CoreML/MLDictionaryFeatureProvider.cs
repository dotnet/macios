//
// MLDictionaryFeatureProvider.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace CoreML {
	public partial class MLDictionaryFeatureProvider {

		/// <param name="featureName">The feature name of the requested value.</param>
		/// <summary>Retrieves the <see cref="MLFeatureValue" /> for the specified <paramref name="featureName" />.</summary>
		/// <value>The feature value associated with the specified name, or <see langword="null" /> if not found.</value>
		public MLFeatureValue? this [string featureName] {
			get { return GetFeatureValue (featureName); }
		}
	}
}
