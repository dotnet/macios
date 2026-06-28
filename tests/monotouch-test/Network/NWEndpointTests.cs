using Network;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWEndpointTests {

		NWEndpoint endpoint;

		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (11, 0);

		[SetUp]
		public void SetUp ()
		{
			endpoint = NWEndpoint.Create ("https://github.com");
		}

		[TearDown]
		public void TearDown ()
		{
			endpoint.Dispose ();
		}

		[Test]
		public void TypeTest () => Assert.That (endpoint.Type, Is.EqualTo (NWEndpointType.Url));

		[Test]
		public void HostNameTest () => Assert.That (endpoint.Hostname, Is.EqualTo ("github.com"));

		[Test]
		public void PortTest () => Assert.That (endpoint.Port, Is.EqualTo ("443"));

		[Test]
		public void BonjourServiceNameTest () => Assert.That (endpoint.BonjourServiceName, Is.Null);

		[Test]
		public void BonjourServiceTypeTest () => Assert.That (endpoint.BonjourServiceType, Is.Null);

		[Test]
		public void BonjourServiceDomainTest () => Assert.That (endpoint.BonjourServiceDomain, Is.Null);

		[Test]
		public void UrlTest () => Assert.That (endpoint.Url, Is.EqualTo ("https://github.com"));

		[Test]
		public void SignatureTest ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			var signature = endpoint.Signature;
			Assert.That (signature.Length, Is.EqualTo (0));
		}

		[Test]
		public void TxtRecordTest ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			Assert.That (endpoint.TxtRecord, Is.Null);
		}
	}
}
