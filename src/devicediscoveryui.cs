//
// DeviceDiscoveryUI C# bindings
//
// Authors:
//	Israel Soto  <issoto@microsoft.com>
//
// Copyright 2022 Microsoft Corporation.
//

using System;
using Foundation;
using ObjCRuntime;
using UIKit;

using OS_nw_browse_descriptor = System.IntPtr;
using OS_nw_parameters = System.IntPtr;
using OS_nw_endpoint = System.IntPtr;
using OS_nw_error = System.IntPtr;

namespace DeviceDiscoveryUI {

	[Internal]
	delegate void _DevicePickerCompletionHandler ([NullAllowed] OS_nw_endpoint endpoint, [NullAllowed] OS_nw_error error);

	[NoMac, iOS (26, 0), NoMacCatalyst, TV (16, 0)]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface DDDevicePickerViewController {
		[Internal]
		[Static]
		[Export ("isSupportedForBrowseDescriptor:parameters:")]
		bool _IsSupported (OS_nw_browse_descriptor browseDescriptor, [NullAllowed] OS_nw_parameters parameters);

		[Internal]
		[Export ("initWithBrowseDescriptor:parameters:")]
		NativeHandle _InitWithBrowseDescriptorAndParameters (OS_nw_browse_descriptor browseDescriptor, [NullAllowed] OS_nw_parameters parameters);

		[Internal]
		[Export ("setDevicePickerCompletionHandler:")]
		void _SetDevicePicker (_DevicePickerCompletionHandler devicePickerCompletionHandler);
	}
}
