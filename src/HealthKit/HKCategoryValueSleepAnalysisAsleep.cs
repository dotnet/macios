// HKCategoryValueSleepAnalysisAsleep

//
// HKCategoryValueSleepAnalysisAsleep.cs:
//
// Authors:
//  TJ Lambert (TJ.Lambert@microsoft.com
//
// Copyright 2022 Microsoft Corp.
//

#nullable enable

#if !TVOS

using System;
using Foundation;
using ObjCRuntime;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace HealthKit {
	public partial class HKCategoryValueSleepAnalysisAsleep {
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.HealthKitLibrary)]
		static extern NativeHandle HKCategoryValueSleepAnalysisAsleepValues ();

		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("macos13.0")]
		[SupportedOSPlatform ("maccatalyst16.0")]
		[UnsupportedOSPlatform ("tvos")]
		public static HashSet<HKCategoryValueSleepAnalysis> GetAsleepValues ()
		{
			using var values = Runtime.GetNSObject<NSSet<NSNumber>> (HKCategoryValueSleepAnalysisAsleepValues ())!;
			var hashSet = new HashSet<HKCategoryValueSleepAnalysis> ();
			foreach (NSNumber value in values) {
				hashSet.Add ((HKCategoryValueSleepAnalysis) (int) value);
			}
			return hashSet;
		}
	}
}

#endif // !TVOS
