//
// MusicTrack unit Tests
//
// Authors:
//	Manuel de la Pena <mandel@microsoft.com>
//
// Copyright 2019 Microsoft Corporation All rights reserved.
//

#if !MONOMAC && !__TVOS__

using AudioToolbox;
using CoreMidi;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MusicTrackTest {

		MusicSequence sequence;
		MusicTrack track;

		[SetUp]
		public void SetUp ()
		{
			sequence = new MusicSequence ();
			track = MusicTrack.FromSequence (sequence);
		}

		[TearDown]
		public void TearDown ()
		{
			track?.Dispose ();
			sequence?.Dispose ();
		}

		[Test]
		public void Defaults ()
		{
			Assert.That (track.Handle, Is.Not.EqualTo (IntPtr.Zero), "Handle");
			Assert.That (track.Sequence, Is.Not.Null, "Sequence");

			Assert.That (track.MuteStatus, Is.False, "MuteStatus");
			track.MuteStatus = true;
			Assert.That (track.MuteStatus, Is.True, "MuteStatus B");
			track.MuteStatus = false;
			Assert.That (track.MuteStatus, Is.False, "MuteStatus C");

			Assert.That (track.SoloStatus, Is.False, "SoloStatus");
			track.SoloStatus = true;
			Assert.That (track.SoloStatus, Is.True, "SoloStatus B");
			track.SoloStatus = false;
			Assert.That (track.SoloStatus, Is.False, "SoloStatus C");

			Assert.That (track.TrackLength, Is.EqualTo (0.0f), "TrackLength");
			var originalTrackLength = track.TrackLength;
			track.TrackLength = 1.32f;
			Assert.That (track.TrackLength, Is.EqualTo (1.32f), "TrackLength B");
			track.TrackLength = originalTrackLength;
			Assert.That (track.TrackLength, Is.EqualTo (0.0f), "TrackLength C");
		}

		[Test]
		public void MidiEndPointProperty ()
		{
			// get one of the endpoints, and set it and get it
			for (int i = 0; i < Midi.SourceCount; i++) {
				using (var endpoint = MidiEndpoint.GetSource (i)) {
					if (endpoint.Handle == 0)
						continue;
					track.SetDestMidiEndpoint (endpoint);
					MidiEndpoint outEnpoint;
					var status = track.GetDestMidiEndpoint (out outEnpoint);
					Assert.That (outEnpoint.Handle, Is.EqualTo (endpoint.Handle), "Track endpoint.");
				}
			}
		}
	}
}

#endif // !MONOMAC && !__TVOS__
