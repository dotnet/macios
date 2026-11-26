//
// NSTextField.cs: Support for the NSTextField class
//

#if !__MACCATALYST__

#nullable enable

namespace AppKit {

	public partial class NSTextField {
		/// <summary>Gets or sets the text field cell associated with this control.</summary>
		/// <value>The <see cref="NSTextFieldCell" /> used by this control.</value>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.10")]
		public new NSTextFieldCell Cell {
			get { return (NSTextFieldCell) base.Cell; }
			set { base.Cell = value; }
		}
	}
}
#endif // !__MACCATALYST__
