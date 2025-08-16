//
// Authors:
//  Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2014-2015 Xamarin, Inc.
//
// While the framework exists on both platforms, they share no common API
//

using System;
using System.ComponentModel;

using AudioUnit;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using CoreGraphics;
#if MONOMAC
using AppKit;
using AUViewControllerBase = AppKit.NSViewController;
using UIViewController = AppKit.NSViewController;
#else
using UIKit;
using AUViewControllerBase = UIKit.UIViewController;
using NSView = Foundation.NSObject;
using NSWindow = Foundation.NSObject;
using NSWindowController = Foundation.NSObject;
using NSViewController = Foundation.NSObject;
#endif

namespace CoreAudioKit {
	[NoiOS]
	[NoMacCatalyst]
	[Flags]
	public enum AUGenericViewDisplayFlags : uint {
		/// <summary>To be added.</summary>
		TitleDisplay = 1u << 0,
		/// <summary>To be added.</summary>
		PropertiesDisplay = 1u << 1,
		/// <summary>To be added.</summary>
		ParametersDisplay = 1u << 2,
	}

	/// <summary>A <see cref="UIViewController" /> class that handles extension requests to support audio unit extensions that have a UI.</summary>
	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreAudioKit/AUViewController">Apple documentation for <c>AUViewController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (AUViewControllerBase))]
	interface AUViewController {
		/// <param name="nibName">
		///           <para>The name of the nib file.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>The name of the bundle.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new audio unit view controller from the nib file in <paramref name="bundle" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface AUAudioUnitViewConfiguration : NSSecureCoding {
		/// <param name="width">To be added.</param>
		/// <param name="height">To be added.</param>
		/// <param name="hostHasController">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithWidth:height:hostHasController:")]
		NativeHandle Constructor (nfloat width, nfloat height, bool hostHasController);

		[Export ("width")]
		nfloat Width { get; }

		[Export ("height")]
		nfloat Height { get; }

		[Export ("hostHasController")]
		bool HostHasController { get; }
	}

	[Category]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (AUAudioUnit))]
	interface AUAudioUnitViewControllerExtensions {
		/// <param name="availableViewConfigurations">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("supportedViewConfigurations:")]
		NSIndexSet GetSupportedViewConfigurations (AUAudioUnitViewConfiguration [] availableViewConfigurations);

		/// <param name="viewConfiguration">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("selectViewConfiguration:")]
		void SelectViewConfiguration (AUAudioUnitViewConfiguration viewConfiguration);
	}

	[NoiOS]
	[NoMacCatalyst]
	[Protocol]
	interface AUCustomViewPersistentData {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("customViewPersistentData", ArgumentSemantic.Assign)]
		NSDictionary<NSString, NSObject> CustomViewPersistentData { get; set; }
	}

	[NoiOS]
	[NoMacCatalyst]
	[DisableDefaultCtor] // Crashes
	[BaseType (typeof (NSView))]
	interface AUGenericView : AUCustomViewPersistentData {

		[Export ("audioUnit")]
		AudioUnit.AudioUnit AudioUnit { get; }

		[Export ("showsExpertParameters")]
		bool ShowsExpertParameters { get; set; }

		[Export ("initWithAudioUnit:")]
		NativeHandle Constructor (AudioUnit.AudioUnit au);

		[Export ("initWithAudioUnit:displayFlags:")]
		NativeHandle Constructor (AudioUnit.AudioUnit au, AUGenericViewDisplayFlags inFlags);
	}

	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NSView))]
	[DisableDefaultCtor]
	interface AUPannerView {

		[Export ("audioUnit")]
		AudioUnit.AudioUnit AudioUnit { get; }

		[Static]
		[Export ("AUPannerViewWithAudioUnit:")]
		AUPannerView Create (AudioUnit.AudioUnit au);
	}

	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NSWindowController), Name = "CABTLEMIDIWindowController")]
	interface CABtleMidiWindowController {

		/// <param name="window">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithWindow:")]
		NativeHandle Constructor ([NullAllowed] NSWindow window);
	}

	[NoiOS]
	[NoMacCatalyst]
	[BaseType (typeof (NSViewController))]
	interface CAInterDeviceAudioViewController {

		/// <param name="nibNameOrNull">To be added.</param>
		/// <param name="nibBundleOrNull">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibNameOrNull, [NullAllowed] NSBundle nibBundleOrNull);
	}

	[NoiOS]
	[NoMacCatalyst]
	[DesignatedDefaultCtor]
	[BaseType (typeof (NSWindowController))]
	interface CANetworkBrowserWindowController {

		/// <param name="window">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithWindow:")]
		NativeHandle Constructor ([NullAllowed] NSWindow window);

		[Static]
		[Export ("isAVBSupported")]
		bool IsAvbSupported { get; }
	}

