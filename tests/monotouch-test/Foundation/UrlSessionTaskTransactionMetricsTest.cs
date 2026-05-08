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
				ClassicAssert.Null (sttm.ConnectEndDate, "RedirectCount");
				ClassicAssert.Null (sttm.ConnectStartDate, "TaskInterval");
				ClassicAssert.Null (sttm.DomainLookupEndDate, "TransactionMetrics");
				ClassicAssert.Null (sttm.DomainLookupStartDate, "TransactionMetrics");
				if (TestRuntime.CheckXcodeVersion (11, 0)) {
					ClassicAssert.NotNull (sttm.FetchStartDate, "TransactionMetrics");
				} else {
					ClassicAssert.Null (sttm.FetchStartDate, "TransactionMetrics");
				}
				ClassicAssert.Null (sttm.NetworkProtocolName, "TransactionMetrics");
				ClassicAssert.False (sttm.ProxyConnection, "TransactionMetrics");
				ClassicAssert.NotNull (sttm.Request, "TransactionMetrics");
				if (TestRuntime.CheckXcodeVersion (11, 0)) {
					ClassicAssert.NotNull (sttm.RequestEndDate, "TransactionMetrics");
					ClassicAssert.NotNull (sttm.RequestStartDate, "TransactionMetrics");
				} else {
					ClassicAssert.Null (sttm.RequestEndDate, "TransactionMetrics");
					ClassicAssert.Null (sttm.RequestStartDate, "TransactionMetrics");
				}
				Assert.That (sttm.ResourceFetchType, Is.EqualTo (NSUrlSessionTaskMetricsResourceFetchType.Unknown), "ResourceFetchType");
				ClassicAssert.Null (sttm.Response, "Response");
				if (TestRuntime.CheckXcodeVersion (11, 0)) {
					ClassicAssert.NotNull (sttm.ResponseEndDate, "ResponseEndDate");
					ClassicAssert.NotNull (sttm.ResponseStartDate, "ResponseStartDate");
				} else {
					ClassicAssert.Null (sttm.ResponseEndDate, "ResponseEndDate");
					ClassicAssert.Null (sttm.ResponseStartDate, "ResponseStartDate");
				}
				Assert.That (sttm.ReusedConnection, Is.EqualTo (true).Or.EqualTo (false), "ReusedConnection");
				ClassicAssert.Null (sttm.SecureConnectionEndDate, "SecureConnectionEndDate");
				ClassicAssert.Null (sttm.SecureConnectionStartDate, "SecureConnectionStartDate");
			}
		}
	}
}
