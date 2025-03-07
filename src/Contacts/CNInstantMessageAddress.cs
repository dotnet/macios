//
// CNInstantMessageAddress.cs: Implements some nicer methods for CNInstantMessageAddress
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
	public enum CNInstantMessageAddressOption {
		/// <summary>Associated with the <see cref="P:Contacts.CNInstantMessageAddress.Username" /> property..</summary>
		Username,
		/// <summary>Associated with the <see cref="P:Contacts.CNInstantMessageAddress.Service" /> property..</summary>
		Service,
	}

	// Strong typed Keys to enum
	public enum CNInstantMessageServiceOption {
		/// <summary>AOL Instant Messenger.</summary>
		Aim,
		/// <summary>Facebook Messenger.</summary>
		Facebook,
		/// <summary>Gadu-Gadu (Polish).</summary>
		GaduGadu,
		/// <summary>Google Talk.</summary>
		GoogleTalk,
		/// <summary>ICQ.</summary>
		Icq,
		/// <summary>Jabber.</summary>
		Jabber,
		/// <summary>MSN Messenger.</summary>
		Msn,
		/// <summary>Tencent QQ.</summary>
		QQ,
		/// <summary>Microsoft Skype.</summary>
		Skype,
		/// <summary>Yahoo Messenger.</summary>
		Yahoo,
	}

	public partial class CNInstantMessageAddress {

		public static string LocalizeProperty (CNInstantMessageAddressOption property)
		{
			switch (property) {
			case CNInstantMessageAddressOption.Username:
				return LocalizeProperty (CNInstantMessageAddressKey.Username);
			case CNInstantMessageAddressOption.Service:
				return LocalizeProperty (CNInstantMessageAddressKey.Service);
			default:
				throw new ArgumentOutOfRangeException ("serviceOption");
			}
		}

		public static string LocalizeService (CNInstantMessageServiceOption serviceOption)
		{
			var srvc = ServiceOptionsToNSString (serviceOption);
			return LocalizeService (srvc);
		}

		static NSString ServiceOptionsToNSString (CNInstantMessageServiceOption serviceOption)
		{
			switch (serviceOption) {
			case CNInstantMessageServiceOption.Aim:
				return CNInstantMessageServiceKey.Aim;
			case CNInstantMessageServiceOption.Facebook:
				return CNInstantMessageServiceKey.Facebook;
			case CNInstantMessageServiceOption.GaduGadu:
				return CNInstantMessageServiceKey.GaduGadu;
			case CNInstantMessageServiceOption.GoogleTalk:
				return CNInstantMessageServiceKey.GoogleTalk;
			case CNInstantMessageServiceOption.Icq:
				return CNInstantMessageServiceKey.Icq;
			case CNInstantMessageServiceOption.Jabber:
				return CNInstantMessageServiceKey.Jabber;
			case CNInstantMessageServiceOption.Msn:
				return CNInstantMessageServiceKey.Msn;
			case CNInstantMessageServiceOption.QQ:
				return CNInstantMessageServiceKey.QQ;
			case CNInstantMessageServiceOption.Skype:
				return CNInstantMessageServiceKey.Skype;
			case CNInstantMessageServiceOption.Yahoo:
				return CNInstantMessageServiceKey.Yahoo;
			default:
				throw new ArgumentOutOfRangeException ("serviceOption");
			}
		}
	}
}
