//
// Unit tests for NSDecimalNumber
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014 Xamarin Inc. All rights reserved.
//

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class DecimalNumberTest {

		[Test]
		public void One ()
		{
			Assert.NotNull (NSDecimalNumber.One, "One");
		}
	}
}
