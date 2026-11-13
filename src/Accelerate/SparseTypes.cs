// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System.Runtime.InteropServices;

namespace Accelerate {
	/// <summary>A complex number with single-precision real and imaginary components.</summary>
	[SupportedOSPlatform ("ios18.5")]
	[SupportedOSPlatform ("tvos18.5")]
	[SupportedOSPlatform ("macos15.5")]
	[SupportedOSPlatform ("maccatalyst18.5")]
	[StructLayout (LayoutKind.Sequential)]
	public struct SparseComplexFloat {
		/// <summary>The real component.</summary>
		public float Real;
		/// <summary>The imaginary component.</summary>
		public float Imaginary;

		/// <summary>Creates a single-precision complex number.</summary>
		/// <param name="real">The real component.</param>
		/// <param name="imaginary">The imaginary component.</param>
		public SparseComplexFloat (float real, float imaginary)
		{
			Real = real;
			Imaginary = imaginary;
		}
	}

	/// <summary>Describes the implicit entries of a sparse matrix.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseMatrixKind {
		/// <summary>Only explicitly stored entries are nonzero.</summary>
		Ordinary = 0,
		/// <summary>A triangular matrix.</summary>
		Triangular = 1,
		/// <summary>A triangular matrix with an implicit unit diagonal.</summary>
		UnitTriangular = 2,
		/// <summary>A symmetric matrix, stored using one triangle.</summary>
		Symmetric = 3,
		/// <summary>A Hermitian matrix, stored using one triangle.</summary>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		Hermitian = 7,
	}

	/// <summary>Specifies whether an operator is transposed or conjugate transposed.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseOperation {
		/// <summary>Apply the operator unchanged.</summary>
		None = 111,
		/// <summary>Transpose the operator.</summary>
		Transpose = 112,
		/// <summary>Transpose and conjugate the operator.</summary>
		ConjugateTranspose = 113,
	}

	/// <summary>Controls the fill-reducing permutation used for factorization.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseOrder : byte {
		/// <summary>Use the library's default ordering.</summary>
		Default,
		/// <summary>Use the supplied permutation, or the original order if none is supplied.</summary>
		User,
		/// <summary>Approximate minimum-degree ordering.</summary>
		ApproximateMinimumDegree,
		/// <summary>Nested-dissection ordering.</summary>
		Metis,
		/// <summary>Column approximate minimum-degree ordering.</summary>
		ColumnApproximateMinimumDegree,
		/// <summary>Multithreaded nested-dissection ordering.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		MultithreadedMetis,
	}

	/// <summary>Controls scaling during numerical factorization.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseScaling : byte {
		/// <summary>Use the library's default scaling.</summary>
		Default,
		/// <summary>Use supplied scaling, or no scaling when none is supplied.</summary>
		User,
		/// <summary>Equilibrate using the infinity norm.</summary>
		EquilibrationInfinity,
		/// <summary>Scale using the Hungarian algorithm.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		Hungarian,
		/// <summary>Scale and order LU factors using the Hungarian algorithm.</summary>
		[SupportedOSPlatform ("ios26.0")]
		[SupportedOSPlatform ("tvos26.0")]
		[SupportedOSPlatform ("macos26.0")]
		[SupportedOSPlatform ("maccatalyst26.0")]
		HungarianAndOrdering,
	}

	/// <summary>Identifies a component of a sparse factorization.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseSubfactorKind {
		/// <summary>The row permutation.</summary>
		P = 1,
		/// <summary>The symmetric scaling.</summary>
		S,
		/// <summary>The lower-triangular factor.</summary>
		L,
		/// <summary>The block-diagonal factor.</summary>
		D,
		/// <summary>The combined permutation, lower-triangular factor, and scaling.</summary>
		Plps,
		/// <summary>The orthogonal factor, or LU column permutation.</summary>
		Q,
		/// <summary>The upper-triangular factor.</summary>
		R,
		/// <summary>The upper-triangular factor with its permutation.</summary>
		Rp,
		/// <summary>The LU row scaling.</summary>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		RowScaling,
		/// <summary>The LU column scaling.</summary>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		ColumnScaling,
	}

	/// <summary>Identifies a built-in iterative preconditioner.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparsePreconditionerKind {
		/// <summary>Do not precondition the system.</summary>
		None = 0,
		/// <summary>Use the inverse diagonal.</summary>
		Diagonal = 2,
		/// <summary>Use diagonal scaling.</summary>
		DiagonalScaling = 3,
	}

	/// <summary>Reports the outcome of an iterative solve.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseIterativeStatus {
		/// <summary>The convergence criterion was satisfied.</summary>
		Converged = 0,
		/// <summary>The iteration limit was reached.</summary>
		MaxIterations = 1,
		/// <summary>An invalid parameter was supplied.</summary>
		ParameterError = -1,
		/// <summary>The problem is too ill-conditioned.</summary>
		IllConditioned = -2,
		/// <summary>An internal failure occurred.</summary>
		InternalError = -99,
	}

	/// <summary>Chooses a generalized minimal residual variant.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseGmresVariant {
		/// <summary>Direct quasi-generalized minimal residual.</summary>
		Quasi,
		/// <summary>Restarted generalized minimal residual.</summary>
		Restarted,
		/// <summary>Flexible generalized minimal residual.</summary>
		Flexible,
	}

	/// <summary>Chooses the convergence criterion for least-squares iteration.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseLsmrConvergence {
		/// <summary>Test the residual of the normal equations.</summary>
		Default,
		/// <summary>Use the Fong-Saunders convergence tests.</summary>
		FongSaunders,
	}
}
