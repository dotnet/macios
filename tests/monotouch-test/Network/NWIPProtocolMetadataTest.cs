
using Network;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWIPProtocolMetadataTest {
		NWIPMetadata metadata;

		[OneTimeSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (10, 0);

		[SetUp]
		public void SetUp ()
		{
			metadata = new NWIPMetadata ();
		}

		[TearDown]
		public void TearDown ()
		{
			metadata.Dispose ();
		}

		[Test]
		public void TestEcnFlagProperty ()
		{
			Assert.That (metadata.EcnFlag, Is.EqualTo (NWIPEcnFlag.NonEct), "default value");
			metadata.EcnFlag = NWIPEcnFlag.Ect1;
			Assert.That (metadata.EcnFlag, Is.EqualTo (NWIPEcnFlag.Ect1), "new value");
		}

		[Test]
		public void TestServiceClassProperty ()
		{
			Assert.That (metadata.ServiceClass, Is.EqualTo (NWServiceClass.BestEffort), "default value");
			metadata.ServiceClass = NWServiceClass.InteractiveVideo;
			Assert.That (metadata.ServiceClass, Is.EqualTo (NWServiceClass.InteractiveVideo), "new value");
		}

		[Test]
		public void TestReceiveTimeProperty ()
		{
			Assert.That (metadata.ReceiveTime, Is.EqualTo (TimeSpan.Zero), "default value");
		}

		[Test]
		public void TestMetadataType ()
		{
			Assert.That (metadata.IsIP, Is.True, "IsIP");
			Assert.That (metadata.IsTcp, Is.False, "IsTcp");
			Assert.That (metadata.IsUdp, Is.False, "IsUdp");
		}
	}
}
