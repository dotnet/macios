// Copyright 2017 Microsoft Inc. All rights reserved.

using Metal;
using MetalPerformanceShaders;

namespace MonoTouchFixtures.MetalPerformanceShaders {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MPSImageHistogramTest {
		IMTLDevice device;

		[OneTimeSetUp]
		public void Metal ()
		{
#if !MONOMAC && !__MACCATALYST__
			TestRuntime.AssertXcodeVersion (7, 0);

			if (Runtime.Arch == Arch.SIMULATOR)
				Assert.Inconclusive ("Metal Performance Shaders is not supported in the simulator");
#else
			TestRuntime.AssertXcodeVersion (9, 0);
			TestRuntime.AssertNotVirtualMachine ();
#endif

			device = MTLDevice.SystemDefault;
			// some older hardware won't have a default
			if (device is null || !MPSKernel.Supports (device))
				Assert.Inconclusive ("Metal is not supported");
		}

		[Test]
		public void Constructors ()
		{
			MPSImageHistogramInfo info = new MPSImageHistogramInfo ();
			info.NumberOfHistogramEntries = 256;
			using (var obj = new MPSImageHistogram (MTLDevice.SystemDefault, ref info)) {
				var rv = obj.HistogramInfo;
				Asserts.AreEqual (info, rv, "HistogramForAlpha");

				Assert.That (obj.ZeroHistogram, Is.True, "ZeroHistogram");
				if (TestRuntime.CheckXcodeVersion (8, 0)) {
					// HistogramSizeForSourceFormat was introduced in iOS 9, but no matter which MTLPixelFormat value I pass in,
					// the native histogramSizeForSourceFormat: function rudely aborts the entire process with an abrupt:
					// > /BuildRoot/Library/Caches/com.apple.xbs/Sources/MetalImage/MetalImage-39.3/MetalImage/Filters/MIHistogram.mm:103: failed assertion `[MPSImageHistogram histogramSizeForSourceFormat:] unsupported texture format: 114'
					// I made sure the MTLPixelFormat values I tested with were also added in iOS 9, so that's not the problem.
					// Conclusion: just avoid executing HistogramSizeForSourceFormat on anything below iOS 10.rm 
					Assert.That (obj.GetHistogramSize (MTLPixelFormat.RGBA16Sint), Is.EqualTo ((nuint) 3072), "HistogramSizeForSourceFormat");
				}
				var crs = obj.ClipRectSource;
				Assert.That (crs.Origin.X, Is.EqualTo ((nint) 0), "ClipRectSource.Origin.X");
				Assert.That (crs.Origin.Y, Is.EqualTo ((nint) 0), "ClipRectSource.Origin.Y");
				Assert.That (crs.Origin.Z, Is.EqualTo ((nint) 0), "ClipRectSource.Origin.Z");
				Assert.That (crs.Size.Depth, Is.EqualTo ((nint) (-1)), "ClipRectSource.Size.Depth");
				Assert.That (crs.Size.Height, Is.EqualTo ((nint) (-1)), "ClipRectSource.Size.Height");
				Assert.That (crs.Size.Width, Is.EqualTo ((nint) (-1)), "ClipRectSource.Size.Width");
			}
		}
	}
}
