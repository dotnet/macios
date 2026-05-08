using Network;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWProtocolMetadataTest {

		[SetUp]
		public void SetUp ()
		{
			TestRuntime.AssertXcodeVersion (10, 0);
		}

		[Test]
		public void IP ()
		{
			using (var m = new NWIPMetadata ()) {
				Assert.That (m.EcnFlag, Is.EqualTo (NWIPEcnFlag.NonEct), "IPMetadataEcnFlag");
				Assert.That (m.ReceiveTime, Is.EqualTo (TimeSpan.Zero), "IPMetadataReceiveTime");
				ClassicAssert.True (m.IsIP, "IsIP");
				ClassicAssert.False (m.IsTcp, "IsTcp");
				ClassicAssert.False (m.IsUdp, "IsUdp");
				ClassicAssert.NotNull (m.ProtocolDefinition, "ProtocolDefinition");
				Assert.That (m.ServiceClass, Is.EqualTo (NWServiceClass.BestEffort), "ServiceClass");
			}
		}

		[Test]
		public void Udp ()
		{
			using (var m = new NWUdpMetadata ()) {
				ClassicAssert.False (m.IsIP, "IsIP");
				ClassicAssert.False (m.IsTcp, "IsTcp");
				ClassicAssert.True (m.IsUdp, "IsUdp");
				ClassicAssert.NotNull (m.ProtocolDefinition, "ProtocolDefinition");
			}
		}

		[Test]
		public void Quic ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using (var m = new NWIPMetadata ()) {
				Assert.That (m.EcnFlag, Is.EqualTo (NWIPEcnFlag.NonEct), "IPMetadataEcnFlag");
				Assert.That (m.ReceiveTime, Is.EqualTo (TimeSpan.Zero), "IPMetadataReceiveTime");
				ClassicAssert.True (m.IsIP, "IsIP");
				ClassicAssert.False (m.IsTcp, "IsTcp");
				ClassicAssert.False (m.IsUdp, "IsUdp");
				ClassicAssert.False (m.IsQuic, "IsQuic");
				ClassicAssert.NotNull (m.ProtocolDefinition, "ProtocolDefinition");
				Assert.That (m.ServiceClass, Is.EqualTo (NWServiceClass.BestEffort), "ServiceClass");
			}
		}
	}
}
