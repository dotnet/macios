//
// Unit tests for AudioFormatAvailabilityTest
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioFormatAvailabilityTest {
		[Test]
		public void GetDecoders ()
		{
			Assert.That (AudioFormatAvailability.GetDecoders (AudioFormatType.LinearPCM), Is.Not.Null);
		}

		[Test]
		public void GetEncoders ()
		{
			Assert.That (AudioFormatAvailability.GetEncoders (AudioFormatType.AC3), Is.Not.Null);
		}
	}
}
