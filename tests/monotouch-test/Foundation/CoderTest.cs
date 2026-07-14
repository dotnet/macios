//
// Unit tests for NSCoder
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CoderTest {
		[Test]
		public void EncodeDecodeTest ()
		{
			var buffer = new byte [] { 3, 14, 15 };
			var obj = new NSString ();
			var ptr = Marshal.AllocHGlobal (buffer.Length);

			for (int i = 0; i < buffer.Length; i++)
				Marshal.WriteByte (ptr, i, buffer [i]);

			using (var mutableData = new NSMutableData (1024)) {
				using (var coder = new NSKeyedArchiver (mutableData)) {
					coder.Encode (obj, "obj");
					coder.Encode (buffer, "buffer");
					coder.Encode (Int32.MaxValue, "int32");
					coder.Encode (float.MaxValue, "float");
					coder.Encode (double.MaxValue, "double");
					coder.Encode (true, "bool");
					coder.Encode (long.MaxValue, "long");
					coder.Encode (buffer, 1, 1, "buffer2");
					coder.Encode (nint.MaxValue, "nint");
					coder.EncodeBlock (ptr, buffer.Length, "block");
					coder.FinishEncoding ();
				}

				using (var decoder = new NSKeyedUnarchiver (mutableData)) {
					Assert.That (decoder.DecodeObject ("obj"), Is.Not.Null);
					var buf = decoder.DecodeBytes ("buffer");
					Assert.That (buffer.Length, Is.EqualTo (buf.Length), "buffer.length");
					for (int i = 0; i < buf.Length; i++)
						Assert.That (buffer [i], Is.EqualTo (buf [i]), "buffer [" + i.ToString () + "]");
					Assert.That (decoder.DecodeInt ("int32"), Is.EqualTo (Int32.MaxValue));
					Assert.That (decoder.DecodeFloat ("float"), Is.EqualTo (float.MaxValue));
					Assert.That (decoder.DecodeBool ("bool"), Is.EqualTo (true));
					Assert.That (decoder.DecodeLong ("long"), Is.EqualTo (long.MaxValue));
					buf = decoder.DecodeBytes ("buffer2");
					Assert.That (buf.Length, Is.EqualTo (1), "buffer2.length");
					Assert.That (buf [0], Is.EqualTo (buffer [1]), "buffer2 [0]");
					Assert.That (decoder.DecodeNInt ("nint"), Is.EqualTo (nint.MaxValue));

					buf = decoder.DecodeBytes ("block");
					Assert.That (buffer.Length, Is.EqualTo (buf.Length), "block.length");
					for (int i = 0; i < buf.Length; i++)
						Assert.That (buffer [i], Is.EqualTo (buf [i]), "block [" + i.ToString () + "]");
				}
			}

			Marshal.FreeHGlobal (ptr);

		}
	}
}
