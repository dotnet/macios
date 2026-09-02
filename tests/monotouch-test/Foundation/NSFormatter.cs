#if __MACOS__

using AppKit;

namespace apitest {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSFormatterTests {
		NSNumberFormatter formatter;

		[SetUp]
		public void SetUp ()
		{
			formatter = new NSNumberFormatter ();
			formatter.NumberStyle = NSNumberFormatterStyle.Currency;
			formatter.Locale = NSLocale.FromLocaleIdentifier ("en-US");
		}

		[Test]
		public void NSFormatter_ShouldGetString ()
		{
			var str = formatter.StringFor (NSNumber.FromFloat (0.12f));

			Assert.That (str, Is.EqualTo ("$0.12"));
		}

		[Test]
		public void NSFormatter_ShouldGetAttributedString ()
		{
			var str = formatter.GetAttributedString (NSNumber.FromFloat (3.21f), new NSStringAttributes () { Font = NSFont.SystemFontOfSize (8) });

			Assert.That (str.Value, Is.EqualTo ("$3.21"));
		}

		[Test]
		public void NSFormatter_ShouldGetEditingString ()
		{
			var str = formatter.EditingStringFor (NSNumber.FromInt32 (14));

			Assert.That (str, Is.EqualTo ("$14.00"));
		}

		[Test]
		public void NSFormatter_IsPartialStringValid ()
		{
			string newstr;
			NSString error;
			formatter.PartialStringValidationEnabled = true;
			var valid = formatter.IsPartialStringValid ("valid string", out newstr, out error);

			Assert.That (valid, Is.True);
		}
	}
}

#endif // __MACOS__
