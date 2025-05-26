//
// NIAlgorithmConvergenceStatusReasonDescription manual binding
//
// Authors:
//	TJ Lambert  <TJ.Lambert@microsoft.com>
//
// Copyright 2022 Microsoft Corp.
//

#if IOS || __MACCATALYST__

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace NearbyInteraction {
	public partial class NIAlgorithmConvergenceStatusReasonValues {
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		[DllImport (Constants.NearbyInteractionLibrary)]
		static extern NativeHandle /* NSString */ NIAlgorithmConvergenceStatusReasonDescription (NativeHandle /* NIAlgorithmConvergenceStatusReason */ reason);

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public static NSString GetConvergenceStatusReason (NIAlgorithmConvergenceStatusReason reason)
		{
			return Runtime.GetNSObject<NSString> (NIAlgorithmConvergenceStatusReasonDescription (reason.GetConstant ().GetHandle ()))!;
		}
	}
}
#endif // IOS || __MACCATALYST__
