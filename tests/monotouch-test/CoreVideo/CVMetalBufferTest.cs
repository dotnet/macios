//
// Unit tests for CVMetalBuffer
//

using CoreVideo;
using Metal;

namespace MonoTouchFixtures.CoreVideo {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CVMetalBufferTests {

		[Test]
		public void GetTypeIdTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			Assert.That (CVMetalBuffer.GetTypeId (), Is.Not.EqualTo (0), "GetTypeId");
		}

#if !MONOMAC
		[Test]
		[TestCase (CVPixelFormatType.CV32BGRA)]
		public void GetMetalBufferTest (CVPixelFormatType pft)
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			TestRuntime.AssertNotSimulator (); // metal api not supported in the simulator
			CVMetalBufferCacheTests.AssertSupported ();

			using var device = MTLDevice.SystemDefault;
			using var cache = new CVMetalBufferCache (device, (CVMetalBufferCacheAttributes) null);
			var dict = new CVPixelBufferAttributes () {
				MetalCompatibility = true,
			};
			using var image = new CVPixelBuffer (320, 320, pft, dict);
			using var buffer = cache.CreateBufferFromImage (image);
			Assert.That (buffer, Is.Not.Null, "Buffer");
			using var metalBuffer = buffer.GetMetalBuffer ();
			Assert.That (metalBuffer, Is.Not.Null, "GetMetalBuffer");
		}
#endif // !MONOMAC
	}
}
