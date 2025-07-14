using System.Threading;
using Foundation;
using Network;
using CoreFoundation;
using MonoTests.System.Net.Http;
using NUnit.Framework;

#nullable enable

namespace MonoTouchFixtures.Network {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWResolutionReportTest {
		AutoResetEvent reportEvent;  // used to let us know when the connection was established and we got the report
		AutoResetEvent resolutionEvent;  // used to let us know when the connection was established and we got the report
		ConnectionManager manager;
		NWConnection connection;
		NWEstablishmentReport report;
		NWResolutionReport resolutionReport;

		[OneTimeSetUp]
		public void Init ()
		{
			TestRuntime.AssertDevice ();
			// connect so that we can later when the report and test with it
			reportEvent = new AutoResetEvent (false);
			resolutionEvent = new AutoResetEvent (false);

			manager = new ConnectionManager ();
			connection = manager.CreateConnection ();

			connection.GetEstablishmentReport (DispatchQueue.DefaultGlobalQueue, (r) => {
				report = r;
				reportEvent.Set ();
			});
			Assert.True (reportEvent.WaitOne (20000), "Timed out fetching establishment reports.");
			report.EnumerateResolutionReports ((r) => {
				resolutionReport = r;
				resolutionEvent.Set ();
			});
			Assert.True (resolutionEvent.WaitOne (20000), "Timed out enumerating resolution reports.");
		}

		[OneTimeTearDown]
		public void Dispose ()
		{
			report?.Dispose ();
			resolutionReport?.Dispose ();
			manager?.Dispose ();
		}

		[Test]
		public void SourceTest ()
			=> Assert.DoesNotThrow (() => {
				var src = resolutionReport.Source;
			});

		[Test]
		public void MillisecondsTest ()
			=> Assert.DoesNotThrow (() => {
				var rtime = resolutionReport.Milliseconds;
			});

		[Test]
		public void EndpointCountTest ()
			=> Assert.DoesNotThrow (() => {
				var c = resolutionReport.EndpointCount;
			});

		[Test]
		public void SuccessfulEndpointTest ()
			=> Assert.DoesNotThrow (() => {
				var s = resolutionReport.SuccessfulEndpoint;
			});

		[Test]
		public void PreferredEndpointTest ()
			=> Assert.DoesNotThrow (() => {
				var e = resolutionReport.PreferredEndpoint;
			});

		[Test]
		public void ProtocolTest ()
			=> Assert.DoesNotThrow (() => {
				var p = resolutionReport.Protocol;
			});
	}
}
