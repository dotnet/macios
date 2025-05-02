// 
// StoreProductParameters.cs: Implements strongly typed access for SKStoreProductParameters
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, Xamarin Inc.
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

#nullable enable

using System;

using Foundation;
using CoreFoundation;
using ObjCRuntime;

namespace StoreKit {

#if __TVOS__
	/// <summary>A subclass of <see cref="Foundation.DictionaryContainer" /> that specifies the product to be displayed.</summary>
#else
	/// <summary>A subclass of <see cref="Foundation.DictionaryContainer" /> that, when passed to <see cref="StoreKit.SKStoreProductViewController.LoadProduct(StoreKit.StoreProductParameters,System.Action{System.Boolean,Foundation.NSError})" />, specifies the product to be displayed.</summary>
#endif
	public partial class StoreProductParameters : DictionaryContainer {
#if !COREBUILD
		/// <param name="iTunesItemIdentifier">To be added.</param>
		///         <summary>Creates a new <see cref="StoreKit.StoreProductParameters" /> for the specified ITunes identifier.</summary>
		///         <remarks>To be added.</remarks>
		public StoreProductParameters (int iTunesItemIdentifier)
			: this ()
		{
			ITunesItemIdentifier = iTunesItemIdentifier;
		}

		// TODO: What is real iTunes Store item identifier length
		/// <summary>Gets or sets the identifier for the ITunes item being advertised.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public int? ITunesItemIdentifier {
			set {
				SetNumberValue (SKStoreProductParameterKey.ITunesItemIdentifier, value);
			}
			get {
				return GetInt32Value (SKStoreProductParameterKey.ITunesItemIdentifier);
			}
		}

		/// <summary>Gets or sets a key for the affiliate token.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? AffiliateToken {
			get {
				return GetStringValue (SKStoreProductParameterKey.AffiliateToken);
			}
			set {
				SetStringValue (SKStoreProductParameterKey.AffiliateToken, value);
			}
		}

		/// <summary>Gets or sets a key for the campaign token.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? CampaignToken {
			get {
				return GetStringValue (SKStoreProductParameterKey.CampaignToken);
			}
			set {
				SetStringValue (SKStoreProductParameterKey.CampaignToken, value);
			}
		}
#endif
	}
}
