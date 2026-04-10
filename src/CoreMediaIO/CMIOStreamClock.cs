#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreFoundation;
using CoreMedia;
using ObjCRuntime;

namespace CoreMediaIO {

#if !COREBUILD
	/// <summary>Represents a CoreMediaIO stream clock used to synchronize media timing.</summary>
	/// <remarks>
	/// <para>A stream clock is driven by timing events posted via <see cref="PostTimingEvent" />.
	/// When the clock is no longer needed, dispose it to invalidate and release the native resource.</para>
	/// </remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public class CMIOStreamClock : NativeObject {

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern IntPtr CFRetain (IntPtr obj);

		[DllImport (Constants.CoreFoundationLibrary)]
		static extern void CFRelease (IntPtr obj);

		[Preserve (Conditional = true)]
		internal CMIOStreamClock (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <inheritdoc />
		protected internal override void Retain ()
		{
			if (Handle != IntPtr.Zero)
				CFRetain (Handle);
		}

		/// <inheritdoc />
		protected internal override void Release ()
		{
			if (Handle != IntPtr.Zero) {
				CMIOInterop.CMIOStreamClockInvalidate (Handle);
				CFRelease (Handle);
			}
		}

		/// <summary>Creates a new clock object that can be used by a CoreMediaIO stream.</summary>
		/// <param name="clockName">The name of the clock.</param>
		/// <param name="sourceIdentifier">An opaque reference to the entity driving the clock, used to determine if two clocks share the same hardware source.</param>
		/// <param name="getTimeCallMinimumInterval">The minimum interval between time queries before interpolation is used.</param>
		/// <param name="numberOfEventsForRateSmoothing">The number of events to use for rate smoothing; must be greater than 0.</param>
		/// <param name="numberOfAveragesForRateSmoothing">The number of averages for rate smoothing; 0 uses the default algorithm.</param>
		/// <param name="status">On return, the status code from the native call; 0 indicates success.</param>
		/// <param name="clock">On success, receives the created clock object.</param>
		/// <returns><see langword="true" /> if the clock was created successfully; otherwise, <see langword="false" />.</returns>
		public static unsafe bool TryCreate (
			string clockName,
			IntPtr sourceIdentifier,
			CMTime getTimeCallMinimumInterval,
			uint numberOfEventsForRateSmoothing,
			uint numberOfAveragesForRateSmoothing,
			out int status,
			[NotNullWhen (true)] out CMIOStreamClock? clock)
		{
			if (clockName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockName));

			clock = null;
			using var nameHandle = new TransientCFString (clockName);
			IntPtr clockOut;
			status = CMIOInterop.CMIOStreamClockCreate (
				IntPtr.Zero,
				nameHandle,
				sourceIdentifier,
				getTimeCallMinimumInterval,
				numberOfEventsForRateSmoothing,
				numberOfAveragesForRateSmoothing,
				&clockOut);

			if (status != 0 || clockOut == IntPtr.Zero)
				return false;

			clock = new CMIOStreamClock (clockOut, owns: true);
			return true;
		}

		/// <summary>Posts a timing event to drive this clock.</summary>
		/// <param name="eventTime">The time when the event occurred on the stream's timeline.</param>
		/// <param name="hostTime">The host time at which the event occurred.</param>
		/// <param name="resynchronize">If <see langword="true" />, indicates a new anchor point for time measurement.</param>
		/// <returns>An <see cref="int" /> status code; 0 indicates success.</returns>
		public int PostTimingEvent (CMTime eventTime, ulong hostTime, bool resynchronize)
		{
			return CMIOInterop.CMIOStreamClockPostTimingEvent (eventTime, hostTime, resynchronize.AsByte (), GetCheckedHandle ());
		}

		/// <summary>Converts a host time value to the equivalent time on this clock.</summary>
		/// <param name="hostTime">The host time value to convert.</param>
		/// <returns>The time on this clock that is equivalent to the given host time.</returns>
		public CMTime ConvertHostTimeToDeviceTime (ulong hostTime)
		{
			return CMIOInterop.CMIOStreamClockConvertHostTimeToDeviceTime (hostTime, GetCheckedHandle ());
		}
	}
#endif // !COREBUILD
}
