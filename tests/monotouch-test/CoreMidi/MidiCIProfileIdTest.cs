//
// Unit tests for MidiCIProfileId
//

#if !__TVOS__
using CoreMidi;

namespace MonoTouchFixtures.CoreMidi {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiCIProfileIdTest {
		[Test]
		public void Standard ()
		{
			var value = default (MidiCIProfileId);
			Assert.That (value.Standard.ProfileIdByte1, Is.EqualTo (0), "ProfileIdByte1 A");
			Assert.That (value.Standard.ProfileBank, Is.EqualTo (0), "ProfileBank A");
			Assert.That (value.Standard.ProfileNumber, Is.EqualTo (0), "ProfileNumber A");
			Assert.That (value.Standard.ProfileVersion, Is.EqualTo (0), "ProfileVersion A");
			Assert.That (value.Standard.ProfileLevel, Is.EqualTo (0), "ProfileLevel A");

			value.Standard = new MidiCIProfileIdStandard () {
				ProfileIdByte1 = 1,
				ProfileBank = 2,
				ProfileNumber = 3,
				ProfileVersion = 4,
				ProfileLevel = 5,
			};

			Assert.That (value.Standard.ProfileIdByte1, Is.EqualTo (1), "ProfileIdByte1 B");
			Assert.That (value.Standard.ProfileBank, Is.EqualTo (2), "ProfileBank B");
			Assert.That (value.Standard.ProfileNumber, Is.EqualTo (3), "ProfileNumber B");
			Assert.That (value.Standard.ProfileVersion, Is.EqualTo (4), "ProfileVersion B");
			Assert.That (value.Standard.ProfileLevel, Is.EqualTo (5), "ProfileLevel B");
		}

		[Test]
		public void ManufacturerSpecific ()
		{
			var value = default (MidiCIProfileId);
			Assert.That (value.ManufacturerSpecific.SysExId1, Is.EqualTo (0), "SysExId1 A");
			Assert.That (value.ManufacturerSpecific.SysExId2, Is.EqualTo (0), "SysExId2 A");
			Assert.That (value.ManufacturerSpecific.SysExId3, Is.EqualTo (0), "SysExId3 A");
			Assert.That (value.ManufacturerSpecific.Info1, Is.EqualTo (0), "Info1 A");
			Assert.That (value.ManufacturerSpecific.Info2, Is.EqualTo (0), "Info2 A");

			value.ManufacturerSpecific = new MidiCIProfileIdManufacturerSpecific () {
				SysExId1 = 1,
				SysExId2 = 2,
				SysExId3 = 3,
				Info1 = 4,
				Info2 = 5,
			};

			Assert.That (value.ManufacturerSpecific.SysExId1, Is.EqualTo (1), "SysExId1 B");
			Assert.That (value.ManufacturerSpecific.SysExId2, Is.EqualTo (2), "SysExId2 B");
			Assert.That (value.ManufacturerSpecific.SysExId3, Is.EqualTo (3), "SysExId3 B");
			Assert.That (value.ManufacturerSpecific.Info1, Is.EqualTo (4), "Info1 B");
			Assert.That (value.ManufacturerSpecific.Info2, Is.EqualTo (5), "Info2 B");
		}
	}
}
#endif
