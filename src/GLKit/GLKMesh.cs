// Copyright 2015 Xamarin Inc.

using ModelIO;

#nullable enable

namespace GLKit {

	public partial class GLKMesh {
		/// <summary>Creates GLKit meshes from the meshes in a Model I/O asset.</summary>
		/// <param name="asset">The asset containing the source meshes.</param>
		/// <param name="sourceMeshes">The Model I/O meshes corresponding to the returned GLKit meshes.</param>
		/// <param name="error">On failure, contains an error that describes the problem.</param>
		/// <returns>The created GLKit meshes, or <see langword="null" /> if they could not be created.</returns>
		public static GLKMesh []? FromAsset (MDLAsset asset, out MDLMesh []? sourceMeshes, out NSError? error)
		{
			var ret = FromAsset (asset, out NSArray? aret, out error);
			sourceMeshes = NSArray.FromArray<MDLMesh> (aret);
			return ret;
		}
	}
}
