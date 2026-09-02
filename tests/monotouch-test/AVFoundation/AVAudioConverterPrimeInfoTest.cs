using AVFoundation;
namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVAudioConverterPrimeInfoTest {

		[Test]
		public void ConstructorTest ()
		{
			uint leading = 2;
			uint trailing = 30;

			var info = new AVAudioConverterPrimeInfo (leading, trailing);

			Assert.That (info.LeadingFrames, Is.EqualTo (leading), "Wrong LeadingFrames value.");
			Assert.That (info.TrailingFrames, Is.EqualTo (trailing), "Wrong TrailingFrames value.");
		}

		[Test]
		public void AreEqualTrueTest ()
		{
			uint leading = 2;
			uint trainling = 20;
			var info1 = new AVAudioConverterPrimeInfo (leading, trainling);
			var info2 = new AVAudioConverterPrimeInfo (leading, trainling);

			Assert.That (info1 == info2, Is.True, "info1 == info2");
			Assert.That (info1.Equals (info2), Is.True, "info1.Equals (info2)");
			Assert.That (info1 != info2, Is.False, "info1 != info2");
		}

		[Test]
		public void AreEqualFalseTest ()
		{
			var info1 = new AVAudioConverterPrimeInfo (2, 30);
			var info2 = new AVAudioConverterPrimeInfo (info1.LeadingFrames * 2, info1.TrailingFrames * 2);
			Assert.That (info1 == info2, Is.False, "info1 == info2");
			Assert.That (info1.Equals (info2), Is.False, "info1.Equals (info2)");
			Assert.That (info1 != info2, Is.True, "info1 != info2");
		}

		[Test]
		public void AreEqualDiffType ()
		{
			var info = new AVAudioConverterPrimeInfo (2, 20);
			var str = new NSString ("Foo");
			Assert.That (info.Equals ((object) str), Is.False);
		}
	}
}
