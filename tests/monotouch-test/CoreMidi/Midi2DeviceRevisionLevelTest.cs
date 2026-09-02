//
// Unit tests for Midi2DeviceRevisionLevel
//

#if !__TVOS__
using CoreMidi;

namespace MonoTouchFixtures.CoreMidi {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class Midi2DeviceRevisionLevelTest {
		[Test]
		public void RevisionLevel ()
		{
			var value = default (Midi2DeviceRevisionLevel);
			Assert.That (value.RevisionLevel, Is.EqualTo (new byte [] { 0, 0, 0, 0 }), "A");

			value.RevisionLevel = new byte [] { 1, 2, 3, 4 };
			Assert.That (value.RevisionLevel, Is.EqualTo (new byte [] { 1, 2, 3, 4 }), "B");

			Assert.Throws<ArgumentNullException> (() => value.RevisionLevel = null, "C");
			Assert.Throws<ArgumentOutOfRangeException> (() => value.RevisionLevel = new byte [2], "D");
		}
	}
}
#endif
