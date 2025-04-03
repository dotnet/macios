//
// PHAssetCreationRequest.cs: supporting code to enhance the API
//
// Copyright 2015 Xamarin Inc
//
// Authors:
//    Miguel de Icaza (miguel@xamarin.com)

#nullable enable

using System;
using Foundation;

namespace Photos {

	partial class PHAssetCreationRequest {
		/// <param name="resourceTypes">To be added.</param>
		///         <summary>Whether Photos supports creating an asset that combines the specified <paramref name="resourceTypes" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool SupportsAssetResourceTypes (params PHAssetResourceType [] resourceTypes)
		{
			var l = resourceTypes.Length;
			if (l == 0)
				return false;
			var a = new NSNumber [l];
			for (int i = 0; i < l; i++)
				a [i] = new NSNumber ((int) resourceTypes [i]);
			return _SupportsAssetResourceTypes (a);
		}
	}
}
