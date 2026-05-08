// Unit test for AVAudioFormat
// Authors: 
// 		Whitney Schmidt (whschm@microsoft.com)
// Copyright 2020 Microsoft Corp.

using AudioToolbox;
using AVFoundation;
using Xamarin.Utils;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVAudioFormatTest {

		[SetUp]
		public void Setup ()
		{
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 10, 10, throwIfOtherPlatform: false);
		}

		[Test]
		public void TestEqualOperatorSameInstace ()
		{
			using (var format = new AVAudioFormat ())
#pragma warning disable CS1718 // warning CS1718: Comparison made to same variable; did you mean to compare something else?
				ClassicAssert.IsTrue (format == format, "format == format");
#pragma warning restore
		}

		[Test]
		public void TestEqualOperatorNull ()
		{
			using (var format = new AVAudioFormat ()) {
				ClassicAssert.IsFalse (format == null, "format == null");
				ClassicAssert.IsFalse (null == format, "null == format");
			}
			using (AVAudioFormat nullFormat = null) {
				ClassicAssert.IsTrue (nullFormat == null, "nullFormat == null");
				ClassicAssert.IsTrue (null == nullFormat, "null == nullFormat");
			}
		}

		[Test]
		public void TestNotEqualOperatorNull ()
		{
			using (var format = new AVAudioFormat ()) {
				ClassicAssert.IsTrue (format != null, "format != null");
				ClassicAssert.IsTrue (null != format, "null != format");
			}
			using (AVAudioFormat nullFormat = null) {
				ClassicAssert.IsFalse (nullFormat != null, "nullFormat != null");
				ClassicAssert.IsFalse (null != nullFormat, "null != nullFormat");
			}

		}

		[Test]
		public void StreamDescription ()
		{
			var format = new AVAudioFormat (AVAudioCommonFormat.PCMFloat32, 44100.0, 2, true);
			var desc = format.StreamDescription;
			ClassicAssert.AreEqual (AudioFormatType.LinearPCM, desc.Format, "Format");
			ClassicAssert.AreEqual (AudioFormatFlags.LinearPCMIsFloat | AudioFormatFlags.LinearPCMIsPacked, desc.FormatFlags, "FormatFlags");
			ClassicAssert.AreEqual (8, desc.BytesPerPacket, "BytesPerPacket");
			ClassicAssert.AreEqual (1, desc.FramesPerPacket, "FramesPerPacket");
			ClassicAssert.AreEqual (8, desc.BytesPerFrame, "BytesPerFrame");
			ClassicAssert.AreEqual (2, desc.ChannelsPerFrame, "ChannelsPerFrame");
			ClassicAssert.AreEqual (32, desc.BitsPerChannel, "BitsPerChannel");
			ClassicAssert.AreEqual (0, desc.Reserved, "Reserved");
		}
	}
}
