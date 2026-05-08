using CloudKit;

namespace MonoTouchFixtures.CloudKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CKUserIdentityLookupInfoTest {
		[SetUp]
		public void MinimumSdkCheck ()
		{
			TestRuntime.AssertXcodeVersion (8, 0);
		}

		[Test]
		public void TestFromEmail ()
		{
			var info = CKUserIdentityLookupInfo.FromEmail ("example@test.com");
			ClassicAssert.NotNull (info);
			ClassicAssert.AreNotEqual (info.Handle, IntPtr.Zero);
		}

		[Test]
		public void TestFromPhoneNumber ()
		{
			var info = CKUserIdentityLookupInfo.FromPhoneNumber ("91899899");
			ClassicAssert.NotNull (info);
			ClassicAssert.AreNotEqual (info.Handle, IntPtr.Zero);
		}

		[Test]
		public void TestFromRecordID ()
		{
			var record = new CKRecordID ("recordName");
			var info = new CKUserIdentityLookupInfo (record);
			ClassicAssert.NotNull (info);
			ClassicAssert.AreNotEqual (info.Handle, IntPtr.Zero);
		}

		[Test]
		public void TestGetLookupInfosWithEmails ()
		{
			var emails = new string [] { "example@test.com" };
			var result = CKUserIdentityLookupInfo.GetLookupInfosWithEmails (emails);
			ClassicAssert.AreEqual (1, result.Length);
		}

		[Test]
		public void TestGetLookupInfosWithPhoneNumbers ()
		{
			var numbers = new string [] { "9111223" };
			var result = CKUserIdentityLookupInfo.GetLookupInfosWithPhoneNumbers (numbers);
			ClassicAssert.AreEqual (1, result.Length);
		}

		[Test]
		public void TestGetLookupInfosWithRecordIds ()
		{
			var record = new CKRecordID ("recordName");
			var records = new CKRecordID [] { record };
			var result = CKUserIdentityLookupInfo.GetLookupInfos (records);
			ClassicAssert.AreEqual (1, result.Length);
		}
	}
}
