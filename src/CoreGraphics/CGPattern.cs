// 
// CGPatterncs.cs: Implements the managed CGPattern
//
// Authors: Mono Team
//     
// Copyright 2009 Novell, Inc
// Copyright 2014 Xamarin Inc.
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
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using ObjCRuntime;
using Foundation;

namespace CoreGraphics {

	// untyped enum -> CGPattern.h
	/// <summary>Pattern styling style.</summary>
	///     <remarks>To be added.</remarks>
	public enum CGPatternTiling {
		/// <summary>No distortion.</summary>
		NoDistortion,
		/// <summary>To be added.</summary>
		ConstantSpacingMinimalDistortion,
		/// <summary>To be added.</summary>
		ConstantSpacing,
	}

	// CGPattern.h
	delegate void DrawPatternCallback (/* void* */ IntPtr info, /* CGContextRef */ IntPtr c);
	delegate void ReleaseInfoCallback (/* void* */ IntPtr info);

	[StructLayout (LayoutKind.Sequential)]
	struct CGPatternCallbacks {
		internal /* unsigned int */ uint version;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, void> draw;
		internal unsafe delegate* unmanaged<IntPtr, void> release;
	}


	/// <summary>A pattern to draw in a CGContext.</summary>
	///     <remarks>To be added.</remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/Drawing/">Example_Drawing</related>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/QuartzSample/">QuartzSample</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGPattern.h
	public class CGPattern : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CGPattern (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		protected internal override void Retain () => CGPatternRetain (Handle);
		protected internal override void Release () => CGPatternRelease (Handle);

		// This is what we expose on the API
		/// <param name="ctx">The CGContext on which the pattern is being drawn.</param>
		///     <summary>Callback signature used to draw patterns on the screen.</summary>
		///     <remarks>This is the delegate that is passed to the CGPattern method.</remarks>
		public delegate void DrawPattern (CGContext ctx);

		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe extern static IntPtr CGPatternCreate (/* void* */ IntPtr info, CGRect bounds, CGAffineTransform matrix,
			/* CGFloat */ nfloat xStep, /* CGFloat */ nfloat yStep, CGPatternTiling tiling, byte isColored,
			/* const CGPatternCallbacks* */ CGPatternCallbacks* callbacks);

		static CGPatternCallbacks callbacks;

		static CGPattern ()
		{
			unsafe {
				callbacks = new CGPatternCallbacks () {
					version = 0,
					draw = &DrawCallback,
					release = &ReleaseCallback,
				};
			}
		}
		GCHandle gch;

		public CGPattern (CGRect bounds, CGAffineTransform matrix, nfloat xStep, nfloat yStep, CGPatternTiling tiling, bool isColored, DrawPattern drawPattern)
		{
			if (drawPattern is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (drawPattern));

			gch = GCHandle.Alloc (drawPattern);
			unsafe {
				fixed (CGPatternCallbacks* callbacksptr = &callbacks)
					InitializeHandle (CGPatternCreate (GCHandle.ToIntPtr (gch), bounds, matrix, xStep, yStep, tiling, isColored.AsByte (), callbacksptr));
			}
		}

		[UnmanagedCallersOnly]
		static void DrawCallback (IntPtr voidptr, IntPtr cgcontextptr)
		{
			GCHandle gch = GCHandle.FromIntPtr (voidptr);
			if (gch.Target is DrawPattern draw_pattern) {
				using (var ctx = new CGContext (cgcontextptr, false))
					draw_pattern (ctx);
			}
		}

		[UnmanagedCallersOnly]
		static void ReleaseCallback (IntPtr voidptr)
		{
			GCHandle gch = GCHandle.FromIntPtr (voidptr);
			gch.Free ();
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGPatternRelease (/* CGPatternRef */ IntPtr pattern);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGPatternRef */ IntPtr CGPatternRetain (/* CGPatternRef */ IntPtr pattern);
#endif // !COREBUILD
	}
}
