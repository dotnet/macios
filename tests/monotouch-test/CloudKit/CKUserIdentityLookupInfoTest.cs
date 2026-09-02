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
			Assert.That (info, Is.Not.Null);
			Assert.That (IntPtr.Zero, Is.Not.EqualTo (info.Handle));
		}

		[Test]
		public void TestFromPhoneNumber ()
		{
			var info = CKUserIdentityLookupInfo.FromPhoneNumber ("91899899");
			Assert.That (info, Is.Not.Null);
			Assert.That (IntPtr.Zero, Is.Not.EqualTo (info.Handle));
		}

		[Test]
		public void TestFromRecordID ()
		{
			var record = new CKRecordID ("recordName");
			var info = new CKUserIdentityLookupInfo (record);
			Assert.That (info, Is.Not.Null);
			Assert.That (IntPtr.Zero, Is.Not.EqualTo (info.Handle));
		}

		[Test]
		public void TestGetLookupInfosWithEmails ()
		{
			var emails = new string [] { "example@test.com" };
			var result = CKUserIdentityLookupInfo.GetLookupInfosWithEmails (emails);
			Assert.That (result.Length, Is.EqualTo (1));
		}

		[Test]
		public void TestGetLookupInfosWithPhoneNumbers ()
		{
			var numbers = new string [] { "9111223" };
			var result = CKUserIdentityLookupInfo.GetLookupInfosWithPhoneNumbers (numbers);
			Assert.That (result.Length, Is.EqualTo (1));
		}

		[Test]
		public void TestGetLookupInfosWithRecordIds ()
		{
			var record = new CKRecordID ("recordName");
			var records = new CKRecordID [] { record };
			var result = CKUserIdentityLookupInfo.GetLookupInfos (records);
			Assert.That (result.Length, Is.EqualTo (1));
		}
	}
}
