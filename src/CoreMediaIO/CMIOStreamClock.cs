// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using CoreMedia;

namespace CoreMediaIO {
	/// <summary>Creates and drives device clocks for Core Media I/O streams.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class CMIOStreamClock {
		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOStreamClockCreate (IntPtr allocator, IntPtr clockName, IntPtr sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing, out IntPtr clock);

		/// <summary>Creates a device clock, returning an OSStatus error code or zero on success.</summary>
		/// <param name="clockName">The clock's name.</param>
		/// <param name="sourceIdentifier">An opaque identity token. Use the same token for streams driven by the same hardware clock.</param>
		/// <param name="getTimeCallMinimumInterval">The minimum interval between time queries before interpolation is used.</param>
		/// <param name="numberOfEventsForRateSmoothing">The positive number of timing events used for rate smoothing.</param>
		/// <param name="numberOfAveragesForRateSmoothing">The number of averages, or zero to use the default smoothing algorithm.</param>
		/// <param name="clock">The created clock, or null on failure. Invalidate it before disposing it when timing events stop.</param>
		public static int Create (string clockName, IntPtr sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing, out CMClock? clock)
		{
			ArgumentNullException.ThrowIfNull (clockName);
			using var name = new NSString (clockName);
			var status = CMIOStreamClockCreate (IntPtr.Zero, name.Handle, sourceIdentifier, getTimeCallMinimumInterval, numberOfEventsForRateSmoothing, numberOfAveragesForRateSmoothing, out var handle);
			clock = status == 0 ? new CMClock (handle, true) : null;
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOStreamClockPostTimingEvent (CMTime eventTime, ulong hostTime, byte resynchronize, IntPtr clock);

		/// <summary>Posts a stream-time and host-time pair, returning an OSStatus error code or zero on success.</summary>
		/// <remarks>Only pass clocks created by <see cref="Create" />. Host time uses the host clock's native units, not nanoseconds.</remarks>
		public static int PostTimingEvent (CMClock clock, CMTime eventTime, ulong hostTime, bool resynchronize)
		{
			var status = CMIOStreamClockPostTimingEvent (eventTime, hostTime, resynchronize.AsByte (), clock.GetNonNullHandle (nameof (clock)));
			GC.KeepAlive (clock);
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern int CMIOStreamClockInvalidate (IntPtr clock);

		/// <summary>Invalidates a clock created by <see cref="Create" />, returning an OSStatus error code or zero on success.</summary>
		/// <remarks>Call this when the stream stops providing timing events, before disposing the clock. Retained clients then observe invalid times.</remarks>
		public static int Invalidate (CMClock clock)
		{
			var status = CMIOStreamClockInvalidate (clock.GetNonNullHandle (nameof (clock)));
			GC.KeepAlive (clock);
			return status;
		}

		[DllImport (Constants.CoreMediaIOLibrary)]
		static extern CMTime CMIOStreamClockConvertHostTimeToDeviceTime (ulong hostTime, IntPtr clock);

		/// <summary>Converts a host-clock time to the time of a clock created by <see cref="Create" />.</summary>
		public static CMTime ConvertHostTimeToDeviceTime (CMClock clock, ulong hostTime)
		{
			var time = CMIOStreamClockConvertHostTimeToDeviceTime (hostTime, clock.GetNonNullHandle (nameof (clock)));
			GC.KeepAlive (clock);
			return time;
		}
	}
}
