//
// UISearchController.cs: Simplify the UISearchController
//

using System;
using Foundation;

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

		/// <param name="updateSearchResults">To be added.</param>
		///         <summary>Assigns the <paramref name="updateSearchResults" /> search controller to update the search results.</summary>
		///         <remarks>To be added.</remarks>
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
