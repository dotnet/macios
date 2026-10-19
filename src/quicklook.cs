//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2012 Xamarin Inc
// Copyright 2019 Microsoft Corporation
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
using CoreGraphics;
#if MONOMAC
using AppKit;
using UIWindowSceneActivationConfiguration = Foundation.NSObject;
#else
using UIKit;
#endif
using System.ComponentModel;
using UniformTypeIdentifiers;

namespace QuickLook {
#if !MONOMAC
	/// <summary>A <see cref="UIKit.UIViewController" /> that manages the user experience of previewing an item.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/QLPreviewController_Class/index.html">Apple documentation for <c>QLPreviewController</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (QLPreviewControllerDelegate) })]
	interface QLPreviewController {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new Quick Look preview controller from the specified NIB name in the specified <paramref name="bundle" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("dataSource", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDataSource { get; set; }

		/// <summary>Gets or sets the data source that contains the items to preview.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Wrap ("WeakDataSource")]
		IQLPreviewControllerDataSource DataSource { get; set; }

		[Export ("delegate", ArgumentSemantic.Weak), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the QuickLook.IQLPreviewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the QuickLook.IQLPreviewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IQLPreviewControllerDelegate Delegate { get; set; }

		[Export ("currentPreviewItemIndex")]
		nint CurrentPreviewItemIndex { get; set; }

		[Export ("currentPreviewItem")]
		[NullAllowed]
		IQLPreviewItem CurrentPreviewItem { get; }

		[Static]
		[Export ("canPreviewItem:")]
		bool CanPreviewItem (IQLPreviewItem item);

		[Export ("reloadData")]
		void ReloadData ();

		[Export ("refreshCurrentPreviewItem")]
		void RefreshCurrentPreviewItem ();
	}

	interface IQLPreviewControllerDataSource { }

	/// <summary>A class that allows a <see cref="QuickLook.QLPreviewController" /> to preview multiple items.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/QLPreviewControllerDataSource_Protocol/index.html">Apple documentation for <c>QLPreviewControllerDataSource</c></related>
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	[NoMac]
	[MacCatalyst (13, 1)]
	interface QLPreviewControllerDataSource {
		/// <param name="controller">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("numberOfPreviewItemsInPreviewController:")]
		nint PreviewItemCount (QLPreviewController controller);

		/// <param name="controller">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("previewController:previewItemAtIndex:")]
		IQLPreviewItem GetPreviewItem (QLPreviewController controller, nint index);
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum QLPreviewItemEditingMode : long {
		Disabled = 0,
		UpdateContents,
		CreateCopy,
	}

	interface IQLPreviewControllerDelegate { }

	/// <summary>A delegate object that gives the application developer fine-grained control over events in the life-cycle of a <see cref="QuickLook.QLPreviewController" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/NetworkingInternet/Reference/QLPreviewControllerDelegate_Protocol/index.html">Apple documentation for <c>QLPreviewControllerDelegate</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface QLPreviewControllerDelegate {
		/// <param name="controller">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("previewControllerWillDismiss:")]
		void WillDismiss (QLPreviewController controller);

		/// <param name="controller">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("previewControllerDidDismiss:")]
		void DidDismiss (QLPreviewController controller);

		/// <param name="controller">To be added.</param>
		/// <param name="url">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("previewController:shouldOpenURL:forPreviewItem:"), DelegateName ("QLOpenUrl"), DefaultValue (false)]
		bool ShouldOpenUrl (QLPreviewController controller, NSUrl url, IQLPreviewItem item);

#if !MONOMAC
		// UIView and UIImage do not exists in MonoMac

		/// <param name="controller">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <param name="view">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("previewController:frameForPreviewItem:inSourceView:"), DelegateName ("QLFrame"), DefaultValue (typeof (CGRect))]
		CGRect FrameForPreviewItem (QLPreviewController controller, IQLPreviewItem item, ref UIView view);

		/// <param name="controller">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <param name="contentRect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("previewController:transitionImageForPreviewItem:contentRect:"), DelegateName ("QLTransition"), DefaultValue (null)]
		[return: NullAllowed]
		UIImage TransitionImageForPreviewItem (QLPreviewController controller, IQLPreviewItem item, CGRect contentRect);

		/// <param name="controller">To be added.</param>
		/// <param name="item">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Delegate invoked by the object to get a value.</summary>
			<value>To be added.</value>
			<remarks>Developers assign a function, delegate or anonymous method to this property to return a value to the object.   If developers assign a value to this property, it this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[MacCatalyst (13, 1)]
		[Export ("previewController:transitionViewForPreviewItem:"), DelegateName ("QLTransitionView"), DefaultValue (null)]
		[return: NullAllowed]
		UIView TransitionViewForPreviewItem (QLPreviewController controller, IQLPreviewItem item);

		[MacCatalyst (13, 1)]
		[Export ("previewController:editingModeForPreviewItem:"), DelegateName ("QLEditingMode"), DefaultValue ("QLPreviewItemEditingMode.Disabled")]
		QLPreviewItemEditingMode GetEditingMode (QLPreviewController controller, IQLPreviewItem previewItem);

		[MacCatalyst (13, 1)]
		[Export ("previewController:didUpdateContentsOfPreviewItem:"), EventArgs ("QLPreviewControllerDelegateDidUpdate")]
		void DidUpdateContents (QLPreviewController controller, IQLPreviewItem previewItem);

		[MacCatalyst (13, 1)]
		[Export ("previewController:didSaveEditedCopyOfPreviewItem:atURL:"), EventArgs ("QLPreviewControllerDelegateDidSave")]
		void DidSaveEditedCopy (QLPreviewController controller, IQLPreviewItem previewItem, NSUrl modifiedContentsUrl);

#endif
	}
#else
	[Static]
	[NoiOS]
	[NoMacCatalyst]
	[NoTV]
	interface QLThumbnailImage {
		[Internal, Field ("kQLThumbnailOptionScaleFactorKey")]
		NSString OptionScaleFactorKey { get; }

		[Internal, Field ("kQLThumbnailOptionIconModeKey")]
		NSString OptionIconModeKey { get; }
	}
#endif
}

// QuickLookUI's managed namespace is preserved on macOS for compatibility.
#if MONOMAC
namespace QuickLookUI {
#else
namespace QuickLook {
#endif

