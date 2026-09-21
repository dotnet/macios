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
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.EqualTo ((nint) 1), "InitialValue");
				Assert.That (obj.CurrentValue, Is.EqualTo ((nint) 1), "CurrentValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.True, "HasNeutralValue");
				Assert.That (neutralValue, Is.EqualTo ((nint) 3), "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.False, "HasCameraValue");
				Assert.That (cameraValue, Is.EqualTo ((nint) 0), "NeutralValue");
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
				Assert.That (obj.Name, Is.EqualTo ("name"), "Name");
				Assert.That (obj.Key, Is.EqualTo ("key"), "Key");
				Assert.That (obj.LongDescription, Is.Null, "LongDescription");
				Assert.That (obj.InitialValue, Is.EqualTo ((nint) 1), "InitialValue");
				Assert.That (obj.CurrentValue, Is.EqualTo ((nint) 1), "CurrentValue");
				Assert.That (obj.HasNeutralValue (out var neutralValue), Is.False, "HasNeutralValue");
				Assert.That (neutralValue, Is.EqualTo ((nint) 0), "NeutralValue");
				Assert.That (obj.HasCameraValue (out var cameraValue), Is.True, "HasCameraValue");
				Assert.That (cameraValue, Is.EqualTo ((nint) 3), "NeutralValue");
			});
		}
	}
}
#endif // HAS_MEDIAEXTENSION
