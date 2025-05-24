//
// WebKit/Defs.cs
//
// Authors:
//   Aaron Bockover (abock@xamarin.com)
//
// Copyright 2014, 2016 Xamarin Inc. All rights reserved.
//

using System;

using ObjCRuntime;

#nullable enable

namespace WebKit {
	/// <summary>Enumerates the types of action that can cause navigation.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum WKNavigationType : long {
		/// <summary>The user activated a link.</summary>
		LinkActivated,
		/// <summary>The user submitted a form.</summary>
		FormSubmitted,
		/// <summary>The user moved forward or backward through the browsing history.</summary>
		BackForward,
		/// <summary>A page was reloaded.</summary>
		Reload,
		/// <summary>The user resubmitted a form.</summary>
		FormResubmitted,
		/// <summary>An action that is not represented by this enumeration caused the navigation.</summary>
		Other = -1,
	}

	/// <summary>Contains values that enumerate whether to cancel or allow navigation actions.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum WKNavigationActionPolicy : long {
		/// <summary>Cancel navigation.</summary>
		Cancel,
		/// <summary>Allow navigation.</summary>
		Allow,
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		Download,
	}

	/// <summary>Contains values that enumerate whether the response delegate should cancel or allow navigation.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum WKNavigationResponsePolicy : long {
		/// <summary>Cancel navigation.</summary>
		Cancel,
		/// <summary>Allow navigation.</summary>
		Allow,
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		Download,
	}

	/// <summary>Enumerates values that indicate when to inject a script.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum WKUserScriptInjectionTime : long {
		/// <summary>Inject the script before the document begins to load.</summary>
		AtDocumentStart,
		/// <summary>Inject the script after the document is loaded..</summary>
		AtDocumentEnd,
	}

	/// <summary>Enumerates WebKit errors.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[ErrorDomain ("WKErrorDomain")]
	public enum WKErrorCode : long {
		/// <summary>No error occurred.</summary>
		None,
		/// <summary>An unknown error occurred.</summary>
		Unknown,
		/// <summary>A WebContent process terminated unexpectetly.</summary>
		WebContentProcessTerminated,
		/// <summary>A WebView was invalidated.</summary>
		WebViewInvalidated,
		/// <summary>A JavaScript exception occurred.</summary>
		JavaScriptExceptionOccurred,
		/// <summary>The result of a JavaScript execution was not supported.</summary>
		JavaScriptResultTypeIsUnsupported,
		// Xcode 9
		/// <summary>Indicates that a rule list could not be compiled.</summary>
		ContentRuleListStoreCompileFailed,
		/// <summary>Indicates that a rule list could not be looked up.</summary>
		ContentRuleListStoreLookUpFailed,
		/// <summary>Indicates that a rule list could not be removed from a store.</summary>
		ContentRuleListStoreRemoveFailed,
		/// <summary>Indicates that there was a mismatch between rule list store versions.</summary>
		ContentRuleListStoreVersionMismatch,
		// Xcode 11
		AttributedStringContentFailedToLoad,
		AttributedStringContentLoadTimedOut,
		// Xcode 12
		JavaScriptInvalidFrameTarget,
		NavigationAppBoundDomain,
		JavaScriptAppBoundDomain,
		// Xcode 14
		DuplicateCredential,
		MalformedCredential,
		CredentialNotFound,
	}

	/// <summary>Enumerates values the controls how selections are created.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	public enum WKSelectionGranularity : long {
		/// <summary>Jump to word boundaries as the user selects.</summary>
		Dynamic,
		/// <summary>Add or remove one character at a time as the user selects.</summary>
		Character,
	}

	/// <summary>Enumerates the kinds of data that are detected and converted to links.</summary>
	[NoMac]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum WKDataDetectorTypes : ulong {
		/// <summary>Indicates that no items are detected and converted to links.</summary>
		None = 0,
		/// <summary>Indicates that phone numbers are detected and converted to links.</summary>
		PhoneNumber = 1 << 0,
		/// <summary>Indicates that URLs are detected and converted to links.</summary>
		Link = 1 << 1,
		/// <summary>Indicates that addresses are detected and converted to links.</summary>
		Address = 1 << 2,
		/// <summary>Indicates that datetimes in the future are detected and converted to links.</summary>
		CalendarEvent = 1 << 3,
		/// <summary>Indicates that shipping numbers are detected and converted to links.</summary>
		TrackingNumber = 1 << 4,
		/// <summary>Indicates that flight numbers are detected and converted to links.</summary>
		FlightNumber = 1 << 5,
		/// <summary>Indicates that lookup suggestions will be provided.</summary>
		LookupSuggestion = 1 << 6,
		/// <summary>Indicates that Spotlight suggestsions are detected and converted into links.</summary>
		SpotlightSuggestion = LookupSuggestion,
		/// <summary>Indicates that all detectable items will be detected and converted to links.</summary>
		All = UInt64.MaxValue,
	}

	/// <summary>Enumerates media types.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum WKAudiovisualMediaTypes : ulong {
		/// <summary>Indicates neither audio nor video media.</summary>
		None = 0,
		/// <summary>Indicates audio media.</summary>
		Audio = 1 << 0,
		/// <summary>Indicates video media.</summary>
		Video = 1 << 1,
		/// <summary>Indicates both audio and video media.</summary>
		All = UInt64.MaxValue,
	}

	[iOS (15, 0), NoTV, MacCatalyst (15, 0)]
	[Native]
	public enum WKMediaCaptureState : long {
		None,
		Active,
		Muted,
	}

	[iOS (15, 0), NoTV, MacCatalyst (15, 0)]
	[Native]
	public enum WKMediaCaptureType : long {
		Camera,
		Microphone,
		CameraAndMicrophone,
	}

	[iOS (15, 0), NoTV, MacCatalyst (15, 0)]
	[Native]
	public enum WKPermissionDecision : long {
		Prompt,
		Grant,
		Deny,
	}

}
