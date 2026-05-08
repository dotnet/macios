#if HAS_MEDIAEXTENSION
using MediaExtension;

namespace MonoTouchFixtures.MediaExtension {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MERawProcessingListParameterTest {
		[Test]
		public void CtorTest_Neutral ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var array = new MERawProcessingListElementParameter []
			{
				new MERawProcessingListElementParameter ("name0", "desc0", 1),
				new MERawProcessingListElementParameter ("name1", "desc1", 3),
				new MERawProcessingListElementParameter ("name2", "desc2", 5),
			};
			using var obj = new MERawProcessingListParameter ("name", "key", "description", array, 1, 3, MERawProcessingListParameterInitializationOption.NeutralValue);
			Assert.Multiple (() => {
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.AreEqual ((nint) 1, obj.InitialValue, "InitialValue");
				ClassicAssert.AreEqual ((nint) 1, obj.CurrentValue, "CurrentValue");
				ClassicAssert.IsTrue (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.AreEqual ((nint) 3, neutralValue, "NeutralValue");
				ClassicAssert.IsFalse (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.AreEqual ((nint) 0, cameraValue, "NeutralValue");
			});
		}

		[Test]
		public void CtorTest_Camera ()
		{
			TestRuntime.AssertXcodeVersion (16, 0);

			var array = new MERawProcessingListElementParameter []
			{
				new MERawProcessingListElementParameter ("name0", "desc0", 1),
				new MERawProcessingListElementParameter ("name1", "desc1", 3),
				new MERawProcessingListElementParameter ("name2", "desc2", 5),
			};
			using var obj = new MERawProcessingListParameter ("name", "key", "description", array, 1, 3, MERawProcessingListParameterInitializationOption.CameraValue);
			Assert.Multiple (() => {
				ClassicAssert.AreEqual ("name", obj.Name, "Name");
				ClassicAssert.AreEqual ("key", obj.Key, "Key");
				ClassicAssert.IsNull (obj.LongDescription, "LongDescription");
				ClassicAssert.AreEqual ((nint) 1, obj.InitialValue, "InitialValue");
				ClassicAssert.AreEqual ((nint) 1, obj.CurrentValue, "CurrentValue");
				ClassicAssert.IsFalse (obj.HasNeutralValue (out var neutralValue), "HasNeutralValue");
				ClassicAssert.AreEqual ((nint) 0, neutralValue, "NeutralValue");
				ClassicAssert.IsTrue (obj.HasCameraValue (out var cameraValue), "HasCameraValue");
				ClassicAssert.AreEqual ((nint) 3, cameraValue, "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
