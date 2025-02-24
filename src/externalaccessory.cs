//
// externalaccessory.cs: API definition for ExternalAccessory binding
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

using System;
using Foundation;
using CoreFoundation;
using ObjCRuntime;
#if !MONOMAC
using UIKit;
#endif

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace ExternalAccessory {

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (EAAccessoryDelegate) })]
	// Objective-C exception thrown.  Name: EAAccessoryInitException Reason: -init not supported. EAAccessoryManager is responsible for creating all objects.
	[DisableDefaultCtor]
	interface EAAccessory {
		/// <summary>Gets a Boolean value that tells whether the accessory is connected to the device.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("connected")]
		bool Connected { [Bind ("isConnected")] get; }

		/// <summary>Gets the unique connection identifier.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("connectionID")]
		nuint ConnectionID { get; }

		[Export ("name")]
		string Name { get; }

		/// <summary>Gets the accessory manufacturer.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("manufacturer")]
		string Manufacturer { get; }

		/// <summary>Gets the model number for the accessory.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("modelNumber")]
		string ModelNumber { get; }

		/// <summary>Gets the serial number of the accessory.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("serialNumber")]
		string SerialNumber { get; }

		/// <summary>Gets the firmware revision for the accessory.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("firmwareRevision")]
		string FirmwareRevision { get; }

		/// <summary>Gets the hardware revision for the accessory.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hardwareRevision")]
		string HardwareRevision { get; }

		/// <summary>Gets an array of descriptions of supported protocols for the accessory.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("protocolStrings")]
		string [] ProtocolStrings { get; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Assign)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the ExternalAccessory.IEAAccessoryDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the ExternalAccessory.IEAAccessoryDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IEAAccessoryDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0)]
		[Deprecated (PlatformName.TvOS, 13, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 14)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("dockType")]
		string DockType { get; }
	}

	interface IEAAccessoryDelegate { }

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface EAAccessoryDelegate {
		[Export ("accessoryDidDisconnect:"), EventArgs ("EAAccessory")]
		void Disconnected (EAAccessory accessory);
	}

	[MacCatalyst (13, 1)]
	interface EAAccessoryEventArgs {
		[Export ("EAAccessoryKey")]
		EAAccessory Accessory { get; }

		[Export ("EAAccessorySelectedKey")]
		EAAccessory Selected { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: EAAccessoryManagerInitException Reason: -init is not supported. Use +sharedAccessoryManager.
	[DisableDefaultCtor]
	interface EAAccessoryManager {
		[Static]
		[Export ("sharedAccessoryManager")]
		EAAccessoryManager SharedAccessoryManager { get; }

		[Export ("registerForLocalNotifications")]
		void RegisterForLocalNotifications ();

		[Export ("unregisterForLocalNotifications")]
		void UnregisterForLocalNotifications ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("connectedAccessories")]
		EAAccessory [] ConnectedAccessories { get; }

		[Field ("EAAccessoryDidConnectNotification")]
		[Notification (typeof (EAAccessoryEventArgs))]
		NSString DidConnectNotification { get; }

		[Field ("EAAccessoryDidDisconnectNotification")]
		[Notification (typeof (EAAccessoryEventArgs))]
		NSString DidDisconnectNotification { get; }

		// [Introduced (PlatformName.MacCatalyst, 14, 0)]
		[NoMacCatalyst] // selector does not respond
		[NoMac]
		[Export ("showBluetoothAccessoryPickerWithNameFilter:completion:")]
		[Async]
		void ShowBluetoothAccessoryPicker ([NullAllowed] NSPredicate predicate, [NullAllowed] Action<NSError> completion);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: EASessionInitException Reason: -init not supported. use -initWithAccessory:forProtocol.
	[DisableDefaultCtor]
	interface EASession {
		[Export ("initWithAccessory:forProtocol:")]
		NativeHandle Constructor (EAAccessory accessory, string protocol);

		[NullAllowed]
		[Export ("accessory")]
		EAAccessory Accessory { get; }

		[NullAllowed]
		[Export ("protocolString")]
		string ProtocolString { get; }

		[NullAllowed]
		[Export ("inputStream")]
		NSInputStream InputStream { get; }

		[NullAllowed]
		[Export ("outputStream")]
		NSOutputStream OutputStream { get; }
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface EAWiFiUnconfiguredAccessory {
		[Export ("name")]
		string Name { get; }

		[Export ("manufacturer")]
		string Manufacturer { get; }

		[Export ("model")]
		string Model { get; }

		[Export ("ssid")]
		string Ssid { get; }

		[Export ("macAddress")]
		string MacAddress { get; }

		[Export ("properties")]
		EAWiFiUnconfiguredAccessoryProperties Properties { get; }
	}


	/// <include file="../docs/api/ExternalAccessory/IEAWiFiUnconfiguredAccessoryBrowserDelegate.xml" path="/Documentation/Docs[@DocId='T:ExternalAccessory.IEAWiFiUnconfiguredAccessoryBrowserDelegate']/*" />
	interface IEAWiFiUnconfiguredAccessoryBrowserDelegate { }

	// This class is exposed for tvOS in the headers, but there's no available initializer (so it can't be constructed)
	// The API is also clearly unusable (you can list the unconfigured accessories, but you can't search for them first...)
	/// <summary>Provides the MFI Wireless Accessory Configuration process, including scanning, connecting, and configuring compatible accessories.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ExternalAccessory/Reference/EAWiFiUnconfiguredAccessoryBrowser_Class/index.html">Apple documentation for <c>EAWiFiUnconfiguredAccessoryBrowser</c></related>
	[NoTV] // 
	[NoMac]
	[MacCatalyst (13, 1)]
#if TVOS
	[BaseType (typeof (NSObject))]
#else
	[BaseType (typeof (NSObject), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (EAWiFiUnconfiguredAccessoryBrowserDelegate) })]
#endif
#if NET
	// There's a designated initializer, which leads to think that the default ctor
	// should not be used (documentation says nothing).
	[DisableDefaultCtor]
#endif
	interface EAWiFiUnconfiguredAccessoryBrowser {

		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("initWithDelegate:queue:")]
		[DesignatedInitializer] // according to header comment (but not in attributes)
		NativeHandle Constructor ([NullAllowed] IEAWiFiUnconfiguredAccessoryBrowserDelegate accessoryBrowserDelegate, [NullAllowed] DispatchQueue queue);

		[NoTV] // no member is available
		[MacCatalyst (13, 1)]
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		[NoTV] // no member is available
		[MacCatalyst (13, 1)]
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IEAWiFiUnconfiguredAccessoryBrowserDelegate Delegate { get; set; }

		[Export ("unconfiguredAccessories", ArgumentSemantic.Copy)]
		NSSet UnconfiguredAccessories { get; }

		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("startSearchingForUnconfiguredAccessoriesMatchingPredicate:")]
		void StartSearchingForUnconfiguredAccessories ([NullAllowed] NSPredicate predicate);

		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("stopSearchingForUnconfiguredAccessories")]
		void StopSearchingForUnconfiguredAccessories ();

