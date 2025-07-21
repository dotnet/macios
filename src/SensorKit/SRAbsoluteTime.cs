using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

#nullable enable

namespace SensorKit {
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("macos")]
	public static class SRAbsoluteTime {

		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeGetCurrent")]
		public static extern /* SRAbsoluteTime */ double GetCurrent ();

		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeFromCFAbsoluteTime")]
		public static extern /* SRAbsoluteTime */ double FromCFAbsoluteTime (/* CFAbsoluteTime */ double cfAbsoluteTime);

		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeToCFAbsoluteTime")]
		public static extern /* CFAbsoluteTime */ double ToCFAbsoluteTime (double srAbsoluteTime);

		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeFromContinuousTime")]
		public static extern /* SRAbsoluteTime */ double FromContinuousTime (ulong continuousTime);
	}
}
