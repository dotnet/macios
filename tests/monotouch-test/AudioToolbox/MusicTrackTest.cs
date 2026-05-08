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

			ClassicAssert.IsFalse (track.MuteStatus, "MuteStatus");
			track.MuteStatus = true;
			ClassicAssert.IsTrue (track.MuteStatus, "MuteStatus B");
			track.MuteStatus = false;
			ClassicAssert.IsFalse (track.MuteStatus, "MuteStatus C");

			ClassicAssert.IsFalse (track.SoloStatus, "SoloStatus");
			track.SoloStatus = true;
			ClassicAssert.IsTrue (track.SoloStatus, "SoloStatus B");
			track.SoloStatus = false;
			ClassicAssert.IsFalse (track.SoloStatus, "SoloStatus C");

			ClassicAssert.AreEqual (0.0f, track.TrackLength, "TrackLength");
			var originalTrackLength = track.TrackLength;
			track.TrackLength = 1.32f;
			ClassicAssert.AreEqual (1.32f, track.TrackLength, "TrackLength B");
			track.TrackLength = originalTrackLength;
			ClassicAssert.AreEqual (0.0f, track.TrackLength, "TrackLength C");
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
					ClassicAssert.AreEqual (endpoint.Handle, outEnpoint.Handle, "Track endpoint.");
				}
			}
		}
	}
}

#endif // !MONOMAC && !__TVOS__
