using ObjCRuntime;
using Foundation;
using CoreFoundation;
using System;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace PushKit {
	/// <summary>Holds the <see cref="P:PushKit.PKPushCredentials.Token" /> that holds the user's credentials.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PushKit/Reference/PKPushCredentials_Class/index.html">Apple documentation for <c>PKPushCredentials</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPushCredentials {
		[Export ("type", ArgumentSemantic.Copy)]
		string Type { get; }

		[Export ("token", ArgumentSemantic.Copy)]
		NSData Token { get; }
	}

	/// <summary>Contains a dictionary of data for a push operation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PushKit/Reference/PKPushPayload_Class/index.html">Apple documentation for <c>PKPushPayload</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPushPayload {
		[Export ("type", ArgumentSemantic.Copy)]
		string Type { get; }

		[Export ("dictionaryPayload", ArgumentSemantic.Copy)]
		NSDictionary DictionaryPayload { get; }
	}

	/// <summary>Allows the developer to register for remote pushes.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PushKit/Reference/PKPushRegistry_Class/index.html">Apple documentation for <c>PKPushRegistry</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPushRegistry {
		/// <summary>An instance of the PushKit.IPKPushRegistryDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the PushKit.IPKPushRegistryDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IPKPushRegistryDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Weak)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("desiredPushTypes", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSSet DesiredPushTypes { get; set; }

		[Export ("pushTokenForType:")]
		[return: NullAllowed]
		NSData PushToken (string type);

		[DesignatedInitializer]
		[Export ("initWithQueue:")]
		NativeHandle Constructor ([NullAllowed] DispatchQueue queue);
	}

	/// <summary>Holds the transports available for Push Kit (currently only Voice Over IP).</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface PKPushType {

		/// <summary>Represents the value associated with the constant PKPushTypeVoIP</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[NoMac]
		[Field ("PKPushTypeVoIP")]
		NSString Voip { get; }

		/// <summary>Gets the WatchKit complication push type, <c>PKPushTypeComplication</c>.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use directly from watchOS instead.")]
		[NoMac]
		[NoMacCatalyst]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use directly from watchOS instead.")]
		[Field ("PKPushTypeComplication")]
		NSString Complication { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPushTypeFileProvider")]
		NSString FileProvider { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:PushKit.PKPushRegistryDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:PushKit.PKPushRegistryDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:PushKit.PKPushRegistryDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:PushKit.PKPushRegistryDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IPKPushRegistryDelegate { }

	/// <summary>Completion handler for registering a push operation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PushKit/Reference/PKPushRegistryDelegate_Protocol/index.html">Apple documentation for <c>PKPushRegistryDelegate</c></related>
	[MacCatalyst (13, 1)]
	[Model]
	[Protocol]
	[BaseType (typeof (NSObject))]
	interface PKPushRegistryDelegate {
		/// <param name="registry">To be added.</param>
		/// <param name="credentials">To be added.</param>
		/// <param name="type">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("pushRegistry:didUpdatePushCredentials:forType:"), EventArgs ("PKPushRegistryUpdated"), EventName ("CredentialsUpdated")]
		void DidUpdatePushCredentials (PKPushRegistry registry, PKPushCredentials credentials, string type);

		/// <param name="registry">To be added.</param>
		/// <param name="payload">To be added.</param>
		/// <param name="type">To be added.</param>
		/// <summary>Developers should not use this deprecated method. Developers should use the 'DidReceiveIncomingPushWithPayload' overload accepting an 'Action' argument instead.</summary>
		/// <remarks>To be added.</remarks>
		[NoMac]
#if !NET
		[Abstract] // now optional in iOS 11
#endif
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use the 'DidReceiveIncomingPushWithPayload' overload accepting an 'Action' argument instead.")]
		[NoMacCatalyst]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the 'DidReceiveIncomingPushWithPayload' overload accepting an 'Action' argument instead.")]
		[Export ("pushRegistry:didReceiveIncomingPushWithPayload:forType:"), EventArgs ("PKPushRegistryRecieved"), EventName ("IncomingPushReceived")]
		void DidReceiveIncomingPush (PKPushRegistry registry, PKPushPayload payload, string type);

		/// <param name="registry">To be added.</param>
		/// <param name="payload">To be added.</param>
		/// <param name="type">To be added.</param>
		/// <param name="completion">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("pushRegistry:didReceiveIncomingPushWithPayload:forType:withCompletionHandler:")]
		void DidReceiveIncomingPush (PKPushRegistry registry, PKPushPayload payload, string type, Action completion);

		/// <param name="registry">To be added.</param>
		/// <param name="type">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("pushRegistry:didInvalidatePushTokenForType:"), EventArgs ("PKPushRegistryRecieved"), EventName ("PushTokenInvalidated")]
		void DidInvalidatePushToken (PKPushRegistry registry, string type);
	}
}
