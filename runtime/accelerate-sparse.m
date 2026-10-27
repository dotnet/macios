// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#include <Accelerate/Accelerate.h>
#include <new>
#include <cstring>
#include <limits>
#include <atomic>

// Generic managed entry points check the scalar type's OS requirements before dispatching here.
#pragma clang diagnostic ignored "-Wunguarded-availability-new"

struct XamarinSparseMatrix {
	int rows, columns;
	long *starts;
	int *indices;
	void *data;
	int kind, triangle, block_size, operation;
};

struct XamarinSparseOptions {
	int order;
	int *permutation;
	int *ignored;
	int scaling;
	void *scale;
	double pivot_tolerance, zero_tolerance;
};

typedef void (*XamarinSparseOperator) (void *, uint8_t, int, int, int, int, void *, int, int, int, void *);
typedef void (*XamarinSparseReport) (void *, const char *);

struct XamarinSparseIterationOptions {
	int method, variant, vectors, max_iterations, convergence;
	double absolute_tolerance, relative_tolerance, damping, rhs_tolerance, condition_limit;
	XamarinSparseReport report;
	void *user;
};

static thread_local char sparse_error [1024];
static thread_local bool sparse_allocation_failed;
static thread_local const XamarinSparseIterationOptions *iteration_options;

static void
sparse_report_error (const char *message)
{
	if (sparse_error [0] == 0)
		snprintf (sparse_error, sizeof (sparse_error), "%s", message);
}

static void
sparse_report_status (const char *message)
{
	if (iteration_options && iteration_options->report)
		iteration_options->report (iteration_options->user, message);
}

struct SparseCall {
	const XamarinSparseIterationOptions *previous;
	SparseCall (const XamarinSparseIterationOptions *options = nullptr) : previous (iteration_options)
	{
		sparse_error [0] = 0;
		sparse_allocation_failed = false;
		iteration_options = options;
	}
	~SparseCall () { iteration_options = previous; }
};

struct SparseResource {
	std::atomic<uint32_t> references { 1 };
	void (*destroy) (SparseResource *) = nullptr;
	void *workspace = nullptr;
	size_t capacity = 0;
	~SparseResource () { free (workspace); }
	void *reserve (size_t size)
	{
		if (size == 0)
			size = 1;
		if (size > capacity) {
			void *replacement = malloc (size);
			if (!replacement) {
				sparse_allocation_failed = true;
				sparse_report_error ("Could not allocate sparse workspace.");
				return nullptr;
			}
			free (workspace);
			workspace = replacement;
			capacity = size;
		}
		return workspace;
	}
};

