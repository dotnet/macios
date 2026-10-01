#nullable enable

using AVFoundation;
using CoreMedia;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVMetadataCaptureTest {
		[TestCase (AVMetadataObjectType.None, 0)]
		[TestCase (AVMetadataObjectType.FocusTrackedObject, 1)]
		[TestCase (AVMetadataObjectType.CinematicVideoMetadata, 1)]
		[TestCase (AVMetadataObjectType.FocusTrackedObject | AVMetadataObjectType.CinematicVideoMetadata, 2)]
		[TestCase (AVMetadataObjectType.Face | AVMetadataObjectType.FocusTrackedObject | AVMetadataObjectType.CinematicVideoMetadata, 3)]
		public void ObjectTypeFlags (AVMetadataObjectType types, int count)
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			var constants = types.ToArray ();
			Assert.That (constants.Length, Is.EqualTo (count), "Count");
			Assert.That (AVMetadataObjectTypeExtensions.ToFlags (constants), Is.EqualTo (types), "Flags");

			foreach (var constant in constants) {
				var type = AVMetadataObjectTypeExtensions.GetValue (constant);
				Assert.That (type.GetConstant (), Is.EqualTo (constant), "Constant");
			}
		}

		[Test]
		public void CinematicVideoFormatDescription ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			for (var i = 0; i < 2; i++) {
				using var description = AVMetadataCinematicVideoMetadataObject.CinematicVideoMetadataFormatDescription;
				if (description is not null)
					Assert.That (description.MediaType, Is.EqualTo (CMMediaType.Metadata), "MediaType");
			}
		}
	}
}
