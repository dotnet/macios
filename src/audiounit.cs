//
// Authors:
//  Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2015 Xamarin, Inc.
//
//

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using AudioUnit;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using CoreGraphics;
#if IOS || MONOMAC
using CoreAudioKit;
using CoreMidi;
#endif
using AudioToolbox;
using AVFoundation;

#if MONOMAC
using AppKit;
using AUViewControllerBase = AppKit.NSViewController;
#else
using UIKit;
using MediaToolbox;
using AUViewControllerBase = UIKit.UIViewController;
#endif
#if TVOS
using MidiCIProfile = Foundation.NSObject;
using MidiCIProfileState = Foundation.NSObject;
#endif

namespace AudioUnit {
	delegate AudioUnitStatus AUInternalRenderBlock (ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timestamp, uint frameCount, nint outputBusNumber, AudioBuffers outputData, AURenderEventEnumerator realtimeEventListHead, [BlockCallback][NullAllowed] AURenderPullInputBlock pullInputBlock);
	delegate AudioUnitStatus AURenderBlock (ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timestamp, uint frameCount, nint outputBusNumber, AudioBuffers outputData, [BlockCallback][NullAllowed] AURenderPullInputBlock pullInputBlock);

	internal delegate AudioUnitStatus AURenderPullInputBlock (ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timestamp,
			uint frameCount, nint inputBusNumber, AudioBuffers inputData);

	delegate void AUScheduleParameterBlock (AUEventSampleTime eventSampleTime, uint rampDurationSampleFrames, ulong parameterAddress, float value);
	[MacCatalyst (13, 1)]
	delegate int AUMidiOutputEventBlock (long eventSampleTime, byte cable, nint length, IntPtr midiBytes);
	/// <param name="param">The parameter that changed.</param>
	///     <param name="value">The new parameter value.</param>
	///     <summary>A delegate that notifies the audio unit when a parameter value changes.</summary>
	delegate void AUImplementorValueObserver (AUParameter param, float value);
	/// <param name="param">The parameter whose value to get.</param>
	///     <summary>A delegate that retrieves a parameter value.</summary>
	///     <returns>The value of the parameter that is identified by <paramref name="param" />.</returns>
	delegate float AUImplementorValueProvider (AUParameter param);

	/// <param name="address">The parameter address.</param>
	///     <param name="value">The current parameter value.</param>
	///     <summary>Observer that notifies an audio unit when a parameter value changes.</summary>
	delegate void AUParameterObserver (ulong address, float value);

	delegate void AUVoiceIOMutedSpeechActivityEventListener (AUVoiceIOSpeechActivityEvent activityEvent);

	// 	AUAudioTODO - We need testing for these bindings
	// 	delegate void AUScheduleMidiEventBlock (AUEventSampleTime eventSampleTime, byte cable, nint length, ref byte midiBytes);
	// 	delegate bool AUHostMusicalContextBlock (ref double currentTempo, ref double timeSignatureNumerator, ref nint timeSignatureDenominator, ref double currentBeatPosition, ref nint sampleOffsetToNextBeat, ref double currentMeasureDownbeatPosition);
	delegate string AUImplementorStringFromValueCallback (AUParameter param, ref float? value);

	/// <param name="node">The parameter node for which to get a possibly shortened name.</param>
	///     <param name="desiredLength">The maximum desired length of the display name.</param>
	///     <summary>A delegate that returns the display name, possibly shortened to <paramref name="desiredLength" /> characters.</summary>
	///     <returns>The display name, possibly shortened to <paramref name="desiredLength" /> characters.</returns>
	delegate string AUImplementorDisplayNameWithLengthCallback (AUParameterNode node, nint desiredLength);
	/// <param name="numberOfEvents">The number of automation events.</param>
	///     <param name="events">The delivered events.</param>
	///     <summary>Delegate that records parameter changes as automation events.</summary>
	delegate void AUParameterRecordingObserver (nint numberOfEvents, ref AURecordedParameterEvent events);
	/// <param name="actionFlags">The action flags that configure the audio unit rendering process.</param>
	///     <param name="timestamp">The unconverted, uncompressed HAL time when the input will render.</param>
	///     <param name="frameCount">The number of available audio frames.</param>
	///     <param name="inputBusNumber">The input bus index.</param>
	///     <summary>Delegate that tells an I/O host when input is available.</summary>
	delegate void AUInputHandler (ref AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timestamp, uint frameCount, nint inputBusNumber);
	/// <param name="transportStateFlags">The state of the audio transport.</param>
	///     <param name="currentSamplePosition">The host sample position, in audio unit samples.</param>
	///     <param name="cycleStartBeatPosition">The starting beat position for the cycle. <see langword="null" /> if not cycling.</param>
	///     <param name="cycleEndBeatPosition">The ending beat position for the cycle. <see langword="null" /> if not cycling.</param>
	///     <summary>A delegate block that a host uses to provide information about its transport state.</summary>
	///     <returns>
	///       <para>
	///         <see langword="true" /> if the state was successfully retrieved. Otherwise, <see langword="false" />.</para>
	///     </returns>
	///     <remarks>
	///       <para>Developers may optionally assign an instance of this class to the <see cref="AudioUnit.AUAudioUnit.TransportStateBlock" /> property so that they can call it at the beginning of render cycles to get the transport state at the cycle start.</para>
	///     </remarks>
	delegate bool AUHostTransportStateBlock (ref AUHostTransportStateFlags transportStateFlags, ref double currentSamplePosition, ref double cycleStartBeatPosition, ref double cycleEndBeatPosition);
	delegate void AURenderObserver (AudioUnitRenderActionFlags actionFlags, ref AudioTimeStamp timestamp, uint frameCount, nint outputBusNumber);
	/// <param name="param">The parameter that will be assigned to the value that is converted from <paramref name="str" />.</param>
	///     <param name="str">The string to convert.</param>
	///     <summary>Converts <paramref name="str" /> to the appropriate type and assigns it to <paramref name="param" />.</summary>
	///     <returns>The new audio unit value.</returns>
	delegate float AUImplementorValueFromStringCallback (AUParameter param, string str);
	[NoTV]
	[MacCatalyst (13, 1)]
	delegate void AUMidiCIProfileChangedCallback (byte cable, byte channel, MidiCIProfile profile, bool enabled);

