// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if !MONOMAC

using AVFoundation;

namespace MonoTouchFixtures.AVFoundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVAudioDataSourcePolarPatternTest {
		[TestCase (AVAudioDataSourcePolarPattern.Omnidirectional)]
		[TestCase (AVAudioDataSourcePolarPattern.Cardioid)]
		[TestCase (AVAudioDataSourcePolarPattern.Subcardioid)]
		public void RoundTrip (AVAudioDataSourcePolarPattern pattern)
		{
			var constant = pattern.GetConstant ();
			Assert.That (constant, Is.Not.Null, "Constant");
			Assert.That (AVAudioDataSourcePolarPatternExtensions.GetValue (constant), Is.EqualTo (pattern), "Pattern");
		}

#if !__TVOS__
		[Test]
		public void Stereo ()
		{
			TestRuntime.AssertXcodeVersion (12, 0);

			var constant = AVAudioDataSourcePolarPattern.Stereo.GetConstant ();
			Assert.That (constant, Is.EqualTo (AVAudioSession.PolarPatternStereo), "Constant");
			Assert.That (AVAudioDataSourcePolarPatternExtensions.GetValue (constant), Is.EqualTo (AVAudioDataSourcePolarPattern.Stereo), "Pattern");
		}
#endif

		[Test]
		public void Unknown ()
		{
			Assert.That (AVAudioDataSourcePolarPattern.Unknown.GetConstant (), Is.Null, "Unknown constant");
			Assert.That (((AVAudioDataSourcePolarPattern) int.MaxValue).GetConstant (), Is.Null, "Invalid constant");
			Assert.That (AVAudioDataSourcePolarPatternExtensions.GetValue (null), Is.EqualTo (AVAudioDataSourcePolarPattern.Unknown), "Null pattern");
			using var constant = new NSString ("Unrecognized polar pattern");
			Assert.That (AVAudioDataSourcePolarPatternExtensions.GetValue (constant), Is.EqualTo (AVAudioDataSourcePolarPattern.Unknown), "Unrecognized pattern");
		}
	}
}

#endif
