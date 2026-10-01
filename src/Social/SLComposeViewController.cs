//
// SLComposeViewController.cs: Extensions to the SLComposeViewController class
//
// Authors:
//    Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012 Xamarin Inc
//

#nullable enable

#if !MONOMAC
using Accounts;
using UIKit;

namespace Social {

	public partial class SLComposeViewController {
		/// <summary>Creates a new compose view controller for the specified service.</summary>
		/// <param name="serviceKind">The kind of social service (such as Twitter or Facebook) to compose a message for.</param>
		public static SLComposeViewController FromService (SLServiceKind serviceKind)
		{
			return FromService (serviceKind.GetConstant ()!);
		}

		/// <summary>Returns <see langword="true" /> if the application can send a request for the specified service type.</summary>
		/// <param name="serviceKind">The kind of social service to check availability for.</param>
		public static bool IsAvailable (SLServiceKind serviceKind)
		{
			return IsAvailable (serviceKind.GetConstant ()!);
		}
	}
}
#endif
