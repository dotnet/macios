//
// API for the Metal framework
//
// Authors:
//   Miguel de Icaza
//
// Copyrigh 2014-2015, Xamarin Inc.
//
// TODO:
//    * Provide friendly accessors instead of those IntPtrs that point to arrays
//    * MTLRenderPipelineReflection: the two arrays are NSObject
//    * Make the *array classes implement all the ICollection methods.
//

using System;
using System.ComponentModel;

using CoreAnimation;
using CoreData;
using CoreGraphics;
using CoreImage;
using CoreLocation;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

#if TVOS
using MTLAccelerationStructureSizes = Foundation.NSObject;
#endif

namespace Metal {

	/// <summary>Completion handler for deallocating a buffer.</summary>
	delegate void MTLDeallocator (IntPtr pointer, nuint length);

	delegate void MTLNewComputePipelineStateWithReflectionCompletionHandler ([NullAllowed] IMTLComputePipelineState computePipelineState, [NullAllowed] MTLComputePipelineReflection reflection, [NullAllowed] NSError error);

	delegate void MTLDrawablePresentedHandler (IMTLDrawable drawable);

	delegate void MTLNewRenderPipelineStateWithReflectionCompletionHandler ([NullAllowed] IMTLRenderPipelineState renderPipelineState, [NullAllowed] MTLRenderPipelineReflection reflection, [NullAllowed] NSError error);

	interface IMTLCommandEncoder { }

	/// <summary>Encapsulates a single parameter to a Metal function.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLArgument_Ref/index.html">Apple documentation for <c>MTLArgument</c></related>
	[Deprecated (PlatformName.MacOSX, 13, 0)]
	[Deprecated (PlatformName.iOS, 16, 0)]
	[Deprecated (PlatformName.TvOS, 16, 0)]
	[Deprecated (PlatformName.MacCatalyst, 16, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLArgument {
		[Export ("name")]
		string Name { get; }

		[Export ("type")]
		MTLArgumentType Type { get; }

		[Export ("access")]
		MTLArgumentAccess Access { get; }

		[Export ("index")]
		nuint Index { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }

		[Export ("bufferAlignment")]
		nuint BufferAlignment { get; }

		[Export ("bufferDataSize")]
		nuint BufferDataSize { get; }

		[Export ("bufferDataType")]
		MTLDataType BufferDataType { get; }

		[NullAllowed]
		[Export ("bufferStructType")]
		MTLStructType BufferStructType { get; }

		[Export ("threadgroupMemoryAlignment")]
		nuint ThreadgroupMemoryAlignment { get; }

		[Export ("threadgroupMemoryDataSize")]
		nuint ThreadgroupMemoryDataSize { get; }

		[Export ("textureType")]
		MTLTextureType TextureType { get; }

		[Export ("textureDataType")]
		MTLDataType TextureDataType { get; }

		[MacCatalyst (13, 1)]
		[Export ("isDepthTexture")]
		bool IsDepthTexture { get; }

		[MacCatalyst (13, 1)]
		[Export ("arrayLength")]
		nuint ArrayLength { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("bufferPointerType")]
		MTLPointerType BufferPointerType { get; }
	}

	/// <summary>Encapsulates the details of an array argument to a Metal function.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLArrayType_Ref/index.html">Apple documentation for <c>MTLArrayType</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MTLType))]
	interface MTLArrayType {
		[Export ("arrayLength")]
		nuint Length { get; }

		[Export ("elementType")]
		MTLDataType ElementType { get; }

		[Export ("stride")]
		nuint Stride { get; }

		[Export ("elementStructType")]
		[return: NullAllowed]
		MTLStructType ElementStructType ();

		[Export ("elementArrayType")]
		[return: NullAllowed]
		MTLArrayType ElementArrayType ();

		[MacCatalyst (13, 1)]
		[Export ("argumentIndexStride")]
		nuint ArgumentIndexStride { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("elementTextureReferenceType")]
		MTLTextureReferenceType ElementTextureReferenceType { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("elementPointerType")]
		MTLPointerType ElementPointerType { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[NullAllowed, Export ("elementTensorReferenceType")]
		MTLTensorReferenceType ElementTensorReferenceType { get; }
	}

	/// <summary>System protocol for enqueuing and writing commands into a buffer.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLCommandEncoder {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("label")]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("endEncoding")]
		void EndEncoding ();

		/// <param name="signpost">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("insertDebugSignpost:")]
		void InsertDebugSignpost (string signpost);

		/// <param name="debugGroup">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("pushDebugGroup:")]
		void PushDebugGroup (string debugGroup);

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("popDebugGroup")]
		void PopDebugGroup ();

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("barrierAfterQueueStages:beforeStages:")]
		void BarrierAfterQueueStages (MTLStages afterQueueStages, MTLStages beforeStages);
	}

	interface IMTLBuffer { }

	/// <summary>System protocol for raw data that is accessible in strides.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLBuffer : MTLResource {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("length")]
		nuint Length { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("contents")]
		IntPtr Contents { get; }

		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[NoiOS, NoTV, MacCatalyst (15, 0)]
		[Abstract, Export ("didModifyRange:")]
		void DidModify (NSRange range);

		/// <param name="descriptor">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="bytesPerRow">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[return: NullAllowed]
		[Abstract]
		[Export ("newTextureWithDescriptor:offset:bytesPerRow:")]
		[return: Release]
		IMTLTexture CreateTexture (MTLTextureDescriptor descriptor, nuint offset, nuint bytesPerRow);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("addDebugMarker:range:")]
		void AddDebugMarker (string marker, NSRange range);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("removeAllDebugMarkers")]
		void RemoveAllDebugMarkers ();

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[NullAllowed, Export ("remoteStorageBuffer")]
		IMTLBuffer RemoteStorageBuffer { get; }

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("newRemoteBufferViewForDevice:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBuffer CreateRemoteBuffer (IMTLDevice device);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("gpuAddress")]
		ulong GpuAddress { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newTensorWithDescriptor:offset:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTensor CreateTensor (MTLTensorDescriptor descriptor, nuint offset, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("sparseBufferTier")]
		MTLBufferSparseTier SparseBufferTier { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLBufferLayoutDescriptor : NSCopying {
		[Export ("stride")]
		nuint Stride { get; set; }

		[Export ("stepFunction", ArgumentSemantic.Assign)]
		MTLStepFunction StepFunction { get; set; }

		[Export ("stepRate")]
		nuint StepRate { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLBufferLayoutDescriptorArray {
		[Internal]
		[Export ("objectAtIndexedSubscript:")]
		MTLBufferLayoutDescriptor ObjectAtIndexedSubscript (nuint index);

		[Internal]
		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLBufferLayoutDescriptor bufferDesc, nuint index);
	}


	interface IMTLCommandBuffer { }

	/// <summary>Protocol for commands that are run on a GPU</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLCommandBuffer {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("commandQueue")]
		IMTLCommandQueue CommandQueue { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("retainedReferences")]
		bool RetainedReferences { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("label")]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("status")]
		MTLCommandBufferStatus Status { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("error")]
		NSError Error { get; }

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("enqueue")]
		void Enqueue ();

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("commit")]
		void Commit ();

		/// <param name="block">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("addScheduledHandler:")]
		void AddScheduledHandler (Action<IMTLCommandBuffer> block);

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("waitUntilScheduled")]
		void WaitUntilScheduled ();

		/// <param name="block">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("addCompletedHandler:")]
		void AddCompletedHandler (Action<IMTLCommandBuffer> block);

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("waitUntilCompleted")]
		void WaitUntilCompleted ();

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("blitCommandEncoder")]
		IMTLBlitCommandEncoder BlitCommandEncoder { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("computeCommandEncoder")]
		IMTLComputeCommandEncoder ComputeCommandEncoder { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("computeCommandEncoderWithDispatchType:")]
		[return: NullAllowed]
		IMTLComputeCommandEncoder ComputeCommandEncoderDispatch (MTLDispatchType dispatchType);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("encodeWaitForEvent:value:")]
		void EncodeWait (IMTLEvent @event, ulong value);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("encodeSignalEvent:value:")]
		void EncodeSignal (IMTLEvent @event, ulong value);

		[Field ("MTLCommandBufferErrorDomain")]
		NSString ErrorDomain { get; }

		/// <param name="renderPassDescriptor">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("parallelRenderCommandEncoderWithDescriptor:")]
		[return: NullAllowed]
		IMTLParallelRenderCommandEncoder CreateParallelRenderCommandEncoder (MTLRenderPassDescriptor renderPassDescriptor);

		/// <param name="drawable">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("presentDrawable:")]
		void PresentDrawable (IMTLDrawable drawable);

		/// <param name="drawable">To be added.</param>
		/// <param name="presentationTime">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("presentDrawable:atTime:")]
		void PresentDrawable (IMTLDrawable drawable, double presentationTime);

		/// <summary>Presents the specified <paramref name="drawable" /> after the previous drawable has been displayed for <paramref name="duration" /> seconds.</summary>
		/// <param name="drawable">The drawable to present immediately after the command buffer is scheduled to run.</param>
		/// <param name="duration">The minimum display time of the previous drawable.</param>
		[Abstract]
		[Introduced (PlatformName.MacCatalyst, 13, 4)]
		[Export ("presentDrawable:afterMinimumDuration:")]
		void PresentDrawableAfter (IMTLDrawable drawable, double duration);

		/// <param name="renderPassDescriptor">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("renderCommandEncoderWithDescriptor:")]
		IMTLRenderCommandEncoder CreateRenderCommandEncoder (MTLRenderPassDescriptor renderPassDescriptor);

		/// <summary>Returns the time, in seconds, when the GPU started scheduling the command buffer.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("kernelStartTime")]
		double /* CFTimeInterval */ KernelStartTime { get; }

		/// <summary>Returns the time, in seconds, when the GPU finished scheduling the command buffer.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("kernelEndTime")]
		double /* CFTimeInterval */ KernelEndTime { get; }

		/// <summary>Returns the time, in seconds, when the GPU started running the command buffer.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("GPUStartTime")]
		double /* CFTimeInterval */ GpuStartTime { get; }

		/// <summary>Returns the time, in seconds, when the GPU stopped running the command buffer.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("GPUEndTime")]
		double /* CFTimeInterval */ GpuEndTime { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("pushDebugGroup:")]
		void PushDebugGroup (string @string);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("popDebugGroup")]
		void PopDebugGroup ();

		[Abstract (GenerateExtensionMethod = true)]
		[MacCatalyst (14, 0), iOS (13, 0), TV (16, 0)]
		[NullAllowed, Export ("resourceStateCommandEncoder")]
		IMTLResourceStateCommandEncoder ResourceStateCommandEncoder { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("errorOptions")]
		MTLCommandBufferErrorOption ErrorOptions { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("logs")]
		IMTLLogContainer Logs { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("computeCommandEncoderWithDescriptor:")]
		IMTLComputeCommandEncoder CreateComputeCommandEncoder (MTLComputePassDescriptor computePassDescriptor);

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("blitCommandEncoderWithDescriptor:")]
		IMTLBlitCommandEncoder CreateBlitCommandEncoder (MTLBlitPassDescriptor blitPassDescriptor);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("resourceStateCommandEncoderWithDescriptor:")]
		IMTLResourceStateCommandEncoder CreateResourceStateCommandEncoder (MTLResourceStatePassDescriptor resourceStatePassDescriptor);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("accelerationStructureCommandEncoder")]
		IMTLAccelerationStructureCommandEncoder CreateAccelerationStructureCommandEncoder ();

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("accelerationStructureCommandEncoderWithDescriptor:")]
		IMTLAccelerationStructureCommandEncoder CreateAccelerationStructureCommandEncoder (MTLAccelerationStructurePassDescriptor descriptor);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("useResidencySet:")]
		void UseResidencySet (IMTLResidencySet residencySet);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("useResidencySets:count:")]
		void UseResidencySets (IntPtr /* const id <MTLResidencySet> _Nonnull[_Nonnull] */ residencySets, nuint count);
	}

	interface IMTLCommandQueue { }

	/// <summary>System protocol for objects that can queue command buffers for running on a GPU.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLCommandQueue {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("label")]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("commandBuffer")]
		[Autorelease]
		[return: NullAllowed]
		IMTLCommandBuffer CommandBuffer ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("commandBufferWithUnretainedReferences")]
		[Autorelease]
		[return: NullAllowed]
		IMTLCommandBuffer CommandBufferWithUnretainedReferences ();

		/// <summary>Developers should not use this deprecated method. Developers should use 'MTLCaptureScope' instead.</summary>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'MTLCaptureScope' instead.")]
		[Deprecated (PlatformName.TvOS, 11, 0, message: "Use 'MTLCaptureScope' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'MTLCaptureScope' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MTLCaptureScope' instead.")]
		[Abstract, Export ("insertDebugCaptureBoundary")]
		void InsertDebugCaptureBoundary ();

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("commandBufferWithDescriptor:")]
		[return: NullAllowed]
		IMTLCommandBuffer CreateCommandBuffer (MTLCommandBufferDescriptor descriptor);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("addResidencySet:")]
		void AddResidencySet (IMTLResidencySet residencySet);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("addResidencySets:count:")]
		void AddResidencySets (IntPtr residencySets, nuint count);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("removeResidencySet:")]
		void RemoveResidencySet (IMTLResidencySet residencySet);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("removeResidencySets:count:")]
		void RemoveResidencySets (IntPtr residencySets, nuint count);
	}

	interface IMTLComputeCommandEncoder { }

	/// <summary>Protocol for encoding and running parallel commands on a GPU.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLComputeCommandEncoder : MTLCommandEncoder {

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("dispatchType")]
		MTLDispatchType DispatchType { get; }

		/// <param name="state">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setComputePipelineState:")]
		void SetComputePipelineState (IMTLComputePipelineState state);

		/// <param name="buffer">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setBuffer:offset:atIndex:")]
		void SetBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		/// <param name="texture">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setTexture:atIndex:")]
		void SetTexture (IMTLTexture texture, nuint index);

		/// <param name="sampler">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setSamplerState:atIndex:")]
		void SetSamplerState (IMTLSamplerState sampler, nuint index);

		/// <param name="sampler">To be added.</param>
		/// <param name="lodMinClamp">To be added.</param>
		/// <param name="lodMaxClamp">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setSamplerState:lodMinClamp:lodMaxClamp:atIndex:")]
		void SetSamplerState (IMTLSamplerState sampler, float /* float, not CGFloat */ lodMinClamp, float /* float, not CGFloat */ lodMaxClamp, nuint index);

		/// <param name="length">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setThreadgroupMemoryLength:atIndex:")]
		void SetThreadgroupMemoryLength (nuint length, nuint index);

		/// <param name="threadgroupsPerGrid">To be added.</param>
		/// <param name="threadsPerThreadgroup">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("dispatchThreadgroups:threadsPerThreadgroup:")]
		void DispatchThreadgroups (MTLSize threadgroupsPerGrid, MTLSize threadsPerThreadgroup);

		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("dispatchThreadgroupsWithIndirectBuffer:indirectBufferOffset:threadsPerThreadgroup:")]
		void DispatchThreadgroups (IMTLBuffer indirectBuffer, nuint indirectBufferOffset, MTLSize threadsPerThreadgroup);

		/// <summary>Encodes <paramref name="buffers" /> to the argument buffer.</summary>
		/// <param name="buffers">An array of buffers in an argument buffer.</param>
		/// <param name="offsets">The byte offsets of <paramref name="buffers" /> in the containing buffer.</param>
		/// <param name="range">Indices into the target buffer of the buffers in <paramref name="buffers" />. Either Metal index IDs or the index members of <see cref="Metal.MTLArgumentDescriptor" />s.</param>
		[Abstract]
		[Export ("setBuffers:offsets:withRange:")]
		void SetBuffers (IntPtr buffers, IntPtr offsets, NSRange range);

		/// <param name="samplers">To be added.</param>
		/// <param name="floatArrayPtrLodMinClamps">To be added.</param>
		/// <param name="floatArrayPtrLodMaxClamps">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setSamplerStates:lodMinClamps:lodMaxClamps:withRange:")]
		void SetSamplerStates (IMTLSamplerState [] samplers, IntPtr floatArrayPtrLodMinClamps, IntPtr floatArrayPtrLodMaxClamps, NSRange range);

		/// <param name="samplers">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setSamplerStates:withRange:")]
		void SetSamplerStates (IMTLSamplerState [] samplers, NSRange range);

		/// <param name="textures">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setTextures:withRange:")]
		void SetTextures (IMTLTexture [] textures, NSRange range);

		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setBufferOffset:atIndex:")]
		void SetBufferOffset (nuint offset, nuint index);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setBytes:length:atIndex:")]
		void SetBytes (IntPtr bytes, nuint length, nuint index);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setStageInRegion:")]
		void SetStage (MTLRegion region);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setStageInRegionWithIndirectBuffer:indirectBufferOffset:")]
		void SetStageInRegion (IMTLBuffer indirectBuffer, nuint indirectBufferOffset);

		/// <summary>Captures all GPU work up to the current fence.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("updateFence:")]
		void Update (IMTLFence fence);

		/// <summary>Prevents additional GPU work by the encoder until the <paramref name="fence" /> is reached.</summary>
		/// <param name="fence">The fence to wait for.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("waitForFence:")]
		void Wait (IMTLFence fence);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("dispatchThreads:threadsPerThreadgroup:")]
		void DispatchThreads (MTLSize threadsPerGrid, MTLSize threadsPerThreadgroup);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useResource:usage:")]
		void UseResource (IMTLResource resource, MTLResourceUsage usage);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useResources:count:usage:")]
		void UseResources (IMTLResource [] resources, nuint count, MTLResourceUsage usage);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useHeap:")]
		void UseHeap (IMTLHeap heap);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useHeaps:count:")]
		void UseHeaps (IMTLHeap [] heaps, nuint count);

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[TV (14, 5)]
		[Abstract]
		[Export ("setImageblockWidth:height:")]
		void SetImageblock (nuint width, nuint height);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("memoryBarrierWithScope:")]
		void MemoryBarrier (MTLBarrierScope scope);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("memoryBarrierWithResources:count:")]
		void MemoryBarrier (IMTLResource [] resources, nuint count);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("executeCommandsInBuffer:withRange:")]
		void ExecuteCommands (IMTLIndirectCommandBuffer indirectCommandBuffer, NSRange executionRange);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("executeCommandsInBuffer:indirectBuffer:indirectBufferOffset:")]
		void ExecuteCommands (IMTLIndirectCommandBuffer indirectCommandbuffer, IMTLBuffer indirectRangeBuffer, nuint indirectBufferOffset);

		[Abstract]
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("sampleCountersInBuffer:atSampleIndex:withBarrier:")]
		void SampleCounters (IMTLCounterSampleBuffer sampleBuffer, nuint sampleIndex, bool barrier);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("setVisibleFunctionTable:atBufferIndex:")]
		void SetVisibleFunctionTable ([NullAllowed] IMTLVisibleFunctionTable visibleFunctionTable, nuint bufferIndex);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("setVisibleFunctionTables:withBufferRange:")]
		void SetVisibleFunctionTables (IMTLVisibleFunctionTable [] visibleFunctionTables, NSRange range);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("setIntersectionFunctionTable:atBufferIndex:")]
		void SetIntersectionFunctionTable ([NullAllowed] IMTLIntersectionFunctionTable intersectionFunctionTable, nuint bufferIndex);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("setIntersectionFunctionTables:withBufferRange:")]
		void SetIntersectionFunctionTables (IMTLIntersectionFunctionTable [] intersectionFunctionTables, NSRange range);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("setAccelerationStructure:atBufferIndex:")]
		void SetAccelerationStructure ([NullAllowed] IMTLAccelerationStructure accelerationStructure, nuint bufferIndex);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setBuffer:offset:attributeStride:atIndex:")]
		void SetBuffer (IMTLBuffer buffer, nuint offset, nuint stride, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setBuffers:offsets:attributeStrides:withRange:")]
		void SetBuffers (IntPtr /* IMTLBuffer[] */ buffers, IntPtr /* nuint[] */ offsets, IntPtr /* nuint[] */ strides, NSRange range);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setBufferOffset:attributeStride:atIndex:")]
		void SetBufferOffset (nuint offset, nuint stride, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setBytes:length:attributeStride:atIndex:")]
		void SetBytes (IntPtr bytes, nuint length, nuint stride, nuint index);

	}

	/// <summary>Encapsulates the details of the arguments of the compute function used to create an <see cref="Metal.IMTLComputePipelineState" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLComputePipelineReflection_Ref/index.html">Apple documentation for <c>MTLComputePipelineReflection</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLComputePipelineReflection {

		[Deprecated (PlatformName.MacOSX, 13, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.TvOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("arguments")]
		MTLArgument [] Arguments { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("bindings")]
		IMTLBinding [] Bindings { get; }
	}

	interface IMTLComputePipelineState { }
	/// <summary>System protocol that represents a compiled compute program.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLComputePipelineState : MTLAllocation {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("maxTotalThreadsPerThreadgroup")]
		nuint MaxTotalThreadsPerThreadgroup { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("threadExecutionWidth")]
		nuint ThreadExecutionWidth { get; }

		/// <summary>Returns the descriptive label for the compute pipeline state.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("staticThreadgroupMemoryLength")]
		nuint StaticThreadgroupMemoryLength { get; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("imageblockMemoryLengthForDimensions:")]
		nuint GetImageblockMemoryLength (MTLSize imageblockDimensions);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("supportIndirectCommandBuffers")]
		bool SupportIndirectCommandBuffers { get; }

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("functionHandleWithFunction:")]
		IMTLFunctionHandle CreateFunctionHandle (IMTLFunction function);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("newComputePipelineStateWithAdditionalBinaryFunctions:error:")]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (IMTLFunction [] functions, [NullAllowed] out NSError error);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("newVisibleFunctionTableWithDescriptor:")]
		[return: Release]
		IMTLVisibleFunctionTable CreateVisibleFunctionTable (MTLVisibleFunctionTableDescriptor descriptor);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("newIntersectionFunctionTableWithDescriptor:")]
		[return: Release]
		IMTLIntersectionFunctionTable CreateIntersectionFunctionTable (MTLIntersectionFunctionTableDescriptor descriptor);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }

		[Abstract]
		[Export ("shaderValidation")]
		MTLShaderValidation ShaderValidation { get; }

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Abstract]
		[NullAllowed, Export ("reflection")]
		MTLComputePipelineReflection Reflection { get; }

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Abstract]
		[Export ("functionHandleWithName:")]
		[return: NullAllowed]
		IMTLFunctionHandle CreateFunctionHandle (string name);

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Abstract]
		[Export ("functionHandleWithBinaryFunction:")]
		[return: NullAllowed]
		IMTLFunctionHandle CreateFunctionHandle (IMTL4BinaryFunction function);

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Abstract]
		[Export ("newComputePipelineStateWithBinaryFunctions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (IMTL4BinaryFunction [] additionalBinaryFunctions, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Abstract]
		[Export ("requiredThreadsPerThreadgroup")]
		MTLSize RequiredThreadsPerThreadgroup { get; }
	}

	interface IMTLBlitCommandEncoder { }

	/// <summary>Protocol for writing data into frame buffers.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLBlitCommandEncoder : MTLCommandEncoder {

		/// <param name="resource">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[NoiOS, NoTV, MacCatalyst (15, 0)]
		[Abstract, Export ("synchronizeResource:")]
		void Synchronize (IMTLResource resource);

		/// <param name="texture">To be added.</param>
		/// <param name="slice">To be added.</param>
		/// <param name="level">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[NoiOS, NoTV, MacCatalyst (15, 0)]
		[Abstract, Export ("synchronizeTexture:slice:level:")]
		void Synchronize (IMTLTexture texture, nuint slice, nuint level);

		/// <param name="sourceTexture">To be added.</param>
		/// <param name="sourceSlice">To be added.</param>
		/// <param name="sourceLevel">To be added.</param>
		/// <param name="sourceOrigin">To be added.</param>
		/// <param name="sourceSize">To be added.</param>
		/// <param name="destinationTexture">To be added.</param>
		/// <param name="destinationSlice">To be added.</param>
		/// <param name="destinationLevel">To be added.</param>
		/// <param name="destinationOrigin">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("copyFromTexture:sourceSlice:sourceLevel:sourceOrigin:sourceSize:toTexture:destinationSlice:destinationLevel:destinationOrigin:")]
		void CopyFromTexture (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, MTLOrigin sourceOrigin, MTLSize sourceSize, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, MTLOrigin destinationOrigin);

		/// <param name="sourceBuffer">To be added.</param>
		/// <param name="sourceOffset">To be added.</param>
		/// <param name="sourceBytesPerRow">To be added.</param>
		/// <param name="sourceBytesPerImage">To be added.</param>
		/// <param name="sourceSize">To be added.</param>
		/// <param name="destinationTexture">To be added.</param>
		/// <param name="destinationSlice">To be added.</param>
		/// <param name="destinationLevel">To be added.</param>
		/// <param name="destinationOrigin">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("copyFromBuffer:sourceOffset:sourceBytesPerRow:sourceBytesPerImage:sourceSize:toTexture:destinationSlice:destinationLevel:destinationOrigin:")]
		void CopyFromBuffer (IMTLBuffer sourceBuffer, nuint sourceOffset, nuint sourceBytesPerRow, nuint sourceBytesPerImage, MTLSize sourceSize, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, MTLOrigin destinationOrigin);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("copyFromBuffer:sourceOffset:sourceBytesPerRow:sourceBytesPerImage:sourceSize:toTexture:destinationSlice:destinationLevel:destinationOrigin:options:")]
		void CopyFromBuffer (IMTLBuffer sourceBuffer, nuint sourceOffset, nuint sourceBytesPerRow, nuint sourceBytesPerImage, MTLSize sourceSize, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, MTLOrigin destinationOrigin, MTLBlitOption options);

		/// <param name="sourceTexture">To be added.</param>
		/// <param name="sourceSlice">To be added.</param>
		/// <param name="sourceLevel">To be added.</param>
		/// <param name="sourceOrigin">To be added.</param>
		/// <param name="sourceSize">To be added.</param>
		/// <param name="destinationBuffer">To be added.</param>
		/// <param name="destinationOffset">To be added.</param>
		/// <param name="destinatinBytesPerRow">To be added.</param>
		/// <param name="destinationBytesPerImage">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("copyFromTexture:sourceSlice:sourceLevel:sourceOrigin:sourceSize:toBuffer:destinationOffset:destinationBytesPerRow:destinationBytesPerImage:")]
		void CopyFromTexture (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, MTLOrigin sourceOrigin, MTLSize sourceSize, IMTLBuffer destinationBuffer, nuint destinationOffset, nuint destinatinBytesPerRow, nuint destinationBytesPerImage);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("copyFromTexture:sourceSlice:sourceLevel:sourceOrigin:sourceSize:toBuffer:destinationOffset:destinationBytesPerRow:destinationBytesPerImage:options:")]
		void CopyFromTexture (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, MTLOrigin sourceOrigin, MTLSize sourceSize, IMTLBuffer destinationBuffer, nuint destinationOffset, nuint destinatinBytesPerRow, nuint destinationBytesPerImage, MTLBlitOption options);

		/// <param name="texture">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("generateMipmapsForTexture:")]
		void GenerateMipmapsForTexture (IMTLTexture texture);

		/// <param name="buffer">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <param name="value">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("fillBuffer:range:value:")]
		void FillBuffer (IMTLBuffer buffer, NSRange range, byte value);

		/// <param name="sourceBuffer">To be added.</param>
		/// <param name="sourceOffset">To be added.</param>
		/// <param name="destinationBuffer">To be added.</param>
		/// <param name="destinationOffset">To be added.</param>
		/// <param name="size">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("copyFromBuffer:sourceOffset:toBuffer:destinationOffset:size:")]
		void CopyFromBuffer (IMTLBuffer sourceBuffer, nuint sourceOffset, IMTLBuffer destinationBuffer, nuint destinationOffset, nuint size);

		/// <summary>Captures GPU work that was enqueued by the encoder for the specified <paramref name="fence" />.</summary>
		/// <param name="fence">The fence to update.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("updateFence:")]
		void Update (IMTLFence fence);

		/// <summary>Prevents additional GPU work by the encoder until the <paramref name="fence" /> is reached.</summary>
		/// <param name="fence">The fence to wait to be updated.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("waitForFence:")]
		void Wait (IMTLFence fence);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("optimizeContentsForGPUAccess:")]
		void OptimizeContentsForGpuAccess (IMTLTexture texture);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("optimizeContentsForGPUAccess:slice:level:")]
		void OptimizeContentsForGpuAccess (IMTLTexture texture, nuint slice, nuint level);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("optimizeContentsForCPUAccess:")]
		void OptimizeContentsForCpuAccess (IMTLTexture texture);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("optimizeContentsForCPUAccess:slice:level:")]
		void OptimizeContentsForCpuAccess (IMTLTexture texture, nuint slice, nuint level);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("resetCommandsInBuffer:withRange:")]
		void ResetCommands (IMTLIndirectCommandBuffer buffer, NSRange range);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("copyIndirectCommandBuffer:sourceRange:destination:destinationIndex:")]
		void Copy (IMTLIndirectCommandBuffer source, NSRange sourceRange, IMTLIndirectCommandBuffer destination, nuint destinationIndex);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("optimizeIndirectCommandBuffer:withRange:")]
		void Optimize (IMTLIndirectCommandBuffer indirectCommandBuffer, NSRange range);

		// @optional in macOS and Mac Catalyst
#if !__MACOS__ && !__MACCATALYST__
		[Abstract (GenerateExtensionMethod = true)]
#endif
		[TV (16, 0), iOS (13, 0), MacCatalyst (15, 0)]
		[Export ("getTextureAccessCounters:region:mipLevel:slice:resetCounters:countersBuffer:countersBufferOffset:")]
		void GetTextureAccessCounters (IMTLTexture texture, MTLRegion region, nuint mipLevel, nuint slice, bool resetCounters, IMTLBuffer countersBuffer, nuint countersBufferOffset);

		// @optional in macOS and Mac Catalyst
#if !__MACOS__ && !__MACCATALYST__
		[Abstract (GenerateExtensionMethod = true)]
