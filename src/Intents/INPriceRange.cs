//
// INPriceRange extensions and syntax sugar
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if IOS
using System;
using Foundation;

#nullable enable

namespace Intents {
	public enum INPriceRangeOption {
		/// <summary>The greatest price.</summary>
		Maximum,
		/// <summary>The lowest price.</summary>
		Minimum,
	}

	public partial class INPriceRange {

		/// <param name="option">To be added.</param>
		///         <param name="price">To be added.</param>
		///         <param name="currencyCode">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public INPriceRange (INPriceRangeOption option, NSDecimalNumber price, string currencyCode)
			: base (NSObjectFlag.Empty)
		{
			switch (option) {
			case INPriceRangeOption.Maximum:
				InitializeHandle (InitWithMaximumPrice (price, currencyCode));
				break;
			case INPriceRangeOption.Minimum:
				InitializeHandle (InitWithMinimumPrice (price, currencyCode));
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (option), option, "Invalid enum value.");
			}
		}
	}
}
#endif // IOS
