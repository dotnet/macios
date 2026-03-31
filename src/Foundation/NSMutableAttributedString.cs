// 
// NSMutableAttributedString.cs: Helpers and overloads for NSMutableAttributedString members.
//
// Authors: Mono Team
//     
// Copyright 2010 Novell, Inc
// Copyright 2011, 2012 Xamarin Inc
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
//

#if !MONOMAC
using UIKit;
#endif

using CoreText;

#nullable enable

namespace Foundation {

	public partial class NSMutableAttributedString {

		/// <summary>
		/// Creates an NSMutableAttributedString from a C# string and applies the specified CoreText attributes to the entire string.
		/// </summary>
		/// <param name="str">The string content.</param>
		/// <param name="attributes">CoreText attributes to be applied to the string.</param>
		public NSMutableAttributedString (string str, CTStringAttributes? attributes)
			: this (str, attributes?.Dictionary)
		{
		}

		/// <summary>
		/// Sets the attributes for the specified range. Any previous attributes in that range are replaced with the new values.
		/// </summary>
		/// <param name="attributes">The attributes to set.</param>
		/// <param name="range">The range to which the attributes will be applied.</param>
		public void SetAttributes (NSDictionary attributes, NSRange range)
		{
			ArgumentNullException.ThrowIfNull (attributes);

			LowLevelSetAttributes (attributes.Handle, range);
			GC.KeepAlive (attributes);
		}

		/// <summary>
		/// Sets the attributes for the specified range. Any previous attributes in that range are replaced with the new values.
		/// </summary>
		/// <param name="attrs">CoreText attributes to be set on the string.</param>
		/// <param name="range">The range to which the attributes will be applied.</param>
		public void SetAttributes (CTStringAttributes attrs, NSRange range)
		{
			ArgumentNullException.ThrowIfNull (attrs);

			SetAttributes (attrs.Dictionary, range);
		}

		/// <summary>
		/// Adds attributes to the specified range of characters in the string.
		/// </summary>
		/// <param name="attrs">The CoreText attributes to add.</param>
		/// <param name="range">The range to which the attributes will be applied.</param>
		public void AddAttributes (CTStringAttributes attrs, NSRange range)
		{
			ArgumentNullException.ThrowIfNull (attrs);

			AddAttributes (attrs.Dictionary, range);
		}

		/// <summary>
		/// Appends an attributed string and additional objects to the end of the receiver.
		/// </summary>
		/// <param name="first">The first attributed string to append.</param>
		/// <param name="rest">Additional objects to append. Can be <see cref="NSAttributedString"/>, <see cref="string"/>, or other objects whose <see cref="object.ToString"/> method will be called.</param>
		public void Append (NSAttributedString first, params object [] rest)
		{
			Append (first);
			foreach (var obj in rest) {
				if (obj is NSAttributedString nsAttributedString)
					Append (nsAttributedString);
				else if (obj is string str)
					Append (new NSAttributedString (str));
				else
					Append (new NSAttributedString (obj?.ToString () ?? ""));

			}
		}
#if !MONOMAC
		/// <summary>
		/// Creates an NSMutableAttributedString from a string with UIKit attributes.
		/// </summary>
		/// <param name="str">The string content.</param>
		/// <param name="attributes">UIKit attributes to be applied to the string.</param>
		public NSMutableAttributedString (string str, UIStringAttributes? attributes)
		: this (str, attributes?.Dictionary)
		{
		}

		/// <summary>
		/// Creates an NSMutableAttributedString from a string with individual UIKit styling attributes.
		/// </summary>
		/// <param name="str">The string content.</param>
		/// <param name="font">The font to apply to the string.</param>
		/// <param name="foregroundColor">The foreground (text) color.</param>
		/// <param name="backgroundColor">The background color.</param>
		/// <param name="strokeColor">The stroke color for outlined text.</param>
		/// <param name="paragraphStyle">The paragraph style to apply.</param>
		/// <param name="ligatures">The ligature type to use.</param>
		/// <param name="kerning">The kerning value to apply.</param>
		/// <param name="underlineStyle">The underline style to apply.</param>
		/// <param name="shadow">The shadow effect to apply.</param>
		/// <param name="strokeWidth">The stroke width for outlined text.</param>
		/// <param name="strikethroughStyle">The strikethrough style to apply.</param>
		public NSMutableAttributedString (string str,
						  UIFont? font = null,
						  UIColor? foregroundColor = null,
						  UIColor? backgroundColor = null,
						  UIColor? strokeColor = null,
						  NSParagraphStyle? paragraphStyle = null,
						  NSLigatureType ligatures = NSLigatureType.Default,
						  float kerning = 0,
						  NSUnderlineStyle underlineStyle = NSUnderlineStyle.None,
						  NSShadow? shadow = null,
						  float strokeWidth = 0,
						  NSUnderlineStyle strikethroughStyle = NSUnderlineStyle.None)
		: this (str, ToDictionary (font, foregroundColor, backgroundColor, strokeColor, paragraphStyle, ligatures, kerning, underlineStyle, shadow, strokeWidth, strikethroughStyle))
		{
		}
#endif
	}
}
