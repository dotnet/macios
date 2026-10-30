#nullable enable
using System.Collections.Generic;
using System.Collections;

namespace Foundation {
	public delegate NSOrderedCollectionChange? NSOrderedCollectionDifferenceGetDifferenceHandler (NSOrderedCollectionChange collectionChange);

	public partial class NSOrderedCollectionDifference {

		public NSOrderedCollectionChange [] Insertions => NSArray.NonNullArrayFromHandleDropNullElements<NSOrderedCollectionChange> (_Insertions);

		public NSOrderedCollectionChange [] Removals => NSArray.NonNullArrayFromHandleDropNullElements<NSOrderedCollectionChange> (_Removals);

		[BindingImpl (BindingImplOptions.Optimizable)]
		internal static unsafe BlockLiteral CreateTransformBlock (Func<IntPtr, NSOrderedCollectionChange?> callback)
		{
			delegate* unmanaged<IntPtr, IntPtr, IntPtr> trampoline = &GetDiffHandler;
			return new BlockLiteral (trampoline, callback, typeof (NSOrderedCollectionDifference), nameof (GetDiffHandler));
		}

		[UnmanagedCallersOnly]
		[UserDelegateType (typeof (NSOrderedCollectionDifferenceGetDifferenceHandler))]
		static IntPtr GetDiffHandler (IntPtr block, IntPtr change)
		{
			var callback = BlockLiteral.GetTarget<Func<IntPtr, NSOrderedCollectionChange?>> (block);
			return Runtime.RetainAndAutoreleaseNSObject (callback?.Invoke (change));
		}

		[BindingImpl (BindingImplOptions.Optimizable)]
		internal static unsafe BlockLiteral CreateEquivalenceTestBlock (Func<IntPtr, IntPtr, bool> callback)
		{
			delegate* unmanaged<IntPtr, IntPtr, IntPtr, byte> trampoline = &EquivalenceTestHandler;
			return new BlockLiteral (trampoline, callback, typeof (NSOrderedCollectionDifference), nameof (EquivalenceTestHandler));
		}

		[UnmanagedCallersOnly]
		[UserDelegateType (typeof (NSOrderedCollectionDifferenceEquivalenceTest))]
		static byte EquivalenceTestHandler (IntPtr block, IntPtr first, IntPtr second)
		{
			var callback = BlockLiteral.GetTarget<Func<IntPtr, IntPtr, bool>> (block);
			return callback is not null && callback (first, second) ? (byte) 1 : (byte) 0;
		}

		public NSOrderedCollectionDifference? GetDifference (NSOrderedCollectionDifferenceGetDifferenceHandler callback)
		{
			if (callback is null)
				throw new ArgumentNullException (nameof (callback));

			unsafe {
				using var block = CreateTransformBlock (change => callback (Runtime.GetNSObject<NSOrderedCollectionChange> (change) ?? throw new InvalidOperationException ("The native change is null.")));
				return Runtime.GetNSObject<NSOrderedCollectionDifference> (_GetDifference (&block));
			}
		}

		public NSOrderedCollectionDifference? GetInverseDifference ()
			=> Runtime.GetNSObject<NSOrderedCollectionDifference> (_InverseDifference ());

	}
}
