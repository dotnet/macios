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
			Assert.That (segue, Is.Not.Null, "NSStoryboardSegueShouldCreateSegueWithConstructor - Failed to create segue, value is null");
		}

		[Test]
		public void NSStoryboardSegueShouldGetIdentifier ()
		{
			Assert.That (string.IsNullOrEmpty (segue.Identifier), Is.False, "NSStoryboardSegueShouldGetIdentifier - Identifier property was empty or null");
		}

		[Test]
		public void NSStoryboardSegueShouldGetSourceController ()
		{
			Assert.That (segue.SourceController, Is.Not.Null, "NSStoryboardSegueShouldGetSourceController - Source controller was null");
			Assert.That (segue.SourceController == source, Is.True, "NSStoryboardSegueShouldGetSourceController - Source controller did not match the source controller passed into the segue.");
		}

		[Test]
		public void NSStoryboardSegueShouldGetDestinationController ()
		{
			Assert.That (segue.DestinationController, Is.Not.Null, "NSStoryboardSegueShouldGetDestinationController - Destination controller was null");
			Assert.That (segue.DestinationController == destination, Is.True, "NSStoryboardSegueShouldGetDestinationController - Destination controller did not mass the destination controller passed into the segue.");
		}

#if false //		Crashes when run in test from command line, works from an actual app
		[Test]
		public void NSStoryboardSegueShouldCreateSegueWithStaticMethod ()
		{
			var segue = NSStoryboardSegue.FromIdentifier ("Test", new NSViewController (), new NSViewController (), () => {
			});
			Assert.That (segue, Is.Not.Null);
		}
#endif
	}
}

#endif // __MACOS__
