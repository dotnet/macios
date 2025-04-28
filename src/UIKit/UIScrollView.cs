//
// UIScrollView.cs: Extensions to UIScrollView
//
// Authors:
//   Geoff Norton
//
// Copyright 2009, Novell, Inc.
//

using System;

namespace UIKit {
	/// <summary>Provides data for the <see cref="E:UIKit.UIScrollView.DraggingEnded" /> event.</summary>
	///     <remarks>
	///     </remarks>
	public partial class DraggingEventArgs : EventArgs {
		/// <summary>Decelerating.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static DraggingEventArgs True;
		/// <summary>Not decelerating.</summary>
		///         <remarks>To be added.</remarks>
		public readonly static DraggingEventArgs False;

		static DraggingEventArgs ()
		{
			True = new DraggingEventArgs (true);
			False = new DraggingEventArgs (false);
		}
	}
}
