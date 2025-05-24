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

namespace ExternalAccessory {

	/// <summary>Provides information about a connected external accessory.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ExternalAccessory/Reference/EAAccessory_class/index.html">Apple documentation for <c>EAAccessory</c></related>
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

		/// <summary>Gets the accessory display name.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="ExternalAccessory.EAAccessoryDelegate" />.</summary>
	/// <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="ExternalAccessory.EAAccessoryDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="ExternalAccessory.EAAccessoryDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="ExternalAccessory.EAAccessoryDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface EAAccessoryDelegate {
		/// <param name="accessory">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("accessoryDidDisconnect:"), EventArgs ("EAAccessory", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void Disconnected (EAAccessory accessory);
	}

	[MacCatalyst (13, 1)]
	interface EAAccessoryEventArgs {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("EAAccessoryKey")]
		EAAccessory Accessory { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("EAAccessorySelectedKey")]
		EAAccessory Selected { get; }
	}

	/// <summary>Used to enumerate the external accessories connected.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ExternalAccessory/Reference/EAAccessoryManager_class/index.html">Apple documentation for <c>EAAccessoryManager</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: EAAccessoryManagerInitException Reason: -init is not supported. Use +sharedAccessoryManager.
	[DisableDefaultCtor]
	interface EAAccessoryManager {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedAccessoryManager")]
		EAAccessoryManager SharedAccessoryManager { get; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("registerForLocalNotifications")]
		void RegisterForLocalNotifications ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("unregisterForLocalNotifications")]
		void UnregisterForLocalNotifications ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("connectedAccessories")]
		EAAccessory [] ConnectedAccessories { get; }

		/// <include file="../docs/api/ExternalAccessory/EAAccessoryManager.xml" path="/Documentation/Docs[@DocId='P:ExternalAccessory.EAAccessoryManager.DidConnectNotification']/*" />
		[Field ("EAAccessoryDidConnectNotification")]
		[Notification (typeof (EAAccessoryEventArgs))]
		NSString DidConnectNotification { get; }

		/// <include file="../docs/api/ExternalAccessory/EAAccessoryManager.xml" path="/Documentation/Docs[@DocId='P:ExternalAccessory.EAAccessoryManager.DidDisconnectNotification']/*" />
		[Field ("EAAccessoryDidDisconnectNotification")]
		[Notification (typeof (EAAccessoryEventArgs))]
		NSString DidDisconnectNotification { get; }

		// [Introduced (PlatformName.MacCatalyst, 14, 0)]
		/// <param name="predicate">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="completion">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoMacCatalyst] // selector does not respond
		[NoMac]
		[Export ("showBluetoothAccessoryPickerWithNameFilter:completion:")]
		[Async (XmlDocs = """
			<param name="predicate">To be added.</param>
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous ShowBluetoothAccessoryPicker operation</returns>
			<remarks>
			          <para copied="true">The ShowBluetoothAccessoryPickerAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void ShowBluetoothAccessoryPicker ([NullAllowed] NSPredicate predicate, [NullAllowed] Action<NSError> completion);
	}

	/// <summary>The EASession is used to communicate with the external hardware accessory.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ExternalAccessory/Reference/EASession_class/index.html">Apple documentation for <c>EASession</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: EASessionInitException Reason: -init not supported. use -initWithAccessory:forProtocol.
	[DisableDefaultCtor]
	interface EASession {
		/// <param name="accessory">To be added.</param>
		/// <param name="protocol">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithAccessory:forProtocol:")]
		NativeHandle Constructor (EAAccessory accessory, string protocol);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("accessory")]
		EAAccessory Accessory { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("protocolString")]
		string ProtocolString { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("inputStream")]
		NSInputStream InputStream { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("outputStream")]
		NSOutputStream OutputStream { get; }
	}

	/// <summary>An MFI Wireless Accessory Configuration accessory that is currently unconfigured.</summary>
	/// <remarks>To be added.</remarks>
	/// <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ExternalAccessory/Reference/EAWiFiUnconfiguredAccessory_Class/index.html">Apple documentation for <c>EAWiFiUnconfiguredAccessory</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface EAWiFiUnconfiguredAccessory {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("manufacturer")]
		string Manufacturer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("model")]
		string Model { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ssid")]
		string Ssid { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("macAddress")]
		string MacAddress { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
	// There's a designated initializer, which leads to think that the default ctor
	// should not be used (documentation says nothing).
	[DisableDefaultCtor]
	interface EAWiFiUnconfiguredAccessoryBrowser {

		/// <param name="accessoryBrowserDelegate">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="queue">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("initWithDelegate:queue:")]
		[DesignatedInitializer] // according to header comment (but not in attributes)
		NativeHandle Constructor ([NullAllowed] IEAWiFiUnconfiguredAccessoryBrowserDelegate accessoryBrowserDelegate, [NullAllowed] DispatchQueue queue);

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[NoTV] // no member is available
		[MacCatalyst (13, 1)]
		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the ExternalAccessory.IEAWiFiUnconfiguredAccessoryBrowserDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the ExternalAccessory.IEAWiFiUnconfiguredAccessoryBrowserDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[NoTV] // no member is available
		[MacCatalyst (13, 1)]
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IEAWiFiUnconfiguredAccessoryBrowserDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("unconfiguredAccessories", ArgumentSemantic.Copy)]
		NSSet UnconfiguredAccessories { get; }

		/// <param name="predicate">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("startSearchingForUnconfiguredAccessoriesMatchingPredicate:")]
		void StartSearchingForUnconfiguredAccessories ([NullAllowed] NSPredicate predicate);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("stopSearchingForUnconfiguredAccessories")]
		void StopSearchingForUnconfiguredAccessories ();

#if !MONOMAC
		/// <param name="accessory">To be added.</param>
		///         <param name="viewController">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (14, 0)] // the headers lie, not usable until at least Mac Catalyst 14.0
		[NoTV]
		[Export ("configureAccessory:withConfigurationUIOnViewController:")]
		void ConfigureAccessory (EAWiFiUnconfiguredAccessory accessory, UIViewController viewController);
#endif
	}

	/// <summary>Delegate object for <see cref="ExternalAccessory.EAWiFiUnconfiguredAccessoryBrowser" />, providing events related to scanning, connecting, and configuring compatible accessories.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/ExternalAccessory/Reference/EAWiFiUnconfiguredAccessoryBrowserDelegate_Protocol/index.html">Apple documentation for <c>EAWiFiUnconfiguredAccessoryBrowserDelegate</c></related>
	[NoMac]
	[NoTV] // no member is available
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface EAWiFiUnconfiguredAccessoryBrowserDelegate {

		/// <param name="browser">To be added.</param>
		///         <param name="state">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("accessoryBrowser:didUpdateState:"), EventArgs ("EAWiFiUnconfiguredAccessory", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidUpdateState (EAWiFiUnconfiguredAccessoryBrowser browser, EAWiFiUnconfiguredAccessoryBrowserState state);

		/// <param name="browser">To be added.</param>
		///         <param name="accessories">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("accessoryBrowser:didFindUnconfiguredAccessories:"), EventArgs ("EAWiFiUnconfiguredAccessoryBrowser", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidFindUnconfiguredAccessories (EAWiFiUnconfiguredAccessoryBrowser browser, NSSet accessories);

		/// <param name="browser">To be added.</param>
		///         <param name="accessories">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("accessoryBrowser:didRemoveUnconfiguredAccessories:"), EventArgs ("EAWiFiUnconfiguredAccessoryBrowser", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidRemoveUnconfiguredAccessories (EAWiFiUnconfiguredAccessoryBrowser browser, NSSet accessories);

		/// <param name="browser">To be added.</param>
		///         <param name="accessory">To be added.</param>
		///         <param name="status">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("accessoryBrowser:didFinishConfiguringAccessory:withStatus:"), EventArgs ("EAWiFiUnconfiguredAccessoryDidFinish", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidFinishConfiguringAccessory (EAWiFiUnconfiguredAccessoryBrowser browser, EAWiFiUnconfiguredAccessory accessory, EAWiFiUnconfiguredAccessoryConfigurationStatus status);
	}
}
