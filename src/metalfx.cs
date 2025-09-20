using System;
using System.ComponentModel;

using CoreFoundation;
using CoreGraphics;
using Foundation;
using Metal;
using ObjCRuntime;

namespace MetalFX {

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 4)]
	[Native]
	public enum MTLFXSpatialScalerColorProcessingMode : long {
		Perceptual = 0,
		Linear = 1,
		Hdr = 2,
	}

	interface IMTLFXSpatialScaler { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 4)]
	[Protocol]
	interface MTLFXSpatialScaler : MTLFXSpatialScalerBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTLCommandBuffer commandBuffer);
	}

	interface IMTLFXTemporalScaler { }

	[Mac (13, 0), iOS (16, 0)]
	[TV (18, 4)]
	[Protocol]
	interface MTLFXTemporalScaler : MTLFXTemporalScalerBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTLCommandBuffer commandBuffer);
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLFXSpatialScalerDescriptor : NSCopying {
		[Export ("colorTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ColorTextureFormat { get; set; }

		[Export ("outputTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat OutputTextureFormat { get; set; }

		[Export ("inputWidth")]
		nuint InputWidth { get; set; }

		[Export ("inputHeight")]
		nuint InputHeight { get; set; }

		[Export ("outputWidth")]
		nuint OutputWidth { get; set; }

		[Export ("outputHeight")]
		nuint OutputHeight { get; set; }

		[Export ("colorProcessingMode", ArgumentSemantic.Assign)]
		MTLFXSpatialScalerColorProcessingMode ColorProcessingMode { get; set; }

		[Export ("newSpatialScalerWithDevice:")]
		[return: NullAllowed, Release]
		IMTLFXSpatialScaler Create (IMTLDevice device);

		[Static]
		[Export ("supportsDevice:")]
		bool SupportsDevice (IMTLDevice device);

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Export ("newSpatialScalerWithDevice:compiler:")]
		[return: NullAllowed, Release]
		IMTL4FXSpatialScaler Create (IMTLDevice device, IMTL4Compiler compiler);

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Static]
		[Export ("supportsMetal4FX:")]
		bool SupportsMetal4FX (IMTLDevice device);
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLFXTemporalScalerDescriptor : NSCopying {
		[Export ("colorTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ColorTextureFormat { get; set; }

		[Export ("depthTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DepthTextureFormat { get; set; }

		[Export ("motionTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat MotionTextureFormat { get; set; }

		[Export ("outputTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat OutputTextureFormat { get; set; }

		[Export ("inputWidth")]
		nuint InputWidth { get; set; }

		[Export ("inputHeight")]
		nuint InputHeight { get; set; }

		[Export ("outputWidth")]
		nuint OutputWidth { get; set; }

		[Export ("outputHeight")]
		nuint OutputHeight { get; set; }

		[Export ("autoExposureEnabled")]
		bool AutoExposureEnabled { [Bind ("isAutoExposureEnabled")] get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("requiresSynchronousInitialization")]
		bool RequiresSynchronousInitialization { get; set; }

		[Export ("inputContentPropertiesEnabled")]
		bool InputContentPropertiesEnabled { [Bind ("isInputContentPropertiesEnabled")] get; set; }

		[Export ("inputContentMinScale")]
		float InputContentMinScale { get; set; }

		[Export ("inputContentMaxScale")]
		float InputContentMaxScale { get; set; }

		[Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("reactiveMaskTextureEnabled")]
		bool ReactiveMaskTextureEnabled { [Bind ("isReactiveMaskTextureEnabled")] get; set; }

		[Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Export ("reactiveMaskTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ReactiveMaskTextureFormat { get; set; }

		[Export ("newTemporalScalerWithDevice:")]
		[return: NullAllowed, Release]
		IMTLFXTemporalScaler Create (IMTLDevice device);

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Export ("newTemporalScalerWithDevice:compiler:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4FXTemporalScaler Create (IMTLDevice device, IMTL4Compiler compiler);

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportedInputContentMinScaleForDevice:")]
		float GetSupportedInputContentMinScale (IMTLDevice device);

		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportedInputContentMaxScaleForDevice:")]
		float GetSupportedInputContentMaxScale (IMTLDevice device);

		[Static]
		[Export ("supportsDevice:")]
		bool SupportsDevice (IMTLDevice device);

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Static]
		[Export ("supportsMetal4FX:")]
		bool SupportsMetal4FX (IMTLDevice device);
	}

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLFXFrameInterpolatorDescriptor : NSCopying {
		[Export ("colorTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ColorTextureFormat { get; set; }

		[Export ("outputTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat OutputTextureFormat { get; set; }

		[Export ("depthTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DepthTextureFormat { get; set; }

		[Export ("motionTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat MotionTextureFormat { get; set; }

		[Export ("uiTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat UITextureFormat { get; [Bind ("setUITextureFormat:")] set; }

		[NullAllowed, Export ("scaler", ArgumentSemantic.Retain)]
		IMTLFXFrameInterpolatableScaler Scaler { get; set; }

		[Export ("inputWidth")]
		nuint InputWidth { get; set; }

		[Export ("inputHeight")]
		nuint InputHeight { get; set; }

		[Export ("outputWidth")]
		nuint OutputWidth { get; set; }

		[Export ("outputHeight")]
		nuint OutputHeight { get; set; }

		[Export ("newFrameInterpolatorWithDevice:")]
		[return: NullAllowed]
		[return: Release]
		IMTLFXFrameInterpolator Create (IMTLDevice device);

		[Export ("newFrameInterpolatorWithDevice:compiler:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4FXFrameInterpolator Create (IMTLDevice device, IMTL4Compiler compiler);

		[Static]
		[Export ("supportsMetal4FX:")]
		bool SupportsMetal4FX (IMTLDevice device);

		[Static]
		[Export ("supportsDevice:")]
		bool SupportsDevice (IMTLDevice device);
	}

	interface IMTLFXFrameInterpolatorBase { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLFXFrameInterpolatorBase {
		[Abstract]
		[Export ("colorTextureUsage")]
		MTLTextureUsage ColorTextureUsage { get; }

		[Abstract]
		[Export ("outputTextureUsage")]
		MTLTextureUsage OutputTextureUsage { get; }

		[Abstract]
		[Export ("depthTextureUsage")]
		MTLTextureUsage DepthTextureUsage { get; }

		[Abstract]
		[Export ("motionTextureUsage")]
		MTLTextureUsage MotionTextureUsage { get; }

		[Abstract]
		[Export ("uiTextureUsage")]
		MTLTextureUsage UITextureUsage { get; }

		[Abstract]
		[Export ("colorTextureFormat")]
		MTLPixelFormat ColorTextureFormat { get; }

		[Abstract]
		[Export ("depthTextureFormat")]
		MTLPixelFormat DepthTextureFormat { get; }

		[Abstract]
		[Export ("motionTextureFormat")]
		MTLPixelFormat MotionTextureFormat { get; }

		[Abstract]
		[Export ("outputTextureFormat")]
		MTLPixelFormat OutputTextureFormat { get; }

		[Abstract]
		[Export ("inputWidth")]
		nuint InputWidth { get; }

		[Abstract]
		[Export ("inputHeight")]
		nuint InputHeight { get; }

		[Abstract]
		[Export ("outputWidth")]
		nuint OutputWidth { get; }

		[Abstract]
		[Export ("outputHeight")]
		nuint OutputHeight { get; }

		[Abstract]
		[Export ("uiTextureFormat")]
		MTLPixelFormat UITextureFormat { get; }

		[Abstract]
		[NullAllowed, Export ("colorTexture", ArgumentSemantic.Retain)]
		IMTLTexture ColorTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("prevColorTexture", ArgumentSemantic.Retain)]
		IMTLTexture PrevColorTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("depthTexture", ArgumentSemantic.Retain)]
		IMTLTexture DepthTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("motionTexture", ArgumentSemantic.Retain)]
		IMTLTexture MotionTexture { get; set; }

		[Abstract]
		[Export ("motionVectorScaleX")]
		float MotionVectorScaleX { get; set; }

		[Abstract]
		[Export ("motionVectorScaleY")]
		float MotionVectorScaleY { get; set; }

		[Abstract]
		[Export ("deltaTime")]
		float DeltaTime { get; set; }

		[Abstract]
		[Export ("nearPlane")]
		float NearPlane { get; set; }

		[Abstract]
		[Export ("farPlane")]
		float FarPlane { get; set; }

		[Abstract]
		[Export ("fieldOfView")]
		float FieldOfView { get; set; }

		[Abstract]
		[Export ("aspectRatio")]
		float AspectRatio { get; set; }

		[Abstract]
		[NullAllowed, Export ("uiTexture", ArgumentSemantic.Retain)]
		IMTLTexture UiTexture { get; [Bind ("setUITexture:")] set; }

		[Abstract]
		[Export ("jitterOffsetX")]
		float JitterOffsetX { get; set; }

		[Abstract]
		[Export ("jitterOffsetY")]
		float JitterOffsetY { get; set; }

		[Abstract]
		[Export ("uiTextureComposited")]
		bool UITextureComposited { [Bind ("isUITextureComposited")] get; [Bind ("setIsUITextureComposited:")] set; }

		[Abstract]
		[Export ("shouldResetHistory")]
		bool ShouldResetHistory { get; set; }

		[Abstract]
		[NullAllowed, Export ("outputTexture", ArgumentSemantic.Retain)]
		IMTLTexture OutputTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("fence", ArgumentSemantic.Retain)]
		IMTLFence Fence { get; set; }

		[Abstract]
		[Export ("depthReversed")]
		bool DepthReversed { [Bind ("isDepthReversed")] get; set; }
	}

	interface IMTLFXFrameInterpolator { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLFXFrameInterpolator : MTLFXFrameInterpolatorBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTLCommandBuffer commandBuffer);
	}

	interface IMTL4FXSpatialScaler { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4FXSpatialScaler : MTLFXSpatialScalerBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTL4CommandBuffer commandBuffer);
	}

	interface IMTL4FXTemporalScaler { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4FXTemporalScaler : MTLFXTemporalScalerBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTL4CommandBuffer commandBuffer);
	}

	interface IMTL4FXTemporalDenoisedScaler { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4FXTemporalDenoisedScaler : MTLFXTemporalDenoisedScalerBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTL4CommandBuffer commandBuffer);
	}

	interface IMTL4FXFrameInterpolator { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4FXFrameInterpolator : MTLFXFrameInterpolatorBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTL4CommandBuffer commandBuffer);
	}

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLFXTemporalDenoisedScalerDescriptor : NSCopying {
		[Export ("colorTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ColorTextureFormat { get; set; }

		[Export ("depthTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DepthTextureFormat { get; set; }

		[Export ("motionTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat MotionTextureFormat { get; set; }

		[Export ("diffuseAlbedoTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DiffuseAlbedoTextureFormat { get; set; }

		[Export ("specularAlbedoTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat SpecularAlbedoTextureFormat { get; set; }

		[Export ("normalTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat NormalTextureFormat { get; set; }

		[Export ("roughnessTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat RoughnessTextureFormat { get; set; }

		[Export ("specularHitDistanceTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat SpecularHitDistanceTextureFormat { get; set; }

		[Export ("denoiseStrengthMaskTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DenoiseStrengthMaskTextureFormat { get; set; }

		[Export ("transparencyOverlayTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat TransparencyOverlayTextureFormat { get; set; }

		[Export ("outputTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat OutputTextureFormat { get; set; }

		[Export ("inputWidth")]
		nuint InputWidth { get; set; }

		[Export ("inputHeight")]
		nuint InputHeight { get; set; }

		[Export ("outputWidth")]
		nuint OutputWidth { get; set; }

		[Export ("outputHeight")]
		nuint OutputHeight { get; set; }

		[Export ("requiresSynchronousInitialization")]
		bool RequiresSynchronousInitialization { get; set; }

		[Export ("autoExposureEnabled")]
		bool AutoExposureEnabled { [Bind ("isAutoExposureEnabled")] get; set; }

		[Export ("reactiveMaskTextureEnabled")]
		bool ReactiveMaskTextureEnabled { [Bind ("isReactiveMaskTextureEnabled")] get; set; }

		[Export ("reactiveMaskTextureFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat ReactiveMaskTextureFormat { get; set; }

		[Export ("specularHitDistanceTextureEnabled")]
		bool SpecularHitDistanceTextureEnabled { [Bind ("isSpecularHitDistanceTextureEnabled")] get; set; }

		[Export ("denoiseStrengthMaskTextureEnabled")]
		bool DenoiseStrengthMaskTextureEnabled { [Bind ("isDenoiseStrengthMaskTextureEnabled")] get; set; }

		[Export ("transparencyOverlayTextureEnabled")]
		bool TransparencyOverlayTextureEnabled { [Bind ("isTransparencyOverlayTextureEnabled")] get; set; }

		[Export ("newTemporalDenoisedScalerWithDevice:")]
		[return: NullAllowed]
		[return: Release]
		IMTLFXTemporalDenoisedScaler Create (IMTLDevice device);

		[Export ("newTemporalDenoisedScalerWithDevice:compiler:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4FXTemporalDenoisedScaler Create (IMTLDevice device, IMTL4Compiler compiler);

		[Static]
		[Export ("supportedInputContentMinScaleForDevice:")]
		float GetSupportedInputContentMinScale (IMTLDevice device);

		[Static]
		[Export ("supportedInputContentMaxScaleForDevice:")]
		float GetSupportedInputContentMaxScale (IMTLDevice device);

		[Static]
		[Export ("supportsMetal4FX:")]
		bool SupportsMetal4FX (IMTLDevice device);

		[Static]
		[Export ("supportsDevice:")]
		bool SupportsDevice (IMTLDevice device);
	}

	interface IMTLFXTemporalDenoisedScalerBase { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLFXTemporalDenoisedScalerBase : MTLFXFrameInterpolatableScaler {
		[Abstract]
		[Export ("colorTextureUsage")]
		MTLTextureUsage ColorTextureUsage { get; }

		[Abstract]
		[Export ("depthTextureUsage")]
		MTLTextureUsage DepthTextureUsage { get; }

		[Abstract]
		[Export ("motionTextureUsage")]
		MTLTextureUsage MotionTextureUsage { get; }

		[Abstract]
		[Export ("reactiveTextureUsage")]
		MTLTextureUsage ReactiveTextureUsage { get; }

		[Abstract]
		[Export ("diffuseAlbedoTextureUsage")]
		MTLTextureUsage DiffuseAlbedoTextureUsage { get; }

		[Abstract]
		[Export ("specularAlbedoTextureUsage")]
		MTLTextureUsage SpecularAlbedoTextureUsage { get; }

		[Abstract]
		[Export ("normalTextureUsage")]
		MTLTextureUsage NormalTextureUsage { get; }

		[Abstract]
		[Export ("roughnessTextureUsage")]
		MTLTextureUsage RoughnessTextureUsage { get; }

		[Abstract]
		[Export ("specularHitDistanceTextureUsage")]
		MTLTextureUsage SpecularHitDistanceTextureUsage { get; }

		[Abstract]
		[Export ("denoiseStrengthMaskTextureUsage")]
		MTLTextureUsage DenoiseStrengthMaskTextureUsage { get; }

		[Abstract]
		[Export ("transparencyOverlayTextureUsage")]
		MTLTextureUsage TransparencyOverlayTextureUsage { get; }

		[Abstract]
		[Export ("outputTextureUsage")]
		MTLTextureUsage OutputTextureUsage { get; }

		[Abstract]
		[NullAllowed, Export ("colorTexture", ArgumentSemantic.Retain)]
		IMTLTexture ColorTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("depthTexture", ArgumentSemantic.Retain)]
		IMTLTexture DepthTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("motionTexture", ArgumentSemantic.Retain)]
		IMTLTexture MotionTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("diffuseAlbedoTexture", ArgumentSemantic.Retain)]
		IMTLTexture DiffuseAlbedoTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("specularAlbedoTexture", ArgumentSemantic.Retain)]
		IMTLTexture SpecularAlbedoTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("normalTexture", ArgumentSemantic.Retain)]
		IMTLTexture NormalTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("roughnessTexture", ArgumentSemantic.Retain)]
		IMTLTexture RoughnessTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("specularHitDistanceTexture", ArgumentSemantic.Retain)]
		IMTLTexture SpecularHitDistanceTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("denoiseStrengthMaskTexture", ArgumentSemantic.Retain)]
		IMTLTexture DenoiseStrengthMaskTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("transparencyOverlayTexture", ArgumentSemantic.Retain)]
		IMTLTexture TransparencyOverlayTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("outputTexture", ArgumentSemantic.Retain)]
		IMTLTexture OutputTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("exposureTexture", ArgumentSemantic.Retain)]
		IMTLTexture ExposureTexture { get; set; }

		[Abstract]
		[Export ("preExposure")]
		float PreExposure { get; set; }

		[Abstract]
		[NullAllowed, Export ("reactiveMaskTexture", ArgumentSemantic.Retain)]
		IMTLTexture ReactiveMaskTexture { get; set; }

		[Abstract]
		[Export ("jitterOffsetX")]
		float JitterOffsetX { get; set; }

		[Abstract]
		[Export ("jitterOffsetY")]
		float JitterOffsetY { get; set; }

		[Abstract]
		[Export ("motionVectorScaleX")]
		float MotionVectorScaleX { get; set; }

		[Abstract]
		[Export ("motionVectorScaleY")]
		float MotionVectorScaleY { get; set; }

		[Abstract]
		[Export ("shouldResetHistory")]
		bool ShouldResetHistory { get; set; }

		[Abstract]
		[Export ("depthReversed")]
		bool DepthReversed { [Bind ("isDepthReversed")] get; set; }

		[Abstract]
		[Export ("colorTextureFormat")]
		MTLPixelFormat ColorTextureFormat { get; }

		[Abstract]
		[Export ("depthTextureFormat")]
		MTLPixelFormat DepthTextureFormat { get; }

		[Abstract]
		[Export ("motionTextureFormat")]
		MTLPixelFormat MotionTextureFormat { get; }

		[Abstract]
		[Export ("diffuseAlbedoTextureFormat")]
		MTLPixelFormat DiffuseAlbedoTextureFormat { get; }

		[Abstract]
		[Export ("specularAlbedoTextureFormat")]
		MTLPixelFormat SpecularAlbedoTextureFormat { get; }

		[Abstract]
		[Export ("normalTextureFormat")]
		MTLPixelFormat NormalTextureFormat { get; }

		[Abstract]
		[Export ("roughnessTextureFormat")]
		MTLPixelFormat RoughnessTextureFormat { get; }

		[Abstract]
		[Export ("specularHitDistanceTextureFormat")]
		MTLPixelFormat SpecularHitDistanceTextureFormat { get; }

		[Abstract]
		[Export ("denoiseStrengthMaskTextureFormat")]
		MTLPixelFormat DenoiseStrengthMaskTextureFormat { get; }

		[Abstract]
		[Export ("transparencyOverlayTextureFormat")]
		MTLPixelFormat TransparencyOverlayTextureFormat { get; }

		[Abstract]
		[Export ("reactiveMaskTextureFormat")]
		MTLPixelFormat ReactiveMaskTextureFormat { get; }

		[Abstract]
		[Export ("outputTextureFormat")]
		MTLPixelFormat OutputTextureFormat { get; }

		[Abstract]
		[Export ("inputWidth")]
		nuint InputWidth { get; }

		[Abstract]
		[Export ("inputHeight")]
		nuint InputHeight { get; }

		[Abstract]
		[Export ("outputWidth")]
		nuint OutputWidth { get; }

		[Abstract]
		[Export ("outputHeight")]
		nuint OutputHeight { get; }

		[Abstract]
		[Export ("inputContentMinScale")]
		float InputContentMinScale { get; }

		[Abstract]
		[Export ("inputContentMaxScale")]
		float InputContentMaxScale { get; }

		[Abstract]
		[Export ("worldToViewMatrix", ArgumentSemantic.Assign)]
		/* simd_float4x4 */
		NMatrix4 WorldToViewMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		[Abstract]
		[Export ("viewToClipMatrix", ArgumentSemantic.Assign)]
		/* simd_float4x4 */
		NMatrix4 ViewToClipMatrix {
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			get;
			[MarshalDirective (NativePrefix = "xamarin_simd__", Library = "__Internal")]
			set;
		}

		[Abstract]
		[NullAllowed, Export ("fence", ArgumentSemantic.Retain)]
		IMTLFence Fence { get; set; }
	}

	interface IMTLFXTemporalDenoisedScaler { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLFXTemporalDenoisedScaler : MTLFXTemporalDenoisedScalerBase {
		[Abstract]
		[Export ("encodeToCommandBuffer:")]
		void Encode (IMTLCommandBuffer commandBuffer);
	}

	interface IMTLFXTemporalScalerBase { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLFXTemporalScalerBase : MTLFXFrameInterpolatableScaler {
		[Abstract]
		[Export ("colorTextureUsage")]
		MTLTextureUsage ColorTextureUsage { get; }

		[Abstract]
		[Export ("depthTextureUsage")]
		MTLTextureUsage DepthTextureUsage { get; }

		[Abstract]
		[Export ("motionTextureUsage")]
		MTLTextureUsage MotionTextureUsage { get; }

		[Abstract]
		[Export ("reactiveTextureUsage")]
		MTLTextureUsage ReactiveTextureUsage { get; }

		[Abstract]
		[Export ("outputTextureUsage")]
		MTLTextureUsage OutputTextureUsage { get; }

		[Abstract]
		[Export ("inputContentWidth")]
		nuint InputContentWidth { get; set; }

		[Abstract]
		[Export ("inputContentHeight")]
		nuint InputContentHeight { get; set; }

		[Abstract]
		[NullAllowed, Export ("colorTexture", ArgumentSemantic.Retain)]
		IMTLTexture ColorTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("depthTexture", ArgumentSemantic.Retain)]
		IMTLTexture DepthTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("motionTexture", ArgumentSemantic.Retain)]
		IMTLTexture MotionTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("outputTexture", ArgumentSemantic.Retain)]
		IMTLTexture OutputTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("exposureTexture", ArgumentSemantic.Retain)]
		IMTLTexture ExposureTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("reactiveMaskTexture", ArgumentSemantic.Retain)]
		IMTLTexture ReactiveMaskTexture { get; set; }

		[Abstract]
		[Export ("preExposure")]
		float PreExposure { get; set; }

		[Abstract]
		[Export ("jitterOffsetX")]
		float JitterOffsetX { get; set; }

		[Abstract]
		[Export ("jitterOffsetY")]
		float JitterOffsetY { get; set; }

		[Abstract]
		[Export ("motionVectorScaleX")]
		float MotionVectorScaleX { get; set; }

		[Abstract]
		[Export ("motionVectorScaleY")]
		float MotionVectorScaleY { get; set; }

		[Abstract]
		[Export ("reset")]
		bool Reset { get; set; }

		[Abstract]
		[Export ("depthReversed")]
		bool DepthReversed { [Bind ("isDepthReversed")] get; set; }

		[Abstract]
		[Export ("colorTextureFormat")]
		MTLPixelFormat ColorTextureFormat { get; }

		[Abstract]
		[Export ("depthTextureFormat")]
		MTLPixelFormat DepthTextureFormat { get; }

		[Abstract]
		[Export ("motionTextureFormat")]
		MTLPixelFormat MotionTextureFormat { get; }

		[Abstract]
		[Export ("reactiveMaskTextureFormat")]
		MTLPixelFormat ReactiveMaskTextureFormat { get; }

		[Abstract]
		[Export ("outputTextureFormat")]
		MTLPixelFormat OutputTextureFormat { get; }

		[Abstract]
		[Export ("inputWidth")]
		nuint InputWidth { get; }

		[Abstract]
		[Export ("inputHeight")]
		nuint InputHeight { get; }

		[Abstract]
		[Export ("outputWidth")]
		nuint OutputWidth { get; }

		[Abstract]
		[Export ("outputHeight")]
		nuint OutputHeight { get; }

		[Abstract]
		[Export ("inputContentMinScale")]
		float InputContentMinScale { get; }

		[Abstract]
		[Export ("inputContentMaxScale")]
		float InputContentMaxScale { get; }

		[Abstract]
		[NullAllowed, Export ("fence", ArgumentSemantic.Retain)]
		IMTLFence Fence { get; set; }
	}

	interface IMTLFXFrameInterpolatableScaler { }

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLFXFrameInterpolatableScaler {
	}

	[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLFXSpatialScalerBase {
		[Abstract]
		[Export ("colorTextureUsage")]
		MTLTextureUsage ColorTextureUsage { get; }

		[Abstract]
		[Export ("outputTextureUsage")]
		MTLTextureUsage OutputTextureUsage { get; }

		[Abstract]
		[Export ("inputContentWidth")]
		nuint InputContentWidth { get; set; }

		[Abstract]
		[Export ("inputContentHeight")]
		nuint InputContentHeight { get; set; }

		[Abstract]
		[NullAllowed, Export ("colorTexture", ArgumentSemantic.Retain)]
		IMTLTexture ColorTexture { get; set; }

		[Abstract]
		[NullAllowed, Export ("outputTexture", ArgumentSemantic.Retain)]
		IMTLTexture OutputTexture { get; set; }

		[Abstract]
		[Export ("colorTextureFormat")]
		MTLPixelFormat ColorTextureFormat { get; }

		[Abstract]
		[Export ("outputTextureFormat")]
		MTLPixelFormat OutputTextureFormat { get; }

		[Abstract]
		[Export ("inputWidth")]
		nuint InputWidth { get; }

		[Abstract]
		[Export ("inputHeight")]
		nuint InputHeight { get; }

		[Abstract]
		[Export ("outputWidth")]
		nuint OutputWidth { get; }

		[Abstract]
		[Export ("outputHeight")]
		nuint OutputHeight { get; }

		[Abstract]
		[Export ("colorProcessingMode")]
		MTLFXSpatialScalerColorProcessingMode ColorProcessingMode { get; }

		[Abstract]
		[NullAllowed, Export ("fence", ArgumentSemantic.Retain)]
		IMTLFence Fence { get; set; }
	}
}
