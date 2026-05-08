#if __MACOS__

using AppKit;

namespace Xamarin.Mac.Tests {
	[Preserve (AllMembers = true)]
	public class NSStoryboardSegueTests {
		NSStoryboardSegue segue;
		NSViewController source;
		NSViewController destination;

		[SetUp]
		public void Setup ()
		{
			source = new NSViewController ();
			destination = new NSViewController ();
			segue = new NSStoryboardSegue ("Test", source, destination);
		}

		[Test]
		public void NSStoryboardSegueShouldCreateSegueWithConstructor ()
		{
			ClassicAssert.IsNotNull (segue, "NSStoryboardSegueShouldCreateSegueWithConstructor - Failed to create segue, value is null");
		}

		[Test]
		public void NSStoryboardSegueShouldGetIdentifier ()
		{
			ClassicAssert.IsFalse (string.IsNullOrEmpty (segue.Identifier), "NSStoryboardSegueShouldGetIdentifier - Identifier property was empty or null");
		}

		[Test]
		public void NSStoryboardSegueShouldGetSourceController ()
		{
			ClassicAssert.IsNotNull (segue.SourceController, "NSStoryboardSegueShouldGetSourceController - Source controller was null");
			ClassicAssert.IsTrue (segue.SourceController == source, "NSStoryboardSegueShouldGetSourceController - Source controller did not match the source controller passed into the segue.");
		}

		[Test]
		public void NSStoryboardSegueShouldGetDestinationController ()
		{
			ClassicAssert.IsNotNull (segue.DestinationController, "NSStoryboardSegueShouldGetDestinationController - Destination controller was null");
			ClassicAssert.IsTrue (segue.DestinationController == destination, "NSStoryboardSegueShouldGetDestinationController - Destination controller did not mass the destination controller passed into the segue.");
		}

#if false //		Crashes when run in test from command line, works from an actual app
		[Test]
		public void NSStoryboardSegueShouldCreateSegueWithStaticMethod ()
		{
			var segue = NSStoryboardSegue.FromIdentifier ("Test", new NSViewController (), new NSViewController (), () => {
			});
			ClassicAssert.IsNotNull (segue);
		}
#endif
	}
}

#endif // __MACOS__
