
#nullable enable

namespace MapKit {
	/// <summary>Enumerates annotation display priorities.</summary>
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	// .net does not allow float-based enumerations
	public static class MKFeatureDisplayPriority {
		/// <summary>Indicates that the annotation is required to be displayed.</summary>
		public const float Required = 1000f;
		/// <summary>Indicates that the annotation is a high priority for display.</summary>
		public const float DefaultHigh = 750f;
		/// <summary>Indicates that the annotation is a low priority for display.</summary>
		public const float DefaultLow = 250f;
	}
}
