// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

using Foundation;
using AudioToolbox;

using NUnit.Framework;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioQueueBufferTest {
		[Test]
		public unsafe void Properties ()
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
			AudioQueueBuffer* buffer = null;
			ClassicAssert.AreEqual (AudioQueueStatus.Ok, aq.AllocateBuffer (5000, 1, out buffer), "AllocateBuffer");
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

				buffer->PacketDescriptions = new AudioStreamPacketDescription [] {
					new AudioStreamPacketDescription () {
						StartOffset = 2,
						VariableFramesInPacket = 3,
						DataByteSize = 4,
					},
				};
				ClassicAssert.AreEqual (1, buffer->PacketDescriptionCapacity, "PacketDescriptionCapacity#2");
				ClassicAssert.AreEqual (1, buffer->PacketDescriptionCount, "PacketDescriptionCount#2");
				ClassicAssert.AreEqual (2, buffer->PacketDescriptions [0].StartOffset, "PacketDescriptions[0].StartOffset");
				ClassicAssert.AreEqual (3, buffer->PacketDescriptions [0].VariableFramesInPacket, "PacketDescriptions[0].VariableFramesInPacket");
				ClassicAssert.AreEqual (4, buffer->PacketDescriptions [0].DataByteSize, "PacketDescriptions[0].DataByteSize");

				buffer->PacketDescriptions = new AudioStreamPacketDescription [0];
				ClassicAssert.AreEqual (1, buffer->PacketDescriptionCapacity, "PacketDescriptionCapacity#3");
				ClassicAssert.AreEqual (0, buffer->PacketDescriptionCount, "PacketDescriptionCount#3");

				Assert.Throws<ArgumentOutOfRangeException> (() => buffer->PacketDescriptions = new AudioStreamPacketDescription [2], "too many packet descriptions");

				var data = new byte [] { 1, 2, 3 };
				fixed (byte* dataPtr = data)
					buffer->CopyToAudioData ((IntPtr) dataPtr, data.Length);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 1");
				ClassicAssert.AreEqual (data.Length, buffer->AudioDataByteSize, "CopyToAudioData 1 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 1 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 1 - Sliced AsSpan");

				data = new byte [] { 2, 3, 4, 5, 6 };
				buffer->CopyToAudioData (data);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 2");
				ClassicAssert.AreEqual (data.Length, buffer->AudioDataByteSize, "CopyToAudioData 2 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 2 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 2 - Sliced AsSpan");

				data = new byte [5000];
				buffer->CopyToAudioData (data);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 3");
				ClassicAssert.AreEqual (data.Length, buffer->AudioDataByteSize, "CopyToAudioData 3 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 3 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 3 - Sliced AsSpan");

				data = new byte [5001];
				Assert.Throws<ArgumentException> (() => {
					fixed (byte* dataPtr = data)
						buffer->CopyToAudioData ((IntPtr) dataPtr, data.Length);
				}, "CopyToAudioData 4 - Exception");

				data = new byte [0];
				buffer->CopyToAudioData (IntPtr.Zero, 0);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 5");
				ClassicAssert.AreEqual (data.Length, buffer->AudioDataByteSize, "CopyToAudioData 5 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 5 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 5 - Sliced AsSpan");

				data = new byte [0];
				buffer->CopyToAudioData (data);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 6");
				ClassicAssert.AreEqual (data.Length, buffer->AudioDataByteSize, "CopyToAudioData 6 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 6 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 6 - Sliced AsSpan");

				data = new byte [5001];
				Assert.Throws<ArgumentException> (() => buffer->CopyToAudioData (data), "CopyToAudioData 7 - Exception");
			});
		}
	}
}
