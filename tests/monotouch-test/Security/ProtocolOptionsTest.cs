
using Network;
using Security;

namespace MonoTouchFixtures.Security {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ProtocolOptionsTest {

		[Test]
		public void Defaults ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			Assert.That (SecProtocolOptions.DefaultMaxDtlsProtocolVersion, Is.EqualTo (TlsProtocolVersion.Dtls12), "MaxDtls");
			if (TestRuntime.CheckXcodeVersion (26, 0)) {
				Assert.That (SecProtocolOptions.DefaultMinDtlsProtocolVersion, Is.EqualTo (TlsProtocolVersion.Dtls12), "MinDtls");
			} else {
				Assert.That (SecProtocolOptions.DefaultMinDtlsProtocolVersion, Is.EqualTo (TlsProtocolVersion.Dtls10), "MinDtls");
			}
			Assert.That (SecProtocolOptions.DefaultMaxTlsProtocolVersion, Is.EqualTo (TlsProtocolVersion.Tls13), "MaxTls");
			if (TestRuntime.CheckXcodeVersion (26, 0)) {
				Assert.That (SecProtocolOptions.DefaultMinTlsProtocolVersion, Is.EqualTo (TlsProtocolVersion.Tls12), "MinTls");
			} else {
				Assert.That (SecProtocolOptions.DefaultMinTlsProtocolVersion, Is.EqualTo (TlsProtocolVersion.Tls10), "MinTls");
			}
		}

		[Test]
		public void Equals ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			using (var npo = new NWProtocolTlsOptions ()) {
				var options = npo.ProtocolOptions;

				Assert.That (SecProtocolOptions.Equals (null, null), Is.True, "1");
				Assert.That (SecProtocolOptions.Equals (options, options), Is.True, "2");
				Assert.That (SecProtocolOptions.Equals (null, options), Is.False, "3");
				Assert.That (SecProtocolOptions.Equals (options, null), Is.False, "4");

				Assert.That (options.Equals (options), Is.True, "5");
				Assert.That (options.Equals (null), Is.False, "6");
			}
		}

		[Test]
		public void NewTlsOptions ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			using (var npo = new NWProtocolTlsOptions ()) {
				var options = npo.ProtocolOptions;
				options.SetTlsMaxVersion (TlsProtocolVersion.Tls12);
				options.SetTlsMinVersion (TlsProtocolVersion.Tls10);
				options.AddTlsCipherSuite (TlsCipherSuite.Aes128GcmSha256);
				options.AddTlsCipherSuiteGroup (TlsCipherSuiteGroup.Legacy);
				using (var dd = DispatchData.FromByteBuffer (new byte [1])) {
					options.SetTlsPreSharedKeyIdentityHint (dd);
				}
			}
		}
	}
}
