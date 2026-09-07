//
// MLMultiArrayConstraint.cs
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

#nullable enable

namespace CoreML {
	public partial class MLMultiArrayConstraint {
		/// <summary>Gets an array of array dimensions for the multidimensional arrays.</summary>
		/// <value>An array of dimension sizes.</value>
		public nint [] Shape {
			get {
				return MLMultiArray.ConvertArray (_Shape);
			}
		}
	}
}
