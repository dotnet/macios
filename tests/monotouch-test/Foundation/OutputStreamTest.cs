//
// Unit tests for NSOutputStream
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class OutputStreamTest {

		[Test]
		public void Path ()
		{
			using (var s = new NSOutputStream ("Info.plist", false)) {
				// initToFileAtPath:append: does not respond (see dontlink.app) but it works
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		[Test]
		public void Memory ()
		{
			using (var s = new NSOutputStream ()) {
				// initToMemory does not respond (see dontlink.app) but it works
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		[Test]
		public unsafe void Write ()
		{
			using (var s = (NSOutputStream) NSOutputStream.OutputStreamToMemory ()) {
				s.Open ();
				s.Write (new byte [] { 1, 2, 3 }, 3);
				using (var obj = s [NSStream.DataWrittenToMemoryStreamKey] as NSData) {
					Assert.That (obj, Is.Not.Null, "a");
					Assert.That (Marshal.ReadByte (obj.Bytes, 0), Is.EqualTo (1), "a[0]");
					Assert.That (Marshal.ReadByte (obj.Bytes, 1), Is.EqualTo (2), "a[1]");
					Assert.That (Marshal.ReadByte (obj.Bytes, 2), Is.EqualTo (3), "a[2]");
				}
			}

			using (var s = new NSOutputStream ()) {
				s.Open ();
				s.Write (new byte [] { 1, 2, 3 });
				using (var obj = s [NSStream.DataWrittenToMemoryStreamKey] as NSData) {
					Assert.That (obj, Is.Not.Null, "a");
					Assert.That (Marshal.ReadByte (obj.Bytes, 0), Is.EqualTo (1), "b[0]");
					Assert.That (Marshal.ReadByte (obj.Bytes, 1), Is.EqualTo (2), "b[1]");
					Assert.That (Marshal.ReadByte (obj.Bytes, 2), Is.EqualTo (3), "b[2]");
				}
			}

			using (var s = (NSOutputStream) NSOutputStream.OutputStreamToMemory ()) {
				s.Open ();
				s.Write (new byte [] { 1, 2, 3 }, 2, 1);
				using (var obj = s [NSStream.DataWrittenToMemoryStreamKey] as NSData) {
					Assert.That (obj, Is.Not.Null, "a");
					Assert.That (Marshal.ReadByte (obj.Bytes, 0), Is.EqualTo (3), "c[0]");
				}
			}
		}
	}
}
