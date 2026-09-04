#nullable enable

namespace AudioUnit {
#if __IOS__ && !__MACCATALYST__
	public partial class AUHeadTrackingBinauralRenderer {
		/// <summary>Create a new <see cref="AUHeadTrackingBinauralRenderer" /> instance.</summary>
		/// <param name="componentDescription">A description of the component to create.</param>
		/// <param name="options">Any options for the returned audio unit.</param>
		/// <param name="error">The error if an error occurred, null otherwise.</param>
		/// <returns>A new <see cref="AUHeadTrackingBinauralRenderer" /> instance if successful, null otherwise.</returns>
		public static AUHeadTrackingBinauralRenderer? Create (AudioComponentDescription componentDescription, AudioComponentInstantiationOptions options, out NSError? error)
		{
			var rv = new AUHeadTrackingBinauralRenderer (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithComponentDescription (componentDescription, options, out error), "initWithComponentDescription:options:error:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
#endif // __IOS__ && !__MACCATALYST__
}
