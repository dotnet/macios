//
// Copyright 2010, 2011 Novell, Inc.
// Copyright 2011, Xamarin, Inc.
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
using System;
using System.ComponentModel;
using ObjCRuntime;
using Foundation;

#nullable enable

namespace AppKit {

	[NoMacCatalyst]
	[Native]
	public enum NSRunResponse : long {
		/// <summary>To be added.</summary>
		Stopped = -1000,
		/// <summary>To be added.</summary>
		Aborted = -1001,
		/// <summary>To be added.</summary>
		Continues = -1002,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSApplicationActivationOptions : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		ActivateAllWindows = 1,
		/// <summary>To be added.</summary>
		ActivateIgnoringOtherWindows = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSApplicationActivationPolicy : long {
		/// <summary>To be added.</summary>
		Regular,
		/// <summary>To be added.</summary>
		Accessory,
		/// <summary>To be added.</summary>
		Prohibited,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSApplicationPresentationOptions : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		AutoHideDock = (1 << 0),
		/// <summary>To be added.</summary>
		HideDock = (1 << 1),

		/// <summary>To be added.</summary>
		AutoHideMenuBar = (1 << 2),
		/// <summary>To be added.</summary>
		HideMenuBar = (1 << 3),

		/// <summary>To be added.</summary>
		DisableAppleMenu = (1 << 4),
		/// <summary>To be added.</summary>
		DisableProcessSwitching = (1 << 5),
		/// <summary>To be added.</summary>
		DisableForceQuit = (1 << 6),
		/// <summary>To be added.</summary>
		DisableSessionTermination = (1 << 7),
		/// <summary>To be added.</summary>
		DisableHideApplication = (1 << 8),
		/// <summary>To be added.</summary>
		DisableMenuBarTransparency = (1 << 9),

		/// <summary>To be added.</summary>
		FullScreen = (1 << 10),
		/// <summary>To be added.</summary>
		AutoHideToolbar = (1 << 11),
		DisableCursorLocationAssistance = (1 << 12),
	}

