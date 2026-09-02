#if !__TVOS__
#nullable enable

using Metal;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLResourceStatePassSampleBufferAttachmentDescriptorArrayTest {
		MTLResourceStatePassSampleBufferAttachmentDescriptorArray array;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (12, TestRuntime.MinorXcode12APIMismatch);
			array = new MTLResourceStatePassSampleBufferAttachmentDescriptorArray ();
		}

		[TearDown]
		public void TearDown ()
		{
			array?.Dispose ();
			array = null;
		}

		[Test]
		public void IndexerTest ()
		{

			var obj = new MTLResourceStatePassSampleBufferAttachmentDescriptor ();
			MTLResourceStatePassSampleBufferAttachmentDescriptor dupe = null;
			Assert.DoesNotThrow (() => {
				array [0] = obj;
			});
			Assert.DoesNotThrow (() => {
				dupe = array [0];
			});
			Assert.That (dupe, Is.Not.Null, "Dupe");
			Assert.That (dupe.Handle, Is.Not.EqualTo (IntPtr.Zero), "Dupe");
		}
	}
}
#endif
