//
// Unit tests for NSUrlSessionTaskTransactionMetrics
//
// Authors:
//	Sebastien Pouliot  <sebastien.pouliot@microsoft.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if MONOMAC
using AppKit;
#else
using UIKit;
#endif

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class UrlSessionTaskTransactionMetricsTest {

		[Test]
		public void Properties ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);

			using (var sttm = new NSUrlSessionTaskTransactionMetrics ()) {
				// in iOS10 those selectors do not respond - but they do work (forwarded to __NSCFURLSessionTaskMetrics type ?)
				Assert.That (sttm.ConnectEndDate, Is.Null, "RedirectCount");
				Assert.That (sttm.ConnectStartDate, Is.Null, "TaskInterval");
				Assert.That (sttm.DomainLookupEndDate, Is.Null, "TransactionMetrics");
				Assert.That (sttm.DomainLookupStartDate, Is.Null, "TransactionMetrics");
				if (TestRuntime.CheckXcodeVersion (11, 0)) {
					Assert.That (sttm.FetchStartDate, Is.Not.Null, "TransactionMetrics");
				} else {
					Assert.That (sttm.FetchStartDate, Is.Null, "TransactionMetrics");
				}
				Assert.That (sttm.NetworkProtocolName, Is.Null, "TransactionMetrics");
				Assert.That (sttm.ProxyConnection, Is.False, "TransactionMetrics");
				Assert.That (sttm.Request, Is.Not.Null, "TransactionMetrics");
				if (TestRuntime.CheckXcodeVersion (11, 0)) {
					Assert.That (sttm.RequestEndDate, Is.Not.Null, "TransactionMetrics");
					Assert.That (sttm.RequestStartDate, Is.Not.Null, "TransactionMetrics");
				} else {
					Assert.That (sttm.RequestEndDate, Is.Null, "TransactionMetrics");
					Assert.That (sttm.RequestStartDate, Is.Null, "TransactionMetrics");
				}
				Assert.That (sttm.ResourceFetchType, Is.EqualTo (NSUrlSessionTaskMetricsResourceFetchType.Unknown), "ResourceFetchType");
				Assert.That (sttm.Response, Is.Null, "Response");
				if (TestRuntime.CheckXcodeVersion (11, 0)) {
					Assert.That (sttm.ResponseEndDate, Is.Not.Null, "ResponseEndDate");
					Assert.That (sttm.ResponseStartDate, Is.Not.Null, "ResponseStartDate");
				} else {
					Assert.That (sttm.ResponseEndDate, Is.Null, "ResponseEndDate");
					Assert.That (sttm.ResponseStartDate, Is.Null, "ResponseStartDate");
				}
				Assert.That (sttm.ReusedConnection, Is.EqualTo (true).Or.EqualTo (false), "ReusedConnection");
				Assert.That (sttm.SecureConnectionEndDate, Is.Null, "SecureConnectionEndDate");
				Assert.That (sttm.SecureConnectionStartDate, Is.Null, "SecureConnectionStartDate");
			}
		}
	}
}
