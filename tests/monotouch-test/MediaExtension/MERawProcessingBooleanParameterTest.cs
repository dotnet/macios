#if HAS_MEDIAEXTENSION
using MediaExtension;

namespace MonoTouchFixtures.MediaExtension {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MERawProcessingBooleanParameterTest {
		[Test]
		public void CtorTest_Neutral ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingBooleanParameter ("name", "key", "description", false, true, MERawProcessingBooleanParameterInitializationOption.NeutralValue);
			Assert.Multiple (() => {
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.IsFalse (obj.InitialValue, "InitialValue");
				ClassicAssert.IsFalse (obj.CurrentValue, "CurrentValue");
				ClassicAssert.IsTrue (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.IsTrue (neutralValue, "NeutralValue");
				ClassicAssert.IsFalse (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.IsFalse (cameraValue, "NeutralValue");
			});
		}

		[Test]
		public void CtorTest_Camera ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingBooleanParameter ("name", "key", "description", false, true, MERawProcessingBooleanParameterInitializationOption.CameraValue);
			Assert.Multiple (() => {
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.IsFalse (obj.InitialValue, "InitialValue");
				ClassicAssert.IsFalse (obj.CurrentValue, "CurrentValue");
				ClassicAssert.IsFalse (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.IsFalse (neutralValue, "NeutralValue");
				ClassicAssert.IsTrue (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.IsTrue (cameraValue, "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
