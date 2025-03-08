//
// VNDetectBarcodesRequest.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;

namespace Vision {
	public partial class VNDetectBarcodesRequest {

		/// <summary>Gets or sets the array of <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=T:VNBarcodeSymbology&amp;scope=Xamarin" title="T:VNBarcodeSymbology">T:VNBarcodeSymbology</a></format> types the request should attempt to recognize.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public VNBarcodeSymbology [] Symbologies {
			get { return VNBarcodeSymbologyExtensions.GetValues (WeakSymbologies); }
			set { WeakSymbologies = value.GetConstants (); }
		}
	}
}
