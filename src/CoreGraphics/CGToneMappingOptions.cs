// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Foundation;
using ObjCRuntime;

namespace CoreGraphics {
	/// <summary>A dictionary of tone mapping options.</summary>
	partial class CGToneMappingOptions {
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr /* CFDictionaryRef */ CGEXRToneMappingGammaGetDefaultOptions ();

		/// <summary>Get the default options for tone mapping using the EXR Gamma method.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		[SupportedOSPlatform ("macos26.0")]
		public static CGToneMappingOptions? GetDefaultExrToneMappingGammaOptions ()
		{
			var handle = CGEXRToneMappingGammaGetDefaultOptions ();
			var dict = Runtime.GetNSObject<NSDictionary> (handle);
			if (dict is null)
				return null;
			return new CGToneMappingOptions (dict);
		}
	}
}
