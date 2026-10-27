// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Accelerate {
	/// <summary>A read-only dense matrix borrowed for the duration of a sparse operator callback.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public readonly ref struct SparseReadOnlyDenseMatrix<T> where T : unmanaged {
		/// <summary>The number of rows.</summary>
		public int RowCount { get; }
		/// <summary>The number of columns.</summary>
		public int ColumnCount { get; }
		/// <summary>The distance between column starts in elements.</summary>
		public int ColumnStride { get; }
		/// <summary>The borrowed column-major elements, including any padding between columns.</summary>
		public ReadOnlySpan<T> Values { get; }
		internal SparseReadOnlyDenseMatrix (ReadOnlySpan<T> values, int rows, int columns, int stride)
		{
			Values = values;
			RowCount = rows;
			ColumnCount = columns;
			ColumnStride = stride;
		}
		/// <summary>Gets a matrix entry.</summary>
		/// <param name="row">The zero-based row.</param>
		/// <param name="column">The zero-based column.</param>
		/// <returns>The entry at the specified coordinate.</returns>
		public T this [int row, int column] {
			get {
				if ((uint) row >= (uint) RowCount || (uint) column >= (uint) ColumnCount)
					throw new ArgumentOutOfRangeException ();
				return Values [checked(column * ColumnStride + row)];
			}
		}
	}

	/// <summary>A writable dense matrix borrowed for the duration of a sparse operator callback.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public readonly ref struct SparseDenseMatrix<T> where T : unmanaged {
		/// <summary>The number of rows.</summary>
		public int RowCount { get; }
		/// <summary>The number of columns.</summary>
		public int ColumnCount { get; }
		/// <summary>The distance between column starts in elements.</summary>
		public int ColumnStride { get; }
		/// <summary>The borrowed column-major elements, including any padding between columns.</summary>
		public Span<T> Values { get; }
		internal SparseDenseMatrix (Span<T> values, int rows, int columns, int stride)
		{
			Values = values;
			RowCount = rows;
			ColumnCount = columns;
			ColumnStride = stride;
		}
		/// <summary>Gets a reference to a matrix entry.</summary>
		/// <param name="row">The zero-based row.</param>
		/// <param name="column">The zero-based column.</param>
		/// <returns>A writable reference to the specified entry.</returns>
		public ref T this [int row, int column] {
			get {
				if ((uint) row >= (uint) RowCount || (uint) column >= (uint) ColumnCount)
					throw new ArgumentOutOfRangeException ();
				return ref Values [checked(column * ColumnStride + row)];
			}
		}
	}

	/// <summary>Applies a matrix-free operator or preconditioner to borrowed dense matrices.</summary>
	/// <typeparam name="T">The supported real or complex element type.</typeparam>
	/// <param name="accumulate">Add the product to the output when true; otherwise overwrite every output entry.</param>
	/// <param name="operation">The requested transpose or conjugate transpose.</param>
	/// <param name="input">The input vectors, valid only during this callback.</param>
	/// <param name="output">The destination vectors, valid only during this callback.</param>
	/// <remarks>Exceptions are captured at the native boundary and rethrown by the invoking managed operation.</remarks>
	public delegate void SparseOperator<T> (bool accumulate, SparseOperation operation, SparseReadOnlyDenseMatrix<T> input, SparseDenseMatrix<T> output) where T : unmanaged;

	/// <summary>Common controls for iterative sparse solvers.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public abstract class SparseIterativeOptions {
		/// <summary>Creates default iterative convergence controls.</summary>
		protected SparseIterativeOptions () { }
		/// <summary>The maximum iteration count; zero selects the native default.</summary>
		public int MaxIterations { get; set; }
		/// <summary>The absolute convergence tolerance.</summary>
		public double AbsoluteTolerance { get; set; }
		/// <summary>The relative tolerance; zero selects the native default and a negative value disables the relative term.</summary>
		public double RelativeTolerance { get; set; }
		/// <summary>An optional callback receiving native iteration progress messages.</summary>
		public Action<string>? ReportStatus { get; set; }
		internal SparseNative.IterationOptions Native (int method)
		{
			if (MaxIterations < 0 || !double.IsFinite (AbsoluteTolerance) || AbsoluteTolerance < 0 || !double.IsFinite (RelativeTolerance))
				throw new ArgumentException ("Iteration limits and convergence tolerances are invalid.");
			return new SparseNative.IterationOptions {
				Method = method,
				MaxIterations = MaxIterations,
				AbsoluteTolerance = AbsoluteTolerance,
				RelativeTolerance = RelativeTolerance,
			};
		}
	}

	/// <summary>Convergence controls for conjugate gradient.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed class SparseConjugateGradientOptions : SparseIterativeOptions {
		/// <summary>Creates default conjugate gradient options.</summary>
		public SparseConjugateGradientOptions () { }
	}

	/// <summary>Controls generalized minimal residual iteration.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed class SparseGmresOptions : SparseIterativeOptions {
		/// <summary>Creates default generalized minimal residual options.</summary>
		public SparseGmresOptions () { }
		/// <summary>The generalized minimal residual variant.</summary>
		public SparseGmresVariant Variant { get; set; }
		/// <summary>The number of retained orthogonal vectors; nonpositive values select the native default.</summary>
		public int VectorCount { get; set; }
	}

	/// <summary>Controls least-squares minimal residual iteration.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed class SparseLsmrOptions : SparseIterativeOptions {
		/// <summary>Creates default least-squares minimal residual options.</summary>
		public SparseLsmrOptions () { }
		/// <summary>The damping parameter for regularization.</summary>
		public double Damping { get; set; }
		/// <summary>The number of local orthogonalization vectors; nonpositive values disable orthogonalization.</summary>
		public int VectorCount { get; set; }
		/// <summary>The convergence test.</summary>
		public SparseLsmrConvergence ConvergenceTest { get; set; }
		/// <summary>The right-hand-side tolerance for the Fong-Saunders test; zero selects machine epsilon.</summary>
		public double RightHandSideTolerance { get; set; }
		/// <summary>The condition-number limit; zero selects the native default.</summary>
		public double ConditionLimit { get; set; }
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	sealed unsafe class SparseOperatorCallback<T> : SparseCallback where T : unmanaged {
		readonly SparseOperator<T>? apply;
		internal SparseOperatorCallback (SparseOperator<T>? apply, Action<string>? report = null)
		{
			this.apply = apply;
			Report = report;
		}
		internal override void Apply (bool accumulate, SparseOperation operation, int inputRows, int inputColumns, int inputStride, void* input, int outputRows, int outputColumns, int outputStride, void* output)
		{
			var destination = new Span<T> (output, checked((outputColumns - 1) * outputStride + outputRows));
			if (Error is not null) {
				destination.Clear ();
				return;
			}
			try {
				if (apply is null)
					throw new InvalidOperationException ("No sparse operator callback was supplied.");
				apply (accumulate, operation,
					new SparseReadOnlyDenseMatrix<T> (new ReadOnlySpan<T> (input, checked((inputColumns - 1) * inputStride + inputRows)), inputRows, inputColumns, inputStride),
					new SparseDenseMatrix<T> (destination, outputRows, outputColumns, outputStride));
			} catch {
				destination.Clear ();
				throw;
			}
		}
	}

	/// <summary>Describes an immutable iterative solver and its convergence controls.</summary>
	/// <remarks>Options are copied at construction. The solution spans supply initial guesses and receive the resulting iterates.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public sealed unsafe class SparseIterativeMethod {
		internal readonly SparseNative.IterationOptions Options;
		internal readonly Action<string>? ReportStatus;
		SparseIterativeMethod (SparseNative.IterationOptions options, Action<string>? report)
		{
			Options = options;
			ReportStatus = report;
		}

		/// <summary>Creates conjugate gradient for symmetric or Hermitian positive-definite systems.</summary>
		/// <param name="options">Optional convergence controls.</param>
		/// <returns>A conjugate gradient method.</returns>
		public static SparseIterativeMethod ConjugateGradient (SparseConjugateGradientOptions? options = null)
		{
			options ??= new SparseConjugateGradientOptions ();
			return new SparseIterativeMethod (options.Native (0), options.ReportStatus);
		}

		/// <summary>Creates generalized minimal residual for square nonsingular systems.</summary>
		/// <param name="options">Optional variant and convergence controls.</param>
		/// <returns>A generalized minimal residual method.</returns>
		public static SparseIterativeMethod Gmres (SparseGmresOptions? options = null)
		{
			options ??= new SparseGmresOptions ();
			if (options.Variant < SparseGmresVariant.Quasi || options.Variant > SparseGmresVariant.Flexible)
				throw new ArgumentOutOfRangeException (nameof (options));
			var native = options.Native (1);
			native.Variant = (int) options.Variant;
			native.Vectors = options.VectorCount;
			return new SparseIterativeMethod (native, options.ReportStatus);
		}

		/// <summary>Creates least-squares minimal residual for rectangular or singular systems.</summary>
		/// <param name="options">Optional regularization and convergence controls.</param>
		/// <returns>A least-squares minimal residual method.</returns>
		public static SparseIterativeMethod Lsmr (SparseLsmrOptions? options = null)
		{
			options ??= new SparseLsmrOptions ();
			if (!double.IsFinite (options.Damping) || options.Damping < 0 || !double.IsFinite (options.RightHandSideTolerance) || options.RightHandSideTolerance < 0 || !double.IsFinite (options.ConditionLimit) || options.ConditionLimit < 0 || options.ConvergenceTest < SparseLsmrConvergence.Default || options.ConvergenceTest > SparseLsmrConvergence.FongSaunders)
				throw new ArgumentOutOfRangeException (nameof (options));
			var native = options.Native (2);
			native.Damping = options.Damping;
			native.Vectors = options.VectorCount;
			native.Convergence = (int) options.ConvergenceTest;
			native.RhsTolerance = options.RightHandSideTolerance;
			native.ConditionLimit = options.ConditionLimit;
			return new SparseIterativeMethod (native, options.ReportStatus);
		}

		internal void Validate (int rows, int columns)
		{
			if (rows <= 0 || columns <= 0)
				throw new ArgumentException ("Operator dimensions must be positive.");
			if (Options.Method != 2 && rows != columns)
				throw new ArgumentException ("Conjugate gradient and generalized minimal residual require square operators.");
		}

		internal static SparseIterativeStatus CheckResult (int status)
		{
			SparseNative.Check ();
			if (status == (int) SparseIterativeStatus.ParameterError || status == (int) SparseIterativeStatus.InternalError)
				throw new InvalidOperationException ($"Accelerate iterative solve failed with status {(SparseIterativeStatus) status}.");
			return (SparseIterativeStatus) status;
		}

		/// <summary>Solves a sparse system using an optional reusable preconditioner.</summary>
		/// <typeparam name="T">The supported real or complex element type.</typeparam>
		/// <param name="matrix">The operator matrix.</param>
		/// <param name="rightHandSides">Contiguous column-major right-hand sides.</param>
		/// <param name="solutions">Initial guesses, overwritten with the resulting solutions.</param>
		/// <param name="rightHandSideCount">The positive number of systems.</param>
		/// <param name="preconditioner">An optional reusable preconditioner.</param>
		/// <returns>The convergence outcome, including nonconvergence or ill-conditioning.</returns>
		public SparseIterativeStatus Solve<T> (SparseMatrix<T> matrix, ReadOnlySpan<T> rightHandSides, Span<T> solutions, int rightHandSideCount = 1, SparsePreconditioner<T>? preconditioner = null) where T : unmanaged
		{
			ArgumentNullException.ThrowIfNull (matrix);
			using var lease = new SparseLease (matrix.Handle);
			var descriptor = matrix.Descriptor;
			return SolveCore (matrix.RowCount, matrix.ColumnCount, &descriptor, null, rightHandSides, solutions, rightHandSideCount, preconditioner);
		}

		/// <summary>Solves a matrix-free system using a managed operator callback.</summary>
		/// <typeparam name="T">The supported real or complex element type.</typeparam>
		/// <param name="rowCount">The number of operator rows.</param>
		/// <param name="columnCount">The number of operator columns.</param>
		/// <param name="applyOperator">Applies the operator, including requested transpose operations.</param>
		/// <param name="rightHandSides">Contiguous column-major right-hand sides.</param>
		/// <param name="solutions">Initial guesses, overwritten with the resulting solutions.</param>
		/// <param name="rightHandSideCount">The positive number of systems.</param>
		/// <param name="preconditioner">An optional reusable preconditioner.</param>
		/// <returns>The convergence outcome.</returns>
		public SparseIterativeStatus Solve<T> (int rowCount, int columnCount, SparseOperator<T> applyOperator, ReadOnlySpan<T> rightHandSides, Span<T> solutions, int rightHandSideCount = 1, SparsePreconditioner<T>? preconditioner = null) where T : unmanaged
		{
			ArgumentNullException.ThrowIfNull (applyOperator);
			return SolveCore (rowCount, columnCount, null, applyOperator, rightHandSides, solutions, rightHandSideCount, preconditioner);
		}

		SparseIterativeStatus SolveCore<T> (int rows, int columns, SparseNative.Matrix* matrix, SparseOperator<T>? apply, ReadOnlySpan<T> rhs, Span<T> solution, int count, SparsePreconditioner<T>? preconditioner) where T : unmanaged
		{
			Validate (rows, columns);
			SparseValidation.ValidateSolve (rows, columns, count, rhs.Length, solution.Length);
			if (rhs.Overlaps (solution)) throw new ArgumentException ("Right-hand sides and solutions must not overlap.");
			if (preconditioner is not null && preconditioner.Order != columns)
				throw new ArgumentException ("The preconditioner order must match the number of columns.", nameof (preconditioner));
			int scalar = SparseNative.Scalar<T> ();
			using var p = new SparseLease (preconditioner?.Handle);
			using var callback = apply is null && ReportStatus is null ? null : new SparseOperatorCallback<T> (apply, ReportStatus);
			preconditioner?.ResetCallback ();
			var options = Options;
			options.Report = ReportStatus is null ? IntPtr.Zero : SparseNative.ReportPointer;
			options.User = callback?.Context ?? IntPtr.Zero;
			fixed (T* b = rhs)
			fixed (T* x = solution) {
				int status = SparseNative.xamarin_sparse_iterative_solve (scalar, &options, matrix, apply is null ? IntPtr.Zero : SparseNative.ApplyPointer, options.User, rows, columns, count, b, x, p.Value);
				callback?.Error?.Throw ();
				preconditioner?.ThrowCallbackError ();
				return CheckResult (status);
			}
		}

		/// <summary>Creates reusable state for caller-controlled single-step iteration.</summary>
		/// <typeparam name="T">The supported real or complex element type.</typeparam>
		/// <param name="rowCount">The number of operator rows.</param>
		/// <param name="columnCount">The number of operator columns.</param>
		/// <param name="applyOperator">The matrix-free operator.</param>
		/// <param name="rightHandSides">Right-hand sides, copied into the iteration state.</param>
		/// <param name="initialSolutions">Initial guesses, copied into the iteration state.</param>
		/// <param name="rightHandSideCount">The positive number of systems.</param>
		/// <param name="preconditioner">An optional reusable preconditioner retained until iteration is disposed.</param>
		/// <returns>An independently disposable iteration state.</returns>
		public SparseIteration<T> CreateIteration<T> (int rowCount, int columnCount, SparseOperator<T> applyOperator, ReadOnlySpan<T> rightHandSides, ReadOnlySpan<T> initialSolutions, int rightHandSideCount = 1, SparsePreconditioner<T>? preconditioner = null) where T : unmanaged
			=> new SparseIteration<T> (this, rowCount, columnCount, applyOperator, rightHandSides, initialSolutions, rightHandSideCount, preconditioner);
	}
}
