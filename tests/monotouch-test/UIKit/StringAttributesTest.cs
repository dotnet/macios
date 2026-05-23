//
// UIStringAttributes Unit Tests
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013 Xamarin Inc.
//

#if !MONOMAC
using CoreGraphics;
using UIKit;

using TextAttributes = UIKit.UIStringAttributes;

namespace MonoTouchFixtures.UIKit {

	[TestFixture]
	// we want the test to be availble if we use the linker
	[Preserve (AllMembers = true)]
	[TestFixture]
	public class StringAttributesTest {

		[Test]
		public void RetainCount ()
		{
			TestRuntime.AssertXcodeVersion (4, 5);

			var sa = new UIStringAttributes ();

			var bc = UIColor.FromRGBA (0.1f, 0.2f, 0.3f, 0.4f);
			Assert.That (bc.RetainCount, Is.EqualTo ((nuint) 2), "BackgroundColor-new"); // bug
			sa.BackgroundColor = bc;
			Assert.That (bc.RetainCount, Is.EqualTo ((nuint) 3), "BackgroundColor-set");

			sa.BaselineOffset = 0.0f;

			var fc = UIColor.FromRGBA (0.5f, 0.6f, 0.7f, 0.8f);
			Assert.That (fc.RetainCount, Is.EqualTo ((nuint) 2), "ForegroundColor-new"); // bug
			sa.ForegroundColor = fc;
			Assert.That (fc.RetainCount, Is.EqualTo ((nuint) 3), "ForegroundColor-set");

			var f = UIFont.FromName ("Helvetica", 12);
			var f_count = f.RetainCount; // lots of owner
			sa.Font = f;
			Assert.That (f.RetainCount, Is.EqualTo (++f_count), "Font-set");

			var ps = new NSParagraphStyle ();
			Assert.That (ps.RetainCount, Is.EqualTo ((nuint) 1), "ParagraphStyle-new");
			sa.ParagraphStyle = ps;
			Assert.That (ps.RetainCount, Is.EqualTo ((nuint) 2), "ParagraphStyle-set");

			for (int i = 0; i < 16; i++) {
				Assert.That (sa.BackgroundColor, Is.Not.Null, "BackgroundColor-get");
				Assert.That (sa.ForegroundColor, Is.Not.Null, "ForegroundColor-get");
				Assert.That (sa.Font, Is.Not.Null, "Font-get");
				Assert.That (sa.ParagraphStyle, Is.Not.Null, "ParagraphStyle-get");
			}

			Assert.That (sa.BackgroundColor.RetainCount, Is.EqualTo ((nuint) 3), "BackgroundColor");
			Assert.That (sa.ForegroundColor.RetainCount, Is.EqualTo ((nuint) 3), "ForegroundColor");
			Assert.That (sa.Font.RetainCount, Is.EqualTo (f_count), "Font");
			Assert.That (sa.ParagraphStyle.RetainCount, Is.EqualTo ((nuint) 2), "ParagraphStyle");

			GC.KeepAlive (bc);
			GC.KeepAlive (fc);
			GC.KeepAlive (f);
			GC.KeepAlive (ps);
		}

