//
// MessageUI.cs: This file describes the API that the generator will produce for MessageUI
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2014 Xamarin, Inc.
//

using ObjCRuntime;

namespace MessageUI {
	/// <summary>The result from composing an email message.</summary>
	[Native]
	public enum MFMailComposeResult : long {
		/// <summary>The operation was cancelled by the user.</summary>
		Cancelled,
		/// <summary>The message was saved.</summary>
		Saved,
		/// <summary>The message was sent.</summary>
		Sent,
		/// <summary>There was a failure sending the message.</summary>
		Failed,
	}

	/// <summary>Details of the composing error.</summary>
	[ErrorDomain ("MFMailComposeErrorDomain")]
	[Native]
	public enum MFMailComposeErrorCode : long {
		/// <summary>Failed to save the message.</summary>
		SaveFailed,
		/// <summary>Failed to send the message.</summary>
		SendFailed,
	}

	/// <summary>An enumeration whose values specify the various results possible from a message being composed.</summary>
	[Native]
	public enum MessageComposeResult : long {
		/// <summary>To be added.</summary>
		Cancelled,
		/// <summary>To be added.</summary>
		Sent,
		/// <summary>To be added.</summary>
		Failed,
	}
}
