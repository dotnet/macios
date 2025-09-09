using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace ARKit {
	public partial class ARSkeleton {
		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.ARKitLibrary)]
		static extern IntPtr /* NSString */ ARSkeletonJointNameForRecognizedPointKey (/* NSString */ IntPtr recognizedPointKey);

		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static NSString? CreateJointName (NSString recognizedPointKey)
		{
			if (recognizedPointKey is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (recognizedPointKey));
			NSString? result = Runtime.GetNSObject<NSString> (ARSkeletonJointNameForRecognizedPointKey (recognizedPointKey.Handle));
			GC.KeepAlive (recognizedPointKey);
			return result;
		}
	}
}
