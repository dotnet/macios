#if !TVOS && !__MACCATALYST__ // __TVOS_PROHIBITED

#nullable enable

namespace UIKit {
	public partial class UISearchDisplayController {
		/// <summary>Gets or sets the <see cref="UITableViewSource" /> holding the search results.</summary>
		public UITableViewSource? SearchResultsSource {
			get {
				return SearchResultsWeakDelegate as UITableViewSource;
			}

			set {
				SearchResultsWeakDelegate = value;
				SearchResultsWeakDataSource = value;
			}
		}
	}
}

#endif // !TVOS && !__MACCATALYST__
