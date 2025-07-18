using System.Runtime.InteropServices;

using Foundation;

namespace Metal {
	/// <summary>A container for the arguments for a update sparse buffer mapping operation.</summary>
	[StructLayout (LayoutKind.Sequential)]
	public struct MTL4UpdateSparseBufferMappingOperation {
		MTLSparseTextureMappingMode mode;
		NSRange bufferRange;
		nuint heapOffset;

		/// <summary>The mapping mode to use.</summary>
		public MTLSparseTextureMappingMode Mode {
			get => mode;
			set => mode = value;
		}

		/// <summary>The range of the buffer.</summary>
		public NSRange BufferRange {
			get => bufferRange;
			set => bufferRange = value;
		}

		/// <summary>The heap offset.</summary>
		public nuint HeapOffset {
			get => heapOffset;
			set => heapOffset = value;
		}
	}
}
