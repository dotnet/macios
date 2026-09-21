//
// ios5-twitter.cs: Twitter bindings
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011-2014 Xamarin Inc
//

#nullable enable

namespace Twitter {

	// untyped enum -> TWTweetComposeViewController.h where the values are equals to those of
	// SLComposeViewControllerResult, which is a NSInteger -> SLComposeViewController.h, but a 
	// sizeof(TWTweetComposeViewControllerResultDone) shows it's 4 bytes (on a 64 bits process)
	/// <summary>An enumeration whose values specify the results of composing a tweet in a <see cref="TWTweetComposeViewController" />.</summary>
	public enum TWTweetComposeViewControllerResult {
		/// <summary>The user cancelled the tweet composition.</summary>
		Cancelled,
		/// <summary>The user completed the tweet composition.</summary>
		Done,
	}

	// untyped enum -> TWRequest.h where the values are equals to those of SLRequestMethod, 
	// which is a NSInteger -> SLRequest.h, but a sizeof(TWRequestMethodDELETE) shows it's
	// 4 bytes (on a 64 bits process)
	// note: the API (selectors) uses this as an NSInteger, e.g. from introspection tests
	// 	Return Value of selector: requestMethod, Type: Twitter.TWRequestMethod, Encoded as: q
	// which likely means it's internally used as a `SLRequestMethod`
	/// <summary>The HTTP verb used to perform a Twitter request.</summary>
	[Native]
	public enum TWRequestMethod : long {
		/// <summary>An HTTP GET request.</summary>
		Get,
		/// <summary>An HTTP POST request.</summary>
		Post,
		/// <summary>An HTTP DELETE request.</summary>
		Delete,
	}
}
