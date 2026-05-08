using System.Threading;

using Network;

using MonoTests.System.Net.Http;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWParametersTest {

		AutoResetEvent secureEvent;  // used to let us know the handler was indeed called.
		AutoResetEvent configureEvent;  // used to let us know the handler was indeed called.
		bool secureConnectionWasSet = false;
		bool protocolConfigured = false;
		List<NWInterface> interfaces = new List<NWInterface> ();
		ConnectionManager manager;
		NWConnection connection;

		[OneTimeSetUp]
		public void Init ()
		{
			// we want to use a single connection, since it is expensive
			interfaces = new List<NWInterface> ();

			manager = new ConnectionManager ();
			connection = manager.CreateConnection ();
			using (var path = connection.CurrentPath) {
				path.EnumerateInterfaces (EnumerateInterfacesHandler);
			}
		}

		[OneTimeTearDown]
		public void Dispose ()
		{
			connection?.Dispose ();
			if (interfaces is not null) {
				foreach (var i in interfaces)
					i.Dispose ();
			}
		}

		[SetUp]
		public void SetUp ()
		{
			secureEvent = new AutoResetEvent (false);
			configureEvent = new AutoResetEvent (false);
			secureConnectionWasSet = false;
			protocolConfigured = false;
		}

		[TearDown]
		public void TearDown ()
		{
			secureEvent = null;
			secureConnectionWasSet = false;
			protocolConfigured = false;
			manager?.Dispose ();
		}

		void EnumerateInterfacesHandler (NWInterface nwInterface)
		{
			interfaces.Add (nwInterface);
		}

		Action<NWProtocolOptions> CreateTlsHandler ()
		{
			return (NWProtocolOptions options) => {
				secureConnectionWasSet = true;
				secureEvent.Set ();
			};
		}

		Action<NWProtocolOptions> CreateConfigureProtocolHandler ()
		{
			return (NWProtocolOptions options) => {
				protocolConfigured = true;
				configureEvent.Set ();
			};
		}

		[Test]
		public void CreateSecureUpdTest ()
		{
			var setUpTls = CreateTlsHandler ();
			var setUpProtocol = CreateConfigureProtocolHandler ();

			using (var parameters = NWParameters.CreateSecureUdp (configureTls: setUpTls, configureUdp: setUpProtocol))
			using (var endpoint = NWEndpoint.Create (NetworkResources.MicrosoftUri.Host, "80")) {
				secureEvent.WaitOne ();
				configureEvent.WaitOne ();
				ClassicAssert.True (secureConnectionWasSet, "Configure TLS handler was not called.");
				ClassicAssert.True (protocolConfigured, "Protocol configure handler was not called.");
			}
		}

		[Test]
		public void CreateSecureUpdTestDoNotSetUpProtocol ()
		{
			var setUpTls = CreateTlsHandler ();

			using (var parameters = NWParameters.CreateSecureUdp (configureTls: setUpTls))
			using (var endpoint = NWEndpoint.Create (NetworkResources.MicrosoftUri.Host, "80")) {
				secureEvent.WaitOne ();
				ClassicAssert.True (secureConnectionWasSet, "Configure TLS handler was not called.");
				ClassicAssert.False (protocolConfigured, "Protocol configure handler was called.");
			}
		}

		[Test]
		public void CreateSecureUpdTestDoNotSetUpTls ()
		{
			var setUpProtocol = CreateConfigureProtocolHandler ();

			using (var parameters = NWParameters.CreateSecureUdp (configureTls: null, configureUdp: setUpProtocol))
			using (var endpoint = NWEndpoint.Create (NetworkResources.MicrosoftUri.Host, "80")) {
				configureEvent.WaitOne ();
				ClassicAssert.False (secureConnectionWasSet, "Configure TLS handler was not called.");
				ClassicAssert.True (protocolConfigured, "Protocol configure handler was not called.");
			}
		}

		[Test]
		public void CreateSecureTcpTest ()
		{
			var setUpTls = CreateTlsHandler ();
			var setUpProtocol = CreateConfigureProtocolHandler ();

			using (var parameters = NWParameters.CreateSecureTcp (configureTls: setUpTls, configureTcp: setUpProtocol))
			using (var endpoint = NWEndpoint.Create (NetworkResources.MicrosoftUri.Host, "80")) {
				secureEvent.WaitOne ();
				configureEvent.WaitOne ();
				ClassicAssert.True (secureConnectionWasSet, "Configure TLS handler was not called.");
				ClassicAssert.True (protocolConfigured, "Protocol configure handler was not called.");
			}
		}

		[Test]
		public void CreateSecureTcpTestDoNotSetUpProtocol ()
		{
			var setUpTls = CreateTlsHandler ();
			var setUpProtocol = CreateConfigureProtocolHandler ();

			using (var parameters = NWParameters.CreateSecureTcp (configureTls: setUpTls))
			using (var endpoint = NWEndpoint.Create (NetworkResources.MicrosoftUri.Host, "80")) {
				secureEvent.WaitOne ();
				ClassicAssert.True (secureConnectionWasSet, "Configure TLS handler was not called.");
				ClassicAssert.False (protocolConfigured, "Protocol configure handler was called.");
			}
		}

		[Test]
		public void CreateSecureTcpTestDoNotSetUpTls ()
		{
			var setUpProtocol = CreateConfigureProtocolHandler ();

			using (var parameters = NWParameters.CreateSecureTcp (configureTls: null, configureTcp: setUpProtocol))
			using (var endpoint = NWEndpoint.Create (NetworkResources.MicrosoftUri.Host, "80")) {
				configureEvent.WaitOne ();
				ClassicAssert.False (secureConnectionWasSet, "Configure TLS handler was called.");
				ClassicAssert.True (protocolConfigured, "Protocol configure handler was not called.");
			}
		}

#if MONOMAC
		[Test]
		public void CreateCustomIP ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			byte ipVersion = 10;
			var setUpProtocol = CreateConfigureProtocolHandler ();
			using (var parameters = NWParameters.CreateCustomIP (ipVersion, setUpProtocol))
			using (var endpoint = NWEndpoint.Create ("wwww.google.com", "80")) {
				configureEvent.WaitOne ();
				ClassicAssert.True (protocolConfigured, "Protocol configure handler was not called.");
			}
		}
