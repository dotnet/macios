// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using AVFoundation;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class AVFoundationTrampolines {
	
	[Export<Property> ("avAssetImageGenerateAsynchronouslyForTimeCompletionHandler", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAssetImageGenerateAsynchronouslyForTimeCompletionHandler AVAssetImageGenerateAsynchronouslyForTimeCompletionHandler { get; set; }

	[Export<Property> ("avAssetImageGeneratorCompletionHandler", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAssetImageGeneratorCompletionHandler AVAssetImageGeneratorCompletionHandler { get; set; }

	[Export<Property> ("avAssetImageGeneratorCompletionHandler2", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAssetImageGeneratorCompletionHandler2 AVAssetImageGeneratorCompletionHandler2 { get; set; }

	[Export<Property> ("avAssetPlaybackAssistantLoadPlaybackConfigurationOptionsHandler", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAssetPlaybackAssistantLoadPlaybackConfigurationOptionsHandler AVAssetPlaybackAssistantLoadPlaybackConfigurationOptionsHandler { get; set; }

	[Export<Property> ("avAudioConverterInputHandler", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioConverterInputHandler AVAudioConverterInputHandler { get; set; }

	[Export<Property> ("avAudioEngineManualRenderingBlock", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioEngineManualRenderingBlock AVAudioEngineManualRenderingBlock { get; set; }

	[Export<Property> ("avAudioIONodeInputBlock", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioIONodeInputBlock AVAudioIONodeInputBlock { get; set; }

	[Export<Property> ("avAudioInputNodeMutedSpeechEventListener", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioInputNodeMutedSpeechEventListener AVAudioInputNodeMutedSpeechEventListener { get; set; }

	[Export<Property> ("avAudioNodeTapBlock", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioNodeTapBlock AVAudioNodeTapBlock { get; set; }

	[Export<Property> ("avAudioSequencerUserCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioSequencerUserCallback AVAudioSequencerUserCallback { get; set; }

	[Export<Property> ("avAudioSinkNodeReceiverHandlerRaw", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioSinkNodeReceiverHandlerRaw AVAudioSinkNodeReceiverHandlerRaw { get; set; }

	[Export<Property> ("avAudioSourceNodeRenderHandlerRaw", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioSourceNodeRenderHandlerRaw AVAudioSourceNodeRenderHandlerRaw { get; set; }

	[Export<Property> ("avAudioUnitComponentFilter", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVAudioUnitComponentFilter AVAudioUnitComponentFilter { get; set; }

	[Export<Property> ("avCaptureCompletionHandler", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVCaptureCompletionHandler AVCaptureCompletionHandler { get; set; }

	[Export<Property> ("avCaptureIndexPickerCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVCaptureIndexPickerCallback AVCaptureIndexPickerCallback { get; set; }

	[Export<Property> ("avCaptureIndexPickerTitleTransform", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVCaptureIndexPickerTitleTransform AVCaptureIndexPickerTitleTransform { get; set; }

	[Export<Property> ("avCaptureSliderCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVCaptureSliderCallback AVCaptureSliderCallback { get; set; }

	[Export<Property> ("avCaptureSystemExposureBiasSliderCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVCaptureSystemExposureBiasSliderCallback AVCaptureSystemExposureBiasSliderCallback { get; set; }

	[Export<Property> ("avCaptureSystemZoomSliderCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVCaptureSystemZoomSliderCallback AVCaptureSystemZoomSliderCallback { get; set; }

	[Export<Property> ("avCompletion", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVCompletion AVCompletion { get; set; }

	[Export<Property> ("avExternalStorageDeviceRequestAccessCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVExternalStorageDeviceRequestAccessCallback AVExternalStorageDeviceRequestAccessCallback { get; set; }

	[Export<Property> ("avMusicEventEnumerationBlock", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVMusicEventEnumerationBlock AVMusicEventEnumerationBlock { get; set; }

	[Export<Property> ("avMutableCompositionInsertHandler", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVMutableCompositionInsertHandler AVMutableCompositionInsertHandler { get; set; }

	[Export<Property> ("avMutableVideoCompositionCreateApplier", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVMutableVideoCompositionCreateApplier AVMutableVideoCompositionCreateApplier { get; set; }

	[Export<Property> ("avMutableVideoCompositionCreateCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVMutableVideoCompositionCreateCallback AVMutableVideoCompositionCreateCallback { get; set; }

	[Export<Property> ("avPermissionGranted", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVPermissionGranted AVPermissionGranted { get; set; }

	[Export<Property> ("avPlayerItemIntegratedTimelineAddBoundaryTimeObserverCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVPlayerItemIntegratedTimelineAddBoundaryTimeObserverCallback AVPlayerItemIntegratedTimelineAddBoundaryTimeObserverCallback { get; set; }

	[Export<Property> ("avPlayerItemIntegratedTimelineAddPeriodicTimeObserverCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVPlayerItemIntegratedTimelineAddPeriodicTimeObserverCallback AVPlayerItemIntegratedTimelineAddPeriodicTimeObserverCallback { get; set; }

	[Export<Property> ("avPlayerItemIntegratedTimelineSeekCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVPlayerItemIntegratedTimelineSeekCallback AVPlayerItemIntegratedTimelineSeekCallback { get; set; }

	[Export<Property> ("avRequestAccessStatus", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVRequestAccessStatus AVRequestAccessStatus { get; set; }

	[Export<Property> ("avSampleBufferGeneratorBatchMakeReadyCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVSampleBufferGeneratorBatchMakeReadyCallback AVSampleBufferGeneratorBatchMakeReadyCallback { get; set; }

	[Export<Property> ("avSampleBufferVideoRendererLoadVideoPerformanceMetricsCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVSampleBufferVideoRendererLoadVideoPerformanceMetricsCallback AVSampleBufferVideoRendererLoadVideoPerformanceMetricsCallback { get; set; }

	[Export<Property> ("avSpeechSynthesisProviderOutputBlock", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVSpeechSynthesisProviderOutputBlock AVSpeechSynthesisProviderOutputBlock { get; set; }

	[Export<Property> ("avSpeechSynthesizerBufferCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVSpeechSynthesizerBufferCallback AVSpeechSynthesizerBufferCallback { get; set; }

	[Export<Property> ("avSpeechSynthesizerMarkerCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVSpeechSynthesizerMarkerCallback AVSpeechSynthesizerMarkerCallback { get; set; }

	[Export<Property> ("avSpeechSynthesizerRequestPersonalVoiceAuthorizationCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVSpeechSynthesizerRequestPersonalVoiceAuthorizationCallback AVSpeechSynthesizerRequestPersonalVoiceAuthorizationCallback { get; set; }

	[Export<Property> ("avVideoCompositionCreateApplier", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVVideoCompositionCreateApplier AVVideoCompositionCreateApplier { get; set; }

	[Export<Property> ("avVideoCompositionCreateCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVVideoCompositionCreateCallback AVVideoCompositionCreateCallback { get; set; }

	[Export<Property> ("avVideoCompositionDetermineValidityCallback", ArgumentSemantic.Copy)]
	public partial AVFoundation.AVVideoCompositionDetermineValidityCallback AVVideoCompositionDetermineValidityCallback { get; set; }
}
