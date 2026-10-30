// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Accelerate {
	/// <summary>Owns a reusable native or managed iterative preconditioner.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	/// <remarks>Custom callbacks are retained until native use has ended. Instance operations are not thread-safe.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparsePreconditioner<T> : IDisposable where T : unmanaged {
		internal readonly SparseHandle Handle;
		readonly SparseOperatorCallback<T>? callback;
		/// <summary>The number of variables transformed by this preconditioner.</summary>
		public int Order { get; }
		SparsePreconditioner (SparseHandle handle, int order, SparseOperatorCallback<T>? callback = null)
		{
			Handle = handle;
			Order = order;
			this.callback = callback;
		}

		/// <summary>Creates a built-in preconditioner from a sparse matrix.</summary>
		/// <param name="matrix">The matrix used to construct the preconditioner.</param>
		/// <param name="kind">The built-in preconditioning method.</param>
		/// <returns>An independently owned reusable preconditioner.</returns>
		public static SparsePreconditioner<T> Create (SparseMatrix<T> matrix, SparsePreconditionerKind kind)
		{
			ArgumentNullException.ThrowIfNull (matrix);
			if (kind != SparsePreconditionerKind.None && kind != SparsePreconditionerKind.Diagonal && kind != SparsePreconditionerKind.DiagonalScaling)
				throw new ArgumentOutOfRangeException (nameof (kind));
			using var lease = new SparseLease (matrix.Handle);
			var descriptor = matrix.Descriptor;
			var result = SparseNative.xamarin_sparse_preconditioner_create (matrix.Scalar, &descriptor, (int) kind, null);
			SparseNative.Check ();
			return new SparsePreconditioner<T> (new SparseHandle (result), matrix.ColumnCount);
		}

		/// <summary>Creates a preconditioner backed by a managed operator.</summary>
		/// <param name="order">The positive number of variables transformed.</param>
		/// <param name="apply">Applies the preconditioner or its requested transpose.</param>
		/// <returns>A preconditioner retaining the callback.</returns>
		public static SparsePreconditioner<T> Create (int order, SparseOperator<T> apply)
		{
			ArgumentNullException.ThrowIfNull (apply);
			if (order <= 0) throw new ArgumentOutOfRangeException (nameof (order));
			int scalar = SparseNative.Scalar<T> ();
			var callback = new SparseOperatorCallback<T> (apply);
			try {
				var user = new SparseNative.UserPreconditioner { Apply = SparseNative.ApplyPointer, User = callback.Context };
				var result = SparseNative.xamarin_sparse_preconditioner_create (scalar, null, 1, &user);
				SparseNative.Check ();
				return new SparsePreconditioner<T> (new SparseHandle (result, cleanup: callback.Dispose), order, callback);
			} catch {
				callback.Dispose ();
				throw;
			}
		}

		internal void ResetCallback ()
		{
			if (callback is not null) callback.Error = null;
		}
		internal void ThrowCallbackError () => callback?.Error?.Throw ();

		/// <summary>Releases the preconditioner after outstanding native uses finish.</summary>
		public void Dispose () => Handle.Dispose ();
	}
}
