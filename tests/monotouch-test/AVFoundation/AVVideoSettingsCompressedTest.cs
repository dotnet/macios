using System.IO;
using Foundation;
using AVFoundation;
using NUnit.Framework;
namespace monotouchtest.AVFoundation {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVVideoSettingsCompressedTest {
		public void CodecType ()
		{
			var settings = new AVVideoSettingsCompressed ();
			Assert.That (settings.CodecType, Is.Null, "CodecType A");
			Assert.That (settings.Codec, Is.Null, "Codec A");
			settings.CodecType = AVVideoCodecType.H264;
			Assert.That (settings.CodecType, Is.EqualTo ((AVVideoCodecType?) AVVideoCodecType.H264), "CodecType H264");
			Assert.That (settings.Codec, Is.EqualTo ((AVVideoCodec?) AVVideoCodec.H264), "Codec H264");
			settings.CodecType = null;
			Assert.That (settings.CodecType, Is.Null, "CodecType C");
			Assert.That (settings.Codec, Is.Null, "Codec C");
		}
	}
}
