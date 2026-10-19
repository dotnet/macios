using System.ComponentModel;
using Vision;

#nullable enable

namespace ARKit {
	public partial class ARSkeleton {
		[SupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.ARKitLibrary)]
		static extern IntPtr /* NSString */ ARSkeletonJointNameForRecognizedPointKey (/* NSString */ IntPtr recognizedPointKey);

		[SupportedOSPlatform ("ios")]
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

		/// <summary>Returns the ARKit joint name corresponding to a Vision human body joint.</summary>
		/// <param name="recognizedPointKey">The Vision human body joint to map.</param>
		/// <returns>The joint name for an <see cref="ARSkeleton2D" />, or <see langword="null" /> if no mapping exists.</returns>
		/// <exception cref="ArgumentOutOfRangeException">The joint has no associated Vision constant.</exception>
		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public static NSString? CreateJointName (VNHumanBodyPoseObservationJointName recognizedPointKey)
		{
			var constant = recognizedPointKey.GetConstant ();
			if (constant is null)
				throw new ArgumentOutOfRangeException (nameof (recognizedPointKey));
			return CreateJointName (constant);
		}
	}
}
