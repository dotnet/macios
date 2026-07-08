using System;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

// 'IOUSBHostDevice' comes from the IOUSBHost framework, which macios intentionally does not bind:
// it's a low-level IOKit/DriverKit-adjacent framework whose API is almost entirely
// NS_REFINED_FOR_SWIFT (see IGNORED_MACOS_FRAMEWORKS in tests/xtro-sharpie/Makefile). The instance
// returned by AAUSBAccessory.Open is a valid IOUSBHostDevice handle; consumers that need to perform
// USB I/O on it should hand the accessory to a native/Swift XPC service using CreateXpcRepresentation.
using IOUSBHostDevice = Foundation.NSObject;

// 'xpc_object_t' is an opaque OS_object that's shuttled across XPC boundaries; it has no managed API
// surface of its own, so macios binds it as NSObject everywhere (see src/browserenginekit.cs).
using OS_xpc_object = Foundation.NSObject;

namespace AccessoryAccess {

	[Mac (27, 0)]
	[Native]
	[ErrorDomain ("AAErrorDomain")]
	public enum AAErrorCode : long {
		Internal = 1,
		AccessoryListenerAlreadyRegistered = 2,
		AccessoryNotAccessible = 3,
		InvalidAccessoryState = 4,
	}

	[Mac (27, 0)]
	[Native]
	public enum AAUSBAccessoryMatchingCriteriaInterfaceMatchingOption : long {
		MatchAll,
		MatchAny,
	}

	delegate void AAUSBAccessoryOpenCompletionHandler ([NullAllowed] IOUSBHostDevice device, [NullAllowed] NSError error);
	delegate void AAUSBAccessoryCloseCompletionHandler ([NullAllowed] NSError error);

	[Mac (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AAUSBAccessory : NSSecureCoding {
		[Export ("initWithXPCRepresentation:")]
		[DesignatedInitializer]
		NativeHandle Constructor (OS_xpc_object xpcRepresentation);

		[Export ("registryID")]
		ulong RegistryId { get; }

		[Export ("deviceDescriptorData")]
		NSData DeviceDescriptorData { get; }

		[NullAllowed, Export ("configurationDescriptorData")]
		NSData ConfigurationDescriptorData { get; }

		[Export ("createXPCRepresentation")]
		OS_xpc_object CreateXpcRepresentation ();

		// The completion handler receives a live IOUSBHostDevice (typed as NSObject; see the
		// IOUSBHostDevice alias at the top of this file). To perform USB I/O, forward the accessory to
		// a native/Swift XPC service via CreateXpcRepresentation, or bridge the handle through IOUSBHost.
		[Export ("openWithServiceQueue:completionHandler:")]
		[Async]
		void Open ([NullAllowed] DispatchQueue serviceQueue, AAUSBAccessoryOpenCompletionHandler completionHandler);

		[Export ("closeWithCompletionHandler:")]
		[Async]
		void Close (AAUSBAccessoryCloseCompletionHandler completionHandler);
	}

	interface IAAUSBAccessoryListener { }

	[Mac (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface AAUSBAccessoryListener {
		[Export ("usbAccessoryDidConnect:")]
		void UsbAccessoryDidConnect (AAUSBAccessory usbAccessory);

		[Export ("usbAccessoryDidDisconnect:")]
		void UsbAccessoryDidDisconnect (AAUSBAccessory usbAccessory);
	}

	delegate void AAUSBAccessoryManagerRegisterListenerCompletionHandler (AAUSBAccessory [] accessories, [NullAllowed] NSError error);
	delegate void AAUSBAccessoryManagerUnregisterListenerCompletionHandler ();

	[Mac (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AAUSBAccessoryManager {
		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		AAUSBAccessoryManager SharedManager { get; }

		[Export ("registerListener:withMatchingCriteria:completionHandler:")]
		[Async]
		void RegisterListener (IAAUSBAccessoryListener listener, AAUSBAccessoryMatchingCriteria [] matchingCriteria, AAUSBAccessoryManagerRegisterListenerCompletionHandler completionHandler);

		[Export ("unregisterListener:completionHandler:")]
		[Async]
		void UnregisterListener (IAAUSBAccessoryListener listener, AAUSBAccessoryManagerUnregisterListenerCompletionHandler completionHandler);
	}

	[Mac (27, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface AAUSBAccessoryMatchingCriteria : NSCopying {
		[Export ("initWithDeviceMatchingDictionary:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary<NSString, NSObject> dictionary);

		[Export ("initWithDeviceMatchingDictionary:interfaceMatchingDictionaries:interfaceMatchingOption:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSDictionary<NSString, NSObject> deviceMatchingDictionary, NSDictionary<NSString, NSObject> [] interfaceMatchingDictionaries, AAUSBAccessoryMatchingCriteriaInterfaceMatchingOption interfaceMatchingOption);
	}
}
