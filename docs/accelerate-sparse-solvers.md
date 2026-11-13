<!--
Copyright (c) Microsoft Corporation.
Licensed under the MIT License.
-->

# Accelerate sparse solvers

The `Accelerate` namespace provides disposable sparse matrices, symbolic and
numerical factorizations, factor components, preconditioners, and iterative
solvers. Matrix and solver types accept `float`, `double`,
`System.Numerics.Complex` (double precision), or `SparseComplexFloat`.
Complex arithmetic and LU require macOS 15.5, iOS 18.5, tvOS 18.5, or
Mac Catalyst 18.5. The newer ordering and scaling algorithms require OS
version 26. `SparseFactorOptions<T>` exposes ordering, ignored rows and columns,
scaling, and pivot tolerances. Scaling buffers contain real `double` values,
including for complex matrices; float-precision solvers convert these values
to and from their native precision.
Complex symmetric (rather than Hermitian) matrices also require OS version 26.

## Direct solves

```csharp
using Accelerate;

// A = [ 4 1 ]
//     [ 1 3 ]
// Store only the upper triangle, in compressed sparse column order.
using var matrix = SparseMatrix<double>.FromCompressedColumns (
    rowCount: 2, columnCount: 2,
    columnStarts: [0, 1, 3],
    rowIndices: [0, 0, 1],
    values: [4, 1, 3],
    kind: SparseMatrixKind.Symmetric);
using var factor = SparseFactorization<double>.Create (
    matrix, SparseFactorization.Cholesky);
Span<double> solution = stackalloc double [2];
factor.Solve ([6, 7], solution); // [1, 2]
```

Factorizations own their numerical data; the source matrix may be disposed after
factorization. Use `Retain`, `GetTranspose`, or `CreateSubfactor` to obtain
independently disposable views. Disposing the original does not invalidate
these views. A transpose followed by a conjugate transpose is not supported
because the native representation cannot express the resulting conjugation-only
view.

Cholesky and LDL factorizations require a square symmetric or Hermitian matrix
stored using one triangle. QR supports rectangular least-squares and
minimum-norm systems. LU currently requires square matrices.
`CholeskyAtA` factors the normal matrix; its right-hand sides contain one value
per original matrix column, not per original row.

Multiple right-hand sides and solutions are contiguous, column-major matrices.
For in-place operations, each vector must have
`max(factor.RowCount, factor.ColumnCount)` entries.

## Workspace and allocations

Convenience solve and subfactor-multiplication methods cache native scratch
storage, increasing its capacity only when necessary. Repeated solves with
the same or fewer right-hand sides do not allocate new workspace.
To provide scratch storage explicitly, including stack storage for small
systems:

```csharp
nuint bytes = factor.GetSolveWorkspaceSize ();
Span<byte> workspace = bytes <= 4096
    ? stackalloc byte [(int) bytes]
    : new byte [checked ((int) bytes)];
factor.Solve ([6, 7], solution, workspace);
```

The size query includes padding for the native 16-byte alignment requirement;
the binding aligns the supplied span internally. Workspace must not overlap the
inputs or outputs. Large or data-dependent workspaces should use reusable
heap storage rather than unbounded stack allocation. Keep the selected buffer
outside repeated-solve loops to reuse it without further allocation.

`SparseSymbolicFactorization<T>` separates symbolic analysis from numerical
factorization. Its `FactorStorageSize` and `FactorWorkspaceSize` properties allow
the caller to allocate `SparseFactorStorage` and reusable scratch storage.
Factor storage remains retained while any dependent factor or view is alive.
It cannot be reused for another factorization during that time.

`Refactor` reuses factors for a matrix with exactly the same nonzero structure.
Pivoting may still require additional storage, and retained views may trigger
native copy-on-write allocation. `Update` performs LU partial refactoring using
changed `(row, column)` pairs and a full updated matrix with identical structure.

## Iterative solves

```csharp
var method = SparseIterativeMethod.ConjugateGradient (
    new SparseConjugateGradientOptions { RelativeTolerance = 1e-10 });
using var preconditioner = SparsePreconditioner<double>.Create (
    matrix, SparsePreconditionerKind.Diagonal);
double[] approximateSolution = new double [2]; // Initial guess.
SparseIterativeStatus status = method.Solve<double> (
    matrix, new double [] { 6, 7 }, approximateSolution,
    preconditioner: preconditioner);
```

Conjugate gradient requires a symmetric or Hermitian positive-definite operator.
`Gmres` supports square nonsingular systems, with quasi, restarted, and flexible
variants (flexible GMRES requires a preconditioner). `Lsmr` supports rectangular
or singular least-squares systems and offers damping and alternative convergence
tests.

The returned status distinguishes convergence, reaching the iteration limit,
and ill-conditioning. Invalid parameters and internal native errors throw
exceptions. Solution spans supply initial guesses as well as receiving results.
Progress reporting is disabled unless a status callback is supplied, and
sparse-matrix solves do not allocate unused managed callback state.

Matrix-free overloads accept `SparseOperator<T>`. The callback receives the
requested transpose operation and borrowed dense-matrix views, including their
column strides. It must overwrite every output entry, or add to it when
`accumulate` is true. Borrowed views cannot be retained beyond the callback.
Exceptions are captured before returning to native code and rethrown when the
managed operation returns.

Custom preconditioners use the same callback shape.
`CreateIteration` returns a disposable `SparseIteration<T>` for custom
convergence tests. Call `Step` with one convergence flag per right-hand side,
inspect the residual estimates, and call `Complete` to finalize the solutions
and exact residuals. Iteration state is retained and reused between steps.

Factorizations and their retained views, preconditioners, and iteration state
are not safe for concurrent operations. Use independent factorizations and
workspaces for concurrent work.

## Native interoperability

`SparseMatrix<T>.FromSparseBlas` converts an existing opaque SparseBLAS matrix
into independently owned solver storage. Its scalar type must match `T`.
The `SafeHandle` overload retains the source during conversion; the `IntPtr`
overload requires the caller to keep the source alive throughout the call.
Neither overload transfers source ownership, and the converted matrix remains
valid after the source is destroyed. SparseBLAS matrix creation and editing
remain separate APIs.

`SparseFactorOptions<T>.SetMemoryAllocator` accepts native allocation and
deallocation function pointers. Both callbacks must be supplied together;
passing null for both restores the system allocator for future analyses.
The allocation callback accepts a byte count and returns a 16-byte-aligned
pointer, or a null pointer on failure. The deallocation callback must accept
null. Callbacks have no context parameter and may run on native threads.
They must not throw, and their code and any associated external state must
remain valid until all derived symbolic analyses, numerical factors, and
subfactors have been released. Managed implementations must use static
`[UnmanagedCallersOnly]` methods.

These are deliberately native callbacks rather than managed delegates so they
work with ahead-of-time compilation without dynamically generated trampolines.
The callbacks are saved by symbolic analysis and also govern later numerical
factorization and refactoring; changing the options does not change existing
factors. They do not replace the binding's matrix or cached workspace allocator.
Use `SparseFactorStorage` and workspace spans for explicit storage ownership
without native allocator callbacks.
