﻿#if XAMCORE_2_0 || !MONOMAC
using System;
using System.Runtime.InteropServices;

using XamCore.Foundation;
using XamCore.ModelIO;
using XamCore.ObjCRuntime;
using XamCore.Metal;

namespace XamCore.ModelIO {
	public partial class MDLVertexDescriptor {
		[DllImport (Constants.MetalKitLibrary)]
		static extern  /* MDLVertexDescriptor __nonnull */ IntPtr MTKModelIOVertexDescriptorFromMetal (/* MTLVertexDescriptor __nonnull */ IntPtr mtlDescriptor);

		public static MDLVertexDescriptor FromMetal (MTLVertexDescriptor descriptor)
		{
			if (descriptor == null)
				throw new ArgumentException ("descriptor");
			return Runtime.GetNSObject<MDLVertexDescriptor> (MTKModelIOVertexDescriptorFromMetal (descriptor.Handle));
		}

		[iOS (10,0)][Mac (10,12, onlyOn64 : true)]
		[DllImport (Constants.MetalKitLibrary)]
		static extern /* MDLVertexDescriptor __nonnull */ IntPtr MTKModelIOVertexDescriptorFromMetalWithError (/* MTLVertexDescriptor __nonnull */ IntPtr metalDescriptor, out /* NSError */ IntPtr error);

		[iOS (10,0)][Mac (10,12, onlyOn64 : true)]
		public static MDLVertexDescriptor FromMetal (MTLVertexDescriptor descriptor, out NSError error)
		{
			if (descriptor == null)
				throw new ArgumentException ("descriptor");
			IntPtr err;
			var vd = Runtime.GetNSObject<MDLVertexDescriptor> (MTKModelIOVertexDescriptorFromMetalWithError (descriptor.Handle, out err));
			error = Runtime.GetNSObject<NSError> (err);
			return vd;
		}
	}
}
#endif
