//
// Unit tests for GCGamepadSnapshot
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

#if !MONOMAC
using UIKit;
#endif
using GameController;

namespace MonoTouchFixtures.GameController {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class GamepadSnapshotTest {

		[Test]
		public void Nullability ()
		{
			if (!TestRuntime.CheckXcodeVersion (5, 0, 1))
				Assert.Inconclusive ("GameController is iOS7+ or macOS 10.9+");

			GCGamepadSnapShotDataV100 data;
			ClassicAssert.False (GCGamepadSnapshot.TryGetSnapshotData (null, out data), "TryGetSnapshotData");
			ClassicAssert.True (data.Version == 0, "Version");
			ClassicAssert.True (data.Size == 0, "Size");

			data = new GCGamepadSnapShotDataV100 ();
			ClassicAssert.True (data.Version == 0, "Version-2");
			ClassicAssert.True (data.Size == 0, "Size-2");

			using (var nsd = data.ToNSData ()) {
				ClassicAssert.True (GCGamepadSnapshot.TryGetSnapshotData (nsd, out data), "TryGetSnapshotData-2");
				ClassicAssert.True (data.Version == 0x100, "Version-3");
				ClassicAssert.True (data.Size == nsd.Length, "Size-3");
			}
		}
	}
}
