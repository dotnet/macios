// Copyright 2015 Xamarin Inc.

#nullable enable

#if !COREBUILD

using CoreGraphics;

namespace TVServices {

	static public class TVContentItemImageShapeExtensions {
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos13.0")]
		[DllImport (Constants.TVServicesLibrary)]
		static extern CGSize TVTopShelfImageSizeForShape (/* TVContentItemImageShape */ nint shape,
			/* TVTopShelfContentStyle */ nint style);

		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos13.0")]
		static public CGSize GetSize (this TVContentItemImageShape self, TVTopShelfContentStyle style)
		{
			return TVTopShelfImageSizeForShape ((nint) (int) self, (nint) (int) style);
		}
	}
}

#endif
