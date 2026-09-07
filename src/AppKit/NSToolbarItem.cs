//
// NSToolbarItem.cs: Support for the NSToolbarItem class
//
// Author:
//   Johan Hammar
//

#nullable enable

namespace AppKit {

	public partial class NSToolbarItem {
		NSObject? target;
		Selector? action;

		/// <summary>Occurs when the toolbar item is activated by the user.</summary>
		public event EventHandler Activated {
			add {
				target = ActionDispatcher.SetupAction (Target, value);
				action = ActionDispatcher.Action;
				MarkDirty ();
				Target = target;
				Action = action;
			}

			remove {
				ActionDispatcher.RemoveAction (Target, value);
				target = null;
				action = null;
				MarkDirty ();
			}
		}

	}
}
