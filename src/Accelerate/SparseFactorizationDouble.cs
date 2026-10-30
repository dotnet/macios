// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Accelerate {
	/// <summary>Owns a reusable double-precision sparse matrix factorization.</summary>
	/// <remarks>Dispose this object to release its native factors. Instance operations are not thread-safe.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed class SparseFactorizationDouble : DisposableObject {
		readonly int rows;
		readonly int columns;

		SparseFactorizationDouble (NativeHandle handle, int rows, int columns) : base (handle, true)
		{
			this.rows = rows;
			this.columns = columns;
		}

		[DllImport ("__Internal")]
		static extern unsafe IntPtr xamarin_sparse_factor_double (byte type, int rows, int columns, long* starts, int* indices, double* data, byte triangle, int* status);

		/// <summary>Factors a real matrix stored in compressed sparse column format.</summary>
		/// <param name="type">The factorization algorithm.</param>
		/// <param name="rowCount">The positive number of matrix rows.</param>
		/// <param name="columnCount">The positive number of matrix columns.</param>
		/// <param name="columnStarts">Zero-based offsets into <paramref name="rowIndices" />, followed by the total entry count.</param>
		/// <param name="rowIndices">Zero-based row indices, strictly increasing within each column.</param>
		/// <param name="values">One value per row index.</param>
		/// <param name="triangle">The stored triangle for Cholesky and LDL transpose factorizations; ignored for QR and LU.</param>
		/// <returns>An independently owned factorization that can solve multiple systems.</returns>
		/// <exception cref="ArgumentException">The matrix dimensions or compressed column arrays are invalid.</exception>
		/// <exception cref="InvalidOperationException">The native factorization failed, for example because the matrix is singular.</exception>
		/// <exception cref="OutOfMemoryException">Native solver storage could not be allocated.</exception>
		/// <remarks>
		///   <para>The input spans are used only during this call. Symmetric factorizations require a square matrix with only the specified triangle stored. QR accepts rectangular matrices.</para>
		///   <para>LU requires a square matrix and macOS 15.5, iOS 18.5, tvOS 18.5, or Mac Catalyst 18.5.</para>
		/// </remarks>
		public static SparseFactorizationDouble Create (SparseFactorization type, int rowCount, int columnCount, ReadOnlySpan<long> columnStarts, ReadOnlySpan<int> rowIndices, ReadOnlySpan<double> values, SparseTriangle triangle = SparseTriangle.Upper)
		{
			SparseValidation.ValidateMatrix (type, rowCount, columnCount, columnStarts, rowIndices, values.Length, triangle);
			unsafe {
				fixed (long* starts = columnStarts)
				fixed (int* indices = rowIndices)
				fixed (double* data = values) {
					int status;
					var handle = xamarin_sparse_factor_double ((byte) type, rowCount, columnCount, starts, indices, data, (byte) triangle, &status);
					SparseValidation.CheckStatus (status);
					return new SparseFactorizationDouble (handle, type == SparseFactorization.CholeskyAtA ? columnCount : rowCount, columnCount);
				}
			}
		}

		[DllImport ("__Internal")]
		static extern unsafe int xamarin_sparse_solve_double (IntPtr handle, int rows, int columns, int count, double* rhs, double* solution);

		/// <summary>Solves one or more systems using the stored factorization.</summary>
		/// <param name="rightHandSides">Right-hand sides stored contiguously in column-major order.</param>
		/// <param name="solutions">Destination for the solution columns, each containing one entry per matrix column.</param>
		/// <param name="rightHandSideCount">The positive number of right-hand sides.</param>
		/// <exception cref="ArgumentException">A span has an incorrect length, or the input and output overlap.</exception>
		/// <exception cref="ObjectDisposedException">The factorization has been disposed.</exception>
		/// <exception cref="OutOfMemoryException">The solve workspace could not be allocated.</exception>
		/// <remarks>For CholeskyAtA, each right-hand side has one entry per matrix column. Otherwise it has one entry per matrix row. Rectangular QR computes a least-squares or minimum-norm solution.</remarks>
		public void Solve (ReadOnlySpan<double> rightHandSides, Span<double> solutions, int rightHandSideCount = 1)
		{
			var handle = GetCheckedHandle ();
			SparseValidation.ValidateSolve (rows, columns, rightHandSideCount, rightHandSides.Length, solutions.Length);
			if (rightHandSides.Overlaps (solutions))
				throw new ArgumentException ("Right-hand sides and solutions must not overlap.", nameof (solutions));
			unsafe {
				fixed (double* rhs = rightHandSides)
				fixed (double* solution = solutions) {
					SparseValidation.CheckStatus (xamarin_sparse_solve_double (handle, rows, columns, rightHandSideCount, rhs, solution));
				}
			}
			GC.KeepAlive (this);
		}

		[DllImport ("__Internal")]
		static extern void xamarin_sparse_cleanup_double (IntPtr handle);

		/// <inheritdoc />
		protected override void Dispose (bool disposing)
		{
			if (Handle != NativeHandle.Zero)
				xamarin_sparse_cleanup_double (Handle);
			base.Dispose (disposing);
		}
	}
}
