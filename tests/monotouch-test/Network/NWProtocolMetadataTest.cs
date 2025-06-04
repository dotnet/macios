using System;
using Foundation;
using Network;

using NUnit.Framework;

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
				Assert.True (m.IsIP, "IsIP");
				Assert.False (m.IsTcp, "IsTcp");
				Assert.False (m.IsUdp, "IsUdp");
				Assert.NotNull (m.ProtocolDefinition, "ProtocolDefinition");
				Assert.That (m.ServiceClass, Is.EqualTo (NWServiceClass.BestEffort), "ServiceClass");
			}
		}

		[Test]
		public void Udp ()
		{
			using (var m = new NWUdpMetadata ()) {
				Assert.False (m.IsIP, "IsIP");
				Assert.False (m.IsTcp, "IsTcp");
				Assert.True (m.IsUdp, "IsUdp");
				Assert.NotNull (m.ProtocolDefinition, "ProtocolDefinition");
			}
		}

		[Test]
		public void Quic ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using (var m = new NWIPMetadata ()) {
				Assert.That (m.EcnFlag, Is.EqualTo (NWIPEcnFlag.NonEct), "IPMetadataEcnFlag");
				Assert.That (m.ReceiveTime, Is.EqualTo (TimeSpan.Zero), "IPMetadataReceiveTime");
				Assert.True (m.IsIP, "IsIP");
				Assert.False (m.IsTcp, "IsTcp");
				Assert.False (m.IsUdp, "IsUdp");
				Assert.False (m.IsQuic, "IsQuic");
				Assert.NotNull (m.ProtocolDefinition, "ProtocolDefinition");
				Assert.That (m.ServiceClass, Is.EqualTo (NWServiceClass.BestEffort), "ServiceClass");
			}
		}
	}
}