	interface IQLPreviewItem { }

#if !MONOMAC
	/// <summary>An item that can be previewed with a <see cref="QuickLook.QLPreviewController" />.</summary>
	///     
	/// <related type="externalDocumentation" href="https://developer.apple.com/documentation/quicklook/qlpreviewitem">Apple documentation for <c>QLPreviewItem</c></related>
	[NoMac]
	[MacCatalyst (13, 1)]
#endif
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface QLPreviewItem {
#if !MONOMAC
		/// <summary>Gets the URL for the preview item.</summary>
		/// <value>The URL for the preview item.</value>
#endif
		[Abstract]
#if !MONOMAC
		[NullAllowed]
#endif
		[Export ("previewItemURL")]
		NSUrl PreviewItemUrl { get; }

#if !MONOMAC
		/// <summary>Gets the title for the preview item.</summary>
		/// <value>The title for the preview item.</value>
#endif
		[Export ("previewItemTitle")]
#if !MONOMAC
		[NullAllowed]
#endif
		string PreviewItemTitle { get; }

#if MONOMAC
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Export ("previewItemDisplayState")]
		NSObject PreviewItemDisplayState { get; }
#endif
	}

	delegate bool QLPreviewReplyDrawingHandler (CGContext context, QLPreviewReply reply, out NSError error);
	delegate NSData QLPreviewReplyDataCreationHandler (QLPreviewReply reply, out NSError error);
	delegate CGPDFDocument QLPreviewReplyUIDocumentCreationHandler (QLPreviewReply reply, out NSError error);

#if !MONOMAC
	[NoMac]
#endif
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

#if !MONOMAC
	[NoMac]
#endif
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

#if !MONOMAC
	[NoMac]
#endif
	[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface QLFilePreviewRequest {
		[Export ("fileURL")]
		NSUrl FileUrl { get; }
	}

#if !MONOMAC
	[NoMac]
#endif
	[NoTV, iOS (15, 0), MacCatalyst (15, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface QLPreviewProvider : NSExtensionRequestHandling {
	}

#if !MONOMAC
	[NoTV]
	[NoMac] // availability not mentioned in the header files
	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface QLPreviewSceneOptions {
		[Export ("initialPreviewIndex")]
		nint InitialPreviewIndex { get; set; }
	}

	[NoMac]
	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (UIWindowSceneActivationConfiguration))]
	interface QLPreviewSceneActivationConfiguration {
		[Export ("initWithItemsAtURLs:options:")]

		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl [] urls, [NullAllowed] QLPreviewSceneOptions options);

		[Export ("initWithUserActivity:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUserActivity userActivity);
	}
#endif

#if !MONOMAC
	[NoMac]
	[MacCatalyst (13, 1)]
#endif
	[Protocol]
	interface QLPreviewingController {
#if MONOMAC
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
#else
		/// <param name="identifier">To be added.</param>
		/// <param name="queryString">To be added.</param>
		/// <param name="handler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("preparePreviewOfSearchableItemWithIdentifier:queryString:completionHandler:")]
		void PreparePreviewOfSearchableItem (string identifier, [NullAllowed] string queryString, Action<NSError> handler);

		/// <param name="url">To be added.</param>
		/// <param name="handler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("preparePreviewOfFileAtURL:completionHandler:")]
		void PreparePreviewOfFile (NSUrl url, Action<NSError> handler);
#endif

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("providePreviewForFileRequest:completionHandler:")]
		void ProvidePreview (QLFilePreviewRequest request, Action<QLPreviewReply, NSError> handler);
	}

#if MONOMAC
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
#endif

}

#if !MONOMAC
// Apple moved ARQuickLookPreviewItem from the ARKit framework to the QuickLook framework in Xcode 26,
// and also added it to Mac Catalyst.
#if XAMCORE_5_0 || __MACCATALYST__
namespace QuickLook {
#else
namespace ARKit {
	using QuickLook;
#endif
	[MacCatalyst (26, 0), NoTV, NoMac]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface ARQuickLookPreviewItem : QLPreviewItem {

		[Export ("initWithFileAtURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl url);

		[NullAllowed, Export ("canonicalWebPageURL", ArgumentSemantic.Strong)]
		NSUrl CanonicalWebPageUrl { get; set; }

		[Export ("allowsContentScaling")]
		bool AllowsContentScaling { get; set; }
	}
}
#endif // !MONOMAC
