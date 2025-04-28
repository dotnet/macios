//
// ios5-twitter.cs: Twitter bindings
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011-2014 Xamarin Inc
//

#nullable enable

using ObjCRuntime;

namespace Twitter {

	// untyped enum -> TWTweetComposeViewController.h where the values are equals to those of
	// SLComposeViewControllerResult, which is a NSInteger -> SLComposeViewController.h, but a 
	// sizeof(TWTweetComposeViewControllerResultDone) shows it's 4 bytes (on a 64 bits process)
	/// <summary>An enumeration whose values specify the results of composing a tweet in a <see cref="T:Twitter.TWTweetComposeViewController" />.</summary>
	///     <remarks>To be added.</remarks>
	public enum TWTweetComposeViewControllerResult {
		/// <summary>To be added.</summary>
		Cancelled,
		/// <summary>To be added.</summary>
		Done,
	}

	// untyped enum -> TWRequest.h where the values are equals to those of SLRequestMethod, 
	// which is a NSInteger -> SLRequest.h, but a sizeof(TWRequestMethodDELETE) shows it's
	// 4 bytes (on a 64 bits process)
	// note: the API (selectors) uses this as an NSInteger, e.g. from introspection tests
	// 	Return Value of selector: requestMethod, Type: Twitter.TWRequestMethod, Encoded as: q
	// which likely means it's internally used as a `SLRequestMethod`
	/// <summary>The HTTP verb used to perform a Twitter request.</summary>
	///     <remarks>To be added.</remarks>
	[Native]
	public enum TWRequestMethod : long {
		/// <summary>To be added.</summary>
		Get,
		/// <summary>To be added.</summary>
		Post,
		/// <summary>To be added.</summary>
		Delete,
	}
}
