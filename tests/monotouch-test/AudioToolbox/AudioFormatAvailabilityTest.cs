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
			ClassicAssert.IsNotNull (AudioFormatAvailability.GetDecoders (AudioFormatType.LinearPCM));
		}

		[Test]
		public void GetEncoders ()
		{
			ClassicAssert.IsNotNull (AudioFormatAvailability.GetEncoders (AudioFormatType.AC3));
		}
	}
}
