#if HAS_MEDIAEXTENSION
using MediaExtension;

namespace MonoTouchFixtures.MediaExtension {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MERawProcessingIntegerParameterTest {
		[Test]
		public void CtorTest_Neutral ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingIntegerParameter ("name", "key", "description", 3, 5, 1, 2, MERawProcessingIntegerParameterInitializationOption.NeutralValue);
			Assert.Multiple (() => {
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.EqualTo ((nint) 3), "InitialValue");
				Assert.That (obj.CurrentValue, Is.EqualTo ((nint) 3), "CurrentValue");
				Assert.That (obj.MaximumValue, Is.EqualTo ((nint) 5), "MaximumValue");
				Assert.That (obj.MinimumValue, Is.EqualTo ((nint) 1), "MinimumValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.True, "HasNeutralValue");
				Assert.That (neutralValue, Is.EqualTo ((nint) 2), "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.False, "HasCameraValue");
				Assert.That (cameraValue, Is.EqualTo ((nint) 0), "NeutralValue");
			});
		}

		[Test]
		public void CtorTest_Camera ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingIntegerParameter ("name", "key", "description", 3, 5, 1, 2, MERawProcessingIntegerParameterInitializationOption.CameraValue);
			Assert.Multiple (() => {
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.EqualTo ((nint) 3), "InitialValue");
				Assert.That (obj.CurrentValue, Is.EqualTo ((nint) 3), "CurrentValue");
				Assert.That (obj.MaximumValue, Is.EqualTo ((nint) 5), "MaximumValue");
				Assert.That (obj.MinimumValue, Is.EqualTo ((nint) 1), "MinimumValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.False, "HasNeutralValue");
				Assert.That (neutralValue, Is.EqualTo ((nint) 0), "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.True, "HasCameraValue");
				Assert.That (cameraValue, Is.EqualTo ((nint) 2), "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
