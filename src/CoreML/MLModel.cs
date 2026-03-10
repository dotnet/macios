
#nullable enable

namespace CoreML {

	public partial class MLModel {
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMLLibrary)]
		static extern /* MLComputeDeviceProtocol[] */ IntPtr MLAllComputeDevices ();

		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public static IMLComputeDeviceProtocol [] AllComputeDevices {
			get {
				var ptr = MLAllComputeDevices ();
				return NSArray.NonNullArrayFromHandleDropNullElements<IMLComputeDeviceProtocol> (ptr);
			}
		}
	}
}