	[NoMacCatalyst]
	[Native]
	public enum NSApplicationDelegateReply : ulong {
		/// <summary>To be added.</summary>
		Success,
		/// <summary>To be added.</summary>
		Cancel,
		/// <summary>To be added.</summary>
		Failure,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSRequestUserAttentionType : ulong {
		/// <summary>To be added.</summary>
		CriticalRequest = 0,
		/// <summary>To be added.</summary>
		InformationalRequest = 10,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSApplicationTerminateReply : ulong {
		/// <summary>To be added.</summary>
		Cancel,
		/// <summary>To be added.</summary>
		Now,
		/// <summary>To be added.</summary>
		Later,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSApplicationPrintReply : ulong {
		/// <summary>To be added.</summary>
		Cancelled,
		/// <summary>To be added.</summary>
		Success,
		/// <summary>To be added.</summary>
		Failure,
		/// <summary>To be added.</summary>
		ReplyLater,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageInterpolation : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Low,
		/// <summary>To be added.</summary>
		Medium,
		/// <summary>To be added.</summary>
		High,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSComposite : ulong {
		/// <summary>To be added.</summary>
		Clear,
		/// <summary>To be added.</summary>
		Copy,
		/// <summary>To be added.</summary>
		SourceOver,
		/// <summary>To be added.</summary>
		SourceIn,
		/// <summary>To be added.</summary>
		SourceOut,
		/// <summary>To be added.</summary>
		SourceAtop,
		/// <summary>To be added.</summary>
		DestinationOver,
		/// <summary>To be added.</summary>
		DestinationIn,
		/// <summary>To be added.</summary>
		DestinationOut,
		/// <summary>To be added.</summary>
		DestinationAtop,
		/// <summary>To be added.</summary>
		XOR,
		/// <summary>To be added.</summary>
		PlusDarker,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use NSCompositeSourceOver instead.")]
		Highlight,
		/// <summary>To be added.</summary>
		PlusLighter,
		/// <summary>To be added.</summary>
		Multiply,
		/// <summary>To be added.</summary>
		Screen,
		/// <summary>To be added.</summary>
		Overlay,
		/// <summary>To be added.</summary>
		Darken,
		/// <summary>To be added.</summary>
		Lighten,
		/// <summary>To be added.</summary>
		ColorDodge,
		/// <summary>To be added.</summary>
		ColorBurn,
		/// <summary>To be added.</summary>
		SoftLight,
		/// <summary>To be added.</summary>
		HardLight,
		/// <summary>To be added.</summary>
		Difference,
		/// <summary>To be added.</summary>
		Exclusion,
		/// <summary>To be added.</summary>
		Hue,
		/// <summary>To be added.</summary>
		Saturation,
		/// <summary>To be added.</summary>
		Color,
		/// <summary>To be added.</summary>
		Luminosity,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSBackingStore : ulong {
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'Buffered' instead.")]
		Retained,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'Buffered' instead.")]
		Nonretained,
		/// <summary>To be added.</summary>
		Buffered,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowOrderingMode : long {
		/// <summary>To be added.</summary>
		Below = -1,
		/// <summary>To be added.</summary>
		Out,
		/// <summary>To be added.</summary>
		Above,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSFocusRingPlacement : ulong {
		/// <summary>To be added.</summary>
		RingOnly,
		/// <summary>To be added.</summary>
		RingBelow,
		/// <summary>To be added.</summary>
		RingAbove,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSFocusRingType : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Exterior,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSColorRenderingIntent : long {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		AbsoluteColorimetric,
		/// <summary>To be added.</summary>
		RelativeColorimetric,
		/// <summary>To be added.</summary>
		Perceptual,
		/// <summary>To be added.</summary>
		Saturation,

	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSRectEdge : ulong {
		/// <summary>To be added.</summary>
		MinXEdge,
		/// <summary>To be added.</summary>
		MinYEdge,
		/// <summary>To be added.</summary>
		MaxXEdge,
		/// <summary>To be added.</summary>
		MaxYEdge,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSUserInterfaceLayoutDirection : long {
		/// <summary>To be added.</summary>
		LeftToRight,
		/// <summary>To be added.</summary>
		RightToLeft,
	}

	#region NSColorSpace
	[NoMacCatalyst]
	[Native]
	public enum NSColorSpaceModel : long {
		/// <summary>To be added.</summary>
		Unknown = -1,
		/// <summary>To be added.</summary>
		Gray,
		/// <summary>To be added.</summary>
		RGB,
		/// <summary>To be added.</summary>
		CMYK,
		/// <summary>To be added.</summary>
		LAB,
		/// <summary>To be added.</summary>
		DeviceN,
		/// <summary>To be added.</summary>
		Indexed,
		/// <summary>To be added.</summary>
		Pattern,
	}
	#endregion

	#region NSFileWrapper
	#endregion

	#region NSParagraphStyle
	[NoMacCatalyst]
	[Native]
	public enum NSTextTabType : ulong {
		/// <summary>To be added.</summary>
		Left,
		/// <summary>To be added.</summary>
		Right,
		/// <summary>To be added.</summary>
		Center,
		/// <summary>To be added.</summary>
		Decimal,
	}

	[Native]
	[NoMacCatalyst]
	public enum NSLineBreakMode : ulong {
		/// <summary>To be added.</summary>
		ByWordWrapping,
		/// <summary>To be added.</summary>
		CharWrapping,
		/// <summary>To be added.</summary>
		Clipping,
		/// <summary>To be added.</summary>
		TruncatingHead,
		/// <summary>To be added.</summary>
		TruncatingTail,
		/// <summary>To be added.</summary>
		TruncatingMiddle,
	}

	#endregion

	#region NSCell Defines 

	[NoMacCatalyst]
	[Native]
	public enum NSCellType : ulong {
		/// <summary>To be added.</summary>
		Null,
		/// <summary>To be added.</summary>
		Text,
		/// <summary>To be added.</summary>
		Image,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCellAttribute : ulong {
		/// <summary>To be added.</summary>
		CellDisabled,
		/// <summary>To be added.</summary>
		CellState,
		/// <summary>To be added.</summary>
		PushInCell,
		/// <summary>To be added.</summary>
		CellEditable,
		/// <summary>To be added.</summary>
		ChangeGrayCell,
		/// <summary>To be added.</summary>
		CellHighlighted,
		/// <summary>To be added.</summary>
		CellLightsByContents,
		/// <summary>To be added.</summary>
		CellLightsByGray,
		/// <summary>To be added.</summary>
		ChangeBackgroundCell,
		/// <summary>To be added.</summary>
		CellLightsByBackground,
		/// <summary>To be added.</summary>
		CellIsBordered,
		/// <summary>To be added.</summary>
		CellHasOverlappingImage,
		/// <summary>To be added.</summary>
		CellHasImageHorizontal,
		/// <summary>To be added.</summary>
		CellHasImageOnLeftOrBottom,
		/// <summary>To be added.</summary>
		CellChangesContents,
		/// <summary>To be added.</summary>
		CellIsInsetButton,
		/// <summary>To be added.</summary>
		CellAllowsMixedState,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCellImagePosition : ulong {
		/// <summary>To be added.</summary>
		NoImage,
		/// <summary>To be added.</summary>
		ImageOnly,
		/// <summary>To be added.</summary>
		ImageLeft,
		/// <summary>To be added.</summary>
		ImageRight,
		/// <summary>To be added.</summary>
		ImageBelow,
		/// <summary>To be added.</summary>
		ImageAbove,
		/// <summary>To be added.</summary>
		ImageOverlaps,
		/// <summary>To be added.</summary>
		ImageLeading,
		/// <summary>To be added.</summary>
		ImageTrailing,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageScale : ulong {
		/// <summary>To be added.</summary>
		ProportionallyDown = 0,
		/// <summary>To be added.</summary>
		AxesIndependently,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		ProportionallyUpOrDown,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCellStateValue : long {
		/// <summary>To be added.</summary>
		Mixed = -1,
		/// <summary>To be added.</summary>
		Off,
		/// <summary>To be added.</summary>
		On,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSCellStyleMask : ulong {
		/// <summary>To be added.</summary>
		NoCell = 0,
		/// <summary>To be added.</summary>
		ContentsCell = 1 << 0,
		/// <summary>To be added.</summary>
		PushInCell = 1 << 1,
		/// <summary>To be added.</summary>
		ChangeGrayCell = 1 << 2,
		/// <summary>To be added.</summary>
		ChangeBackgroundCell = 1 << 3,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSCellHit : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		ContentArea = 1,
		/// <summary>To be added.</summary>
		EditableTextArea = 2,
		/// <summary>To be added.</summary>
		TrackableArae = 4,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSControlTint : ulong {
		/// <summary>To be added.</summary>
		Default = 0,    // system 'default'
		/// <summary>To be added.</summary>
		Blue = 1,
		/// <summary>To be added.</summary>
		Graphite = 6,
		/// <summary>To be added.</summary>
		Clear = 7,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSControlSize : ulong {
		/// <summary>To be added.</summary>
		Regular = 0,
		/// <summary>To be added.</summary>
		Small = 1,
		/// <summary>To be added.</summary>
		Mini = 2,
		Large = 3,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSBackgroundStyle : long {
		/// <summary>To be added.</summary>
		Normal = 0,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Normal' instead.")]
		Light = Normal,
		/// <summary>To be added.</summary>
		Emphasized,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Emphasized' instead.")]
		Dark = Emphasized,
		/// <summary>To be added.</summary>
		Raised,
		/// <summary>To be added.</summary>
		Lowered,
	}
	#endregion

	#region NSImage

	[NoMacCatalyst]
	[Native]
	public enum NSImageLoadStatus : ulong {
		/// <summary>To be added.</summary>
		Completed,
		/// <summary>To be added.</summary>
		Cancelled,
		/// <summary>To be added.</summary>
		InvalidData,
		/// <summary>To be added.</summary>
		UnexpectedEOF,
		/// <summary>To be added.</summary>
		ReadError,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageCacheMode : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		Always,
		/// <summary>To be added.</summary>
		BySize,
		/// <summary>To be added.</summary>
		Never,
	}

	[NoMacCatalyst]
	[Native (ConvertToNative = "NSImageResizingModeExtensions.ToNative", ConvertToManaged = "NSImageResizingModeExtensions.ToManaged")]
	public enum NSImageResizingMode : long {
		/// <summary>To be added.</summary>
		Stretch,
		/// <summary>To be added.</summary>
		Tile,
	}

	#endregion

	#region NSAlert
	[NoMacCatalyst]
	[Native]
	public enum NSAlertStyle : ulong {
		/// <summary>To be added.</summary>
		Warning,
		/// <summary>To be added.</summary>
		Informational,
		/// <summary>To be added.</summary>
		Critical,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSModalResponse : long {
		/// <summary>To be added.</summary>
		OK = 1,
		/// <summary>To be added.</summary>
		Cancel = 0,
		/// <summary>To be added.</summary>
		Stop = -1000,
		/// <summary>To be added.</summary>
		Abort = -1001,
		/// <summary>To be added.</summary>
		Continue = -1002,
	}
	#endregion

	#region NSEvent
	[NoMacCatalyst]
	[Native]
	public enum NSEventType : ulong {
		/// <summary>To be added.</summary>
		LeftMouseDown = 1,
		/// <summary>To be added.</summary>
		LeftMouseUp = 2,
		/// <summary>To be added.</summary>
		RightMouseDown = 3,
		/// <summary>To be added.</summary>
		RightMouseUp = 4,
		/// <summary>To be added.</summary>
		MouseMoved = 5,
		/// <summary>To be added.</summary>
		LeftMouseDragged = 6,
		/// <summary>To be added.</summary>
		RightMouseDragged = 7,
		/// <summary>To be added.</summary>
		MouseEntered = 8,
		/// <summary>To be added.</summary>
		MouseExited = 9,
		/// <summary>To be added.</summary>
		KeyDown = 10,
		/// <summary>To be added.</summary>
		KeyUp = 11,
		/// <summary>To be added.</summary>
		FlagsChanged = 12,
		/// <summary>To be added.</summary>
		AppKitDefined = 13,
		/// <summary>To be added.</summary>
		SystemDefined = 14,
		/// <summary>To be added.</summary>
		ApplicationDefined = 15,
		/// <summary>To be added.</summary>
		Periodic = 16,
		/// <summary>To be added.</summary>
		CursorUpdate = 17,

		/// <summary>To be added.</summary>
		ScrollWheel = 22,

		/// <summary>To be added.</summary>
		TabletPoint = 23,
		/// <summary>To be added.</summary>
		TabletProximity = 24,

		/// <summary>To be added.</summary>
		OtherMouseDown = 25,
		/// <summary>To be added.</summary>
		OtherMouseUp = 26,
		/// <summary>To be added.</summary>
		OtherMouseDragged = 27,

		/// <summary>To be added.</summary>
		Gesture = 29,
		/// <summary>To be added.</summary>
		Magnify = 30,
		/// <summary>To be added.</summary>
		Swipe = 31,
		/// <summary>To be added.</summary>
		Rotate = 18,
		/// <summary>To be added.</summary>
		BeginGesture = 19,
		/// <summary>To be added.</summary>
		EndGesture = 20,

		/// <summary>To be added.</summary>
		SmartMagnify = 32,
		/// <summary>To be added.</summary>
		QuickLook = 33,
		/// <summary>To be added.</summary>
		Pressure = 34, // 10.10.3, 64-bit-only
		/// <summary>To be added.</summary>
		DirectTouch = 37, // 10.10
		ChangeMode = 38,
	}

	[NoMacCatalyst]
	[Flags]
	public enum NSEventMask : ulong {
		/// <summary>To be added.</summary>
		LeftMouseDown = 1UL << (int) NSEventType.LeftMouseDown,
		/// <summary>To be added.</summary>
		LeftMouseUp = 1UL << (int) NSEventType.LeftMouseUp,
		/// <summary>To be added.</summary>
		RightMouseDown = 1UL << (int) NSEventType.RightMouseDown,
		/// <summary>To be added.</summary>
		RightMouseUp = 1UL << (int) NSEventType.RightMouseUp,
		/// <summary>To be added.</summary>
		MouseMoved = 1UL << (int) NSEventType.MouseMoved,
		/// <summary>To be added.</summary>
		LeftMouseDragged = 1UL << (int) NSEventType.LeftMouseDragged,
		/// <summary>To be added.</summary>
		RightMouseDragged = 1UL << (int) NSEventType.RightMouseDragged,
		/// <summary>To be added.</summary>
		MouseEntered = 1UL << (int) NSEventType.MouseEntered,
		/// <summary>To be added.</summary>
		MouseExited = 1UL << (int) NSEventType.MouseExited,
		/// <summary>To be added.</summary>
		KeyDown = 1UL << (int) NSEventType.KeyDown,
		/// <summary>To be added.</summary>
		KeyUp = 1UL << (int) NSEventType.KeyUp,
		/// <summary>To be added.</summary>
		FlagsChanged = 1UL << (int) NSEventType.FlagsChanged,
		/// <summary>To be added.</summary>
		AppKitDefined = 1UL << (int) NSEventType.AppKitDefined,
		/// <summary>To be added.</summary>
		SystemDefined = 1UL << (int) NSEventType.SystemDefined,
		/// <summary>To be added.</summary>
		ApplicationDefined = 1UL << (int) NSEventType.ApplicationDefined,
		/// <summary>To be added.</summary>
		Periodic = 1UL << (int) NSEventType.Periodic,
		/// <summary>To be added.</summary>
		CursorUpdate = 1UL << (int) NSEventType.CursorUpdate,
		/// <summary>To be added.</summary>
		ScrollWheel = 1UL << (int) NSEventType.ScrollWheel,
		/// <summary>To be added.</summary>
		TabletPoint = 1UL << (int) NSEventType.TabletPoint,
		/// <summary>To be added.</summary>
		TabletProximity = 1UL << (int) NSEventType.TabletProximity,
		/// <summary>To be added.</summary>
		OtherMouseDown = 1UL << (int) NSEventType.OtherMouseDown,
		/// <summary>To be added.</summary>
		OtherMouseUp = 1UL << (int) NSEventType.OtherMouseUp,
		/// <summary>To be added.</summary>
		OtherMouseDragged = 1UL << (int) NSEventType.OtherMouseDragged,
		/// <summary>To be added.</summary>
		EventGesture = 1UL << (int) NSEventType.Gesture,
		/// <summary>To be added.</summary>
		EventMagnify = 1UL << (int) NSEventType.Magnify,
		/// <summary>To be added.</summary>
		EventSwipe = 1UL << (int) NSEventType.Swipe,
		/// <summary>To be added.</summary>
		EventRotate = 1UL << (int) NSEventType.Rotate,
		/// <summary>To be added.</summary>
		EventBeginGesture = 1UL << (int) NSEventType.BeginGesture,
		/// <summary>To be added.</summary>
		EventEndGesture = 1UL << (int) NSEventType.EndGesture,
		/// <summary>To be added.</summary>
		SmartMagnify = 1UL << (int) NSEventType.SmartMagnify,
		/// <summary>To be added.</summary>
		Pressure = 1UL << (int) NSEventType.Pressure, // 10.10.3, 64-bit-only
		/// <summary>To be added.</summary>
		DirectTouch = 1UL << (int) NSEventType.DirectTouch, // 10.10
		ChangeMode = 1UL << (int) NSEventType.ChangeMode,
		/// <summary>To be added.</summary>
		AnyEvent = unchecked((ulong) UInt64.MaxValue),
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSEventModifierMask : ulong {
		/// <summary>To be added.</summary>
		AlphaShiftKeyMask = 1 << 16,
		/// <summary>To be added.</summary>
		ShiftKeyMask = 1 << 17,
		/// <summary>To be added.</summary>
		ControlKeyMask = 1 << 18,
		/// <summary>To be added.</summary>
		AlternateKeyMask = 1 << 19,
		/// <summary>To be added.</summary>
		CommandKeyMask = 1 << 20,
		/// <summary>To be added.</summary>
		NumericPadKeyMask = 1 << 21,
		/// <summary>To be added.</summary>
		HelpKeyMask = 1 << 22,
		/// <summary>To be added.</summary>
		FunctionKeyMask = 1 << 23,
		/// <summary>To be added.</summary>
		DeviceIndependentModifierFlagsMask = 0xffff0000,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPointingDeviceType : ulong {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Pen,
		/// <summary>To be added.</summary>
		Cursor,
		/// <summary>To be added.</summary>
		Eraser,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSEventButtonMask : ulong {
		/// <summary>To be added.</summary>
		Pen = 1,
		/// <summary>To be added.</summary>
		PenLower = 2,
		/// <summary>To be added.</summary>
		PenUpper = 4,
	}

	// This enum is defined as an untyped enum in MacOSX.sdk/System/Library/Frameworks/Carbon.framework/Versions/A/Frameworks/HIToolbox.framework/Versions/A/Headers/Events.h
	// It represents values that may be returned by NSEvent.KeyCode (which isn't typed as 'NSKey' because it may be many other values as well).
	[NoMacCatalyst]
	public enum NSKey {
		/// <summary>To be added.</summary>
		A = 0x00,
		/// <summary>To be added.</summary>
		S = 0x01,
		/// <summary>To be added.</summary>
		D = 0x02,
		/// <summary>To be added.</summary>
		F = 0x03,
		/// <summary>To be added.</summary>
		H = 0x04,
		/// <summary>To be added.</summary>
		G = 0x05,
		/// <summary>To be added.</summary>
		Z = 0x06,
		/// <summary>To be added.</summary>
		X = 0x07,
		/// <summary>To be added.</summary>
		C = 0x08,
		/// <summary>To be added.</summary>
		V = 0x09,
		/// <summary>To be added.</summary>
		B = 0x0B,
		/// <summary>To be added.</summary>
		Q = 0x0C,
		/// <summary>To be added.</summary>
		W = 0x0D,
		/// <summary>To be added.</summary>
		E = 0x0E,
		/// <summary>To be added.</summary>
		R = 0x0F,
		/// <summary>To be added.</summary>
		Y = 0x10,
		/// <summary>To be added.</summary>
		T = 0x11,
		/// <summary>To be added.</summary>
		D1 = 0x12,
		/// <summary>To be added.</summary>
		D2 = 0x13,
		/// <summary>To be added.</summary>
		D3 = 0x14,
		/// <summary>To be added.</summary>
		D4 = 0x15,
		/// <summary>To be added.</summary>
		D6 = 0x16,
		/// <summary>To be added.</summary>
		D5 = 0x17,
		/// <summary>To be added.</summary>
		Equal = 0x18,
		/// <summary>To be added.</summary>
		D9 = 0x19,
		/// <summary>To be added.</summary>
		D7 = 0x1A,
		/// <summary>To be added.</summary>
		Minus = 0x1B,
		/// <summary>To be added.</summary>
		D8 = 0x1C,
		/// <summary>To be added.</summary>
		D0 = 0x1D,
		/// <summary>To be added.</summary>
		RightBracket = 0x1E,
		/// <summary>To be added.</summary>
		O = 0x1F,
		/// <summary>To be added.</summary>
		U = 0x20,
		/// <summary>To be added.</summary>
		LeftBracket = 0x21,
		/// <summary>To be added.</summary>
		I = 0x22,
		/// <summary>To be added.</summary>
		P = 0x23,
		/// <summary>To be added.</summary>
		L = 0x25,
		/// <summary>To be added.</summary>
		J = 0x26,
		/// <summary>To be added.</summary>
		Quote = 0x27,
		/// <summary>To be added.</summary>
		K = 0x28,
		/// <summary>To be added.</summary>
		Semicolon = 0x29,
		/// <summary>To be added.</summary>
		Backslash = 0x2A,
		/// <summary>To be added.</summary>
		Comma = 0x2B,
		/// <summary>To be added.</summary>
		Slash = 0x2C,
		/// <summary>To be added.</summary>
		N = 0x2D,
		/// <summary>To be added.</summary>
		M = 0x2E,
		/// <summary>To be added.</summary>
		Period = 0x2F,
		/// <summary>To be added.</summary>
		Grave = 0x32,
		/// <summary>To be added.</summary>
		KeypadDecimal = 0x41,
		/// <summary>To be added.</summary>
		KeypadMultiply = 0x43,
		/// <summary>To be added.</summary>
		KeypadPlus = 0x45,
		/// <summary>To be added.</summary>
		KeypadClear = 0x47,
		/// <summary>To be added.</summary>
		KeypadDivide = 0x4B,
		/// <summary>To be added.</summary>
		KeypadEnter = 0x4C,
		/// <summary>To be added.</summary>
		KeypadMinus = 0x4E,
		/// <summary>To be added.</summary>
		KeypadEquals = 0x51,
		/// <summary>To be added.</summary>
		Keypad0 = 0x52,
		/// <summary>To be added.</summary>
		Keypad1 = 0x53,
		/// <summary>To be added.</summary>
		Keypad2 = 0x54,
		/// <summary>To be added.</summary>
		Keypad3 = 0x55,
		/// <summary>To be added.</summary>
		Keypad4 = 0x56,
		/// <summary>To be added.</summary>
		Keypad5 = 0x57,
		/// <summary>To be added.</summary>
		Keypad6 = 0x58,
		/// <summary>To be added.</summary>
		Keypad7 = 0x59,
		/// <summary>To be added.</summary>
		Keypad8 = 0x5B,
		/// <summary>To be added.</summary>
		Keypad9 = 0x5C,
		/// <summary>To be added.</summary>
		Return = 0x24,
		/// <summary>To be added.</summary>
		Tab = 0x30,
		/// <summary>To be added.</summary>
		Space = 0x31,
		/// <summary>To be added.</summary>
		Delete = 0x33,
		/// <summary>To be added.</summary>
		Escape = 0x35,
		/// <summary>To be added.</summary>
		Command = 0x37,
		/// <summary>To be added.</summary>
		Shift = 0x38,
		/// <summary>To be added.</summary>
		CapsLock = 0x39,
		/// <summary>To be added.</summary>
		Option = 0x3A,
		/// <summary>To be added.</summary>
		Control = 0x3B,
		RightCommand = 0x36,
		/// <summary>To be added.</summary>
		RightShift = 0x3C,
		/// <summary>To be added.</summary>
		RightOption = 0x3D,
		/// <summary>To be added.</summary>
		RightControl = 0x3E,
		/// <summary>To be added.</summary>
		Function = 0x3F,
		F17 = 0x40,
		/// <summary>To be added.</summary>
		VolumeUp = 0x48,
		/// <summary>To be added.</summary>
		VolumeDown = 0x49,
		/// <summary>To be added.</summary>
		Mute = 0x4A,
		/// <summary>To be added.</summary>
		ForwardDelete = 0x75,
		/// <summary>To be added.</summary>
		ISOSection = 0x0A,
		/// <summary>To be added.</summary>
		JISYen = 0x5D,
		/// <summary>To be added.</summary>
		JISUnderscore = 0x5E,
		/// <summary>To be added.</summary>
		JISKeypadComma = 0x5F,
		/// <summary>To be added.</summary>
		JISEisu = 0x66,
		/// <summary>To be added.</summary>
		JISKana = 0x68,
		/// <summary>To be added.</summary>
		F18 = 0x4F,
		/// <summary>To be added.</summary>
		F19 = 0x50,
		/// <summary>To be added.</summary>
		F20 = 0x5A,
		/// <summary>To be added.</summary>
		F5 = 0x60,
		/// <summary>To be added.</summary>
		F6 = 0x61,
		/// <summary>To be added.</summary>
		F7 = 0x62,
		/// <summary>To be added.</summary>
		F3 = 0x63,
		/// <summary>To be added.</summary>
		F8 = 0x64,
		/// <summary>To be added.</summary>
		F9 = 0x65,
		/// <summary>To be added.</summary>
		F11 = 0x67,
		/// <summary>To be added.</summary>
		F13 = 0x69,
		/// <summary>To be added.</summary>
		F16 = 0x6A,
		/// <summary>To be added.</summary>
		F14 = 0x6B,
		/// <summary>To be added.</summary>
		F10 = 0x6D,
		/// <summary>To be added.</summary>
		F12 = 0x6F,
		/// <summary>To be added.</summary>
		F15 = 0x71,
		/// <summary>To be added.</summary>
		Help = 0x72,
		/// <summary>To be added.</summary>
		Home = 0x73,
		/// <summary>To be added.</summary>
		PageUp = 0x74,
		/// <summary>To be added.</summary>
		F4 = 0x76,
		/// <summary>To be added.</summary>
		End = 0x77,
		/// <summary>To be added.</summary>
		F2 = 0x78,
		/// <summary>To be added.</summary>
		PageDown = 0x79,
		/// <summary>To be added.</summary>
		F1 = 0x7A,
		/// <summary>To be added.</summary>
		LeftArrow = 0x7B,
		/// <summary>To be added.</summary>
		RightArrow = 0x7C,
		/// <summary>To be added.</summary>
		DownArrow = 0x7D,
		/// <summary>To be added.</summary>
		UpArrow = 0x7E,
	}

	// This is an untyped enum in AppKit's NSEvent.h
	[NoMacCatalyst]
	public enum NSFunctionKey : int {
		/// <summary>To be added.</summary>
		UpArrow = 0xF700,
		/// <summary>To be added.</summary>
		DownArrow = 0xF701,
		/// <summary>To be added.</summary>
		LeftArrow = 0xF702,
		/// <summary>To be added.</summary>
		RightArrow = 0xF703,
		/// <summary>To be added.</summary>
		F1 = 0xF704,
		/// <summary>To be added.</summary>
		F2 = 0xF705,
		/// <summary>To be added.</summary>
		F3 = 0xF706,
		/// <summary>To be added.</summary>
		F4 = 0xF707,
		/// <summary>To be added.</summary>
		F5 = 0xF708,
		/// <summary>To be added.</summary>
		F6 = 0xF709,
		/// <summary>To be added.</summary>
		F7 = 0xF70A,
		/// <summary>To be added.</summary>
		F8 = 0xF70B,
		/// <summary>To be added.</summary>
		F9 = 0xF70C,
		/// <summary>To be added.</summary>
		F10 = 0xF70D,
		/// <summary>To be added.</summary>
		F11 = 0xF70E,
		/// <summary>To be added.</summary>
		F12 = 0xF70F,
		/// <summary>To be added.</summary>
		F13 = 0xF710,
		/// <summary>To be added.</summary>
		F14 = 0xF711,
		/// <summary>To be added.</summary>
		F15 = 0xF712,
		/// <summary>To be added.</summary>
		F16 = 0xF713,
		/// <summary>To be added.</summary>
		F17 = 0xF714,
		/// <summary>To be added.</summary>
		F18 = 0xF715,
		/// <summary>To be added.</summary>
		F19 = 0xF716,
		/// <summary>To be added.</summary>
		F20 = 0xF717,
		/// <summary>To be added.</summary>
		F21 = 0xF718,
		/// <summary>To be added.</summary>
		F22 = 0xF719,
		/// <summary>To be added.</summary>
		F23 = 0xF71A,
		/// <summary>To be added.</summary>
		F24 = 0xF71B,
		/// <summary>To be added.</summary>
		F25 = 0xF71C,
		/// <summary>To be added.</summary>
		F26 = 0xF71D,
		/// <summary>To be added.</summary>
		F27 = 0xF71E,
		/// <summary>To be added.</summary>
		F28 = 0xF71F,
		/// <summary>To be added.</summary>
		F29 = 0xF720,
		/// <summary>To be added.</summary>
		F30 = 0xF721,
		/// <summary>To be added.</summary>
		F31 = 0xF722,
		/// <summary>To be added.</summary>
		F32 = 0xF723,
		/// <summary>To be added.</summary>
		F33 = 0xF724,
		/// <summary>To be added.</summary>
		F34 = 0xF725,
		/// <summary>To be added.</summary>
		F35 = 0xF726,
		/// <summary>To be added.</summary>
		Insert = 0xF727,
		/// <summary>To be added.</summary>
		Delete = 0xF728,
		/// <summary>To be added.</summary>
		Home = 0xF729,
		/// <summary>To be added.</summary>
		Begin = 0xF72A,
		/// <summary>To be added.</summary>
		End = 0xF72B,
		/// <summary>To be added.</summary>
		PageUp = 0xF72C,
		/// <summary>To be added.</summary>
		PageDown = 0xF72D,
		/// <summary>To be added.</summary>
		PrintScreen = 0xF72E,
		/// <summary>To be added.</summary>
		ScrollLock = 0xF72F,
		/// <summary>To be added.</summary>
		Pause = 0xF730,
		/// <summary>To be added.</summary>
		SysReq = 0xF731,
		/// <summary>To be added.</summary>
		Break = 0xF732,
		/// <summary>To be added.</summary>
		Reset = 0xF733,
		/// <summary>To be added.</summary>
		Stop = 0xF734,
		/// <summary>To be added.</summary>
		Menu = 0xF735,
		/// <summary>To be added.</summary>
		User = 0xF736,
		/// <summary>To be added.</summary>
		System = 0xF737,
		/// <summary>To be added.</summary>
		Print = 0xF738,
		/// <summary>To be added.</summary>
		ClearLine = 0xF739,
		/// <summary>To be added.</summary>
		ClearDisplay = 0xF73A,
		/// <summary>To be added.</summary>
		InsertLine = 0xF73B,
		/// <summary>To be added.</summary>
		DeleteLine = 0xF73C,
		/// <summary>To be added.</summary>
		InsertChar = 0xF73D,
		/// <summary>To be added.</summary>
		DeleteChar = 0xF73E,
		/// <summary>To be added.</summary>
		Prev = 0xF73F,
		/// <summary>To be added.</summary>
		Next = 0xF740,
		/// <summary>To be added.</summary>
		Select = 0xF741,
		/// <summary>To be added.</summary>
		Execute = 0xF742,
		/// <summary>To be added.</summary>
		Undo = 0xF743,
		/// <summary>To be added.</summary>
		Redo = 0xF744,
		/// <summary>To be added.</summary>
		Find = 0xF745,
		/// <summary>To be added.</summary>
		Help = 0xF746,
		/// <summary>To be added.</summary>
		ModeSwitch = 0xF747,
	}

	[NoMacCatalyst]
	public enum NSEventSubtype : short {
		/* event subtypes for NSEventTypeAppKitDefined events */
		/// <summary>To be added.</summary>
		WindowExposed = 0,
		/// <summary>To be added.</summary>
		ApplicationActivated = 1,
		/// <summary>To be added.</summary>
		ApplicationDeactivated = 2,
		/// <summary>To be added.</summary>
		WindowMoved = 4,
		/// <summary>To be added.</summary>
		ScreenChanged = 8,
		/* event subtypes for NSEventTypeSystemDefined events */
		/* the value is repeated from above */
		PowerOff = 1,

		/* event subtypes for mouse events */
		/* the values are repeated from above */
		MouseEvent = 0, /* NX_SUBTYPE_DEFAULT */
		TabletPoint = 1, /* NX_SUBTYPE_TABLET_POINT */
		TabletProximity = 2, /* NX_SUBTYPE_TABLET_PROXIMITY */
		Touch = 3, /* NX_SUBTYPE_MOUSE_TOUCH */
	}

	#endregion

	#region NSView
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSViewResizingMask : ulong {
		/// <summary>To be added.</summary>
		NotSizable = 0,
		/// <summary>To be added.</summary>
		MinXMargin = 1,
		/// <summary>To be added.</summary>
		WidthSizable = 2,
		/// <summary>To be added.</summary>
		MaxXMargin = 4,
		/// <summary>To be added.</summary>
		MinYMargin = 8,
		/// <summary>To be added.</summary>
		HeightSizable = 16,
		/// <summary>To be added.</summary>
		MaxYMargin = 32,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSBorderType : ulong {
		/// <summary>To be added.</summary>
		NoBorder,
		/// <summary>To be added.</summary>
		LineBorder,
		/// <summary>To be added.</summary>
		BezelBorder,
		/// <summary>To be added.</summary>
		GrooveBorder,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextFieldBezelStyle : ulong {
		/// <summary>To be added.</summary>
		Square,
		/// <summary>To be added.</summary>
		Rounded,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSViewLayerContentsRedrawPolicy : long {
		/// <summary>To be added.</summary>
		Never,
		/// <summary>To be added.</summary>
		OnSetNeedsDisplay,
		/// <summary>To be added.</summary>
		DuringViewResize,
		/// <summary>To be added.</summary>
		BeforeViewResize,
		Crossfade = 4,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSViewLayerContentsPlacement : long {
		/// <summary>To be added.</summary>
		ScaleAxesIndependently,
		/// <summary>To be added.</summary>
		ScaleProportionallyToFit,
		/// <summary>To be added.</summary>
		ScaleProportionallyToFill,
		/// <summary>To be added.</summary>
		Center,
		/// <summary>To be added.</summary>
		Top,
		/// <summary>To be added.</summary>
		TopRight,
		/// <summary>To be added.</summary>
		Right,
		/// <summary>To be added.</summary>
		BottomRight,
		/// <summary>To be added.</summary>
		Bottom,
		/// <summary>To be added.</summary>
		BottomLeft,
		/// <summary>To be added.</summary>
		Left,
		/// <summary>To be added.</summary>
		TopLeft,
	}

	#endregion

	#region NSWindow
	[NoMacCatalyst]
	[Flags]
	[Native ("NSWindowStyleMask")]
	public enum NSWindowStyle : ulong {
		/// <summary>To be added.</summary>
		Borderless = 0 << 0,
		/// <summary>To be added.</summary>
		Titled = 1 << 0,
		/// <summary>To be added.</summary>
		Closable = 1 << 1,
		/// <summary>To be added.</summary>
		Miniaturizable = 1 << 2,
		/// <summary>To be added.</summary>
		Resizable = 1 << 3,
		/// <summary>To be added.</summary>
		Utility = 1 << 4,
		/// <summary>To be added.</summary>
		DocModal = 1 << 6,
		/// <summary>To be added.</summary>
		NonactivatingPanel = 1 << 7,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Don't use 'TexturedBackground' anymore.")]
		TexturedBackground = 1 << 8,
		/// <summary>To be added.</summary>
		UnifiedTitleAndToolbar = 1 << 12,
		/// <summary>To be added.</summary>
		Hud = 1 << 13,
		/// <summary>To be added.</summary>
		FullScreenWindow = 1 << 14,
		/// <summary>To be added.</summary>
		FullSizeContentView = 1 << 15,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowSharingType : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		ReadOnly,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'ReadOnly' instead.")]
		ReadWrite,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowBackingLocation : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		VideoMemory,
		/// <summary>To be added.</summary>
		MainMemory,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowCollectionBehavior : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		CanJoinAllSpaces = 1 << 0,
		/// <summary>To be added.</summary>
		MoveToActiveSpace = 1 << 1,
		/// <summary>To be added.</summary>
		Managed = 1 << 2,
		/// <summary>To be added.</summary>
		Transient = 1 << 3,
		/// <summary>To be added.</summary>
		Stationary = 1 << 4,
		/// <summary>To be added.</summary>
		ParticipatesInCycle = 1 << 5,
		/// <summary>To be added.</summary>
		IgnoresCycle = 1 << 6,
		/// <summary>To be added.</summary>
		FullScreenPrimary = 1 << 7,
		/// <summary>To be added.</summary>
		FullScreenAuxiliary = 1 << 8,
		/// <summary>To be added.</summary>
		FullScreenNone = 1 << 9,
		/// <summary>To be added.</summary>
		FullScreenAllowsTiling = 1 << 11,
		/// <summary>To be added.</summary>
		FullScreenDisallowsTiling = 1 << 12,
		Primary = 1 << 16,
		Auxiliary = 1 << 17,
		CanJoinAllApplications = 1 << 18,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowNumberListOptions : ulong {
		/// <summary>To be added.</summary>
		AllApplication = 1 << 0,
		/// <summary>To be added.</summary>
		AllSpaces = 1 << 4,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSelectionDirection : ulong {
		/// <summary>To be added.</summary>
		Direct = 0,
		/// <summary>To be added.</summary>
		Next,
		/// <summary>To be added.</summary>
		Previous,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowButton : ulong {
		/// <summary>To be added.</summary>
		CloseButton,
		/// <summary>To be added.</summary>
		MiniaturizeButton,
		/// <summary>To be added.</summary>
		ZoomButton,
		/// <summary>To be added.</summary>
		ToolbarButton,
		/// <summary>To be added.</summary>
		DocumentIconButton,
		/// <summary>To be added.</summary>
		DocumentVersionsButton = 6,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "The standard window button for FullScreenButton is always null; use ZoomButton instead.")]
		FullScreenButton,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTouchPhase : ulong {
		/// <summary>To be added.</summary>
		Began = 1 << 0,
		/// <summary>To be added.</summary>
		Moved = 1 << 1,
		/// <summary>To be added.</summary>
		Stationary = 1 << 2,
		/// <summary>To be added.</summary>
		Ended = 1 << 3,
		/// <summary>To be added.</summary>
		Cancelled = 1 << 4,

		/// <summary>To be added.</summary>
		Touching = Began | Moved | Stationary,
		/// <summary>To be added.</summary>
		Any = unchecked((ulong) UInt64.MaxValue),
	}
	#endregion
	#region NSAnimation

	[NoMacCatalyst]
	[Native]
	public enum NSAnimationCurve : ulong {
		/// <summary>To be added.</summary>
		EaseInOut,
		/// <summary>To be added.</summary>
		EaseIn,
		/// <summary>To be added.</summary>
		EaseOut,
		/// <summary>To be added.</summary>
		Linear,
	};

	[NoMacCatalyst]
	[Native]
	public enum NSAnimationBlockingMode : ulong {
		/// <summary>To be added.</summary>
		Blocking,
		/// <summary>To be added.</summary>
		Nonblocking,
		/// <summary>To be added.</summary>
		NonblockingThreaded,
	};
	#endregion

	#region NSBox

	[NoMacCatalyst]
	[Native]
	public enum NSTitlePosition : ulong {
		/// <summary>To be added.</summary>
		NoTitle,
		/// <summary>To be added.</summary>
		AboveTop,
		/// <summary>To be added.</summary>
		AtTop,
		/// <summary>To be added.</summary>
		BelowTop,
		/// <summary>To be added.</summary>
		AboveBottom,
		/// <summary>To be added.</summary>
		AtBottom,
		/// <summary>To be added.</summary>
		BelowBottom,
	};

	[NoMacCatalyst]
	[Native]
	public enum NSBoxType : ulong {
		/// <summary>To be added.</summary>
		NSBoxPrimary,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 15, message: "Identical to 'NSBoxPrimary'.")]
		NSBoxSecondary,
		/// <summary>To be added.</summary>
		NSBoxSeparator,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 15, message: "'NSBoxOldStyle' is discouraged. Use 'NSBoxPrimary' or 'NSBoxCustom'.")]
		NSBoxOldStyle,
		/// <summary>To be added.</summary>
		NSBoxCustom,
	};
	#endregion

	#region NSButtonCell
	[NoMacCatalyst]
	[Native]
	public enum NSButtonType : ulong {
		/// <summary>To be added.</summary>
		MomentaryLightButton,
		/// <summary>To be added.</summary>
		PushOnPushOff,
		/// <summary>To be added.</summary>
		Toggle,
		/// <summary>To be added.</summary>
		Switch,
		/// <summary>To be added.</summary>
		Radio,
		/// <summary>To be added.</summary>
		MomentaryChange,
		/// <summary>To be added.</summary>
		OnOff,
		/// <summary>To be added.</summary>
		MomentaryPushIn,
		/// <summary>To be added.</summary>
		Accelerator, // 10.10.3
		/// <summary>To be added.</summary>
		MultiLevelAccelerator, // 10.10.3
	}

	[NoMacCatalyst]
	[Native]
	public enum NSBezelStyle : ulong {
		Automatic = 0,
		Push = 1,
		FlexiblePush = 2,
		/// <summary>To be added.</summary>
		Disclosure = 5,
		/// <summary>To be added.</summary>
		Circular = 7,
		/// <summary>To be added.</summary>
		HelpButton = 9,
		/// <summary>To be added.</summary>
		SmallSquare = 10,
		Toolbar = 11,
		AccessoryBarAction = 12,
		AccessoryBar = 13,
		PushDisclosure = 14,
		Badge = 15,
#if !XAMCORE_5_0
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'Push' instead.")]
		Rounded = 1,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'FlexiblePush' instead.")]
		RegularSquare = 2,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 12, message: "Use 'FlexiblePush' instead.")]
		ThickSquare = 3,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 12, message: "Use 'FlexiblePush' instead.")]
		ThickerSquare = 4,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'SmallSquare' instead.")]
		ShadowlessSquare = 6,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'SmallSquare' instead.")]
		TexturedSquare = 8,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'Toolbar' instead.")]
		TexturedRounded = 11,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'AccessoryBarAction' instead.")]
		RoundRect = 12,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'AccessoryBar' instead.")]
		Recessed = 13,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'PushDisclosure' instead.")]
		RoundedDisclosure = 14,
		/// <summary>To be added.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 14, message: "Use 'Badge' instead.")]
		Inline = 15,
#endif // !XAMCORE_5_0
	}

	[NoMacCatalyst]
	[Native]
	[Deprecated (PlatformName.MacOSX, 10, 12, message: "The GradientType property is unused, and setting it has no effect.")]
	public enum NSGradientType : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		ConcaveWeak,
		/// <summary>To be added.</summary>
		ConcaveStrong,
		/// <summary>To be added.</summary>
		ConvexWeak,
		/// <summary>To be added.</summary>
		ConvexStrong,
	}

	#endregion

	#region NSGraphics
	[NoMacCatalyst]
	// NSGraphics.h:typedef int NSWindowDepth;
	public enum NSWindowDepth : int {
		/// <summary>To be added.</summary>
		TwentyfourBitRgb = 0x208,
		/// <summary>To be added.</summary>
		SixtyfourBitRgb = 0x210,
		/// <summary>To be added.</summary>
		OneHundredTwentyEightBitRgb = 0x220,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCompositingOperation : ulong {
		/// <summary>To be added.</summary>
		Clear,
		/// <summary>To be added.</summary>
		Copy,
		/// <summary>To be added.</summary>
		SourceOver,
		/// <summary>To be added.</summary>
		SourceIn,
		/// <summary>To be added.</summary>
		SourceOut,
		/// <summary>To be added.</summary>
		SourceAtop,
		/// <summary>To be added.</summary>
		DestinationOver,
		/// <summary>To be added.</summary>
		DestinationIn,
		/// <summary>To be added.</summary>
		DestinationOut,
		/// <summary>To be added.</summary>
		DestinationAtop,
		/// <summary>To be added.</summary>
		Xor,
		/// <summary>To be added.</summary>
		PlusDarker,
		/// <summary>To be added.</summary>
		Highlight,
		/// <summary>To be added.</summary>
		PlusLighter,

		/// <summary>To be added.</summary>
		Multiply,
		/// <summary>To be added.</summary>
		Screen,
		/// <summary>To be added.</summary>
		Overlay,
		/// <summary>To be added.</summary>
		Darken,
		/// <summary>To be added.</summary>
		Lighten,
		/// <summary>To be added.</summary>
		ColorDodge,
		/// <summary>To be added.</summary>
		ColorBurn,
		/// <summary>To be added.</summary>
		SoftLight,
		/// <summary>To be added.</summary>
		HardLight,
		/// <summary>To be added.</summary>
		Difference,
		/// <summary>To be added.</summary>
		Exclusion,
		/// <summary>To be added.</summary>
		Hue,
		/// <summary>To be added.</summary>
		Saturation,
		/// <summary>To be added.</summary>
		Color,
		/// <summary>To be added.</summary>
		Luminosity,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSAnimationEffect : ulong {
		/// <summary>To be added.</summary>
		DissapearingItemDefault = 0,
		/// <summary>To be added.</summary>
		EffectPoof = 10,
	}
	#endregion

	#region NSMatrix
	[NoMacCatalyst]
	[Native]
	public enum NSMatrixMode : ulong {
		/// <summary>To be added.</summary>
		Radio,
		/// <summary>To be added.</summary>
		Highlight,
		/// <summary>To be added.</summary>
		List,
		/// <summary>To be added.</summary>
		Track,
	}
	#endregion

	#region NSBrowser
	[NoMacCatalyst]
	[Native]
	public enum NSBrowserColumnResizingType : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Auto,
		/// <summary>To be added.</summary>
		User,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSBrowserDropOperation : ulong {
		/// <summary>To be added.</summary>
		On,
		/// <summary>To be added.</summary>
		Above,
	}
	#endregion

	#region NSColorPanel
	[NoMacCatalyst]
	[Native]
	public enum NSColorPanelMode : long {
		/// <summary>To be added.</summary>
		None = -1,
		/// <summary>To be added.</summary>
		Gray = 0,
		/// <summary>To be added.</summary>
		RGB,
		/// <summary>To be added.</summary>
		CMYK,
		/// <summary>To be added.</summary>
		HSB,
		/// <summary>To be added.</summary>
		CustomPalette,
		/// <summary>To be added.</summary>
		ColorList,
		/// <summary>To be added.</summary>
		Wheel,
		/// <summary>To be added.</summary>
		Crayon,
	};

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSColorPanelFlags : ulong {
		/// <summary>To be added.</summary>
		Gray = 0x00000001,
		/// <summary>To be added.</summary>
		RGB = 0x00000002,
		/// <summary>To be added.</summary>
		CMYK = 0x00000004,
		/// <summary>To be added.</summary>
		HSB = 0x00000008,
		/// <summary>To be added.</summary>
		CustomPalette = 0x00000010,
		/// <summary>To be added.</summary>
		ColorList = 0x00000020,
		/// <summary>To be added.</summary>
		Wheel = 0x00000040,
		/// <summary>To be added.</summary>
		Crayon = 0x00000080,
		/// <summary>To be added.</summary>
		All = 0x0000ffff,
	}


	#endregion
	#region NSDocument

	[NoMacCatalyst]
	[Native]
	public enum NSDocumentChangeType : ulong {
		/// <summary>To be added.</summary>
		Done,
		/// <summary>To be added.</summary>
		Undone,
		/// <summary>To be added.</summary>
		Cleared,
		/// <summary>To be added.</summary>
		ReadOtherContents,
		/// <summary>To be added.</summary>
		Autosaved,
		/// <summary>To be added.</summary>
		Redone,
		/// <summary>To be added.</summary>
		Discardable = 256, /* New in Lion */
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSaveOperationType : ulong {
		/// <summary>To be added.</summary>
		Save,
		/// <summary>To be added.</summary>
		SaveAs,
		/// <summary>To be added.</summary>
		SaveTo,
		/// <summary>To be added.</summary>
		Autosave = 3,   /* Deprecated name in Lion */
		/// <summary>To be added.</summary>
		Elsewhere = 3,  /* New Lion name */
		/// <summary>To be added.</summary>
		InPlace = 4,    /* New in Lion */
		/// <summary>To be added.</summary>
		AutoSaveAs = 5, /* New in Mountain Lion */
	}

	#endregion

	#region NSBezelPath

	[NoMacCatalyst]
	[Native]
	public enum NSLineCapStyle : ulong {
		/// <summary>To be added.</summary>
		Butt,
		/// <summary>To be added.</summary>
		Round,
		/// <summary>To be added.</summary>
		Square,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSLineJoinStyle : ulong {
		/// <summary>To be added.</summary>
		Miter,
		/// <summary>To be added.</summary>
		Round,
		/// <summary>To be added.</summary>
		Bevel,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindingRule : ulong {
		/// <summary>To be added.</summary>
		NonZero,
		/// <summary>To be added.</summary>
		EvenOdd,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSBezierPathElement : ulong {
		/// <summary>To be added.</summary>
		MoveTo,
		/// <summary>To be added.</summary>
		LineTo,
		/// <summary>To be added.</summary>
		CurveTo,
		/// <summary>To be added.</summary>
		ClosePath,
		[Mac (14, 0)]
		QuadraticCurveTo,
	}
	#endregion

	#region NSRulerView
	[NoMacCatalyst]
	[Native]
	public enum NSRulerOrientation : ulong {
		/// <summary>To be added.</summary>
		Horizontal,
		/// <summary>To be added.</summary>
		Vertical,
	}
	#endregion

	#region NSGestureRecognizer
	[NoMacCatalyst]
	[Native]
	public enum NSGestureRecognizerState : long {
		/// <summary>To be added.</summary>
		Possible,
		/// <summary>To be added.</summary>
		Began,
		/// <summary>To be added.</summary>
		Changed,
		/// <summary>To be added.</summary>
		Ended,
		/// <summary>To be added.</summary>
		Cancelled,
		/// <summary>To be added.</summary>
		Failed,
		/// <summary>To be added.</summary>
		Recognized = NSGestureRecognizerState.Ended,
	}
	#endregion

	#region NSStackLayout
	[NoMacCatalyst]
	[Native]
	public enum NSUserInterfaceLayoutOrientation : long {
		/// <summary>To be added.</summary>
		Horizontal = 0,
		/// <summary>To be added.</summary>
		Vertical = 1,
	}

	// NSStackView.h:typedef float NSStackViewVisibilityPriority
	[NoMacCatalyst]
	public enum NSStackViewVisibilityPriority : int {
		/// <summary>To be added.</summary>
		MustHold = 1000,
		/// <summary>To be added.</summary>
		DetachOnlyIfNecessary = 900,
		/// <summary>To be added.</summary>
		NotVisible = 0,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSStackViewGravity : long {
		/// <summary>To be added.</summary>
		Top = 1,
		/// <summary>To be added.</summary>
		Leading = 1,
		/// <summary>To be added.</summary>
		Center = 2,
		/// <summary>To be added.</summary>
		Bottom = 3,
		/// <summary>To be added.</summary>
		Trailing = 3,
	}
	#endregion

	[NoMacCatalyst]
	[Native]
	public enum NSStackViewDistribution : long {
		/// <summary>To be added.</summary>
		GravityAreas = -1,
		/// <summary>To be added.</summary>
		Fill = 0,
		/// <summary>To be added.</summary>
		FillEqually,
		/// <summary>To be added.</summary>
		FillProportionally,
		/// <summary>To be added.</summary>
		EqualSpacing,
		/// <summary>To be added.</summary>
		EqualCentering,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSDragOperation : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Copy = 1,
		/// <summary>To be added.</summary>
		Link = 2,
		/// <summary>To be added.</summary>
		Generic = 4,
		/// <summary>To be added.</summary>
		Private = 8,
		/// <summary>To be added.</summary>
		AllObsolete = 15,
		/// <summary>To be added.</summary>
		Move = 16,
		/// <summary>To be added.</summary>
		Delete = 32,
		/// <summary>To be added.</summary>
		All = ulong.MaxValue,
	}

	[NoMacCatalyst]
	[Native (ConvertToNative = "NSTextAlignmentExtensions.ToNative", ConvertToManaged = "NSTextAlignmentExtensions.ToManaged")]
	public enum NSTextAlignment : ulong {
		/// <summary>To be added.</summary>
		Left = 0,
		/// <summary>To be added.</summary>
		Right = 1,
		/// <summary>To be added.</summary>
		Center = 2,
		/// <summary>To be added.</summary>
		Justified = 3,
		/// <summary>To be added.</summary>
		Natural = 4,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextMovement : long {
		/// <summary>To be added.</summary>
		Other = 0,
		/// <summary>To be added.</summary>
		Return = 0x10,
		/// <summary>To be added.</summary>
		Tab = 0x11,
		/// <summary>To be added.</summary>
		Backtab = 0x12,
		/// <summary>To be added.</summary>
		Left = 0x13,
		/// <summary>To be added.</summary>
		Right = 0x14,
		/// <summary>To be added.</summary>
		Up = 0x15,
		/// <summary>To be added.</summary>
		Down = 0x16,
		/// <summary>To be added.</summary>
		Cancel = 0x17,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSMenuProperty : ulong {
		/// <summary>To be added.</summary>
		Title = 1 << 0,
		/// <summary>To be added.</summary>
		AttributedTitle = 1 << 1,
		/// <summary>To be added.</summary>
		KeyEquivalent = 1 << 2,
		/// <summary>To be added.</summary>
		Image = 1 << 3,
		/// <summary>To be added.</summary>
		Enabled = 1 << 4,
		/// <summary>To be added.</summary>
		AccessibilityDescription = 1 << 5,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSFontRenderingMode : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		Antialiased,
		/// <summary>To be added.</summary>
		IntegerAdvancements,
		/// <summary>To be added.</summary>
		AntialiasedIntegerAdvancements,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSPasteboardReadingOptions : ulong {
		/// <summary>To be added.</summary>
		AsData = 0,
		/// <summary>To be added.</summary>
		AsString = 1,
		/// <summary>To be added.</summary>
		AsPropertyList = 2,
		/// <summary>To be added.</summary>
		AsKeyedArchive = 4,
	}

	// Convenience enum, untyped in ObjC
	[NoMacCatalyst]
	public enum NSUnderlinePattern : int {
		/// <summary>To be added.</summary>
		Solid = 0x0000,
		/// <summary>To be added.</summary>
		Dot = 0x0100,
		/// <summary>To be added.</summary>
		Dash = 0x0200,
		/// <summary>To be added.</summary>
		DashDot = 0x0300,
		/// <summary>To be added.</summary>
		DashDotDot = 0x0400,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSelectionAffinity : ulong {
		/// <summary>To be added.</summary>
		Upstream,
		/// <summary>To be added.</summary>
		Downstream,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSelectionGranularity : ulong {
		/// <summary>To be added.</summary>
		Character,
		/// <summary>To be added.</summary>
		Word,
		/// <summary>To be added.</summary>
		Paragraph,
	}

	#region NSTrackingArea
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTrackingAreaOptions : ulong {
		/// <summary>To be added.</summary>
		MouseEnteredAndExited = 0x01,
		/// <summary>To be added.</summary>
		MouseMoved = 0x02,
		/// <summary>To be added.</summary>
		CursorUpdate = 0x04,
		/// <summary>To be added.</summary>
		ActiveWhenFirstResponder = 0x10,
		/// <summary>To be added.</summary>
		ActiveInKeyWindow = 0x20,
		/// <summary>To be added.</summary>
		ActiveInActiveApp = 0x40,
		/// <summary>To be added.</summary>
		ActiveAlways = 0x80,
		/// <summary>To be added.</summary>
		AssumeInside = 0x100,
		/// <summary>To be added.</summary>
		InVisibleRect = 0x200,
		/// <summary>To be added.</summary>
		EnabledDuringMouseDrag = 0x400,
	}
	#endregion

	[NoMacCatalyst]
	[Native]
	public enum NSLineSweepDirection : ulong {
		/// <summary>To be added.</summary>
		NSLineSweepLeft,
		/// <summary>To be added.</summary>
		NSLineSweepRight,
		/// <summary>To be added.</summary>
		NSLineSweepDown,
		/// <summary>To be added.</summary>
		NSLineSweepUp,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSLineMovementDirection : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Left,
		/// <summary>To be added.</summary>
		Right,
		/// <summary>To be added.</summary>
		Down,
		/// <summary>To be added.</summary>
		Up,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTiffCompression : ulong {
		/// <summary>To be added.</summary>
		None = 1,
		/// <summary>To be added.</summary>
		CcittFax3 = 3,
		/// <summary>To be added.</summary>
		CcittFax4 = 4,
		/// <summary>To be added.</summary>
		Lzw = 5,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		Jpeg = 6,
		/// <summary>To be added.</summary>
		Next = 32766,
		/// <summary>To be added.</summary>
		PackBits = 32773,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		OldJpeg = 32865,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSBitmapImageFileType : ulong {
		/// <summary>To be added.</summary>
		Tiff,
		/// <summary>To be added.</summary>
		Bmp,
		/// <summary>To be added.</summary>
		Gif,
		/// <summary>To be added.</summary>
		Jpeg,
		/// <summary>To be added.</summary>
		Png,
		/// <summary>To be added.</summary>
		Jpeg2000,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageRepLoadStatus : long {
		/// <summary>To be added.</summary>
		UnknownType = -1,
		/// <summary>To be added.</summary>
		ReadingHeader = -2,
		/// <summary>To be added.</summary>
		WillNeedAllData = -3,
		/// <summary>To be added.</summary>
		InvalidData = -4,
		/// <summary>To be added.</summary>
		UnexpectedEOF = -5,
		/// <summary>To be added.</summary>
		Completed = -6,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSBitmapFormat : ulong {
		/// <summary>To be added.</summary>
		AlphaFirst = 1,
		/// <summary>To be added.</summary>
		AlphaNonpremultiplied = 2,
		/// <summary>To be added.</summary>
		FloatingPointSamples = 4,

		/// <summary>To be added.</summary>
		LittleEndian16Bit = 1 << 8,
		/// <summary>To be added.</summary>
		LittleEndian32Bit = 1 << 9,
		/// <summary>To be added.</summary>
		BigEndian16Bit = 1 << 10,
		/// <summary>To be added.</summary>
		BigEndian32Bit = 1 << 11,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPrintingOrientation : ulong {
		/// <summary>To be added.</summary>
		Portrait,
		/// <summary>To be added.</summary>
		Landscape,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPrintingPaginationMode : ulong {
		/// <summary>To be added.</summary>
		Auto,
		/// <summary>To be added.</summary>
		Fit,
		/// <summary>To be added.</summary>
		Clip,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPrinterTableStatus : ulong {
		/// <summary>To be added.</summary>
		Ok,
		/// <summary>To be added.</summary>
		NotFound,
		/// <summary>To be added.</summary>
		Error,
	}

	[NoMacCatalyst]
	[Native]
	[Deprecated (PlatformName.MacOSX, 10, 14)]
	public enum NSScrollArrowPosition : ulong {
		/// <summary>To be added.</summary>
		MaxEnd = 0,
		/// <summary>To be added.</summary>
		MinEnd = 1,
		/// <summary>To be added.</summary>
		DefaultSetting = 0,
		/// <summary>To be added.</summary>
		None = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSUsableScrollerParts : ulong {
		/// <summary>To be added.</summary>
		NoScroller,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 14)]
		OnlyArrows,
		/// <summary>To be added.</summary>
		All,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSScrollerPart : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		DecrementPage,
		/// <summary>To be added.</summary>
		Knob,
		/// <summary>To be added.</summary>
		IncrementPage,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 14)]
		DecrementLine,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 14)]
		IncrementLine,
		/// <summary>To be added.</summary>
		KnobSlot,
	}

	[NoMacCatalyst]
	[Native]
	[Deprecated (PlatformName.MacOSX, 10, 14)]
	public enum NSScrollerArrow : ulong {
		/// <summary>To be added.</summary>
		IncrementArrow,
		/// <summary>To be added.</summary>
		DecrementArrow,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPrintingPageOrder : long {
		/// <summary>To be added.</summary>
		Descending = -1,
		/// <summary>To be added.</summary>
		Special,
		/// <summary>To be added.</summary>
		Ascending,
		/// <summary>To be added.</summary>
		Unknown,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSPrintPanelOptions : long {
		/// <summary>To be added.</summary>
		ShowsCopies = 1,
		/// <summary>To be added.</summary>
		ShowsPageRange = 2,
		/// <summary>To be added.</summary>
		ShowsPaperSize = 4,
		/// <summary>To be added.</summary>
		ShowsOrientation = 8,
		/// <summary>To be added.</summary>
		ShowsScaling = 16,
		/// <summary>To be added.</summary>
		ShowsPrintSelection = 32,
		/// <summary>To be added.</summary>
		ShowsPageSetupAccessory = 256,
		/// <summary>To be added.</summary>
		ShowsPreview = 131072,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockValueType : ulong {
		/// <summary>To be added.</summary>
		Absolute,
		/// <summary>To be added.</summary>
		Percentage,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockDimension : ulong {
		/// <summary>To be added.</summary>
		Width = 0,
		/// <summary>To be added.</summary>
		MinimumWidth = 1,
		/// <summary>To be added.</summary>
		MaximumWidth = 2,
		/// <summary>To be added.</summary>
		Height = 4,
		/// <summary>To be added.</summary>
		MinimumHeight = 5,
		/// <summary>To be added.</summary>
		MaximumHeight = 6,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockLayer : long {
		/// <summary>To be added.</summary>
		Padding = -1,
		/// <summary>To be added.</summary>
		Border,
		/// <summary>To be added.</summary>
		Margin,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockVerticalAlignment : ulong {
		/// <summary>To be added.</summary>
		Top,
		/// <summary>To be added.</summary>
		Middle,
		/// <summary>To be added.</summary>
		Bottom,
		/// <summary>To be added.</summary>
		Baseline,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextTableLayoutAlgorithm : ulong {
		/// <summary>To be added.</summary>
		Automatic,
		/// <summary>To be added.</summary>
		Fixed,
	}

	[NoMacCatalyst]
	[Flags]
	public enum NSFontSymbolicTraits : int { // uint32_t NSFontSymbolicTraits
		/// <summary>To be added.</summary>
		ItalicTrait = (1 << 0),
		/// <summary>To be added.</summary>
		BoldTrait = (1 << 1),
		/// <summary>To be added.</summary>
		ExpandedTrait = (1 << 5),
		/// <summary>To be added.</summary>
		CondensedTrait = (1 << 6),
		/// <summary>To be added.</summary>
		MonoSpaceTrait = (1 << 10),
		/// <summary>To be added.</summary>
		VerticalTrait = (1 << 11),
		/// <summary>To be added.</summary>
		UIOptimizedTrait = (1 << 12),
		/// <summary>To be added.</summary>
		TraitTightLeading = 1 << 15,
		/// <summary>To be added.</summary>
		TraitLooseLeading = 1 << 16,
		TraitEmphasized = BoldTrait,
		/// <summary>To be added.</summary>
		UnknownClass = 0 << 28,
		/// <summary>To be added.</summary>
		OldStyleSerifsClass = 1 << 28,
		/// <summary>To be added.</summary>
		TransitionalSerifsClass = 2 << 28,
		/// <summary>To be added.</summary>
		ModernSerifsClass = 3 << 28,
		/// <summary>To be added.</summary>
		ClarendonSerifsClass = 4 << 28,
		/// <summary>To be added.</summary>
		SlabSerifsClass = 5 << 28,
		/// <summary>To be added.</summary>
		FreeformSerifsClass = 7 << 28,
		/// <summary>To be added.</summary>
		SansSerifClass = 8 << 28,
		/// <summary>To be added.</summary>
		OrnamentalsClass = 9 << 28,
		/// <summary>To be added.</summary>
		ScriptsClass = 10 << 28,
		/// <summary>To be added.</summary>
		SymbolicClass = 12 << 28,

		/// <summary>To be added.</summary>
		FamilyClassMask = (int) -268435456,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSFontTraitMask : ulong {
		/// <summary>To be added.</summary>
		Italic = 1,
		/// <summary>To be added.</summary>
		Bold = 2,
		/// <summary>To be added.</summary>
		Unbold = 4,
		/// <summary>To be added.</summary>
		NonStandardCharacterSet = 8,
		/// <summary>To be added.</summary>
		Narrow = 0x10,
		/// <summary>To be added.</summary>
		Expanded = 0x20,
		/// <summary>To be added.</summary>
		Condensed = 0x40,
		/// <summary>To be added.</summary>
		SmallCaps = 0x80,
		/// <summary>To be added.</summary>
		Poster = 0x100,
		/// <summary>To be added.</summary>
		Compressed = 0x200,
		/// <summary>To be added.</summary>
		FixedPitch = 0x400,
		/// <summary>To be added.</summary>
		Unitalic = 0x1000000,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSPasteboardWritingOptions : ulong {
		/// <summary>To be added.</summary>
		WritingPromised = 1 << 9,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSToolbarDisplayMode : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		IconAndLabel,
		/// <summary>To be added.</summary>
		Icon,
		/// <summary>To be added.</summary>
		Label,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSToolbarSizeMode : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		Regular,
		/// <summary>To be added.</summary>
		Small,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableViewColumnAutoresizingStyle : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Uniform,
		/// <summary>To be added.</summary>
		Sequential,
		/// <summary>To be added.</summary>
		ReverseSequential,
		/// <summary>To be added.</summary>
		LastColumnOnly,
		/// <summary>To be added.</summary>
		FirstColumnOnly,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableViewSelectionHighlightStyle : long {
		/// <summary>To be added.</summary>
		None = -1,
		/// <summary>To be added.</summary>
		Regular = 0,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Set 'NSTableView.Style' to 'NSTableViewStyle.SourceList' instead.")]
		SourceList = 1,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableViewDraggingDestinationFeedbackStyle : long {
		/// <summary>To be added.</summary>
		None = -1,
		/// <summary>To be added.</summary>
		Regular = 0,
		/// <summary>To be added.</summary>
		SourceList = 1,
		FeedbackStyleGap = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableViewDropOperation : ulong {
		/// <summary>To be added.</summary>
		On,
		/// <summary>To be added.</summary>
		Above,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTableColumnResizing : long {
		/// <summary>To be added.</summary>
		None = -1,
		/// <summary>To be added.</summary>
		Autoresizing = (1 << 0),
		/// <summary>To be added.</summary>
		UserResizingMask = (1 << 1),
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTableViewGridStyle : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		SolidVerticalLine = 1 << 0,
		/// <summary>To be added.</summary>
		SolidHorizontalLine = 1 << 1,
		/// <summary>To be added.</summary>
		DashedHorizontalGridLine = 1 << 3,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSGradientDrawingOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		BeforeStartingLocation = (1 << 0),
		/// <summary>To be added.</summary>
		AfterEndingLocation = (1 << 1),
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageAlignment : ulong {
		/// <summary>To be added.</summary>
		Center = 0,
		/// <summary>To be added.</summary>
		Top,
		/// <summary>To be added.</summary>
		TopLeft,
		/// <summary>To be added.</summary>
		TopRight,
		/// <summary>To be added.</summary>
		Left,
		/// <summary>To be added.</summary>
		Bottom,
		/// <summary>To be added.</summary>
		BottomLeft,
		/// <summary>To be added.</summary>
		BottomRight,
		/// <summary>To be added.</summary>
		Right,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageFrameStyle : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Photo,
		/// <summary>To be added.</summary>
		GrayBezel,
		/// <summary>To be added.</summary>
		Groove,
		/// <summary>To be added.</summary>
		Button,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSpeechBoundary : ulong {
		/// <summary>To be added.</summary>
		Immediate = 0,
		/// <summary>To be added.</summary>
		Word = 1,
		/// <summary>To be added.</summary>
		Sentence,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSplitViewDividerStyle : long {
		/// <summary>To be added.</summary>
		Thick = 1,
		/// <summary>To be added.</summary>
		Thin = 2,
		/// <summary>To be added.</summary>
		PaneSplitter = 3,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSplitViewItemBehavior : long {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		Sidebar,
		/// <summary>To be added.</summary>
		ContentList,
		[Mac (14, 0)]
		Inspector,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageScaling : ulong {
		/// <summary>To be added.</summary>
		ProportionallyDown = 0,
		/// <summary>To be added.</summary>
		AxesIndependently,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		ProportionallyUpOrDown,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSegmentStyle : long {
		/// <summary>To be added.</summary>
		Automatic = 0,
		/// <summary>To be added.</summary>
		Rounded = 1,
		/// <summary>To be added.</summary>
		TexturedRounded = 2,
		/// <summary>To be added.</summary>
		RoundRect = 3,
		/// <summary>To be added.</summary>
		TexturedSquare = 4,
		/// <summary>To be added.</summary>
		Capsule = 5,
		/// <summary>To be added.</summary>
		SmallSquare = 6,
		/// <summary>To be added.</summary>
		Separated = 8,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSegmentSwitchTracking : ulong {
		/// <summary>To be added.</summary>
		SelectOne = 0,
		/// <summary>To be added.</summary>
		SelectAny = 1,
		/// <summary>To be added.</summary>
		Momentary = 2,
		/// <summary>To be added.</summary>
		MomentaryAccelerator, // 10.10.3
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTickMarkPosition : ulong {
		/// <summary>To be added.</summary>
		Below,
		/// <summary>To be added.</summary>
		Above,
		/// <summary>To be added.</summary>
		Left,
		/// <summary>To be added.</summary>
		Right,
		/// <summary>To be added.</summary>
		Leading = Left,
		/// <summary>To be added.</summary>
		Trailing = Right,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSliderType : ulong {
		/// <summary>To be added.</summary>
		Linear = 0,
		/// <summary>To be added.</summary>
		Circular = 1,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTokenStyle : ulong {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		PlainText,
		/// <summary>To be added.</summary>
		Rounded,
		Squared = 3,
		PlainSquared = 4,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	[Deprecated (PlatformName.MacOSX, 11, 0)]
	public enum NSWorkspaceLaunchOptions : ulong {
		/// <summary>To be added.</summary>
		Print = 2,
		WithErrorPresentation = 0x40,
		/// <summary>To be added.</summary>
		InhibitingBackgroundOnly = 0x80,
		/// <summary>To be added.</summary>
		WithoutAddingToRecents = 0x100,
		/// <summary>To be added.</summary>
		WithoutActivation = 0x200,
		/// <summary>To be added.</summary>
		Async = 0x10000,
		/// <summary>To be added.</summary>
		AllowingClassicStartup = 0x20000,
		/// <summary>To be added.</summary>
		PreferringClassic = 0x40000,
		/// <summary>To be added.</summary>
		NewInstance = 0x80000,
		/// <summary>To be added.</summary>
		Hide = 0x100000,
		/// <summary>To be added.</summary>
		HideOthers = 0x200000,
		/// <summary>To be added.</summary>
		Default = Async | AllowingClassicStartup,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWorkspaceIconCreationOptions : ulong {
		/// <summary>To be added.</summary>
		NSExcludeQuickDrawElements = 1 << 1,
		/// <summary>To be added.</summary>
		NSExclude10_4Elements = 1 << 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPathStyle : long {
		/// <summary>To be added.</summary>
		Standard,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		NavigationBar,
		/// <summary>To be added.</summary>
		PopUp,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTabViewType : ulong {
		/// <summary>To be added.</summary>
		NSTopTabsBezelBorder,
		/// <summary>To be added.</summary>
		NSLeftTabsBezelBorder,
		/// <summary>To be added.</summary>
		NSBottomTabsBezelBorder,
		/// <summary>To be added.</summary>
		NSRightTabsBezelBorder,
		/// <summary>To be added.</summary>
		NSNoTabsBezelBorder,
		/// <summary>To be added.</summary>
		NSNoTabsLineBorder,
		/// <summary>To be added.</summary>
		NSNoTabsNoBorder,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTabState : ulong {
		/// <summary>To be added.</summary>
		Selected,
		/// <summary>To be added.</summary>
		Background,
		/// <summary>To be added.</summary>
		Pressed,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTabViewControllerTabStyle : long {
		/// <summary>To be added.</summary>
		SegmentedControlOnTop = 0,
		/// <summary>To be added.</summary>
		SegmentedControlOnBottom,
		/// <summary>To be added.</summary>
		Toolbar,
		/// <summary>To be added.</summary>
		Unspecified = -1,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSLevelIndicatorStyle : ulong {
		/// <summary>To be added.</summary>
		Relevancy,
		/// <summary>To be added.</summary>
		ContinuousCapacity,
		/// <summary>To be added.</summary>
		DiscreteCapacity,
		/// <summary>To be added.</summary>
		RatingLevel,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSFontCollectionOptions : long {
		/// <summary>To be added.</summary>
		ApplicationOnlyMask = 1,
	}

#if XAMCORE_5_0
	[NoMacCatalyst]
#else
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "This enum doesn't exist on this platform.")]
#if __MACCATALYST__
	[EditorBrowsable (EditorBrowsableState.Never)]
#endif
#endif
	[Native]
	public enum NSCollectionViewDropOperation : long {
		/// <summary>To be added.</summary>
		On = 0,
		/// <summary>To be added.</summary>
		Before = 1,
	}

#if XAMCORE_5_0
	[NoMacCatalyst]
#else
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "This enum doesn't exist on this platform.")]
#if __MACCATALYST__
	[EditorBrowsable (EditorBrowsableState.Never)]
#endif
#endif
	[Native]
	public enum NSCollectionViewItemHighlightState : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		ForSelection = 1,
		/// <summary>To be added.</summary>
		ForDeselection = 2,
		/// <summary>To be added.</summary>
		AsDropTarget = 3,
	}

#if XAMCORE_5_0
	[NoMacCatalyst]
#else
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "This enum doesn't exist on this platform.")]
#if __MACCATALYST__
	[EditorBrowsable (EditorBrowsableState.Never)]
#endif
#endif
	[Native]
	[Flags]
	public enum NSCollectionViewScrollPosition : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Top = 1 << 0,
		/// <summary>To be added.</summary>
		CenteredVertically = 1 << 1,
		/// <summary>To be added.</summary>
		Bottom = 1 << 2,
		/// <summary>To be added.</summary>
		NearestHorizontalEdge = 1 << 9,
		/// <summary>To be added.</summary>
		Left = 1 << 3,
		/// <summary>To be added.</summary>
		CenteredHorizontally = 1 << 4,
		/// <summary>To be added.</summary>
		Right = 1 << 5,
		/// <summary>To be added.</summary>
		LeadingEdge = 1 << 6,
		/// <summary>To be added.</summary>
		TrailingEdge = 1 << 7,
		/// <summary>To be added.</summary>
		NearestVerticalEdge = 1 << 8,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSCollectionElementCategory : long {
		/// <summary>To be added.</summary>
		Item,
		/// <summary>To be added.</summary>
		SupplementaryView,
		/// <summary>To be added.</summary>
		DecorationView,
		/// <summary>To be added.</summary>
		InterItemGap,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSCollectionUpdateAction : long {
		/// <summary>To be added.</summary>
		Insert,
		/// <summary>To be added.</summary>
		Delete,
		/// <summary>To be added.</summary>
		Reload,
		/// <summary>To be added.</summary>
		Move,
		/// <summary>To be added.</summary>
		None,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum NSCollectionViewScrollDirection : long {
		/// <summary>To be added.</summary>
		Vertical,
		/// <summary>To be added.</summary>
		Horizontal,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSDatePickerStyle : ulong {
		/// <summary>To be added.</summary>
		TextFieldAndStepper,
		/// <summary>To be added.</summary>
		ClockAndCalendar,
		/// <summary>To be added.</summary>
		TextField,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSDatePickerMode : ulong {
		/// <summary>To be added.</summary>
		Single,
		/// <summary>To be added.</summary>
		Range,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSDatePickerElementFlags : ulong {
		/// <summary>To be added.</summary>
		HourMinute = 0xc,
		/// <summary>To be added.</summary>
		HourMinuteSecond = 0xe,
		/// <summary>To be added.</summary>
		TimeZone = 0x10,

		/// <summary>To be added.</summary>
		YearMonthDate = 0xc0,
		/// <summary>To be added.</summary>
		YearMonthDateDay = 0xe0,
		/// <summary>To be added.</summary>
		Era = 0x100,
	}

	[NoMacCatalyst]
	[Native]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSOpenGLContextParameter : ulong {
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		SwapRectangle = 200,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		SwapRectangleEnable = 201,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		RasterizationEnable = 221,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		StateValidation = 301,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		SurfaceSurfaceVolatile = 306,

		/// <summary>To be added.</summary>
		SwapInterval = 222,
		/// <summary>To be added.</summary>
		SurfaceOrder = 235,
		/// <summary>To be added.</summary>
		SurfaceOpacity = 236,

		/// <summary>To be added.</summary>
		SurfaceBackingSize = 304,
		/// <summary>To be added.</summary>
		ReclaimResources = 308,
		/// <summary>To be added.</summary>
		CurrentRendererID = 309,
		/// <summary>To be added.</summary>
		GpuVertexProcessing = 310,
		/// <summary>To be added.</summary>
		GpuFragmentProcessing = 311,
		/// <summary>To be added.</summary>
		HasDrawable = 314,
		/// <summary>To be added.</summary>
		MpsSwapsInFlight = 315,
	}

	[NoMacCatalyst]
	public enum NSSurfaceOrder {
		/// <summary>To be added.</summary>
		AboveWindow = 1,
		/// <summary>To be added.</summary>
		BelowWindow = -1,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSOpenGLPixelFormatAttribute : uint { // uint32_t NSOpenGLPixelFormatAttribute
		/// <summary>To be added.</summary>
		AllRenderers = 1,
		/// <summary>To be added.</summary>
		DoubleBuffer = 5,
		/// <summary>To be added.</summary>
		TripleBuffer = 3,
		/// <summary>To be added.</summary>
		Stereo = 6,
		/// <summary>To be added.</summary>
		AuxBuffers = 7,
		/// <summary>To be added.</summary>
		ColorSize = 8,
		/// <summary>To be added.</summary>
		AlphaSize = 11,
		/// <summary>To be added.</summary>
		DepthSize = 12,
		/// <summary>To be added.</summary>
		StencilSize = 13,
		/// <summary>To be added.</summary>
		AccumSize = 14,
		/// <summary>To be added.</summary>
		MinimumPolicy = 51,
		/// <summary>To be added.</summary>
		MaximumPolicy = 52,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		OffScreen = 53,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 6)]
		FullScreen = 54,
		/// <summary>To be added.</summary>
		SampleBuffers = 55,
		/// <summary>To be added.</summary>
		Samples = 56,
		/// <summary>To be added.</summary>
		AuxDepthStencil = 57,
		/// <summary>To be added.</summary>
		ColorFloat = 58,
		/// <summary>To be added.</summary>
		Multisample = 59,
		/// <summary>To be added.</summary>
		Supersample = 60,
		/// <summary>To be added.</summary>
		SampleAlpha = 61,
		/// <summary>To be added.</summary>
		RendererID = 70,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 9)]
		SingleRenderer = 71,
		/// <summary>To be added.</summary>
		NoRecovery = 72,
		/// <summary>To be added.</summary>
		Accelerated = 73,
		/// <summary>To be added.</summary>
		ClosestPolicy = 74,
		/// <summary>To be added.</summary>
		BackingStore = 76,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 9)]
		Window = 80,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 9)]
		Compliant = 83,
		/// <summary>To be added.</summary>
		ScreenMask = 84,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		PixelBuffer = 90,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 7)]
		RemotePixelBuffer = 91,
		/// <summary>To be added.</summary>
		AllowOfflineRenderers = 96,
		/// <summary>To be added.</summary>
		AcceleratedCompute = 97,

		// Specify the profile
		/// <summary>To be added.</summary>
		OpenGLProfile = 99,
		/// <summary>To be added.</summary>
		VirtualScreenCount = 128,

		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 5)]
		Robust = 75,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 5)]
		MPSafe = 78,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 5)]
		MultiScreen = 81,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSOpenGLProfile : int {
		/// <summary>To be added.</summary>
		VersionLegacy = 0x1000, // Legacy
		/// <summary>To be added.</summary>
		Version3_2Core = 0x3200,  // 3.2 or better
		/// <summary>To be added.</summary>
		Version4_1Core = 0x4100,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSAlertButtonReturn : long {
		/// <summary>To be added.</summary>
		First = 1000,
		/// <summary>To be added.</summary>
		Second = 1001,
		/// <summary>To be added.</summary>
		Third = 1002,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSOpenGLGlobalOption : uint {
		/// <summary>To be added.</summary>
		FormatCacheSize = 501,
		/// <summary>To be added.</summary>
		ClearFormatCache = 502,
		/// <summary>To be added.</summary>
		RetainRenderers = 503,
		/// <summary>To be added.</summary>
		UseBuildCache = 506,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 4)]
		ResetLibrary = 504,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSGLTextureTarget : uint {
		/// <summary>To be added.</summary>
		T2D = 0x0de1,
		/// <summary>To be added.</summary>
		CubeMap = 0x8513,
		/// <summary>To be added.</summary>
		RectangleExt = 0x84F5,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSGLFormat : uint {
		/// <summary>To be added.</summary>
		RGB = 0x1907,
		/// <summary>To be added.</summary>
		RGBA = 0x1908,
		/// <summary>To be added.</summary>
		DepthComponent = 0x1902,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSGLTextureCubeMap : uint {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		PositiveX = 0x8515,
		/// <summary>To be added.</summary>
		PositiveY = 0x8517,
		/// <summary>To be added.</summary>
		PositiveZ = 0x8519,
		/// <summary>To be added.</summary>
		NegativeX = 0x8516,
		/// <summary>To be added.</summary>
		NegativeY = 0x8517,
		/// <summary>To be added.</summary>
		NegativeZ = 0x851A,
	}

	[NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' Framework instead.")]
	public enum NSGLColorBuffer : uint {
		/// <summary>To be added.</summary>
		Front = 0x0404,
		/// <summary>To be added.</summary>
		Back = 0x0405,
		/// <summary>To be added.</summary>
		Aux0 = 0x0409,
	}

	[NoMacCatalyst]
	[Native]
	[Deprecated (PlatformName.MacOSX, 10, 14)]
	public enum NSProgressIndicatorThickness : ulong {
		/// <summary>To be added.</summary>
		Small = 10,
		/// <summary>To be added.</summary>
		Regular = 14,
		/// <summary>To be added.</summary>
		Aqua = 12,
		/// <summary>To be added.</summary>
		Large = 18,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSProgressIndicatorStyle : ulong {
		/// <summary>To be added.</summary>
		Bar,
		/// <summary>To be added.</summary>
		Spinning,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPopUpArrowPosition : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Center,
		/// <summary>To be added.</summary>
		Bottom,
	}

	// FileType 4cc values to use with NSFileTypeForHFSTypeCode.
	[MacCatalyst (15, 0)]
	public enum HfsTypeCode : uint {
		/* Generic Finder icons */
		ClipboardIcon = 0x434C4950,   //'CLIP'
		ClippingUnknownTypeIcon = 0x636C7075,   //'clpu'
		ClippingPictureTypeIcon = 0x636C7070,   //'clpp'
		ClippingTextTypeIcon = 0x636C7074,   //'clpt'
		ClippingSoundTypeIcon = 0x636C7073,   //'clps'
		DesktopIcon = 0x6465736B,   //'desk'
		FinderIcon = 0x464E4452,   //'FNDR'
		ComputerIcon = 0x726F6F74,   //'root'
		FontSuitcaseIcon = 0x4646494C,   //'FFIL'
		FullTrashIcon = 0x66747268,   //'ftrh'
		GenericApplicationIcon = 0x4150504C,   //'APPL'
		GenericCdromIcon = 0x63646472,   //'cddr'
		GenericControlPanelIcon = 0x41505043,   //'APPC'
		GenericControlStripModuleIcon = 0x73646576,   //'sdev'
		GenericComponentIcon = 0x74686E67,   //'thng'
		GenericDeskAccessoryIcon = 0x41505044,   //'APPD'
		GenericDocumentIcon = 0x646F6375,   //'docu'
		GenericEditionFileIcon = 0x65647466,   //'edtf'
		GenericExtensionIcon = 0x494E4954,   //'INIT'
		GenericFileServerIcon = 0x73727672,   //'srvr'
		GenericFontIcon = 0x6666696C,   //'ffil'
		GenericFontScalerIcon = 0x73636C72,   //'sclr'
		GenericFloppyIcon = 0x666C7079,   //'flpy'
		GenericHardDiskIcon = 0x6864736B,   //'hdsk'
		GenericIDiskIcon = 0x6964736B,   //'idsk'
		GenericRemovableMediaIcon = 0x726D6F76,   //'rmov'
		GenericMoverObjectIcon = 0x6D6F7672,   //'movr'
		GenericPCCardIcon = 0x70636D63,   //'pcmc'
		GenericPreferencesIcon = 0x70726566,   //'pref'
		GenericQueryDocumentIcon = 0x71657279,   //'qery'
		GenericRamDiskIcon = 0x72616D64,   //'ramd'
		GenericSharedLibraryIcon = 0x73686C62,   //'shlb'
		GenericStationeryIcon = 0x73646F63,   //'sdoc'
		GenericSuitcaseIcon = 0x73756974,   //'suit'
		GenericUrlIcon = 0x6775726C,   //'gurl'
		GenericWormIcon = 0x776F726D,   //'worm'
		InternationalResourcesIcon = 0x6966696C,   //'ifil'
		KeyboardLayoutIcon = 0x6B66696C,   //'kfil'
		SoundFileIcon = 0x7366696C,   //'sfil'
		SystemSuitcaseIcon = 0x7A737973,   //'zsys'
		TrashIcon = 0x74727368,   //'trsh'
		TrueTypeFontIcon = 0x7466696C,   //'tfil'
		TrueTypeFlatFontIcon = 0x73666E74,   //'sfnt'
		TrueTypeMultiFlatFontIcon = 0x74746366,   //'ttcf'
		UserIDiskIcon = 0x7564736B,   //'udsk'
		UnknownFSObjectIcon = 0x756E6673,   //'unfs'

		/* Internet locations */
		InternetLocationHttpIcon = 0x696C6874,   //'ilht'
		InternetLocationFtpIcon = 0x696C6674,   //'ilft'
		InternetLocationAppleShareIcon = 0x696C6166,   //'ilaf'
		InternetLocationAppleTalkZoneIcon = 0x696C6174,   //'ilat'
		InternetLocationFileIcon = 0x696C6669,   //'ilfi'
		InternetLocationMailIcon = 0x696C6D61,   //'ilma'
		InternetLocationNewsIcon = 0x696C6E77,   //'ilnw'
		InternetLocationNslNeighborhoodIcon = 0x696C6E73,   //'ilns'
		InternetLocationGenericIcon = 0x696C6765,   //'ilge'

		/* Folders */
		GenericFolderIcon = 0x666C6472,   //'fldr'
		DropFolderIcon = 0x64626F78,   //'dbox'
		MountedFolderIcon = 0x6D6E7464,   //'mntd'
		OpenFolderIcon = 0x6F666C64,   //'ofld'
		OwnedFolderIcon = 0x6F776E64,   //'ownd'
		PrivateFolderIcon = 0x70727666,   //'prvf'
		SharedFolderIcon = 0x7368666C,   //'shfl'

		/* Sharingprivileges icons */
		SharingPrivsNotApplicableIcon = 0x73686E61,   //'shna'
		SharingPrivsReadOnlyIcon = 0x7368726F,   //'shro'
		SharingPrivsReadWriteIcon = 0x73687277,   //'shrw'
		SharingPrivsUnknownIcon = 0x7368756B,   //'shuk'
		SharingPrivsWritableIcon = 0x77726974,   //'writ'

		/* Users and Groups icons */
		UserFolderIcon = 0x75666C64,   //'ufld'
		WorkgroupFolderIcon = 0x77666C64,   //'wfld'
		GuestUserIcon = 0x67757372,   //'gusr'
		UserIcon = 0x75736572,   //'user'
		OwnerIcon = 0x73757372,   //'susr'
		GroupIcon = 0x67727570,   //'grup'

		/* Special folders */
		AppearanceFolderIcon = 0x61707072,   //'appr'
		AppleMenuFolderIcon = 0x616D6E75,   //'amnu'
		ApplicationsFolderIcon = 0x61707073,   //'apps'
		ApplicationSupportFolderIcon = 0x61737570,   //'asup'
		ColorSyncFolderIcon = 0x70726F66,   //'prof'
		ContextualMenuItemsFolderIcon = 0x636D6E75,   //'cmnu'
		ControlPanelDisabledFolderIcon = 0x63747244,   //'ctrD'
		ControlPanelFolderIcon = 0x6374726C,   //'ctrl'
		DocumentsFolderIcon = 0x646F6373,   //'docs'
		ExtensionsDisabledFolderIcon = 0x65787444,   //'extD'
		ExtensionsFolderIcon = 0x6578746E,   //'extn'
		FavoritesFolderIcon = 0x66617673,   //'favs'
		FontsFolderIcon = 0x666F6E74,   //'font'
		InternetSearchSitesFolderIcon = 0x69737366,   //'issf'
		PublicFolderIcon = 0x70756266,   //'pubf'
		PrinterDescriptionFolderIcon = 0x70706466,   //'ppdf'
		PrintMonitorFolderIcon = 0x70726E74,   //'prnt'
		RecentApplicationsFolderIcon = 0x72617070,   //'rapp'
		RecentDocumentsFolderIcon = 0x72646F63,   //'rdoc'
		RecentServersFolderIcon = 0x72737276,   //'rsrv'
		ShutdownItemsDisabledFolderIcon = 0x73686444,   //'shdD'
		ShutdownItemsFolderIcon = 0x73686466,   //'shdf'
		SpeakableItemsFolder = 0x73706B69,   //'spki'
		StartupItemsDisabledFolderIcon = 0x73747244,   //'strD'
		StartupItemsFolderIcon = 0x73747274,   //'strt'
		SystemExtensionDisabledFolderIcon = 0x6D616344,   //'macD'
		SystemFolderIcon = 0x6D616373,   //'macs'
		VoicesFolderIcon = 0x66766F63,   //'fvoc'

		/* Badges */
		AppleScriptBadgeIcon = 0x73637270,   //'scrp'
		LockedBadgeIcon = 0x6C626467,   //'lbdg'
		MountedBadgeIcon = 0x6D626467,   //'mbdg'
		SharedBadgeIcon = 0x73626467,   //'sbdg'
		AliasBadgeIcon = 0x61626467,   //'abdg'
		AlertCautionBadgeIcon = 0x63626467,   //'cbdg'

		/* Alert icons */
		AlertNoteIcon = 0x6E6F7465,   //'note'
		AlertCautionIcon = 0x63617574,   //'caut'
		AlertStopIcon = 0x73746F70,   //'stop'

		/* Networking icons */
		AppleTalkIcon = 0x61746C6B,   //'atlk'
		AppleTalkZoneIcon = 0x61747A6E,   //'atzn'
		AfpServerIcon = 0x61667073,   //'afps'
		FtpServerIcon = 0x66747073,   //'ftps'
		HttpServerIcon = 0x68747073,   //'htps'
		GenericNetworkIcon = 0x676E6574,   //'gnet'
		IPFileServerIcon = 0x69737276,   //'isrv'

		/* Toolbar icons */
		ToolbarCustomizeIcon = 0x74637573,   //'tcus'
		ToolbarDeleteIcon = 0x7464656C,   //'tdel'
		ToolbarFavoritesIcon = 0x74666176,   //'tfav'
		ToolbarHomeIcon = 0x74686F6D,   //'thom'
		ToolbarAdvancedIcon = 0x74626176,   //'tbav'
		ToolbarInfoIcon = 0x7462696E,   //'tbin'
		ToolbarLabelsIcon = 0x74626C62,   //'tblb'
		ToolbarApplicationsFolderIcon = 0x74417073,   //'tAps'
		ToolbarDocumentsFolderIcon = 0x74446F63,   //'tDoc'
		ToolbarMovieFolderIcon = 0x744D6F76,   //'tMov'
		ToolbarMusicFolderIcon = 0x744D7573,   //'tMus'
		ToolbarPicturesFolderIcon = 0x74506963,   //'tPic'
		ToolbarPublicFolderIcon = 0x74507562,   //'tPub'
		ToolbarDesktopFolderIcon = 0x7444736B,   //'tDsk'
		ToolbarDownloadsFolderIcon = 0x7444776E,   //'tDwn'
		ToolbarLibraryFolderIcon = 0x744C6962,   //'tLib'
		ToolbarUtilitiesFolderIcon = 0x7455746C,   //'tUtl'
		ToolbarSitesFolderIcon = 0x74537473,   //'tSts'

		/* Other icons */
		AppleLogoIcon = 0x6361706C,   //'capl'
		AppleMenuIcon = 0x7361706C,   //'sapl'
		BackwardArrowIcon = 0x6261726F,   //'baro'
		FavoriteItemsIcon = 0x66617672,   //'favr'
		ForwardArrowIcon = 0x6661726F,   //'faro'
		GridIcon = 0x67726964,   //'grid'
		HelpIcon = 0x68656C70,   //'help'
		KeepArrangedIcon = 0x61726E67,   //'arng'
		LockedIcon = 0x6C6F636B,   //'lock'
		NoFilesIcon = 0x6E66696C,   //'nfil'
		NoFolderIcon = 0x6E666C64,   //'nfld'
		NoWriteIcon = 0x6E777274,   //'nwrt'
		ProtectedApplicationFolderIcon = 0x70617070,   //'papp'
		ProtectedSystemFolderIcon = 0x70737973,   //'psys'
		RecentItemsIcon = 0x72636E74,   //'rcnt'
		ShortcutIcon = 0x73687274,   //'shrt'
		SortAscendingIcon = 0x61736E64,   //'asnd'
		SortDescendingIcon = 0x64736E64,   //'dsnd'
		UnlockedIcon = 0x756C636B,   //'ulck'
		ConnectToIcon = 0x636E6374,   //'cnct'
		GenericWindowIcon = 0x6777696E,   //'gwin'
		QuestionMarkIcon = 0x71756573,   //'ques'
		DeleteAliasIcon = 0x64616C69,   //'dali'
		EjectMediaIcon = 0x656A6563,   //'ejec'
		BurningIcon = 0x6275726E,   //'burn'
		RightContainerArrowIcon = 0x72636172,   //'rcar'
	}

	// These constants specify the possible states of a drawer.
	[NoMacCatalyst]
	[Native]
	[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'NSSplitViewController' instead.")]
	public enum NSDrawerState : ulong {
		/// <summary>To be added.</summary>
		Closed = 0,
		/// <summary>To be added.</summary>
		Opening = 1,
		/// <summary>To be added.</summary>
		Open = 2,
		/// <summary>To be added.</summary>
		Closing = 3,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowLevel : long {
		/// <summary>To be added.</summary>
		Normal = 0,
		/// <summary>To be added.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 13)]
		Dock = 20,
		/// <summary>To be added.</summary>
		Floating = 3,
		/// <summary>To be added.</summary>
		MainMenu = 24,
		/// <summary>To be added.</summary>
		ModalPanel = 8,
		/// <summary>To be added.</summary>
		PopUpMenu = 101,
		/// <summary>To be added.</summary>
		ScreenSaver = 1000,
		/// <summary>To be added.</summary>
		Status = 25,
		/// <summary>To be added.</summary>
		Submenu = 3,
		/// <summary>To be added.</summary>
		TornOffMenu = 3,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSRuleEditorRowType : ulong {
		/// <summary>To be added.</summary>
		Simple = 0,
		/// <summary>To be added.</summary>
		Compound,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSRuleEditorNestingMode : ulong {
		/// <summary>To be added.</summary>
		Single,
		/// <summary>To be added.</summary>
		List,
		/// <summary>To be added.</summary>
		Compound,
		/// <summary>To be added.</summary>
		Simple,
	}

	[NoMacCatalyst]
	[Native]
	[Deprecated (PlatformName.MacOSX, 10, 11, message: "Use 'NSGlyphProperty' instead.")]
	public enum NSGlyphInscription : ulong {
		/// <summary>To be added.</summary>
		Base,
		/// <summary>To be added.</summary>
		Below,
		/// <summary>To be added.</summary>
		Above,
		/// <summary>To be added.</summary>
		Overstrike,
		/// <summary>To be added.</summary>
		OverBelow,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTypesetterBehavior : long {
		/// <summary>To be added.</summary>
		Latest = -1,
		/// <summary>To be added.</summary>
		Original = 0,
		/// <summary>To be added.</summary>
		Specific_10_2_WithCompatibility = 1,
		/// <summary>To be added.</summary>
		Specific_10_2 = 2,
		/// <summary>To be added.</summary>
		Specific_10_3 = 3,
		/// <summary>To be added.</summary>
		Specific_10_4 = 4,

	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSRemoteNotificationType : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Badge = 1 << 0,
		/// <summary>To be added.</summary>
		Sound = 1 << 1,
		/// <summary>To be added.</summary>
		Alert = 1 << 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSScrollViewFindBarPosition : long {
		/// <summary>To be added.</summary>
		AboveHorizontalRuler = 0,
		/// <summary>To be added.</summary>
		AboveContent,
		/// <summary>To be added.</summary>
		BelowContent,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSScrollerStyle : long {
		/// <summary>To be added.</summary>
		Legacy = 0,
		/// <summary>To be added.</summary>
		Overlay,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSScrollElasticity : long {
		/// <summary>To be added.</summary>
		Automatic = 0,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Allowed,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSScrollerKnobStyle : long {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Dark = 1,
		/// <summary>To be added.</summary>
		Light = 2,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSEventPhase : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Began = 1,
		/// <summary>To be added.</summary>
		Stationary = 2,
		/// <summary>To be added.</summary>
		Changed = 4,
		/// <summary>To be added.</summary>
		Ended = 8,
		/// <summary>To be added.</summary>
		Cancelled = 16,
		/// <summary>To be added.</summary>
		MayBegin = 32,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSEventSwipeTrackingOptions : ulong {
		/// <summary>To be added.</summary>
		LockDirection = 1,
		/// <summary>To be added.</summary>
		ClampGestureAmount = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSEventGestureAxis : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Horizontal,
		/// <summary>To be added.</summary>
		Vertical,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSLayoutConstraintOrientation : long {
		/// <summary>To be added.</summary>
		Horizontal,
		/// <summary>To be added.</summary>
		Vertical,
	}

	[NoMacCatalyst]
	public enum NSLayoutPriority : int /*float*/ {
		/// <summary>To be added.</summary>
		Required = 1000,
		/// <summary>To be added.</summary>
		DefaultHigh = 750,
		/// <summary>To be added.</summary>
		DragThatCanResizeWindow = 510,
		/// <summary>To be added.</summary>
		WindowSizeStayPut = 500,
		/// <summary>To be added.</summary>
		DragThatCannotResizeWindow = 490,
		/// <summary>To be added.</summary>
		DefaultLow = 250,
		/// <summary>To be added.</summary>
		FittingSizeCompression = 50,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPopoverAppearance : long {
		/// <summary>To be added.</summary>
		Minimal,
		/// <summary>To be added.</summary>
		HUD,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPopoverBehavior : long {
		/// <summary>To be added.</summary>
		ApplicationDefined,
		/// <summary>To be added.</summary>
		Transient,
		/// <summary>To be added.</summary>
		Semitransient,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableViewRowSizeStyle : long {
		/// <summary>To be added.</summary>
		Default = -1,
		/// <summary>To be added.</summary>
		Custom = 0,
		/// <summary>To be added.</summary>
		Small,
		/// <summary>To be added.</summary>
		Medium,
		/// <summary>To be added.</summary>
		Large,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableRowActionEdge : long {
		/// <summary>To be added.</summary>
		Leading,
		/// <summary>To be added.</summary>
		Trailing,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableViewRowActionStyle : long {
		/// <summary>To be added.</summary>
		Regular,
		/// <summary>To be added.</summary>
		Destructive,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTableViewAnimation : ulong {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Fade = 1,
		/// <summary>To be added.</summary>
		Gap = 2,
		/// <summary>To be added.</summary>
		SlideUp = 0x10,
		/// <summary>To be added.</summary>
		SlideDown = 0x20,
		/// <summary>To be added.</summary>
		SlideLeft = 0x30,
		/// <summary>To be added.</summary>
		SlideRight = 0x40,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSDraggingItemEnumerationOptions : ulong {
		/// <summary>To be added.</summary>
		Concurrent = 1 << 0,
		/// <summary>To be added.</summary>
		ClearNonenumeratedImages = 1 << 16,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSDraggingFormation : long {
		/// <summary>To be added.</summary>
		Default,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Pile,
		/// <summary>To be added.</summary>
		List,
		/// <summary>To be added.</summary>
		Stack,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSDraggingContext : long {
		/// <summary>To be added.</summary>
		OutsideApplication,
		/// <summary>To be added.</summary>
		WithinApplication,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowAnimationBehavior : long {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		None = 2,
		/// <summary>To be added.</summary>
		DocumentWindow,
		/// <summary>To be added.</summary>
		UtilityWindow,
		/// <summary>To be added.</summary>
		AlertPanel,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTextFinderAction : long {
		/// <summary>To be added.</summary>
		ShowFindInterface = 1,
		/// <summary>To be added.</summary>
		NextMatch = 2,
		/// <summary>To be added.</summary>
		PreviousMatch = 3,
		/// <summary>To be added.</summary>
		ReplaceAll = 4,
		/// <summary>To be added.</summary>
		Replace = 5,
		/// <summary>To be added.</summary>
		ReplaceAndFind = 6,
		/// <summary>To be added.</summary>
		SetSearchString = 7,
		/// <summary>To be added.</summary>
		ReplaceAllInSelection = 8,
		/// <summary>To be added.</summary>
		SelectAll = 9,
		/// <summary>To be added.</summary>
		SelectAllInSelection = 10,
		/// <summary>To be added.</summary>
		HideFindInterface = 11,
		/// <summary>To be added.</summary>
		ShowReplaceInterface = 12,
		/// <summary>To be added.</summary>
		HideReplaceInterface = 13,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSFontPanelMode : ulong {
		/// <summary>To be added.</summary>
		FaceMask = 1 << 0,
		/// <summary>To be added.</summary>
		SizeMask = 1 << 1,
		/// <summary>To be added.</summary>
		CollectionMask = 1 << 2,
		/// <summary>To be added.</summary>
		UnderlineEffectMask = 1 << 8,
		/// <summary>To be added.</summary>
		StrikethroughEffectMask = 1 << 9,
		/// <summary>To be added.</summary>
		TextColorEffectMask = 1 << 10,
		/// <summary>To be added.</summary>
		DocumentColorEffectMask = 1 << 11,
		/// <summary>To be added.</summary>
		ShadowEffectMask = 1 << 12,
		/// <summary>To be added.</summary>
		AllEffectsMask = 0XFFF00,
		/// <summary>To be added.</summary>
		StandardMask = 0xFFFF,
		/// <summary>To be added.</summary>
		AllModesMask = unchecked((ulong) UInt32.MaxValue),
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSFontCollectionVisibility : ulong {
		/// <summary>To be added.</summary>
		Process = 1 << 0,
		/// <summary>To be added.</summary>
		User = 1 << 1,
		/// <summary>To be added.</summary>
		Computer = 1 << 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSharingContentScope : long {
		/// <summary>To be added.</summary>
		Item,
		/// <summary>To be added.</summary>
		Partial,
		/// <summary>To be added.</summary>
		Full,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTypesetterControlCharacterAction : ulong {
		/// <summary>To be added.</summary>
		ZeroAdvancement = 1 << 0,
		/// <summary>To be added.</summary>
		Whitespace = 1 << 1,
		/// <summary>To be added.</summary>
		HorizontalTab = 1 << 2,
		/// <summary>To be added.</summary>
		LineBreak = 1 << 3,
		/// <summary>To be added.</summary>
		ParagraphBreak = 1 << 4,
		/// <summary>To be added.</summary>
		ContainerBreak = 1 << 5,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPageControllerTransitionStyle : long {
		/// <summary>To be added.</summary>
		StackHistory,
		/// <summary>To be added.</summary>
		StackBook,
		/// <summary>To be added.</summary>
		HorizontalStrip,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowTitleVisibility : long {
		/// <summary>To be added.</summary>
		Visible = 0,
		/// <summary>To be added.</summary>
		Hidden = 1,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSViewControllerTransitionOptions : ulong {
		/// <summary>To be added.</summary>
		None = 0x0,
		/// <summary>To be added.</summary>
		Crossfade = 0x1,
		/// <summary>To be added.</summary>
		SlideUp = 0x10,
		/// <summary>To be added.</summary>
		SlideDown = 0x20,
		/// <summary>To be added.</summary>
		SlideLeft = 0x40,
		/// <summary>To be added.</summary>
		SlideRight = 0x80,
		/// <summary>To be added.</summary>
		SlideForward = 0x140,
		/// <summary>To be added.</summary>
		SlideBackward = 0x180,
		/// <summary>To be added.</summary>
		AllowUserInteraction = 0x1000,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSApplicationOcclusionState : ulong {
		/// <summary>To be added.</summary>
		Visible = 1 << 1,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowOcclusionState : ulong {
		/// <summary>To be added.</summary>
		Visible = 1 << 1,
	}



	#region NSVisualEffectView
	[NoMacCatalyst]
	[Native]
	public enum NSVisualEffectMaterial : long {
		/// <summary>To be added.</summary>
		[Advice ("Use a specific material instead.")]
		AppearanceBased,
		/// <summary>To be added.</summary>
		[Advice ("Use a semantic material instead.")]
		Light,
		/// <summary>To be added.</summary>
		[Advice ("Use a semantic material instead.")]
		Dark,
		/// <summary>To be added.</summary>
		Titlebar,
		/// <summary>To be added.</summary>
		Selection,
		/// <summary>To be added.</summary>
		Menu,
		/// <summary>To be added.</summary>
		Popover,
		/// <summary>To be added.</summary>
		Sidebar,
		/// <summary>To be added.</summary>
		[Advice ("Use a semantic material instead.")]
		MediumLight,
		/// <summary>To be added.</summary>
		[Advice ("Use a semantic material instead.")]
		UltraDark,
		/// <summary>To be added.</summary>
		HeaderView = 10,
		/// <summary>To be added.</summary>
		Sheet = 11,
		/// <summary>To be added.</summary>
		WindowBackground = 12,
		/// <summary>To be added.</summary>
		HudWindow = 13,
		/// <summary>To be added.</summary>
		FullScreenUI = 15,
		/// <summary>To be added.</summary>
		ToolTip = 17,
		/// <summary>To be added.</summary>
		ContentBackground = 18,
		/// <summary>To be added.</summary>
		UnderWindowBackground = 21,
		/// <summary>To be added.</summary>
		UnderPageBackground = 22,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSVisualEffectBlendingMode : long {
		/// <summary>To be added.</summary>
		BehindWindow,
		/// <summary>To be added.</summary>
		WithinWindow,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSVisualEffectState : long {
		/// <summary>To be added.</summary>
		FollowsWindowActiveState,
		/// <summary>To be added.</summary>
		Active,
		/// <summary>To be added.</summary>
		Inactive,
	}
	#endregion

	[NoMacCatalyst]
	[Native]
	public enum NSPressureBehavior : long {
		/// <summary>To be added.</summary>
		Unknown = -1,
		/// <summary>To be added.</summary>
		PrimaryDefault = 0,
		/// <summary>To be added.</summary>
		PrimaryClick = 1,
		/// <summary>To be added.</summary>
		PrimaryGeneric = 2,
		/// <summary>To be added.</summary>
		PrimaryAccelerator = 3,
		/// <summary>To be added.</summary>
		PrimaryDeepClick = 5,
		/// <summary>To be added.</summary>
		PrimaryDeepDrag = 6,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSHapticFeedbackPattern : long {
		/// <summary>To be added.</summary>
		Generic = 0,
		/// <summary>To be added.</summary>
		Alignment,
		/// <summary>To be added.</summary>
		LevelChange,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSHapticFeedbackPerformanceTime : ulong {
		/// <summary>To be added.</summary>
		Default = 0,
		/// <summary>To be added.</summary>
		Now,
		/// <summary>To be added.</summary>
		DrawCompleted,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSpringLoadingHighlight : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Standard,
		/// <summary>To be added.</summary>
		Emphasized,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSSpringLoadingOptions : ulong {
		/// <summary>To be added.</summary>
		Disabled = 0,
		/// <summary>To be added.</summary>
		Enabled = 1 << 0,
		/// <summary>To be added.</summary>
		ContinuousActivation = 1 << 1,
		/// <summary>To be added.</summary>
		NoHover = 1 << 3,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowListOptions : long {
		/// <summary>To be added.</summary>
		OrderedFrontToBack = (1 << 0),
	}

	[NoMacCatalyst]
	[Native]
	public enum NSStatusItemBehavior : ulong {
		/// <summary>To be added.</summary>
		RemovalAllowed = (1 << 1),
		/// <summary>To be added.</summary>
		TerminationOnRemoval = (1 << 2),
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowTabbingMode : long {
		/// <summary>To be added.</summary>
		Automatic,
		/// <summary>To be added.</summary>
		Preferred,
		/// <summary>To be added.</summary>
		Disallowed,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowUserTabbingPreference : long {
		/// <summary>To be added.</summary>
		Manual,
		/// <summary>To be added.</summary>
		Always,
		/// <summary>To be added.</summary>
		InFullScreen,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSGridCellPlacement : long {
		/// <summary>To be added.</summary>
		Inherited = 0,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Leading,
		/// <summary>To be added.</summary>
		Top = Leading,
		/// <summary>To be added.</summary>
		Trailing,
		/// <summary>To be added.</summary>
		Bottom = Trailing,
		/// <summary>To be added.</summary>
		Center,
		/// <summary>To be added.</summary>
		Fill,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSGridRowAlignment : long {
		/// <summary>To be added.</summary>
		Inherited = 0,
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		FirstBaseline,
		/// <summary>To be added.</summary>
		LastBaseline,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageLayoutDirection : long {
		/// <summary>To be added.</summary>
		Unspecified = -1,
		/// <summary>To be added.</summary>
		LeftToRight = 2,
		/// <summary>To be added.</summary>
		RightToLeft = 3,
	}

	[NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSCloudKitSharingServiceOptions : ulong {
		/// <summary>To be added.</summary>
		Standard = 0,
		/// <summary>To be added.</summary>
		AllowPublic = 1 << 0,
		/// <summary>To be added.</summary>
		AllowPrivate = 1 << 1,
		/// <summary>To be added.</summary>
		AllowReadOnly = 1 << 4,
		/// <summary>To be added.</summary>
		AllowReadWrite = 1 << 5,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSDisplayGamut : long {
		/// <summary>To be added.</summary>
		Srgb = 1,
		/// <summary>To be added.</summary>
		P3,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTabPosition : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Top,
		/// <summary>To be added.</summary>
		Left,
		/// <summary>To be added.</summary>
		Bottom,
		/// <summary>To be added.</summary>
		Right,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTabViewBorderType : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Line,
		/// <summary>To be added.</summary>
		Bezel,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPasteboardContentsOptions : ulong {
		/// <summary>To be added.</summary>
		CurrentHostOnly = 1,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTouchType : long {
		/// <summary>To be added.</summary>
		Direct,
		/// <summary>To be added.</summary>
		Indirect,
	}

	[NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSTouchTypeMask : ulong {
		/// <summary>To be added.</summary>
		Direct = (1 << (int) NSTouchType.Direct),
		/// <summary>To be added.</summary>
		Indirect = (1 << (int) NSTouchType.Indirect),
	}

	[NoMacCatalyst]
	[Native]
	public enum NSScrubberMode : long {
		/// <summary>To be added.</summary>
		Fixed = 0,
		/// <summary>To be added.</summary>
		Free,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSScrubberAlignment : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Leading,
		/// <summary>To be added.</summary>
		Trailing,
		/// <summary>To be added.</summary>
		Center,
	}

	[NoMacCatalyst]
	public enum NSFontError : int {
		/// <summary>To be added.</summary>
		AssetDownloadError = 66304,
		/// <summary>To be added.</summary>
		ErrorMinimum = 66304,
		/// <summary>To be added.</summary>
		ErrorMaximum = 66335,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSAccessibilityAnnotationPosition : long {
		/// <summary>To be added.</summary>
		FullRange,
		/// <summary>To be added.</summary>
		Start,
		/// <summary>To be added.</summary>
		End,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSAccessibilityCustomRotorSearchDirection : long {
		/// <summary>To be added.</summary>
		Previous,
		/// <summary>To be added.</summary>
		Next,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSAccessibilityCustomRotorType : long {
		/// <summary>To be added.</summary>
		Custom = 0,
		/// <summary>To be added.</summary>
		Any = 1,
		/// <summary>To be added.</summary>
		Annotation,
		/// <summary>To be added.</summary>
		BoldText,
		/// <summary>To be added.</summary>
		Heading,
		/// <summary>To be added.</summary>
		HeadingLevel1,
		/// <summary>To be added.</summary>
		HeadingLevel2,
		/// <summary>To be added.</summary>
		HeadingLevel3,
		/// <summary>To be added.</summary>
		HeadingLevel4,
		/// <summary>To be added.</summary>
		HeadingLevel5,
		/// <summary>To be added.</summary>
		HeadingLevel6,
		/// <summary>To be added.</summary>
		Image,
		/// <summary>To be added.</summary>
		ItalicText,
		/// <summary>To be added.</summary>
		Landmark,
		/// <summary>To be added.</summary>
		Link,
		/// <summary>To be added.</summary>
		List,
		/// <summary>To be added.</summary>
		MisspelledWord,
		/// <summary>To be added.</summary>
		Table,
		/// <summary>To be added.</summary>
		TextField,
		/// <summary>To be added.</summary>
		UnderlinedText,
		/// <summary>To be added.</summary>
		VisitedLink,
		Audiograph,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSColorType : long {
		/// <summary>To be added.</summary>
		ComponentBased,
		/// <summary>To be added.</summary>
		Pattern,
		/// <summary>To be added.</summary>
		Catalog,
	}

	[NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSFontAssetRequestOptions : ulong {
		/// <summary>To be added.</summary>
		UsesStandardUI = 1 << 0,
	}

	[NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSFontPanelModeMask : ulong {
		/// <summary>To be added.</summary>
		Face = 1 << 0,
		/// <summary>To be added.</summary>
		Size = 1 << 1,
		/// <summary>To be added.</summary>
		Collection = 1 << 2,
		/// <summary>To be added.</summary>
		UnderlineEffect = 1 << 8,
		/// <summary>To be added.</summary>
		StrikethroughEffect = 1 << 9,
		/// <summary>To be added.</summary>
		TextColorEffect = 1 << 10,
		/// <summary>To be added.</summary>
		DocumentColorEffect = 1 << 11,
		/// <summary>To be added.</summary>
		ShadowEffect = 1 << 12,
		/// <summary>To be added.</summary>
		AllEffects = (ulong) 0XFFF00,
		/// <summary>To be added.</summary>
		StandardModes = (ulong) 0XFFFF,
		/// <summary>To be added.</summary>
		AllModes = (ulong) 0XFFFFFFFF,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSLevelIndicatorPlaceholderVisibility : long {
		/// <summary>To be added.</summary>
		Automatic = 0,
		/// <summary>To be added.</summary>
		Always = 1,
		/// <summary>To be added.</summary>
		WhileEditing = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSSegmentDistribution : long {
		/// <summary>To be added.</summary>
		Fit = 0,
		/// <summary>To be added.</summary>
		Fill,
		/// <summary>To be added.</summary>
		FillEqually,
		/// <summary>To be added.</summary>
		FillProportionally,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSColorSystemEffect : long {
		/// <summary>To be added.</summary>
		None,
		/// <summary>To be added.</summary>
		Pressed,
		/// <summary>To be added.</summary>
		DeepPressed,
		/// <summary>To be added.</summary>
		Disabled,
		/// <summary>To be added.</summary>
		Rollover,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWorkspaceAuthorizationType : long {
		/// <summary>To be added.</summary>
		CreateSymbolicLink,
		/// <summary>To be added.</summary>
		SetAttributes,
		/// <summary>To be added.</summary>
		ReplaceFile,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTableViewStyle : long {
		Automatic,
		FullWidth,
		Inset,
		SourceList,
		Plain,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSTitlebarSeparatorStyle : long {
		Automatic,
		None,
		Line,
		Shadow,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSWindowToolbarStyle : long {
		Automatic,
		Expanded,
		Preference,
		Unified,
		UnifiedCompact,
	}

	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTableViewAnimationOptions : ulong {
		EffectNone = 0x0,
		EffectFade = 0x1,
		EffectGap = 0x2,
		SlideUp = 0x10,
		SlideDown = 0x20,
		SlideLeft = 0x30,
		SlideRight = 0x40,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageSymbolScale : long {
		Small = 1,
		Medium = 2,
		Large = 3,
	}

	[NoMacCatalyst, Mac (13, 0)]
	[Native]
	public enum NSColorWellStyle : long {
		Default = 0,
		Minimal,
		Expanded,
	}

	[NoMacCatalyst, Mac (13, 0)]
	[Native]
	public enum NSComboButtonStyle : long {
		Split = 0,
		Unified = 1,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSImageDynamicRange : long {
		Unspecified = -1,
		Standard = 0,
		ConstrainedHigh = 1,
		High = 2,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSMenuItemBadgeType : long {
		None = 0,
		Updates,
		NewItems,
		Alerts,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSMenuPresentationStyle : long {
		Regular = 0,
		Palette = 1,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSMenuSelectionMode : long {
		Automatic = 0,
		SelectOne = 1,
		SelectAny = 2,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSPageLayoutResult : long {
		Cancelled = 0,
		Changed,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSPrintPanelResult : long {
		Cancelled = 0,
		Printed,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSTextCursorAccessoryPlacement : long {
		Unspecified,
		Backward,
		Forward,
		Invisible,
		Center,
		OffscreenLeft,
		OffscreenTop,
		OffscreenRight,
		OffscreenBottom,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	[Flags]
	public enum NSTextInsertionIndicatorAutomaticModeOptions : long {
		EffectsView = 1L << 0,
		WhileTracking = 1L << 1,
	}

	[NoMacCatalyst, Mac (14, 0)]
	[Native]
	public enum NSTextInsertionIndicatorDisplayMode : long {
		Automatic = 0,
		Hidden,
		Visible,
	}

	[Native]
	[Mac (15, 4), NoMacCatalyst]
	public enum NSPasteboardAccessBehavior : ulong {
		Default = 0,
		Ask = 1,
		AlwaysAllow = 2,
		AlwaysDeny = 3,
	}

	[Mac (15, 4), NoMacCatalyst]
	enum NSPasteboardDetectionPattern {
		[Field ("NSPasteboardDetectionPatternProbableWebURL")]
		ProbableWebUrl,

		[Field ("NSPasteboardDetectionPatternProbableWebSearch")]
		ProbableWebSearch,

		[Field ("NSPasteboardDetectionPatternNumber")]
		Number,

		[Field ("NSPasteboardDetectionPatternLink")]
		Link,

		[Field ("NSPasteboardDetectionPatternPhoneNumber")]
		PhoneNumber,

		[Field ("NSPasteboardDetectionPatternEmailAddress")]
		EmailAddress,

		[Field ("NSPasteboardDetectionPatternPostalAddress")]
		PostalAddress,

		[Field ("NSPasteboardDetectionPatternCalendarEvent")]
		CalendarEvent,

		[Field ("NSPasteboardDetectionPatternShipmentTrackingNumber")]
		ShipmentTrackingNumber,

		[Field ("NSPasteboardDetectionPatternFlightNumber")]
		FlightNumber,

		[Field ("NSPasteboardDetectionPatternMoneyAmount")]
		MoneyAmount,
	}

	[Mac (15, 4), NoMacCatalyst]
	enum NSPasteboardMetadataType {
		[Field ("NSPasteboardMetadataTypeContentType")]
		ContentType,
	}
}
