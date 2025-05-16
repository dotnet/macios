using System;

using Foundation;
using Metal;

using NUnit.Framework;

namespace MonoTouchFixtures.Metal {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MTLRasterizationRateLayerDescriptorTest {

		MTLRasterizationRateLayerDescriptor descriptor = null;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			descriptor = new MTLRasterizationRateLayerDescriptor (new MTLSize (10, 01, 0));
		}

		[TearDown]
		public void TearDown ()
		{
			if (descriptor is not null)
				descriptor.Dispose ();
			descriptor = null;
		}

		[Test]
		public void Horizontal ()
			=> Assert.DoesNotThrow (() => {
				var h = descriptor.Horizontal;
			});

		[Test]
		public void Vertical ()
			=> Assert.DoesNotThrow (() => {
				var v = descriptor.Vertical;
			});

		[Test]
		public void MaxSampleCount ()
			=> Assert.DoesNotThrow (() => {
				var m = descriptor.MaxSampleCount;
			});

		[Test]
		public void SampleCount ()
			=> Assert.DoesNotThrow (() => {
				var c = descriptor.SampleCount;
			});

		[Test]
		public void Create_1 ()
		{
			var horizontal = new float [] { 1, 2, 3 };
			var vertical = new float [] { 5, 6, 7, 8 };
			using var obj = MTLRasterizationRateLayerDescriptor.Create (new MTLSize (horizontal.Length, vertical.Length, 42), horizontal, vertical);
			Assert.AreEqual ((nint) horizontal.Length, obj.SampleCount.Width, "Width");
			Assert.AreEqual ((nint) vertical.Length, obj.SampleCount.Height, "Height");
			Assert.AreEqual ((nint) 0, obj.SampleCount.Depth, "Depth");
			Assert.That (obj.HorizontalSampleStorage, Is.EqualTo (horizontal), "HorizontalSampleStorage");
			Assert.That (obj.VerticalSampleStorage, Is.EqualTo (vertical), "VerticalSampleStorage");

			Assert.Throws<ArgumentNullException> (() => MTLRasterizationRateLayerDescriptor.Create (new MTLSize (horizontal.Length, vertical.Length, 0), null, vertical), "H-Null");
			Assert.Throws<ArgumentNullException> (() => MTLRasterizationRateLayerDescriptor.Create (new MTLSize (horizontal.Length, vertical.Length, 0), horizontal, null), "V-Null");
			Assert.Throws<ArgumentOutOfRangeException> (() => MTLRasterizationRateLayerDescriptor.Create (new MTLSize (horizontal.Length + 1, vertical.Length, 0), horizontal, vertical), "H-AOORE");
			Assert.Throws<ArgumentOutOfRangeException> (() => MTLRasterizationRateLayerDescriptor.Create (new MTLSize (horizontal.Length, vertical.Length - 1, 0), horizontal, vertical), "V-AOORE");
		}

		[Test]
		public void Create_2 ()
		{
			var horizontal = new float [] { 1, 2, 3 };
			var vertical = new float [] { 5, 6, 7, 8 };
			using var obj = MTLRasterizationRateLayerDescriptor.Create (horizontal, vertical);
			Assert.AreEqual ((nint) horizontal.Length, obj.SampleCount.Width, "Width");
			Assert.AreEqual ((nint) vertical.Length, obj.SampleCount.Height, "Height");
			Assert.AreEqual ((nint) 0, obj.SampleCount.Depth, "Depth");
			Assert.That (obj.HorizontalSampleStorage, Is.EqualTo (horizontal), "HorizontalSampleStorage");
			Assert.That (obj.VerticalSampleStorage, Is.EqualTo (vertical), "VerticalSampleStorage");

			Assert.Throws<ArgumentNullException> (() => MTLRasterizationRateLayerDescriptor.Create (null, vertical), "H-Null");
			Assert.Throws<ArgumentNullException> (() => MTLRasterizationRateLayerDescriptor.Create (horizontal, null), "V-Null");
		}

		[Test]
		public void Ctor ()
		{
			var horizontal = new float [3];
			var vertical = new float [4];
			using var obj = new MTLRasterizationRateLayerDescriptor (new MTLSize (horizontal.Length, vertical.Length, 0));
			Assert.AreEqual ((nint) horizontal.Length, obj.SampleCount.Width, "Width");
			Assert.AreEqual ((nint) vertical.Length, obj.SampleCount.Height, "Height");
			Assert.AreEqual ((nint) 0, obj.SampleCount.Depth, "Depth");
			Assert.That (obj.HorizontalSampleStorage, Is.EqualTo (horizontal), "HorizontalSampleStorage");
			Assert.That (obj.VerticalSampleStorage, Is.EqualTo (vertical), "VerticalSampleStorage");
		}
	}
}
