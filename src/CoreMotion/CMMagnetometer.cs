//
// CMMagnetometer.cs: Support classes
//
// Copyright 2011-2014, Xamarin Inc.
//
// Authors:
//   Miguel de Icaza 
//

#nullable enable

using System.Runtime.InteropServices;

namespace CoreMotion {

	// CMMagnetometer.h
	/// <summary>Represents the 3-axis magnetometer data in microteslas.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMMagneticField {
		/// <summary>X component of the magnetic field, measured in microteslas.</summary>
		///         <remarks>To be added.</remarks>
		public double X;
		/// <summary>Y component of the magnetic field, measured in microteslas.</summary>
		///         <remarks>To be added.</remarks>
		public double Y;
		/// <summary>Z component of the magnetic field, measured in microteslas.</summary>
		///         <remarks>To be added.</remarks>
		public double Z;

		/// <summary>String representation of the magnetometer reading.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public override string ToString ()
		{
			return string.Format ("({0},{1},{2})", X, Y, Z);
		}
	}
}
