//
// CoreMotion's struct and enum definitions used by the API file
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Foundation;
using ObjCRuntime;

namespace CoreMotion {

	// CMAccelerometer.h
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct CMAcceleration {
		/// <summary>Acceleration along the X axis, in Gs.</summary>
		///         <remarks>To be added.</remarks>
		public double X;
		/// <summary>Acceleration along the Y axis, in Gs.</summary>
		///         <remarks>To be added.</remarks>
		public double Y;
		/// <summary>Acceleration along the Z axis, in Gs.</summary>
		///         <remarks>To be added.</remarks>
		public double Z;

		public CMAcceleration (double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public override string ToString ()
		{
			return String.Format ("a=({0},{1},{2})", X, Y, Z);
		}
	}

	// CMAttitude.h
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct CMRotationMatrix {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m11;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m12;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m13;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m21;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m22;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m23;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m31;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m32;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public double m33;
	}

	// CMAttitude.h
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct CMQuaternion {
		/// <summary>The imaginary x component.</summary>
		///         <remarks>To be added.</remarks>
		public double x;
		/// <summary>The imaginary y component.</summary>
		///         <remarks>To be added.</remarks>
		public double y;
		/// <summary>The imaginary z component.</summary>
		///         <remarks>To be added.</remarks>
		public double z;
		/// <summary>The real part of the quaternion.</summary>
		///         <remarks>To be added.</remarks>
		public double w;

		public CMQuaternion (double x, double y, double z, double w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public override string ToString ()
		{
			return String.Format ("quaternion=({0},{1},{2},{3})", x, y, z, w);
		}
	}

	// CMGyro.h
#if NET
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct CMRotationRate {
		/// <summary>The rotation rate around the X axis, in radians per second.</summary>
		///         <remarks>To be added.</remarks>
		public double x;
		/// <summary>The rotation rate around the Y axis, in radians per second.</summary>
		///         <remarks>To be added.</remarks>
		public double y;
		/// <summary>The rotation rate around the Z axis, in radians per second.</summary>
		///         <remarks>To be added.</remarks>
		public double z;

		public CMRotationRate (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString ()
		{
			return String.Format ("rotationRate=({0},{1},{2}", x, y, z);
		}
	}

	// untyped enum -> CMDeviceMotion.h
	public enum CMMagneticFieldCalibrationAccuracy {
		/// <summary>Magnetic calibration has not occurred.</summary>
		Uncalibrated = -1,
		/// <summary>The magnetic calibration was marginal.</summary>
		Low,
		/// <summary>The magnetic calibration was of medium quality.</summary>
		Medium,
		/// <summary>The magnetic calibration was of high quality.</summary>
		High,
	}
}
