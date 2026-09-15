//
// Unit tests for UrlConnection
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

using System.Threading;

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using MonoTests.System.Net.Http;

#nullable enable

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UrlConnectionTest {

		class MyDelegate : NSUrlConnectionDelegate {

		}

		[Test]
		public void StartCancel ()
		{
			using (var url = new NSUrl (NetworkResources.MicrosoftUrl))
			using (var r = new NSUrlRequest (url))
			using (var d = new MyDelegate ())
			using (var c = new NSUrlConnection (r, d)) {
				c.Start ();
				c.Cancel ();
			}
		}

		[Test]
		public void SendSynchronousRequest ()
		{
			Exception? ex = null;
			var thread = new Thread ((v) => {
				try {
					using var url = new NSUrl (NetworkResources.MicrosoftUrl);
					using var request = new NSUrlRequest (url);
					using var data = NSUrlConnection.SendSynchronousRequest (request, out var response, out var error);
					TestRuntime.IgnoreInCIIfBadNetwork (error);
					Assert.That (error, Is.Null, $"Error: {error?.Description}");
					Assert.That (data, Is.Not.Null, "Data");
					Assert.That (response, Is.Not.Null, "Response");
					response?.Dispose ();
					error?.Dispose ();
				} catch (Exception e) {
					ex = e;
				}
			});
			thread.Start ();
			var timedOut = !thread.Join (TimeSpan.FromSeconds (15));
			if (timedOut) {
				TestRuntime.IgnoreInCI ("Timed out");
				Assert.That (timedOut, Is.False, "Timed out");
			}
			TestRuntime.IgnoreInCIIfBadNetwork (ex);
			TestRuntime.AssertNoNonNUnitException (ex, "Exception");
		}
	}
}
