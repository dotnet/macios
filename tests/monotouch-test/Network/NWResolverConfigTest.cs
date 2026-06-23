using Network;

#nullable enable

namespace MonoTouchFixtures.Network {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWResolverConfigTest {

		NWEndpoint? endpoint;

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			endpoint = NWEndpoint.Create ("https://microsoft.com");
		}

		[TearDown]
		public void TearDown ()
		{
			endpoint?.Dispose ();
		}

		[Test]
		public void HttpConstructorTest ()
		{
			using var resolver = new NWResolverConfig (endpoint, NWResolverConfigEndpointType.Https);
			Assert.That (resolver, Is.Not.Null, "Not null https");
			Assert.That (resolver.Handle, Is.Not.EqualTo (IntPtr.Zero), "Zero Handle htttps");
		}

		[Test]
		public void AddServerAddressTest ()
		{
			using var secondEndpoint = NWEndpoint.Create ("https://github.com");
			using var resolver = new NWResolverConfig (endpoint, NWResolverConfigEndpointType.Https);

			Assert.DoesNotThrow (() => resolver.AddServerAddress (secondEndpoint));
		}
	}
}
