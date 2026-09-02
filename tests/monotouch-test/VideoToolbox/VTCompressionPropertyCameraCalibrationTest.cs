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

			Assert.That (dict.LensAlgorithmKind, Is.Null, "LensAlgorithmKind");
			Assert.That (dict.LensDomain, Is.Null, "LensDomain");
			Assert.That (dict.LensIdentifier, Is.Null, "LensIdentifier");
			Assert.That (dict.LensRole, Is.Null, "LensRole");
			Assert.That (dict.LensDistortions, Is.Null, "LensDistortions");
			Assert.That (dict.RadialAngleLimit, Is.Null, "RadialAngleLimit");
			Assert.That (dict.LensFrameAdjustmentsPolynomialX, Is.Null, "LensFrameAdjustmentsPolynomialX");
			Assert.That (dict.LensFrameAdjustmentsPolynomialY, Is.Null, "LensFrameAdjustmentsPolynomialY");
			Assert.That (dict.IntrinsicMatrix, Is.Null, "IntrinsicMatrix");
			Assert.That (dict.IntrinsicMatrixProjectionOffset, Is.Null, "IntrinsicMatrixProjectionOffset");
			Assert.That (dict.IntrinsicMatrixReferenceDimensions, Is.Null, "IntrinsicMatrixReferenceDimensions");
			Assert.That (dict.ExtrinsicOriginSource, Is.Null, "ExtrinsicOriginSource");
			Assert.That (dict.ExtrinsicOrientationQuaternion, Is.Null, "ExtrinsicOrientationQuaternion");
			Assert.That (dict.ToString (), Is.EqualTo ("VideoToolbox.VTCompressionPropertyCameraCalibration"), "ToString");
			Assert.That (dict.Dictionary.ToString (), Is.EqualTo ("{\n}"), "ToString");
		});
	}
}
