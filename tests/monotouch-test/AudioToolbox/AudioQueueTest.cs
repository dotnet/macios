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
				Assert.That (aq.SetChannelAssignments (assignments.ToArray ()), Is.EqualTo (AudioQueueStatus.Ok));
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
				Assert.That (ret, Is.EqualTo (AudioQueueStatus.Ok), "#1");

				unsafe {
					AudioQueueBuffer* buffer;
					Assert.That (aq.AllocateBuffer (5000, out buffer), Is.EqualTo (AudioQueueStatus.Ok), "#2");
					Assert.That (aq.EnqueueBuffer (buffer), Is.EqualTo (AudioQueueStatus.Ok), "#3");
					//Assert.That (aq.Start (), Is.EqualTo (AudioQueueStatus.Ok), "#4");
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
			Assert.That (aq.AllocateBuffer (5000, out AudioQueueBuffer* buffer), Is.EqualTo (AudioQueueStatus.Ok), "AllocateBuffer");
			Assert.Multiple (() => {
				Assert.That (buffer->AudioDataBytesCapacity, Is.EqualTo (5000), "AudioDataBytesCapacity");
				Assert.That (buffer->AudioData, Is.Not.EqualTo (IntPtr.Zero), "AudioData");
				Assert.That (buffer->AudioDataByteSize, Is.EqualTo (0), "AudioDataByteSize");
				Assert.That (buffer->UserData, Is.EqualTo (IntPtr.Zero), "UserData");
				Assert.That (buffer->PacketDescriptionCapacity, Is.EqualTo (0), "PacketDescriptionCapacity");
				Assert.That (buffer->IntPtrPacketDescriptions, Is.EqualTo (IntPtr.Zero), "IntPtrPacketDescriptions");
				Assert.That (buffer->PacketDescriptionCount, Is.EqualTo (0), "PacketDescriptionCount");
				Assert.That (buffer->PacketDescriptions.Length, Is.EqualTo (0), "PacketDescriptions");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "AsSpan ().Length");
				Assert.That (buffer->AsSpanOfValidData ().Length, Is.EqualTo (0), "AsSpanOfValidData ().Length");
			});
			Assert.That (aq.FreeBuffer (buffer), Is.EqualTo (AudioQueueStatus.Ok), "FreeBuffer");
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
			Assert.That (aq.AllocateBuffer (5000, 1, out var buffer), Is.EqualTo (AudioQueueStatus.Ok), "AllocateBuffer");
			Assert.Multiple (() => {
				Assert.That (buffer->AudioDataBytesCapacity, Is.EqualTo (5000), "AudioDataBytesCapacity");
				Assert.That (buffer->AudioData, Is.Not.EqualTo (IntPtr.Zero), "AudioData");
				Assert.That (buffer->AudioDataByteSize, Is.EqualTo (0), "AudioDataByteSize");
				Assert.That (buffer->UserData, Is.EqualTo (IntPtr.Zero), "UserData");
				Assert.That (buffer->PacketDescriptionCapacity, Is.EqualTo (1), "PacketDescriptionCapacity");
				Assert.That (buffer->IntPtrPacketDescriptions, Is.Not.EqualTo (IntPtr.Zero), "IntPtrPacketDescriptions");
				Assert.That (buffer->PacketDescriptionCount, Is.EqualTo (0), "PacketDescriptionCount");
				Assert.That (buffer->PacketDescriptions.Length, Is.EqualTo (0), "PacketDescriptions");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "AsSpan ().Length");
				Assert.That (buffer->AsSpanOfValidData ().Length, Is.EqualTo (0), "AsSpanOfValidData ().Length");
			});
		}
	}
}
