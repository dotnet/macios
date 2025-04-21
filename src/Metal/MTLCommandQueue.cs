using System;

using ObjCRuntime;

#nullable enable

namespace Metal {

	public partial interface IMTLCommandQueue {

		/// <summary>Marks the specified residency sets as part of the current command buffer execution.</summary>
		/// <param name="residencySets">The residency sets to mark.</param>
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public void AddResidencySets (params IMTLResidencySet [] residencySets)
		{
			NativeObjectExtensions.CallWithPointerToFirstElementAndCount (residencySets, nameof (residencySets), AddResidencySets);
		}

		/// <summary>Removes the specified residency sets from the current command buffer execution.</summary>
		/// <param name="residencySets">The residency sets to mark.</param>
		[SupportedOSPlatform ("macos15.0")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("maccatalyst18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public void RemoveResidencySets (params IMTLResidencySet [] residencySets)
		{
			NativeObjectExtensions.CallWithPointerToFirstElementAndCount (residencySets, nameof (residencySets), RemoveResidencySets);
		}
	}
}
