//
// UIKit/UISearchBar.cs: Extensions to UISearchBar
//
// Copyright 2011, Xamarin, Inc.
//
// Author:
//   Miguel de Icaza
//

using TextAttributes = UIKit.UIStringAttributes;

namespace UIKit {
	public partial class UISearchBar {
		/// <param name="attributes">The attributes.</param>
		/// <param name="state">The state.</param>
		/// <summary>The title and attributes of the scope bar button for the specified UIControlState.</summary>
		/// <remarks>
		///           <para id="tool-remark">This member participates in the <see cref="UIKit.UIAppearance" /> styling system.  See the <see cref="UIKit.UISearchBar.Appearance" /> property and the <see cref="UIKit.UISearchBar.AppearanceWhenContainedIn(System.Type[])" /> method.</para>
		///         </remarks>
		public void SetScopeBarButtonTitle (TextAttributes attributes, UIControlState state)
		{
			if (attributes is null)
				throw new ArgumentNullException ("attributes");

			var dict = attributes.Dictionary;
			_SetScopeBarButtonTitle (dict, state);
		}

		/// <param name="state">The state.</param>
		///         <summary>The text attributes of the scope bar button's title for the specified UIControlState.</summary>
		///         <remarks>
		///           <para id="tool-remark">This member participates in the <see cref="UIKit.UIAppearance" /> styling system.  See the <see cref="UIKit.UISearchBar.Appearance" /> property and the <see cref="UIKit.UISearchBar.AppearanceWhenContainedIn(System.Type[])" /> method.</para>
		///         </remarks>
		public TextAttributes GetScopeBarButtonTitleTextAttributes (UIControlState state)
		{
			using (var d = _GetScopeBarButtonTitleTextAttributes (state)) {
				return new TextAttributes (d);
			}
		}

		public partial class UISearchBarAppearance {
			/// <param name="attributes">The attributes.</param>
			/// <param name="state">The state.</param>
			/// <summary>Sets the attributes of the scope bar button for the specified UIControlState.</summary>
			public void SetScopeBarButtonTitle (TextAttributes attributes, UIControlState state)
			{
				if (attributes is null)
					throw new ArgumentNullException ("attributes");

				var dict = attributes.Dictionary;
				_SetScopeBarButtonTitle (dict, state);
			}

			/// <param name="state">The state.</param>
			///         <summary>The background image for the scope bar button for the specified state.</summary>
			public TextAttributes GetScopeBarButtonTitleTextAttributes (UIControlState state)
			{
				using (var d = _GetScopeBarButtonTitleTextAttributes (state)) {
					return new TextAttributes (d);
				}
			}
		}
	}
}
