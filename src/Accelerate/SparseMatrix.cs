// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Numerics;

namespace Accelerate {
	/// <summary>Owns an immutable sparse matrix in compressed sparse column or block-column format.</summary>
	/// <typeparam name="T">Double, float, <see cref="Complex" />, or <see cref="SparseComplexFloat" />.</typeparam>
	/// <remarks>Complex arithmetic requires macOS 15.5 or iOS, tvOS, or Mac Catalyst 18.5. Complex symmetric matrices additionally require OS version 26. Dispose the matrix to release its storage.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparseMatrix<T> : IDisposable where T : unmanaged {
		internal readonly SparseHandle Handle;
		internal readonly SparseNative.Matrix Descriptor;
		internal readonly int Scalar;

		SparseMatrix (SparseHandle handle, SparseNative.Matrix descriptor)
		{
			Handle = handle;
			Descriptor = descriptor;
			Scalar = SparseNative.Scalar<T> ();
		}

		/// <summary>The number of scalar rows, after applying the matrix operation.</summary>
		public int RowCount => checked ((Descriptor.Operation == (int) SparseOperation.None ? Descriptor.Rows : Descriptor.Columns) * Descriptor.BlockSize);
		/// <summary>The number of scalar columns, after applying the matrix operation.</summary>
		public int ColumnCount => checked ((Descriptor.Operation == (int) SparseOperation.None ? Descriptor.Columns : Descriptor.Rows) * Descriptor.BlockSize);
		/// <summary>The size of each square stored block.</summary>
		public int BlockSize => Descriptor.BlockSize;
		/// <summary>The interpretation of implicitly stored entries.</summary>
		public SparseMatrixKind Kind => (SparseMatrixKind) Descriptor.Kind;
		/// <summary>The stored triangle.</summary>
		public SparseTriangle Triangle => (SparseTriangle) Descriptor.Triangle;

		static SparseNative.Matrix ValidateDimensions (int rowCount, int columnCount, SparseMatrixKind kind, SparseTriangle triangle, int blockSize)
		{
			int scalar = SparseNative.Scalar<T> ();
			if (rowCount <= 0 || columnCount <= 0 || blockSize <= 0 || blockSize > byte.MaxValue || rowCount % blockSize != 0 || columnCount % blockSize != 0)
				throw new ArgumentException ("Positive matrix dimensions must be multiples of a block size between 1 and 255.");
			if (kind != SparseMatrixKind.Ordinary && kind != SparseMatrixKind.Triangular && kind != SparseMatrixKind.UnitTriangular && kind != SparseMatrixKind.Symmetric && kind != SparseMatrixKind.Hermitian)
				throw new ArgumentOutOfRangeException (nameof (kind));
			if (kind == SparseMatrixKind.Hermitian && scalar < 2)
				throw new ArgumentException ("Hermitian storage requires a complex element type.", nameof (kind));
			if (kind == SparseMatrixKind.Symmetric && scalar >= 2)
				SparseNative.RequireVersion26 ();
			if (kind != SparseMatrixKind.Ordinary && rowCount != columnCount)
				throw new ArgumentException ("Structured matrices must be square.");
			if (triangle != SparseTriangle.Upper && triangle != SparseTriangle.Lower)
				throw new ArgumentOutOfRangeException (nameof (triangle));
			return new SparseNative.Matrix {
				Rows = rowCount / blockSize, Columns = columnCount / blockSize, BlockSize = blockSize,
				Kind = (int) kind, Triangle = (int) triangle, Operation = (int) SparseOperation.None,
			};
		}

		/// <summary>Copies a compressed sparse column matrix into independently owned storage.</summary>
		/// <param name="rowCount">The positive number of scalar rows.</param>
		/// <param name="columnCount">The positive number of scalar columns.</param>
		/// <param name="columnStarts">One zero-based offset per block column, followed by the total number of blocks.</param>
		/// <param name="rowIndices">Strictly increasing block-row indices within each block column.</param>
		/// <param name="values">Column-major values for each square block.</param>
		/// <param name="kind">The interpretation of implicit entries.</param>
		/// <param name="triangle">The stored triangle for structured matrices.</param>
		/// <param name="blockSize">The square block size; one for scalar entries.</param>
		/// <returns>An independently owned matrix.</returns>
		public static SparseMatrix<T> FromCompressedColumns (int rowCount, int columnCount, ReadOnlySpan<long> columnStarts, ReadOnlySpan<int> rowIndices, ReadOnlySpan<T> values, SparseMatrixKind kind = SparseMatrixKind.Ordinary, SparseTriangle triangle = SparseTriangle.Upper, int blockSize = 1)
		{
			var matrix = ValidateDimensions (rowCount, columnCount, kind, triangle, blockSize);
			if (columnStarts.Length != (long) matrix.Columns + 1 || columnStarts [0] != 0 || columnStarts [matrix.Columns] != rowIndices.Length)
				throw new ArgumentException ("Column offsets must start at zero and end at the block count.", nameof (columnStarts));
			if (values.Length != (long) rowIndices.Length * blockSize * blockSize)
				throw new ArgumentException ("There must be blockSize squared values per block.", nameof (values));
			for (int column = 0; column < matrix.Columns; column++) {
				long start = columnStarts [column], end = columnStarts [column + 1];
				if (start < 0 || end < start || end > rowIndices.Length)
					throw new ArgumentException ("Column offsets must be nonnegative, ordered, and within the row-index array.", nameof (columnStarts));
				int previous = -1;
				for (int index = (int) start; index < end; index++) {
					int row = rowIndices [index];
					if (row <= previous || row >= matrix.Rows)
						throw new ArgumentException ("Block-row indices must be in range and strictly increasing in each column.", nameof (rowIndices));
					if (kind != SparseMatrixKind.Ordinary && (triangle == SparseTriangle.Upper ? row > column : row < column))
						throw new ArgumentException ("Stored blocks must belong to the specified triangle.", nameof (rowIndices));
					previous = row;
				}
			}
			fixed (long* starts = columnStarts)
			fixed (int* indices = rowIndices)
			fixed (T* data = values) {
				matrix.Starts = starts;
				matrix.Indices = indices;
				matrix.Data = data;
				var result = SparseNative.xamarin_sparse_matrix_create (SparseNative.Scalar<T> (), &matrix, rowIndices.Length);
				SparseNative.Check ();
				return new SparseMatrix<T> (new SparseHandle (result), matrix);
			}
		}

