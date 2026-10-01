//
// Enums.cs: Enumerations for the Social framework
//
// Authors:
//    Miguel de Icaza (miguel@xamarin.com)
//
// Copyright 2012-2014 Xamarin Inc
//

namespace Social {

	// NSInteger -> SLRequest.h
	/// <summary>The HTTP verb associated with a social service request.</summary>
	[Native]
	public enum SLRequestMethod : long {
		/// <summary>An HTTP GET request.</summary>
		Get,
		/// <summary>An HTTP POST request.</summary>
		Post,
		/// <summary>An HTTP DELETE request.</summary>
		Delete,
		/// <summary>An HTTP PUT request.</summary>
		Put,
	}

	// NSInteger -> SLComposeViewController.h
	/// <summary>An enumeration whose values specify whether composition in a <see cref="Social.SLComposeViewController" /> was completed or cancelled.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SLComposeViewControllerResult : long {
		/// <summary>The user cancelled composition.</summary>
		Cancelled,
		/// <summary>The user completed composition.</summary>
		Done,
	}
}
