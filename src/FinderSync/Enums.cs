#nullable enable

using System;
using ObjCRuntime;

namespace FinderSync {
	[Native]
	public enum FIMenuKind : ulong {
		/// <summary>To be added.</summary>
		ContextualMenuForItems = 0,
		/// <summary>To be added.</summary>
		ContextualMenuForContainer = 1,
		/// <summary>To be added.</summary>
		ContextualMenuForSidebar = 2,
		/// <summary>To be added.</summary>
		ToolbarItemMenu = 3,
	}
}
