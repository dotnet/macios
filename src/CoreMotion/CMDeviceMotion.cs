//
// CoreMotion/CMDeviceMotion.cs
//
// Copyright (C) 2011-2014 Xamarin Inc

#nullable enable

using System;
using System.Runtime.InteropServices;

namespace CoreMotion {

	// CMDeviceMotion.h
#if NET
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct CMCalibratedMagneticField {
		/// <summary>The magnetic field.</summary>
		///         <remarks>To be added.</remarks>
		public CMMagneticField Field;
		/// <summary>The accuracy of the calibration.</summary>
		///         <remarks>To be added.</remarks>
		public CMMagneticFieldCalibrationAccuracy Accuracy;

		public override string ToString ()
		{
			return String.Format ("({0},{1})", Field, Accuracy);
		}
	}
}
