//
// NSUrlSessionHandlerTest.cs
//

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Foundation;
using NUnit.Framework;
using Xamarin.Utils;

namespace MonoTests.System.Net.Http {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSUrlSessionHandlerTest {

		// https://github.com/dotnet/macios/issues/23958
		[Test]
		public void DecompressedResponseDoesNotHaveContentEncodingOrContentLength ()
		{
			bool noContentEncoding = false;
			bool noContentLength = false;
			string body = "";

			var done = TestRuntime.TryRunAsync (TimeSpan.FromSeconds (30), async () => {
				using var handler = new NSUrlSessionHandler ();
				using var client = new HttpClient (handler);
				// Explicitly request gzip to ensure the server compresses the response.
				using var request = new HttpRequestMessage (HttpMethod.Get, $"{NetworkResources.Httpbin.Url}/gzip");
				request.Headers.TryAddWithoutValidation ("Accept-Encoding", "gzip");
				// Use ResponseHeadersRead so that the response content is not buffered,
				// which would cause HttpContent to compute Content-Length from the buffer.
				var response = await client.SendAsync (request, HttpCompletionOption.ResponseHeadersRead);

				if (!response.IsSuccessStatusCode) {
					Assert.Inconclusive ($"Request failed with status {response.StatusCode}");
					return;
				}

				noContentEncoding = response.Content.Headers.ContentEncoding.Count == 0;
				noContentLength = response.Content.Headers.ContentLength is null;
				body = await response.Content.ReadAsStringAsync ();
			}, out var ex);

			if (!done) {
				TestRuntime.IgnoreInCI ("Transient network failure - ignore in CI");
				Assert.Inconclusive ("Request timed out.");
			}
			TestRuntime.IgnoreInCIIfBadNetwork (ex);
			Assert.IsNull (ex, $"Exception: {ex}");
			Assert.IsTrue (noContentEncoding, "Content-Encoding header should be removed for decompressed content");
			Assert.IsTrue (noContentLength, "Content-Length header should be removed for decompressed content");
			Assert.IsTrue (body.Contains ("\"gzipped\"", StringComparison.OrdinalIgnoreCase), "Response body should contain decompressed gzip data");
		}

		// https://github.com/dotnet/macios/issues/23958
		[Test]
		public void NonCompressedResponseHasContentLength ()
		{
			bool noContentEncoding = false;
			long? contentLength = null;
			string body = "";

			var done = TestRuntime.TryRunAsync (TimeSpan.FromSeconds (30), async () => {
				using var handler = new NSUrlSessionHandler ();
				using var client = new HttpClient (handler);
				// Request identity encoding to ensure no compression is applied.
				using var request = new HttpRequestMessage (HttpMethod.Get, $"{NetworkResources.Httpbin.Url}/html");
				request.Headers.TryAddWithoutValidation ("Accept-Encoding", "identity");
				// Use ResponseHeadersRead so that the response content is not buffered,
				// which would cause HttpContent to compute Content-Length from the buffer.
				var response = await client.SendAsync (request, HttpCompletionOption.ResponseHeadersRead);

				if (!response.IsSuccessStatusCode) {
					Assert.Inconclusive ($"Request failed with status {response.StatusCode}");
					return;
				}

				noContentEncoding = response.Content.Headers.ContentEncoding.Count == 0;
				contentLength = response.Content.Headers.ContentLength;
				body = await response.Content.ReadAsStringAsync ();
			}, out var ex);

			if (!done) {
				TestRuntime.IgnoreInCI ("Transient network failure - ignore in CI");
				Assert.Inconclusive ("Request timed out.");
			}
			TestRuntime.IgnoreInCIIfBadNetwork (ex);
			Assert.IsNull (ex, $"Exception: {ex}");
			Assert.IsTrue (noContentEncoding, "Content-Encoding should not be present for non-compressed content");
			Assert.IsNotNull (contentLength, "Content-Length header should be present for non-compressed content");
			Assert.IsTrue (contentLength > 0, "Content-Length should be greater than zero");
			Assert.IsTrue (body.Length > 0, "Response body should not be empty");
		}

