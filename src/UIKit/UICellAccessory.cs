//
// UICellAccessory.cs
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
// Copyright (c) Microsoft Corporation.
//

using System;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;

#nullable enable

namespace UIKit {

	public partial class UICellAccessory {

		[DllImport (Constants.UIKitLibrary)]
		static extern IntPtr UICellAccessoryPositionBeforeAccessoryOfClass (IntPtr accessoryCls);

		[return: DelegateProxy (typeof (SDUICellAccessoryPosition))]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static UICellAccessoryPosition GetPositionBeforeAccessory (Class accessoryClass)
		{
			if (accessoryClass is null)
				throw new ArgumentNullException (nameof (accessoryClass));
			var ret = UICellAccessoryPositionBeforeAccessoryOfClass (accessoryClass.Handle);
			GC.KeepAlive (accessoryClass);
			return NIDUICellAccessoryPosition.Create (ret)!;
		}

		[return: DelegateProxy (typeof (SDUICellAccessoryPosition))]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static UICellAccessoryPosition GetPositionBeforeAccessory (Type accessoryType) => GetPositionBeforeAccessory (new Class (accessoryType));

		[DllImport (Constants.UIKitLibrary)]
		static extern IntPtr UICellAccessoryPositionAfterAccessoryOfClass (IntPtr accessoryCls);

		[return: DelegateProxy (typeof (SDUICellAccessoryPosition))]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static UICellAccessoryPosition GetPositionAfterAccessory (Class accessoryClass)
		{
			if (accessoryClass is null)
				throw new ArgumentNullException (nameof (accessoryClass));
			var ret = UICellAccessoryPositionAfterAccessoryOfClass (accessoryClass.Handle);
			GC.KeepAlive (accessoryClass);
			return NIDUICellAccessoryPosition.Create (ret)!;
		}

		[return: DelegateProxy (typeof (SDUICellAccessoryPosition))]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static UICellAccessoryPosition GetPositionAfterAccessory (Type accessoryType) => GetPositionAfterAccessory (new Class (accessoryType));
	} /* class UICellAccessory */

	[UnmanagedFunctionPointerAttribute (CallingConvention.Cdecl)]
	[UserDelegateType (typeof (UICellAccessoryPosition))]
	internal delegate nuint DUICellAccessoryPosition (IntPtr block, IntPtr accessories);

	//
	// This class bridges native block invocations that call into C#
	//
	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("maccatalyst")]
	static internal class SDUICellAccessoryPosition {
		[UnmanagedCallersOnly]
		static unsafe nuint Invoke (IntPtr block, IntPtr accessories)
		{
			var descriptor = (BlockLiteral*) block;
			var del = (UICellAccessoryPosition) (descriptor->Target);
			nuint retval = del (NSArray.ArrayFromHandle<UICellAccessory> (accessories));
			return retval;
		}
	} /* class SDUICellAccessoryPosition */

	[SupportedOSPlatform ("tvos14.0")]
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("maccatalyst")]
	internal sealed class NIDUICellAccessoryPosition : TrampolineBlockBase {
		DUICellAccessoryPosition invoker;

		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe NIDUICellAccessoryPosition (BlockLiteral* block) : base (block)
		{
			invoker = block->GetDelegateForBlock<DUICellAccessoryPosition> ();
		}

		[Preserve (Conditional = true)]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public unsafe static UICellAccessoryPosition? Create (IntPtr block)
		{
			if (block == IntPtr.Zero)
				return null;
			var del = (UICellAccessoryPosition) GetExistingManagedDelegate (block);
			return del ?? new NIDUICellAccessoryPosition ((BlockLiteral*) block).Invoke;
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		nuint Invoke (UICellAccessory [] accessories)
		{
			using var nsa_accessories = accessories is null ? null : NSArray.FromNSObjects (accessories);

			nuint result = invoker (BlockPointer, nsa_accessories.GetHandle ());
			GC.KeepAlive (nsa_accessories);
			return result;
		}
	} /* class NIDUICellAccessoryPosition */
}
