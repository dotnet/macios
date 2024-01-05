//
// Unit tests for AudioQueue
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__WATCHOS__

using System;
using System.Collections.Generic;
using System.Threading;
using Foundation;
using AudioToolbox;
using NUnit.Framework;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioQueueTest {
#if !MONOMAC && !__MACCATALYST__ // HardwareCodecPolicy and SetChannelAssignments are iOS only
		[Test]
		public void Properties ()
		{
			TestRuntime.RequestMicrophonePermission ();

			var b = new InputAudioQueue (AudioStreamBasicDescription.CreateLinearPCM ());

			b.HardwareCodecPolicy = AudioQueueHardwareCodecPolicy.UseSoftwareOnly;

			Assert.That (b.HardwareCodecPolicy, Is.EqualTo (AudioQueueHardwareCodecPolicy.UseSoftwareOnly), "#1");
		}

		[Test]
		public void ChannelAssignments ()
		{
			var aq = new OutputAudioQueue (AudioStreamBasicDescription.CreateLinearPCM ());

			var route = global::AVFoundation.AVAudioSession.SharedInstance ().CurrentRoute;
			var outputs = route.Outputs;
			if (outputs.Length > 0) {
				var port = outputs [0];
				var assignments = new List<AudioQueueChannelAssignment> ();
				var id = port.UID;
				for (int i = 0; i < aq.AudioStreamDescription.ChannelsPerFrame; i++) {
					assignments.Add (new AudioQueueChannelAssignment (id, (uint) i));
				}
				Assert.AreEqual (AudioQueueStatus.Ok, aq.SetChannelAssignments (assignments.ToArray ()));
			} else {
				Assert.Ignore ("No outputs in the current route ({0})", route.Description);
			}

		}
#endif

		[Test]
		public void ProcessingTap ()
		{
			TestRuntime.AssertNotVirtualMachine (); // this test doesn't seem to work well in a virtual machine
			// TestRuntime.RequestMicrophonePermission ();

			using var aq = new InputAudioQueue (AudioStreamBasicDescription.CreateLinearPCM ());
			aq.InputCompleted += (object sender, InputCompletedEventArgs ea) =>
			{
				Console.WriteLine ($"InputCompleted ({sender}, 0x{ea.IntPtrBuffer:X} TimeStamp: {ea.TimeStamp} PacketDescriptions: {ea.PacketDescriptions}");
				unsafe {
					var q = aq.EnqueueBuffer (ea.UnsafeBuffer);
					Console.WriteLine ($"    Enqueue: {q}");
				}
			};
			AudioQueueStatus ret;
			bool called = false;

			unsafe {
				AudioQueueBuffer* buffer;
				Assert.AreEqual (AudioQueueStatus.Ok, aq.AllocateBuffer (4096, out buffer), $"AllocateBuffer");
				Assert.AreEqual (AudioQueueStatus.Ok, aq.EnqueueBuffer (buffer), $"EnqueueBuffer");
			}
			using (var tap = aq.CreateProcessingTap (
				delegate (AudioQueueProcessingTap audioQueueTap, uint numberOfFrames, ref AudioTimeStamp timeStamp, ref AudioQueueProcessingTapFlags flags, AudioBuffers data)
				{
					called = true;
					timeStamp = default (AudioTimeStamp);
					return numberOfFrames;
				}, AudioQueueProcessingTapFlags.PreEffects, out ret)) {
				Console.WriteLine ($"ret: {ret}");
				if (ret == AudioQueueStatus.InvalidDevice)
					Assert.Inconclusive ("Could not find a valid device.");
				Assert.AreEqual (AudioQueueStatus.Ok, ret, "CreateProcessingTap");

				ret = aq.Start ();
				Assert.That (ret, Is.EqualTo (AudioQueueStatus.Ok).Or.EqualTo (AudioQueueStatus.GeneralParamError).Or.EqualTo ((AudioQueueStatus) (-66628)), "#4");
			}

			if (called) {
				Assert.AreEqual (ret, AudioQueueStatus.Ok, "Called A");
			} else {
				Assert.AreNotEqual (ret, AudioQueueStatus.Ok, "Called B");
			}
			Assert.That (aq.Stop (true), Is.EqualTo (ret).Or.EqualTo (AudioQueueStatus.Ok), "#5 - Stop");
		}

		[Test]
		public void InvalidAudioBasicDescription ()
		{
			TestRuntime.RequestMicrophonePermission ();
			Assert.Throws<AudioQueueException> (() => new InputAudioQueue (new AudioStreamBasicDescription ()), "A");
		}
	}
}

#endif // !__WATCHOS__
