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

		/// <summary>Gets or sets the array of <see cref="VNBarcodeSymbology" /> types the request should attempt to recognize.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public VNBarcodeSymbology [] Symbologies {
			get { return VNBarcodeSymbologyExtensions.GetValues (WeakSymbologies); }
			set { WeakSymbologies = value.GetConstants (); }
		}
	}
}
