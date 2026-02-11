#if IOS

using CoreGraphics;

#nullable enable

namespace UIKit {
	public partial class UIPickerView : UIView, IUITableViewDataSource {
		UIPickerViewModel? model;

		/// <summary>Gets or sets the <see cref="UIPickerViewModel" /> that this <see cref="UIPickerView" /> is representing.</summary>
		public UIPickerViewModel? Model {
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
