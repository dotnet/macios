//
// Unit tests for NSUrlSession
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System.IO;
using System.Threading.Tasks;

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif
using MonoTests.System.Net.Http;
using Xamarin.Utils;

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UrlSessionTest {
		void AssertCompleted (Task task, string message)
		{
			var value = TestRuntime.TryRunAsync (TimeSpan.FromSeconds (30), task, out var ex);

			Assert.That (value, Is.True, $"Request timed out: {message}");
			Assert.That (ex, Is.Null, message + " Exception");
		}

		[Test]
		public void CreateDataTaskAsync ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			NSUrlSession session = NSUrlSession.SharedSession;
			var url = new NSUrl (NetworkResources.Httpbin.PostUrl);
			var tmpfile = Path.GetTempFileName ();
			File.WriteAllText (tmpfile, "TMPFILE");
			var file_url = NSUrl.FromFilename (tmpfile);
			var file_data = NSData.FromFile (tmpfile);
			var request = new NSUrlRequest (url);
			var uploadRequest = new NSMutableUrlRequest (url);
			uploadRequest.HttpMethod = "POST";

			/* CreateDataTask */
			AssertCompleted (session.CreateDataTaskAsync (request), "CreateDataTask a");
			AssertCompleted (session.CreateDataTaskAsync (url), "CreateDataTask b");

			/* CreateDownloadTask */
			AssertCompleted (session.CreateDownloadTaskAsync (request), "CreateDownloadTask a");
			AssertCompleted (session.CreateDownloadTaskAsync (url), "CreateDownloadTask b");

			/* CreateUploadTask */
			AssertCompleted (session.CreateUploadTaskAsync (uploadRequest, file_url), "CreateUploadTask a");
			AssertCompleted (session.CreateUploadTaskAsync (uploadRequest, file_data), "CreateUploadTask b");
		}

		[Test]
		public void DownloadDataAsync ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			int failed_iteration = -1;

			var rv = TestRuntime.TryRunAsync (TimeSpan.FromSeconds (30), async () => {
				for (int i = 0; i < 5; i++) {
					// Use the default configuration so we can make use of the shared cookie storage.
					var session = NSUrlSession.FromConfiguration (NSUrlSessionConfiguration.DefaultSessionConfiguration);

					var downloadUri = NetworkResources.MicrosoftUri;
					var downloadResponse = await session.CreateDownloadTaskAsync (downloadUri);

					var tempLocation = downloadResponse.Location;
					if (!File.Exists (tempLocation.Path)) {
						Console.WriteLine ("#{1} {0} does not exists", tempLocation, i);
						failed_iteration = i;
						break;
					}
				}
			}, out var ex);

			TestRuntime.IgnoreInCIIfBadNetwork (ex);
			Assert.That (ex, Is.Null, "Exception");
			Assert.That (failed_iteration, Is.EqualTo (-1), "Failed");
		}

		[Test]
		public void SharedSession ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 9, throwIfOtherPlatform: false);

			// in iOS9 those selectors do not respond - but they do work (forwarded to __NSURLSessionLocal type ?)
			// * delegateQueue, sessionDescription, setSessionDescription:, delegate
			var session = NSUrlSession.SharedSession;
			Assert.That (session.Delegate, Is.Null, "delegate");
			Assert.That (session.DelegateQueue, Is.Not.Null, "delegateQueue");
			Assert.That (session.SessionDescription, Is.Null, "sessionDescription");
			session.SessionDescription = "descriptive label";
			Assert.That ((string) session.SessionDescription, Is.EqualTo ("descriptive label"), "setSessionDescription:");
			session.SessionDescription = null; // the session instance is global, so revert value to to make sure the test can be re-run successfully.
		}
	}
}
