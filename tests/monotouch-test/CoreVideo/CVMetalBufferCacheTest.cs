//
// Unit tests for CVMetalBufferCache
//

using CoreVideo;
using Metal;

namespace MonoTouchFixtures.CoreVideo {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CVMetalBufferCacheTests {

		static bool? supported;
		public static bool Supported {
			get {
				if (supported is null) {
					using var device = MTLDevice.SystemDefault;
					if (device is null) {
						supported = false;
					} else {
						supported = CVMetalBufferCache.TryCreate (device, (NSDictionary) null, out var cache, out var _);
						cache?.Dispose ();
					}
				}
				return supported.Value;
			}
		}

		public static void AssertSupported ()
		{
			if (Supported)
				return;
			Assert.Ignore ("CVMetalBufferCache is not supported on this machine.");
		}

		[Test]
		public void GetTypeIdTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			Assert.That (CVMetalBufferCache.GetTypeId (), Is.Not.EqualTo (0), "GetTypeId");
		}

		[Test]
		public void CtorTest_NSDictionary ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			AssertSupported ();

			using var device = MTLDevice.SystemDefault;
			using var cache = new CVMetalBufferCache (device, (NSDictionary) null);
			Assert.That (cache, Is.Not.Null);
		}

		[Test]
		public void TryCreate ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Ignore ("Metal is not supported on this machine.");

			var rv = CVMetalBufferCache.TryCreate (device, (NSDictionary) null, out var metalBufferCache, out var status);
			if (rv) {
				Assert.That (status, Is.EqualTo (CVReturn.Success), "Status A");
				Assert.That (metalBufferCache, Is.Not.Null, "MetalBufferCache A");
			} else {
				Assert.That (status, Is.EqualTo (CVReturn.Unsupported), "Status B");
				Assert.That (metalBufferCache, Is.Null, "MetalBufferCache B");
			}
			metalBufferCache?.Dispose ();
		}

		[Test]
		public void TryCreateHandle ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			using var device = MTLDevice.SystemDefault;
			if (device is null)
				Assert.Ignore ("Metal is not supported on this machine.");

			var rv = CVMetalBufferCache.TryCreateHandle (device, (NSDictionary) null, out var metalBufferCache, out var status);
			if (rv) {
				Assert.That (status, Is.EqualTo (CVReturn.Success), "Status A");
				Assert.That (metalBufferCache, Is.Not.EqualTo (IntPtr.Zero), "MetalBufferCache A");
			} else {
				Assert.That (status, Is.EqualTo (CVReturn.Unsupported), "Status B");
				Assert.That (metalBufferCache, Is.EqualTo (IntPtr.Zero), "MetalBufferCache B");
			}
			if (metalBufferCache != IntPtr.Zero)
				TestRuntime.CFRelease (metalBufferCache);
		}

		[Test]
		public void CtorTest_CVMetalBufferCacheAttributes ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			AssertSupported ();

			using var device = MTLDevice.SystemDefault;
			using var cache = new CVMetalBufferCache (device, (CVMetalBufferCacheAttributes) null);
			Assert.That (cache, Is.Not.Null);
		}

#if !MONOMAC
		[Test]
		[TestCase (CVPixelFormatType.CV32BGRA)]
		public void CreateBufferFromImageTest (CVPixelFormatType pft)
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			TestRuntime.AssertNotSimulator (); // metal api not supported in the simulator
			AssertSupported ();

			using var device = MTLDevice.SystemDefault;
			using var cache = new CVMetalBufferCache (device, (CVMetalBufferCacheAttributes) null);
			var dict = new CVPixelBufferAttributes () {
				MetalCompatibility = true,
			};
			using var image = new CVPixelBuffer (320, 320, pft, dict);
			using var buffer = cache.CreateBufferFromImage (image);
			Assert.That (buffer, Is.Not.Null, "Buffer");
		}
#endif // !MONOMAC

		[Test]
		public void FlushTest ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);
			AssertSupported ();

			using var device = MTLDevice.SystemDefault;
			using var cache = new CVMetalBufferCache (device, (CVMetalBufferCacheAttributes) null);
			cache.Flush ();
			cache.Flush (CVOptionFlags.None);
		}
	}
}
