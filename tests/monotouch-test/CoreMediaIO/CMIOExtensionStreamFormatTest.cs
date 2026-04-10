#if __MACOS__ || __MACCATALYST__
#nullable enable

using System;
using CoreMedia;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionStreamFormatTest {

		[Test]
		public void Create_WithVideoFormat ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var formatDescription = new CMVideoFormatDescription (CMVideoCodecType.H264, new CMVideoDimensions (1920, 1080));
			Assert.IsNotNull (formatDescription, "FormatDescription");

			var maxDuration = new CMTime (1, 30);
			var minDuration = new CMTime (1, 60);

			var format = CMIOExtensionStreamFormat.Create (formatDescription, maxDuration, minDuration, null);
			Assert.IsNotNull (format, "Created format");
			Assert.IsNotNull (format.FormatDescription, "FormatDescription");
			Assert.AreEqual (minDuration, format.MinFrameDuration, "MinFrameDuration");
			Assert.AreEqual (maxDuration, format.MaxFrameDuration, "MaxFrameDuration");
			Assert.IsNull (format.ValidFrameDurations, "ValidFrameDurations");

			formatDescription.Dispose ();
		}
	}
}
#endif // __MACOS__ || __MACCATALYST__