		/// <summary>Converts coordinate-format blocks, combining entries with duplicate coordinates.</summary>
		/// <param name="rowCount">The positive number of scalar rows.</param>
		/// <param name="columnCount">The positive number of scalar columns.</param>
		/// <param name="rows">Zero-based block-row coordinates.</param>
		/// <param name="columns">Zero-based block-column coordinates.</param>
		/// <param name="values">Column-major values for each block.</param>
		/// <param name="kind">The interpretation of implicit entries.</param>
		/// <param name="triangle">The stored triangle.</param>
		/// <param name="blockSize">The square block size.</param>
		/// <returns>An independently owned compressed-column matrix.</returns>
		public static SparseMatrix<T> FromCoordinates (int rowCount, int columnCount, ReadOnlySpan<int> rows, ReadOnlySpan<int> columns, ReadOnlySpan<T> values, SparseMatrixKind kind = SparseMatrixKind.Ordinary, SparseTriangle triangle = SparseTriangle.Upper, int blockSize = 1)
		{
			var matrix = ValidateDimensions (rowCount, columnCount, kind, triangle, blockSize);
			if (columns.Length != rows.Length || values.Length != (long) rows.Length * blockSize * blockSize)
				throw new ArgumentException ("Each coordinate must have one block of values.");
			for (int index = 0; index < rows.Length; index++) {
				if (rows [index] < 0 || rows [index] >= matrix.Rows || columns [index] < 0 || columns [index] >= matrix.Columns)
					throw new ArgumentException ("Block coordinates must be within the matrix.");
			}
			fixed (int* r = rows)
			fixed (int* c = columns)
			fixed (T* v = values) {
				var result = SparseNative.xamarin_sparse_coordinate (SparseNative.Scalar<T> (), &matrix, rows.Length, r, c, v);
				SparseNative.Check ();
				return new SparseMatrix<T> (new SparseHandle (result), matrix);
			}
		}

		/// <summary>Creates an independently disposable transpose view sharing the matrix storage.</summary>
		/// <param name="conjugate">Whether to conjugate complex entries.</param>
		/// <returns>A transpose view that remains valid after this matrix is disposed.</returns>
		public SparseMatrix<T> GetTranspose (bool conjugate = false)
		{
			using var lease = new SparseLease (Handle);
			var descriptor = Descriptor;
			int operation = conjugate && Scalar >= 2 ? (int) SparseOperation.ConjugateTranspose : (int) SparseOperation.Transpose;
			if (descriptor.Operation != (int) SparseOperation.None && descriptor.Operation != operation)
				throw new InvalidOperationException ("A transpose and a conjugate transpose cannot be combined in a matrix view.");
			descriptor.Operation = descriptor.Operation == operation ? (int) SparseOperation.None : operation;
			return new SparseMatrix<T> (new SparseHandle (SparseNative.xamarin_sparse_matrix_retain (lease.Value)), descriptor);
		}

		/// <summary>Computes destination = alpha * matrix * input, optionally accumulating into the destination.</summary>
		/// <param name="input">Contiguous column-major input vectors.</param>
		/// <param name="destination">Contiguous column-major destination vectors.</param>
		/// <param name="alpha">The scale applied to the product.</param>
		/// <param name="columnCount">The positive number of input vectors.</param>
		/// <param name="accumulate">Whether to add to the existing destination values.</param>
		public void Multiply (ReadOnlySpan<T> input, Span<T> destination, T alpha, int columnCount = 1, bool accumulate = false)
		{
			using var lease = new SparseLease (Handle);
			SparseValidation.ValidateSolve (ColumnCount, RowCount, columnCount, input.Length, destination.Length);
			if (input.Overlaps (destination))
				throw new ArgumentException ("Matrix multiplication input and destination must not overlap.");
			var descriptor = Descriptor;
			fixed (T* x = input)
			fixed (T* y = destination)
				SparseNative.xamarin_sparse_matrix_multiply (Scalar, &descriptor, columnCount, x, y, &alpha, accumulate ? (byte) 1 : (byte) 0);
			SparseNative.Check ();
		}

		internal long[] CopyStarts ()
		{
			using var lease = new SparseLease (Handle);
			return new ReadOnlySpan<long> (Descriptor.Starts, Descriptor.Columns + 1).ToArray ();
		}

		internal int[] CopyIndices ()
		{
			using var lease = new SparseLease (Handle);
			return new ReadOnlySpan<int> (Descriptor.Indices, checked ((int) Descriptor.Starts [Descriptor.Columns])).ToArray ();
		}

		/// <summary>Releases the native matrix storage.</summary>
		public void Dispose () => Handle.Dispose ();
	}
}
