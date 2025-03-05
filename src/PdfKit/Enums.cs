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
// Enums.cs: Enums for PdfKit
//
using System;

using ObjCRuntime;

#nullable enable

#if MONOMAC || IOS || TVOS

namespace PdfKit {

	[Native]
	[TV (18, 2)]
	public enum PdfActionNamedName : long {
		/// <summary>Indicates an action that has no name, or is not yet named.</summary>
		None = 0,
		/// <summary>Indicates an action that goes to the next page in a document.</summary>
		NextPage = 1,
		/// <summary>Indicates an action that goes to the previous page in a document.</summary>
		PreviousPage = 2,
		/// <summary>Indicates an action that goes to the first page of the document.</summary>
		FirstPage = 3,
		/// <summary>Indicates an action that goes to the last page of a document.</summary>
		LastPage = 4,
		/// <summary>Indicates an action that goes back one location in a navigation list.</summary>
		GoBack = 5,
		/// <summary>Indicates an action that goes forward one location in a navigation list.</summary>
		GoForward = 6,
		/// <summary>Indicates an action that goes to a specific page in a document.</summary>
		GoToPage = 7,
		/// <summary>Indicates the Find action.</summary>
		Find = 8,
		/// <summary>Indicates an action that prints a document.</summary>
		Print = 9,
		/// <summary>Indicates an action that zooms in.</summary>
		ZoomIn = 10,
		/// <summary>Indicates an action that zooms out.</summary>
		ZoomOut = 11,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfWidgetControlType : long {
		Unknown = -1,
		PushButton = 0,
		RadioButton = 1,
		CheckBox = 2,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfLineStyle : long {
		/// <summary>Indicates no line ending glyph.</summary>
		None = 0,
		/// <summary>Indicates a solid square line ending glyph.</summary>
		Square = 1,
		/// <summary>Indicates a filled circle line ending glyph.</summary>
		Circle = 2,
		/// <summary>Indicates a solid diamond line ending glyph.</summary>
		Diamond = 3,
		/// <summary>Indicates an open arrow line ending glyph.</summary>
		OpenArrow = 4,
		/// <summary>Indicates a solid triangular line ending glyph.</summary>
		ClosedArrow = 5,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfMarkupType : long {
		/// <summary>Indicates a highlight markup.</summary>
		Highlight = 0,
		/// <summary>Indicates a strikethrough markup.</summary>
		StrikeOut = 1,
		/// <summary>Indicates an underline markup.</summary>
		Underline = 2,
		Redact = 3,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfTextAnnotationIconType : long {
		Comment = 0,
		Key = 1,
		Note = 2,
		Help = 3,
		NewParagraph = 4,
		Paragraph = 5,
		Insert = 6,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfBorderStyle : long {
		/// <summary>Indicates a solid line.</summary>
		Solid = 0,
		/// <summary>Indicates a dashed effect.</summary>
		Dashed = 1,
		/// <summary>Indicates a beveled effect.</summary>
		Beveled = 2,
		/// <summary>Indicates an inset effect.</summary>
		Inset = 3,
		/// <summary>Indicates the annotation will be underlined.</summary>
		Underline = 4,
	}

	/// <summary>Enumerates print scaling behaviors.</summary>
	[NoiOS]
	[NoTV]
	[Unavailable (PlatformName.MacCatalyst)]
	[Native]
	public enum PdfPrintScalingMode : long {
		None = 0,
		ToFit = 1,
		DownToFit = 2,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfDocumentPermissions : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		User = 1,
		/// <summary>To be added.</summary>
		Owner = 2,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfDisplayBox : long {
		/// <summary>Indicates a rectangle around a physical medium in user space.</summary>
		Media = 0,
		/// <summary>Indicates a crop rectangle around the visible portion of the page.</summary>
		Crop = 1,
		/// <summary>Indicates a bleed box for production.</summary>
		Bleed = 2,
		/// <summary>Indicates a box around intended finish boundaries.</summary>
		Trim = 3,
		/// <summary>Indicates a rectangle around the boundaries of a page's content for display.</summary>
		Art = 4,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfDisplayMode : long {
		/// <summary>Indicates that a single page at a time is displayed.</summary>
		SinglePage = 0,
		/// <summary>Indicates that pages are displayed in a single continuously scrolling column.</summary>
		SinglePageContinuous = 1,
		/// <summary>Indicates that two pages are displayed side by side.</summary>
		TwoUp = 2,
		/// <summary>Indicates that pages are displayed in a continuously scrolling double column.</summary>
		TwoUpContinuous = 3,
	}

	[Flags]
	[Native]
	[TV (18, 2)]
	public enum PdfAreaOfInterest : long {
		/// <summary>Indicates that the touch position is over an ordinary area.</summary>
		NoArea = 0,
		/// <summary>Indicates that the touch position is over a page.</summary>
		PageArea = 1 << 0,
		/// <summary>Indicates that the touch position is over a text area.</summary>
		TextArea = 1 << 1,
		/// <summary>Indicates that the touch position is over an annotation.</summary>
		AnnotationArea = 1 << 2,
		/// <summary>Indicates that the touch position is over a link.</summary>
		LinkArea = 1 << 3,
		/// <summary>Indicates that the touch position is over a control.</summary>
		ControlArea = 1 << 4,
		/// <summary>Indicates that the touch position is over a text field.</summary>
		TextFieldArea = 1 << 5,
		/// <summary>Indicates that the touch position is over an icon.</summary>
		IconArea = 1 << 6,
		/// <summary>Indicates that the touch position is over a popup.</summary>
		PopupArea = 1 << 7,
		/// <summary>Indicates that the touch position is over an image.</summary>
		ImageArea = 1 << 8,
		[iOS (15, 0), MacCatalyst (15, 0)]
		AnyArea = Int64.MaxValue,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfDisplayDirection : long {
		/// <summary>To be added.</summary>
		Vertical = 0,
		/// <summary>To be added.</summary>
		Horizontal = 1,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfInterpolationQuality : long {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Low = 1,
		/// <summary>To be added.</summary>
		High = 2,
	}

	[NoMac]
	[Native]
	[TV (18, 2)]
	public enum PdfThumbnailLayoutMode : long {
		Vertical = 0,
		Horizontal = 1,
	}

	[Native]
	[TV (18, 2)]
	public enum PdfWidgetCellState : long {
		Mixed = -1,
		Off = 0,
		On = 1,
	}
}
#endif
