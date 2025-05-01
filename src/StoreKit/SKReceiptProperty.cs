//
// SKReceiptProperty.cs: strongly typed dictionary for options in StoreKit
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2013 Xamarin Inc.
//

#nullable enable

using ObjCRuntime;
using Foundation;
using CoreFoundation;
using StoreKit;

#if !MONOMAC
using UIKit;
#endif
using System;

namespace StoreKit {
	/// <summary>Defines test properties for the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=C:StoreKit.SKReceiptRefreshRequest(Foundation.NSDictionary)&amp;scope=Xamarin" title="C:StoreKit.SKReceiptRefreshRequest(Foundation.NSDictionary)">C:StoreKit.SKReceiptRefreshRequest(Foundation.NSDictionary)</a></format> constructor.</summary>
	///     <remarks>To be added.</remarks>
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
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SKReceiptProperties ()
			: base (new NSMutableDictionary ())
		{
		}

		/// <param name="dictionary">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SKReceiptProperties (NSDictionary dictionary)
			: base (dictionary)
		{
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsExpired {
			get {
				return GetInt32Value (_SKReceiptProperty.IsExpired) != 0;
			}
			set {
				SetNumberValue (_SKReceiptProperty.IsExpired, value ? 1 : 0);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsRevoked {
			get {
				return GetInt32Value (_SKReceiptProperty.IsRevoked) != 0;
			}
			set {
				SetNumberValue (_SKReceiptProperty.IsRevoked, value ? 1 : 0);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
