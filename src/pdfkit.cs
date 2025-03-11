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
// PdfKit.cs: Bindings for the PdfKit API
//

using System;
#if MONOMAC
using AppKit;
using UIViewController = Foundation.NSObject;
using UIFindInteraction = Foundation.NSObject;
using UIFindInteractionDelegate = Foundation.NSObject;
using UIView = AppKit.NSView;
#else
using UIKit;
using NSColor = UIKit.UIColor;
using NSFont = UIKit.UIFont;
using NSImage = UIKit.UIImage;
using NSView = UIKit.UIView;
using NSEdgeInsets = UIKit.UIEdgeInsets;
using NSEvent = UIKit.UIEvent;
using NSBezierPath = UIKit.UIBezierPath;
using NSTextAlignment = UIKit.UITextAlignment;
// HACK: to make intermediate dll build, since we use these 
// types in a few [NoiOS] members (this way we avoid numerous #ifdefs later)
using NSPrintInfo = Foundation.NSObject;
using NSPrintOperation = Foundation.NSObject;
#endif
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using System.ComponentModel;

#if !NET
using NativeHandle = System.IntPtr;
#endif

// Verify/Test Delegate Models
// Check for missing NullAllowed on all object properties
// Test methods returning typed arrays in lieu of NSArray
// Check classes with no public inits - Should I make the constructors private?
// Check the few abnormal properties

