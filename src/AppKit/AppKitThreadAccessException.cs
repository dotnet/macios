using System;
using System.Runtime.Versioning;

#nullable enable

namespace AppKit {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public class AppKitThreadAccessException : Exception {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public AppKitThreadAccessException () : base ("AppKit Consistency error: you are calling a method that can only be invoked from the UI thread.")
		{
		}
	}
}
