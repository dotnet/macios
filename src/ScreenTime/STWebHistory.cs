using System.Diagnostics.CodeAnalysis;

using Foundation;

namespace ScreenTime {
	public partial class STWebHistory {
		/// <summary>Create a new <see cref="STWebHistory" /> for the specified bundle identifier.</summary>
		/// <param name="bundleIdentifier">The bundle identifier whose web history to retrieve.</param>
		/// <param name="error">In case of failure, an error describing the failure, otherwise null.</param>
		/// <returns>A new <see cref="STWebHistory" />, or null in case of failure.</returns>
		public static STWebHistory? Create (string bundleIdentifier, out NSError? error)
		{
			var rv = new STWebHistory (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithBundleIdentifier (bundleIdentifier, out error), "initWithBundleIdentifier:error:", false);
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}

		/// <summary>Create a new <see cref="STWebHistory" /> for the specified bundle identifier and browsing profile.</summary>
		/// <param name="bundleIdentifier">The bundle identifier whose web history to retrieve.</param>
		/// <param name="profileIdentifier">The identifier for the browsing profile whose web histor to retrieve.</param>
		/// <param name="error">In case of failure, an error describing the failure, otherwise null.</param>
		/// <returns>A new <see cref="STWebHistory" />, or null in case of failure.</returns>
		[SupportedOSPlatform ("ios18.4")]
		[SupportedOSPlatform ("maccatalyst18.4")]
		[SupportedOSPlatform ("macos15.4")]
		public static STWebHistory? Create (string bundleIdentifier, NSString profileIdentifier, out NSError? error)
		{
			var rv = new STWebHistory (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithBundleIdentifier (bundleIdentifier, profileIdentifier, out error), "initWithBundleIdentifier:profileIdentifier:error:", false);
			if (rv.Handle == IntPtr.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
}
