//
// UISearchController.cs: Simplify the UISearchController
//

namespace UIKit {

	public partial class UISearchController {
		class __Xamarin_UISearchResultsUpdating : UISearchResultsUpdating {
			Action<UISearchController> cback;

			public __Xamarin_UISearchResultsUpdating (Action<UISearchController> cback)
			{
				this.cback = cback;
				IsDirectBinding = false;
			}

			[Preserve (Conditional = true)] // called back from native, no direct managed reference (except on the type itself)
			public override void UpdateSearchResultsForSearchController (UISearchController searchController)
			{
				cback (searchController);
			}
		}

		/// <param name="updateSearchResults">An action to invoke when the search results need to be updated.</param>
		/// <summary>Sets a callback to be invoked when the search results need to be updated.</summary>
		public void SetSearchResultsUpdater (Action<UISearchController> updateSearchResults)
		{
			if (updateSearchResults is null) {
				WeakSearchResultsUpdater = null;
				return;
			}

			WeakSearchResultsUpdater = new __Xamarin_UISearchResultsUpdating (updateSearchResults);
		}
	}
}
