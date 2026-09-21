
namespace monotouchtest {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSStringTest {
		[Test]
		public void LocalizedFormatTest ()
		{
			// Strings and NSstring
			Assert.That (NSString.LocalizedFormat ("hello").ToString (), Is.EqualTo ("hello"));
			Assert.That (NSString.LocalizedFormat (new NSString ("hello")).ToString (), Is.EqualTo ("hello"));

			// Test the overloads with numbers
			Assert.That (NSString.LocalizedFormat ("hello").ToString (), Is.EqualTo ("hello"));
			Assert.That (NSString.LocalizedFormat ("hello%@", 0).ToString (), Is.EqualTo ("hello0"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@", 0, 1).ToString (), Is.EqualTo ("hello01"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@%@", 0, 1, 2).ToString (), Is.EqualTo ("hello012"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@%@%@", 0, 1, 2, 3).ToString (), Is.EqualTo ("hello0123"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@%@%@%@", 0, 1, 2, 3, 4).ToString (), Is.EqualTo ("hello01234"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@%@%@%@%@", 0, 1, 2, 3, 4, 5).ToString (), Is.EqualTo ("hello012345"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@%@%@%@%@%@", 0, 1, 2, 3, 4, 5, 6).ToString (), Is.EqualTo ("hello0123456"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@%@%@%@%@%@%@", 0, 1, 2, 3, 4, 5, 6, 7).ToString (), Is.EqualTo ("hello01234567"));
			Assert.That (NSString.LocalizedFormat ("hello%@%@%@%@%@%@%@%@%@", 0, 1, 2, 3, 4, 5, 6, 7, 8).ToString (), Is.EqualTo ("hello012345678"));
		}

		[TestCase ("asdf", -1, 0, "start")]
		[TestCase ("asdf", 0, -1, "length")]
		[TestCase ("asdf", 5, 0, "start")]
		[TestCase ("asdf", 0, 5, "length")]
		public void NSStringSubstringExceptions (string input, int start, int length, string paramName)
		{
			var exception = Assert.Throws<ArgumentOutOfRangeException> (() => new NSString (input, start, length));

			Assert.That (exception.ParamName, Is.EqualTo (paramName));

			exception = Assert.Throws<ArgumentOutOfRangeException> (() => NSString.CreateNative (input, start, length));

			Assert.That (exception.ParamName, Is.EqualTo (paramName));
		}

		[TestCase ("asdf", 0, 4)] // Whole string
		[TestCase ("asdf", 0, 2)] // Substring length
		[TestCase ("asdf", 1, 3)] // Substring offset and length
		[TestCase ("asdf", 4, 0)] // Empty string
		public void TestNSStringSubstrings (string input, int start, int length)
		{
			var str = new NSString (input.Substring (start, length));
			var substring = new NSString (input, start, length);
			var substringHandle = NSString.CreateNative (input, start, length);
			try {
				Assert.That (substring, Is.EqualTo (str));

				substring = (NSString) NSString.FromHandle (substringHandle);
				Assert.That (substring, Is.EqualTo (str));
			} finally {
				NSString.ReleaseNative (substringHandle);
			}
		}

		[TestCase (false)]
		[TestCase (true)]
		public void TestFromHandle_owns (bool owns)
		{
			const string testString = "a random and long string that should not be cached by the OS";
			using var str = new NSString (testString);
			for (var i = 0; i < 100; i++) {
				if (owns)
					str.DangerousRetain ();
				Assert.That (NSString.FromHandle (str.Handle, owns), Is.EqualTo (testString), $"true #{i}");
			}
			// If there was a leak, RetainCount would be 100+ because we looped 100 times above.
			Assert.That (str.RetainCount, Is.LessThan ((nuint) 10), "RetainCount");
		}
	}
}
