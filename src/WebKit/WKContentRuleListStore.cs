using System;
using System.ComponentModel;

using Foundation;

#nullable enable

namespace WebKit {
#if !XAMCORE_5_0
#if !COREBUILD
	public partial class WKContentRuleListStore {
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Do not use this constructor to create instances of this class, use 'WKContentRuleListStore.DefaultStore' or 'WKContentRuleListStore.FromUrl' instead.")]
		public WKContentRuleListStore ()
		{
		}
	}
#endif // !COREBUILD
#endif // !XAMCORE_5_0
}
