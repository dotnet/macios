//
// UIBarItem extension
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, 2015 Xamarin Inc.
//

using TextAttributes = UIKit.UIStringAttributes;

namespace UIKit {
	public partial class UIBarItem {

		/// <param name="attributes">Specified title text attributes.</param>
		/// <param name="state">Specified <see cref="UIKit.UIControlState" />.</param>
		/// <summary>Specifies the text attributes of the title of the UIBarItem.</summary>
		public void SetTitleTextAttributes (TextAttributes attributes, UIControlState state)
		{
			var dict = attributes?.Dictionary;
			_SetTitleTextAttributes (dict, state);
		}

		/// <param name="state">The state for which text attributes are to be set for the title.</param>
		/// <summary>The text attributes of the title of the UIBarItem.</summary>
		/// <returns>The <see cref="UIKit.UIStringAttributes" /></returns>
		public TextAttributes GetTitleTextAttributes (UIControlState state)
		{
			using (var d = _GetTitleTextAttributes (state)) {
				return new TextAttributes (d);
			}
		}

		public partial class UIBarItemAppearance {
			/// <param name="attributes">The attributes.</param>
			/// <param name="state">The state.</param>
			/// <summary>Sets the attributes applied to the title text of the UIBarItem.</summary>
			public virtual void SetTitleTextAttributes (TextAttributes attributes, UIControlState state)
			{
				if (attributes is null)
					throw new ArgumentNullException ("attributes");
				var dict = attributes.Dictionary;
				_SetTitleTextAttributes (dict, state);
			}

			/// <param name="state">The state.</param>
			///         <summary>The attributes applies to the title text of the UIBarItem.</summary>
			public virtual TextAttributes GetTitleTextAttributes (UIControlState state)
			{
				using (var d = _GetTitleTextAttributes (state)) {
					return new TextAttributes (d);
				}
			}
		}
	}
}
