// 
// NSMutableAttributedString.cs: Helpers and overloads for NSMutableAttributedString members on UIKit
//
//     
// Copyright 2013 Xamarin Inc
//
//

#if !MONOMAC

using UIKit;
using CoreText;

#nullable enable

namespace Foundation {

	public partial class NSMutableAttributedString {
		/// <summary>
		/// Sets the attributes for the specified range. Any previous attributes in that range are replaced with the new values.
		/// </summary>
		/// <param name="attrs">The UIKit attributes to set.</param>
		/// <param name="range">The range to which the attributes will be applied.</param>
		public void SetAttributes (UIStringAttributes attrs, NSRange range)
		{
			ArgumentNullException.ThrowIfNull (attrs);

			SetAttributes (attrs.Dictionary, range);
		}

		/// <summary>
		/// Adds attributes to the specified range of characters in the string.
		/// </summary>
		/// <param name="attrs">The UIKit attributes to add.</param>
		/// <param name="range">The range to which the attributes will be applied.</param>
		public void AddAttributes (UIStringAttributes attrs, NSRange range)
		{
			ArgumentNullException.ThrowIfNull (attrs);

			AddAttributes (attrs.Dictionary, range);
		}

	}
}

#endif // !MONOMAC
