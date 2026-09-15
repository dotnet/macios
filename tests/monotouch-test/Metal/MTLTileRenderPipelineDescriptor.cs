#if __IOS__

using Metal;

using Xamarin.Utils;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLTileRenderPipelineDescriptorTests {
		MTLTileRenderPipelineDescriptor descriptor = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (9, 0);
			// The API here was introduced to Mac Catalyst later than for the other frameworks, so we have this additional check
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 14, 0, throwIfOtherPlatform: false);
			descriptor = new MTLTileRenderPipelineDescriptor ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (descriptor is not null)
				descriptor.Dispose ();
			descriptor = null;
		}

		[Test]
		public void ColorAttachmentsTest ()
		{
			var attachments = descriptor.ColorAttachments;
			Assert.That (attachments, Is.Not.Null);
		}

		[Test]
		public void GetSetLabelTest ()
		{
			descriptor.Label = "Foo";
			Assert.That (descriptor.Label, Is.EqualTo ("Foo"));
		}

		[Test]
		public void GetSetRasterSampleCount ()
		{
			descriptor.RasterSampleCount = 2;
			Assert.That (descriptor.RasterSampleCount, Is.EqualTo ((nuint) 2));
		}

		[Test]
		public void GetSetThreadgroupSizeMatchesTileSize ()
		{
			descriptor.ThreadgroupSizeMatchesTileSize = true;
			Assert.That (descriptor.ThreadgroupSizeMatchesTileSize, Is.EqualTo (true));

			descriptor.ThreadgroupSizeMatchesTileSize = false;
			Assert.That (descriptor.ThreadgroupSizeMatchesTileSize, Is.EqualTo (false));
		}

		[Test]
		public void GetTileBuffers ()
		{
			var buffers = descriptor.TileBuffers;
			Assert.That (buffers, Is.Not.Null);
		}

		[Test]
		public void GetSetMaxTotalThreadsPerThreadgroupTest ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
			descriptor.MaxTotalThreadsPerThreadgroup = 10;
			Assert.That (descriptor.MaxTotalThreadsPerThreadgroup, Is.EqualTo ((nuint) 10));
		}
	}
}

#endif // !__IOS__
