//
// CMMagnetometer.cs: Support classes
//
// Copyright 2011-2014, Xamarin Inc.
//
// Authors:
//   Miguel de Icaza 
//

#nullable enable

namespace CoreMotion {

	// CMMagnetometer.h
	/// <summary>Represents the 3-axis magnetometer data in microteslas.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMMagneticField {
		/// <summary>X component of the magnetic field, measured in microteslas.</summary>
		public double X;
		/// <summary>Y component of the magnetic field, measured in microteslas.</summary>
		public double Y;
		/// <summary>Z component of the magnetic field, measured in microteslas.</summary>
		public double Z;

		/// <summary>String representation of the magnetometer reading.</summary>
		public override string ToString ()
		{
			return string.Format ("({0},{1},{2})", X, Y, Z);
		}
	}
}
