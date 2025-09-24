//
// SSEnums.cs: SafariServices framework enums
//
// Authors:
//   Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014, 2016 Xamarin Inc.
// Copyright 2019 Microsoft Corporation
//

using System;

using Foundation;
using ObjCRuntime;

namespace SafariServices {

	// NSInteger -> SSReadingList.h
	/// <summary>An enumeration that specify possible errors associated with adding a URL to the Safari Reading List.</summary>
	[NoMac]
	[MacCatalyst (14, 0)]
	[Native ("SSReadingListErrorCode")]
	[ErrorDomain ("SSReadingListErrorDomain")]
	public enum SSReadingListError : long {
		/// <summary>To be added.</summary>
		UrlSchemeNotAllowed = 1,
	}

	[NoMac]
	[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'SFErrorCode' enum.")]
	[MacCatalyst (13, 4)]
	[Deprecated (PlatformName.MacCatalyst, 13, 4, message: "Use 'SFErrorCode' enum.")]
	[Native]
	[ErrorDomain ("SFContentBlockerErrorDomain")]
	public enum SFContentBlockerErrorCode : long {
		/// <summary>To be added.</summary>
		Ok = 0,
		/// <summary>To be added.</summary>
		NoExtensionFound = 1,
		/// <summary>To be added.</summary>
		NoAttachmentFound = 2,
		/// <summary>To be added.</summary>
		LoadingInterrupted = 3,
	}

	/// <summary>Enumerates errors that that Safari can encounter when working with content blockers or app extensions.</summary>
	[Introduced (PlatformName.MacCatalyst, 13, 4)]
	[Native]
	[ErrorDomain ("SFErrorDomain")]
	public enum SFErrorCode : long {
		/// <summary>No error occurred.</summary>
		Ok = 0,
		/// <summary>No blocker or extension was found for the specified bundle ID, or the specified extension is owned by someone else.</summary>
		NoExtensionFound = 1,
		/// <summary>The blocker returned an extension item that did not contain an attachment.</summary>
		NoAttachmentFound = 2,
		/// <summary>An error occurred that prevented the blocker or extension from loading.</summary>
		LoadingInterrupted = 3,
		/// <summary>An internal error occurred.</summary>
		InternalError = 4,
		/// <summary>The app is missing an entitlement.</summary>
		MissingEntitlement = 5,
	}

	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum SFSafariViewControllerDismissButtonStyle : long {
		/// <summary>To be added.</summary>
		Done,
		/// <summary>To be added.</summary>
		Close,
		/// <summary>To be added.</summary>
		Cancel,
	}

	[NoMac]
	[Native]
	[ErrorDomain ("SFAuthenticationErrorDomain")]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'ASWebAuthenticationSessionErrorCode' instead.")]
	[MacCatalyst (13, 1)]
	[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ASWebAuthenticationSessionErrorCode' instead.")]
	public enum SFAuthenticationError : long {
		/// <summary>To be added.</summary>
		CanceledLogin = 1,
	}
}
