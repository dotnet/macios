//
// Unit tests for AudioQueue
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

using AudioToolbox;
using CoreFoundation;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioQueueTest {
		[Test]
		[TestCase (0)]
		[TestCase (1)]
		[TestCase (2)]
		public void OutputFormatInputs (int overload)
		{
			var format = AudioStreamBasicDescription.CreateLinearPCM ();
			var expected = format;
			using var mode = new CFString ("kCFRunLoopDefaultMode");
#if XAMCORE_5_0
			using var queue = overload switch {
				0 => new OutputAudioQueue (in format),
				1 => new OutputAudioQueue (in format, CFRunLoop.Current, "kCFRunLoopDefaultMode"),
				_ => new OutputAudioQueue (in format, CFRunLoop.Current, mode),
			};
			var status = queue.SetOfflineRenderFormat (in format, null);
#else
			using var queue = overload switch {
				0 => new OutputAudioQueue (format),
				1 => new OutputAudioQueue (format, CFRunLoop.Current, "kCFRunLoopDefaultMode"),
				_ => new OutputAudioQueue (format, CFRunLoop.Current, mode),
			};
			var status = queue.SetOfflineRenderFormat (format, null);
#endif
			Assert.That (queue.GetProperty<AudioStreamBasicDescription> (AudioQueueProperty.StreamDescription), Is.EqualTo (expected), "Format");
			Assert.That (status, Is.EqualTo (AudioQueueStatus.Ok), "Offline format");
			Assert.That (queue.DisableOfflineRender (), Is.EqualTo (AudioQueueStatus.Ok), "Disable offline render");
			Assert.That (format, Is.EqualTo (expected), "Input");
		}

		[Test]
		[TestCase (false)]
		[TestCase (true)]
		public void InputFormatInputs (bool runLoop)
		{
			TestRuntime.RequestMicrophonePermission ();
			var format = AudioStreamBasicDescription.CreateLinearPCM ();
			var expected = format;
#if XAMCORE_5_0
			using var queue = runLoop ? new InputAudioQueue (in format, CFRunLoop.Current, "kCFRunLoopDefaultMode") : new InputAudioQueue (in format);
#else
			using var queue = runLoop ? new InputAudioQueue (format, CFRunLoop.Current, "kCFRunLoopDefaultMode") : new InputAudioQueue (format);
#endif
			Assert.That (queue.GetProperty<AudioStreamBasicDescription> (AudioQueueProperty.StreamDescription), Is.EqualTo (expected), "Format");
			Assert.That (format, Is.EqualTo (expected), "Input");
		}

		[Test]
		public unsafe void TimestampInputs ()
		{
			TestRuntime.AssertNotVirtualMachine ();
			TestRuntime.IgnoreIfLockedScreen ();
			using var queue = new OutputAudioQueue (AudioStreamBasicDescription.CreateLinearPCM ());
			Assert.That (queue.AllocateBuffer (4096, out AudioQueueBuffer* buffer), Is.EqualTo (AudioQueueStatus.Ok), "Allocate");
			buffer->AsSpan ().Clear ();
			buffer->AudioDataByteSize = 4096;
			Assert.That (queue.EnqueueBuffer (buffer, []), Is.EqualTo (AudioQueueStatus.Ok), "Enqueue");
			var startTime = new AudioTimeStamp ();
			var expectedStart = startTime;
#if XAMCORE_5_0
			var status = queue.Start (in startTime);
#else
			var status = queue.Start (startTime);
#endif
			Assert.That (status, Is.EqualTo (AudioQueueStatus.Ok), "Start with timestamp");
			try {
				var time = queue.CurrentTime;
				var expectedTime = time;
#if XAMCORE_5_0
				var translated = queue.TranslateTime (in time);
#else
				var translated = queue.TranslateTime (time);
#endif
				var translatedFlags = AudioTimeStamp.AtsFlags.SampleTimeValid | AudioTimeStamp.AtsFlags.HostTimeValid;
				Assert.That (translated.Flags & translatedFlags, Is.EqualTo (translatedFlags), "Translated flags");
				Assert.That (translated.SampleTime, Is.EqualTo (time.SampleTime).Within (1), "Translated sample time");
				Assert.That (time, Is.EqualTo (expectedTime), "Translate input");
			} finally {
				queue.Stop (true);
			}
			Assert.That (startTime, Is.EqualTo (expectedStart), "Start input");
			Assert.That (queue.Reset (), Is.EqualTo (AudioQueueStatus.Ok), "Reset");
			Assert.That (queue.EnqueueBuffer (buffer, []), Is.EqualTo (AudioQueueStatus.Ok), "Re-enqueue");
			Assert.That (queue.Start (), Is.EqualTo (AudioQueueStatus.Ok), "Start without timestamp");
			Assert.That (queue.Stop (true), Is.EqualTo (AudioQueueStatus.Ok), "Stop");
		}

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
			TestRuntime.AssertNotVirtualMachine ();

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
		[TestCase (false)]
		[TestCase (true)]
		public unsafe void EnqueueBufferPreservesStartTime (bool usePointer)
		{
			using var queue = new OutputAudioQueue (AudioStreamBasicDescription.CreateLinearPCM ());
			Assert.That (queue.AllocateBuffer (4096, out AudioQueueBuffer* buffer), Is.EqualTo (AudioQueueStatus.Ok), "AllocateBuffer");
			buffer->AsSpan ().Clear ();
			buffer->AudioDataByteSize = 4096;
			var startTime = new AudioTimeStamp {
				SampleTime = 1234,
				Flags = AudioTimeStamp.AtsFlags.SampleTimeValid,
			};
			var expected = startTime;
			AudioQueueStatus status;
			if (usePointer) {
#if XAMCORE_5_0
				status = queue.EnqueueBuffer (buffer, 4096, [], 0, 0, [], in startTime, out var actualStartTime);
#else
				status = queue.EnqueueBuffer (buffer, 4096, [], 0, 0, [], ref startTime, out var actualStartTime);
#endif
			} else {
#if XAMCORE_5_0
				status = queue.EnqueueBuffer ((IntPtr) buffer, 4096, [], 0, 0, [], in startTime, out var actualStartTime);
#else
				status = queue.EnqueueBuffer ((IntPtr) buffer, 4096, [], 0, 0, [], ref startTime, out var actualStartTime);
#endif
			}
			Assert.That (status, Is.EqualTo (AudioQueueStatus.Ok), "EnqueueBuffer");
			Assert.That (startTime, Is.EqualTo (expected), "StartTime");
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