	/// <summary>A subclass of <see cref="AVFoundation.AVAudioNode" /> whose subclasses process audio.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUAudioUnit_ClassReference/index.html">Apple documentation for <c>AUAudioUnit</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AUAudioUnit {
		/// <param name="cls">To be added.</param>
		///         <param name="componentDescription">To be added.</param>
		///         <param name="name">To be added.</param>
		///         <param name="version">To be added.</param>
		///         <summary>Registers a component that has been implemented as a subclass of <see cref="AudioUnit.AudioUnit" />.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("registerSubclass:asComponentDescription:name:version:")] // AUAudioUnitImplementation
		void RegisterSubclass (Class cls, AudioComponentDescription componentDescription, string name, uint version);

		/// <param name="componentDescription">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <param name="outError">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new <see cref="AudioUnit.AUAudioUnit" /> with the specified values.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithComponentDescription:options:error:")]
		[DesignatedInitializer]
		NativeHandle Constructor (AudioComponentDescription componentDescription, AudioComponentInstantiationOptions options, [NullAllowed] out NSError outError);

		/// <param name="componentDescription">To be added.</param>
		/// <param name="outError">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new <see cref="AudioUnit.AUAudioUnit" /> with the specified values.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithComponentDescription:error:")]
		NativeHandle Constructor (AudioComponentDescription componentDescription, [NullAllowed] out NSError outError);

