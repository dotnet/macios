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

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Foundation {

	public partial class NSMutableAttributedString {
		/// <param name="attrs">To be added.</param>
		///         <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetAttributes (UIStringAttributes attrs, NSRange range)
		{
			SetAttributes (attrs is null ? null : attrs.Dictionary, range);
		}

		/// <param name="attrs">To be added.</param>
		///         <param name="range">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void AddAttributes (UIStringAttributes attrs, NSRange range)
		{
			AddAttributes (attrs is null ? null : attrs.Dictionary, range);
		}

	}
}

#endif // !MONOMAC
