//
// Copyright 2019 Microsoft Corp.
//

#if !__MACCATALYST__

using System;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

#nullable enable

namespace AppKit {

	public partial class NSView {
		sealed class SortData {
			public Exception? Exception;
			public Func<NSView?, NSView?, NSComparisonResult> Comparer;

			public SortData (Func<NSView?, NSView?, NSComparisonResult> comparer)
			{
				Comparer = comparer;
			}
		}

		[UnmanagedCallersOnly]
		static nint view_compare (IntPtr view1, IntPtr view2, IntPtr context)
		{
			var data = GCHandle.FromIntPtr (context).Target as SortData;
			// should never happen, but we must be ready and not crash
			if (data is null)
				return (nint) (long) NSComparisonResult.Same;

			try {
				var a = Runtime.GetNSObject (view1) as NSView;
				var b = Runtime.GetNSObject (view2) as NSView;
				return (nint) (long) data.Comparer (a, b);
			} catch (Exception e) {
				data.Exception = e;
				return (nint) (long) NSComparisonResult.Same;
			}
		}

		public unsafe void SortSubviews (Func<NSView?, NSView?, NSComparisonResult> comparer)
		{
			if (comparer is null)
				throw new ArgumentNullException (nameof (comparer));

			delegate* unmanaged<IntPtr, IntPtr, IntPtr, nint> fptr = &view_compare;
			var func = (IntPtr) fptr;
			var context = new SortData (comparer);
			var handle = GCHandle.Alloc (context);
			try {
				SortSubviews (func, GCHandle.ToIntPtr (handle));
				if (context.Exception is not null)
					throw new Exception ($"An exception occurred during sorting.", context.Exception);
			} finally {
				handle.Free ();
			}
		}
	}
}
#endif // !__MACCATALYST__
