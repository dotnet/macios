//
// Copyright 2011, Novell, Inc.
// Copyright 2011, Regan Sarwas
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

//
// imagekit.cs: Bindings for the Image Kit API
//
using System;
using AppKit;
using Foundation;
using ObjCRuntime;
using CoreImage;
using ImageCaptureCore;
using CoreGraphics;
using CoreAnimation;

namespace ImageKit {

	enum IKToolMode { // Constants introduced in 10.5 and 10.6
		/// <summary>To be added.</summary>
		[Field ("IKToolModeAnnotate")]
		Annotate,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeCrop")]
		Crop,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeMove")]
		Move,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeNone")]
		None,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeRotate")]
		Rotate,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeSelect")]
		Select,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeSelectEllipse")]
		SelectEllipse,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeSelectLasso")]
		SelectLasso,

		/// <summary>To be added.</summary>
		[Field ("IKToolModeSelectRect")]
		SelectRect,
	}

	enum IKOverlayType { // Constants introduced in 10.5
		/// <summary>To be added.</summary>
		[Field ("IKOverlayTypeBackground")]
		Background,

		/// <summary>To be added.</summary>
		[Field ("IKOverlayTypeImage")]
		Image,
	}

	[BaseType (typeof (NSView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (IKCameraDeviceViewDelegate) })]
	interface IKCameraDeviceView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDelegate")]
		IIKCameraDeviceViewDelegate Delegate { get; set; }

		[Export ("cameraDevice", ArgumentSemantic.Assign)]
		ICCameraDevice CameraDevice { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasDisplayModeTable")]
		bool HasDisplayModeTable { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasDisplayModeIcon")]
		bool HasDisplayModeIcon { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("downloadAllControlLabel", ArgumentSemantic.Copy)]
		string DownloadAllControlLabel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("downloadSelectedControlLabel", ArgumentSemantic.Copy)]
		string DownloadSelectedControlLabel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("iconSize")]
		nint IconSize { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transferMode")]
		IKCameraDeviceViewTransferMode TransferMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysDownloadsDirectoryControl")]
		bool DisplaysDownloadsDirectoryControl { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("downloadsDirectory", ArgumentSemantic.Retain)]
		NSUrl DownloadsDirectory { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysPostProcessApplicationControl")]
		bool DisplaysPostProcessApplicationControl { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("postProcessApplication", ArgumentSemantic.Retain)]
		NSUrl PostProcessApplication { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("canRotateSelectedItemsLeft")]
		bool CanRotateSelectedItemsLeft { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("canRotateSelectedItemsRight")]
		bool CanRotateSelectedItemsRight { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("canDeleteSelectedItems")]
		bool CanDeleteSelectedItems { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("canDownloadSelectedItems")]
		bool CanDownloadSelectedItems { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("selectedIndexes")]
		NSIndexSet SelectedIndexes { get; }

		/// <param name="indexes">To be added.</param>
		///         <param name="extendSelection">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("selectIndexes:byExtendingSelection:")]
		void SelectItemsAt (NSIndexSet indexes, bool extendSelection);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("rotateLeft:")]
		void RotateLeft (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("rotateRight:")]
		void RotateRight (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("deleteSelectedItems:")]
		void DeleteSelectedItems (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("downloadSelectedItems:")]
		void DownloadSelectedItems (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("downloadAllItems:")]
		void DownloadAllItems (NSObject sender);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mode", ArgumentSemantic.Assign)]
		IKCameraDeviceViewDisplayMode Mode { get; set; }

		[Export ("setCustomActionControl:")]
		void SetCustomActionControl (NSSegmentedControl control);

		[Export ("setCustomDeleteControl:")]
		void SetCustomDelete (NSSegmentedControl control);

		[Export ("setCustomIconSizeSlider:")]
		void SetCustomIconSizeSlider (NSSlider slider);

		[Export ("setCustomModeControl:")]
		void SetCustomModeControl (NSSegmentedControl control);

		[Export ("setCustomRotateControl:")]
		void SetCustomRotateControl (NSSegmentedControl control);

		[Export ("setShowStatusInfoAsWindowSubtitle:")]
		void SetShowStatusInfoAsWindowSubtitle (bool value);
	}

	interface IIKCameraDeviceViewDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface IKCameraDeviceViewDelegate {
		/// <param name="cameraDeviceView">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cameraDeviceViewSelectionDidChange:"), EventArgs ("IKCameraDeviceView", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void SelectionDidChange (IKCameraDeviceView cameraDeviceView);

