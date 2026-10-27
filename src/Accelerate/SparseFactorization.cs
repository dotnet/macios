// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Accelerate {
	/// <summary>Owns aligned storage for a numerical factorization using caller-managed factor storage.</summary>
	/// <remarks>Do not reuse this storage for another factorization until all factors and subfactors using it have been disposed.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparseFactorStorage : IDisposable {
		internal readonly SparseHandle Handle;
		internal readonly void* Data;
		int inUse;
		/// <summary>The capacity in bytes.</summary>
		public nuint Capacity { get; }

		/// <summary>Allocates reusable, 16-byte-aligned factor storage.</summary>
		/// <param name="capacity">The required byte capacity reported by a symbolic factorization.</param>
		public SparseFactorStorage (nuint capacity)
		{
			void* data;
			var handle = SparseNative.xamarin_sparse_storage_create (capacity, &data);
			SparseNative.Check ();
			Handle = new SparseHandle (handle);
			Data = data;
			Capacity = capacity;
		}

		/// <summary>Releases the storage after all dependent factors have been disposed.</summary>
		public void Dispose () => Handle.Dispose ();

		internal void Acquire ()
		{
			if (System.Threading.Interlocked.CompareExchange (ref inUse, 1, 0) != 0)
				throw new InvalidOperationException ("Factor storage is already in use by a live factorization.");
		}
		internal void Release () => System.Threading.Interlocked.Exchange (ref inUse, 0);
	}

	/// <summary>Provides reusable direct solves with owned or caller-provided workspace.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	/// <remarks>Instance operations are not thread-safe. Convenience operations cache workspace, growing it only when needed. Explicit workspace overloads allow stack-allocated buffers.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public abstract unsafe class SparseFactorOperator<T> : IDisposable where T : unmanaged {
		internal readonly SparseHandle Handle;
		internal readonly int Scalar;
		readonly bool subfactor;
		/// <summary>The number of rows of the represented operator.</summary>
		public int RowCount { get; }
		/// <summary>The number of columns of the represented operator.</summary>
		public int ColumnCount { get; }

		internal SparseFactorOperator (SparseHandle handle, int rows, int columns, bool subfactor)
		{
			Handle = handle;
			Scalar = SparseNative.Scalar<T> ();
			RowCount = rows;
			ColumnCount = columns;
			this.subfactor = subfactor;
		}

		/// <summary>Gets the workspace byte count, including up to 15 bytes of alignment padding.</summary>
		/// <param name="rightHandSideCount">The positive number of right-hand sides.</param>
		/// <returns>The minimum length for an arbitrarily aligned workspace span.</returns>
		public nuint GetSolveWorkspaceSize (int rightHandSideCount = 1)
		{
			if (rightHandSideCount <= 0)
				throw new ArgumentOutOfRangeException (nameof (rightHandSideCount));
			using var lease = new SparseLease (Handle);
			nuint size = SparseNative.xamarin_sparse_solve_size (Scalar, lease.Value, rightHandSideCount, subfactor ? (byte) 1 : (byte) 0);
			SparseNative.Check ();
			return checked (size + 15);
		}

		/// <summary>Solves systems using cached workspace.</summary>
		/// <param name="rightHandSides">Contiguous column-major right-hand sides.</param>
		/// <param name="solutions">Contiguous column-major solution destination.</param>
		/// <param name="rightHandSideCount">The positive number of systems.</param>
		public void Solve (ReadOnlySpan<T> rightHandSides, Span<T> solutions, int rightHandSideCount = 1)
			=> Perform (rightHandSides, solutions, rightHandSideCount, default, false, false, false);

		/// <summary>Solves systems without allocating workspace internally.</summary>
		/// <param name="rightHandSides">Contiguous column-major right-hand sides.</param>
		/// <param name="solutions">Contiguous column-major solution destination.</param>
		/// <param name="workspace">Scratch storage of at least the length returned by <see cref="GetSolveWorkspaceSize" />.</param>
		/// <param name="rightHandSideCount">The positive number of systems.</param>
		public void Solve (ReadOnlySpan<T> rightHandSides, Span<T> solutions, Span<byte> workspace, int rightHandSideCount = 1)
			=> Perform (rightHandSides, solutions, rightHandSideCount, workspace, true, false, false);

		/// <summary>Overwrites right-hand sides with solutions using cached workspace.</summary>
		/// <param name="rightHandSidesAndSolutions">Column-major vectors with max(RowCount, ColumnCount) entries each.</param>
		/// <param name="rightHandSideCount">The positive number of systems.</param>
		public void SolveInPlace (Span<T> rightHandSidesAndSolutions, int rightHandSideCount = 1)
			=> Perform (rightHandSidesAndSolutions, rightHandSidesAndSolutions, rightHandSideCount, default, false, false, true);

		/// <summary>Overwrites right-hand sides with solutions without allocating workspace internally.</summary>
		/// <param name="rightHandSidesAndSolutions">Column-major vectors with max(RowCount, ColumnCount) entries each.</param>
		/// <param name="workspace">Scratch storage of at least the length returned by <see cref="GetSolveWorkspaceSize" />.</param>
		/// <param name="rightHandSideCount">The positive number of systems.</param>
		public void SolveInPlace (Span<T> rightHandSidesAndSolutions, Span<byte> workspace, int rightHandSideCount = 1)
			=> Perform (rightHandSidesAndSolutions, rightHandSidesAndSolutions, rightHandSideCount, workspace, true, false, true);

		internal void Perform (ReadOnlySpan<T> input, Span<T> output, int count, Span<byte> workspace, bool suppliedWorkspace, bool multiply, bool inPlace)
		{
			using var lease = new SparseLease (Handle);
			int rows = multiply ? ColumnCount : RowCount;
			int columns = multiply ? RowCount : ColumnCount;
			if (inPlace)
				rows = columns = Math.Max (RowCount, ColumnCount);
			SparseValidation.ValidateSolve (rows, columns, count, input.Length, output.Length);
			if (!inPlace && input.Overlaps (output))
				throw new ArgumentException ("Input and output must not overlap; use the in-place overload instead.");
			fixed (T* b = input)
			fixed (T* x = output)
			fixed (byte* w = workspace) {
				void* scratch = null;
				if (suppliedWorkspace) {
					nuint size = SparseNative.xamarin_sparse_solve_size (Scalar, lease.Value, count, subfactor ? (byte) 1 : (byte) 0);
					SparseNative.Check ();
					scratch = SparseNative.Align (workspace, w, size);
					if (System.Runtime.InteropServices.MemoryMarshal.AsBytes (input).Overlaps (workspace) || System.Runtime.InteropServices.MemoryMarshal.AsBytes (output).Overlaps (workspace))
						throw new ArgumentException ("Workspace must not overlap input or output.", nameof (workspace));
				}
				SparseNative.xamarin_sparse_direct_solve (Scalar, lease.Value, rows, columns, count, b, x, scratch, subfactor ? (multiply ? 3 : 2) : 0);
				SparseNative.Check ();
			}
		}

		/// <summary>Releases this operator and its cached workspace.</summary>
		public void Dispose () => Handle.Dispose ();
	}

	/// <summary>Owns a reusable numerical sparse factorization.</summary>
	/// <typeparam name="T">Double, float, System.Numerics.Complex, or SparseComplexFloat.</typeparam>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparseFactorization<T> : SparseFactorOperator<T> where T : unmanaged {
		readonly SparseMatrix<T> structure;
		readonly long[] starts;
		readonly int[] indices;
		readonly bool transposed;
		readonly bool conjugated;
		/// <summary>The algorithm used to create the factorization.</summary>
		public SparseFactorization Type { get; }

		internal SparseFactorization (SparseHandle handle, SparseMatrix<T> matrix, SparseFactorization type, bool transposed = false, long[]? starts = null, int[]? indices = null, bool conjugated = false)
			: base (handle, type == SparseFactorization.CholeskyAtA ? matrix.ColumnCount : transposed ? matrix.ColumnCount : matrix.RowCount, transposed ? matrix.RowCount : matrix.ColumnCount, false)
		{
			structure = matrix;
			this.starts = starts ?? matrix.CopyStarts ();
			this.indices = indices ?? matrix.CopyIndices ();
			Type = type;
			this.transposed = transposed;
			this.conjugated = conjugated;
		}

		internal static void Validate (SparseMatrix<T> matrix, SparseFactorization type)
		{
			ArgumentNullException.ThrowIfNull (matrix);
			if ((byte) type > 4 && type != SparseFactorization.Qr && type != SparseFactorization.CholeskyAtA && ((byte) type < 80 || (byte) type > 83))
				throw new ArgumentOutOfRangeException (nameof (type));
			if ((byte) type >= 80) SparseNative.RequireComplex ();
			if ((byte) type < 40 && (matrix.RowCount != matrix.ColumnCount || (matrix.Kind != SparseMatrixKind.Symmetric && matrix.Kind != SparseMatrixKind.Hermitian)))
				throw new ArgumentException ("Cholesky and LDL factorizations require a square symmetric or Hermitian matrix.", nameof (matrix));
			if ((byte) type >= 80 && matrix.RowCount != matrix.ColumnCount)
				throw new ArgumentException ("LU requires a square matrix.", nameof (matrix));
			if (type == SparseFactorization.CholeskyAtA && matrix.RowCount < matrix.ColumnCount)
				throw new ArgumentException ("CholeskyAtA requires at least as many rows as columns.", nameof (matrix));
		}

		/// <summary>Performs symbolic and numerical factorization.</summary>
		/// <param name="matrix">The matrix to factor; its input storage is not borrowed by the numerical factor.</param>
		/// <param name="type">The factorization algorithm.</param>
		/// <param name="options">Optional ordering, scaling, and pivoting controls.</param>
		/// <returns>An independently disposable factorization.</returns>
		public static SparseFactorization<T> Create (SparseMatrix<T> matrix, SparseFactorization type, SparseFactorOptions<T>? options = null)
		{
			Validate (matrix, type);
			using var lease = new SparseLease (matrix.Handle);
			using var pin = (options ?? new SparseFactorOptions<T> ()).PinOptions (type, matrix.RowCount, matrix.ColumnCount);
			var descriptor = matrix.Descriptor;
			var nativeOptions = pin.Options;
			int status = 0;
			var result = SparseNative.xamarin_sparse_factor_create (matrix.Scalar, (byte) type, &descriptor, &nativeOptions, IntPtr.Zero, null, null, &status);
			SparseNative.Check (status);
			return new SparseFactorization<T> (new SparseHandle (result), matrix, type);
		}

		/// <summary>Creates an independently disposable copy sharing the native factors.</summary>
		/// <returns>A retained factorization.</returns>
		public SparseFactorization<T> Retain () => View (0);

		/// <summary>Creates a transpose view sharing the native factors.</summary>
		/// <param name="conjugate">Whether to conjugate complex entries.</param>
		/// <returns>An independently disposable view.</returns>
		public SparseFactorization<T> GetTranspose (bool conjugate = false) => View (conjugate && Scalar >= 2 ? 5 : 1);

		SparseFactorization<T> View (int operation)
		{
			using var lease = new SparseLease (Handle);
			if (operation != 0 && transposed && conjugated != (operation == 5))
				throw new InvalidOperationException ("A transpose and a conjugate transpose cannot be combined in a factor view.");
			int kind = 0;
			var result = SparseNative.xamarin_sparse_factor_view (Scalar, lease.Value, operation, &kind);
			SparseNative.Check ();
			return new SparseFactorization<T> (new SparseHandle (result, Handle), structure, Type, operation == 0 ? transposed : !transposed, starts, indices, operation == 0 ? conjugated : operation == 5);
		}

		/// <summary>Creates a factor component that remains valid after this factorization is disposed.</summary>
		/// <param name="kind">The component to extract.</param>
		/// <returns>An independently disposable factor component.</returns>
		public SparseSubfactor<T> CreateSubfactor (SparseSubfactorKind kind)
		{
			if ((int) kind < 1 || (int) kind > 10) throw new ArgumentOutOfRangeException (nameof (kind));
			if ((int) kind >= 9) SparseNative.RequireComplex ();
			using var lease = new SparseLease (Handle);
			int value = (int) kind;
			var result = SparseNative.xamarin_sparse_factor_view (Scalar, lease.Value, 2, &value);
			SparseNative.Check ();
			int n = Math.Min (structure.RowCount, structure.ColumnCount);
			int m = kind == SparseSubfactorKind.Q && Type == SparseFactorization.Qr ? Math.Max (structure.RowCount, structure.ColumnCount) : n;
			return new SparseSubfactor<T> (new SparseHandle (result, Handle), m, n, kind);
		}

		/// <summary>Refactors a matrix with exactly the same nonzero structure.</summary>
		/// <param name="matrix">Updated values with the original compressed-column structure.</param>
		/// <param name="options">Optional updated scaling and pivoting controls.</param>
		/// <param name="workspace">Optional scratch storage; otherwise Accelerate allocates temporary workspace.</param>
		/// <remarks>Pivoted factorizations may allocate additional storage. Retained views cause copy-on-write factor storage.</remarks>
		public void Refactor (SparseMatrix<T> matrix, SparseFactorOptions<T>? options = null, Span<byte> workspace = default)
		{
			Validate (matrix, Type);
			if (transposed || matrix.RowCount != structure.RowCount || matrix.ColumnCount != structure.ColumnCount || matrix.BlockSize != structure.BlockSize || matrix.Kind != structure.Kind || matrix.Triangle != structure.Triangle || matrix.Descriptor.Operation != structure.Descriptor.Operation)
				throw new ArgumentException ("Refactor requires the original matrix structure and an untransposed factor.", nameof (matrix));
			using var matrixLease = new SparseLease (matrix.Handle);
			var descriptor = matrix.Descriptor;
			if (!starts.AsSpan ().SequenceEqual (new ReadOnlySpan<long> (descriptor.Starts, descriptor.Columns + 1)) || !indices.AsSpan ().SequenceEqual (new ReadOnlySpan<int> (descriptor.Indices, checked ((int) descriptor.Starts [descriptor.Columns]))))
				throw new ArgumentException ("Refactor requires exactly the same nonzero structure.", nameof (matrix));
			using var lease = new SparseLease (Handle);
			using var pin = (options ?? new SparseFactorOptions<T> ()).PinOptions (Type, matrix.RowCount, matrix.ColumnCount);
			var nativeOptions = pin.Options;
			fixed (byte* w = workspace) {
				void* scratch = null;
				if (!workspace.IsEmpty) {
					nuint size = SparseNative.xamarin_sparse_refactor_size (Scalar, lease.Value);
					SparseNative.Check ();
					scratch = SparseNative.Align (workspace, w, size);
				}
				int status = 0;
				SparseNative.xamarin_sparse_refactor (Scalar, lease.Value, &descriptor, &nativeOptions, scratch, &status);
				SparseNative.Check (status);
			}
		}

		/// <summary>Gets factorization workspace size including alignment padding.</summary>
		/// <returns>The byte length required by the explicit workspace overload of <see cref="Refactor" />.</returns>
		public nuint GetRefactorWorkspaceSize ()
		{
			using var lease = new SparseLease (Handle);
			nuint size = SparseNative.xamarin_sparse_refactor_size (Scalar, lease.Value);
			SparseNative.Check ();
			return checked (size + 15);
		}

		/// <summary>Counts positive, zero, and negative pivots in an LDL transpose factorization with threshold partial pivoting.</summary>
		/// <returns>The inertia, or an exception if unavailable for this factorization.</returns>
		[SupportedOSPlatform ("ios16.0")]
		[SupportedOSPlatform ("tvos16.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public (int Positive, int Zero, int Negative) GetInertia ()
		{
			if (!(OperatingSystem.IsMacOSVersionAtLeast (13) || OperatingSystem.IsIOSVersionAtLeast (16) || OperatingSystem.IsTvOSVersionAtLeast (16) || OperatingSystem.IsMacCatalystVersionAtLeast (16)))
				throw new PlatformNotSupportedException ("Sparse inertia requires macOS 13, iOS 16, tvOS 16, or Mac Catalyst 16.");
			if (Type != SparseFactorization.LdltTpp)
				throw new InvalidOperationException ("Inertia requires an LDL transpose factorization with threshold partial pivoting.");
			using var lease = new SparseLease (Handle);
			int positive, zero, negative;
			int status = SparseNative.xamarin_sparse_inertia (Scalar, lease.Value, &positive, &zero, &negative);
			SparseNative.Check ();
			if (status != 0)
				throw new InvalidOperationException ($"Accelerate could not determine inertia (status {status}).");
			return (positive, zero, negative);
		}

		/// <summary>Partially refactors an LU factorization after modifying selected entries.</summary>
		/// <param name="updatedIndices">Zero-based (row, column) pairs identifying changed entries.</param>
		/// <param name="update">The full updated matrix, with exactly the original nonzero structure.</param>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		public void Update (ReadOnlySpan<int> updatedIndices, SparseMatrix<T> update)
		{
			SparseNative.RequireComplex ();
			ArgumentNullException.ThrowIfNull (update);
			if (transposed || (byte) Type < 80 || updatedIndices.IsEmpty || updatedIndices.Length % 2 != 0)
				throw new ArgumentException ("Update requires an untransposed LU factor and nonempty (row, column) pairs.");
			for (int i = 0; i < updatedIndices.Length; i += 2) {
				if (updatedIndices [i] < 0 || updatedIndices [i] >= RowCount || updatedIndices [i + 1] < 0 || updatedIndices [i + 1] >= ColumnCount)
					throw new ArgumentException ("Updated coordinates must be in range.", nameof (updatedIndices));
			}
			using var lease = new SparseLease (Handle);
			using var matrixLease = new SparseLease (update.Handle);
			var descriptor = update.Descriptor;
			if (descriptor.Rows != structure.Descriptor.Rows || descriptor.Columns != structure.Descriptor.Columns || descriptor.BlockSize != structure.BlockSize || descriptor.Kind != structure.Descriptor.Kind || descriptor.Triangle != structure.Descriptor.Triangle || descriptor.Operation != structure.Descriptor.Operation || !starts.AsSpan ().SequenceEqual (new ReadOnlySpan<long> (descriptor.Starts, descriptor.Columns + 1)) || !indices.AsSpan ().SequenceEqual (new ReadOnlySpan<int> (descriptor.Indices, checked ((int) descriptor.Starts [descriptor.Columns]))))
				throw new ArgumentException ("Update requires exactly the original nonzero structure.", nameof (update));
			int status = 0;
			fixed (int* indices = updatedIndices)
				SparseNative.xamarin_sparse_update (Scalar, lease.Value, updatedIndices.Length / 2, indices, &descriptor, &status);
			SparseNative.Check (status);
		}
	}

	/// <summary>Owns a reusable factor component for solves and multiplication.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparseSubfactor<T> : SparseFactorOperator<T> where T : unmanaged {
		readonly SparseOperation operation;
		/// <summary>The represented factor component.</summary>
		public SparseSubfactorKind Kind { get; }
		internal SparseSubfactor (SparseHandle handle, int rows, int columns, SparseSubfactorKind kind, SparseOperation operation = SparseOperation.None) : base (handle, rows, columns, true)
		{
			Kind = kind;
			this.operation = operation;
		}

		/// <summary>Creates an independently disposable retained component.</summary>
		/// <returns>A view sharing this component's storage.</returns>
		public SparseSubfactor<T> Retain () => View (3);
		/// <summary>Creates a transposed factor component.</summary>
		/// <param name="conjugate">Whether to conjugate complex entries.</param>
		/// <returns>An independently disposable transpose view.</returns>
		public SparseSubfactor<T> GetTranspose (bool conjugate = false) => View (conjugate && Scalar >= 2 ? 6 : 4);
		SparseSubfactor<T> View (int operation)
		{
			using var lease = new SparseLease (Handle);
			var requested = operation == 6 ? SparseOperation.ConjugateTranspose : SparseOperation.Transpose;
			if (operation != 3 && this.operation != SparseOperation.None && this.operation != requested)
				throw new InvalidOperationException ("A transpose and a conjugate transpose cannot be combined in a subfactor view.");
			int kind = (int) Kind;
			var result = SparseNative.xamarin_sparse_factor_view (Scalar, lease.Value, operation, &kind);
			SparseNative.Check ();
			return new SparseSubfactor<T> (new SparseHandle (result, Handle), operation == 3 ? RowCount : ColumnCount, operation == 3 ? ColumnCount : RowCount, Kind, operation == 3 ? this.operation : this.operation == requested ? SparseOperation.None : requested);
		}

		/// <summary>Multiplies vectors by this factor component.</summary>
		/// <param name="input">Contiguous column-major input vectors.</param>
		/// <param name="destination">Contiguous column-major result vectors.</param>
		/// <param name="columnCount">The number of vectors.</param>
		public void Multiply (ReadOnlySpan<T> input, Span<T> destination, int columnCount = 1)
			=> Perform (input, destination, columnCount, default, false, true, false);
		/// <summary>Multiplies vectors without allocating workspace internally.</summary>
		/// <param name="input">Contiguous column-major input vectors.</param>
		/// <param name="destination">Contiguous column-major result vectors.</param>
		/// <param name="workspace">Workspace of at least the length returned by <see cref="SparseFactorOperator{T}.GetSolveWorkspaceSize" />.</param>
		/// <param name="columnCount">The number of vectors.</param>
		public void Multiply (ReadOnlySpan<T> input, Span<T> destination, Span<byte> workspace, int columnCount = 1)
			=> Perform (input, destination, columnCount, workspace, true, true, false);
		/// <summary>Multiplies vectors in place using cached workspace.</summary>
		/// <param name="values">Vectors with max(RowCount, ColumnCount) entries each.</param>
		/// <param name="columnCount">The number of vectors.</param>
		public void MultiplyInPlace (Span<T> values, int columnCount = 1)
			=> Perform (values, values, columnCount, default, false, true, true);
		/// <summary>Multiplies vectors in place without allocating workspace internally.</summary>
		/// <param name="values">Vectors with max(RowCount, ColumnCount) entries each.</param>
		/// <param name="workspace">Workspace of at least the length returned by <see cref="SparseFactorOperator{T}.GetSolveWorkspaceSize" />.</param>
		/// <param name="columnCount">The number of vectors.</param>
		public void MultiplyInPlace (Span<T> values, Span<byte> workspace, int columnCount = 1)
			=> Perform (values, values, columnCount, workspace, true, true, true);
	}
}
