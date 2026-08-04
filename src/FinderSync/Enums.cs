#nullable enable

namespace FinderSync {
	/// <summary>Specifies the Finder location where a Finder Sync extension menu appears.</summary>
	[Native]
	public enum FIMenuKind : ulong {
		/// <summary>A contextual menu for selected items.</summary>
		ContextualMenuForItems = 0,
		/// <summary>A contextual menu for the monitored folder containing the selected items.</summary>
		ContextualMenuForContainer = 1,
		/// <summary>A contextual menu for an item in the Finder sidebar.</summary>
		ContextualMenuForSidebar = 2,
		/// <summary>The menu associated with the extension's Finder toolbar item.</summary>
		ToolbarItemMenu = 3,
	}
}
