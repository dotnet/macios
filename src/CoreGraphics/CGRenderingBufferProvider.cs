// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using CoreFoundation;

namespace CoreGraphics {
	// typedef struct CF_BRIDGED_TYPE(id) CGRenderingBufferProvider* CGRenderingBufferProviderRef;
	/// <summary>This struct is used when creating adaptive bitmap contexts.</summary>
	[SupportedOSPlatform ("ios26.0")]
	[SupportedOSPlatform ("tvos26.0")]
	[SupportedOSPlatform ("maccatalyst26.0")]
	[SupportedOSPlatform ("macos26.0")]
	public class CGRenderingBufferProvider : NativeObject {
		[Preserve (Conditional = true)]
		internal CGRenderingBufferProvider (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}
#if !COREBUILD
		[DllImport (Constants.CoreGraphicsLibrary)]
		unsafe static extern IntPtr /* CGRenderingBufferProviderRef __nullable */ CGRenderingBufferProviderCreate (
				IntPtr /* void *__nullable */ info,
				nuint size,
				/* void* __nullable(^__nonnull  lockPointer)(void* __nullable info) */ BlockLiteral* lockPointer,
				/* void            (^__nullable unlockPointer)(void* __nullable info, void*  __nonnull pointer) */ BlockLiteral* unlockPointer,
				/* void            (^__nullable releaseInfo)(void* __nullable info) */ BlockLiteral* releaseInfo);

		/// <summary>Create a new <see cref="CGRenderingBufferProvider" /> instance.</summary>
		/// <param name="info">A user-defined value that is passed to the callbacks.</param>
		/// <param name="size">The size of the buffer to create.</param>
		/// <param name="lockPointer">A callback that is called to lock the pointer.</param>
		/// <param name="unlockPointer">A callback that is called to unlock the pointer.</param>
		/// <param name="releaseInfo">A callback that is called to release <paramref name="info" /> when the <see cref="CGRenderingBufferProvider" /> is destroyed.</param>
		/// <returns>A new <see cref="CGRenderingBufferProvider" /> instance if successful, <see langword="null" /> otherwise.</returns>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static CGRenderingBufferProvider? Create (IntPtr info, nuint size, LockPointerCallback lockPointer, UnlockPointerCallback unlockPointer, ReleaseInfoCallback releaseInfo)
		{
			if (lockPointer is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (lockPointer));

			delegate* unmanaged<BlockLiteral*, IntPtr, IntPtr> lockPointerTrampoline = &LockPointerBlock;
			using var lockPointerBlock = new BlockLiteral (lockPointerTrampoline, lockPointer, typeof (CGRenderingBufferProvider), nameof (LockPointerBlock));

			delegate* unmanaged<BlockLiteral*, IntPtr, IntPtr, void> unlockPointerTrampoline = &UnlockPointerBlock;
			using var unlockPointerBlock = unlockPointer is null ? default (BlockLiteral) : new BlockLiteral (unlockPointerTrampoline, unlockPointer, typeof (CGRenderingBufferProvider), nameof (UnlockPointerBlock));

			delegate* unmanaged<BlockLiteral*, IntPtr, void> releaseInfoTrampoline = &ReleaseInfoBlock;
			using var releaseInfoBlock = releaseInfo is null ? default (BlockLiteral) : new BlockLiteral (releaseInfoTrampoline, releaseInfo, typeof (CGRenderingBufferProvider), nameof (ReleaseInfoBlock));

			var h = CGRenderingBufferProviderCreate (
						info,
						size,
						&lockPointerBlock,
						unlockPointer is null ? null : &unlockPointerBlock,
						releaseInfo is null ? null : &releaseInfoBlock);
			if (h == IntPtr.Zero)
				return null;
			return new CGRenderingBufferProvider (h, true);
		}

		public delegate IntPtr LockPointerCallback (IntPtr info);

		[UnmanagedCallersOnly]
		unsafe static IntPtr LockPointerBlock (BlockLiteral* block, IntPtr info)
		{
			var del = BlockLiteral.GetTarget<LockPointerCallback> ((IntPtr) block);
			if (del is not null)
				return del (info);
			return IntPtr.Zero;
		}

		public delegate void UnlockPointerCallback (IntPtr info, IntPtr pointer);

		[UnmanagedCallersOnly]
		unsafe static void UnlockPointerBlock (BlockLiteral* block, IntPtr info, IntPtr pointer)
		{
			var del = BlockLiteral.GetTarget<UnlockPointerCallback> ((IntPtr) block);
			if (del is not null)
				del (info, pointer);
		}

		public delegate void ReleaseInfoCallback (IntPtr info);

		[UnmanagedCallersOnly]
		unsafe static void ReleaseInfoBlock (BlockLiteral* block, IntPtr info)
		{
			var del = BlockLiteral.GetTarget<ReleaseInfoCallback> ((IntPtr) block);
			if (del is not null)
				del (info);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr /* CGRenderingBufferProviderRef __nullable */ CGRenderingBufferProviderCreateWithCFData (IntPtr /* CFMutableDataRef */ data);

		/// <summary>Create a new <see cref="CGRenderingBufferProvider" /> instance for a given <see cref="NSData" /> instance.</summary>
		/// <param name="data">The data to use when creawting the new <see cref="CGRenderingBufferProvider" /> instance.</param>
		/// <returns>A new <see cref="CGRenderingBufferProvider" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static CGRenderingBufferProvider? Create (NSData data)
		{
			var h = CGRenderingBufferProviderCreateWithCFData (data.GetNonNullHandle (nameof (data)));
			GC.KeepAlive (data);
			if (h == IntPtr.Zero)
				return null;
			return new CGRenderingBufferProvider (h, true);
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern nuint CGRenderingBufferProviderGetSize (IntPtr /* CGRenderingBufferProviderRef */ provider);

		/// <summary>Get the size of this <see cref="CGRenderingBufferProvider" />.</summary>
		public nuint Size {
			get => CGRenderingBufferProviderGetSize (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern IntPtr CGRenderingBufferLockBytePtr (IntPtr /* CGRenderingBufferProviderRef */ provider);

		/// <summary>Lock the pointer, and return it.</summary>
		public IntPtr LockBytePointer ()
		{
			return CGRenderingBufferLockBytePtr (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern void CGRenderingBufferUnlockBytePtr (IntPtr /* CGRenderingBufferProviderRef */ provider);

		/// <summary>Unlock the pointer.</summary>
		public void UnlockBytePointer ()
		{
			CGRenderingBufferUnlockBytePtr (GetCheckedHandle ());
		}

		[DllImport (Constants.CoreGraphicsLibrary)]
		static extern nuint CGRenderingBufferProviderGetTypeID ();

		/// <summary>Get this type's CFTypeID.</summary>
		[DllImport (Constants.CoreGraphicsLibrary, EntryPoint = "CGRenderingBufferProviderGetTypeID")]
		public extern static /* CFTypeID */ nint GetTypeId ();
#endif // !COREBUILD
	}
}
