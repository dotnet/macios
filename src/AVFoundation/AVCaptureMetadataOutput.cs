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
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable enable

namespace AVFoundation {

	public partial class AVCaptureMetadataOutput {

		/// <summary>Gets the metadata object types produced by the active <see cref="AVFoundation.AVCaptureInputPort" />.</summary>
		/// <value>The available metadata object types.</value>
		public AVMetadataObjectType AvailableMetadataObjectTypes {
			get { return AVMetadataObjectTypeExtensions.ToFlags (WeakAvailableMetadataObjectTypes); }
		}

		/// <summary>Gets or sets the metadata object types to filter. Only metadata matching these types will be forwarded to the <see cref="AVFoundation.AVCaptureMetadataOutput.Delegate" />.</summary>
		/// <value>
		///   <para tool="nullallowed">This value can be <see langword="null" />.</para>
		/// </value>
		public AVMetadataObjectType MetadataObjectTypes {
			get { return AVMetadataObjectTypeExtensions.ToFlags (WeakMetadataObjectTypes); }
			set { WeakMetadataObjectTypes = value.ToArray (); }
		}

	}
}
#endif
