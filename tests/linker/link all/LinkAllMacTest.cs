#if __MACOS__
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

using AppKit;

namespace LinkAllTests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class LinkAllTest {
		[Test]
		public void EnsureUIThreadException ()
		{
			// works on main/ui thread
			NSApplication.EnsureUIThread ();

			ThreadPool.QueueUserWorkItem ((v) => Tester.Test ());
			Assert.That (Tester.mre.WaitOne (TimeSpan.FromSeconds (10)), Is.True, "Successful wait");
			// The UI thread check only happens for debug builds, on release build it's linked away.
#if DEBUG
			var expected_ex_thrown = true;
#else
			var expected_ex_thrown = false;
#endif
			Assert.That (Tester.exception_thrown, Is.EqualTo (expected_ex_thrown), "Success");
		}


		class Tester : NSObject {
			public static ManualResetEvent mre = new ManualResetEvent (false);
			public static bool exception_thrown;

			[CompilerGenerated]
			[Export ("foo")]
			[BindingImpl (BindingImplOptions.Optimizable)]
			public static void Test ()
			{
				try {
					exception_thrown = false;
					NSApplication.EnsureUIThread ();
				} catch (AppKitThreadAccessException) {
					exception_thrown = true;
				} finally {
					mre.Set ();
				}
			}
		}

		[Test]
		public void XmlSerialization ()
		{
			const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				"<SerializeMe xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" SetMe=\"2\">" +
				"</SerializeMe>";
			using (var sr = new StringReader (xml))
			using (var xr = new XmlTextReader (sr)) {
				var xs = new XmlSerializer (typeof (SerializeMe));
				var item = xs.Deserialize (xr) as SerializeMe;
				Assert.That (item!.SetMe, Is.EqualTo (2), "SetMe");
			}
		}

		public class SerializeMe {

			[XmlAttribute]
			public int SetMe { get; set; }

			public SerializeMe ()
			{
				SetMe = 1;
			}
		}
	}
}
#endif // __MACOS__
