using XamCore.ObjCRuntime;
using XamCore.Foundation;
using XamCore.CoreGraphics;
using XamCore.AppKit;

using System;
using System.ComponentModel;

namespace XamCore.QuickLookUI {
	[BaseType (typeof (NSObject))]
	[Protocol, Model]
	interface QLPreviewPanelDataSource {
		[Export ("numberOfPreviewItemsInPreviewPanel:")]
		[Abstract]
		nint NumberOfPreviewItemsInPreviewPanel (QLPreviewPanel panel);

		[Export ("previewPanel:previewItemAtIndex:")]
		[Abstract]
		[return:Protocolize]
		QLPreviewItem PreviewItemAtIndex (QLPreviewPanel panel, nint index);
	}
	
	[BaseType (typeof (NSObject))]
	[Protocol, Model]
	interface QLPreviewPanelDelegate : NSWindowDelegate {
		[Export ("previewPanel:handleEvent:")]
		bool HandleEvent (QLPreviewPanel panel, NSEvent theEvent);

		[Export ("previewPanel:sourceFrameOnScreenForPreviewItem:")]
		CGRect SourceFrameOnScreenForPreviewItem (QLPreviewPanel panel, [Protocolize] QLPreviewItem item);

		[Export ("previewPanel:transitionImageForPreviewItem:contentRect:")]
		NSObject TransitionImageForPreviewItem (QLPreviewPanel panel, [Protocolize] QLPreviewItem item, CGRect contentRect);
	}
	
	[Mac (10,6)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface QLPreviewItem {
		[Export ("previewItemURL")]
		NSUrl PreviewItemURL { get; }

		[Export ("previewItemTitle")]
		string PreviewItemTitle { get; }

		[Export ("previewItemDisplayState")]
		NSObject PreviewItemDisplayState { get; }
	}
	
	[Category]
	[BaseType (typeof (NSObject))]
	interface QLPreviewPanelController {

		[Export ("acceptsPreviewPanelControl:")]
		bool AcceptsPreviewPanelControl (QLPreviewPanel panel);

		[Export ("beginPreviewPanelControl:")]
		void BeginPreviewPanelControl (QLPreviewPanel panel);

		[Export ("endPreviewPanelControl:")]
		void EndPreviewPanelControl (QLPreviewPanel panel);
	}

	[Mac (10,6)]
	[BaseType (typeof (NSPanel))]
	interface QLPreviewPanel {
		[Export ("currentController")]
		NSObject CurrentController { get; }
		
		[Export ("dataSource", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDataSource  { get; set; }
		
		[Wrap ("WeakDataSource")][NullAllowed]
		[Protocolize]
		QLPreviewPanelDataSource DataSource  { get; set; }

		[Export ("currentPreviewItemIndex")]
		nint CurrentPreviewItemIndex { get; set; }

		[Export ("currentPreviewItem")]
		[Protocolize]
		QLPreviewItem CurrentPreviewItem { get; }

		[Export ("displayState", ArgumentSemantic.Retain)]
		NSObject DisplayState { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")][NullAllowed]
		[Protocolize]
		QLPreviewPanelDelegate Delegate { get; set; }

		[Export ("inFullScreenMode")]
		bool InFullScreenMode { [Bind ("isInFullScreenMode")] get; }

		[Static, Export ("sharedPreviewPanel")]
		QLPreviewPanel SharedPreviewPanel ();

		[Static, Export ("sharedPreviewPanelExists")]
		bool SharedPreviewPanelExists ();

		[Export ("updateController")]
		void UpdateController ();

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("refreshCurrentPreviewItem")]
		void RefreshCurrentPreviewItem ();

		// @required - (BOOL)enterFullScreenMode:(NSScreen *)screen withOptions:(NSDictionary *)options;
		[Export ("enterFullScreenMode:withOptions:")]
		bool EnterFullScreenMode ([NullAllowed]NSScreen screen, [NullAllowed]NSDictionary options);

		// @required - (void)exitFullScreenModeWithOptions:(NSDictionary *)options;
		[Export ("exitFullScreenModeWithOptions:")]
		void ExitFullScreenModeWithOptions ([NullAllowed]NSDictionary options);
	}
}
