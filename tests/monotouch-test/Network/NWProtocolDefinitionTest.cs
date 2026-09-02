using Network;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWProtocolDefinitionTest {

		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (10, 0);



		[Test]
		public void IPDefinitionTest ()
		{
			using (var definition = NWProtocolDefinition.CreateIPDefinition ())
				Assert.That (definition, Is.Not.Null);
		}

		[Test]
		public void TcpDefinitionTest ()
		{
			using (var definition = NWProtocolDefinition.CreateTcpDefinition ())
				Assert.That (definition, Is.Not.Null);
		}

		[Test]
		public void TlsDefinitionTest ()
		{
			using (var definition = NWProtocolDefinition.CreateTlsDefinition ())
				Assert.That (definition, Is.Not.Null);
		}

		[Test]
		public void UdpDefinitionTest ()
		{
			using (var definition = NWProtocolDefinition.CreateUdpDefinition ())
				Assert.That (definition, Is.Not.Null);
		}

		[Test]
		public void WebSocketDefinitionTest ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			using (var definition = NWProtocolDefinition.CreateWebSocketDefinition ())
				Assert.That (definition, Is.Not.Null);
		}
	}
}