#if !MONOMAC
		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("configureAccessory:withConfigurationUIOnViewController:")]
		void ConfigureAccessory (EAWiFiUnconfiguredAccessory accessory, UIViewController viewController);
#endif
	}

	/// <summary>Delegate object for <see cref="T:ExternalAccessory.EAWiFiUnconfiguredAccessoryBrowser" />, providing events related to scanning, connecting, and configuring compatible accessories.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ExternalAccessory/Reference/EAWiFiUnconfiguredAccessoryBrowserDelegate_Protocol/index.html">Apple documentation for <c>EAWiFiUnconfiguredAccessoryBrowserDelegate</c></related>
	[NoMac]
	[NoTV] // no member is available
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface EAWiFiUnconfiguredAccessoryBrowserDelegate {

		[Abstract]
		[Export ("accessoryBrowser:didUpdateState:"), EventArgs ("EAWiFiUnconfiguredAccessory")]
		void DidUpdateState (EAWiFiUnconfiguredAccessoryBrowser browser, EAWiFiUnconfiguredAccessoryBrowserState state);

		[Abstract]
		[Export ("accessoryBrowser:didFindUnconfiguredAccessories:"), EventArgs ("EAWiFiUnconfiguredAccessoryBrowser")]
		void DidFindUnconfiguredAccessories (EAWiFiUnconfiguredAccessoryBrowser browser, NSSet accessories);

		[Abstract]
		[Export ("accessoryBrowser:didRemoveUnconfiguredAccessories:"), EventArgs ("EAWiFiUnconfiguredAccessoryBrowser")]
		void DidRemoveUnconfiguredAccessories (EAWiFiUnconfiguredAccessoryBrowser browser, NSSet accessories);

		[Abstract]
		[Export ("accessoryBrowser:didFinishConfiguringAccessory:withStatus:"), EventArgs ("EAWiFiUnconfiguredAccessoryDidFinish")]
		void DidFinishConfiguringAccessory (EAWiFiUnconfiguredAccessoryBrowser browser, EAWiFiUnconfiguredAccessory accessory, EAWiFiUnconfiguredAccessoryConfigurationStatus status);
	}
}
