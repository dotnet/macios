using System.Runtime.InteropServices;

using Foundation;

namespace Metal {
	/// <summary>A container for the arguments for a copy sparse buffer mapping operation.</summary>
	[StructLayout (LayoutKind.Sequential)]
	public struct MTL4CopySparseBufferMappingOperation {
		NSRange sourceRange;
		nuint destinationOffset;

		/// <summary>The range in the source buffer.</summary>
		public NSRange SourceRange {
			get => sourceRange;
			set => sourceRange = value;
		}

		/// <summary>The offset in the destination buffer.</summary>
		public nuint DestinationOffset {
			get => destinationOffset;
			set => destinationOffset = value;
		}
	}
}