		// https://github.com/dotnet/macios/issues/24376
		[Test]
		public void DisposeAndRecreateBackgroundSessionHandler ()
		{
			bool firstRequestSucceeded = false;

			// First request - should succeed
			var done = TestRuntime.TryRunAsync (TimeSpan.FromSeconds (30), async () => {
				using (var handler = new NSUrlSessionHandler (NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration ("test-id"))) {
					using (var client = new HttpClient (handler)) {
						var response = await client.GetByteArrayAsync (NetworkResources.MicrosoftUrl);
						Assert.IsNotNull (response, "First request response");
						Assert.IsTrue (response.Length > 0, "First request response length");
						firstRequestSucceeded = true;
					}
				}
			}, out var ex);

			if (!done || !firstRequestSucceeded) {
				TestRuntime.IgnoreInCI ("Transient network failure - ignore in CI");
				Assert.Inconclusive ("First request failed or timed out - cannot verify the bug.");
			}

			IgnoreIfExceptionDueToBackgroundServiceInUseByAnotherProcess (ex);
			TestRuntime.IgnoreInCIIfBadNetwork (ex);
			Assert.IsNull (ex, "First request exception");

			// Second request with new handler using same background session ID - should not timeout
			done = TestRuntime.TryRunAsync (TimeSpan.FromSeconds (30), async () => {
				using (var handler = new NSUrlSessionHandler (NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration ("test-id"))) {
					using (var client = new HttpClient (handler)) {
						var response = await client.GetByteArrayAsync (NetworkResources.MicrosoftUrl);
						Assert.IsNotNull (response, "Second request response");
						Assert.IsTrue (response.Length > 0, "Second request response length");
					}
				}
			}, out ex);

			if (!done) {
				TestRuntime.IgnoreInCI ("Transient network failure - ignore in CI");
				Assert.Fail ("Second request timedout - this indicates the bug is present.");
			}

			IgnoreIfExceptionDueToBackgroundServiceInUseByAnotherProcess (ex);
			TestRuntime.IgnoreInCIIfBadNetwork (ex);
			if (ex is ObjCException && ex.ToString ().Contains ("Task created in a session that has been invalidated")) {
				// When disposing an NSUrlSessionHandler backed by a background NSUrlSession
				// and immediately creating a new handler with the same background session
				// identifier, the new session can fail with 'Task created in a session
				// that has been invalidated'.
				//
				// This happens because InvalidateAndCancel() is asynchronous - it marks
				// the session for invalidation but doesn't wait for it to complete. Apple
				// reuses the same native session object for background sessions with the
				// same identifier, so creating a new session before invalidation completes
				// returns the already-invalidated session.
				//
				// There are a couple of fixes:
				// * Add a Thread.Sleep before creating the second NSUrlSessionHandler - but this will slow down every test run,
				// * Wait for the session to become invalid in NSUrlSessionHandler (add a 'DidBecomeInvalid' implementation, and wait for that in Dispose) - which may unnecessarily slow down working code.
				// * Detect this scenario here, and just mark the test as inconclusive. The test does something somewhat unusual (create two background sessions with the same identifier in quick succession), so this seems like the best approach for now.
				Assert.Inconclusive ("The previous background session wasn't fully invalidated before we tried to create a new background session (with the same identifier)");
			}
			Assert.IsNull (ex, "Second request exception");
		}

		void IgnoreIfExceptionDueToBackgroundServiceInUseByAnotherProcess (Exception? e)
		{
			if (e is null)
				return;

			if (e is not HttpRequestException hre)
				return;

			if (hre.InnerException is not NSErrorException nee)
				return;

			if (nee.Code != (nint) NSUrlError.BackgroundSessionInUseByAnotherProcess)
				return;

			Assert.Ignore ("The background service is in use by another process.");
		}
	}
}
