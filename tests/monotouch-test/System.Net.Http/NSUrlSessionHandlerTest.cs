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
