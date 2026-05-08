using System.Threading;

using Network;

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
			ClassicAssert.True (reportEvent.WaitOne (20000), "Connection timed out.");
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
			ClassicAssert.IsFalse (report.UsedProxy, "Used proxy");
		}

		[Test]
		public void TestProxyConfigured ()
		{
			TestRuntime.IgnoreInCI ("CI bots might have proxies setup and will mean that the test will fail.");
			ClassicAssert.IsFalse (report.ProxyConfigured, "Proxy configured.");
		}

		[Test]
		public void TestPreviousAttemptCount () => ClassicAssert.AreNotEqual (uint.MaxValue, report.PreviousAttemptCount);

		[Test]
		public void TestDuration () => ClassicAssert.IsTrue (report.Duration > TimeSpan.MinValue);

		[Test]
		public void TestConnectionSetupTime () => ClassicAssert.IsTrue (report.ConnectionSetupTime > TimeSpan.MinValue);

		[Test]
		public void TestEnumerateResolutions ()
		{
			var e = new AutoResetEvent (false);
			report.EnumerateResolutions ((source, duration, count, endpoint, preferred) => {
				ClassicAssert.IsTrue (duration > TimeSpan.MinValue, "Durantion");
				ClassicAssert.AreNotEqual (0, count, "Count");
				ClassicAssert.IsNotNull (endpoint, "endpoint");
				ClassicAssert.IsNotNull (preferred, "preferred");
				e.Set ();
			});
			e.WaitOne ();
		}

		[Test]
		public void TestProxyEnpoint ()
		{
			TestRuntime.IgnoreInCI ("CI bots might have proxies setup and will mean that the test will fail.");
			ClassicAssert.IsNull (report.ProxyEndpoint);
		}

		[Test]
		public void EnumerateResolutionReportsTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
		}

	}
}
