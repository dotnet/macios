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
	/// <summary>A 3D vector containing acceleration values.</summary>
	///     <remarks>To be added.</remarks>
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

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <param name="z">To be added.</param>
		///         <summary>Creates a new <see cref="CoreMotion.CMAcceleration" /> object, along the specified axes, with values in Gs.</summary>
		///         <remarks>To be added.</remarks>
		public CMAcceleration (double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>A string, of the form $"a=({x},{y},{z})".</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("a=({0},{1},{2})", X, Y, Z);
		}
	}

	// CMAttitude.h
#if NET
	/// <include file="../../docs/api/CoreMotion/CMRotationMatrix.xml" path="/Documentation/Docs[@DocId='T:CoreMotion.CMRotationMatrix']/*" />
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
	/// <summary>Represents a Quaternion, used as one of the possible CMAttitude representations.</summary>
	///     <remarks>
	///       <para>Quaternions can be used to specify a non-ambiguous rotation. They avoid the issue of gymbal lock and are simpler to compose.</para>
	///     </remarks>
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

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <param name="z">To be added.</param>
		///         <param name="w">To be added.</param>
		///         <summary>Constructs a new <see cref="CoreMotion.CMQuaternion" /> with the specified components.</summary>
		///         <remarks>To be added.</remarks>
		public CMQuaternion (double x, double y, double z, double w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		/// <summary>In the form $"quaternion({x},{y},{z},{w}").</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("quaternion=({0},{1},{2},{3})", x, y, z, w);
		}
	}

	// CMGyro.h
#if NET
	/// <summary>3D rotation rate.</summary>
	///     <remarks>To be added.</remarks>
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

		/// <param name="x">To be added.</param>
		///         <param name="y">To be added.</param>
		///         <param name="z">To be added.</param>
		///         <summary>Creates a new <see cref="CoreMotion.CMRotationRate" /> that rotates around the various axes at the specified rate, in radians per second.</summary>
		///         <remarks>To be added.</remarks>
		public CMRotationRate (double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		/// <summary>A string of the form $"rotationRate=({x},{y},{z}"	string".</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return String.Format ("rotationRate=({0},{1},{2}", x, y, z);
		}
	}

	// untyped enum -> CMDeviceMotion.h
	/// <summary>An enumeration whose values specify the quality of the magnetometer calibration.</summary>
	///     <remarks>To be added.</remarks>
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
