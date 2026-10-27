// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Accelerate {
	/// <summary>Owns symbolic analysis reusable for matrices with the same nonzero structure.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparseSymbolicFactorization<T> : IDisposable where T : unmanaged {
		readonly SparseHandle handle;
		readonly SparseMatrix<T> structure;
		readonly long[] starts;
		readonly int[] indices;
		readonly SparseFactorization type;
		/// <summary>The required factor-storage capacity in bytes.</summary>
		public nuint FactorStorageSize { get; }
		/// <summary>The required numerical workspace length, including alignment padding.</summary>
		public nuint FactorWorkspaceSize { get; }

		SparseSymbolicFactorization (SparseHandle handle, SparseMatrix<T> structure, SparseFactorization type, nuint storageSize, nuint workspaceSize, long[] starts, int[] indices)
		{
			this.handle = handle;
			this.structure = structure;
			this.type = type;
			FactorStorageSize = storageSize;
			FactorWorkspaceSize = workspaceSize;
			this.starts = starts;
			this.indices = indices;
		}

		/// <summary>Analyzes a matrix structure without computing numerical factors.</summary>
		/// <param name="matrix">The matrix whose structure is analyzed; values are ignored.</param>
		/// <param name="type">The factorization algorithm.</param>
		/// <param name="options">Optional ordering controls.</param>
		/// <returns>Reusable symbolic analysis.</returns>
		public static SparseSymbolicFactorization<T> Create (SparseMatrix<T> matrix, SparseFactorization type, SparseFactorOptions<T>? options = null)
		{
			SparseFactorization<T>.Validate (matrix, type);
			using var lease = new SparseLease (matrix.Handle);
			using var pin = (options ?? new SparseFactorOptions<T> ()).PinOptions (type, matrix.RowCount, matrix.ColumnCount, true);
			var descriptor = matrix.Descriptor;
			var nativeOptions = pin.Options;
			nuint* sizes = stackalloc nuint [4];
			var result = SparseNative.xamarin_sparse_symbolic_create (matrix.Scalar, (byte) type, &descriptor, &nativeOptions, sizes);
			SparseNative.Check ();
			int index = matrix.Scalar == 0 || matrix.Scalar == 2 ? 0 : 1;
			return new SparseSymbolicFactorization<T> (new SparseHandle (result), matrix, type, sizes [index], checked (sizes [index + 2] + 15), matrix.CopyStarts (), matrix.CopyIndices ());
		}

		/// <summary>Creates an independently disposable reference to this analysis.</summary>
		/// <returns>Retained symbolic analysis.</returns>
		public SparseSymbolicFactorization<T> Retain ()
		{
			using var lease = new SparseLease (handle);
			var retained = SparseNative.xamarin_sparse_matrix_retain (lease.Value);
			return new SparseSymbolicFactorization<T> (new SparseHandle (retained), structure, type, FactorStorageSize, FactorWorkspaceSize, starts, indices);
		}

		/// <summary>Computes numerical factors using the existing symbolic analysis.</summary>
		/// <param name="matrix">The matrix with exactly the analyzed nonzero structure.</param>
		/// <param name="options">Optional numerical scaling and pivoting controls.</param>
		/// <param name="storage">Optional explicitly owned factor storage.</param>
		/// <param name="workspace">Optional scratch storage of at least <see cref="FactorWorkspaceSize" /> bytes.</param>
		/// <returns>Numerical factors independent of the symbolic object's lifetime.</returns>
		/// <remarks>Storage remains retained by the factors even if its owner is disposed. Reusing storage concurrently with live factors is invalid. Pivoting may allocate additional storage.</remarks>
		public SparseFactorization<T> Factor (SparseMatrix<T> matrix, SparseFactorOptions<T>? options = null, SparseFactorStorage? storage = null, Span<byte> workspace = default)
		{
			SparseFactorization<T>.Validate (matrix, type);
			using var lease = new SparseLease (handle);
			using var matrixLease = new SparseLease (matrix.Handle);
			using var storageLease = new SparseLease (storage?.Handle);
			var descriptor = matrix.Descriptor;
			if (descriptor.Rows != structure.Descriptor.Rows || descriptor.Columns != structure.Descriptor.Columns || descriptor.Kind != structure.Descriptor.Kind || descriptor.Triangle != structure.Descriptor.Triangle || descriptor.BlockSize != structure.BlockSize || descriptor.Operation != structure.Descriptor.Operation || !starts.AsSpan ().SequenceEqual (new ReadOnlySpan<long> (descriptor.Starts, descriptor.Columns + 1)) || !indices.AsSpan ().SequenceEqual (new ReadOnlySpan<int> (descriptor.Indices, checked ((int) descriptor.Starts [descriptor.Columns]))))
				throw new ArgumentException ("Numerical factorization requires exactly the analyzed matrix structure.", nameof (matrix));
			if (storage is not null && storage.Capacity < FactorStorageSize)
				throw new ArgumentException ("Factor storage is too small.", nameof (storage));
			using var pin = (options ?? new SparseFactorOptions<T> ()).PinOptions (type, matrix.RowCount, matrix.ColumnCount, true);
			var nativeOptions = pin.Options;
			fixed (byte* w = workspace) {
				void* scratch = workspace.IsEmpty ? null : SparseNative.Align (workspace, w, FactorWorkspaceSize - 15);
				storage?.Acquire ();
				try {
					int status = 0;
					var result = SparseNative.xamarin_sparse_factor_create (matrix.Scalar, (byte) type, &descriptor, &nativeOptions, lease.Value, storage is null ? null : storage.Data, scratch, &status);
					SparseNative.Check (status);
					Action? releaseStorage = storage is null ? null : storage.Release;
					return new SparseFactorization<T> (new SparseHandle (result, storage?.Handle, releaseStorage), matrix, type, false, starts, indices);
				} catch {
					storage?.Release ();
					throw;
				}
			}
		}

		/// <summary>Releases the symbolic analysis.</summary>
		public void Dispose () => handle.Dispose ();
	}
}
