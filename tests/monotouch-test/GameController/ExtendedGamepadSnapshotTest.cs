//
// Unit tests for GCExtendedGamepadSnapshot
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
	public class ExtendedGamepadSnapshotTest {

		[Test]
		public void Nullability ()
		{
			if (!TestRuntime.CheckXcodeVersion (5, 0, 1))
				Assert.Inconclusive ("GameController is iOS7+ or macOS 10.9+");

			GCExtendedGamepadSnapShotDataV100 data;
			Assert.That (GCExtendedGamepadSnapshot.TryGetSnapShotData (null, out data), Is.False, "TryGetSnapshotData");
			Assert.That (data.Version == 0, Is.True, "Version");
			Assert.That (data.Size == 0, Is.True, "Size");

			data = new GCExtendedGamepadSnapShotDataV100 ();
			Assert.That (data.Version == 0, Is.True, "Version-2");
			Assert.That (data.Size == 0, Is.True, "Size-2");

			using (var nsd = data.ToNSData ()) {
				Assert.That (GCExtendedGamepadSnapshot.TryGetSnapShotData (nsd, out data), Is.True, "TryGetSnapshotData-2");
				Assert.That (data.Version == 0x100, Is.True, "Version-3");
				Assert.That (data.Size == nsd.Length, Is.True, "Size-3");
			}
		}
	}
}
