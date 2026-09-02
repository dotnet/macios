//
// UITextAttributes.cs: strongly typed version of the UITextAttributes
// that can be used to convert back and from NSDictionaries for the
// underlying Objective-C API
//
// Copyright 2011, 2013, 2015 Xamarin Inc.
//

#if IOS

#nullable enable

namespace UIKit {

	/// <summary>Type used to describe the text attributes to set on some user interface elements.</summary>
	/// <remarks>
	///   <para>
	///     Typically, developers create an instance of this class and
	///     fill out the various properties to configure the desired text
	///     attributes.
	///   </para>
	///   <para>
	///     For example, the following code can be used to change the style of the <see cref="UINavigationBar" />:
	///   </para>
	///   <example>
	///     <code lang="csharp lang-csharp"><![CDATA[
	/// var attrs = new UITextAttributes () {
	/// 	Font = UIFont.FromName ("Chalkduster", 20),
	/// 	TextColor = UIColor.Red,
	/// 	TextShadowColor = UIColor.Black,
	/// 	TextShadowOffset = new UIOffset (6, 6)
	/// };
	/// myNavController.NavigationBar.SetTitleTextAttributes (attrs);
	/// ]]></code>
	///   </example>
	///   <para>
	///     You can use code completion inside the constructor for
	///     <see cref="UITextAttributes" /> without having to remember the properties
	///     that you want to set.
	///   </para>
	///   <para>
	///     You can set the text attributes on the
	///     following items: <see cref="UIBarItem" />,
	///     <see cref="UINavigationBar" />, <see cref="UISearchBar" />, and <see cref="UISegmentedControl" />.
	///   </para>
	/// </remarks>
	public class UITextAttributes {
		/// <summary>The font to use to render the text.</summary>
		public UIFont? Font;
		/// <summary>The color to use for the text.</summary>
		public UIColor? TextColor;
		/// <summary>The color to use for the shadow of the text when <see cref="TextShadowOffset" /> is set.</summary>
		public UIColor? TextShadowColor;
		/// <summary>The offset describing the distance between the text and its shadow.</summary>
		public UIOffset TextShadowOffset;

		/// <summary>Initializes a new instance of the <see cref="UITextAttributes" /> class.</summary>
		public UITextAttributes ()
		{
		}

		internal UITextAttributes (NSDictionary? dict)
		{
			if (dict is null)
				return;

			if (dict.TryGetValue (UITextAttributesConstants.Font, out var val))
				Font = val as UIFont;
			if (dict.TryGetValue (UITextAttributesConstants.TextColor, out val))
				TextColor = val as UIColor;
			if (dict.TryGetValue (UITextAttributesConstants.TextShadowColor, out val))
				TextShadowColor = val as UIColor;
			if (dict.TryGetValue (UITextAttributesConstants.TextShadowOffset, out val)) {
				var value = val as NSValue;
				if (value is not null)
					TextShadowOffset = value.UIOffsetValue;
			}
		}

		internal NSDictionary ToDictionary ()
		{
			int n = 0;
			var font = Font;
			if (font is not null)
				n++;
			var text_color = TextColor;
			if (text_color is not null)
				n++;
			var text_shadow_color = TextShadowColor;
			if (text_shadow_color is not null)
				n++;
			var text_shadow_offset = TextShadowOffset;
			if (text_shadow_offset.Horizontal != 0 || text_shadow_offset.Vertical != 0)
				n++;
			if (n == 0)
				return new NSDictionary ();

			var keys = new NSObject [n];
			var values = new NSObject [n];
			n = 0;
			if (font is not null) {
				keys [n] = UITextAttributesConstants.Font;
				values [n] = font;
				n++;
			}
			if (text_color is not null) {
				keys [n] = UITextAttributesConstants.TextColor;
				values [n] = text_color;
				n++;
			}
			if (text_shadow_color is not null) {
				keys [n] = UITextAttributesConstants.TextShadowColor;
				values [n] = text_shadow_color;
				n++;
			}
			if (text_shadow_offset.Horizontal != 0 || text_shadow_offset.Vertical != 0) {
				keys [n] = UITextAttributesConstants.TextShadowOffset;
				values [n] = NSValue.FromUIOffset (text_shadow_offset);
			}
			using (NSArray avalues = NSArray.FromObjects (values),
				   akeys = NSArray.FromObjects (keys)) {
				return NSDictionary.FromObjectsAndKeysInternal (avalues, akeys);
			}
		}
	}
}

#endif // IOS