#endif

		[Test]
		public void MultiPathServicePropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.MultipathService;
				ClassicAssert.AreEqual (defaultValue, NWMultiPathService.Disabled, "Default value changed.");
				var newValue = NWMultiPathService.Aggregate;
				parameters.MultipathService = newValue;
				ClassicAssert.AreEqual (newValue, parameters.MultipathService, "New value was not stored.");
			}
		}

		[Test]
		public void ProtocolStackPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var stack = parameters.ProtocolStack;
				ClassicAssert.AreNotEqual (IntPtr.Zero, stack.Handle);
			}
		}

		[Test]
		public void LocalOnlyPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.LocalOnly;
				ClassicAssert.False (defaultValue, "Default value changed.");
				parameters.LocalOnly = true;
				ClassicAssert.True (parameters.LocalOnly, "New value was not stored.");
			}
		}

		[Test]
		public void PreferNoProxyPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.PreferNoProxy;
				ClassicAssert.False (defaultValue, "Default value changed.");
				parameters.PreferNoProxy = true;
				ClassicAssert.True (parameters.PreferNoProxy, "New value was not stored.");
			}
		}

		[Test]
		public void ExpiredDnsBehaviorPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.ExpiredDnsBehavior;
				ClassicAssert.AreEqual (NWParametersExpiredDnsBehavior.Default, defaultValue, "Default value changed.");
				parameters.ExpiredDnsBehavior = NWParametersExpiredDnsBehavior.Allow;
				ClassicAssert.AreEqual (NWParametersExpiredDnsBehavior.Allow, parameters.ExpiredDnsBehavior, "New value was not stored.");
			}
		}

		[Test]
		public void RequiredInterfacePropertyTest ()
		{

			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.RequiredInterface;
				ClassicAssert.IsNull (defaultValue, "Default value changed.");
				// try to set a null value, we should have no issues
				parameters.RequiredInterface = null;
				ClassicAssert.IsNull (parameters.RequiredInterface, "Value should still be null.");
				parameters.RequiredInterface = interfaces [0];
				ClassicAssert.AreNotEqual (IntPtr.Zero, parameters.RequiredInterface.Handle, "New value was not set.");
			}
		}

		[Test]
		public void ProhibitInterfaceTest ()
		{
			using (var parameters = new NWParameters ()) {
				Assert.Throws<ArgumentNullException> (() => parameters.ProhibitInterface (null), "");
				ClassicAssert.AreNotEqual (0, interfaces.Count, "No network interfaces found.");
				parameters.ProhibitInterface (interfaces [0]);
			}
		}

		[Test]
		public void RequiredInterfaceTypePropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.RequiredInterfaceType;
				ClassicAssert.AreEqual (NWInterfaceType.Other, defaultValue, "Default value changed.");
				parameters.RequiredInterfaceType = NWInterfaceType.Wifi;
				ClassicAssert.AreEqual (NWInterfaceType.Wifi, parameters.RequiredInterfaceType, "BNe value was not stored.");
			}
		}

		[Test]
		public void ProhibitInterfaceTypeTest ()
		{
			using (var parameters = new NWParameters ()) {
				var types = new List<NWInterfaceType> ();
				parameters.ProhibitInterfaceType (NWInterfaceType.Wifi);
				parameters.IterateProhibitedInterfaces ((type) => { types.Add (type); return true; });
				ClassicAssert.True (types.Contains (NWInterfaceType.Wifi), "Type was not prohibited.");
			}
		}

		[Test]
		public void ReuseLocalAddressPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.ReuseLocalAddress;
				ClassicAssert.False (defaultValue, "Default value changed.");
				parameters.ReuseLocalAddress = true;
				ClassicAssert.True (parameters.ReuseLocalAddress, "New value was not stored.");
			}
		}

		[Test]
		public void FastOpenEnabledPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.FastOpenEnabled;
				ClassicAssert.False (defaultValue, "Defalue value changed.");
				parameters.FastOpenEnabled = true;
				ClassicAssert.True (parameters.FastOpenEnabled, "New value was not stored.");
			}
		}

		[Test]
		public void ServiceClassPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.ServiceClass;
				ClassicAssert.AreEqual (NWServiceClass.BestEffort, defaultValue, "Default value changed.");
				parameters.ServiceClass = NWServiceClass.InteractiveVideo;
				ClassicAssert.AreEqual (NWServiceClass.InteractiveVideo, parameters.ServiceClass, "New value was not stored.");
			}
		}

		[Test]
		public void LocalEndpointPropertyTest ()
		{
			Assert.Ignore ("nw_parameters_copy_local_endpoint always return null. Rdar filled 44095278.");
			using (var parameters = NWParameters.CreateUdp ())
			using (var endpoint = NWEndpoint.Create (NetworkResources.MicrosoftUri.Host, "80")) {
				var defaultValue = parameters.LocalEndpoint;
				ClassicAssert.IsNull (defaultValue, "Default value changed.");
				parameters.LocalEndpoint = endpoint;
				ClassicAssert.IsNotNull (parameters.LocalEndpoint, "New value was not stored.");
			}
		}

		[Test]
		public void IncludePeerToPeerPropertyTest ()
		{
			using (var parameters = new NWParameters ()) {
				var defaultValue = parameters.IncludePeerToPeer;
				ClassicAssert.False (defaultValue, "Default value changed.");
				parameters.IncludePeerToPeer = true;
				ClassicAssert.True (parameters.IncludePeerToPeer, "New value was not stored.");
			}
		}

		[Test]
		public void TestProhibitConstrained ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);
			using (var parameters = new NWParameters ()) {
				var defaultValue = false;
				ClassicAssert.False (defaultValue, "Default value changed.");
				parameters.ProhibitConstrained = true;
				ClassicAssert.True (parameters.ProhibitConstrained, "New value was not stored.");
			}
		}

		[Test]
		public void AttributionPropertyTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using (var parameters = new NWParameters ()) {
				Assert.DoesNotThrow (() => {
					parameters.Attribution = NWParametersAttribution.Developer;
				});
				ClassicAssert.AreEqual (NWParametersAttribution.Developer, parameters.Attribution);
			}
		}

		[Test]
		[Ignore ("Crashes everywhere. Feedback filed: https://github.com/xamarin/maccore/issues/2675")]
		public void SetPrivacyContextTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			using (var privacy = NWPrivacyContext.Default)
			using (var parameters = new NWParameters ()) {
				Assert.DoesNotThrow (() => {
					parameters.SetPrivacyContext (privacy);
				});
			}
		}

		[Test]
		public void CreateApplicationServiceTest ()
		{
			TestRuntime.AssertXcodeVersion (14, 0);
			using var nwParams = NWParameters.CreateApplicationService ();
			ClassicAssert.NotNull (nwParams);
		}

		[Test]
		public void MiscPropertiesTest ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);
			using var nwParams = NWParameters.CreateApplicationService ();
			Assert.That (nwParams.AllowUltraConstrained, Is.True.Or.False, "AllowUltraConstrained#1");
			nwParams.AllowUltraConstrained = true;
			Assert.That (nwParams.AllowUltraConstrained, Is.True, "AllowUltraConstrained#2");
			nwParams.AllowUltraConstrained = false;
			Assert.That (nwParams.AllowUltraConstrained, Is.False, "AllowUltraConstrained#1");
		}
	}
}
