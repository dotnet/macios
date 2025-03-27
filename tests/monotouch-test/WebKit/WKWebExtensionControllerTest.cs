// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if HAS_WEBKIT

using System;
using System.Linq;
using System.Numerics;
using System.Threading;

using Foundation;
using ObjCRuntime;
using WebKit;

using NUnit.Framework;

namespace MonoTouchFixtures.WebKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class WKWebExtensionControllerTest {
		[Test]
		public void AllExtensionDataTypes ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			var weak = WKWebExtensionController.WeakAllExtensionDataTypes;
			var strong = WKWebExtensionController.AllExtensionDataTypes;
			var strongBitCount = BitOperations.PopCount (unchecked((ulong) strong));
			Assert.That (strongBitCount, Is.EqualTo ((int) weak.Count), "Count");
			Assert.That (strongBitCount, Is.GreaterThan (0), ">0");
		}

		[Test]
		public void DataRecords ()
		{
			TestRuntime.AssertXcodeVersion (16, 3);

			using var obj = new WKWebExtensionController ();
			var evt = new ManualResetEvent (false);
			WKWebExtensionDataRecord? [] records = null;

			obj.FetchDataRecords (WKWebExtensionController.AllExtensionDataTypes, (fetchedRecords) => {
				records = fetchedRecords;
				evt.Set ();
			});
			Assert.That (evt.WaitOne (TimeSpan.FromSeconds (1)), "Wait");
			Assert.That (records, Is.Not.Null, "Not null");
			Assert.That (records.Length, Is.EqualTo (0), "No records");
		}
	}
}

#endif // HAS_WEBKIT
