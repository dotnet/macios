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
				Assert.That (format == format, Is.True, "format == format");
#pragma warning restore
		}

		[Test]
		public void TestEqualOperatorNull ()
		{
			using (var format = new AVAudioFormat ()) {
				Assert.That (format == null, Is.False, "format == null");
				Assert.That (null == format, Is.False, "null == format");
			}
			using (AVAudioFormat nullFormat = null) {
				Assert.That (nullFormat == null, Is.True, "nullFormat == null");
				Assert.That (null == nullFormat, Is.True, "null == nullFormat");
			}
		}

		[Test]
		public void TestNotEqualOperatorNull ()
		{
			using (var format = new AVAudioFormat ()) {
				Assert.That (format != null, Is.True, "format != null");
				Assert.That (null != format, Is.True, "null != format");
			}
			using (AVAudioFormat nullFormat = null) {
				Assert.That (nullFormat != null, Is.False, "nullFormat != null");
				Assert.That (null != nullFormat, Is.False, "null != nullFormat");
			}

		}

		[Test]
		public void StreamDescription ()
		{
			var format = new AVAudioFormat (AVAudioCommonFormat.PCMFloat32, 44100.0, 2, true);
			var desc = format.StreamDescription;
			Assert.That (desc.Format, Is.EqualTo (AudioFormatType.LinearPCM), "Format");
			Assert.That (desc.FormatFlags, Is.EqualTo (AudioFormatFlags.LinearPCMIsFloat | AudioFormatFlags.LinearPCMIsPacked), "FormatFlags");
			Assert.That (desc.BytesPerPacket, Is.EqualTo (8), "BytesPerPacket");
			Assert.That (desc.FramesPerPacket, Is.EqualTo (1), "FramesPerPacket");
			Assert.That (desc.BytesPerFrame, Is.EqualTo (8), "BytesPerFrame");
			Assert.That (desc.ChannelsPerFrame, Is.EqualTo (2), "ChannelsPerFrame");
			Assert.That (desc.BitsPerChannel, Is.EqualTo (32), "BitsPerChannel");
			Assert.That (desc.Reserved, Is.EqualTo (0), "Reserved");
		}
	}
}
