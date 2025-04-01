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

#if !NET
using NativeHandle = System.IntPtr;
#endif

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

		[Export ("selectIndexes:byExtendingSelection:")]
		void SelectItemsAt (NSIndexSet indexes, bool extendSelection);

		[Export ("rotateLeft:")]
		void RotateLeft (NSObject sender);

		[Export ("rotateRight:")]
		void RotateRight (NSObject sender);

		[Export ("deleteSelectedItems:")]
		void DeleteSelectedItems (NSObject sender);

		[Export ("downloadSelectedItems:")]
		void DownloadSelectedItems (NSObject sender);

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
		[Export ("cameraDeviceViewSelectionDidChange:"), EventArgs ("IKCameraDeviceView")]
		void SelectionDidChange (IKCameraDeviceView cameraDeviceView);

		[Export ("cameraDeviceView:didDownloadFile:location:fileData:error:"), EventArgs ("IKCameraDeviceViewICCameraFileNSUrlNSDataNSError")]
		void DidDownloadFile (IKCameraDeviceView cameraDeviceView, ICCameraFile file, NSUrl url, NSData data, NSError error);

		[Export ("cameraDeviceView:didEncounterError:"), EventArgs ("IKCameraDeviceViewNSError")]
		void DidEncounterError (IKCameraDeviceView cameraDeviceView, NSError error);
	}

	[BaseType (typeof (NSView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (IKDeviceBrowserViewDelegate) })]
	interface IKDeviceBrowserView {
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

		[Export ("deviceBrowserView:didEncounterError:"), EventArgs ("IKDeviceBrowserViewNSError")]
		void DidEncounterError (IKDeviceBrowserView deviceBrowserView, NSError error);
	}

	[BaseType (typeof (NSPanel))]
	interface IKFilterBrowserPanel {
		[Static]
		[Export ("filterBrowserPanelWithStyleMask:")]
		IKFilterBrowserPanel Create (IKFilterBrowserPanelStyleMask styleMask);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("filterName")]
		string FilterName { get; }

		//FIXME - can we do this in a more C#ish way.
		[Export ("beginWithOptions:modelessDelegate:didEndSelector:contextInfo:")]
		void Begin (NSDictionary options, NSObject modelessDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("beginSheetWithOptions:modalForWindow:modalDelegate:didEndSelector:contextInfo:")]
		void BeginSheet (NSDictionary options, NSWindow docWindow, NSObject modalDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("runModalWithOptions:")]
		int RunModal (NSDictionary options); /* int, not NSInteger */

		[Export ("filterBrowserViewWithOptions:")]
		IKFilterBrowserView FilterBrowserView (NSDictionary options);

		[Export ("finish:")]
		void Finish (NSObject sender);

		//Check - Do we need Notifications strings?
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Notification]
		[Field ("IKFilterBrowserFilterSelectedNotification")]
		NSString FilterSelectedNotification { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Notification]
		[Field ("IKFilterBrowserFilterDoubleClickNotification")]
		NSString FilterDoubleClickNotification { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

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
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frameRect);

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

		[Export ("newCellForRepresentedItem:")]
		IKImageBrowserCell NewCell (IIKImageBrowserItem representedItem);

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

		[Export ("indexOfItemAtPoint:")]
		nint GetIndexOfItem (CGPoint point);

		[Export ("itemFrameAtIndex:")]
		CGRect GetItemFrame (nint index);

		[Export ("visibleItemIndexes")]
		NSIndexSet GetVisibleItemIndexes ();

		[Export ("rowIndexesInRect:")]
		NSIndexSet GetRowIndexes (CGRect rect);

		[Export ("columnIndexesInRect:")]
		NSIndexSet GetColumnIndexes (CGRect rect);

		[Export ("rectOfColumn:")]
		CGRect GetRectOfColumn (nint columnIndex);

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

		[Export ("expandGroupAtIndex:")]
		void ExpandGroup (nint index);

		[Export ("collapseGroupAtIndex:")]
		void CollapseGroup (nint index);

		[Export ("isGroupExpandedAtIndex:")]
		bool IsGroupExpanded (nint index);

		//@category IKImageBrowserView (IKDragNDrop)
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("draggingDestinationDelegate", ArgumentSemantic.Weak)]
		INSDraggingDestination DraggingDestinationDelegate { get; set; }

		[Export ("indexAtLocationOfDroppedItem")]
		nint GetIndexAtLocationOfDroppedItem ();

		[Export ("dropOperation")]
		IKImageBrowserDropOperation DropOperation ();

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsDroppingOnItems")]
		bool AllowsDroppingOnItems { get; set; }

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
		[Abstract]
		[Export ("numberOfItemsInImageBrowser:")]
		nint ItemCount (IKImageBrowserView aBrowser);

		[Abstract]
		[Export ("imageBrowser:itemAtIndex:")]
		IIKImageBrowserItem GetItem (IKImageBrowserView aBrowser, nint index);

		[Export ("imageBrowser:removeItemsAtIndexes:")]
		void RemoveItems (IKImageBrowserView aBrowser, NSIndexSet indexes);

		[Export ("imageBrowser:moveItemsAtIndexes:toIndex:")]
		bool MoveItems (IKImageBrowserView aBrowser, NSIndexSet indexes, nint destinationIndex);

		[Export ("imageBrowser:writeItemsAtIndexes:toPasteboard:")]
		nint WriteItemsToPasteboard (IKImageBrowserView aBrowser, NSIndexSet itemIndexes, NSPasteboard pasteboard);

		[Export ("numberOfGroupsInImageBrowser:")]
		nint GroupCount (IKImageBrowserView aBrowser);

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
		[Export ("imageBrowserSelectionDidChange:"), EventArgs ("IKImageBrowserView")]
		void SelectionDidChange (IKImageBrowserView browser);

		[Export ("imageBrowser:cellWasDoubleClickedAtIndex:"), EventArgs ("IKImageBrowserViewIndex")]
		void CellWasDoubleClicked (IKImageBrowserView browser, nint index);

		[Export ("imageBrowser:cellWasRightClickedAtIndex:withEvent:"), EventArgs ("IKImageBrowserViewIndexEvent")]
		void CellWasRightClicked (IKImageBrowserView browser, nint index, NSEvent nsevent);

		[Export ("imageBrowser:backgroundWasRightClickedWithEvent:"), EventArgs ("IKImageBrowserViewEvent")]
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

		[Abstract]
		[Export ("setImage:imageProperties:")]
		void SetImageAndProperties (CGImage image, NSDictionary metaData);

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

