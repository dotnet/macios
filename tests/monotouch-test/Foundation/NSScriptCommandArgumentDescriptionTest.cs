#if __MACOS__
// Copyright 2015 Xamarin, Inc.

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSScriptCommandArgumentDescriptionKeysTest {
		[Test]
		public void TestAppleEventCodeKey ()
		{
			Assert.That (NSScriptCommandArgumentDescriptionKeys.AppleEventCodeKey, Is.Not.Null);
			Assert.That (NSScriptCommandArgumentDescriptionKeys.AppleEventCodeKey?.ToString (), Is.EqualTo ("AppleEventCode"));
		}

		[Test]
		public void TestTypeKey ()
		{
			Assert.That (NSScriptCommandArgumentDescriptionKeys.TypeKey.ToString (), Is.EqualTo ("Type"));
		}

		[Test]
		public void TestOptionalKey ()
		{
			Assert.That (NSScriptCommandArgumentDescriptionKeys.OptionalKey.ToString (), Is.EqualTo ("Optional"));
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSScriptCommandArgumentDescriptionTest {

		[TestCase ("")]
		[TestCase (null)]
		public void TestConstructorNameNullOrEmpty (string name)
		{
			Assert.Throws<ArgumentException> (() => new NSScriptCommandArgumentDescription (name, "eeee", "NSString", false));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestConstructorEventCodeNullOrEmpty (string eventCode)
		{
			Assert.Throws<ArgumentException> (() => new NSScriptCommandArgumentDescription ("name", eventCode, "NSString", false));
		}

		[TestCase ("srf")]
		[TestCase ("TooLong")]
		public void TestConstructorEventCodeWrongLength (string eventCode)
		{
			Assert.Throws<ArgumentException> (() => new NSScriptCommandArgumentDescription ("name", eventCode, "NSString", false));
		}

		[TestCase ("")]
		[TestCase (null)]
		public void TestConstructorTypeNullOrEmpty (string type)
		{
			Assert.Throws<ArgumentException> (() => new NSScriptCommandArgumentDescription ("name", "****", type, false));
		}

		[TestCase ("name", "cdfd", "NSString", true)]
		[TestCase ("name", "cdfd", "NSNumber", false)]
		[TestCase ("name", "****", "NSNumber", true)]
		[TestCase ("otherName", "****", "NSNumber", false)]
		public void TestDescription (string name, string code, string type, bool isOptional)
		{
			var arg = new NSScriptCommandArgumentDescription (name, code, type, isOptional);
			var description = arg.Dictionary;

			Assert.That (description [new NSString ("AppleEventCode")].ToString (), Is.EqualTo (code));
			Assert.That (description [new NSString ("Type")].ToString (), Is.EqualTo (type));
			Assert.That (description [new NSString ("Optional")].ToString (), Is.EqualTo (isOptional ? "Yes" : "No"));
		}
	}
}
#endif // __MACOS__
