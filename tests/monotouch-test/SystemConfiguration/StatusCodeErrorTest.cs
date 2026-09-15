//
// Unit tests for StatusCodeError
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using SystemConfiguration;

namespace MonoTouchFixtures.SystemConfiguration {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StatusCodeErrorTest {

		[Test]
		public void InvalidStatusCode ()
		{
			var s = StatusCodeError.GetErrorDescription ((StatusCode) 1);
			// "Operation not permitted" (might be localized so we just check non-null)
			Assert.That (s, Is.Not.Null, "1");
			s = StatusCodeError.GetErrorDescription ((StatusCode) Int32.MinValue);
			// in previous version of xcode, if the error was not known you would get a null ptr, in Xcode 13 and later you
			// get a message stating that the error is not knwon.
			if (TestRuntime.CheckXcodeVersion (13, 0, 0)) {
				Assert.That (s, Is.Not.Null, "MinValue null");
				Assert.That (s.StartsWith ("Unknown error:"), Is.True, "MinValue value");
			} else {
				Assert.That (s, Is.Null, "MinValue");
			}
		}
	}
}
