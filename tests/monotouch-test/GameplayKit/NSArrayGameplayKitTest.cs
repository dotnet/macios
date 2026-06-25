// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// Unit tests for NSArray_GameplayKit
//

using GameplayKit;

namespace MonoTouchFixtures.GameplayKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NSArrayGameplayKitTest {

		[Test]
		public void GetShuffledArray_WithRandomSource ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);

			var array = NSArray.FromNSObjects (
				(NSString) "a",
				(NSString) "b",
				(NSString) "c",
				(NSString) "d",
				(NSString) "e"
			);
			var randomSource = new GKMersenneTwisterRandomSource ();

			var shuffled = array.GetShuffledArray<NSString> (randomSource);
			Assert.That (shuffled, Is.Not.Null, "shuffled");
			Assert.That (shuffled.Length, Is.EqualTo (5), "shuffled/length");
		}

		[Test]
		public void GetShuffledArray_NoArgs ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);

			var array = NSArray.FromNSObjects (
				(NSString) "a",
				(NSString) "b",
				(NSString) "c"
			);

			var shuffled = array.GetShuffledArray<NSString> ();
			Assert.That (shuffled, Is.Not.Null, "shuffled");
			Assert.That (shuffled.Length, Is.EqualTo (3), "shuffled/length");
		}
	}
}
