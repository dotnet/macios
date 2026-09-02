#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSTableColumnTests {
		NSTableColumn column;

		[SetUp]
		public void SetUp ()
		{
			column = new NSTableColumn ();
		}

		[Test]
		public void NSTableColumnShouldChangeTitle ()
		{
			var title = column.Title;
			column.Title = "Test";

			Assert.That (column.Title == title, Is.False, "NSTableColumnShouldChangeTitle - Failed to set the Title property");
		}
	}
}

#endif // __MACOS__
