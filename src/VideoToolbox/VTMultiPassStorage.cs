// 
// VTMultiPassStorage.cs: VideoToolbox VTMultiPassStorage class
//
// Authors:
//	Alex Soto (alex.soto@xamarin.com)
//     
// Copyright 2015 Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using CoreMedia;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace VideoToolbox {

#if NET
	/// <summary>Class that provides a storage for encoding metadata.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst")]
#endif
	public class VTMultiPassStorage : NativeObject {
		bool closed;
		VTStatus closedStatus;

#if !NET
		protected internal VTMultiPassStorage (NativeHandle handle)
			: base (handle, false)
		{
		}
#endif

		[Preserve (Conditional = true)]
		internal VTMultiPassStorage (NativeHandle handle, bool owns)
			: base (handle, false)
		{
		}

		/// <include file="../../docs/api/VideoToolbox/VTMultiPassStorage.xml" path="/Documentation/Docs[@DocId='M:VideoToolbox.VTMultiPassStorage.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero)
				Close ();
			base.Dispose (disposing);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		unsafe extern static /* OSStatus */ VTStatus VTMultiPassStorageCreate (
			/* CFAllocatorRef */IntPtr allocator, /* can be null */
			/* CFURLRef */ IntPtr fileUrl, /* can be null */
			/* CMTimeRange */ CMTimeRange timeRange, /* can be kCMTimeRangeInvalid */
			/* CFDictionaryRef */ IntPtr options, /* can be null */
			/* VTMultiPassStorageRef */ IntPtr* multiPassStorageOut);

		// Convenience method taking a strong dictionary
		/// <param name="options">To be added.</param>
		///         <param name="fileUrl">To be added.</param>
		///         <param name="timeRange">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static VTMultiPassStorage? Create (
			VTMultiPassStorageCreationOptions? options,
			NSUrl? fileUrl = null,
			CMTimeRange? timeRange = null)
		{
			return Create (fileUrl, timeRange, options?.Dictionary);
		}

		/// <param name="fileUrl">To be added.</param>
		///         <param name="timeRange">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static VTMultiPassStorage? Create (
			NSUrl? fileUrl = null,
			CMTimeRange? timeRange = null,
			NSDictionary? options = null)
		{
			VTStatus status;
			IntPtr ret;
			unsafe {
				status = VTMultiPassStorageCreate (
				IntPtr.Zero,
				fileUrl.GetHandle (),
				timeRange ?? CMTimeRange.InvalidRange,
				options.GetHandle (),
				&ret);
				GC.KeepAlive (fileUrl);
				GC.KeepAlive (options);
			}

			if (status != VTStatus.Ok)
				return null;

			return new VTMultiPassStorage (ret, true);
		}

		[DllImport (Constants.VideoToolboxLibrary)]
		extern static /* OSStatus */ VTStatus VTMultiPassStorageClose (/* VTMultiPassStorage */ IntPtr multiPassStorage);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public VTStatus Close ()
		{
			if (closed)
				return closedStatus;
			closedStatus = VTMultiPassStorageClose (Handle);
			closed = true;
			return closedStatus;
		}
	}
}
