// 
// CVPixelBufferPool.cs: Implements the managed CVPixelBufferPool
//
// Authors: Mono Team
//          Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2010 Novell, Inc
// Copyright 2012-2014 Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreFoundation;
using ObjCRuntime;
using Foundation;

#nullable enable

namespace CoreVideo {

	// CVPixelBufferPool.h
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public partial class CVPixelBufferPool : NativeObject {
#if !COREBUILD
		[Preserve (Conditional = true)]
		internal CVPixelBufferPool (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static void CVPixelBufferPoolRelease (/* CVPixelBufferPoolRef __nullable */ IntPtr handle);

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* CVPixelBufferPoolRef __nullable */ IntPtr CVPixelBufferPoolRetain (
			/* CVPixelBufferPoolRef __nullable */ IntPtr handle);

		protected internal override void Retain ()
		{
			CVPixelBufferPoolRetain (GetCheckedHandle ());
		}

		protected internal override void Release ()
		{
			CVPixelBufferPoolRelease (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static nint CVPixelBufferPoolGetTypeID ();
		/// <summary>CoreFoundation TypeID for the CVPixelBufferPool.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public nint TypeID {
			get {
				return CVPixelBufferPoolGetTypeID ();
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* CFDictionaryRef __nullable */ IntPtr CVPixelBufferPoolGetPixelBufferAttributes (
			/* CVPixelBufferPoolRef __nonnull */ IntPtr pool);

		// TODO: Return type is CVPixelBufferAttributes but need different name when this one is not WeakXXXX
		/// <summary>Loosely typed NSDictionary containing all of the PixelBuffer attributes in the pool, it is easier to use the strogly typed Settings property.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///         </remarks>
		public NSDictionary? PixelBufferAttributes {
			get {
				return Runtime.GetNSObject<NSDictionary> (CVPixelBufferPoolGetPixelBufferAttributes (Handle));
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		extern static /* CFDictionaryRef __nullable */ IntPtr CVPixelBufferPoolGetAttributes (
			/* CVPixelBufferPoolRef __nonnull */ IntPtr pool);

		/// <summary>Returns the attributes of the object, you should use the Settings property instead that returns a strongly-typed version of this instead. </summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSDictionary? Attributes {
			get {
				return Runtime.GetNSObject<NSDictionary> (CVPixelBufferPoolGetAttributes (Handle));
			}
		}

		/// <summary>The settings in use for the PixelBufferPool.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public CVPixelBufferPoolSettings? Settings {
			get {
				var attr = Attributes;
				return attr is null ? null : new CVPixelBufferPoolSettings (attr);
			}
		}

		[DllImport (Constants.CoreVideoLibrary)]
		unsafe extern static CVReturn CVPixelBufferPoolCreatePixelBuffer (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* CVPixelBufferPoolRef __nonnull */ IntPtr pixelBufferPool,
			/* CVPixelBufferRef  __nullable * __nonnull */ IntPtr* pixelBufferOut);

		/// <summary>Creates a new CVPixelBuffer in the pool.</summary>
		///         <returns>The newly allocated CVPixelBuffer.</returns>
		///         <remarks>
		///         </remarks>
		public CVPixelBuffer CreatePixelBuffer ()
		{
			CVReturn ret;
			IntPtr pixelBufferOut;
			unsafe {
				ret = CVPixelBufferPoolCreatePixelBuffer (IntPtr.Zero, Handle, &pixelBufferOut);
			}

			if (ret != CVReturn.Success)
				throw new Exception ("CVPixelBufferPoolCreatePixelBuffer returned " + ret.ToString ());

			return new CVPixelBuffer (pixelBufferOut, true);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		unsafe extern static CVReturn CVPixelBufferPoolCreatePixelBufferWithAuxAttributes (
			/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* CVPixelBufferPoolRef __nonnull */ IntPtr pixelBufferPool,
			/* CFDictionaryRef __nullable */ IntPtr auxAttributes,
			/* CVPixelBufferRef  __nullable * __nonnull */ IntPtr* pixelBufferOut);

		/// <param name="allocationSettings">Allocation settings for creating this CVPixelBuffer.</param>
		///         <param name="error">Return error code</param>
		///         <summary>Creates a new CVPixelBuffer in the pool.</summary>
		///         <returns>The newly allocated CVPixelBuffer.</returns>
		///         <remarks>
		///         </remarks>
		public CVPixelBuffer? CreatePixelBuffer (CVPixelBufferPoolAllocationSettings? allocationSettings, out CVReturn error)
		{
			IntPtr pb;
			unsafe {
				error = CVPixelBufferPoolCreatePixelBufferWithAuxAttributes (IntPtr.Zero, Handle, allocationSettings.GetHandle (), &pb);
			}
			if (error != CVReturn.Success)
				return null;

			return new CVPixelBuffer (pb, true);
		}

		[DllImport (Constants.CoreVideoLibrary)]
		unsafe extern static CVReturn CVPixelBufferPoolCreate (/* CFAllocatorRef __nullable */ IntPtr allocator,
			/* CFDictionaryRef __nullable */ IntPtr poolAttributes,
			/* CFDictionaryRef __nullable */ IntPtr pixelBufferAttributes,
			/* CVPixelBufferPoolRef  __nullable * __nonnull */ IntPtr* poolOut);

		static IntPtr Create (NSDictionary? poolAttributes, NSDictionary? pixelBufferAttributes)
		{
			CVReturn ret;
			IntPtr handle;
			unsafe {
				ret = CVPixelBufferPoolCreate (IntPtr.Zero, poolAttributes.GetHandle (), pixelBufferAttributes.GetHandle (), &handle);
				GC.KeepAlive (poolAttributes);
				GC.KeepAlive (pixelBufferAttributes);
			}

			if (ret != CVReturn.Success)
				throw new Exception ("CVPixelBufferPoolCreate returned " + ret.ToString ());

			return handle;
		}

		/// <param name="poolAttributes">Loosely typed set of configuration parameters for the CVPixelBufferPool.</param>
		///         <param name="pixelBufferAttributes">Configuration parameters for creating the CVPixelBuffers in the pool.</param>
		///         <summary>Creates a CVPixelBufferPool with the specified parameters (weak types).</summary>
		///         <remarks>It is best to use the strongly typed constructor.</remarks>
		[Advice ("Use overload with CVPixelBufferPoolSettings")]
		public CVPixelBufferPool (NSDictionary? poolAttributes, NSDictionary? pixelBufferAttributes)
			: base (Create (poolAttributes, pixelBufferAttributes), true)
		{
		}

		/// <param name="settings">Configuration parameters for the CVPixelBufferPool</param>
		///         <param name="pixelBufferAttributes">Configuration parameters for creating the CVPixelBuffers in the pool.</param>
		///         <summary>Creates a CVPixelBufferPool with the specified parameters.</summary>
		///         <remarks>
		///         </remarks>
		public CVPixelBufferPool (CVPixelBufferPoolSettings? settings, CVPixelBufferAttributes? pixelBufferAttributes)
			: this (settings?.GetDictionary (), pixelBufferAttributes?.GetDictionary ())
		{
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.CoreVideoLibrary)]
		static extern void CVPixelBufferPoolFlush (/* CVPixelBufferPoolRef __nonnull */ IntPtr pool,
			CVPixelBufferPoolFlushFlags options);

		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public void Flush (CVPixelBufferPoolFlushFlags options)
		{
			CVPixelBufferPoolFlush (Handle, options);
		}

#endif // !COREBUILD
	}
}
