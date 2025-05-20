using System;
using ObjCRuntime;

using Vector2i = global::CoreGraphics.NVector2i;

#nullable enable

namespace ModelIO {

	public partial class MDLNoiseTexture {

		public MDLNoiseTexture (float input, string name, Vector2i textureDimensions, MDLTextureChannelEncoding channelEncoding) : this (input, name, textureDimensions, channelEncoding, MDLNoiseTextureType.Vector)
		{
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public MDLNoiseTexture (float input, string name, Vector2i textureDimensions, MDLTextureChannelEncoding channelEncoding, MDLNoiseTextureType type)
		{
			// two different `init*` would share the same C# signature
			switch (type) {
			case MDLNoiseTextureType.Vector:
				Handle = InitVectorNoiseWithSmoothness (input, name, textureDimensions, channelEncoding);
				break;
			case MDLNoiseTextureType.Cellular:
				Handle = InitCellularNoiseWithFrequency (input, name, textureDimensions, channelEncoding);
				break;
			default:
				throw new ArgumentException ("type");
			}
		}
	}
}
