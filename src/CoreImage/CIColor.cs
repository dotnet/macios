//
// CIColor.cs: Extensions
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2014 Xamarin Inc.
//
using System.Diagnostics;
using CoreGraphics;
#if !MONOMAC
using UIKit;
#endif

#nullable enable

namespace CoreImage {
	public partial class CIColor {

		/// <summary>Gets the color components, including the alpha channel if present, as an array of floating point numbers in the range [0,1].</summary>
		/// <value>An array of color component values.</value>
		public nfloat [] Components {
			get {
				var n = NumberOfComponents;
				var result = new nfloat [n];
				unsafe {
					nfloat* p = (nfloat*) GetComponents ();
					for (int i = 0; i < n; i++)
						result [i] = p [i];
				}
				return result;
			}
		}
	}
}
