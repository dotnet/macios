// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Numerics;
using Accelerate;
using Foundation;
using NUnit.Framework;
using Xamarin.Utils;

namespace MonoTouchFixtures.Accelerate {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SparseSolverTest {
		static T Number<T> (Complex value) where T : unmanaged
		{
			if (typeof (T) == typeof (double)) return (T) (object) value.Real;
			if (typeof (T) == typeof (float)) return (T) (object) (float) value.Real;
			if (typeof (T) == typeof (Complex)) return (T) (object) value;
			return (T) (object) new SparseComplexFloat ((float) value.Real, (float) value.Imaginary);
		}

		static Complex Number<T> (T value) where T : unmanaged
		{
			if (typeof (T) == typeof (double)) return (double) (object) value;
			if (typeof (T) == typeof (float)) return (float) (object) value;
			if (typeof (T) == typeof (Complex)) return (Complex) (object) value;
			var number = (SparseComplexFloat) (object) value;
			return new Complex (number.Real, number.Imaginary);
		}

		static T[] Numbers<T> (params Complex[] values) where T : unmanaged
		{
			var result = new T [values.Length];
			for (int i = 0; i < values.Length; i++) result [i] = Number<T> (values [i]);
			return result;
		}

		static void Equal<T> (ReadOnlySpan<T> actual, ReadOnlySpan<T> expected, string message) where T : unmanaged
		{
			Assert.That (actual.Length, Is.EqualTo (expected.Length), message + " length");
			double tolerance = typeof (T) == typeof (float) || typeof (T) == typeof (SparseComplexFloat) ? 2e-4 : 1e-9;
			for (int i = 0; i < actual.Length; i++)
				Assert.That ((Number (actual [i]) - Number (expected [i])).Magnitude, Is.LessThan (tolerance), $"{message} [{i}]");
		}

