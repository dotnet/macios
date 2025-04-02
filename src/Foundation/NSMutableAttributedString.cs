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

using System;
#if !MONOMAC
using UIKit;
#endif

using CoreText;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSMutableAttributedString {

		/// <param name="str">C# string.</param>
		///         <param name="attributes">CoreText attributes to be applied to the string.</param>
		///         <summary>Creates an NSMutableAttributedString from a C# string and applies the specified CoreText attributes to the entire string.</summary>
		///         <remarks>
		///         </remarks>
		public NSMutableAttributedString (string str, CTStringAttributes attributes)
			: this (str, attributes is null ? null : attributes.Dictionary)
		{
		}

		/// <param name="attributes">To be added.</param>
		///         <param name="range">Range to which the attribute will be applied.</param>
		///         <summary>Sets the attributes for the specified ranges.  Any previous attributes in that range are replaces with the new values.</summary>
		///         <remarks>
		///         </remarks>
		public void SetAttributes (NSDictionary attributes, NSRange range)
		{
			if (attributes is null)
				throw new ArgumentNullException ("attributes");

			LowLevelSetAttributes (attributes.Handle, range);
			GC.KeepAlive (attributes);
		}

		/// <param name="attrs">CoreText attributes to be set on the string.</param>
		///         <param name="range">Range to which the attribute will be applied.</param>
		///         <summary>Sets the attributes for the specified ranges.  Any previous attributes in that range are replaces with the new values.</summary>
		///         <remarks>
		///         </remarks>
		public void SetAttributes (CTStringAttributes attrs, NSRange range)
		{
			SetAttributes (attrs is null ? null : attrs.Dictionary, range);
		}

		/// <param name="attrs">The CoreText attributes to add.</param>
		///         <param name="range">Range to which the attribute will be applied.</param>
		///         <summary>Adds an attribute and its value to the specified range of characters in the string.</summary>
		///         <remarks>
		///         </remarks>
		public void AddAttributes (CTStringAttributes attrs, NSRange range)
		{
			AddAttributes (attrs is null ? null : attrs.Dictionary, range);
		}

		/// <param name="first">To be added.</param>
		///         <param name="rest">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void Append (NSAttributedString first, params object [] rest)
		{
			Append (first);
			foreach (var obj in rest) {
				if (obj is NSAttributedString)
					Append ((NSAttributedString) obj);
				else if (obj is string)
					Append (new NSAttributedString ((string) obj));
				else
					Append (new NSAttributedString (obj.ToString ()));

			}
		}
#if !MONOMAC
		/// <param name="str">To be added.</param>
		///         <param name="attributes">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableAttributedString (string str, UIStringAttributes attributes)
		: this (str, attributes is not null ? attributes.Dictionary : null)
		{
		}

		/// <param name="str">To be added.</param>
		///         <param name="font">To be added.</param>
		///         <param name="foregroundColor">To be added.</param>
		///         <param name="backgroundColor">To be added.</param>
		///         <param name="strokeColor">To be added.</param>
		///         <param name="paragraphStyle">To be added.</param>
		///         <param name="ligatures">To be added.</param>
		///         <param name="kerning">To be added.</param>
		///         <param name="underlineStyle">To be added.</param>
		///         <param name="shadow">To be added.</param>
		///         <param name="strokeWidth">To be added.</param>
		///         <param name="strikethroughStyle">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public NSMutableAttributedString (string str,
						  UIFont font = null,
						  UIColor foregroundColor = null,
						  UIColor backgroundColor = null,
						  UIColor strokeColor = null,
						  NSParagraphStyle paragraphStyle = null,
						  NSLigatureType ligatures = NSLigatureType.Default,
						  float kerning = 0,
						  NSUnderlineStyle underlineStyle = NSUnderlineStyle.None,
						  NSShadow shadow = null,
						  float strokeWidth = 0,
						  NSUnderlineStyle strikethroughStyle = NSUnderlineStyle.None)
		: this (str, ToDictionary (font, foregroundColor, backgroundColor, strokeColor, paragraphStyle, ligatures, kerning, underlineStyle, shadow, strokeWidth, strikethroughStyle))
		{
		}
#endif
	}
}
