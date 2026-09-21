//
// Unit tests for NSRegularExpression
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2019 Microsoft Corp. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class RegularExpressionTest {
		[Test]
		public void GetMatches ()
		{
			var text = "some text https://microsoft.com text text";

			var range = new NSRange (0, text.Length);
			var detector = NSDataDetector.Create (NSTextCheckingType.Link, out NSError error);

			var matches = detector.GetMatches (new NSString (text), 0, range);

			Assert.That (matches [0].Range.Location, Is.EqualTo ((nint) 10), "Range.Location");
			Assert.That (matches [0].Range.Length, Is.EqualTo ((nint) 21), "Range.Length");
			Assert.That (matches [0].Url.AbsoluteString, Is.EqualTo ("https://microsoft.com"), "Url");
		}
	}
}
