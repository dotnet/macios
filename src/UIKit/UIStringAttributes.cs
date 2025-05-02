// 
// UIStringAttributes.cs: Implements strongly typed access to UIKit specific part of UIStringAttributeKey
//
// Authors:
//   Marek Safar (marek.safar@gmail.com)
//   Miguel de Icaza (miguel@xamarin.com)
//     
// Copyright 2012-2013, Xamarin Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;

using ObjCRuntime;
using Foundation;
using CoreFoundation;
using CoreGraphics;
using CoreText;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace UIKit {

	/// <summary>Strongly helper to define UIKit attributes for use with <see cref="Foundation.NSAttributedString" />. </summary>
	///     <remarks>
	///       <para>
	/// 	You use this class to create attributes that can be used with
	/// 	both <see cref="Foundation.NSAttributedString" />
	/// 	and <see cref="Foundation.NSMutableAttributedString" />.
	/// 	Since this class is strongly typed, you will get code
	/// 	completion as well as avoid common mistakes when using
	/// 	attributed strings with UIKit.
	///
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// // 
	/// // Using NSAttributedString with UIKit
	/// //
	/// var myText = new NSAttributedString ("Hello, world",
	///        new UIStringAttributes () {
	///               ForegroundColor =  UIColor.Red,
	/// 	      KerningAdjustment = 3
	///        });
	/// label.AttributedText = myText;
	/// 	]]></code>
	///       </example>
	///     </remarks>
	public class UIStringAttributes : DictionaryContainer {
#if !COREBUILD
		/// <summary>Default constructor</summary>
		///         <remarks>
		///         </remarks>
		public UIStringAttributes ()
			: base (new NSMutableDictionary ())
		{
		}

		/// <param name="dictionary">Dictionary to initialize from</param>
		///         <summary>Creates a UIStringAttributes from UIKit NSAttributedString attributes stored in a dictionary.</summary>
		///         <remarks>To be added.</remarks>
		public UIStringAttributes (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>Foreground Color for the text</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public UIColor ForegroundColor {
			get {
				return Dictionary [UIStringAttributeKey.ForegroundColor] as UIColor;
			}
			set {
				SetNativeValue (UIStringAttributeKey.ForegroundColor, value);
			}
		}

		/// <summary>Background Color for the text.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public UIColor BackgroundColor {
			get {
				return Dictionary [UIStringAttributeKey.BackgroundColor] as UIColor;
			}
			set {
				SetNativeValue (UIStringAttributeKey.BackgroundColor, value);
			}
		}

		/// <summary>Font to use for the text.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public UIFont Font {
			get {
				return Dictionary [UIStringAttributeKey.Font] as UIFont;
			}
			set {
				SetNativeValue (UIStringAttributeKey.Font, value);
			}
		}

		/// <summary>Kerning value used for the text</summary>
		///         <value>This value is measured in points, and the value zero is used to mean no kerning.</value>
		///         <remarks>
		///         </remarks>
		public float? KerningAdjustment {
			get {
				return GetFloatValue (UIStringAttributeKey.KerningAdjustment);
			}
			set {
				SetNumberValue (UIStringAttributeKey.KerningAdjustment, value);
			}
		}

		/// <summary>The style of ligatures to use.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSLigatureType? Ligature {
			get {
				var value = GetInt32Value (UIStringAttributeKey.Ligature);
				return value is null ? null : (NSLigatureType?) value.Value;
			}
			set {
				SetNumberValue (UIStringAttributeKey.Ligature, (int?) value);
			}
		}

		/// <summary>Used to specify a custom paragraph style.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSParagraphStyle ParagraphStyle {
			get {
				return Dictionary [UIStringAttributeKey.ParagraphStyle] as NSParagraphStyle;
			}
			set {
				SetNativeValue (UIStringAttributeKey.ParagraphStyle, value);
			}
		}

		/// <summary>Strikethrough style.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSUnderlineStyle? StrikethroughStyle {
			get {
				var value = GetInt32Value (UIStringAttributeKey.StrikethroughStyle);
				return value is null ? null : (NSUnderlineStyle?) value.Value;
			}
			set {
				SetNumberValue (UIStringAttributeKey.StrikethroughStyle, (int?) value);
			}
		}

		/// <summary>Stroke Color.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public UIColor StrokeColor {
			get {
				return Dictionary [UIStringAttributeKey.StrokeColor] as UIColor;
			}
			set {
				SetNativeValue (UIStringAttributeKey.StrokeColor, value);
			}
		}

		/// <summary>The stroke width for drawing the text</summary>
		///         <value>Expressed as percentage of the font size.   Positive values stroke the text, negative values stroke and fill the text.</value>
		///         <remarks>
		///         </remarks>
		public float? StrokeWidth {
			get {
				return GetFloatValue (UIStringAttributeKey.StrokeWidth);
			}
			set {
				SetNumberValue (UIStringAttributeKey.StrokeWidth, value);
			}
		}

		/// <summary>Shadow to use for the text.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSShadow Shadow {
			get {
				return Dictionary [UIStringAttributeKey.Shadow] as NSShadow;
			}
			set {
				SetNativeValue (UIStringAttributeKey.Shadow, value);
			}
		}

		/// <summary>Underline style for the text.</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		public NSUnderlineStyle? UnderlineStyle {
			get {
				var value = GetInt32Value (UIStringAttributeKey.UnderlineStyle);
				return value is null ? null : (NSUnderlineStyle?) value.Value;
			}
			set {
				SetNumberValue (UIStringAttributeKey.UnderlineStyle, (int?) value);
			}
		}

		/// <summary>A reference to the text effect that does not prevent garbage collection of the underlying resource.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSString WeakTextEffect {
			get {
				return Dictionary [UIStringAttributeKey.TextEffect] as NSString;
			}
			set {
				SetStringValue (UIStringAttributeKey.TextEffect, value);
			}
		}

		/// <summary>The NSTextEffect applied to the string.</summary>
		///         <value>The default value is <see langword="null" />.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSTextEffect TextEffect {
			get {
				var s = WeakTextEffect;
				if (s is null)
					return NSTextEffect.None;

				if (s == UIStringAttributeKey.NSTextEffectLetterpressStyle)
					return NSTextEffect.LetterPressStyle;
				return NSTextEffect.UnknownUseWeakEffect;
			}
			set {
				if (value == NSTextEffect.LetterPressStyle)
					SetStringValue (UIStringAttributeKey.TextEffect, UIStringAttributeKey.NSTextEffectLetterpressStyle);
				else
					SetStringValue (UIStringAttributeKey.TextEffect, (NSString) null);
			}
		}

		/// <summary>The NSTextAttachment, if any.</summary>
		///         <value>The default value is <see langword="null" />.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSTextAttachment TextAttachment {
			get {
				return Dictionary [UIStringAttributeKey.Attachment] as NSTextAttachment;
			}
			set {
				SetNativeValue (UIStringAttributeKey.Attachment, value);
			}
		}

		/// <summary>The destination URL of a hyperlink.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSUrl Link {
			get {
				return Dictionary [UIStringAttributeKey.Link] as NSUrl;
			}
			set {
				SetNativeValue (UIStringAttributeKey.Link, value);
			}
		}

		/// <summary>The distance from the bottom of the bounding box of the glyphs of the string to their baseline.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public float? BaselineOffset {
			get {
				return GetFloatValue (UIStringAttributeKey.BaselineOffset);
			}
			set {
				SetNumberValue (UIStringAttributeKey.BaselineOffset, value);
			}
		}

		/// <summary>The color to be used for the strikethrough stroke.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public UIColor StrikethroughColor {
			get {
				return Dictionary [UIStringAttributeKey.StrikethroughColor] as UIColor;
			}
			set {
				SetNativeValue (UIStringAttributeKey.StrikethroughColor, value);
			}
		}

		/// <summary>The color of the underline stroke.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public UIColor UnderlineColor {
			get {
				return Dictionary [UIStringAttributeKey.UnderlineColor] as UIColor;
			}
			set {
				SetNativeValue (UIStringAttributeKey.UnderlineColor, value);
			}
		}


		/// <summary>The amount of skew to apply to glyphs.</summary>
		///         <value>The default value of 0 indicates no skew.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public float? Obliqueness {
			get {
				return GetFloatValue (UIStringAttributeKey.Obliqueness);
			}
			set {
				SetNumberValue (UIStringAttributeKey.Obliqueness, value);
			}
		}

		/// <summary>The log of the expansion factor to be applied to glyphs.</summary>
		///         <value>The default value is 0, indicating no expansion.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public float? Expansion {
			get {
				return GetFloatValue (UIStringAttributeKey.Expansion);
			}
			set {
				SetNumberValue (UIStringAttributeKey.Expansion, value);
			}
		}

		/// <summary>An array indicating the writing-direction overrides.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public NSNumber [] WritingDirectionInt {
			get {
				return GetArray<NSNumber> (UIStringAttributeKey.WritingDirection);
			}
			set {
				SetArrayValue (UIStringAttributeKey.WritingDirection, value);
			}
		}
#endif
	}
}
