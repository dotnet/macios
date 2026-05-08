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
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.False, "InitialValue");
				Assert.That (obj.CurrentValue, Is.False, "CurrentValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.True, "HasNeutralValue");
				Assert.That (neutralValue, Is.True, "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.False, "HasCameraValue");
				Assert.That (cameraValue, Is.False, "NeutralValue");
			});
		}

		[Test]
		public void CtorTest_Camera ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingBooleanParameter ("name", "key", "description", false, true, MERawProcessingBooleanParameterInitializationOption.CameraValue);
			Assert.Multiple (() => {
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.False, "InitialValue");
				Assert.That (obj.CurrentValue, Is.False, "CurrentValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.False, "HasNeutralValue");
				Assert.That (neutralValue, Is.False, "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.True, "HasCameraValue");
				Assert.That (cameraValue, Is.True, "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
