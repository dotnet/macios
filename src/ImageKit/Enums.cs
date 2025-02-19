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
// Enums.cs: Enums for ImageKit
//

#nullable enable

using System;

using ObjCRuntime;

namespace ImageKit {

	[Native]
	public enum IKCameraDeviceViewDisplayMode : long {
		None = -1,
		/// <summary>To be added.</summary>
		Table = 0,
		/// <summary>To be added.</summary>
		Icon = 1,
	};

	[Native]
	public enum IKCameraDeviceViewTransferMode : long {
		/// <summary>To be added.</summary>
		File = 0,
		/// <summary>To be added.</summary>
		Memory = 1,
	};

	[Native]
	public enum IKDeviceBrowserViewDisplayMode : long {
		/// <summary>To be added.</summary>
		Table = 0,
		/// <summary>To be added.</summary>
		Outline = 1,
		/// <summary>To be added.</summary>
		Icon = 2,
	};

	// Untyped enum in ObjC
	public enum IKImageBrowserCellState : int {
		/// <summary>To be added.</summary>
		NoImage = 0,
		/// <summary>To be added.</summary>
		Invalid = 1,
		/// <summary>To be added.</summary>
		Ready = 2,
	};

	[Flags]
	[Native]
	public enum IKCellsStyle : ulong {
		/// <summary>To be added.</summary>
		None = 0,
		/// <summary>To be added.</summary>
		Shadowed = 1 << 0,
		/// <summary>To be added.</summary>
		Outlined = 1 << 1,
		/// <summary>To be added.</summary>
		Titled = 1 << 2,
		/// <summary>To be added.</summary>
		Subtitled = 1 << 3,
	};

	//used as a value for the IKImageBrowserGroupStyleKey in the NSDictionary that defines a group in IKImageBrowserView
	[Native]
	public enum IKGroupStyle : long {
		/// <summary>To be added.</summary>
		Bezel = 0,
		/// <summary>To be added.</summary>
		Disclosure = 1,
	};

	// Untyped enum in ObjC
	public enum IKImageBrowserDropOperation : int {
		/// <summary>To be added.</summary>
		On = 0,
		/// <summary>To be added.</summary>
		Before = 1,
	};

	[Native]
	public enum IKScannerDeviceViewTransferMode : long {
		/// <summary>To be added.</summary>
		File = 0,
		/// <summary>To be added.</summary>
		Memory = 1,
	};

	[Native]
	public enum IKScannerDeviceViewDisplayMode : long {
		None = -1,
		/// <summary>To be added.</summary>
		Simple = 0,
		/// <summary>To be added.</summary>
		Advanced = 1,
	};

	[Flags]
	public enum IKFilterBrowserPanelStyleMask : uint {
		/// <summary>To be added.</summary>
		Normal = 0,
		/// <summary>To be added.</summary>
		Textured = 1 << 8,
		// Other NSWindow Style Mask bit settings do not apply to this panel
	}
}
