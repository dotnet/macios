#nullable enable

using System;

using Foundation;
using ObjCRuntime;

namespace MetricKit {

	public partial class MXMetaData {
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		public virtual NSDictionary DictionaryRepresentation {
			get {
#if !__MACOS__
				if (!SystemVersion.IsAtLeastXcode12) {
#pragma warning disable CA1416 // This call site is reachable on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later, 'macOS/OSX' 12.0 and later. 'MXMetaData._DictionaryRepresentation13' is only supported on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later. (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1416)
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later, 'macOS/OSX' 12.0 and later. 'MXMetaData._DictionaryRepresentation13' is obsoleted on: 'ios' 14.0 and later, 'maccatalyst' 14.0 and later. (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1422)
					return _DictionaryRepresentation13;
#pragma warning restore CA1422
#pragma warning restore CA1416
				}
#endif
				return _DictionaryRepresentation14;
			}
		}
	}
}
