//
// MLMultiArrayConstraint.cs
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2017 Microsoft Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace CoreML {
	public partial class MLMultiArrayConstraint {
		/// <summary>Gets an array of array dimensions for the multidimensional arrays.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint [] Shape {
			get {
				return MLMultiArray.ConvertArray (_Shape);
			}
		}
	}
}
