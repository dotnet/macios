#nullable enable

using System.Numerics;
using System.Runtime.InteropServices;

using Metal;
using MetalPerformanceShaders;
using Xamarin.Utils;

namespace MonoTouchFixtures.MetalPerformanceShaders {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MPSFColorConversionTest {

		[Test]
		public void AxisAlignedBoundingBoxLayout ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			Assert.That (Marshal.SizeOf<MPSFunctionsAxisAlignedBoundingBox> (), Is.EqualTo (32), "Size");
			Assert.That (Marshal.OffsetOf<MPSFunctionsAxisAlignedBoundingBox> ("max"), Is.EqualTo (IntPtr.Zero), "Max");
			Assert.That (Marshal.OffsetOf<MPSFunctionsAxisAlignedBoundingBox> ("min"), Is.EqualTo ((IntPtr) 16), "Min");
		}

		[Test]
		public void CreateAndGetEffectiveRange ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);
			TestRuntime.AssertSystemVersion (ApplePlatform.iOS, 27, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.TVOS, 27, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacOSX, 27, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (ApplePlatform.MacCatalyst, 27, 0, throwIfOtherPlatform: false);

			using var device = MTLDevice.SystemDefault;
			if (device is null || !MPSKernel.Supports (device))
				Assert.Inconclusive ("Metal Performance Shaders is not supported.");

			var range = new MPSFunctionsAxisAlignedBoundingBox {
				Max = new Vector4 (1, 2, 3, 4),
				Min = new Vector4 (-1, -2, -3, -4),
			};

			const string functionName = "MPSFColorConversionTest";
			var conversion = MPSFColorConversion.Create (device, null, functionName, range, MPSFColorConversionOptions.None, out var error);
			using (error)
			using (conversion) {
				Assert.That (error, Is.Null, "Error");
				Assert.That (conversion, Is.Not.Null, "Conversion");
				if (conversion is null)
					return;

				Assert.That (MPSFunction.SupportsSecureCoding, Is.True, "SupportsSecureCoding");
				Assert.That (conversion.Name, Is.EqualTo (functionName), "Name");
				Assert.That (conversion.Options, Is.EqualTo (MPSFColorConversionOptions.None), "Options");
				Assert.That (conversion.InputColorChannels, Is.EqualTo ((nuint) 0), "InputColorChannels");
				Assert.That (conversion.OutputColorChannels, Is.EqualTo ((nuint) 0), "OutputColorChannels");

				var effectiveRange = conversion.GetEffectiveRange (range);
				Assert.That (effectiveRange.Max, Is.EqualTo (range.Max), "Max");
				Assert.That (effectiveRange.Min, Is.EqualTo (range.Min), "Min");
			}
		}
	}
}
