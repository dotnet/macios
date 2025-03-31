// 
// CMCustomBlockAllocator.cs: Custom allocator for CMBlockBuffer apis 
//
// Authors:
//    Alex Soto (alex.soto@xamarin.com
// 
// Copyright 2015 Xamarin Inc.
//

#nullable enable

using System;
using System.Runtime.InteropServices;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace CoreMedia {

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CMCustomBlockAllocator : IDisposable {

		GCHandle gch;

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public CMCustomBlockAllocator ()
		{
			gch = GCHandle.Alloc (this);
			// kCMBlockBufferCustomBlockSourceVersion = 0 <- this is the only and current value
			Cblock.Version = 0;
			unsafe {
				// Assign function pointers to temporary variable due to https://github.com/dotnet/runtime/issues/107396.
				delegate* unmanaged<IntPtr, nuint, IntPtr> allocate = &AllocateCallback;
				delegate* unmanaged<IntPtr, IntPtr, nuint, void> free = &FreeCallback;

				Cblock.Allocate = allocate;
				Cblock.Free = free;
			}
			Cblock.RefCon = GCHandle.ToIntPtr (gch);
		}

		// 1:1 mapping to the real underlying structure
		[StructLayout (LayoutKind.Sequential, Pack = 4)] // it's 28 bytes (not 32) on 64 bits iOS
		internal struct CMBlockBufferCustomBlockSource {
			public uint Version;
			public unsafe delegate* unmanaged<IntPtr, nuint, IntPtr> Allocate;
			public unsafe delegate* unmanaged<IntPtr, IntPtr, nuint, void> Free;
			public IntPtr RefCon;
		}
		internal CMBlockBufferCustomBlockSource Cblock;

		[UnmanagedCallersOnly]
		static IntPtr AllocateCallback (IntPtr refCon, nuint sizeInBytes)
		{
			var gch = GCHandle.FromIntPtr (refCon);
			if (gch.Target is CMCustomBlockAllocator target)
				return target.Allocate (sizeInBytes);
			return IntPtr.Zero;
		}

		public virtual IntPtr Allocate (nuint sizeInBytes)
		{
			return Marshal.AllocHGlobal ((int) sizeInBytes);
		}

		[UnmanagedCallersOnly]
		static void FreeCallback (IntPtr refCon, IntPtr doomedMemoryBlock, nuint sizeInBytes)
		{
			var gch = GCHandle.FromIntPtr (refCon);
			if (gch.Target is CMCustomBlockAllocator allocator)
				allocator.Free (doomedMemoryBlock, sizeInBytes);
		}

		public virtual void Free (IntPtr doomedMemoryBlock, nuint sizeInBytes)
		{
			Marshal.FreeHGlobal (doomedMemoryBlock);
		}

		~CMCustomBlockAllocator ()
		{
			Dispose (false);
		}

		/// <summary>Releases the resources used by the CMCustomBlockAllocator object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the CMCustomBlockAllocator class.</para>
		///           <para>Calling the Dispose method when the application is finished using the CMCustomBlockAllocator ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/CoreMedia/CMCustomBlockAllocator.xml" path="/Documentation/Docs[@DocId='M:CoreMedia.CMCustomBlockAllocator.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (gch.IsAllocated)
				gch.Free ();
		}
	}

	// This class is used internally by a couple of CMBlockBuffer methods
	// that take a managed array as input parameter
	internal class CMManagedArrayBlockAllocator : CMCustomBlockAllocator {

		GCHandle dataHandle;
		public CMManagedArrayBlockAllocator (byte [] data)
		{
			dataHandle = GCHandle.Alloc (data, GCHandleType.Pinned); // This requires a pinned GCHandle, because unsafe code is scoped to the current block, and the address of the byte array will be used after this function returns.
		}

		public override IntPtr Allocate (nuint sizeInBytes)
		{
			return dataHandle.AddrOfPinnedObject ();
		}

		public override void Free (IntPtr doomedMemoryBlock, nuint sizeInBytes)
		{
			if (dataHandle.IsAllocated)
				dataHandle.Free ();
		}
	}
}
