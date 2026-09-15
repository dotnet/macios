
using CoreGraphics;

using Xamarin.Utils;

#if MONOMAC
using XColor = AppKit.NSColor;
#else
using XColor = UIKit.UIColor;
#endif

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSAttributedStringDocumentAttributesTest {
		[Test]
		public void DefaultTabInterval ()
		{
			Exception ex;

			var attribs = new NSAttributedStringDocumentAttributes ();
			Assert.That (attribs.DefaultTabInterval, Is.Null, "Initial");
			attribs.DefaultTabInterval = 0.5f;
			Assert.That (attribs.DefaultTabInterval.Value, Is.EqualTo (0.5f), "Half");
			ex = Assert.Throws<ArgumentOutOfRangeException> (() => { attribs.DefaultTabInterval = -1; }, "Negative 1");
			Assert.That (ex.Message, Does.StartWith ("Value must be between 0 and 1"), "Negative 1 - Message");
			ex = Assert.Throws<ArgumentOutOfRangeException> (() => { attribs.DefaultTabInterval = 2; }, "Positive 2");
			Assert.That (ex.Message, Does.StartWith ("Value must be between 0 and 1"), "Positive 1 - Message");

			attribs.DefaultTabInterval = 0f;
			Assert.That (attribs.DefaultTabInterval.Value, Is.EqualTo (0f), "Zero");
			attribs.DefaultTabInterval = 1f;
			Assert.That (attribs.DefaultTabInterval.Value, Is.EqualTo (1f), "One");
			attribs.DefaultTabInterval = null;
			Assert.That (attribs.DefaultTabInterval, Is.Null, "Null End");
		}

		[Test]
		public void HyphenationFactor ()
		{
			Exception ex;

			var attribs = new NSAttributedStringDocumentAttributes ();
			Assert.That (attribs.HyphenationFactor, Is.Null, "Initial");
			attribs.HyphenationFactor = 0.5f;
			Assert.That (attribs.HyphenationFactor.Value, Is.EqualTo (0.5f), "Half");
			ex = Assert.Throws<ArgumentOutOfRangeException> (() => { attribs.HyphenationFactor = -1; }, "Negative 1");
			Assert.That (ex.Message, Does.StartWith ("Value must be between 0 and 1"), "Negative 1 - Message");
			ex = Assert.Throws<ArgumentOutOfRangeException> (() => { attribs.HyphenationFactor = 2; }, "Positive 2");
			Assert.That (ex.Message, Does.StartWith ("Value must be between 0 and 1"), "Positive 1 - Message");

			attribs.HyphenationFactor = 0f;
			Assert.That (attribs.HyphenationFactor.Value, Is.EqualTo (0f), "Zero");
			attribs.HyphenationFactor = 1f;
			Assert.That (attribs.HyphenationFactor.Value, Is.EqualTo (1f), "One");
			attribs.HyphenationFactor = null;
			Assert.That (attribs.HyphenationFactor, Is.Null, "Null End");
		}
	}
}
