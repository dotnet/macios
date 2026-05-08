#if __MACOS__

using AppKit;
using AudioUnit;
using AVFoundation;
using AUUnit = AudioUnit.AudioUnit;

namespace Xamarin.Mac.Tests {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVPlayerLayerTests {
		[Test]
		public void AVPlayerLayer_VideoGravity ()
		{
			AVPlayerLayer layer = new AVPlayerLayer ();
			Assert.That (layer.VideoGravity, Is.Not.Null);
		}
	}
}
#endif // __MACOS__
