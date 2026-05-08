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
					coder.Encode (buffer, 2, 1, "buffer2");
					coder.Encode (nint.MaxValue, "nint");
					coder.EncodeBlock (ptr, buffer.Length, "block");
					coder.FinishEncoding ();
				}

				using (var decoder = new NSKeyedUnarchiver (mutableData)) {
					ClassicAssert.IsNotNull (decoder.DecodeObject ("obj"));
					var buf = decoder.DecodeBytes ("buffer");
					ClassicAssert.AreEqual (buf.Length, buffer.Length, "buffer.length");
					for (int i = 0; i < buf.Length; i++)
						ClassicAssert.AreEqual (buf [i], buffer [i], "buffer [" + i.ToString () + "]");
					ClassicAssert.AreEqual (Int32.MaxValue, decoder.DecodeInt ("int32"));
					ClassicAssert.AreEqual (float.MaxValue, decoder.DecodeFloat ("float"));
					ClassicAssert.AreEqual (true, decoder.DecodeBool ("bool"));
					ClassicAssert.AreEqual (long.MaxValue, decoder.DecodeLong ("long"));
					buf = decoder.DecodeBytes ("buffer2");
					ClassicAssert.AreEqual (buf.Length, buffer.Length, "buffer2.length");
					for (int i = 0; i < buf.Length; i++)
						ClassicAssert.AreEqual (buf [i], buffer [i], "buffer2 [" + i.ToString () + "]");
					ClassicAssert.AreEqual (nint.MaxValue, decoder.DecodeNInt ("nint"));

					buf = decoder.DecodeBytes ("block");
					ClassicAssert.AreEqual (buf.Length, buffer.Length, "block.length");
					for (int i = 0; i < buf.Length; i++)
						ClassicAssert.AreEqual (buf [i], buffer [i], "block [" + i.ToString () + "]");
				}
			}

			Marshal.FreeHGlobal (ptr);

		}
	}
}
