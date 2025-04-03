// 
// CGDataProvider.cs: Implements the managed CGDataProvider
//
// Authors: Miguel de Icaza
//     
// Copyright 2009 Novell, Inc
// Copyright 2011-2014 Xamarin Inc
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
	/// <summary>A class that wraps a data source and exposes it to the CGImage class.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	// CGDataProvider.h
	public partial class CGDataProvider : NativeObject {
		[Preserve (Conditional = true)]
		internal CGDataProvider (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static void CGDataProviderRelease (/* CGDataProviderRef */ IntPtr provider);

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGDataProviderRef */ IntPtr CGDataProviderRetain (/* CGDataProviderRef */ IntPtr provider);

		protected internal override void Retain ()
		{
			CGDataProviderRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CGDataProviderRelease (GetCheckedHandle ());
		}

#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CGDataProviderRef */ IntPtr CGDataProviderCreateWithFilename (/* const char* */ IntPtr filename);

		/// <param name="file">File name.</param>
		///         <summary>Creates a CGDataProvider from an on-disk file.</summary>
		///         <returns>An initialized CGDataProvider.</returns>
		///         <remarks>To be added.</remarks>
		static public CGDataProvider? FromFile (string file)
		{
			if (file is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (file));

			using var filePtr = new TransientString (file);
			var handle = CGDataProviderCreateWithFilename (filePtr);
			if (handle == IntPtr.Zero)
				return null;

			return new CGDataProvider (handle, true);
		}

		static IntPtr Create (string file)
		{
			if (file is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (file));

			using var filePtr = new TransientString (file);
			var handle = CGDataProviderCreateWithFilename (filePtr);
			if (handle == IntPtr.Zero)
				throw new ArgumentException ("Could not create provider from the specified file");
			return handle;
		}

		/// <param name="file">The file to load data from.</param>
		///         <summary>Exposes the contents of the file as a CGDataProvider.</summary>
		///         <remarks>
		///         </remarks>
		public CGDataProvider (string file)
			: base (Create (file), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* CGDataProviderRef */ IntPtr CGDataProviderCreateWithURL (/* CFURLRef __nullable */ IntPtr url);

		static IntPtr Create (NSUrl url)
		{
			// not it's a __nullable parameter but it would return nil (see unit tests) and create an invalid instance
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
			IntPtr result = CGDataProviderCreateWithURL (url.Handle);
			GC.KeepAlive (url);
			return result;
		}

		/// <param name="url">To be added.</param>
		///         <summary>Creates a new <see cref="T:CoreGraphics.CGDataProvider" /> from the data at the specified <paramref name="url" />.</summary>
		///         <remarks>To be added.</remarks>
		public CGDataProvider (NSUrl url)
			: base (Create (url), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern /* CGDataProviderRef */ IntPtr CGDataProviderCreateWithCFData (/* CFDataRef __nullable */ IntPtr data);

		static IntPtr Create (NSData data)
		{
			// not it's a __nullable parameter but it would return nil (see unit tests) and create an invalid instance
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			IntPtr result = CGDataProviderCreateWithCFData (data.Handle);
			GC.KeepAlive (data);
			return result;
		}

		/// <param name="data">To be added.</param>
		///         <summary>Creates a new <see cref="T:CoreGraphics.CGDataProvider" /> from the provided <paramref name="data" />.</summary>
		///         <remarks>To be added.</remarks>
		public CGDataProvider (NSData data)
			: base (Create (data), true)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static unsafe IntPtr CGDataProviderCreateWithData (/* void* */ IntPtr info, /* const void* */ IntPtr data, /* size_t */ nint size, /* CGDataProviderReleaseDataCallback */ delegate* unmanaged<IntPtr, IntPtr, nint, void> releaseData);

		[UnmanagedCallersOnly]
		private static void ReleaseGCHandle (IntPtr info, IntPtr data, nint size)
		{
			var gch = GCHandle.FromIntPtr (info);
			gch.Free ();
		}

		[UnmanagedCallersOnly]
		private static void ReleaseBuffer (IntPtr info, IntPtr data, nint size)
		{
			if (data != IntPtr.Zero)
				Marshal.FreeHGlobal (data);
		}

		[UnmanagedCallersOnly]
		private static void ReleaseFunc (IntPtr info, IntPtr data, nint size)
		{
			var gch = GCHandle.FromIntPtr (info);
			var target = gch.Target as Action<IntPtr>;
			try {
				if (target is not null)
					target (data);
			} finally {
				gch.Free ();
			}
		}

		/// <param name="memoryBlock">Pointer to the memory block.</param>
		///         <param name="size">Size of the block.</param>
		///         <summary>Creates a CGDataProvider from an in-memory block.</summary>
		///         <remarks>To be added.</remarks>
		public CGDataProvider (IntPtr memoryBlock, int size)
			: this (memoryBlock, size, false)
		{
		}

		static IntPtr Create (IntPtr memoryBlock, int size, bool ownBuffer)
		{
			if (!ownBuffer)
				memoryBlock = Runtime.CloneMemory (memoryBlock, size);
			unsafe {
				return CGDataProviderCreateWithData (IntPtr.Zero, memoryBlock, size, &ReleaseBuffer);
			}
		}

		/// <param name="memoryBlock">Pointer to the memory block.</param>
		///         <param name="size">Size of the block.</param>
		///         <param name="ownBuffer">If true this means that the CGDataProvider owns the buffer and will call FreeHGlobal when it is disposed, otherwise it is assumed that the block is owned by another object.</param>
		///         <summary>Creates a CGDataProvider from an in-memory block.</summary>
		///         <remarks>To be added.</remarks>
		public CGDataProvider (IntPtr memoryBlock, int size, bool ownBuffer)
			: base (Create (memoryBlock, size, ownBuffer), true)
		{
		}

		static IntPtr Create (IntPtr memoryBlock, int size, Action<IntPtr> releaseMemoryBlockCallback)
		{
			if (releaseMemoryBlockCallback is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (releaseMemoryBlockCallback));

			var gch = GCHandle.Alloc (releaseMemoryBlockCallback);
			unsafe {
				return CGDataProviderCreateWithData (GCHandle.ToIntPtr (gch), memoryBlock, size, &ReleaseFunc);
			}
		}

		/// <param name="memoryBlock">To be added.</param>
		///         <param name="size">To be added.</param>
		///         <param name="releaseMemoryBlockCallback">To be added.</param>
		///         <summary>Creates a new <see cref="T:CoreGraphics.CGDataProvider" /> from the data at the specified <paramref name="memoryBlock" />.</summary>
		///         <remarks>To be added.</remarks>
		public CGDataProvider (IntPtr memoryBlock, int size, Action<IntPtr> releaseMemoryBlockCallback)
			: base (Create (memoryBlock, size, releaseMemoryBlockCallback), true)
		{
		}

		static IntPtr Create (byte [] buffer, int offset, int count)
		{
			if (buffer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (buffer));
			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentException (nameof (offset));
			if (offset + count > buffer.Length)
				throw new ArgumentException (nameof (offset));

			var gch = GCHandle.Alloc (buffer, GCHandleType.Pinned); // This requires a pinned GCHandle, because unsafe code is scoped to the current block, and the address of the byte array will be used after this function returns.
			var ptr = gch.AddrOfPinnedObject () + offset;
			unsafe {
				return CGDataProviderCreateWithData (GCHandle.ToIntPtr (gch), ptr, count, &ReleaseGCHandle);
			}
		}

		/// <param name="buffer">A block of bytes that contains the data to be provided.</param>
		///         <param name="offset">Offset into the block that is considered part of the data to be provided.</param>
		///         <param name="count">The number of bytes to consume from the offset start from the block.</param>
		///         <summary>Creates a CGDataProvider that exposes the byte array starting at the specified offset for the specified amount of bytes.</summary>
		///         <remarks>To be added.</remarks>
		public CGDataProvider (byte [] buffer, int offset, int count)
			: base (Create (buffer, offset, count), true)
		{
		}

		/// <param name="buffer">To be added.</param>
		///         <summary>Creates a new <see cref="T:CoreGraphics.CGDataProvider" /> from the data in the provided <paramref name="buffer" />.</summary>
		///         <remarks>To be added.</remarks>
		public CGDataProvider (byte [] buffer)
			: this (buffer, 0, buffer.Length)
		{
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		extern static /* CFDataRef */ IntPtr CGDataProviderCopyData (/* CGDataProviderRef */ IntPtr provider);

		/// <summary>Returns a copy of the provider's data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSData? CopyData ()
		{
			return Runtime.GetNSObject<NSData> (CGDataProviderCopyData (Handle), true);
		}
#endif
	}
}
