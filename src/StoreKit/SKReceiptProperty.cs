//
// SKReceiptProperty.cs: strongly typed dictionary for options in StoreKit
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2013 Xamarin Inc.
//

#nullable enable

using CoreFoundation;
using StoreKit;

#if !MONOMAC
using UIKit;
#endif

namespace StoreKit {
	/// <summary>Defines test properties for the <see cref="SKReceiptRefreshRequest" /> constructor.</summary>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("ios18.0")]
	[ObsoletedOSPlatform ("maccatalyst18.0")]
	[ObsoletedOSPlatform ("macos15.0")]
	[ObsoletedOSPlatform ("tvos18.0")]
	public partial class SKReceiptProperties : DictionaryContainer {
#if !COREBUILD
		/// <summary>Creates a new empty <see cref="SKReceiptProperties" /> instance.</summary>
		public SKReceiptProperties ()
			: base (new NSMutableDictionary ())
		{
		}

		/// <summary>Creates a new <see cref="SKReceiptProperties" /> instance backed by the specified dictionary.</summary>
		/// <param name="dictionary">The dictionary containing the receipt property values.</param>
		public SKReceiptProperties (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>Gets or sets a value indicating whether the receipt represents an expired subscription.</summary>
		/// <value><see langword="true" /> if the receipt is expired; otherwise, <see langword="false" />.</value>
		public bool IsExpired {
			get {
				return GetInt32Value (_SKReceiptProperty.IsExpired) != 0;
			}
			set {
				SetNumberValue (_SKReceiptProperty.IsExpired, value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets a value indicating whether the receipt has been revoked.</summary>
		/// <value><see langword="true" /> if the receipt is revoked; otherwise, <see langword="false" />.</value>
		public bool IsRevoked {
			get {
				return GetInt32Value (_SKReceiptProperty.IsRevoked) != 0;
			}
			set {
				SetNumberValue (_SKReceiptProperty.IsRevoked, value ? 1 : 0);
			}
		}

		/// <summary>Gets or sets a value indicating whether the receipt represents a volume purchase.</summary>
		/// <value><see langword="true" /> if the receipt is for a volume purchase; otherwise, <see langword="false" />.</value>
		public bool IsVolumePurchase {
			get {
				return GetInt32Value (_SKReceiptProperty.IsVolumePurchase) != 0;
			}
			set {
				SetNumberValue (_SKReceiptProperty.IsVolumePurchase, value ? 1 : 0);
			}
		}
#endif

	}
}
