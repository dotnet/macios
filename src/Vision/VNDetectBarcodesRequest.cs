//
// VNDetectBarcodesRequest.cs
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
//

#nullable enable

namespace Vision {
	public partial class VNDetectBarcodesRequest {

		/// <summary>Gets or sets the array of <see cref="VNBarcodeSymbology" /> types the request should attempt to recognize.</summary>
		/// <value>An array of barcode symbologies to detect.</value>
		public VNBarcodeSymbology [] Symbologies {
			get { return VNBarcodeSymbologyExtensions.GetValues (WeakSymbologies); }
			set { WeakSymbologies = value.GetConstants (); }
		}
	}
}