		static void RequireModernOS ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 15, 5, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 18, 5, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 18, 5, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 18, 5, throwIfOtherPlatform: false);
		}

		static void EachScalar (Action doubleTest, Action floatTest, Action complexDoubleTest, Action complexFloatTest, int scalar)
		{
			if (scalar >= 2) RequireModernOS ();
			switch (scalar) {
			case 0: doubleTest (); break;
			case 1: floatTest (); break;
			case 2: complexDoubleTest (); break;
			case 3: complexFloatTest (); break;
			default: throw new ArgumentOutOfRangeException (nameof (scalar));
			}
		}

		static bool IsComplex<T> () where T : unmanaged => typeof (T) == typeof (Complex) || typeof (T) == typeof (SparseComplexFloat);

		static SparseMatrix<T> Symmetric<T> (double diagonal = 4) where T : unmanaged
			=> SparseMatrix<T>.FromCompressedColumns (2, 2, [0, 1, 3], [0, 0, 1], Numbers<T> (diagonal, new Complex (1, IsComplex<T> () ? 1 : 0), 3), IsComplex<T> () ? SparseMatrixKind.Hermitian : SparseMatrixKind.Symmetric);

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void Direct (int scalar) => EachScalar (Direct<double>, Direct<float>, Direct<Complex>, Direct<SparseComplexFloat>, scalar);

		static void Direct<T> () where T : unmanaged
		{
			using var matrix = Symmetric<T> ();
			T[] expected = Numbers<T> (new Complex (1, 1), new Complex (2, -1), new Complex (2, 2), new Complex (4, -2));
			T[] rhs = new T [4];
			matrix.Multiply (expected, rhs, Number<T> (1), 2);
			foreach (var type in new [] { SparseFactorization.Cholesky, SparseFactorization.Ldlt, SparseFactorization.LdltUnpivoted, SparseFactorization.LdltSbk, SparseFactorization.LdltTpp }) {
				using var factor = SparseFactorization<T>.Create (matrix, type);
				var actual = new T [4];
				factor.Solve (rhs, actual, 2);
				Equal<T> (actual, expected, type + " cached");
				Span<byte> workspace = new byte [checked ((int) factor.GetSolveWorkspaceSize (2))];
				factor.Solve (rhs, actual, workspace, 2);
				Equal<T> (actual, expected, type + " caller workspace");
				rhs.CopyTo (actual, 0);
				factor.SolveInPlace (actual, workspace, 2);
				Equal<T> (actual, expected, type + " in-place");
				using var copy = factor.Retain ();
				factor.Dispose ();
				copy.Solve (rhs, actual, 2);
				Equal<T> (actual, expected, type + " retain");
			}
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void SymbolicAndRefactor (int scalar) => EachScalar (SymbolicAndRefactor<double>, SymbolicAndRefactor<float>, SymbolicAndRefactor<Complex>, SymbolicAndRefactor<SparseComplexFloat>, scalar);

		static void SymbolicAndRefactor<T> () where T : unmanaged
		{
			using var matrix = Symmetric<T> ();
			using var symbolic = SparseSymbolicFactorization<T>.Create (matrix, SparseFactorization.Cholesky);
			using var retained = symbolic.Retain ();
			symbolic.Dispose ();
			using var storage = new SparseFactorStorage (retained.FactorStorageSize);
			Span<byte> workspace = new byte [checked ((int) retained.FactorWorkspaceSize)];
			using var factor = retained.Factor (matrix, storage: storage, workspace: workspace);
			storage.Dispose ();
			retained.Dispose ();
			var expected = Numbers<T> (new Complex (1, 1), new Complex (2, -1));
			var rhs = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			var actual = new T [2];
			factor.Solve (rhs, actual);
			Equal<T> (actual, expected, "Symbolic");
			using var updated = Symmetric<T> (8);
			Span<byte> refactorWorkspace = new byte [checked ((int) factor.GetRefactorWorkspaceSize ())];
			factor.Refactor (updated, workspace: refactorWorkspace);
			updated.Multiply (expected, rhs, Number<T> (1));
			factor.Solve (rhs, actual);
			Equal<T> (actual, expected, "Refactor");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void SubfactorAndTranspose (int scalar) => EachScalar (SubfactorAndTranspose<double>, SubfactorAndTranspose<float>, SubfactorAndTranspose<Complex>, SubfactorAndTranspose<SparseComplexFloat>, scalar);

		static void SubfactorAndTranspose<T> () where T : unmanaged
		{
			using var matrix = Symmetric<T> ();
			using var factor = SparseFactorization<T>.Create (matrix, SparseFactorization.Cholesky);
			using var lower = factor.CreateSubfactor (SparseSubfactorKind.L);
			using var transpose = lower.GetTranspose (conjugate: true);
			using var retained = lower.Retain ();
			factor.Dispose ();
			lower.Dispose ();
			var expected = Numbers<T> (new Complex (1, 1), new Complex (2, -1));
			var rhs = new T [2];
			var actual = new T [2];
			retained.Multiply (expected, rhs);
			retained.Solve (rhs, actual);
			Equal<T> (actual, expected, "Lower");
			transpose.Multiply (expected, rhs);
			transpose.Solve (rhs, actual);
			Equal<T> (actual, expected, "Conjugate transpose");
			using var full = SparseFactorization<T>.Create (matrix, SparseFactorization.Cholesky);
			using var transposed = full.GetTranspose (conjugate: true);
			using var transposedMatrix = matrix.GetTranspose (conjugate: true);
			matrix.Dispose ();
			transposedMatrix.Multiply (expected, rhs, Number<T> (1));
			transposed.Solve (rhs, actual);
			Equal<T> (actual, expected, "Factor transpose");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void CoordinateAndBlockMatrix (int scalar) => EachScalar (CoordinateAndBlockMatrix<double>, CoordinateAndBlockMatrix<float>, CoordinateAndBlockMatrix<Complex>, CoordinateAndBlockMatrix<SparseComplexFloat>, scalar);

		static void CoordinateAndBlockMatrix<T> () where T : unmanaged
		{
			using var coordinate = SparseMatrix<T>.FromCoordinates (2, 2, [1, 0, 0, 1, 0], [1, 0, 1, 0, 0], Numbers<T> (4, 2, 1, 2, 1));
			var x = Numbers<T> (1, 2);
			var actual = new T [2];
			coordinate.Multiply (x, actual, Number<T> (1));
			Equal<T> (actual, Numbers<T> (5, 10), "Coordinate duplicate sum");
			coordinate.Multiply (x, actual, Number<T> (2), accumulate: true);
			Equal<T> (actual, Numbers<T> (15, 30), "Multiply-add");
			using var blocks = SparseMatrix<T>.FromCompressedColumns (2, 2, [0, 1], [0], Numbers<T> (3, 2, 1, 4), blockSize: 2);
			blocks.Multiply (x, actual, Number<T> (1));
			Equal<T> (actual, Numbers<T> (5, 10), "Block matrix");
			using var transpose = blocks.GetTranspose ();
			transpose.Multiply (x, actual, Number<T> (1));
			Equal<T> (actual, Numbers<T> (7, 9), "Block transpose");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void Iterative (int scalar) => EachScalar (Iterative<double>, Iterative<float>, Iterative<Complex>, Iterative<SparseComplexFloat>, scalar);

		static void Iterative<T> () where T : unmanaged
		{
			using var matrix = Symmetric<T> ();
			var expected = Numbers<T> (new Complex (1, 1), new Complex (2, -1));
			var rhs = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			foreach (var (method, requiresPreconditioner) in new [] {
				(SparseIterativeMethod.ConjugateGradient (), false),
				(SparseIterativeMethod.Gmres (), false),
				(SparseIterativeMethod.Gmres (new SparseGmresOptions { Variant = SparseGmresVariant.Restarted }), false),
				(SparseIterativeMethod.Gmres (new SparseGmresOptions { Variant = SparseGmresVariant.Flexible }), true),
				(SparseIterativeMethod.Lsmr (), false),
			}) {
				foreach (var kind in new [] { SparsePreconditionerKind.None, SparsePreconditionerKind.Diagonal, SparsePreconditionerKind.DiagonalScaling }) {
					using var preconditioner = SparsePreconditioner<T>.Create (matrix, kind);
					var actual = new T [2];
					if (requiresPreconditioner && kind == SparsePreconditionerKind.None) {
						Assert.Throws<InvalidOperationException> (() => method.Solve<T> (matrix, rhs, actual, preconditioner: preconditioner), "Flexible GMRES needs a preconditioner");
						continue;
					}
					var status = method.Solve<T> (matrix, rhs, actual, preconditioner: preconditioner);
					Assert.That (status, Is.EqualTo (SparseIterativeStatus.Converged), "Converged " + kind);
					Equal<T> (actual, expected, "Iterative " + kind);
				}
			}
		}

		static SparseOperator<T> Operator<T> () where T : unmanaged => (accumulate, operation, input, output) => {
			bool transposed = operation != SparseOperation.None;
			bool conjugate = operation == SparseOperation.ConjugateTranspose;
			Complex offDiagonal = new Complex (1, IsComplex<T> () ? 1 : 0);
			for (int column = 0; column < input.ColumnCount; column++) {
				Complex a = transposed ? Complex.Conjugate (offDiagonal) : offDiagonal;
				Complex b = transposed ? offDiagonal : Complex.Conjugate (offDiagonal);
				if (conjugate) { a = Complex.Conjugate (a); b = Complex.Conjugate (b); }
				Complex x = Number (input [0, column]), y = Number (input [1, column]);
				output [0, column] = Number<T> ((accumulate ? Number (output [0, column]) : Complex.Zero) + 4 * x + a * y);
				output [1, column] = Number<T> ((accumulate ? Number (output [1, column]) : Complex.Zero) + b * x + 3 * y);
			}
		};

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void CallbacksAndSteps (int scalar) => EachScalar (CallbacksAndSteps<double>, CallbacksAndSteps<float>, CallbacksAndSteps<Complex>, CallbacksAndSteps<SparseComplexFloat>, scalar);

		static void CallbacksAndSteps<T> () where T : unmanaged
		{
			using var matrix = Symmetric<T> ();
			var expected = Numbers<T> (new Complex (1, 1), new Complex (2, -1));
			var rhs = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			var method = SparseIterativeMethod.ConjugateGradient ();
			using var identity = SparsePreconditioner<T>.Create (2, (accumulate, operation, input, output) => {
				Assert.That (accumulate, Is.False, "Preconditioner is not accumulated");
				for (int column = 0; column < input.ColumnCount; column++)
					for (int row = 0; row < input.RowCount; row++)
						output [row, column] = input [row, column];
			});
			var actual = new T [2];
			Assert.That (method.Solve<T> (2, 2, Operator<T> (), rhs, actual, preconditioner: identity), Is.EqualTo (SparseIterativeStatus.Converged));
			Equal<T> (actual, expected, "Matrix-free");
			using var iteration = method.CreateIteration<T> (2, 2, Operator<T> (), rhs, new T [2], preconditioner: identity);
			identity.Dispose ();
			for (int step = 0; step < 2; step++)
				iteration.Step ([false]);
			iteration.Complete ();
			Equal<T> (iteration.Solutions, expected, "Single-step");
			Assert.Throws<InvalidOperationException> (() => iteration.Step ([false]), "Completed state");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void LuAndPartialUpdate (int scalar)
		{
			RequireModernOS ();
			EachScalar (LuAndPartialUpdate<double>, LuAndPartialUpdate<float>, LuAndPartialUpdate<Complex>, LuAndPartialUpdate<SparseComplexFloat>, scalar);
		}

		static void LuAndPartialUpdate<T> () where T : unmanaged
		{
			using var matrix = SparseMatrix<T>.FromCompressedColumns (2, 2, [0, 2, 4], [0, 1, 0, 1], Numbers<T> (new Complex (3, 1), new Complex (2, -1), new Complex (1, -2), new Complex (4, 1)));
			T[] expected = Numbers<T> (new Complex (1, 1), new Complex (2, -1));
			var rhs = new T [2];
			var actual = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			foreach (var type in new [] { SparseFactorization.Lu, SparseFactorization.LuUnpivoted, SparseFactorization.LuSpp, SparseFactorization.LuTpp }) {
				using var factor = SparseFactorization<T>.Create (matrix, type);
				factor.Solve (rhs, actual);
				Equal<T> (actual, expected, type.ToString ());
			}
			using var updated = SparseMatrix<T>.FromCompressedColumns (2, 2, [0, 2, 4], [0, 1, 0, 1], Numbers<T> (new Complex (6, 1), new Complex (2, -1), new Complex (1, -2), new Complex (4, 1)));
			using var lu = SparseFactorization<T>.Create (matrix, SparseFactorization.Lu);
			lu.Update ([0, 0], updated);
			updated.Multiply (expected, rhs, Number<T> (1));
			lu.Solve (rhs, actual);
			Equal<T> (actual, expected, "Partial refactor");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void RectangularQrAndLsmr (int scalar) => EachScalar (RectangularQrAndLsmr<double>, RectangularQrAndLsmr<float>, RectangularQrAndLsmr<Complex>, RectangularQrAndLsmr<SparseComplexFloat>, scalar);

		static void RectangularQrAndLsmr<T> () where T : unmanaged
		{
			using var matrix = SparseMatrix<T>.FromCompressedColumns (4, 2, [0, 3, 6], [0, 2, 3, 1, 2, 3], Numbers<T> (new Complex (1, 1), 1, 2, new Complex (1, -1), 1, 1));
			T[] expected = Numbers<T> (new Complex (1, 1), new Complex (2, -1));
			var rhs = new T [4];
			var actual = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			using var qr = SparseFactorization<T>.Create (matrix, SparseFactorization.Qr);
			qr.Solve (rhs, actual);
			Equal<T> (actual, expected, "Rectangular QR");
			using var q = qr.CreateSubfactor (SparseSubfactorKind.Q);
			Assert.That (q.RowCount, Is.EqualTo (4), "Q rows");
			Assert.That (q.ColumnCount, Is.EqualTo (2), "Q columns");
			var product = new T [4];
			q.Multiply (expected, product);
			q.Solve (product, actual);
			Equal<T> (actual, expected, "Q multiply and solve");
			Array.Clear (actual);
			Assert.That (SparseIterativeMethod.Lsmr ().Solve<T> (matrix, rhs, actual), Is.EqualTo (SparseIterativeStatus.Converged));
			Equal<T> (actual, expected, "Rectangular LSMR");
			using var normal = SparseFactorization<T>.Create (matrix, SparseFactorization.CholeskyAtA);
			using var conjugate = matrix.GetTranspose (conjugate: true);
			var normalRhs = new T [2];
			conjugate.Multiply (rhs, normalRhs, Number<T> (1));
			normal.Solve (normalRhs, actual);
			Equal<T> (actual, expected, "Normal equations");
			using var underdetermined = SparseFactorization<T>.Create (conjugate, SparseFactorization.Qr, new SparseFactorOptions<T> { Order = SparseOrder.User, Permutation = new int [] { 1, 0 } });
			var minimumNorm = new T [4];
			underdetermined.Solve (expected, minimumNorm);
			conjugate.Multiply (minimumNorm, actual, Number<T> (1));
			Equal<T> (actual, expected, "Underdetermined QR with user ordering");
			using var analysis = SparseSymbolicFactorization<T>.Create (conjugate, SparseFactorization.Qr);
			using var underdeterminedSymbolic = analysis.Factor (conjugate);
			underdeterminedSymbolic.Solve (expected, minimumNorm);
			conjugate.Multiply (minimumNorm, actual, Number<T> (1));
			Equal<T> (actual, expected, "Underdetermined symbolic QR");
		}

		[Test]
		public void Inertia ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 13, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 16, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 16, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 16, 0, throwIfOtherPlatform: false);
			using var matrix = SparseMatrix<double>.FromCompressedColumns (2, 2, [0, 1, 2], [0, 1], [2, -3], SparseMatrixKind.Symmetric);
			using var factor = SparseFactorization<double>.Create (matrix, SparseFactorization.LdltTpp);
			Assert.That (factor.GetInertia (), Is.EqualTo ((1, 0, 1)));
		}

		[Test]
		public void CallbackExceptions ()
		{
			var method = SparseIterativeMethod.ConjugateGradient ();
			var error = new ApplicationException ("Operator failure");
			var actual = new double [2];
			var thrown = Assert.Throws<ApplicationException> (() => method.Solve<double> (2, 2, (accumulate, operation, input, output) => throw error, new double [] { 1, 2 }, actual));
			Assert.That (thrown, Is.SameAs (error), "Original callback exception");
			using var matrix = Symmetric<double> ();
			using var preconditioner = SparsePreconditioner<double>.Create (2, (accumulate, operation, input, output) => throw error);
			thrown = Assert.Throws<ApplicationException> (() => method.Solve<double> (matrix, new double [] { 1, 2 }, actual, preconditioner: preconditioner));
			Assert.That (thrown, Is.SameAs (error), "Preconditioner exception");
			var reporting = SparseIterativeMethod.ConjugateGradient (new SparseConjugateGradientOptions { ReportStatus = _ => throw error });
			thrown = Assert.Throws<ApplicationException> (() => reporting.Solve<double> (matrix, new double [] { 1, 2 }, actual));
			Assert.That (thrown, Is.SameAs (error), "Status callback exception");
		}

		[Test]
		public void StackWorkspace ()
		{
			using var matrix = Symmetric<double> ();
			using var factor = SparseFactorization<double>.Create (matrix, SparseFactorization.Cholesky);
			nuint size = factor.GetSolveWorkspaceSize ();
			Assert.That (size, Is.LessThanOrEqualTo ((nuint) 4096), "Bounded stack allocation");
			Span<byte> workspace = stackalloc byte [checked ((int) size)];
			Span<double> actual = stackalloc double [2];
			factor.Solve ([6, 7], actual, workspace);
			Equal<double> (actual, [1, 2], "Stack workspace");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void FailedRefactor (int scalar) => EachScalar (FailedRefactor<double>, FailedRefactor<float>, FailedRefactor<Complex>, FailedRefactor<SparseComplexFloat>, scalar);

		static void FailedRefactor<T> () where T : unmanaged
		{
			using var matrix = Symmetric<T> ();
			using var zero = SparseMatrix<T>.FromCompressedColumns (2, 2, [0, 1, 3], [0, 0, 1], Numbers<T> (0, 0, 0), matrix.Kind);
			using var factor = SparseFactorization<T>.Create (matrix, SparseFactorization.Cholesky);
			Assert.Throws<InvalidOperationException> (() => factor.Refactor (zero), "Singular refactor");
			var actual = new T [2];
			Assert.Throws<InvalidOperationException> (() => factor.Solve (Numbers<T> (6, 7), actual), "Invalid solve");
			Assert.Throws<InvalidOperationException> (() => factor.Retain (), "Invalid retain");
			Assert.Throws<InvalidOperationException> (() => factor.CreateSubfactor (SparseSubfactorKind.L), "Invalid subfactor");
			factor.Refactor (matrix);
			var expected = Numbers<T> (1, 2);
			var rhs = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			factor.Solve (rhs, actual);
			Equal<T> (actual, expected, "Recovered factor");
		}

		[TestCase (2)]
		[TestCase (3)]
		public void ComplexSymmetric (int scalar)
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 26, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 26, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 26, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 26, 0, throwIfOtherPlatform: false);
			EachScalar (() => throw new InvalidOperationException (), () => throw new InvalidOperationException (), ComplexSymmetric<Complex>, ComplexSymmetric<SparseComplexFloat>, scalar);
		}

		static void ComplexSymmetric<T> () where T : unmanaged
		{
			using var matrix = SparseMatrix<T>.FromCompressedColumns (2, 2, [0, 1, 3], [0, 0, 1],
				Numbers<T> (new Complex (4, 1), new Complex (1, 2), new Complex (3, 1)), SparseMatrixKind.Symmetric);
			var expected = Numbers<T> (new Complex (1, -1), new Complex (2, 1));
			var rhs = new T [2];
			var actual = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			using var factor = SparseFactorization<T>.Create (matrix, SparseFactorization.LdltTpp);
			factor.Solve (rhs, actual);
			Equal<T> (actual, expected, "Complex symmetric LDL");
			factor.Refactor (matrix);
			factor.Solve (rhs, actual);
			Equal<T> (actual, expected, "Complex symmetric refactor");
		}

		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		[TestCase (3)]
		public void OptionsAndFailures (int scalar) => EachScalar (OptionsAndFailures<double>, OptionsAndFailures<float>, OptionsAndFailures<Complex>, OptionsAndFailures<SparseComplexFloat>, scalar);

		static void OptionsAndFailures<T> () where T : unmanaged
		{
			using var matrix = Symmetric<T> ();
			var expected = Numbers<T> (1, 2);
			var rhs = new T [2];
			var actual = new T [2];
			matrix.Multiply (expected, rhs, Number<T> (1));
			var options = new SparseFactorOptions<T> {
				Order = SparseOrder.User, Permutation = new int [] { 1, 0 },
				Scaling = SparseScaling.User, ScalingValues = new double [] { 2, 3 },
				PivotTolerance = 0.05, ZeroTolerance = 1e-14,
			};
			using var factor = SparseFactorization<T>.Create (matrix, SparseFactorization.LdltTpp, options);
			factor.Solve (rhs, actual);
			Equal<T> (actual, expected, "User ordering and scaling");
			using var symbolic = SparseSymbolicFactorization<T>.Create (matrix, SparseFactorization.Cholesky);
			using var storage = new SparseFactorStorage (symbolic.FactorStorageSize);
			using var first = symbolic.Factor (matrix, storage: storage);
			Assert.Throws<InvalidOperationException> (() => symbolic.Factor (matrix, storage: storage), "Live factor storage cannot be reused");
			using var retained = first.Retain ();
			first.Dispose ();
			Assert.Throws<InvalidOperationException> (() => symbolic.Factor (matrix, storage: storage), "Retained view keeps storage in use");
			retained.Dispose ();
			using var reused = symbolic.Factor (matrix, storage: storage);
			reused.Solve (rhs, actual);
			Equal<T> (actual, expected, "Reused factor storage");
			using var otherStructure = SparseMatrix<T>.FromCompressedColumns (2, 2, [0, 1, 2], [0, 1], Numbers<T> (4, 3), IsComplex<T> () ? SparseMatrixKind.Hermitian : SparseMatrixKind.Symmetric);
			Assert.Throws<ArgumentException> (() => factor.Refactor (otherStructure), "Changed structure");
			Assert.Throws<ArgumentException> (() => symbolic.Factor (otherStructure), "Changed symbolic structure");
			options.Permutation = new int [] { 0, 0 };
			Assert.Throws<ArgumentException> (() => SparseFactorization<T>.Create (matrix, SparseFactorization.Cholesky, options), "Invalid user permutation");
		}

		[Test]
		public void ValidationAndAllocation ()
		{
			using var empty = SparseMatrix<double>.FromCompressedColumns (2, 2, [0, 0, 0], [], []);
			using var emptyCoordinates = SparseMatrix<double>.FromCoordinates (2, 2, [], [], []);
			var zeroProduct = new double [] { 1, 1 };
			empty.Multiply ([1, 2], zeroProduct, 1);
			Assert.That (zeroProduct, Is.EqualTo (new double [] { 0, 0 }), "Empty compressed matrix");
			emptyCoordinates.Multiply ([1, 2], zeroProduct, 1);
			Assert.That (zeroProduct, Is.EqualTo (new double [] { 0, 0 }), "Empty coordinate matrix");
			using var matrix = Symmetric<double> ();
			using var factor = SparseFactorization<double>.Create (matrix, SparseFactorization.Cholesky);
			var rhs = new double [] { 6, 7 };
			var actual = new double [2];
			for (int i = 0; i < 10; i++) factor.Solve (rhs, actual);
			long before = GC.GetAllocatedBytesForCurrentThread ();
			for (int i = 0; i < 100; i++) factor.Solve (rhs, actual);
			Assert.That (GC.GetAllocatedBytesForCurrentThread () - before, Is.EqualTo (0), "Repeated solves allocate no managed memory");
			var method = SparseIterativeMethod.ConjugateGradient ();
			for (int i = 0; i < 10; i++) {
				Array.Clear (actual);
				method.Solve<double> (matrix, rhs, actual);
			}
			before = GC.GetAllocatedBytesForCurrentThread ();
			for (int i = 0; i < 100; i++) {
				Array.Clear (actual);
				method.Solve<double> (matrix, rhs, actual);
			}
			Assert.That (GC.GetAllocatedBytesForCurrentThread () - before, Is.EqualTo (0), "Sparse iterative solves allocate no unused managed callbacks");
			Equal<double> (actual, [1, 2], "Repeated iterative solves");
			Assert.Throws<ArgumentException> (() => factor.Solve (rhs, actual, new byte [1]), "Short workspace");
			Assert.Throws<ArgumentException> (() => factor.Solve (rhs, rhs), "Aliased input");
			Assert.Throws<ArgumentException> (() => SparseMatrix<double>.FromCompressedColumns (2, 2, [0, 2, 2], [0, 0], [1, 2]), "Duplicate row");
			Assert.Throws<ArgumentException> (() => SparseMatrix<double>.FromCoordinates (2, 2, [2], [0], [1]), "Coordinate bounds");
			factor.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => factor.Solve (rhs, actual), "Disposed factor");
			matrix.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => matrix.Multiply (rhs, actual, 1), "Disposed matrix");
		}
	}
}
