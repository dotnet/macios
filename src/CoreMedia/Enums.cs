using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace CoreMedia {
	// keys names got changed at some point, but they all refer to a CMSampleBuffer (there is not CMSample obj)
	[MacCatalyst (13, 1)]
	enum CMSampleBufferAttachmentKey {
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_NotSync")]
		NotSync,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_PartialSync")]
		PartialSync,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_HasRedundantCoding")]
		HasRedundantCoding,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_IsDependedOnByOthers")]
		IsDependedOnByOthers,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_DependsOnOthers")]
		DependsOnOthers,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_EarlierDisplayTimesAllowed")]
		EarlierDisplayTimesAllowed,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_DisplayImmediately")]
		DisplayImmediately,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleAttachmentKey_DoNotDisplay")]
		DoNotDisplay,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCTemporalLevelInfo")]
		HevcTemporalLevelInfo,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCTemporalSubLayerAccess")]
		HevcTemporalSubLayerAccess,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCStepwiseTemporalSubLayerAccess")]
		HevcStepwiseTemporalSubLayerAccess,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleAttachmentKey_HEVCSyncSampleNALUnitType")]
		HevcSyncSampleNalUnitType,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_ResetDecoderBeforeDecoding")]
		ResetDecoderBeforeDecoding,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_DrainAfterDecoding")]
		DrainAfterDecoding,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_PostNotificationWhenConsumed")]
		PostNotificationWhenConsumed,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_ResumeOutput")]
		ResumeOutput,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_TransitionID")]
		TransitionId,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_TrimDurationAtStart")]
		TrimDurationAtStart,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_TrimDurationAtEnd")]
		TrimDurationAtEnd,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_SpeedMultiplier")]
		SpeedMultiplier,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_Reverse")]
		Reverse,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_FillDiscontinuitiesWithSilence")]
		FillDiscontinuitiesWithSilence,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_EmptyMedia")]
		EmptyMedia,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_PermanentEmptyMedia")]
		PermanentEmptyMedia,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_DisplayEmptyMediaImmediately")]
		DisplayEmptyMediaImmediately,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_EndsPreviousSampleDuration")]
		EndsPreviousSampleDuration,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_SampleReferenceURL")]
		SampleReferenceUrl,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_SampleReferenceByteOffset")]
		SampleReferenceByteOffset,
		/// <summary>To be added.</summary>
		[Field ("kCMSampleBufferAttachmentKey_GradualDecoderRefresh")]
		GradualDecoderRefresh,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_DroppedFrameReason")]
		DroppedFrameReason,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_StillImageLensStabilizationInfo")]
		StillImageLensStabilizationInfo,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_CameraIntrinsicMatrix")]
		CameraIntrinsicMatrix,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_DroppedFrameReasonInfo")]
		DroppedFrameReasonInfo,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		[Field ("kCMSampleBufferAttachmentKey_ForceKeyFrame")]
		ForceKeyFrame,
		[TV (16, 0), Mac (13, 0), iOS (16, 0)]
		[MacCatalyst (16, 0)]
		[Field ("kCMSampleAttachmentKey_HDR10PlusPerFrameData")]
		Hdr10PlusPerFrameData,
	}
}
