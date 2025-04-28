using System;
using System.Runtime.InteropServices;

using Foundation;
using ModelIO;
using ObjCRuntime;
using Metal;

#nullable enable

namespace ModelIO {
	public partial class MDLVertexDescriptor {
		[DllImport (Constants.MetalKitLibrary)]
		static extern  /* MDLVertexDescriptor __nonnull */ IntPtr MTKModelIOVertexDescriptorFromMetal (/* MTLVertexDescriptor __nonnull */ IntPtr mtlDescriptor);

		/// <param name="descriptor">To be added.</param>
		///         <summary>Creates a new vertex descriptor from a Metal vertex descriptor.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static MDLVertexDescriptor? FromMetal (MTLVertexDescriptor descriptor)
		{
			if (descriptor is null)
				throw new ArgumentException (nameof (descriptor));
			MDLVertexDescriptor? result = Runtime.GetNSObject<MDLVertexDescriptor> (MTKModelIOVertexDescriptorFromMetal (descriptor.Handle));
			GC.KeepAlive (descriptor);
			return result;
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		[DllImport (Constants.MetalKitLibrary)]
		unsafe static extern /* MDLVertexDescriptor __nonnull */ IntPtr MTKModelIOVertexDescriptorFromMetalWithError (/* MTLVertexDescriptor __nonnull */ IntPtr metalDescriptor, /* NSError */ IntPtr* error);

#if NET
		/// <param name="descriptor">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Creates a Model IO vertex descriptor from the specified metal vertex descriptor.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public static MDLVertexDescriptor? FromMetal (MTLVertexDescriptor descriptor, out NSError? error)
		{
			if (descriptor is null)
				throw new ArgumentException (nameof (descriptor));
			IntPtr err;
			MDLVertexDescriptor? vd;
			unsafe {
				vd = Runtime.GetNSObject<MDLVertexDescriptor> (MTKModelIOVertexDescriptorFromMetalWithError (descriptor.Handle, &err));
				GC.KeepAlive (descriptor);
			}
			error = Runtime.GetNSObject<NSError> (err);
			return vd;
		}
	}
}
