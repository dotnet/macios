#if !WATCH

using System;

using Foundation;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace AVFoundation {
	public partial class AVPlayerItem {

#if NET
		[SupportedOSPlatform ("tvos11.0")]
		[SupportedOSPlatform ("macos10.13")]
		[SupportedOSPlatform ("ios11.0")]
#else
		[TV (11, 0)]
		[NoWatch]
		[Mac (10, 13)]
		[iOS (11, 0)]
#endif
		public AVVideoApertureMode VideoApertureMode {
			get { return AVVideoApertureModeExtensions.GetValue (_VideoApertureMode); }
			set { _VideoApertureMode = value.GetConstant (); }
		}
	}
}

#endif
