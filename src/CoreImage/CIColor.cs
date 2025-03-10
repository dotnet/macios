//
// CIColor.cs: Extensions
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2014 Xamarin Inc.
//
using System;
using System.Diagnostics;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
#if !MONOMAC
using UIKit;
#endif

#nullable enable

namespace CoreImage {
	public partial class CIColor {

		/// <summary>Gets the color components, including the alpha channel if it is present, as an array of floating point numbers that are each in the range [0,1].</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
