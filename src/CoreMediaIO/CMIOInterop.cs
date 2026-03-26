#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreMedia;
using ObjCRuntime;

namespace CoreMediaIO {

#if !COREBUILD
	/// <summary>Provides P/Invoke declarations for CoreMediaIO hardware object, device, and stream C functions.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static unsafe class CMIOInterop {

		// CMIOHardwareObject.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern void CMIOObjectShow (uint objectId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern byte CMIOObjectHasProperty (uint objectId, IntPtr address);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectIsPropertySettable (uint objectId, IntPtr address, byte* isSettable);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectGetPropertyDataSize (uint objectId, IntPtr address, uint qualifierDataSize, IntPtr qualifierData, uint* dataSize);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectGetPropertyData (uint objectId, IntPtr address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, uint* dataUsed, IntPtr data);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectSetPropertyData (uint objectId, IntPtr address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, IntPtr data);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectAddPropertyListener (uint objectId, IntPtr address, IntPtr listener, IntPtr clientData);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectRemovePropertyListener (uint objectId, IntPtr address, IntPtr listener, IntPtr clientData);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectAddPropertyListenerBlock (uint objectId, IntPtr address, IntPtr dispatchQueue, IntPtr listener);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOObjectRemovePropertyListenerBlock (uint objectId, IntPtr address, IntPtr dispatchQueue, IntPtr listener);

		// CMIOHardwareDevice.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIODeviceStartStream (uint deviceId, uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIODeviceStopStream (uint deviceId, uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIODeviceProcessAVCCommand (uint deviceId, IntPtr ioAvcCommand);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIODeviceProcessRS422Command (uint deviceId, IntPtr ioRS422Command);

		// CMIOHardwareStream.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamCopyBufferQueue (uint streamId, IntPtr queueAlteredProc, IntPtr queueAlteredRefCon, IntPtr* queue);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamDeckPlay (uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamDeckStop (uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamDeckJog (uint streamId, int speed);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamDeckCueTo (uint streamId, ulong frameNumber, byte playOnCue);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamClockCreate (IntPtr allocator, IntPtr clockName, IntPtr sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing, IntPtr* clock);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamClockPostTimingEvent (CMTime eventTime, ulong hostTime, byte resynchronize, IntPtr clock);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOStreamClockInvalidate (IntPtr clock);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern CMTime CMIOStreamClockConvertHostTimeToDeviceTime (ulong hostTime, IntPtr clock);

		// CMIOSampleBuffer.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOSampleBufferCreate (IntPtr allocator, IntPtr dataBuffer, IntPtr formatDescription, uint numSamples, uint numSampleTimingEntries, IntPtr sampleTimingArray, uint numSampleSizeEntries, IntPtr sampleSizeArray, ulong sequenceNumber, uint discontinuityFlags, IntPtr* sampleBufferOut);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOSampleBufferCreateForImageBuffer (IntPtr allocator, IntPtr imageBuffer, IntPtr formatDescription, IntPtr sampleTiming, ulong sequenceNumber, uint discontinuityFlags, IntPtr* sampleBufferOut);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOSampleBufferCreateNoDataMarker (IntPtr allocator, uint noDataEvent, IntPtr formatDescription, ulong sequenceNumber, uint discontinuityFlags, IntPtr* sampleBufferOut);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern void CMIOSampleBufferSetSequenceNumber (IntPtr sampleBuffer, ulong sequenceNumber);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern ulong CMIOSampleBufferGetSequenceNumber (IntPtr sampleBuffer);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern void CMIOSampleBufferSetDiscontinuityFlags (IntPtr sampleBuffer, uint discontinuityFlags);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern uint CMIOSampleBufferGetDiscontinuityFlags (IntPtr sampleBuffer);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOSampleBufferCopyNonRequiredAttachments (IntPtr sourceSampleBuffer, IntPtr destinationSampleBuffer, uint attachmentMode);

		[DllImport (Constants.CoreMediaIOLibrary)]
		public static extern int CMIOSampleBufferCopySampleAttachments (IntPtr sourceSampleBuffer, IntPtr destinationSampleBuffer);
	}
#endif // !COREBUILD
}
