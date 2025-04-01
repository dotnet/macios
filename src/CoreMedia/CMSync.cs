// 
// CMSync.cs: Implements the managed CMSync infrastructure
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012-2014 Xamarin Inc
//

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

#nullable enable

namespace CoreMedia {

	// CMSync.h
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMClock : CMClockOrTimebase {
		[Preserve (Conditional = true)]
		internal CMClock (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}
#if !COREBUILD

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMClockRef */ IntPtr CMClockGetHostTimeClock ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static CMClock HostTimeClock {
			get {
				return new CMClock (CMClockGetHostTimeClock (), false);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMClockGetTime (/* CMClockRef */ IntPtr clock);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime CurrentTime {
			get {
				return CMClockGetTime (Handle);
			}
		}

#if !MONOMAC
		// CMAudioClock.h
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMClockError CMAudioClockCreate (/* CFAllocatorRef */ IntPtr allocator, /* CMClockRef* */ IntPtr* clockOut);

		/// <param name="clockError">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMClock? CreateAudioClock (out CMClockError clockError)
		{
			IntPtr ptr;
			unsafe {
				clockError = CMAudioClockCreate (IntPtr.Zero, &ptr);
			}
			return clockError == CMClockError.None ? new CMClock (ptr, true) : null;
		}
#endif

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMClockError CMClockGetAnchorTime (/* CMClockRef */ IntPtr clock, CMTime* outClockTime, CMTime* outReferenceClockTime);

		/// <param name="clockTime">To be added.</param>
		///         <param name="referenceClockTime">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CMClockError GetAnchorTime (out CMTime clockTime, out CMTime referenceClockTime)
		{
			clockTime = default;
			referenceClockTime = default;
			unsafe {
				return CMClockGetAnchorTime (Handle, (CMTime*) Unsafe.AsPointer<CMTime> (ref clockTime), (CMTime*) Unsafe.AsPointer<CMTime> (ref referenceClockTime));
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Boolean */ byte CMClockMightDrift (/* CMClockRef */ IntPtr clock, /* CMClockRef */ IntPtr otherClock);

		/// <param name="otherClock">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool MightDrift (CMClock otherClock)
		{
			if (otherClock is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (otherClock));

			bool result = CMClockMightDrift (Handle, otherClock.Handle) != 0;
			GC.KeepAlive (otherClock);
			return result;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static void CMClockInvalidate (/* CMClockRef */ IntPtr clock);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Invalidate ()
		{
			CMClockInvalidate (Handle);
		}

		/// <param name="hostTime">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.CoreMediaLibrary, EntryPoint = "CMClockConvertHostTimeToSystemUnits")]
		public extern static /* uint64_t */ ulong ConvertHostTimeToSystemUnits (CMTime hostTime);

		/// <param name="hostTime">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[DllImport (Constants.CoreMediaLibrary, EntryPoint = "CMClockMakeHostTimeFromSystemUnits")]
		public extern static CMTime CreateHostTimeFromSystemUnits (/* uint64_t */ ulong hostTime);
#endif // !COREBUILD
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMTimebase : CMClockOrTimebase {
		[Preserve (Conditional = true)]
		private CMTimebase (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}
#if !COREBUILD

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMTimebaseError CMTimebaseCreateWithMasterClock (/* CFAllocatorRef */ IntPtr allocator, /* CMClockRef */ IntPtr masterClock, /* CMTimebaseRef* */ IntPtr* timebaseOut);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use 'CMTimebaseCreateWithSourceClock' instead.")]
		static IntPtr Create (CMClock masterClock)
		{
			if (masterClock is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (masterClock));

			CMTimebaseError error;
			IntPtr handle;
			unsafe {
				error = CMTimebaseCreateWithMasterClock (IntPtr.Zero, masterClock.Handle, &handle);
				GC.KeepAlive (masterClock);
			}
			if (error != CMTimebaseError.None)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (error.ToString ());
			return handle;
		}

		/// <param name="masterClock">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use the (CFAllocator, CMClock) overload instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use the (CFAllocator, CMClock) overload instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use the (CFAllocator, CMClock) overload instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use the (CFAllocator, CMClock) overload instead.")]
		public CMTimebase (CMClock masterClock)
			: base (Create (masterClock), true)
		{
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios8.0", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMTimebaseError CMTimebaseCreateWithMasterTimebase (/* CFAllocatorRef */ IntPtr allocator, /* CMTimebaseRef */ IntPtr masterTimebase, /* CMTimebaseRef* */ IntPtr* timebaseOut);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios8.0", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCreateWithSourceTimebase' instead.")]
		static IntPtr Create (CMTimebase masterTimebase)
		{
			if (masterTimebase is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (masterTimebase));

			CMTimebaseError error;
			IntPtr handle;
			unsafe {
				error = CMTimebaseCreateWithMasterTimebase (IntPtr.Zero, masterTimebase.Handle, &handle);
				GC.KeepAlive (masterTimebase);
			}
			if (error != CMTimebaseError.None)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (error.ToString ());
			return handle;
		}

		/// <param name="masterTimebase">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use the (CFAllocator, CMTimebase) overload instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use the (CFAllocator, CMTimebase) overload instead.")]
		[ObsoletedOSPlatform ("ios8.0", message: "Use the (CFAllocator, CMTimebase) overload instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use the (CFAllocator, CMTimebase) overload instead.")]
		public CMTimebase (CMTimebase masterTimebase)
			: base (Create (masterTimebase), true)
		{
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern CMTimebaseError CMTimebaseCreateWithSourceClock (/* [NullAllowed] CFAllocatorRef */ IntPtr allocator, /* CMClock */ IntPtr sourceClock, /* CMTimebase */ IntPtr* timebaseOut);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static IntPtr Create (CFAllocator? allocator, CMClock sourceClock)
		{
			if (sourceClock is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sourceClock));

			CMTimebaseError error;
			IntPtr handle;
			unsafe {
				error = CMTimebaseCreateWithSourceClock (allocator.GetHandle (), sourceClock.Handle, &handle);
				GC.KeepAlive (allocator);
				GC.KeepAlive (sourceClock);
			}
			if (error != CMTimebaseError.None)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (error.ToString ());
			return handle;
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public CMTimebase (CFAllocator? allocator, CMClock sourceClock)
			: base (Create (allocator, sourceClock), true)
		{
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		unsafe static extern CMTimebaseError CMTimebaseCreateWithSourceTimebase (/* [NullAllowed] CFAllocatorRef */ IntPtr allocator, /* CMTimebase */ IntPtr sourceTimebase, /* CMTimebase */ IntPtr* timebaseOut);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		static IntPtr Create (CFAllocator? allocator, CMTimebase sourceTimebase)
		{
			if (sourceTimebase is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (sourceTimebase));

			CMTimebaseError error;
			IntPtr handle;
			unsafe {
				error = CMTimebaseCreateWithSourceTimebase (allocator.GetHandle (), sourceTimebase.Handle, &handle);
				GC.KeepAlive (allocator);
				GC.KeepAlive (sourceTimebase);
			}
			if (error != CMTimebaseError.None)
				ObjCRuntime.ThrowHelper.ThrowArgumentException (error.ToString ());
			return handle;
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public CMTimebase (CFAllocator? allocator, CMTimebase sourceTimebase)
			: base (Create (allocator, sourceTimebase), true)
		{
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Float64 */ double CMTimebaseGetEffectiveRate (/* CMTimebaseRef */ IntPtr timebase);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double EffectiveRate {
			get {
				return CMTimebaseGetEffectiveRate (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Float64 */ double CMTimebaseGetRate (/* CMTimebaseRef */ IntPtr timebase);

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseSetRate (/* CMTimebaseRef */ IntPtr timebase, /* Float64 */ double rate);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Rate {
			get {
				return CMTimebaseGetRate (Handle);
			}
			set {
				var error = CMTimebaseSetRate (Handle, value);
				if (error != CMTimebaseError.None)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (error.ToString ());
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimebaseGetTime (/* CMTimebaseRef */ IntPtr timebase);

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseSetTime (/* CMTimebaseRef */ IntPtr timebase, CMTime time);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public new CMTime Time {
			get {
				return CMTimebaseGetTime (Handle);
			}
			set {
				var error = CMTimebaseSetTime (Handle, value);
				if (error != CMTimebaseError.None)
					ObjCRuntime.ThrowHelper.ThrowArgumentException (error.ToString ());
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMTimebaseRef */ IntPtr CMTimebaseGetMasterTimebase (/* CMTimebaseRef */ IntPtr timebase);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.10", "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.0", "Use 'SourceTimebase' instead.")]
		public CMTimebase? GetMasterTimebase ()
		{
			var ptr = CMTimebaseGetMasterTimebase (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMTimebase (ptr, false);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCopySourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCopySourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseCopySourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use 'CMTimebaseCopySourceClock' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMClockRef */ IntPtr CMTimebaseGetMasterClock (/* CMTimebaseRef */ IntPtr timebase);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use 'SourceClock' instead.")]
		public CMClock? GetMasterClock ()
		{
			var ptr = CMTimebaseGetMasterClock (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMClock (ptr, false);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCopySource' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCopySource' instead.")]
		[ObsoletedOSPlatform ("macos10.11", message: "Use 'CMTimebaseCopySource' instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use 'CMTimebaseCopySource' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMClockOrTimebaseRef */ IntPtr CMTimebaseGetMaster (/* CMTimebaseRef */ IntPtr timebase);

		/// <summary>Developers should not use this deprecated method. Developers should use 'CopyMaster' instead.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'CopyMaster' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'CopyMaster' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'CopyMaster' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.0", "Use 'CopyMaster' instead.")]
		public CMClockOrTimebase? GetMaster ()
		{
			var ptr = CMTimebaseGetMaster (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMClockOrTimebase (ptr, false);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.11", message: "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", message: "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* CMClockRef */ IntPtr CMTimebaseGetUltimateMasterClock (/* CMTimebaseRef */ IntPtr timebase);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'UltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'UltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'UltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("maccatalyst13.0", "Use 'UltimateSourceClock' instead.")]
		public CMClock? GetUltimateMasterClock ()
		{
			var ptr = CMTimebaseGetUltimateMasterClock (Handle);
			if (ptr == IntPtr.Zero)
				return null;

			return new CMClock (ptr, false);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMTimebaseGetTimeWithTimeScale (/* CMTimebaseRef */ IntPtr timebase, CMTimeScale timescale, CMTimeRoundingMethod method);

		public CMTime GetTime (CMTimeScale timeScale, CMTimeRoundingMethod roundingMethod)
		{
			return CMTimebaseGetTimeWithTimeScale (Handle, timeScale, roundingMethod);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseSetAnchorTime (/* CMTimebaseRef */ IntPtr timebase, CMTime timebaseTime, CMTime immediateMasterTime);

		public CMTimebaseError SetAnchorTime (CMTime timebaseTime, CMTime immediateMasterTime)
		{
			return CMTimebaseSetAnchorTime (Handle, timebaseTime, immediateMasterTime);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMTimebaseError CMTimebaseGetTimeAndRate (/* CMTimebaseRef */ IntPtr timebase, CMTime* time, /* Float64* */ double* rate);

		public CMTimebaseError GetTimeAndRate (out CMTime time, out double rate)
		{
			time = default;
			rate = default;
			unsafe {
				return CMTimebaseGetTimeAndRate (Handle, (CMTime*) Unsafe.AsPointer<CMTime> (ref time), (double*) Unsafe.AsPointer<double> (ref rate));
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */  CMTimebaseError CMTimebaseSetRateAndAnchorTime (/* CMTimebaseRef */ IntPtr timebase, /* Float64 */ double rate, CMTime timebaseTime, CMTime immediateMasterTime);

		public CMTimebaseError SetRateAndAnchorTime (double rate, CMTime timebaseTime, CMTime immediateMasterTime)
		{
			return CMTimebaseSetRateAndAnchorTime (Handle, rate, timebaseTime, immediateMasterTime);
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseNotificationBarrier (/* CMTimebaseRef */ IntPtr timebase);

		public CMTimebaseError NotificationBarrier ()
		{
			return CMTimebaseNotificationBarrier (Handle);
		}

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public const double VeryLongTimeInterval = 256.0 * 365.0 * 24.0 * 60.0 * 60.0;

#if !COREBUILD
		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseAddTimer (/* CMTimebaseRef */ IntPtr timebase, /* CFRunLoopTimerRef */ IntPtr timer, /* CFRunLoopRef */ IntPtr runloop);

		public CMTimebaseError AddTimer (NSTimer timer, NSRunLoop runloop)
		{
			if (timer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (timer));
			if (runloop is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (runloop));

			// NSRunloop and CFRunloop[Ref] are NOT toll free bridged types
			using (var cf = runloop.GetCFRunLoop ()) {
				CMTimebaseError result = CMTimebaseAddTimer (Handle, timer.Handle, cf.Handle);
				GC.KeepAlive (timer);
				return result;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseRemoveTimer (/* CMTimebaseRef */ IntPtr timebase, /* CFRunLoopTimerRef */ IntPtr timer);

		public CMTimebaseError RemoveTimer (NSTimer timer)
		{
			if (timer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (timer));

			CMTimebaseError result = CMTimebaseRemoveTimer (Handle, timer.Handle);
			GC.KeepAlive (timer);
			return result;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseSetTimerNextFireTime (/* CMTimebaseRef */ IntPtr timebase, /* CFRunLoopTimerRef */ IntPtr timer, CMTime fireTime, /* uint32_t */ uint flags);

		public CMTimebaseError SetTimerNextFireTime (NSTimer timer, CMTime fireTime)
		{
			if (timer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (timer));

			CMTimebaseError result = CMTimebaseSetTimerNextFireTime (Handle, timer.Handle, fireTime, 0);
			GC.KeepAlive (timer);
			return result;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* OSStatus */ CMTimebaseError CMTimebaseSetTimerToFireImmediately (/* CMTimebaseRef */ IntPtr timebase, /* CFRunLoopTimerRef */ IntPtr timer);

		public CMTimebaseError SetTimerToFireImmediately (NSTimer timer)
		{
			if (timer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (timer));

			CMTimebaseError result = CMTimebaseSetTimerToFireImmediately (Handle, timer.Handle);
			GC.KeepAlive (timer);
			return result;
		}

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseSetSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseSetSourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios8.0", message: "Use 'CMTimebaseSetSourceTimebase' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseSetMasterTimebase (/* CMTimebaseRef* */ IntPtr timebase, /* CMTimebaseRef* */ IntPtr newMasterTimebase);

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios8.0", message: "Use 'SourceTimebase' instead.")]
		public CMTimebaseError SetMasterTimebase (CMTimebase newMasterTimebase)
		{
			if (newMasterTimebase is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newMasterTimebase));

			var result = CMTimebaseSetMasterTimebase (Handle, newMasterTimebase.Handle);
			GC.KeepAlive (newMasterTimebase);
			return result;
		}

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'CMTimebaseSetSourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'CMTimebaseSetSourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'CMTimebaseSetSourceClock' instead.")]
		[ObsoletedOSPlatform ("ios8.0", message: "Use 'CMTimebaseSetSourceClock' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTimebaseError CMTimebaseSetMasterClock (/* CMTimebaseRef* */ IntPtr timebase, /* CMClockRef* */ IntPtr newMasterClock);

		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", message: "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", message: "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.10", message: "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("ios8.0", message: "Use 'SourceClock' instead.")]
		public CMTimebaseError SetMasterClock (CMClock newMasterClock)
		{
			if (newMasterClock is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newMasterClock));

			var result = CMTimebaseSetMasterClock (Handle, newMasterClock.Handle);
			GC.KeepAlive (newMasterClock);
			return result;
		}
#endif

#if !COREBUILD
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'SourceTimebase' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'SourceTimebase' instead.")]
		public CMTimebase? CopyMasterTimebase ()
		{
			var ptr = CMTimebaseCopyMasterTimebase (Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return new CMTimebase (ptr, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'SourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'SourceClock' instead.")]
		public CMClock? CopyMasterClock ()
		{
			var ptr = CMTimebaseCopyMasterClock (Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return new CMClock (ptr, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'CopySource' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'CopySource' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'CopySource' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'CopySource' instead.")]
		public CMClockOrTimebase? CopyMaster ()
		{
			var ptr = CMTimebaseCopyMaster (Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return new CMClockOrTimebase (ptr, true);
		}

		public CMClockOrTimebase? CopySource ()
		{
			var ptr = CMTimebaseCopySource (Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return new CMClockOrTimebase (ptr, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'UltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'UltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'UltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'UltimateSourceClock' instead.")]
		public CMClock? CopyUltimateMasterClock ()
		{
			var ptr = CMTimebaseCopyUltimateMasterClock (Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return new CMClock (ptr, true);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'CMTimebaseCopySourceTimebase' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern unsafe /* CMTimebaseRef */ IntPtr CMTimebaseCopyMasterTimebase (/* CMTimebaseRef */ IntPtr timebase);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst13.0", "Use 'CMTimebaseCopySourceClock' instead.")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'CMTimebaseCopySourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'CMTimebaseCopySourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'CMTimebaseCopySourceClock' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern unsafe /* CMClockRef */ IntPtr CMTimebaseCopyMasterClock (/* CMTimebaseRef */ IntPtr timebase);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'CMTimebaseCopySource' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'CMTimebaseCopySource' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'CMTimebaseCopySource' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'CMTimebaseCopySource' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern unsafe IntPtr /* void* */ CMTimebaseCopyMaster (/* CMTimebaseRef */ IntPtr timebase);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos9.0", "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("macos10.11", "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("ios9.0", "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'CMTimebaseCopyUltimateSourceClock' instead.")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern unsafe /* CMClockRef */ IntPtr CMTimebaseCopyUltimateMasterClock (/* CMTimebaseRef */ IntPtr timebase);
#endif

		[DllImport (Constants.CoreMediaLibrary)]
		static extern unsafe IntPtr /* CMClockOrTimebaseRef * */ CMTimebaseCopySource (/* CMTimebaseRef */ IntPtr timebase);

		//
		// Dispatch timers not supported
		//
		// CMTimebaseAddTimerDispatchSource
		// CMTimebaseRemoveTimerDispatchSource
		// CMTimebaseSetTimerDispatchSourceNextFireTime
		// CMTimebaseSetTimerDispatchSourceToFireImmediately
#endif // !COREBUILD
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMClockOrTimebase : NativeObject {
		[Preserve (Conditional = true)]
		internal CMClockOrTimebase (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

#if !COREBUILD
		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSyncGetTime (/* CMClockOrTimebaseRef */ IntPtr clockOrTimebase);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public CMTime Time {
			get {
				return CMSyncGetTime (Handle);
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Float64 */ double CMSyncGetRelativeRate (/* CMClockOrTimebaseRef */ IntPtr ofClockOrTimebase, /* CMClockOrTimebaseRef */ IntPtr relativeToClockOrTimebase);

		/// <param name="clockOrTimebaseA">To be added.</param>
		///         <param name="clockOrTimebaseB">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static double GetRelativeRate (CMClockOrTimebase clockOrTimebaseA, CMClockOrTimebase clockOrTimebaseB)
		{
			if (clockOrTimebaseA is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockOrTimebaseA));

			if (clockOrTimebaseB is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockOrTimebaseB));

			double result = CMSyncGetRelativeRate (clockOrTimebaseA.Handle, clockOrTimebaseB.Handle);
			GC.KeepAlive (clockOrTimebaseA);
			GC.KeepAlive (clockOrTimebaseB);
			return result;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		unsafe extern static /* OSStatus */ CMSyncError CMSyncGetRelativeRateAndAnchorTime (
			/* CMClockOrTimebaseRef */ IntPtr ofClockOrTimebase,
			/* CMClockOrTimebaseRef */ IntPtr relativeToClockOrTimebase,
			/* Float64* */ double* outRelativeRate,
			CMTime* outOfClockOrTimebaseAnchorTime,
			CMTime* outRelativeToClockOrTimebaseAnchorTime);

		/// <param name="clockOrTimebaseA">To be added.</param>
		///         <param name="clockOrTimebaseB">To be added.</param>
		///         <param name="relativeRate">To be added.</param>
		///         <param name="timeA">To be added.</param>
		///         <param name="timeB">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMSyncError GetRelativeRateAndAnchorTime (CMClockOrTimebase clockOrTimebaseA, CMClockOrTimebase clockOrTimebaseB, out double relativeRate, out CMTime timeA, out CMTime timeB)
		{
			if (clockOrTimebaseA is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockOrTimebaseA));

			if (clockOrTimebaseB is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockOrTimebaseB));

			relativeRate = default;
			timeA = default;
			timeB = default;
			unsafe {
				CMSyncError result = CMSyncGetRelativeRateAndAnchorTime (
					clockOrTimebaseA.Handle,
					clockOrTimebaseB.Handle,
					(double*) Unsafe.AsPointer<double> (ref relativeRate),
					(CMTime*) Unsafe.AsPointer<CMTime> (ref timeA),
					(CMTime*) Unsafe.AsPointer<CMTime> (ref timeB));
				GC.KeepAlive (clockOrTimebaseA);
				GC.KeepAlive (clockOrTimebaseB);
				return result;
			}
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static CMTime CMSyncConvertTime (CMTime time, /* CMClockOrTimebaseRef */ IntPtr fromClockOrTimebase, /* CMClockOrTimebaseRef */ IntPtr toClockOrTimebase);

		/// <param name="time">To be added.</param>
		///         <param name="from">To be added.</param>
		///         <param name="to">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static CMTime ConvertTime (CMTime time, CMClockOrTimebase from, CMClockOrTimebase to)
		{
			if (from is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (from));
			if (to is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (to));

			CMTime result = CMSyncConvertTime (time, from.Handle, to.Handle);
			GC.KeepAlive (from);
			GC.KeepAlive (to);
			return result;
		}

		[DllImport (Constants.CoreMediaLibrary)]
		extern static /* Boolean */ byte CMSyncMightDrift (/* CMClockOrTimebaseRef */ IntPtr clockOrTimebase1, /* CMClockOrTimebaseRef */ IntPtr clockOrTimebase2);

		/// <param name="clockOrTimebaseA">To be added.</param>
		///         <param name="clockOrTimebaseB">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool MightDrift (CMClockOrTimebase clockOrTimebaseA, CMClockOrTimebase clockOrTimebaseB)
		{
			if (clockOrTimebaseA is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockOrTimebaseA));

			if (clockOrTimebaseB is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (clockOrTimebaseB));

			bool result = CMSyncMightDrift (clockOrTimebaseA.Handle, clockOrTimebaseB.Handle) != 0;
			GC.KeepAlive (clockOrTimebaseA);
			GC.KeepAlive (clockOrTimebaseB);
			return result;
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern /* CMTimebase */ IntPtr CMTimebaseCopySourceTimebase (/* CMTimebase */ IntPtr timebase);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern int CMTimebaseSetSourceTimebase (/* CMTimebase */ IntPtr timebase, /* CMTimebase */ IntPtr newSourceTimebase);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public CMTimebase? SourceTimebase {
			get {
				var source = CMTimebaseCopySourceTimebase (Handle);
				return Runtime.GetINativeObject<CMTimebase> (source, true);
			}
			set {
				CMTimebaseSetSourceTimebase (Handle, value.GetHandle ());
				GC.KeepAlive (value);
			}
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern /* CMClock */ IntPtr CMTimebaseCopySourceClock (/* CMTimebase */ IntPtr timebase);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern int CMTimebaseSetSourceClock (/* CMTimebase */ IntPtr timebase, /* CMClock */ IntPtr newSourceClock);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public CMClock? SourceClock {
			get {
				var clock = CMTimebaseCopySourceClock (Handle);
				return Runtime.GetINativeObject<CMClock> (clock, true);
			}
			set {
				CMTimebaseSetSourceClock (Handle, value.GetHandle ());
				GC.KeepAlive (value);
			}
		}

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMediaLibrary)]
		static extern /* CMClock */ IntPtr CMTimebaseCopyUltimateSourceClock (/* CMTimebase */ IntPtr timebase);

		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
		public CMClock? UltimateSourceClock {
			get {
				var clock = CMTimebaseCopyUltimateSourceClock (Handle);
				return Runtime.GetINativeObject<CMClock> (clock, true);
			}
		}

#endif // !COREBUILD
	}
}
