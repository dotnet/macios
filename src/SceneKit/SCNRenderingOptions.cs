#if !__MACCATALYST__

#nullable enable

namespace SceneKit {
	public partial class SCNRenderingOptions {
		/// <summary>Gets the rendering API used for this scene.</summary>
		/// <value>The rendering API, or <see langword="null" /> if not specified.</value>
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public SCNRenderingApi? RenderingApi {
			get {
				var val = GetNUIntValue (SCNRenderingOptionsKeys.RenderingApiKey);
				if (val is not null)
					return (SCNRenderingApi) (uint) val;
				return null;
			}

			set {
				if (value.HasValue)
					SetNumberValue (SCNRenderingOptionsKeys.RenderingApiKey, (nuint) (uint) value.Value);
				else
					RemoveValue (SCNRenderingOptionsKeys.RenderingApiKey);
			}
		}
	}
}

#endif
