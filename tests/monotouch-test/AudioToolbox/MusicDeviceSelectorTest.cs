// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MusicDeviceSelectorTest {
		[TestCase (MusicDeviceSelector.Range, 0x0100u)]
		[TestCase (MusicDeviceSelector.MidiEvent, 0x0101u)]
		[TestCase (MusicDeviceSelector.SysEx, 0x0102u)]
		[TestCase (MusicDeviceSelector.PrepareInstrument, 0x0103u)]
		[TestCase (MusicDeviceSelector.ReleaseInstrument, 0x0104u)]
		[TestCase (MusicDeviceSelector.StartNote, 0x0105u)]
		[TestCase (MusicDeviceSelector.StopNote, 0x0106u)]
		[TestCase (MusicDeviceSelector.MidiEventList, 0x0107u)]
		public void NativeValue (MusicDeviceSelector selector, uint expected)
		{
			Assert.That ((uint) selector, Is.EqualTo (expected), selector.ToString ());
		}
	}
}