		[Test]
		public void RetainCount_7 ()
		{
			TestRuntime.AssertXcodeVersion (5, 0);

			var sa = new UIStringAttributes ();
			sa.Expansion = 0.0f;

			var uc = UIColor.FromRGBA (0.1f, 0.2f, 0.3f, 0.4f);
			Assert.That (uc.RetainCount, Is.EqualTo ((nuint) 2), "UnderlineColor-new");
			sa.UnderlineColor = uc;
			Assert.That (uc.RetainCount, Is.EqualTo ((nuint) 3), "UnderlineColor-set");

			var sc = UIColor.FromRGBA (0.5f, 0.6f, 0.7f, 0.8f);
			Assert.That (sc.RetainCount, Is.EqualTo ((nuint) 2), "StrikethroughColor-new");
			sa.StrikethroughColor = sc;
			Assert.That (sc.RetainCount, Is.EqualTo ((nuint) 3), "StrikethroughColor-set");

			var u = new NSUrl ("http://xamarin.com");
			Assert.That (u.RetainCount, Is.EqualTo ((nuint) 1), "Link-new");
			sa.Link = u;
			Assert.That (u.RetainCount, Is.EqualTo ((nuint) 2), "Link-set");

			var ta = new NSTextAttachment ();
			Assert.That (ta.RetainCount, Is.EqualTo ((nuint) 1), "TextAttachment-new");
			sa.TextAttachment = ta;
			Assert.That (ta.RetainCount, Is.EqualTo ((nuint) 2), "TextAttachment-set");

			for (int i = 0; i < 16; i++) {
				Assert.That (sa.UnderlineColor, Is.Not.Null, "UnderlineColor-get");
				Assert.That (sa.StrikethroughColor, Is.Not.Null, "StrikethroughColor-get");
				Assert.That (sa.Link, Is.Not.Null, "Link-get");
				Assert.That (sa.TextAttachment, Is.Not.Null, "TextAttachment-get");
			}

			Assert.That (sa.UnderlineColor.RetainCount, Is.EqualTo ((nuint) 3), "UnderlineColor");
			Assert.That (sa.StrikethroughColor.RetainCount, Is.EqualTo ((nuint) 3), "StrikethroughColor");
			Assert.That (sa.Link.RetainCount, Is.EqualTo ((nuint) 2), "Link");
			Assert.That (sa.TextAttachment.RetainCount, Is.EqualTo ((nuint) 2), "TextAttachment");

			GC.KeepAlive (uc);
			GC.KeepAlive (sc);
			GC.KeepAlive (u);
			GC.KeepAlive (ta);
		}

		[Test]
		public void MutableStringAttributesTest ()
		{
			// ref: https://bugzilla.xamarin.com/show_bug.cgi?id=28158
			// issue: Properties of type UIStringAttributes produce immutable objects that crash when you try to modify them
			// This test proves that the bug is fixed

			using (var nb = new UINavigationBar ()) {
				Assert.That (nb.TitleTextAttributes, Is.Null, "TitleTextAttributes should be null");
				nb.TitleTextAttributes = new UIStringAttributes { ForegroundColor = UIColor.Green };
				Assert.That (nb.TitleTextAttributes.ForegroundColor, Is.SameAs (UIColor.Green), "TitleTextAttributes.ForegroundColor should match");

				var titleAttribtues = nb.TitleTextAttributes; // we now get a mutable dictionary for this DictionaryContainer
				titleAttribtues.ForegroundColor = UIColor.Red; // this used to throw unrecognized selector before fixing bug 28158
				nb.TitleTextAttributes = titleAttribtues;
				Assert.That (nb.TitleTextAttributes.ForegroundColor, Is.SameAs (UIColor.Red), "TitleTextAttributes.ForegroundColor should match");
			}
		}

		[Test]
		public void PrematureDisposal_SegmentedControl ()
		{
			// https://github.com/dotnet/macios/issues/20409
			using var control = new UISegmentedControl ();
			var attrs = new TextAttributes ();
			control.SetTitleTextAttributes (attrs, UIControlState.Normal);
			control.SetTitleTextAttributes (attrs, UIControlState.Selected);
		}

		[Test]
		public void PrematureDisposal_BarItem ()
		{
			// https://github.com/dotnet/macios/issues/20409
			using var control = new UIBarButtonItem (); // UIBarItem is abstract, so use a derived class.
			var attrs = new TextAttributes ();
			control.SetTitleTextAttributes (attrs, UIControlState.Normal);
			control.SetTitleTextAttributes (attrs, UIControlState.Selected);
		}

#if !__TVOS__
		[Test]
		public void PrematureDisposal_SearchBar ()
		{
			// https://github.com/dotnet/macios/issues/20409
			using var control = new UISearchBar (new CGRect (0, 0, 42, 42));
			var attrs = new TextAttributes ();
			control.SetScopeBarButtonTitle (attrs, UIControlState.Normal);
			control.SetScopeBarButtonTitle (attrs, UIControlState.Selected);
		}
#endif // !__TVOS__
	}
}
#endif
