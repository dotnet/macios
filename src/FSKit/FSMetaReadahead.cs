#if NET

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#nullable enable

// Let's hope that by .NET 11 we've ironed out all the bugs in the API.
// This can of course be adjusted as needed (until we've released as stable).
#if NET11_0_OR_GREATER
#define STABLE_FSKIT
#endif

namespace FSKit {
#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[SupportedOSPlatform ("macos15.4")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	[StructLayout (LayoutKind.Sequential)]
	public struct FSMetadataReadahead
	{
		public long Offset;
		public nuint Length;
	}
}
#endif
