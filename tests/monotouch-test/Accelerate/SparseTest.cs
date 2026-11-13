// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using Accelerate;
using Foundation;
using NUnit.Framework;
using Xamarin.Utils;

namespace MonoTouchFixtures.Accelerate {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class SparseTest {
		static readonly long [] upperStarts = [0, 1, 3];
		static readonly int [] upperIndices = [0, 0, 1];

		[TestCase (SparseFactorization.Cholesky)]
		[TestCase (SparseFactorization.Ldlt)]
		[TestCase (SparseFactorization.LdltUnpivoted)]
		[TestCase (SparseFactorization.LdltSbk)]
		[TestCase (SparseFactorization.LdltTpp)]
		public void SymmetricDouble (SparseFactorization type)
		{
			using var factor = SparseFactorizationDouble.Create (type, 2, 2, upperStarts, upperIndices, [4, 1, 3]);
			double [] solution = new double [2];
			factor.Solve ([6, 7], solution);
			Assert.That (solution, Is.EqualTo (new double [] { 1, 2 }).Within (1e-12), "First solve");
			factor.Solve ([12, 14], solution);
			Assert.That (solution, Is.EqualTo (new double [] { 2, 4 }).Within (1e-12), "Reused factors");
		}

		[TestCase (SparseFactorization.Cholesky)]
		[TestCase (SparseFactorization.Ldlt)]
		[TestCase (SparseFactorization.LdltUnpivoted)]
		[TestCase (SparseFactorization.LdltSbk)]
		[TestCase (SparseFactorization.LdltTpp)]
		public void SymmetricFloat (SparseFactorization type)
		{
			using var factor = SparseFactorizationFloat.Create (type, 2, 2, upperStarts, upperIndices, [4, 1, 3]);
			float [] solution = new float [2];
			factor.Solve ([6, 7], solution);
			Assert.That (solution, Is.EqualTo (new float [] { 1, 2 }).Within (1e-5));
		}

		[Test]
		public void LowerTriangleAndMultipleRightHandSides ()
		{
			using var factor = SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [0, 2, 3], [0, 1, 1], [4, 1, 3], SparseTriangle.Lower);
			double [] solution = new double [4];
			factor.Solve ([6, 7, 12, 14], solution, 2);
			Assert.That (solution, Is.EqualTo (new double [] { 1, 2, 2, 4 }).Within (1e-12));
		}

		[Test]
		public void FloatMultipleRightHandSides ()
		{
			using var factor = SparseFactorizationFloat.Create (SparseFactorization.Cholesky, 2, 2, upperStarts, upperIndices, [4, 1, 3]);
			float [] solution = new float [4];
			factor.Solve ([6, 7, 12, 14], solution, 2);
			Assert.That (solution, Is.EqualTo (new float [] { 1, 2, 2, 4 }).Within (1e-5));
		}

		[Test]
		public void RectangularQr ()
		{
			using var factor = SparseFactorizationDouble.Create (SparseFactorization.Qr, 3, 2, [0, 2, 4], [0, 2, 1, 2], [1, 1, 1, 1]);
			double [] solution = new double [2];
			factor.Solve ([1, 2, 3], solution);
			Assert.That (solution, Is.EqualTo (new double [] { 1, 2 }).Within (1e-12), "Least squares");

			using var underdetermined = SparseFactorizationFloat.Create (SparseFactorization.Qr, 2, 3, [0, 1, 2, 4], [0, 1, 0, 1], [1, 1, 1, 1]);
			float [] minimumNorm = new float [3];
			underdetermined.Solve ([1, 1], minimumNorm);
			Assert.That (minimumNorm, Is.EqualTo (new float [] { 1f / 3, 1f / 3, 2f / 3 }).Within (1e-5), "Minimum norm");
		}

		[Test]
		public void CholeskyAtA ()
		{
			using var factor = SparseFactorizationDouble.Create (SparseFactorization.CholeskyAtA, 3, 2, [0, 2, 4], [0, 2, 1, 2], [1, 1, 1, 1]);
			double [] solution = new double [2];
			factor.Solve ([4, 5], solution);
			Assert.That (solution, Is.EqualTo (new double [] { 1, 2 }).Within (1e-12));
		}

