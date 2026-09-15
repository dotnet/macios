//
// INPriceRange extensions and syntax sugar
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if IOS

#nullable enable

namespace Intents {
	/// <summary>Enumerates the minimum and maximum values of a price range.</summary>
	public enum INPriceRangeOption {
		/// <summary>The greatest price.</summary>
		Maximum,
		/// <summary>The lowest price.</summary>
		Minimum,
	}

	public partial class INPriceRange {

		/// <summary>Creates a price range with either a minimum or maximum price.</summary>
		/// <param name="option">Whether <paramref name="price" /> is the minimum or maximum price.</param>
		/// <param name="price">The price at the selected range boundary.</param>
		/// <param name="currencyCode">The ISO 4217 currency code for <paramref name="price" />.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="option" /> is not a valid value.</exception>
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
