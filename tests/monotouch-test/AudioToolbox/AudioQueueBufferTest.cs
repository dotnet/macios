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
			Assert.That (aq.AllocateBuffer (5000, 1, out buffer), Is.EqualTo (AudioQueueStatus.Ok), "AllocateBuffer");
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

				buffer->PacketDescriptions = new AudioStreamPacketDescription [] {
					new AudioStreamPacketDescription () {
						StartOffset = 2,
						VariableFramesInPacket = 3,
						DataByteSize = 4,
					},
				};
				Assert.That (buffer->PacketDescriptionCapacity, Is.EqualTo (1), "PacketDescriptionCapacity#2");
				Assert.That (buffer->PacketDescriptionCount, Is.EqualTo (1), "PacketDescriptionCount#2");
				Assert.That (buffer->PacketDescriptions [0].StartOffset, Is.EqualTo (2), "PacketDescriptions[0].StartOffset");
				Assert.That (buffer->PacketDescriptions [0].VariableFramesInPacket, Is.EqualTo (3), "PacketDescriptions[0].VariableFramesInPacket");
				Assert.That (buffer->PacketDescriptions [0].DataByteSize, Is.EqualTo (4), "PacketDescriptions[0].DataByteSize");

				buffer->PacketDescriptions = new AudioStreamPacketDescription [0];
				Assert.That (buffer->PacketDescriptionCapacity, Is.EqualTo (1), "PacketDescriptionCapacity#3");
				Assert.That (buffer->PacketDescriptionCount, Is.EqualTo (0), "PacketDescriptionCount#3");

				Assert.Throws<ArgumentOutOfRangeException> (() => buffer->PacketDescriptions = new AudioStreamPacketDescription [2], "too many packet descriptions");

				var data = new byte [] { 1, 2, 3 };
				fixed (byte* dataPtr = data)
					buffer->CopyToAudioData ((IntPtr) dataPtr, data.Length);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 1");
				Assert.That (buffer->AudioDataByteSize, Is.EqualTo (data.Length), "CopyToAudioData 1 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 1 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 1 - Sliced AsSpan");

				data = new byte [] { 2, 3, 4, 5, 6 };
				buffer->CopyToAudioData (data);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 2");
				Assert.That (buffer->AudioDataByteSize, Is.EqualTo (data.Length), "CopyToAudioData 2 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 2 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 2 - Sliced AsSpan");

				data = new byte [5000];
				buffer->CopyToAudioData (data);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 3");
				Assert.That (buffer->AudioDataByteSize, Is.EqualTo (data.Length), "CopyToAudioData 3 - AudioDataByteSize");
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
				Assert.That (buffer->AudioDataByteSize, Is.EqualTo (data.Length), "CopyToAudioData 5 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 5 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 5 - Sliced AsSpan");

				data = new byte [0];
				buffer->CopyToAudioData (data);
				Assert.That (buffer->AsSpanOfValidData ().ToArray (), Is.EqualTo (data), "CopyToAudioData 6");
				Assert.That (buffer->AudioDataByteSize, Is.EqualTo (data.Length), "CopyToAudioData 6 - AudioDataByteSize");
				Assert.That (buffer->AsSpan ().Length, Is.EqualTo (5000), "CopyToAudioData 6 - AsSpan");
				Assert.That (buffer->AsSpan ().Slice (0, data.Length).ToArray (), Is.EqualTo (buffer->AsSpanOfValidData ().ToArray ()), "CopyToAudioData 6 - Sliced AsSpan");

				data = new byte [5001];
				Assert.Throws<ArgumentException> (() => buffer->CopyToAudioData (data), "CopyToAudioData 7 - Exception");
			});
		}
	}
}
