// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CoreML;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CoreMotionTrampolines {
	
	[Export<Property> ("cmAccelerometerHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMAccelerometerHandler CMAccelerometerHandler { get; set; }

	[Export<Property> ("cmDeviceMotionHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMDeviceMotionHandler CMDeviceMotionHandler { get; set; }

	[Export<Property> ("cmGyroHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMGyroHandler CMGyroHandler { get; set; }

	[Export<Property> ("cmHeadphoneActivityHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMHeadphoneActivityHandler CMHeadphoneActivityHandler { get; set; }

	[Export<Property> ("cmHeadphoneActivityStatusHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMHeadphoneActivityStatusHandler CMHeadphoneActivityStatusHandler { get; set; }

	[Export<Property> ("cmHeadphoneDeviceMotionHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMHeadphoneDeviceMotionHandler CMHeadphoneDeviceMotionHandler { get; set; }

	[Export<Property> ("cmMagnetometerHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMMagnetometerHandler CMMagnetometerHandler { get; set; }

	[Export<Property> ("cmMotionActivityHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMMotionActivityHandler CMMotionActivityHandler { get; set; }

	[Export<Property> ("cmMotionActivityQueryHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMMotionActivityQueryHandler CMMotionActivityQueryHandler { get; set; }

	[Export<Property> ("cmStepQueryHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMStepQueryHandler CMStepQueryHandler { get; set; }

	[Export<Property> ("cmStepUpdateHandler", ArgumentSemantic.Copy)]
	public partial CoreMotion.CMStepUpdateHandler CMStepUpdateHandler { get; set; }
}
