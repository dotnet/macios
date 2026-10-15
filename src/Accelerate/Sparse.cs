// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

namespace Accelerate {
	/// <summary>Specifies the algorithm used to factor a real sparse matrix.</summary>
	[SupportedOSPlatform ("ios11.0")]
	[SupportedOSPlatform ("tvos11.0")]
	[SupportedOSPlatform ("macos10.13")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseFactorization : byte {
		/// <summary>Cholesky factorization of a symmetric positive-definite matrix.</summary>
		Cholesky = 0,
		/// <summary>LDL transpose factorization with the default pivoting strategy.</summary>
		Ldlt = 1,
		/// <summary>LDL transpose factorization without pivoting.</summary>
		LdltUnpivoted = 2,
		/// <summary>LDL transpose factorization with supernode Bunch-Kaufman pivoting.</summary>
		LdltSbk = 3,
		/// <summary>LDL transpose factorization with threshold partial pivoting.</summary>
		LdltTpp = 4,
		/// <summary>QR factorization, including rectangular matrices.</summary>
		Qr = 40,
		/// <summary>Factorization of the normal matrix, without storing the Q factor.</summary>
		CholeskyAtA = 41,
		/// <summary>LU factorization with the default pivoting strategy.</summary>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		Lu = 80,
		/// <summary>LU factorization without numerical pivoting.</summary>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		LuUnpivoted = 81,
		/// <summary>LU factorization with supernode partial pivoting.</summary>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		LuSpp = 82,
		/// <summary>LU factorization with threshold partial pivoting.</summary>
		[SupportedOSPlatform ("ios18.5")]
		[SupportedOSPlatform ("tvos18.5")]
		[SupportedOSPlatform ("macos15.5")]
		[SupportedOSPlatform ("maccatalyst18.5")]
		LuTpp = 83,
	}

	/// <summary>Specifies which triangle stores a symmetric sparse matrix.</summary>
	[SupportedOSPlatform ("ios11.0")]
	[SupportedOSPlatform ("tvos11.0")]
	[SupportedOSPlatform ("macos10.13")]
	[SupportedOSPlatform ("maccatalyst")]
	public enum SparseTriangle : byte {
		/// <summary>The upper triangle is stored.</summary>
		Upper = 0,
		/// <summary>The lower triangle is stored.</summary>
		Lower = 1,
	}

	[SupportedOSPlatform ("ios11.0")]
	[SupportedOSPlatform ("tvos11.0")]
	[SupportedOSPlatform ("macos10.13")]
	[SupportedOSPlatform ("maccatalyst")]
	static class SparseValidation {
		internal static void ValidateMatrix (SparseFactorization type, int rowCount, int columnCount, ReadOnlySpan<long> columnStarts, ReadOnlySpan<int> rowIndices, int valueCount, SparseTriangle triangle)
		{
			if (rowCount <= 0)
				throw new ArgumentOutOfRangeException (nameof (rowCount));
			if (columnCount <= 0)
				throw new ArgumentOutOfRangeException (nameof (columnCount));
			switch ((byte) type) {
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
				if (rowCount != columnCount)
					throw new ArgumentException ("Symmetric factorizations require a square matrix.");
				break;
			case 40:
				break;
			case 41:
				if (rowCount < columnCount)
					throw new ArgumentException ("CholeskyAtA requires at least as many rows as columns.");
				break;
			case 80:
			case 81:
			case 82:
			case 83:
				if (!(OperatingSystem.IsMacOSVersionAtLeast (15, 5) || OperatingSystem.IsIOSVersionAtLeast (18, 5) || OperatingSystem.IsTvOSVersionAtLeast (18, 5) || OperatingSystem.IsMacCatalystVersionAtLeast (18, 5)))
					throw new PlatformNotSupportedException ("LU factorization requires macOS 15.5, iOS 18.5, tvOS 18.5, or Mac Catalyst 18.5.");
				if (rowCount != columnCount)
					throw new ArgumentException ("LU factorization requires a square matrix.");
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (type));
			}
			if (triangle != SparseTriangle.Upper && triangle != SparseTriangle.Lower)
				throw new ArgumentOutOfRangeException (nameof (triangle));
			if (columnStarts.Length != (long) columnCount + 1 || columnStarts [0] != 0 || columnStarts [columnCount] != rowIndices.Length)
				throw new ArgumentException ("Column starts must contain one zero-based offset per column and a final offset equal to the number of row indices.", nameof (columnStarts));
			if (valueCount != rowIndices.Length)
				throw new ArgumentException ("There must be one value per row index.", "values");
			for (int column = 0; column < columnCount; column++) {
				long start = columnStarts [column];
				long end = columnStarts [column + 1];
				if (start < 0 || end < start || end > rowIndices.Length)
					throw new ArgumentException ("Column starts must be nonnegative, nondecreasing offsets within the row indices.", nameof (columnStarts));
				int previous = -1;
				for (int index = (int) start; index < end; index++) {
					int row = rowIndices [index];
					if (row <= previous || row >= rowCount)
						throw new ArgumentException ("Row indices must be in range and strictly increasing within each column.", nameof (rowIndices));
					if ((byte) type < 40 && (triangle == SparseTriangle.Upper ? row > column : row < column))
						throw new ArgumentException ("Symmetric matrix entries must belong to the specified triangle.", nameof (rowIndices));
					previous = row;
				}
			}
		}

		internal static void ValidateSolve (int rows, int columns, int rightHandSideCount, int rhsLength, int solutionLength)
		{
			if (rightHandSideCount <= 0)
				throw new ArgumentOutOfRangeException (nameof (rightHandSideCount));
			if (rhsLength != (long) rows * rightHandSideCount)
				throw new ArgumentException ("The right-hand side length must equal the number of rows times the right-hand side count.", "rightHandSides");
			if (solutionLength != (long) columns * rightHandSideCount)
				throw new ArgumentException ("The solution length must equal the number of columns times the right-hand side count.", "solutions");
		}

		internal static void CheckStatus (int status)
		{
			if (status == -3)
				throw new OutOfMemoryException ("Accelerate could not allocate sparse solver storage.");
			if (status != 0)
				throw new InvalidOperationException ($"Accelerate sparse solver failed with status {status}.");
		}
	}
}
