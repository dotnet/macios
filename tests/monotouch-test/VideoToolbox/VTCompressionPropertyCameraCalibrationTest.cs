// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;

using Foundation;
using CoreGraphics;
using ObjCRuntime;
using VideoToolbox;

using NUnit.Framework;

namespace MonoTouchFixtures.VideoToolbox;

[TestFixture]
[Preserve (AllMembers = true)]
public class VTCompressionPropertyCameraCalibrationTest {
	[Test]
	public void DefaultValues ()
	{
		TestRuntime.AssertXcodeVersion (26, 0);

		Assert.Multiple (() => {
			var dict = new VTCompressionPropertyCameraCalibration ();

			ClassicAssert.IsNull (dict.LensAlgorithmKind, "LensAlgorithmKind");
			ClassicAssert.IsNull (dict.LensDomain, "LensDomain");
			ClassicAssert.IsNull (dict.LensIdentifier, "LensIdentifier");
			ClassicAssert.IsNull (dict.LensRole, "LensRole");
			ClassicAssert.IsNull (dict.LensDistortions, "LensDistortions");
			ClassicAssert.IsNull (dict.RadialAngleLimit, "RadialAngleLimit");
			ClassicAssert.IsNull (dict.LensFrameAdjustmentsPolynomialX, "LensFrameAdjustmentsPolynomialX");
			ClassicAssert.IsNull (dict.LensFrameAdjustmentsPolynomialY, "LensFrameAdjustmentsPolynomialY");
			ClassicAssert.IsNull (dict.IntrinsicMatrix, "IntrinsicMatrix");
			ClassicAssert.IsNull (dict.IntrinsicMatrixProjectionOffset, "IntrinsicMatrixProjectionOffset");
			ClassicAssert.IsNull (dict.IntrinsicMatrixReferenceDimensions, "IntrinsicMatrixReferenceDimensions");
			ClassicAssert.IsNull (dict.ExtrinsicOriginSource, "ExtrinsicOriginSource");
			ClassicAssert.IsNull (dict.ExtrinsicOrientationQuaternion, "ExtrinsicOrientationQuaternion");
			Assert.That (dict.ToString (), Is.EqualTo ("VideoToolbox.VTCompressionPropertyCameraCalibration"), "ToString");
			Assert.That (dict.Dictionary.ToString (), Is.EqualTo ("{\n}"), "ToString");
		});
	}
}
