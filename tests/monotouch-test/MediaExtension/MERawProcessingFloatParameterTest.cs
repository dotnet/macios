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
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.EqualTo (1.2f), "InitialValue");
				Assert.That (obj.CurrentValue, Is.EqualTo (1.2f), "CurrentValue");
				Assert.That (obj.MaximumValue, Is.EqualTo (3.4f), "MaximumValue");
				Assert.That (obj.MinimumValue, Is.EqualTo (0.1f), "MinimumValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.True, "HasNeutralValue");
				Assert.That (neutralValue, Is.EqualTo (1.1f), "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.False, "HasCameraValue");
				Assert.That (cameraValue, Is.EqualTo (0f), "NeutralValue");
			});
		}

		[Test]
		public void CtorTest_Camera ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingFloatParameter ("name", "key", "description", 1.2f, 3.4f, 0.1f, 1.1f, MERawProcessingFloatParameterInitializationOption.CameraValue);
			Assert.Multiple (() => {
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.EqualTo (1.2f), "InitialValue");
				Assert.That (obj.CurrentValue, Is.EqualTo (1.2f), "CurrentValue");
				Assert.That (obj.MaximumValue, Is.EqualTo (3.4f), "MaximumValue");
				Assert.That (obj.MinimumValue, Is.EqualTo (0.1f), "MinimumValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.False, "HasNeutralValue");
				Assert.That (neutralValue, Is.EqualTo (0f), "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.True, "HasCameraValue");
				Assert.That (cameraValue, Is.EqualTo (1.1f), "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
