using ObjCRuntime;

#nullable enable

namespace Metal {

	public partial interface IMTL4CommandQueue {
		/// <summary>Queue an array of command buffers for execution.</summary>
		/// <param name="commandBuffers">The array of command buffers to queue for execution.</param>
		public void Commit (params IMTL4CommandBuffer [] commandBuffers)
		{
			NativeObjectExtensions.CallWithPointerToFirstElementAndCount (commandBuffers, nameof (commandBuffers), Commit);
		}

		/// <summary>Queue an array of command buffers for execution.</summary>
		/// <param name="options">Any options to configure the operation.</param>
		/// <param name="commandBuffers">The array of command buffers to queue for execution.</param>
		public void Commit (MTL4CommitOptions options, params IMTL4CommandBuffer [] commandBuffers)
		{
			NativeObjectExtensions.CallWithPointerToFirstElementAndCount (commandBuffers, nameof (commandBuffers), (ptr, count) => Commit (ptr, count, options));
		}

		/// <summary>Marks the specified residency sets as part of the current command buffer execution.</summary>
		/// <param name="residencySets">The residency sets to mark.</param>
		public void AddResidencySets (params IMTLResidencySet [] residencySets)
		{
			NativeObjectExtensions.CallWithPointerToFirstElementAndCount (residencySets, nameof (residencySets), AddResidencySets);
		}

		/// <summary>Removes the specified residency sets from the current command buffer execution.</summary>
		/// <param name="residencySets">The residency sets to mark.</param>
		public void RemoveResidencySets (params IMTLResidencySet [] residencySets)
		{
			NativeObjectExtensions.CallWithPointerToFirstElementAndCount (residencySets, nameof (residencySets), RemoveResidencySets);
		}

		/// <summary>Update multiple texture mappings.</summary>
		/// <param name="texture">The texture to update.</param>
		/// <param name="heap">The heap to use.</param>
		/// <param name="operations">The update operations.</param>
		public void UpdateTextureMappings (IMTLTexture texture, IMTLHeap? heap, params MTL4UpdateSparseTextureMappingOperation [] operations)
		{
			if (operations is null)
				ThrowHelper.ThrowArgumentNullException (nameof (operations));

			unsafe {
				fixed (MTL4UpdateSparseTextureMappingOperation* ptr = operations) {
					UpdateTextureMappings (texture, heap, (IntPtr) ptr, (nuint) operations.Length);
				}
			}
		}

		/// <summary>Copy texture mappings between two textures.</summary>
		/// <param name="sourceTexture">The source texture for the texture mappings.</param>
		/// <param name="destinationTexture">The destination texture for the texture mappings.</param>
		/// <param name="operations">The copy operations.</param>
		public void CopyTextureMappings (IMTLTexture sourceTexture, IMTLTexture destinationTexture, params MTL4CopySparseTextureMappingOperation [] operations)
		{
			if (operations is null)
				ThrowHelper.ThrowArgumentNullException (nameof (operations));

			unsafe {
				fixed (MTL4CopySparseTextureMappingOperation* ptr = operations) {
					CopyTextureMappings (sourceTexture, destinationTexture, (IntPtr) ptr, (nuint) operations.Length);
				}
			}
		}

		/// <summary>Update multiple buffer mappings.</summary>
		/// <param name="buffer">The buffer to update.</param>
		/// <param name="heap">The heap to use.</param>
		/// <param name="operations">The update operations.</param>
		public void UpdateBufferMappings (IMTLBuffer buffer, IMTLHeap? heap, params MTL4UpdateSparseBufferMappingOperation [] operations)
		{
			if (operations is null)
				ThrowHelper.ThrowArgumentNullException (nameof (operations));

			unsafe {
				fixed (MTL4UpdateSparseBufferMappingOperation* ptr = operations) {
					UpdateBufferMappings (buffer, heap, (IntPtr) ptr, (nuint) operations.Length);
				}
			}
		}

		/// <summary>Copy buffer mappings between two textures.</summary>
		/// <param name="sourceBuffer">The source buffer for the buffer mappings.</param>
		/// <param name="destinationBuffer">The destination buffer for the buffer mappings.</param>
		/// <param name="operations">The copy operations.</param>
		public void CopyBufferMappings (IMTLBuffer sourceBuffer, IMTLBuffer destinationBuffer, params MTL4CopySparseBufferMappingOperation [] operations)
		{
			if (operations is null)
				ThrowHelper.ThrowArgumentNullException (nameof (operations));

			unsafe {
				fixed (MTL4CopySparseBufferMappingOperation* ptr = operations) {
					CopyBufferMappings (sourceBuffer, destinationBuffer, (IntPtr) ptr, (nuint) operations.Length);
				}
			}
		}
	}
}
