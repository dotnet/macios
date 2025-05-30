//
// CNSocialProfile.cs: Implements some nicer methods for CNSocialProfile
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;

namespace Contacts {
	// Strong typed Keys to enum
	/// <summary>Enumerates properties of social services that are always fetched.</summary>
	///     <remarks>To be added.</remarks>
	public enum CNSocialProfileOption {
		/// <summary>Associated with the <see cref="Contacts.CNSocialProfileKey.UrlString" /> property.</summary>
		UrlString,
		/// <summary>Associated with the <see cref="Contacts.CNSocialProfileKey.Username" /> property.</summary>
		Username,
		/// <summary>Associated with the <see cref="Contacts.CNSocialProfileKey.UserIdentifier" /> property.</summary>
		UserIdentifier,
		/// <summary>Associated with the <see cref="Contacts.CNSocialProfileKey.Service" /> property.</summary>
		Service,
	}

	// Strong typed Keys to enum
	/// <summary>Enumerates known social services.</summary>
	///     <remarks>To be added.</remarks>
	public enum CNSocialProfileServiceOption {
		/// <summary>Facebook.</summary>
		Facebook,
		/// <summary>Flickr.</summary>
		Flickr,
		/// <summary>LinkedIn.</summary>
		LinkedIn,
		/// <summary>MySpace.</summary>
		MySpace,
		/// <summary>SinaWeibo.</summary>
		SinaWeibo,
		/// <summary>TencentWeibo.</summary>
		TencentWeibo,
		/// <summary>Twitter.</summary>
		Twitter,
		/// <summary>Yelp.</summary>
		Yelp,
		/// <summary>Apple GameCenter.</summary>
		GameCenter,
	}

	public partial class CNSocialProfile {

		/// <param name="option">To be added.</param>
		///         <summary>Returns the localized string representing the <paramref name="option" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string LocalizeProperty (CNSocialProfileOption option)
		{
			switch (option) {
			case CNSocialProfileOption.UrlString:
				return LocalizeProperty (CNSocialProfileKey.UrlString);
			case CNSocialProfileOption.Username:
				return LocalizeProperty (CNSocialProfileKey.Username);
			case CNSocialProfileOption.UserIdentifier:
				return LocalizeProperty (CNSocialProfileKey.UserIdentifier);
			case CNSocialProfileOption.Service:
				return LocalizeProperty (CNSocialProfileKey.Service);
			default:
				throw new ArgumentOutOfRangeException ("option");
			}
		}

		/// <param name="serviceOption">To be added.</param>
		///         <summary>Returns the localized string representing the <paramref name="serviceOption" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string LocalizeService (CNSocialProfileServiceOption serviceOption)
		{
			var srvc = ServiceOptionsToNSString (serviceOption);
			return LocalizeService (srvc);
		}

		static NSString ServiceOptionsToNSString (CNSocialProfileServiceOption serviceOption)
		{
			switch (serviceOption) {
			case CNSocialProfileServiceOption.Facebook:
				return CNSocialProfileServiceKey.Facebook;
			case CNSocialProfileServiceOption.Flickr:
				return CNSocialProfileServiceKey.Flickr;
			case CNSocialProfileServiceOption.LinkedIn:
				return CNSocialProfileServiceKey.LinkedIn;
			case CNSocialProfileServiceOption.MySpace:
				return CNSocialProfileServiceKey.MySpace;
			case CNSocialProfileServiceOption.SinaWeibo:
				return CNSocialProfileServiceKey.SinaWeibo;
			case CNSocialProfileServiceOption.TencentWeibo:
				return CNSocialProfileServiceKey.TencentWeibo;
			case CNSocialProfileServiceOption.Twitter:
				return CNSocialProfileServiceKey.Twitter;
			case CNSocialProfileServiceOption.Yelp:
				return CNSocialProfileServiceKey.Yelp;
			case CNSocialProfileServiceOption.GameCenter:
				return CNSocialProfileServiceKey.GameCenter;
			default:
				throw new ArgumentOutOfRangeException ("serviceOption");
			}
		}
	}
}
