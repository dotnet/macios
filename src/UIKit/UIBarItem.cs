//
// UIBarItem extension
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011, 2015 Xamarin Inc.
//

using System;
using ObjCRuntime;
using Foundation;

using TextAttributes = UIKit.UIStringAttributes;

namespace UIKit {
	public partial class UIBarItem {

		public void SetTitleTextAttributes (TextAttributes attributes, UIControlState state)
		{
			var dict = attributes?.Dictionary;
			_SetTitleTextAttributes (dict, state);
		}

		/// <param name="state">The state for which text attributes are to be set for the title.</param>
		///         <summary>The text attributes of the title of the UIBarItem.</summary>
		///         <returns>The <see cref="T:UIKit.UITextAttributes" /></returns>
		///         <remarks>To be added.</remarks>
		public TextAttributes GetTitleTextAttributes (UIControlState state)
		{
			using (var d = _GetTitleTextAttributes (state)) {
				return new TextAttributes (d);
			}
		}

		public partial class UIBarItemAppearance {
			public virtual void SetTitleTextAttributes (TextAttributes attributes, UIControlState state)
			{
				if (attributes is null)
					throw new ArgumentNullException ("attributes");
				var dict = attributes.Dictionary;
				_SetTitleTextAttributes (dict, state);
			}

			/// <param name="state">To be added.</param>
			///         <summary>The attributes applies to the title text of the UIBarItem.</summary>
			///         <returns>To be added.</returns>
			///         <remarks>To be added.</remarks>
			public virtual TextAttributes GetTitleTextAttributes (UIControlState state)
			{
				using (var d = _GetTitleTextAttributes (state)) {
					return new TextAttributes (d);
				}
			}
		}
	}
}
