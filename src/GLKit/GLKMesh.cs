// Copyright 2015 Xamarin Inc.

using Foundation;
using ModelIO;

#nullable enable

namespace GLKit {

	public partial class GLKMesh {
		/// <param name="asset">To be added.</param>
		///         <param name="sourceMeshes">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static GLKMesh []? FromAsset (MDLAsset asset, out MDLMesh [] sourceMeshes, out NSError error)
		{
			NSArray aret;

			var ret = FromAsset (asset, out aret, out error);
			sourceMeshes = NSArray.FromArray<MDLMesh> (aret);
			return ret;
		}
	}
}
