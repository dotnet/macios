using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreML {

	public partial class MLModel {
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.CoreMLLibrary)]
		static extern /* MLComputeDeviceProtocol[] */ IntPtr MLAllComputeDevices ();

		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public static IMLComputeDeviceProtocol [] AllComputeDevices {
			get {
				var ptr = MLAllComputeDevices ();
				if (ptr == IntPtr.Zero)
					return Array.Empty<IMLComputeDeviceProtocol> ();
				return NSArray.ArrayFromHandle<IMLComputeDeviceProtocol> (ptr);
			}
		}
	}
}