		/// <param name="componentDescription">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>Asynchronously creates an <see cref="AUAudioUnit" />.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("instantiateWithComponentDescription:options:completionHandler:")]
		[Async (XmlDocs = """
			<param name="componentDescription">To be added.</param>
			<param name="options">To be added.</param>
			<summary>Asynchronously creates an <see cref="AUAudioUnit" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous FromComponentDescription operation.  The value of the TResult parameter is of type System.Action&lt;AudioUnit.AUAudioUnit,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void FromComponentDescription (AudioComponentDescription componentDescription, AudioComponentInstantiationOptions options, Action<AUAudioUnit, NSError> completionHandler);

		/// <summary>Gets the component from the description with which the audio unit was created.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("componentDescription")]
		AudioComponentDescription ComponentDescription { get; }

		/// <summary>Gets the render block that is used for rendering audio.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("renderBlock")]
		AURenderBlock RenderBlock { get; }

		/// <summary>Gets the developer-provided block that performs audio rendering.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("internalRenderBlock")]
		AUInternalRenderBlock InternalRenderBlock { get; }

		// @property (readonly, nonatomic) AudioComponent __nonnull component;
		/// <summary>Gets the component from the description with which the audio unit was created.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("component")]
		unsafe AudioComponent Component { get; }

		/// <summary>Gets the audio units component name.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("componentName")]
		string ComponentName { get; }

		/// <summary>Gets the name of the audio unit.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("audioUnitName")]
		string AudioUnitName { get; }

		/// <summary>Gets the manufacturer name for the audio unit.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("manufacturerName")]
		string ManufacturerName { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("audioUnitShortName")]
		string ShortName { get; }

		/// <summary>Gets the version of the audio unit.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("componentVersion")]
		uint ComponentVersion { get; }

		/// <param name="outError">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Allocates the resources that are needed to render audio.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("allocateRenderResourcesAndReturnError:")]
		bool AllocateRenderResources ([NullAllowed] out NSError outError);

		/// <summary>Deallocates the resources that are needed to render audio.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("deallocateRenderResources")]
		void DeallocateRenderResources ();

		/// <summary>Gets a Boolean value that tells whether rendering resources have been allocated.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("renderResourcesAllocated")]
		bool RenderResourcesAllocated { get; }

		/// <summary>Resets rendering to its initial state.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reset")]
		void Reset ();

		/// <summary>Gets an array that contains the input connections for the audio unit.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inputBusses")]
		AUAudioUnitBusArray InputBusses { get; }

		/// <summary>Gets an array that contains the output connections for the audio unit.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("outputBusses")]
		AUAudioUnitBusArray OutputBusses { get; }

		/// <summary>Block that schedules parameters.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scheduleParameterBlock")]
		AUScheduleParameterBlock ScheduleParameterBlock { get; }

		// TODO: https://github.com/dotnet/macios/issues/12489
		// [TV (15,0), iOS (15,0), MacCatalyst (15,0)]
		// [NullAllowed]
		// [Export ("scheduleMIDIEventListBlock")]
		// AUMidiEventListBlock ScheduleMidiEventListBlock { get; }

		// 		[Export ("tokenByAddingRenderObserver:")]
		// 		nint GetToken (AURenderObserver observer);

		// 		[NullAllowed, Export ("scheduleMIDIEventBlock")]
		// 		AUScheduleMidiEventBlock ScheduleMidiEventBlock { get; }

		// 		[NullAllowed, Export ("musicalContextBlock", ArgumentSemantic.Copy)]
		// 		AUHostMusicalContextBlock MusicalContextBlock { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("MIDIOutputNames", ArgumentSemantic.Copy)]
		string [] MidiOutputNames { get; }

		// TODO: https://github.com/dotnet/macios/issues/12489
		// [TV (15,0), iOS (15,0), MacCatalyst (15,0)]
		// [NullAllowed]
		// [Export ("MIDIOutputEventListBlock", ArgumentSemantic.Copy)]
		// AUMidiEventListBlock MidiOutputEventListBlock { get; set; }

		// TODO: https://github.com/dotnet/macios/issues/12489
		// [TV (15,0), iOS (15,0), MacCatalyst (15,0)]
		// [Export ("AudioUnitMIDIProtocol")]
		// MIDIProtocolID AudioUnitMidiProtocol { get; }

		// TODO: https://github.com/dotnet/macios/issues/12489
		// [TV (15,0), iOS (15,0), MacCatalyst (15,0)]
		// [Export ("hostMIDIProtocol", ArgumentSemantic.Assign)]
		// MIDIProtocolID HostMIDIProtocol { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("providesUserInterface")]
		bool ProvidesUserInterface { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("MIDIOutputEventBlock", ArgumentSemantic.Copy)]
		AUMidiOutputEventBlock MidiOutputEventBlock { get; set; }

		/// <summary>A delegate block that provides information about its transport state.</summary>
		///         <value>
		///           <para>Developers may optionally assign an instance of the <see cref="AudioUnit.AUAudioUnit.TransportStateBlock" /> delegate to thi property so that they can call it at the beginning of render cycles to get the transport state at the cycle start.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("transportStateBlock", ArgumentSemantic.Copy)]
		AUHostTransportStateBlock TransportStateBlock { get; set; }

		/// <param name="token">To be added.</param>
		/// <summary>Removes the observer block that is identified by <paramref name="token" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("removeRenderObserver:")]
		void RemoveRenderObserver (nint token);

		/// <summary>Gets or sets the maximum number of audio frames to render simultaneously.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumFramesToRender")]
		uint MaximumFramesToRender { get; set; }

		/// <summary>Gets or sets the entire tree of output parameters.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("parameterTree")]

		AUParameterTree ParameterTree {
			get;
			[TV (13, 0), iOS (13, 0)]
			[MacCatalyst (13, 1)]
			set;
		}

		/// <param name="count">To be added.</param>
		/// <summary>Returns the <paramref name="count" /> most important parameters.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("parametersForOverviewWithCount:")]
		NSNumber [] GetParametersForOverview (nint count);

		/// <summary>Gets a special value that is required for key-value observing notifications</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allParameterValues")]
		bool AllParameterValues { get; }

		/// <summary>Gets a value that is <see langword="true" /> if the audio unit is either a music device or an effect.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("musicDeviceOrEffect")]
		bool MusicDeviceOrEffect { [Bind ("isMusicDeviceOrEffect")] get; }

		/// <summary>Gets the number of virtual MIDI cables that are implemented by the device or effect.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("virtualMIDICableCount")]
		nint VirtualMidiCableCount { get; }

		// @property (copy, nonatomic) NSDictionary<NSString * __nonnull,id __nonnull> * __nullable fullState;
		/// <summary>Gets or sets the snapshot of the audio unit's entire state, for saving presets.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("fullState", ArgumentSemantic.Copy)]
		NSDictionary FullState { get; set; }

		// @property (copy, nonatomic) NSDictionary<NSString * __nonnull,id __nonnull> * __nullable fullStateForDocument;
		/// <summary>Gets or sets the snapshot of the audio unit's entire state, for saving in a document.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("fullStateForDocument", ArgumentSemantic.Copy)]
		NSDictionary FullStateForDocument { get; set; }

		/// <summary>Gets the factory presets for the audio unit.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("factoryPresets", ArgumentSemantic.Copy)]
		AUAudioUnitPreset [] FactoryPresets { get; }

		/// <summary>Gets the most recently selected preset.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("currentPreset", ArgumentSemantic.Retain)]
		AUAudioUnitPreset CurrentPreset { get; set; }

		/// <summary>Gets the latency of the audio unit, in seconds</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("latency")]
		double Latency { get; }

		/// <summary>Gets the tail time, in seconds, of the audio unit.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("tailTime")]
		double TailTime { get; }

		/// <summary>Gets or sets the rendering quality, a value in [0,127].</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("renderQuality", ArgumentSemantic.Assign)]
		nint RenderQuality { get; set; }

		/// <summary>Gets or sets a Boolean value that contols whether an effect should skip processing and go directly to the output.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldBypassEffect")]
		bool ShouldBypassEffect { get; set; }

		/// <summary>Gets a Boolean value that tells whether the audio unit can transform input to output without allocating a new buffer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("canProcessInPlace")]
		bool CanProcessInPlace { get; }

		/// <summary>Gets or sets a Boolean value that controls whether the audio unit is currently rendering offline.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("renderingOffline")]
		bool RenderingOffline { [Bind ("isRenderingOffline")] get; set; }

		/// <summary>Gets an array that describes valid combinations of input and output channels.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("channelCapabilities", ArgumentSemantic.Copy)]
		NSNumber [] ChannelCapabilities { get; }

		/// <summary>Gets the name of the context to which the audio unit is connected.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("contextName")]
		string ContextName { get; set; }

		/// <summary>Gets a Boolean values that tells whether MPE audio is supported.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("supportsMPE")]
		bool SupportsMpe { get; }

		/// <summary>Gets or sets the channel map.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("channelMap"), NullAllowed]
		NSNumber [] ChannelMap { get; set; }

		/// <param name="completionHandler">To be added.</param>
		///         <summary>Requests the view controller for the audio unit and runs <paramref name="completionHandler" /> when finished.</summary>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("requestViewControllerWithCompletionHandler:")]
		[Async (XmlDocs = """
			<summary>Asynchronously requests the view controller for the audio unit.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous RequestViewController operation.  The result is of type System.Threading.Tasks.Task&lt;AppKit.NSViewController&gt; on MacOS and System.Threading.Tasks.Task&lt;AppKit.UIViewController&gt; on iOS.</para>
			        </returns>
			<remarks>
			          <para copied="true">The RequestViewControllerAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			        </remarks>
			""")]
		void RequestViewController (Action<AUViewControllerBase> completionHandler);

		// AUAudioUnitImplementation
		/// <param name="flag">To be added.</param>
		///         <summary>Sets the <see cref="AudioUnit.AUAudioUnit.RenderResourcesAllocated" /> property.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setRenderResourcesAllocated:")]
		void SetRenderResourcesAllocated (bool flag);

		/// <param name="format">To be added.</param>
		///         <param name="bus">To be added.</param>
		///         <summary>Method that is called when the developer sets the bus format.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("shouldChangeToFormat:forBus:")]
		bool ShouldChangeToFormat (AVAudioFormat format, AUAudioUnitBus bus);

		/// <include file="../docs/api/AudioUnit/AUAudioUnit.xml" path="/Documentation/Docs[@DocId='P:AudioUnit.AUAudioUnit.AudioComponentRegistrationsChangedNotification']/*" />
		[Notification, Field ("kAudioComponentRegistrationsChangedNotification")]
		NSString AudioComponentRegistrationsChangedNotification { get; }

		/// <include file="../docs/api/AudioUnit/AUAudioUnit.xml" path="/Documentation/Docs[@DocId='P:AudioUnit.AUAudioUnit.AudioComponentInstanceInvalidationNotification']/*" />
		[Notification, Field ("kAudioComponentInstanceInvalidationNotification")]
		NSString AudioComponentInstanceInvalidationNotification { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("MIDIOutputBufferSizeHint")]
		nint MidiOutputBufferSizeHint { get; set; }

		/// <param name="cable">To be added.</param>
		///         <param name="channel">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("profileStateForCable:channel:")]
		MidiCIProfileState GetProfileState (byte cable, byte channel);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("profileChangedBlock", ArgumentSemantic.Assign)]
		AUMidiCIProfileChangedCallback ProfileChangedCallback { get; set; }

		/// <param name="profile">To be added.</param>
		///         <param name="cable">To be added.</param>
		///         <param name="channel">To be added.</param>
		///         <param name="outError">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("disableProfile:cable:onChannel:error:")]
		bool Disable (MidiCIProfile profile, byte cable, byte channel, [NullAllowed] out NSError outError);

		/// <param name="profile">To be added.</param>
		///         <param name="cable">To be added.</param>
		///         <param name="channel">To be added.</param>
		///         <param name="outError">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("enableProfile:cable:onChannel:error:")]
		bool Enable (MidiCIProfile profile, byte cable, byte channel, [NullAllowed] out NSError outError);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("userPresets", ArgumentSemantic.Copy)]
		AUAudioUnitPreset [] UserPresets { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("saveUserPreset:error:")]
		bool SaveUserPreset (AUAudioUnitPreset userPreset, [NullAllowed] out NSError outError);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("deleteUserPreset:error:")]
		bool DeleteUserPreset (AUAudioUnitPreset userPreset, [NullAllowed] out NSError outError);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("presetStateFor:error:")]
		[return: NullAllowed]
		NSDictionary<NSString, NSObject> GetPresetState (AUAudioUnitPreset userPreset, [NullAllowed] out NSError outError);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("supportsUserPresets")]
		bool SupportsUserPresets { get; }

		[Introduced (PlatformName.MacCatalyst, 13, 0)] // needed since it's not in iOS
		[NoTV, NoiOS]
		[Export ("isLoadedInProcess")]
		bool IsLoadedInProcess { get; }

		[TV (16, 0), Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("migrateFromPlugin")]
		NSData [] MigrateFromPlugin { get; }
	}

	// kept separate from AUAudioUnit, quote:
	// These methods will fail if the audio unit is not an input/output audio unit.
	/// <summary>Defines the interface of a host to an audio unit.</summary>
	[MacCatalyst (13, 1)]
	[Category]
	[BaseType (typeof (AUAudioUnit))]
	interface AUAudioUnit_AUAudioInputOutputUnit {

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[NoTV, NoiOS]
		[NoMacCatalyst]
		[Export ("deviceID")]
		uint GetDeviceId ();

		/// <param name="deviceID">To be added.</param>
		/// <param name="outError">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[NoTV, NoiOS]
		[NoMacCatalyst]
		[Export ("setDeviceID:error:")]
		bool SetDeviceId (uint deviceID, out NSError outError);

		/// <summary>Returns a Boolean value that tells whether the audio unit can perform input operations.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("canPerformInput")]
		bool GetCanPerformInput ();

		/// <summary>Returns a Boolean value that tells whether the audio unit can perform output operations.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("canPerformOutput")]
		bool CanPerformOutput ();

		/// <summary>Returns a Boolean value that tells whether input is currently enabled on the audio unit.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isInputEnabled")]
		bool IsInputEnabled ();

		/// <param name="enabled">To be added.</param>
		/// <summary>Sets a Boolean value that controls whether input is enabled on the audio unit.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("setInputEnabled:")]
		bool SetInputEnabled (bool enabled);

		/// <summary>Returns a Boolean value that tells whether input is currently enabled on the audio unit.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isOutputEnabled")]
		bool IsOutputEnabled ();

		/// <param name="enabled">To be added.</param>
		/// <summary>Sets a Boolean value that controls whether output is enabled on the audio unit..</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("setOutputEnabled:")]
		bool SetOutputEnabled (bool enabled);

		/// <summary>Gets the input handler for this IO unit</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("inputHandler", ArgumentSemantic.Copy)]
		AUInputHandler GetInputHandler ();

		/// <param name="handler">The handler to set.</param>
		/// <summary>Sets the input handler to the specified value.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setInputHandler:")]
		void SetInputHandler ([NullAllowed] AUInputHandler handler);

		/// <param name="outError">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Starts the audio unit's hardware.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("startHardwareAndReturnError:")]
		bool StartHardware ([NullAllowed] out NSError outError);

		/// <summary>Stops the audio unit's hardware.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("stopHardware")]
		void StopHardware ();

		/// <summary>Gets the output provider for this IO unit.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("outputProvider", ArgumentSemantic.Copy)]
		AURenderPullInputBlock GetOutputProvider ();

		/// <param name="provider">The provider to set.</param>
		/// <summary>Sets the output provider to the specified value.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setOutputProvider:")]
		void SetOutputProvider ([NullAllowed] AURenderPullInputBlock provider);

		// the following are properties but we cannot have properties in Categories.
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("deviceInputLatency")]
		double GetDeviceInputLatency ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("deviceOutputLatency")]
		double GetDeviceOutputLatency ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("running")]
		bool IsRunning ();
	}

	/// <summary>An input or output connection to an audio unit.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUAudioUnitBus_ClassReference/index.html">Apple documentation for <c>AUAudioUnitBus</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface AUAudioUnitBus {
		/// <param name="format">A value that provides a detailed description of the channels and formats of audio data.</param>
		/// <param name="outError">
		///           <para>A location to which to record success or failure.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new <see cref="AudioUnit.AUAudioUnitBus" /> with the specified <paramref name="format" /> and reports success or failure to <paramref name="outError" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFormat:error:")]
		NativeHandle Constructor (AVAudioFormat format, [NullAllowed] out NSError outError);

		/// <summary>Gets a value that provides a detailed description of the channels and formats of audio data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("format")]
		AVAudioFormat Format { get; }

		/// <param name="format">A description of the audio format for the bus.</param>
		///         <param name="outError">
		///           <para>A location to which to record success or failure.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Sets configuration details about the supported channels and formats of audio data on this bus.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("setFormat:error:")]
		bool SetFormat (AVAudioFormat format, [NullAllowed] out NSError outError);

		/// <summary>Gets or sets a value that determines whether the bus is active.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <summary>Gets or sets the name for the bus.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("name")]
		string Name { get; set; }

		/// <summary>Gets the zero-based location of this audio bus in the array that contains it.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("index")]
		nuint Index { get; }

		/// <summary>Gets a value of type <see cref="AudioUnit.AUAudioUnitBusType" /> that tells whether this is an input or output connection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("busType")]
		AUAudioUnitBusType BusType { get; }

		/// <summary>Gets the audio unit that owns this bus.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ownerAudioUnit", ArgumentSemantic.Assign)]
		AUAudioUnit OwnerAudioUnit { get; }

		/// <summary>Gets an array of audio channel layout tags that are supported by this bus.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("supportedChannelLayoutTags", ArgumentSemantic.Copy)]
		NSNumber [] SupportedChannelLayoutTags { get; }

		/// <summary>Gets or sets the latency of the processing context for the audio unit.</summary>
		///         <value>To be added.</value>
		///         <remarks>For input units, this is the time, in seconds, since the arriving audio was acquired. For ouput units, this is the delay, in seconds, until the output is presented.</remarks>
		[Export ("contextPresentationLatency")]
		double ContextPresentationLatency { get; set; }

		// AUAudioUnitImplementation
		/// <summary>Gets or sets an array that contains the allowable channel counts for this bus. <see langword="null" /> indicates that any number less than <see cref="AudioUnit.AUAudioUnitBus.MaximumChannelCount" /> is supported.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("supportedChannelCounts", ArgumentSemantic.Retain)]
		NSNumber [] SupportedChannelCounts { get; set; }

		/// <summary>Gets or sets the maximum number of audio channels that the bus supports.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumChannelCount")]
		uint MaximumChannelCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shouldAllocateBuffer")]
		bool ShouldAllocateBuffer { get; set; }
	}

	/// <summary>A container that holds <see cref="AudioUnit.AUAudioUnitBus" /> objects for an audio unit.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUAudioUnitBusArray_ClassReference/index.html">Apple documentation for <c>AUAudioUnitBusArray</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AUAudioUnitBusArray : INSFastEnumeration {
		/// <param name="owner">The owner of the bus array.</param>
		/// <param name="busType">Whether the bus array will be for input or output.</param>
		/// <param name="busArray">The bus array whose members to copy.</param>
		/// <summary>Creates a new <see cref="AudioUnit.AUAudioUnitBusArray" />, with the specified owner ant type, by copying the buses in <paramref name="busArray" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithAudioUnit:busType:busses:")]
		[DesignatedInitializer]
		NativeHandle Constructor (AUAudioUnit owner, AUAudioUnitBusType busType, AUAudioUnitBus [] busArray);

		/// <param name="owner">The owner of the bus array.</param>
		/// <param name="busType">Whether the bus array will be for input or output.</param>
		/// <summary>Creates a new <see cref="AudioUnit.AUAudioUnitBusArray" /> with the specified owner ant type.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithAudioUnit:busType:")]
		NativeHandle Constructor (AUAudioUnit owner, AUAudioUnitBusType busType);

		/// <summary>Gets the number of buses in the array.</summary>
		///         <value>The number of buses in the array.</value>
		///         <remarks>To be added.</remarks>
		[Export ("count")]
		nuint Count { get; }

		// -(AUAudioUnitBus * __nonnull)objectAtIndexedSubscript:(NSUInteger)index;
		/// <param name="index">The zero-based index into the bus array of the desired bus.</param>
		/// <summary>Returns the bus at the spcified location in the array.</summary>
		/// <returns>The bus at the spcified location in the array.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("objectAtIndexedSubscript:")]
		AUAudioUnitBus GetObject (nuint index);

		/// <summary>Gets a Boolean value that tells whether buses can be added and removed from the array.</summary>
		///         <value>A Boolean value that tells whether buses can be added and removed from the array.</value>
		///         <remarks>To be added.</remarks>
		[Export ("countChangeable")]
		bool CountChangeable { [Bind ("isCountChangeable")] get; }

		/// <param name="count">To be added.</param>
		/// <param name="outError">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("setBusCount:error:")]
		bool SetBusCount (nuint count, [NullAllowed] out NSError outError);

		// -(void)addObserverToAllBusses:(NSObject * __nonnull)observer forKeyPath:(NSString * __nonnull)keyPath options:(NSKeyValueObservingOptions)options context:(void * __nullable)context;
		/// <param name="observer">The KVO observer to add to all buses in the array.</param>
		///         <param name="keyPath">The object-relative keypath that identifies the property to observe.</param>
		///         <param name="options">The observing options for the observer.</param>
		///         <param name="context">An object or value that is used to disambiguate observer calls.</param>
		///         <summary>Adds a key-value observer to every bus in the array.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addObserverToAllBusses:forKeyPath:options:context:")]
		void AddObserver (NSObject observer, string keyPath, NSKeyValueObservingOptions options, /* void * */ IntPtr context);

		// -(void)removeObserverFromAllBusses:(NSObject * __nonnull)observer forKeyPath:(NSString * __nonnull)keyPath context:(void * __nullable)context;
		/// <param name="observer">The KVO observer to remove from all buses in the array.</param>
		///         <param name="keyPath">The object-relative keypath that identifies the observer to remove.</param>
		///         <param name="context">The object or value that was used to disambiguate observer calls.</param>
		///         <summary>Removes the specified key-value observer from every bus in the array.</summary>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removeObserverFromAllBusses:forKeyPath:context:")]
		void RemoveObserver (NSObject observer, string keyPath, /* void * */ IntPtr context);

		/// <summary>Gets the audio unit that owns this array.</summary>
		///         <value>The audio unit that owns this array.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ownerAudioUnit", ArgumentSemantic.Assign)]
		AUAudioUnit OwnerAudioUnit { get; }

		/// <summary>Gets a value that tells whether this is an input or output bus array.</summary>
		///         <value>A value that tells whether this is an input or output bus array.</value>
		///         <remarks>To be added.</remarks>
		[Export ("busType")]
		AUAudioUnitBusType BusType { get; }

		//AUAudioUnitBusImplementation
		/// <param name="busArray">An array of buses to copy into this bus array.</param>
		///         <summary>Copies <paramref name="busArray" /> into this bus array, replacing the current buses in this array.</summary>
		///         <remarks>This method is applicable only to subclasses of <see cref="AudioUnit.AUAudioUnit" />.</remarks>
		[Export ("replaceBusses:")]
		void ReplaceBusses (AUAudioUnitBus [] busArray);
	}

	/// <summary>A name and identifier for a custom parameter preset.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUAudioUnitPreset_ClassReference/index.html">Apple documentation for <c>AUAudioUnitPreset</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface AUAudioUnitPreset : NSSecureCoding {
		/// <summary>Gets or sets a unique identifier for the preset.</summary>
		///         <value>The unique identifier for the preset.</value>
		///         <remarks>To be added.</remarks>
		[Export ("number", ArgumentSemantic.Assign)]
		nint Number { get; set; }

		/// <summary>Gets or sets the name of the preset.</summary>
		///         <value>The name of the preset.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; set; }
	}

	/// <summary>An audio unit parameter.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUParameter_ClassReference/index.html">Apple documentation for <c>AUParameter</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (AUParameterNode))]
	interface AUParameter : NSSecureCoding {
		/// <summary>Gets the parameter's minimum value.</summary>
		///         <value>The parameter's minimum value.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minValue")]
		float MinValue { get; }

		/// <summary>Gets the parameter's maximum value.</summary>
		///         <value>The parameter's maximum value.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maxValue")]
		float MaxValue { get; }

		/// <summary>Gets the unit of measurement for the value of the parameter.</summary>
		///         <value>The unit of measurement for the value of the parameter.</value>
		///         <remarks>To be added.</remarks>
		[Export ("unit")]
		AudioUnitParameterUnit Unit { get; }

		/// <summary>Gets the localized unit of measurement for the value of the parameter.</summary>
		///         <value>
		///           <para>The localized unit of measurement for the value of the parameter.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("unitName")]
		string UnitName { get; }

		/// <summary>Gets the parameter option flags for this parameter.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("flags")]
		AudioUnitParameterOptions Flags { get; }

		/// <summary>Gets the address (a non-persistent numerical identifier) for the parameter.</summary>
		///         <value>A non-persistent numeric identifier for the parameter.</value>
		///         <remarks>To be added.</remarks>
		[Export ("address")]
		ulong Address { get; }

		/// <summary>Gets an array that contains allowable localized value names for the parameter.</summary>
		///         <value>
		///           <para>An array that contains allowable localized value names for the parameter.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("valueStrings", ArgumentSemantic.Copy)]
		string [] ValueStrings { get; }

		/// <summary>Gets an array of addresses for parameters that may be changed when the developer changes this parameter.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("dependentParameters", ArgumentSemantic.Copy)]
		NSNumber [] DependentParameters { get; }

		/// <summary>Gets the current value of the parameter.</summary>
		///         <value>The current value of the parameter.</value>
		///         <remarks>To be added.</remarks>
		[Export ("value")]
		float Value { get; set; }

		/// <param name="value">The value to set.</param>
		///         <param name="originator">The originator, whose notification should be skipped.</param>
		///         <summary>Sets the parameter's value without notifying <paramref name="originator" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setValue:originator:")]
		void SetValue (float value, IntPtr originator);

		/// <param name="value">The value to set.</param>
		///         <param name="originator">The originator, whose notification should be skipped.</param>
		///         <summary>Sets the parameter to <paramref name="value" />.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("SetValue (value, originator.ObserverToken)")]
		void SetValue (float value, AUParameterObserverToken originator);

		/// <param name="value">The value to set.</param>
		///         <param name="originator">The originator, whose notification should be skipped.</param>
		///         <param name="hostTime">The time to apply the change.</param>
		///         <summary>Sets the parameter's value, without notifying <paramref name="originator" />, at the specified <paramref name="hostTime" />.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setValue:originator:atHostTime:")]
		void SetValue (float value, IntPtr originator, ulong hostTime);

		/// <param name="value">The value to set.</param>
		///         <param name="originator">The originator, whose notification should be skipped.</param>
		///         <param name="hostTime">The host time of the initiating gesture.</param>
		///         <summary>Sets the parameter to the specified value, and preserves the initiating gesture time.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("SetValue (value, originator.ObserverToken, hostTime)")]
		void SetValue (float value, AUParameterObserverToken originator, ulong hostTime);

		// -(NSString * __nonnull)stringFromValue:(const AUValue * __nullable)value;
		/// <param name="value">The parameter value to represent as a string.</param>
		///         <summary>Returns the string representation of the parameter value that corresponds to <paramref name="value" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("stringFromValue:")]
		string GetString (ref float value);

		[Internal]
		[Sealed]
		[Export ("stringFromValue:")]
		string _GetString (IntPtr value);

		/// <param name="str">The string representation for which to get a parameter value.</param>
		///         <summary>Returns the numeric value for the parameter in <paramref name="str" />.</summary>
		///         <returns>The numeric value for the parameter string.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("valueFromString:")]
		float GetValue (string str);

		[MacCatalyst (13, 1)]
		[Internal]
		[Export ("setValue:originator:atHostTime:eventType:")]
		void SetValue (float value, IntPtr originator, ulong hostTime, AUParameterAutomationEventType eventType);

		/// <param name="value">To be added.</param>
		///         <param name="originator">To be added.</param>
		///         <param name="hostTime">To be added.</param>
		///         <param name="eventType">To be added.</param>
		///         <summary>Sets the parameter's value to <paramref name="value" />, with the specified <paramref name="hostTime" /> and <paramref name="eventType" />.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("SetValue (value, originator.ObserverToken, hostTime, eventType)")]
		void SetValue (float value, AUParameterObserverToken originator, ulong hostTime, AUParameterAutomationEventType eventType);
	}

	[MacCatalyst (13, 1)]
	delegate void AUParameterAutomationObserver (ulong address, float value);

	/// <summary>A node which represents a parameter or parameter group in an <see cref="AudioUnit.AUParameterTree" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUParameterNode_ClassReference/index.html">Apple documentation for <c>AUParameterNode</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface AUParameterNode {
		/// <summary>Gets the permanent unique identifier for the node.</summary>
		///         <value>The unique identifier for the node.</value>
		///         <remarks>App developers must not change this property, as doing so will invalidate host documents between releases.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>Gets or sets the key path, the concatenation of the parameter's and its parents' identifiers.</summary>
		///         <value>The key path, the concatenation of the parameter's and its parents' identifiers.</value>
		///         <remarks>To be added.</remarks>
		[Export ("keyPath")]
		string KeyPath { get; }

		/// <summary>Gets the localized display name for the node.</summary>
		///         <value>The localized display name for the node.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displayName")]
		string DisplayName { get; }

		/// <param name="maximumLength">The maximum length of the returned localized display name or display name fragment.</param>
		/// <summary>Returns the possibly truncated localized display name for the node.</summary>
		/// <returns>The possibly truncated localized display name for the node.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("displayNameWithLength:")]
		string GetDisplayName (nint maximumLength);

		/// <param name="observer">To be added.</param>
		///         <summary>Adds an observer to a parameter or parameter group, and returns a token that identifies the observers for later removal.</summary>
		///         <returns>A token that identifies the observers for later removal.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("tokenByAddingParameterObserver:")]
		/* void * */
		IntPtr TokenByAddingParameterObserver (AUParameterObserver observer);

		/// <param name="observer">The block that is called after the parameter changes.</param>
		///         <summary>Adds a observer for the parameter and returns a token that developers can use to identify it.</summary>
		///         <returns>A token that can be passed to the <see cref="RemoveParameterObserver" /> and <see cref="AudioUnit.AUParameter.SetValue(System.Single,AudioUnit.AUParameterObserverToken,System.UInt64,AudioUnit.AUParameterAutomationEventType)" /> methods.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("new AUParameterObserverToken { ObserverToken = TokenByAddingParameterObserver (observer) }")]
		AUParameterObserverToken CreateTokenByAddingParameterObserver (AUParameterObserver observer);

		/// <param name="observer">To be added.</param>
		///         <summary>Adds a recording observer to a parameter or parameter group, and returns a token that identifies the observers for later removal.</summary>
		///         <returns>A token that identifies the observers for later removal.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("tokenByAddingParameterRecordingObserver:")]
		/* void * */
		IntPtr TokenByAddingParameterRecordingObserver (AUParameterRecordingObserver observer);

		/// <param name="observer">The block that is called after the parameter changes.</param>
		///         <summary>Adds a recording observer for the parameter and returns a token that developers can use to identify it.</summary>
		///         <returns>A token that can be passed to the <see cref="AudioUnit.AUParameterNode.RemoveParameterObserver(System.IntPtr)" /> and <see cref="AudioUnit.AUParameter.SetValue(System.Single,AudioUnit.AUParameterObserverToken,System.UInt64,AudioUnit.AUParameterAutomationEventType)" /> methods.</returns>
		///         <remarks>To be added.</remarks>
		[Wrap ("new AUParameterObserverToken { ObserverToken = TokenByAddingParameterRecordingObserver (observer) }")]
		AUParameterObserverToken CreateTokenByAddingParameterRecordingObserver (AUParameterRecordingObserver observer);

		/// <summary>Gets or sets the <see cref="AudioUnit.AUImplementorValueObserver" /> that notifies the audio unit when a parameter value changes.</summary>
		///         <value>The <see cref="AudioUnit.AUImplementorValueObserver" /> that notifies the audio unit when a parameter value changes.</value>
		///         <remarks>To be added.</remarks>
		[Export ("implementorValueObserver", ArgumentSemantic.Copy)]
		AUImplementorValueObserver ImplementorValueObserver { get; set; }

		/// <summary>Gets or sets the <see cref="AudioUnit.AUImplementorValueObserver" /> that updates stale parameter values.</summary>
		///         <value>The <see cref="AudioUnit.AUImplementorValueObserver" /> that updates stale parameter values.</value>
		///         <remarks>To be added.</remarks>
		[Export ("implementorValueProvider", ArgumentSemantic.Copy)]
		AUImplementorValueProvider ImplementorValueProvider { get; set; }

		/// <summary>Gets or sets the callback that provides a parameter value when provided with a string representation of the value of the parameter.</summary>
		///         <value>The callback that provides a parameter value when provided with a string representation of the value of the parameter.</value>
		///         <remarks>To be added.</remarks>
		[Export ("implementorValueFromStringCallback", ArgumentSemantic.Copy)]
		AUImplementorValueFromStringCallback ImplementorValueFromStringCallback { get; set; }

		/// <param name="token">An opaque pointer to the parameter observer to remove.</param>
		///         <summary>Removes the parameter observer that is specified by <paramref name="token" />.</summary>
		///         <remarks>Developers get valid <paramref name="token" /> instances by saving the value that is returned from <see cref="AudioUnit.AUParameterNode.TokenByAddingParameterRecordingObserver(AudioUnit.AUParameterRecordingObserver)" />.</remarks>
		[Export ("removeParameterObserver:")]
		void RemoveParameterObserver (/* void * */ IntPtr token);

		/// <param name="token">An opaque pointer to the parameter observer to remove.</param>
		///         <summary>Removes the parameter observer that is identified by <paramref name="token" />.</summary>
		///         <remarks>To be added.</remarks>
		[Wrap ("RemoveParameterObserver (token.ObserverToken)")]
		void RemoveParameterObserver (AUParameterObserverToken token);

		/// <summary>Gets or sets the callback that provides a string representation of the value of the parameter.</summary>
		///         <value>The callback that provides the string representation of the value of the parameter.</value>
		///         <remarks>To be added.</remarks>
		[Export ("implementorStringFromValueCallback", ArgumentSemantic.Copy),]
		AUImplementorStringFromValueCallback ImplementorStringFromValueCallback { get; set; }

		/// <summary>Callback for retrieving an abbreviated display name.</summary>
		///         <value>The possibly abbreviated name.</value>
		///         <remarks>To be added.</remarks>
		[Export ("implementorDisplayNameWithLengthCallback", ArgumentSemantic.Copy)]
		AUImplementorDisplayNameWithLengthCallback ImplementorDisplayNameWithLengthCallback { get; set; }

		[MacCatalyst (13, 1)]
		[Internal]
		[Export ("tokenByAddingParameterAutomationObserver:")]
		IntPtr _GetToken (AUParameterAutomationObserver observer);

		/// <param name="observer">To be added.</param>
		///         <summary>Adds a parameter automation observer for the parameter and returns a token that developers can use to identify it.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Wrap ("new AUParameterObserverToken (_GetToken (observer))")]
		AUParameterObserverToken GetToken (AUParameterAutomationObserver observer);
	}

	/// <summary>A group of <see cref="AudioUnit.AUParameter" /> objects for an audio unit.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUParameterGroup_ClassReference/index.html">Apple documentation for <c>AUParameterGroup</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (AUParameterNode))]
	interface AUParameterGroup : NSSecureCoding {
		/// <summary>Gets an array of the child nodes in the parameter group.</summary>
		///         <value>An array of the child nodes in the parameter group.</value>
		///         <remarks>To be added.</remarks>
		[Export ("children")]
		AUParameterNode [] Children { get; }

		/// <summary>Gets an array that contains all the parameters in the parameter group, including parameters in parameter group children.</summary>
		///         <value>An array that contains all the parameters in the parameter group, including parameters in parameter group children.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allParameters")]
		AUParameter [] AllParameters { get; }
	}

	/// <summary>A tree that contains all of the audio unit parameters for an audio unit.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/AudioUnit/Reference/AUParameterTree_ClassReference/index.html">Apple documentation for <c>AUParameterTree</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (AUParameterGroup))]
	interface AUParameterTree : NSSecureCoding {
		/// <param name="address">The address of the parameter to get.</param>
		///         <summary>Gets the parameter at the specified address.</summary>
		///         <returns>The parameter at the specified address.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("parameterWithAddress:")]
		[return: NullAllowed]
		AUParameter GetParameter (ulong address);

		/// <param name="paramID">The parameter ID search parameter.</param>
		///         <param name="scope">The scope search parameter.</param>
		///         <param name="element">The element search parameter..</param>
		///         <summary>Gets the parameter at the specified <paramref name="paramID" />, in the specified <paramref name="scope" />, that corresponds to the specified <paramref name="element" />.</summary>
		///         <returns>The matching parameter, or <see langword="null" /> if no such parameter exists.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("parameterWithID:scope:element:")]
		[return: NullAllowed]
		AUParameter GetParameter (uint paramID, uint scope, uint element);

		//Factory
		/// <include file="../docs/api/AudioUnit/AUParameterTree.xml" path="/Documentation/Docs[@DocId='M:AudioUnit.AUParameterTree.CreateParameter(System.String,System.String,System.UInt64,System.Single,System.Single,AudioUnit.AudioUnitParameterUnit,System.String,AudioUnit.AudioUnitParameterOptions,System.String[],Foundation.NSNumber[])']/*" />
		[Static]
		[Export ("createParameterWithIdentifier:name:address:min:max:unit:unitName:flags:valueStrings:dependentParameters:")]
		AUParameter CreateParameter (string identifier, string name, ulong address, float min, float max, AudioUnitParameterUnit unit, [NullAllowed] string unitName, AudioUnitParameterOptions flags, [NullAllowed] string [] valueStrings, [NullAllowed] NSNumber [] dependentParameters);

		/// <param name="identifier">A permanent non-localized name for the group.</param>
		///         <param name="name">A localized display name.</param>
		///         <param name="children">The array of parameter nodes that will become the group's children.</param>
		///         <summary>Creates a parameter group with the specified <paramref name="identifier" />, <paramref name="name" />, and <paramref name="children" />.</summary>
		///         <returns>A new parameter group.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("createGroupWithIdentifier:name:children:")]
		AUParameterGroup CreateGroup (string identifier, string name, AUParameterNode [] children);

		/// <param name="children">The template group's children.</param>
		///         <summary>Creates a prototype parameter group for creating related classes of parameter groups.</summary>
		///         <returns>A prototype parameter group for creating related classes of parameter groups.</returns>
		///         <remarks>Template parameter groups can only appear in trees at the root.</remarks>
		[Static]
		[Export ("createGroupTemplate:")]
		AUParameterGroup CreateGroupTemplate (AUParameterNode [] children);

		/// <param name="templateGroup">The parameter group to copy.</param>
		///         <param name="identifier">A permanent non-localized name for the new group.</param>
		///         <param name="name">A localized display name for the new group.</param>
		///         <param name="addressOffset">The offset, relative to the template group, of the new group's parameters.</param>
		///         <summary>Copies a template parameter group and sets the <paramref name="identifier" />, <paramref name="name" />, and template-group-relative <paramref name="addressOffset" />.</summary>
		///         <returns>The copied parameter group.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("createGroupFromTemplate:identifier:name:addressOffset:")]
		AUParameterGroup CreateGroup (AUParameterGroup templateGroup, string identifier, string name, ulong addressOffset);

		/// <param name="children">The children of the new tree.</param>
		///         <summary>Creates a new parameter tree.</summary>
		///         <returns>A new parameter tree.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("createTreeWithChildren:")]
		AUParameterTree CreateTree (AUParameterNode [] children);
	}

	/// <summary>Interface that version 3 Audio Unit extensions must implement.</summary>
	///     <remarks>
	///       <para>Developers who want to create a version 3 Audio Unit extension must implement this interface on a class that inherits from <see cref="Foundation.NSObject" /> or <see cref="CoreAudioKit.AUViewController" />.</para>
	///     </remarks>
	[Protocol]
	interface AUAudioUnitFactory : NSExtensionRequestHandling {
		/// <param name="desc">A description for the audio unit.</param>
		/// <param name="error">An <see langword="out" /> parameter into which any errors that are encountered are written.</param>
		/// <summary>Creates and returns an audio unit.</summary>
		/// <returns>An audio unit.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("createAudioUnitWithComponentDescription:error:")]
		[return: NullAllowed]
		AUAudioUnit CreateAudioUnit (AudioComponentDescription desc, [NullAllowed] out NSError error);
	}
}