namespace PdfKit {

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	enum PdfAnnotationKey {

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyAppearanceDictionary", "+PDFKit")]
		AppearanceDictionary,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyAppearanceState", "+PDFKit")]
		AppearanceState,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyBorder", "+PDFKit")]
		Border,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyColor", "+PDFKit")]
		Color,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyContents", "+PDFKit")]
		Contents,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyFlags", "+PDFKit")]
		Flags,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyDate", "+PDFKit")]
		Date,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyName", "+PDFKit")]
		Name,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyPage", "+PDFKit")]
		Page,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyRect", "+PDFKit")]
		Rect,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeySubtype", "+PDFKit")]
		Subtype,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyAction", "+PDFKit")]
		Action,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyAdditionalActions", "+PDFKit")]
		AdditionalActions,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyBorderStyle", "+PDFKit")]
		BorderStyle,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyDefaultAppearance", "+PDFKit")]
		DefaultAppearance,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyDestination", "+PDFKit")]
		Destination,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyHighlightingMode", "+PDFKit")]
		HighlightingMode,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyInklist", "+PDFKit")]
		Inklist,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyInteriorColor", "+PDFKit")]
		InteriorColor,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyLinePoints", "+PDFKit")]
		LinePoints,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyLineEndingStyles", "+PDFKit")]
		LineEndingStyles,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyIconName", "+PDFKit")]
		IconName,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyOpen", "+PDFKit")]
		Open,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyParent", "+PDFKit")]
		Parent,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyPopup")]
		Popup,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyQuadding", "+PDFKit")]
		Quadding,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyQuadPoints", "+PDFKit")]
		QuadPoints,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyTextLabel", "+PDFKit")]
		TextLabel,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetDownCaption", "+PDFKit")]
		WidgetDownCaption,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetBorderColor", "+PDFKit")]
		WidgetBorderColor,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetBackgroundColor", "+PDFKit")]
		WidgetBackgroundColor,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetCaption", "+PDFKit")]
		WidgetCaption,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetDefaultValue", "+PDFKit")]
		WidgetDefaultValue,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetFieldFlags", "+PDFKit")]
		WidgetFieldFlags,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetFieldType", "+PDFKit")]
		WidgetFieldType,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetAppearanceDictionary", "+PDFKit")]
		WidgetAppearanceDictionary,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetMaxLen", "+PDFKit")]
		WidgetMaxLen,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetOptions", "+PDFKit")]
		WidgetOptions,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetRotation", "+PDFKit")]
		WidgetRotation,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetRolloverCaption", "+PDFKit")]
		WidgetRolloverCaption,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetTextLabelUI", "+PDFKit")]
		WidgetTextLabelUI,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationKeyWidgetValue", "+PDFKit")]
		WidgetValue,
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	enum PdfAnnotationSubtype {

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeText", "+PDFKit")]
		Text,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeLink", "+PDFKit")]
		Link,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeFreeText", "+PDFKit")]
		FreeText,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeLine", "+PDFKit")]
		Line,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeSquare", "+PDFKit")]
		Square,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeCircle", "+PDFKit")]
		Circle,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeHighlight", "+PDFKit")]
		Highlight,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeUnderline", "+PDFKit")]
		Underline,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeStrikeOut", "+PDFKit")]
		StrikeOut,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeInk", "+PDFKit")]
		Ink,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeStamp", "+PDFKit")]
		Stamp,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypePopup", "+PDFKit")]
		Popup,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationSubtypeWidget", "+PDFKit")]
		Widget,
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	enum PdfAnnotationWidgetSubtype {

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationWidgetSubtypeButton", "+PDFKit")]
		Button,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationWidgetSubtypeChoice", "+PDFKit")]
		Choice,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationWidgetSubtypeSignature", "+PDFKit")]
		Signature,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationWidgetSubtypeText", "+PDFKit")]
		Text,
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	enum PdfAnnotationLineEndingStyle {

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationLineEndingStyleNone", "+PDFKit")]
		None,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationLineEndingStyleSquare", "+PDFKit")]
		Square,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationLineEndingStyleCircle", "+PDFKit")]
		Circle,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationLineEndingStyleDiamond", "+PDFKit")]
		Diamond,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationLineEndingStyleOpenArrow", "+PDFKit")]
		OpenArrow,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationLineEndingStyleClosedArrow", "+PDFKit")]
		ClosedArrow,
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	enum PdfAnnotationTextIconType {

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationTextIconTypeComment", "+PDFKit")]
		Comment,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationTextIconTypeKey", "+PDFKit")]
		Key,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationTextIconTypeNote", "+PDFKit")]
		Note,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationTextIconTypeHelp", "+PDFKit")]
		Help,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationTextIconTypeNewParagraph", "+PDFKit")]
		NewParagraph,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationTextIconTypeParagraph", "+PDFKit")]
		Paragraph,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationTextIconTypeInsert", "+PDFKit")]
		Insert,
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	enum PdfAnnotationHighlightingMode {

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationHighlightingModeNone", "+PDFKit")]
		None,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationHighlightingModeInvert", "+PDFKit")]
		Invert,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationHighlightingModeOutline", "+PDFKit")]
		Outline,

		/// <summary>To be added.</summary>
		[Field ("PDFAnnotationHighlightingModePush", "+PDFKit")]
		Push,
	}

	[Native]
	[iOS (15, 0), MacCatalyst (15, 0)]
	[TV (18, 2)]
	public enum PdfAccessPermissions : ulong {
		LowQualityPrinting = (1uL << 0),
		HighQualityPrinting = (1uL << 1),
		DocumentChanges = (1uL << 2),
		DocumentAssembly = (1uL << 3),
		ContentCopying = (1uL << 4),
		ContentAccessibility = (1uL << 5),
		Commenting = (1uL << 6),
		FormFieldEntry = (1uL << 7),
	}

	[Native]
	[iOS (18, 0), Mac (15, 0), MacCatalyst (18, 0)]
	[TV (18, 2)]
	enum PdfSelectionGranularity : ulong {
		Character,
		Word,
		Line,
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[Static]
	interface PdfAppearanceCharacteristicsKeys {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFAppearanceCharacteristicsKeyBackgroundColor", "+PDFKit")]
		NSString BackgroundColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFAppearanceCharacteristicsKeyBorderColor", "+PDFKit")]
		NSString BorderColorKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFAppearanceCharacteristicsKeyRotation", "+PDFKit")]
		NSString RotationKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFAppearanceCharacteristicsKeyCaption", "+PDFKit")]
		NSString CaptionKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFAppearanceCharacteristicsKeyRolloverCaption", "+PDFKit")]
		NSString RolloverCaptionKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFAppearanceCharacteristicsKeyDownCaption", "+PDFKit")]
		NSString DownCaptionKey { get; }
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[Static]
	interface PdfBorderKeys {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFBorderKeyLineWidth", "+PDFKit")]
		NSString LineWidthKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFBorderKeyStyle", "+PDFKit")]
		NSString StyleKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PDFBorderKeyDashPattern", "+PDFKit")]
		NSString DashPatternKey { get; }
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[Internal]
	[Static]
	interface PdfDocumentAttributeKeys {

		[Field ("PDFDocumentTitleAttribute", "+PDFKit")]
		NSString TitleKey { get; }

		[Field ("PDFDocumentAuthorAttribute", "+PDFKit")]
		NSString AuthorKey { get; }

		[Field ("PDFDocumentSubjectAttribute", "+PDFKit")]
		NSString SubjectKey { get; }

		[Field ("PDFDocumentCreatorAttribute", "+PDFKit")]
		NSString CreatorKey { get; }

		[Field ("PDFDocumentProducerAttribute", "+PDFKit")]
		NSString ProducerKey { get; }

		[Field ("PDFDocumentCreationDateAttribute", "+PDFKit")]
		NSString CreationDateKey { get; }

		[Field ("PDFDocumentModificationDateAttribute", "+PDFKit")]
		NSString ModificationDateKey { get; }

		[Field ("PDFDocumentKeywordsAttribute", "+PDFKit")]
		NSString KeywordsKey { get; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 2)]
	[StrongDictionary ("PdfPageImageInitializationOptionKeys")]
	interface PdfPageImageInitializationOption {
		CGRect MediaBox { get; set; }
		int Rotation { get; set; }
		bool UpscaleIfSmaller { get; set; }
		double CompressionQuality { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 2)]
	[Static]
	interface PdfPageImageInitializationOptionKeys {
		[Field ("PDFPageImageInitializationOptionMediaBox")]
		NSString MediaBoxKey { get; }

		[Field ("PDFPageImageInitializationOptionRotation")]
		NSString RotationKey { get; }

		[Field ("PDFPageImageInitializationOptionUpscaleIfSmaller")]
		NSString UpscaleIfSmallerKey { get; }

		[Field ("PDFPageImageInitializationOptionCompressionQuality")]
		NSString CompressionQualityKey { get; }
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[StrongDictionary ("PdfDocumentAttributeKeys")]
	interface PdfDocumentAttributes {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string Title { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string Author { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string Subject { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string Creator { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string Producer { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDate CreationDate { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		NSDate ModificationDate { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string [] Keywords { get; set; }
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[Internal]
	[Static]
	interface PdfDocumentWriteOptionKeys {

		[Field ("PDFDocumentOwnerPasswordOption", "+PDFKit")]
		NSString OwnerPasswordKey { get; }

		[Field ("PDFDocumentUserPasswordOption", "+PDFKit")]
		NSString UserPasswordKey { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("PDFDocumentAccessPermissionsOption", "+PDFKit")]
		NSString AccessPermissionsKey { get; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Field ("PDFDocumentBurnInAnnotationsOption", "+PDFKit")]
		NSString BurnInAnnotationsKey { get; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Field ("PDFDocumentSaveTextFromOCROption", "+PDFKit")]
		NSString SaveTextFromOcrKey { get; }

		[iOS (16, 4), Mac (13, 3), MacCatalyst (16, 4)]
		[Field ("PDFDocumentSaveImagesAsJPEGOption", "+PDFKit")]
		NSString SaveImagesAsJpegKey { get; }

		[iOS (16, 4), Mac (13, 3), MacCatalyst (16, 4)]
		[Field ("PDFDocumentOptimizeImagesForScreenOption", "+PDFKit")]
		NSString OptimizeImagesForScreenKey { get; }
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[StrongDictionary ("PdfDocumentWriteOptionKeys")]
	interface PdfDocumentWriteOptions {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string OwnerPassword { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		string UserPassword { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		string AccessPermissions { get; set; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		bool BurnInAnnotations { get; set; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		bool SaveTextFromOcr { get; set; }

		[iOS (16, 4), Mac (13, 3), MacCatalyst (16, 4)]
		bool SaveImagesAsJpeg { get; set; }

		[iOS (16, 4), Mac (13, 3), MacCatalyst (16, 4)]
		bool OptimizeImagesForScreen { get; set; }
	}

	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFAppearanceCharacteristics")]
	interface PdfAppearanceCharacteristics : NSCopying {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("controlType", ArgumentSemantic.Assign)]
		PdfWidgetControlType ControlType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("backgroundColor", ArgumentSemantic.Copy)]
		NSColor BackgroundColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("borderColor", ArgumentSemantic.Copy)]
		NSColor BorderColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rotation")]
		nint Rotation { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("caption")]
		string Caption { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("rolloverCaption")]
		string RolloverCaption { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("downCaption")]
		string DownCaption { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("appearanceCharacteristicsKeyValues", ArgumentSemantic.Copy)]
		NSDictionary WeakAppearanceCharacteristicsKeyValues { get; }
	}

	/// <summary>Base class for actions that can be performed on a PDF.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFAction")]
	[Abstract]
	interface PdfAction : NSCopying {
		//This is an abstract superclass with no public init - should it have a private constructor??
		//As it is, I can create instances, that segfault when you access the type method.
		//marking the method as [Abstract] doesn't work because the subclasses do not explictly
		//define this method (although they implement it)
		/// <summary>Gets the action type.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type")]
		string Type { get; }
	}

	/// <summary>An action that contains data for going to a location within a PDF.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (PdfAction), Name = "PDFActionGoTo")]
	interface PdfActionGoTo {

		[DesignatedInitializer]
		[Export ("initWithDestination:")]
		NativeHandle Constructor (PdfDestination destination);

		/// <summary>Gets or sets the destination of the go-to PDF action.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destination")]
		PdfDestination Destination { get; set; }
	}

	/// <summary>An named PDF action.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (PdfAction), Name = "PDFActionNamed")]
	interface PdfActionNamed {

		[DesignatedInitializer]
		[Export ("initWithName:")]
		NativeHandle Constructor (PdfActionNamedName name);

		/// <summary>Gets or sets the name of the named PDF action.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		PdfActionNamedName Name { get; set; }
	}

	/// <summary>An action that contains data for going to a location in another document.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (PdfAction), Name = "PDFActionRemoteGoTo")]
	interface PdfActionRemoteGoTo {

		[DesignatedInitializer]
		[Export ("initWithPageIndex:atPoint:fileURL:")]
		NativeHandle Constructor (nint pageIndex, CGPoint point, NSUrl fileUrl);

		/// <summary>Gets or sets the zero-based index of the page in the destination document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pageIndex")]
		nint PageIndex { get; set; }

		/// <summary>Gets or sets the page-space point on the page in the destination document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("point")]
		CGPoint Point { get; set; }

		/// <summary>Gets or sets the URL for the remote document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URL")]
		NSUrl Url { get; set; }
	}

	/// <summary>An action that resets a form in a PDF.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[DisableDefaultCtor]
	[BaseType (typeof (PdfAction), Name = "PDFActionResetForm")]
	interface PdfActionResetForm {
		// - (instancetype)init NS_DESIGNATED_INITIALIZER;
		[Export ("init")]
		[DesignatedInitializer]
		NativeHandle Constructor ();

		//NSArray of NSString
		/// <summary>Gets or sets the fields that will be affected by the action.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("fields"), NullAllowed]
		string [] Fields { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the fields that are in <see cref="P:PdfKit.PdfActionResetForm.Fields" /> will be cleared.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fieldsIncludedAreCleared")]
		bool FieldsIncludedAreCleared { get; set; }
	}

	/// <summary>A PDF action for visiting a URL.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (PdfAction), Name = "PDFActionURL")]
	interface PdfActionUrl {

		[DesignatedInitializer]
		[Export ("initWithURL:")]
		NativeHandle Constructor (NSUrl url);

		/// <summary>Gets or sets the URL for the PDF action.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URL"), NullAllowed]
		NSUrl Url { get; set; }
	}

	/// <summary>Notes, highlights, or other additions to a PDF file.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFAnnotation")]
	interface PdfAnnotation : NSCoding, NSCopying {

		[MacCatalyst (13, 1)]
		[Export ("initWithBounds:forType:withProperties:")]
		[DesignatedInitializer]
		NativeHandle Constructor (CGRect bounds, NSString annotationType, [NullAllowed] NSDictionary properties);

		[MacCatalyst (13, 1)]
		[Wrap ("this (bounds, annotationType.GetConstant ()!, properties)")]
		NativeHandle Constructor (CGRect bounds, PdfAnnotationKey annotationType, [NullAllowed] NSDictionary properties);

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use '.ctor (CGRect, PDFAnnotationKey, NSDictionary)' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use '.ctor (CGRect, PDFAnnotationKey, NSDictionary)' instead.")]
		[NoMacCatalyst]
		[NoTV]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use '.ctor (CGRect, PDFAnnotationKey, NSDictionary)' instead.")]
		[Export ("initWithBounds:")]
		NativeHandle Constructor (CGRect bounds);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("page")]
		[NullAllowed]
		PdfPage Page { get; set; }

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Protected]
		[Export ("type")]
		[NullAllowed]
		NSString Type { get; set; }
#else
		[Export ("type")]
		[NullAllowed]
		string Type { get; set; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("bounds")]
		CGRect Bounds { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("modificationDate")]
		[NullAllowed]
		NSDate ModificationDate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("userName")]
		[NullAllowed]
		string UserName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("popup")]
		[NullAllowed]
#if MONOMAC
		PdfAnnotationPopup Popup { get; set; }
#else
		PdfAnnotation Popup { get; set; }
#endif

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldDisplay")]
		bool ShouldDisplay { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shouldPrint")]
		bool ShouldPrint { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("border")]
		[NullAllowed]
		PdfBorder Border { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("color")]
		NSColor Color { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[NullAllowed]
		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("mouseUpAction")]
		PdfAction MouseUpAction { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("contents")]
		[NullAllowed]
		string Contents { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("toolTip")]
		[NullAllowed]
		string ToolTip { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("hasAppearanceStream")]
		bool HasAppearanceStream { get; }

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("removeAllAppearanceStreams")]
		void RemoveAllAppearanceStreams ();

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("drawWithBox:")]
		void Draw (PdfDisplayBox box);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("action", ArgumentSemantic.Strong), NullAllowed]
		PdfAction Action { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("highlighted")]
		bool Highlighted { [Bind ("isHighlighted")] get; set; }

		[MacCatalyst (13, 1)]
		[Export ("drawWithBox:inContext:")]
		void Draw (PdfDisplayBox box, CGContext context);

		[Internal]
		[MacCatalyst (13, 1)]
		[Export ("setValue:forAnnotationKey:")]
		bool _SetValue (IntPtr value, NSString key);

		[Internal]
		[MacCatalyst (13, 1)]
		[Export ("valueForAnnotationKey:")]
		[return: NullAllowed]
		IntPtr _GetValue (NSString key);

		[Protected]
		[MacCatalyst (13, 1)]
		[Export ("setBoolean:forAnnotationKey:")]
		bool SetValue (bool boolean, NSString key);

		[MacCatalyst (13, 1)]
		[Wrap ("SetValue (boolean, key.GetConstant ()!)")]
		bool SetValue (bool boolean, PdfAnnotationKey key);

		[Protected]
		[MacCatalyst (13, 1)]
		[Export ("setRect:forAnnotationKey:")]
		bool SetValue (CGRect rect, NSString key);

		[MacCatalyst (13, 1)]
		[Wrap ("SetValue (rect, key.GetConstant ()!)")]
		bool SetValue (CGRect rect, PdfAnnotationKey key);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("annotationKeyValues", ArgumentSemantic.Copy)]
		NSDictionary AnnotationKeyValues { get; }

		[Protected]
		[MacCatalyst (13, 1)]
		[Export ("removeValueForAnnotationKey:")]
		void RemoveValue (NSString key);

		[MacCatalyst (13, 1)]
		[Wrap ("RemoveValue (key.GetConstant ()!)")]
		void RemoveValue (PdfAnnotationKey key);

		// PDFAnnotation (PDFAnnotationUtilities) Category

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("font", ArgumentSemantic.Copy)]
		NSFont Font { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("fontColor", ArgumentSemantic.Copy)]
		NSColor FontColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("interiorColor", ArgumentSemantic.Copy)]
		NSColor InteriorColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("alignment", ArgumentSemantic.Assign)]
		NSTextAlignment Alignment { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("startPoint", ArgumentSemantic.Assign)]
		CGPoint StartPoint { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("endPoint", ArgumentSemantic.Assign)]
		CGPoint EndPoint { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("startLineStyle", ArgumentSemantic.Assign)]
		PdfLineStyle StartLineStyle { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("endLineStyle", ArgumentSemantic.Assign)]
		PdfLineStyle EndLineStyle { get; set; }

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("lineStyleFromName:")]
		PdfLineStyle GetLineStyle (string fromName);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("nameForLineStyle:")]
		string GetName (PdfLineStyle style);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("iconType", ArgumentSemantic.Assign)]
		PdfTextAnnotationIconType IconType { get; set; }

		[Internal]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("quadrilateralPoints", ArgumentSemantic.Copy)]
		IntPtr _QuadrilateralPoints { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("markupType", ArgumentSemantic.Assign)]
		PdfMarkupType MarkupType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("widgetFieldType")]
		string WidgetFieldType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("widgetControlType", ArgumentSemantic.Assign)]
		PdfWidgetControlType WidgetControlType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("multiline")]
		bool Multiline { [Bind ("isMultiline")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("isPasswordField")]
		bool IsPasswordField { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("comb")]
		bool Comb { [Bind ("hasComb")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("maximumLength")]
		nint MaximumLength { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("widgetStringValue")]
		string WidgetStringValue { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("widgetDefaultStringValue")]
		string WidgetDefaultStringValue { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsToggleToOff")]
		bool AllowsToggleToOff { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("radiosInUnison")]
		bool RadiosInUnison { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("readOnly")]
		bool ReadOnly { [Bind ("isReadOnly")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("listChoice")]
		bool ListChoice { [Bind ("isListChoice")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("choices", ArgumentSemantic.Copy)]
		string [] Choices { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("values", ArgumentSemantic.Copy)]
		string [] Values { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("buttonWidgetState", ArgumentSemantic.Assign)]
		PdfWidgetCellState ButtonWidgetState { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("buttonWidgetStateString")]
		string ButtonWidgetStateString { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("open")]
		bool Open { [Bind ("isOpen")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("paths")]
		NSBezierPath [] Paths { get; }

		[MacCatalyst (13, 1)]
		[Export ("addBezierPath:")]
		void AddBezierPath (NSBezierPath path);

		[MacCatalyst (13, 1)]
		[Export ("removeBezierPath:")]
		void RemoveBezierPath (NSBezierPath path);

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("destination", ArgumentSemantic.Strong)]
		PdfDestination Destination { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("URL", ArgumentSemantic.Copy)]
		NSUrl Url { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("fieldName")]
		string FieldName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("caption")]
		string Caption { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("backgroundColor", ArgumentSemantic.Copy)]
		NSColor BackgroundColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("stampName")]
		string StampName { get; set; }

		[iOS (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		[Export ("activatableTextField")]
		bool ActivatableTextField { [Bind ("isActivatableTextField")] get; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationButtonWidget")]
	interface PdfAnnotationButtonWidget {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("controlType")]
		PdfWidgetControlType ControlType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("state")]
		nint State { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("highlighted")]
		bool Highlighted { [Bind ("isHighlighted")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsToggleToOff")]
		bool AllowsToggleToOff { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("font")]
		NSFont Font { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("caption")]
		string Caption { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fieldName")]
		string FieldName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("onStateValue")]
		string OnStateValue { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationChoiceWidget")]
	interface PdfAnnotationChoiceWidget {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("stringValue")]
		string Text { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("font")]
		NSFont Font { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fieldName")]
		string FieldName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isListChoice")]
		bool IsListChoice { get; set; }

		// NSArray of NSString
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("choices")]
		string [] Choices { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationCircle")]
	interface PdfAnnotationCircle {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interiorColor")]
		NSColor InteriorColor { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationFreeText")]
	interface PdfAnnotationFreeText {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("font")]
		NSFont Font { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationInk")]
	interface PdfAnnotationInk {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paths")]
		NSBezierPath [] Paths { get; }

#if !NET
		[Export ("addBezierPath:")]
		void AddBezierPathpath (NSBezierPath path);

		[Export ("removeBezierPath:")]
		void RemoveBezierPathpath (NSBezierPath path);
#else
		[Export ("addBezierPath:")]
		void AddBezierPath (NSBezierPath path);

		[Export ("removeBezierPath:")]
		void RemoveBezierPath (NSBezierPath path);
#endif
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationLine")]
	interface PdfAnnotationLine {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("startPoint")]
		CGPoint StartPoint { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("endPoint")]
		CGPoint EndPoint { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("startLineStyle")]
		PdfLineStyle StartLineStyle { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("endLineStyle")]
		PdfLineStyle EndLineStyle { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interiorColor")]
		NSColor InteriorColor { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationLink")]
	interface PdfAnnotationLink {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("destination")]
		PdfDestination Destination { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("URL")]
		NSUrl Url { get; set; }

		[Export ("setHighlighted:")]
		void SetHighlighted (bool highlighted);
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationMarkup")]
	interface PdfAnnotationMarkup {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("quadrilateralPoints", ArgumentSemantic.Assign), NullAllowed]
		NSArray WeakQuadrilateralPoints { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("markupType")]
		PdfMarkupType MarkupType { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationPopup")]
	interface PdfAnnotationPopup {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isOpen")]
		bool IsOpen { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationSquare")]
	interface PdfAnnotationSquare {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("interiorColor")]
		NSColor InteriorColor { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationStamp")]
	interface PdfAnnotationStamp {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("name")]
		string Name { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationText")]
	interface PdfAnnotationText {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("iconType")]
		PdfTextAnnotationIconType IconType { get; set; }
	}

	[NoiOS]
	[NoTV]
	[Deprecated (PlatformName.MacOSX, 10, 12)]
	[NoMacCatalyst]
	[BaseType (typeof (PdfAnnotation), Name = "PDFAnnotationTextWidget")]
	interface PdfAnnotationTextWidget {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("stringValue")]
		string StringValue { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rotation")]
		int Rotation { get; set; } // (int) rotation;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("font")]
		NSFont Font { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fontColor")]
		NSColor FontColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("alignment")]
		NSTextAlignment Alignment { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("maximumLength")]
		nint MaximumLength { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fieldName")]
		string FieldName { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributedStringValue")]
		NSAttributedString AttributedStringValue { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isMultiline")]
		bool IsMultiline { get; set; }
	}

	/// <summary>A border that may be drawn within the rectangle of an annotation.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFBorder")]
	interface PdfBorder : NSCoding, NSCopying {
		/// <summary>Gets or sets the border style.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("style")]
		PdfBorderStyle Style { get; set; }

		/// <summary>Gets or sets the line width, in points.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lineWidth")]
		nfloat LineWidth { get; set; }

		/// <summary>Gets or sets a weak reference to the dash pattern for the border.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("dashPattern", ArgumentSemantic.Assign), NullAllowed]
		NSArray WeakDashPattern { get; set; }

		/// <summary>Gets or sets a weak reference to the border properties dictionary.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("borderKeyValues", ArgumentSemantic.Copy)]
		NSDictionary WeakBorderKeyValues { get; }

		[Export ("drawInRect:")]
		void Draw (CGRect rect);
	}

	/// <summary>A destination for a go-to PDF action.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFDestination")]
	interface PdfDestination : NSCopying {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("kPDFDestinationUnspecifiedValue")]
		nfloat UnspecifiedValue { get; }

		[DesignatedInitializer]
		[Export ("initWithPage:atPoint:")]
		NativeHandle Constructor (PdfPage page, CGPoint point);

		/// <summary>Gets the page of the destination.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("page")]
		[NullAllowed]
		PdfPage Page { get; }

		/// <summary>Gets the point on the destination page.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("point")]
		CGPoint Point { get; }

		/// <summary>Gets or sets the zoom level of at the destination.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("zoom")]
		nfloat Zoom { get; set; }

		//Should Compare be more more .Net ified ?
		[Export ("compare:")]
		NSComparisonResult Compare (PdfDestination destination);
	}

	//Add attributes for delegates/events
	/// <summary>Class for working with PDF documents.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject), Name = "PDFDocument", Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (PdfDocumentDelegate) })]
	interface PdfDocument : NSCopying {

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidUnlockNotification']/*" />
		[Field ("PDFDocumentDidUnlockNotification", "+PDFKit")]
		[Notification]
		NSString DidUnlockNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidBeginFindNotification']/*" />
		[Field ("PDFDocumentDidBeginFindNotification", "+PDFKit")]
		[Notification]
		NSString DidBeginFindNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidEndFindNotification']/*" />
		[Field ("PDFDocumentDidEndFindNotification", "+PDFKit")]
		[Notification]
		NSString DidEndFindNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidBeginPageFindNotification']/*" />
		[Field ("PDFDocumentDidBeginPageFindNotification", "+PDFKit")]
		[Notification]
		NSString DidBeginPageFindNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidEndPageFindNotification']/*" />
		[Field ("PDFDocumentDidEndPageFindNotification", "+PDFKit")]
		[Notification]
		NSString DidEndPageFindNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidFindMatchNotification']/*" />
		[Field ("PDFDocumentDidFindMatchNotification", "+PDFKit")]
		[Notification]
		NSString DidFindMatchNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidBeginWriteNotification']/*" />
		[Field ("PDFDocumentDidBeginWriteNotification", "+PDFKit")]
		[Notification]
		NSString DidBeginWriteNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidEndWriteNotification']/*" />
		[Field ("PDFDocumentDidEndWriteNotification", "+PDFKit")]
		[Notification]
		NSString DidEndWriteNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidBeginPageWriteNotification']/*" />
		[Field ("PDFDocumentDidBeginPageWriteNotification", "+PDFKit")]
		[Notification]
		NSString DidBeginPageWriteNotification { get; }

		/// <include file="../docs/api/PdfKit/PdfDocument.xml" path="/Documentation/Docs[@DocId='P:PdfKit.PdfDocument.DidEndPageWriteNotification']/*" />
		[Field ("PDFDocumentDidEndPageWriteNotification", "+PDFKit")]
		[Notification]
		NSString DidEndPageWriteNotification { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("PDFDocumentFoundSelectionKey")]
		NSString FoundSelectionKey { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("PDFDocumentPageIndexKey")]
		NSString PageIndexKey { get; }

		// - (instancetype)init NS_DESIGNATED_INITIALIZER;
		[Export ("init")]
		[DesignatedInitializer]
		NativeHandle Constructor ();

		[Export ("initWithURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSUrl url);

		[Export ("initWithData:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSData data);

		/// <summary>Gets the URL of the PDF document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("documentURL")]
		[NullAllowed]
		NSUrl DocumentUrl { get; }

		/// <summary>Gets a Core Graphics PDF object for this PdfKit PDF object.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("documentRef")]
		[NullAllowed]
		CGPDFDocument Document { get; }

		/// <summary>Gets or sets the attribute dictionary.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Advice ("Use the strongly typed '[Get|Set]DocumentAttributes' instead.")]
		[Export ("documentAttributes", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSDictionary DocumentAttributes { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("accessPermissions")]
		PdfAccessPermissions AccessPermissions { get; }

		[Wrap ("new PdfDocumentAttributes (DocumentAttributes)")]
		PdfDocumentAttributes GetDocumentAttributes ();

		[Wrap ("DocumentAttributes = attributes?.GetDictionary ()")]
		void SetDocumentAttributes ([NullAllowed] PdfDocumentAttributes attributes);

#if NET || IOS
		/// <summary>Gets the major version number of the document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("majorVersion")]
		nint MajorVersion { get; }

		/// <summary>Gets the minor version of the document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("minorVersion")]
		nint MinorVersion { get; }
#else
		[NoiOS]
		[Export ("majorVersion")]
		int MajorVersion { get; } /* int, not NSInteger */

		[NoiOS]
		[Export ("minorVersion")]
		int MinorVersion { get; } /* int, not NSInteger */
#endif

		/// <summary>Gets a Boolean value that tells whether the document is encrypted.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isEncrypted")]
		bool IsEncrypted { get; }

		/// <summary>Gets a Boolean value that tells whether the document is locked.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isLocked")]
		bool IsLocked { get; }

		[Export ("unlockWithPassword:")]
		bool Unlock (string password);

		/// <summary>Gets a Boolean value that tells whether the document allows printing.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsPrinting")]
		bool AllowsPrinting { get; }

		/// <summary>Gets a Boolean value that tells whether the document allows copying.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("allowsCopying")]
		bool AllowsCopying { get; }

		/// <summary>Gets a Boolean value that tells whether the document allows changes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsDocumentChanges")]
		bool AllowsDocumentChanges { get; }

		/// <summary>Gets a Boolean value that tells whether the document allows document assembly.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsDocumentAssembly")]
		bool AllowsDocumentAssembly { get; }

		/// <summary>Gets a Boolean value that tells whether the document allows content accessibility.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsContentAccessibility")]
		bool AllowsContentAccessibility { get; }

		/// <summary>Gets a Boolean value that tells whether the document allows comments.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsCommenting")]
		bool AllowsCommenting { get; }

		/// <summary>Gets a Boolean value that tells whether the document allows form field entry.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("allowsFormFieldEntry")]
		bool AllowsFormFieldEntry { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("permissionsStatus")]
		PdfDocumentPermissions PermissionsStatus { get; }

		/// <summary>Gets a linefeed-separated, page-oriented string that contains the entire textual content of the PDF document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("string")]
		[NullAllowed]
		string Text { get; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the PdfKit.IPdfDocumentDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the PdfKit.IPdfDocumentDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IPdfDocumentDelegate Delegate { get; set; }

		[Export ("dataRepresentation")]
		[return: NullAllowed]
		NSData GetDataRepresentation ();

		[Export ("dataRepresentationWithOptions:")]
		[return: NullAllowed]
		NSData GetDataRepresentation (NSDictionary options);

		[Export ("writeToFile:")]
		bool Write (string path);

		[Export ("writeToFile:withOptions:")]
		bool Write (string path, [NullAllowed] NSDictionary options);

		[MacCatalyst (13, 1)]
		[Wrap ("Write (path, options.GetDictionary ()!)")]
		bool Write (string path, PdfDocumentWriteOptions options);

		[Export ("writeToURL:")]
		bool Write (NSUrl url);

		[Export ("writeToURL:withOptions:")]
		bool Write (NSUrl url, [NullAllowed] NSDictionary options);

		[MacCatalyst (13, 1)]
		[Wrap ("Write (url, options.GetDictionary ()!)")]
		bool Write (NSUrl url, PdfDocumentWriteOptions options);

		/// <summary>Gets or sets the root outline object.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("outlineRoot")]
		PdfOutline OutlineRoot { get; set; }

		[Export ("outlineItemForSelection:")]
		[return: NullAllowed]
		PdfOutline OutlineItem (PdfSelection selection);

		/// <summary>Gets the number of pages in the document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pageCount")]
		nint PageCount { get; }

		[Export ("pageAtIndex:")]
		[return: NullAllowed]
		PdfPage GetPage (nint index);

		[Export ("indexForPage:")]
		nint GetPageIndex (PdfPage page);

		[Export ("insertPage:atIndex:")]
		void InsertPage (PdfPage page, nint index);

		[Export ("removePageAtIndex:")]
		void RemovePage (nint index);

		[Export ("exchangePageAtIndex:withPageAtIndex:")]
		void ExchangePages (nint indexA, nint indexB);

		/// <summary>Gets the class (Objective-C type) of pages in the document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pageClass")]
		Class PageClass { get; }

		/// <summary>Gets the .NET type of page objects in this document.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Wrap ("Class.Lookup (PageClass)")]
		Type PageType { get; }

		[Export ("findString:withOptions:")]
#if MONOMAC && !NET
		[Obsolete ("Use 'Find (string, NSStringCompareOptions)' instead.")]
		PdfSelection [] Find (string text, nint options);

		[Wrap ("Find (text: text, options: (nint) (int) compareOptions)", IsVirtual = true)]
#endif
		PdfSelection [] Find (string text, NSStringCompareOptions compareOptions);

		[Export ("beginFindString:withOptions:")]
#if MONOMAC && !NET
		[Obsolete ("Use 'FindAsync (string, NSStringCompareOptions)' instead.")]
		[return: NullAllowed]
		void FindAsync (string text, nint options);

		[Wrap ("FindAsync (text: text, options: (nint) (int) compareOptions)", IsVirtual = true)]
#endif
		[return: NullAllowed]
		void FindAsync (string text, NSStringCompareOptions compareOptions);

		[Export ("beginFindStrings:withOptions:")]
#if MONOMAC && !NET
		[Obsolete ("Use 'FindAsync (string [], NSStringCompareOptions)' instead.")]
		[return: NullAllowed]
		void FindAsync (string [] text, nint options);

		[Wrap ("FindAsync (text: text, options: (nint) (int) compareOptions)", IsVirtual = true)]
#endif
		[return: NullAllowed]
		void FindAsync (string [] text, NSStringCompareOptions compareOptions);

		[Export ("findString:fromSelection:withOptions:")]
#if MONOMAC && !NET
		[Obsolete ("Use 'Find (string, PdfSelection, NSStringCompareOptions)' instead.")]
		[return: NullAllowed]
		PdfSelection Find (string text, [NullAllowed] PdfSelection selection, nint options);

		[Wrap ("Find (text: text, selection: selection, options: (nint) (int) compareOptions)", IsVirtual = true)]
#endif
		[return: NullAllowed]
		PdfSelection Find (string text, [NullAllowed] PdfSelection selection, NSStringCompareOptions compareOptions);

		/// <summary>Gets a Boolean value that tells whether the document is currently searching for text.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isFinding")]
		bool IsFinding { get; }

		[Export ("cancelFindString")]
		void CancelFind ();

		[Export ("selectionForEntireDocument")]
		[return: NullAllowed]
		PdfSelection SelectEntireDocument ();

		[Export ("selectionFromPage:atPoint:toPage:atPoint:")]
		[return: NullAllowed]
		PdfSelection GetSelection (PdfPage startPage, CGPoint startPoint, PdfPage endPage, CGPoint endPoint);

		[Export ("selectionFromPage:atCharacterIndex:toPage:atCharacterIndex:")]
		[return: NullAllowed]
		PdfSelection GetSelection (PdfPage startPage, nint startCharIndex, PdfPage endPage, nint endCharIndex);

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("printOperationForPrintInfo:scalingMode:autoRotate:")]
		[return: NullAllowed]
#pragma warning disable 0618 // 'PdfPrintScalingMode' is obsolete: 'This type is not available on iOS.'
		NSPrintOperation GetPrintOperation ([NullAllowed] NSPrintInfo printInfo, PdfPrintScalingMode scaleMode, bool doRotate);
#pragma warning restore

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("selectionFromPage:atPoint:toPage:atPoint:withGranularity:")]
		[return: NullAllowed]
		PdfSelection GetSelection (PdfPage startPage, CGPoint startPoint, PdfPage endPage, CGPoint endPoint, PdfSelectionGranularity granularity);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:PdfKit.PdfDocumentDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:PdfKit.PdfDocumentDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:PdfKit.PdfDocumentDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:PdfKit.PdfDocumentDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IPdfDocumentDelegate { }

	/// <summary>Delegate object for the <see cref="T:PdfKit.PdfDocument" /> class, presenting methods relating to search process and results.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFDocumentDelegate")]
#if IOS
	[Protocol]
#else
	[Protocol (FormalSince = "10.13", Name = "PDFDocumentDelegate")]
#endif
	[Model]
	interface PdfDocumentDelegate {

		[Export ("documentDidUnlock:"), EventArgs ("NSNotification")]
		void DidUnlock (NSNotification notification);

		[Export ("documentDidBeginDocumentFind:"), EventArgs ("NSNotification")]
		void DidBeginDocumentFind (NSNotification notification);

		[Export ("didMatchString:"), EventArgs ("PdfSelection")]
		void DidMatchString (PdfSelection sender);

		[Export ("classForPage"), IgnoredInDelegate]
		Class GetClassForPage ();

		[MacCatalyst (13, 1)]
		[DelegateName ("ClassForAnnotationTypeDelegate"), DefaultValue (null)]
		[Export ("classForAnnotationType:")]
		Class GetClassForAnnotationType (string annotationType);

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "Use 'GetClassForAnnotationType' instead.")]
		[NoTV]
		[NoMacCatalyst]
		[Export ("classForAnnotationClass:"), DelegateName ("ClassForAnnotationClassDelegate"), DefaultValue (null)]
#if NET
		Class GetClassForAnnotationClass (Class sender);
#else
		Class ClassForAnnotationClass (Class sender);
#endif

		[Export ("documentDidEndDocumentFind:"), EventArgs ("NSNotification")]
		void FindFinished (NSNotification notification);

		[Export ("documentDidBeginPageFind:"), EventArgs ("NSNotification")]
		void PageFindStarted (NSNotification notification);

		[Export ("documentDidEndPageFind:"), EventArgs ("NSNotification")]
		void PageFindFinished (NSNotification notification);

		[Export ("documentDidFindMatch:"), EventArgs ("NSNotification")]
		void MatchFound (NSNotification notification);
	}

	/// <summary>A node in a logical outline of a PDF document.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject), Name = "PDFOutline")]
	interface PdfOutline {

		// - (instancetype)init NS_DESIGNATED_INITIALIZER;
		[Export ("init")]
		[DesignatedInitializer]
		NativeHandle Constructor ();

		/// <summary>Gets the document for which this outline node participates in an outline.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("document")]
		[NullAllowed]
		PdfDocument Document { get; }

		/// <summary>Gets the parent outline node.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("parent"), NullAllowed]
		PdfOutline Parent { get; }

		/// <summary>Gets the number of children that this outline node has.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfChildren")]
		nint ChildrenCount { get; }

		/// <summary>Gets the index of this outline node within the parent node's list of childrent.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("index")]
		nint Index { get; }

		[return: NullAllowed]
		[Export ("childAtIndex:")]
		PdfOutline Child (nint index);

		[Export ("insertChild:atIndex:")]
		void InsertChild (PdfOutline child, nint index);

		[Export ("removeFromParent")]
		void RemoveFromParent ();

		/// <summary>Gets or sets the label for the node.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("label")]
		[NullAllowed]
		string Label { get; set; }

		/// <summary>Gets or sets a Boolean value that tells whether the outline is displayed when the document opens.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("isOpen")]
		bool IsOpen { get; set; }

		/// <summary>Gets or sets the destination in the PDF that this outline node represents.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("destination"), NullAllowed]
		PdfDestination Destination { get; set; }

		/// <summary>Gets or sets the action to perform when responding to user taps on the outline.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("action"), NullAllowed]
		PdfAction Action { get; set; }
	}

	/// <summary>Class for working with and rendering PDF pages.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject), Name = "PDFPage")]
	interface PdfPage : NSCopying {

		// - (instancetype)init NS_DESIGNATED_INITIALIZER;
		[Export ("init")]
		[DesignatedInitializer]
		NativeHandle Constructor ();

		[Export ("initWithImage:")]
		NativeHandle Constructor (NSImage image);

		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[DesignatedInitializer]
		[Export ("initWithImage:options:")]
		NativeHandle Constructor (NSImage image, NSDictionary options);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Wrap ("this (image, options.GetDictionary ()!)")]
		NativeHandle Constructor (NSImage image, PdfPageImageInitializationOption options);

		/// <summary>Gets the PDF document object that contains the PDF page.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("document"), NullAllowed]
		PdfDocument Document { get; }

		/// <summary>Gets the Core Graphics PDFPage object for this PDF page.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("pageRef"), NullAllowed]
		CGPDFPage Page { get; }

		/// <summary>Gets the page label.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("label"), NullAllowed]
		string Label { get; }

		[Export ("boundsForBox:")]
		CGRect GetBoundsForBox (PdfDisplayBox box);

		[Export ("setBounds:forBox:")]
		void SetBoundsForBox (CGRect bounds, PdfDisplayBox box);

		/// <summary>Gets or sets the rotation, in degrees, for displaying the page.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rotation")]
		nint Rotation { get; set; } // - (NSInteger) rotation; - rotation is not consistently declared

		//Check  Docs say: "array will _most likely_ be typed to subclasses of the PdfAnnotation class"
		//do they mean that if it isn't a subclass it is the base class ??
		//Maybe we should be safe and return NSArray ??
		/// <summary>Gets an array that contains all the annotations on the PDF page.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("annotations")]
		PdfAnnotation [] Annotations { get; }

		/// <summary>Gets or sets a Boolean value that controls whether annotations are displayed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displaysAnnotations")]
		bool DisplaysAnnotations { get; set; }

		[Export ("addAnnotation:")]
		void AddAnnotation (PdfAnnotation annotation);

		[Export ("removeAnnotation:")]
		void RemoveAnnotation (PdfAnnotation annotation);

		[Export ("annotationAtPoint:")]
		[return: NullAllowed]
		PdfAnnotation GetAnnotation (CGPoint point);

		[MacCatalyst (13, 1)]
		[Export ("transformForBox:")]
		CGAffineTransform GetTransform (PdfDisplayBox box);

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("drawWithBox:")]
		void Draw (PdfDisplayBox box);

		[MacCatalyst (13, 1)]
		[Export ("drawWithBox:toContext:")]
		void Draw (PdfDisplayBox box, CGContext context);

		[MacCatalyst (13, 1)]
		[Export ("transformContext:forBox:")]
		void TransformContext (CGContext context, PdfDisplayBox box);

		[MacCatalyst (13, 1)]
		[Export ("thumbnailOfSize:forBox:")]
		NSImage GetThumbnail (CGSize size, PdfDisplayBox box);

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("transformContextForBox:")]
		void TransformContext (PdfDisplayBox box);

		/// <summary>Gets the number of characters in the text content of the PDF page.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("numberOfCharacters")]
		nint CharacterCount { get; }

		/// <summary>Gets the text content of the page.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("string")]
		[NullAllowed]
		string Text { get; }

		/// <summary>Gets the text contents of the PDF page as an annotate string.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("attributedString")]
		[NullAllowed]
		NSAttributedString AttributedString { get; }

		[Export ("characterBoundsAtIndex:")]
		CGRect GetCharacterBounds (nint index);

		[Export ("characterIndexAtPoint:")]
		nint GetCharacterIndex (CGPoint point);

		[Export ("selectionForRect:")]
		[return: NullAllowed]
		PdfSelection GetSelection (CGRect rect);

		[Export ("selectionForWordAtPoint:")]
		[return: NullAllowed]
		PdfSelection SelectWord (CGPoint point);

		[Export ("selectionForLineAtPoint:")]
		[return: NullAllowed]
		PdfSelection SelectLine (CGPoint point);

		[Export ("selectionFromPoint:toPoint:")]
		[return: NullAllowed]
		PdfSelection GetSelection (CGPoint startPoint, CGPoint endPoint);

		[Export ("selectionForRange:")]
		[return: NullAllowed]
		PdfSelection GetSelection (NSRange range);

		/// <summary>Gets the PDF page data as an NSData object.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("dataRepresentation"), NullAllowed]
		NSData DataRepresentation { get; }
	}

	/// <summary>A text selection in a PDF document.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFSelection")]
	[DisableDefaultCtor] // An uncaught exception was raised: init: not a valid initializer for PDFSelection
	interface PdfSelection : NSCopying {

		[DesignatedInitializer]
		[Export ("initWithDocument:")]
		NativeHandle Constructor (PdfDocument document);

		/// <summary>Returns the pages that are in the selection.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pages")]
		PdfPage [] Pages { get; }

		/// <summary>Gets or sets the color with which to draw the selection.</summary>
		///         <value>
		///           <para>This overrides both the active and inactive selection drawing colors.</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("color", ArgumentSemantic.Copy), NullAllowed]
		NSColor Color { get; set; }

		/// <summary>Gets the text of the selection.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("string"), NullAllowed]
		string Text { get; }

		/// <summary>Returns an attributed string for this selection.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("attributedString"), NullAllowed]
		NSAttributedString AttributedString { get; }

		[Export ("boundsForPage:")]
		CGRect GetBoundsForPage (PdfPage page);

		[Export ("numberOfTextRangesOnPage:")]
		nuint GetNumberOfTextRanges (PdfPage page);

		[Export ("rangeAtIndex:onPage:")]
		NSRange GetRange (nuint index, PdfPage page);

		[Export ("selectionsByLine")]
		PdfSelection [] SelectionsByLine ();

		[Export ("addSelection:")]
		void AddSelection (PdfSelection selection);

		[Export ("addSelections:")]
		void AddSelections (PdfSelection [] selections);

		[Export ("extendSelectionAtEnd:")]
		void ExtendSelectionAtEnd (nint succeed);

		[Export ("extendSelectionAtStart:")]
		void ExtendSelectionAtStart (nint precede);

		[MacCatalyst (13, 1)]
		[Export ("extendSelectionForLineBoundaries")]
		void ExtendSelectionForLineBoundaries ();

		[Export ("drawForPage:active:")]
		void Draw (PdfPage page, bool active);

		[Export ("drawForPage:withBox:active:")]
		void Draw (PdfPage page, PdfDisplayBox box, bool active);
	}

	/// <summary>A list of thumbnail images for each page of a document.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSView), Name = "PDFThumbnailView")]
	interface PdfThumbnailView : NSCoding {

		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Field ("PDFThumbnailViewDocumentEditedNotification", "+PDFKit")]
		[Notification]
		NSString DocumentEditedNotification { get; }

		[Export ("PDFView", ArgumentSemantic.Weak)]
		[NullAllowed]
		PdfView PdfView { get; set; }

		[Export ("thumbnailSize")]
		CGSize ThumbnailSize { get; set; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("layoutMode")]
		PdfThumbnailLayoutMode LayoutMode { get; set; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("contentInset")]
		NSEdgeInsets ContentInset { get; set; }

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("maximumNumberOfColumns")]
		nint MaximumNumberOfColumns { get; set; }

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("labelFont")]
		[NullAllowed]
		NSFont LabelFont { get; set; }

		[Export ("backgroundColor", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSColor BackgroundColor { get; set; }

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("allowsDragging")]
		bool AllowsDragging { get; set; }

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("allowsMultipleSelection")]
		bool AllowsMultipleSelection { get; set; }

		[Export ("selectedPages", ArgumentSemantic.Strong), NullAllowed]
		PdfPage [] SelectedPages { get; }
	}

	/// <summary>A viewer for PDF files.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSView), Name = "PDFView", Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (PdfViewDelegate) })]
#if TVOS
	[DisableDefaultCtor]
#endif
	interface PdfView :
#if IOS
	UIGestureRecognizerDelegate, UIFindInteractionDelegate
#elif TVOS
	UIFindInteractionDelegate
#else
	NSMenuDelegate, NSAnimationDelegate
#endif
	{
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		[Export ("document"), NullAllowed]
		PdfDocument Document { get; set; }

		[Export ("canGoToFirstPage")]
		bool CanGoToFirstPage { get; }

		//Verify
		[Export ("goToFirstPage:")]
		void GoToFirstPage ([NullAllowed] NSObject sender);

		[Export ("canGoToLastPage")]
		bool CanGoToLastPage { get; }

		[Export ("goToLastPage:")]
		void GoToLastPage ([NullAllowed] NSObject sender);

		[Export ("canGoToNextPage")]
		bool CanGoToNextPage { get; }

		[Export ("goToNextPage:")]
		void GoToNextPage ([NullAllowed] NSObject sender);

		[Export ("canGoToPreviousPage")]
		bool CanGoToPreviousPage { get; }

		[Export ("goToPreviousPage:")]
		void GoToPreviousPage ([NullAllowed] NSObject sender);

		[Export ("canGoBack")]
		bool CanGoBack { get; }

		[Export ("goBack:")]
		void GoBack ([NullAllowed] NSObject sender);

		[Export ("canGoForward")]
		bool CanGoForward { get; }

		[Export ("goForward:")]
		void GoForward ([NullAllowed] NSObject sender);

		[Export ("currentPage")]
		[NullAllowed]
		PdfPage CurrentPage { get; }

		[Export ("goToPage:")]
		void GoToPage (PdfPage page);

		[Export ("currentDestination")]
		[NullAllowed]
		PdfDestination CurrentDestination { get; }

		[Export ("goToDestination:")]
		void GoToDestination (PdfDestination destination);

		[Export ("goToSelection:")]
		void GoToSelection (PdfSelection selection);

		[Export ("goToRect:onPage:")]
		void GoToRectangle (CGRect rect, PdfPage page);

		[Export ("displayMode")]
		PdfDisplayMode DisplayMode { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("displayDirection")]
		PdfDisplayDirection DisplayDirection { get; set; }

		[Export ("displaysPageBreaks")]
		bool DisplaysPageBreaks { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("pageBreakMargins")]
		NSEdgeInsets PageBreakMargins { get; set; }

		[Export ("displayBox")]
		PdfDisplayBox DisplayBox { get; set; }

		[Export ("displaysAsBook")]
		bool DisplaysAsBook { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("displaysRTL")]
		bool DisplaysRtl { get; set; }

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("shouldAntiAlias")]
		bool ShouldAntiAlias { get; set; }

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("greekingThreshold")]
		nfloat GreekingThreshold { get; set; }

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("takeBackgroundColorFrom:")]
		void TakeBackgroundColor (NSObject sender);

		[Export ("backgroundColor")]
		NSColor BackgroundColor { get; set; }

		[Export ("interpolationQuality", ArgumentSemantic.Assign)]
		PdfInterpolationQuality InterpolationQuality { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("pageShadowsEnabled")]
		bool PageShadowsEnabled { get; [Bind ("enablePageShadows:")] set; }

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("usePageViewController:withViewOptions:")]
		void UsePageViewController (bool enable, [NullAllowed] NSDictionary viewOptions);

		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("isUsingPageViewController")]
		bool IsUsingPageViewController { get; }

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		IPdfViewDelegate Delegate { get; set; }

		[Export ("scaleFactor")]
		nfloat ScaleFactor { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("minScaleFactor")]
		nfloat MinScaleFactor { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("maxScaleFactor")]
		nfloat MaxScaleFactor { get; set; }

		[Export ("zoomIn:")]
		void ZoomIn ([NullAllowed] NSObject sender);

		[Export ("canZoomIn")]
		bool CanZoomIn { get; }

		[Export ("zoomOut:")]
		void ZoomOut ([NullAllowed] NSObject sender);

		[Export ("canZoomOut")]
		bool CanZoomOut { get; }

		[Export ("autoScales")]
		bool AutoScales { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("scaleFactorForSizeToFit")]
		nfloat ScaleFactorForSizeToFit { get; }

		[Export ("areaOfInterestForMouse:")]
		PdfAreaOfInterest GetAreaOfInterest (NSEvent mouseEvent);

		[MacCatalyst (13, 1)]
		[Export ("areaOfInterestForPoint:")]
		PdfAreaOfInterest GetAreaOfInterest (CGPoint point);

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("setCursorForAreaOfInterest:")]
		void SetCursor (PdfAreaOfInterest area);

		[Export ("performAction:")]
		void PerformAction (PdfAction action);

		[Export ("currentSelection")]
		[NullAllowed]
		PdfSelection CurrentSelection { get; set; }

		[Export ("setCurrentSelection:animate:")]
		void SetCurrentSelection ([NullAllowed] PdfSelection selection, bool animate);

		[Export ("clearSelection")]
		void ClearSelection ();

		[Export ("selectAll:")]
		void SelectAll ([NullAllowed] NSObject sender);

		[Export ("scrollSelectionToVisible:")]
		void ScrollSelectionToVisible ([NullAllowed] NSObject sender);

		[Export ("highlightedSelections")]
		[NullAllowed]
		PdfSelection [] HighlightedSelections { get; set; }

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("takePasswordFrom:")]
		void TakePasswordFrom (NSObject sender);

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("drawPage:")]
		void DrawPage (PdfPage page);

		[MacCatalyst (13, 1)]
		[Export ("drawPage:toContext:")]
		void DrawPage (PdfPage page, CGContext context);

		[MacCatalyst (13, 1)]
		[Export ("drawPagePost:toContext:")]
		void DrawPagePost (PdfPage page, CGContext context);

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("drawPagePost:")]
		void DrawPagePost (PdfPage page);

		[Export ("copy:")]
		void Copy ([NullAllowed] NSObject sender);

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("printWithInfo:autoRotate:")]
		void Print (NSPrintInfo printInfo, bool doRotate);

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("printWithInfo:autoRotate:pageScaling:")]
#pragma warning disable // 0618: 'PdfPrintScalingMode' is obsolete: 'This type is not available on iOS.'
		void Print (NSPrintInfo printInfo, bool doRotate, PdfPrintScalingMode scaleMode);
#pragma warning restore

		[Export ("pageForPoint:nearest:")]
		[return: NullAllowed]
		PdfPage GetPage (CGPoint point, bool nearest);

		[Export ("convertPoint:toPage:")]
		CGPoint ConvertPointToPage (CGPoint point, PdfPage page);

		[Export ("convertRect:toPage:")]
		CGRect ConvertRectangleToPage (CGRect rect, PdfPage page);

		[Export ("convertPoint:fromPage:")]
		CGPoint ConvertPointFromPage (CGPoint point, PdfPage page);

		[Export ("convertRect:fromPage:")]
		CGRect ConvertRectangleFromPage (CGRect rect, PdfPage page);

		[Export ("documentView")]
		[NullAllowed]
		NSView DocumentView { get; }

		[Export ("layoutDocumentView")]
		void LayoutDocumentView ();

		[Export ("annotationsChangedOnPage:")]
		void AnnotationsChanged (PdfPage page);

		[Export ("rowSizeForPage:")]
		CGSize RowSize (PdfPage page);

		[NoiOS]
		[Deprecated (PlatformName.MacOSX, 10, 13)]
		[NoMacCatalyst]
		[NoTV]
		[Export ("allowsDragging")]
		bool AllowsDragging { get; set; }

		[Export ("visiblePages")]
		PdfPage [] VisiblePages { get; }

		[Deprecated (PlatformName.MacOSX, 15, 0)]
		[Deprecated (PlatformName.MacCatalyst, 18, 0)]
		[Deprecated (PlatformName.iOS, 18, 0)]
		[Deprecated (PlatformName.TvOS, 18, 2)]
		[Export ("enableDataDetectors")]
		bool EnableDataDetectors { get; set; }

		[Field ("PDFViewChangedHistoryNotification", "+PDFKit")]
		[Notification]
		NSString ChangedHistoryNotification { get; }

		[Field ("PDFViewDocumentChangedNotification", "+PDFKit")]
		[Notification]
		NSString DocumentChangedNotification { get; }

		[Field ("PDFViewPageChangedNotification", "+PDFKit")]
		[Notification]
		NSString PageChangedNotification { get; }

		[Field ("PDFViewScaleChangedNotification", "+PDFKit")]
		[Notification]
		NSString ScaleChangedNotification { get; }

		[Field ("PDFViewAnnotationHitNotification", "+PDFKit")]
		[Notification (typeof (PdfViewAnnotationHitEventArgs))]
		NSString AnnotationHitNotification { get; }

		[Field ("PDFViewCopyPermissionNotification", "+PDFKit")]
		[Notification]
		NSString CopyPermissionNotification { get; }

		[Field ("PDFViewPrintPermissionNotification", "+PDFKit")]
		[Notification]
		NSString PrintPermissionNotification { get; }

		[Field ("PDFViewAnnotationWillHitNotification", "+PDFKit")]
		[Notification]
		NSString AnnotationWillHitNotification { get; }

		[Field ("PDFViewSelectionChangedNotification", "+PDFKit")]
		[Notification]
		NSString SelectionChangedNotification { get; }

		[Field ("PDFViewDisplayModeChangedNotification", "+PDFKit")]
		[Notification]
		NSString DisplayModeChangedNotification { get; }

		[Field ("PDFViewDisplayBoxChangedNotification", "+PDFKit")]
		[Notification]
		NSString DisplayBoxChangedNotification { get; }

		[Field ("PDFViewVisiblePagesChangedNotification", "+PDFKit")]
		[Notification]
		NSString VisiblePagesChangedNotification { get; }

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("acceptsDraggedFiles")]
		bool AcceptsDraggedFiles { get; set; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[NullAllowed, Export ("pageOverlayViewProvider", ArgumentSemantic.Weak)]
		IPdfPageOverlayViewProvider PageOverlayViewProvider { get; set; }

		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Export ("inMarkupMode")]
		bool InMarkupMode { [Bind ("isInMarkupMode")] get; set; }

		[iOS (16, 0), NoMac, MacCatalyst (16, 0), NoTV]
		[Export ("findInteraction")]
		UIFindInteraction FindInteraction { get; }

		[iOS (16, 0), NoMac, MacCatalyst (16, 0)]
		[Export ("findInteractionEnabled")]
		bool FindInteractionEnabled { [Bind ("isFindInteractionEnabled")] get; set; }
	}

	/// <summary>Provides data for the  event.</summary>
	[NoiOS]
	[NoTV]
	[NoMacCatalyst]
	interface PdfViewAnnotationHitEventArgs {
		/// <summary>Gets the annotation that was hit.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("PDFAnnotationHit")]
		PdfAnnotation AnnotationHit { get; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="T:PdfKit.PdfViewDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="T:PdfKit.PdfViewDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="T:PdfKit.PdfViewDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="T:PdfKit.PdfViewDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IPdfViewDelegate { }

	//Verify delegate methods.  There are default actions (not just return null ) that should occur
	//if the delegate does not implement the method.
	/// <summary>Delegate object for <see cref="T:PdfKit.PdfView" /> objects, allowing the developer to respond to navigation, printing, and linking events.</summary>
	[MacCatalyst (13, 1)]
	[TV (18, 2)]
	[BaseType (typeof (NSObject), Name = "PDFViewDelegate")]
#if IOS || TV
	[Protocol]
#else
	[Protocol (FormalSince = "10.12", Name = "PDFViewDelegate")]
#endif
	[Model]
	interface PdfViewDelegate {
		//from docs: 'By default, the scale factor is restricted to a range between 0.1 and 10.0 inclusive.'
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("PDFViewWillChangeScaleFactor:toScale:"), DelegateName ("PdfViewScale"), DefaultValueFromArgument ("scale")]
		nfloat WillChangeScaleFactor (PdfView sender, nfloat scale);

		[Export ("PDFViewWillClickOnLink:withURL:"), EventArgs ("PdfViewUrl")]
		void WillClickOnLink (PdfView sender, NSUrl url);

		// from the docs: 'By default, this method uses the string, if any, associated with the
		// 'Title' key in the view's PDFDocument attribute dictionary. If there is no such string,
		// this method uses the last path component if the document is URL-based.
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("PDFViewPrintJobTitle:"), DelegateName ("PdfViewTitle"), DefaultValue ("String.Empty")]
		string TitleOfPrintJob (PdfView sender);

		[Export ("PDFViewPerformFind:"), EventArgs ("PdfView")]
		void PerformFind (PdfView sender);

		[Export ("PDFViewPerformGoToPage:"), EventArgs ("PdfView")]
		void PerformGoToPage (PdfView sender);

		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("PDFViewPerformPrint:"), EventArgs ("PdfView")]
		void PerformPrint (PdfView sender);

		[Export ("PDFViewOpenPDF:forRemoteGoToAction:"), EventArgs ("PdfViewAction")]
		void OpenPdf (PdfView sender, PdfActionRemoteGoTo action);

		[iOS (13, 0)]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Export ("PDFViewParentViewController")]
		UIViewController ParentViewController { get; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 2)]
	interface IPdfPageOverlayViewProvider { }

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
	[TV (18, 2)]
	[Protocol (Name = "PDFPageOverlayViewProvider")]
	interface PdfPageOverlayViewProvider {
		[Abstract]
		[Export ("pdfView:overlayViewForPage:")]
		[return: NullAllowed]
		UIView GetOverlayView (PdfView view, PdfPage page);

		[Export ("pdfView:willDisplayOverlayView:forPage:")]
		void WillDisplayOverlayView (PdfView pdfView, UIView overlayView, PdfPage page);

		[Export ("pdfView:willEndDisplayingOverlayView:forPage:")]
		void WillEndDisplayingOverlayView (PdfView pdfView, UIView overlayView, PdfPage page);
	}
}