#endif
		[TV (16, 0), iOS (13, 0), MacCatalyst (15, 0)]
		[Export ("resetTextureAccessCounters:region:mipLevel:slice:")]
		void ResetTextureAccessCounters (IMTLTexture texture, MTLRegion region, nuint mipLevel, nuint slice);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("copyFromTexture:sourceSlice:sourceLevel:toTexture:destinationSlice:destinationLevel:sliceCount:levelCount:")]
		void Copy (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, nuint sliceCount, nuint levelCount);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("copyFromTexture:toTexture:")]
		void Copy (IMTLTexture sourceTexture, IMTLTexture destinationTexture);

		[Abstract]
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("sampleCountersInBuffer:atSampleIndex:withBarrier:")]
		void SampleCounters (IMTLCounterSampleBuffer sampleBuffer, nuint sampleIndex, bool barrier);

		[Abstract]
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("resolveCounters:inRange:destinationBuffer:destinationOffset:")]
		void ResolveCounters (IMTLCounterSampleBuffer sampleBuffer, NSRange range, IMTLBuffer destinationBuffer, nuint destinationOffset);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("copyFromTensor:sourceOrigin:sourceDimensions:toTensor:destinationOrigin:destinationDimensions:")]
		void CopyFromTensor (IMTLTensor sourceTensor, MTLTensorExtents sourceOrigin, MTLTensorExtents sourceDimensions, IMTLTensor destinationTensor, MTLTensorExtents destinationOrigin, MTLTensorExtents destinationDimensions);
	}

	interface IMTLFence { }

	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	interface MTLFence {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }
	}

	interface IMTLDevice { }

	/// <summary>System protocol for interacting with a single graphics device.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLDevice {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("name")]
		string Name { get; }

		/// <summary>Returns the number of threads per threadgroup on the device.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("maxThreadsPerThreadgroup")]
		MTLSize MaxThreadsPerThreadgroup { get; }

		[Abstract]
		[MacCatalyst (15, 0)]
		[NoiOS]
		[NoTV]
		[Export ("lowPower")]
		bool LowPower { [Bind ("isLowPower")] get; }

		[Abstract]
		[MacCatalyst (15, 0)]
		[NoiOS]
		[NoTV]
		[Export ("headless")]
		bool Headless { [Bind ("isHeadless")] get; }

		[iOS (17, 0), TV (17, 0), MacCatalyst (15, 0)]
		[Abstract]
		[Export ("recommendedMaxWorkingSetSize")]
		ulong RecommendedMaxWorkingSetSize { get; }

		[Abstract]
		[MacCatalyst (15, 0)]
		[NoiOS]
		[NoTV]
		[Export ("depth24Stencil8PixelFormatSupported")]
		bool Depth24Stencil8PixelFormatSupported { [Bind ("isDepth24Stencil8PixelFormatSupported")] get; }

		/// <summary>Gets the size and alignment of a texture with specified description, when allocated from a heap.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("heapTextureSizeAndAlignWithDescriptor:")]
		MTLSizeAndAlign GetHeapTextureSizeAndAlign (MTLTextureDescriptor desc);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("heapBufferSizeAndAlignWithLength:options:")]
		MTLSizeAndAlign GetHeapBufferSizeAndAlignWithLength (nuint length, MTLResourceOptions options);

		/// <summary>Creates and returns a new heap.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newHeapWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLHeap CreateHeap (MTLHeapDescriptor descriptor);

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newCommandQueue")]
		[return: NullAllowed]
		[return: Release]
		IMTLCommandQueue CreateCommandQueue ();

		/// <param name="maxCommandBufferCount">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newCommandQueueWithMaxCommandBufferCount:")]
		[return: NullAllowed]
		[return: Release]
		IMTLCommandQueue CreateCommandQueue (nuint maxCommandBufferCount);

		/// <param name="length">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newBufferWithLength:options:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBuffer CreateBuffer (nuint length, MTLResourceOptions options);

		[Abstract, Export ("newBufferWithBytes:length:options:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBuffer CreateBuffer (IntPtr pointer, nuint length, MTLResourceOptions options);

		/// <summary>Creates and returns a new buffer that is wrapped around the specified data, and runs an optional <paramref name="deallocator" /> when the memory is deallocated.</summary>
		/// <param name="pointer">The data to wrap.</param>
		/// <param name="length">The length of the data to wrap.</param>
		/// <param name="options">Options for creating the buffer.</param>
		/// <param name="deallocator">The deallocator to use when deleting the buffer.</param>
		[Abstract, Export ("newBufferWithBytesNoCopy:length:options:deallocator:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBuffer CreateBufferNoCopy (IntPtr pointer, nuint length, MTLResourceOptions options, MTLDeallocator deallocator);

		/// <param name="descriptor">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newDepthStencilStateWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLDepthStencilState CreateDepthStencilState (MTLDepthStencilDescriptor descriptor);

		/// <param name="descriptor">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newTextureWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateTexture (MTLTextureDescriptor descriptor);

		/// <summary>Creates a Metal texture with the specified values.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[return: NullAllowed]
		[return: Release]
		[Export ("newTextureWithDescriptor:iosurface:plane:")]
		IMTLTexture CreateTexture (MTLTextureDescriptor descriptor, IOSurface.IOSurface iosurface, nuint plane);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newSharedTextureWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateSharedTexture (MTLTextureDescriptor descriptor);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newSharedTextureWithHandle:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateSharedTexture (MTLSharedTextureHandle sharedHandle);

		/// <param name="descriptor">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newSamplerStateWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLSamplerState CreateSamplerState (MTLSamplerDescriptor descriptor);

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newDefaultLibrary")]
		[return: Release]
		IMTLLibrary CreateDefaultLibrary ();

		/// <param name="filepath">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newLibraryWithFile:error:")]
		[return: Release]
		IMTLLibrary CreateLibrary (string filepath, out NSError error);

		[Abstract]
		[Export ("newLibraryWithData:error:")]
		[return: Release]
		IMTLLibrary CreateLibrary (DispatchData data, out NSError error);

		/// <param name="source">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newLibraryWithSource:options:error:")]
		[return: Release]
		IMTLLibrary CreateLibrary (string source, MTLCompileOptions options, out NSError error);

		/// <param name="source">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newLibraryWithSource:options:completionHandler:")]
		[Async]
		void CreateLibrary (string source, MTLCompileOptions options, Action<IMTLLibrary, NSError> completionHandler);

		/// <summary>Creates and returns a new library from the functions in the specified bundle.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("newDefaultLibraryWithBundle:error:")]
		[return: Release]
		[return: NullAllowed]
		IMTLLibrary CreateDefaultLibrary (NSBundle bundle, out NSError error);

		/// <param name="descriptor">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newRenderPipelineStateWithDescriptor:error:")]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTLRenderPipelineDescriptor descriptor, out NSError error);

		/// <param name="descriptor">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newRenderPipelineStateWithDescriptor:completionHandler:")]
		void CreateRenderPipelineState (MTLRenderPipelineDescriptor descriptor, Action<IMTLRenderPipelineState, NSError> completionHandler);

		/// <param name="descriptor">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <param name="reflection">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:options:reflection:error:")]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTLRenderPipelineDescriptor descriptor, MTLPipelineOption options, out MTLRenderPipelineReflection reflection, out NSError error);

		/// <param name="descriptor">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:options:completionHandler:")]
		void CreateRenderPipelineState (MTLRenderPipelineDescriptor descriptor, MTLPipelineOption options, Action<IMTLRenderPipelineState, MTLRenderPipelineReflection, NSError> completionHandler);

		/// <param name="computeFunction">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <param name="reflection">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newComputePipelineStateWithFunction:options:reflection:error:")]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (IMTLFunction computeFunction, MTLPipelineOption options, out MTLComputePipelineReflection reflection, out NSError error);

		/// <param name="computeFunction">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newComputePipelineStateWithFunction:completionHandler:")]
		void CreateComputePipelineState (IMTLFunction computeFunction, Action<IMTLComputePipelineState, NSError> completionHandler);

		/// <param name="computeFunction">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newComputePipelineStateWithFunction:error:")]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (IMTLFunction computeFunction, out NSError error);

		/// <param name="computeFunction">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newComputePipelineStateWithFunction:options:completionHandler:")]
		void CreateComputePipelineState (IMTLFunction computeFunction, MTLPipelineOption options, Action<IMTLComputePipelineState, MTLComputePipelineReflection, NSError> completionHandler);

		/// <summary>Creates a new pipeline state from the specified compute pipeline descriptor, options, and completion handler, and stores reflection information in the <paramref name="reflection" /><see langword="out" /> parameter.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:options:reflection:error:")]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (MTLComputePipelineDescriptor descriptor, MTLPipelineOption options, out MTLComputePipelineReflection reflection, out NSError error);

		/// <summary>Creates a new pipeline state from the specified compute pipeline descriptor, options, and completion handler.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:options:completionHandler:")]
		void CreateComputePipelineState (MTLComputePipelineDescriptor descriptor, MTLPipelineOption options, MTLNewComputePipelineStateWithReflectionCompletionHandler completionHandler);

		/// <summary>Creates and returns a new fence for tracking and managing dependencies between command encoders.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newFence")]
		[return: Release]
		IMTLFence CreateFence ();

		/// <param name="featureSet">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("supportsFeatureSet:")]
		bool SupportsFeatureSet (MTLFeatureSet featureSet);

		/// <summary>Returns a Boolean value that tells whether the device supports the specified texture count.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("supportsTextureSampleCount:")]
		bool SupportsTextureSampleCount (nuint sampleCount);

		[NoiOS, NoTV, MacCatalyst (15, 0)]
		[Abstract]
		[Export ("removable")]
		bool Removable { [Bind ("isRemovable")] get; }

		/// <summary>Gets the texture read-write support tier.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("readWriteTextureSupport")]
		MTLReadWriteTextureTier ReadWriteTextureSupport { get; }

		/// <summary>Returns the argument buffer support tier.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("argumentBuffersSupport")]
		MTLArgumentBuffersTier ArgumentBuffersSupport { get; }

		/// <summary>Returns a Boolean value that tells whether raster order groups are supported.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("rasterOrderGroupsSupported")]
		bool RasterOrderGroupsSupported { [Bind ("areRasterOrderGroupsSupported")] get; }

		/// <summary>Creates and returns a new library from the functions at the specified URL.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newLibraryWithURL:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLLibrary CreateLibrary (NSUrl url, [NullAllowed] out NSError error);

		/// <summary>Gets the minimum alignment required for a linear texture in the given pixel format.</summary>
		/// <param name="format">The pixel format. Depth, stencil, and compressed formats are not supported.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("minimumLinearTextureAlignmentForPixelFormat:")]
		nuint GetMinimumLinearTextureAlignment (MTLPixelFormat format);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("minimumTextureBufferAlignmentForPixelFormat:")]
		nuint GetMinimumTextureBufferAlignment (MTLPixelFormat format);

		/// <summary>Gets the largest available length of memory for threadgroups.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("maxThreadgroupMemoryLength")]
		nuint MaxThreadgroupMemoryLength { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("maxArgumentBufferSamplerCount")]
		nuint MaxArgumentBufferSamplerCount { get; }

		/// <summary>Returns a Boolean value that tells whether programmable sample positions are supported.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("programmableSamplePositionsSupported")]
		bool ProgrammableSamplePositionsSupported { [Bind ("areProgrammableSamplePositionsSupported")] get; }

		/// <summary>Provides the default sample positions for the specified sample <paramref name="count" />.</summary>
		/// <param name="positions">Array that will be filled with the default sample postions.</param>
		/// <param name="count">The number of positions, which determines the set of default positions.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("getDefaultSamplePositions:count:")]
		void GetDefaultSamplePositions (IntPtr positions, nuint count);

		/// <summary>Creates an encoder for the specified array of arguments.</summary>
		/// <param name="arguments">An array of arguments within a buffer.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newArgumentEncoderWithArguments:")]
		[return: NullAllowed]
		[return: Release]
		IMTLArgumentEncoder CreateArgumentEncoder (MTLArgumentDescriptor [] arguments);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newIndirectCommandBufferWithDescriptor:maxCommandCount:options:")]
		[return: NullAllowed]
		[return: Release]
		IMTLIndirectCommandBuffer CreateIndirectCommandBuffer (MTLIndirectCommandBufferDescriptor descriptor, nuint maxCount, MTLResourceOptions options);

		[MacCatalyst (13, 1)]
		[Abstract]
		[return: NullAllowed]
		[return: Release]
		[Export ("newEvent")]
		IMTLEvent CreateEvent ();

		[MacCatalyst (13, 1)]
		[Abstract]
		[return: NullAllowed]
		[return: Release]
		[Export ("newSharedEvent")]
		IMTLSharedEvent CreateSharedEvent ();

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newSharedEventWithHandle:")]
		[return: NullAllowed]
		[return: Release]
		IMTLSharedEvent CreateSharedEvent (MTLSharedEventHandle sharedEventHandle);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("maxBufferLength")]
		nuint MaxBufferLength { get; }

		/// <summary>Gets the registry ID.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("registryID")]
		ulong RegistryId { get; }

		/// <summary>Gets the size, in bytes, of all the resources that the device has allocated.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("currentAllocatedSize")]
		nuint CurrentAllocatedSize { get; }

#if false // https://bugzilla.xamarin.com/show_bug.cgi?id=59342
		[NoiOS, NoTV]
		[Notification]
		[Field ("MTLDeviceWasAddedNotification")]
		NSString DeviceWasAdded { get; }

		[NoiOS, NoTV]
		[Notification]
		[Field ("MTLDeviceRemovalRequestedNotification")]
		NSString DeviceRemovalRequested { get; }

		[NoiOS, NoTV]
		[Notification]
		[Field ("MTLDeviceWasRemovedNotification")]
		NSString DeviceWasRemoved { get; }
#endif

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[TV (14, 5)]
		[Abstract]
		[Export ("newRenderPipelineStateWithTileDescriptor:options:reflection:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTLTileRenderPipelineDescriptor descriptor, MTLPipelineOption options, [NullAllowed] out MTLRenderPipelineReflection reflection, [NullAllowed] out NSError error);

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[TV (14, 5)]
		[Abstract]
		[Export ("newRenderPipelineStateWithTileDescriptor:options:completionHandler:")]
		void CreateRenderPipelineState (MTLTileRenderPipelineDescriptor descriptor, MTLPipelineOption options, MTLNewRenderPipelineStateWithReflectionCompletionHandler completionHandler);

		[Abstract (GenerateExtensionMethod = true)]
		[MacCatalyst (13, 4), TV (16, 0), iOS (13, 0)]
		[Export ("supportsVertexAmplificationCount:")]
		bool SupportsVertexAmplification (nuint count);

		[Abstract (GenerateExtensionMethod = true)]
		[MacCatalyst (13, 4), TV (16, 0), iOS (13, 0)]
		[Export ("supportsRasterizationRateMapWithLayerCount:")]
		bool SupportsRasterizationRateMap (nuint layerCount);

		[Abstract (GenerateExtensionMethod = true)]
		[MacCatalyst (14, 0), TV (16, 0), iOS (13, 0)]
		[Export ("sparseTileSizeWithTextureType:pixelFormat:sampleCount:")]
		MTLSize GetSparseTileSize (MTLTextureType textureType, MTLPixelFormat pixelFormat, nuint sampleCount);

		[Abstract (GenerateExtensionMethod = true)]
		[MacCatalyst (14, 0), TV (16, 0), iOS (13, 0)]
		[Export ("sparseTileSizeInBytes")]
		nuint SparseTileSizeInBytes { get; }

		[Abstract (GenerateExtensionMethod = true)]
		[MacCatalyst (13, 4), TV (16, 0), iOS (13, 0)]
		[Export ("newRasterizationRateMapWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRasterizationRateMap CreateRasterizationRateMap (MTLRasterizationRateMapDescriptor descriptor);

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[TV (16, 0), iOS (13, 0)]
		[Export ("convertSparseTileRegions:toPixelRegions:withTileSize:numRegions:")]
		void ConvertSparseTileRegions (IntPtr tileRegions, IntPtr pixelRegions, MTLSize tileSize, nuint numRegions);

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[TV (16, 0), iOS (13, 0)]
		[Export ("convertSparsePixelRegions:toTileRegions:withTileSize:alignmentMode:numRegions:")]
		void ConvertSparsePixelRegions (IntPtr pixelRegions, IntPtr tileRegions, MTLSize tileSize, MTLSparseTextureRegionAlignmentMode mode, nuint numRegions);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("hasUnifiedMemory")]
		bool HasUnifiedMemory { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("supportsFamily:")]
		bool SupportsFamily (MTLGpuFamily gpuFamily);

		[Abstract]
		[iOS (14, 0), NoTV, MacCatalyst (14, 0)]
		[Export ("barycentricCoordsSupported")]
		bool BarycentricCoordsSupported { [Bind ("areBarycentricCoordsSupported")] get; }

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("supportsShaderBarycentricCoordinates")]
		bool SupportsShaderBarycentricCoordinates { get; }

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("peerIndex")]
		uint PeerIndex { get; }

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("peerCount")]
		uint PeerCount { get; }

		[Abstract]
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("counterSets")]
		IMTLCounterSet [] CounterSets { get; }

		[Abstract]
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("newCounterSampleBufferWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLCounterSampleBuffer CreateCounterSampleBuffer (MTLCounterSampleBufferDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("sampleTimestamps:gpuTimestamp:")]
		void GetSampleTimestamps (nuint cpuTimestamp, nuint gpuTimestamp);

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("peerGroupID")]
		ulong PeerGroupId { get; }

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("maxTransferRate")]
		ulong MaxTransferRate { get; }

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("location")]
		MTLDeviceLocation Location { get; }

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("locationNumber")]
		nuint LocationNumber { get; }

		[Abstract (GenerateExtensionMethod = true)]
		[TV (16, 0), iOS (14, 5), MacCatalyst (14, 5)]
		[Export ("supports32BitFloatFiltering")]
		bool Supports32BitFloatFiltering { get; }

		[Abstract (GenerateExtensionMethod = true)]
		[TV (16, 0), iOS (14, 5), MacCatalyst (14, 5)]
		[Export ("supports32BitMSAA")]
		bool Supports32BitMsaa { get; }

		[iOS (16, 4), TV (16, 4), MacCatalyst (16, 4)]
		[Abstract]
		[Export ("supportsBCTextureCompression")]
		bool SupportsBCTextureCompression { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("supportsPullModelInterpolation")]
		bool SupportsPullModelInterpolation { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("supportsCounterSampling:")]
		bool SupportsCounterSampling (MTLCounterSamplingPoint samplingPoint);

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("supportsDynamicLibraries")]
		bool SupportsDynamicLibraries { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("newDynamicLibrary:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLDynamicLibrary CreateDynamicLibrary (IMTLLibrary library, [NullAllowed] out NSError error);

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("newDynamicLibraryWithURL:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLDynamicLibrary CreateDynamicLibrary (NSUrl url, [NullAllowed] out NSError error);

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("newBinaryArchiveWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBinaryArchive CreateBinaryArchive (MTLBinaryArchiveDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract (GenerateExtensionMethod = true)]
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("supportsRaytracing")]
		bool SupportsRaytracing { get; }

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("accelerationStructureSizesWithDescriptor:")]
#pragma warning disable 0618 // warning CS0618: 'MTLAccelerationStructureSizes' is obsolete: 'This API is not available on this platform.'
		MTLAccelerationStructureSizes CreateAccelerationStructureSizes (MTLAccelerationStructureDescriptor descriptor);
#pragma warning restore

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newAccelerationStructureWithSize:")]
		[return: NullAllowed]
		[return: Release]
		IMTLAccelerationStructure CreateAccelerationStructure (nuint size);

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newAccelerationStructureWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLAccelerationStructure CreateAccelerationStructure (MTLAccelerationStructureDescriptor descriptor);

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("supportsFunctionPointers")]
		bool SupportsFunctionPointers { get; }

		[TV (16, 0), iOS (14, 5), MacCatalyst (14, 5)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("supportsQueryTextureLOD")]
		bool SupportsQueryTextureLod { get; }

		[Abstract]
		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("supportsRenderDynamicLibraries")]
		bool SupportsRenderDynamicLibraries { get; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("supportsRaytracingFromRender")]
		bool SupportsRaytracingFromRender { get; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("supportsPrimitiveMotionBlur")]
		bool SupportsPrimitiveMotionBlur { get; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("supportsFunctionPointersFromRender")]
		bool SupportsFunctionPointersFromRender { get; }

		[Abstract]
		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("newLibraryWithStitchedDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLLibrary CreateLibrary (MTLStitchedLibraryDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Async]
		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("newLibraryWithStitchedDescriptor:completionHandler:")]
		void CreateLibrary (MTLStitchedLibraryDescriptor descriptor, Action<IMTLLibrary, NSError> completionHandler);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("architecture")]
		MTLArchitecture Architecture { get; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("heapAccelerationStructureSizeAndAlignWithDescriptor:")]
		MTLSizeAndAlign GetHeapAccelerationStructureSizeAndAlign (MTLAccelerationStructureDescriptor descriptor);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("heapAccelerationStructureSizeAndAlignWithSize:")]
		MTLSizeAndAlign GetHeapAccelerationStructureSizeAndAlign (nuint size);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newArgumentEncoderWithBufferBinding:")]
		[return: Release]
		IMTLArgumentEncoder CreateArgumentEncoder (IMTLBufferBinding bufferBinding);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newRenderPipelineStateWithMeshDescriptor:options:reflection:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTLMeshRenderPipelineDescriptor descriptor, MTLPipelineOption options, [NullAllowed] out MTLRenderPipelineReflection reflection, [NullAllowed] out NSError error);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newRenderPipelineStateWithMeshDescriptor:options:completionHandler:")]
		void CreateRenderPipelineState (MTLMeshRenderPipelineDescriptor descriptor, MTLPipelineOption options, MTLNewRenderPipelineStateWithReflectionCompletionHandler completionHandler);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("sparseTileSizeInBytesForSparsePageSize:")]
		nuint GetSparseTileSizeInBytes (MTLSparsePageSize sparsePageSize);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("sparseTileSizeWithTextureType:pixelFormat:sampleCount:sparsePageSize:")]
		MTLSize GetSparseTileSize (MTLTextureType textureType, MTLPixelFormat pixelFormat, nuint sampleCount, MTLSparsePageSize sparsePageSize);

		[NoiOS, Mac (13, 3), NoTV, MacCatalyst (26, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("maximumConcurrentCompilationTaskCount")]
		nuint MaximumConcurrentCompilationTaskCount { get; }

		[NoiOS, Mac (13, 3), NoTV, NoMacCatalyst]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("shouldMaximizeConcurrentCompilation")]
		bool ShouldMaximizeConcurrentCompilation { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Abstract]
		[Export ("newLogStateWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLLogState GetNewLogState (MTLLogStateDescriptor descriptor, out NSError error);

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Abstract]
		[Export ("newCommandQueueWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLCommandQueue CreateCommandQueue (MTLCommandQueueDescriptor descriptor);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[return: NullAllowed]
		[Export ("newResidencySetWithDescriptor:error:")]
		[return: Release]
		IMTLResidencySet CreateResidencySet (MTLResidencySetDescriptor descriptor, out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("tensorSizeAndAlignWithDescriptor:")]
		MTLSizeAndAlign CreateTensorSizeAndAlign (MTLTensorDescriptor descriptor);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newTensorWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTensor CreateTensor (MTLTensorDescriptor descriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("functionHandleWithFunction:")]
		[return: NullAllowed]
		IMTLFunctionHandle CreateFunctionHandle (IMTLFunction function);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newCommandAllocator")]
		[return: NullAllowed]
		[return: Release]
		IMTL4CommandAllocator CreateCommandAllocator ();

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newCommandAllocatorWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4CommandAllocator CreateCommandAllocator (MTL4CommandAllocatorDescriptor descriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newMTL4CommandQueue")]
		[return: NullAllowed]
		[return: Release]
		IMTL4CommandQueue CreateMTL4CommandQueue ();

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newMTL4CommandQueueWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4CommandQueue CreateMTL4CommandQueue (MTL4CommandQueueDescriptor descriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newCommandBuffer")]
		[return: NullAllowed]
		[return: Release]
		IMTL4CommandBuffer CreateCommandBuffer ();

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newArgumentTableWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4ArgumentTable CreateArgumentTable (MTL4ArgumentTableDescriptor descriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newTextureViewPoolWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTextureViewPool CreateTextureViewPool (MTLResourceViewPoolDescriptor descriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newCompilerWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4Compiler CreateCompiler (MTL4CompilerDescriptor descriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newArchiveWithURL:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4Archive CreateArchive (NSUrl url, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newPipelineDataSetSerializerWithDescriptor:")]
		[return: Release]
		IMTL4PipelineDataSetSerializer CreatePipelineDataSetSerializer (MTL4PipelineDataSetSerializerDescriptor descriptor);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newBufferWithLength:options:placementSparsePageSize:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBuffer CreateBuffer (nuint length, MTLResourceOptions options, MTLSparsePageSize placementSparsePageSize);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newCounterHeapWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4CounterHeap CreateCounterHeap (MTL4CounterHeapDescriptor descriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("sizeOfCounterHeapEntry:")]
		nuint GetSizeOf (MTL4CounterHeapType type);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("queryTimestampFrequency")]
		ulong QueryTimestampFrequency { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("functionHandleWithBinaryFunction:")]
		[return: NullAllowed]
		[return: Release]
		IMTLFunctionHandle CreateFunctionHandle (IMTL4BinaryFunction function);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="Metal.MTLDrawable" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="Metal.MTLDrawable" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="Metal.MTLDrawable" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="Metal.MTLDrawable_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	/// <summary>Extension methods to the <see cref="Metal.IMTLDrawable" /> interface to support all the methods from the <see cref="Metal.MTLDrawable" /> protocol.</summary>
	///     <remarks>
	///       <para>The extension methods for <see cref="Metal.IMTLDrawable" /> allow developers to treat instances of the interface as having all the optional methods of the original <see cref="Metal.MTLDrawable" /> protocol.   Since the interface only contains the required members, these extension methods allow developers to call the optional members of the protocol.</para>
	///     </remarks>
	interface IMTLDrawable { }
	/// <summary>Interface definition for objects that can receive rendering commands.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLDrawable_Ref/index.html">Apple documentation for <c>MTLDrawable</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	partial interface MTLDrawable {
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("present")]
		void Present ();

		/// <param name="presentationTime">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("presentAtTime:")]
		void Present (double presentationTime);

		/// <summary>Causes the drawable to be presented at least <paramref name="duration" /> seconds after the previous drawable has been presented.</summary>
		/// <param name="duration">The minimum time after which to display the drawable.</param>
		[Abstract]
		[Introduced (PlatformName.MacCatalyst, 13, 4)]
		[Export ("presentAfterMinimumDuration:")]
		void PresentAfter (double duration);

		/// <summary>Causes the provided <paramref name="block" /> to be run after the drawable is displayed.</summary>
		/// <param name="block">The code that will be called after the drawable is displayed.</param>
		[Abstract]
		[Introduced (PlatformName.MacCatalyst, 13, 4)]
		[Export ("addPresentedHandler:")]
		void AddPresentedHandler (Action<IMTLDrawable> block);

		/// <summary>Returns the time, in seconds, when the host displayed this drawable.</summary>
		[Abstract]
		[Introduced (PlatformName.MacCatalyst, 13, 4)]
		[Export ("presentedTime")]
		double /* CFTimeInterval */ PresentedTime { get; }

		/// <summary>Returns the positive integer that identifies the drawable.</summary>
		[Abstract]
		[Introduced (PlatformName.MacCatalyst, 13, 4)]
		[Export ("drawableID")]
		nuint DrawableId { get; }
	}

	interface IMTLTexture : INativeObject { }

	// Apple added several new *required* members in iOS 9,
	// but that breaks our binary compat, so we can't do that in our existing code.
	/// <summary>System protocol for image data that is used by vertex shaders, fragment shaders, and compute kernels.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLTexture : MTLResource {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.iOS, 10, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 12)]
		[Deprecated (PlatformName.TvOS, 10, 0)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Abstract, Export ("rootResource")]
		IMTLResource RootResource { get; }

		/// <summary>Returns the parent texture.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("parentTexture")]
		IMTLTexture ParentTexture { get; }

		/// <summary>Returns the base level of the parent texture from which the target texture was created.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("parentRelativeLevel")]
		nuint ParentRelativeLevel { get; }

		/// <summary>Returns the base slice of the parent texture from which the target texture was created.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("parentRelativeSlice")]
		nuint ParentRelativeSlice { get; }

		/// <summary>Returns the buffer for the target texture.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("buffer")]
		IMTLBuffer Buffer { get; }

		/// <summary>Gets the offset into the parent texture where the the target texture data begins.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("bufferOffset")]
		nuint BufferOffset { get; }

		/// <summary>Gets the bytes per row in the buffer for the target texture.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("bufferBytesPerRow")]
		nuint BufferBytesPerRow { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("textureType")]
		MTLTextureType TextureType { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("pixelFormat")]
		MTLPixelFormat PixelFormat { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("width")]
		nuint Width { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("height")]
		nuint Height { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("depth")]
		nuint Depth { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("mipmapLevelCount")]
		nuint MipmapLevelCount { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.MacOSX, 13, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.TvOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Abstract, Export ("sampleCount")]
		nuint SampleCount { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("arrayLength")]
		nuint ArrayLength { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("framebufferOnly")]
		bool FramebufferOnly { [Bind ("isFramebufferOnly")] get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("allowGPUOptimizedContents")]
		bool AllowGpuOptimizedContents { get; }

		[Abstract (GenerateExtensionMethod = true)]
		[Mac (12, 5), iOS (15, 0), MacCatalyst (15, 0), TV (16, 0)]
		[Export ("compressionType")]
		MTLTextureCompressionType CompressionType { get; }

		/// <param name="pixelFormat">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newTextureViewWithPixelFormat:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateTextureView (MTLPixelFormat pixelFormat);

		/// <summary>Gets a description of how the texture can be used. (For example, as a write target for compute shaders.)</summary>
		[Abstract]
		[Export ("usage")]
		MTLTextureUsage Usage { get; }

		/// <summary>Creates and returns a Metal texture that shares the same memory as the source object, but that is interpreted with the new pixel format.</summary>
		[Abstract]
		[Export ("newTextureViewWithPixelFormat:textureType:levels:slices:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateTextureView (MTLPixelFormat pixelFormat, MTLTextureType textureType, NSRange levelRange, NSRange sliceRange);

		[Abstract]
		[Export ("getBytes:bytesPerRow:bytesPerImage:fromRegion:mipmapLevel:slice:")]
		void GetBytes (IntPtr pixelBytes, nuint bytesPerRow, nuint bytesPerImage, MTLRegion region, nuint level, nuint slice);

		[Abstract]
		[Export ("getBytes:bytesPerRow:fromRegion:mipmapLevel:")]
		void GetBytes (IntPtr pixelBytes, nuint bytesPerRow, MTLRegion region, nuint level);

		[Abstract]
		[Export ("replaceRegion:mipmapLevel:slice:withBytes:bytesPerRow:bytesPerImage:")]
		void ReplaceRegion (MTLRegion region, nuint level, nuint slice, IntPtr pixelBytes, nuint bytesPerRow, nuint bytesPerImage);

		[Abstract]
		[Export ("replaceRegion:mipmapLevel:withBytes:bytesPerRow:")]
		void ReplaceRegion (MTLRegion region, nuint level, IntPtr pixelBytes, nuint bytesPerRow);

		/// <summary>Gets the IOSurface that was used to create this texture, if one was used.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[NullAllowed, Export ("iosurface")]
		IOSurface.IOSurface IOSurface { get; }

		/// <summary>Returns the IOSurface plane used by the surface that is returned from <see cref="IOSurface" />.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("iosurfacePlane")]
		nuint IOSurfacePlane { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("shareable")]
		bool Shareable { [Bind ("isShareable")] get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[return: NullAllowed]
		[return: Release]
		[Export ("newSharedTextureHandle")]
		MTLSharedTextureHandle CreateSharedTextureHandle ();

		// @optional in macOS and Mac Catalyst
#if !__MACOS__ && !__MACCATALYST__
		[Abstract (GenerateExtensionMethod = true)]
#endif
		[TV (16, 0), iOS (13, 0), MacCatalyst (15, 0)]
		[Export ("firstMipmapInTail")]
		nuint FirstMipmapInTail { get; }

		// @optional in macOS and Mac Catalyst
#if !__MACOS__ && !__MACCATALYST__
		[Abstract (GenerateExtensionMethod = true)]
#endif
		[TV (16, 0), iOS (13, 0), MacCatalyst (15, 0)]
		[Export ("tailSizeInBytes")]
		nuint TailSizeInBytes { get; }

		// @optional in macOS and Mac Catalyst
#if !__MACOS__ && !__MACCATALYST__
		[Abstract (GenerateExtensionMethod = true)]
#endif
		[TV (16, 0), iOS (13, 0), MacCatalyst (15, 0)]
		[Export ("isSparse")]
		bool IsSparse { get; }

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("swizzle")]
		MTLTextureSwizzleChannels Swizzle { get; }

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("newTextureViewWithPixelFormat:textureType:levels:slices:swizzle:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture Create (MTLPixelFormat pixelFormat, MTLTextureType textureType, NSRange levelRange, NSRange sliceRange, MTLTextureSwizzleChannels swizzle);

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[NullAllowed, Export ("remoteStorageTexture")]
		IMTLTexture RemoteStorageTexture { get; }

		[Abstract]
		[NoiOS, NoTV]
		[NoMacCatalyst]
		[Export ("newRemoteTextureViewForDevice:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateRemoteTexture (IMTLDevice device);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newTextureViewWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateTextureView (MTLTextureViewDescriptor descriptor);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("sparseTextureTier")]
		MTLTextureSparseTier SparseTextureTier { get; }
	}


	/// <summary>Configuration for <see cref="Metal.IMTLTexture" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLTextureDescriptor_Ref/index.html">Apple documentation for <c>MTLTextureDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLTextureDescriptor : NSCopying {

		[Export ("textureType", ArgumentSemantic.Assign)]
		MTLTextureType TextureType { get; set; }

		[Export ("pixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat PixelFormat { get; set; }

		[Export ("width")]
		nuint Width { get; set; }

		[Export ("height")]
		nuint Height { get; set; }

		[Export ("depth")]
		nuint Depth { get; set; }

		[Export ("mipmapLevelCount")]
		nuint MipmapLevelCount { get; set; }

		[Export ("sampleCount")]
		nuint SampleCount { get; set; }

		[Export ("arrayLength")]
		nuint ArrayLength { get; set; }

		[Export ("resourceOptions", ArgumentSemantic.Assign)]
		MTLResourceOptions ResourceOptions { get; set; }

		/// <param name="pixelFormat">To be added.</param>
		/// <param name="width">To be added.</param>
		/// <param name="height">To be added.</param>
		/// <param name="mipmapped">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static, Export ("texture2DDescriptorWithPixelFormat:width:height:mipmapped:")]
		MTLTextureDescriptor CreateTexture2DDescriptor (MTLPixelFormat pixelFormat, nuint width, nuint height, bool mipmapped);

		/// <param name="pixelFormat">To be added.</param>
		/// <param name="size">To be added.</param>
		/// <param name="mipmapped">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static, Export ("textureCubeDescriptorWithPixelFormat:size:mipmapped:")]
		MTLTextureDescriptor CreateTextureCubeDescriptor (MTLPixelFormat pixelFormat, nuint size, bool mipmapped);

		/// <param name="pixelFormat">To be added.</param>
		/// <param name="width">To be added.</param>
		/// <param name="resourceOptions">To be added.</param>
		/// <param name="usage">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static, Export ("textureBufferDescriptorWithPixelFormat:width:resourceOptions:usage:")]
		MTLTextureDescriptor CreateTextureBufferDescriptor (MTLPixelFormat pixelFormat, nuint width, MTLResourceOptions resourceOptions, MTLTextureUsage usage);

		[MacCatalyst (13, 1)]
		[Export ("cpuCacheMode", ArgumentSemantic.Assign)]
		MTLCpuCacheMode CpuCacheMode { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("storageMode", ArgumentSemantic.Assign)]
		MTLStorageMode StorageMode { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("usage", ArgumentSemantic.Assign)]
		MTLTextureUsage Usage { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("allowGPUOptimizedContents")]
		bool AllowGpuOptimizedContents { get; set; }

		[Mac (12, 5), iOS (15, 0), MacCatalyst (15, 0), TV (17, 0)]
		[Export ("compressionType")]
		MTLTextureCompressionType CompressionType { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("hazardTrackingMode", ArgumentSemantic.Assign)]
		MTLHazardTrackingMode HazardTrackingMode { get; set; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("swizzle", ArgumentSemantic.Assign)]
		MTLTextureSwizzleChannels Swizzle { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("placementSparsePageSize", ArgumentSemantic.Assign)]
		MTLSparsePageSize PlacementSparsePageSize { get; set; }
	}

	/// <summary>Configures a sampler (see <see cref="Metal.IMTLSamplerState" />).</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLSamplerDescriptor_Ref/index.html">Apple documentation for <c>MTLSamplerDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLSamplerDescriptor : NSCopying {

		[Export ("minFilter", ArgumentSemantic.Assign)]
		MTLSamplerMinMagFilter MinFilter { get; set; }

		[Export ("magFilter", ArgumentSemantic.Assign)]
		MTLSamplerMinMagFilter MagFilter { get; set; }

		[Export ("mipFilter", ArgumentSemantic.Assign)]
		MTLSamplerMipFilter MipFilter { get; set; }

		[Export ("maxAnisotropy")]
		nuint MaxAnisotropy { get; set; }

		[Export ("sAddressMode", ArgumentSemantic.Assign)]
		MTLSamplerAddressMode SAddressMode { get; set; }

		[Export ("tAddressMode", ArgumentSemantic.Assign)]
		MTLSamplerAddressMode TAddressMode { get; set; }

		[Export ("rAddressMode", ArgumentSemantic.Assign)]
		MTLSamplerAddressMode RAddressMode { get; set; }

		[Export ("normalizedCoordinates")]
		bool NormalizedCoordinates { get; set; }

		[Export ("lodMinClamp")]
		float LodMinClamp { get; set; } /* float, not CGFloat */

		[Export ("lodMaxClamp")]
		float LodMaxClamp { get; set; } /* float, not CGFloat */

		[MacCatalyst (13, 1)]
		[Export ("lodAverage")]
		bool LodAverage { get; set; }

		[iOS (14, 0), TV (17, 0)]
		[MacCatalyst (14, 0)]
		[Export ("borderColor", ArgumentSemantic.Assign)]
		MTLSamplerBorderColor BorderColor { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("compareFunction")]
		MTLCompareFunction CompareFunction { get; set; }

		[Export ("label")]
		[NullAllowed]
		string Label { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("supportArgumentBuffers")]
		bool SupportArgumentBuffers { get; set; }
	}

	interface IMTLSamplerState { }
	/// <summary>System protocol the way that shaders or compute kernels will sample textures.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLSamplerState {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("label")]
		string Label { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }
	}

	/// <summary>Configures a rendering pipeline with rasterization properties, visibility, blending, and shader functions.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPipelineDescriptor_Ref/index.html">Apple documentation for <c>MTLRenderPipelineDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLRenderPipelineDescriptor : NSCopying {

		[Export ("label")]
		[NullAllowed]
		string Label { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("vertexFunction", ArgumentSemantic.Retain)]
		IMTLFunction VertexFunction { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("fragmentFunction", ArgumentSemantic.Retain)]
		IMTLFunction FragmentFunction { get; set; }

		[Export ("vertexDescriptor", ArgumentSemantic.Copy)]
		[NullAllowed]
		MTLVertexDescriptor VertexDescriptor { get; set; }

		[Deprecated (PlatformName.MacOSX, 13, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.TvOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("sampleCount")]
		nuint SampleCount { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("alphaToCoverageEnabled")]
		bool AlphaToCoverageEnabled { [Bind ("isAlphaToCoverageEnabled")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("alphaToOneEnabled")]
		bool AlphaToOneEnabled { [Bind ("isAlphaToOneEnabled")] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("rasterizationEnabled")]
		bool RasterizationEnabled { [Bind ("isRasterizationEnabled")] get; set; }

		[Export ("reset")]
		void Reset ();

		[Export ("colorAttachments")]
		MTLRenderPipelineColorAttachmentDescriptorArray ColorAttachments { get; }

		[Export ("depthAttachmentPixelFormat")]
		MTLPixelFormat DepthAttachmentPixelFormat { get; set; }

		[Export ("stencilAttachmentPixelFormat")]
		MTLPixelFormat StencilAttachmentPixelFormat { get; set; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("inputPrimitiveTopology", ArgumentSemantic.Assign)]
		MTLPrimitiveTopologyClass InputPrimitiveTopology { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("tessellationPartitionMode", ArgumentSemantic.Assign)]
		MTLTessellationPartitionMode TessellationPartitionMode { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("maxTessellationFactor")]
		nuint MaxTessellationFactor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("tessellationFactorScaleEnabled")]
		bool IsTessellationFactorScaleEnabled { [Bind ("isTessellationFactorScaleEnabled")] get; set; }

		[MacCatalyst (13, 1)]
		[Export ("tessellationFactorFormat", ArgumentSemantic.Assign)]
		MTLTessellationFactorFormat TessellationFactorFormat { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("tessellationControlPointIndexType", ArgumentSemantic.Assign)]
		MTLTessellationControlPointIndexType TessellationControlPointIndexType { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("tessellationFactorStepFunction", ArgumentSemantic.Assign)]
		MTLTessellationFactorStepFunction TessellationFactorStepFunction { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("tessellationOutputWindingOrder", ArgumentSemantic.Assign)]
		MTLWinding TessellationOutputWindingOrder { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("vertexBuffers")]
		MTLPipelineBufferDescriptorArray VertexBuffers { get; }

		[MacCatalyst (13, 1)]
		[Export ("fragmentBuffers")]
		MTLPipelineBufferDescriptorArray FragmentBuffers { get; }

		[MacCatalyst (13, 1)]
		[Export ("rasterSampleCount")]
		nuint RasterSampleCount { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("supportIndirectCommandBuffers")]
		bool SupportIndirectCommandBuffers { get; set; }

		[Introduced (PlatformName.MacCatalyst, 13, 4)]
		[TV (17, 0), iOS (13, 0)]
		[Export ("maxVertexAmplificationCount")]
		nuint MaxVertexAmplificationCount { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("binaryArchives", ArgumentSemantic.Copy)]
		IMTLBinaryArchive [] BinaryArchives { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("vertexPreloadedLibraries", ArgumentSemantic.Copy)]
		IMTLDynamicLibrary [] VertexPreloadedLibraries { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("vertexLinkedFunctions", ArgumentSemantic.Copy)]
		MTLLinkedFunctions VertexLinkedFunctions { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("supportAddingVertexBinaryFunctions")]
		bool SupportAddingVertexBinaryFunctions { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("supportAddingFragmentBinaryFunctions")]
		bool SupportAddingFragmentBinaryFunctions { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("maxVertexCallStackDepth")]
		nuint MaxVertexCallStackDepth { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("maxFragmentCallStackDepth")]
		nuint MaxFragmentCallStackDepth { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("fragmentPreloadedLibraries", ArgumentSemantic.Copy)]
		IMTLDynamicLibrary [] FragmentPreloadedLibraries { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("fragmentLinkedFunctions", ArgumentSemantic.Copy)]
		MTLLinkedFunctions FragmentLinkedFunctions { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("shaderValidation")]
		MTLShaderValidation ShaderValidation { get; set; }
	}

	/// <summary>An array of <see cref="Metal.MTLRenderPipelineColorAttachmentDescriptor" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPipelineColorAttachmentDescriptorArray_Ref/index.html">Apple documentation for <c>MTLRenderPipelineColorAttachmentDescriptorArray</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPipelineColorAttachmentDescriptorArray {

		[Export ("objectAtIndexedSubscript:"), Internal]
		MTLRenderPipelineColorAttachmentDescriptor ObjectAtIndexedSubscript (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:"), Internal]
		void SetObject ([NullAllowed] MTLRenderPipelineColorAttachmentDescriptor attachment, nuint attachmentIndex);
	}

	interface IMTLRenderPipelineState { }

	/// <summary>System protocol for encoding the state of a rendering pipeline.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLRenderPipelineState {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("label")]
		string Label { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("maxTotalThreadsPerThreadgroup")]
		nuint MaxTotalThreadsPerThreadgroup { get; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("threadgroupSizeMatchesTileSize")]
		bool ThreadgroupSizeMatchesTileSize { get; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("imageblockSampleLength")]
		nuint ImageblockSampleLength { get; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("imageblockMemoryLengthForDimensions:")]
		nuint GetImageblockMemoryLength (MTLSize imageblockDimensions);

		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("supportIndirectCommandBuffers")]
		bool SupportIndirectCommandBuffers { get; }

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("functionHandleWithFunction:stage:")]
		[return: NullAllowed]
		IMTLFunctionHandle FunctionHandleWithFunction (IMTLFunction function, MTLRenderStages stage);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newVisibleFunctionTableWithDescriptor:stage:")]
		[return: NullAllowed]
		[return: Release]
		IMTLVisibleFunctionTable NewVisibleFunctionTableWithDescriptor (MTLVisibleFunctionTableDescriptor descriptor, MTLRenderStages stage);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newIntersectionFunctionTableWithDescriptor:stage:")]
		[return: NullAllowed]
		[return: Release]
		IMTLIntersectionFunctionTable NewIntersectionFunctionTableWithDescriptor (MTLIntersectionFunctionTableDescriptor descriptor, MTLRenderStages stage);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newRenderPipelineStateWithAdditionalBinaryFunctions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState NewRenderPipelineStateWithAdditionalBinaryFunctions (MTLRenderPipelineFunctionsDescriptor additionalBinaryFunctions, [NullAllowed] out NSError error);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract]
		[Export ("meshThreadExecutionWidth")]
		nuint MeshThreadExecutionWidth { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract]
		[Export ("maxTotalThreadgroupsPerMeshGrid")]
		nuint MaxTotalThreadgroupsPerMeshGrid { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("maxTotalThreadsPerMeshThreadgroup")]
		nuint MaxTotalThreadsPerMeshThreadgroup { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("maxTotalThreadsPerObjectThreadgroup")]
		nuint MaxTotalThreadsPerObjectThreadgroup { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("objectThreadExecutionWidth")]
		nuint ObjectThreadExecutionWidth { get; }

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("shaderValidation")]
		MTLShaderValidation ShaderValidation { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[NullAllowed, Export ("reflection")]
		MTLRenderPipelineReflection Reflection { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("functionHandleWithName:stage:")]
		[return: NullAllowed]
		IMTLFunctionHandle CreateFunctionHandle (string name, MTLRenderStages stage);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("functionHandleWithBinaryFunction:stage:")]
		[return: NullAllowed]
		IMTLFunctionHandle CreateFunctionHandle (IMTL4BinaryFunction function, MTLRenderStages stage);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newRenderPipelineStateWithBinaryFunctions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTL4RenderPipelineBinaryFunctionsDescriptor binaryFunctionsDescriptor, [NullAllowed] out NSError error);

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("newRenderPipelineDescriptorForSpecialization")]
		[return: Release]
		MTL4PipelineDescriptor CreateRenderPipelineDescriptorForSpecialization ();

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("requiredThreadsPerTileThreadgroup")]
		MTLSize RequiredThreadsPerTileThreadgroup { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("requiredThreadsPerObjectThreadgroup")]
		MTLSize RequiredThreadsPerObjectThreadgroup { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("requiredThreadsPerMeshThreadgroup")]
		MTLSize RequiredThreadsPerMeshThreadgroup { get; }
	}

	/// <summary>Configures how vertex and attribute data are fetched by a vertex shader function.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLVertexBufferLayoutDescriptor_Ref/index.html">Apple documentation for <c>MTLVertexBufferLayoutDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLVertexBufferLayoutDescriptor : NSCopying {
		[Export ("stride", ArgumentSemantic.UnsafeUnretained)]
		nuint Stride { get; set; }

		[Export ("stepFunction", ArgumentSemantic.Assign)]
		MTLVertexStepFunction StepFunction { get; set; }

		[Export ("stepRate", ArgumentSemantic.UnsafeUnretained)]
		nuint StepRate { get; set; }
	}

	/// <summary>Holds an array of <see cref="Metal.MTLVertexBufferLayoutDescriptor" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLVertexBufferLayoutDescriptorArray_Ref/index.html">Apple documentation for <c>MTLVertexBufferLayoutDescriptorArray</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLVertexBufferLayoutDescriptorArray {
		[Export ("objectAtIndexedSubscript:"), Internal]
		MTLVertexBufferLayoutDescriptor ObjectAtIndexedSubscript (nuint index);

		[Export ("setObject:atIndexedSubscript:"), Internal]
		void SetObject ([NullAllowed] MTLVertexBufferLayoutDescriptor bufferDesc, nuint index);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLAttribute {
		[Export ("name")]
		string Name { get; }

		[Export ("attributeIndex")]
		nuint AttributeIndex { get; }

		[Export ("attributeType")]
		MTLDataType AttributeType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("patchData")]
		bool IsPatchData { [Bind ("isPatchData")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("patchControlPointData")]
		bool IsPatchControlPointData { [Bind ("isPatchControlPointData")] get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLAttributeDescriptor : NSCopying {
		[Export ("format", ArgumentSemantic.Assign)]
		MTLAttributeFormat Format { get; set; }

		[Export ("offset")]
		nuint Offset { get; set; }

		[Export ("bufferIndex")]
		nuint BufferIndex { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLAttributeDescriptorArray {
		[Internal]
		[Export ("objectAtIndexedSubscript:")]
		MTLAttributeDescriptor ObjectAtIndexedSubscript (nuint index);

		[Internal]
		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLAttributeDescriptor attributeDesc, nuint index);
	}

	/// <summary>An attribute for per-vertex input for a vertex shader function.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLVertexAttributeDescriptor_Ref/index.html">Apple documentation for <c>MTLVertexAttributeDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLVertexAttributeDescriptor : NSCopying {
		[Export ("format", ArgumentSemantic.Assign)]
		MTLVertexFormat Format { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		nuint Offset { get; set; }

		[Export ("bufferIndex", ArgumentSemantic.Assign)]
		nuint BufferIndex { get; set; }
	}

	/// <summary>Holds an array of <see cref="Metal.MTLVertexAttributeDescriptor" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLVertexAttributeDescriptorArray_Ref/index.html">Apple documentation for <c>MTLVertexAttributeDescriptorArray</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLVertexAttributeDescriptorArray {
		[Export ("objectAtIndexedSubscript:"), Internal]
		MTLVertexAttributeDescriptor ObjectAtIndexedSubscript (nuint index);

		[Export ("setObject:atIndexedSubscript:"), Internal]
		void SetObject ([NullAllowed] MTLVertexAttributeDescriptor attributeDesc, nuint index);
	}

	/// <summary>Maps vertex data in memory to attributes in a vertex shader.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLVertexDescriptor_Ref/index.html">Apple documentation for <c>MTLVertexDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLVertexDescriptor : NSCopying {
		[Static, Export ("vertexDescriptor")]
		MTLVertexDescriptor Create ();

		[Export ("reset")]
		void Reset ();

		[Export ("layouts")]
		MTLVertexBufferLayoutDescriptorArray Layouts { get; }

		[Export ("attributes")]
		MTLVertexAttributeDescriptorArray Attributes { get; }
	}

	/// <summary>An attribute for per-vertex input to a vertex shader function.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLVertexAttribute_Ref/index.html">Apple documentation for <c>MTLVertexAttribute</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLVertexAttribute {
		[Export ("attributeIndex")]
		nuint AttributeIndex { get; }

		[MacCatalyst (13, 1)]
		[Export ("attributeType")]
		MTLDataType AttributeType { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("active")]
		bool Active { [Bind ("isActive")] get; }

		[Export ("name")]
		string Name { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("patchData")]
		bool PatchData { [Bind ("isPatchData")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("patchControlPointData")]
		bool PatchControlPointData { [Bind ("isPatchControlPointData")] get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLFunctionConstantValues : NSCopying {
		[MacCatalyst (13, 1)]
		[Export ("init")]
		NativeHandle Constructor ();

		[Export ("setConstantValue:type:atIndex:")]
		void SetConstantValue (IntPtr value, MTLDataType type, nuint index);

		[Export ("setConstantValues:type:withRange:")]
		void SetConstantValues (IntPtr values, MTLDataType type, NSRange range);

		[Export ("setConstantValue:type:withName:")]
		void SetConstantValue (IntPtr value, MTLDataType type, string name);

		[Export ("reset")]
		void Reset ();
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLFunctionConstant {
		[Export ("name")]
		string Name { get; }

		[Export ("type")]
		MTLDataType Type { get; }

		[Export ("index")]
		nuint Index { get; }

		[Export ("required")]
		bool IsRequired { get; }
	}

	interface IMTLFunction { }
	/// <summary>System protocol for shader functions that are suitable for use on a GPU in a shader or compute function.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLFunction {

		[MacCatalyst (13, 1)]
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("functionType")]
		MTLFunctionType FunctionType { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("vertexAttributes")]
		MTLVertexAttribute [] VertexAttributes { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("name")]
		string Name { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("patchType")]
		MTLPatchType PatchType { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("patchControlPointCount")]
		nint PatchControlPointCount { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[NullAllowed, Export ("stageInputAttributes")]
		MTLAttribute [] StageInputAttributes { get; }

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("functionConstantsDictionary")]
		NSDictionary<NSString, MTLFunctionConstant> FunctionConstants { get; }

		/// <summary>Creates a new argument encoder for the specified buffer index.</summary>
		/// <param name="bufferIndex">Index into a graphics function or compute function of the argument buffer.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newArgumentEncoderWithBufferIndex:")]
		[return: Release]
		IMTLArgumentEncoder CreateArgumentEncoder (nuint bufferIndex);

		/// <summary>Creates a new argument encoder for the specified buffer index and reflection argument.</summary>
		/// <param name="bufferIndex">Index into a graphics function or compute function of the argument buffer.</param>
		/// <param name="reflection">The resulting reflection data.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newArgumentEncoderWithBufferIndex:reflection:")]
		[return: Release]
		IMTLArgumentEncoder CreateArgumentEncoder (nuint bufferIndex, [NullAllowed] out MTLArgument reflection);

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("options")]
		MTLFunctionOptions Options { get; }
	}

	interface IMTLLibrary { }

	/// <summary>System protocol for libraries of shaders.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLLibrary {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("label")]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("functionNames")]
		string [] FunctionNames { get; }

		/// <param name="functionName">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("newFunctionWithName:")]
		[return: Release]
		IMTLFunction CreateFunction (string functionName);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newFunctionWithName:constantValues:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLFunction CreateFunction (string name, MTLFunctionConstantValues constantValues, out NSError error);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newFunctionWithName:constantValues:completionHandler:")]
		[Async]
		void CreateFunction (string name, MTLFunctionConstantValues constantValues, Action<IMTLFunction, NSError> completionHandler);

		[Field ("MTLLibraryErrorDomain")]
		NSString ErrorDomain { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("newFunctionWithDescriptor:completionHandler:")]
		void CreateFunction (MTLFunctionDescriptor descriptor, Action<IMTLFunction, NSError> completionHandler);

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("newFunctionWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLFunction CreateFunction (MTLFunctionDescriptor descriptor, [NullAllowed] out NSError error);

		// protocol, so no Async
		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newIntersectionFunctionWithDescriptor:completionHandler:")]
		void CreateIntersectionFunction (MTLIntersectionFunctionDescriptor descriptor, Action<IMTLFunction, NSError> completionHandler);

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("newIntersectionFunctionWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLFunction CreateIntersectionFunction (MTLIntersectionFunctionDescriptor descriptor, [NullAllowed] out NSError error);

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("type")]
		MTLLibraryType Type { get; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[NullAllowed, Export ("installName")]
		string InstallName { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("reflectionForFunctionWithName:")]
		[return: NullAllowed]
		MTLFunctionReflection CreateFunctionReflection (string functionName);
	}

	/// <summary>Configures the compilation of a Metal shader library.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLCompileOptions_Ref/index.html">Apple documentation for <c>MTLCompileOptions</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLCompileOptions : NSCopying {

		[NullAllowed] // by default this property is null
		[Export ("preprocessorMacros", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> PreprocessorMacros { get; set; }

		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'MathMode' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'MathMode' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'MathMode' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'MathMode' instead.")]
		[Export ("fastMathEnabled")]
		bool FastMathEnabled { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("languageVersion", ArgumentSemantic.Assign)]
		MTLLanguageVersion LanguageVersion { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("libraryType", ArgumentSemantic.Assign)]
		MTLLibraryType LibraryType { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("installName")]
		string InstallName { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("libraries", ArgumentSemantic.Copy)]
		IMTLDynamicLibrary [] Libraries { get; set; }

		[Introduced (PlatformName.MacCatalyst, 14, 0)]
		[iOS (13, 0), TV (14, 0)]
		[Export ("preserveInvariance")]
		bool PreserveInvariance { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("optimizationLevel", ArgumentSemantic.Assign)]
		MTLLibraryOptimizationLevel OptimizationLevel { get; set; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4), TV (16, 4)]
		[Export ("compileSymbolVisibility", ArgumentSemantic.Assign)]
		MTLCompileSymbolVisibility CompileSymbolVisibility { get; set; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4), TV (16, 4)]
		[Export ("allowReferencingUndefinedSymbols")]
		bool AllowReferencingUndefinedSymbols { get; set; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4), TV (16, 4)]
		[Export ("maxTotalThreadsPerThreadgroup")]
		nuint MaxTotalThreadsPerThreadgroup { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("mathMode")]
		MTLMathMode MathMode { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("mathFloatingPointFunctions")]
		MTLMathFloatingPointFunctions MathFloatingPointFunctions { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("enableLogging")]
		bool EnableLogging { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("requiredThreadsPerThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerThreadgroup { get; set; }
	}

	/// <summary>Configures a stencil test operation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLStencilDescriptor_Ref/index.html">Apple documentation for <c>MTLStencilDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLStencilDescriptor : NSCopying {
		[Export ("stencilCompareFunction")]
		MTLCompareFunction StencilCompareFunction { get; set; }

		[Export ("stencilFailureOperation")]
		MTLStencilOperation StencilFailureOperation { get; set; }

		[Export ("depthFailureOperation")]
		MTLStencilOperation DepthFailureOperation { get; set; }

		[Export ("depthStencilPassOperation")]
		MTLStencilOperation DepthStencilPassOperation { get; set; }

		[Export ("readMask")]
		uint ReadMask { get; set; } /* uint32_t */

		[Export ("writeMask")]
		uint WriteMask { get; set; } /* uint32_t */
	}

	/// <summary>Describes a single field within a <see cref="Metal.MTLStructType" /> struct.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLStructMember_Ref/index.html">Apple documentation for <c>MTLStructMember</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLStructMember {
		[Export ("name")]
		string Name { get; }

		[Export ("offset")]
		nuint Offset { get; }

		[Export ("dataType")]
		MTLDataType DataType { get; }

		[Export ("structType")]
		[NullAllowed]
		MTLStructType StructType { get; }

		[Export ("arrayType")]
		[NullAllowed]
		MTLArrayType ArrayType { get; }

		[MacCatalyst (13, 1)]
		[Export ("argumentIndex")]
		nuint ArgumentIndex { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("textureReferenceType")]
		MTLTextureReferenceType TextureReferenceType { get; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("pointerType")]
		MTLPointerType PointerType { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[NullAllowed, Export ("tensorReferenceType")]
		MTLTensorReferenceType TensorReferenceType { get; }
	}

	/// <summary>Defines a type representing a struct, which can be passed as an argument to Metal functions.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLStructType_Ref/index.html">Apple documentation for <c>MTLStructType</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MTLType))]
	interface MTLStructType {
		[Export ("members")]
		MTLStructMember [] Members { get; }

		[Export ("memberByName:")]
		[return: NullAllowed]
		MTLStructMember Lookup (string name);
	}

	interface IMTLDepthStencilState { }

	/// <summary>System protocol for describing how the depth stencil should interact with the depth buffer during rendering.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLDepthStencilState {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("label")]
		string Label { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }
	}

	/// <summary>Configures a depth stencil test operation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLDepthStencilDescriptor_Ref/index.html">Apple documentation for <c>MTLDepthStencilDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface MTLDepthStencilDescriptor : NSCopying {

		[Export ("depthCompareFunction")]
		MTLCompareFunction DepthCompareFunction { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("depthWriteEnabled")]
		bool DepthWriteEnabled { [Bind ("isDepthWriteEnabled")] get; set; }

		[Export ("frontFaceStencil", ArgumentSemantic.Copy)]
		[NullAllowed]
		MTLStencilDescriptor FrontFaceStencil { get; set; }

		[Export ("backFaceStencil", ArgumentSemantic.Copy)]
		[NullAllowed]
		MTLStencilDescriptor BackFaceStencil { get; set; }

		[Export ("label")]
		[NullAllowed]
		string Label { get; set; }
	}

	interface IMTLParallelRenderCommandEncoder { }

	/// <summary>System protocol for breaking a single rendering pass into parallel command sets.</summary>
	/// <summary>Extension methods to the <see cref="Metal.IMTLParallelRenderCommandEncoder" /> interface to support all the methods from the <see cref="Metal.IMTLParallelRenderCommandEncoder" /> protocol.</summary>
	///     <remarks>
	///       <para>The extension methods for <see cref="Metal.IMTLParallelRenderCommandEncoder" /> allow developers to treat instances of the interface as having all the optional methods of the original <see cref="Metal.IMTLParallelRenderCommandEncoder" /> protocol.   Since the interface only contains the required members, these extension methods allow developers to call the optional members of the protocol.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	interface MTLParallelRenderCommandEncoder : MTLCommandEncoder {
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("renderCommandEncoder")]
		[Autorelease]
		[return: NullAllowed]
		IMTLRenderCommandEncoder CreateRenderCommandEncoder ();

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setColorStoreAction:atIndex:")]
		void SetColorStoreAction (MTLStoreAction storeAction, nuint colorAttachmentIndex);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setDepthStoreAction:")]
		void SetDepthStoreAction (MTLStoreAction storeAction);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setStencilStoreAction:")]
		void SetStencilStoreAction (MTLStoreAction storeAction);

		/// <summary>Sets the store action options on the color attachment at the specified index.</summary>
		/// <param name="storeActionOptions">The action to set.</param>
		/// <param name="colorAttachmentIndex">The index of the color attachment.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setColorStoreActionOptions:atIndex:")]
		void SetColorStoreActionOptions (MTLStoreActionOptions storeActionOptions, nuint colorAttachmentIndex);

		// <summary>Sets the store action options on the depth attachment.</summary>
		// <param name="storeActionOptions">The action options to set.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setDepthStoreActionOptions:")]
		void SetDepthStoreActionOptions (MTLStoreActionOptions storeActionOptions);

		/// <summary>Sets the store action options on the stencil attachment.</summary>
		/// <param name="storeActionOptions">The action options to set.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setStencilStoreActionOptions:")]
		void SetStencilStoreActionOptions (MTLStoreActionOptions storeActionOptions);
	}

	interface IMTLRenderCommandEncoder { }

	/// <summary>System protocol for encoding render commands and state into a buffer.</summary>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLRenderCommandEncoder : MTLCommandEncoder {

		/// <param name="pipelineState">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setRenderPipelineState:")]
		void SetRenderPipelineState (IMTLRenderPipelineState pipelineState);

		/// <param name="buffer">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVertexBuffer:offset:atIndex:")]
		void SetVertexBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		/// <param name="texture">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVertexTexture:atIndex:")]
		void SetVertexTexture (IMTLTexture texture, nuint index);

		/// <param name="sampler">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVertexSamplerState:atIndex:")]
		void SetVertexSamplerState (IMTLSamplerState sampler, nuint index);

		/// <param name="sampler">To be added.</param>
		/// <param name="lodMinClamp">To be added.</param>
		/// <param name="lodMaxClamp">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVertexSamplerState:lodMinClamp:lodMaxClamp:atIndex:")]
		void SetVertexSamplerState (IMTLSamplerState sampler, float /* float, not CGFloat */ lodMinClamp, float /* float, not CGFloat */ lodMaxClamp, nuint index);

		/// <param name="viewport">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setViewport:")]
		void SetViewport (MTLViewport viewport);

		/// <param name="frontFacingWinding">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFrontFacingWinding:")]
		void SetFrontFacingWinding (MTLWinding frontFacingWinding);

		/// <param name="cullMode">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setCullMode:")]
		void SetCullMode (MTLCullMode cullMode);

		/// <summary>Sets a value that controls how clipped values are handled.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setDepthClipMode:")]
		void SetDepthClipMode (MTLDepthClipMode depthClipMode);

		/// <param name="depthBias">To be added.</param>
		/// <param name="slopeScale">To be added.</param>
		/// <param name="clamp">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setDepthBias:slopeScale:clamp:")]
		void SetDepthBias (float /* float, not CGFloat */ depthBias, float /* float, not CGFloat */ slopeScale, float /* float, not CGFloat */ clamp);

		/// <param name="rect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setScissorRect:")]
		void SetScissorRect (MTLScissorRect rect);

		/// <param name="fillMode">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setTriangleFillMode:")]
		void SetTriangleFillMode (MTLTriangleFillMode fillMode);

		/// <param name="buffer">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentBuffer:offset:atIndex:")]
		void SetFragmentBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Abstract, Export ("setFragmentBufferOffset:atIndex:")]
		void SetFragmentBufferOffset (nuint offset, nuint index);

		[MacCatalyst (13, 1)]
		[Abstract, Export ("setFragmentBytes:length:atIndex:")]
		void SetFragmentBytes (IntPtr bytes, nuint length, nuint index);

		/// <param name="texture">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentTexture:atIndex:")]
		void SetFragmentTexture (IMTLTexture texture, nuint index);

		/// <param name="sampler">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentSamplerState:atIndex:")]
		void SetFragmentSamplerState (IMTLSamplerState sampler, nuint index);

		/// <param name="sampler">To be added.</param>
		/// <param name="lodMinClamp">To be added.</param>
		/// <param name="lodMaxClamp">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentSamplerState:lodMinClamp:lodMaxClamp:atIndex:")]
		void SetFragmentSamplerState (IMTLSamplerState sampler, float /* float, not CGFloat */ lodMinClamp, float /* float, not CGFloat */ lodMaxClamp, nuint index);

		/// <param name="red">To be added.</param>
		/// <param name="green">To be added.</param>
		/// <param name="blue">To be added.</param>
		/// <param name="alpha">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setBlendColorRed:green:blue:alpha:")]
		void SetBlendColor (float /* float, not CGFloat */ red, float /* float, not CGFloat */ green, float /* float, not CGFloat */ blue, float /* float, not CGFloat */ alpha);

		/// <param name="depthStencilState">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setDepthStencilState:")]
		void SetDepthStencilState (IMTLDepthStencilState depthStencilState);

		/// <param name="referenceValue">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setStencilReferenceValue:")]
		void SetStencilReferenceValue (uint /* uint32_t */ referenceValue);

		/// <summary>Sets the front and back reference stencil values.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setStencilFrontReferenceValue:backReferenceValue:")]
		void SetStencilFrontReferenceValue (uint frontReferenceValue, uint backReferenceValue);

		/// <param name="mode">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVisibilityResultMode:offset:")]
		void SetVisibilityResultMode (MTLVisibilityResultMode mode, nuint offset);

		/// <summary>Sets a value that controls how color results are handled after a rendering pass.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setColorStoreAction:atIndex:")]
		void SetColorStoreAction (MTLStoreAction storeAction, nuint colorAttachmentIndex);

		/// <summary>Sets a value that controls how depth results are handled after a rendering pass.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setDepthStoreAction:")]
		void SetDepthStoreAction (MTLStoreAction storeAction);

		/// <summary>Sets a value that controls how stencil results are handled after a rendering pass.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setStencilStoreAction:")]
		void SetStencilStoreAction (MTLStoreAction storeAction);

		/// <param name="primitiveType">To be added.</param>
		/// <param name="vertexStart">To be added.</param>
		/// <param name="vertexCount">To be added.</param>
		/// <param name="instanceCount">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("drawPrimitives:vertexStart:vertexCount:instanceCount:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, nuint vertexStart, nuint vertexCount, nuint instanceCount);

		/// <param name="primitiveType">To be added.</param>
		/// <param name="vertexStart">To be added.</param>
		/// <param name="vertexCount">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("drawPrimitives:vertexStart:vertexCount:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, nuint vertexStart, nuint vertexCount);

		/// <param name="primitiveType">To be added.</param>
		/// <param name="indexCount">To be added.</param>
		/// <param name="indexType">To be added.</param>
		/// <param name="indexBuffer">To be added.</param>
		/// <param name="indexBufferOffset">To be added.</param>
		/// <param name="instanceCount">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("drawIndexedPrimitives:indexCount:indexType:indexBuffer:indexBufferOffset:instanceCount:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, nuint indexCount, MTLIndexType indexType, IMTLBuffer indexBuffer, nuint indexBufferOffset, nuint instanceCount);

		/// <param name="primitiveType">To be added.</param>
		/// <param name="indexCount">To be added.</param>
		/// <param name="indexType">To be added.</param>
		/// <param name="indexBuffer">To be added.</param>
		/// <param name="indexBufferOffset">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("drawIndexedPrimitives:indexCount:indexType:indexBuffer:indexBufferOffset:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, nuint indexCount, MTLIndexType indexType, IMTLBuffer indexBuffer, nuint indexBufferOffset);

		/// <summary>Draws a range of primitives.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("drawPrimitives:vertexStart:vertexCount:instanceCount:baseInstance:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, nuint vertexStart, nuint vertexCount, nuint instanceCount, nuint baseInstance);

		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("drawIndexedPrimitives:indexCount:indexType:indexBuffer:indexBufferOffset:instanceCount:baseVertex:baseInstance:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, nuint indexCount, MTLIndexType indexType, IMTLBuffer indexBuffer, nuint indexBufferOffset, nuint instanceCount, nint baseVertex, nuint baseInstance);

		/// <summary>Draws a range of primitives.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("drawPrimitives:indirectBuffer:indirectBufferOffset:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, IMTLBuffer indirectBuffer, nuint indirectBufferOffset);

		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("drawIndexedPrimitives:indexType:indexBuffer:indexBufferOffset:indirectBuffer:indirectBufferOffset:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, MTLIndexType indexType, IMTLBuffer indexBuffer, nuint indexBufferOffset, IMTLBuffer indirectBuffer, nuint indirectBufferOffset);

		/// <param name="buffers">To be added.</param>
		/// <param name="IntPtrOffsets">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentBuffers:offsets:withRange:")]
		void SetFragmentBuffers (IMTLBuffer buffers, IntPtr IntPtrOffsets, NSRange range);

		/// <param name="samplers">To be added.</param>
		/// <param name="floatArrayPtrLodMinClamps">To be added.</param>
		/// <param name="floatArrayPtrLodMaxClamps">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentSamplerStates:lodMinClamps:lodMaxClamps:withRange:")]
		void SetFragmentSamplerStates (IMTLSamplerState [] samplers, IntPtr floatArrayPtrLodMinClamps, IntPtr floatArrayPtrLodMaxClamps, NSRange range);

		/// <param name="samplers">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentSamplerStates:withRange:")]
		void SetFragmentSamplerStates (IMTLSamplerState [] samplers, NSRange range);

		/// <param name="textures">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setFragmentTextures:withRange:")]
		void SetFragmentTextures (IMTLTexture [] textures, NSRange range);

		/// <param name="buffers">To be added.</param>
		/// <param name="uintArrayPtrOffsets">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVertexBuffers:offsets:withRange:")]
		void SetVertexBuffers (IMTLBuffer [] buffers, IntPtr uintArrayPtrOffsets, NSRange range);

		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Abstract, Export ("setVertexBufferOffset:atIndex:")]
		void SetVertexBufferOffset (nuint offset, nuint index);

		[MacCatalyst (13, 1)]
		[Abstract, Export ("setVertexBytes:length:atIndex:")]
		void SetVertexBytes (IntPtr bytes, nuint length, nuint index);

		/// <param name="samplers">To be added.</param>
		/// <param name="floatArrayPtrLodMinClamps">To be added.</param>
		/// <param name="floatArrayPtrLodMaxClamps">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVertexSamplerStates:lodMinClamps:lodMaxClamps:withRange:")]
		void SetVertexSamplerStates (IMTLSamplerState [] samplers, IntPtr floatArrayPtrLodMinClamps, IntPtr floatArrayPtrLodMaxClamps, NSRange range);

		/// <param name="samplers">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setVertexSamplerStates:withRange:")]
		void SetVertexSamplerStates (IMTLSamplerState [] samplers, NSRange range);

		/// <param name="textures">To be added.</param>
		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setVertexTextures:withRange:")]
		void SetVertexTextures (IMTLTexture [] textures, NSRange range);

		[NoiOS, NoTV]
		[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'MemoryBarrier (MTLBarrierScope, MTLRenderStages, MTLRenderStages)' instead.")]
		[NoMacCatalyst]
		[Abstract]
		[Export ("textureBarrier")]
		void TextureBarrier ();

		/// <summary>Captures all GPU work up to the current fence.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("updateFence:afterStages:")]
		void Update (IMTLFence fence, MTLRenderStages stages);

		/// <summary>Prevents additional GPU work by the encoder until the <paramref name="fence" /> is reached.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("waitForFence:beforeStages:")]
		void Wait (IMTLFence fence, MTLRenderStages stages);

		/// <summary>Sets the offset and stride value for a tessellation buffer.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTessellationFactorBuffer:offset:instanceStride:")]
		void SetTessellationFactorBuffer ([NullAllowed] IMTLBuffer buffer, nuint offset, nuint instanceStride);

		/// <summary>Sets the offset and stride value for a tessellation buffer.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTessellationFactorScale:")]
		void SetTessellationFactorScale (float scale);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("drawPatches:patchStart:patchCount:patchIndexBuffer:patchIndexBufferOffset:instanceCount:baseInstance:")]
		void DrawPatches (nuint numberOfPatchControlPoints, nuint patchStart, nuint patchCount, [NullAllowed] IMTLBuffer patchIndexBuffer, nuint patchIndexBufferOffset, nuint instanceCount, nuint baseInstance);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("drawPatches:patchIndexBuffer:patchIndexBufferOffset:indirectBuffer:indirectBufferOffset:")]
		void DrawPatches (nuint numberOfPatchControlPoints, [NullAllowed] IMTLBuffer patchIndexBuffer, nuint patchIndexBufferOffset, IMTLBuffer indirectBuffer, nuint indirectBufferOffset);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("drawIndexedPatches:patchStart:patchCount:patchIndexBuffer:patchIndexBufferOffset:controlPointIndexBuffer:controlPointIndexBufferOffset:instanceCount:baseInstance:")]
		void DrawIndexedPatches (nuint numberOfPatchControlPoints, nuint patchStart, nuint patchCount, [NullAllowed] IMTLBuffer patchIndexBuffer, nuint patchIndexBufferOffset, IMTLBuffer controlPointIndexBuffer, nuint controlPointIndexBufferOffset, nuint instanceCount, nuint baseInstance);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("drawIndexedPatches:patchIndexBuffer:patchIndexBufferOffset:controlPointIndexBuffer:controlPointIndexBufferOffset:indirectBuffer:indirectBufferOffset:")]
		void DrawIndexedPatches (nuint numberOfPatchControlPoints, [NullAllowed] IMTLBuffer patchIndexBuffer, nuint patchIndexBufferOffset, IMTLBuffer controlPointIndexBuffer, nuint controlPointIndexBufferOffset, IMTLBuffer indirectBuffer, nuint indirectBufferOffset);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setViewports:count:")]
		void SetViewports (IntPtr viewports, nuint count);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setScissorRects:count:")]
		void SetScissorRects (IntPtr scissorRects, nuint count);

		/// <summary>Sets the store action options on the color attachment at the specified index.</summary>
		/// <param name="storeActionOptions">The action options to set.</param>
		/// <param name="colorAttachmentIndex">The index of the color attachment.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setColorStoreActionOptions:atIndex:")]
		void SetColorStoreActionOptions (MTLStoreActionOptions storeActionOptions, nuint colorAttachmentIndex);

		/// <summary>Sets the store action options on the depth attachment.</summary>
		/// <param name="storeActionOptions">The action options to set.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setDepthStoreActionOptions:")]
		void SetDepthStoreActionOptions (MTLStoreActionOptions storeActionOptions);

		/// <summary>Sets the store action options on the stencil attachment.</summary>
		/// <param name="storeActionOptions">The action options to set.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setStencilStoreActionOptions:")]
		void SetStencilStoreActionOptions (MTLStoreActionOptions storeActionOptions);

		/// <summary>Marks the specified resource as usable by a render pass.</summary>
		/// <param name="resource">The resource to use.</param>
		/// <param name="usage">Whether to read, write, or sample the resource.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useResource:usage:")]
		void UseResource (IMTLResource resource, MTLResourceUsage usage);

		/// <summary>Marks the specified resources as usable by a render pass.</summary>
		/// <param name="resources">The resources to use.</param>
		/// <param name="count">The number of resources.</param>
		/// <param name="usage">Whether to read, write, or sample the resource.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useResources:count:usage:")]
		void UseResources (IMTLResource [] resources, nuint count, MTLResourceUsage usage);

		/// <summary>Marks the specified heap as usable by a render pass.</summary>
		/// <param name="heap">The heap from which to read resources that are wrapped in an argument buffer.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useHeap:")]
		void UseHeap (IMTLHeap heap);

		/// <summary>Marks the specified heaps as usable by a render pass.</summary>
		/// <param name="heaps">The heaps from which to read resources that are wrapped in an argument buffer.</param>
		/// <param name="count">The number of heaps.</param>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("useHeaps:count:")]
		void UseHeaps (IMTLHeap [] heaps, nuint count);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("executeCommandsInBuffer:withRange:")]
		void ExecuteCommands (IMTLIndirectCommandBuffer indirectCommandBuffer, NSRange executionRange);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("executeCommandsInBuffer:indirectBuffer:indirectBufferOffset:")]
		void ExecuteCommands (IMTLIndirectCommandBuffer indirectCommandbuffer, IMTLBuffer indirectRangeBuffer, nuint indirectBufferOffset);

		/// <param name="scope">To be added.</param>
		/// <param name="after">To be added.</param>
		/// <param name="before">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract (GenerateExtensionMethod = true)]
		[iOS (16, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Export ("memoryBarrierWithScope:afterStages:beforeStages:")]
		void MemoryBarrier (MTLBarrierScope scope, MTLRenderStages after, MTLRenderStages before);

		/// <param name="resources">To be added.</param>
		/// <param name="count">To be added.</param>
		/// <param name="after">To be added.</param>
		/// <param name="before">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract (GenerateExtensionMethod = true)]
		[iOS (16, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Export ("memoryBarrierWithResources:count:afterStages:beforeStages:")]
		void MemoryBarrier (IMTLResource [] resources, nuint count, MTLRenderStages after, MTLRenderStages before);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("tileWidth")]
		nuint TileWidth { get; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("tileHeight")]
		nuint TileHeight { get; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileBytes:length:atIndex:")]
		void SetTileBytes (IntPtr /* void* */ bytes, nuint length, nuint index);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileBuffer:offset:atIndex:")]
		void SetTileBuffer ([NullAllowed] IMTLBuffer buffer, nuint offset, nuint index);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileBufferOffset:atIndex:")]
		void SetTileBufferOffset (nuint offset, nuint index);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileBuffers:offsets:withRange:")]
		void SetTileBuffers (IMTLBuffer [] buffers, IntPtr offsets, NSRange range);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileTexture:atIndex:")]
		void SetTileTexture ([NullAllowed] IMTLTexture texture, nuint index);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileTextures:withRange:")]
		void SetTileTextures (IMTLTexture [] textures, NSRange range);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileSamplerState:atIndex:")]
		void SetTileSamplerState ([NullAllowed] IMTLSamplerState sampler, nuint index);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileSamplerStates:withRange:")]
		void SetTileSamplerStates (IMTLSamplerState [] samplers, NSRange range);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileSamplerState:lodMinClamp:lodMaxClamp:atIndex:")]
		void SetTileSamplerState ([NullAllowed] IMTLSamplerState sampler, float lodMinClamp, float lodMaxClamp, nuint index);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setTileSamplerStates:lodMinClamps:lodMaxClamps:withRange:")]
		void SetTileSamplerStates (IMTLSamplerState [] samplers, IntPtr /* float[] */ lodMinClamps, IntPtr /* float[] */ lodMaxClamps, NSRange range);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("dispatchThreadsPerTile:")]
		void DispatchThreadsPerTile (MTLSize threadsPerTile);

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setThreadgroupMemoryLength:offset:atIndex:")]
		void SetThreadgroupMemoryLength (nuint length, nuint offset, nuint index);

		[Abstract (GenerateExtensionMethod = true)]
		[MacCatalyst (13, 4), TV (16, 0), iOS (13, 0)]
		[Export ("setVertexAmplificationCount:viewMappings:")]
		void SetVertexAmplificationCount (nuint count, MTLVertexAmplificationViewMapping viewMappings);

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("useResource:usage:stages:")]
		void UseResource (IMTLResource resource, MTLResourceUsage usage, MTLRenderStages stages);

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("useResources:count:usage:stages:")]
		void UseResources (IMTLResource [] resources, nuint count, MTLResourceUsage usage, MTLRenderStages stages);

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("useHeap:stages:")]
		void UseHeap (IMTLHeap heap, MTLRenderStages stages);

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("useHeaps:count:stages:")]
		void UseHeaps (IMTLHeap [] heaps, nuint count, MTLRenderStages stages);

		[Abstract]
		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("sampleCountersInBuffer:atSampleIndex:withBarrier:")]
		void SampleCounters (IMTLCounterSampleBuffer sampleBuffer, nuint sampleIndex, bool barrier);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexVisibleFunctionTable:atBufferIndex:")]
		void SetVertexVisibleFunctionTable ([NullAllowed] IMTLVisibleFunctionTable functionTable, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexVisibleFunctionTables:withBufferRange:")]
		void SetVertexVisibleFunctionTables (IMTLVisibleFunctionTable [] functionTables, NSRange range);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexIntersectionFunctionTable:atBufferIndex:")]
		void SetVertexIntersectionFunctionTable ([NullAllowed] IMTLIntersectionFunctionTable intersectionFunctionTable, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexIntersectionFunctionTables:withBufferRange:")]
		void SetVertexIntersectionFunctionTables (IMTLIntersectionFunctionTable [] intersectionFunctionTable, NSRange range);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexAccelerationStructure:atBufferIndex:")]
		void SetVertexAccelerationStructure ([NullAllowed] IMTLAccelerationStructure accelerationStructure, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setFragmentAccelerationStructure:atBufferIndex:")]
		void SetFragmentAccelerationStructure ([NullAllowed] IMTLAccelerationStructure accelerationStructure, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setFragmentIntersectionFunctionTable:atBufferIndex:")]
		void SetFragmentIntersectionFunctionTable ([NullAllowed] IMTLIntersectionFunctionTable intersectionFunctionTable, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setFragmentIntersectionFunctionTables:withBufferRange:")]
		void SetFragmentIntersectionFunctionTables (IMTLIntersectionFunctionTable [] intersectionFunctionTable, NSRange range);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setFragmentVisibleFunctionTable:atBufferIndex:")]
		void SetFragmentVisibleFunctionTable ([NullAllowed] IMTLVisibleFunctionTable functionTable, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setFragmentVisibleFunctionTables:withBufferRange:")]
		void SetFragmentVisibleFunctionTables (IMTLVisibleFunctionTable [] functionTables, NSRange range);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setTileAccelerationStructure:atBufferIndex:")]
		void SetTileAccelerationStructure ([NullAllowed] IMTLAccelerationStructure accelerationStructure, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setTileIntersectionFunctionTable:atBufferIndex:")]
		void SetTileIntersectionFunctionTable ([NullAllowed] IMTLIntersectionFunctionTable intersectionFunctionTable, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setTileIntersectionFunctionTables:withBufferRange:")]
		void SetTileIntersectionFunctionTables (IMTLIntersectionFunctionTable [] intersectionFunctionTable, NSRange range);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setTileVisibleFunctionTable:atBufferIndex:")]
		void SetTileVisibleFunctionTable ([NullAllowed] IMTLVisibleFunctionTable functionTable, nuint bufferIndex);

		[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setTileVisibleFunctionTables:withBufferRange:")]
		void SetTileVisibleFunctionTables (IMTLVisibleFunctionTable [] functionTables, NSRange range);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexBuffer:offset:attributeStride:atIndex:")]
		void SetVertexBuffer ([NullAllowed] IMTLBuffer buffer, nuint offset, nuint stride, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexBuffers:offsets:attributeStrides:withRange:")]
		void SetVertexBuffers (IntPtr buffers, IntPtr offsets, IntPtr strides, NSRange range);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexBufferOffset:attributeStride:atIndex:")]
		void SetVertexBufferOffset (nuint offset, nuint stride, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexBytes:length:attributeStride:atIndex:")]
		void SetVertexBytes (IntPtr bytes, nuint length, nuint stride, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("drawMeshThreadgroups:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreadgroups (MTLSize threadgroupsPerGrid, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("drawMeshThreadgroupsWithIndirectBuffer:indirectBufferOffset:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreadgroups (IMTLBuffer indirectBuffer, nuint indirectBufferOffset, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("drawMeshThreads:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreads (MTLSize threadsPerGrid, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshBufferOffset:atIndex:")]
		void SetMeshBufferOffset (nuint offset, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshBuffers:offsets:withRange:")]
		void SetMeshBuffers (IntPtr buffers, IntPtr offsets, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshTexture:atIndex:")]
		void SetMeshTexture ([NullAllowed] IMTLTexture texture, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshTextures:withRange:")]
		void SetMeshTextures (IntPtr textures, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshSamplerState:atIndex:")]
		void SetMeshSamplerState ([NullAllowed] IMTLSamplerState sampler, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshSamplerStates:withRange:")]
		void SetMeshSamplerStates (IntPtr samplers, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshSamplerState:lodMinClamp:lodMaxClamp:atIndex:")]
		void SetMeshSamplerState ([NullAllowed] IMTLSamplerState sampler, float lodMinClamp, float lodMaxClamp, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshSamplerStates:lodMinClamps:lodMaxClamps:withRange:")]
		void SetMeshSamplerStates (IntPtr samplers, IntPtr lodMinClamps, IntPtr lodMaxClamps, NSRange range);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectBuffer:offset:atIndex:")]
		void SetObjectBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectBufferOffset:atIndex:")]
		void SetObjectBufferOffset (nuint offset, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectBuffers:offsets:withRange:")]
		void SetObjectBuffers (IntPtr buffers, IntPtr offsets, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectBytes:length:atIndex:")]
		void SetObjectBytes (IntPtr bytes, nuint length, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshBuffer:offset:atIndex:")]
		void SetMeshBuffer ([NullAllowed] IMTLBuffer buffer, nuint offset, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshBytes:length:atIndex:")]
		void SetMeshBytes (IntPtr bytes, nuint length, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectSamplerState:atIndex:")]
		void SetObjectSamplerState ([NullAllowed] IMTLSamplerState sampler, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectSamplerState:lodMinClamp:lodMaxClamp:atIndex:")]
		void SetObjectSamplerState ([NullAllowed] IMTLSamplerState sampler, float lodMinClamp, float lodMaxClamp, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectSamplerStates:lodMinClamps:lodMaxClamps:withRange:")]
		void SetObjectSamplerStates (IntPtr samplers, IntPtr lodMinClamps, IntPtr lodMaxClamps, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectSamplerStates:withRange:")]
		void SetObjectSamplerStates (IntPtr samplers, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectTexture:atIndex:")]
		void SetObjectTexture ([NullAllowed] IMTLTexture texture, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectTextures:withRange:")]
		void SetObjectTextures (IntPtr textures, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectThreadgroupMemoryLength:atIndex:")]
		void SetObjectThreadgroupMemoryLength (nuint length, nuint index);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract]
		[Export ("setColorAttachmentMap:")]
		void SetColorAttachmentMap ([NullAllowed] MTLLogicalToPhysicalColorAttachmentMap mapping);
	}

	/// <summary>Configures a color attachment associated with a rendering pipeline.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPipelineColorAttachmentDescriptor_Ref/index.html">Apple documentation for <c>MTLRenderPipelineColorAttachmentDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPipelineColorAttachmentDescriptor : NSCopying {

		[Export ("pixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat PixelFormat { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("blendingEnabled")]
		bool BlendingEnabled { [Bind ("isBlendingEnabled")] get; set; }

		[Export ("sourceRGBBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor SourceRgbBlendFactor { get; set; }

		[Export ("destinationRGBBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor DestinationRgbBlendFactor { get; set; }

		[Export ("rgbBlendOperation", ArgumentSemantic.Assign)]
		MTLBlendOperation RgbBlendOperation { get; set; }

		[Export ("sourceAlphaBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor SourceAlphaBlendFactor { get; set; }

		[Export ("destinationAlphaBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor DestinationAlphaBlendFactor { get; set; }

		[Export ("alphaBlendOperation", ArgumentSemantic.Assign)]
		MTLBlendOperation AlphaBlendOperation { get; set; }

		[Export ("writeMask", ArgumentSemantic.Assign)]
		MTLColorWriteMask WriteMask { get; set; }
	}

	/// <summary>The arguments (see <see cref="Metal.MTLArgument" />) of a vertex or fragment function within a <see cref="Metal.IMTLRenderPipelineState" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPipelineReflection_Ref/index.html">Apple documentation for <c>MTLRenderPipelineReflection</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPipelineReflection {
		[Deprecated (PlatformName.MacOSX, 13, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.TvOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("vertexArguments")]
		[NullAllowed]
		MTLArgument [] VertexArguments { get; }

		[Deprecated (PlatformName.MacOSX, 13, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.TvOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("fragmentArguments")]
		[NullAllowed]
		MTLArgument [] FragmentArguments { get; }

		[Deprecated (PlatformName.MacOSX, 13, 0)]
		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.TvOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[TV (14, 5)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("tileArguments")]
		MTLArgument [] TileArguments { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("vertexBindings")]
		IMTLBinding [] VertexBindings { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("fragmentBindings")]
		IMTLBinding [] FragmentBindings { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("tileBindings")]
		IMTLBinding [] TileBindings { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("objectBindings")]
		IMTLBinding [] ObjectBindings { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("meshBindings")]
		IMTLBinding [] MeshBindings { get; }
	}

	/// <summary>Configures a render target of a framebuffer.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPassAttachmentDescriptor_Ref/index.html">Apple documentation for <c>MTLRenderPassAttachmentDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPassAttachmentDescriptor : NSCopying {

		[NullAllowed] // by default this property is null
		[Export ("texture", ArgumentSemantic.Retain)]
		IMTLTexture Texture { get; set; }

		[Export ("level")]
		nuint Level { get; set; }

		[Export ("slice")]
		nuint Slice { get; set; }

		[Export ("depthPlane")]
		nuint DepthPlane { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("resolveTexture", ArgumentSemantic.Retain)]
		IMTLTexture ResolveTexture { get; set; }

		[Export ("resolveLevel")]
		nuint ResolveLevel { get; set; }

		[Export ("resolveSlice")]
		nuint ResolveSlice { get; set; }

		[Export ("resolveDepthPlane")]
		nuint ResolveDepthPlane { get; set; }

		[Export ("loadAction")]
		MTLLoadAction LoadAction { get; set; }

		[Export ("storeAction")]
		MTLStoreAction StoreAction { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("storeActionOptions", ArgumentSemantic.Assign)]
		MTLStoreActionOptions StoreActionOptions { get; set; }
	}

	/// <summary>A <see cref="Metal.MTLRenderPassAttachmentDescriptor" /> that holds the clear color for the rendering pass.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPassColorAttachmentDescriptor_Ref/index.html">Apple documentation for <c>MTLRenderPassColorAttachmentDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MTLRenderPassAttachmentDescriptor))]
	interface MTLRenderPassColorAttachmentDescriptor {
		[Export ("clearColor")]
		MTLClearColor ClearColor { get; set; }
	}

	/// <summary>A <see cref="Metal.MTLRenderPassAttachmentDescriptor" /> that holds the clear depth for a rendering pass.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPassDepthAttachmentDescriptor_Ref/index.html">Apple documentation for <c>MTLRenderPassDepthAttachmentDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MTLRenderPassAttachmentDescriptor))]
	interface MTLRenderPassDepthAttachmentDescriptor {

		[Export ("clearDepth")]
		double ClearDepth { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("depthResolveFilter")]
		MTLMultisampleDepthResolveFilter DepthResolveFilter { get; set; }
	}

	/// <summary>A <see cref="Metal.MTLRenderPassAttachmentDescriptor" /> that holds the clear stencil for a rendering pass.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/help/MTLRenderPassStencilAttachmentDescriptor_Ref/index.html">Apple documentation for <c>MTLRenderPassStencilAttachmentDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (MTLRenderPassAttachmentDescriptor))]
	interface MTLRenderPassStencilAttachmentDescriptor {

		[Export ("clearStencil")]
		uint ClearStencil { get; set; } /* uint32_t */

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("stencilResolveFilter", ArgumentSemantic.Assign)]
		MTLMultisampleStencilResolveFilter StencilResolveFilter { get; set; }
	}

	/// <summary>Holds an array of <see cref="Metal.MTLRenderPassColorAttachmentDescriptor" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPassColorAttachmentDescriptorArray_Ref/index.html">Apple documentation for <c>MTLRenderPassColorAttachmentDescriptorArray</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPassColorAttachmentDescriptorArray {
		[Export ("objectAtIndexedSubscript:"), Internal]
		MTLRenderPassColorAttachmentDescriptor ObjectAtIndexedSubscript (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:"), Internal]
		void SetObject ([NullAllowed] MTLRenderPassColorAttachmentDescriptor attachment, nuint attachmentIndex);
	}

	/// <summary>Defines the rendering target for pixels generated by a rendering pass.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLRenderPassDescriptor_Ref/index.html">Apple documentation for <c>MTLRenderPassDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPassDescriptor : NSCopying {

		[Export ("colorAttachments")]
		MTLRenderPassColorAttachmentDescriptorArray ColorAttachments { get; }

		[Export ("depthAttachment", ArgumentSemantic.Copy)]
		[NullAllowed]
		MTLRenderPassDepthAttachmentDescriptor DepthAttachment { get; set; }

		[Export ("stencilAttachment", ArgumentSemantic.Copy)]
		[NullAllowed]
		MTLRenderPassStencilAttachmentDescriptor StencilAttachment { get; set; }

		[NullAllowed] // by default this property is null
		[Export ("visibilityResultBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer VisibilityResultBuffer { get; set; }

		[Static, Export ("renderPassDescriptor")]
		[Autorelease]
		MTLRenderPassDescriptor CreateRenderPassDescriptor ();

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("renderTargetArrayLength")]
		nuint RenderTargetArrayLength { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("setSamplePositions:count:")]
		unsafe void SetSamplePositions ([NullAllowed] IntPtr positions, nuint count);

		[MacCatalyst (13, 1)]
		[Export ("getSamplePositions:count:")]
		nuint GetSamplePositions ([NullAllowed] IntPtr positions, nuint count);

		[TV (14, 5)]
		[MacCatalyst (14, 0)]
		[Export ("imageblockSampleLength")]
		nuint ImageblockSampleLength { get; set; }

		[TV (14, 5)]
		[MacCatalyst (14, 0)]
		[Export ("threadgroupMemoryLength")]
		nuint ThreadgroupMemoryLength { get; set; }

		[TV (14, 5)]
		[MacCatalyst (14, 0)]
		[Export ("tileWidth")]
		nuint TileWidth { get; set; }

		[TV (14, 5)]
		[MacCatalyst (14, 0)]
		[Export ("tileHeight")]
		nuint TileHeight { get; set; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("defaultRasterSampleCount")]
		nuint DefaultRasterSampleCount { get; set; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("renderTargetWidth")]
		nuint RenderTargetWidth { get; set; }

		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("renderTargetHeight")]
		nuint RenderTargetHeight { get; set; }

		/* Selectors reported missing by instrospection: https://github.com/xamarin/maccore/issues/1978
				[NoMac, NoTV, iOS (13, 0)]
				[NoMacCatalyst]
				[Export ("maxVertexAmplificationCount")]
				nuint MaxVertexAmplificationCount { get; set; }
		*/

		[Introduced (PlatformName.MacCatalyst, 13, 4)]
		[TV (17, 0), iOS (13, 0)]
		[NullAllowed, Export ("rasterizationRateMap", ArgumentSemantic.Strong)]
		IMTLRasterizationRateMap RasterizationRateMap { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("sampleBufferAttachments")]
		MTLRenderPassSampleBufferAttachmentDescriptorArray SampleBufferAttachments { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("visibilityResultType", ArgumentSemantic.Assign)]
		MTLVisibilityResultType VisibilityResultType { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("supportColorAttachmentMapping")]
		bool SupportColorAttachmentMapping { get; set; }
	}


	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	// note: type works only on devices, symbol is missing on the simulator
	interface MTLHeapDescriptor : NSCopying {
		[Export ("size")]
		nuint Size { get; set; }

		[Export ("storageMode", ArgumentSemantic.Assign)]
		MTLStorageMode StorageMode { get; set; }

		[Export ("cpuCacheMode", ArgumentSemantic.Assign)]
		MTLCpuCacheMode CpuCacheMode { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("hazardTrackingMode", ArgumentSemantic.Assign)]
		MTLHazardTrackingMode HazardTrackingMode { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("resourceOptions", ArgumentSemantic.Assign)]
		MTLResourceOptions ResourceOptions { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (15, 0)]
		[Export ("type", ArgumentSemantic.Assign)]
		MTLHeapType Type { get; set; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("sparsePageSize", ArgumentSemantic.Assign)]
		MTLSparsePageSize SparsePageSize { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("maxCompatiblePlacementSparsePageSize", ArgumentSemantic.Assign)]
		MTLSparsePageSize MaxCompatiblePlacementSparsePageSize { get; set; }
	}

	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	interface MTLHeap : MTLAllocation {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("storageMode")]
		MTLStorageMode StorageMode { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("cpuCacheMode")]
		MTLCpuCacheMode CpuCacheMode { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("size")]
		nuint Size { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("usedSize")]
		nuint UsedSize { get; }

		/// <param name="alignment">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("maxAvailableSizeWithAlignment:")]
		nuint GetMaxAvailableSize (nuint alignment);

		/// <param name="length">To be added.</param>
		/// <param name="options">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newBufferWithLength:options:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBuffer CreateBuffer (nuint length, MTLResourceOptions options);

		/// <param name="desc">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newTextureWithDescriptor:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateTexture (MTLTextureDescriptor desc);

		/// <param name="state">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setPurgeableState:")]
		MTLPurgeableState SetPurgeableState (MTLPurgeableState state);

		/// <summary>Returns the current allcoated size of the heap.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("currentAllocatedSize")]
		nuint CurrentAllocatedSize { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("hazardTrackingMode")]
		MTLHazardTrackingMode HazardTrackingMode { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("resourceOptions")]
		MTLResourceOptions ResourceOptions { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("type")]
		MTLHeapType Type { get; }

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newBufferWithLength:options:offset:")]
		[return: NullAllowed]
		[return: Release]
		IMTLBuffer CreateBuffer (nuint length, MTLResourceOptions options, nuint offset);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("newTextureWithDescriptor:offset:")]
		[return: NullAllowed]
		[return: Release]
		IMTLTexture CreateTexture (MTLTextureDescriptor descriptor, nuint offset);

		[Abstract (GenerateExtensionMethod = true)]
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("newAccelerationStructureWithSize:")]
		[return: NullAllowed, Release]
		IMTLAccelerationStructure CreateAccelerationStructure (nuint size);

		[Abstract (GenerateExtensionMethod = true)]
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("newAccelerationStructureWithDescriptor:")]
		[return: NullAllowed, Release]
		IMTLAccelerationStructure CreateAccelerationStructure (MTLAccelerationStructureDescriptor descriptor);

		[Abstract (GenerateExtensionMethod = true)]
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("newAccelerationStructureWithSize:offset:")]
		[return: NullAllowed, Release]
		IMTLAccelerationStructure CreateAccelerationStructure (nuint size, nuint offset);

		[Abstract (GenerateExtensionMethod = true)]
		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Export ("newAccelerationStructureWithDescriptor:offset:")]
		[return: NullAllowed, Release]
		IMTLAccelerationStructure CreateAccelerationStructure (MTLAccelerationStructureDescriptor descriptor, nuint offset);
	}

	interface IMTLResource { }
	interface IMTLHeap { }
	/// <summary>System protocol for for allocated segments of GPU memory.</summary>
	/// <summary>Extension methods to the <see cref="Metal.IMTLResource" /> interface to support all the methods from the <see cref="Metal.IMTLResource" /> protocol.</summary>
	///     <remarks>
	///       <para>The extension methods for <see cref="Metal.IMTLResource" /> allow developers to treat instances of the interface as having all the optional methods of the original <see cref="Metal.IMTLResource" /> protocol.   Since the interface only contains the required members, these extension methods allow developers to call the optional members of the protocol.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	partial interface MTLResource : MTLAllocation {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("label")]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("cpuCacheMode")]
		MTLCpuCacheMode CpuCacheMode { get; }

		/// <summary>Returns a description of the location and permissions of the resource.</summary>
		[Abstract]
		[MacCatalyst (13, 1)]
		[Export ("storageMode")]
		MTLStorageMode StorageMode { get; }

		/// <param name="state">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract, Export ("setPurgeableState:")]
		MTLPurgeableState SetPurgeableState (MTLPurgeableState state);

		/// <summary>Returns the heap that sub-allocated the resource.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[NullAllowed, Export ("heap")]
		IMTLHeap Heap { get; }

		/// <summary>Makes the resource aliasable.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("makeAliasable")]
		void MakeAliasable ();

		/// <summary>Returns a Boolean value that tells whether future sub-allocations can alias the resource's memory.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("isAliasable")]
		bool IsAliasable { get; }

		[TV (17, 4), Mac (14, 4), iOS (17, 4), MacCatalyst (17, 4)]
		[Abstract]
		[Export ("setOwnerWithIdentity:")]
		int SetOwnerWithIdentity (uint taskIdToken);

		/// <summary>Returns the allocated size of the resource.</summary>
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("allocatedSize")]
		new nuint AllocatedSize { get; }

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("resourceOptions")]
		MTLResourceOptions ResourceOptions { get; }

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("heapOffset")]
		nuint HeapOffset { get; }

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("hazardTrackingMode")]
		MTLHazardTrackingMode HazardTrackingMode { get; }
	}

	/// <summary>Describes the compute state used during a compute operation pass. (See also <see cref="Metal.IMTLComputePipelineState" />.)</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Metal/Reference/MTLComputePipelineDescriptor_ClassReference/index.html">Apple documentation for <c>MTLComputePipelineDescriptor</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLComputePipelineDescriptor : NSCopying {

		[Export ("label")]
		[NullAllowed]
		string Label { get; set; }

		[Export ("computeFunction", ArgumentSemantic.Strong)]
		[NullAllowed]
		IMTLFunction ComputeFunction { get; set; }

		[Export ("threadGroupSizeIsMultipleOfThreadExecutionWidth")]
		bool ThreadGroupSizeIsMultipleOfThreadExecutionWidth { get; set; }

		[Export ("reset")]
		void Reset ();

		[MacCatalyst (13, 1)]
		[Export ("maxTotalThreadsPerThreadgroup")]
		nuint MaxTotalThreadsPerThreadgroup { get; set; }

		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("stageInputDescriptor", ArgumentSemantic.Copy)]
		MTLStageInputOutputDescriptor StageInputDescriptor { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("buffers")]
		MTLPipelineBufferDescriptorArray Buffers { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (14, 0)]
		[Export ("supportIndirectCommandBuffers")]
		bool SupportIndirectCommandBuffers { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use 'PreloadedLibraries' instead.")]
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use 'PreloadedLibraries' instead.")]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use 'PreloadedLibraries' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use 'PreloadedLibraries' instead.")]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("insertLibraries", ArgumentSemantic.Copy)]
		IMTLDynamicLibrary [] InsertLibraries { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("binaryArchives", ArgumentSemantic.Copy)]
		IMTLBinaryArchive [] BinaryArchives { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("linkedFunctions", ArgumentSemantic.Copy)]
		MTLLinkedFunctions LinkedFunctions { get; set; }

		[iOS (14, 0), TV (17, 0)]
		[MacCatalyst (14, 0)]
		[Export ("supportAddingBinaryFunctions")]
		bool SupportAddingBinaryFunctions { get; set; }

		[iOS (14, 0), TV (17, 0)]
		[MacCatalyst (14, 0)]
		[Export ("maxCallStackDepth")]
		nuint MaxCallStackDepth { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("preloadedLibraries", ArgumentSemantic.Copy)]
		IMTLDynamicLibrary [] PreloadedLibraries { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("shaderValidation")]
		MTLShaderValidation ShaderValidation { get; set; }

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Export ("requiredThreadsPerThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerThreadgroup { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLStageInputOutputDescriptor : NSCopying {
		[Static]
		[Export ("stageInputOutputDescriptor")]
		MTLStageInputOutputDescriptor Create ();

		[Export ("layouts")]
		MTLBufferLayoutDescriptorArray Layouts { get; }

		[Export ("attributes")]
		MTLAttributeDescriptorArray Attributes { get; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("indexBufferIndex")]
		nuint IndexBufferIndex { get; set; }

		[Export ("reset")]
		void Reset ();
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLType {
		[Export ("dataType")]
		MTLDataType DataType { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MTLType))]
	interface MTLPointerType {
		[Export ("elementType")]
		MTLDataType ElementType { get; }

		[Export ("access")]
		MTLArgumentAccess Access { get; }

		[Export ("alignment")]
		nuint Alignment { get; }

		[Export ("dataSize")]
		nuint DataSize { get; }

		[Export ("elementIsArgumentBuffer")]
		bool ElementIsArgumentBuffer { get; }

		[NullAllowed, Export ("elementStructType")]
		MTLStructType ElementStructType { get; }

		[NullAllowed, Export ("elementArrayType")]
		MTLArrayType ElementArrayType { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (MTLType))]
	interface MTLTextureReferenceType {
		[Export ("textureDataType")]
		MTLDataType TextureDataType { get; }

		[Export ("textureType")]
		MTLTextureType TextureType { get; }

		[Export ("access")]
		MTLArgumentAccess Access { get; }

		[Export ("isDepthTexture")]
		bool IsDepthTexture { get; }
	}

	[MacCatalyst (13, 1)]
	interface IMTLCaptureScope { }

	/// <summary>Custom capture scope boundary for debugging from Xcode.</summary>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface MTLCaptureScope {
		/// <summary>Begins capturing.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("beginScope")]
		void BeginScope ();

		/// <summary>Ends capturing.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("endScope")]
		void EndScope ();

		/// <summary>Gets or sets a descriptive label for the scope.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		/// <summary>Gets the on which the scope was created.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>Gets the command queue that created the scope.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("commandQueue")]
		IMTLCommandQueue CommandQueue { get; }

		[iOS (26, 0), TV (26, 0), MacCatalyst (26, 0), Mac (26, 0)]
		[Abstract]
		[NullAllowed, Export ("mtl4CommandQueue")]
		IMTL4CommandQueue Mtl4CommandQueue { get; }
	}


	/// <summary>Manages GPU captures for apps launched from Xcode.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLCaptureManager {
		[Static]
		[Export ("sharedCaptureManager")]
		MTLCaptureManager Shared { get; }

		[Export ("newCaptureScopeWithDevice:")]
		[return: Release]
		IMTLCaptureScope CreateNewCaptureScope (IMTLDevice device);

		[Export ("newCaptureScopeWithCommandQueue:")]
		[return: Release]
		IMTLCaptureScope CreateNewCaptureScope (IMTLCommandQueue commandQueue);

		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Export ("startCaptureWithDevice:")]
		void StartCapture (IMTLDevice device);

		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Export ("startCaptureWithCommandQueue:")]
		void StartCapture (IMTLCommandQueue commandQueue);

		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'StartCapture (MTLCaptureDescriptor, NSError)' instead.")]
		[Export ("startCaptureWithScope:")]
		void StartCapture (IMTLCaptureScope captureScope);

		[Export ("stopCapture")]
		void StopCapture ();

		[NullAllowed, Export ("defaultCaptureScope", ArgumentSemantic.Strong)]
		IMTLCaptureScope DefaultCaptureScope { get; set; }

		[Export ("isCapturing")]
		bool IsCapturing { get; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("supportsDestination:")]
		bool SupportsDestination (MTLCaptureDestination destination);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("startCaptureWithDescriptor:error:")]
		bool StartCapture (MTLCaptureDescriptor descriptor, [NullAllowed] out NSError error);

		[iOS (26, 0), TV (26, 0), Mac (26, 0), MacCatalyst (26, 0)]
		[Export ("newCaptureScopeWithMTL4CommandQueue:")]
		[return: Release]
		IMTLCaptureScope CreateNewCaptureScope (IMTL4CommandQueue commandQueue);

	}

	/// <summary>Contains a mutability description for a buffer.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLPipelineBufferDescriptor : NSCopying {
		[Export ("mutability", ArgumentSemantic.Assign)]
		MTLMutability Mutability { get; set; }
	}

	/// <summary>An array of buffer mutability descriptors.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLPipelineBufferDescriptorArray {
		[Internal]
		[Export ("objectAtIndexedSubscript:")]
		MTLPipelineBufferDescriptor GetObject (nuint bufferIndex);

		[Internal]
		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLPipelineBufferDescriptor buffer, nuint bufferIndex);
	}

	/// <summary>An description of an argument inside an argument buffer.</summary>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLArgumentDescriptor : NSCopying {
		[Static]
		[Export ("argumentDescriptor")]
		MTLArgumentDescriptor Create ();

		[Export ("dataType", ArgumentSemantic.Assign)]
		MTLDataType DataType { get; set; }

		[Export ("index")]
		nuint Index { get; set; }

		[Export ("arrayLength")]
		nuint ArrayLength { get; set; }

		[Export ("access", ArgumentSemantic.Assign)]
		MTLArgumentAccess Access { get; set; }

		[Export ("textureType", ArgumentSemantic.Assign)]
		MTLTextureType TextureType { get; set; }

		[Export ("constantBlockAlignment")]
		nuint ConstantBlockAlignment { get; set; }
	}

	interface IMTLArgumentEncoder { }

	/// <summary>Encodes data into argument buffers.</summary>
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MTLArgumentEncoder {
		/// <summary>Gets the device for the encoder.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>Gets or sets a descriptive label for the encoder.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		/// <summary>Gets the number of bytes that are required to store the encoded resources in the buffer.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("encodedLength")]
		nuint EncodedLength { get; }

		/// <summary>Gets the byte alignment for the encoded data.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("alignment")]
		nuint Alignment { get; }

		/// <param name="argumentBuffer">The destination buffer.</param>
		/// <param name="offset">The offset of the buffer, in bytes.</param>
		/// <summary>Sets the target buffer to which arguments will be encoded.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setArgumentBuffer:offset:")]
		void SetArgumentBuffer ([NullAllowed] IMTLBuffer argumentBuffer, nuint offset);

		/// <param name="argumentBuffer">The destination buffer.</param>
		/// <param name="startOffset">The offset of the buffer, in bytes.</param>
		/// <param name="arrayElement">The index, into the targeted buffer, of the argument.</param>
		/// <summary>Sets the target buffer to which arguments will be encoded.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setArgumentBuffer:startOffset:arrayElement:")]
		void SetArgumentBuffer ([NullAllowed] IMTLBuffer argumentBuffer, nuint startOffset, nuint arrayElement);

		/// <param name="buffer">A buffer in an argument buffer.</param>
		/// <param name="offset">The byte offset of <paramref name="buffer" />.</param>
		/// <param name="index">The index of the nested buffer. Either a Metal index ID or the index member of a <see cref="Metal.MTLArgumentDescriptor" />.</param>
		/// <summary>Encodes <paramref name="buffer" /> to the argument buffer.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setBuffer:offset:atIndex:")]
		void SetBuffer ([NullAllowed] IMTLBuffer buffer, nuint offset, nuint index);

		[Abstract]
		[Export ("setBuffers:offsets:withRange:")]
		void SetBuffers (IntPtr buffers, IntPtr offsets, NSRange range);

		/// <param name="texture">A texture within an argument buffer.</param>
		/// <param name="index">The index of the texture. Either a Metal index ID or the index member of a <see cref="Metal.MTLArgumentDescriptor" />.</param>
		/// <summary>Encodes the provided <paramref name="texture" /> into the argument buffer.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setTexture:atIndex:")]
		void SetTexture ([NullAllowed] IMTLTexture texture, nuint index);

		/// <param name="textures">An array of textures from which to select the textures to encode.</param>
		/// <param name="range">Indices into <paramref name="textures" />. Either Metal index IDs or the index members of <see cref="Metal.MTLArgumentDescriptor" />s.</param>
		/// <summary>Encodes the provided <paramref name="textures" /> into the argument buffer.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setTextures:withRange:")]
		void SetTextures (IMTLTexture [] textures, NSRange range);

		/// <param name="sampler">A sampler within an argument buffer.</param>
		/// <param name="index">The sampler index. Either a Metal index ID or the index member of a <see cref="Metal.MTLArgumentDescriptor" />.</param>
		/// <summary>Encodes <paramref name="sampler" /> into the argument buffer.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setSamplerState:atIndex:")]
		void SetSamplerState ([NullAllowed] IMTLSamplerState sampler, nuint index);

		/// <param name="samplers">An array of samplers from which to select the samplers to encode.</param>
		/// <param name="range">Indices into <paramref name="samplers" />. Either Metal index IDs or the index members of <see cref="Metal.MTLArgumentDescriptor" />s.</param>
		/// <summary>Encodes the provided <paramref name="samplers" /> into the argument buffer.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setSamplerStates:withRange:")]
		void SetSamplerStates (IMTLSamplerState [] samplers, NSRange range);

		/// <param name="index">The index for the constant. Either a Metal index ID or the index member of a <see cref="Metal.MTLArgumentDescriptor" />.</param>
		/// <summary>Returns a pointer to the constant at the specified <paramref name="index" /> into the buffer.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("constantDataAtIndex:")]
		IntPtr GetConstantData (nuint index);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setRenderPipelineState:atIndex:")]
		void SetRenderPipelineState ([NullAllowed] IMTLRenderPipelineState pipeline, nuint index);

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setRenderPipelineStates:withRange:")]
		void SetRenderPipelineStates (IMTLRenderPipelineState [] pipelines, NSRange range);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setIndirectCommandBuffer:atIndex:")]
		void SetIndirectCommandBuffer ([NullAllowed] IMTLIndirectCommandBuffer indirectCommandBuffer, nuint index);

		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setIndirectCommandBuffers:withRange:")]
		void SetIndirectCommandBuffers (IMTLIndirectCommandBuffer [] buffers, NSRange range);

		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newArgumentEncoderForBufferAtIndex:")]
		[return: NullAllowed]
		[return: Release]
		IMTLArgumentEncoder CreateArgumentEncoder (nuint index);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setComputePipelineState:atIndex:")]
		void SetComputePipelineState ([NullAllowed] IMTLComputePipelineState pipeline, nuint index);

		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Abstract]
		[Export ("setComputePipelineStates:withRange:")]
		void SetComputePipelineStates (IMTLComputePipelineState [] pipelines, NSRange range);

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setAccelerationStructure:atIndex:")]
		void SetAccelerationStructure ([NullAllowed] IMTLAccelerationStructure accelerationStructure, nuint index);

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVisibleFunctionTable:atIndex:")]
		void SetVisibleFunctionTable ([NullAllowed] IMTLVisibleFunctionTable visibleFunctionTable, nuint index);

		[iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVisibleFunctionTables:withRange:")]
		void SetVisibleFunctionTables (IMTLVisibleFunctionTable [] visibleFunctionTables, NSRange range);

		[Abstract (GenerateExtensionMethod = true)]
		[Mac (11, 0), iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("setIntersectionFunctionTable:atIndex:")]
		void SetIntersectionFunctionTable ([NullAllowed] IMTLIntersectionFunctionTable intersectionFunctionTable, nuint index);

		[Abstract (GenerateExtensionMethod = true)]
		[Mac (11, 0), iOS (14, 0), TV (16, 0), MacCatalyst (14, 0)]
		[Export ("setIntersectionFunctionTables:withRange:")]
		void SetIntersectionFunctionTables (IMTLIntersectionFunctionTable [] intersectionFunctionTables, NSRange range);

	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (14, 5)]
	[BaseType (typeof (NSObject))]
	interface MTLTileRenderPipelineColorAttachmentDescriptor : NSCopying {
		[Export ("pixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat PixelFormat { get; set; }
	}

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (14, 5)]
	[BaseType (typeof (NSObject))]
	interface MTLTileRenderPipelineColorAttachmentDescriptorArray {
		[Internal]
		[Export ("objectAtIndexedSubscript:")]
		MTLTileRenderPipelineColorAttachmentDescriptor GetObject (nuint attachmentIndex);

		[Internal]
		[Export ("setObject:atIndexedSubscript:")]
		void SetObject (MTLTileRenderPipelineColorAttachmentDescriptor attachment, nuint attachmentIndex);
	}

	interface IMTLBinaryArchive { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLBinaryArchive {

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[Export ("addComputePipelineFunctionsWithDescriptor:error:")]
		bool AddComputePipelineFunctions (MTLComputePipelineDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("addRenderPipelineFunctionsWithDescriptor:error:")]
		bool AddRenderPipelineFunctions (MTLRenderPipelineDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[TV (14, 5)]
		[MacCatalyst (14, 0)]
		[Export ("addTileRenderPipelineFunctionsWithDescriptor:error:")]
		bool AddTileRenderPipelineFunctions (MTLTileRenderPipelineDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("serializeToURL:error:")]
		bool Serialize (NSUrl url, [NullAllowed] out NSError error);

		[Abstract]
		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("addFunctionWithDescriptor:library:error:")]
		bool AddFunctionWithDescriptor (MTLFunctionDescriptor descriptor, IMTLLibrary library, [NullAllowed] out NSError error);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("addMeshRenderPipelineFunctionsWithDescriptor:error:")]
		bool AddMeshRenderPipelineFunctions (MTLMeshRenderPipelineDescriptor descriptor, out NSError error);

		[Abstract]
		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("addLibraryWithDescriptor:error:")]
		bool AddLibrary (MTLStitchedLibraryDescriptor descriptor, out NSError error);
	}


	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[TV (14, 5)]
	[BaseType (typeof (NSObject))]
	interface MTLTileRenderPipelineDescriptor : NSCopying {
		[NullAllowed]
		[Export ("label")]
		string Label { get; set; }

		[Export ("tileFunction", ArgumentSemantic.Strong)]
		IMTLFunction TileFunction { get; set; }

		[Export ("rasterSampleCount")]
		nuint RasterSampleCount { get; set; }

		[Export ("colorAttachments")]
		MTLTileRenderPipelineColorAttachmentDescriptorArray ColorAttachments { get; }

		[Export ("threadgroupSizeMatchesTileSize")]
		bool ThreadgroupSizeMatchesTileSize { get; set; }

		[Export ("tileBuffers")]
		MTLPipelineBufferDescriptorArray TileBuffers { get; }

		[MacCatalyst (14, 0)]
		[Export ("maxTotalThreadsPerThreadgroup")]
		nuint MaxTotalThreadsPerThreadgroup { get; set; }

		[Export ("reset")]
		void Reset ();

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[NullAllowed, Export ("binaryArchives", ArgumentSemantic.Copy)]
		IMTLBinaryArchive [] BinaryArchives { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0), TV (17, 0)]
		[Export ("supportAddingBinaryFunctions")]
		bool SupportAddingBinaryFunctions { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("preloadedLibraries", ArgumentSemantic.Copy)]
		IMTLDynamicLibrary [] PreloadedLibraries { get; set; }

		[iOS (14, 0), MacCatalyst (15, 0), TV (17, 0)]
		[Export ("maxCallStackDepth")]
		nuint MaxCallStackDepth { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("linkedFunctions", ArgumentSemantic.Copy)]
		MTLLinkedFunctions LinkedFunctions { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("shaderValidation")]
		MTLShaderValidation ShaderValidation { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("requiredThreadsPerThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerThreadgroup { get; set; }
	}

	interface IMTLEvent { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MTLEvent {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("device")]
		IMTLDevice Device { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DesignatedDefaultCtor]
	interface MTLSharedEventListener {
		[Export ("initWithDispatchQueue:")]
		[DesignatedInitializer]
		NativeHandle Constructor (DispatchQueue dispatchQueue);

		[Export ("dispatchQueue")]
		DispatchQueue DispatchQueue { get; }

		[Mac (26, 0), iOS (26, 0), MacCatalyst (26, 0), TV (26, 0)]
		[Static]
		[Export ("sharedListener")]
		MTLSharedEventListener SharedListener { get; }
	}

	delegate void MTLSharedEventNotificationBlock (IMTLSharedEvent @event, ulong value);

	interface IMTLSharedEvent { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MTLSharedEvent : MTLEvent {
		/// <param name="listener">To be added.</param>
		/// <param name="atValue">To be added.</param>
		/// <param name="block">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("notifyListener:atValue:block:")]
		void NotifyListener (MTLSharedEventListener listener, ulong atValue, MTLSharedEventNotificationBlock block);

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("newSharedEventHandle")]
		[return: Release]
		MTLSharedEventHandle CreateSharedEventHandle ();

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("signaledValue")]
		ulong SignaledValue { get; set; }

		[Mac (14, 4), iOS (17, 4), TV (17, 4), MacCatalyst (17, 4)]
		[Abstract]
		[Export ("waitUntilSignaledValue:timeoutMS:")]
		bool WaitUntilSignaledValue (ulong value, ulong milliseconds);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLSharedEventHandle : NSSecureCoding {
		[NullAllowed, Export ("label")]
		string Label { get; }
	}

	interface IMTLIndirectRenderCommand { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MTLIndirectRenderCommand {

		/// <param name="pipelineState">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("setRenderPipelineState:")]
		void SetRenderPipelineState (IMTLRenderPipelineState pipelineState);

		/// <param name="buffer">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setVertexBuffer:offset:atIndex:")]
		void SetVertexBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		/// <param name="buffer">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="index">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("setFragmentBuffer:offset:atIndex:")]
		void SetFragmentBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		/// <param name="numberOfPatchControlPoints">To be added.</param>
		/// <param name="patchStart">To be added.</param>
		/// <param name="patchCount">To be added.</param>
		/// <param name="patchIndexBuffer">To be added.</param>
		/// <param name="patchIndexBufferOffset">To be added.</param>
		/// <param name="instanceCount">To be added.</param>
		/// <param name="baseInstance">To be added.</param>
		/// <param name="buffer">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="instanceStride">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("drawPatches:patchStart:patchCount:patchIndexBuffer:patchIndexBufferOffset:instanceCount:baseInstance:tessellationFactorBuffer:tessellationFactorBufferOffset:tessellationFactorBufferInstanceStride:")]
		void DrawPatches (nuint numberOfPatchControlPoints, nuint patchStart, nuint patchCount, [NullAllowed] IMTLBuffer patchIndexBuffer, nuint patchIndexBufferOffset, nuint instanceCount, nuint baseInstance, IMTLBuffer buffer, nuint offset, nuint instanceStride);

		/// <param name="numberOfPatchControlPoints">To be added.</param>
		/// <param name="patchStart">To be added.</param>
		/// <param name="patchCount">To be added.</param>
		/// <param name="patchIndexBuffer">To be added.</param>
		/// <param name="patchIndexBufferOffset">To be added.</param>
		/// <param name="controlPointIndexBuffer">To be added.</param>
		/// <param name="controlPointIndexBufferOffset">To be added.</param>
		/// <param name="instanceCount">To be added.</param>
		/// <param name="baseInstance">To be added.</param>
		/// <param name="buffer">To be added.</param>
		/// <param name="offset">To be added.</param>
		/// <param name="instanceStride">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[TV (14, 5)]
		[MacCatalyst (13, 1)]
		[Export ("drawIndexedPatches:patchStart:patchCount:patchIndexBuffer:patchIndexBufferOffset:controlPointIndexBuffer:controlPointIndexBufferOffset:instanceCount:baseInstance:tessellationFactorBuffer:tessellationFactorBufferOffset:tessellationFactorBufferInstanceStride:")]
		void DrawIndexedPatches (nuint numberOfPatchControlPoints, nuint patchStart, nuint patchCount, [NullAllowed] IMTLBuffer patchIndexBuffer, nuint patchIndexBufferOffset, IMTLBuffer controlPointIndexBuffer, nuint controlPointIndexBufferOffset, nuint instanceCount, nuint baseInstance, IMTLBuffer buffer, nuint offset, nuint instanceStride);

		/// <param name="primitiveType">To be added.</param>
		/// <param name="vertexStart">To be added.</param>
		/// <param name="vertexCount">To be added.</param>
		/// <param name="instanceCount">To be added.</param>
		/// <param name="baseInstance">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("drawPrimitives:vertexStart:vertexCount:instanceCount:baseInstance:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, nuint vertexStart, nuint vertexCount, nuint instanceCount, nuint baseInstance);

		/// <param name="primitiveType">To be added.</param>
		/// <param name="indexCount">To be added.</param>
		/// <param name="indexType">To be added.</param>
		/// <param name="indexBuffer">To be added.</param>
		/// <param name="indexBufferOffset">To be added.</param>
		/// <param name="instanceCount">To be added.</param>
		/// <param name="baseVertex">To be added.</param>
		/// <param name="baseInstance">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("drawIndexedPrimitives:indexCount:indexType:indexBuffer:indexBufferOffset:instanceCount:baseVertex:baseInstance:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, nuint indexCount, MTLIndexType indexType, IMTLBuffer indexBuffer, nuint indexBufferOffset, nuint instanceCount, nint baseVertex, nuint baseInstance);

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("reset")]
		void Reset ();

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setVertexBuffer:offset:attributeStride:atIndex:")]
		void SetVertexBuffer (IMTLBuffer buffer, nuint offset, nuint stride, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (18, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectThreadgroupMemoryLength:atIndex:")]
		void SetObjectThreadgroupMemoryLength (nuint length, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (18, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setObjectBuffer:offset:atIndex:")]
		void SetObjectBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (18, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setMeshBuffer:offset:atIndex:")]
		void SetMeshBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		[Mac (14, 0), iOS (17, 0), TV (18, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("drawMeshThreadgroups:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreadgroups (MTLSize threadgroupsPerGrid, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Mac (14, 0), iOS (17, 0), TV (18, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("drawMeshThreads:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreads (MTLSize threadsPerGrid, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Mac (14, 0), iOS (17, 0), TV (18, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setBarrier")]
		void SetBarrier ();

		[Mac (14, 0), iOS (17, 0), TV (18, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("clearBarrier")]
		void ClearBarrier ();
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLIndirectCommandBufferDescriptor : NSCopying {
		[Export ("commandTypes", ArgumentSemantic.Assign)]
		MTLIndirectCommandType CommandTypes { get; set; }

		[iOS (13, 0), TV (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("inheritPipelineState")]
		bool InheritPipelineState { get; set; }

		[Export ("inheritBuffers")]
		bool InheritBuffers { get; set; }

		[Export ("maxVertexBufferBindCount")]
		nuint MaxVertexBufferBindCount { get; set; }

		[Export ("maxFragmentBufferBindCount")]
		nuint MaxFragmentBufferBindCount { get; set; }

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("maxKernelBufferBindCount")]
		nuint MaxKernelBufferBindCount { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Export ("maxKernelThreadgroupMemoryBindCount")]
		nuint MaxKernelThreadgroupMemoryBindCount { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Export ("maxObjectBufferBindCount")]
		nuint MaxObjectBufferBindCount { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Export ("maxMeshBufferBindCount")]
		nuint MaxMeshBufferBindCount { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Export ("maxObjectThreadgroupMemoryBindCount")]
		nuint MaxObjectThreadgroupMemoryBindCount { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Export ("supportDynamicAttributeStride")]
		bool SupportDynamicAttributeStride { get; set; }

		[Mac (13, 0), iOS (16, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Export ("supportRayTracing")]
		bool SupportRayTracing { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("supportColorAttachmentMapping")]
		bool SupportColorAttachmentMapping { get; set; }
	}

	interface IMTLIndirectCommandBuffer { }

	[MacCatalyst (13, 1)]
	[Protocol]
	interface MTLIndirectCommandBuffer : MTLResource {
		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("size")]
		nuint Size { get; }

		/// <param name="range">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("resetWithRange:")]
		void Reset (NSRange range);

		/// <param name="commandIndex">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("indirectRenderCommandAtIndex:")]
		IMTLIndirectRenderCommand GetCommand (nuint commandIndex);

		[Abstract]
		[TV (13, 0), iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("indirectComputeCommandAtIndex:")]
		IMTLIndirectComputeCommand GetIndirectComputeCommand (nuint commandIndex);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceID { get; }
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLSharedTextureHandle : NSSecureCoding {
		[Export ("device")]
		IMTLDevice Device { get; }

		[NullAllowed, Export ("label")]
		string Label { get; }
	}

	[Introduced (PlatformName.MacCatalyst, 13, 4)]
	[TV (16, 0), iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLRasterizationRateSampleArray {
		[Export ("objectAtIndexedSubscript:")]
		NSNumber GetObject (nuint index);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject (NSNumber value, nuint index);
	}

	[Introduced (PlatformName.MacCatalyst, 13, 4)]
	[TV (16, 0), iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLRasterizationRateMapDescriptor : NSCopying {
		[Static]
		[Export ("rasterizationRateMapDescriptorWithScreenSize:")]
		MTLRasterizationRateMapDescriptor Create (MTLSize screenSize);

		[Static]
		[Export ("rasterizationRateMapDescriptorWithScreenSize:layer:")]
		MTLRasterizationRateMapDescriptor Create (MTLSize screenSize, MTLRasterizationRateLayerDescriptor layer);

		[Static]
		[Export ("rasterizationRateMapDescriptorWithScreenSize:layerCount:layers:")]
		MTLRasterizationRateMapDescriptor Create (MTLSize screenSize, nuint layerCount, out MTLRasterizationRateLayerDescriptor layers);

		[Export ("layerAtIndex:")]
		[return: NullAllowed]
		MTLRasterizationRateLayerDescriptor GetLayer (nuint layerIndex);

		[Export ("setLayer:atIndex:")]
		void SetLayer ([NullAllowed] MTLRasterizationRateLayerDescriptor layer, nuint layerIndex);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("layers")]
		MTLRasterizationRateLayerArray Layers { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("screenSize", ArgumentSemantic.Assign)]
		MTLSize ScreenSize { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("layerCount")]
		nuint LayerCount { get; }
	}

	[Introduced (PlatformName.MacCatalyst, 13, 4)]
	[TV (16, 0), iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLRasterizationRateLayerDescriptor : NSCopying {

		[Export ("initWithSampleCount:")]
		[DesignatedInitializer]
		NativeHandle Constructor (MTLSize sampleCount);

		[Internal]
		[Export ("initWithSampleCount:horizontal:vertical:")]
		NativeHandle Constructor (MTLSize sampleCount, IntPtr horizontal, IntPtr vertical);

		[MacCatalyst (15, 0)]
		[Internal]
		[Export ("horizontalSampleStorage")]
		IntPtr _HorizontalSampleStorage { get; }

		[MacCatalyst (15, 0)]
		[Internal]
		[Export ("verticalSampleStorage")]
		IntPtr _VerticalSampleStorage { get; }

		[MacCatalyst (15, 0)]
		[Export ("horizontal")]
		MTLRasterizationRateSampleArray Horizontal { get; }

		[MacCatalyst (15, 0)]
		[Export ("vertical")]
		MTLRasterizationRateSampleArray Vertical { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("maxSampleCount")]
		MTLSize MaxSampleCount { get; }

		[Export ("sampleCount", ArgumentSemantic.Assign)]
		MTLSize SampleCount { get; set; }
	}

	[Introduced (PlatformName.MacCatalyst, 13, 4)]
	[TV (16, 0), iOS (13, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLRasterizationRateLayerArray {
		[Export ("objectAtIndexedSubscript:")]
		[return: NullAllowed]
		MTLRasterizationRateLayerDescriptor GetObject (nuint layerIndex);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLRasterizationRateLayerDescriptor layer, nuint layerIndex);
	}

	interface IMTLRasterizationRateMap { }

	[Introduced (PlatformName.MacCatalyst, 13, 4)]
	[TV (16, 0), iOS (13, 0)]
	[Protocol]
	interface MTLRasterizationRateMap {
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }

		[Abstract]
		[Export ("screenSize")]
		MTLSize ScreenSize { get; }

		[Abstract]
		[Export ("physicalGranularity")]
		MTLSize PhysicalGranularity { get; }

		[Abstract]
		[Export ("layerCount")]
		nuint LayerCount { get; }

		[Abstract]
		[Export ("parameterBufferSizeAndAlign")]
		MTLSizeAndAlign ParameterBufferSizeAndAlign { get; }

		[Abstract]
		[Export ("copyParameterDataToBuffer:offset:")]
		void CopyParameterData (IMTLBuffer buffer, nuint offset);

		[Abstract]
		[Export ("physicalSizeForLayer:")]
		MTLSize GetPhysicalSize (nuint layerIndex);

		[Abstract]
		[Export ("mapScreenToPhysicalCoordinates:forLayer:")]
		MTLCoordinate2D MapScreenToPhysicalCoordinates (MTLCoordinate2D screenCoordinates, nuint layerIndex);

		[Abstract]
		[Export ("mapPhysicalToScreenCoordinates:forLayer:")]
		MTLCoordinate2D MapPhysicalToScreenCoordinates (MTLCoordinate2D physicalCoordinates, nuint layerIndex);
	}

	interface IMTLResourceStateCommandEncoder { }

	[Introduced (PlatformName.MacCatalyst, 14, 0)]
	[iOS (13, 0), TV (16, 0)]
	[Protocol]
	interface MTLResourceStateCommandEncoder : MTLCommandEncoder {
#if !MONOMAC && !__MACCATALYST__
		[Abstract]
#endif
		[Export ("updateTextureMappings:mode:regions:mipLevels:slices:numRegions:")]
		void Update (IMTLTexture texture, MTLSparseTextureMappingMode mode, IntPtr regions, IntPtr mipLevels, IntPtr slices, nuint numRegions);

#if !MONOMAC && !__MACCATALYST__
		[Abstract]
#endif
		[Export ("updateTextureMapping:mode:region:mipLevel:slice:")]
		void Update (IMTLTexture texture, MTLSparseTextureMappingMode mode, MTLRegion region, nuint mipLevel, nuint slice);

#if !MONOMAC && !__MACCATALYST__
		[Abstract]
#endif
		[Export ("updateTextureMapping:mode:indirectBuffer:indirectBufferOffset:")]
		void Update (IMTLTexture texture, MTLSparseTextureMappingMode mode, IMTLBuffer indirectBuffer, nuint indirectBufferOffset);

#if !MONOMAC && !__MACCATALYST__
		[Abstract]
#endif
		[Export ("updateFence:")]
		void Update (IMTLFence fence);

#if !MONOMAC && !__MACCATALYST__
		[Abstract]
#endif
		[Export ("waitForFence:")]
		void Wait (IMTLFence fence);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		// @optional in macOS and Mac Catalyst
#if !__MACOS__ && !__MACCATALYST__
		[Abstract (GenerateExtensionMethod = true)]
#endif
		[Export ("moveTextureMappingsFromTexture:sourceSlice:sourceLevel:sourceOrigin:sourceSize:toTexture:destinationSlice:destinationLevel:destinationOrigin:")]
		void MoveTextureMappings (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, MTLOrigin sourceOrigin, MTLSize sourceSize, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, MTLOrigin destinationOrigin);
	}

	[iOS (13, 0), TV (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface MTLCaptureDescriptor : NSCopying {
		[NullAllowed, Export ("captureObject", ArgumentSemantic.Strong)]
		NSObject CaptureObject { get; set; }

		[Export ("destination", ArgumentSemantic.Assign)]
		MTLCaptureDestination Destination { get; set; }

		[NullAllowed, Export ("outputURL", ArgumentSemantic.Copy)]
		NSUrl OutputUrl { get; set; }
	}

	interface IMTLIndirectComputeCommand { }

	[TV (13, 0), iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface MTLIndirectComputeCommand {
		[Abstract]
		[Export ("setComputePipelineState:")]
		void SetComputePipelineState (IMTLComputePipelineState pipelineState);

		[Abstract]
		[Export ("setKernelBuffer:offset:atIndex:")]
		void SetKernelBuffer (IMTLBuffer buffer, nuint offset, nuint index);

		[Abstract]
		[Export ("concurrentDispatchThreadgroups:threadsPerThreadgroup:")]
		void ConcurrentDispatchThreadgroups (MTLSize threadgroupsPerGrid, MTLSize threadsPerThreadgroup);

		[Abstract]
		[Export ("concurrentDispatchThreads:threadsPerThreadgroup:")]
		void ConcurrentDispatchThreads (MTLSize threadsPerGrid, MTLSize threadsPerThreadgroup);

		[Abstract]
		[Export ("setBarrier")]
		void SetBarrier ();

		[Abstract]
		[Export ("clearBarrier")]
		void ClearBarrier ();

		[Abstract]
		[Export ("reset")]
		void Reset ();

		[Abstract]
		[Export ("setThreadgroupMemoryLength:atIndex:")]
		void SetThreadgroupMemoryLength (nuint length, nuint index);

		[Abstract]
		[Export ("setStageInRegion:")]
		void SetStageInRegion (MTLRegion region);

		[iOS (14, 0), TV (14, 0)]
		[MacCatalyst (14, 0)]
		[Abstract]
		[Export ("setImageblockWidth:height:")]
		void SetImageblock (nuint width, nuint height);

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setKernelBuffer:offset:attributeStride:atIndex:")]
		void SetKernelBuffer (IMTLBuffer buffer, nuint offset, nuint stride, nuint index);
	}

	interface IMTLCounter { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLCounter {
		[Abstract]
		[Export ("name")]
		string Name { get; }
	}

	interface IMTLCounterSet { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLCounterSet {
		[Abstract]
		[Export ("name")]
		string Name { get; }

		[Abstract]
		[Export ("counters", ArgumentSemantic.Copy)]
		IMTLCounter [] Counters { get; }
	}

	interface IMTLCounterSampleBuffer { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLCounterSampleBuffer {
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[Export ("label")]
		string Label { get; }

		[Abstract]
		[Export ("sampleCount")]
		nuint SampleCount { get; }

		[Abstract]
		[Export ("resolveCounterRange:")]
		[return: NullAllowed]
		NSData ResolveCounterRange (NSRange range);
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLCounterSampleBufferDescriptor : NSCopying {
		[NullAllowed]
		[Export ("counterSet", ArgumentSemantic.Retain)]
		IMTLCounterSet CounterSet { get; set; }

		[Export ("label")]
		string Label { get; set; }

		[Export ("storageMode", ArgumentSemantic.Assign)]
		MTLStorageMode StorageMode { get; set; }

		[Export ("sampleCount")]
		nuint SampleCount { get; set; }
	}

	[iOS (14, 0), NoTV]
	[MacCatalyst (14, 0)]
	interface IMTLAccelerationStructure { }

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLAccelerationStructure : MTLResource {
		[Abstract]
		[Export ("size")]
		nuint Size { get; }

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (MTLAccelerationStructureGeometryDescriptor))]
	interface MTLAccelerationStructureBoundingBoxGeometryDescriptor {
		[NullAllowed, Export ("boundingBoxBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer BoundingBoxBuffer { get; set; }

		[Export ("boundingBoxBufferOffset")]
		nuint BoundingBoxBufferOffset { get; set; }

		[Export ("boundingBoxStride")]
		nuint BoundingBoxStride { get; set; }

		[Export ("boundingBoxCount")]
		nuint BoundingBoxCount { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLAccelerationStructureBoundingBoxGeometryDescriptor Create ();
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLAccelerationStructureDescriptor : NSCopying {
		[Mac (14, 0), iOS (17, 0), MacCatalyst (17, 0), TV (17, 0)]
		[Export ("usage", ArgumentSemantic.Assign)]
		MTLAccelerationStructureUsage Usage { get; set; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLAccelerationStructureGeometryDescriptor : NSCopying {
		[Export ("intersectionFunctionTableOffset")]
		nuint IntersectionFunctionTableOffset { get; set; }

		[Export ("opaque")]
		bool Opaque { get; set; }

		[Export ("allowDuplicateIntersectionFunctionInvocation")]
		bool AllowDuplicateIntersectionFunctionInvocation { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[NullAllowed, Export ("primitiveDataBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer PrimitiveDataBuffer { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("primitiveDataBufferOffset")]
		nuint PrimitiveDataBufferOffset { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("primitiveDataStride")]
		nuint PrimitiveDataStride { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("primitiveDataElementSize")]
		nuint PrimitiveDataElementSize { get; set; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (MTLAccelerationStructureGeometryDescriptor))]
	interface MTLAccelerationStructureTriangleGeometryDescriptor {
		[NullAllowed, Export ("vertexBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer VertexBuffer { get; set; }

		[Export ("vertexBufferOffset")]
		nuint VertexBufferOffset { get; set; }

		[Export ("vertexStride")]
		nuint VertexStride { get; set; }

		[NullAllowed, Export ("indexBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer IndexBuffer { get; set; }

		[Export ("indexBufferOffset")]
		nuint IndexBufferOffset { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("triangleCount")]
		nuint TriangleCount { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLAccelerationStructureTriangleGeometryDescriptor Create ();

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("vertexFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat VertexFormat { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[NullAllowed, Export ("transformationMatrixBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer TransformationMatrixBuffer { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), TV (16, 0)]
		[Export ("transformationMatrixBufferOffset")]
		nuint TransformationMatrixBufferOffset { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("transformationMatrixLayout")]
		MTLMatrixLayout TransformationMatrixLayout { get; set; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLBinaryArchiveDescriptor : NSCopying {
		[NullAllowed, Export ("url", ArgumentSemantic.Copy)]
		NSUrl Url { get; set; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLBlitPassDescriptor : NSCopying {
		[Static]
		[Export ("blitPassDescriptor")]
		MTLBlitPassDescriptor Create ();

		[Export ("sampleBufferAttachments")]
		MTLBlitPassSampleBufferAttachmentDescriptorArray SampleBufferAttachments { get; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLBlitPassSampleBufferAttachmentDescriptor : NSCopying {
		[NullAllowed, Export ("sampleBuffer", ArgumentSemantic.Retain)]
		IMTLCounterSampleBuffer SampleBuffer { get; set; }

		[Export ("startOfEncoderSampleIndex")]
		nuint StartOfEncoderSampleIndex { get; set; }

		[Export ("endOfEncoderSampleIndex")]
		nuint EndOfEncoderSampleIndex { get; set; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLBlitPassSampleBufferAttachmentDescriptorArray {
		[Export ("objectAtIndexedSubscript:")]
		MTLBlitPassSampleBufferAttachmentDescriptor GetObject (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLBlitPassSampleBufferAttachmentDescriptor attachment, nuint attachmentIndex);
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLCommandBufferDescriptor : NSCopying {

		[Field ("MTLCommandBufferEncoderInfoErrorKey")]
		NSString BufferEncoderInfoErrorKey { get; }

		[Export ("retainedReferences")]
		bool RetainedReferences { get; set; }

		[Export ("errorOptions", ArgumentSemantic.Assign)]
		MTLCommandBufferErrorOption ErrorOptions { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("logState", ArgumentSemantic.Retain), NullAllowed]
		IMTLLogState LogState { get; set; }

	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLComputePassDescriptor : NSCopying {
		[Static]
		[Export ("computePassDescriptor")]
		MTLComputePassDescriptor Create ();

		[Export ("dispatchType", ArgumentSemantic.Assign)]
		MTLDispatchType DispatchType { get; set; }

		[Export ("sampleBufferAttachments")]
		MTLComputePassSampleBufferAttachmentDescriptorArray SampleBufferAttachments { get; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLComputePassSampleBufferAttachmentDescriptor : NSCopying {

		[NullAllowed, Export ("sampleBuffer", ArgumentSemantic.Retain)]
		IMTLCounterSampleBuffer SampleBuffer { get; set; }

		[Export ("startOfEncoderSampleIndex")]
		nuint StartOfEncoderSampleIndex { get; set; }

		[Export ("endOfEncoderSampleIndex")]
		nuint EndOfEncoderSampleIndex { get; set; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLComputePassSampleBufferAttachmentDescriptorArray {
		[Export ("objectAtIndexedSubscript:")]
		MTLComputePassSampleBufferAttachmentDescriptor GetObject (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLComputePassSampleBufferAttachmentDescriptor attachment, nuint attachmentIndex);
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface MTLFunctionDescriptor : NSCopying {
		[Static]
		[Export ("functionDescriptor")]
		MTLFunctionDescriptor Create ();

		[NullAllowed, Export ("name")]
		string Name { get; set; }

		[NullAllowed, Export ("specializedName")]
		string SpecializedName { get; set; }

		[NullAllowed, Export ("constantValues", ArgumentSemantic.Copy)]
		MTLFunctionConstantValues ConstantValues { get; set; }

		[Export ("options", ArgumentSemantic.Assign)]
		MTLFunctionOptions Options { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("binaryArchives", ArgumentSemantic.Copy)]
		IMTLBinaryArchive [] BinaryArchives { get; set; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (MTLAccelerationStructureDescriptor))]
	interface MTLInstanceAccelerationStructureDescriptor {
		[NullAllowed, Export ("instanceDescriptorBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer InstanceDescriptorBuffer { get; set; }

		[Export ("instanceDescriptorBufferOffset")]
		nuint InstanceDescriptorBufferOffset { get; set; }

		[Export ("instanceDescriptorStride")]
		nuint InstanceDescriptorStride { get; set; }

		[Export ("instanceCount")]
		nuint InstanceCount { get; set; }

		[NullAllowed, Export ("instancedAccelerationStructures", ArgumentSemantic.Retain)]
		IMTLAccelerationStructure [] InstancedAccelerationStructures { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLInstanceAccelerationStructureDescriptor Create ();

		[iOS (15, 0), MacCatalyst (15, 0), TV (17, 0)]
		[Export ("instanceDescriptorType", ArgumentSemantic.Assign)]
		MTLAccelerationStructureInstanceDescriptorType InstanceDescriptorType { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("motionTransformBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer MotionTransformBuffer { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("motionTransformBufferOffset")]
		nuint MotionTransformBufferOffset { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("motionTransformCount")]
		nuint MotionTransformCount { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("instanceTransformationMatrixLayout")]
		MTLMatrixLayout InstanceTransformationMatrixLayout { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("motionTransformType")]
		MTLTransformType MotionTransformType { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("motionTransformStride")]
		nuint MotionTransformStride { get; set; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (MTLFunctionDescriptor))]
	interface MTLIntersectionFunctionDescriptor : NSCopying { }

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface MTLIntersectionFunctionTableDescriptor : NSCopying {
		[Static]
		[Export ("intersectionFunctionTableDescriptor")]
		MTLIntersectionFunctionTableDescriptor Create ();

		[Export ("functionCount")]
		nuint FunctionCount { get; set; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface MTLLinkedFunctions : NSCopying {
		[Static]
		[Export ("linkedFunctions")]
		MTLLinkedFunctions Create ();

		[NullAllowed, Export ("functions", ArgumentSemantic.Copy)]
		IMTLFunction [] Functions { get; set; }

		[TV (17, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("binaryFunctions", ArgumentSemantic.Copy)]
		IMTLFunction [] BinaryFunctions { get; set; }

		[NullAllowed, Export ("groups", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<IMTLFunction>> Groups { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("privateFunctions", ArgumentSemantic.Copy)]
		IMTLFunction [] PrivateFunctions { get; set; }

		[iOS (15, 0), NoTV, MacCatalyst (15, 0)]
		[Export ("instanceDescriptorType", ArgumentSemantic.Assign)]
		MTLAccelerationStructureInstanceDescriptorType InstanceDescriptorType { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("motionTransformBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer MotionTransformBuffer { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("motionTransformBufferOffset")]
		nuint MotionTransformBufferOffset { get; set; }

		[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
		[Export ("motionTransformCount")]
		nuint MotionTransformCount { get; set; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (MTLAccelerationStructureDescriptor))]
	interface MTLPrimitiveAccelerationStructureDescriptor {
		[NullAllowed, Export ("geometryDescriptors", ArgumentSemantic.Retain)]
		MTLAccelerationStructureGeometryDescriptor [] GeometryDescriptors { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLPrimitiveAccelerationStructureDescriptor Create ();

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("motionStartBorderMode", ArgumentSemantic.Assign)]
		MTLMotionBorderMode MotionStartBorderMode { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("motionEndBorderMode", ArgumentSemantic.Assign)]
		MTLMotionBorderMode MotionEndBorderMode { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("motionStartTime")]
		float MotionStartTime { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("motionEndTime")]
		float MotionEndTime { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("motionKeyframeCount")]
		nuint MotionKeyframeCount { get; set; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPassSampleBufferAttachmentDescriptor : NSCopying {
		[NullAllowed, Export ("sampleBuffer", ArgumentSemantic.Retain)]
		IMTLCounterSampleBuffer SampleBuffer { get; set; }

		[Export ("startOfVertexSampleIndex")]
		nuint StartOfVertexSampleIndex { get; set; }

		[Export ("endOfVertexSampleIndex")]
		nuint EndOfVertexSampleIndex { get; set; }

		[Export ("startOfFragmentSampleIndex")]
		nuint StartOfFragmentSampleIndex { get; set; }

		[Export ("endOfFragmentSampleIndex")]
		nuint EndOfFragmentSampleIndex { get; set; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPassSampleBufferAttachmentDescriptorArray {
		[Export ("objectAtIndexedSubscript:")]
		MTLRenderPassSampleBufferAttachmentDescriptor GetObject (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLRenderPassSampleBufferAttachmentDescriptor attachment, nuint attachmentIndex);

	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface MTLResourceStatePassDescriptor : NSCopying {
		[Static]
		[Export ("resourceStatePassDescriptor")]
		MTLResourceStatePassDescriptor Create ();

		[Export ("sampleBufferAttachments")]
		MTLResourceStatePassSampleBufferAttachmentDescriptorArray SampleBufferAttachments { get; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLResourceStatePassSampleBufferAttachmentDescriptor : NSCopying {
		[NullAllowed, Export ("sampleBuffer", ArgumentSemantic.Retain)]
		IMTLCounterSampleBuffer SampleBuffer { get; set; }

		[Export ("startOfEncoderSampleIndex")]
		nuint StartOfEncoderSampleIndex { get; set; }

		[Export ("endOfEncoderSampleIndex")]
		nuint EndOfEncoderSampleIndex { get; set; }
	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLResourceStatePassSampleBufferAttachmentDescriptorArray {
		[Export ("objectAtIndexedSubscript:")]
		MTLResourceStatePassSampleBufferAttachmentDescriptor GetObject (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLResourceStatePassSampleBufferAttachmentDescriptor attachment, nuint attachmentIndex);

	}

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface MTLVisibleFunctionTableDescriptor : NSCopying {
		[Static]
		[Export ("visibleFunctionTableDescriptor")]
		MTLVisibleFunctionTableDescriptor Create ();

		[Export ("functionCount")]
		nuint FunctionCount { get; set; }
	}

	interface IMTLFunctionHandle { }

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLFunctionHandle {
		[Abstract]
		[Export ("functionType")]
		MTLFunctionType FunctionType { get; }

		[Abstract]
		[Export ("name")]
		string Name { get; }

		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Abstract]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }
	}

	interface IMTLAccelerationStructureCommandEncoder { }

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLAccelerationStructureCommandEncoder : MTLCommandEncoder {
		[Abstract]
		[Export ("buildAccelerationStructure:descriptor:scratchBuffer:scratchBufferOffset:")]
		void BuildAccelerationStructure (IMTLAccelerationStructure accelerationStructure, MTLAccelerationStructureDescriptor descriptor, IMTLBuffer scratchBuffer, nuint scratchBufferOffset);

		[Abstract]
		[Export ("refitAccelerationStructure:descriptor:destination:scratchBuffer:scratchBufferOffset:")]
		void RefitAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, MTLAccelerationStructureDescriptor descriptor, [NullAllowed] IMTLAccelerationStructure destinationAccelerationStructure, IMTLBuffer scratchBuffer, nuint scratchBufferOffset);

		[Abstract]
		[Export ("copyAccelerationStructure:toAccelerationStructure:")]
		void CopyAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, IMTLAccelerationStructure destinationAccelerationStructure);

		[Abstract]
		[Export ("writeCompactedAccelerationStructureSize:toBuffer:offset:")]
		void WriteCompactedAccelerationStructureSize (IMTLAccelerationStructure accelerationStructure, IMTLBuffer buffer, nuint offset);

		[Abstract]
		[Export ("copyAndCompactAccelerationStructure:toAccelerationStructure:")]
		void CopyAndCompactAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, IMTLAccelerationStructure destinationAccelerationStructure);

		[Abstract]
		[Export ("updateFence:")]
		void UpdateFence (IMTLFence fence);

		[Abstract]
		[Export ("waitForFence:")]
		void WaitForFence (IMTLFence fence);

		[Abstract]
		[Export ("useResource:usage:")]
		void UseResource (IMTLResource resource, MTLResourceUsage usage);

		[Abstract]
		[Export ("useResources:count:usage:")]
		void UseResources (IMTLResource [] resources, nuint count, MTLResourceUsage usage);

		[Abstract]
		[Export ("useHeap:")]
		void UseHeap (IMTLHeap heap);

		[Abstract]
		[Export ("useHeaps:count:")]
		void UseHeaps (IMTLHeap [] heaps, nuint count);

		[Abstract]
		[Export ("sampleCountersInBuffer:atSampleIndex:withBarrier:")]
		void SampleCountersInBuffer (IMTLCounterSampleBuffer sampleBuffer, nuint sampleIndex, bool barrier);

		[Abstract]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("writeCompactedAccelerationStructureSize:toBuffer:offset:sizeDataType:")]
		void WriteCompactedAccelerationStructureSize (IMTLAccelerationStructure accelerationStructure, IMTLBuffer buffer, nuint offset, MTLDataType sizeDataType);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("refitAccelerationStructure:descriptor:destination:scratchBuffer:scratchBufferOffset:options:")]
		void RefitAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, MTLAccelerationStructureDescriptor descriptor, [NullAllowed] IMTLAccelerationStructure destinationAccelerationStructure, [NullAllowed] IMTLBuffer scratchBuffer, nuint scratchBufferOffset, MTLAccelerationStructureRefitOptions options);

	}

	interface IMTLVisibleFunctionTable { }

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLVisibleFunctionTable : MTLResource {
		[Abstract]
		[Export ("setFunction:atIndex:")]
		void SetFunction ([NullAllowed] IMTLFunctionHandle function, nuint index);

		[Abstract]
		[Export ("setFunctions:withRange:")]
		void SetFunctions (IMTLFunctionHandle [] functions, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }
	}

	interface IMTLIntersectionFunctionTable { }

	[iOS (14, 0), TV (16, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLIntersectionFunctionTable : MTLResource {
		[Abstract]
		[Export ("setBuffer:offset:atIndex:")]
		void SetBuffer ([NullAllowed] IMTLBuffer buffer, nuint offset, nuint index);

		[Abstract]
		[Export ("setBuffers:offsets:withRange:")]
		void SetBuffers (IntPtr /* IMTLBuffer[] */ buffers, /* nuint[]*/ IntPtr offsets, NSRange range);

		[Abstract]
		[Export ("setFunction:atIndex:")]
		void SetFunction ([NullAllowed] IMTLFunctionHandle function, nuint index);

		[Abstract]
		[Export ("setFunctions:withRange:")]
		void SetFunctions (IMTLFunctionHandle [] functions, NSRange range);

		[Abstract]
		[Export ("setOpaqueTriangleIntersectionFunctionWithSignature:atIndex:")]
		void SetOpaqueTriangleIntersectionFunction (MTLIntersectionFunctionSignature signature, nuint index);

		[Abstract]
		[Export ("setOpaqueTriangleIntersectionFunctionWithSignature:withRange:")]
		void SetOpaqueTriangleIntersectionFunction (MTLIntersectionFunctionSignature signature, NSRange range);

		[Abstract]
		[Export ("setVisibleFunctionTable:atBufferIndex:")]
		void SetVisibleFunctionTable ([NullAllowed] IMTLVisibleFunctionTable functionTable, nuint bufferIndex);

		[Abstract]
		[Export ("setVisibleFunctionTables:withBufferRange:")]
		void SetVisibleFunctionTables (IMTLVisibleFunctionTable [] functionTables, NSRange bufferRange);

		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setOpaqueCurveIntersectionFunctionWithSignature:atIndex:")]
		void SetOpaqueCurveIntersectionFunction (MTLIntersectionFunctionSignature signature, nuint index);

		[Abstract (GenerateExtensionMethod = true)]
		[Export ("setOpaqueCurveIntersectionFunctionWithSignature:withRange:")]
		void SetOpaqueCurveIntersectionFunction (MTLIntersectionFunctionSignature signature, NSRange range);

		[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
		[Abstract (GenerateExtensionMethod = true)]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }
	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLCommandBufferEncoderInfo {

		[Abstract]
		[Export ("label")]
		string Label { get; }

		[Abstract]
		[Export ("debugSignposts")]
		string [] DebugSignposts { get; }

		[Abstract]
		[Export ("errorState")]
		MTLCommandEncoderErrorState ErrorState { get; }
	}

	interface IMTLDynamicLibrary { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLDynamicLibrary {

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[Export ("installName")]
		string InstallName { get; }

		[Abstract]
		[Export ("serializeToURL:error:")]
		bool Serialize (NSUrl url, [NullAllowed] out NSError error);
	}

	interface IMTLLogContainer { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLLogContainer : INSFastEnumeration {

	}

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLFunctionLog {
		[Abstract]
		[Export ("type")]
		MTLFunctionLogType Type { get; }

		[Abstract]
		[NullAllowed, Export ("encoderLabel")]
		string EncoderLabel { get; }

		[Abstract]
		[NullAllowed, Export ("function")]
		IMTLFunction Function { get; }

		[Abstract]
		[NullAllowed, Export ("debugLocation")]
		IMTLFunctionLogDebugLocation DebugLocation { get; }
	}

	interface IMTLFunctionLogDebugLocation { }

	[iOS (14, 0), TV (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface MTLFunctionLogDebugLocation {
		[Abstract]
		[NullAllowed, Export ("functionName")]
		string FunctionName { get; }

		[Abstract]
		[NullAllowed, Export ("URL")]
		NSUrl Url { get; }

		[Abstract]
		[Export ("line")]
		nuint Line { get; }

		[Abstract]
		[Export ("column")]
		nuint Column { get; }
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLStitchedLibraryDescriptor : NSCopying {
		[Export ("functionGraphs", ArgumentSemantic.Copy)]
		MTLFunctionStitchingGraph [] FunctionGraphs { get; set; }

		[Export ("functions", ArgumentSemantic.Copy)]
		IMTLFunction [] Functions { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("binaryArchives", ArgumentSemantic.Copy)]
		IMTLBinaryArchive [] BinaryArchives { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("options")]
		MTLStitchedLibraryOptions Options { get; set; }
	}

	[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLRenderPipelineFunctionsDescriptor : NSCopying {
		[NullAllowed, Export ("vertexAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTLFunction [] VertexAdditionalBinaryFunctions { get; set; }

		[NullAllowed, Export ("fragmentAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTLFunction [] FragmentAdditionalBinaryFunctions { get; set; }

		[NullAllowed, Export ("tileAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTLFunction [] TileAdditionalBinaryFunctions { get; set; }
	}

	[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLMotionKeyframeData {
		[NullAllowed, Export ("buffer", ArgumentSemantic.Retain)]
		IMTLBuffer Buffer { get; set; }

		[Export ("offset")]
		nuint Offset { get; set; }

		[Static]
		[Export ("data")]
		MTLMotionKeyframeData Create ();
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	interface MTLFunctionStitchingNode : NSCopying { }

	interface IMTLFunctionStitchingNode { }

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[Protocol] // From Apple Docs: Your app does not define classes that implement this protocol. Model is not needed
	interface MTLFunctionStitchingAttribute : NSCopying { }

	interface IMTLFunctionStitchingAttribute { }

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	interface MTLFunctionStitchingAttributeAlwaysInline : MTLFunctionStitchingAttribute { }

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLFunctionStitchingInputNode : MTLFunctionStitchingNode {
		[Export ("argumentIndex")]
		nuint ArgumentIndex { get; set; }

		[Export ("initWithArgumentIndex:")]
		NativeHandle Constructor (nuint argument);
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLFunctionStitchingGraph : NSCopying {
		[Export ("functionName")]
		string FunctionName { get; set; }

		[Export ("nodes", ArgumentSemantic.Copy)]
		MTLFunctionStitchingFunctionNode [] Nodes { get; set; }

		[NullAllowed, Export ("outputNode", ArgumentSemantic.Retain)]
		MTLFunctionStitchingFunctionNode OutputNode { get; set; }

		[Export ("attributes", ArgumentSemantic.Copy)]
		IMTLFunctionStitchingAttribute [] Attributes { get; set; }

		[Export ("initWithFunctionName:nodes:outputNode:attributes:")]
		NativeHandle Constructor (string functionName, MTLFunctionStitchingFunctionNode [] nodes, [NullAllowed] MTLFunctionStitchingFunctionNode outputNode, IMTLFunctionStitchingAttribute [] attributes);
	}

	[iOS (15, 0), TV (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface MTLFunctionStitchingFunctionNode : MTLFunctionStitchingNode {
		[Export ("name")]
		string Name { get; set; }

		[Export ("arguments", ArgumentSemantic.Copy)]
		IMTLFunctionStitchingNode [] Arguments { get; set; }

		[Export ("controlDependencies", ArgumentSemantic.Copy)]
		MTLFunctionStitchingFunctionNode [] ControlDependencies { get; set; }

		[Export ("initWithName:arguments:controlDependencies:")]
		NativeHandle Constructor (string name, IMTLFunctionStitchingNode [] arguments, MTLFunctionStitchingFunctionNode [] controlDependencies);
	}

	[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MTLAccelerationStructureGeometryDescriptor))]
	interface MTLAccelerationStructureMotionTriangleGeometryDescriptor {
		[Export ("vertexBuffers", ArgumentSemantic.Copy)]
		MTLMotionKeyframeData [] VertexBuffers { get; set; }

		[Export ("vertexStride")]
		nuint VertexStride { get; set; }

		[NullAllowed, Export ("indexBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer IndexBuffer { get; set; }

		[Export ("indexBufferOffset")]
		nuint IndexBufferOffset { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("triangleCount")]
		nuint TriangleCount { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLAccelerationStructureMotionTriangleGeometryDescriptor Create ();

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("vertexFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat VertexFormat { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[NullAllowed, Export ("transformationMatrixBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer TransformationMatrixBuffer { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0)]
		[Export ("transformationMatrixBufferOffset")]
		nuint TransformationMatrixBufferOffset { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("transformationMatrixLayout")]
		MTLMatrixLayout TransformationMatrixLayout { get; set; }
	}

	[iOS (15, 0), TV (16, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (MTLAccelerationStructureGeometryDescriptor))]
	interface MTLAccelerationStructureMotionBoundingBoxGeometryDescriptor {
		[Export ("boundingBoxBuffers", ArgumentSemantic.Copy)]
		MTLMotionKeyframeData [] BoundingBoxBuffers { get; set; }

		[Export ("boundingBoxStride")]
		nuint BoundingBoxStride { get; set; }

		[Export ("boundingBoxCount")]
		nuint BoundingBoxCount { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLAccelerationStructureMotionBoundingBoxGeometryDescriptor Create ();
	}

	interface IMTLBinding { }

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface MTLBinding {
		[Abstract]
		[Export ("name")]
		string Name { get; }

		[Abstract]
		[Export ("type")]
		MTLBindingType Type { get; }

		[Abstract]
		[Export ("access")]
		MTLBindingAccess Access { get; }

		[Abstract]
		[Export ("index")]
		nuint Index { get; }

		[Abstract]
		[Export ("used")]
		bool Used { [Bind ("isUsed")] get; }

		[Abstract]
		[Export ("argument")]
		bool Argument { [Bind ("isArgument")] get; }
	}

	interface IMTLBufferBinding { }

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface MTLBufferBinding : MTLBinding {
		[Abstract]
		[Export ("bufferAlignment")]
		nuint BufferAlignment { get; }

		[Abstract]
		[Export ("bufferDataSize")]
		nuint BufferDataSize { get; }

		[Abstract]
		[Export ("bufferDataType")]
		MTLDataType BufferDataType { get; }

		[Abstract]
		[NullAllowed, Export ("bufferStructType")]
		MTLStructType BufferStructType { get; }

		[Abstract]
		[NullAllowed, Export ("bufferPointerType")]
		MTLPointerType BufferPointerType { get; }
	}


	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface MTLObjectPayloadBinding : MTLBinding {
		[Abstract]
		[Export ("objectPayloadAlignment")]
		nuint ObjectPayloadAlignment { get; }

		[Abstract]
		[Export ("objectPayloadDataSize")]
		nuint ObjectPayloadDataSize { get; }
	}

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface MTLTextureBinding : MTLBinding {
		[Abstract]
		[Export ("textureType")]
		MTLTextureType TextureType { get; }

		[Abstract]
		[Export ("textureDataType")]
		MTLDataType TextureDataType { get; }

		[Abstract]
		[Export ("depthTexture")]
		bool DepthTexture { [Bind ("isDepthTexture")] get; }

		[Abstract]
		[Export ("arrayLength")]
		nuint ArrayLength { get; }
	}

	[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (MTLAccelerationStructureGeometryDescriptor))]
	interface MTLAccelerationStructureCurveGeometryDescriptor {
		[NullAllowed, Export ("controlPointBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer ControlPointBuffer { get; set; }

		[Export ("controlPointBufferOffset")]
		nuint ControlPointBufferOffset { get; set; }

		[Export ("controlPointCount")]
		nuint ControlPointCount { get; set; }

		[Export ("controlPointStride")]
		nuint ControlPointStride { get; set; }

		[Export ("controlPointFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat ControlPointFormat { get; set; }

		[NullAllowed, Export ("radiusBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer RadiusBuffer { get; set; }

		[Export ("radiusBufferOffset")]
		nuint RadiusBufferOffset { get; set; }

		[Export ("radiusFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat RadiusFormat { get; set; }

		[Export ("radiusStride")]
		nuint RadiusStride { get; set; }

		[NullAllowed, Export ("indexBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer IndexBuffer { get; set; }

		[Export ("indexBufferOffset")]
		nuint IndexBufferOffset { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("segmentCount")]
		nuint SegmentCount { get; set; }

		[Export ("segmentControlPointCount")]
		nuint SegmentControlPointCount { get; set; }

		[Export ("curveType", ArgumentSemantic.Assign)]
		MTLCurveType CurveType { get; set; }

		[Export ("curveBasis", ArgumentSemantic.Assign)]
		MTLCurveBasis CurveBasis { get; set; }

		[Export ("curveEndCaps", ArgumentSemantic.Assign)]
		MTLCurveEndCaps CurveEndCaps { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLAccelerationStructureCurveGeometryDescriptor GetDescriptor ();
	}

	[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (MTLAccelerationStructureGeometryDescriptor))]
	interface MTLAccelerationStructureMotionCurveGeometryDescriptor {
		[Export ("controlPointBuffers", ArgumentSemantic.Copy)]
		MTLMotionKeyframeData [] ControlPointBuffers { get; set; }

		[Export ("controlPointCount")]
		nuint ControlPointCount { get; set; }

		[Export ("controlPointStride")]
		nuint ControlPointStride { get; set; }

		[Export ("controlPointFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat ControlPointFormat { get; set; }

		[Export ("radiusBuffers", ArgumentSemantic.Copy)]
		MTLMotionKeyframeData [] RadiusBuffers { get; set; }

		[Export ("radiusFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat RadiusFormat { get; set; }

		[Export ("radiusStride")]
		nuint RadiusStride { get; set; }

		[NullAllowed, Export ("indexBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer IndexBuffer { get; set; }

		[Export ("indexBufferOffset")]
		nuint IndexBufferOffset { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("segmentCount")]
		nuint SegmentCount { get; set; }

		[Export ("segmentControlPointCount")]
		nuint SegmentControlPointCount { get; set; }

		[Export ("curveType", ArgumentSemantic.Assign)]
		MTLCurveType CurveType { get; set; }

		[Export ("curveBasis", ArgumentSemantic.Assign)]
		MTLCurveBasis CurveBasis { get; set; }

		[Export ("curveEndCaps", ArgumentSemantic.Assign)]
		MTLCurveEndCaps CurveEndCaps { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLAccelerationStructureMotionCurveGeometryDescriptor GetDescriptor ();
	}

	[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLArchitecture : NSCopying {
		[Export ("name")]
		string Name { get; }
	}

	[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (MTLAccelerationStructureDescriptor))]
	interface MTLIndirectInstanceAccelerationStructureDescriptor {
		[NullAllowed, Export ("instanceDescriptorBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer InstanceDescriptorBuffer { get; set; }

		[Export ("instanceDescriptorBufferOffset")]
		nuint InstanceDescriptorBufferOffset { get; set; }

		[Export ("instanceDescriptorStride")]
		nuint InstanceDescriptorStride { get; set; }

		[Export ("maxInstanceCount")]
		nuint MaxInstanceCount { get; set; }

		[NullAllowed, Export ("instanceCountBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer InstanceCountBuffer { get; set; }

		[Export ("instanceCountBufferOffset")]
		nuint InstanceCountBufferOffset { get; set; }

		[Export ("instanceDescriptorType", ArgumentSemantic.Assign)]
		MTLAccelerationStructureInstanceDescriptorType InstanceDescriptorType { get; set; }

		[NullAllowed, Export ("motionTransformBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer MotionTransformBuffer { get; set; }

		[Export ("motionTransformBufferOffset")]
		nuint MotionTransformBufferOffset { get; set; }

		[Export ("maxMotionTransformCount")]
		nuint MaxMotionTransformCount { get; set; }

		[NullAllowed, Export ("motionTransformCountBuffer", ArgumentSemantic.Retain)]
		IMTLBuffer MotionTransformCountBuffer { get; set; }

		[Export ("motionTransformCountBufferOffset")]
		nuint MotionTransformCountBufferOffset { get; set; }

		[Static]
		[Export ("descriptor")]
		MTLIndirectInstanceAccelerationStructureDescriptor GetDescriptor ();

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("instanceTransformationMatrixLayout")]
		MTLMatrixLayout InstanceTransformationMatrixLayout { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("motionTransformType")]
		MTLTransformType MotionTransformType { get; set; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
		[Export ("motionTransformStride")]
		nuint MotionTransformStride { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLMeshRenderPipelineDescriptor : NSCopying {
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[NullAllowed, Export ("objectFunction", ArgumentSemantic.Strong)]
		IMTLFunction ObjectFunction { get; set; }

		[NullAllowed, Export ("meshFunction", ArgumentSemantic.Strong)]
		IMTLFunction MeshFunction { get; set; }

		[NullAllowed, Export ("fragmentFunction", ArgumentSemantic.Strong)]
		IMTLFunction FragmentFunction { get; set; }

		[Export ("maxTotalThreadsPerObjectThreadgroup")]
		nuint MaxTotalThreadsPerObjectThreadgroup { get; set; }

		[Export ("maxTotalThreadsPerMeshThreadgroup")]
		nuint MaxTotalThreadsPerMeshThreadgroup { get; set; }

		[Export ("objectThreadgroupSizeIsMultipleOfThreadExecutionWidth")]
		bool ObjectThreadgroupSizeIsMultipleOfThreadExecutionWidth { get; set; }

		[Export ("meshThreadgroupSizeIsMultipleOfThreadExecutionWidth")]
		bool MeshThreadgroupSizeIsMultipleOfThreadExecutionWidth { get; set; }

		[Export ("payloadMemoryLength")]
		nuint PayloadMemoryLength { get; set; }

		[Export ("maxTotalThreadgroupsPerMeshGrid")]
		nuint MaxTotalThreadgroupsPerMeshGrid { get; set; }

		[Export ("objectBuffers")]
		MTLPipelineBufferDescriptorArray ObjectBuffers { get; }

		[Export ("meshBuffers")]
		MTLPipelineBufferDescriptorArray MeshBuffers { get; }

		[Export ("fragmentBuffers")]
		MTLPipelineBufferDescriptorArray FragmentBuffers { get; }

		[Export ("rasterSampleCount")]
		nuint RasterSampleCount { get; set; }

		[Export ("alphaToCoverageEnabled")]
		bool AlphaToCoverageEnabled { [Bind ("isAlphaToCoverageEnabled")] get; set; }

		[Export ("alphaToOneEnabled")]
		bool AlphaToOneEnabled { [Bind ("isAlphaToOneEnabled")] get; set; }

		[Export ("rasterizationEnabled")]
		bool RasterizationEnabled { [Bind ("isRasterizationEnabled")] get; set; }

		[Export ("maxVertexAmplificationCount")]
		nuint MaxVertexAmplificationCount { get; set; }

		[Export ("colorAttachments")]
		MTLRenderPipelineColorAttachmentDescriptorArray ColorAttachments { get; }

		[Export ("depthAttachmentPixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat DepthAttachmentPixelFormat { get; set; }

		[Export ("stencilAttachmentPixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat StencilAttachmentPixelFormat { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[Export ("supportIndirectCommandBuffers")]
		bool SupportIndirectCommandBuffers { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[NullAllowed, Export ("objectLinkedFunctions", ArgumentSemantic.Copy)]
		MTLLinkedFunctions ObjectLinkedFunctions { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[NullAllowed, Export ("meshLinkedFunctions", ArgumentSemantic.Copy)]
		MTLLinkedFunctions MeshLinkedFunctions { get; set; }

		[Mac (14, 0), iOS (17, 0), TV (17, 0), MacCatalyst (17, 0)]
		[NullAllowed, Export ("fragmentLinkedFunctions", ArgumentSemantic.Copy)]
		MTLLinkedFunctions FragmentLinkedFunctions { get; set; }

		[Export ("reset")]
		void Reset ();

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("binaryArchives", ArgumentSemantic.Copy), NullAllowed]
		IMTLBinaryArchive [] BinaryArchives { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("shaderValidation")]
		MTLShaderValidation ShaderValidation { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("requiredThreadsPerObjectThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerObjectThreadgroup { get; set; }

		[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
		[Export ("requiredThreadsPerMeshThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerMeshThreadgroup { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLAccelerationStructurePassSampleBufferAttachmentDescriptor : NSCopying {
		[NullAllowed, Export ("sampleBuffer", ArgumentSemantic.Retain)]
		IMTLCounterSampleBuffer SampleBuffer { get; set; }

		[Export ("startOfEncoderSampleIndex")]
		nuint StartOfEncoderSampleIndex { get; set; }

		[Export ("endOfEncoderSampleIndex")]
		nuint EndOfEncoderSampleIndex { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLAccelerationStructurePassSampleBufferAttachmentDescriptorArray {
		[Export ("objectAtIndexedSubscript:")]
		MTLAccelerationStructurePassSampleBufferAttachmentDescriptor GetObject (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTLAccelerationStructurePassSampleBufferAttachmentDescriptor attachment, nuint attachmentIndex);
	}

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLAccelerationStructurePassDescriptor : NSCopying {
		[Static]
		[Export ("accelerationStructurePassDescriptor")]
		MTLAccelerationStructurePassDescriptor AccelerationStructurePassDescriptor { get; }

		[Export ("sampleBufferAttachments")]
		MTLAccelerationStructurePassSampleBufferAttachmentDescriptorArray SampleBufferAttachments { get; }
	}

	[Mac (13, 0), iOS (16, 0), TV (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface MTLThreadgroupBinding : MTLBinding {
		[Abstract]
		[Export ("threadgroupMemoryAlignment")]
		nuint ThreadgroupMemoryAlignment { get; }

		[Abstract]
		[Export ("threadgroupMemoryDataSize")]
		nuint ThreadgroupMemoryDataSize { get; }
	}

	[Native]
	[Mac (15, 0), TV (18, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MTLMatrixLayout : long {
		ColumnMajor = 0,
		RowMajor = 1,
	}

	[Native]
	[Mac (15, 0), TV (18, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MTLTransformType : long {
		PackedFloat4x3 = 0,
		Component = 1,
	}

	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
	interface MTLAllocation {
		[Abstract]
		[Export ("allocatedSize")]
		nuint AllocatedSize { get; }
	}

	interface IMTLAllocation { }

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLCommandQueueDescriptor : NSCopying {
		[Export ("maxCommandBufferCount")]
		nuint MaxCommandBufferCount { get; set; }

		[Export ("logState", ArgumentSemantic.Retain), NullAllowed]
		IMTLLogState LogState { get; set; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BackingFieldType (typeof (nint))]
	enum NSDeviceCertification {
		[Field ("NSDeviceCertificationiPhonePerformanceGaming")]
		iPhonePerformanceGaming,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BackingFieldType (typeof (nint))]
	enum NSProcessPerformanceProfile {
		[Field ("NSProcessPerformanceProfileDefault")]
		Default,

		[Field ("NSProcessPerformanceProfileSustained")]
		Sustained,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Category]
	[BaseType (typeof (NSProcessInfo))]
	interface NSProcessInfo_NSDeviceCertification {
		[Export ("isDeviceCertifiedFor:")]
		bool IsDeviceCertifiedFor (NSDeviceCertification performanceTier);

		[Export ("hasPerformanceProfile:")]
		bool HasPerformanceProfile (NSProcessPerformanceProfile performanceProfile);
	}

	[Flags]
	[Native]
	[Mac (15, 0), TV (18, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MTLStitchedLibraryOptions : ulong {
		None = 0,
		FailOnBinaryArchiveMiss = 1 << 0,
		StoreLibraryInMetalPipelinesScript = 1 << 1,
	}

	[Native]
	[Mac (15, 0), TV (18, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MTLMathMode : long {
		Safe = 0,
		Relaxed = 1,
		Fast = 2,
	}

	[Native]
	[Mac (15, 0), TV (18, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MTLMathFloatingPointFunctions : long {
		Fast = 0,
		Precise = 1,
	}

	[Native]
	[Mac (15, 0), TV (18, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MTLLogLevel : long {
		Undefined,
		Debug,
		Info,
		Notice,
		Error,
		Fault,
	}

	delegate void MTLLogStateLogHandler ([NullAllowed] string subSystem, [NullAllowed] string category, MTLLogLevel logLevel, string message);

	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
	interface MTLLogState {
		[Abstract]
		[Export ("addLogHandler:")]
		void AddLogHandler (MTLLogStateLogHandler handler);
	}

	interface IMTLLogState { }

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLLogStateDescriptor : NSCopying {
		[Export ("level", ArgumentSemantic.Assign)]
		MTLLogLevel Level { get; set; }

		[Export ("bufferSize", ArgumentSemantic.Assign)]
		nint BufferSize { get; set; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
	[ErrorDomain ("MTLLogStateErrorDomain")]
	[Native]
	enum MTLLogStateError : ulong {
		InvalidSize = 1,
		Invalid = 2,
	}

	[Native]
	[Mac (15, 0), TV (18, 0), iOS (18, 0), MacCatalyst (18, 0)]
	enum MTLShaderValidation : long {
		Default = 0,
		Enabled = 1,
		Disabled = 2,
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLResidencySetDescriptor : NSCopying {
		[Export ("label", ArgumentSemantic.Copy), NullAllowed]
		string Label { get; set; }

		[Export ("initialCapacity")]
		nuint InitialCapacity { get; set; }
	}

	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), TV (18, 0)]
	interface MTLResidencySet {
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[Export ("label"), NullAllowed]
		string Label { get; }

		[Abstract]
		[Export ("allocatedSize")]
		ulong AllocatedSize { get; }

		[Abstract]
		[Export ("requestResidency")]
		void RequestResidency ();

		[Abstract]
		[Export ("endResidency")]
		void EndResidency ();

		[Abstract]
		[Export ("addAllocation:")]
		void AddAllocation (IMTLAllocation allocation);

		[Abstract]
		[Export ("addAllocations:count:")]
		void AddAllocations (IntPtr allocations, nuint count);

		[Abstract]
		[Export ("removeAllocation:")]
		void RemoveAllocation (IMTLAllocation allocation);

		[Abstract]
		[Export ("removeAllocations:count:")]
		void RemoveAllocations (IntPtr allocations, nuint count);

		[Abstract]
		[Export ("removeAllAllocations")]
		void RemoveAllAllocations ();

		[Abstract]
		[Export ("containsAllocation:")]
		bool ContainsAllocation (IMTLAllocation allocation);

		[Abstract]
		[Export ("allAllocations", ArgumentSemantic.Copy)]
		IMTLAllocation [] AllAllocations { get; }

		[Abstract]
		[Export ("allocationCount")]
		nuint AllocationCount { get; }

		[Abstract]
		[Export ("commit")]
		void Commit ();
	}

	interface IMTLResidencySet { }

	interface IMTL4Archive { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4Archive {
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (MTL4ComputePipelineDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:dynamicLinkingDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (MTL4ComputePipelineDescriptor descriptor, MTL4PipelineStageDynamicLinkingDescriptor dynamicLinkingDescriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTL4PipelineDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:dynamicLinkingDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTL4PipelineDescriptor descriptor, MTL4RenderPipelineDynamicLinkingDescriptor dynamicLinkingDescriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newBinaryFunctionWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4BinaryFunction CreateBinaryFunction (MTL4BinaryFunctionDescriptor descriptor, [NullAllowed] out NSError error);
	}

	interface IMTL4ArgumentTable { }
	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4ArgumentTable {
		[Abstract]
		[Export ("setAddress:atIndex:")]
		void SetAddress (ulong gpuAddress, nuint bindingIndex);

		[Abstract]
		[Export ("setAddress:attributeStride:atIndex:")]
		void SetAddress (ulong gpuAddress, nuint stride, nuint bindingIndex);

		[Abstract]
		[Export ("setResource:atBufferIndex:")]
		void SetResource (MTLResourceId resourceId, nuint bindingIndex);

		[Abstract]
		[Export ("setTexture:atIndex:")]
		void SetTexture (MTLResourceId resourceId, nuint bindingIndex);

		[Abstract]
		[Export ("setSamplerState:atIndex:")]
		void SetSamplerState (MTLResourceId resourceId, nuint bindingIndex);

		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }
	}

	interface IMTL4BinaryFunction { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4BinaryFunction {
		[Abstract]
		[NullAllowed, Export ("name")]
		string Name { get; }

		[Abstract]
		[Export ("functionType")]
		MTLFunctionType FunctionType { get; }
	}

	interface IMTL4CommandAllocator { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4CommandAllocator {
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }

		[Abstract]
		[Export ("allocatedSize")]
		ulong AllocatedSize { get; }

		[Abstract]
		[Export ("reset")]
		void Reset ();
	}

	interface IMTL4CommandBuffer { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4CommandBuffer {
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Abstract]
		[Export ("beginCommandBufferWithAllocator:")]
		void BeginCommandBuffer (IMTL4CommandAllocator allocator);

		[Abstract]
		[Export ("beginCommandBufferWithAllocator:options:")]
		void BeginCommandBuffer (IMTL4CommandAllocator allocator, MTL4CommandBufferOptions options);

		[Abstract]
		[Export ("endCommandBuffer")]
		void EndCommandBuffer ();

		[Abstract]
		[Export ("renderCommandEncoderWithDescriptor:")]
		[return: NullAllowed]
		IMTL4RenderCommandEncoder CreateRenderCommandEncoder (MTL4RenderPassDescriptor descriptor);

		[Abstract]
		[Export ("renderCommandEncoderWithDescriptor:options:")]
		[return: NullAllowed]
		IMTL4RenderCommandEncoder CreateRenderCommandEncoder (MTL4RenderPassDescriptor descriptor, MTL4RenderEncoderOptions options);

		[Abstract]
		[Export ("computeCommandEncoder")]
		[return: NullAllowed]
		IMTL4ComputeCommandEncoder CreateComputeCommandEncoder ();

		[Abstract]
		[Export ("machineLearningCommandEncoder")]
		[return: NullAllowed]
		IMTL4MachineLearningCommandEncoder CreateMachineLearningCommandEncoder ();

		[Abstract]
		[Export ("useResidencySet:")]
		void UseResidencySet (IMTLResidencySet residencySet);

		[Abstract]
		[Export ("useResidencySets:count:")]
		void UseResidencySets (/* C Array: IMTLResidencySet[] */ IntPtr residencySets, nuint count);

		[Abstract]
		[Export ("pushDebugGroup:")]
		void PushDebugGroup (string @string);

		[Abstract]
		[Export ("popDebugGroup")]
		void PopDebugGroup ();

		[Abstract]
		[Export ("writeTimestampIntoHeap:atIndex:")]
		void WriteTimestamp (IMTL4CounterHeap counterHeap, nuint index);

		[Abstract]
		[Export ("resolveCounterHeap:withRange:intoBuffer:waitFence:updateFence:")]
		void ResolveCounterHeap (IMTL4CounterHeap counterHeap, NSRange range, MTL4BufferRange bufferRange, [NullAllowed] IMTLFence fenceToWait, [NullAllowed] IMTLFence fenceToUpdate);
	}

	interface IMTL4CommandEncoder { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4CommandEncoder {
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Abstract]
		[NullAllowed, Export ("commandBuffer")]
		IMTL4CommandBuffer CommandBuffer { get; }

		[Abstract]
		[Export ("barrierAfterQueueStages:beforeStages:visibilityOptions:")]
		void BarrierAfterQueueStages (MTLStages afterQueueStages, MTLStages beforeStages, MTL4VisibilityOptions visibilityOptions);

		[Abstract]
		[Export ("barrierAfterStages:beforeQueueStages:visibilityOptions:")]
		void BarrierAfterStages (MTLStages afterStages, MTLStages beforeQueueStages, MTL4VisibilityOptions visibilityOptions);

		[Abstract]
		[Export ("barrierAfterEncoderStages:beforeEncoderStages:visibilityOptions:")]
		void BarrierAfterEncoderStages (MTLStages afterEncoderStages, MTLStages beforeEncoderStages, MTL4VisibilityOptions visibilityOptions);

		[Abstract]
		[Export ("updateFence:afterEncoderStages:")]
		void UpdateFence (IMTLFence fence, MTLStages afterEncoderStages);

		[Abstract]
		[Export ("waitForFence:beforeEncoderStages:")]
		void WaitForFence (IMTLFence fence, MTLStages beforeEncoderStages);

		[Abstract]
		[Export ("insertDebugSignpost:")]
		void InsertDebugSignpost (string @string);

		[Abstract]
		[Export ("pushDebugGroup:")]
		void PushDebugGroup (string @string);

		[Abstract]
		[Export ("popDebugGroup")]
		void PopDebugGroup ();

		[Abstract]
		[Export ("endEncoding")]
		void EndEncoding ();
	}

	interface IMTL4CommandQueue { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4CommandQueue {
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }

		[Abstract]
		[Export ("commit:count:")]
		void Commit (/* C Array: IMTL4CommandBuffer[] */ IntPtr commandBuffers, nuint count);

		[Abstract]
		[Export ("commit:count:options:")]
		void Commit (/* C Array: IMTL4CommandBuffer[] */ IntPtr commandBuffers, nuint count, MTL4CommitOptions options);

		[Abstract]
		[Export ("signalEvent:value:")]
		void SignalEvent (IMTLEvent @event, ulong value);

		[Abstract]
		[Export ("waitForEvent:value:")]
		void WaitForEvent (IMTLEvent @event, ulong value);

		[Abstract]
		[Export ("signalDrawable:")]
		void SignalDrawable (IMTLDrawable drawable);

		[Abstract]
		[Export ("waitForDrawable:")]
		void WaitForDrawable (IMTLDrawable drawable);

		[Abstract]
		[Export ("addResidencySet:")]
		void AddResidencySet (IMTLResidencySet residencySet);

		[Abstract]
		[Export ("addResidencySets:count:")]
		void AddResidencySets (/* C Array: MTLResidencySet[] */ IntPtr residencySets, nuint count);

		[Abstract]
		[Export ("removeResidencySet:")]
		void RemoveResidencySet (IMTLResidencySet residencySet);

		[Abstract]
		[Export ("removeResidencySets:count:")]
		void RemoveResidencySets (/* C Array: IMTLResidencySet[] */ IntPtr residencySets, nuint count);

		[Abstract]
		[Export ("updateTextureMappings:heap:operations:count:")]
		void UpdateTextureMappings (IMTLTexture texture, [NullAllowed] IMTLHeap heap, /* C Array: MTL4UpdateSparseTextureMappingOperation[] */ IntPtr operations, nuint count);

		[Abstract]
		[Export ("copyTextureMappingsFromTexture:toTexture:operations:count:")]
		void CopyTextureMappings (IMTLTexture sourceTexture, IMTLTexture destinationTexture, /* C Array: MTL4CopySparseTextureMappingOperation[] */ IntPtr operations, nuint count);

		[Abstract]
		[Export ("updateBufferMappings:heap:operations:count:")]
		void UpdateBufferMappings (IMTLBuffer buffer, [NullAllowed] IMTLHeap heap, /* C Array: MTL4UpdateSparseBufferMappingOperation[] */ IntPtr operations, nuint count);

		[Abstract]
		[Export ("copyBufferMappingsFromBuffer:toBuffer:operations:count:")]
		void CopyBufferMappings (IMTLBuffer sourceBuffer, IMTLBuffer destinationBuffer, /* C Array: MTL4CopySparseBufferMappingOperation[] */ IntPtr operations, nuint count);
	}

	interface IMTL4CommitFeedback { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4CommitFeedback {
		[Abstract]
		[Export ("error")]
		[NullAllowed]
		NSError Error { get; }

		[Abstract]
		[Export ("GPUStartTime")]
		double GpuStartTime { get; }

		[Abstract]
		[Export ("GPUEndTime")]
		double GpuEndTime { get; }
	}

	interface IMTL4CompilerTask { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4CompilerTask {
		[Abstract]
		[Export ("compiler")]
		IMTL4Compiler Compiler { get; }

		[Abstract]
		[Export ("status")]
		MTL4CompilerTaskStatus Status { get; }

		[Abstract]
		[Export ("waitUntilCompleted")]
		void WaitUntilCompleted ();
	}

	delegate void MTLCreateLibraryCompletionHandler ([NullAllowed] IMTLLibrary library, [NullAllowed] NSError error);
	delegate void MTLCreateRenderPipelineStateCompletionHandler ([NullAllowed] IMTLRenderPipelineState renderPipelineState, [NullAllowed] NSError error);
	delegate void MTLCreateRenderPipelineStateWithReflectionCompletionHandler ([NullAllowed] IMTLRenderPipelineState renderPipelineState, [NullAllowed] MTLRenderPipelineReflection reflection, [NullAllowed] NSError error);
	delegate void MTLCreateComputePipelineStateCompletionHandler ([NullAllowed] IMTLComputePipelineState computePipelineState, [NullAllowed] NSError error);
	delegate void MTLCreateComputePipelineStateWithReflectionCompletionHandler ([NullAllowed] IMTLComputePipelineState computePipelineState, [NullAllowed] MTLComputePipelineReflection reflection, [NullAllowed] NSError error);
	delegate void MTLCreateDynamicLibraryCompletionHandler ([NullAllowed] IMTLDynamicLibrary library, [NullAllowed] NSError error);
	delegate void MTL4CreateBinaryFunctionCompletionHandler ([NullAllowed] IMTL4BinaryFunction function, [NullAllowed] NSError error);
	delegate void MTL4CreateMachineLearningPipelineStateCompletionHandler ([NullAllowed] IMTL4MachineLearningPipelineState mlPipelineState, [NullAllowed] NSError error);

	interface IMTL4Compiler { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4Compiler {
		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }

		[Abstract]
		[NullAllowed, Export ("pipelineDataSetSerializer", ArgumentSemantic.Strong)]
		IMTL4PipelineDataSetSerializer PipelineDataSetSerializer { get; }

		[Abstract]
		[Export ("newLibraryWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLLibrary CreateLibrary (MTL4LibraryDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newDynamicLibrary:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLDynamicLibrary CreateDynamicLibrary (IMTLLibrary library, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newDynamicLibraryWithURL:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLDynamicLibrary CreateDynamicLibrary (NSUrl url, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:compilerTaskOptions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (MTL4ComputePipelineDescriptor descriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:dynamicLinkingDescriptor:compilerTaskOptions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLComputePipelineState CreateComputePipelineState (MTL4ComputePipelineDescriptor descriptor, [NullAllowed] MTL4PipelineStageDynamicLinkingDescriptor dynamicLinkingDescriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:compilerTaskOptions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTL4PipelineDescriptor descriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:dynamicLinkingDescriptor:compilerTaskOptions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineState (MTL4PipelineDescriptor descriptor, [NullAllowed] MTL4RenderPipelineDynamicLinkingDescriptor dynamicLinkingDescriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newRenderPipelineStateBySpecializationWithDescriptor:pipeline:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTLRenderPipelineState CreateRenderPipelineStateBySpecialization (MTL4PipelineDescriptor descriptor, IMTLRenderPipelineState pipeline, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newBinaryFunctionWithDescriptor:compilerTaskOptions:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4BinaryFunction CreateBinaryFunction (MTL4BinaryFunctionDescriptor descriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newLibraryWithDescriptor:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateLibrary (MTL4LibraryDescriptor descriptor, MTLCreateLibraryCompletionHandler completionHandler);

		[Abstract]
		[Export ("newDynamicLibrary:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateDynamicLibrary (IMTLLibrary library, MTLCreateDynamicLibraryCompletionHandler completionHandler);

		[Abstract]
		[Export ("newDynamicLibraryWithURL:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateDynamicLibrary (NSUrl url, MTLCreateDynamicLibraryCompletionHandler completionHandler);

		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:compilerTaskOptions:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateComputePipelineState (MTL4ComputePipelineDescriptor descriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, MTLCreateComputePipelineStateCompletionHandler completionHandler);

		[Abstract]
		[Export ("newComputePipelineStateWithDescriptor:dynamicLinkingDescriptor:compilerTaskOptions:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateComputePipelineState (MTL4ComputePipelineDescriptor descriptor, [NullAllowed] MTL4PipelineStageDynamicLinkingDescriptor dynamicLinkingDescriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, MTLCreateComputePipelineStateCompletionHandler completionHandler);

		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:compilerTaskOptions:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateRenderPipelineState (MTL4PipelineDescriptor descriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, MTLCreateRenderPipelineStateCompletionHandler completionHandler);

		[Abstract]
		[Export ("newRenderPipelineStateWithDescriptor:dynamicLinkingDescriptor:compilerTaskOptions:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateRenderPipelineState (MTL4PipelineDescriptor descriptor, [NullAllowed] MTL4RenderPipelineDynamicLinkingDescriptor dynamicLinkingDescriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, MTLCreateRenderPipelineStateCompletionHandler completionHandler);

		[Abstract]
		[Export ("newRenderPipelineStateBySpecializationWithDescriptor:pipeline:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateRenderPipelineStateBySpecialization (MTL4PipelineDescriptor descriptor, IMTLRenderPipelineState pipeline, MTLCreateRenderPipelineStateCompletionHandler completionHandler);

		[Abstract]
		[Export ("newBinaryFunctionWithDescriptor:compilerTaskOptions:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateBinaryFunction (MTL4BinaryFunctionDescriptor descriptor, [NullAllowed] MTL4CompilerTaskOptions compilerTaskOptions, MTL4CreateBinaryFunctionCompletionHandler completionHandler);

		[Abstract]
		[Export ("newMachineLearningPipelineStateWithDescriptor:error:")]
		[return: NullAllowed]
		[return: Release]
		IMTL4MachineLearningPipelineState CreateMachineLearningPipelineState (MTL4MachineLearningPipelineDescriptor descriptor, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("newMachineLearningPipelineStateWithDescriptor:completionHandler:")]
		[return: Release]
		IMTL4CompilerTask CreateMachineLearningPipelineState (MTL4MachineLearningPipelineDescriptor descriptor, MTL4CreateMachineLearningPipelineStateCompletionHandler completionHandler);
	}

	interface IMTL4ComputeCommandEncoder { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4ComputeCommandEncoder : MTL4CommandEncoder {
		[Abstract]
		[Export ("stages")]
		MTLStages Stages { get; }

		[Abstract]
		[Export ("setComputePipelineState:")]
		void SetComputePipelineState (IMTLComputePipelineState state);

		[Abstract]
		[Export ("setThreadgroupMemoryLength:atIndex:")]
		void SetThreadgroupMemoryLength (nuint length, nuint index);

		[Abstract]
		[Export ("setImageblockWidth:height:")]
		void SetImageblock (nuint width, nuint height);

		[Abstract]
		[Export ("dispatchThreads:threadsPerThreadgroup:")]
		void DispatchThreads (MTLSize threadsPerGrid, MTLSize threadsPerThreadgroup);

		[Abstract]
		[Export ("dispatchThreadgroups:threadsPerThreadgroup:")]
		void DispatchThreadgroups (MTLSize threadgroupsPerGrid, MTLSize threadsPerThreadgroup);

		[Abstract]
		[Export ("dispatchThreadgroupsWithIndirectBuffer:threadsPerThreadgroup:")]
		void DispatchThreadgroups (ulong indirectBuffer, MTLSize threadsPerThreadgroup);

		[Abstract]
		[Export ("dispatchThreadsWithIndirectBuffer:")]
		void DispatchThreads (ulong indirectBuffer);

		[Abstract]
		[Export ("executeCommandsInBuffer:withRange:")]
		void ExecuteCommandsInBuffer (IMTLIndirectCommandBuffer indirectCommandBuffer, NSRange executionRange);

		[Abstract]
		[Export ("executeCommandsInBuffer:indirectBuffer:")]
		void ExecuteCommandsInBuffer (IMTLIndirectCommandBuffer indirectCommandbuffer, ulong indirectRangeBuffer);

		[Abstract]
		[Export ("copyFromTexture:toTexture:")]
		void CopyFromTexture (IMTLTexture sourceTexture, IMTLTexture destinationTexture);

		[Abstract]
		[Export ("copyFromTexture:sourceSlice:sourceLevel:toTexture:destinationSlice:destinationLevel:sliceCount:levelCount:")]
		void CopyFromTexture (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, nuint sliceCount, nuint levelCount);

		[Abstract]
		[Export ("copyFromTexture:sourceSlice:sourceLevel:sourceOrigin:sourceSize:toTexture:destinationSlice:destinationLevel:destinationOrigin:")]
		void CopyFromTexture (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, MTLOrigin sourceOrigin, MTLSize sourceSize, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, MTLOrigin destinationOrigin);

		[Abstract]
		[Export ("copyFromTexture:sourceSlice:sourceLevel:sourceOrigin:sourceSize:toBuffer:destinationOffset:destinationBytesPerRow:destinationBytesPerImage:")]
		void CopyFromTexture (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, MTLOrigin sourceOrigin, MTLSize sourceSize, IMTLBuffer destinationBuffer, nuint destinationOffset, nuint destinationBytesPerRow, nuint destinationBytesPerImage);

		[Abstract]
		[Export ("copyFromTexture:sourceSlice:sourceLevel:sourceOrigin:sourceSize:toBuffer:destinationOffset:destinationBytesPerRow:destinationBytesPerImage:options:")]
		void CopyFromTexture (IMTLTexture sourceTexture, nuint sourceSlice, nuint sourceLevel, MTLOrigin sourceOrigin, MTLSize sourceSize, IMTLBuffer destinationBuffer, nuint destinationOffset, nuint destinationBytesPerRow, nuint destinationBytesPerImage, MTLBlitOption options);

		[Abstract]
		[Export ("copyFromBuffer:sourceOffset:toBuffer:destinationOffset:size:")]
		void CopyFromBuffer (IMTLBuffer sourceBuffer, nuint sourceOffset, IMTLBuffer destinationBuffer, nuint destinationOffset, nuint size);

		[Abstract]
		[Export ("copyFromBuffer:sourceOffset:sourceBytesPerRow:sourceBytesPerImage:sourceSize:toTexture:destinationSlice:destinationLevel:destinationOrigin:")]
		void CopyFromBuffer (IMTLBuffer sourceBuffer, nuint sourceOffset, nuint sourceBytesPerRow, nuint sourceBytesPerImage, MTLSize sourceSize, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, MTLOrigin destinationOrigin);

		[Abstract]
		[Export ("copyFromBuffer:sourceOffset:sourceBytesPerRow:sourceBytesPerImage:sourceSize:toTexture:destinationSlice:destinationLevel:destinationOrigin:options:")]
		void CopyFromBuffer (IMTLBuffer sourceBuffer, nuint sourceOffset, nuint sourceBytesPerRow, nuint sourceBytesPerImage, MTLSize sourceSize, IMTLTexture destinationTexture, nuint destinationSlice, nuint destinationLevel, MTLOrigin destinationOrigin, MTLBlitOption options);

		[Abstract]
		[Export ("copyFromTensor:sourceOrigin:sourceDimensions:toTensor:destinationOrigin:destinationDimensions:")]
		void CopyFromTensor (IMTLTensor sourceTensor, MTLTensorExtents sourceOrigin, MTLTensorExtents sourceDimensions, IMTLTensor destinationTensor, MTLTensorExtents destinationOrigin, MTLTensorExtents destinationDimensions);

		[Abstract]
		[Export ("generateMipmapsForTexture:")]
		void GenerateMipmaps (IMTLTexture texture);

		[Abstract]
		[Export ("fillBuffer:range:value:")]
		void FillBuffer (IMTLBuffer buffer, NSRange range, byte value);

		[Abstract]
		[Export ("optimizeContentsForGPUAccess:")]
		void OptimizeContentsForGpuAccess (IMTLTexture texture);

		[Abstract]
		[Export ("optimizeContentsForGPUAccess:slice:level:")]
		void OptimizeContentsForGpuAccess (IMTLTexture texture, nuint slice, nuint level);

		[Abstract]
		[Export ("optimizeContentsForCPUAccess:")]
		void OptimizeContentsForCpuAccess (IMTLTexture texture);

		[Abstract]
		[Export ("optimizeContentsForCPUAccess:slice:level:")]
		void OptimizeContentsForCpuAccess (IMTLTexture texture, nuint slice, nuint level);

		[Abstract]
		[Export ("resetCommandsInBuffer:withRange:")]
		void ResetCommandsInBuffer (IMTLIndirectCommandBuffer buffer, NSRange range);

		[Abstract]
		[Export ("copyIndirectCommandBuffer:sourceRange:destination:destinationIndex:")]
		void CopyIndirectCommandBuffer (IMTLIndirectCommandBuffer source, NSRange sourceRange, IMTLIndirectCommandBuffer destination, nuint destinationIndex);

		[Abstract]
		[Export ("optimizeIndirectCommandBuffer:withRange:")]
		void OptimizeIndirectCommandBuffer (IMTLIndirectCommandBuffer indirectCommandBuffer, NSRange range);

		[Abstract]
		[Export ("setArgumentTable:")]
		void SetArgumentTable ([NullAllowed] IMTL4ArgumentTable argumentTable);

		[Abstract]
		[Export ("buildAccelerationStructure:descriptor:scratchBuffer:")]
		void BuildAccelerationStructure (IMTLAccelerationStructure accelerationStructure, MTL4AccelerationStructureDescriptor descriptor, MTL4BufferRange scratchBuffer);

		[Abstract]
		[Export ("refitAccelerationStructure:descriptor:destination:scratchBuffer:")]
		void RefitAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, MTL4AccelerationStructureDescriptor descriptor, [NullAllowed] IMTLAccelerationStructure destinationAccelerationStructure, MTL4BufferRange scratchBuffer);

		[Abstract]
		[Export ("refitAccelerationStructure:descriptor:destination:scratchBuffer:options:")]
		void RefitAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, MTL4AccelerationStructureDescriptor descriptor, [NullAllowed] IMTLAccelerationStructure destinationAccelerationStructure, MTL4BufferRange scratchBuffer, MTLAccelerationStructureRefitOptions options);

		[Abstract]
		[Export ("copyAccelerationStructure:toAccelerationStructure:")]
		void CopyAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, IMTLAccelerationStructure destinationAccelerationStructure);

		[Abstract]
		[Export ("writeCompactedAccelerationStructureSize:toBuffer:")]
		void WriteCompactedAccelerationStructureSize (IMTLAccelerationStructure accelerationStructure, MTL4BufferRange buffer);

		[Abstract]
		[Export ("copyAndCompactAccelerationStructure:toAccelerationStructure:")]
		void CopyAndCompactAccelerationStructure (IMTLAccelerationStructure sourceAccelerationStructure, IMTLAccelerationStructure destinationAccelerationStructure);

		[Abstract]
		[Export ("writeTimestampWithGranularity:intoHeap:atIndex:")]
		void WriteTimestamp (MTL4TimestampGranularity granularity, IMTL4CounterHeap counterHeap, nuint index);
	}

	interface IMTL4CounterHeap { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4CounterHeap {
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Abstract]
		[Export ("count")]
		nuint Count { get; }

		[Abstract]
		[Export ("type")]
		MTL4CounterHeapType Type { get; }

		[Abstract]
		[Export ("resolveCounterRange:")]
		[return: NullAllowed]
		NSData ResolveCounterRange (NSRange range);

		[Abstract]
		[Export ("invalidateCounterRange:")]
		void InvalidateCounterRange (NSRange range);
	}

	interface IMTL4MachineLearningCommandEncoder { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4MachineLearningCommandEncoder : MTL4CommandEncoder {
		[Abstract]
		[Export ("setPipelineState:")]
		void SetPipelineState (IMTL4MachineLearningPipelineState pipelineState);

		[Abstract]
		[Export ("setArgumentTable:")]
		void SetArgumentTable (IMTL4ArgumentTable argumentTable);

		[Abstract]
		[Export ("dispatchNetworkWithIntermediatesHeap:")]
		void DispatchNetwork (IMTLHeap intermediatesHeap);
	}

	interface IMTL4MachineLearningPipelineState { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4MachineLearningPipelineState : MTLAllocation {
		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }

		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("reflection")]
		MTL4MachineLearningPipelineReflection Reflection { get; }

		[Abstract]
		[Export ("intermediatesHeapSize")]
		nuint IntermediatesHeapSize { get; }
	}

	interface IMTL4PipelineDataSetSerializer { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4PipelineDataSetSerializer {
		[Abstract]
		[Export ("serializeAsArchiveAndFlushToURL:error:")]
		bool SerializeAsArchiveAndFlush (NSUrl url, [NullAllowed] out NSError error);

		[Abstract]
		[Export ("serializeAsPipelinesScriptWithError:")]
		[return: NullAllowed]
		NSData SerializeAsPipelinesScript ([NullAllowed] out NSError error);
	}

	interface IMTL4RenderCommandEncoder { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTL4RenderCommandEncoder : MTL4CommandEncoder {
		[Abstract]
		[Export ("tileWidth")]
		nuint TileWidth { get; }

		[Abstract]
		[Export ("tileHeight")]
		nuint TileHeight { get; }

		[Abstract]
		[Export ("setColorAttachmentMap:")]
		void SetColorAttachmentMap ([NullAllowed] MTLLogicalToPhysicalColorAttachmentMap mapping);

		[Abstract]
		[Export ("setRenderPipelineState:")]
		void SetRenderPipelineState (IMTLRenderPipelineState pipelineState);

		[Abstract]
		[Export ("setViewport:")]
		void SetViewport (MTLViewport viewport);

		[Abstract]
		[Export ("setViewports:count:")]
		void SetViewports (/* C Array: MTLViewport[] */ IntPtr viewports, nuint count);

		[Abstract]
		[Export ("setVertexAmplificationCount:viewMappings:")]
		void SetVertexAmplificationCount (nuint count, /* C Array: [NullAllowed] MTLVertexAmplificationViewMapping[] */ IntPtr viewMappings);

		[Abstract]
		[Export ("setCullMode:")]
		void SetCullMode (MTLCullMode cullMode);

		[Abstract]
		[Export ("setDepthClipMode:")]
		void SetDepthClipMode (MTLDepthClipMode depthClipMode);

		[Abstract]
		[Export ("setDepthBias:slopeScale:clamp:")]
		void SetDepthBias (float depthBias, float slopeScale, float clamp);

		[Abstract]
		[Export ("setScissorRect:")]
		void SetScissorRect (MTLScissorRect rect);

		[Abstract]
		[Export ("setScissorRects:count:")]
		void SetScissorRects (/* C Array: MTLScissorRect[] */ IntPtr scissorRects, nuint count);

		[Abstract]
		[Export ("setTriangleFillMode:")]
		void SetTriangleFillMode (MTLTriangleFillMode fillMode);

		[Abstract]
		[Export ("setBlendColorRed:green:blue:alpha:")]
		void SetBlendColorRed (float red, float green, float blue, float alpha);

		[Abstract]
		[Export ("setDepthStencilState:")]
		void SetDepthStencilState ([NullAllowed] IMTLDepthStencilState depthStencilState);

		[Abstract]
		[Export ("setStencilReferenceValue:")]
		void SetStencilReferenceValue (uint referenceValue);

		[Abstract]
		[Export ("setStencilFrontReferenceValue:backReferenceValue:")]
		void SetStencilFrontReferenceValue (uint frontReferenceValue, uint backReferenceValue);

		[Abstract]
		[Export ("setVisibilityResultMode:offset:")]
		void SetVisibilityResultMode (MTLVisibilityResultMode mode, nuint offset);

		[Abstract]
		[Export ("setColorStoreAction:atIndex:")]
		void SetColorStoreAction (MTLStoreAction storeAction, nuint colorAttachmentIndex);

		[Abstract]
		[Export ("setDepthStoreAction:")]
		void SetDepthStoreAction (MTLStoreAction storeAction);

		[Abstract]
		[Export ("setStencilStoreAction:")]
		void SetStencilStoreAction (MTLStoreAction storeAction);

		[Abstract]
		[Export ("drawPrimitives:vertexStart:vertexCount:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, nuint vertexStart, nuint vertexCount);

		[Abstract]
		[Export ("drawPrimitives:vertexStart:vertexCount:instanceCount:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, nuint vertexStart, nuint vertexCount, nuint instanceCount);

		[Abstract]
		[Export ("drawPrimitives:vertexStart:vertexCount:instanceCount:baseInstance:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, nuint vertexStart, nuint vertexCount, nuint instanceCount, nuint baseInstance);

		[Abstract]
		[Export ("drawIndexedPrimitives:indexCount:indexType:indexBuffer:indexBufferLength:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, nuint indexCount, MTLIndexType indexType, ulong indexBuffer, nuint indexBufferLength);

		[Abstract]
		[Export ("drawIndexedPrimitives:indexCount:indexType:indexBuffer:indexBufferLength:instanceCount:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, nuint indexCount, MTLIndexType indexType, ulong indexBuffer, nuint indexBufferLength, nuint instanceCount);

		[Abstract]
		[Export ("drawIndexedPrimitives:indexCount:indexType:indexBuffer:indexBufferLength:instanceCount:baseVertex:baseInstance:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, nuint indexCount, MTLIndexType indexType, ulong indexBuffer, nuint indexBufferLength, nuint instanceCount, nint baseVertex, nuint baseInstance);

		[Abstract]
		[Export ("drawPrimitives:indirectBuffer:")]
		void DrawPrimitives (MTLPrimitiveType primitiveType, ulong indirectBuffer);

		[Abstract]
		[Export ("drawIndexedPrimitives:indexType:indexBuffer:indexBufferLength:indirectBuffer:")]
		void DrawIndexedPrimitives (MTLPrimitiveType primitiveType, MTLIndexType indexType, ulong indexBuffer, nuint indexBufferLength, ulong indirectBuffer);

		[Abstract]
		[Export ("executeCommandsInBuffer:withRange:")]
		void ExecuteCommandsInBuffer (IMTLIndirectCommandBuffer indirectCommandBuffer, NSRange executionRange);

		[Abstract]
		[Export ("executeCommandsInBuffer:indirectBuffer:")]
		void ExecuteCommandsInBuffer (IMTLIndirectCommandBuffer indirectCommandBuffer, ulong indirectRangeBuffer);

		[Abstract]
		[Export ("setObjectThreadgroupMemoryLength:atIndex:")]
		void SetObjectThreadgroupMemoryLength (nuint length, nuint index);

		[Abstract]
		[Export ("drawMeshThreadgroups:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreadgroups (MTLSize threadgroupsPerGrid, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Abstract]
		[Export ("drawMeshThreads:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreads (MTLSize threadsPerGrid, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Abstract]
		[Export ("drawMeshThreadgroupsWithIndirectBuffer:threadsPerObjectThreadgroup:threadsPerMeshThreadgroup:")]
		void DrawMeshThreadgroups (ulong indirectBuffer, MTLSize threadsPerObjectThreadgroup, MTLSize threadsPerMeshThreadgroup);

		[Abstract]
		[Export ("dispatchThreadsPerTile:")]
		void DispatchThreadsPerTile (MTLSize threadsPerTile);

		[Abstract]
		[Export ("setThreadgroupMemoryLength:offset:atIndex:")]
		void SetThreadgroupMemoryLength (nuint length, nuint offset, nuint index);

		[Abstract]
		[Export ("setArgumentTable:atStages:")]
		void SetArgumentTable (IMTL4ArgumentTable argumentTable, MTLRenderStages stages);

		[Abstract]
		[Export ("setFrontFacingWinding:")]
		void SetFrontFacingWinding (MTLWinding frontFacingWinding);

		[Abstract]
		[Export ("writeTimestampWithGranularity:afterStage:intoHeap:atIndex:")]
		void WriteTimestamp (MTL4TimestampGranularity granularity, MTLRenderStages stage, IMTL4CounterHeap counterHeap, nuint index);
	}

	interface IMTLResourceViewPool { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLResourceViewPool {
		[Abstract]
		[Export ("baseResourceID")]
		MTLResourceId BaseResourceId { get; }

		[Abstract]
		[Export ("resourceViewCount")]
		nuint ResourceViewCount { get; }

		[Abstract]
		[Export ("device")]
		IMTLDevice Device { get; }

		[Abstract]
		[NullAllowed, Export ("label")]
		string Label { get; }

		[Abstract]
		[Export ("copyResourceViewsFromPool:sourceRange:destinationIndex:")]
		MTLResourceId CopyResourceViews (IMTLResourceViewPool sourcePool, NSRange sourceRange, nuint destinationIndex);
	}

	interface IMTLTensor { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLTensor : MTLResource {
		[Abstract]
		[Export ("gpuResourceID")]
		MTLResourceId GpuResourceId { get; }

		[Abstract]
		[NullAllowed, Export ("buffer")]
		IMTLBuffer Buffer { get; }

		[Abstract]
		[Export ("bufferOffset")]
		nuint BufferOffset { get; }

		[Abstract]
		[Export ("strides")]
		[NullAllowed]
		MTLTensorExtents Strides { get; }

		[Abstract]
		[Export ("dimensions")]
		MTLTensorExtents Dimensions { get; }

		[Abstract]
		[Export ("dataType")]
		MTLTensorDataType DataType { get; }

		[Abstract]
		[Export ("usage")]
		MTLTensorUsage Usage { get; }

		[Abstract]
		[Export ("replaceSliceOrigin:sliceDimensions:withBytes:strides:")]
		void ReplaceSliceOrigin (MTLTensorExtents sliceOrigin, MTLTensorExtents sliceDimensions, /* const void * _Nonnull */ IntPtr bytes, MTLTensorExtents strides);

		[Abstract]
		[Export ("getBytes:strides:fromSliceOrigin:sliceDimensions:")]
		void GetBytes (IntPtr /* void * _Nonnull */ bytes, MTLTensorExtents strides, MTLTensorExtents sliceOrigin, MTLTensorExtents sliceDimensions);
	}

	interface IMTLTensorBinding { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLTensorBinding : MTLBinding {
		[Abstract]
		[Export ("tensorDataType")]
		MTLTensorDataType TensorDataType { get; }

		[Abstract]
		[Export ("indexType")]
		MTLDataType IndexType { get; }

		[Abstract]
		[NullAllowed, Export ("dimensions")]
		MTLTensorExtents Dimensions { get; }
	}

	interface IMTLTextureViewPool { }

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface MTLTextureViewPool : MTLResourceViewPool {
		[Abstract]
		[Export ("setTextureView:atIndex:")]
		MTLResourceId SetTextureView (IMTLTexture texture, nuint index);

		[Abstract]
		[Export ("setTextureView:descriptor:atIndex:")]
		MTLResourceId SetTextureView (IMTLTexture texture, MTLTextureViewDescriptor descriptor, nuint index);

		[Abstract]
		[Export ("setTextureViewFromBuffer:descriptor:offset:bytesPerRow:atIndex:")]
		MTLResourceId SetTextureViewFromBuffer (IMTLBuffer buffer, MTLTextureDescriptor descriptor, nuint offset, nuint bytesPerRow, nuint index);
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureGeometryDescriptor))]
	interface MTL4AccelerationStructureBoundingBoxGeometryDescriptor {
		[Export ("boundingBoxBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange BoundingBoxBuffer { get; set; }

		[Export ("boundingBoxStride")]
		nuint BoundingBoxStride { get; set; }

		[Export ("boundingBoxCount")]
		nuint BoundingBoxCount { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureGeometryDescriptor))]
	interface MTL4AccelerationStructureCurveGeometryDescriptor {
		[Export ("controlPointBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange ControlPointBuffer { get; set; }

		[Export ("controlPointCount")]
		nuint ControlPointCount { get; set; }

		[Export ("controlPointStride")]
		nuint ControlPointStride { get; set; }

		[Export ("controlPointFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat ControlPointFormat { get; set; }

		[Export ("radiusBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange RadiusBuffer { get; set; }

		[Export ("radiusFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat RadiusFormat { get; set; }

		[Export ("radiusStride")]
		nuint RadiusStride { get; set; }

		[Export ("indexBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange IndexBuffer { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("segmentCount")]
		nuint SegmentCount { get; set; }

		[Export ("segmentControlPointCount")]
		nuint SegmentControlPointCount { get; set; }

		[Export ("curveType", ArgumentSemantic.Assign)]
		MTLCurveType CurveType { get; set; }

		[Export ("curveBasis", ArgumentSemantic.Assign)]
		MTLCurveBasis CurveBasis { get; set; }

		[Export ("curveEndCaps", ArgumentSemantic.Assign)]
		MTLCurveEndCaps CurveEndCaps { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTLAccelerationStructureDescriptor))]
	interface MTL4AccelerationStructureDescriptor {
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4AccelerationStructureGeometryDescriptor : NSCopying {
		[Export ("intersectionFunctionTableOffset")]
		nuint IntersectionFunctionTableOffset { get; set; }

		[Export ("opaque")]
		bool Opaque { get; set; }

		[Export ("allowDuplicateIntersectionFunctionInvocation")]
		bool AllowDuplicateIntersectionFunctionInvocation { get; set; }

		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Export ("primitiveDataBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange PrimitiveDataBuffer { get; set; }

		[Export ("primitiveDataStride")]
		nuint PrimitiveDataStride { get; set; }

		[Export ("primitiveDataElementSize")]
		nuint PrimitiveDataElementSize { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureGeometryDescriptor))]
	interface MTL4AccelerationStructureMotionBoundingBoxGeometryDescriptor {
		[Export ("boundingBoxBuffers", ArgumentSemantic.Assign)]
		MTL4BufferRange BoundingBoxBuffers { get; set; }

		[Export ("boundingBoxStride")]
		nuint BoundingBoxStride { get; set; }

		[Export ("boundingBoxCount")]
		nuint BoundingBoxCount { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureGeometryDescriptor))]
	interface MTL4AccelerationStructureMotionCurveGeometryDescriptor {
		[Export ("controlPointBuffers", ArgumentSemantic.Assign)]
		MTL4BufferRange ControlPointBuffers { get; set; }

		[Export ("controlPointCount")]
		nuint ControlPointCount { get; set; }

		[Export ("controlPointStride")]
		nuint ControlPointStride { get; set; }

		[Export ("controlPointFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat ControlPointFormat { get; set; }

		[Export ("radiusBuffers", ArgumentSemantic.Assign)]
		MTL4BufferRange RadiusBuffers { get; set; }

		[Export ("radiusFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat RadiusFormat { get; set; }

		[Export ("radiusStride")]
		nuint RadiusStride { get; set; }

		[Export ("indexBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange IndexBuffer { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("segmentCount")]
		nuint SegmentCount { get; set; }

		[Export ("segmentControlPointCount")]
		nuint SegmentControlPointCount { get; set; }

		[Export ("curveType", ArgumentSemantic.Assign)]
		MTLCurveType CurveType { get; set; }

		[Export ("curveBasis", ArgumentSemantic.Assign)]
		MTLCurveBasis CurveBasis { get; set; }

		[Export ("curveEndCaps", ArgumentSemantic.Assign)]
		MTLCurveEndCaps CurveEndCaps { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureGeometryDescriptor))]
	interface MTL4AccelerationStructureMotionTriangleGeometryDescriptor {
		[Export ("vertexBuffers", ArgumentSemantic.Assign)]
		MTL4BufferRange VertexBuffers { get; set; }

		[Export ("vertexFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat VertexFormat { get; set; }

		[Export ("vertexStride")]
		nuint VertexStride { get; set; }

		[Export ("indexBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange IndexBuffer { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("triangleCount")]
		nuint TriangleCount { get; set; }

		[Export ("transformationMatrixBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange TransformationMatrixBuffer { get; set; }

		[Export ("transformationMatrixLayout", ArgumentSemantic.Assign)]
		MTLMatrixLayout TransformationMatrixLayout { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureGeometryDescriptor))]
	interface MTL4AccelerationStructureTriangleGeometryDescriptor {
		[Export ("vertexBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange VertexBuffer { get; set; }

		[Export ("vertexFormat", ArgumentSemantic.Assign)]
		MTLAttributeFormat VertexFormat { get; set; }

		[Export ("vertexStride")]
		nuint VertexStride { get; set; }

		[Export ("indexBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange IndexBuffer { get; set; }

		[Export ("indexType", ArgumentSemantic.Assign)]
		MTLIndexType IndexType { get; set; }

		[Export ("triangleCount")]
		nuint TriangleCount { get; set; }

		[Export ("transformationMatrixBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange TransformationMatrixBuffer { get; set; }

		[Export ("transformationMatrixLayout", ArgumentSemantic.Assign)]
		MTLMatrixLayout TransformationMatrixLayout { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4ArgumentTableDescriptor : NSCopying {
		[Export ("maxBufferBindCount")]
		nuint MaxBufferBindCount { get; set; }

		[Export ("maxTextureBindCount")]
		nuint MaxTextureBindCount { get; set; }

		[Export ("maxSamplerStateBindCount")]
		nuint MaxSamplerStateBindCount { get; set; }

		[Export ("initializeBindings")]
		bool InitializeBindings { get; set; }

		[Export ("supportAttributeStrides")]
		bool SupportAttributeStrides { get; set; }

		[NullAllowed, Export ("label")]
		string Label { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4BinaryFunctionDescriptor : NSCopying {
		[Export ("name")]
		string Name { get; set; }

		[Export ("functionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor FunctionDescriptor { get; set; }

		[Export ("options", ArgumentSemantic.Assign)]
		MTL4BinaryFunctionOptions Options { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4CommandAllocatorDescriptor : NSCopying {
		[NullAllowed, Export ("label")]
		string Label { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4CommandBufferOptions : NSCopying {
		[NullAllowed, Export ("logState", ArgumentSemantic.Retain)]
		IMTLLogState LogState { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4CommandQueueDescriptor : NSCopying {
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[NullAllowed, Export ("feedbackQueue", ArgumentSemantic.Assign)]
		DispatchQueue FeedbackQueue { get; set; }
	}

	delegate void MTL4CommitFeedbackHandler (IMTL4CommitFeedback commitFeedback);

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4CommitOptions {
		[Export ("addFeedbackHandler:")]
		void AddFeedbackHandler (MTL4CommitFeedbackHandler block);
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4CompilerDescriptor : NSCopying {
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[NullAllowed, Export ("pipelineDataSetSerializer", ArgumentSemantic.Strong)]
		IMTL4PipelineDataSetSerializer PipelineDataSetSerializer { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4CompilerTaskOptions : NSCopying {
		[NullAllowed, Export ("lookupArchives", ArgumentSemantic.Copy)]
		IMTL4Archive [] LookupArchives { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4PipelineDescriptor))]
	interface MTL4ComputePipelineDescriptor {
		[NullAllowed, Export ("computeFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor ComputeFunctionDescriptor { get; set; }

		[Export ("threadGroupSizeIsMultipleOfThreadExecutionWidth")]
		bool ThreadGroupSizeIsMultipleOfThreadExecutionWidth { get; set; }

		[Export ("maxTotalThreadsPerThreadgroup")]
		nuint MaxTotalThreadsPerThreadgroup { get; set; }

		[Export ("requiredThreadsPerThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerThreadgroup { get; set; }

		[Export ("supportBinaryLinking")]
		bool SupportBinaryLinking { get; set; }

		[NullAllowed, Export ("staticLinkingDescriptor", ArgumentSemantic.Copy)]
		MTL4StaticLinkingDescriptor StaticLinkingDescriptor { get; set; }

		[Export ("supportIndirectCommandBuffers", ArgumentSemantic.Assign)]
		MTL4IndirectCommandBufferSupportState SupportIndirectCommandBuffers { get; set; }

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4CounterHeapDescriptor : NSCopying {
		[Export ("type", ArgumentSemantic.Assign)]
		MTL4CounterHeapType Type { get; set; }

		[Export ("count")]
		nuint Count { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4FunctionDescriptor : NSCopying {
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureDescriptor))]
	interface MTL4IndirectInstanceAccelerationStructureDescriptor {
		[Export ("instanceDescriptorBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange InstanceDescriptorBuffer { get; set; }

		[Export ("instanceDescriptorStride")]
		nuint InstanceDescriptorStride { get; set; }

		[Export ("maxInstanceCount")]
		nuint MaxInstanceCount { get; set; }

		[Export ("instanceCountBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange InstanceCountBuffer { get; set; }

		[Export ("instanceDescriptorType", ArgumentSemantic.Assign)]
		MTLAccelerationStructureInstanceDescriptorType InstanceDescriptorType { get; set; }

		[Export ("motionTransformBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange MotionTransformBuffer { get; set; }

		[Export ("maxMotionTransformCount")]
		nuint MaxMotionTransformCount { get; set; }

		[Export ("motionTransformCountBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange MotionTransformCountBuffer { get; set; }

		[Export ("instanceTransformationMatrixLayout", ArgumentSemantic.Assign)]
		MTLMatrixLayout InstanceTransformationMatrixLayout { get; set; }

		[Export ("motionTransformType", ArgumentSemantic.Assign)]
		MTLTransformType MotionTransformType { get; set; }

		[Export ("motionTransformStride")]
		nuint MotionTransformStride { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureDescriptor))]
	interface MTL4InstanceAccelerationStructureDescriptor {
		[Export ("instanceDescriptorBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange InstanceDescriptorBuffer { get; set; }

		[Export ("instanceDescriptorStride")]
		nuint InstanceDescriptorStride { get; set; }

		[Export ("instanceCount")]
		nuint InstanceCount { get; set; }

		[Export ("instanceDescriptorType", ArgumentSemantic.Assign)]
		MTLAccelerationStructureInstanceDescriptorType InstanceDescriptorType { get; set; }

		[Export ("motionTransformBuffer", ArgumentSemantic.Assign)]
		MTL4BufferRange MotionTransformBuffer { get; set; }

		[Export ("motionTransformCount")]
		nuint MotionTransformCount { get; set; }

		[Export ("instanceTransformationMatrixLayout", ArgumentSemantic.Assign)]
		MTLMatrixLayout InstanceTransformationMatrixLayout { get; set; }

		[Export ("motionTransformType", ArgumentSemantic.Assign)]
		MTLTransformType MotionTransformType { get; set; }

		[Export ("motionTransformStride")]
		nuint MotionTransformStride { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4LibraryDescriptor : NSCopying {
		[NullAllowed, Export ("source")]
		string Source { get; set; }

		[NullAllowed, Export ("options", ArgumentSemantic.Copy)]
		MTLCompileOptions Options { get; set; }

		[NullAllowed, Export ("name")]
		string Name { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4FunctionDescriptor))]
	interface MTL4LibraryFunctionDescriptor {
		[NullAllowed, Export ("name")]
		string Name { get; set; }

		[NullAllowed, Export ("library", ArgumentSemantic.Retain)]
		IMTLLibrary Library { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4PipelineDescriptor))]
	interface MTL4MachineLearningPipelineDescriptor {
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[NullAllowed, Export ("machineLearningFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor MachineLearningFunctionDescriptor { get; set; }

		[Export ("setInputDimensions:atBufferIndex:")]
		void SetInputDimensions ([NullAllowed] MTLTensorExtents dimensions, nint bufferIndex);

		[Export ("setInputDimensions:withRange:")]
		void SetInputDimensions (MTLTensorExtents [] dimensions, NSRange range);

		[Export ("inputDimensionsAtBufferIndex:")]
		[return: NullAllowed]
		MTLTensorExtents GetInputDimensions (nint bufferIndex);

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4MachineLearningPipelineReflection {
		[Export ("bindings")]
		IMTLBinding [] Bindings { get; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4PipelineDescriptor))]
	interface MTL4MeshRenderPipelineDescriptor {
		[NullAllowed, Export ("objectFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor ObjectFunctionDescriptor { get; set; }

		[NullAllowed, Export ("meshFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor MeshFunctionDescriptor { get; set; }

		[NullAllowed, Export ("fragmentFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor FragmentFunctionDescriptor { get; set; }

		[Export ("maxTotalThreadsPerObjectThreadgroup")]
		nuint MaxTotalThreadsPerObjectThreadgroup { get; set; }

		[Export ("maxTotalThreadsPerMeshThreadgroup")]
		nuint MaxTotalThreadsPerMeshThreadgroup { get; set; }

		[Export ("requiredThreadsPerObjectThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerObjectThreadgroup { get; set; }

		[Export ("requiredThreadsPerMeshThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerMeshThreadgroup { get; set; }

		[Export ("objectThreadgroupSizeIsMultipleOfThreadExecutionWidth")]
		bool ObjectThreadgroupSizeIsMultipleOfThreadExecutionWidth { get; set; }

		[Export ("meshThreadgroupSizeIsMultipleOfThreadExecutionWidth")]
		bool MeshThreadgroupSizeIsMultipleOfThreadExecutionWidth { get; set; }

		[Export ("payloadMemoryLength")]
		nuint PayloadMemoryLength { get; set; }

		[Export ("maxTotalThreadgroupsPerMeshGrid")]
		nuint MaxTotalThreadgroupsPerMeshGrid { get; set; }

		[Export ("rasterSampleCount")]
		nuint RasterSampleCount { get; set; }

		[Export ("alphaToCoverageState", ArgumentSemantic.Assign)]
		MTL4AlphaToCoverageState AlphaToCoverageState { get; set; }

		[Export ("alphaToOneState", ArgumentSemantic.Assign)]
		MTL4AlphaToOneState AlphaToOneState { get; set; }

		[Export ("rasterizationEnabled")]
		bool RasterizationEnabled { [Bind ("isRasterizationEnabled")] get; set; }

		[Export ("maxVertexAmplificationCount")]
		nuint MaxVertexAmplificationCount { get; set; }

		[Export ("colorAttachments")]
		MTL4RenderPipelineColorAttachmentDescriptorArray ColorAttachments { get; }

		[NullAllowed, Export ("objectStaticLinkingDescriptor", ArgumentSemantic.Copy)]
		MTL4StaticLinkingDescriptor ObjectStaticLinkingDescriptor { get; set; }

		[NullAllowed, Export ("meshStaticLinkingDescriptor", ArgumentSemantic.Copy)]
		MTL4StaticLinkingDescriptor MeshStaticLinkingDescriptor { get; set; }

		[NullAllowed, Export ("fragmentStaticLinkingDescriptor", ArgumentSemantic.Copy)]
		MTL4StaticLinkingDescriptor FragmentStaticLinkingDescriptor { get; set; }

		[Export ("supportObjectBinaryLinking")]
		bool SupportObjectBinaryLinking { get; set; }

		[Export ("supportMeshBinaryLinking")]
		bool SupportMeshBinaryLinking { get; set; }

		[Export ("supportFragmentBinaryLinking")]
		bool SupportFragmentBinaryLinking { get; set; }

		[Export ("colorAttachmentMappingState", ArgumentSemantic.Assign)]
		MTL4LogicalToPhysicalColorAttachmentMappingState ColorAttachmentMappingState { get; set; }

		[Export ("supportIndirectCommandBuffers", ArgumentSemantic.Assign)]
		MTL4IndirectCommandBufferSupportState SupportIndirectCommandBuffers { get; set; }

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4PipelineDataSetSerializerDescriptor : NSCopying {
		[Export ("configuration", ArgumentSemantic.Assign)]
		MTL4PipelineDataSetSerializerConfiguration Configuration { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4PipelineDescriptor : NSCopying {
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		[Export ("options", ArgumentSemantic.Retain)]
		[NullAllowed]
		MTL4PipelineOptions Options { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4PipelineOptions : NSCopying {
		[Export ("shaderValidation", ArgumentSemantic.Assign)]
		MTLShaderValidation ShaderValidation { get; set; }

		[Export ("shaderReflection", ArgumentSemantic.Assign)]
		MTL4ShaderReflection ShaderReflection { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4PipelineStageDynamicLinkingDescriptor : NSCopying {
		[Export ("maxCallStackDepth")]
		nuint MaxCallStackDepth { get; set; }

		[NullAllowed, Export ("binaryLinkedFunctions", ArgumentSemantic.Copy)]
		IMTL4BinaryFunction [] BinaryLinkedFunctions { get; set; }

		[Export ("preloadedLibraries", ArgumentSemantic.Copy)]
		IMTLDynamicLibrary [] PreloadedLibraries { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4AccelerationStructureDescriptor))]
	interface MTL4PrimitiveAccelerationStructureDescriptor {
		[NullAllowed, Export ("geometryDescriptors", ArgumentSemantic.Retain)]
		MTL4AccelerationStructureGeometryDescriptor [] GeometryDescriptors { get; set; }

		[Export ("motionStartBorderMode", ArgumentSemantic.Assign)]
		MTLMotionBorderMode MotionStartBorderMode { get; set; }

		[Export ("motionEndBorderMode", ArgumentSemantic.Assign)]
		MTLMotionBorderMode MotionEndBorderMode { get; set; }

		[Export ("motionStartTime")]
		float MotionStartTime { get; set; }

		[Export ("motionEndTime")]
		float MotionEndTime { get; set; }

		[Export ("motionKeyframeCount")]
		nuint MotionKeyframeCount { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4RenderPassDescriptor : NSCopying {
		[Export ("colorAttachments")]
		MTLRenderPassColorAttachmentDescriptorArray ColorAttachments { get; }

		[NullAllowed, Export ("depthAttachment", ArgumentSemantic.Copy)]
		MTLRenderPassDepthAttachmentDescriptor DepthAttachment { get; set; }

		[NullAllowed, Export ("stencilAttachment", ArgumentSemantic.Copy)]
		MTLRenderPassStencilAttachmentDescriptor StencilAttachment { get; set; }

		[Export ("renderTargetArrayLength")]
		nuint RenderTargetArrayLength { get; set; }

		[Export ("imageblockSampleLength")]
		nuint ImageblockSampleLength { get; set; }

		[Export ("threadgroupMemoryLength")]
		nuint ThreadgroupMemoryLength { get; set; }

		[Export ("tileWidth")]
		nuint TileWidth { get; set; }

		[Export ("tileHeight")]
		nuint TileHeight { get; set; }

		[Export ("defaultRasterSampleCount")]
		nuint DefaultRasterSampleCount { get; set; }

		[Export ("renderTargetWidth")]
		nuint RenderTargetWidth { get; set; }

		[Export ("renderTargetHeight")]
		nuint RenderTargetHeight { get; set; }

		[NullAllowed, Export ("rasterizationRateMap", ArgumentSemantic.Strong)]
		IMTLRasterizationRateMap RasterizationRateMap { get; set; }

		[NullAllowed, Export ("visibilityResultBuffer", ArgumentSemantic.Strong)]
		IMTLBuffer VisibilityResultBuffer { get; set; }

		[Export ("visibilityResultType", ArgumentSemantic.Assign)]
		MTLVisibilityResultType VisibilityResultType { get; set; }

		[Export ("setSamplePositions:count:")]
		void SetSamplePositions (/* C Array: [NullAllowed] MTLSamplePosition [] */ IntPtr positions, nuint count);

		[Export ("getSamplePositions:count:")]
		nuint GetSamplePositions (/* C Array: [NullAllowed] MTLSamplePosition [] */ IntPtr positions, nuint count);

		[Export ("supportColorAttachmentMapping")]
		bool SupportColorAttachmentMapping { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4RenderPipelineBinaryFunctionsDescriptor : NSCopying {
		[NullAllowed, Export ("vertexAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTL4BinaryFunction [] VertexAdditionalBinaryFunctions { get; set; }

		[NullAllowed, Export ("fragmentAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTL4BinaryFunction [] FragmentAdditionalBinaryFunctions { get; set; }

		[NullAllowed, Export ("tileAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTL4BinaryFunction [] TileAdditionalBinaryFunctions { get; set; }

		[NullAllowed, Export ("objectAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTL4BinaryFunction [] ObjectAdditionalBinaryFunctions { get; set; }

		[NullAllowed, Export ("meshAdditionalBinaryFunctions", ArgumentSemantic.Copy)]
		IMTL4BinaryFunction [] MeshAdditionalBinaryFunctions { get; set; }

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4RenderPipelineColorAttachmentDescriptor : NSCopying {
		[Export ("pixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat PixelFormat { get; set; }

		[Export ("blendingState", ArgumentSemantic.Assign)]
		MTL4BlendState BlendingState { get; set; }

		[Export ("sourceRGBBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor SourceRgbBlendFactor { get; set; }

		[Export ("destinationRGBBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor DestinationRgbBlendFactor { get; set; }

		[Export ("rgbBlendOperation", ArgumentSemantic.Assign)]
		MTLBlendOperation RgbBlendOperation { get; set; }

		[Export ("sourceAlphaBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor SourceAlphaBlendFactor { get; set; }

		[Export ("destinationAlphaBlendFactor", ArgumentSemantic.Assign)]
		MTLBlendFactor DestinationAlphaBlendFactor { get; set; }

		[Export ("alphaBlendOperation", ArgumentSemantic.Assign)]
		MTLBlendOperation AlphaBlendOperation { get; set; }

		[Export ("writeMask", ArgumentSemantic.Assign)]
		MTLColorWriteMask WriteMask { get; set; }

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4RenderPipelineColorAttachmentDescriptorArray : NSCopying {
		[Export ("objectAtIndexedSubscript:")]
		MTL4RenderPipelineColorAttachmentDescriptor GetObject (nuint attachmentIndex);

		[Export ("setObject:atIndexedSubscript:")]
		void SetObject ([NullAllowed] MTL4RenderPipelineColorAttachmentDescriptor attachment, nuint attachmentIndex);

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4PipelineDescriptor))]
	interface MTL4RenderPipelineDescriptor {
		[NullAllowed, Export ("vertexFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor VertexFunctionDescriptor { get; set; }

		[NullAllowed, Export ("fragmentFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor FragmentFunctionDescriptor { get; set; }

		[NullAllowed, Export ("vertexDescriptor", ArgumentSemantic.Copy)]
		MTLVertexDescriptor VertexDescriptor { get; set; }

		[Export ("rasterSampleCount")]
		nuint RasterSampleCount { get; set; }

		[Export ("alphaToCoverageState", ArgumentSemantic.Assign)]
		MTL4AlphaToCoverageState AlphaToCoverageState { get; set; }

		[Export ("alphaToOneState", ArgumentSemantic.Assign)]
		MTL4AlphaToOneState AlphaToOneState { get; set; }

		[Export ("rasterizationEnabled")]
		bool RasterizationEnabled { [Bind ("isRasterizationEnabled")] get; set; }

		[Export ("maxVertexAmplificationCount")]
		nuint MaxVertexAmplificationCount { get; set; }

		[Export ("colorAttachments")]
		MTL4RenderPipelineColorAttachmentDescriptorArray ColorAttachments { get; }

		[Export ("inputPrimitiveTopology", ArgumentSemantic.Assign)]
		MTLPrimitiveTopologyClass InputPrimitiveTopology { get; set; }

		[NullAllowed, Export ("vertexStaticLinkingDescriptor", ArgumentSemantic.Copy)]
		MTL4StaticLinkingDescriptor VertexStaticLinkingDescriptor { get; set; }

		[NullAllowed, Export ("fragmentStaticLinkingDescriptor", ArgumentSemantic.Copy)]
		MTL4StaticLinkingDescriptor FragmentStaticLinkingDescriptor { get; set; }

		[Export ("supportVertexBinaryLinking")]
		bool SupportVertexBinaryLinking { get; set; }

		[Export ("supportFragmentBinaryLinking")]
		bool SupportFragmentBinaryLinking { get; set; }

		[Export ("colorAttachmentMappingState", ArgumentSemantic.Assign)]
		MTL4LogicalToPhysicalColorAttachmentMappingState ColorAttachmentMappingState { get; set; }

		[Export ("supportIndirectCommandBuffers", ArgumentSemantic.Assign)]
		MTL4IndirectCommandBufferSupportState SupportIndirectCommandBuffers { get; set; }

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4RenderPipelineDynamicLinkingDescriptor : NSCopying {
		[Export ("vertexLinkingDescriptor")]
		MTL4PipelineStageDynamicLinkingDescriptor VertexLinkingDescriptor { get; }

		[Export ("fragmentLinkingDescriptor")]
		MTL4PipelineStageDynamicLinkingDescriptor FragmentLinkingDescriptor { get; }

		[Export ("tileLinkingDescriptor")]
		MTL4PipelineStageDynamicLinkingDescriptor TileLinkingDescriptor { get; }

		[Export ("objectLinkingDescriptor")]
		MTL4PipelineStageDynamicLinkingDescriptor ObjectLinkingDescriptor { get; }

		[Export ("meshLinkingDescriptor")]
		MTL4PipelineStageDynamicLinkingDescriptor MeshLinkingDescriptor { get; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4FunctionDescriptor))]
	interface MTL4SpecializedFunctionDescriptor {
		[NullAllowed, Export ("functionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor FunctionDescriptor { get; set; }

		[NullAllowed, Export ("specializedName")]
		string SpecializedName { get; set; }

		[NullAllowed, Export ("constantValues", ArgumentSemantic.Copy)]
		MTLFunctionConstantValues ConstantValues { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTL4StaticLinkingDescriptor : NSCopying {
		[NullAllowed, Export ("functionDescriptors", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor [] FunctionDescriptors { get; set; }

		[NullAllowed, Export ("privateFunctionDescriptors", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor [] PrivateFunctionDescriptors { get; set; }

		[NullAllowed, Export ("groups", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<MTL4FunctionDescriptor>> Groups { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4FunctionDescriptor))]
	interface MTL4StitchedFunctionDescriptor {
		[NullAllowed, Export ("functionGraph", ArgumentSemantic.Copy)]
		MTLFunctionStitchingGraph FunctionGraph { get; set; }

		[NullAllowed, Export ("functionDescriptors", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor [] FunctionDescriptors { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTL4PipelineDescriptor))]
	interface MTL4TileRenderPipelineDescriptor {
		[NullAllowed, Export ("tileFunctionDescriptor", ArgumentSemantic.Copy)]
		MTL4FunctionDescriptor TileFunctionDescriptor { get; set; }

		[Export ("rasterSampleCount")]
		nuint RasterSampleCount { get; set; }

		[Export ("colorAttachments")]
		MTLTileRenderPipelineColorAttachmentDescriptorArray ColorAttachments { get; }

		[Export ("threadgroupSizeMatchesTileSize")]
		bool ThreadgroupSizeMatchesTileSize { get; set; }

		[Export ("maxTotalThreadsPerThreadgroup")]
		nuint MaxTotalThreadsPerThreadgroup { get; set; }

		[Export ("requiredThreadsPerThreadgroup", ArgumentSemantic.Assign)]
		MTLSize RequiredThreadsPerThreadgroup { get; set; }

		[NullAllowed, Export ("staticLinkingDescriptor", ArgumentSemantic.Copy)]
		MTL4StaticLinkingDescriptor StaticLinkingDescriptor { get; set; }

		[Export ("supportBinaryLinking")]
		bool SupportBinaryLinking { get; set; }

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLFunctionReflection {
		[Export ("bindings")]
		IMTLBinding [] Bindings { get; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLLogicalToPhysicalColorAttachmentMap : NSCopying {
		[Export ("setPhysicalIndex:forLogicalIndex:")]
		void SetPhysicalIndex (nuint physicalIndex, nuint logicalIndex);

		[Export ("getPhysicalIndexForLogicalIndex:")]
		nuint GetPhysicalIndex (nuint logicalIndex);

		[Export ("reset")]
		void Reset ();
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLResourceViewPoolDescriptor : NSCopying {
		[Export ("resourceViewCount")]
		nuint ResourceViewCount { get; set; }

		[NullAllowed, Export ("label")]
		string Label { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLTensorDescriptor : NSCopying {
		[Export ("dimensions", ArgumentSemantic.Copy)]
		MTLTensorExtents Dimensions { get; set; }

		[Export ("strides", ArgumentSemantic.Copy)]
		[NullAllowed]
		MTLTensorExtents Strides { get; set; }

		[Export ("dataType", ArgumentSemantic.Assign)]
		MTLTensorDataType DataType { get; set; }

		[Export ("usage", ArgumentSemantic.Assign)]
		MTLTensorUsage Usage { get; set; }

		[Export ("resourceOptions", ArgumentSemantic.Assign)]
		MTLResourceOptions ResourceOptions { get; set; }

		[Export ("cpuCacheMode", ArgumentSemantic.Assign)]
		MTLCpuCacheMode CpuCacheMode { get; set; }

		[Export ("storageMode", ArgumentSemantic.Assign)]
		MTLStorageMode StorageMode { get; set; }

		[Export ("hazardTrackingMode", ArgumentSemantic.Assign)]
		MTLHazardTrackingMode HazardTrackingMode { get; set; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // all properties are readonly, and has a non-default ctor
	interface MTLTensorExtents {
		[Internal]
		[Export ("initWithRank:values:")]
		NativeHandle _InitWithRank (nuint rank, /* C Array: [NullAllowed] nint[] */ IntPtr values);

		[Export ("rank")]
		nuint Rank { get; }

		[Export ("extentAtDimensionIndex:")]
		nint GetExtent (nuint dimensionIndex);
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (MTLType))]
	interface MTLTensorReferenceType {
		[Export ("tensorDataType")]
		MTLTensorDataType TensorDataType { get; }

		[Export ("indexType")]
		MTLDataType IndexType { get; }

		[NullAllowed, Export ("dimensions")]
		MTLTensorExtents Dimensions { get; }

		[Export ("access")]
		MTLBindingAccess Access { get; }
	}

	[Mac (26, 0), iOS (26, 0), TV (26, 0), MacCatalyst (26, 0)]
	[BaseType (typeof (NSObject))]
	interface MTLTextureViewDescriptor : NSCopying {
		[Export ("pixelFormat", ArgumentSemantic.Assign)]
		MTLPixelFormat PixelFormat { get; set; }

		[Export ("textureType", ArgumentSemantic.Assign)]
		MTLTextureType TextureType { get; set; }

		[Export ("levelRange", ArgumentSemantic.Assign)]
		NSRange LevelRange { get; set; }

		[Export ("sliceRange", ArgumentSemantic.Assign)]
		NSRange SliceRange { get; set; }

		[Export ("swizzle", ArgumentSemantic.Assign)]
		MTLTextureSwizzleChannels Swizzle { get; set; }
	}
}
