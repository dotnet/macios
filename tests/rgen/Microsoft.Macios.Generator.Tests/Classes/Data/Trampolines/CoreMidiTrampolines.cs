// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using CoreMidi;
using Foundation;
using ObjCBindings;
using ObjCRuntime;

namespace TestNamespace;

[BindingType<Class>]
public class CoreMidiTrampolines {

	[Export<Property> ("midiCIDiscoveryResponseDelegate", ArgumentSemantic.Copy)]
	public partial CoreMidi.MidiCIDiscoveryResponseDelegate MidiCIDiscoveryResponseDelegate { get; set; }

	[Export<Property> ("midiCIProfileChangedHandler", ArgumentSemantic.Copy)]
	public partial CoreMidi.MidiCIProfileChangedHandler MidiCIProfileChangedHandler { get; set; }

	[Export<Property> ("midiCIProfileSpecificDataHandler", ArgumentSemantic.Copy)]
	public partial CoreMidi.MidiCIProfileSpecificDataHandler MidiCIProfileSpecificDataHandler { get; set; }

	[Export<Property> ("midiCISessionDisconnectHandler", ArgumentSemantic.Copy)]
	public partial CoreMidi.MidiCISessionDisconnectHandler MidiCISessionDisconnectHandler { get; set; }

	[Export<Property> ("midiReceiveBlock", ArgumentSemantic.Copy)]
	public partial CoreMidi.MidiReceiveBlock MidiReceiveBlock { get; set; }
}
