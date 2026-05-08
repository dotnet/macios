
using Xamarin.Utils;

#if HAS_UIKIT
using UIKit;
#else
using AppKit;
#endif

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSTextListTest {

		[TestCase ("{decimal}.")]
		[TestCase ("•")]
		public void Constructor_CustomFormat (string format)
		{
			var textList = new NSTextList (format);
			ClassicAssert.AreEqual (format, textList.CustomMarkerFormat, "CustomMarkerFormat");
			ClassicAssert.AreEqual (NSTextListMarkerFormats.CustomString, textList.MarkerFormat, "MarkerFormat");
			ClassicAssert.AreEqual (NSTextListOptions.None, textList.ListOptions, "ListOptions");
		}

		[TestCase ("{decimal}.", NSTextListOptions.None)]
		[TestCase ("•", NSTextListOptions.PrependEnclosingMarker)]
		public void Constructor_CustomFormat_2 (string format, NSTextListOptions options)
		{
			var textList = new NSTextList (format, options);
			ClassicAssert.AreEqual (format, textList.CustomMarkerFormat, "CustomMarkerFormat");
			ClassicAssert.AreEqual (NSTextListMarkerFormats.CustomString, textList.MarkerFormat, "MarkerFormat");
			ClassicAssert.AreEqual (options, textList.ListOptions, "ListOptions");
		}


		[TestCase (NSTextListMarkerFormats.Check, NSTextListOptions.None)]
		[TestCase (NSTextListMarkerFormats.Box, NSTextListOptions.PrependEnclosingMarker)]
		public void Constructor_TypedFormat_2 (NSTextListMarkerFormats format, NSTextListOptions options)
		{
			var textList = new NSTextList (format, options);
			ClassicAssert.AreEqual ((string) format.GetConstant ()!, textList.CustomMarkerFormat, "CustomMarkerFormat");
			ClassicAssert.AreEqual (format, textList.MarkerFormat, "MarkerFormat");
			ClassicAssert.AreEqual (options, textList.ListOptions, "ListOptions");
		}

		[TestCase (NSTextListMarkerFormats.Circle)]
		[TestCase (NSTextListMarkerFormats.Diamond)]
		public void Constructor_TypedFormat (NSTextListMarkerFormats format)
		{
			var textList = new NSTextList (format);
			ClassicAssert.AreEqual ((string) format.GetConstant ()!, textList.CustomMarkerFormat, "CustomMarkerFormat");
			ClassicAssert.AreEqual (format, textList.MarkerFormat, "MarkerFormat");
			ClassicAssert.AreEqual (NSTextListOptions.None, textList.ListOptions, "ListOptions");
		}
	}
}
