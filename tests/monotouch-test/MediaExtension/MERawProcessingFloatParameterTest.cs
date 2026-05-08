#if HAS_MEDIAEXTENSION
using MediaExtension;

namespace MonoTouchFixtures.MediaExtension {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MERawProcessingFloatParameterTest {
		[Test]
		public void CtorTest_Neutral ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingFloatParameter ("name", "key", "description", 1.2f, 3.4f, 0.1f, 1.1f, MERawProcessingFloatParameterInitializationOption.NeutralValue);
			Assert.Multiple (() => {
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.AreEqual (1.2f, obj.InitialValue, "InitialValue");
				ClassicAssert.AreEqual (1.2f, obj.CurrentValue, "CurrentValue");
				ClassicAssert.AreEqual (3.4f, obj.MaximumValue, "MaximumValue");
				ClassicAssert.AreEqual (0.1f, obj.MinimumValue, "MinimumValue");
				ClassicAssert.IsTrue (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.AreEqual (1.1f, neutralValue, "NeutralValue");
				ClassicAssert.IsFalse (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.AreEqual (0f, cameraValue, "NeutralValue");
			});
		}

		[Test]
		public void CtorTest_Camera ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingFloatParameter ("name", "key", "description", 1.2f, 3.4f, 0.1f, 1.1f, MERawProcessingFloatParameterInitializationOption.CameraValue);
			Assert.Multiple (() => {
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.AreEqual (1.2f, obj.InitialValue, "InitialValue");
				ClassicAssert.AreEqual (1.2f, obj.CurrentValue, "CurrentValue");
				ClassicAssert.AreEqual (3.4f, obj.MaximumValue, "MaximumValue");
				ClassicAssert.AreEqual (0.1f, obj.MinimumValue, "MinimumValue");
				ClassicAssert.IsFalse (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.AreEqual (0f, neutralValue, "NeutralValue");
				ClassicAssert.IsTrue (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.AreEqual (1.1f, cameraValue, "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
