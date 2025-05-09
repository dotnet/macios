#if !__MACCATALYST__
using System;
using Foundation;
using CoreGraphics;

#nullable enable

namespace AppKit {

	public partial class NSComboBox {
		public NSObject this [nint index] {
			get {
				return GetItemObject (index);
			}
		}
	}
}
#endif // !__MACCATALYST__
