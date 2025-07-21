using System;
using System.Runtime.InteropServices;

using Foundation;
using ModelIO;
using ObjCRuntime;
using Metal;

#nullable enable

namespace Metal {
	public partial class MTLVertexDescriptor {
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[DllImport (Constants.MetalKitLibrary)]
		static extern  /* MTLVertexDescriptor __nonnull */ IntPtr MTKMetalVertexDescriptorFromModelIO (/* MDLVertexDescriptor __nonnull */ IntPtr modelIODescriptor);

		/// <param name="descriptor">To be added.</param>
		///         <summary>Creates and returns a new vertex descriptor object from the provided Model IO vertex descriptor.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public static MTLVertexDescriptor? FromModelIO (MDLVertexDescriptor descriptor)
		{
			if (descriptor is null)
				throw new ArgumentException ("descriptor");
			MTLVertexDescriptor? result = Runtime.GetNSObject<MTLVertexDescriptor> (MTKMetalVertexDescriptorFromModelIO (descriptor.Handle));
			GC.KeepAlive (descriptor);
			return result;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.MetalKitLibrary)]
		unsafe static extern /* MTLVertexDescriptor __nonnull */ IntPtr MTKMetalVertexDescriptorFromModelIOWithError (/* MDLVertexDescriptor __nonnull */ IntPtr modelIODescriptor, IntPtr* error);

		/// <param name="descriptor">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Creates and returns a new vertex descriptor object from the provided Model IO vertex descriptor.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		public static MTLVertexDescriptor? FromModelIO (MDLVertexDescriptor descriptor, out NSError? error)
		{
			if (descriptor is null)
				throw new ArgumentException ("descriptor");
			IntPtr err;
			MTLVertexDescriptor? vd;
			unsafe {
				vd = Runtime.GetNSObject<MTLVertexDescriptor> (MTKMetalVertexDescriptorFromModelIOWithError (descriptor.Handle, &err));
				GC.KeepAlive (descriptor);
			}
			error = Runtime.GetNSObject<NSError> (err);
			return vd;
		}
	}
}
