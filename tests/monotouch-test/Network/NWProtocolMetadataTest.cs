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
				Assert.That (m.IsIP, Is.True, "IsIP");
				Assert.That (m.IsTcp, Is.False, "IsTcp");
				Assert.That (m.IsUdp, Is.False, "IsUdp");
				Assert.That (m.ProtocolDefinition, Is.Not.Null, "ProtocolDefinition");
				Assert.That (m.ServiceClass, Is.EqualTo (NWServiceClass.BestEffort), "ServiceClass");
			}
		}

		[Test]
		public void Udp ()
		{
			using (var m = new NWUdpMetadata ()) {
				Assert.That (m.IsIP, Is.False, "IsIP");
				Assert.That (m.IsTcp, Is.False, "IsTcp");
				Assert.That (m.IsUdp, Is.True, "IsUdp");
				Assert.That (m.ProtocolDefinition, Is.Not.Null, "ProtocolDefinition");
			}
		}

		[Test]
		public void Quic ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using (var m = new NWIPMetadata ()) {
				Assert.That (m.EcnFlag, Is.EqualTo (NWIPEcnFlag.NonEct), "IPMetadataEcnFlag");
				Assert.That (m.ReceiveTime, Is.EqualTo (TimeSpan.Zero), "IPMetadataReceiveTime");
				Assert.That (m.IsIP, Is.True, "IsIP");
				Assert.That (m.IsTcp, Is.False, "IsTcp");
				Assert.That (m.IsUdp, Is.False, "IsUdp");
				Assert.That (m.IsQuic, Is.False, "IsQuic");
				Assert.That (m.ProtocolDefinition, Is.Not.Null, "ProtocolDefinition");
				Assert.That (m.ServiceClass, Is.EqualTo (NWServiceClass.BestEffort), "ServiceClass");
			}
		}
	}
}
