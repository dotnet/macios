// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Buffers;

namespace Accelerate {
	/// <summary>Options for symbolic ordering and numerical factorization.</summary>
	/// <typeparam name="T">The matrix element type.</typeparam>
	/// <remarks>Permutation and scaling buffers are inputs for user ordering/scaling, and outputs for computed ordering/scaling. Do not modify options during an operation.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed class SparseFactorOptions<T> where T : unmanaged {
		IntPtr allocate, deallocate;

		/// <summary>Creates default ordering and numerical factorization options.</summary>
		public SparseFactorOptions () { }
		/// <summary>The fill-reducing ordering method.</summary>
		public SparseOrder Order { get; set; }
		/// <summary>Optional ordering buffer; LU uses row and column permutations concatenated.</summary>
		public Memory<int> Permutation { get; set; }
		/// <summary>Rows and columns to ignore; a negative terminator is added automatically.</summary>
		public ReadOnlyMemory<int> IgnoredRowsAndColumns { get; set; }
		/// <summary>The scaling method.</summary>
		public SparseScaling Scaling { get; set; }
		/// <summary>Optional real scaling buffer; LU uses row and column scaling concatenated. Values are converted to single precision for float and SparseComplexFloat matrices.</summary>
		public Memory<double> ScalingValues { get; set; }
		/// <summary>The pivot tolerance, or null to use the precision-specific native default.</summary>
		public double? PivotTolerance { get; set; }
		/// <summary>The zero tolerance, or null to use the precision-specific native default.</summary>
		public double? ZeroTolerance { get; set; }

		/// <summary>Sets the native memory callbacks saved in a new symbolic factorization.</summary>
		/// <param name="allocate">An unmanaged callback returning a 16-byte-aligned allocation, or null on allocation failure.</param>
		/// <param name="deallocate">An unmanaged callback releasing an allocation; it must accept a null pointer.</param>
		/// <remarks>
		///   <para>Pass null for both callbacks to restore the system allocator. Supplying only one callback is invalid.</para>
		///   <para>The callbacks have no context parameter. They must be thread-safe, must not throw, and must remain callable until all derived symbolic analyses, numerical factors, and subfactors are released. For managed implementations, use static methods marked with <see cref="System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute" />.</para>
		///   <para>These callbacks control native factor allocations, not the binding's matrix or cached workspace storage. Changing them does not change an existing factorization's allocator. Numerical factorization from existing symbolic analysis and refactoring use the callbacks saved by that analysis.</para>
		/// </remarks>
		public unsafe void SetMemoryAllocator (delegate* unmanaged<nuint, IntPtr> allocate, delegate* unmanaged<IntPtr, void> deallocate)
		{
			if ((allocate == null) != (deallocate == null))
				throw new ArgumentException ("Allocation and deallocation callbacks must both be supplied or both be null.");
			this.allocate = (IntPtr) allocate;
			this.deallocate = (IntPtr) deallocate;
		}

