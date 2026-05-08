//
// Unit tests for GKNoiseMap
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//	
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

using GameplayKit;

using Vector2d = global::CoreGraphics.NVector2d;
using Vector2i = global::CoreGraphics.NVector2i;

namespace MonoTouchFixtures.GamePlayKit {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GKNoiseMapTests {

		[Test]
		public void Vector2dTest ()
		{
			if (!TestRuntime.CheckXcodeVersion (8, 0))
				Assert.Ignore ("Ignoring GameplayKit tests: Requires iOS10+");

			var size = new Vector2d (1.0, 1.0);
			var origin = new Vector2d (0, 0);
			var sample = new Vector2i (100, 100);

			using (var noise = new GKNoise (GKCylindersNoiseSource.Create (1)))
			using (var baseMap = new GKNoiseMap (noise))
			using (var map = new GKNoiseMap (noise, size, origin, sample, false)) {
				ClassicAssert.NotNull (baseMap, "baseMap is null");
				ClassicAssert.NotNull (map, "baseMap is null");

				ClassicAssert.AreEqual (size, map.Size, "map size is different");
				ClassicAssert.AreEqual (size, baseMap.Size, "baseMap size is different");
				ClassicAssert.AreEqual (origin, map.Origin, "map origin is different");
				ClassicAssert.AreEqual (origin, baseMap.Origin, "baseMap origin is different");
				ClassicAssert.AreEqual (sample, map.SampleCount, "map sample is different");
				ClassicAssert.AreEqual (sample, baseMap.SampleCount, "baseMap sample is different");
			}
		}
	}
}
