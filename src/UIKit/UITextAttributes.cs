//
// UITextAttributes.cs: strongly typed version of the UITextAttribetus
// that can be used to convert back and from NSDictionaries for the
// underlying Objective-C API
//
// Copyright 2011, 2013, 2015 Xamarin Inc.
//

#if IOS

using Foundation;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	/// <include file="../../docs/api/UIKit/UITextAttributes.xml" path="/Documentation/Docs[@DocId='T:UIKit.UITextAttributes']/*" />
	public class UITextAttributes {
		/// <summary>The font to use to render the text.</summary>
		///         <remarks>
		///         </remarks>
		public UIFont Font;
		/// <summary>The color to use for the text.</summary>
		///         <remarks>
		///         </remarks>
		public UIColor TextColor;
		/// <summary>If you set the TextShadowOffset, the color to use for the shadow of the text.</summary>
		///         <remarks>
		///         </remarks>
		public UIColor TextShadowColor;
		/// <summary>The offset describing the distance between the text and its shadow.</summary>
		///         <remarks>
		///         </remarks>
		public UIOffset TextShadowOffset;

		/// <summary>The default constructor does nothing, you must fill at least one property for this to be useful.</summary>
		///         <remarks>
		///         </remarks>
		public UITextAttributes ()
		{
		}

		internal UITextAttributes (NSDictionary dict)
		{
			if (dict is null)
				return;

			NSObject val;

			if (dict.TryGetValue (UITextAttributesConstants.Font, out val))
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
