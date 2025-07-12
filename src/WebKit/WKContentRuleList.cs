using System;
using System.ComponentModel;

using Foundation;

#nullable enable

namespace WebKit {
#if !XAMCORE_5_0
#if !COREBUILD
	public partial class WKContentRuleList {
		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Do not create instances of this class, use 'WKContentRuleListStore.CompileContentRuleList' instead.")]
		public WKContentRuleList ()
		{
		}
	}
#endif // !COREBUILD
#endif // !XAMCORE_5_0
}
