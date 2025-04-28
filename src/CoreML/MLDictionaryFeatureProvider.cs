//
// MLDictionaryFeatureProvider.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace CoreML {
	public partial class MLDictionaryFeatureProvider {

		/// <param name="featureName">The feature name of the requested value.</param>
		/// <summary>Retrieves the <see cref="T:CoreML.MLFeatureValue" /> for the specified <paramref name="featureName" />.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public MLFeatureValue? this [string featureName] {
			get { return GetFeatureValue (featureName); }
		}
	}
}
