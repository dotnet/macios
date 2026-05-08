// Copyright 2016 Xamarin Inc. All rights reserved.

namespace LinkSdk {

	[TestFixture]
	// we want the tests to be available because we use the linker
	[Preserve (AllMembers = true)]
	public class LocaleTest {

		[Test]
		[SetCulture ("cs-CZ")]
		public void CzechDictComparer ()
		{
			DictComparer ();
		}

		[Test]
		[SetCulture ("en-US")]
		public void EnglishDictComparer ()
		{
			DictComparer ();
		}

		[Test]
		// runs with whatever the device / simulator is configured
		public void DefaultDictComparer ()
		{
			DictComparer ();
		}

		void DictComparer ()
		{
			var n1 = "SEARCHFIELDS";
			var n2 = "Searchfields";

			Assert.That (string.Equals (n1, n2, StringComparison.OrdinalIgnoreCase), Is.True, "string equality");

			var dict = new Dictionary<string, string> (StringComparer.OrdinalIgnoreCase);
			dict [n1] = "test";

			string? result;
			Assert.That (dict.TryGetValue (n2, out result), Is.True, "dictionary value");
		}
	}
}
