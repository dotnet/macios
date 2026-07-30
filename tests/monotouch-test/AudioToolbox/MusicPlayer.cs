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
				Assert.That (player.IsPlaying, Is.False, "IsPlaying");
				Assert.That (player.Time, Is.EqualTo (0), "Time");
				Assert.That (player.PlayRateScalar, Is.EqualTo (1), "PlayRateScalar");
				Assert.That (player.GetHostTimeForBeats (0, out var hosttime), Is.EqualTo (MusicPlayerStatus.InvalidPlayerState), "GetHostTimeForBeats");
				Assert.That (hosttime, Is.EqualTo (0), "GetHostTimeForBeats - rv");
				Assert.That (player.GetBeatsForHostTime (0, out var beats), Is.EqualTo (MusicPlayerStatus.InvalidPlayerState), "GetBeatsForHostTime");
				Assert.That (beats, Is.EqualTo (0), "GetBeatsForHostTime - rv");
				Assert.That (player.MusicSequence, Is.Null, "MusicSequence");
			}
		}

		[Test]
		public void MusicSequenceTest ()
		{
			using (var player = new MusicPlayer ()) {
				using (var ms = new MusicSequence ()) {
					Assert.That (player.MusicSequence, Is.Null, "MusicSequence A");
					player.MusicSequence = null;
					Assert.That (player.MusicSequence, Is.Null, "MusicSequence B");
					player.MusicSequence = ms;
					Assert.That (player.MusicSequence, Is.SameAs (ms), "MusicSequence C");
					player.MusicSequence = null;
					Assert.That (player.MusicSequence, Is.Null, "MusicSequence D");
				}
			}
		}

		[Test]
		public void PlayRateScalarTest ()
		{
			using (var player = new MusicPlayer ()) {
				Assert.That (player.PlayRateScalar, Is.EqualTo (1), "PlayRateScalar A");
				player.PlayRateScalar = 2;
				Assert.That (player.PlayRateScalar, Is.EqualTo (2), "PlayRateScalar B");
			}
		}

		[Test]
		public void TimeTest ()
		{
			using (var player = new MusicPlayer ()) {
				Assert.That (player.Time, Is.EqualTo (0), "Time A");
				player.Time = 1;
				Assert.That (player.Time, Is.EqualTo (0), "Time B");
				Assert.That (player.GetTime (out var time), Is.EqualTo (MusicPlayerStatus.Success), "GetTime A");
				Assert.That (time, Is.EqualTo (0), "GetTime B");
				Assert.That (player.SetTime (1), Is.EqualTo (MusicPlayerStatus.Success), "SetTime A");
				Assert.That (player.GetTime (out time), Is.EqualTo (MusicPlayerStatus.Success), "GetTime C");
				Assert.That (time, Is.EqualTo (0), "GetTime D");
			}
		}

		[Test]
		public void CreateTest ()
		{
			using var player = MusicPlayer.Create (out var status);
			Assert.That (player, Is.Not.Null, "Got a player");
			Assert.That (status, Is.EqualTo (MusicPlayerStatus.Success), "Status");
		}

		[Test]
		public void StartStopPreroll ()
		{
			using var player = MusicPlayer.Create (out var status);
			Assert.That (player, Is.Not.Null, "Got a player");
			Assert.That (status, Is.EqualTo (MusicPlayerStatus.Success), "Status");
			Assert.That (player.Preroll (), Is.EqualTo (MusicPlayerStatus.NoSequence), "Preroll");
			Assert.That (player.Start (), Is.EqualTo (MusicPlayerStatus.NoSequence), "Start");
			Assert.That (player.Stop (), Is.EqualTo (MusicPlayerStatus.NoSequence), "Stop");
		}
	}
}
