using System;

using ObjCRuntime;

namespace WebKit {

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	public enum DomCssRuleType : ushort {
		/// <summary>To be added.</summary>
		Unknown = 0,
		/// <summary>To be added.</summary>
		Style = 1,
		/// <summary>To be added.</summary>
		Charset = 2,
		/// <summary>To be added.</summary>
		Import = 3,
		/// <summary>To be added.</summary>
		Media = 4,
		/// <summary>To be added.</summary>
		FontFace = 5,
		/// <summary>To be added.</summary>
		Page = 6,
		/// <summary>To be added.</summary>
		Variables = 7,
		/// <summary>To be added.</summary>
		WebKitKeyFrames = 8,
		/// <summary>To be added.</summary>
		WebKitKeyFrame = 9,
		/// <summary>To be added.</summary>
		NamespaceRule = 10,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	public enum DomCssValueType : ushort {
		/// <summary>To be added.</summary>
		Inherit = 0,
		/// <summary>To be added.</summary>
		PrimitiveValue = 1,
		/// <summary>To be added.</summary>
		ValueList = 2,
		/// <summary>To be added.</summary>
		Custom = 3,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	[Flags]
	public enum DomDocumentPosition : ushort {
		/// <summary>To be added.</summary>
		Disconnected = 0x01,
		/// <summary>To be added.</summary>
		Preceeding = 0x02,
		/// <summary>To be added.</summary>
		Following = 0x04,
		/// <summary>To be added.</summary>
		Contains = 0x08,
		/// <summary>To be added.</summary>
		ContainedBy = 0x10,
		/// <summary>To be added.</summary>
		ImplementationSpecific = 0x20,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	public enum DomNodeType : ushort {
		/// <summary>To be added.</summary>
		Element = 1,
		/// <summary>To be added.</summary>
		Attribute = 2,
		/// <summary>To be added.</summary>
		Text = 3,
		/// <summary>To be added.</summary>
		CData = 4,
		/// <summary>To be added.</summary>
		EntityReference = 5,
		/// <summary>To be added.</summary>
		Entity = 6,
		/// <summary>To be added.</summary>
		ProcessingInstruction = 7,
		/// <summary>To be added.</summary>
		Comment = 8,
		/// <summary>To be added.</summary>
		Document = 9,
		/// <summary>To be added.</summary>
		DocumentType = 10,
		/// <summary>To be added.</summary>
		DocumentFragment = 11,
		/// <summary>To be added.</summary>
		Notation = 12,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	public enum DomRangeCompareHow : ushort {
		/// <summary>To be added.</summary>
		StartToStart = 0,
		/// <summary>To be added.</summary>
		StartToEnd = 1,
		/// <summary>To be added.</summary>
		EndToEnd = 2,
		/// <summary>To be added.</summary>
		EndToStart = 3,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Native]
	public enum WebCacheModel : ulong {
		/// <summary>To be added.</summary>
		DocumentViewer,
		/// <summary>To be added.</summary>
		DocumentBrowser,
		/// <summary>To be added.</summary>
		PrimaryWebBrowser,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	public enum DomEventPhase : ushort {
		/// <summary>To be added.</summary>
		Capturing = 1,
		/// <summary>To be added.</summary>
		AtTarget,
		/// <summary>To be added.</summary>
		Bubbling,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	[Flags]
	public enum WebDragSourceAction : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		DHTML = 1,
		/// <summary>To be added.</summary>
		Image = 2,
		/// <summary>To be added.</summary>
		Link = 4,
		/// <summary>To be added.</summary>
		Selection = 8,
		/// <summary>To be added.</summary>
		Any = UInt64.MaxValue,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	[Flags]
	public enum WebDragDestinationAction : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		DHTML = 1,
		/// <summary>To be added.</summary>
		Image = 2,
		/// <summary>To be added.</summary>
		Link = 4,
		/// <summary>To be added.</summary>
		[Obsolete ("This API is not available on this platform.")]
		Selection = 8,
		/// <summary>To be added.</summary>
		Any = UInt64.MaxValue,
	}

	[NoiOS, NoTV, NoMacCatalyst]
	[Native]
	public enum WebNavigationType : long {
		/// <summary>To be added.</summary>
		LinkClicked,
		/// <summary>To be added.</summary>
		FormSubmitted,
		/// <summary>To be added.</summary>
		BackForward,
		/// <summary>To be added.</summary>
		Reload,
		/// <summary>To be added.</summary>
		FormResubmitted,
		/// <summary>To be added.</summary>
		Other,
	}

	// Used as an 'unsigned int' parameter 
	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	public enum DomKeyLocation : uint {
		/// <summary>To be added.</summary>
		Standard = 0,
		/// <summary>To be added.</summary>
		Left = 1,
		/// <summary>To be added.</summary>
		Right = 2,
		/// <summary>To be added.</summary>
		NumberPad = 3,
	}

	// Used as an 'int' parameter 
	[NoiOS, NoTV, NoMacCatalyst]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "No longer supported.")]
	public enum DomDelta : int {
		/// <summary>To be added.</summary>
		Pixel = 0,
		/// <summary>To be added.</summary>
		Line = 1,
		/// <summary>To be added.</summary>
		Page = 2,
	}
}
