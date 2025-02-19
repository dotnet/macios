// 
// AVCaptureMetadataOutput.cs:
//     
// Copyright 2014-2015 Xamarin Inc.
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

#if IOS
using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace AVFoundation {

	public partial class AVCaptureMetadataOutput {

		/// <summary>Keys for the metadata types produced by the active <see cref="T:AVFoundation.AVCaptureInputPort" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public AVMetadataObjectType AvailableMetadataObjectTypes {
			get { return AVMetadataObjectTypeExtensions.ToFlags (WeakAvailableMetadataObjectTypes); }
		}

		/// <summary>A filter of metadata keys. Only metadata whose keys are in this array will be forwarded to the <see cref="P:AVFoundation.AVCaptureMetadataOutput.Delegate" /></summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		public AVMetadataObjectType MetadataObjectTypes {
			get { return AVMetadataObjectTypeExtensions.ToFlags (WeakMetadataObjectTypes); }
			set { WeakMetadataObjectTypes = value.ToArray (); }
		}

	}
}
#endif
