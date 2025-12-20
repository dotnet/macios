//
// NSLocale.cs: extensions for the NSLocale class
//
// Authors:
//   Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2011 Xamarin Inc.
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

#nullable enable

namespace Foundation {

	public partial class NSLocale {
		/// <summary>Gets the locale identifier string.</summary>
		/// <value>The locale identifier.</value>
		public string? Identifier {
			get {
				return (string?) (NSString?) ObjectForKey (_Identifier);
			}
		}

		/// <summary>Gets the display name for the specified locale identifier.</summary>
		/// <param name="value">The locale identifier.</param>
		/// <returns>The localized display name for the identifier, or <see langword="null" /> if not available.</returns>
		public string? GetIdentifierDisplayName (string value)
		{
			ArgumentNullException.ThrowIfNull (value);
			return DisplayNameForKey (_Identifier, value);
		}

		/// <summary>Gets the language code for the locale.</summary>
		/// <value>The language code.</value>
		public string? LanguageCode {
			get {
				return (string?) (NSString?) ObjectForKey (_LanguageCode);
			}
		}

		/// <summary>Gets the display name for the specified language code.</summary>
		/// <param name="value">The language code.</param>
		/// <returns>The localized display name for the language code, or <see langword="null" /> if not available.</returns>
		public string? GetLanguageCodeDisplayName (string value)
		{
			ArgumentNullException.ThrowIfNull (value);
			return DisplayNameForKey (_LanguageCode, value);
		}

		/// <summary>Gets the country code for the locale.</summary>
		/// <value>The country code.</value>
		public string? CountryCode {
			get {
				return (string?) (NSString?) ObjectForKey (_CountryCode);
			}
		}

		/// <summary>Gets the display name for the specified country code.</summary>
		/// <param name="value">The country code.</param>
		/// <returns>The localized display name for the country code, or <see langword="null" /> if not available.</returns>
		public string? GetCountryCodeDisplayName (string value)
		{
			ArgumentNullException.ThrowIfNull (value);
			return DisplayNameForKey (_CountryCode, value);
		}

		/// <summary>Gets the script code for the locale.</summary>
		/// <value>The script code.</value>
		public string? ScriptCode {
			get {
				return (string?) (NSString?) ObjectForKey (_ScriptCode);
			}
		}

		/// <summary>Gets the variant code for the locale.</summary>
		/// <value>The variant code.</value>
		public string? VariantCode {
			get {
				return (string?) (NSString?) ObjectForKey (_VariantCode);
			}
		}

		/// <summary>Gets the exemplar character set for the locale.</summary>
		/// <value>The character set containing exemplar characters.</value>
		public NSCharacterSet? ExemplarCharacterSet {
			get {
				return ObjectForKey (_ExemplarCharacterSet) as NSCharacterSet;
			}
		}

		/// <summary>Gets the calendar for the locale.</summary>
		/// <value>The <see cref="NSCalendar" /> object associated with the locale.</value>
		public NSCalendar? Calendar {
			get {
				return ObjectForKey (_Calendar) as NSCalendar;
			}
		}

		/// <summary>Gets the collation identifier for the locale.</summary>
		/// <value>The collation identifier.</value>
		public string? CollationIdentifier {
			get {
				return (string?) (NSString?) ObjectForKey (_CollationIdentifier);
			}
		}

		/// <summary>Gets a value indicating whether the locale uses the metric system.</summary>
		/// <value><see langword="true" /> if the locale uses the metric system; otherwise, <see langword="false" />.</value>
		public bool UsesMetricSystem {
			get {
				return (ObjectForKey (_UsesMetricSystem) as NSNumber)?.BoolValue == true;
			}
		}

		/// <summary>Gets the measurement system for the locale.</summary>
		/// <value>The measurement system identifier.</value>
		public string? MeasurementSystem {
			get {
				return (string?) (NSString?) ObjectForKey (_MeasurementSystem);
			}
		}

		/// <summary>Gets the decimal separator for the locale.</summary>
		/// <value>The decimal separator character.</value>
		public string? DecimalSeparator {
			get {
				return (string?) (NSString?) ObjectForKey (_DecimalSeparator);
			}
		}

		/// <summary>Gets the grouping separator for the locale.</summary>
		/// <value>The grouping separator character used in numbers.</value>
		public string? GroupingSeparator {
			get {
				return (string?) (NSString?) ObjectForKey (_GroupingSeparator);
			}
		}

		/// <summary>Gets the currency symbol for the locale.</summary>
		/// <value>The currency symbol.</value>
		public string? CurrencySymbol {
			get {
				return (string?) (NSString?) ObjectForKey (_CurrencySymbol);
			}
		}

		/// <summary>Gets the currency code for the locale.</summary>
		/// <value>The ISO 4217 currency code.</value>
		public string? CurrencyCode {
			get {
				return (string?) (NSString?) ObjectForKey (_CurrencyCode);
			}
		}

		/// <summary>Gets the display name for the specified currency code.</summary>
		/// <param name="value">The currency code.</param>
		/// <returns>The localized display name for the currency code, or <see langword="null" /> if not available.</returns>
		public string? GetCurrencyCodeDisplayName (string value)
		{
			ArgumentNullException.ThrowIfNull (value);
			return DisplayNameForKey (_CurrencyCode, value);
		}

		/// <summary>Gets the collator identifier for the locale.</summary>
		/// <value>The collator identifier.</value>
		public string? CollatorIdentifier {
			get {
				return (string?) (NSString?) ObjectForKey (_CollatorIdentifier);
			}
		}

		/// <summary>Gets the quotation begin delimiter for the locale.</summary>
		/// <value>The character or string used to begin a quotation.</value>
		public string? QuotationBeginDelimiterKey {
			get {
				return (string?) (NSString?) ObjectForKey (_QuotationBeginDelimiterKey);
			}
		}

		/// <summary>Gets the quotation end delimiter for the locale.</summary>
		/// <value>The character or string used to end a quotation.</value>
		public string? QuotationEndDelimiterKey {
			get {
				return (string?) (NSString?) ObjectForKey (_QuotationEndDelimiterKey);
			}
		}

		/// <summary>Gets the alternate quotation begin delimiter for the locale.</summary>
		/// <value>The character or string used to begin an alternate (nested) quotation.</value>
		public string? AlternateQuotationBeginDelimiterKey {
			get {
				return (string?) (NSString?) ObjectForKey (_AlternateQuotationBeginDelimiterKey);
			}
		}

		/// <summary>Gets the alternate quotation end delimiter for the locale.</summary>
		/// <value>The character or string used to end an alternate (nested) quotation.</value>
		public string? AlternateQuotationEndDelimiterKey {
			get {
				return (string?) (NSString?) ObjectForKey (_AlternateQuotationEndDelimiterKey);
			}
		}
	}
}
