//
// NSLevelIndicator: Support for the NSLevelIndicator class
//
// Author:
//   Pavel Sich (pavel.sich@me.com)
//

#if !__MACCATALYST__

#nullable enable

namespace AppKit {

	public partial class NSLevelIndicator {
		/// <summary>Gets or sets the level indicator cell associated with this control.</summary>
		/// <value>The <see cref="NSLevelIndicatorCell" /> used by this control.</value>
		public new NSLevelIndicatorCell Cell {
#pragma warning disable CA1422 // This call site is reachable on: 'macOS/OSX' 14.0 and later. 'NSControl.Cell' is obsoleted on: 'macOS/OSX' 10.10 and later.
			get { return (NSLevelIndicatorCell) base.Cell; }
			set { base.Cell = value; }
#pragma warning restore CA1422
		}
	}
}
#endif // !__MACCATALYST__