		[Export ("cameraDeviceView:didDownloadFile:location:fileData:error:"), EventArgs ("IKCameraDeviceViewICCameraFileNSUrlNSDataNSError")]
		void DidDownloadFile (IKCameraDeviceView cameraDeviceView, ICCameraFile file, NSUrl url, NSData data, NSError error);

		/// <param name="cameraDeviceView">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("cameraDeviceView:didEncounterError:"), EventArgs ("IKCameraDeviceViewNSError", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void DidEncounterError (IKCameraDeviceView cameraDeviceView, NSError error);
	}

	[BaseType (typeof (NSView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (IKDeviceBrowserViewDelegate) })]
	interface IKDeviceBrowserView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDelegate")]
		IIKDeviceBrowserViewDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysLocalCameras")]
		bool DisplaysLocalCameras { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysNetworkCameras")]
		bool DisplaysNetworkCameras { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysLocalScanners")]
		bool DisplaysLocalScanners { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysNetworkScanners")]
		bool DisplaysNetworkScanners { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mode")]
		IKDeviceBrowserViewDisplayMode Mode { get; set; }

		[Export ("selectedDevice")]
		ICDevice SelectedDevice { get; }
	}

	interface IIKDeviceBrowserViewDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface IKDeviceBrowserViewDelegate {
		[Abstract]
		[Export ("deviceBrowserView:selectionDidChange:"), EventArgs ("IKDeviceBrowserViewICDevice")]
		void SelectionDidChange (IKDeviceBrowserView deviceBrowserView, ICDevice device);

		/// <param name="deviceBrowserView">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("deviceBrowserView:didEncounterError:"), EventArgs ("IKDeviceBrowserViewNSError", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void DidEncounterError (IKDeviceBrowserView deviceBrowserView, NSError error);
	}

	[BaseType (typeof (NSPanel))]
	interface IKFilterBrowserPanel {
		/// <param name="styleMask">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("filterBrowserPanelWithStyleMask:")]
		IKFilterBrowserPanel Create (IKFilterBrowserPanelStyleMask styleMask);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("filterName")]
		string FilterName { get; }

		//FIXME - can we do this in a more C#ish way.
		/// <param name="options">To be added.</param>
		///         <param name="modelessDelegate">To be added.</param>
		///         <param name="didEndSelector">To be added.</param>
		///         <param name="contextInfo">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginWithOptions:modelessDelegate:didEndSelector:contextInfo:")]
		void Begin (NSDictionary options, NSObject modelessDelegate, Selector didEndSelector, IntPtr contextInfo);

		/// <param name="options">To be added.</param>
		///         <param name="docWindow">To be added.</param>
		///         <param name="modalDelegate">To be added.</param>
		///         <param name="didEndSelector">To be added.</param>
		///         <param name="contextInfo">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginSheetWithOptions:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSDictionary options, NSWindow docWindow, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("runModalWithOptions:")]
		int RunModal (NSDictionary options); /* int, not NSInteger */

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("filterBrowserViewWithOptions:")]
		IKFilterBrowserView FilterBrowserView (NSDictionary options);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("finish:")]
		void Finish (NSObject sender);

		//Check - Do we need Notifications strings?
		[Notification]
		[Field ("IKFilterBrowserFilterSelectedNotification")]
		NSString FilterSelectedNotification { get; }

		[Notification]
		[Field ("IKFilterBrowserFilterDoubleClickNotification")]
		NSString FilterDoubleClickNotification { get; }

		[Notification]
		[Field ("IKFilterBrowserWillPreviewFilterNotification")]
		NSString WillPreviewFilterNotification { get; }

		//Dictionary Keys
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKFilterBrowserShowCategories")]
		NSString ShowCategories { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKFilterBrowserShowPreview")]
		NSString ShowPreview { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKFilterBrowserExcludeCategories")]
		NSString ExcludeCategories { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKFilterBrowserExcludeFilters")]
		NSString ExcludeFilters { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKFilterBrowserDefaultInputImage")]
		NSString DefaultInputImage { get; }
	}

	[BaseType (typeof (NSView))]
	interface IKFilterBrowserView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		/// <param name="showPreview">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setPreviewState:")]
		void SetPreviewState (bool showPreview);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("filterName")]
		string FilterName { get; }
	}

	//This protocol is an addition to CIFilter.  It is implemented by any filter that provides its own user interface.
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface IKFilterCustomUIProvider {
		// The Apple documentation says the native implementation of CIFilter.GetFilterUIView will call
		// this method (if it exists). This means that This method should not be called GetFilterUIView
		// (because it seems like you shouldn't override CIFilter.GetFilterUIView, and implementing
		// IIKFilterCustomUIProvider.GetFilterUIView in a CIFilter subclass without overriding CIFilter.GetFilterUIView
		// just turns ugly). So rename this for new-style assemblies to ProvideFilterUIView.
		/// <param name="configurationOptions">To be added.</param>
		///         <param name="excludedKeys">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("provideViewForUIConfiguration:excludedKeys:")]
		IKFilterUIView ProvideFilterUIView (NSDictionary configurationOptions, [NullAllowed] NSArray excludedKeys);

		//UIConfiguration keys for NSDictionary
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKUISizeFlavor")]
		NSString SizeFlavor { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKUISizeMini")]
		NSString SizeMini { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKUISizeSmall")]
		NSString SizeSmall { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKUISizeRegular")]
		NSString SizeRegular { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKUImaxSize")]
		NSString MaxSize { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKUIFlavorAllowFallback")]
		NSString FlavorAllowFallback { get; }
	}

	[BaseType (typeof (NSView))]
	interface IKFilterUIView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		/// <param name="frame">To be added.</param>
		/// <param name="filter">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:filter:")]
		NativeHandle Constructor (CGRect frame, CIFilter filter);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("filter")]
		CIFilter Filter { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("objectController")]
		NSObjectController ObjectController { get; }

		/// <param name="frame">To be added.</param>
		///         <param name="filter">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("viewWithFrame:filter:")]
		IKFilterUIView Create (CGRect frame, CIFilter filter);
	}

	[BaseType (typeof (NSObject))]
	interface IKImageBrowserCell {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageBrowserView")]
		IKImageBrowserView ImageBrowserView { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("representedItem")]
		NSObject RepresentedItem { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexOfRepresentedItem")]
		nint IndexOfRepresentedItem { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("frame")]
		CGRect Frame { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageContainerFrame")]
		CGRect ImageContainerFrame { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageFrame")]
		CGRect ImageFrame { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("selectionFrame")]
		CGRect SelectionFrame { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("titleFrame")]
		CGRect TitleFrame { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("subtitleFrame")]
		CGRect SubtitleFrame { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageAlignment")]
		NSImageAlignment ImageAlignment { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isSelected")]
		bool IsSelected { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cellState")]
		IKImageBrowserCellState CellState { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("opacity")]
		nfloat Opacity { get; }

		/// <param name="layerType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("layerForType:")]
		CALayer Layer (string layerType);

		// layerType is one of the following
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellBackgroundLayer")]
		NSString BackgroundLayer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellForegroundLayer")]
		NSString ForegroundLayer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellSelectionLayer")]
		NSString SelectionLayer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellPlaceHolderLayer")]
		NSString PlaceHolderLayer { get; }
	}

	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'NSCollectionView' instead.")]
	[BaseType (typeof (NSView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (IKImageBrowserDelegate) })]
	interface IKImageBrowserView : NSDraggingSource {
		//@category IKImageBrowserView (IKMainMethods)
		/// <param name="frame">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		//Having a weak and strong datasource seems to work.
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dataSource", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDataSource { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDataSource")]
		IIKImageBrowserDataSource DataSource { get; set; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reloadData")]
		void ReloadData ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDelegate")]
		IIKImageBrowserDelegate Delegate { get; set; }

		//@category IKImageBrowserView (IKAppearance)
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cellsStyleMask")]
		IKCellsStyle CellsStyleMask { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("constrainsToOriginalSize")]
		bool ConstrainsToOriginalSize { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("backgroundLayer")]
		CALayer BackgroundLayer { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("foregroundLayer")]
		CALayer ForegroundLayer { get; set; }

		/// <param name="representedItem">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("newCellForRepresentedItem:")]
		IKImageBrowserCell NewCell (IIKImageBrowserItem representedItem);

		/// <param name="itemIndex">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("cellForItemAtIndex:")]
		IKImageBrowserCell GetCellAt (nint itemIndex);

		//@category IKImageBrowserView (IKBrowsing)
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("zoomValue")]
		float ZoomValue { get; set; } /* float, not CGFloat */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contentResizingMask")]
		NSViewResizingMask ContentResizingMask { get; set; }

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("scrollIndexToVisible:")]
		void ScrollIndexToVisible (nint index);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("cellSize")]
		CGSize CellSize { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("intercellSpacing")]
		CGSize IntercellSpacing { get; set; }

		/// <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("indexOfItemAtPoint:")]
		nint GetIndexOfItem (CGPoint point);

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("itemFrameAtIndex:")]
		CGRect GetItemFrame (nint index);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("visibleItemIndexes")]
		NSIndexSet GetVisibleItemIndexes ();

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("rowIndexesInRect:")]
		NSIndexSet GetRowIndexes (CGRect rect);

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("columnIndexesInRect:")]
		NSIndexSet GetColumnIndexes (CGRect rect);

		/// <param name="columnIndex">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("rectOfColumn:")]
		CGRect GetRectOfColumn (nint columnIndex);

		/// <param name="rowIndex">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("rectOfRow:")]
		CGRect GetRectOfRow (nint rowIndex);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfRows")]
		nint RowCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfColumns")]
		nint ColumnCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("canControlQuickLookPanel")]
		bool CanControlQuickLookPanel { get; set; }

		//@category IKImageBrowserView (IKSelectionReorderingAndGrouping)
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("selectionIndexes")]
		NSIndexSet SelectionIndexes { get; }

		/// <param name="indexes">To be added.</param>
		///         <param name="extendSelection">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setSelectionIndexes:byExtendingSelection:")]
		void SelectItemsAt (NSIndexSet indexes, bool extendSelection);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsEmptySelection")]
		bool AllowsEmptySelection { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsReordering")]
		bool AllowsReordering { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("animates")]
		bool Animates { get; set; }

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("expandGroupAtIndex:")]
		void ExpandGroup (nint index);

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("collapseGroupAtIndex:")]
		void CollapseGroup (nint index);

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isGroupExpandedAtIndex:")]
		bool IsGroupExpanded (nint index);

		//@category IKImageBrowserView (IKDragNDrop)
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("draggingDestinationDelegate", ArgumentSemantic.Weak)]
		INSDraggingDestination DraggingDestinationDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("indexAtLocationOfDroppedItem")]
		nint GetIndexAtLocationOfDroppedItem ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("dropOperation")]
		IKImageBrowserDropOperation DropOperation ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsDroppingOnItems")]
		bool AllowsDroppingOnItems { get; set; }

		/// <param name="index">To be added.</param>
		/// <param name="operation">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setDropIndex:dropOperation:")]
		void SetDropIndex (nint index, IKImageBrowserDropOperation operation);

		// Keys for the view options, set with base.setValue
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserBackgroundColorKey")]
		NSString BackgroundColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserSelectionColorKey")]
		NSString SelectionColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellsOutlineColorKey")]
		NSString CellsOutlineColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellsTitleAttributesKey")]
		NSString CellsTitleAttributesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellsHighlightedTitleAttributesKey")]
		NSString CellsHighlightedTitleAttributesKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCellsSubtitleAttributesKey")]
		NSString CellsSubtitleAttributesKey { get; }
	}

	interface IIKImageBrowserDataSource { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol (IsInformal = true)]
	interface IKImageBrowserDataSource {
		/// <param name="aBrowser">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("numberOfItemsInImageBrowser:")]
		nint ItemCount (IKImageBrowserView aBrowser);

		/// <param name="aBrowser">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("imageBrowser:itemAtIndex:")]
		IIKImageBrowserItem GetItem (IKImageBrowserView aBrowser, nint index);

		/// <param name="aBrowser">To be added.</param>
		///         <param name="indexes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("imageBrowser:removeItemsAtIndexes:")]
		void RemoveItems (IKImageBrowserView aBrowser, NSIndexSet indexes);

		/// <param name="aBrowser">To be added.</param>
		/// <param name="indexes">To be added.</param>
		/// <param name="destinationIndex">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("imageBrowser:moveItemsAtIndexes:toIndex:")]
		bool MoveItems (IKImageBrowserView aBrowser, NSIndexSet indexes, nint destinationIndex);

		/// <param name="aBrowser">To be added.</param>
		///         <param name="itemIndexes">To be added.</param>
		///         <param name="pasteboard">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("imageBrowser:writeItemsAtIndexes:toPasteboard:")]
		nint WriteItemsToPasteboard (IKImageBrowserView aBrowser, NSIndexSet itemIndexes, NSPasteboard pasteboard);

		/// <param name="aBrowser">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfGroupsInImageBrowser:")]
		nint GroupCount (IKImageBrowserView aBrowser);

		/// <param name="aBrowser">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("imageBrowser:groupAtIndex:")]
		NSDictionary GetGroup (IKImageBrowserView aBrowser, nint index);

		// Keys for Dictionary returned by GetGroup
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserGroupRangeKey")]
		NSString GroupRangeKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserGroupBackgroundColorKey")]
		NSString GroupBackgroundColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserGroupTitleKey")]
		NSString GroupTitleKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserGroupStyleKey")]
		NSString GroupStyleKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserGroupHeaderLayer")]
		NSString GroupHeaderLayer { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserGroupFooterLayer")]
		NSString GroupFooterLayer { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol (IsInformal = true)]
	interface IKImageBrowserItem {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("imageUID")]
		string ImageUID { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("imageRepresentationType")]
		NSString ImageRepresentationType { get; }

		//possible strings returned by ImageRepresentationType
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserPathRepresentationType")]
		NSString PathRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserNSURLRepresentationType")]
		NSString NSURLRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserNSImageRepresentationType")]
		NSString NSImageRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCGImageRepresentationType")]
		NSString CGImageRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserCGImageSourceRepresentationType")]
		NSString CGImageSourceRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserNSDataRepresentationType")]
		NSString NSDataRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserNSBitmapImageRepresentationType")]
		NSString NSBitmapImageRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserQTMovieRepresentationType")]
		NSString QTMovieRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserQTMoviePathRepresentationType")]
		NSString QTMoviePathRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserQCCompositionRepresentationType")]
		NSString QCCompositionRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserQCCompositionPathRepresentationType")]
		NSString QCCompositionPathRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserQuickLookPathRepresentationType")]
		NSString QuickLookPathRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserIconRefPathRepresentationType")]
		NSString IconRefPathRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserIconRefRepresentationType")]
		NSString IconRefRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKImageBrowserPDFPageRepresentationType")]
		NSString PDFPageRepresentationType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("imageRepresentation")]
		NSObject ImageRepresentation { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageVersion")]
		nint ImageVersion { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageTitle")]
		string ImageTitle { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageSubtitle")]
		string ImageSubtitle { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isSelectable")]
		bool IsSelectable { get; }
	}

	interface IIKImageBrowserItem { }

	interface IIKImageBrowserDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol (IsInformal = true)]
	interface IKImageBrowserDelegate {
		/// <param name="browser">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("imageBrowserSelectionDidChange:"), EventArgs ("IKImageBrowserView", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void SelectionDidChange (IKImageBrowserView browser);

		/// <param name="browser">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("imageBrowser:cellWasDoubleClickedAtIndex:"), EventArgs ("IKImageBrowserViewIndex", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void CellWasDoubleClicked (IKImageBrowserView browser, nint index);

		/// <param name="browser">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <param name="nsevent">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("imageBrowser:cellWasRightClickedAtIndex:withEvent:"), EventArgs ("IKImageBrowserViewIndexEvent", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void CellWasRightClicked (IKImageBrowserView browser, nint index, NSEvent nsevent);

		/// <param name="browser">To be added.</param>
		///         <param name="nsevent">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("imageBrowser:backgroundWasRightClickedWithEvent:"), EventArgs ("IKImageBrowserViewEvent", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void BackgroundWasRightClicked (IKImageBrowserView browser, NSEvent nsevent);
	}

	[BaseType (typeof (NSPanel))]
	[DisableDefaultCtor] // crash when disposed, sharedImageEditPanel must be used
	interface IKImageEditPanel {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedImageEditPanel")]
		IKImageEditPanel SharedPanel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("dataSource", ArgumentSemantic.Assign), NullAllowed]
		IIKImageEditPanelDataSource DataSource { get; set; }

#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Obsolete ("Use the 'FilterArray' property instead.")]
		[Wrap ("FilterArray", IsVirtual = true)]
		NSArray filterArray { get; }
#endif

		[Export ("filterArray")]
		NSArray FilterArray { get; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reloadData")]
		void ReloadData ();
	}

	interface IIKImageEditPanelDataSource { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface IKImageEditPanelDataSource {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("image")]
		CGImage Image { get; }

		/// <param name="image">To be added.</param>
		///         <param name="metaData">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setImage:imageProperties:")]
		void SetImageAndProperties (CGImage image, NSDictionary metaData);

		/// <param name="maximumSize">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("thumbnailWithMaximumSize:")]
		CGImage GetThumbnail (CGSize maximumSize);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageProperties")]
		NSDictionary ImageProperties { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasAdjustMode")]
		bool HasAdjustMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasEffectsMode")]
		bool HasEffectsMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasDetailsMode")]
		bool HasDetailsMode { get; }
	}

	[BaseType (typeof (NSView))]
	interface IKImageView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		//There is no protocol for this delegate.  used to respond to messages in the responder chain
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("zoomFactor")]
		nfloat ZoomFactor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rotationAngle")]
		nfloat RotationAngle { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("currentToolMode")]
		string CurrentToolMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autoresizes")]
		bool Autoresizes { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasHorizontalScroller")]
		bool HasHorizontalScroller { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasVerticalScroller")]
		bool HasVerticalScroller { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("autohidesScrollers")]
		bool AutohidesScrollers { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("supportsDragAndDrop")]
		bool SupportsDragAndDrop { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("editable")]
		bool Editable { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("doubleClickOpensImageEditPanel")]
		bool DoubleClickOpensImageEditPanel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageCorrection", ArgumentSemantic.Assign)]
		CIFilter ImageCorrection { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("backgroundColor", ArgumentSemantic.Assign)]
		NSColor BackgroundColor { get; set; }

		[Export ("setImage:imageProperties:")]
		void SetImage (CGImage image, NSDictionary metaData);

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setImageWithURL:")]
		void SetImageWithURL (NSUrl url);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("image")]
		CGImage Image { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageSize")]
		CGSize ImageSize { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageProperties")]
		NSDictionary ImageProperties { get; }

		/// <param name="rotationAngle">To be added.</param>
		/// <param name="centerPoint">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setRotationAngle:centerPoint:")]
		void SetRotation (nfloat rotationAngle, CGPoint centerPoint);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("rotateImageLeft:")]
		void RotateImageLeft (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("rotateImageRight:")]
		void RotateImageRight (NSObject sender);

		/// <param name="zoomFactor">To be added.</param>
		/// <param name="centerPoint">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("setImageZoomFactor:centerPoint:")]
		void SetImageZoomFactor (nfloat zoomFactor, CGPoint centerPoint);

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("zoomImageToRect:")]
		void ZoomImageToRect (CGRect rect);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("zoomImageToFit:")]
		void ZoomImageToFit (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("zoomImageToActualSize:")]
		void ZoomImageToActualSize (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("zoomIn:")]
		void ZoomIn (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("zoomOut:")]
		void ZoomOut (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("flipImageHorizontal:")]
		void FlipImageHorizontal (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("flipImageVertical:")]
		void FlipImageVertical (NSObject sender);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("crop:")]
		void Crop (NSObject sender);

		/// <param name="layer">To be added.</param>
		///         <param name="layerType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("setOverlay:forType:")]
		void SetOverlay (CALayer layer, string layerType);

		/// <param name="layerType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("overlayForType:")]
		CALayer GetOverlay (string layerType);

		/// <param name="point">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scrollToPoint:")]
		void ScrollTo (CGPoint point);

		/// <param name="rect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scrollToRect:")]
		void ScrollTo (CGRect rect);

		/// <param name="viewPoint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertViewPointToImagePoint:")]
		CGPoint ConvertViewPointToImagePoint (CGPoint viewPoint);

		/// <param name="viewRect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertViewRectToImageRect:")]
		CGRect ConvertViewRectToImageRect (CGRect viewRect);

		/// <param name="imagePoint">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertImagePointToViewPoint:")]
		CGPoint ConvertImagePointToViewPoint (CGPoint imagePoint);

		/// <param name="imageRect">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("convertImageRectToViewRect:")]
		CGRect ConvertImageRectToViewRect (CGRect imageRect);
	}

	[BaseType (typeof (NSPanel))]
	interface IKPictureTaker {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("pictureTaker")]
		IKPictureTaker SharedPictureTaker { get; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("runModal")]
		nint RunModal ();

		//FIXME - Yuck.  What can I do to fix these three methods?
		/// <param name="aDelegate">To be added.</param>
		///         <param name="didEndSelector">To be added.</param>
		///         <param name="contextInfo">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginPictureTakerWithDelegate:didEndSelector:contextInfo:")]
		void BeginPictureTaker (NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		/// <param name="aWindow">To be added.</param>
		///         <param name="aDelegate">To be added.</param>
		///         <param name="didEndSelector">To be added.</param>
		///         <param name="contextInfo">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("beginPictureTakerSheetForWindow:withDelegate:didEndSelector:contextInfo:")]
		void BeginPictureTakerSheet (NSWindow aWindow, NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		/// <param name="aView">To be added.</param>
		///         <param name="aDelegate">To be added.</param>
		///         <param name="didEndSelector">To be added.</param>
		///         <param name="contextInfo">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("popUpRecentsMenuForView:withDelegate:didEndSelector:contextInfo:")]
		void PopUpRecentsMenu (NSView aView, NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inputImage")]
		NSImage InputImage { get; set; }

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("outputImage")]
		NSImage GetOutputImage ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mirroring")]
		bool Mirroring { get; set; }

		//Use with NSKeyValueCoding to customize the pictureTaker panel
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerAllowsVideoCaptureKey")]
		NSString AllowsVideoCaptureKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerAllowsFileChoosingKey")]
		NSString AllowsFileChoosingKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerShowRecentPictureKey")]
		NSString ShowRecentPictureKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerUpdateRecentPictureKey")]
		NSString UpdateRecentPictureKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerAllowsEditingKey")]
		NSString AllowsEditingKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerShowEffectsKey")]
		NSString ShowEffectsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerInformationalTextKey")]
		NSString InformationalTextKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerImageTransformsKey")]
		NSString ImageTransformsKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerOutputImageMaxSizeKey")]
		NSString OutputImageMaxSizeKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerCropAreaSizeKey")]
		NSString CropAreaSizeKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerShowAddressBookPictureKey")]
		NSString ShowAddressBookPictureKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerShowEmptyPictureKey")]
		NSString ShowEmptyPictureKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKPictureTakerRemainOpenAfterValidateKey")]
		NSString RemainOpenAfterValidateKey { get; }
	}

	[BaseType (typeof (NSObject), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (IKSaveOptionsDelegate) })]
	interface IKSaveOptions {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageProperties")]
		NSDictionary ImageProperties { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("imageUTType")]
		string ImageUTType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userSelection")]
		NSDictionary UserSelection { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDelegate")]
		IIKSaveOptionsDelegate Delegate { get; set; }

		/// <param name="imageProperties">To be added.</param>
		/// <param name="imageUTType">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithImageProperties:imageUTType:")]
		NativeHandle Constructor (NSDictionary imageProperties, string imageUTType);

		/// <param name="savePanel">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addSaveOptionsAccessoryViewToSavePanel:")]
		void AddSaveOptionsToPanel (NSSavePanel savePanel);

		/// <param name="view">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addSaveOptionsToView:")]
		void AddSaveOptionsToView (NSView view);

		[Export ("rememberLastSetting")]
		bool RememberLastSetting { get; set; }
	}

	interface IIKSaveOptionsDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol (IsInformal = true)]
	interface IKSaveOptionsDelegate {
		/// <param name="saveOptions">To be added.</param>
		///         <param name="imageUTType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>To be added.</summary>
			<value>To be added.</value>
			<remarks>To be added.</remarks>
			""")]
		[Export ("saveOptions:shouldShowUTType:"), DelegateName ("SaveOptionsShouldShowUTType"), DefaultValue (false)]
		bool ShouldShowType (IKSaveOptions saveOptions, string imageUTType);
	}

	[BaseType (typeof (NSView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (IKScannerDeviceViewDelegate) })]
	interface IKScannerDeviceView {
		/// <param name="frameRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDelegate")]
		IIKScannerDeviceViewDelegate Delegate { get; set; }

		[Export ("scannerDevice", ArgumentSemantic.Assign)]
		ICScannerDevice ScannerDevice { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mode")]
		IKScannerDeviceViewDisplayMode DisplayMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasDisplayModeSimple")]
		bool HasDisplayModeSimple { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasDisplayModeAdvanced")]
		bool HasDisplayModeAdvanced { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transferMode")]
		IKScannerDeviceViewTransferMode TransferMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("scanControlLabel", ArgumentSemantic.Copy)]
		string ScanControlLabel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("overviewControlLabel", ArgumentSemantic.Copy)]
		string OverviewControlLabel { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysDownloadsDirectoryControl")]
		bool DisplaysDownloadsDirectoryControl { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("downloadsDirectory", ArgumentSemantic.Retain)]
		NSUrl DownloadsDirectory { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("documentName", ArgumentSemantic.Copy)]
		string DocumentName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysPostProcessApplicationControl")]
		bool DisplaysPostProcessApplicationControl { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("postProcessApplication", ArgumentSemantic.Retain)]
		NSUrl PostProcessApplication { get; set; }
	}

	interface IIKScannerDeviceViewDelegate { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface IKScannerDeviceViewDelegate {
		/// <param name="scannerDeviceView">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="data">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scannerDeviceView:didScanToURL:fileData:error:"), EventArgs ("IKScannerDeviceViewScan", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void DidScan (IKScannerDeviceView scannerDeviceView, NSUrl url, NSData data, NSError error);

		/// <param name="scannerDeviceView">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scannerDeviceView:didEncounterError:"), EventArgs ("IKScannerDeviceViewError", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void DidEncounterError (IKScannerDeviceView scannerDeviceView, NSError error);

		/// <param name="scannerDeviceView">To be added.</param>
		///         <param name="url">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("scannerDeviceView:didScanToURL:error:"), EventArgs ("IKScannerDeviceViewScanUrl", XmlDocs = """
			<summary>To be added.</summary>
			<remarks>To be added.</remarks>
			""")]
		void DidScanToUrl (IKScannerDeviceView scannerDeviceView, NSUrl url, NSError error);

		[Export ("scannerDeviceView:didScanToBandData:scanInfo:error:"), EventArgs ("IKScannerDeviceViewScanBandData")]
		void DidScanToBandData (IKScannerDeviceView scannerDeviceView, ICScannerBandData data, NSDictionary scanInfo, NSError error);
	}

	[BaseType (typeof (NSObject))]
	interface IKSlideshow {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("sharedSlideshow")]
		IKSlideshow SharedSlideshow { get; }

#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Obsolete ("Use the 'AutoPlayDelay' property instead.")]
		[Wrap ("AutoPlayDelay", IsVirtual = true)]
		double autoPlayDelay { get; set; }
#endif

		[Export ("autoPlayDelay")]
		double AutoPlayDelay { get; set; }

		/// <param name="dataSource">To be added.</param>
		///         <param name="slideshowMode">To be added.</param>
		///         <param name="slideshowOptions">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("runSlideshowWithDataSource:inMode:options:")]
		void RunSlideshow (IIKSlideshowDataSource dataSource, string slideshowMode, NSDictionary slideshowOptions);

		/// <param name="sender">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("stopSlideshow:")]
		void StopSlideshow (NSObject sender);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("reloadData")]
		void ReloadData ();

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("reloadSlideshowItemAtIndex:")]
		void ReloadSlideshowItem (nint index);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexOfCurrentSlideshowItem")]
		nint IndexOfCurrentSlideshowItem { get; }

		/// <param name="applicationBundleIdentifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canExportToApplication:")]
		bool CanExportToApplication (string applicationBundleIdentifier);

		/// <param name="item">To be added.</param>
		///         <param name="applicationBundleIdentifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("exportSlideshowItem:toApplication:")]
		void ExportSlideshowItemtoApplication (NSObject item, string applicationBundleIdentifier);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowModeImages")]
		NSString ModeImages { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowModePDF")]
		NSString ModePDF { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowModeOther")]
		NSString ModeOther { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowWrapAround")]
		NSString WrapAround { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowStartPaused")]
		NSString StartPaused { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowStartIndex")]
		NSString StartIndex { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowScreen")]
		NSString Screen { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowAudioFile")]
		NSString AudioFile { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowPDFDisplayBox")]
		NSString PDFDisplayBox { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowPDFDisplayMode")]
		NSString PDFDisplayMode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IKSlideshowPDFDisplaysAsBook")]
		NSString PDFDisplaysAsBook { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IK_iPhotoBundleIdentifier")]
		NSString IPhotoBundleIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IK_ApertureBundleIdentifier")]
		NSString ApertureBundleIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IK_MailBundleIdentifier")]
		NSString MailBundleIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("IK_PhotosBundleIdentifier")]
		NSString PhotosBundleIdentifier { get; }
	}

	interface IIKSlideshowDataSource { }

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface IKSlideshowDataSource {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("numberOfSlideshowItems")]
		nint ItemCount { get; }

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("slideshowItemAtIndex:")]
		NSObject GetItemAt (nint index);

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("nameOfSlideshowItemAtIndex:")]
		string GetNameOfItemAt (nint index);

		/// <param name="index">To be added.</param>
		/// <param name="applicationBundleIdentifier">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("canExportSlideshowItemAtIndex:toApplication:")]
		bool CanExportItemToApplication (nint index, string applicationBundleIdentifier);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("slideshowWillStart")]
		void WillStart ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("slideshowDidStop")]
		void DidStop ();

		/// <param name="newIndex">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("slideshowDidChangeCurrentIndex:")]
		void DidChange (nint newIndex);
	}
}
