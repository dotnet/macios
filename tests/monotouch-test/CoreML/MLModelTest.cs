// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

//
// Unit tests for MLModel
//

using CoreML;

namespace MonoTouchFixtures.CoreML {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MLModelTest {

		[Test]
		public void AllComputeDevices ()
		{
			TestRuntime.AssertXcodeVersion (15, 0);

			var devices = MLModel.AllComputeDevices;
			Assert.IsNotNull (devices, "AllComputeDevices");
			Assert.That (devices.Length, Is.GreaterThanOrEqualTo (1), "AllComputeDevices/length");
		}
	}
}
