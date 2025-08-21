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

			Assert.IsNull (dict.LensAlgorithmKind, "LensAlgorithmKind");
			Assert.IsNull (dict.LensDomain, "LensDomain");
			Assert.IsNull (dict.LensIdentifier, "LensIdentifier");
			Assert.IsNull (dict.LensRole, "LensRole");
			Assert.IsNull (dict.LensDistortions, "LensDistortions");
			Assert.IsNull (dict.RadialAngleLimit, "RadialAngleLimit");
			Assert.IsNull (dict.LensFrameAdjustmentsPolynomialX, "LensFrameAdjustmentsPolynomialX");
			Assert.IsNull (dict.LensFrameAdjustmentsPolynomialY, "LensFrameAdjustmentsPolynomialY");
			Assert.IsNull (dict.IntrinsicMatrix, "IntrinsicMatrix");
			Assert.IsNull (dict.IntrinsicMatrixProjectionOffset, "IntrinsicMatrixProjectionOffset");
			Assert.IsNull (dict.IntrinsicMatrixReferenceDimensions, "IntrinsicMatrixReferenceDimensions");
			Assert.IsNull (dict.ExtrinsicOriginSource, "ExtrinsicOriginSource");
			Assert.IsNull (dict.ExtrinsicOrientationQuaternion, "ExtrinsicOrientationQuaternion");
			Assert.That (dict.ToString (), Is.EqualTo ("VideoToolbox.VTCompressionPropertyCameraCalibration"), "ToString");
			Assert.That (dict.Dictionary.ToString (), Is.EqualTo ("{\n}"), "ToString");
		});
	}
}
