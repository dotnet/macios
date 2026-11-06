// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Accelerate {
	/// <summary>Owns reusable state for single-step sparse iteration and custom convergence tests.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	/// <remarks>Instance operations are not thread-safe. Call <see cref="Complete" /> to finalize solutions after the desired convergence criterion is met.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparseIteration<T> : IDisposable where T : unmanaged {
		readonly SparseHandle handle;
		readonly void* state;
		readonly SparseNative.IterationOptions options;
		readonly SparseOperatorCallback<T> callback;
		readonly SparsePreconditioner<T>? preconditioner;
		readonly IntPtr nativePreconditioner;
		readonly T [] rhs, residual, solutions;
		readonly byte [] converged;
		readonly int rows, columns, count, scalar;
		int iteration;
		bool completed, failed;
		/// <summary>The number of bytes occupied by the native iteration state.</summary>
		public nuint StateSize { get; }
		/// <summary>The distance in elements between residual-vector starts.</summary>
		public int ResidualStride { get; }
		/// <summary>The current solutions; call Complete to finalize intermediate representations.</summary>
		public ReadOnlySpan<T> Solutions {
			get {
				ObjectDisposedException.ThrowIf (handle.IsClosed, this);
				return solutions;
			}
		}
		/// <summary>Strided residual estimates; after Complete, the exact residual vectors.</summary>
		/// <remarks>For CG and GMRES, the first entry estimates the residual norm. For LSMR, the first four entries estimate the normal residual norm, residual norm, operator norm, and condition number.</remarks>
		public ReadOnlySpan<T> Residuals {
			get {
				ObjectDisposedException.ThrowIf (handle.IsClosed, this);
				return residual;
			}
		}

		internal SparseIteration (SparseIterativeMethod method, int rows, int columns, SparseOperator<T> apply, ReadOnlySpan<T> rhs, ReadOnlySpan<T> initial, int count, SparsePreconditioner<T>? preconditioner)
		{
			ArgumentNullException.ThrowIfNull (apply);
			method.Validate (rows, columns);
			SparseValidation.ValidateSolve (rows, columns, count, rhs.Length, initial.Length);
			if (preconditioner is not null && preconditioner.Order != columns)
				throw new ArgumentException ("The preconditioner order must match the number of columns.", nameof (preconditioner));
			scalar = SparseNative.Scalar<T> ();
			this.rows = rows;
			this.columns = columns;
			this.count = count;
			this.preconditioner = preconditioner;
			this.rhs = rhs.ToArray ();
			solutions = initial.ToArray ();
			converged = new byte [count];
			ResidualStride = Math.Max (rows, 4);
			residual = new T [checked(ResidualStride * count)];
			apply (false, SparseOperation.None,
				new SparseReadOnlyDenseMatrix<T> (solutions, columns, count, columns),
				new SparseDenseMatrix<T> (residual, rows, count, ResidualStride));
			for (int column = 0; column < count; column++) {
				for (int row = 0; row < rows; row++)
					residual [column * ResidualStride + row] = Subtract (this.rhs [column * rows + row], residual [column * ResidualStride + row]);
			}
			callback = new SparseOperatorCallback<T> (apply, method.ReportStatus);
			try {
				options = method.Options;
				options.Report = method.ReportStatus is null ? IntPtr.Zero : SparseNative.ReportPointer;
				options.User = callback.Context;
				var nativeOptions = options;
				StateSize = SparseNative.xamarin_sparse_state_size (scalar, &nativeOptions, preconditioner is null ? (byte) 0 : (byte) 1, rows, columns, count);
				SparseNative.Check ();
				using var p = new SparseLease (preconditioner?.Handle);
				nativePreconditioner = p.Value;
				void* storage;
				var result = SparseNative.xamarin_sparse_storage_create (StateSize, &storage);
				SparseNative.Check ();
				handle = new SparseHandle (result, preconditioner?.Handle, callback.Dispose);
				state = storage;
			} catch {
				callback.Dispose ();
				throw;
			}
		}

		static T Subtract (T left, T right)
		{
			if (typeof (T) == typeof (double)) {
				double result = *(double*) &left - *(double*) &right;
				return *(T*) &result;
			}
			if (typeof (T) == typeof (float)) {
				float result = *(float*) &left - *(float*) &right;
				return *(T*) &result;
			}
			if (typeof (T) == typeof (System.Numerics.Complex)) {
				var result = *(System.Numerics.Complex*) &left - *(System.Numerics.Complex*) &right;
				return *(T*) &result;
			}
			var a = *(SparseComplexFloat*) &left;
			var b = *(SparseComplexFloat*) &right;
			var difference = new SparseComplexFloat (a.Real - b.Real, a.Imaginary - b.Imaginary);
			return *(T*) &difference;
		}

		/// <summary>Advances one iteration, ignoring systems already marked converged.</summary>
		/// <param name="convergence">One convergence flag per right-hand side.</param>
		public void Step (ReadOnlySpan<bool> convergence)
		{
			if (convergence.Length != count)
				throw new ArgumentException ("One convergence flag is required per right-hand side.", nameof (convergence));
			for (int index = 0; index < count; index++)
				converged [index] = convergence [index] ? (byte) 1 : (byte) 0;
			Run (iteration);
			iteration = checked(iteration + 1);
		}

		/// <summary>Finalizes the solution estimates and computes exact residuals.</summary>
		public void Complete ()
		{
			if (iteration == 0)
				throw new InvalidOperationException ("Perform at least one iteration before finalizing.");
			Run (-1);
			completed = true;
		}

		void Run (int step)
		{
			using var lease = new SparseLease (handle);
			if (completed || failed)
				throw new InvalidOperationException ("The iteration has completed or failed.");
			callback.Error = null;
			preconditioner?.ResetCallback ();
			var nativeOptions = options;
			fixed (T* b = rhs)
			fixed (T* r = residual)
			fixed (T* x = solutions)
			fixed (byte* c = converged) {
				try {
					SparseNative.xamarin_sparse_iterate (scalar, &nativeOptions, step, c, state, SparseNative.ApplyPointer, callback.Context, rows, columns, count, b, r, ResidualStride, x, nativePreconditioner);
					callback.Error?.Throw ();
					preconditioner?.ThrowCallbackError ();
					SparseNative.Check ();
				} catch {
					failed = true;
					throw;
				}
			}
		}

		/// <summary>Releases the state, callback, and retained preconditioner.</summary>
		public void Dispose () => handle.Dispose ();
	}
}
