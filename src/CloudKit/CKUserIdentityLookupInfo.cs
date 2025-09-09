#nullable enable

using ObjCRuntime;
using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CloudKit {
	public partial class CKUserIdentityLookupInfo {
		// extra parameter to get a unique signature for a string argument
		CKUserIdentityLookupInfo (string id, int type)
			: base (NSObjectFlag.Empty)
		{
			switch (type) {
			case 0:
				InitializeHandle (_FromEmail (id), "initWithEmailAddress:");
				break;
			case 1:
				InitializeHandle (_FromPhoneNumber (id), "initWithPhoneNumber:");
				break;
			default:
				throw new ArgumentOutOfRangeException (nameof (type));
			}
		}

		/// <summary>Creates a new <see cref="CKUserIdentityLookupInfo" /> using the specified <paramref name="email" /> address.</summary>
		/// <param name="email">The email to use in the lookup.</param>
		/// <returns>A new <see cref="CKUserIdentityLookupInfo" /> instance for the specified email.</returns>
		public static CKUserIdentityLookupInfo FromEmail (string email)
		{
			if (string.IsNullOrEmpty (email))
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (email));
			return new CKUserIdentityLookupInfo (email, 0);
		}

		/// <summary>Creates a new <see cref="CloudKit.CKUserIdentityLookupInfo" /> using the specified <paramref name="phoneNumber" />.</summary>
		/// <param name="phoneNumber">The phone number to use in the lookup.</param>
		/// <returns>A new <see cref="CKUserIdentityLookupInfo" /> instance for the specified phone number.</returns>
		public static CKUserIdentityLookupInfo FromPhoneNumber (string phoneNumber)
		{
			if (string.IsNullOrEmpty (phoneNumber))
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (phoneNumber));
			return new CKUserIdentityLookupInfo (phoneNumber, 1);
		}
	}
}
