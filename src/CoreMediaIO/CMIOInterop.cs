#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreMedia;
using ObjCRuntime;

namespace CoreMediaIO {

#if !COREBUILD
	/// <summary>Low-level P/Invoke declarations for CoreMediaIO hardware object, device, stream, and sample buffer C functions.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst15.4")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	internal static unsafe class CMIOInterop {

		// CMIOHardwareObject.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern void CMIOObjectShow (uint objectId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern byte CMIOObjectHasProperty (uint objectId, CMIOObjectPropertyAddress* address);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectIsPropertySettable (uint objectId, CMIOObjectPropertyAddress* address, byte* isSettable);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectGetPropertyDataSize (uint objectId, CMIOObjectPropertyAddress* address, uint qualifierDataSize, IntPtr qualifierData, uint* dataSize);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectGetPropertyData (uint objectId, CMIOObjectPropertyAddress* address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, uint* dataUsed, IntPtr data);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectSetPropertyData (uint objectId, CMIOObjectPropertyAddress* address, uint qualifierDataSize, IntPtr qualifierData, uint dataSize, IntPtr data);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectAddPropertyListener (uint objectId, CMIOObjectPropertyAddress* address, IntPtr listener, IntPtr clientData);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectRemovePropertyListener (uint objectId, CMIOObjectPropertyAddress* address, IntPtr listener, IntPtr clientData);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectAddPropertyListenerBlock (uint objectId, CMIOObjectPropertyAddress* address, IntPtr dispatchQueue, IntPtr listener);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOObjectRemovePropertyListenerBlock (uint objectId, CMIOObjectPropertyAddress* address, IntPtr dispatchQueue, IntPtr listener);

		// CMIOHardwareDevice.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIODeviceStartStream (uint deviceId, uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIODeviceStopStream (uint deviceId, uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIODeviceProcessAVCCommand (uint deviceId, CMIODeviceAvcCommand* ioAvcCommand);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIODeviceProcessRS422Command (uint deviceId, CMIODeviceRS422Command* ioRS422Command);

		// CMIOHardwareStream.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamCopyBufferQueue (uint streamId, IntPtr queueAlteredProc, IntPtr queueAlteredRefCon, IntPtr* queue);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamDeckPlay (uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamDeckStop (uint streamId);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamDeckJog (uint streamId, int speed);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamDeckCueTo (uint streamId, ulong frameNumber, byte playOnCue);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamClockCreate (IntPtr allocator, IntPtr clockName, IntPtr sourceIdentifier, CMTime getTimeCallMinimumInterval, uint numberOfEventsForRateSmoothing, uint numberOfAveragesForRateSmoothing, IntPtr* clock);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamClockPostTimingEvent (CMTime eventTime, ulong hostTime, byte resynchronize, IntPtr clock);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOStreamClockInvalidate (IntPtr clock);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern CMTime CMIOStreamClockConvertHostTimeToDeviceTime (ulong hostTime, IntPtr clock);

		// CMIOSampleBuffer.h

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOSampleBufferCreate (IntPtr allocator, IntPtr dataBuffer, IntPtr formatDescription, uint numSamples, uint numSampleTimingEntries, CMSampleTimingInfo* sampleTimingArray, uint numSampleSizeEntries, nuint* sampleSizeArray, ulong sequenceNumber, uint discontinuityFlags, IntPtr* sampleBufferOut);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOSampleBufferCreateForImageBuffer (IntPtr allocator, IntPtr imageBuffer, IntPtr formatDescription, CMSampleTimingInfo* sampleTiming, ulong sequenceNumber, uint discontinuityFlags, IntPtr* sampleBufferOut);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOSampleBufferCreateNoDataMarker (IntPtr allocator, uint noDataEvent, IntPtr formatDescription, ulong sequenceNumber, uint discontinuityFlags, IntPtr* sampleBufferOut);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern void CMIOSampleBufferSetSequenceNumber (IntPtr allocator, IntPtr sampleBuffer, ulong sequenceNumber);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern ulong CMIOSampleBufferGetSequenceNumber (IntPtr sampleBuffer);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern void CMIOSampleBufferSetDiscontinuityFlags (IntPtr allocator, IntPtr sampleBuffer, uint discontinuityFlags);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern uint CMIOSampleBufferGetDiscontinuityFlags (IntPtr sampleBuffer);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOSampleBufferCopyNonRequiredAttachments (IntPtr sourceSampleBuffer, IntPtr destinationSampleBuffer, uint attachmentMode);

		[DllImport (Constants.CoreMediaIOLibrary)]
		internal static extern int CMIOSampleBufferCopySampleAttachments (IntPtr sourceSampleBuffer, IntPtr destinationSampleBuffer);
	}
#endif // !COREBUILD
}
