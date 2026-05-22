using AVFoundation;
namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVCaptureWhiteBalanceGainsTest {

		[Test]
		public void ConstructorTest ()
		{
			float red = 2.3f;
			float green = 30.90f;
			float blue = 30.90f;

			var gains = new AVCaptureWhiteBalanceGains (red, green, blue);

			Assert.That (gains.RedGain, Is.EqualTo (red), "Wrong RedGain value.");
			Assert.That (gains.GreenGain, Is.EqualTo (green), "Wrong GreenGain value.");
			Assert.That (gains.BlueGain, Is.EqualTo (blue), "Wrong BlueGain value.");
		}

		[Test]
		public void AreEqualTrueTest ()
		{
			float red = 2.3f;
			float green = 30.90f;
			float blue = 30.90f;

			var gains1 = new AVCaptureWhiteBalanceGains (red, green, blue);
			var gains2 = new AVCaptureWhiteBalanceGains (red, green, blue);

			Assert.That (gains1 == gains2, Is.True, "gains1 == gains2");
			Assert.That (gains1.Equals (gains2), Is.True, "gains1.Equals (gains2)");
			Assert.That (gains1 != gains2, Is.False, "gains1 != gains2");
		}

		[Test]
		public void AreEqualFalseTest ()
		{
			var gains1 = new AVCaptureWhiteBalanceGains (2.3f, 3f, 90f);
			var gains2 = new AVCaptureWhiteBalanceGains (gains1.RedGain * 2, gains1.GreenGain * 2, gains1.BlueGain * 2);
			Assert.That (gains1 == gains2, Is.False, "gains1 == gains2");
			Assert.That (gains1.Equals (gains2), Is.False, "gains1.Equals (gains2)");
			Assert.That (gains1 != gains2, Is.True, "gains1 != gains2");
		}

		[Test]
		public void AreEqualDiffType ()
		{
			var gains = new AVCaptureWhiteBalanceGains (2.3f, 3f, 90f);
			var str = new NSString ("Foo");
			Assert.That (gains.Equals ((object) str), Is.False);
		}
	}
}
