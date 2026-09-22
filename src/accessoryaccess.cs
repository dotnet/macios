using System;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

// 'xpc_object_t' is an opaque OS_object that's shuttled across XPC boundaries; it has no managed API
// surface of its own, so macios binds it as NSObject everywhere (see src/browserenginekit.cs).
using OS_xpc_object = Foundation.NSObject;

namespace AccessoryAccess {

	/// <summary>Specifies errors reported by AccessoryAccess.</summary>
	[Mac (27, 0)]
	[Native]
	[ErrorDomain ("AAErrorDomain")]
	public enum AAErrorCode : long {
		/// <summary>An internal error occurred.</summary>
		Internal = 1,
		/// <summary>An accessory listener is already registered.</summary>
		AccessoryListenerAlreadyRegistered = 2,
		/// <summary>The accessory is not accessible.</summary>
		AccessoryNotAccessible = 3,
		/// <summary>The accessory is in an invalid state for the requested operation.</summary>
		InvalidAccessoryState = 4,
	}

	/// <summary>Specifies how interface matching dictionaries are applied when matching a USB accessory.</summary>
	[Mac (27, 0)]
	[Native ("AAUSBAccessoryMatchingCriteriaInterfaceMatchingOption")]
	public enum AAUsbAccessoryMatchingCriteriaInterfaceMatchingOption : long {
		/// <summary>All interface matching dictionaries must match.</summary>
		MatchAll,
		/// <summary>At least one interface matching dictionary must match.</summary>
		MatchAny,
	}

	/// <summary>A completion handler for opening a USB accessory.</summary>
	/// <param name="device">A live IOUSBHostDevice handle that remains valid until the accessory is closed.</param>
	/// <param name="error">The error that was encountered, or <see langword="null" /> if no error occurred.</param>
	/// <remarks>IOUSBHost is not bound in macios, so this handle is surfaced as a raw <see cref="IntPtr"/>. To perform USB I/O, hand the accessory to a native/Swift service via <see cref="AAUsbAccessory.CreateXpcRepresentation"/> or bridge the handle through native IOUSBHost code.</remarks>
	delegate void AAUsbAccessoryOpenCompletionHandler (IntPtr device, [NullAllowed] NSError error);
	/// <summary>A completion handler for closing a USB accessory.</summary>
	/// <param name="error">The error that occurred, or <see langword="null" /> if the accessory closed successfully.</param>
	delegate void AAUsbAccessoryCloseCompletionHandler ([NullAllowed] NSError error);

	[Mac (27, 0)]
	[BaseType (typeof (NSObject), Name = "AAUSBAccessory")]
	[DisableDefaultCtor]
	interface AAUsbAccessory : NSSecureCoding {
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

		[Export ("openWithServiceQueue:completionHandler:")]
		[Async]
		void Open ([NullAllowed] DispatchQueue serviceQueue, AAUsbAccessoryOpenCompletionHandler completionHandler);

		[Export ("closeWithCompletionHandler:")]
		[Async]
		void Close (AAUsbAccessoryCloseCompletionHandler completionHandler);
	}

	interface IAAUsbAccessoryListener { }

	[Mac (27, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false, Name = "AAUSBAccessoryListener"), Model]
	[BaseType (typeof (NSObject))]
	interface AAUsbAccessoryListener {
		[Export ("usbAccessoryDidConnect:")]
		void UsbAccessoryDidConnect (AAUsbAccessory usbAccessory);

		[Export ("usbAccessoryDidDisconnect:")]
		void UsbAccessoryDidDisconnect (AAUsbAccessory usbAccessory);
	}

	/// <summary>A completion handler for registering a USB accessory listener.</summary>
	/// <param name="accessories">The USB accessories that are already connected.</param>
	/// <param name="error">The error that occurred, or <see langword="null" /> if registration succeeded.</param>
	delegate void AAUsbAccessoryManagerRegisterListenerCompletionHandler (AAUsbAccessory [] accessories, [NullAllowed] NSError error);
	/// <summary>A completion handler for unregistering a USB accessory listener.</summary>
	delegate void AAUsbAccessoryManagerUnregisterListenerCompletionHandler ();

	[Mac (27, 0)]
	[BaseType (typeof (NSObject), Name = "AAUSBAccessoryManager")]
	[DisableDefaultCtor]
	interface AAUsbAccessoryManager {
		[Static]
		[Export ("sharedManager", ArgumentSemantic.Strong)]
		AAUsbAccessoryManager SharedManager { get; }

		[Export ("registerListener:withMatchingCriteria:completionHandler:")]
		[Async]
		void RegisterListener (IAAUsbAccessoryListener listener, AAUsbAccessoryMatchingCriteria [] matchingCriteria, AAUsbAccessoryManagerRegisterListenerCompletionHandler completionHandler);

		[Export ("unregisterListener:completionHandler:")]
		[Async]
		void UnregisterListener (IAAUsbAccessoryListener listener, AAUsbAccessoryManagerUnregisterListenerCompletionHandler completionHandler);
	}

	[Mac (27, 0)]
	[BaseType (typeof (NSObject), Name = "AAUSBAccessoryMatchingCriteria")]
	[DisableDefaultCtor]
	interface AAUsbAccessoryMatchingCriteria : NSCopying {
		[Export ("initWithDeviceMatchingDictionary:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary<NSString, NSObject> dictionary);

		[Export ("initWithDeviceMatchingDictionary:interfaceMatchingDictionaries:interfaceMatchingOption:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSDictionary<NSString, NSObject> deviceMatchingDictionary, NSDictionary<NSString, NSObject> [] interfaceMatchingDictionaries, AAUsbAccessoryMatchingCriteriaInterfaceMatchingOption interfaceMatchingOption);
	}
}
