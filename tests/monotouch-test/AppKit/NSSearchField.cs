#if __MACOS__

using AppKit;
using Xamarin.Utils;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSSearchFieldTests {
		[Test]
		public void NSSearchFieldShouldSetSearchMenuTemplate ()
		{
			TestRuntime.AssertXcodeVersion (6, 1);

			var searchField = new NSSearchField ();
			var searchMenuTemplate = searchField.SearchMenuTemplate;
			searchField.SearchMenuTemplate = new NSMenu ("Test");

			ClassicAssert.IsTrue (searchField.SearchMenuTemplate != searchMenuTemplate, "NSSearchFieldShouldSetSearchMenuTemplate - Failed to set the SearchMenuTemplate property.");
		}

		[Test]
		public void NSSearchFieldShouldSetSendsWholeSearchString ()
		{
			TestRuntime.AssertXcodeVersion (6, 1);

			var searchField = new NSSearchField ();
			var sendsWholeSearchString = searchField.SendsWholeSearchString;
			searchField.SendsWholeSearchString = !sendsWholeSearchString;

			ClassicAssert.IsTrue (searchField.SendsWholeSearchString != sendsWholeSearchString, "NSSearchFieldShouldSetSendsWholeSearchString - Failed to set the SendsWholeSearchString property.");
		}

		[Test]
		public void NSSearchFieldShouldSetMaximumRecents ()
		{
			TestRuntime.AssertXcodeVersion (6, 1);

			var searchField = new NSSearchField ();
			var maximumRecents = searchField.MaximumRecents;
			searchField.MaximumRecents = maximumRecents + 3;

			ClassicAssert.IsTrue (searchField.MaximumRecents != maximumRecents, "NSSearchFieldShouldSetMaximumRecents - Failed to set the MaximumRecents property.");
		}

		[Test]
		public void NSSearchFieldShouldSetSendsSearchStringImmediately ()
		{
			TestRuntime.AssertXcodeVersion (6, 1);

			var searchField = new NSSearchField ();
			var sendsSearchStringImmediately = searchField.SendsSearchStringImmediately;
			searchField.SendsSearchStringImmediately = !sendsSearchStringImmediately;

			ClassicAssert.IsTrue (searchField.SendsSearchStringImmediately != sendsSearchStringImmediately, "NSSearchFieldShouldSetSendsSearchStringImmediately - Failed to set the SendsSearchStringImmediately property.");
		}
	}
}

#endif // __MACOS__
