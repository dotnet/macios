//
// PHAssetCreationRequest.cs: supporting code to enhance the API
//
// Copyright 2015 Xamarin Inc
//
// Authors:
//    Miguel de Icaza (miguel@xamarin.com)

#nullable enable

namespace Photos {

	partial class PHAssetCreationRequest {
		/// <param name="resourceTypes">The resource types to check for support.</param>
		/// <summary>Whether Photos supports creating an asset that combines the specified <paramref name="resourceTypes" />.</summary>
		/// <returns><see langword="true" /> if the combination of resource types is supported; otherwise, <see langword="false" />.</returns>
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
