//
// NSTextField.cs: Support for the NSTextField class
//

#if !__MACCATALYST__

#nullable enable

namespace AppKit {

	public partial class NSTextField {
		/// <summary>Gets or sets the text field cell associated with this control.</summary>
		/// <value>The <see cref="NSTextFieldCell" /> used by this control.</value>
		public new NSTextFieldCell Cell {
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSControl.Cell' is obsoleted on: 'macOS/OSX' 10.10 and later.
			get { return (NSTextFieldCell) base.Cell; }
			set { base.Cell = value; }
#pragma warning restore CA1422
		}
	}
}
#endif // !__MACCATALYST__
