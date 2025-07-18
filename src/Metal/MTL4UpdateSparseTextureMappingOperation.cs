using System.Runtime.InteropServices;

namespace Metal {
	/// <summary>A container for the arguments for a update sparse texture mapping operation.</summary>
	[StructLayout (LayoutKind.Sequential)]
	public struct MTL4UpdateSparseTextureMappingOperation {
		MTLSparseTextureMappingMode mode;
		MTLRegion textureRegion;
		nuint textureLevel;
		nuint textureSlice;
		nuint heapOffset;

		/// <summary>The mapping mode to use.</summary>
		public MTLSparseTextureMappingMode Mode {
			get => mode;
			set => mode = value;
		}

		/// <summary>The region in the texture to update.</summary>
		public MTLRegion TextureRegion {
			get => textureRegion;
			set => textureRegion = value;
		}

		/// <summary>The mipmap level index in the texture to update.</summary>
		public nuint TextureLevel {
			get => textureLevel;
			set => textureLevel = value;
		}

		/// <summary>The array slice index in the texture to update</summary>
		public nuint TextureSlice {
			get => textureSlice;
			set => textureSlice = value;
		}

		/// <summary>The heap offset.</summary>
		public nuint HeapOffset {
			get => heapOffset;
			set => heapOffset = value;
		}
	}
}