		internal unsafe Pin PinOptions (SparseFactorization type, int rows, int columns, bool symbolicOnly = false)
		{
			int permutationLength = (byte) type >= 80 ? checked (rows + columns) : type == SparseFactorization.Qr ? Math.Min (rows, columns) : columns;
			if (!Permutation.IsEmpty && Permutation.Length != permutationLength)
				throw new ArgumentException ("The permutation buffer has an incorrect length.", nameof (Permutation));
			if (Order < SparseOrder.Default || Order > SparseOrder.MultithreadedMetis)
				throw new ArgumentOutOfRangeException (nameof (Order));
			if (Scaling < SparseScaling.Default || Scaling > SparseScaling.HungarianAndOrdering)
				throw new ArgumentOutOfRangeException (nameof (Scaling));
			if (Order == SparseOrder.MultithreadedMetis || Scaling >= SparseScaling.Hungarian)
				SparseNative.RequireVersion26 ();
			if (Scaling == SparseScaling.HungarianAndOrdering && ((byte) type < 80 || symbolicOnly))
				throw new ArgumentException ("Combined Hungarian scaling and ordering requires an all-in-one LU factorization.");
			int scalingLength = (byte) type >= 80 ? checked (rows + columns) : columns;
			if (!ScalingValues.IsEmpty && ScalingValues.Length != scalingLength)
				throw new ArgumentException ("The scaling buffer has an incorrect length.", nameof (ScalingValues));
			if (Order == SparseOrder.User && !Permutation.IsEmpty) {
				ValidatePermutation (Permutation.Span.Slice (0, (byte) type >= 80 ? rows : permutationLength));
				if ((byte) type >= 80)
					ValidatePermutation (Permutation.Span.Slice (rows, columns));
			}
			if (PivotTolerance is double pivot && (!double.IsFinite (pivot) || pivot < 0 || pivot > 0.5))
				throw new ArgumentOutOfRangeException (nameof (PivotTolerance));
			if (ZeroTolerance is double zero && (!double.IsFinite (zero) || zero < 0))
				throw new ArgumentOutOfRangeException (nameof (ZeroTolerance));
			int limit = (byte) type < 40 ? rows : checked (rows + columns);
			foreach (int index in IgnoredRowsAndColumns.Span) {
				if (index < 0 || index >= limit)
					throw new ArgumentOutOfRangeException (nameof (IgnoredRowsAndColumns));
			}
			return new Pin (this);
		}

		static void ValidatePermutation (ReadOnlySpan<int> permutation)
		{
			var seen = new bool [permutation.Length];
			foreach (int index in permutation) {
				if (index < 0 || index >= seen.Length || seen [index])
					throw new ArgumentException ("User ordering must be a permutation.");
				seen [index] = true;
			}
		}

		internal unsafe ref struct Pin {
			MemoryHandle permutation, ignored, scaling;
			readonly Memory<double> scalingValues;
			readonly float[]? singleScaling;
			internal SparseNative.Options Options;
			internal Pin (SparseFactorOptions<T> source)
			{
				permutation = source.Permutation.Pin ();
				ignored = default;
				scaling = default;
				scalingValues = source.ScalingValues;
				singleScaling = null;
				try {
					if (!scalingValues.IsEmpty && (typeof (T) == typeof (float) || typeof (T) == typeof (SparseComplexFloat))) {
						singleScaling = new float [scalingValues.Length];
						for (int i = 0; i < singleScaling.Length; i++)
							singleScaling [i] = (float) scalingValues.Span [i];
						scaling = singleScaling.AsMemory ().Pin ();
					} else {
						scaling = scalingValues.Pin ();
					}
					if (!source.IgnoredRowsAndColumns.IsEmpty) {
						var indices = new int [checked (source.IgnoredRowsAndColumns.Length + 1)];
						source.IgnoredRowsAndColumns.Span.CopyTo (indices);
						indices [indices.Length - 1] = -1;
						ignored = indices.AsMemory ().Pin ();
					}
					Options = new SparseNative.Options {
						Order = (int) source.Order, Permutation = (int*) permutation.Pointer,
						Ignored = (int*) ignored.Pointer, Scaling = (int) source.Scaling, Scale = scaling.Pointer,
						PivotTolerance = source.PivotTolerance ?? -1, ZeroTolerance = source.ZeroTolerance ?? -1,
						Allocate = source.allocate, Deallocate = source.deallocate,
					};
				} catch {
					permutation.Dispose ();
					ignored.Dispose ();
					scaling.Dispose ();
					throw;
				}
			}
			public void Dispose ()
			{
				if (singleScaling is not null) {
					for (int i = 0; i < singleScaling.Length; i++)
						scalingValues.Span [i] = singleScaling [i];
				}
				permutation.Dispose ();
				ignored.Dispose ();
				scaling.Dispose ();
			}
		}
	}
}
