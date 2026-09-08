#if __MACOS__
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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

		// https://github.com/xamarin/bugzilla-archives/blob/main/16/16505/bug.html
		[Test]
		public void PrintPreview_NSGraphicsContextCurrentContext ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);

			// Verify that accessing NSGraphicsContext.CurrentContext during print preview
			// doesn't crash due to the linker trimming NSPrintPreviewGraphicsContext.
			var printableView = new PrintableView (new CoreGraphics.CGRect (0, 0, 100, 100));

			var printInfo = (NSPrintInfo) NSPrintInfo.SharedPrintInfo.Copy ();
			printInfo.JobDisposition = "NSPrintPreviewJob";

			var printOp = NSPrintOperation.FromView (printableView, printInfo);
			printOp.ShowsPrintPanel = true; // this is required to trigger the bug
			printOp.ShowsProgressPanel = true;

			var closedPreview = false;
			var closeAction = new Action (() => {
				if (closedPreview)
					return;
				NSApplication.SharedApplication.AbortModal ();
				closedPreview = true;
			});

			printableView.TaskCompletionSource.Task.ContinueWith (task => {
				closeAction ();
			}, TaskScheduler.FromCurrentSynchronizationContext ());

			// Auto-close after 3 seconds in case something goes wrong
			var timer = NSTimer.CreateScheduledTimer (3.0, (t) => closeAction ());
			NSRunLoop.Current.AddTimer (timer, NSRunLoopMode.ModalPanel);

			printOp.RunOperation ();

			Assert.That (printableView.TaskCompletionSource.Task.IsCompletedSuccessfully, Is.True, "DrawPageBorder was called successfully");
			var context = printableView.TaskCompletionSource.Task.Result;
			Assert.That (context, Is.Not.Null, "NSGraphicsContext.CurrentContext was not null during print preview");
		}
	}

	class PrintableView : NSView {
		public PrintableView (CoreGraphics.CGRect frame) : base (frame) { }

		public TaskCompletionSource<NSGraphicsContext?> TaskCompletionSource = new ();

		public override void DrawPageBorder (CoreGraphics.CGSize borderSize)
		{
			try {
				var context = NSGraphicsContext.CurrentContext;
				base.DrawPageBorder (borderSize);
				TaskCompletionSource.TrySetResult (context);
			} catch (Exception e) {
				Console.WriteLine ($"Unexpected exception occurred: {e}");
				TaskCompletionSource.TrySetException (e);
			}
		}

		public override bool KnowsPageRange (ref Foundation.NSRange range)
		{
			range = new Foundation.NSRange (1, 1);
			return true;
		}

		public override CoreGraphics.CGRect RectForPage (nint pageNumber)
		{
			return Bounds;
		}
	}
}
#endif // __MACOS__
