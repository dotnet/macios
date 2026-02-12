#if IOS

using CoreGraphics;

#nullable enable

namespace UIKit {
	public partial class UIPickerView : UIView, IUITableViewDataSource {
		UIPickerViewModel? model;

		/// <summary>Gets or sets the <see cref="UIPickerViewModel" /> that this <see cref="UIPickerView" /> is representing.</summary>
		/// <value>The model used by the picker view. May be <see langword="null" /> to indicate that no model is set or to clear the current model.</value>
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
