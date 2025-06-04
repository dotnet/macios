#if IOS || __MACCATALYST__

#nullable enable

using System;

using Foundation;
using ObjCRuntime;
using UIKit;

namespace MetricKit {

	public partial class MXMetaData {
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		public virtual NSDictionary DictionaryRepresentation {
			get {
				if (SystemVersion.IsAtLeastXcode12)
					return _DictionaryRepresentation14;
				else {
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later, 'macOS/OSX' 12.0 and later. 'MXMetaData._DictionaryRepresentation13' is only supported on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later. (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1416) [/Users/rolf/work/dotnet/macios/main/macios/src/build/dotnet/MacCatalyst/csproj/platform/Microsoft.MacCatalyst.csproj]
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later, 'macOS/OSX' 12.0 and later. 'MXMetaData._DictionaryRepresentation13' is obsoleted on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later. (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1422) [/Users/rolf/work/dotnet/macios/main/macios/src/build/dotnet/MacCatalyst/csproj/platform/Microsoft.MacCatalyst.csproj]
					return _DictionaryRepresentation13;
#pragma warning restore CA1422
#pragma warning restore CA1416
				}
			}
		}
	}
}
#endif