		[TestCase (SparseFactorization.Lu)]
		[TestCase (SparseFactorization.LuUnpivoted)]
		[TestCase (SparseFactorization.LuSpp)]
		[TestCase (SparseFactorization.LuTpp)]
		public void Lu (SparseFactorization type)
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 15, 5, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 18, 5, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 18, 5, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 18, 5, throwIfOtherPlatform: false);
			using var factor = SparseFactorizationDouble.Create (type, 2, 2, [0, 2, 4], [0, 1, 0, 1], [3, 2, 1, 4]);
			double [] solution = new double [2];
			factor.Solve ([5, 10], solution);
			Assert.That (solution, Is.EqualTo (new double [] { 1, 2 }).Within (1e-12));

			using var single = SparseFactorizationFloat.Create (type, 2, 2, [0, 2, 4], [0, 1, 0, 1], [3, 2, 1, 4]);
			float [] singleSolution = new float [2];
			single.Solve ([5, 10], singleSolution);
			Assert.That (singleSolution, Is.EqualTo (new float [] { 1, 2 }).Within (1e-5));
		}

		[Test]
		public void InputsAreNotRetained ()
		{
			long [] starts = [0, 1, 3];
			int [] indices = [0, 0, 1];
			double [] values = [4, 1, 3];
			using var factor = SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, starts, indices, values);
			Array.Clear (starts);
			Array.Clear (indices);
			Array.Clear (values);
			GC.Collect ();
			double [] solution = new double [2];
			factor.Solve ([6, 7], solution);
			Assert.That (solution, Is.EqualTo (new double [] { 1, 2 }).Within (1e-12));
		}

		[Test]
		public void InvalidMatrix ()
		{
			Assert.Throws<ArgumentOutOfRangeException> (() => SparseFactorizationDouble.Create ((SparseFactorization) 255, 2, 2, upperStarts, upperIndices, [4, 1, 3]), "Algorithm");
			Assert.Throws<ArgumentOutOfRangeException> (() => SparseFactorizationDouble.Create (SparseFactorization.Qr, 0, 2, upperStarts, upperIndices, [4, 1, 3]), "Rows");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 3, 2, upperStarts, upperIndices, [4, 1, 3]), "Square");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [0, 1], upperIndices, [4, 1, 3]), "Start count");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [1, 1, 3], upperIndices, [4, 1, 3]), "First offset");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [0, 4, 3], upperIndices, [4, 1, 3]), "Offset bounds");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [0, -1, 3], upperIndices, [4, 1, 3]), "Negative offset");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, upperStarts, [0, 1, 0], [4, 1, 3]), "Unsorted rows");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, upperStarts, [0, 0, 2], [4, 1, 3]), "Row bounds");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, upperStarts, [0, 0, 0], [4, 1, 3]), "Duplicate rows");
			Assert.Throws<ArgumentException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [0, 2, 3], [0, 1, 1], [4, 1, 3]), "Wrong triangle");
			Assert.Throws<ArgumentException> (() => SparseFactorizationFloat.Create (SparseFactorization.Cholesky, 2, 2, upperStarts, upperIndices, [4, 1]), "Value count");
		}

		[Test]
		public void InvalidSolveAndDispose ()
		{
			using var factor = SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, upperStarts, upperIndices, [4, 1, 3]);
			Assert.Throws<ArgumentException> (() => factor.Solve ([6], new double [2]), "RHS length");
			Assert.Throws<ArgumentException> (() => factor.Solve ([6, 7], new double [1]), "Solution length");
			Assert.Throws<ArgumentOutOfRangeException> (() => factor.Solve ([6, 7], new double [2], 0), "RHS count");
			double [] overlap = [6, 7, 0];
			Assert.Throws<ArgumentException> (() => factor.Solve (overlap.AsSpan (0, 2), overlap.AsSpan (1, 2)), "Overlap");
			factor.Dispose ();
			factor.Dispose ();
			Assert.That (factor.Handle, Is.EqualTo (global::ObjCRuntime.NativeHandle.Zero), "Released");
			Assert.Throws<ObjectDisposedException> (() => factor.Solve ([6, 7], new double [2]), "Disposed");

			using var single = SparseFactorizationFloat.Create (SparseFactorization.Cholesky, 2, 2, upperStarts, upperIndices, [4, 1, 3]);
			Assert.Throws<ArgumentException> (() => single.Solve ([6], new float [2]), "Float RHS length");
			single.Dispose ();
			single.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => single.Solve ([6, 7], new float [2]), "Float disposed");
		}

		[Test]
		public void FailedFactorization ()
		{
			Assert.Throws<InvalidOperationException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [0, 1, 2], [0, 1], [-1, 1]), "Indefinite double");
			Assert.Throws<InvalidOperationException> (() => SparseFactorizationFloat.Create (SparseFactorization.Cholesky, 2, 2, [0, 1, 2], [0, 1], [-1, 1]), "Indefinite float");
			Assert.Throws<InvalidOperationException> (() => SparseFactorizationDouble.Create (SparseFactorization.Cholesky, 2, 2, [0, 0, 0], [], []), "Empty double");
			Assert.Throws<InvalidOperationException> (() => SparseFactorizationFloat.Create (SparseFactorization.Cholesky, 2, 2, [0, 0, 0], [], []), "Empty float");
		}
	}
}
