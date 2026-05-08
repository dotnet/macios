//
// MusicPlayer unit Tests
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2023 Microsoft Corp. All rights reserved.
//

using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MusicPlayerTest {

		[Test]
		public void Defaults ()
		{
			using (var player = new MusicPlayer ()) {
				ClassicAssert.IsFalse (player.IsPlaying, "IsPlaying");
				ClassicAssert.AreEqual (0, player.Time, "Time");
				ClassicAssert.AreEqual (1, player.PlayRateScalar, "PlayRateScalar");
				ClassicAssert.AreEqual (MusicPlayerStatus.InvalidPlayerState, player.GetHostTimeForBeats (0, out var hosttime), "GetHostTimeForBeats");
				ClassicAssert.AreEqual (0, hosttime, "GetHostTimeForBeats - rv");
				ClassicAssert.AreEqual (MusicPlayerStatus.InvalidPlayerState, player.GetBeatsForHostTime (0, out var beats), "GetBeatsForHostTime");
				ClassicAssert.AreEqual (0, beats, "GetBeatsForHostTime - rv");
				ClassicAssert.IsNull (player.MusicSequence, "MusicSequence");
			}
		}

		[Test]
		public void MusicSequenceTest ()
		{
			using (var player = new MusicPlayer ()) {
				using (var ms = new MusicSequence ()) {
					ClassicAssert.IsNull (player.MusicSequence, "MusicSequence A");
					player.MusicSequence = null;
					ClassicAssert.IsNull (player.MusicSequence, "MusicSequence B");
					player.MusicSequence = ms;
					ClassicAssert.AreSame (ms, player.MusicSequence, "MusicSequence C");
					player.MusicSequence = null;
					ClassicAssert.IsNull (player.MusicSequence, "MusicSequence D");
				}
			}
		}

		[Test]
		public void PlayRateScalarTest ()
		{
			using (var player = new MusicPlayer ()) {
				ClassicAssert.AreEqual (1, player.PlayRateScalar, "PlayRateScalar A");
				player.PlayRateScalar = 2;
				ClassicAssert.AreEqual (2, player.PlayRateScalar, "PlayRateScalar B");
			}
		}

		[Test]
		public void TimeTest ()
		{
			using (var player = new MusicPlayer ()) {
				ClassicAssert.AreEqual (0, player.Time, "Time A");
				player.Time = 1;
				ClassicAssert.AreEqual (0, player.Time, "Time B");
				ClassicAssert.AreEqual (MusicPlayerStatus.Success, player.GetTime (out var time), "GetTime A");
				ClassicAssert.AreEqual (0, time, "GetTime B");
				ClassicAssert.AreEqual (MusicPlayerStatus.Success, player.SetTime (1), "SetTime A");
				ClassicAssert.AreEqual (MusicPlayerStatus.Success, player.GetTime (out time), "GetTime C");
				ClassicAssert.AreEqual (0, time, "GetTime D");
			}
		}

		[Test]
		public void CreateTest ()
		{
			using var player = MusicPlayer.Create (out var status);
			ClassicAssert.NotNull (player, "Got a player");
			ClassicAssert.AreEqual (MusicPlayerStatus.Success, status, "Status");
		}

		[Test]
		public void StartStopPreroll ()
		{
			using var player = MusicPlayer.Create (out var status);
			ClassicAssert.NotNull (player, "Got a player");
			ClassicAssert.AreEqual (MusicPlayerStatus.Success, status, "Status");
			ClassicAssert.AreEqual (MusicPlayerStatus.NoSequence, player.Preroll (), "Preroll");
			ClassicAssert.AreEqual (MusicPlayerStatus.NoSequence, player.Start (), "Start");
			ClassicAssert.AreEqual (MusicPlayerStatus.NoSequence, player.Stop (), "Stop");
		}
	}
}
