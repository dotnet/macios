//
// Unit tests for NSMutableSAttributedString
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MutableSAttributedStringTest {
		[TestCase ("prefix-text-suffix", 7, 4)]
		[TestCase ("prefix-\u65e5\ud83e\udd14\0-suffix", 7, 4)]
		[TestCase ("", 0, 0)]
		[TestCase ("text", 4, 0)]
		public void InitWithSpan (string value, int start, int length)
		{
			var span = value.AsSpan (start, length);
			var expected = value.Substring (start, length);
			using var plain = new NSMutableAttributedString (span);
			using var noAttributes = new NSMutableAttributedString (span, (NSDictionary) null);
			using var key = new NSString ("SpanTestAttribute");
			using var attributeValue = new NSString ("value");
			using var attributes = NSDictionary.FromObjectAndKey (attributeValue, key);
			using var attributed = new NSMutableAttributedString (span, attributes);
			Assert.That (plain.Value, Is.EqualTo (expected), "Plain value");
			Assert.That (noAttributes.Value, Is.EqualTo (expected), "Null attributes value");
			Assert.That (attributed.Value, Is.EqualTo (expected), "Attributed value");
			if (length > 0) {
				using var actual = attributed.GetAttributes (0, out var range);
				Assert.That (actual [key], Is.EqualTo (attributeValue), "Attribute");
				Assert.That (range, Is.EqualTo (new NSRange (0, length)), "Attribute range");
			}
			plain.Replace (new NSRange (0, length), "replacement");
			Assert.That (plain.Value, Is.EqualTo ("replacement"), "Mutable value");
		}


		[Test]
		public void InitWith ()
		{
			using (var s1 = new NSMutableAttributedString ("string")) {
				// initWithString: does not respond (see dontlink.app) but it works
				Assert.That (s1.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle-1");

				using (var d = new NSDictionary ())
				using (var s2 = new NSMutableAttributedString ("string", d)) {
					// initWithString:attributes: does not respond (see dontlink.app) but it works
					Assert.That (s2.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle-2");
				}

				using (var s3 = new NSMutableAttributedString (s1)) {
					// initWithAttributedString: does not respond (see dontlink.app) but it works
					Assert.That (s3.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle-3");
				}
			}
		}

		[Test]
		public void NullDictionary ()
		{
			using (var s = new NSMutableAttributedString ("string", (NSDictionary) null)) {
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero));
			}
		}

#if !MONOMAC // No foregroundColor parameter for mac
		[Test]
		public void IndirectNullDictionary ()
		{
			// that will call NSAttributedString.ToDictionary which may return null (if empty)
			using (var s = new NSMutableAttributedString ("string", foregroundColor: null)) {
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero));
			}
		}
#endif // !MONOMAC
	}
}
