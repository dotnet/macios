//
// UIScrollView.cs: Extensions to UIScrollView
//
// Authors:
//   Geoff Norton
//
// Copyright 2009, Novell, Inc.
//

namespace UIKit {
	/// <summary>Provides data for the <see cref="UIKit.UIScrollView.DraggingEnded" /> event.</summary>
	public partial class DraggingEventArgs : EventArgs {
		/// <summary>Shared event data indicating that scrolling continues to decelerate after dragging ends.</summary>
		public readonly static DraggingEventArgs True = new DraggingEventArgs (true);
		/// <summary>Shared event data indicating that scrolling stops when dragging ends.</summary>
		public readonly static DraggingEventArgs False = new DraggingEventArgs (false);
	}
}
