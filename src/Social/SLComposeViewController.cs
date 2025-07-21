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
using System;
using ObjCRuntime;
using Foundation;
using Accounts;
using UIKit;

namespace Social {

	public partial class SLComposeViewController {
		/// <param name="serviceKind">To be added.</param>
		///         <summary>Creates a new compose view controller for the specified service.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SLComposeViewController FromService (SLServiceKind serviceKind)
		{
			return FromService (serviceKind.GetConstant ()!);
		}

		/// <param name="serviceKind">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the application can send a request for the specified service type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool IsAvailable (SLServiceKind serviceKind)
		{
			return IsAvailable (serviceKind.GetConstant ()!);
		}
	}
}
#endif
