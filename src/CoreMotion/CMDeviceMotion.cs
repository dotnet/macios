//
// CoreMotion/CMDeviceMotion.cs
//
// Copyright (C) 2011-2014 Xamarin Inc

#nullable enable

namespace CoreMotion {

	// CMDeviceMotion.h
	/// <summary>Encapsulates the accuracy and field strength of the magnetometer after calibration.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CMCalibratedMagneticField {
		/// <summary>The magnetic field.</summary>
		public CMMagneticField Field;
		/// <summary>The accuracy of the calibration.</summary>
		public CMMagneticFieldCalibrationAccuracy Accuracy;

		/// <summary>A string describing the magnetic field.</summary>
		public override string ToString ()
		{
			return String.Format ("({0},{1})", Field, Accuracy);
		}
	}
}
