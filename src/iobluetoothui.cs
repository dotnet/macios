// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
// iobluetoothui.cs: Bindings for the IOBluetoothUI framework (macOS only).
//
// APIs that were deprecated in macOS 10.9 or earlier (including all the
// *Ref-based accessors) are not bound.

using AppKit;
using Foundation;
using IOBluetooth;
using ObjCRuntime;

namespace IOBluetoothUI {

	[NoMacCatalyst]
	[BaseType (typeof (NSWindowController))]
	interface IOBluetoothDeviceSelectorController {
		[Static]
		[NullAllowed, Export ("deviceSelector")]
		IOBluetoothDeviceSelectorController DeviceSelector { get; }

		[Export ("runModal")]
		int RunModal ();

		[Export ("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		int BeginSheetModal (NSWindow sheetWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[return: NullAllowed]
		[Export ("getResults")]
		IOBluetoothDevice [] GetResults ();

		[Export ("setOptions:")]
		void SetOptions (IOBluetoothServiceBrowserControllerOptions options);

		[Export ("getOptions")]
		IOBluetoothServiceBrowserControllerOptions GetOptions ();

		// const IOBluetoothDeviceSearchAttributes *
		[Export ("setSearchAttributes:")]
		void SetSearchAttributes (IntPtr searchAttributes);

		// const IOBluetoothDeviceSearchAttributes *
		[Export ("getSearchAttributes")]
		IntPtr GetSearchAttributes ();

		[Export ("addAllowedUUID:")]
		void AddAllowedUuid (IOBluetoothSDPUUID allowedUuid);

		[Export ("addAllowedUUIDArray:")]
		void AddAllowedUuids (IOBluetoothSDPUUID [] allowedUuids);

		[Export ("clearAllowedUUIDs")]
		void ClearAllowedUuids ();

		[Export ("setTitle:")]
		void SetTitle (string windowTitle);

		[return: NullAllowed]
		[Export ("getTitle")]
		string GetTitle ();

		[Export ("setHeader:")]
		void SetHeader (string headerText);

		[return: NullAllowed]
		[Export ("getHeader")]
		string GetHeader ();

		[Export ("setDescriptionText:")]
		void SetDescriptionText (string descriptionText);

		[return: NullAllowed]
		[Export ("getDescriptionText")]
		string GetDescriptionText ();

		[Export ("setPrompt:")]
		void SetPrompt (string prompt);

		[return: NullAllowed]
		[Export ("getPrompt")]
		string GetPrompt ();

		[Export ("setCancel:")]
		void SetCancel (string prompt);

		[return: NullAllowed]
		[Export ("getCancel")]
		string GetCancel ();
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSWindowController))]
	interface IOBluetoothObjectPushUIController {
		[Export ("initObjectPushWithBluetoothDevice:withFiles:delegate:")]
		NativeHandle Constructor (IOBluetoothDevice device, NSArray files, [NullAllowed] NSObject delegateObject);

		[Export ("runModal")]
		void RunModal ();

		[Export ("runPanel")]
		void RunPanel ();

		[Export ("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		int BeginSheetModal (NSWindow sheetWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[Export ("stop")]
		void Stop ();

		[Export ("setTitle:")]
		void SetTitle (string windowTitle);

		[return: NullAllowed]
		[Export ("getTitle")]
		string GetTitle ();

		[Export ("setIconImage:")]
		void SetIconImage (NSImage image);

		[return: NullAllowed]
		[Export ("getDevice")]
		IOBluetoothDevice GetDevice ();

		[Export ("isTransferInProgress")]
		bool IsTransferInProgress { get; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSWindowController))]
	interface IOBluetoothPairingController {
		[Static]
		[NullAllowed, Export ("pairingController")]
		IOBluetoothPairingController PairingController { get; }

		[Export ("runModal")]
		int RunModal ();

		[return: NullAllowed]
		[Export ("getResults")]
		IOBluetoothDevice [] GetResults ();

		[Export ("setOptions:")]
		void SetOptions (IOBluetoothServiceBrowserControllerOptions options);

		[Export ("getOptions")]
		IOBluetoothServiceBrowserControllerOptions GetOptions ();

		// const IOBluetoothDeviceSearchAttributes *
		[Export ("setSearchAttributes:")]
		void SetSearchAttributes (IntPtr searchAttributes);

		// const IOBluetoothDeviceSearchAttributes *
		[Export ("getSearchAttributes")]
		IntPtr GetSearchAttributes ();

		[Export ("addAllowedUUID:")]
		void AddAllowedUuid (IOBluetoothSDPUUID allowedUuid);

