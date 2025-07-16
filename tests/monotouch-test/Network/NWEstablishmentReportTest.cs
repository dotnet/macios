using System;
using System.Threading;

using CoreFoundation;
using Foundation;
using Network;

using NUnit.Framework;
using MonoTests.System.Net.Http;


namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWEstablishmentReportTest {
		AutoResetEvent reportEvent;  // used to let us know when the connection was established and we got the report
		ConnectionManager manager;
		NWConnection connection;
		NWEstablishmentReport report;

		[OneTimeSetUp]
		public void Init ()
		{
			// connect so that we can later when the report and test with it
			reportEvent = new AutoResetEvent (false);


			manager = new ConnectionManager ();
			connection = manager.CreateConnection ();

			connection.GetEstablishmentReport (DispatchQueue.DefaultGlobalQueue, (r) => {
				report = r;
				reportEvent.Set ();
			});
			Assert.True (reportEvent.WaitOne (20000), "Connection timed out.");
		}

		[OneTimeTearDown]
		public void Dispose ()
		{
			report?.Dispose ();
			manager?.Dispose ();
		}

		[Test]
		public void TestUsedProxy ()
		{
			TestRuntime.IgnoreInCI ("CI bots might have proxies setup and will mean that the test will fail.");
			Assert.IsFalse (report.UsedProxy, "Used proxy");
		}

		[Test]
		public void TestProxyConfigured ()
		{
			TestRuntime.IgnoreInCI ("CI bots might have proxies setup and will mean that the test will fail.");
			Assert.IsFalse (report.ProxyConfigured, "Proxy configured.");
		}

		[Test]
		public void TestPreviousAttemptCount () => Assert.AreNotEqual (uint.MaxValue, report.PreviousAttemptCount);

		[Test]
		public void TestDuration () => Assert.IsTrue (report.Duration > TimeSpan.MinValue);

		[Test]
		public void TestConnectionSetupTime () => Assert.IsTrue (report.ConnectionSetupTime > TimeSpan.MinValue);

		[Test]
		public void TestEnumerateResolutions ()
		{
			var e = new AutoResetEvent (false);
			report.EnumerateResolutions ((source, duration, count, endpoint, preferred) => {
				Assert.IsTrue (duration > TimeSpan.MinValue, "Durantion");
				Assert.AreNotEqual (0, count, "Count");
				Assert.IsNotNull (endpoint, "endpoint");
				Assert.IsNotNull (preferred, "preferred");
				e.Set ();
			});
			e.WaitOne ();
		}

		[Test]
		public void TestProxyEnpoint ()
		{
			TestRuntime.IgnoreInCI ("CI bots might have proxies setup and will mean that the test will fail.");
			Assert.IsNull (report.ProxyEndpoint);
		}

		[Test]
		public void EnumerateResolutionReportsTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
		}

	}
}
