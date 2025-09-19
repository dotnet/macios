// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using ObjCRuntime;

namespace CoreGraphics {
	/// <summary>A struct that defines tone mapping information.</summary>
	[SupportedOSPlatform ("ios26.0")]
	[SupportedOSPlatform ("tvos26.0")]
	[SupportedOSPlatform ("maccatalyst26.0")]
	[SupportedOSPlatform ("macos26.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CGContentToneMappingInfo {
		CGToneMapping method;
		IntPtr /* CFDictionaryRef __nullable */ options;

		/// <summary>The tone mapping method to use.</summary>
		public CGToneMapping Method {
			get => method;
			set => method = value;
		}

		/// <summary>Any tone-mapping options.</summary>
		public NSDictionary? Options {
			get => Runtime.GetNSObject<NSDictionary> (options);
			set => options = Runtime.RetainAndAutoreleaseNSObject (value);
		}

		/// <summary>Strongly typed tone-mapping options.</summary>
		public CGToneMappingOptions? ToneMappingOptions {
			get {
				var dict = Options;
				if (dict is null)
					return null;
				return new CGToneMappingOptions (dict);
			}
			set {
				Options = value.GetDictionary ();
			}
		}
	}
}
