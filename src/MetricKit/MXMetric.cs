#nullable enable

using System;

using Foundation;
using ObjCRuntime;

namespace MetricKit {

	public partial class MXMetric {
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		public virtual NSDictionary DictionaryRepresentation {
			get {
#if !__MACOS__
				if (!SystemVersion.IsAtLeastXcode12)
					return _DictionaryRepresentation13;
#endif
				return _DictionaryRepresentation14;
			}
		}
	}
}
