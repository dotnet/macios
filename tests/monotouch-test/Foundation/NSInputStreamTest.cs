
namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSInputStreamTest {
		[Test]
		public void SubclassedCtor ()
		{
			using (var s = new SubStream ()) {
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		class SubStream : NSInputStream {
		}

		[Test]
		public void Path ()
		{
			using (var s = new NSInputStream ("Info.plist")) {
				// initWithFileAtPath: does not respond (see dontlink.app) but it works
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		[Test]
		public void Data ()
		{
#if MONOMAC || __MACCATALYST__
			// Info.Plist isn't there to load from the same location on mac
			var plistPath = global::System.IO.Path.Combine (NSBundle.MainBundle.BundlePath, "Contents", "Info.plist");
#else
			var plistPath = global::System.IO.Path.Combine (NSBundle.MainBundle.BundlePath, "Info.plist");
#endif
			using (var d = NSData.FromFile (plistPath))
			using (var s = new NSInputStream (d)) {
				// initWithData: does not respond (see dontlink.app) but it works
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		[Test]
		public void Url ()
		{
			using (var u = NSUrl.FromFilename ("Info.plist"))
			using (var s = new NSInputStream (u)) {
				// initWithURL: does not respond (see dontlink.app) but it works
				Assert.That (s.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			}
		}

		[Test]
		public unsafe void Read ()
		{
			using (var data = NSData.FromArray (new byte [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })) {
				using (var s = NSInputStream.FromData (data)) {
					byte [] arr = new byte [10];
					s.Open ();
					Assert.That (s.HasBytesAvailable (), Is.True);
					Assert.That (s.Read (arr, 2), Is.EqualTo ((nint) 2), "#a 1");
					Assert.That (arr [0], Is.EqualTo (0), "#a[0]");
					Assert.That (arr [1], Is.EqualTo (1), "#a[1]");
				}

				using (var s = new NSInputStream (data)) {
					byte [] arr = new byte [10];
					s.Open ();
					Assert.That (s.HasBytesAvailable (), Is.True);
					Assert.That (s.Read (arr, 1, 2), Is.EqualTo ((nint) 2), "#b 1");
					Assert.That (arr [0], Is.EqualTo (0), "#b[0]");
					Assert.That (arr [1], Is.EqualTo (0), "#b[1]");
					Assert.That (arr [2], Is.EqualTo (1), "#b[2]");
				}

				using (var s = new NSInputStream (data)) {
					byte [] arr = new byte [10];
					s.Open ();
					Assert.That (s.HasBytesAvailable (), Is.True);
					fixed (byte* ptr = &arr [2])
						Assert.That (s.Read ((IntPtr) ptr, 2), Is.EqualTo ((nint) 2), "#c 1");
					Assert.That (arr [0], Is.EqualTo (0), "#c[0]");
					Assert.That (arr [1], Is.EqualTo (0), "#c[1]");
					Assert.That (arr [2], Is.EqualTo (0), "#c[2]");
					Assert.That (arr [3], Is.EqualTo (1), "#c[3]");
				}
			}
		}
	}
}
