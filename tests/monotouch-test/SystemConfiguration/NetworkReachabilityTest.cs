//
// Unit tests for NetworkReachability
//
// Authors:
//	Marek Safar <msafar@xamarin.com>
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !MONOMAC
using UIKit;
#endif
using SystemConfiguration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using MonoTests.System.Net.Http;

namespace MonoTouchFixtures.SystemConfiguration {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NetworkReachabilityTest {
		[Test]
		public void CtorNameAddress ()
		{
			using (var nr = new NetworkReachability ("apple.com")) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True);
				flags &= ~NetworkReachabilityFlags.TransientConnection; // Remove the TransientConnection flag if it's set
				Assert.That (flags, Is.EqualTo (NetworkReachabilityFlags.Reachable), "Reachable");
			}
		}

		[Test]
		public void CtorIPAddress ()
		{
			using (var nr = new NetworkReachability (IPAddress.Loopback)) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True, "#1");

				// inconsistent results across different iOS versions
				// < 9.0 -> Reachable | IsLocalAddress
				// 9.x -> Reachable | IsLocalAddress | IsDirect
				// 10.0 -> Reachable
				// so we're only checking the (most important) Reachable flag
				Assert.That ((flags & NetworkReachabilityFlags.Reachable) != 0, Is.True, "Reachable");
			}

			using (var nr = new NetworkReachability (new IPAddress (new byte [] { 10, 99, 99, 99 }))) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True, "#2");
				//Assert.That (flags, Is.EqualTo (NetworkReachabilityFlags.Reachable), "#2 Reachable");
			}

			using (var nr = new NetworkReachability (IPAddress.IPv6Loopback)) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True, "#3");
				//Assert.That (flags, Is.EqualTo (
				//	NetworkReachabilityFlags.TransientConnection | NetworkReachabilityFlags.Reachable | NetworkReachabilityFlags.ConnectionRequired), "#3 Reachable");
			}

			using (var nr = new NetworkReachability (IPAddress.Parse ("2001:4860:4860::8844"))) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True, "#4");

				// TODO: Will probably change when IPv6 is enabled locally
				//Assert.That (flags, Is.EqualTo (
				//	NetworkReachabilityFlags.TransientConnection | NetworkReachabilityFlags.Reachable | NetworkReachabilityFlags.ConnectionRequired), "#4 Reachable");
			}

		}

		[Test]
		public void CtorIPAddressPair ()
		{
			IPAddress address;
			try {
				var addresses = Dns.GetHostAddresses (NetworkResources.AppleHost);
				address = null;
				foreach (var candidate in addresses) {
					if (candidate.AddressFamily == AddressFamily.InterNetwork) {
						address = candidate;
						break;
					}
				}
				if (address is null)
					throw new InvalidOperationException ("No IPv4 address found.");
			} catch (Exception e) {
				TestRuntime.IgnoreInCIIfBadNetwork (e);
				throw;
			}

			using (var nr = new NetworkReachability (IPAddress.Loopback, address)) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True, "#1");
				CheckLoopbackFlags (flags, "1", true);
			}

			using (var nr = new NetworkReachability (null, address)) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True, "#2");
				// Different OS versions report different flags, so just
				// check that Reachable is set and no unexpected flags appear.
				CheckRemoteFlags (flags, "2");
			}

			using (var nr = new NetworkReachability (IPAddress.Loopback, null)) {
				NetworkReachabilityFlags flags;

				Assert.That (nr.TryGetFlags (out flags), Is.True, "#3");
				CheckLoopbackFlags (flags, "3", false);
			}
		}

		void CheckLoopbackFlags (NetworkReachabilityFlags flags, string number, bool has_address)
		{
			var noFlags = (NetworkReachabilityFlags) 0;
			var otherFlags = (flags & ~(NetworkReachabilityFlags.Reachable | NetworkReachabilityFlags.IsLocalAddress | NetworkReachabilityFlags.IsDirect | NetworkReachabilityFlags.TransientConnection));

			// Different versions of OSes report different flags. Trying to
			// figure out which OS versions have which flags set turned out to
			// be a never-ending game of whack-a-mole, so just don't assert
			// that any specific flags are set.
			Assert.That (otherFlags, Is.EqualTo (noFlags), $"#{number} No other flags: {flags.ToString ()}");
		}

		void CheckRemoteFlags (NetworkReachabilityFlags flags, string number)
		{
			var noFlags = (NetworkReachabilityFlags) 0;
			var otherFlags = (flags & ~(NetworkReachabilityFlags.Reachable | NetworkReachabilityFlags.TransientConnection | NetworkReachabilityFlags.ConnectionRequired));

			// Different versions of OSes report different flags, so just
			// verify Reachable is set and no unexpected flags appear.
			Assert.That (flags & NetworkReachabilityFlags.Reachable, Is.Not.EqualTo (noFlags), $"#{number} Reachable: {flags.ToString ()}");
			Assert.That (otherFlags, Is.EqualTo (noFlags), $"#{number} No other flags: {flags.ToString ()}");
		}

		[Test]
		public void Ctor_Invalid ()
		{
			try {
				new NetworkReachability ((string) null);
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {
			}

			try {
				new NetworkReachability ((IPAddress) null);
				Assert.Fail ("#2");
			} catch (ArgumentNullException) {
			}

			try {
				new NetworkReachability (null, null);
				Assert.Fail ("#3");
			} catch (ArgumentException) {
			}
		}

		[Test]
		public void Schedule ()
		{
			var ip = new IPAddress (0);
			using var defaultRouteReachability = new NetworkReachability (ip);
			Assert.That (defaultRouteReachability.Schedule (CFRunLoop.Main, CFRunLoop.ModeDefault), Is.True, "Schedule");
			Assert.That (defaultRouteReachability.Unschedule (CFRunLoop.Main, CFRunLoop.ModeDefault), Is.True, "Unschedule");
		}

		[Test]
		public void SetNotification ()
		{
			var ip = new IPAddress (0);
			using var reachability = new NetworkReachability (ip);

			// Test setting a notification
			var statusCode = reachability.SetNotification ((flags) => {
			});
			Assert.That (statusCode, Is.EqualTo (StatusCode.OK), "SetNotification should succeed");

			// Test clearing the notification (this should free the GCHandle)
			statusCode = reachability.SetNotification (null);
			Assert.That (statusCode, Is.EqualTo (StatusCode.OK), "SetNotification(null) should succeed");

			// Test setting notification again after clearing
			statusCode = reachability.SetNotification ((flags) => {
			});
			Assert.That (statusCode, Is.EqualTo (StatusCode.OK), "SetNotification should succeed again");

			// Test that disposing also works (should free the GCHandle in Dispose)
		}

		[Test]
		public void SetNotification_GCHandleFreed ()
		{
			// Create weak references to track GC collection
			var weakRefs = new WeakReference [10];

			// Create NetworkReachability instances on a background thread
			var thread = new Thread (() => {
				for (int i = 0; i < 10; i++) {
					var ip = new IPAddress (0);
					var reachability = new NetworkReachability (ip);

					// Set a notification to allocate the GCHandle
					reachability.SetNotification ((flags) => {
					});

					// Store weak reference to track if object is collected
					weakRefs [i] = new WeakReference (reachability);

					// Dispose to ensure GCHandle is freed
					reachability.Dispose ();
				}
			}) {
				IsBackground = true,
			};

			thread.Start ();
			Assert.That (thread.Join (TimeSpan.FromSeconds (5)), Is.True, "Thread.Join timed out");

			// Force garbage collection
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();

			// Assert that at least one NetworkReachability instance has been collected
			var collectedCount = 0;
			for (int i = 0; i < weakRefs.Length; i++) {
				if (!weakRefs [i].IsAlive) {
					collectedCount++;
				}
			}

			Assert.That (collectedCount > 0, Is.True, $"Expected at least one NetworkReachability instance to be collected, but {collectedCount} were collected");
		}

		[Test]
		public void SetNotification_GCHandleFreedWithNull ()
		{
			// Create weak references to track GC collection
			var weakRefs = new WeakReference [10];

			// Create NetworkReachability instances on a background thread
			var thread = new Thread (() => {
				for (int i = 0; i < 10; i++) {
					var ip = new IPAddress (0);
					var reachability = new NetworkReachability (ip);

					// Set a notification to allocate the GCHandle
					reachability.SetNotification ((flags) => {
					});

					// Clear notification to free the GCHandle
					reachability.SetNotification (null);

					// Store weak reference to track if object is collected
					weakRefs [i] = new WeakReference (reachability);
				}
			}) {
				IsBackground = true,
			};

			thread.Start ();
			Assert.That (thread.Join (TimeSpan.FromSeconds (5)), Is.True, "Thread.Join timed out");

			// Force garbage collection
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();

			// Assert that at least one NetworkReachability instance has been collected
			var collectedCount = 0;
			for (int i = 0; i < weakRefs.Length; i++) {
				if (!weakRefs [i].IsAlive) {
					collectedCount++;
				}
			}

			Assert.That (collectedCount > 0, Is.True, $"Expected at least one NetworkReachability instance to be collected, but {collectedCount} were collected");
		}
	}
}
