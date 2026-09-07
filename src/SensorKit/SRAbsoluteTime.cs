
#nullable enable

namespace SensorKit {
	/// <summary>Provides methods for converting between SensorKit absolute time and other time representations.</summary>
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("tvos")]
	[UnsupportedOSPlatform ("macos")]
	public static class SRAbsoluteTime {

		/// <summary>Returns the current SensorKit absolute time.</summary>
		/// <returns>The current absolute time value.</returns>
		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeGetCurrent")]
		public static extern /* SRAbsoluteTime */ double GetCurrent ();

		/// <summary>Converts a Core Foundation absolute time value to a SensorKit absolute time value.</summary>
		/// <param name="cfAbsoluteTime">The Core Foundation absolute time to convert.</param>
		/// <returns>The equivalent SensorKit absolute time value.</returns>
		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeFromCFAbsoluteTime")]
		public static extern /* SRAbsoluteTime */ double FromCFAbsoluteTime (/* CFAbsoluteTime */ double cfAbsoluteTime);

		/// <summary>Converts a SensorKit absolute time value to a Core Foundation absolute time value.</summary>
		/// <param name="srAbsoluteTime">The SensorKit absolute time to convert.</param>
		/// <returns>The equivalent Core Foundation absolute time value.</returns>
		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeToCFAbsoluteTime")]
		public static extern /* CFAbsoluteTime */ double ToCFAbsoluteTime (double srAbsoluteTime);

		/// <summary>Converts a continuous time value to a SensorKit absolute time value.</summary>
		/// <param name="continuousTime">The continuous time value to convert.</param>
		/// <returns>The equivalent SensorKit absolute time value.</returns>
		[DllImport (Constants.SensorKitLibrary, EntryPoint = "SRAbsoluteTimeFromContinuousTime")]
		public static extern /* SRAbsoluteTime */ double FromContinuousTime (ulong continuousTime);
	}
}
