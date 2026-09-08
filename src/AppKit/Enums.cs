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
using System.ComponentModel;

#nullable enable

namespace AppKit {

	/// <summary>Specifies a return response from an application modal session.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSRunResponse : long {
		/// <summary>Indicates that the modal session stopped.</summary>
		Stopped = -1000,
		/// <summary>Indicates that the modal session was aborted.</summary>
		Aborted = -1001,
		/// <summary>Indicates that the modal session should continue.</summary>
		Continues = -1002,
	}

	/// <summary>Specifies options for activating an application.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSApplicationActivationOptions : ulong {
		/// <summary>Activates the application using the default behavior.</summary>
		Default = 0,
		/// <summary>Brings all the application's windows forward.</summary>
		ActivateAllWindows = 1,
		/// <summary>Activates the application regardless of which application is currently active.</summary>
		ActivateIgnoringOtherWindows = 2,
	}

	/// <summary>Specifies how an application participates in the user interface.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSApplicationActivationPolicy : long {
		/// <summary>The application appears in the Dock and can have a menu bar.</summary>
		Regular,
		/// <summary>The application does not appear in the Dock, but can present a user interface.</summary>
		Accessory,
		/// <summary>The application does not appear in the Dock and cannot create windows or be activated.</summary>
		Prohibited,
	}

	/// <summary>Specifies options that control the visibility of system UI, such as the Dock and menu bar, while the application is active.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSApplicationPresentationOptions : ulong {
		/// <summary>No special presentation options; the Dock and menu bar behave normally.</summary>
		Default = 0,
		/// <summary>The Dock is hidden and appears only when the pointer moves to its screen edge.</summary>
		AutoHideDock = (1 << 0),
		/// <summary>The Dock is entirely unavailable to the user.</summary>
		HideDock = (1 << 1),

		/// <summary>The menu bar is hidden and appears only when the pointer moves to the top of the screen.</summary>
		AutoHideMenuBar = (1 << 2),
		/// <summary>The menu bar is entirely unavailable to the user.</summary>
		HideMenuBar = (1 << 3),

		/// <summary>All Apple menu items are disabled.</summary>
		DisableAppleMenu = (1 << 4),
		/// <summary>The process switching user interface (such as Command-Tab) is disabled.</summary>
		DisableProcessSwitching = (1 << 5),
		/// <summary>The Force Quit panel is disabled.</summary>
		DisableForceQuit = (1 << 6),
		/// <summary>Logging out and shutting down are disabled.</summary>
		DisableSessionTermination = (1 << 7),
		/// <summary>The "Hide" and "Hide Others" menu commands, as well as their keyboard shortcuts, are disabled.</summary>
		DisableHideApplication = (1 << 8),
		/// <summary>The menu bar does not display its usual translucent background.</summary>
		DisableMenuBarTransparency = (1 << 9),

		/// <summary>The application's windows are displayed in full-screen mode, hiding the Dock and menu bar.</summary>
		FullScreen = (1 << 10),
		/// <summary>The toolbar is hidden and shown automatically together with the menu bar.</summary>
		AutoHideToolbar = (1 << 11),
		/// <summary>The floating overlay that assists in locating the pointer is disabled.</summary>
		DisableCursorLocationAssistance = (1 << 12),
	}

	/// <summary>Specifies the result of an application delegate operation.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSApplicationDelegateReply : ulong {
		/// <summary>The operation completed successfully.</summary>
		Success,
		/// <summary>The operation was canceled.</summary>
		Cancel,
		/// <summary>The operation failed.</summary>
		Failure,
	}

	/// <summary>Specifies the urgency of a request for the user's attention.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSRequestUserAttentionType : ulong {
		/// <summary>Requests the user's attention for a critical event.</summary>
		CriticalRequest = 0,
		/// <summary>Requests the user's attention for an informational event.</summary>
		InformationalRequest = 10,
	}

	/// <summary>Specifies how an application responds to a termination request.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSApplicationTerminateReply : ulong {
		/// <summary>Cancels application termination.</summary>
		Cancel,
		/// <summary>Terminates the application immediately.</summary>
		Now,
		/// <summary>Defers the termination decision.</summary>
		Later,
	}

	/// <summary>Specifies the result of an application print request.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSApplicationPrintReply : ulong {
		/// <summary>The print request was canceled.</summary>
		Cancelled,
		/// <summary>The print request completed successfully.</summary>
		Success,
		/// <summary>The print request failed.</summary>
		Failure,
		/// <summary>The application will reply after completing the print request asynchronously.</summary>
		ReplyLater,
	}

	/// <summary>Specifies the level of interpolation quality used when an image is scaled or transformed.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSImageInterpolation : ulong {
		/// <summary>Uses the interpolation quality most appropriate for the current context.</summary>
		Default,
		/// <summary>Performs no interpolation; the image is drawn using nearest-neighbor sampling.</summary>
		None,
		/// <summary>Uses a fast, low-quality interpolation algorithm.</summary>
		Low,
		/// <summary>Uses an interpolation algorithm that balances speed and quality.</summary>
		Medium,
		/// <summary>Uses the slowest, highest-quality interpolation algorithm available.</summary>
		High,
	}

	/// <summary>Specifies a Porter-Duff compositing operator, or a Core Image-style blend mode, used to combine a source image with a destination image.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSComposite : ulong {
		/// <summary>The result is transparent black; neither the source nor the destination contributes.</summary>
		Clear,
		/// <summary>The source image replaces the destination image.</summary>
		Copy,
		/// <summary>The source image is composited over the destination image.</summary>
		SourceOver,
		/// <summary>The part of the source image that lies within the destination image replaces the destination; everything else is discarded.</summary>
		SourceIn,
		/// <summary>The part of the source image that lies outside the destination image is displayed; everything else is discarded.</summary>
		SourceOut,
		/// <summary>The source image is composited over the destination image, but only the part of the source that lies within the destination is displayed.</summary>
		SourceAtop,
		/// <summary>The destination image is composited over the source image.</summary>
		DestinationOver,
		/// <summary>The part of the destination image that lies within the source image replaces the destination; everything else is discarded.</summary>
		DestinationIn,
		/// <summary>The part of the destination image that lies outside the source image is displayed; everything else is discarded.</summary>
		DestinationOut,
		/// <summary>The destination image is composited over the source image, but only the part of the destination that lies within the source is displayed.</summary>
		DestinationAtop,
		/// <summary>The parts of the source and destination images that do not overlap are displayed; the overlapping region is discarded.</summary>
		XOR,
		/// <summary>The source and destination color values are summed and the result approaches 0 (black) as the limit.</summary>
		PlusDarker,
		/// <summary>The source image is composited over the destination image using the same behavior as <see cref="F:AppKit.NSComposite.SourceOver" />.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 10, message: "Use NSCompositeSourceOver instead.")]
		Highlight,
		/// <summary>The source and destination color values are summed and the result approaches 1 (white) as the limit.</summary>
		PlusLighter,
		/// <summary>The source and destination color values are multiplied together, always producing a darker or equal color.</summary>
		Multiply,
		/// <summary>The source and destination color values are inverted, multiplied, and inverted again, always producing a lighter or equal color.</summary>
		Screen,
		/// <summary>Multiplies or screens the color values, depending on the destination color, preserving highlights and shadows.</summary>
		Overlay,
		/// <summary>The darker of the source and destination color values is used at each pixel.</summary>
		Darken,
		/// <summary>The lighter of the source and destination color values is used at each pixel.</summary>
		Lighten,
		/// <summary>The destination color is brightened to reflect the source color, increasing the contrast between the two.</summary>
		ColorDodge,
		/// <summary>The destination color is darkened to reflect the source color, increasing the contrast between the two.</summary>
		ColorBurn,
		/// <summary>Darkens or lightens the colors, depending on the source color value, producing an effect similar to shining a diffuse spotlight on the destination.</summary>
		SoftLight,
		/// <summary>Multiplies or screens the color values, depending on the source color, producing an effect similar to shining a harsh spotlight on the destination.</summary>
		HardLight,
		/// <summary>Subtracts either the source color from the destination color or vice versa, whichever produces a positive value, producing an inversion effect.</summary>
		Difference,
		/// <summary>Produces an effect similar to <see cref="F:AppKit.NSComposite.Difference" /> but with lower contrast.</summary>
		Exclusion,
		/// <summary>Uses the luminance and saturation of the destination and the hue of the source.</summary>
		Hue,
		/// <summary>Uses the luminance and hue of the destination and the saturation of the source.</summary>
		Saturation,
		/// <summary>Uses the luminance of the destination and the hue and saturation of the source.</summary>
		Color,
		/// <summary>Uses the hue and saturation of the destination and the luminance of the source.</summary>
		Luminosity,
	}

	/// <summary>Specifies how a window's drawing is buffered before being flushed to the screen.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSBackingStore : ulong {
		/// <summary>The window renders directly into display memory and is not buffered.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'Buffered' instead.")]
		Retained,
		/// <summary>The window renders directly into display memory, without retaining its contents.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'Buffered' instead.")]
		Nonretained,
		/// <summary>The window renders into an off-screen buffer, which is then flushed to the display in a single operation.</summary>
		Buffered,
	}

	/// <summary>Specifies how a window is ordered relative to other windows.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSWindowOrderingMode : long {
		/// <summary>Orders the window below another window.</summary>
		Below = -1,
		/// <summary>Removes the window from the screen.</summary>
		Out,
		/// <summary>Orders the window above another window.</summary>
		Above,
	}

	/// <summary>Specifies where a focus ring is drawn relative to content.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSFocusRingPlacement : ulong {
		/// <summary>Draws only the focus ring.</summary>
		RingOnly,
		/// <summary>Draws the focus ring below the content.</summary>
		RingBelow,
		/// <summary>Draws the focus ring above the content.</summary>
		RingAbove,
	}

	/// <summary>Specifies the focus ring displayed by a control.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSFocusRingType : ulong {
		/// <summary>Uses the default focus ring.</summary>
		Default,
		/// <summary>Does not display a focus ring.</summary>
		None,
		/// <summary>Displays a focus ring outside the control.</summary>
		Exterior,
	}

	/// <summary>Specifies how out-of-gamut colors are mapped to the destination color space.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSColorRenderingIntent : long {
		/// <summary>Uses the rendering intent embedded in the source profile, or <see cref="F:AppKit.NSColorRenderingIntent.Perceptual" /> if none is specified.</summary>
		Default,
		/// <summary>Preserves the exact color values that fall within both color spaces' gamuts and clips out-of-gamut colors to the nearest reproducible color.</summary>
		AbsoluteColorimetric,
		/// <summary>Preserves the exact color values that fall within both color spaces' gamuts, and rescales white points to compensate for differing white points between the color spaces.</summary>
		RelativeColorimetric,
		/// <summary>Compresses the entire source gamut into the destination gamut, preserving the overall visual relationship between colors.</summary>
		Perceptual,
		/// <summary>Preserves the relative saturation of colors, sacrificing color accuracy to maintain vivid colors.</summary>
		Saturation,

	}

	/// <summary>Specifies one of the four edges of a rectangle.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSRectEdge : ulong {
		/// <summary>The minimum edge on the x-axis (the left edge, in a flipped coordinate system).</summary>
		MinXEdge,
		/// <summary>The minimum edge on the y-axis (the bottom edge, in a non-flipped coordinate system).</summary>
		MinYEdge,
		/// <summary>The maximum edge on the x-axis (the right edge, in a flipped coordinate system).</summary>
		MaxXEdge,
		/// <summary>The maximum edge on the y-axis (the top edge, in a non-flipped coordinate system).</summary>
		MaxYEdge,
	}

	/// <summary>Specifies the layout direction of the user interface.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSUserInterfaceLayoutDirection : long {
		/// <summary>Arranges interface elements from left to right.</summary>
		LeftToRight,
		/// <summary>Arranges interface elements from right to left.</summary>
		RightToLeft,
	}

	#region NSColorSpace
	/// <summary>Specifies the fundamental type of color space, describing the number and meaning of its color components.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSColorSpaceModel : long {
		/// <summary>The color space model is not known.</summary>
		Unknown = -1,
		/// <summary>A grayscale color space, with a single component representing intensity.</summary>
		Gray,
		/// <summary>A red-green-blue color space.</summary>
		RGB,
		/// <summary>A cyan-magenta-yellow-black color space.</summary>
		CMYK,
		/// <summary>A CIE L*a*b* color space.</summary>
		LAB,
		/// <summary>A color space with an arbitrary number of color components, such as those used by spot-color or multi-ink printing.</summary>
		DeviceN,
		/// <summary>A color space whose colors are looked up by index in a fixed palette.</summary>
		Indexed,
		/// <summary>A color space whose colors are defined by a repeating pattern image.</summary>
		Pattern,
	}
	#endregion

	#region NSFileWrapper
	#endregion

	#region NSParagraphStyle
	/// <summary>Specifies the alignment of text at a tab stop.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTextTabType : ulong {
		/// <summary>Left-aligns text at the tab stop.</summary>
		Left,
		/// <summary>Right-aligns text at the tab stop.</summary>
		Right,
		/// <summary>Centers text at the tab stop.</summary>
		Center,
		/// <summary>Aligns decimal characters at the tab stop.</summary>
		Decimal,
	}

	/// <summary>Specifies how a line of text is wrapped or truncated when it does not fit within its container.</summary>
	[Native]
	[NoMacCatalyst]
	public enum NSLineBreakMode : ulong {
		/// <summary>Wraps lines at word boundaries, so a word is never split across lines.</summary>
		ByWordWrapping,
		/// <summary>Wraps lines at character boundaries, splitting words if necessary.</summary>
		CharWrapping,
		/// <summary>Clips text that does not fit, without wrapping to a new line.</summary>
		Clipping,
		/// <summary>Truncates the beginning of the line, inserting an ellipsis, so the end of the text remains visible.</summary>
		TruncatingHead,
		/// <summary>Truncates the end of the line, inserting an ellipsis, so the beginning of the text remains visible.</summary>
		TruncatingTail,
		/// <summary>Truncates the middle of the line, inserting an ellipsis, so both the beginning and end of the text remain visible.</summary>
		TruncatingMiddle,
	}

	#endregion

	#region NSCell Defines 

	/// <summary>Specifies the type of content displayed by a cell.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSCellType : ulong {
		/// <summary>The cell has no content type.</summary>
		Null,
		/// <summary>The cell displays text.</summary>
		Text,
		/// <summary>The cell displays an image.</summary>
		Image,
	}

	/// <summary>Specifies a generic attribute of a cell that can be queried or modified using the legacy <c>GetCellAttribute</c> and <c>SetCellAttribute</c> APIs.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSCellAttribute : ulong {
		/// <summary>The cell does not respond to mouse events or other user actions.</summary>
		CellDisabled,
		/// <summary>The current on/off/mixed state of the cell.</summary>
		CellState,
		/// <summary>The cell is drawn with a pushed-in (pressed) appearance.</summary>
		PushInCell,
		/// <summary>The cell's contents can be edited by the user.</summary>
		CellEditable,
		/// <summary>The cell displays a grayed appearance to indicate its highlighted state.</summary>
		ChangeGrayCell,
		/// <summary>The cell is currently highlighted.</summary>
		CellHighlighted,
		/// <summary>The cell indicates its highlighted state by changing its contents rather than its background.</summary>
		CellLightsByContents,
		/// <summary>The cell indicates its highlighted state by changing to a grayed appearance.</summary>
		CellLightsByGray,
		/// <summary>The cell indicates its highlighted state by changing its background color.</summary>
		ChangeBackgroundCell,
		/// <summary>The cell indicates its highlighted state by lightening or darkening its background.</summary>
		CellLightsByBackground,
		/// <summary>The cell draws a border around itself.</summary>
		CellIsBordered,
		/// <summary>The cell's image is drawn overlapping its title text.</summary>
		CellHasOverlappingImage,
		/// <summary>The cell's image and title are arranged horizontally, side by side.</summary>
		CellHasImageHorizontal,
		/// <summary>The cell's image is positioned to the left of, or below, its title.</summary>
		CellHasImageOnLeftOrBottom,
		/// <summary>The cell's displayed contents change to reflect its current state.</summary>
		CellChangesContents,
		/// <summary>The degree to which the cell's bezel is inset, expressed as an integer level.</summary>
		CellIsInsetButton,
		/// <summary>The cell supports a third, mixed state in addition to its on and off states.</summary>
		CellAllowsMixedState,
	}

	/// <summary>Specifies how an image and title text are arranged within a cell.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSCellImagePosition : ulong {
		/// <summary>The cell displays no image.</summary>
		NoImage,
		/// <summary>The cell displays only its image; any title is not shown.</summary>
		ImageOnly,
		/// <summary>The image appears to the left of the title.</summary>
		ImageLeft,
		/// <summary>The image appears to the right of the title.</summary>
		ImageRight,
		/// <summary>The image appears below the title.</summary>
		ImageBelow,
		/// <summary>The image appears above the title.</summary>
		ImageAbove,
		/// <summary>The image is drawn directly on top of the title.</summary>
		ImageOverlaps,
		/// <summary>The image appears at the leading edge of the title, honoring the current layout direction.</summary>
		ImageLeading,
		/// <summary>The image appears at the trailing edge of the title, honoring the current layout direction.</summary>
		ImageTrailing,
	}

	/// <summary>Specifies how an image is scaled to fit the space allotted to it.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSImageScale : ulong {
		/// <summary>Scales the image proportionally down to fit the space, but never scales it up.</summary>
		ProportionallyDown = 0,
		/// <summary>Scales each dimension of the image independently, without regard to the image's original aspect ratio, so it fills the space exactly.</summary>
		AxesIndependently,
		/// <summary>Does not scale the image.</summary>
		None,
		/// <summary>Scales the image proportionally, either up or down, to fit the space as closely as possible.</summary>
		ProportionallyUpOrDown,
	}

	/// <summary>Specifies the state of a cell.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSCellStateValue : long {
		/// <summary>The cell has a mixed state.</summary>
		Mixed = -1,
		/// <summary>The cell is off.</summary>
		Off,
		/// <summary>The cell is on.</summary>
		On,
	}

	/// <summary>Specifies legacy style-mask flags that describe the appearance and behavior of a cell.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSCellStyleMask : ulong {
		/// <summary>No style flags are set.</summary>
		NoCell = 0,
		/// <summary>The cell displays content, such as text or an image.</summary>
		ContentsCell = 1 << 0,
		/// <summary>The cell is drawn with a pushed-in (pressed) appearance.</summary>
		PushInCell = 1 << 1,
		/// <summary>The cell displays a grayed appearance to indicate its highlighted state.</summary>
		ChangeGrayCell = 1 << 2,
		/// <summary>The cell indicates its highlighted state by changing its background color.</summary>
		ChangeBackgroundCell = 1 << 3,
	}

	/// <summary>Specifies which part of a cell, if any, a hit-test point falls within.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSCellHit : ulong {
		/// <summary>The point does not fall within any part of the cell.</summary>
		None,
		/// <summary>The point falls within the cell's content area.</summary>
		ContentArea = 1,
		/// <summary>The point falls within an editable and selectable text area of the cell.</summary>
		EditableTextArea = 2,
		/// <summary>The point falls within a trackable area of the cell, such as one that responds to mouse tracking.</summary>
		TrackableArea = 4,