#define SPARSE_TRAITS(name, scalar, suffix, attributes, structure, is_complex) \
struct name { \
	using Scalar = scalar; \
	using Matrix = SparseMatrix_##suffix; \
	using Dense = DenseMatrix_##suffix; \
	using Factor = SparseOpaqueFactorization_##suffix; \
	using Subfactor = SparseOpaqueSubfactor_##suffix; \
	using Preconditioner = SparseOpaquePreconditioner_##suffix; \
	using Attributes = attributes; \
	using Structure = structure; \
	static constexpr bool complex = is_complex; \
	static size_t state_size (SparseIterativeMethod method, bool preconditioner, int m, int n, int count) \
	{ return SparseGetStateSize_##suffix (method, preconditioner, m, n, count); } \
};

SPARSE_TRAITS (SparseDouble, double, Double, SparseAttributes_t, SparseMatrixStructure, false)
SPARSE_TRAITS (SparseFloat, float, Float, SparseAttributes_t, SparseMatrixStructure, false)
SPARSE_TRAITS (SparseComplexDouble, __SPARSE_double_complex, Complex_Double, SparseAttributesComplex_t, SparseMatrixStructureComplex, true)
SPARSE_TRAITS (SparseComplexFloat, __SPARSE_float_complex, Complex_Float, SparseAttributesComplex_t, SparseMatrixStructureComplex, true)

static void set_operation (SparseAttributes_t &attributes, int operation) { attributes.transpose = operation != CblasNoTrans; }
static void set_kind (SparseAttributes_t &attributes, int kind)
{
	switch (kind) {
	case SparseTriangular: attributes.kind = SparseTriangular; break;
	case SparseUnitTriangular: attributes.kind = SparseUnitTriangular; break;
	case SparseSymmetric: attributes.kind = SparseSymmetric; break;
	default: attributes.kind = SparseOrdinary; break;
	}
}
static void set_kind (SparseAttributesComplex_t &attributes, int kind) { attributes.kind = (SparseKind_t) kind; }
static void set_operation (SparseAttributesComplex_t &attributes, int operation)
{
	attributes.transpose = operation != CblasNoTrans;
	attributes.conjugate_transpose = operation == CblasConjTrans;
}

template<class T> static typename T::Matrix
sparse_matrix (const XamarinSparseMatrix *input)
{
	typename T::Matrix result = {};
	result.structure.rowCount = input->rows;
	result.structure.columnCount = input->columns;
	result.structure.columnStarts = input->starts;
	result.structure.rowIndices = input->indices;
	set_kind (result.structure.attributes, input->kind);
	result.structure.attributes.triangle = (SparseTriangle_t) input->triangle;
	set_operation (result.structure.attributes, input->operation);
	result.structure.blockSize = (uint8_t) input->block_size;
	result.data = (typename T::Scalar *) input->data;
	return result;
}

template<class T> static typename T::Dense
sparse_dense (int rows, int count, void *data)
{
	typename T::Dense result = {};
	result.rowCount = rows;
	result.columnCount = count;
	result.columnStride = rows;
	result.data = (typename T::Scalar *) data;
	return result;
}

static SparseSymbolicFactorOptions
sparse_symbolic_options (const XamarinSparseOptions *input)
{
	SparseSymbolicFactorOptions result = {};
	result.orderMethod = input ? (SparseOrder_t) input->order : SparseOrderDefault;
	result.order = input ? input->permutation : nullptr;
	result.ignoreRowsAndColumns = input ? input->ignored : nullptr;
	result.malloc = malloc;
	result.free = free;
	result.reportError = sparse_report_error;
	return result;
}

template<class T> static SparseNumericFactorOptions
sparse_numeric_options (const XamarinSparseOptions *input)
{
	SparseNumericFactorOptions result = {};
	result.pivotTolerance = sizeof (typename T::Scalar) / (T::complex ? 2 : 1) == sizeof (double) ? 0.01 : 0.1;
	result.zeroTolerance = 1e-4 * (sizeof (typename T::Scalar) / (T::complex ? 2 : 1) == sizeof (double) ? __DBL_EPSILON__ : __FLT_EPSILON__);
	if (input) {
		result.scalingMethod = (SparseScaling_t) input->scaling;
		result.scaling = input->scale;
		if (input->pivot_tolerance >= 0)
			result.pivotTolerance = input->pivot_tolerance;
		if (input->zero_tolerance >= 0)
			result.zeroTolerance = input->zero_tolerance;
	}
	return result;
}

struct SparseSymbolicResource : SparseResource {
	SparseOpaqueSymbolicFactorization factor = {};
	~SparseSymbolicResource () { if (factor.factorization) SparseCleanup (factor); }
};

template<class T> struct SparseFactorResource : SparseResource {
	typename T::Factor factor = {};
	~SparseFactorResource () { if (factor.symbolicFactorization.factorization) SparseCleanup (factor); }
};

template<class T> struct SparseSubfactorResource : SparseResource {
	typename T::Subfactor factor = {};
	~SparseSubfactorResource () { if (factor.contents != SparseSubfactorInvalid) SparseCleanup (factor); }
};

template<class T> static bool
sparse_validate_factor (void *handle)
{
	if (((SparseFactorResource<T> *) handle)->factor.status == SparseStatusOK)
		return true;
	sparse_report_error ("The numerical factorization is not valid.");
	return false;
}

template<class T> struct SparseMatrixResource : SparseResource {
	typename T::Matrix matrix = {};
	~SparseMatrixResource () { if (matrix.structure.attributes._allocatedBySparse) SparseCleanup (matrix); }
};

template<class T> struct SparsePreconditionerResource : SparseResource {
	typename T::Preconditioner preconditioner = {};
	~SparsePreconditionerResource ()
	{
		if (preconditioner.type != SparsePreconditionerNone && preconditioner.type != SparsePreconditionerUser)
			SparseCleanup (preconditioner);
	}
};

template<class R> static R *
sparse_allocate ()
{
	void *memory = malloc (sizeof (R));
	R *result = memory ? new (memory) R () : nullptr;
	if (!result) {
		sparse_allocation_failed = true;
		sparse_report_error ("Could not allocate sparse solver storage.");
	} else
		result->destroy = [](SparseResource *resource) {
			static_cast<R *> (resource)->~R ();
			free (resource);
		};
	return result;
}

static void sparse_destroy (SparseResource *resource) { resource->destroy (resource); }

template<class T> static void *
sparse_symbolic_create (uint8_t type, const XamarinSparseMatrix *matrix, const XamarinSparseOptions *options, size_t *sizes)
{
	auto result = sparse_allocate<SparseSymbolicResource> ();
	if (!result)
		return nullptr;
	result->factor = SparseFactor ((SparseFactorization_t) type, sparse_matrix<T> (matrix).structure, sparse_symbolic_options (options));
	if (result->factor.status != SparseStatusOK || sparse_error [0]) {
		if (!sparse_error [0]) sparse_report_error ("Sparse symbolic factorization failed.");
		sparse_destroy (result);
		return nullptr;
	}
	sizes [0] = result->factor.factorSize_Double * (T::complex ? 2 : 1);
	sizes [1] = result->factor.factorSize_Float * (T::complex ? 2 : 1);
	sizes [2] = result->factor.workspaceSize_Double * (T::complex ? 2 : 1);
	sizes [3] = result->factor.workspaceSize_Float * (T::complex ? 2 : 1);
	return result;
}

template<class T> static void *
sparse_factor_create (uint8_t type, const XamarinSparseMatrix *matrix, const XamarinSparseOptions *options, void *symbolic, void *storage, void *workspace, int *status)
{
	auto result = sparse_allocate<SparseFactorResource<T>> ();
	if (!result)
		return nullptr;
	if (symbolic) {
		auto source = (SparseSymbolicResource *) symbolic;
		result->factor = SparseFactor (source->factor, sparse_matrix<T> (matrix), sparse_numeric_options<T> (options), storage, workspace);
	} else {
		result->factor = SparseFactor ((SparseFactorization_t) type, sparse_matrix<T> (matrix), sparse_symbolic_options (options), sparse_numeric_options<T> (options));
	}
	*status = result->factor.status;
	if (*status != SparseStatusOK || sparse_error [0]) {
		sparse_destroy (result);
		return nullptr;
	}
	return result;
}

static bool
sparse_workspace_size (size_t fixed_size, size_t per_rhs, int count, size_t *size)
{
	if (count <= 0 || per_rhs > (SIZE_MAX - fixed_size) / (size_t) count) {
		sparse_report_error ("Sparse workspace size overflow.");
		return false;
	}
	*size = fixed_size + per_rhs * (size_t) count;
	return true;
}

template<class T> static size_t
sparse_solve_size (void *handle, int count, uint8_t subfactor)
{
	size_t size = 0;
	if (subfactor) {
		auto factor = ((SparseSubfactorResource<T> *) handle)->factor;
		sparse_workspace_size (factor.workspaceRequiredStatic, factor.workspaceRequiredPerRHS, count, &size);
	} else {
		auto factor = ((SparseFactorResource<T> *) handle)->factor;
		sparse_workspace_size (factor.solveWorkspaceRequiredStatic, factor.solveWorkspaceRequiredPerRHS, count, &size);
	}
	return size;
}

template<class T> static void
sparse_direct_solve (void *handle, int rows, int columns, int count, void *rhs, void *solution, void *workspace, int operation)
{
	if (operation < 2 && !sparse_validate_factor<T> (handle))
		return;
	size_t size = sparse_solve_size<T> (handle, count, operation >= 2);
	if (sparse_error [0])
		return;
	if (!workspace)
		workspace = ((SparseResource *) handle)->reserve (size);
	if (!workspace)
		return;
	auto b = sparse_dense<T> (rows, count, rhs);
	auto x = sparse_dense<T> (columns, count, solution);
	if (operation >= 2) {
		auto factor = ((SparseSubfactorResource<T> *) handle)->factor;
		if (operation == 3) {
			if (rhs == solution) SparseMultiply (factor, x, workspace);
			else SparseMultiply (factor, b, x, workspace);
		} else {
			if (rhs == solution) SparseSolve (factor, x, workspace);
			else SparseSolve (factor, b, x, workspace);
		}
	} else {
		auto factor = ((SparseFactorResource<T> *) handle)->factor;
		if (rhs == solution) SparseSolve (factor, x, workspace);
		else SparseSolve (factor, b, x, workspace);
	}
}

template<class F> static F sparse_factor_transpose (F factor, bool) { return SparseGetTranspose (factor); }
static SparseOpaqueFactorization_Complex_Double sparse_factor_transpose (SparseOpaqueFactorization_Complex_Double factor, bool conjugate) { return conjugate ? SparseGetConjugateTranspose (factor) : SparseGetTranspose (factor); }
static SparseOpaqueFactorization_Complex_Float sparse_factor_transpose (SparseOpaqueFactorization_Complex_Float factor, bool conjugate) { return conjugate ? SparseGetConjugateTranspose (factor) : SparseGetTranspose (factor); }
static SparseOpaqueSubfactor_Complex_Double sparse_factor_transpose (SparseOpaqueSubfactor_Complex_Double factor, bool conjugate) { return conjugate ? SparseGetConjugateTranspose (factor) : SparseGetTranspose (factor); }
static SparseOpaqueSubfactor_Complex_Float sparse_factor_transpose (SparseOpaqueSubfactor_Complex_Float factor, bool conjugate) { return conjugate ? SparseGetConjugateTranspose (factor) : SparseGetTranspose (factor); }

template<class T> static void *
sparse_factor_view (void *handle, int operation, int *subfactor)
{
	if (operation <= 1 || operation == 5) {
		if (!sparse_validate_factor<T> (handle)) return nullptr;
		auto result = sparse_allocate<SparseFactorResource<T>> ();
		if (!result) return nullptr;
		auto source = ((SparseFactorResource<T> *) handle)->factor;
		if (operation == 0) result->factor = SparseRetain (source);
		else result->factor = sparse_factor_transpose (source, operation == 5);
		return result;
	}
	if (operation == 2 && !sparse_validate_factor<T> (handle)) return nullptr;
	auto result = sparse_allocate<SparseSubfactorResource<T>> ();
	if (!result) return nullptr;
	if (operation == 2)
		result->factor = SparseCreateSubfactor ((SparseSubfactor_t) *subfactor, ((SparseFactorResource<T> *) handle)->factor);
	else if (operation == 3)
		result->factor = SparseRetain (((SparseSubfactorResource<T> *) handle)->factor);
	else
		result->factor = sparse_factor_transpose (((SparseSubfactorResource<T> *) handle)->factor, operation == 6);
	if (result->factor.contents == SparseSubfactorInvalid || sparse_error [0]) {
		sparse_destroy (result);
		return nullptr;
	}
	*subfactor = result->factor.contents;
	return result;
}

template<class T> static void
sparse_refactor (void *handle, const XamarinSparseMatrix *matrix, const XamarinSparseOptions *options, void *workspace, int *status)
{
	auto result = (SparseFactorResource<T> *) handle;
	if (workspace) SparseRefactor (sparse_matrix<T> (matrix), &result->factor, sparse_numeric_options<T> (options), workspace);
	else SparseRefactor (sparse_matrix<T> (matrix), &result->factor, sparse_numeric_options<T> (options));
	*status = result->factor.status;
}

template<class T> static size_t
sparse_refactor_size (void *handle)
{
	auto factor = ((SparseFactorResource<T> *) handle)->factor.symbolicFactorization;
	size_t size = sizeof (typename T::Scalar) / (T::complex ? 2 : 1) == sizeof (double) ? factor.workspaceSize_Double : factor.workspaceSize_Float;
	return size * (T::complex ? 2 : 1);
}

template<class T> static int
sparse_inertia (void *handle, int *positive, int *zero, int *negative)
{
	if (!sparse_validate_factor<T> (handle)) return -1;
	return SparseGetInertia (((SparseFactorResource<T> *) handle)->factor, positive, zero, negative);
}

template<class T> static void
sparse_update (void *handle, int count, const int *indices, const XamarinSparseMatrix *matrix, int *status)
{
	auto result = (SparseFactorResource<T> *) handle;
	if (!sparse_validate_factor<T> (handle)) { *status = result->factor.status; return; }
	SparseUpdateFactor (SparseUpdatePartialRefactor, &result->factor, count, indices, sparse_matrix<T> (matrix));
	*status = result->factor.status;
}

template<class T> static void
sparse_matrix_multiply (const XamarinSparseMatrix *matrix, int count, void *input, void *output, void *alpha, uint8_t accumulate)
{
	auto a = sparse_matrix<T> (matrix);
	int rows = matrix->block_size * (matrix->operation == CblasNoTrans ? matrix->rows : matrix->columns);
	int columns = matrix->block_size * (matrix->operation == CblasNoTrans ? matrix->columns : matrix->rows);
	auto x = sparse_dense<T> (columns, count, input);
	auto y = sparse_dense<T> (rows, count, output);
	if (accumulate) SparseMultiplyAdd (*(typename T::Scalar *) alpha, a, x, y);
	else SparseMultiply (*(typename T::Scalar *) alpha, a, x, y);
}

template<class T> static void *
sparse_coordinate (XamarinSparseMatrix *matrix, long count, const int *rows, const int *columns, const void *values)
{
	auto result = sparse_allocate<SparseMatrixResource<T>> ();
	if (!result) return nullptr;
	typename T::Attributes attributes = {};
	set_kind (attributes, matrix->kind);
	attributes.triangle = (SparseTriangle_t) matrix->triangle;
	size_t storage_size = 48 + ((size_t) matrix->columns + 1) * sizeof (long) + (size_t) count * sizeof (int) +
		(size_t) count * (size_t) matrix->block_size * (size_t) matrix->block_size * sizeof (typename T::Scalar);
	size_t work_size = (size_t) matrix->rows * sizeof (int);
	char *storage = (char *) result->reserve (storage_size + work_size);
	if (!storage) { sparse_destroy (result); return nullptr; }
	int empty_index = 0;
	typename T::Scalar empty_value = {};
	if (count == 0) {
		rows = columns = &empty_index;
		values = &empty_value;
	}
	result->matrix = SparseConvertFromCoordinate (matrix->rows, matrix->columns, count, (uint8_t) matrix->block_size, attributes, rows, columns, (const typename T::Scalar *) values, storage, storage + storage_size);
	matrix->starts = result->matrix.structure.columnStarts;
	matrix->indices = result->matrix.structure.rowIndices;
	matrix->data = result->matrix.data;
	if (!matrix->starts) { sparse_destroy (result); return nullptr; }
	return result;
}

template<class T> static void *
sparse_matrix_create (XamarinSparseMatrix *matrix, long count)
{
	auto result = sparse_allocate<SparseMatrixResource<T>> ();
	if (!result) return nullptr;
	size_t starts_size = ((size_t) matrix->columns + 1) * sizeof (long);
	size_t indices_size = (size_t) count * sizeof (int);
	size_t values_size = (size_t) count * (size_t) matrix->block_size * (size_t) matrix->block_size * sizeof (typename T::Scalar);
	if (starts_size > SIZE_MAX - indices_size - 15 || values_size > SIZE_MAX - starts_size - indices_size - 15) {
		sparse_report_error ("Sparse matrix storage size overflow.");
		sparse_destroy (result);
		return nullptr;
	}
	char *storage = (char *) result->reserve (starts_size + indices_size + 15 + values_size);
	if (!storage) { sparse_destroy (result); return nullptr; }
	memcpy (storage, matrix->starts, starts_size);
	if (indices_size) memcpy (storage + starts_size, matrix->indices, indices_size);
	char *data = (char *) (((uintptr_t) (storage + starts_size + indices_size) + 15) & ~(uintptr_t) 15);
	if (values_size) memcpy (data, matrix->data, values_size);
	matrix->starts = (long *) storage;
	matrix->indices = (int *) (storage + starts_size);
	matrix->data = data;
	result->matrix = sparse_matrix<T> (matrix);
	return result;
}

static SparseIterativeMethod
sparse_method (const XamarinSparseIterationOptions *input)
{
	if (input->method == 0) {
		SparseCGOptions options = {};
		options.reportError = sparse_report_error;
		options.maxIterations = input->max_iterations;
		options.atol = input->absolute_tolerance;
		options.rtol = input->relative_tolerance;
		options.reportStatus = input->report ? sparse_report_status : nullptr;
		return SparseConjugateGradient (options);
	}
	if (input->method == 1) {
		SparseGMRESOptions options = {};
		options.reportError = sparse_report_error;
		options.variant = (SparseGMRESVariant_t) input->variant;
		options.nvec = input->vectors;
		options.maxIterations = input->max_iterations;
		options.atol = input->absolute_tolerance;
		options.rtol = input->relative_tolerance;
		options.reportStatus = input->report ? sparse_report_status : nullptr;
		return SparseGMRES (options);
	}
	SparseLSMROptions options = {};
	options.reportError = sparse_report_error;
	options.lambda = input->damping;
	options.nvec = input->vectors;
	options.convergenceTest = (SparseLSMRConvergenceTest_t) input->convergence;
	options.atol = input->absolute_tolerance;
	options.rtol = input->relative_tolerance;
	options.btol = input->rhs_tolerance;
	options.conditionLimit = input->condition_limit;
	options.maxIterations = input->max_iterations;
	options.reportStatus = input->report ? sparse_report_status : nullptr;
	return SparseLSMR (options);
}

template<class T> static void
sparse_apply (XamarinSparseOperator callback, void *user, bool accumulate, CBLAS_TRANSPOSE operation, typename T::Dense x, typename T::Dense y)
{
	callback (user, accumulate, operation, x.rowCount, x.columnCount, x.columnStride, x.data, y.rowCount, y.columnCount, y.columnStride, y.data);
}

struct SparseUserPreconditioner {
	XamarinSparseOperator apply;
	void *user;
};

template<class T> static void
sparse_preconditioner_apply (void *context, CBLAS_TRANSPOSE operation, typename T::Dense x, typename T::Dense y)
{
	auto input = (SparseUserPreconditioner *) context;
	sparse_apply<T> (input->apply, input->user, false, operation, x, y);
}

template<class T> static void *
sparse_preconditioner_create (const XamarinSparseMatrix *matrix, int type, SparseUserPreconditioner *user)
{
	auto result = sparse_allocate<SparsePreconditionerResource<T>> ();
	if (!result) return nullptr;
	if (user) {
		result->reserve (sizeof (*user));
		if (!result->workspace) { sparse_destroy (result); return nullptr; }
		memcpy (result->workspace, user, sizeof (*user));
		result->preconditioner.type = SparsePreconditionerUser;
		result->preconditioner.mem = result->workspace;
		result->preconditioner.apply = sparse_preconditioner_apply<T>;
	} else if (type != SparsePreconditionerNone) {
		result->preconditioner = SparseCreatePreconditioner ((SparsePreconditioner_t) type, sparse_matrix<T> (matrix));
		if (result->preconditioner.type == SparsePreconditionerNone) {
			sparse_report_error ("Could not create sparse preconditioner.");
			sparse_destroy (result);
			return nullptr;
		}
	}
	return result;
}

template<class T> static int
sparse_iterative_solve (const XamarinSparseIterationOptions *options, const XamarinSparseMatrix *matrix, XamarinSparseOperator callback, void *user, int rows, int columns, int count, void *rhs, void *solution, void *preconditioner)
{
	auto b = sparse_dense<T> (rows, count, rhs);
	auto x = sparse_dense<T> (columns, count, solution);
	typename T::Preconditioner p = {};
	if (preconditioner) p = ((SparsePreconditionerResource<T> *) preconditioner)->preconditioner;
	auto method = sparse_method (options);
	if (matrix) {
		if (p.type == SparsePreconditionerNone) return SparseSolve (method, sparse_matrix<T> (matrix), b, x);
		return SparseSolve (method, sparse_matrix<T> (matrix), b, x, p);
	}
	if (p.type == SparsePreconditionerNone)
		return SparseSolve (method, ^(bool accumulate, CBLAS_TRANSPOSE operation, typename T::Dense input, typename T::Dense output) {
			sparse_apply<T> (callback, user, accumulate, operation, input, output);
		}, b, x);
	return SparseSolve (method, ^(bool accumulate, CBLAS_TRANSPOSE operation, typename T::Dense input, typename T::Dense output) {
		sparse_apply<T> (callback, user, accumulate, operation, input, output);
	}, b, x, p);
}

template<class T> static size_t
sparse_state_size (const XamarinSparseIterationOptions *options, uint8_t preconditioner, int rows, int columns, int count)
{
	return T::state_size (sparse_method (options), preconditioner != 0, rows, columns, count);
}

template<class T> static void
sparse_iterate (const XamarinSparseIterationOptions *options, int iteration, const uint8_t *converged, void *state, XamarinSparseOperator callback, void *user, int rows, int columns, int count, void *rhs, void *residual, int residual_stride, void *solution, void *preconditioner)
{
	typename T::Preconditioner p = {};
	if (preconditioner) p = ((SparsePreconditionerResource<T> *) preconditioner)->preconditioner;
	auto r = sparse_dense<T> (rows, count, residual);
	r.columnStride = residual_stride;
	SparseIterate (sparse_method (options), iteration, (const bool *) converged, state, ^(bool accumulate, CBLAS_TRANSPOSE operation, typename T::Dense input, typename T::Dense output) {
		sparse_apply<T> (callback, user, accumulate, operation, input, output);
	}, sparse_dense<T> (rows, count, rhs), r, sparse_dense<T> (columns, count, solution), p);
}

#define SPARSE_DISPATCH(function, ...) \
	switch (scalar) { \
	case 0: return function<SparseDouble> (__VA_ARGS__); \
	case 1: return function<SparseFloat> (__VA_ARGS__); \
	case 2: return function<SparseComplexDouble> (__VA_ARGS__); \
	case 3: return function<SparseComplexFloat> (__VA_ARGS__); \
	default: __builtin_trap (); \
	}

extern "C" {
const char *xamarin_sparse_error () { return sparse_error; }
uint8_t xamarin_sparse_allocation_failed () { return sparse_allocation_failed; }
void *xamarin_sparse_matrix_retain (void *handle)
{
	((SparseResource *) handle)->references.fetch_add (1, std::memory_order_relaxed);
	return handle;
}
void xamarin_sparse_release (void *handle)
{
	auto resource = (SparseResource *) handle;
	if (resource->references.fetch_sub (1, std::memory_order_acq_rel) == 1)
		sparse_destroy (resource);
}
void *xamarin_sparse_storage_create (size_t size, void **data)
{
	SparseCall call;
	auto result = sparse_allocate<SparseResource> ();
	if (!result) return nullptr;
	*data = result->reserve (size);
	if (!*data) { sparse_destroy (result); return nullptr; }
	return result;
}
void *xamarin_sparse_symbolic_create (int scalar, uint8_t type, const XamarinSparseMatrix *matrix, const XamarinSparseOptions *options, size_t *sizes)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_symbolic_create, type, matrix, options, sizes)
}
void *xamarin_sparse_factor_create (int scalar, uint8_t type, const XamarinSparseMatrix *matrix, const XamarinSparseOptions *options, void *symbolic, void *storage, void *workspace, int *status)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_factor_create, type, matrix, options, symbolic, storage, workspace, status)
}
size_t xamarin_sparse_solve_size (int scalar, void *handle, int count, uint8_t subfactor)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_solve_size, handle, count, subfactor)
}
void xamarin_sparse_direct_solve (int scalar, void *handle, int rows, int columns, int count, void *rhs, void *solution, void *workspace, int operation)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_direct_solve, handle, rows, columns, count, rhs, solution, workspace, operation)
}
void *xamarin_sparse_factor_view (int scalar, void *handle, int operation, int *subfactor)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_factor_view, handle, operation, subfactor)
}
void xamarin_sparse_refactor (int scalar, void *handle, const XamarinSparseMatrix *matrix, const XamarinSparseOptions *options, void *workspace, int *status)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_refactor, handle, matrix, options, workspace, status)
}
size_t xamarin_sparse_refactor_size (int scalar, void *handle)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_refactor_size, handle)
}
int xamarin_sparse_inertia (int scalar, void *handle, int *positive, int *zero, int *negative)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_inertia, handle, positive, zero, negative)
}
void xamarin_sparse_update (int scalar, void *handle, int count, const int *indices, const XamarinSparseMatrix *matrix, int *status)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_update, handle, count, indices, matrix, status)
}
void xamarin_sparse_matrix_multiply (int scalar, const XamarinSparseMatrix *matrix, int count, void *input, void *output, void *alpha, uint8_t accumulate)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_matrix_multiply, matrix, count, input, output, alpha, accumulate)
}
void *xamarin_sparse_coordinate (int scalar, XamarinSparseMatrix *matrix, long count, const int *rows, const int *columns, const void *values)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_coordinate, matrix, count, rows, columns, values)
}
void *xamarin_sparse_matrix_create (int scalar, XamarinSparseMatrix *matrix, long count)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_matrix_create, matrix, count)
}
void *xamarin_sparse_preconditioner_create (int scalar, const XamarinSparseMatrix *matrix, int type, SparseUserPreconditioner *user)
{
	SparseCall call;
	SPARSE_DISPATCH (sparse_preconditioner_create, matrix, type, user)
}
int xamarin_sparse_iterative_solve (int scalar, const XamarinSparseIterationOptions *options, const XamarinSparseMatrix *matrix, XamarinSparseOperator callback, void *user, int rows, int columns, int count, void *rhs, void *solution, void *preconditioner)
{
	SparseCall call (options);
	SPARSE_DISPATCH (sparse_iterative_solve, options, matrix, callback, user, rows, columns, count, rhs, solution, preconditioner)
}
size_t xamarin_sparse_state_size (int scalar, const XamarinSparseIterationOptions *options, uint8_t preconditioner, int rows, int columns, int count)
{
	SparseCall call (options);
	SPARSE_DISPATCH (sparse_state_size, options, preconditioner, rows, columns, count)
}
void xamarin_sparse_iterate (int scalar, const XamarinSparseIterationOptions *options, int iteration, const uint8_t *converged, void *state, XamarinSparseOperator callback, void *user, int rows, int columns, int count, void *rhs, void *residual, int residual_stride, void *solution, void *preconditioner)
{
	SparseCall call (options);
	SPARSE_DISPATCH (sparse_iterate, options, iteration, converged, state, callback, user, rows, columns, count, rhs, residual, residual_stride, solution, preconditioner)
}
}
