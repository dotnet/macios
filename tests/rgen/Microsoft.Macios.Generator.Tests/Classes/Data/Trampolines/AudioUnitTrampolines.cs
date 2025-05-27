// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using AudioUnit;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class AudioUnitTrampolines {
	
	[Export<Property> ("auHostTransportStateBlock", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUHostTransportStateBlock AUHostTransportStateBlock { get; set; }

	[Export<Property> ("auImplementorDisplayNameWithLengthCallback", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUImplementorDisplayNameWithLengthCallback AUImplementorDisplayNameWithLengthCallback { get; set; }

	[Export<Property> ("auImplementorStringFromValueCallback", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUImplementorStringFromValueCallback AUImplementorStringFromValueCallback { get; set; }

	[Export<Property> ("auImplementorValueFromStringCallback", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUImplementorValueFromStringCallback AUImplementorValueFromStringCallback { get; set; }

	[Export<Property> ("auImplementorValueObserver", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUImplementorValueObserver AUImplementorValueObserver { get; set; }

	[Export<Property> ("auImplementorValueProvider", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUImplementorValueProvider AUImplementorValueProvider { get; set; }

	[Export<Property> ("auInputHandler", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUInputHandler AUInputHandler { get; set; }

	[Export<Property> ("auInternalRenderBlock", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUInternalRenderBlock AUInternalRenderBlock { get; set; }

	[Export<Property> ("auMidiCIProfileChangedCallback", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUMidiCIProfileChangedCallback AUMidiCIProfileChangedCallback { get; set; }

	[Export<Property> ("auMidiOutputEventBlock", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUMidiOutputEventBlock AUMidiOutputEventBlock { get; set; }

	[Export<Property> ("auParameterAutomationObserver", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUParameterAutomationObserver AUParameterAutomationObserver { get; set; }

	[Export<Property> ("auParameterObserver", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUParameterObserver AUParameterObserver { get; set; }

	[Export<Property> ("auParameterRecordingObserver", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUParameterRecordingObserver AUParameterRecordingObserver { get; set; }

	[Export<Property> ("auRenderBlock", ArgumentSemantic.Copy)]
	public partial AudioUnit.AURenderBlock AURenderBlock { get; set; }

	[Export<Property> ("auRenderPullInputBlock", ArgumentSemantic.Copy)]
	public partial AudioUnit.AURenderPullInputBlock AURenderPullInputBlock { get; set; }

	[Export<Property> ("auScheduleParameterBlock", ArgumentSemantic.Copy)]
	public partial AudioUnit.AUScheduleParameterBlock AUScheduleParameterBlock { get; set; }
}
