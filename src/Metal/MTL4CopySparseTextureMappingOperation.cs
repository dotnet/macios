using System.Runtime.InteropServices;

namespace Metal {
	/// <summary>A container for the arguments for a copy sparse texture mapping operation.</summary>
	[StructLayout (LayoutKind.Sequential)]
	public struct MTL4CopySparseTextureMappingOperation {
		MTLRegion sourceRegion;
		nuint sourceLevel;
		nuint sourceSlice;
		MTLOrigin destinationOrigin;
		nuint destinationLevel;
		nuint destinationSlice;

		/// <summary>The source texture's region.</summary>
		public MTLRegion SourceRegion {
			get => sourceRegion;
			set => sourceRegion = value;
		}

		/// <summary>The mipmap level index in the source texture.</summary>
		public nuint SourceLevel {
			get => sourceLevel;
			set => sourceLevel = value;
		}

		/// <summary>The index of the array slice in the source texture.</summary>
		public nuint SourceSlice {
			get => sourceSlice;
			set => sourceSlice = value;
		}

		/// <summary>The destination texture's region.</summary>
		public MTLOrigin DestinationOrigin {
			get => destinationOrigin;
			set => destinationOrigin = value;
		}

		/// <summary>The mipmap level index in the destination texture.</summary>
		public nuint DestinationLevel {
			get => destinationLevel;
			set => destinationLevel = value;
		}

		/// <summary>The index of the array slice in the destination texture.</summary>
		public nuint DestinationSlice {
			get => destinationSlice;
			set => destinationSlice = value;
		}
	}
}
