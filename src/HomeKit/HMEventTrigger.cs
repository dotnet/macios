#nullable enable

using System;
using ObjCRuntime;
using Foundation;

namespace HomeKit {

	partial class HMEventTrigger {

#if NET
		/// <param name="significantEvent">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <summary>Creates a predicate that causes a trigger to evaluate <paramref name="offset" /> before the specified significant event.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos11.0", "Use 'CreatePredicateForEvaluatingTriggerOccurringBeforeSignificantEvent (HMSignificantTimeEvent)' instead.")]
		[ObsoletedOSPlatform ("ios11.0", "Use 'CreatePredicateForEvaluatingTriggerOccurringBeforeSignificantEvent (HMSignificantTimeEvent)' instead.")]
#else
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CreatePredicateForEvaluatingTriggerOccurringBeforeSignificantEvent (HMSignificantTimeEvent)' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CreatePredicateForEvaluatingTriggerOccurringBeforeSignificantEvent (HMSignificantTimeEvent)' instead.")]
#endif
		static public NSPredicate CreatePredicateForEvaluatingTriggerOccurringBeforeSignificantEvent (HMSignificantEvent significantEvent, NSDateComponents offset)
		{
			var constant = significantEvent.GetConstant ();
			if (constant is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (significantEvent));
			return CreatePredicateForEvaluatingTriggerOccurringBeforeSignificantEvent (constant, offset);
		}

#if NET
		/// <param name="significantEvent">To be added.</param>
		///         <param name="offset">To be added.</param>
		///         <summary>Factory method to create an <see cref="Foundation.NSPredicate" /> that evaluates to <see langword="true" /> if the <paramref name="significantEvent" /> occurred.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("tvos11.0", "Use 'CreatePredicateForEvaluatingTriggerOccurringAfterSignificantEvent (HMSignificantTimeEvent)' instead.")]
		[ObsoletedOSPlatform ("ios11.0", "Use 'CreatePredicateForEvaluatingTriggerOccurringAfterSignificantEvent (HMSignificantTimeEvent)' instead.")]
#else
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CreatePredicateForEvaluatingTriggerOccurringAfterSignificantEvent (HMSignificantTimeEvent)' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'CreatePredicateForEvaluatingTriggerOccurringAfterSignificantEvent (HMSignificantTimeEvent)' instead.")]
#endif
		static public NSPredicate CreatePredicateForEvaluatingTriggerOccurringAfterSignificantEvent (HMSignificantEvent significantEvent, NSDateComponents offset)
		{
			var constant = significantEvent.GetConstant ();
			if (constant is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (significantEvent));
			return CreatePredicateForEvaluatingTriggerOccurringAfterSignificantEvent (constant, offset);
		}
	}
}
