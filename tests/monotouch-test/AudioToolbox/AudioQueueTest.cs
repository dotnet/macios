//
// Unit tests for AudioQueue
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using AudioToolbox;

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
				ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.SetChannelAssignments (assignments.ToArray ()));
			} else {
				Assert.Ignore ($"No outputs in the current route ({route.Description})");
			}

		}
#endif

		[Test]
		[Ignore ("Fails on some machines with undefined error code 5")]
		public void ProcessingTap ()
		{
			var aq = new InputAudioQueue (AudioStreamBasicDescription.CreateLinearPCM ());
			AudioQueueStatus ret;
			// bool called = false;

			using (var tap = aq.CreateProcessingTap (
				delegate (AudioQueueProcessingTap audioQueueTap, uint numberOfFrames, ref AudioTimeStamp timeStamp, ref AudioQueueProcessingTapFlags flags, AudioBuffers data)
				{
					// called = true;
					return 33;
				}, AudioQueueProcessingTapFlags.PreEffects, out ret)) {
				ClassicAssert.AreEqual (AudioQueueStatus.Ok, ret, "#1");

				unsafe {
					AudioQueueBuffer* buffer;
					ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.AllocateBuffer (5000, out buffer), "#2");
					ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.EnqueueBuffer (buffer), "#3");
					//ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.Start (), "#4");
				}
			}

			//Assert.That (called, Is.True, "#10");
		}

		[Test]
		public void InvalidAudioBasicDescription ()
		{
			TestRuntime.RequestMicrophonePermission ();
			Assert.Throws<AudioQueueException> (() => new InputAudioQueue (new AudioStreamBasicDescription ()), "A");
		}

		[Test]
		public unsafe void AllocateBuffer_1 ()
		{
			var asbd = AudioStreamBasicDescription.CreateLinearPCM ();
			using var aq = new InputAudioQueue (asbd);
			ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.AllocateBuffer (5000, out AudioQueueBuffer* buffer), "AllocateBuffer");
			Assert.Multiple (() => {
				ClassicAssert.AreEqual (5000, buffer->AudioDataBytesCapacity, "AudioDataBytesCapacity");
				ClassicAssert.AreNotEqual (IntPtr.Zero, buffer->AudioData, "AudioData");
				ClassicAssert.AreEqual (0, buffer->AudioDataByteSize, "AudioDataByteSize");
				ClassicAssert.AreEqual (IntPtr.Zero, buffer->UserData, "UserData");
				ClassicAssert.AreEqual (0, buffer->PacketDescriptionCapacity, "PacketDescriptionCapacity");
				ClassicAssert.AreEqual (IntPtr.Zero, buffer->IntPtrPacketDescriptions, "IntPtrPacketDescriptions");
				ClassicAssert.AreEqual (0, buffer->PacketDescriptionCount, "PacketDescriptionCount");
				ClassicAssert.AreEqual (0, buffer->PacketDescriptions.Length, "PacketDescriptions");
				ClassicAssert.AreEqual (5000, buffer->AsSpan ().Length, "AsSpan ().Length");
				ClassicAssert.AreEqual (0, buffer->AsSpanOfValidData ().Length, "AsSpanOfValidData ().Length");
			});
			ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.FreeBuffer (buffer), "FreeBuffer");
		}

		[Test]
		public unsafe void AllocateBuffer_2 ()
		{
			var asbd = new AudioStreamBasicDescription () {
				Format = AudioFormatType.MPEG4AAC_HE,
				BytesPerPacket = 0,
				BitsPerChannel = 0,
				Reserved = 0,
				FormatFlags = 0,
				BytesPerFrame = 0,
				SampleRate = 16000,
				ChannelsPerFrame = 1,
				FramesPerPacket = 1024,
			};
			using var aq = new OutputAudioQueue (asbd);
			ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.AllocateBuffer (5000, 1, out var buffer), "AllocateBuffer");
			Assert.Multiple (() => {
				ClassicAssert.AreEqual (5000, buffer->AudioDataBytesCapacity, "AudioDataBytesCapacity");
				ClassicAssert.AreNotEqual (IntPtr.Zero, buffer->AudioData, "AudioData");
				ClassicAssert.AreEqual (0, buffer->AudioDataByteSize, "AudioDataByteSize");
				ClassicAssert.AreEqual (IntPtr.Zero, buffer->UserData, "UserData");
				ClassicAssert.AreEqual (1, buffer->PacketDescriptionCapacity, "PacketDescriptionCapacity");
				ClassicAssert.AreNotEqual (IntPtr.Zero, buffer->IntPtrPacketDescriptions, "IntPtrPacketDescriptions");
				ClassicAssert.AreEqual (0, buffer->PacketDescriptionCount, "PacketDescriptionCount");
				ClassicAssert.AreEqual (0, buffer->PacketDescriptions.Length, "PacketDescriptions");
				ClassicAssert.AreEqual (5000, buffer->AsSpan ().Length, "AsSpan ().Length");
				ClassicAssert.AreEqual (0, buffer->AsSpanOfValidData ().Length, "AsSpanOfValidData ().Length");
			});
		}
	}
}
