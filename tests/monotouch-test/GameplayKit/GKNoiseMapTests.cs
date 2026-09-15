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
				Assert.That (baseMap, Is.Not.Null, "baseMap is null");
				Assert.That (map, Is.Not.Null, "baseMap is null");

				Assert.That (map.Size, Is.EqualTo (size), "map size is different");
				Assert.That (baseMap.Size, Is.EqualTo (size), "baseMap size is different");
				Assert.That (map.Origin, Is.EqualTo (origin), "map origin is different");
				Assert.That (baseMap.Origin, Is.EqualTo (origin), "baseMap origin is different");
				Assert.That (map.SampleCount, Is.EqualTo (sample), "map sample is different");
				Assert.That (baseMap.SampleCount, Is.EqualTo (sample), "baseMap sample is different");
			}
		}
	}
}
