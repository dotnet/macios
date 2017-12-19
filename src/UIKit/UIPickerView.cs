#if IOS

using System;

using XamCore.Foundation; 
using XamCore.CoreGraphics;
using XamCore.ObjCRuntime;

namespace XamCore.UIKit {
	public partial class UIPickerView : UIView, IUITableViewDataSource {
		private UIPickerViewModel model;

#if !XAMCORE_2_0
		[Obsolete ("Use Model instead")]
		public UIPickerViewModel Source {
			get {
				return Model;
			}
			set {
				Model = value;
			}
		}
#endif

		public UIPickerViewModel Model {
			get {
				return model;
			}
			set {
				model = value;
				WeakDelegate = value;
				DataSource = value;
				MarkDirty ();
			}
		}
	}
}

#endif // IOS
