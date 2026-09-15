#nullable enable

using AVFoundation;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class AVCaptureAncillaryDataEncoderTest {
		[Test]
		public void EmptyUserData ()
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			var data = new AVCaptureAncillaryDataUserData ();
			using (data.Dictionary) {
				Assert.That (data.Rdd18InstanceUid, Is.Null, "InstanceUid");
				Assert.That (data.Rdd18UdamSetVersion, Is.Null, "UdamSetVersion");
				Assert.That (data.Rdd18UserItems, Is.Null, "UserItems");
			}
		}

		[TestCase (1)]
		[TestCase (65535)]
		public void UserData (int versionValue)
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			using var output = new AVCaptureBroadcastVideoOutput ();
			using var encoder = output.AncillaryDataEncoder;
			Assert.That (encoder.Enabled, Is.True, "InitiallyEnabled");
			encoder.Enabled = false;
			Assert.That (encoder.Enabled, Is.False, "Disabled");
			encoder.Enabled = true;
			Assert.That (encoder.Enabled, Is.True, "Enabled");

			using var uuid = new NSUuid ("00112233-4455-6677-8899-aabbccddeeff");
			encoder.SetUserInstanceUid (uuid, (ushort) versionValue);

			var data = encoder.CurrentUserDefinedAncillaryData;
			using (data.Dictionary) {
				var instanceUid = data.Rdd18InstanceUid;
				Assert.That (instanceUid, Is.EqualTo (uuid), "InstanceUid");
				Assert.That (data.Rdd18UdamSetVersion, Is.EqualTo ((ushort) versionValue), "UdamSetVersion");
			}

			using var dictionary = encoder.WeakCurrentUserDefinedAncillaryData;
			Assert.That (dictionary.Count, Is.GreaterThanOrEqualTo ((nuint) 2), "NativeDictionary");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void AncillaryData (bool useString)
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			using var output = new AVCaptureBroadcastVideoOutput ();
			using var encoder = output.AncillaryDataEncoder;
			using var uuid = new NSUuid ("00112233-4455-6677-8899-aabbccddeeff");
			encoder.SetUserInstanceUid (uuid, 1);
			using var payload = NSData.FromArray (new byte [] { 1, 2, 3, 4 });
			const ushort tag = 0xE100;
			var initialCapacity = encoder.UserDefinedAncillaryDataSizeRemaining;
			var initialData = encoder.CurrentUserDefinedAncillaryData;
			nuint initialItemCount;
			using (initialData.Dictionary) {
				using var items = initialData.Rdd18UserItems;
				initialItemCount = items?.Count ?? 0;
			}

			NSError? error;
			var added = useString
				? encoder.SetRdd18AncillaryData ("metadata", tag, out error)
				: encoder.SetRdd18AncillaryData (payload, tag, out error);
			using (error) {
				Assert.That (added, Is.True, error?.ToString ());
				Assert.That (error, Is.Null, "Error");
			}

			var data = encoder.CurrentUserDefinedAncillaryData;
			using (data.Dictionary) {
				using var items = data.Rdd18UserItems;
				Assert.That (items?.Count, Is.EqualTo (initialItemCount + 1), "UserItems");
			}
			var remainingCapacity = encoder.UserDefinedAncillaryDataSizeRemaining;
			Assert.That (remainingCapacity, Is.LessThan (initialCapacity), "CapacityUsed");

			encoder.RemoveRdd18AncillaryData (tag);
			var updatedData = encoder.CurrentUserDefinedAncillaryData;
			using (updatedData.Dictionary) {
				using var items = updatedData.Rdd18UserItems;
				Assert.That (items?.Count ?? 0, Is.EqualTo (initialItemCount), "RemovedUserItems");
			}
			Assert.That (encoder.UserDefinedAncillaryDataSizeRemaining, Is.GreaterThan (remainingCapacity), "CapacityRecovered");
		}

		[TestCase (false)]
		[TestCase (true)]
		public void ReservedTag (bool useString)
		{
			TestRuntime.AssertXcodeVersion (27, 0);

			using var output = new AVCaptureBroadcastVideoOutput ();
			using var encoder = output.AncillaryDataEncoder;
			using var uuid = new NSUuid ("00112233-4455-6677-8899-aabbccddeeff");
			encoder.SetUserInstanceUid (uuid, 1);
			using var payload = NSData.FromArray (new byte [] { 1, 2, 3, 4 });
			NSError? error;
			// The Xcode 27 RC1 runtimes reject 0xFFFF as a reserved tag for both payload types.
			var added = useString
				? encoder.SetRdd18AncillaryData ("metadata", ushort.MaxValue, out error)
				: encoder.SetRdd18AncillaryData (payload, ushort.MaxValue, out error);
			using (error) {
				Assert.That (added, Is.False, "Added");
				Assert.That (error, Is.Not.Null, "Error");
			}
		}
	}
}
