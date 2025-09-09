//
// CNContact.cs: Implements some nicer methods for CNContact
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;

namespace Contacts {

	public partial class CNContact {

		/// <param name="options">To be added.</param>
		///         <summary>Whether the key described in <paramref name="options" /> is available for this <see cref="Contacts.CNContact" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public virtual bool IsKeyAvailable (CNContactOptions options)
		{
			var key = ContactOptionsToNSString (options);
			return IsKeyAvailable (key);
		}

		/// <param name="options">To be added.</param>
		///         <summary>Gets the localized version of the key described in <paramref name="options" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string LocalizeProperty (CNContactOptions options)
		{
			var key = ContactOptionsToNSString (options);
			return LocalizeProperty (key);
		}

		static NSString ContactOptionsToNSString (CNContactOptions options)
		{
			switch (options) {
			case CNContactOptions.Nickname:
				return CNContactKey.Nickname;
			case CNContactOptions.PhoneticGivenName:
				return CNContactKey.PhoneticGivenName;
			case CNContactOptions.PhoneticMiddleName:
				return CNContactKey.PhoneticMiddleName;
			case CNContactOptions.PhoneticFamilyName:
				return CNContactKey.PhoneticFamilyName;
			case CNContactOptions.OrganizationName:
				return CNContactKey.OrganizationName;
			case CNContactOptions.DepartmentName:
				return CNContactKey.DepartmentName;
			case CNContactOptions.JobTitle:
				return CNContactKey.JobTitle;
			case CNContactOptions.Birthday:
				return CNContactKey.Birthday;
			case CNContactOptions.NonGregorianBirthday:
				return CNContactKey.NonGregorianBirthday;
			case CNContactOptions.Note:
				return CNContactKey.Note;
#if !MONOMAC
			case CNContactOptions.ImageData:
				return CNContactKey.ImageData;
			case CNContactOptions.ImageDataAvailable:
				return CNContactKey.ImageDataAvailable;
#endif
			case CNContactOptions.ThumbnailImageData:
				return CNContactKey.ThumbnailImageData;
			case CNContactOptions.Type:
				return CNContactKey.Type;
			case CNContactOptions.PhoneNumbers:
				return CNContactKey.PhoneNumbers;
			case CNContactOptions.EmailAddresses:
				return CNContactKey.EmailAddresses;
			case CNContactOptions.PostalAddresses:
				return CNContactKey.PostalAddresses;
			case CNContactOptions.Dates:
				return CNContactKey.Dates;
			case CNContactOptions.UrlAddresses:
				return CNContactKey.UrlAddresses;
			case CNContactOptions.Relations:
				return CNContactKey.Relations;
			case CNContactOptions.SocialProfiles:
				return CNContactKey.SocialProfiles;
			case CNContactOptions.InstantMessageAddresses:
				return CNContactKey.InstantMessageAddresses;
			default:
				throw new ArgumentOutOfRangeException ("contactOption");
			}
		}

		/// <typeparam name="T">To be added.</typeparam>
		///         <param name="keyDescriptors">To be added.</param>
		///         <summary>Whether the keys described in <paramref name="keyDescriptors" /> are available.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool AreKeysAvailable<T> (T [] keyDescriptors)
			where T : INSObjectProtocol, INSSecureCoding, INSCopying
		{
			using (var array = NSArray.From<T> (keyDescriptors))
				return AreKeysAvailable (array);
		}

		/// <param name="options">To be added.</param>
		///         <summary>Whether the keys specified in <paramref name="options" /> are available.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public bool AreKeysAvailable (CNContactOptions options)
		{
			using (var array = new NSMutableArray ()) {
				foreach (var value in Enum.GetValues<CNContactOptions> ()) {
					if ((options & value) != CNContactOptions.None)
						array.Add (ContactOptionsToNSString (value));
				}
				return AreKeysAvailable (array);
			}
		}
	}
}
