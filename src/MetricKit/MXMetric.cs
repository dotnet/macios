#if IOS

#nullable enable

using System;

using Foundation;
using ObjCRuntime;
using UIKit;

namespace MetricKit {

	public partial class MXMetric {

		public virtual NSDictionary DictionaryRepresentation {
			get {
				if (SystemVersion.IsAtLeastXcode12)
					return _DictionaryRepresentation14;
				else
					return _DictionaryRepresentation13;
			}
		}
	}
}
#endif
