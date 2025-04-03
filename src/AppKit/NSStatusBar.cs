//
// NSStatusBar.cs: Proprietary extensions to MonoMac NSStatusBar
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2012 Xamarin Inc
//

#if !__MACCATALYST__

using System;

#nullable enable

namespace AppKit {
	public enum NSStatusItemLength {
		/// <summary>To be added.</summary>
		Variable = -1,
		/// <summary>To be added.</summary>
		Square = -2,
	}

	public partial class NSStatusBar {
		/// <param name="length">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSStatusItem CreateStatusItem (NSStatusItemLength length)
		{
			return CreateStatusItem ((float) length);
		}
	}
}
#endif // !__MACCATALYST__