#if !NET
		[Export ("setImage:imageProperties:")]
		void SetImageimageProperties (CGImage image, NSDictionary metaData);
#else
		[Export ("setImage:imageProperties:")]
		void SetImage (CGImage image, NSDictionary metaData);
#endif

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

		[Export ("setRotationAngle:centerPoint:")]
		void SetRotation (nfloat rotationAngle, CGPoint centerPoint);

		[Export ("rotateImageLeft:")]
		void RotateImageLeft (NSObject sender);

		[Export ("rotateImageRight:")]
		void RotateImageRight (NSObject sender);

		[Export ("setImageZoomFactor:centerPoint:")]
		void SetImageZoomFactor (nfloat zoomFactor, CGPoint centerPoint);

		[Export ("zoomImageToRect:")]
		void ZoomImageToRect (CGRect rect);

		[Export ("zoomImageToFit:")]
		void ZoomImageToFit (NSObject sender);

		[Export ("zoomImageToActualSize:")]
		void ZoomImageToActualSize (NSObject sender);

		[Export ("zoomIn:")]
		void ZoomIn (NSObject sender);

		[Export ("zoomOut:")]
		void ZoomOut (NSObject sender);

		[Export ("flipImageHorizontal:")]
		void FlipImageHorizontal (NSObject sender);

		[Export ("flipImageVertical:")]
		void FlipImageVertical (NSObject sender);

		[Export ("crop:")]
		void Crop (NSObject sender);

		[Export ("setOverlay:forType:")]
		void SetOverlay (CALayer layer, string layerType);

		[Export ("overlayForType:")]
		CALayer GetOverlay (string layerType);

		[Export ("scrollToPoint:")]
		void ScrollTo (CGPoint point);

		[Export ("scrollToRect:")]
		void ScrollTo (CGRect rect);

		[Export ("convertViewPointToImagePoint:")]
		CGPoint ConvertViewPointToImagePoint (CGPoint viewPoint);

		[Export ("convertViewRectToImageRect:")]
		CGRect ConvertViewRectToImageRect (CGRect viewRect);

		[Export ("convertImagePointToViewPoint:")]
		CGPoint ConvertImagePointToViewPoint (CGPoint imagePoint);

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

		[Export ("runModal")]
		nint RunModal ();

		//FIXME - Yuck.  What can I do to fix these three methods?
		[Export ("beginPictureTakerWithDelegate:didEndSelector:contextInfo:")]
		void BeginPictureTaker (NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("beginPictureTakerSheetForWindow:withDelegate:didEndSelector:contextInfo:")]
		void BeginPictureTakerSheet (NSWindow aWindow, NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		[Export ("popUpRecentsMenuForView:withDelegate:didEndSelector:contextInfo:")]
		void PopUpRecentsMenu (NSView aView, NSObject aDelegate, Selector didEndSelector, IntPtr contextInfo);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inputImage")]
		NSImage InputImage { get; set; }

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

		[Export ("initWithImageProperties:imageUTType:")]
		NativeHandle Constructor (NSDictionary imageProperties, string imageUTType);

		[Export ("addSaveOptionsAccessoryViewToSavePanel:")]
		void AddSaveOptionsToPanel (NSSavePanel savePanel);

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
		[Export ("saveOptions:shouldShowUTType:"), DelegateName ("SaveOptionsShouldShowUTType"), DefaultValue (false)]
		bool ShouldShowType (IKSaveOptions saveOptions, string imageUTType);
	}

	[BaseType (typeof (NSView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (IKScannerDeviceViewDelegate) })]
	interface IKScannerDeviceView {
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
		[Export ("scannerDeviceView:didScanToURL:fileData:error:"), EventArgs ("IKScannerDeviceViewScan")]
		void DidScan (IKScannerDeviceView scannerDeviceView, NSUrl url, NSData data, NSError error);

		[Export ("scannerDeviceView:didEncounterError:"), EventArgs ("IKScannerDeviceViewError")]
		void DidEncounterError (IKScannerDeviceView scannerDeviceView, NSError error);

		[Export ("scannerDeviceView:didScanToURL:error:"), EventArgs ("IKScannerDeviceViewScanUrl")]
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

		[Export ("runSlideshowWithDataSource:inMode:options:")]
		void RunSlideshow (IIKSlideshowDataSource dataSource, string slideshowMode, NSDictionary slideshowOptions);

		[Export ("stopSlideshow:")]
		void StopSlideshow (NSObject sender);

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("reloadSlideshowItemAtIndex:")]
		void ReloadSlideshowItem (nint index);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("indexOfCurrentSlideshowItem")]
		nint IndexOfCurrentSlideshowItem { get; }

		[Static]
		[Export ("canExportToApplication:")]
		bool CanExportToApplication (string applicationBundleIdentifier);

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

		[Abstract]
		[Export ("slideshowItemAtIndex:")]
		NSObject GetItemAt (nint index);

		[Export ("nameOfSlideshowItemAtIndex:")]
		string GetNameOfItemAt (nint index);

		[Export ("canExportSlideshowItemAtIndex:toApplication:")]
		bool CanExportItemToApplication (nint index, string applicationBundleIdentifier);

		[Export ("slideshowWillStart")]
		void WillStart ();

		[Export ("slideshowDidStop")]
		void DidStop ();

		[Export ("slideshowDidChangeCurrentIndex:")]
		void DidChange (nint newIndex);
	}
}
