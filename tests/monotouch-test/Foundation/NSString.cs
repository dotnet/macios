#if __MACOS__

using AppKit;
using CoreGraphics;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSStringTests {
		[Test]
		public void NSString_LineRangeForRange ()
		{
			// Test from http://stackoverflow.com/questions/1085524/how-to-count-the-number-of-lines-in-an-objective-c-string-nsstring
			NSString input = new NSString ("Hey\nHow\nYou\nDoing");
			int stringLength = (int) input.Length;
			int numberOfLines = 0;
			for (int index = 0; index < stringLength; numberOfLines++) {
				NSRange range = input.LineRangeForRange (new NSRange (index, 0));
				index = (int) (range.Location + range.Length);
			}
			Assert.That (numberOfLines, Is.EqualTo (4));
		}

		[Test]
		public void NSString_GetLineStart ()
		{
			NSString input = new NSString ("Hey\nHow\nYou\nDoing");
			nuint start, lineEnd, contentsEnd;
			input.GetLineStart (out start, out lineEnd, out contentsEnd, new NSRange (5, 11));
			Assert.That (start, Is.EqualTo ((nuint) 4));
			Assert.That (lineEnd, Is.EqualTo ((nuint) 17));
			Assert.That (contentsEnd, Is.EqualTo ((nuint) 17));
		}

		[Test]
		public void NSString_BoundingRectWithSize ()
		{
			NSString input = new NSString ("Hey\nHow\nYou\nDoing");
			CGRect rect = input.BoundingRectWithSize (new CGSize (20, 30), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, new NSDictionary ());
			Assert.That (rect.Width > 0, Is.True);
			Assert.That (rect.Height > 0, Is.True);
		}

		[Test]
		public void NSString_CompareTo ()
		{
			using (var a = new NSString ("a"))
			using (var b = new NSString ("b"))
			using (var c = new NSString ("c")) {
				var tests = new NSString [] { c, a, b };

				Array.Sort (tests);

				Assert.That (tests [0], Is.SameAs (a), "0");
				Assert.That (tests [1], Is.SameAs (b), "1");
				Assert.That (tests [2], Is.SameAs (c), "2");
			}
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSAttributedStringTests {
		[Test]
		public void NSAttributedString_BoundingRectWithSize ()
		{
			NSFont font = NSFont.FromFontName ("Arial", 40);
			NSAttributedString str = new NSAttributedString ("Hello World", font);
			CGRect rect = str.BoundingRectWithSize (new CGSize (20, 30), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading);
			Assert.That (rect.Width > 0, Is.True);
			Assert.That (rect.Height > 0, Is.True);
		}

		[Test]
		public void NSAttributedString_GetUrl ()
		{
			NSRange range;
			var str = new NSAttributedString ("Test string with url: http://www.google.com");
			var url = str.GetUrl (42, out range);

			Assert.That (url, Is.Not.Null);
			Assert.That (url.AbsoluteString, Is.EqualTo ("http://www.google.com"));
			Assert.That (range.Location, Is.EqualTo ((nint) 22));
			Assert.That (range.Length, Is.EqualTo ((nint) 21));
		}
	}
}
#endif // __MACOS__
