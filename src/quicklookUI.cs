using ObjCRuntime;
using Foundation;
using CoreGraphics;
using AppKit;

using System;
using System.ComponentModel;
using UniformTypeIdentifiers;

namespace QuickLookUI {

	[Native]
	enum QLPreviewViewStyle : ulong {
		/// <summary>To be added.</summary>
		Normal = 0,
		/// <summary>To be added.</summary>
		Compact = 1,
	}

	interface IQLPreviewPanelDataSource { }

	[BaseType (typeof (NSObject))]
	[Protocol, Model]
	interface QLPreviewPanelDataSource {
		/// <param name="panel">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("numberOfPreviewItemsInPreviewPanel:")]
		[Abstract]
		nint NumberOfPreviewItemsInPreviewPanel (QLPreviewPanel panel);

		/// <param name="panel">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("previewPanel:previewItemAtIndex:")]
		[Abstract]
		IQLPreviewItem PreviewItemAtIndex (QLPreviewPanel panel, nint index);
	}

	interface IQLPreviewPanelDelegate { }

	[BaseType (typeof (NSObject))]
	[Protocol, Model]
	interface QLPreviewPanelDelegate : NSWindowDelegate {
		/// <param name="panel">To be added.</param>
		/// <param name="theEvent">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("previewPanel:handleEvent:")]
		bool HandleEvent (QLPreviewPanel panel, NSEvent theEvent);

		/// <param name="panel">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("previewPanel:sourceFrameOnScreenForPreviewItem:")]
		CGRect SourceFrameOnScreenForPreviewItem (QLPreviewPanel panel, IQLPreviewItem item);

		/// <param name="panel">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <param name="contentRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("previewPanel:transitionImageForPreviewItem:contentRect:")]
		NSObject TransitionImageForPreviewItem (QLPreviewPanel panel, IQLPreviewItem item, CGRect contentRect);
	}

	interface IQLPreviewItem { }

	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface QLPreviewItem {
		[Abstract]
		[Export ("previewItemURL")]
		NSUrl PreviewItemUrl { get; }

		[Export ("previewItemTitle")]
		string PreviewItemTitle { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Export ("previewItemDisplayState")]
		NSObject PreviewItemDisplayState { get; }
	}

	[Category]
	[BaseType (typeof (NSObject))]
	interface QLPreviewPanelController {

		/// <param name="panel">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("acceptsPreviewPanelControl:")]
		bool AcceptsPreviewPanelControl (QLPreviewPanel panel);

		/// <param name="panel">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("beginPreviewPanelControl:")]
		void BeginPreviewPanelControl (QLPreviewPanel panel);

		/// <param name="panel">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("endPreviewPanelControl:")]
		void EndPreviewPanelControl (QLPreviewPanel panel);
	}

	[BaseType (typeof (NSPanel))]
	interface QLPreviewPanel {
		[Export ("currentController")]
		NSObject CurrentController { get; }

		[Export ("dataSource", ArgumentSemantic.Assign)]
		[NullAllowed]
		NSObject WeakDataSource { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Wrap ("WeakDataSource")]
		[NullAllowed]
		IQLPreviewPanelDataSource DataSource { get; set; }

		[Export ("currentPreviewItemIndex")]
		nint CurrentPreviewItemIndex { get; set; }

		[Export ("currentPreviewItem")]
		IQLPreviewItem CurrentPreviewItem { get; }

		[Export ("displayState", ArgumentSemantic.Retain)]
		NSObject DisplayState { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IQLPreviewPanelDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
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
		bool EnterFullScreenMode ([NullAllowed] NSScreen screen, [NullAllowed] NSDictionary options);

		// @required - (void)exitFullScreenModeWithOptions:(NSDictionary *)options;
		[Export ("exitFullScreenModeWithOptions:")]
		void ExitFullScreenModeWithOptions ([NullAllowed] NSDictionary options);
	}

	[BaseType (typeof (NSView))] // Mac 10.6
	interface QLPreviewView {

		[Export ("initWithFrame:style:")]
		NativeHandle Constructor (CGRect frame, QLPreviewViewStyle style);

		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("previewItem", ArgumentSemantic.Retain)]
		IQLPreviewItem PreviewItem { get; set; }

		[Export ("refreshPreviewItem")]
		void RefreshPreviewItem ();

		[Export ("displayState", ArgumentSemantic.Retain)]
		NSObject DisplayState { get; set; }

		[Export ("close")]
		void Close ();

		[Export ("shouldCloseWithWindow")]
		bool ShouldCloseWithWindow { get; set; }

		[Export ("autostarts")]
		bool Autostarts { get; set; }
	}

	[Protocol]
	interface QLPreviewingController {
		/// <param name="identifier">To be added.</param>
		/// <param name="queryString">To be added.</param>
		/// <param name="ItemLoadingHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("preparePreviewOfSearchableItemWithIdentifier:queryString:completionHandler:")]
#if XAMCORE_5_0
		void PreparePreviewOfSearchableItem (string identifier, string queryString, Action<NSError> itemLoadingHandler);
#else
		void PreparePreviewOfSearchableItem (string identifier, string queryString, Action<NSError> ItemLoadingHandler);
#endif

		[Export ("preparePreviewOfFileAtURL:completionHandler:")]
		void PreparePreviewOfFile (NSUrl url, Action<NSError> completionHandler);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("providePreviewForFileRequest:completionHandler:")]
		void ProvidePreview (QLFilePreviewRequest request, Action<QLPreviewReply, NSError> handler);
	}

	[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface QLFilePreviewRequest {
		[Export ("fileURL")]
		NSUrl FileUrl { get; }
	}

	[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface QLPreviewProvider : NSExtensionRequestHandling {
	}

	[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface QLPreviewReplyAttachment {
		[Export ("data")]
		NSData Data { get; }

		[Export ("contentType")]
		UTType ContentType { get; }

		[Export ("initWithData:contentType:")]
		NativeHandle Constructor (NSData data, UTType contentType);
	}

	delegate bool QLPreviewReplyDrawingHandler (CGContext context, QLPreviewReply reply, out NSError error);
	delegate NSData QLPreviewReplyDataCreationHandler (QLPreviewReply reply, out NSError error);
	delegate CGPDFDocument QLPreviewReplyUIDocumentCreationHandler (QLPreviewReply reply, out NSError error);

	[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface QLPreviewReply {
		[Export ("stringEncoding")]
		NSStringEncoding StringEncoding { get; set; }

		[Export ("attachments", ArgumentSemantic.Copy)]
		NSDictionary<NSString, QLPreviewReplyAttachment> Attachments { get; set; }

		[Export ("title")]
		string Title { get; set; }

		[Export ("initWithContextSize:isBitmap:drawingBlock:")]
		NativeHandle Constructor (CGSize contextSize, bool isBitmap, QLPreviewReplyDrawingHandler drawingHandler);

		[Export ("initWithFileURL:")]
		NativeHandle Constructor (NSUrl fileUrl);

		[Export ("initWithDataOfContentType:contentSize:dataCreationBlock:")]
		NativeHandle Constructor (UTType contentType, CGSize contentSize, QLPreviewReplyDataCreationHandler dataCreationHandler);

		// QLPreviewReply_UI
		[Export ("initForPDFWithPageSize:documentCreationBlock:")]
		NativeHandle Constructor (CGSize defaultPageSize, QLPreviewReplyUIDocumentCreationHandler documentCreationHandler);
	}
}
