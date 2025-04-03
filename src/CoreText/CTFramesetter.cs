// 
// CTFramesetter.cs: Implements the managed CTFramesetter
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
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

#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ObjCRuntime;
using Foundation;
using CoreFoundation;
using CoreGraphics;

namespace CoreText {
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CTFramesetter : NativeObject {
		[Preserve (Conditional = true)]
		internal CTFramesetter (NativeHandle handle, bool owns)
			: base (handle, owns, true)
		{
		}

		#region Framesetter Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFramesetterCreateWithAttributedString (IntPtr @string);
		/// <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CTFramesetter (NSAttributedString value)
			: base (CTFramesetterCreateWithAttributedString (value.GetNonNullHandle (nameof (value))), true, true)
		{
			GC.KeepAlive (value);
		}
		#endregion

		#region Frame Creation
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFramesetterCreateFrame (IntPtr framesetter, NSRange stringRange, IntPtr path, IntPtr frameAttributes);
		/// <param name="stringRange">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <param name="frameAttributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTFrame? GetFrame (NSRange stringRange, CGPath path, CTFrameAttributes? frameAttributes)
		{
			if (path is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (path));
			var frame = CTFramesetterCreateFrame (Handle, stringRange, path.Handle, frameAttributes.GetHandle ());
			GC.KeepAlive (path);
			GC.KeepAlive (frameAttributes);
			if (frame == IntPtr.Zero)
				return null;
			return new CTFrame (frame, true);
		}

		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFramesetterGetTypesetter (IntPtr framesetter);
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CTTypesetter? GetTypesetter ()
		{
			var h = CTFramesetterGetTypesetter (Handle);

			if (h == IntPtr.Zero)
				return null;
			return new CTTypesetter (h, false);
		}
		#endregion

		#region Frame Sizing
		[DllImport (Constants.CoreTextLibrary)]
		unsafe static extern CGSize CTFramesetterSuggestFrameSizeWithConstraints (
				IntPtr framesetter, NSRange stringRange, IntPtr frameAttributes, CGSize constraints, NSRange* fitRange);
		/// <param name="stringRange">To be added.</param>
		///         <param name="frameAttributes">To be added.</param>
		///         <param name="constraints">To be added.</param>
		///         <param name="fitRange">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public CGSize SuggestFrameSize (NSRange stringRange, CTFrameAttributes? frameAttributes, CGSize constraints, out NSRange fitRange)
		{
			fitRange = default;
			unsafe {
				return CTFramesetterSuggestFrameSizeWithConstraints (
						Handle, stringRange,
						frameAttributes.GetHandle (),
						constraints, (NSRange*) Unsafe.AsPointer<NSRange> (ref fitRange));
			}
		}
		#endregion
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreTextLibrary)]
		static extern IntPtr CTFramesetterCreateWithTypesetter (IntPtr typesetter);

		/// <param name="typesetter">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static CTFramesetter? Create (CTTypesetter typesetter)
		{
			if (typesetter is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (typesetter));

			var ret = CTFramesetterCreateWithTypesetter (typesetter.Handle);
			GC.KeepAlive (typesetter);
			if (ret == IntPtr.Zero)
				return null;

			return new CTFramesetter (ret, owns: true);
		}
	}
}
