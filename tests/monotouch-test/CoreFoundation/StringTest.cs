//
// Unit tests for CFString
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2014 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.CoreFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class StringTest {
		[TestCase ("prefix-text-suffix", 7, 4)]
		[TestCase ("prefix-\u65e5\ud83e\udd14\0-suffix", 7, 4)]
		[TestCase ("", 0, 0)]
		[TestCase ("text", 4, 0)]
		public void CreateNativeSpan (string value, int start, int length)
		{
			var handle = CFString.CreateNative (value.AsSpan (start, length));
			try {
				Assert.That (handle, Is.Not.EqualTo (NativeHandle.Zero), "Handle");
				Assert.That (CFString.FromHandle (handle), Is.EqualTo (value.Substring (start, length)), "Value");
			} finally {
				CFString.ReleaseNative (handle);
			}
		}

		[Test]
		public void CreateNativeNullString ()
		{
			Assert.That (CFString.CreateNative (null), Is.EqualTo (NativeHandle.Zero), "Null string");
		}

		[Test]
		public void CreateNativeDefaultSpan ()
		{
			var handle = CFString.CreateNative (default (ReadOnlySpan<char>));
			try {
				Assert.That (handle, Is.Not.EqualTo (NativeHandle.Zero), "Handle");
				Assert.That (CFString.FromHandle (handle), Is.EqualTo (""), "Value");
			} finally {
				CFString.ReleaseNative (handle);
			}
		}

		[Test]
		public void CreateNativeStackSpan ()
		{
			Span<char> value = stackalloc char [] { 'a', '\0', 'b' };
			var handle = CFString.CreateNative (value);
			value.Fill ('x');
			try {
				Assert.That (CFString.FromHandle (handle), Is.EqualTo ("a\0b"), "Copied value");
			} finally {
				CFString.ReleaseNative (handle);
			}
		}


		[Test]
		public void ToString_ ()
		{
			using (CFString str = new CFString ("string")) {
				Assert.That (str.ToString (), Is.EqualTo ("string"), "ctor(string)");
			}
		}

		[Test]
		public void Null ()
		{
			Assert.Throws<ArgumentNullException> (delegate { new CFString (null); }, "null");
		}

		[Test]
		public void Index ()
		{
			var str = "Ab🤔日ㅁ名";
			using var nativeStr = new CFString (str);
			var array = str.ToCharArray ();
			for (int i = 0; i < array.Length; i++) {
				Assert.That (nativeStr [i], Is.EqualTo (str [i]), $"{str [i]} != {nativeStr [i]}");
			}
		}
	}
}
