using Network;

namespace MonoTouchFixtures.Network {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWBrowserDescriptorTest {

		NWBrowserDescriptor descriptor;
		string type = "_ftp._tcp";
		string domain = "MonoTouchFixtures.Network";

		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (11, 0);

		[SetUp]
		public void SetUp ()
		{
			descriptor = NWBrowserDescriptor.CreateBonjourService (type, domain);
		}

		[TearDown]
		public void TearDown ()
		{
			descriptor.Dispose ();
		}

		[Test]
		public void TestIncludeTxtRecordProperty ()
		{
			Assert.That (descriptor.IncludeTxtRecord, Is.False, "Get default value.");
			descriptor.IncludeTxtRecord = true;
			Assert.That (descriptor.IncludeTxtRecord, Is.True, "Get new value.");
		}

		[Test]
		public void TestCreateNullDomain ()
		{
			using (var newDescriptor = NWBrowserDescriptor.CreateBonjourService (type)) {
				Assert.That (descriptor.BonjourType, Is.EqualTo (type), "service type");
				Assert.That (newDescriptor.BonjourDomain, Is.Null);
			}
		}

		[Test]
		public void TestBonjourTypeProperty () => Assert.That (descriptor.BonjourType, Is.EqualTo (type));

		[Test]
		public void TestBonjourDomainProperty () => Assert.That (descriptor.BonjourDomain, Is.EqualTo (domain));

		[Test]
		public void TestApplicationServiceConstructor ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			Assert.Throws<ArgumentNullException> (() => {
				using var appServiceDescriptor = NWBrowserDescriptor.CreateApplicationServiceName (null);
			}, "App service is null");

			var appName = "myService";
			using var appServiceDescriptor = NWBrowserDescriptor.CreateApplicationServiceName (appName);
			Assert.That (appServiceDescriptor.ApplicationServiceName, Is.EqualTo (appName));
		}
	}
}
