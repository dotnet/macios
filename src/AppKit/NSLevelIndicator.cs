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
			get { return (NSLevelIndicatorCell) base.Cell; }
			set { base.Cell = value; }
		}
	}
}
#endif // !__MACCATALYST__
