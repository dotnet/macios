#nullable enable

namespace AppKit {
	/// <summary>An exception that is thrown when AppKit methods are called from a non-UI thread.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public class AppKitThreadAccessException : Exception {
		/// <summary>Initializes a new instance of the <see cref="AppKitThreadAccessException" /> class.</summary>
		public AppKitThreadAccessException () : base ("AppKit Consistency error: you are calling a method that can only be invoked from the UI thread.")
		{
		}
	}
}
