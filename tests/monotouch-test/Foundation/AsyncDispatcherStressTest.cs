// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//
// ⚠️ THROWAWAY INSTRUMENTATION / STRESS TEST — DO NOT MERGE ⚠️
//
// Stress test to reproduce the GC/marshalling race in
// https://github.com/dotnet/macios/issues/25861 faster.
//
// The failure was observed to originate from TouchRunner.TestFinished, which
// (on the background NUnit thread) calls NSObject.BeginInvokeOnMainThread after
// every test. That creates an __MonoMac_NSAsyncActionDispatcher on the
// background thread; the main run loop later invokes it. The race is between the
// GC and that main-thread invocation.
//
// This test reproduces exactly that pattern: several background threads hammer
// BeginInvokeOnMainThread while another thread forces the GC in a tight loop,
// and (matching the observed correlation with AudioPlayerTest) we also allocate
// AVAudioPlayer instances to add GC pressure.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

using AVFoundation;
using Foundation;

using NUnit.Framework;

#nullable enable

namespace MonoTouchFixtures.Foundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AsyncDispatcherStressTest {

		volatile bool stop;
		long dispatchersCreated;
		long applied;

		[Test]
		public void Issue25861 ()
		{
			// Time-box the stress test so it doesn't run forever on CI.
			var duration = TimeSpan.FromSeconds (45);
			var receiver = new NSObject ();

			stop = false;
			dispatchersCreated = 0;
			applied = 0;

			// A thread that forces the GC as fast as possible.
			var gcThread = new Thread (() => {
				while (!stop) {
					GC.Collect ();
					GC.WaitForPendingFinalizers ();
				}
			}) { IsBackground = true, Name = "issue25861-gc" };

			// Several producer threads hammering BeginInvokeOnMainThread, mimicking
			// TouchRunner.TestFinished (background thread -> BeginInvokeOnMainThread).
			// Use many more producers than the single main thread can apply per run
			// loop iteration, so a backlog of in-flight (created-but-not-yet-applied)
			// dispatchers builds up - more objects being marshalled native->managed
			// concurrently with the GC means more chances to hit the race.
			var producers = new Thread [32];
			for (var i = 0; i < producers.Length; i++) {
				producers [i] = new Thread (() => {
					while (!stop) {
						receiver.BeginInvokeOnMainThread (() => {
							Interlocked.Increment (ref applied);
						});
						Interlocked.Increment (ref dispatchersCreated);
					}
				}) { IsBackground = true, Name = $"issue25861-producer-{i}" };
			}

			// A thread that allocates AVAudioPlayer instances (matches the observed
			// correlation with AudioPlayerTest, adds GC pressure). Best-effort.
			var audioThread = new Thread (() => {
				string? file = null;
				try {
					file = Path.Combine (NSBundle.MainBundle.ResourcePath!, "Hand.wav");
					if (!File.Exists (file))
						file = null;
				} catch {
					file = null;
				}
				while (!stop && file is not null) {
					try {
						using var url = new NSUrl (file, false);
						using var ap = AVAudioPlayer.FromUrl (url, out var _);
					} catch {
						// ignore
					}
				}
			}) { IsBackground = true, Name = "issue25861-audio" };

			gcThread.Start ();
			audioThread.Start ();
			foreach (var p in producers)
				p.Start ();

			var sw = Stopwatch.StartNew ();
			var lastReport = TimeSpan.Zero;
			// The main thread must actively pump the run loop (this is the
			// NSRunLoop.RunUntil code path from the issue) so that the queued
			// __MonoMac_NSAsyncActionDispatcher instances are applied on the main
			// thread *concurrently* with the background threads that create them
			// and force the GC. If we just Thread.Sleep here instead, the run loop
			// never pumps, the dispatchers pile up unapplied, and there's no
			// concurrency between Apply (main thread) and create/GC (background) -
			// which is exactly what's needed to trigger the race.
			while (sw.Elapsed < duration) {
				NSRunLoop.Current.RunUntil (NSDate.FromTimeIntervalSinceNow (0.02));
				if (sw.Elapsed - lastReport >= TimeSpan.FromSeconds (1)) {
					lastReport = sw.Elapsed;
					var created = Interlocked.Read (ref dispatchersCreated);
					var app = Interlocked.Read (ref applied);
					Console.WriteLine ($"#25861# stress: {sw.Elapsed.TotalSeconds:F0}s created={created} applied={app}");
				}
			}

			stop = true;
			gcThread.Join ();
			audioThread.Join ();
			foreach (var p in producers)
				p.Join ();

			Console.WriteLine ($"#25861# stress DONE: created={Interlocked.Read (ref dispatchersCreated)} applied={Interlocked.Read (ref applied)}");

			// If we got here without the marshalling exception, the race didn't
			// trigger this run. The test always "passes" - we're only interested in
			// whether error 8027 is raised.
			Assert.Pass ($"Created {Interlocked.Read (ref dispatchersCreated)} dispatchers without reproducing issue #25861.");
		}
	}
}
