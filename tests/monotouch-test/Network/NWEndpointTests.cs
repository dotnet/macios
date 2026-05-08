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
		public void TypeTest () => ClassicAssert.AreEqual (NWEndpointType.Url, endpoint.Type);

		[Test]
		public void HostNameTest () => ClassicAssert.AreEqual ("github.com", endpoint.Hostname);

		[Test]
		public void PortTest () => ClassicAssert.AreEqual ("443", endpoint.Port);

		[Test]
		public void BonjourServiceNameTest () => ClassicAssert.Null (endpoint.BonjourServiceName);

		[Test]
		public void BonjourServiceTypeTest () => ClassicAssert.Null (endpoint.BonjourServiceType);

		[Test]
		public void BonjourServiceDomainTest () => ClassicAssert.Null (endpoint.BonjourServiceDomain);

		[Test]
		public void UrlTest () => ClassicAssert.AreEqual ("https://github.com", endpoint.Url);

		[Test]
		public void SignatureTest ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			var signature = endpoint.Signature;
			ClassicAssert.AreEqual (0, signature.Length);
		}

		[Test]
		public void TxtRecordTest ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			ClassicAssert.Null (endpoint.TxtRecord);
		}
	}
}
