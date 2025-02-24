//
// MTKMesh.cs: just so we can implement IMDLMeshBufferAllocator
//

#nullable enable

using ModelIO;
using Metal;
using Foundation;
namespace MetalKit {

	public partial class MTKMesh {
		/// <param name="asset">To be added.</param>
		///         <param name="device">To be added.</param>
		///         <param name="sourceMeshes">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Creates and returns a new Metal Kit mesh from the supplied Model IO asset.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static MTKMesh []? FromAsset (MDLAsset asset, IMTLDevice device, out MDLMesh [] sourceMeshes, out NSError error)
		{
			NSArray aret;

			var ret = FromAsset (asset, device, out aret, out error);
			sourceMeshes = NSArray.FromArray<MDLMesh> (aret);
			return ret;
		}
	}
}