#if !XAMCORE_5_0
		/// <summary>Obsolete alias kept for backward compatibility. Use <see cref="F:AppKit.NSCellHit.TrackableArea" /> instead.</summary>
		[Obsolete ("Use 'TrackableArea' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		TrackableArae = TrackableArea,
#endif
	}

	/// <summary>Specifies the tint color applied to certain controls, following the user's system-wide color preference.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSControlTint : ulong {
		/// <summary>Uses the system's default control tint.</summary>
		Default = 0,    // system 'default'
		/// <summary>Uses a blue control tint.</summary>
		Blue = 1,
		/// <summary>Uses a graphite (gray) control tint.</summary>
		Graphite = 6,
		/// <summary>No control tint is applied.</summary>
		Clear = 7,
	}

	/// <summary>Specifies the physical size at which a control is drawn.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSControlSize : ulong {
		/// <summary>The standard, full-size control.</summary>
		Regular = 0,
		/// <summary>A smaller variant of the control.</summary>
		Small = 1,
		/// <summary>The smallest variant of the control.</summary>
		Mini = 2,
		/// <summary>A control size that is larger than <see cref="F:AppKit.NSControlSize.Regular" />.</summary>
		Large = 3,
		/// <summary>A control size that is larger than <see cref="F:AppKit.NSControlSize.Large" />.</summary>
		[Mac (26, 0)]
		ExtraLarge = 4,
	}

	/// <summary>Specifies the visual context in which content, such as a cell's text or image, is being drawn, so it can adjust its appearance for legibility.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSBackgroundStyle : long {
		/// <summary>The content is drawn against a normal, unemphasized background.</summary>
		Normal = 0,
		/// <summary>The content is drawn against a light background.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Normal' instead.")]
		Light = Normal,
		/// <summary>The content is drawn against a dark, selected, or otherwise emphasized background, and should adjust its appearance for contrast.</summary>
		Emphasized,
		/// <summary>The content is drawn against a dark background.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Emphasized' instead.")]
		Dark = Emphasized,
		/// <summary>The content is drawn on a raised, embossed surface.</summary>
		Raised,
		/// <summary>The content is drawn on a lowered, engraved surface.</summary>
		Lowered,
	}
	#endregion

	#region NSImage

	/// <summary>Specifies the outcome of an incremental image-loading operation.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSImageLoadStatus : ulong {
		/// <summary>The image finished loading successfully.</summary>
		Completed,
		/// <summary>The image load operation was canceled.</summary>
		Cancelled,
		/// <summary>The image data was invalid or malformed.</summary>
		InvalidData,
		/// <summary>The image data ended unexpectedly before loading completed.</summary>
		UnexpectedEOF,
		/// <summary>An error occurred while reading the image data.</summary>
		ReadError,
	}

	/// <summary>Specifies when an <see cref="T:AppKit.NSImageRep" /> caches its rendered output as a bitmap.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSImageCacheMode : ulong {
		/// <summary>Lets the image decide the appropriate caching behavior.</summary>
		Default,
		/// <summary>Always caches the rendered image.</summary>
		Always,
		/// <summary>Caches the rendered image only when it is drawn at its native size.</summary>
		BySize,
		/// <summary>Never caches the rendered image; it is always redrawn from its source representation.</summary>
		Never,
	}

	/// <summary>Specifies how an image is resized to fill its destination.</summary>
	[NoMacCatalyst]
	[Native (ConvertToNative = "NSImageResizingModeExtensions.ToNative", ConvertToManaged = "NSImageResizingModeExtensions.ToManaged")]
	public enum NSImageResizingMode : long {
		/// <summary>Stretches the image to fill the destination.</summary>
		Stretch,
		/// <summary>Tiles the image to fill the destination.</summary>
		Tile,
	}

	#endregion

	#region NSAlert
	/// <summary>Specifies the visual style of an alert.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSAlertStyle : ulong {
		/// <summary>Displays a warning alert.</summary>
		Warning,
		/// <summary>Displays an informational alert.</summary>
		Informational,
		/// <summary>Displays an alert for a critical condition.</summary>
		Critical,
	}

	/// <summary>Specifies a response returned by a modal session.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSModalResponse : long {
		/// <summary>Indicates that the user accepted the modal session.</summary>
		OK = 1,
		/// <summary>Indicates that the user canceled the modal session.</summary>
		Cancel = 0,
		/// <summary>Indicates that the modal session should stop.</summary>
		Stop = -1000,
		/// <summary>Indicates that the modal session should abort.</summary>
		Abort = -1001,
		/// <summary>Indicates that the modal session should continue.</summary>
		Continue = -1002,
	}
	#endregion

	#region NSEvent
	/// <summary>Specifies the kind of event represented by an <see cref="T:AppKit.NSEvent" />, such as a mouse, keyboard, or gesture event.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSEventType : ulong {
		/// <summary>The left (primary) mouse button was pressed.</summary>
		LeftMouseDown = 1,
		/// <summary>The left (primary) mouse button was released.</summary>
		LeftMouseUp = 2,
		/// <summary>The right (secondary) mouse button was pressed.</summary>
		RightMouseDown = 3,
		/// <summary>The right (secondary) mouse button was released.</summary>
		RightMouseUp = 4,
		/// <summary>The mouse moved without any mouse button pressed.</summary>
		MouseMoved = 5,
		/// <summary>The mouse moved while the left mouse button was held down.</summary>
		LeftMouseDragged = 6,
		/// <summary>The mouse moved while the right mouse button was held down.</summary>
		RightMouseDragged = 7,
		/// <summary>The pointer entered a tracking rectangle or tracking area.</summary>
		MouseEntered = 8,
		/// <summary>The pointer exited a tracking rectangle or tracking area.</summary>
		MouseExited = 9,
		/// <summary>A key on the keyboard was pressed.</summary>
		KeyDown = 10,
		/// <summary>A key on the keyboard was released.</summary>
		KeyUp = 11,
		/// <summary>The state of one or more modifier keys (such as Shift, Control, or Command) changed.</summary>
		FlagsChanged = 12,
		/// <summary>An event generated internally by AppKit, not directly caused by user interaction.</summary>
		AppKitDefined = 13,
		/// <summary>An event generated internally by the system, not directly caused by user interaction.</summary>
		SystemDefined = 14,
		/// <summary>A custom event defined and posted by the application, typically used to wake up the run loop.</summary>
		ApplicationDefined = 15,
		/// <summary>A periodic event generated by a repeating timer, typically used during mouse-tracking loops.</summary>
		Periodic = 16,
		/// <summary>The pointer moved over a region whose cursor should be updated, such as when entering a cursor rectangle.</summary>
		CursorUpdate = 17,

		/// <summary>The scroll wheel, or a trackpad scrolling gesture, was moved.</summary>
		ScrollWheel = 22,

		/// <summary>A point event generated by a graphics tablet stylus.</summary>
		TabletPoint = 23,
		/// <summary>A proximity event indicating that a graphics tablet stylus entered or left proximity to the tablet.</summary>
		TabletProximity = 24,

		/// <summary>A mouse button other than the left or right button was pressed.</summary>
		OtherMouseDown = 25,
		/// <summary>A mouse button other than the left or right button was released.</summary>
		OtherMouseUp = 26,
		/// <summary>The mouse moved while a mouse button other than the left or right button was held down.</summary>
		OtherMouseDragged = 27,

		/// <summary>A general, non-specific touch or trackpad gesture event.</summary>
		Gesture = 29,
		/// <summary>A pinch-to-zoom (magnify) gesture on a trackpad.</summary>
		Magnify = 30,
		/// <summary>A swipe gesture on a trackpad.</summary>
		Swipe = 31,
		/// <summary>A rotation gesture on a trackpad.</summary>
		Rotate = 18,
		/// <summary>Indicates the start of a sequence of gesture-tracking events.</summary>
		BeginGesture = 19,
		/// <summary>Indicates the end of a sequence of gesture-tracking events.</summary>
		EndGesture = 20,

		/// <summary>A "smart zoom" gesture, such as a two-finger double-tap on a trackpad.</summary>
		SmartMagnify = 32,
		/// <summary>A Quick Look, or "look up", gesture, such as a force click on a trackpad.</summary>
		QuickLook = 33,
		/// <summary>A change in the amount of force applied by the user on a pressure-sensitive (Force Touch) trackpad.</summary>
		Pressure = 34, // 10.10.3, 64-bit-only
		/// <summary>A touch event received directly from a touch-capable surface, such as the Touch Bar.</summary>
		DirectTouch = 37, // 10.10
		/// <summary>Indicates a change in input mode, such as switching between mouse and touch input.</summary>
		ChangeMode = 38,
		/// <summary>Indicates that a mouse event in progress was cancelled.</summary>
		[Mac (26, 0)]
		MouseCancelled = 40,
	}

	/// <summary>Specifies a bitmask of one or more <see cref="T:AppKit.NSEventType" /> values, used to filter which kinds of events are retrieved or handled.</summary>
	[NoMacCatalyst]
	[Flags]
	public enum NSEventMask : ulong {
		/// <summary>Matches <see cref="F:AppKit.NSEventType.LeftMouseDown" /> events.</summary>
		LeftMouseDown = 1UL << (int) NSEventType.LeftMouseDown,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.LeftMouseUp" /> events.</summary>
		LeftMouseUp = 1UL << (int) NSEventType.LeftMouseUp,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.RightMouseDown" /> events.</summary>
		RightMouseDown = 1UL << (int) NSEventType.RightMouseDown,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.RightMouseUp" /> events.</summary>
		RightMouseUp = 1UL << (int) NSEventType.RightMouseUp,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.MouseMoved" /> events.</summary>
		MouseMoved = 1UL << (int) NSEventType.MouseMoved,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.LeftMouseDragged" /> events.</summary>
		LeftMouseDragged = 1UL << (int) NSEventType.LeftMouseDragged,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.RightMouseDragged" /> events.</summary>
		RightMouseDragged = 1UL << (int) NSEventType.RightMouseDragged,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.MouseEntered" /> events.</summary>
		MouseEntered = 1UL << (int) NSEventType.MouseEntered,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.MouseExited" /> events.</summary>
		MouseExited = 1UL << (int) NSEventType.MouseExited,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.KeyDown" /> events.</summary>
		KeyDown = 1UL << (int) NSEventType.KeyDown,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.KeyUp" /> events.</summary>
		KeyUp = 1UL << (int) NSEventType.KeyUp,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.FlagsChanged" /> events.</summary>
		FlagsChanged = 1UL << (int) NSEventType.FlagsChanged,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.AppKitDefined" /> events.</summary>
		AppKitDefined = 1UL << (int) NSEventType.AppKitDefined,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.SystemDefined" /> events.</summary>
		SystemDefined = 1UL << (int) NSEventType.SystemDefined,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.ApplicationDefined" /> events.</summary>
		ApplicationDefined = 1UL << (int) NSEventType.ApplicationDefined,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.Periodic" /> events.</summary>
		Periodic = 1UL << (int) NSEventType.Periodic,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.CursorUpdate" /> events.</summary>
		CursorUpdate = 1UL << (int) NSEventType.CursorUpdate,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.ScrollWheel" /> events.</summary>
		ScrollWheel = 1UL << (int) NSEventType.ScrollWheel,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.TabletPoint" /> events.</summary>
		TabletPoint = 1UL << (int) NSEventType.TabletPoint,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.TabletProximity" /> events.</summary>
		TabletProximity = 1UL << (int) NSEventType.TabletProximity,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.OtherMouseDown" /> events.</summary>
		OtherMouseDown = 1UL << (int) NSEventType.OtherMouseDown,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.OtherMouseUp" /> events.</summary>
		OtherMouseUp = 1UL << (int) NSEventType.OtherMouseUp,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.OtherMouseDragged" /> events.</summary>
		OtherMouseDragged = 1UL << (int) NSEventType.OtherMouseDragged,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.Gesture" /> events.</summary>
		EventGesture = 1UL << (int) NSEventType.Gesture,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.Magnify" /> events.</summary>
		EventMagnify = 1UL << (int) NSEventType.Magnify,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.Swipe" /> events.</summary>
		EventSwipe = 1UL << (int) NSEventType.Swipe,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.Rotate" /> events.</summary>
		EventRotate = 1UL << (int) NSEventType.Rotate,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.BeginGesture" /> events.</summary>
		EventBeginGesture = 1UL << (int) NSEventType.BeginGesture,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.EndGesture" /> events.</summary>
		EventEndGesture = 1UL << (int) NSEventType.EndGesture,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.SmartMagnify" /> events.</summary>
		SmartMagnify = 1UL << (int) NSEventType.SmartMagnify,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.Pressure" /> events.</summary>
		Pressure = 1UL << (int) NSEventType.Pressure, // 10.10.3, 64-bit-only
		/// <summary>Matches <see cref="F:AppKit.NSEventType.DirectTouch" /> events.</summary>
		DirectTouch = 1UL << (int) NSEventType.DirectTouch, // 10.10
		/// <summary>Matches <see cref="F:AppKit.NSEventType.ChangeMode" /> events.</summary>
		ChangeMode = 1UL << (int) NSEventType.ChangeMode,
		/// <summary>Matches <see cref="F:AppKit.NSEventType.MouseCancelled" /> events.</summary>
		[Mac (26, 0)]
		MouseCancelled = 1UL << (int) NSEventType.MouseCancelled,
		/// <summary>Matches every kind of event.</summary>
		AnyEvent = unchecked((ulong) UInt64.MaxValue),
	}

	/// <summary>Specifies a bitmask of modifier keys, such as Shift, Control, or Command, that were held down when an event occurred.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSEventModifierMask : ulong {
		/// <summary>The Caps Lock key is engaged.</summary>
		AlphaShiftKeyMask = 1 << 16,
		/// <summary>A Shift key is held down.</summary>
		ShiftKeyMask = 1 << 17,
		/// <summary>A Control key is held down.</summary>
		ControlKeyMask = 1 << 18,
		/// <summary>An Option (Alternate) key is held down.</summary>
		AlternateKeyMask = 1 << 19,
		/// <summary>A Command key is held down.</summary>
		CommandKeyMask = 1 << 20,
		/// <summary>The key is located on the numeric keypad.</summary>
		NumericPadKeyMask = 1 << 21,
		/// <summary>The Help key is held down.</summary>
		HelpKeyMask = 1 << 22,
		/// <summary>The key is a function key, such as F1 through F12, or an arrow key.</summary>
		FunctionKeyMask = 1 << 23,
		/// <summary>A mask covering all of the device-independent modifier flags.</summary>
		DeviceIndependentModifierFlagsMask = 0xffff0000,
	}

	/// <summary>Specifies the type of pointing device that generated a tablet event.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSPointingDeviceType : ulong {
		/// <summary>The device type is not known.</summary>
		Unknown,
		/// <summary>The device is the tip (writing point) of a stylus.</summary>
		Pen,
		/// <summary>The device is a puck-style pointing device, such as a mouse used with a graphics tablet.</summary>
		Cursor,
		/// <summary>The device is the eraser end of a stylus.</summary>
		Eraser,
	}

	/// <summary>Specifies a bitmask of stylus buttons on a graphics tablet pen.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSEventButtonMask : ulong {
		/// <summary>The tip of the pen is in contact with the tablet.</summary>
		Pen = 1,
		/// <summary>The lower side button of the pen is pressed.</summary>
		PenLower = 2,
		/// <summary>The upper side button of the pen is pressed.</summary>
		PenUpper = 4,
	}

	// This enum is defined as an untyped enum in MacOSX.sdk/System/Library/Frameworks/Carbon.framework/Versions/A/Frameworks/HIToolbox.framework/Versions/A/Headers/Events.h
	// It represents values that may be returned by NSEvent.KeyCode (which isn't typed as 'NSKey' because it may be many other values as well).
	/// <summary>Specifies a virtual key code identifying a physical key on the keyboard, independent of the current keyboard layout.</summary>
	[NoMacCatalyst]
	public enum NSKey {
		/// <summary>The virtual key code for the 'A' key.</summary>
		A = 0x00,
		/// <summary>The virtual key code for the 'S' key.</summary>
		S = 0x01,
		/// <summary>The virtual key code for the 'D' key.</summary>
		D = 0x02,
		/// <summary>The virtual key code for the 'F' key.</summary>
		F = 0x03,
		/// <summary>The virtual key code for the 'H' key.</summary>
		H = 0x04,
		/// <summary>The virtual key code for the 'G' key.</summary>
		G = 0x05,
		/// <summary>The virtual key code for the 'Z' key.</summary>
		Z = 0x06,
		/// <summary>The virtual key code for the 'X' key.</summary>
		X = 0x07,
		/// <summary>The virtual key code for the 'C' key.</summary>
		C = 0x08,
		/// <summary>The virtual key code for the 'V' key.</summary>
		V = 0x09,
		/// <summary>The virtual key code for the 'B' key.</summary>
		B = 0x0B,
		/// <summary>The virtual key code for the 'Q' key.</summary>
		Q = 0x0C,
		/// <summary>The virtual key code for the 'W' key.</summary>
		W = 0x0D,
		/// <summary>The virtual key code for the 'E' key.</summary>
		E = 0x0E,
		/// <summary>The virtual key code for the 'R' key.</summary>
		R = 0x0F,
		/// <summary>The virtual key code for the 'Y' key.</summary>
		Y = 0x10,
		/// <summary>The virtual key code for the 'T' key.</summary>
		T = 0x11,
		/// <summary>The virtual key code for the '1' key on the top row.</summary>
		D1 = 0x12,
		/// <summary>The virtual key code for the '2' key on the top row.</summary>
		D2 = 0x13,
		/// <summary>The virtual key code for the '3' key on the top row.</summary>
		D3 = 0x14,
		/// <summary>The virtual key code for the '4' key on the top row.</summary>
		D4 = 0x15,
		/// <summary>The virtual key code for the '6' key on the top row.</summary>
		D6 = 0x16,
		/// <summary>The virtual key code for the '5' key on the top row.</summary>
		D5 = 0x17,
		/// <summary>The virtual key code for the '=' (equals) key.</summary>
		Equal = 0x18,
		/// <summary>The virtual key code for the '9' key on the top row.</summary>
		D9 = 0x19,
		/// <summary>The virtual key code for the '7' key on the top row.</summary>
		D7 = 0x1A,
		/// <summary>The virtual key code for the '-' (minus/hyphen) key.</summary>
		Minus = 0x1B,
		/// <summary>The virtual key code for the '8' key on the top row.</summary>
		D8 = 0x1C,
		/// <summary>The virtual key code for the '0' key on the top row.</summary>
		D0 = 0x1D,
		/// <summary>The virtual key code for the ']' (right bracket) key.</summary>
		RightBracket = 0x1E,
		/// <summary>The virtual key code for the 'O' key.</summary>
		O = 0x1F,
		/// <summary>The virtual key code for the 'U' key.</summary>
		U = 0x20,
		/// <summary>The virtual key code for the '[' (left bracket) key.</summary>
		LeftBracket = 0x21,
		/// <summary>The virtual key code for the 'I' key.</summary>
		I = 0x22,
		/// <summary>The virtual key code for the 'P' key.</summary>
		P = 0x23,
		/// <summary>The virtual key code for the 'L' key.</summary>
		L = 0x25,
		/// <summary>The virtual key code for the 'J' key.</summary>
		J = 0x26,
		/// <summary>The virtual key code for the quote (') key.</summary>
		Quote = 0x27,
		/// <summary>The virtual key code for the 'K' key.</summary>
		K = 0x28,
		/// <summary>The virtual key code for the ';' (semicolon) key.</summary>
		Semicolon = 0x29,
		/// <summary>The virtual key code for the backslash ('\') key.</summary>
		Backslash = 0x2A,
		/// <summary>The virtual key code for the ',' (comma) key.</summary>
		Comma = 0x2B,
		/// <summary>The virtual key code for the '/' (slash) key.</summary>
		Slash = 0x2C,
		/// <summary>The virtual key code for the 'N' key.</summary>
		N = 0x2D,
		/// <summary>The virtual key code for the 'M' key.</summary>
		M = 0x2E,
		/// <summary>The virtual key code for the '.' (period) key.</summary>
		Period = 0x2F,
		/// <summary>The virtual key code for the grave accent/tilde ('`') key.</summary>
		Grave = 0x32,
		/// <summary>The virtual key code for the decimal point key on the numeric keypad.</summary>
		KeypadDecimal = 0x41,
		/// <summary>The virtual key code for the multiply ('*') key on the numeric keypad.</summary>
		KeypadMultiply = 0x43,
		/// <summary>The virtual key code for the plus ('+') key on the numeric keypad.</summary>
		KeypadPlus = 0x45,
		/// <summary>The virtual key code for the Clear key on the numeric keypad.</summary>
		KeypadClear = 0x47,
		/// <summary>The virtual key code for the divide ('/') key on the numeric keypad.</summary>
		KeypadDivide = 0x4B,
		/// <summary>The virtual key code for the Enter key on the numeric keypad.</summary>
		KeypadEnter = 0x4C,
		/// <summary>The virtual key code for the minus ('-') key on the numeric keypad.</summary>
		KeypadMinus = 0x4E,
		/// <summary>The virtual key code for the equals ('=') key on the numeric keypad.</summary>
		KeypadEquals = 0x51,
		/// <summary>The virtual key code for the '0' key on the numeric keypad.</summary>
		Keypad0 = 0x52,
		/// <summary>The virtual key code for the '1' key on the numeric keypad.</summary>
		Keypad1 = 0x53,
		/// <summary>The virtual key code for the '2' key on the numeric keypad.</summary>
		Keypad2 = 0x54,
		/// <summary>The virtual key code for the '3' key on the numeric keypad.</summary>
		Keypad3 = 0x55,
		/// <summary>The virtual key code for the '4' key on the numeric keypad.</summary>
		Keypad4 = 0x56,
		/// <summary>The virtual key code for the '5' key on the numeric keypad.</summary>
		Keypad5 = 0x57,
		/// <summary>The virtual key code for the '6' key on the numeric keypad.</summary>
		Keypad6 = 0x58,
		/// <summary>The virtual key code for the '7' key on the numeric keypad.</summary>
		Keypad7 = 0x59,
		/// <summary>The virtual key code for the '8' key on the numeric keypad.</summary>
		Keypad8 = 0x5B,
		/// <summary>The virtual key code for the '9' key on the numeric keypad.</summary>
		Keypad9 = 0x5C,
		/// <summary>The virtual key code for the Return key.</summary>
		Return = 0x24,
		/// <summary>The virtual key code for the Tab key.</summary>
		Tab = 0x30,
		/// <summary>The virtual key code for the Space bar.</summary>
		Space = 0x31,
		/// <summary>The virtual key code for the Delete (Backspace) key.</summary>
		Delete = 0x33,
		/// <summary>The virtual key code for the Escape key.</summary>
		Escape = 0x35,
		/// <summary>The virtual key code for the (left) Command key.</summary>
		Command = 0x37,
		/// <summary>The virtual key code for the (left) Shift key.</summary>
		Shift = 0x38,
		/// <summary>The virtual key code for the Caps Lock key.</summary>
		CapsLock = 0x39,
		/// <summary>The virtual key code for the (left) Option key.</summary>
		Option = 0x3A,
		/// <summary>The virtual key code for the (left) Control key.</summary>
		Control = 0x3B,
		/// <summary>The virtual key code for the right Command key.</summary>
		RightCommand = 0x36,
		/// <summary>The virtual key code for the right Shift key.</summary>
		RightShift = 0x3C,
		/// <summary>The virtual key code for the right Option key.</summary>
		RightOption = 0x3D,
		/// <summary>The virtual key code for the right Control key.</summary>
		RightControl = 0x3E,
		/// <summary>The virtual key code for the Fn (Function) key.</summary>
		Function = 0x3F,
		/// <summary>The virtual key code for the F17 function key.</summary>
		F17 = 0x40,
		/// <summary>The virtual key code for the volume up media key.</summary>
		VolumeUp = 0x48,
		/// <summary>The virtual key code for the volume down media key.</summary>
		VolumeDown = 0x49,
		/// <summary>The virtual key code for the mute media key.</summary>
		Mute = 0x4A,
		/// <summary>The virtual key code for the Forward Delete key (deletes the character in front of the insertion point).</summary>
		ForwardDelete = 0x75,
		/// <summary>The virtual key code for the extra key found between Shift and 'Z' on ISO keyboard layouts.</summary>
		ISOSection = 0x0A,
		/// <summary>The virtual key code for the Yen ('¥') key found on JIS keyboard layouts.</summary>
		JISYen = 0x5D,
		/// <summary>The virtual key code for the underscore ('_') key found on JIS keyboard layouts.</summary>
		JISUnderscore = 0x5E,
		/// <summary>The virtual key code for the comma key on the numeric keypad, found on JIS keyboard layouts.</summary>
		JISKeypadComma = 0x5F,
		/// <summary>The virtual key code for the Eisu (alphanumeric input) key found on JIS keyboard layouts.</summary>
		JISEisu = 0x66,
		/// <summary>The virtual key code for the Kana (Japanese kana input) key found on JIS keyboard layouts.</summary>
		JISKana = 0x68,
		/// <summary>The virtual key code for the F18 function key.</summary>
		F18 = 0x4F,
		/// <summary>The virtual key code for the F19 function key.</summary>
		F19 = 0x50,
		/// <summary>The virtual key code for the F20 function key.</summary>
		F20 = 0x5A,
		/// <summary>The virtual key code for the F5 function key.</summary>
		F5 = 0x60,
		/// <summary>The virtual key code for the F6 function key.</summary>
		F6 = 0x61,
		/// <summary>The virtual key code for the F7 function key.</summary>
		F7 = 0x62,
		/// <summary>The virtual key code for the F3 function key.</summary>
		F3 = 0x63,
		/// <summary>The virtual key code for the F8 function key.</summary>
		F8 = 0x64,
		/// <summary>The virtual key code for the F9 function key.</summary>
		F9 = 0x65,
		/// <summary>The virtual key code for the F11 function key.</summary>
		F11 = 0x67,
		/// <summary>The virtual key code for the F13 function key.</summary>
		F13 = 0x69,
		/// <summary>The virtual key code for the F16 function key.</summary>
		F16 = 0x6A,
		/// <summary>The virtual key code for the F14 function key.</summary>
		F14 = 0x6B,
		/// <summary>The virtual key code for the F10 function key.</summary>
		F10 = 0x6D,
		/// <summary>The virtual key code for the F12 function key.</summary>
		F12 = 0x6F,
		/// <summary>The virtual key code for the F15 function key.</summary>
		F15 = 0x71,
		/// <summary>The virtual key code for the Help key.</summary>
		Help = 0x72,
		/// <summary>The virtual key code for the Home key.</summary>
		Home = 0x73,
		/// <summary>The virtual key code for the Page Up key.</summary>
		PageUp = 0x74,
		/// <summary>The virtual key code for the F4 function key.</summary>
		F4 = 0x76,
		/// <summary>The virtual key code for the End key.</summary>
		End = 0x77,
		/// <summary>The virtual key code for the F2 function key.</summary>
		F2 = 0x78,
		/// <summary>The virtual key code for the Page Down key.</summary>
		PageDown = 0x79,
		/// <summary>The virtual key code for the F1 function key.</summary>
		F1 = 0x7A,
		/// <summary>The virtual key code for the left arrow key.</summary>
		LeftArrow = 0x7B,
		/// <summary>The virtual key code for the right arrow key.</summary>
		RightArrow = 0x7C,
		/// <summary>The virtual key code for the down arrow key.</summary>
		DownArrow = 0x7D,
		/// <summary>The virtual key code for the up arrow key.</summary>
		UpArrow = 0x7E,
	}

	// This is an untyped enum in AppKit's NSEvent.h
	/// <summary>Specifies a Unicode private-use-area code point representing a non-printable function key, such as an arrow key or F-key.</summary>
	[NoMacCatalyst]
	public enum NSFunctionKey : int {
		/// <summary>A unicode private-use-area code point representing the up arrow key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		UpArrow = 0xF700,
		/// <summary>A unicode private-use-area code point representing the down arrow key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		DownArrow = 0xF701,
		/// <summary>A unicode private-use-area code point representing the left arrow key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		LeftArrow = 0xF702,
		/// <summary>A unicode private-use-area code point representing the right arrow key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		RightArrow = 0xF703,
		/// <summary>A unicode private-use-area code point representing the F1 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F1 = 0xF704,
		/// <summary>A unicode private-use-area code point representing the F2 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F2 = 0xF705,
		/// <summary>A unicode private-use-area code point representing the F3 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F3 = 0xF706,
		/// <summary>A unicode private-use-area code point representing the F4 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F4 = 0xF707,
		/// <summary>A unicode private-use-area code point representing the F5 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F5 = 0xF708,
		/// <summary>A unicode private-use-area code point representing the F6 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F6 = 0xF709,
		/// <summary>A unicode private-use-area code point representing the F7 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F7 = 0xF70A,
		/// <summary>A unicode private-use-area code point representing the F8 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F8 = 0xF70B,
		/// <summary>A unicode private-use-area code point representing the F9 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F9 = 0xF70C,
		/// <summary>A unicode private-use-area code point representing the F10 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F10 = 0xF70D,
		/// <summary>A unicode private-use-area code point representing the F11 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F11 = 0xF70E,
		/// <summary>A unicode private-use-area code point representing the F12 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F12 = 0xF70F,
		/// <summary>A unicode private-use-area code point representing the F13 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F13 = 0xF710,
		/// <summary>A unicode private-use-area code point representing the F14 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F14 = 0xF711,
		/// <summary>A unicode private-use-area code point representing the F15 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F15 = 0xF712,
		/// <summary>A unicode private-use-area code point representing the F16 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F16 = 0xF713,
		/// <summary>A unicode private-use-area code point representing the F17 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F17 = 0xF714,
		/// <summary>A unicode private-use-area code point representing the F18 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F18 = 0xF715,
		/// <summary>A unicode private-use-area code point representing the F19 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F19 = 0xF716,
		/// <summary>A unicode private-use-area code point representing the F20 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F20 = 0xF717,
		/// <summary>A unicode private-use-area code point representing the F21 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F21 = 0xF718,
		/// <summary>A unicode private-use-area code point representing the F22 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F22 = 0xF719,
		/// <summary>A unicode private-use-area code point representing the F23 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F23 = 0xF71A,
		/// <summary>A unicode private-use-area code point representing the F24 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F24 = 0xF71B,
		/// <summary>A unicode private-use-area code point representing the F25 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F25 = 0xF71C,
		/// <summary>A unicode private-use-area code point representing the F26 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F26 = 0xF71D,
		/// <summary>A unicode private-use-area code point representing the F27 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F27 = 0xF71E,
		/// <summary>A unicode private-use-area code point representing the F28 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F28 = 0xF71F,
		/// <summary>A unicode private-use-area code point representing the F29 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F29 = 0xF720,
		/// <summary>A unicode private-use-area code point representing the F30 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F30 = 0xF721,
		/// <summary>A unicode private-use-area code point representing the F31 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F31 = 0xF722,
		/// <summary>A unicode private-use-area code point representing the F32 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F32 = 0xF723,
		/// <summary>A unicode private-use-area code point representing the F33 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F33 = 0xF724,
		/// <summary>A unicode private-use-area code point representing the F34 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F34 = 0xF725,
		/// <summary>A unicode private-use-area code point representing the F35 function key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		F35 = 0xF726,
		/// <summary>A unicode private-use-area code point representing the Insert key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Insert = 0xF727,
		/// <summary>A unicode private-use-area code point representing the Delete key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Delete = 0xF728,
		/// <summary>A unicode private-use-area code point representing the Home key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Home = 0xF729,
		/// <summary>A unicode private-use-area code point representing the Begin key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Begin = 0xF72A,
		/// <summary>A unicode private-use-area code point representing the End key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		End = 0xF72B,
		/// <summary>A unicode private-use-area code point representing the Page Up key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		PageUp = 0xF72C,
		/// <summary>A unicode private-use-area code point representing the Page Down key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		PageDown = 0xF72D,
		/// <summary>A unicode private-use-area code point representing the Print Screen key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		PrintScreen = 0xF72E,
		/// <summary>A unicode private-use-area code point representing the Scroll Lock key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		ScrollLock = 0xF72F,
		/// <summary>A unicode private-use-area code point representing the Pause key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Pause = 0xF730,
		/// <summary>A unicode private-use-area code point representing the System Request key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		SysReq = 0xF731,
		/// <summary>A unicode private-use-area code point representing the Break key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Break = 0xF732,
		/// <summary>A unicode private-use-area code point representing the Reset key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Reset = 0xF733,
		/// <summary>A unicode private-use-area code point representing the Stop key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Stop = 0xF734,
		/// <summary>A unicode private-use-area code point representing the Menu key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Menu = 0xF735,
		/// <summary>A unicode private-use-area code point representing the User key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		User = 0xF736,
		/// <summary>A unicode private-use-area code point representing the System key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		System = 0xF737,
		/// <summary>A unicode private-use-area code point representing the Print key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Print = 0xF738,
		/// <summary>A unicode private-use-area code point representing the Clear Line key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		ClearLine = 0xF739,
		/// <summary>A unicode private-use-area code point representing the Clear Display key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		ClearDisplay = 0xF73A,
		/// <summary>A unicode private-use-area code point representing the Insert Line key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		InsertLine = 0xF73B,
		/// <summary>A unicode private-use-area code point representing the Delete Line key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		DeleteLine = 0xF73C,
		/// <summary>A unicode private-use-area code point representing the Insert Character key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		InsertChar = 0xF73D,
		/// <summary>A unicode private-use-area code point representing the Delete Character key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		DeleteChar = 0xF73E,
		/// <summary>A unicode private-use-area code point representing the Previous key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Prev = 0xF73F,
		/// <summary>A unicode private-use-area code point representing the Next key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Next = 0xF740,
		/// <summary>A unicode private-use-area code point representing the Select key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Select = 0xF741,
		/// <summary>A unicode private-use-area code point representing the Execute key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Execute = 0xF742,
		/// <summary>A unicode private-use-area code point representing the Undo key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Undo = 0xF743,
		/// <summary>A unicode private-use-area code point representing the Redo key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Redo = 0xF744,
		/// <summary>A unicode private-use-area code point representing the Find key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Find = 0xF745,
		/// <summary>A unicode private-use-area code point representing the Help key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		Help = 0xF746,
		/// <summary>A unicode private-use-area code point representing the Mode Switch key, used when reporting the <see cref="P:AppKit.NSEvent.CharactersIgnoringModifiers" /> for a non-printable key.</summary>
		ModeSwitch = 0xF747,
	}

	/// <summary>Specifies additional information about certain kinds of events; the meaning of each value depends on the event's <see cref="T:AppKit.NSEventType" />.</summary>
	[NoMacCatalyst]
	public enum NSEventSubtype : short {
		/* event subtypes for NSEventTypeAppKitDefined events */
		/// <summary>For an <see cref="F:AppKit.NSEventType.AppKitDefined" /> event, indicates that a window was exposed.</summary>
		WindowExposed = 0,
		/// <summary>For an <see cref="F:AppKit.NSEventType.AppKitDefined" /> event, indicates that the application was activated.</summary>
		ApplicationActivated = 1,
		/// <summary>For an <see cref="F:AppKit.NSEventType.AppKitDefined" /> event, indicates that the application was deactivated.</summary>
		ApplicationDeactivated = 2,
		/// <summary>For an <see cref="F:AppKit.NSEventType.AppKitDefined" /> event, indicates that a window was moved.</summary>
		WindowMoved = 4,
		/// <summary>For an <see cref="F:AppKit.NSEventType.AppKitDefined" /> event, indicates that the screen configuration changed.</summary>
		ScreenChanged = 8,
		/* event subtypes for NSEventTypeSystemDefined events */
		/* the value is repeated from above */
		/// <summary>For a <see cref="F:AppKit.NSEventType.SystemDefined" /> event, indicates that the system is powering off.</summary>
		PowerOff = 1,

		/* event subtypes for mouse events */
		/* the values are repeated from above */
		/// <summary>For a mouse event, indicates a standard mouse event.</summary>
		MouseEvent = 0, /* NX_SUBTYPE_DEFAULT */
		/// <summary>For a mouse event, indicates that the event originated from a graphics tablet stylus point.</summary>
		TabletPoint = 1, /* NX_SUBTYPE_TABLET_POINT */
		/// <summary>For a mouse event, indicates that the event originated from a graphics tablet proximity change.</summary>
		TabletProximity = 2, /* NX_SUBTYPE_TABLET_PROXIMITY */
		/// <summary>For a mouse event, indicates that the event originated from a touch on a touch-sensitive surface.</summary>
		Touch = 3, /* NX_SUBTYPE_MOUSE_TOUCH */
	}

	#endregion

	#region NSView
	/// <summary>Specifies a bitmask of options that describe how a view resizes relative to its superview.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSViewResizingMask : ulong {
		/// <summary>The view does not resize with its superview.</summary>
		NotSizable = 0,
		/// <summary>The left margin between the view and its superview can grow or shrink.</summary>
		MinXMargin = 1,
		/// <summary>The view's width can grow or shrink.</summary>
		WidthSizable = 2,
		/// <summary>The right margin between the view and its superview can grow or shrink.</summary>
		MaxXMargin = 4,
		/// <summary>The bottom margin between the view and its superview can grow or shrink.</summary>
		MinYMargin = 8,
		/// <summary>The view's height can grow or shrink.</summary>
		HeightSizable = 16,
		/// <summary>The top margin between the view and its superview can grow or shrink.</summary>
		MaxYMargin = 32,
	}

	/// <summary>Specifies the style of border drawn around a view.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSBorderType : ulong {
		/// <summary>No border is drawn.</summary>
		NoBorder,
		/// <summary>A line border is drawn.</summary>
		LineBorder,
		/// <summary>A bezel border is drawn.</summary>
		BezelBorder,
		/// <summary>A groove border is drawn.</summary>
		GrooveBorder,
	}

	/// <summary>Specifies the shape of a text field's bezel.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTextFieldBezelStyle : ulong {
		/// <summary>A square bezel.</summary>
		Square,
		/// <summary>A rounded bezel.</summary>
		Rounded,
	}

	/// <summary>Specifies when a view's backing layer redraws its contents in response to the view being resized.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSViewLayerContentsRedrawPolicy : long {
		/// <summary>The layer never redraws its contents automatically; the view is responsible for updating it.</summary>
		Never,
		/// <summary>The layer redraws its contents only when the view calls <c>SetNeedsDisplay</c>.</summary>
		OnSetNeedsDisplay,
		/// <summary>The layer redraws its contents continuously while the view is being resized.</summary>
		DuringViewResize,
		/// <summary>The layer redraws its contents once, immediately before the view is resized.</summary>
		BeforeViewResize,
		/// <summary>The layer redraws its contents once, immediately after the view is resized, and cross-fades from the previous contents.</summary>
		Crossfade = 4,
	}

	/// <summary>Specifies how a view's backing layer positions its contents within the layer's bounds when the layer's <see cref="T:AppKit.NSViewLayerContentsRedrawPolicy" /> does not redraw on every resize.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSViewLayerContentsPlacement : long {
		/// <summary>Scales the contents independently along each axis to exactly fill the layer's bounds.</summary>
		ScaleAxesIndependently,
		/// <summary>Scales the contents proportionally so the entire contents fit within the layer's bounds.</summary>
		ScaleProportionallyToFit,
		/// <summary>Scales the contents proportionally so they completely fill the layer's bounds, cropping if necessary.</summary>
		ScaleProportionallyToFill,
		/// <summary>Centers the contents within the layer's bounds, without scaling.</summary>
		Center,
		/// <summary>Aligns the contents to the top edge of the layer's bounds, without scaling.</summary>
		Top,
		/// <summary>Aligns the contents to the top-right corner of the layer's bounds, without scaling.</summary>
		TopRight,
		/// <summary>Aligns the contents to the right edge of the layer's bounds, without scaling.</summary>
		Right,
		/// <summary>Aligns the contents to the bottom-right corner of the layer's bounds, without scaling.</summary>
		BottomRight,
		/// <summary>Aligns the contents to the bottom edge of the layer's bounds, without scaling.</summary>
		Bottom,
		/// <summary>Aligns the contents to the bottom-left corner of the layer's bounds, without scaling.</summary>
		BottomLeft,
		/// <summary>Aligns the contents to the left edge of the layer's bounds, without scaling.</summary>
		Left,
		/// <summary>Aligns the contents to the top-left corner of the layer's bounds, without scaling.</summary>
		TopLeft,
	}

	#endregion

	#region NSWindow
	/// <summary>Specifies a bitmask of stylistic and behavioral traits for a window's title bar, borders, and behavior.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native ("NSWindowStyleMask")]
	public enum NSWindowStyle : ulong {
		/// <summary>The window has no title bar and no border.</summary>
		Borderless = 0 << 0,
		/// <summary>The window displays a title bar.</summary>
		Titled = 1 << 0,
		/// <summary>The window displays a close button in its title bar.</summary>
		Closable = 1 << 1,
		/// <summary>The window displays a minimize button in its title bar.</summary>
		Miniaturizable = 1 << 2,
		/// <summary>The window can be resized by the user.</summary>
		Resizable = 1 << 3,
		/// <summary>The window is drawn using the "utility" panel appearance (a thinner title bar), typically used for auxiliary windows.</summary>
		Utility = 1 << 4,
		/// <summary>The panel behaves like a document-modal sheet.</summary>
		DocModal = 1 << 6,
		/// <summary>The panel does not activate the application when it is clicked, and it does not become the key window.</summary>
		NonactivatingPanel = 1 << 7,
		/// <summary>The window has a textured background, such as the appearance historically used by metal-style windows.</summary>
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Don't use 'TexturedBackground' anymore.")]
		TexturedBackground = 1 << 8,
		/// <summary>The title bar and toolbar are drawn as a single, unified area.</summary>
		UnifiedTitleAndToolbar = 1 << 12,
		/// <summary>The window is drawn using the dark, translucent heads-up display (HUD) panel appearance.</summary>
		Hud = 1 << 13,
		/// <summary>The window is currently participating in full-screen mode.</summary>
		FullScreenWindow = 1 << 14,
		/// <summary>The window's content view extends to fill the entire window frame, including the area behind the title bar.</summary>
		FullSizeContentView = 1 << 15,
	}

	/// <summary>Specifies whether, and how, a window's contents can be read by other processes, such as screen-recording or accessibility clients.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSWindowSharingType : ulong {
		/// <summary>The window's contents cannot be read by another process.</summary>
		None,
		/// <summary>The window's contents can be read, but not modified, by another process.</summary>
		ReadOnly,
		/// <summary>The window's contents can be both read and modified by another process.</summary>
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'ReadOnly' instead.")]
		ReadWrite,
	}

	/// <summary>Specifies where a window's backing store is located.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSWindowBackingLocation : ulong {
		/// <summary>The system determines the backing store location.</summary>
		Default,
		/// <summary>The backing store is located in video memory.</summary>
		VideoMemory,
		/// <summary>The backing store is located in main memory.</summary>
		MainMemory,
	}

	/// <summary>Specifies a bitmask that describes how a window behaves with respect to Spaces and Exposé/Mission Control.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowCollectionBehavior : ulong {
		/// <summary>The window uses the default Spaces and Exposé behavior.</summary>
		Default = 0,
		/// <summary>The window can be visible on every Space simultaneously.</summary>
		CanJoinAllSpaces = 1 << 0,
		/// <summary>The window moves to the currently active Space when it is ordered to the front.</summary>
		MoveToActiveSpace = 1 << 1,
		/// <summary>The window participates in Spaces and Exposé, and can be managed like an ordinary application window.</summary>
		Managed = 1 << 2,
		/// <summary>The window participates in Exposé and Spaces the same way as a transient window, such as a menu or panel, and is not affected by Exposé.</summary>
		Transient = 1 << 3,
		/// <summary>The window is treated like a desktop icon and does not move to a different Space, similar to the Finder desktop.</summary>
		Stationary = 1 << 4,
		/// <summary>The window's minimized representation participates in the Cmd-`/Cmd-Shift-` window-cycling order.</summary>
		ParticipatesInCycle = 1 << 5,
		/// <summary>The window does not participate in the Cmd-`/Cmd-Shift-` window-cycling order.</summary>
		IgnoresCycle = 1 << 6,
		/// <summary>The window is the primary content window in a full-screen session.</summary>
		FullScreenPrimary = 1 << 7,
		/// <summary>The window can appear alongside a full-screen window as an auxiliary, such as a panel or inspector.</summary>
		FullScreenAuxiliary = 1 << 8,
		/// <summary>The window does not support full-screen mode.</summary>
		FullScreenNone = 1 << 9,
		/// <summary>The window supports being tiled side-by-side with another window in full-screen mode.</summary>
		FullScreenAllowsTiling = 1 << 11,
		/// <summary>The window does not support being tiled side-by-side with another window in full-screen mode.</summary>
		FullScreenDisallowsTiling = 1 << 12,
		/// <summary>The window is a primary window in a Stage Manager-style set, and other secondary windows can be grouped with it.</summary>
		Primary = 1 << 16,
		/// <summary>The window is an auxiliary window that is grouped together with a primary window.</summary>
		Auxiliary = 1 << 17,
		/// <summary>The window can join a set of windows that spans multiple applications.</summary>
		CanJoinAllApplications = 1 << 18,
	}

	/// <summary>Specifies which windows to include in a window-number list.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowNumberListOptions : ulong {
		/// <summary>Includes windows from all applications, instead of only the calling application.</summary>
		AllApplication = 1 << 0,
		/// <summary>Includes windows from all spaces, instead of only the active space.</summary>
		AllSpaces = 1 << 4,
	}

	/// <summary>Specifies the direction in which a window traverses its key-view loop.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSSelectionDirection : ulong {
		/// <summary>Indicates that the window is not traversing the key-view loop.</summary>
		Direct = 0,
		/// <summary>Proceeds to the next valid key view.</summary>
		Next,
		/// <summary>Proceeds to the previous valid key view.</summary>
		Previous,
	}

	/// <summary>Specifies one of the standard buttons that can appear in a window's title bar.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSWindowButton : ulong {
		/// <summary>The button that closes the window.</summary>
		CloseButton,
		/// <summary>The button that minimizes the window to the Dock.</summary>
		MiniaturizeButton,
		/// <summary>The button that zooms the window between its standard and user-defined sizes.</summary>
		ZoomButton,
		/// <summary>The button used to toggle the visibility of the window's toolbar.</summary>
		ToolbarButton,
		/// <summary>The button that displays the document icon in the title bar.</summary>
		DocumentIconButton,
		/// <summary>The button that displays the document's version history.</summary>
		DocumentVersionsButton = 6,
		/// <summary>The button that toggles full-screen mode.</summary>
		[Deprecated (PlatformName.MacOSX, 10, 12, message: "The standard window button for FullScreenButton is always null; use ZoomButton instead.")]
		FullScreenButton,
	}

	/// <summary>Specifies a bitmask describing the phase of a touch during a multi-touch tracking sequence.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTouchPhase : ulong {
		/// <summary>The touch has just begun.</summary>
		Began = 1 << 0,
		/// <summary>The position of the touch has changed.</summary>
		Moved = 1 << 1,
		/// <summary>The touch has not moved since the previous event.</summary>
		Stationary = 1 << 2,
		/// <summary>The touch has ended.</summary>
		Ended = 1 << 3,
		/// <summary>The touch was cancelled and did not end normally.</summary>
		Cancelled = 1 << 4,

		/// <summary>A combination of the phases that indicate an ongoing touch: <see cref="F:AppKit.NSTouchPhase.Began" />, <see cref="F:AppKit.NSTouchPhase.Moved" />, and <see cref="F:AppKit.NSTouchPhase.Stationary" />.</summary>
		Touching = Began | Moved | Stationary,
		/// <summary>Matches every touch phase.</summary>
		Any = unchecked((ulong) UInt64.MaxValue),
	}
	#endregion
	#region NSAnimation

	/// <summary>Specifies the timing curve of an animation.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSAnimationCurve : ulong {
		/// <summary>The animation accelerates at the beginning and decelerates at the end.</summary>
		EaseInOut,
		/// <summary>The animation accelerates from a slow start.</summary>
		EaseIn,
		/// <summary>The animation decelerates toward the end.</summary>
		EaseOut,
		/// <summary>The animation proceeds at a constant rate.</summary>
		Linear,
	};

	/// <summary>Specifies how an animation runs relative to the application.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSAnimationBlockingMode : ulong {
		/// <summary>Runs the animation synchronously and blocks user interaction until it completes.</summary>
		Blocking,
		/// <summary>Runs the animation asynchronously on the main thread while allowing user interaction.</summary>
		Nonblocking,
		/// <summary>Runs the animation asynchronously on a separate thread.</summary>
		NonblockingThreaded,
	};
	#endregion

	#region NSBox

	/// <summary>Specifies where a box's title is positioned relative to its border.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTitlePosition : ulong {
		/// <summary>No title is displayed.</summary>
		NoTitle,
		/// <summary>The title is centered above the box's top border.</summary>
		AboveTop,
		/// <summary>The title is centered on the box's top border.</summary>
		AtTop,
		/// <summary>The title is centered just below the box's top border, inside the box.</summary>
		BelowTop,
		/// <summary>The title is centered just above the box's bottom border, inside the box.</summary>
		AboveBottom,
		/// <summary>The title is centered on the box's bottom border.</summary>
		AtBottom,
		/// <summary>The title is centered below the box's bottom border.</summary>
		BelowBottom,
	};

	/// <summary>Specifies the visual style of an <see cref="T:AppKit.NSBox" />.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSBoxType : ulong {
		/// <summary>A box with a simple border and an optional title, used as a general-purpose grouping box.</summary>
		NSBoxPrimary,
		/// <summary>A box style identical to <see cref="F:AppKit.NSBoxType.NSBoxPrimary" />.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 15, message: "Identical to 'NSBoxPrimary'.")]
		NSBoxSecondary,
		/// <summary>A box drawn as a thin separator line, typically used to divide sections of a user interface.</summary>
		NSBoxSeparator,
		/// <summary>A box drawn with the appearance used in older versions of macOS.</summary>
		[Obsoleted (PlatformName.MacOSX, 10, 15, message: "'NSBoxOldStyle' is discouraged. Use 'NSBoxPrimary' or 'NSBoxCustom'.")]
		NSBoxOldStyle,
		/// <summary>A box whose border, background, and title appearance can be fully customized.</summary>
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
		[Mac (20, 0)]
		Glass = 16,
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
	/// <summary>Specifies the color depth of a window's backing store.</summary>
	[NoMacCatalyst]
	// NSGraphics.h:typedef int NSWindowDepth;
	public enum NSWindowDepth : int {
		/// <summary>A 24-bit RGB color depth.</summary>
		TwentyfourBitRgb = 0x208,
		/// <summary>A 64-bit RGB color depth.</summary>
		SixtyfourBitRgb = 0x210,
		/// <summary>A 128-bit RGB color depth.</summary>
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

	/// <summary>Specifies an animation effect to display at a screen location.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSAnimationEffect : ulong {
		/// <summary>The default effect for a disappearing item.</summary>
		DisappearingItemDefault = 0,
#if !XAMCORE_5_0
		/// <summary>The obsolete, misspelled name for <see cref="DisappearingItemDefault" />.</summary>
		[Obsolete ("Use 'DisappearingItemDefault' instead.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		DissapearingItemDefault = DisappearingItemDefault,
#endif
		/// <summary>A poof animation effect.</summary>
		EffectPoof = 10,
	}
	#endregion

	#region NSMatrix
	/// <summary>Specifies how a matrix tracks and selects its cells.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSMatrixMode : ulong {
		/// <summary>Allows a single cell to be selected at a time.</summary>
		Radio,
		/// <summary>Highlights a cell while asking it to track the mouse.</summary>
		Highlight,
		/// <summary>Highlights cells without asking them to track the mouse.</summary>
		List,
		/// <summary>Allows individual cells to track the mouse.</summary>
		Track,
	}
	#endregion

	#region NSBrowser
	/// <summary>Specifies how browser columns can be resized.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSBrowserColumnResizingType : ulong {
		/// <summary>Prevents columns from being resized.</summary>
		None,
		/// <summary>Automatically resizes columns.</summary>
		Auto,
		/// <summary>Allows the user to resize columns.</summary>
		User,
	}

	/// <summary>Specifies where a browser accepts a drag-and-drop operation.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSBrowserDropOperation : ulong {
		/// <summary>The drop occurs on an item.</summary>
		On,
		/// <summary>The drop occurs above an item.</summary>
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

	/// <summary>Specifies the shape of the endpoints of an open path.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSLineCapStyle : ulong {
		/// <summary>Ends the path at the endpoint.</summary>
		Butt,
		/// <summary>Uses a semicircular cap centered on the endpoint.</summary>
		Round,
		/// <summary>Uses a square cap that extends beyond the endpoint.</summary>
		Square,
	}

	/// <summary>Specifies the shape used to join connected path segments.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSLineJoinStyle : ulong {
		/// <summary>Joins segments by extending their outer edges until they meet.</summary>
		Miter,
		/// <summary>Joins segments with a rounded corner.</summary>
		Round,
		/// <summary>Joins segments with a beveled corner.</summary>
		Bevel,
	}

	/// <summary>Specifies the rule used to determine which areas of a path are filled.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSWindingRule : ulong {
		/// <summary>Fills areas using the nonzero winding rule.</summary>
		NonZero,
		/// <summary>Fills areas using the even-odd winding rule.</summary>
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
		QuadraticCurveTo,
	}
	#endregion

	#region NSRulerView
	/// <summary>Specifies the orientation of a ruler view.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSRulerOrientation : ulong {
		/// <summary>A horizontal ruler.</summary>
		Horizontal,
		/// <summary>A vertical ruler.</summary>
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
	/// <summary>Specifies the axis along which user interface elements are laid out.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSUserInterfaceLayoutOrientation : long {
		/// <summary>Elements are laid out along the horizontal axis.</summary>
		Horizontal = 0,
		/// <summary>Elements are laid out along the vertical axis.</summary>
		Vertical = 1,
	}

	// NSStackView.h:typedef float NSStackViewVisibilityPriority
	/// <summary>Provides predefined visibility-priority values that can be cast to <see cref="float"/> for use with <see cref="NSStackView"/> APIs.</summary>
	[NoMacCatalyst]
	public enum NSStackViewVisibilityPriority : int {
		/// <summary>The view must remain attached to the stack view.</summary>
		MustHold = 1000,
		/// <summary>The view is detached only when necessary.</summary>
		DetachOnlyIfNecessary = 900,
		/// <summary>The view is not visible.</summary>
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

	/// <summary>Specifies the direction associated with a selection at a line boundary.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSSelectionAffinity : ulong {
		/// <summary>The selection is associated with the preceding line.</summary>
		Upstream,
		/// <summary>The selection is associated with the following line.</summary>
		Downstream,
	}

	/// <summary>Specifies the unit used when modifying a text selection.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSSelectionGranularity : ulong {
		/// <summary>Selects text by character.</summary>
		Character,
		/// <summary>Selects text by word.</summary>
		Word,
		/// <summary>Selects text by paragraph.</summary>
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

	/// <summary>Specifies the direction in which text layout sweeps across a line.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSLineSweepDirection : ulong {
		/// <summary>Sweeps toward the left.</summary>
		NSLineSweepLeft,
		/// <summary>Sweeps toward the right.</summary>
		NSLineSweepRight,
		/// <summary>Sweeps downward.</summary>
		NSLineSweepDown,
		/// <summary>Sweeps upward.</summary>
		NSLineSweepUp,
	}

	/// <summary>Specifies the direction in which text layout moves between lines.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSLineMovementDirection : ulong {
		/// <summary>Does not move between lines.</summary>
		None,
		/// <summary>Moves to the left.</summary>
		Left,
		/// <summary>Moves to the right.</summary>
		Right,
		/// <summary>Moves downward.</summary>
		Down,
		/// <summary>Moves upward.</summary>
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

	/// <summary>Specifies the orientation of printed pages.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSPrintingOrientation : ulong {
		/// <summary>Prints pages in portrait orientation.</summary>
		Portrait,
		/// <summary>Prints pages in landscape orientation.</summary>
		Landscape,
	}

	/// <summary>Specifies how printed content is paginated along one axis.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSPrintingPaginationMode : ulong {
		/// <summary>Divides the content into equal-sized page rectangles.</summary>
		Auto,
		/// <summary>Scales the content to produce one row or column of pages.</summary>
		Fit,
		/// <summary>Clips the content to produce one row or column of pages.</summary>
		Clip,
	}

	/// <summary>Specifies the status of a table in a printer's PostScript Printer Description.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSPrinterTableStatus : ulong {
		/// <summary>The table is available.</summary>
		Ok,
		/// <summary>The table was not found.</summary>
		NotFound,
		/// <summary>An error occurred while accessing the table.</summary>
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

	/// <summary>Specifies the order in which pages are printed.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSPrintingPageOrder : long {
		/// <summary>Prints pages from front to back.</summary>
		Descending = -1,
		/// <summary>Prints pages in the order received by the spooler without rearranging them.</summary>
		Special,
		/// <summary>Prints pages from back to front.</summary>
		Ascending,
		/// <summary>The page order is unknown.</summary>
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

	/// <summary>Specifies how a text block value is interpreted.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockValueType : ulong {
		/// <summary>The value is an absolute number of points.</summary>
		Absolute,
		/// <summary>The value is a percentage of the containing text block.</summary>
		Percentage,
	}

	/// <summary>Identifies a dimension of a text block.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockDimension : ulong {
		/// <summary>The text block's width.</summary>
		Width = 0,
		/// <summary>The minimum width of the text block.</summary>
		MinimumWidth = 1,
		/// <summary>The maximum width of the text block.</summary>
		MaximumWidth = 2,
		/// <summary>The text block's height.</summary>
		Height = 4,
		/// <summary>The minimum height of the text block.</summary>
		MinimumHeight = 5,
		/// <summary>The maximum height of the text block.</summary>
		MaximumHeight = 6,
	}

	/// <summary>Identifies a layer of a text block.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockLayer : long {
		/// <summary>The padding layer between the text and the border.</summary>
		Padding = -1,
		/// <summary>The border layer surrounding the padding.</summary>
		Border,
		/// <summary>The margin layer outside the border.</summary>
		Margin,
	}

	/// <summary>Specifies the vertical alignment of content in a text block.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTextBlockVerticalAlignment : ulong {
		/// <summary>Aligns content with the top of the text block.</summary>
		Top,
		/// <summary>Centers content vertically in the text block.</summary>
		Middle,
		/// <summary>Aligns content with the bottom of the text block.</summary>
		Bottom,
		/// <summary>Aligns content with the text block's baseline.</summary>
		Baseline,
	}

	/// <summary>Specifies how a text table calculates column widths.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTextTableLayoutAlgorithm : ulong {
		/// <summary>Calculates column widths based on the table's contents.</summary>
		Automatic,
		/// <summary>Uses the table's specified column widths without considering its contents.</summary>
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

	/// <summary>Specifies options for writing objects to a pasteboard.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSPasteboardWritingOptions : ulong {
		/// <summary>The object promises to provide its pasteboard data later.</summary>
		WritingPromised = 1 << 9,
	}

	/// <summary>Specifies how a toolbar displays its items.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSToolbarDisplayMode : ulong {
		/// <summary>Uses the toolbar's default display mode.</summary>
		Default,
		/// <summary>Displays both the icon and label for each toolbar item.</summary>
		IconAndLabel,
		/// <summary>Displays only the icon for each toolbar item.</summary>
		Icon,
		/// <summary>Displays only the label for each toolbar item.</summary>
		Label,
	}

	/// <summary>Specifies the size of toolbar items.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSToolbarSizeMode : ulong {
		/// <summary>Uses the toolbar's default item size.</summary>
		Default,
		/// <summary>Uses regular-sized toolbar items.</summary>
		Regular,
		/// <summary>Uses small toolbar items.</summary>
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

	/// <summary>Specifies how a table view highlights selected rows.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTableViewSelectionHighlightStyle : long {
		/// <summary>Does not highlight selected rows.</summary>
		None = -1,
		/// <summary>Uses the regular selection highlight style.</summary>
		Regular = 0,
		/// <summary>Uses the source-list selection highlight style.</summary>
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

	/// <summary>Specifies where a table view performs a drop operation.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTableViewDropOperation : ulong {
		/// <summary>Performs the drop on the specified row.</summary>
		On,
		/// <summary>Performs the drop above the specified row.</summary>
		Above,
	}

	/// <summary>Specifies how a table column can be resized.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTableColumnResizing : long {
		/// <summary>Represents -1, which sets all table column resizing option bits.</summary>
		None = -1,
		/// <summary>Allows the table view to resize the column automatically.</summary>
		Autoresizing = (1 << 0),
		/// <summary>Allows the user to resize the column.</summary>
		UserResizingMask = (1 << 1),
	}

	/// <summary>Specifies which grid lines a table view draws.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSTableViewGridStyle : ulong {
		/// <summary>Does not draw grid lines.</summary>
		None = 0,
		/// <summary>Draws solid vertical grid lines.</summary>
		SolidVerticalLine = 1 << 0,
		/// <summary>Draws solid horizontal grid lines.</summary>
		SolidHorizontalLine = 1 << 1,
		/// <summary>Draws dashed horizontal grid lines.</summary>
		DashedHorizontalGridLine = 1 << 3,
	}

	/// <summary>Specifies how a gradient extends beyond its starting and ending locations.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSGradientDrawingOptions : ulong {
		/// <summary>Does not extend the gradient beyond its starting or ending location.</summary>
		None = 0,
		/// <summary>Extends the gradient before its starting location using the starting color.</summary>
		BeforeStartingLocation = (1 << 0),
		/// <summary>Extends the gradient after its ending location using the ending color.</summary>
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

	/// <summary>Specifies the frame drawn around an image.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSImageFrameStyle : ulong {
		/// <summary>Draws no frame.</summary>
		None = 0,
		/// <summary>Draws a frame suitable for a photograph.</summary>
		Photo,
		/// <summary>Draws a gray bezel frame.</summary>
		GrayBezel,
		/// <summary>Draws a grooved frame.</summary>
		Groove,
		/// <summary>Draws a button-style frame.</summary>
		Button,
	}

	/// <summary>Specifies when a speech synthesizer pauses or stops speaking.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSSpeechBoundary : ulong {
		/// <summary>Pauses or stops speaking immediately.</summary>
		Immediate = 0,
		/// <summary>Pauses or stops speaking at the next word boundary.</summary>
		Word = 1,
		/// <summary>Pauses or stops speaking at the next sentence boundary.</summary>
		Sentence,
	}

	/// <summary>Specifies the visual style of a split view divider.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSSplitViewDividerStyle : long {
		/// <summary>Uses a thick divider.</summary>
		Thick = 1,
		/// <summary>Uses a thin divider.</summary>
		Thin = 2,
		/// <summary>Uses a pane-splitter divider.</summary>
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
		Inspector,
	}

	/// <summary>Specifies how an image is scaled to fit its frame.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSImageScaling : ulong {
		/// <summary>Scales the image down proportionally when it is larger than the frame.</summary>
		ProportionallyDown = 0,
		/// <summary>Scales the image independently along each axis to fill the frame.</summary>
		AxesIndependently,
		/// <summary>Does not scale the image.</summary>
		None,
		/// <summary>Scales the image proportionally up or down to fit the frame.</summary>
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

	/// <summary>Specifies the shape of a slider.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSSliderType : ulong {
		/// <summary>A horizontal or vertical linear slider.</summary>
		Linear = 0,
		/// <summary>A circular slider.</summary>
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

	/// <summary>Specifies legacy icon representations to exclude when creating a file icon.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWorkspaceIconCreationOptions : ulong {
		/// <summary>Excludes QuickDraw icon representations.</summary>
		NSExcludeQuickDrawElements = 1 << 1,
		/// <summary>Excludes icon representations introduced in macOS 10.4.</summary>
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

	/// <summary>Specifies the visual state of a tab view item.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTabState : ulong {
		/// <summary>The tab is selected.</summary>
		Selected,
		/// <summary>The tab is displayed in the background.</summary>
		Background,
		/// <summary>The tab is being pressed.</summary>
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

	/// <summary>Specifies options for creating or modifying font collections.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSFontCollectionOptions : long {
		/// <summary>Limits the font collection to the current application.</summary>
		ApplicationOnlyMask = 1,
	}

	/// <summary>Specifies whether a drop occurs on an item or before it.</summary>
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
		/// <summary>The drop occurs on the item at the specified index path.</summary>
		On = 0,
		/// <summary>The drop occurs before the item at the specified index path.</summary>
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

	/// <summary>Specifies the category of an element in a collection view layout.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSCollectionElementCategory : long {
		/// <summary>A collection view item.</summary>
		Item,
		/// <summary>A supplementary view.</summary>
		SupplementaryView,
		/// <summary>A decoration view.</summary>
		DecorationView,
		/// <summary>An inter-item gap.</summary>
		InterItemGap,
	}

	/// <summary>Specifies an update applied to a collection view.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSCollectionUpdateAction : long {
		/// <summary>Inserts an item.</summary>
		Insert,
		/// <summary>Deletes an item.</summary>
		Delete,
		/// <summary>Reloads an item.</summary>
		Reload,
		/// <summary>Moves an item.</summary>
		Move,
		/// <summary>No update action.</summary>
		None,
	}

	/// <summary>Specifies the scrolling direction of a collection view layout.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum NSCollectionViewScrollDirection : long {
		/// <summary>The collection view scrolls vertically.</summary>
		Vertical,
		/// <summary>The collection view scrolls horizontally.</summary>
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

	/// <summary>Specifies whether a date picker selects a single date or a date range.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSDatePickerMode : ulong {
		/// <summary>The date picker selects a single date.</summary>
		Single,
		/// <summary>The date picker selects a range of dates.</summary>
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

	/// <summary>Specifies the position of an OpenGL surface relative to its window.</summary>
	[NoMacCatalyst]
	public enum NSSurfaceOrder {
		/// <summary>The surface is displayed above the window.</summary>
		AboveWindow = 1,
		/// <summary>The surface is displayed below the window.</summary>
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

	/// <summary>Identifies which button dismissed an alert.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSAlertButtonReturn : long {
		/// <summary>The first alert button.</summary>
		First = 1000,
		/// <summary>The second alert button.</summary>
		Second = 1001,
		/// <summary>The third alert button.</summary>
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

	/// <summary>Specifies the visual style of a progress indicator.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSProgressIndicatorStyle : ulong {
		/// <summary>Displays progress as a bar.</summary>
		Bar,
		/// <summary>Displays progress as a spinning indicator.</summary>
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

	/// <summary>Specifies whether a rule editor row is a leaf condition or a group of subrows.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSRuleEditorRowType : ulong {
		/// <summary>A leaf row that represents a single condition.</summary>
		Simple = 0,
		/// <summary>A parent row that groups one or more subrows.</summary>
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

	/// <summary>Specifies how a scroller is displayed relative to its content.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSScrollerStyle : long {
		/// <summary>The scroller occupies space reserved for it in the layout.</summary>
		Legacy = 0,
		/// <summary>The scroller is drawn over the content without reserving layout space.</summary>
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

	/// <summary>Specifies options for tracking a swipe gesture.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSEventSwipeTrackingOptions : ulong {
		/// <summary>Clamps the gesture amount to zero when the user reverses the initial swipe direction.</summary>
		LockDirection = 1,
		/// <summary>Clamps the gesture amount to the range from -1.0 through 1.0.</summary>
		ClampGestureAmount = 2,
	}

	/// <summary>Specifies the axis associated with a gesture event.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSEventGestureAxis : long {
		/// <summary>The gesture is not associated with an axis.</summary>
		None,
		/// <summary>The gesture is associated with the horizontal axis.</summary>
		Horizontal,
		/// <summary>The gesture is associated with the vertical axis.</summary>
		Vertical,
	}

	/// <summary>Specifies the axis affected by Auto Layout constraints.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSLayoutConstraintOrientation : long {
		/// <summary>The constraint has a horizontal orientation.</summary>
		Horizontal,
		/// <summary>The constraint has a vertical orientation.</summary>
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

	/// <summary>Specifies the visual appearance of a popover.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSPopoverAppearance : long {
		/// <summary>A minimal popover appearance.</summary>
		Minimal,
		/// <summary>A heads-up display appearance.</summary>
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

	/// <summary>Specifies the edge of a table row where row actions appear.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTableRowActionEdge : long {
		/// <summary>The leading edge in the current user interface layout direction.</summary>
		Leading,
		/// <summary>The trailing edge in the current user interface layout direction.</summary>
		Trailing,
	}

	/// <summary>Specifies whether a table row action is regular or destructive.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTableViewRowActionStyle : long {
		/// <summary>A regular, nondestructive action.</summary>
		Regular,
		/// <summary>An action that performs a destructive operation.</summary>
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

	/// <summary>Specifies whether a dragging operation occurs within or outside the application.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSDraggingContext : long {
		/// <summary>The dragging operation occurs outside the application.</summary>
		OutsideApplication,
		/// <summary>The dragging operation occurs within the application.</summary>
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

	/// <summary>Specifies the scope in which a font collection is visible.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSFontCollectionVisibility : ulong {
		/// <summary>The collection is visible only to the current process and is not persistent.</summary>
		Process = 1 << 0,
		/// <summary>The collection is persisted and visible to all processes for the current user.</summary>
		User = 1 << 1,
		/// <summary>The collection is persisted and visible to all users of the computer.</summary>
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

	/// <summary>Specifies whether a window's title is displayed.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSWindowTitleVisibility : long {
		/// <summary>The window title is displayed.</summary>
		Visible = 0,
		/// <summary>The window title is hidden.</summary>
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

	/// <summary>Describes whether an application has visible content.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSApplicationOcclusionState : ulong {
		/// <summary>At least part of the application's content is visible.</summary>
		Visible = 1 << 1,
	}

	/// <summary>Describes whether a window is visible to the user.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowOcclusionState : ulong {
		/// <summary>At least part of the window is visible.</summary>
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

	/// <summary>Specifies which content a visual effect view blends with.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSVisualEffectBlendingMode : long {
		/// <summary>Blends with content behind the window.</summary>
		BehindWindow,
		/// <summary>Blends with content behind the view in the current window.</summary>
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

	/// <summary>Specifies how to order windows in a window list.</summary>
	[NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSWindowListOptions : long {
		/// <summary>Orders windows from front to back.</summary>
		OrderedFrontToBack = (1 << 0),
	}

	/// <summary>Specifies optional behaviors for a status item.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSStatusItemBehavior : ulong {
		/// <summary>Allows the user to remove the status item from the menu bar.</summary>
		RemovalAllowed = (1 << 1),
		/// <summary>Allows the user to remove the status item and terminates the application when they do so. This option implies <see cref="RemovalAllowed" />.</summary>
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

	/// <summary>Specifies a color gamut that a display can represent.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSDisplayGamut : long {
		/// <summary>The sRGB color gamut.</summary>
		Srgb = 1,
		/// <summary>The Display P3 color gamut.</summary>
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

	/// <summary>Specifies options for replacing the contents of a pasteboard.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSPasteboardContentsOptions : ulong {
		/// <summary>Restricts the pasteboard contents to the current host.</summary>
		CurrentHostOnly = 1,
	}

	/// <summary>Specifies whether contact occurs directly on a display or through an indirect input device.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSTouchType : long {
		/// <summary>Contact occurs directly on a display.</summary>
		Direct,
		/// <summary>Contact occurs on an indirect input device, such as a trackpad.</summary>
		Indirect,
	}

	/// <summary>Specifies the types of touch input to include.</summary>
	[NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSTouchTypeMask : ulong {
		/// <summary>Includes direct touch input.</summary>
		Direct = (1 << (int) NSTouchType.Direct),
		/// <summary>Includes indirect touch input.</summary>
		Indirect = (1 << (int) NSTouchType.Indirect),
	}

	/// <summary>Specifies how a scrubber responds to touch input.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSScrubberMode : long {
		/// <summary>Items remain fixed while the user moves the selection across them.</summary>
		Fixed = 0,
		/// <summary>Items scroll freely in response to the user's touch.</summary>
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

	/// <summary>Specifies the position of an accessibility annotation within a range.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSAccessibilityAnnotationPosition : long {
		/// <summary>The annotation applies to the entire range.</summary>
		FullRange,
		/// <summary>The annotation applies to the start of the range.</summary>
		Start,
		/// <summary>The annotation applies to the end of the range.</summary>
		End,
	}

	/// <summary>Specifies the direction in which an accessibility custom rotor searches.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSAccessibilityCustomRotorSearchDirection : long {
		/// <summary>Searches for the previous item.</summary>
		Previous,
		/// <summary>Searches for the next item.</summary>
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

	/// <summary>Specifies how a color represents its color data.</summary>
	[NoMacCatalyst]
	[Native]
	public enum NSColorType : long {
		/// <summary>A color defined by components in a color space.</summary>
		ComponentBased,
		/// <summary>A color defined by a repeating image pattern.</summary>
		Pattern,
		/// <summary>A color obtained from a named color catalog.</summary>
		Catalog,
	}

	/// <summary>Specifies options for requesting downloadable font assets.</summary>
	[NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSFontAssetRequestOptions : ulong {
		/// <summary>Displays the standard user interface while downloading fonts.</summary>
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

	[NoMacCatalyst]
	[Native]
	public enum NSColorWellStyle : long {
		Default = 0,
		Minimal,
		Expanded,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSComboButtonStyle : long {
		Split = 0,
		Unified = 1,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSImageDynamicRange : long {
		Unspecified = -1,
		Standard = 0,
		ConstrainedHigh = 1,
		High = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSMenuItemBadgeType : long {
		None = 0,
		Updates,
		NewItems,
		Alerts,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSMenuPresentationStyle : long {
		Regular = 0,
		Palette = 1,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSMenuSelectionMode : long {
		Automatic = 0,
		SelectOne = 1,
		SelectAny = 2,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPageLayoutResult : long {
		Cancelled = 0,
		Changed,
	}

	[NoMacCatalyst]
	[Native]
	public enum NSPrintPanelResult : long {
		Cancelled = 0,
		Printed,
	}

	[NoMacCatalyst]
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

	[NoMacCatalyst]
	[Native]
	[Flags]
	public enum NSTextInsertionIndicatorAutomaticModeOptions : long {
		EffectsView = 1L << 0,
		WhileTracking = 1L << 1,
	}

	[NoMacCatalyst]
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

	[MacCatalyst (26, 0), Mac (26, 0)]
	[Native]
	public enum NSToolbarItemStyle : long {
		Plain,
		Prominent,
	}

	[NoMacCatalyst, Mac (26, 0)]
	[Native]
	public enum NSImageSymbolColorRenderingMode : long {
		Automatic = 0,
		Flat,
		Gradient,
	}

	[NoMacCatalyst, Mac (26, 0)]
	[Native]
	public enum NSImageSymbolVariableValueMode : long {
		Automatic = 0,
		Color,
		Draw,
	}

	[NoMacCatalyst, Mac (26, 0)]
	[Native]
	public enum NSTintProminence : long {
		Automatic = 0,
		None,
		Primary,
		Secondary,
	}

	[NoMacCatalyst, Mac (26, 0)]
	[Native]
	public enum NSControlBorderShape : long {
		Automatic,
		Capsule,
		RoundedRectangle,
		Circle,
	}
}
