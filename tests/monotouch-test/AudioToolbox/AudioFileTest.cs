// Copyright 2011 Xamarin Inc. All rights reserved

using System.IO;

using AudioToolbox;

namespace MonoTouchFixtures.AudioToolbox {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AudioFileTest {

		[Test]
		public void ReadTest ()
		{
			var path = NSBundle.MainBundle.PathForResource ("1", "caf", "AudioToolbox");
			var af = AudioFile.Open (CFUrl.FromFile (path), AudioFilePermission.Read, AudioFileType.CAF);

			int len;
			long current = 0;
			long size = 1024;
			byte [] buffer = new byte [size];
			while ((len = af.Read (current, buffer, 0, buffer.Length, false)) != -1) {
				current += len;
			}

			var full_len = new FileInfo (path).Length;
			int header = 4096;
			Assert.That (header + current == full_len, "#1");
		}

		[Test]
		public void ApiTest ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			var path = NSBundle.MainBundle.PathForResource ("1", "caf", "AudioToolbox");
			using var af = AudioFile.Open (CFUrl.FromFile (path), AudioFilePermission.Read, AudioFileType.CAF);
			var chunkIds = af.ChunkIDs;
			Assert.That (chunkIds.Length, Is.GreaterThan (0), "ChunkIDs");

			var memorySize = 1024;
			IntPtr memory = Marshal.AllocHGlobal (memorySize); ;
			int size = 0;
			int offset;
			byte [] buffer;
			var expectedData = new byte [] { 0x40, 0xc5, 0x7c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x69, 0x6d, 0x61, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x22, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 };

			try {
				var chunkType = AudioFileChunkType.CAFStreamDescription;
				Assert.Multiple (() => {
					Assert.That (af.CountUserData (chunkType), Is.EqualTo (1), "CountUserData #1");
					Assert.That (af.CountUserData ((uint) chunkType), Is.EqualTo (1), "CountUserData #2");

					Assert.That (af.GetUserDataSize (chunkType, 0), Is.EqualTo (32), "GetUserDataSize #1");
					Assert.That (af.GetUserDataSize ((uint) chunkType, 0), Is.EqualTo (32), "GetUserDataSize #2");

					Assert.That (af.GetUserDataSize (chunkType, 0, out var userDataSize64), Is.EqualTo (AudioFileError.Success), "GetUserDataSize64 #1");
					Assert.That (userDataSize64, Is.EqualTo (32), "GetUserDataSize64 #2");

					Assert.That (af.GetUserDataSize ((uint) chunkType, 0, out userDataSize64), Is.EqualTo (AudioFileError.Success), "GetUserDataSize64 #3");
					Assert.That (userDataSize64, Is.EqualTo (32), "GetUserDataSize64 #4");

					size = memorySize;
					Assert.That (af.GetUserData (chunkType, 0, ref size, memory), Is.EqualTo (AudioFileError.Success), "GetUserData #1");
					Assert.That (size, Is.EqualTo (32), "GetUserData #2");
					Assert.That (expectedData.Length, Is.EqualTo (size), "GetUserData #3");
					for (var i = 0; i < expectedData.Length; i++) {
						Assert.That (Marshal.ReadByte (memory, i), Is.EqualTo (expectedData [i]), $"GetUserData #4[{i}]");
						Marshal.WriteByte (memory, i, 0);
					}

					size = memorySize;
					Assert.That (af.GetUserData ((int) chunkType, 0, ref size, memory), Is.EqualTo (0), "GetUserData/B #1");
					Assert.That (size, Is.EqualTo (32), "GetUserData/B #2");
					Assert.That (expectedData.Length, Is.EqualTo (size), "GetUserData/B #3");
					for (var i = 0; i < expectedData.Length; i++) {
						Assert.That (Marshal.ReadByte (memory, i), Is.EqualTo (expectedData [i]), $"GetUserData/B #4[{i}]");
						Marshal.WriteByte (memory, i, 0);
					}

					size = memorySize;
					offset = 16;
					Assert.That (af.GetUserData (chunkType, 0, offset, ref size, memory), Is.EqualTo (AudioFileError.Success), "GetUserDataAtOffset/A #1");
					Assert.That (size, Is.EqualTo (32 - offset), "GetUserDataAtOffset/A #2");
					Assert.That (expectedData.Length - offset, Is.EqualTo (size), "GetUserDataAtOffset/A #3");
					for (var i = offset; i < expectedData.Length; i++) {
						Assert.That (Marshal.ReadByte (memory, i - offset), Is.EqualTo (expectedData [i]), $"GetUserDataAtOffset/A #4[{i}]");
						Marshal.WriteByte (memory, i - offset, 0);
					}

					size = memorySize;
					offset = 12;
					Assert.That (af.GetUserData ((uint) chunkType, 0, offset, ref size, memory), Is.EqualTo (AudioFileError.Success), "GetUserDataAtOffset/B #1");
					Assert.That (size, Is.EqualTo (32 - offset), "GetUserDataAtOffset/B #2");
					Assert.That (expectedData.Length - offset, Is.EqualTo (size), "GetUserDataAtOffset/B #3");
					for (var i = offset; i < expectedData.Length; i++) {
						Assert.That (Marshal.ReadByte (memory, i - offset), Is.EqualTo (expectedData [i]), $"GetUserDataAtOffset/B #4[{i}]");
						Marshal.WriteByte (memory, i - offset, 0);
					}

					size = memorySize;
					offset = 24;
					buffer = new byte [memorySize];
					Assert.That (af.GetUserData (chunkType, 0, offset, buffer, out size), Is.EqualTo (AudioFileError.Success), "GetUserDataAtOffset/C #1");
					Assert.That (size, Is.EqualTo (32 - offset), "GetUserDataAtOffset/C #2");
					Assert.That (expectedData.Length - offset, Is.EqualTo (size), "GetUserDataAtOffset/C #3");
					for (var i = offset; i < expectedData.Length; i++)
						Assert.That (buffer [i - offset], Is.EqualTo (expectedData [i]), $"GetUserDataAtOffset/C #4[{i}]");

					size = memorySize;
					offset = 8;
					Assert.That (af.GetUserData ((uint) chunkType, 0, offset, buffer, out size), Is.EqualTo (AudioFileError.Success), "GetUserDataAtOffset/D #1");
					Assert.That (size, Is.EqualTo (32 - offset), "GetUserDataAtOffset/D #2");
					Assert.That (expectedData.Length - offset, Is.EqualTo (size), "GetUserDataAtOffset/D #3");
					for (var i = offset; i < expectedData.Length; i++)
						Assert.That (buffer [i - offset], Is.EqualTo (expectedData [i]), $"GetUserDataAtOffset/D #4[{i}]");
				});
			} finally {
				Marshal.FreeHGlobal (memory);
			}
		}
	}
}
