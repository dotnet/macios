// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#include <Accelerate/Accelerate.h>

#ifdef __cplusplus
extern "C" {
#endif

enum { XamarinSparseAllocationFailed = 1 };

// Keep the overloaded inline functions and their semi-opaque structs on the native side.
#define SPARSE_BINDINGS(suffix, scalar, precision) \
void * \
xamarin_sparse_factor_##suffix (uint8_t type, int rows, int columns, long *starts, int *indices, scalar *data, uint8_t triangle, int *status) \
{ \
	SparseOpaqueFactorization_##precision *factor = (SparseOpaqueFactorization_##precision *) malloc (sizeof (*factor)); \
	if (factor == NULL) { \
		*status = XamarinSparseAllocationFailed; \
		return NULL; \
	} \
	SparseMatrix_##precision matrix = { \
		.structure = { \
			.rowCount = rows, \
			.columnCount = columns, \
			.columnStarts = starts, \
			.rowIndices = indices, \
			.attributes = { \
				.triangle = (SparseTriangle_t) triangle, \
			}, \
			.blockSize = 1, \
		}, \
		.data = data, \
	}; \
	if (type < SparseFactorizationQR) \
		matrix.structure.attributes.kind = SparseSymmetric; \
	*factor = SparseFactor ((SparseFactorization_t) type, matrix); \
	*status = factor->status; \
	if (*status != SparseStatusOK) { \
		SparseCleanup (*factor); \
		free (factor); \
		return NULL; \
	} \
	return factor; \
} \
int \
xamarin_sparse_solve_##suffix (void *handle, int rows, int columns, int count, scalar *rhs, scalar *solution) \
{ \
	SparseOpaqueFactorization_##precision *factor = (SparseOpaqueFactorization_##precision *) handle; \
	size_t per_rhs = factor->solveWorkspaceRequiredPerRHS; \
	size_t fixed_size = factor->solveWorkspaceRequiredStatic; \
	if (count <= 0 || per_rhs > (SIZE_MAX - fixed_size) / (size_t) count) \
		return XamarinSparseAllocationFailed; \
	size_t size = fixed_size + (size_t) count * per_rhs; \
	void *workspace = malloc (size == 0 ? 1 : size); \
	if (workspace == NULL) \
		return XamarinSparseAllocationFailed; \
	DenseMatrix_##precision b = { .rowCount = rows, .columnCount = count, .columnStride = rows, .data = rhs }; \
	DenseMatrix_##precision x = { .rowCount = columns, .columnCount = count, .columnStride = columns, .data = solution }; \
	SparseSolve (*factor, b, x, workspace); \
	free (workspace); \
	return SparseStatusOK; \
} \
void \
xamarin_sparse_cleanup_##suffix (void *handle) \
{ \
	SparseOpaqueFactorization_##precision *factor = (SparseOpaqueFactorization_##precision *) handle; \
	SparseCleanup (*factor); \
	free (factor); \
}

SPARSE_BINDINGS (double, double, Double)
SPARSE_BINDINGS (float, float, Float)

#ifdef __cplusplus
}
#endif
