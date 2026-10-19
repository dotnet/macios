// 
// IOSurface.cs
//
// Copyright 2016 Microsoft 
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

using CoreFoundation;

namespace IOSurface {
	public partial class IOSurface {

		[DllImport (Constants.IOSurfaceLibrary)]
		static extern uint IOSurfaceCreateMachPort (IntPtr buffer);

		/// <summary>Creates a Mach port that holds a reference to this surface.</summary>
		/// <returns>A Mach send right that can be transferred to another process.</returns>
		/// <remarks>The caller must deallocate the returned port with <c>mach_port_deallocate</c> when it is no longer needed.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public uint CreateMachPort ()
		{
			if (Handle == NativeHandle.Zero)
				ObjCRuntime.ThrowHelper.ThrowObjectDisposedException (this);
			var port = IOSurfaceCreateMachPort (Handle);
			GC.KeepAlive (this);
			return port;
		}

		[DllImport (Constants.IOSurfaceLibrary)]
		static extern IntPtr IOSurfaceLookupFromMachPort (uint port);

		/// <summary>Looks up a surface using a Mach port created by <see cref="CreateMachPort" />.</summary>
		/// <param name="port">The Mach send right referring to the surface.</param>
		/// <returns>The surface, or <see langword="null" /> if the lookup fails.</returns>
		/// <remarks>This method does not deallocate <paramref name="port" />.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static IOSurface? LookupFromMachPort (uint port)
		{
			return Runtime.GetNSObject<IOSurface> (IOSurfaceLookupFromMachPort (port), owns: true);
		}

		[DllImport (Constants.IOSurfaceLibrary)]
		static extern IntPtr IOSurfaceLookup (uint surfaceId);

		/// <summary>Looks up a surface using its system-wide identifier.</summary>
		/// <param name="surfaceId">The identifier of the surface to look up.</param>
		/// <returns>The surface, or <see langword="null" /> if the surface does not exist or is not accessible to the calling process.</returns>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static IOSurface? Lookup (uint surfaceId)
		{
			return Runtime.GetNSObject<IOSurface> (IOSurfaceLookup (surfaceId), owns: true);
		}

#if MONOMAC || __MACCATALYST__
		[DllImport (Constants.IOSurfaceLibrary)]
		static extern IntPtr IOSurfaceCreateXPCObject (IntPtr surface);

		/// <summary>Creates an XPC object that holds a reference to this surface.</summary>
		/// <returns>An XPC object that can be sent to another process. The caller must dispose of this object when it is no longer needed.</returns>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		public NSObject? CreateXpcObject ()
		{
			if (Handle == NativeHandle.Zero)
				ObjCRuntime.ThrowHelper.ThrowObjectDisposedException (this);
			var handle = IOSurfaceCreateXPCObject (Handle);
			GC.KeepAlive (this);
			return Runtime.GetNSObject (handle, owns: true);
		}

		[DllImport (Constants.IOSurfaceLibrary)]
		static extern IntPtr IOSurfaceLookupFromXPCObject (IntPtr xpcObject);

		/// <summary>Looks up a surface using an XPC object created by <see cref="CreateXpcObject" />.</summary>
		/// <param name="xpcObject">The XPC object referring to the surface.</param>
		/// <returns>The surface, or <see langword="null" /> if the lookup fails.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="xpcObject" /> is <see langword="null" />.</exception>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		public static IOSurface? LookupFromXpcObject (NSObject xpcObject)
		{
			var handle = IOSurfaceLookupFromXPCObject (xpcObject.GetNonNullHandle (nameof (xpcObject)));
			GC.KeepAlive (xpcObject);
			return Runtime.GetNSObject<IOSurface> (handle, owns: true);
		}
#endif

		// kern_return_t
		// See bug #59201 
		/// <param name="options">To be added.</param>
		///         <param name="seed">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Lock (IOSurfaceLockOptions options, ref int seed)
		{
			unsafe {
				fixed (int* p = &seed) {
					return _Lock (options, (IntPtr) p);
				}
			}
		}

		// kern_return_t
		// See bug #59201 
		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Lock (IOSurfaceLockOptions options)
		{
			return _Lock (options, IntPtr.Zero);
		}

		// kern_return_t
		// See bug #59201 
		/// <param name="options">To be added.</param>
		///         <param name="seed">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Unlock (IOSurfaceLockOptions options, ref int seed)
		{
			unsafe {
				fixed (int* p = &seed) {
					return _Unlock (options, (IntPtr) p);
				}
			}
		}

		// kern_return_t
		// See bug #59201 
		/// <param name="options">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int Unlock (IOSurfaceLockOptions options)
		{
			return _Unlock (options, IntPtr.Zero);
		}

#if !MONOMAC
		// kern_return_t
		/// <param name="newState">To be added.</param>
		///         <param name="oldState">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int SetPurgeable (IOSurfacePurgeabilityState newState, ref IOSurfacePurgeabilityState oldState)
		{
			unsafe {
				fixed (IOSurfacePurgeabilityState* p = &oldState) {
					return _SetPurgeable (newState, (IntPtr) p);
				}
			}
		}

		/// <param name="newState">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public int SetPurgeable (IOSurfacePurgeabilityState newState)
		{
			return _SetPurgeable (newState, IntPtr.Zero);
		}
#endif
	}
}