		[Export ("addAllowedUUIDArray:")]
		void AddAllowedUuids (IOBluetoothSDPUUID [] allowedUuids);

		[Export ("clearAllowedUUIDs")]
		void ClearAllowedUuids ();

		[Export ("setTitle:")]
		void SetTitle (string windowTitle);

		[return: NullAllowed]
		[Export ("getTitle")]
		string GetTitle ();

		[Export ("setDescriptionText:")]
		void SetDescriptionText (string descriptionText);

		[return: NullAllowed]
		[Export ("getDescriptionText")]
		string GetDescriptionText ();

		[Export ("setPrompt:")]
		void SetPrompt (string prompt);

		[return: NullAllowed]
		[Export ("getPrompt")]
		string GetPrompt ();
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSWindowController))]
	interface IOBluetoothServiceBrowserController {
		[Static]
		[Export ("serviceBrowserController:")]
		[return: NullAllowed]
		IOBluetoothServiceBrowserController Create (IOBluetoothServiceBrowserControllerOptions options);

		[Export ("setOptions:")]
		void SetOptions (IOBluetoothServiceBrowserControllerOptions options);

		[Export ("runModal")]
		int RunModal ();

		[Export ("beginSheetModalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		int BeginSheetModal (NSWindow sheetWindow, [NullAllowed] NSObject modalDelegate, [NullAllowed] Selector didEndSelector, IntPtr contextInfo);

		[return: NullAllowed]
		[Export ("getResults")]
		IOBluetoothSDPServiceRecord [] GetResults ();

		[Export ("getOptions")]
		IOBluetoothServiceBrowserControllerOptions GetOptions ();

		// const IOBluetoothDeviceSearchAttributes *
		[Export ("setSearchAttributes:")]
		void SetSearchAttributes (IntPtr searchAttributes);

		// const IOBluetoothDeviceSearchAttributes *
		[Export ("getSearchAttributes")]
		IntPtr GetSearchAttributes ();

		[Export ("addAllowedUUID:")]
		void AddAllowedUuid (IOBluetoothSDPUUID allowedUuid);

		[Export ("addAllowedUUIDArray:")]
		void AddAllowedUuids (IOBluetoothSDPUUID [] allowedUuids);

		[Export ("clearAllowedUUIDs")]
		void ClearAllowedUuids ();

		[Export ("setTitle:")]
		void SetTitle (string windowTitle);

		[return: NullAllowed]
		[Export ("getTitle")]
		string GetTitle ();

		[Export ("setDescriptionText:")]
		void SetDescriptionText (string descriptionText);

		[return: NullAllowed]
		[Export ("getDescriptionText")]
		string GetDescriptionText ();

		[Export ("setPrompt:")]
		void SetPrompt (string prompt);

		[return: NullAllowed]
		[Export ("getPrompt")]
		string GetPrompt ();
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSView))]
	interface IOBluetoothPasskeyDisplay {
		[Export ("usePasskeyNotificaitons")]
		bool UsePasskeyNotifications { get; set; }

		[Export ("isIncomingRequest")]
		bool IsIncomingRequest { get; set; }

		[NullAllowed, Export ("passkey", ArgumentSemantic.Copy)]
		string Passkey { get; set; }

		[NullAllowed, Export ("returnImage", ArgumentSemantic.Retain)]
		NSImage ReturnImage { get; set; }

		[NullAllowed, Export ("returnHighlightImage", ArgumentSemantic.Retain)]
		NSImage ReturnHighlightImage { get; set; }

		[NullAllowed, Export ("centeredView", ArgumentSemantic.Assign)]
		NSView CenteredView { get; set; }

		[NullAllowed, Export ("backgroundImageConstraint", ArgumentSemantic.Assign)]
		NSLayoutConstraint BackgroundImageConstraint { get; set; }

		[Static]
		[Export ("sharedDisplayView")]
		IOBluetoothPasskeyDisplay SharedDisplayView { get; }

		[Export ("setPasskey:forDevice:usingSSP:")]
		void SetPasskey (string passkey, IOBluetoothDevice device, bool isSsp);

		[Export ("advancePasskeyIndicator")]
		void AdvancePasskeyIndicator ();

		[Export ("retreatPasskeyIndicator")]
		void RetreatPasskeyIndicator ();

		[Export ("resetPasskeyIndicator")]
		void ResetPasskeyIndicator ();
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSTextFieldCell))]
	interface IOBluetoothAccessibilityIgnoredTextFieldCell {
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSImageCell))]
	interface IOBluetoothAccessibilityIgnoredImageCell {
	}
}
