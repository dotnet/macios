#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreFoundation;
using CoreMedia;
using ObjCRuntime;

namespace CoreMediaIO {

#if !COREBUILD
	/// <summary>Provides managed wrappers for CoreMediaIO stream clock C functions.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static unsafe class CMIOStreamClock {

		/// <summary>Creates a clock object that can be used by a CoreMediaIO stream.</summary>
		/// <param name="clockName">The name of the clock.</param>
		/// <param name="sourceIdentifier">An opaque reference to the entity driving the clock.</param>
		/// <param name="getTimeCallMinimumInterval">The minimum interval between time queries before interpolation is used.</param>
		/// <param name="numberOfEventsForRateSmoothing">The number of events to use for rate smoothing; must be greater than 0.</param>
		/// <param name="numberOfAveragesForRateSmoothing">The number of averages for rate smoothing; 0 uses the default algorithm.</param>
		/// <param name="clock">On success, receives the created clock handle. Call <see cref="Invalidate" /> followed by <c>CFRelease</c> when done.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int Create (
			string clockName,
			IntPtr sourceIdentifier,
			CMTime getTimeCallMinimumInterval,
			uint numberOfEventsForRateSmoothing,
			uint numberOfAveragesForRateSmoothing,
			out IntPtr clock)
		{
			if (clockName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockName));

			var nameHandle = CFString.CreateNative (clockName);
			try {
				IntPtr clockOut;
				int status = CMIOInterop.CMIOStreamClockCreate (
					IntPtr.Zero,
					nameHandle,
					sourceIdentifier,
					getTimeCallMinimumInterval,
					numberOfEventsForRateSmoothing,
					numberOfAveragesForRateSmoothing,
					&clockOut);
				clock = clockOut;
				return status;
			} finally {
				CFString.ReleaseNative (nameHandle);
			}
		}

		/// <summary>Posts a timing event to drive a clock created by <see cref="Create" />.</summary>
		/// <param name="eventTime">The time when the event occurred on the stream's timeline.</param>
		/// <param name="hostTime">The host time at which the event occurred.</param>
		/// <param name="resynchronize">If <see langword="true" />, indicates a new anchor point for time measurement.</param>
		/// <param name="clock">The clock handle returned by <see cref="Create" />.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int PostTimingEvent (CMTime eventTime, ulong hostTime, bool resynchronize, IntPtr clock)
		{
			return CMIOInterop.CMIOStreamClockPostTimingEvent (eventTime, hostTime, resynchronize ? (byte) 1 : (byte) 0, clock);
		}

		/// <summary>Invalidates a clock, indicating it will no longer receive timing events.</summary>
		/// <param name="clock">The clock handle returned by <see cref="Create" />.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public static int Invalidate (IntPtr clock)
		{
			return CMIOInterop.CMIOStreamClockInvalidate (clock);
		}

		/// <summary>Converts a host time value to the equivalent time on the device's clock.</summary>
		/// <param name="hostTime">The host time value to convert.</param>
		/// <param name="clock">The clock handle returned by <see cref="Create" />.</param>
		/// <returns>The time on the clock that is equivalent to the given host time.</returns>
		public static CMTime ConvertHostTimeToDeviceTime (ulong hostTime, IntPtr clock)
		{
			return CMIOInterop.CMIOStreamClockConvertHostTimeToDeviceTime (hostTime, clock);
		}
	}
#endif // !COREBUILD
}
