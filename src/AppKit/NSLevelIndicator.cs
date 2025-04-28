//
// NSLevelIndicator: Support for the NSLevelIndicator class
//
// Author:
//   Pavel Sich (pavel.sich@me.com)
//

#if !__MACCATALYST__

using System;
using ObjCRuntime;
using Foundation;

#nullable enable

namespace AppKit {

	public partial class NSLevelIndicator {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public new NSLevelIndicatorCell Cell {
			get { return (NSLevelIndicatorCell) base.Cell; }
			set { base.Cell = value; }
		}
	}
}
#endif // !__MACCATALYST__
