using System;
using Foundation;
using ObjCRuntime;

#if NET
using Vector2i = global::CoreGraphics.NVector2i;
#else
using Vector2i = global::OpenTK.Vector2i;
#endif

#nullable enable

namespace ModelIO {

	public partial class MDLNoiseTexture {

		public MDLNoiseTexture (float input, string name, Vector2i textureDimensions, MDLTextureChannelEncoding channelEncoding) : this (input, name, textureDimensions, channelEncoding, MDLNoiseTextureType.Vector)
		{
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public MDLNoiseTexture (float input, string name, Vector2i textureDimensions, MDLTextureChannelEncoding channelEncoding, MDLNoiseTextureType type)
			: base (NSObjectFlag.Empty)
		{
			// two different `init*` would share the same C# signature
			switch (type) {
			case MDLNoiseTextureType.Vector:
				InitializeHandle (_InitVectorNoiseWithSmoothness (input, name, textureDimensions, channelEncoding), "initVectorNoiseWithSmoothness:name:textureDimensions:channelEncoding:");
				break;
			case MDLNoiseTextureType.Cellular:
				InitializeHandle (_InitCellularNoiseWithFrequency (input, name, textureDimensions, channelEncoding), "initCellularNoiseWithFrequency:name:textureDimensions:channelEncoding:");
				break;
			default:
				throw new ArgumentException (nameof (type));
			}
		}
	}
}
