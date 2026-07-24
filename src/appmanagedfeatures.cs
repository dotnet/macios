#nullable enable

namespace AppManagedFeatures {

	/// <summary>Provides version information for the AppManagedFeatures framework.</summary>
	[iOS (27, 0)]
	[UnsupportedSimulator ("ios")]
	[Static]
	interface AppManagedFeaturesConstants {
		/// <summary>Gets the AppManagedFeatures framework version number.</summary>
		[Field ("AppManagedFeaturesVersionNumber")]
		double AppManagedFeaturesVersionNumber { get; }
	}
}
