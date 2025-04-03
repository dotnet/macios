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
	public enum CNSocialProfileOption {
		/// <summary>Associated with the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Contacts%20CNSocial%20Service%20Url%20String&amp;scope=Xamarin" title="P:Contacts.CNSocialService.UrlString">P:Contacts.CNSocialService.UrlString</a></format> property.</summary>
		UrlString,
		/// <summary>Associated with the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Contacts%20CNSocial%20Service%20Username&amp;scope=Xamarin" title="P:Contacts.CNSocialService.Username">P:Contacts.CNSocialService.Username</a></format> property.</summary>
		Username,
		/// <summary>Associated with the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Contacts%20CNSocial%20Service%20User%20Identifier&amp;scope=Xamarin" title="P:Contacts.CNSocialService.UserIdentifier">P:Contacts.CNSocialService.UserIdentifier</a></format> property.</summary>
		UserIdentifier,
		/// <summary>Associated with the <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=Contacts%20CNSocial%20Service%20Service&amp;scope=Xamarin" title="P:Contacts.CNSocialService.Service">P:Contacts.CNSocialService.Service</a></format> property.</summary>
		Service,
	}

	// Strong typed Keys to enum
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
