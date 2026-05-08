
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
			Assert.That (textList.CustomMarkerFormat, Is.EqualTo (format), "CustomMarkerFormat");
			Assert.That (textList.MarkerFormat, Is.EqualTo (NSTextListMarkerFormats.CustomString), "MarkerFormat");
			Assert.That (textList.ListOptions, Is.EqualTo (NSTextListOptions.None), "ListOptions");
		}

		[TestCase ("{decimal}.", NSTextListOptions.None)]
		[TestCase ("•", NSTextListOptions.PrependEnclosingMarker)]
		public void Constructor_CustomFormat_2 (string format, NSTextListOptions options)
		{
			var textList = new NSTextList (format, options);
			Assert.That (textList.CustomMarkerFormat, Is.EqualTo (format), "CustomMarkerFormat");
			Assert.That (textList.MarkerFormat, Is.EqualTo (NSTextListMarkerFormats.CustomString), "MarkerFormat");
			Assert.That (textList.ListOptions, Is.EqualTo (options), "ListOptions");
		}


		[TestCase (NSTextListMarkerFormats.Check, NSTextListOptions.None)]
		[TestCase (NSTextListMarkerFormats.Box, NSTextListOptions.PrependEnclosingMarker)]
		public void Constructor_TypedFormat_2 (NSTextListMarkerFormats format, NSTextListOptions options)
		{
			var textList = new NSTextList (format, options);
			Assert.That (textList.CustomMarkerFormat, Is.EqualTo ((string) format.GetConstant ()!), "CustomMarkerFormat");
			Assert.That (textList.MarkerFormat, Is.EqualTo (format), "MarkerFormat");
			Assert.That (textList.ListOptions, Is.EqualTo (options), "ListOptions");
		}

		[TestCase (NSTextListMarkerFormats.Circle)]
		[TestCase (NSTextListMarkerFormats.Diamond)]
		public void Constructor_TypedFormat (NSTextListMarkerFormats format)
		{
			var textList = new NSTextList (format);
			Assert.That (textList.CustomMarkerFormat, Is.EqualTo ((string) format.GetConstant ()!), "CustomMarkerFormat");
			Assert.That (textList.MarkerFormat, Is.EqualTo (format), "MarkerFormat");
			Assert.That (textList.ListOptions, Is.EqualTo (NSTextListOptions.None), "ListOptions");
		}
	}
}
