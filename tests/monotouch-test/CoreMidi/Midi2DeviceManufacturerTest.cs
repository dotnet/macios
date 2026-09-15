//
// Unit tests for Midi2DeviceManufacturer
//

#if !__TVOS__
using CoreMidi;

namespace MonoTouchFixtures.CoreMidi {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class Midi2DeviceManufacturerTest {
		[Test]
		public void SysExIdByte ()
		{
			var value = default (Midi2DeviceManufacturer);
			Assert.That (value.SysExIdByte, Is.EqualTo (new byte [] { 0, 0, 0 }), "A");

			value.SysExIdByte = new byte [] { 1, 2, 3 };
			Assert.That (value.SysExIdByte, Is.EqualTo (new byte [] { 1, 2, 3 }), "B");

			Assert.Throws<ArgumentNullException> (() => value.SysExIdByte = null, "C");
			Assert.Throws<ArgumentOutOfRangeException> (() => value.SysExIdByte = new byte [2], "D");
		}
	}
}
#endif
