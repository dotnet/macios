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
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.AreEqual ((nint) 3, obj.InitialValue, "InitialValue");
				ClassicAssert.AreEqual ((nint) 3, obj.CurrentValue, "CurrentValue");
				ClassicAssert.AreEqual ((nint) 5, obj.MaximumValue, "MaximumValue");
				ClassicAssert.AreEqual ((nint) 1, obj.MinimumValue, "MinimumValue");
				ClassicAssert.IsTrue (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.AreEqual ((nint) 2, neutralValue, "NeutralValue");
				ClassicAssert.IsFalse (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.AreEqual ((nint) 0, cameraValue, "NeutralValue");
			});
		}

		[Test]
		public void CtorTest_Camera ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			using var obj = new MERawProcessingIntegerParameter ("name", "key", "description", 3, 5, 1, 2, MERawProcessingIntegerParameterInitializationOption.CameraValue);
			Assert.Multiple (() => {
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.AreEqual ((nint) 3, obj.InitialValue, "InitialValue");
				ClassicAssert.AreEqual ((nint) 3, obj.CurrentValue, "CurrentValue");
				ClassicAssert.AreEqual ((nint) 5, obj.MaximumValue, "MaximumValue");
				ClassicAssert.AreEqual ((nint) 1, obj.MinimumValue, "MinimumValue");
				ClassicAssert.IsFalse (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.AreEqual ((nint) 0, neutralValue, "NeutralValue");
				ClassicAssert.IsTrue (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.AreEqual ((nint) 2, cameraValue, "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