#if !MONOMAC
	/// <summary>A <see cref="UIKit.UIViewController" /> that allows discovery and connection to MIDI over Bluetooth peripherals.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreAudioKit/Reference/CABTMIDICentralViewController_Ref/index.html">Apple documentation for <c>CABTMIDICentralViewController</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	// in iOS 8.3 (Xcode 6.3 SDK) the base type was changed from UIViewController to UITableViewController
	[BaseType (typeof (UITableViewController), Name = "CABTMIDICentralViewController")]
	interface CABTMidiCentralViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates and returns a new <see cref="CoreAudioKit.CABTMidiCentralViewController" /> from the specified <paramref name="nibName" /> in the specified <paramref name="bundle" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <param name="withStyle">To be added.</param>
		/// <summary>Creates a new <see cref="CoreAudioKit.CABTMidiCentralViewController" /> with the specified style.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("initWithStyle:")]
		NativeHandle Constructor (UITableViewStyle withStyle);
	}

	/// <summary>A <see cref="UIKit.UIViewController" /> that allows the iOS device to serve as a Midi-over-Bluetooth peripheral.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/CoreAudioKit/Reference/CABTMIDILocalPeripheralViewController/index.html">Apple documentation for <c>CABTMIDILocalPeripheralViewController</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController), Name = "CABTMIDILocalPeripheralViewController")]
	interface CABTMidiLocalPeripheralViewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates and returns a new <see cref="CoreAudioKit.CABTMidiLocalPeripheralViewController" /> from the specified <paramref name="nibName" /> in the specified <paramref name="bundle" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);
	}

	/// <summary>A <see cref="UIKit.UIView" /> that defines the standard inter-app audio user interface.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreAudioKit/CAInterAppAudioSwitcherView">Apple documentation for <c>CAInterAppAudioSwitcherView</c></related>
	[NoMac]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AudioUnit' instead.")]
	[NoMacCatalyst]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AudioUnit' instead.")]
	[BaseType (typeof (UIView))]
	interface CAInterAppAudioSwitcherView {
		/// <param name="bounds">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the CAInterAppAudioSwitcherView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of CAInterAppAudioSwitcherView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect bounds);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("showingAppNames")]
		bool ShowingAppNames { [Bind ("isShowingAppNames")] get; set; }

		[Export ("setOutputAudioUnit:")]
		void SetOutputAudioUnit ([NullAllowed] AudioUnit.AudioUnit audioUnit);

		[Export ("contentWidth")]
		nfloat ContentWidth ();
	}

	/// <summary>A <see cref="UIKit.UIView" /> that shows the standard inter-app audio transport view (rewind, play, record, time, etc.).</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/CoreAudioKit/CAInterAppAudioTransportView">Apple documentation for <c>CAInterAppAudioTransportView</c></related>
	[NoMac]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'AudioUnit' instead.")]
	[NoMacCatalyst]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'AudioUnit' instead.")]
	[BaseType (typeof (UIView))]
	interface CAInterAppAudioTransportView {
		/// <param name="bounds">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the CAInterAppAudioTransportView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of CAInterAppAudioTransportView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect bounds);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("playing")]
		bool Playing { [Bind ("isPlaying")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("recording")]
		bool Recording { [Bind ("isRecording")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("connected")]
		bool Connected { [Bind ("isConnected")] get; }

		[Export ("labelColor", ArgumentSemantic.Retain)]
		UIColor LabelColor { get; set; }

		// [NullAllowed] // by default this property is null
		// NSInvalidArgumentException *** -[__NSPlaceholderDictionary initWithObjects:forKeys:count:]: attempt to insert nil object from objects[0]
		[Export ("currentTimeLabelFont", ArgumentSemantic.Retain)]
		UIFont CurrentTimeLabelFont { get; set; }

		[Export ("rewindButtonColor", ArgumentSemantic.Retain)]
		UIColor RewindButtonColor { get; set; }

		[Export ("playButtonColor", ArgumentSemantic.Retain)]
		UIColor PlayButtonColor { get; set; }

		[Export ("pauseButtonColor", ArgumentSemantic.Retain)]
		UIColor PauseButtonColor { get; set; }

		[Export ("recordButtonColor", ArgumentSemantic.Retain)]
		UIColor RecordButtonColor { get; set; }

		[Export ("setOutputAudioUnit:")]
		void SetOutputAudioUnit (AudioUnit.AudioUnit audioUnit);
	}
#endif

	[Mac (13, 0), iOS (16, 0)]
	[MacCatalyst (16, 0)]
	[BaseType (typeof (UIViewController))]
	interface AUGenericViewController {

		[DesignatedInitializer]
		[Export ("initWithNibName:bundle:")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[NullAllowed, Export ("auAudioUnit", ArgumentSemantic.Strong)]
		AUAudioUnit AuAudioUnit { get; set; }
	}
}
