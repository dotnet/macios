//
// Unit tests for CGContextPDF
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

using CoreGraphics;

namespace MonoTouchFixtures.CoreGraphics {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PDFContextTest {

		[TestCase (false, false)]
		[TestCase (false, true)]
		[TestCase (true, false)]
		[TestCase (true, true)]
		public void MediaBoxInputs (bool useUrl, bool withInfo)
		{
			var mediaBox = new CGRect (10, 20, 200, 300);
			var saved = mediaBox;
			var path = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.CommonDocuments), Guid.NewGuid () + ".pdf");
			using var url = NSUrl.FromFilename (path);
			using var data = new NSMutableData ();
			using var consumer = new CGDataConsumer (data);
			var info = new CGPDFInfo { Title = "Media box input" };
			try {
#if XAMCORE_5_0
				using var context = useUrl
					? (withInfo ? new CGContextPDF (url, in mediaBox, info) : new CGContextPDF (url, in mediaBox))
					: (withInfo ? new CGContextPDF (consumer, in mediaBox, info) : new CGContextPDF (consumer, in mediaBox));
#else
				using var context = useUrl
					? (withInfo ? new CGContextPDF (url, mediaBox, info) : new CGContextPDF (url, mediaBox))
					: (withInfo ? new CGContextPDF (consumer, mediaBox, info) : new CGContextPDF (consumer, mediaBox));
#endif
				context.BeginPage ((CGPDFPageInfo) null);
				context.FillRect (new CGRect (20, 30, 40, 50));
				context.EndPage ();
				context.Close ();
				using var provider = useUrl ? new CGDataProvider (path) : new CGDataProvider (data);
				using var document = new CGPDFDocument (provider);
				Assert.That (document.Pages, Is.EqualTo ((nint) 1), "Page count");
				using var page = document.GetPage (1);
				Assert.That (page.GetBoxRect (CGPDFBox.Media), Is.EqualTo (saved), "Page media box");
				if (withInfo) {
					Assert.That (document.GetInfo ().GetString ("Title", out var title), Is.True, "Title exists");
					Assert.That (title, Is.EqualTo (info.Title), "Title");
				}
				Assert.That (mediaBox, Is.EqualTo (saved), "Input unchanged");
			} finally {
				if (System.IO.File.Exists (path))
					System.IO.File.Delete (path);
			}
		}

		[TestCase (false, false)]
		[TestCase (false, true)]
		[TestCase (true, false)]
		[TestCase (true, true)]
		public void DefaultMediaBox (bool useUrl, bool withInfo)
		{
			var path = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.CommonDocuments), Guid.NewGuid () + ".pdf");
			using var url = NSUrl.FromFilename (path);
			using var data = new NSMutableData ();
			using var consumer = new CGDataConsumer (data);
			var info = new CGPDFInfo { Title = "Default media box" };
			try {
				using var context = useUrl
					? (withInfo ? new CGContextPDF (url, info) : new CGContextPDF (url))
					: (withInfo ? new CGContextPDF (consumer, info) : new CGContextPDF (consumer));
				context.BeginPage ((CGPDFPageInfo) null);
				context.FillRect (new CGRect (20, 30, 40, 50));
				context.EndPage ();
				context.Close ();
				using var provider = useUrl ? new CGDataProvider (path) : new CGDataProvider (data);
				using var document = new CGPDFDocument (provider);
				Assert.That (document.Pages, Is.EqualTo ((nint) 1), "Page count");
				using var page = document.GetPage (1);
				Assert.That (page.GetBoxRect (CGPDFBox.Media), Is.EqualTo (new CGRect (0, 0, 612, 792)), "Default page media box");
				if (withInfo) {
					Assert.That (document.GetInfo ().GetString ("Title", out var title), Is.True, "Title exists");
					Assert.That (title, Is.EqualTo (info.Title), "Title");
				}
			} finally {
				if (System.IO.File.Exists (path))
					System.IO.File.Delete (path);
			}
		}

		static string filename;

		static PDFContextTest ()
		{
			filename = Environment.GetFolderPath (Environment.SpecialFolder.CommonDocuments) + "/t.pdf";
		}

		[Test]
		public void Context_Url ()
		{
			using (var url = new NSUrl (filename))
			using (var ctx = new CGContextPDF (url)) {
				ctx.BeginPage (PDFInfoTest.GetInfo ());
				ctx.SetUrl (url, CGRect.Empty);
				ctx.EndPage ();
			}
		}

		[Test]
		public void Context_Url_Rect ()
		{
			var rect = new CGRect (10, 10, 100, 100);
			using (var url = new NSUrl (filename))
			using (var ctx = new CGContextPDF (url, rect)) {
				ctx.BeginPage ((CGPDFPageInfo) null);
				ctx.SetDestination ("xamarin", rect);
				ctx.EndPage ();
			}
		}

		[Test]
		public void Context_Url_Rect_Info ()
		{
			using (var url = new NSUrl (filename))
			using (var ctx = new CGContextPDF (url, new CGRect (0, 0, 1000, 1000), PDFInfoTest.GetInfo ())) {
				ctx.AddDestination ("monkey", CGPoint.Empty);
				ctx.Close ();
			}
		}

		[Test]
		public void Constructors ()
		{
			if (TestRuntime.CheckXcodeVersion (9, 3))
				Assert.Ignore ("Crash (at least on simulator) with iOS 11.3 beta 1");

			Assert.Throws<Exception> (() => new CGContextPDF ((CGDataConsumer) null), "null CGDataConsumer");

			Assert.Throws<Exception> (() => new CGContextPDF ((CGDataConsumer) null, CGRect.Empty), "null CGDataConsumer, Empty");

			Assert.Throws<Exception> (() => new CGContextPDF ((CGDataConsumer) null, CGRect.Empty, null), "null CGDataConsumer, Empty, null");

			Assert.Throws<Exception> (() => new CGContextPDF ((CGDataConsumer) null, null), "null CGDataConsumer, null");

			Assert.Throws<Exception> (() => new CGContextPDF ((NSUrl) null), "null NSUrl");

			Assert.Throws<Exception> (() => new CGContextPDF ((NSUrl) null, CGRect.Empty), "null NSUrl, Empty");

			Assert.Throws<Exception> (() => new CGContextPDF ((NSUrl) null, CGRect.Empty, null), "null NSUrl, Empty, null");

			Assert.Throws<Exception> (() => new CGContextPDF ((NSUrl) null, null), "null NSUrl, null");

		}

		[Test]
		public void Context_Tag ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			using (var d = new NSDictionary ())
			using (var url = new NSUrl (filename))
			using (var ctx = new CGContextPDF (url)) {
				ctx.BeginPage (PDFInfoTest.GetInfo ());
				ctx.BeginTag (CGPdfTagType.Header, (NSDictionary) null);
				ctx.EndTag ();
				ctx.BeginTag (CGPdfTagType.Caption, d);
				ctx.SetUrl (url, CGRect.Empty);
				ctx.EndTag ();
				ctx.EndPage ();
			}
		}

		[Test]
		public void Context_Tag_Strong ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			using (var url = new NSUrl (filename))
			using (var ctx = new CGContextPDF (url)) {
				var tp = new CGPdfTagProperties () {
					ActualText = "ActualText",
					AlternativeText = "AlternativeText",
					TitleText = "TitleText",
					LanguageText = "LanguageText",
				};
				ctx.BeginPage (PDFInfoTest.GetInfo ());
				ctx.BeginTag (CGPdfTagType.Header, tp);
				ctx.EndTag ();
				ctx.BeginTag (CGPdfTagType.Caption, (CGPdfTagProperties) null);
				ctx.SetUrl (url, CGRect.Empty);
				ctx.EndTag ();
				ctx.EndPage ();
			}
		}

		[Test]
		public void SetParentTree ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var url = new NSUrl (filename);
			using var ctx = new CGContextPDF (url);
			using var doc = CGPDFDocument.FromFile (NSBundle.MainBundle.PathForResource ("Tamarin", "pdf"));
			ctx.SetParentTree (doc.GetCatalog ());
		}

		[Test]
		public void SetIdTree ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var url = new NSUrl (filename);
			using var ctx = new CGContextPDF (url);
			using var doc = CGPDFDocument.FromFile (NSBundle.MainBundle.PathForResource ("Tamarin", "pdf"));
			ctx.SetIdTree (doc.GetCatalog ());
		}

		[Test]
		public void SetPageTagStructureTree ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var url = new NSUrl (filename);
			using var ctx = new CGContextPDF (url);
			using var dict = new NSDictionary ();
			ctx.SetPageTagStructureTree (dict);
		}
	}
}
