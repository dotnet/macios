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

			ClassicAssert.AreEqual (red, gains.RedGain, "Wrong RedGain value.");
			ClassicAssert.AreEqual (green, gains.GreenGain, "Wrong GreenGain value.");
			ClassicAssert.AreEqual (blue, gains.BlueGain, "Wrong BlueGain value.");
		}

		[Test]
		public void AreEqualTrueTest ()
		{
			float red = 2.3f;
			float green = 30.90f;
			float blue = 30.90f;

			var gains1 = new AVCaptureWhiteBalanceGains (red, green, blue);
			var gains2 = new AVCaptureWhiteBalanceGains (red, green, blue);

			ClassicAssert.True (gains1 == gains2, "gains1 == gains2");
			ClassicAssert.True (gains1.Equals (gains2), "gains1.Equals (gains2)");
			ClassicAssert.False (gains1 != gains2, "gains1 != gains2");
		}

		[Test]
		public void AreEqualFalseTest ()
		{
			var gains1 = new AVCaptureWhiteBalanceGains (2.3f, 3f, 90f);
			var gains2 = new AVCaptureWhiteBalanceGains (gains1.RedGain * 2, gains1.GreenGain * 2, gains1.BlueGain * 2);
			ClassicAssert.False (gains1 == gains2, "gains1 == gains2");
			ClassicAssert.False (gains1.Equals (gains2), "gains1.Equals (gains2)");
			ClassicAssert.True (gains1 != gains2, "gains1 != gains2");
		}

		[Test]
		public void AreEqualDiffType ()
		{
			var gains = new AVCaptureWhiteBalanceGains (2.3f, 3f, 90f);
			var str = new NSString ("Foo");
			ClassicAssert.False (gains.Equals ((object) str));
		}
	}
}
