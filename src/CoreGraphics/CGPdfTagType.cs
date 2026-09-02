//
// CGPdfTagType.cs
//
// Author:
//   Vincent Dondain (vidondai@microsoft.com)
//
// Copyright 2018-2019 Microsoft Corporation
//

#nullable enable

namespace CoreGraphics {

	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
	public static class CGPdfTagType_Extensions {

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* const char * _Nullable */ IntPtr CGPDFTagTypeGetName (CGPdfTagType tagType);

		public static string? GetName (this CGPdfTagType self)
		{
			return Marshal.PtrToStringAnsi (CGPDFTagTypeGetName (self));
		}
	}
}
