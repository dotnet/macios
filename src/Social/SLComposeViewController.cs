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
		/// <param name="serviceKind">The service kind.</param>
		///         <summary>Creates a new compose view controller for the specified service.</summary>
		public static SLComposeViewController FromService (SLServiceKind serviceKind)
		{
			return FromService (serviceKind.GetConstant ()!);
		}

		/// <param name="serviceKind">The service kind.</param>
		///         <summary>Returns <see langword="true" /> if the application can send a request for the specified service type.</summary>
		public static bool IsAvailable (SLServiceKind serviceKind)
		{
			return IsAvailable (serviceKind.GetConstant ()!);
		}
	}
}
#endif
