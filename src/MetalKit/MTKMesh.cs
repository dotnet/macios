//
// MTKMesh.cs: just so we can implement IMDLMeshBufferAllocator
//

#nullable enable

using ModelIO;
using Metal;
namespace MetalKit {

	public partial class MTKMesh {
		/// <summary>Creates MetalKit meshes from the meshes in a Model I/O asset.</summary>
		/// <param name="asset">The asset containing the source meshes.</param>
		/// <param name="device">The Metal device on which to create the mesh resources.</param>
		/// <param name="sourceMeshes">The Model I/O meshes corresponding to the returned MetalKit meshes.</param>
		/// <param name="error">On failure, contains an error that describes the problem.</param>
		/// <returns>The created MetalKit meshes, or <see langword="null" /> if they could not be created.</returns>
		public static MTKMesh []? FromAsset (MDLAsset asset, IMTLDevice device, out MDLMesh []? sourceMeshes, out NSError error)
		{
			NSArray aret;

			var ret = FromAsset (asset, device, out aret, out error);
			sourceMeshes = NSArray.FromArray<MDLMesh> (aret);
			return ret;
		}
	}
}
