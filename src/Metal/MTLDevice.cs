//
// API for the Metal framework
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2014-2015, Xamarin Inc.
//

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace Metal {
#if MONOMAC
	[Advice ("The 'NSString' argument will match a property of 'MTLDeviceNotificationHandler'.")]
	public delegate void MTLDeviceNotificationHandler (IMTLDevice device, NSString notifyName);
#endif

	/// <summary>Represents a single GPU.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("tvos")]
	public static partial class MTLDevice {
		[DllImport (Constants.MetalLibrary)]
		extern static IntPtr MTLCreateSystemDefaultDevice ();

		static IMTLDevice? system_default;

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public static IMTLDevice? SystemDefault {
			get {
				// Metal could be unavailable on the hardware (and we don't want to return an invalid instance)
				// also the instance could be disposed (by mistake) which would make the app unusable
				if ((system_default is null) || (system_default.Handle == IntPtr.Zero)) {
					try {
						var h = MTLCreateSystemDefaultDevice ();
						if (h != IntPtr.Zero)
							system_default = new MTLDeviceWrapper (h, false);
					} catch (EntryPointNotFoundException) {
					} catch (DllNotFoundException) {
					}
				}
				return system_default;
			}
		}

		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		[DllImport (Constants.MetalLibrary)]
		unsafe static extern IntPtr MTLCopyAllDevices ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios18.0")]
		[SupportedOSPlatform ("tvos18.0")]
		public static IMTLDevice [] GetAllDevices ()
		{
			var rv = MTLCopyAllDevices ();
			var devices = NSArray.ArrayFromHandle<IMTLDevice> (rv);
			NSObject.DangerousRelease (rv);
			return devices;
		}

#if MONOMAC

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalLibrary)]
		unsafe static extern IntPtr MTLCopyAllDevicesWithObserver (IntPtr* observer, BlockLiteral* handler);

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static IMTLDevice [] GetAllDevices (MTLDeviceNotificationHandler handler, out NSObject? observer)
		{
			IntPtr rv;
			IntPtr observer_handle;

			unsafe {
				delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> trampoline = &TrampolineNotificationHandler;
				using var block = new BlockLiteral (trampoline, handler, typeof (MTLDevice), nameof (TrampolineNotificationHandler));
				rv = MTLCopyAllDevicesWithObserver (&observer_handle, &block);
			}

			var obj = NSArray.ArrayFromHandle<IMTLDevice> (rv);
			NSObject.DangerousRelease (rv);

			observer = Runtime.GetNSObject (observer_handle);
			NSObject.DangerousRelease (observer_handle); // Apple's documentation says "The observer out parameter is returned with a +1 retain count [...]."

			return obj;
		}

		/// <param name="block">To be added.</param>
		///         <param name="device">To be added.</param>
		///         <param name="notifyName">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[UnmanagedCallersOnly]
		public static unsafe void TrampolineNotificationHandler (IntPtr block, IntPtr device, IntPtr notifyName)
		{
			var descriptor = (BlockLiteral*) block;
			var del = (MTLDeviceNotificationHandler) (descriptor->Target);
			if (del is not null)
				del ((IMTLDevice) Runtime.GetNSObject (device)!, (Foundation.NSString) Runtime.GetNSObject (notifyName)!);
		}

		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalLibrary)]
		static extern void MTLRemoveDeviceObserver (IntPtr observer);

		/// <param name="observer">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static void RemoveObserver (NSObject observer)
		{
			if (observer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (observer));

			MTLRemoveDeviceObserver (observer.Handle);
			GC.KeepAlive (observer);
		}
#endif
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static partial class MTLDevice_Extensions {
		/// <typeparam name="T">The type for which to create a buffer.</typeparam>
		///         <param name="This">The instance on which this method operates.</param>
		///         <param name="data">The data to copy into the buffer.</param>
		///         <param name="options">Options for creating the buffer.</param>
		///         <summary>Creates and returns a new buffer with a copy of the specified data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static IMTLBuffer? CreateBuffer<T> (this IMTLDevice This, T [] data, MTLResourceOptions options) where T : struct
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));

			var handle = GCHandle.Alloc (data, GCHandleType.Pinned); // This requires a pinned GCHandle, since it's not possible to use unsafe code to get the address of a generic object.
			try {
				IntPtr ptr = handle.AddrOfPinnedObject ();
				return This.CreateBuffer (ptr, (nuint) (data.Length * Marshal.SizeOf<T> ()), options);
			} finally {
				handle.Free ();
			}
		}

		/// <param name="This">The instance on which this method operates.</param>
		/// <param name="positions">Array that will be filled with the default sample postions.</param>
		/// <param name="count">The number of positions, which determines the set of default positions.</param>
		/// <summary>Provides the default sample positions for the specified sample <paramref name="count" />.</summary>
		/// <remarks>To be added.</remarks>
		public unsafe static void GetDefaultSamplePositions (this IMTLDevice This, MTLSamplePosition [] positions, nuint count)
		{
			if (positions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (positions));

			if (positions.Length < (nint) count)
				throw new ArgumentException ("Length of 'positions' cannot be less than 'count'.");
			fixed (void* handle = positions)
				This.GetDefaultSamplePositions ((IntPtr) handle, count);
		}
#if IOS

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public static void ConvertSparseTileRegions (this IMTLDevice This, MTLRegion [] tileRegions, MTLRegion [] pixelRegions, MTLSize tileSize, nuint numRegions)
		{
			if (tileRegions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (tileRegions));
			if (pixelRegions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (pixelRegions));

			var tileRegionsHandle = GCHandle.Alloc (tileRegions, GCHandleType.Pinned);
			var pixelRegionsHandle = GCHandle.Alloc (pixelRegions, GCHandleType.Pinned);
			try {
				IntPtr tilePtr = tileRegionsHandle.AddrOfPinnedObject ();
				IntPtr pixelPtr = pixelRegionsHandle.AddrOfPinnedObject ();
				This.ConvertSparseTileRegions (tilePtr, pixelPtr, tileSize, numRegions);
			} finally {
				tileRegionsHandle.Free ();
				pixelRegionsHandle.Free ();
			}
		}

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public static void ConvertSparsePixelRegions (this IMTLDevice This, MTLRegion [] pixelRegions, MTLRegion [] tileRegions, MTLSize tileSize, MTLSparseTextureRegionAlignmentMode mode, nuint numRegions)
		{
			if (tileRegions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (tileRegions));
			if (pixelRegions is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (pixelRegions));

			var tileRegionsHandle = GCHandle.Alloc (tileRegions, GCHandleType.Pinned);
			var pixelRegionsHandle = GCHandle.Alloc (pixelRegions, GCHandleType.Pinned);
			try {
				IntPtr tilePtr = tileRegionsHandle.AddrOfPinnedObject ();
				IntPtr pixelPtr = pixelRegionsHandle.AddrOfPinnedObject ();
				This.ConvertSparsePixelRegions (pixelPtr, tilePtr, tileSize, mode, numRegions);
			} finally {
				tileRegionsHandle.Free ();
				pixelRegionsHandle.Free ();
			}
		}
#endif
	}
}
