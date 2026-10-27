// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#include <Accelerate/Accelerate.h>

#ifdef __cplusplus
extern "C" {
#endif

enum { XamarinSparseAllocationFailed = 1 };

// Keep the overloaded inline functions and their semi-opaque structs on the native side.
#define SPARSE_BINDINGS(suffix, scalar, precision) \
struct XamarinSparseFactor_##suffix { \
	SparseOpaqueFactorization_##precision factor; \
	void *workspace; \
	size_t workspace_capacity; \
}; \
void * \
xamarin_sparse_factor_##suffix (uint8_t type, int rows, int columns, long *starts, int *indices, scalar *data, uint8_t triangle, int *status) \
{ \
	XamarinSparseFactor_##suffix *storage = (XamarinSparseFactor_##suffix *) calloc (1, sizeof (*storage)); \
	if (storage == NULL) { \
		*status = XamarinSparseAllocationFailed; \
		return NULL; \
	} \
	SparseOpaqueFactorization_##precision *factor = &storage->factor; \
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
		free (storage); \
		return NULL; \
	} \
	return storage; \
} \
int \
xamarin_sparse_solve_##suffix (void *handle, int rows, int columns, int count, scalar *rhs, scalar *solution) \
{ \
	XamarinSparseFactor_##suffix *storage = (XamarinSparseFactor_##suffix *) handle; \
	SparseOpaqueFactorization_##precision *factor = &storage->factor; \
	size_t per_rhs = factor->solveWorkspaceRequiredPerRHS; \
	size_t fixed_size = factor->solveWorkspaceRequiredStatic; \
	if (count <= 0 || per_rhs > (SIZE_MAX - fixed_size) / (size_t) count) \
		return XamarinSparseAllocationFailed; \
	size_t size = fixed_size + (size_t) count * per_rhs; \
	if (size == 0) size = 1; \
	if (storage->workspace_capacity < size) { \
		void *workspace = malloc (size); \
		if (workspace == NULL) return XamarinSparseAllocationFailed; \
		free (storage->workspace); \
		storage->workspace = workspace; \
		storage->workspace_capacity = size; \
	} \
	DenseMatrix_##precision b = { .rowCount = rows, .columnCount = count, .columnStride = rows, .data = rhs }; \
	DenseMatrix_##precision x = { .rowCount = columns, .columnCount = count, .columnStride = columns, .data = solution }; \
	SparseSolve (*factor, b, x, storage->workspace); \
	return SparseStatusOK; \
} \
void \
xamarin_sparse_cleanup_##suffix (void *handle) \
{ \
	XamarinSparseFactor_##suffix *storage = (XamarinSparseFactor_##suffix *) handle; \
	SparseCleanup (storage->factor); \
	free (storage->workspace); \
	free (storage); \
}

SPARSE_BINDINGS (double, double, Double)
SPARSE_BINDINGS (float, float, Float)

#ifdef __cplusplus
}
#endif
