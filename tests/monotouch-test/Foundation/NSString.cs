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
			ClassicAssert.AreEqual (4, numberOfLines);
		}

		[Test]
		public void NSString_GetLineStart ()
		{
			NSString input = new NSString ("Hey\nHow\nYou\nDoing");
			nuint start, lineEnd, contentsEnd;
			input.GetLineStart (out start, out lineEnd, out contentsEnd, new NSRange (5, 11));
			ClassicAssert.AreEqual ((nuint) 4, start);
			ClassicAssert.AreEqual ((nuint) 17, lineEnd);
			ClassicAssert.AreEqual ((nuint) 17, contentsEnd);
		}

		[Test]
		public void NSString_BoundingRectWithSize ()
		{
			NSString input = new NSString ("Hey\nHow\nYou\nDoing");
			CGRect rect = input.BoundingRectWithSize (new CGSize (20, 30), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, new NSDictionary ());
			ClassicAssert.IsTrue (rect.Width > 0);
			ClassicAssert.IsTrue (rect.Height > 0);
		}

		[Test]
		public void NSString_CompareTo ()
		{
			using (var a = new NSString ("a"))
			using (var b = new NSString ("b"))
			using (var c = new NSString ("c")) {
				var tests = new NSString [] { c, a, b };

				Array.Sort (tests);

				ClassicAssert.AreSame (a, tests [0], "0");
				ClassicAssert.AreSame (b, tests [1], "1");
				ClassicAssert.AreSame (c, tests [2], "2");
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
			ClassicAssert.IsTrue (rect.Width > 0);
			ClassicAssert.IsTrue (rect.Height > 0);
		}

		[Test]
		public void NSAttributedString_GetUrl ()
		{
			NSRange range;
			var str = new NSAttributedString ("Test string with url: http://www.google.com");
			var url = str.GetUrl (42, out range);

			ClassicAssert.IsNotNull (url);
			ClassicAssert.IsTrue (url.AbsoluteString == "http://www.google.com");
			ClassicAssert.IsTrue (range.Location == 22);
			ClassicAssert.IsTrue (range.Length == 21);
		}
	}
}
#endif // __MACOS__
