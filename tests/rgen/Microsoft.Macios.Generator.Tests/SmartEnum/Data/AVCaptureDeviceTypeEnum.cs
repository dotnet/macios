using System;
using Foundation;
using ObjCBindings;

namespace AVFoundation;

[BindingType<SmartEnum>]
public enum AVCaptureDeviceType {

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInMicrophone")]
	BuiltInMicrophone,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInWideAngleCamera")]
	BuiltInWideAngleCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInTelephotoCamera")]
	BuiltInTelephotoCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInDuoCamera")]
	BuiltInDuoCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInDualCamera")]
	BuiltInDualCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInTrueDepthCamera")]
	BuiltInTrueDepthCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInUltraWideCamera")]
	BuiltInUltraWideCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInTripleCamera")]
	BuiltInTripleCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInDualWideCamera")]
	BuiltInDualWideCamera,

	[Field<EnumValue> ("AVCaptureDeviceTypeExternalUnknown")]
	ExternalUnknown,

	[Field<EnumValue> ("AVCaptureDeviceTypeBuiltInLiDARDepthCamera")]
	BuiltInLiDarDepthCamera,
}
