//
// Unit tests for MPSImageNormalizedHistogram
//
// Authors:
//	Alex Soto <alexsoto@microsoft.com>
//
//
// Copyright 2019 Microsoft Corporation.
//

using Metal;
using MetalPerformanceShaders;

using Xamarin.Utils;

namespace MonoTouchFixtures.MetalPerformanceShaders {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MPSImageNormalizedHistogramTests {
		IMTLDevice device;

		[OneTimeSetUp]
		public void Metal ()
		{
			TestRuntime.AssertDevice ();
			TestRuntime.AssertXcodeVersion (10, 0);
			TestRuntime.AssertNotVirtualMachine ();

			device = MTLDevice.SystemDefault;
			// some older hardware won't have a default
			if (device is null || !MPSKernel.Supports (device))
				Assert.Inconclusive ("Metal is not supported.");
		}

		[Test]
		public void Constructors ()
		{
			var info = new MPSImageHistogramInfo { NumberOfHistogramEntries = 256 };
#if !__MACOS__
			var obj = new MPSImageNormalizedHistogram (MTLDevice.SystemDefault, ref info);
#else
			MPSImageNormalizedHistogram obj = null;
			try {
				obj = new MPSImageNormalizedHistogram (MTLDevice.SystemDefault, ref info);
			} catch (Exception ex) {
				Assert.Fail (ex.Message);
			}
#endif
			Assert.That (obj, Is.Not.Null, "MPSImageNormalizedHistogram obj");
			var rv = obj.HistogramInfo;
			Asserts.AreEqual (info, rv, "HistogramInfo");

			Assert.That (obj.ZeroHistogram, Is.True, "ZeroHistogram");
			Assert.That (obj.GetHistogramSize (MTLPixelFormat.RGBA16Sint), Is.EqualTo ((nuint) 3072), "HistogramSizeForSourceFormat");

			var crs = obj.ClipRectSource;
			Assert.That (crs.Origin.X, Is.EqualTo ((nint) 0), "ClipRectSource.Origin.X");
			Assert.That (crs.Origin.Y, Is.EqualTo ((nint) 0), "ClipRectSource.Origin.Y");
			Assert.That (crs.Origin.Z, Is.EqualTo ((nint) 0), "ClipRectSource.Origin.Z");
			Assert.That ((nuint) crs.Size.Depth, Is.EqualTo (nuint.MaxValue), "ClipRectSource.Size.Depth");
			Assert.That ((nuint) crs.Size.Height, Is.EqualTo (nuint.MaxValue), "ClipRectSource.Size.Height");
			Assert.That ((nuint) crs.Size.Width, Is.EqualTo (nuint.MaxValue), "ClipRectSource.Size.Width");
		}
	}
}
