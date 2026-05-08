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
				ClassicAssert.NotNull (definition);
		}

		[Test]
		public void TcpDefinitionTest ()
		{
			using (var definition = NWProtocolDefinition.CreateTcpDefinition ())
				ClassicAssert.NotNull (definition);
		}

		[Test]
		public void TlsDefinitionTest ()
		{
			using (var definition = NWProtocolDefinition.CreateTlsDefinition ())
				ClassicAssert.NotNull (definition);
		}

		[Test]
		public void UdpDefinitionTest ()
		{
			using (var definition = NWProtocolDefinition.CreateUdpDefinition ())
				ClassicAssert.NotNull (definition);
		}

		[Test]
		public void WebSocketDefinitionTest ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			using (var definition = NWProtocolDefinition.CreateWebSocketDefinition ())
				ClassicAssert.NotNull (definition);
		}
	}
}
