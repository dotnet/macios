using System;
using System.Threading;

using Foundation;
using Network;
using CoreFoundation;

using NUnit.Framework;
using MonoTests.System.Net.Http;


namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWProtocolIPOptionsTest {
		ConnectionManager manager;
		NWConnection connection;
		NWProtocolStack stack;
		NWProtocolIPOptions options;

		[OneTimeSetUp]
		public void Init ()
		{
			manager = new ConnectionManager (true);
			connection = manager.CreateConnection (out var parameters);

			stack = parameters.ProtocolStack;
			using (var ipOptions = stack.InternetProtocol) {
				if (ipOptions is not null) {
					stack.PrependApplicationProtocol (ipOptions);
				}
			}
		}

		[OneTimeTearDown]
		public void Dispose ()
		{
			manager?.Dispose ();
			stack?.Dispose ();
		}

		[SetUp]
		public void SetUp ()
		{
			options = stack.InternetProtocol as NWProtocolIPOptions;
			Assert.NotNull (options, "options");
		}

		// we cannot assert that the C code those the right thing, BUT we do know
		// that if we call the property with the wrong pointer we will get an exception
		// from the runtime because the C lib does check the pointer that is used for the call

		[Test]
		public void SetIPVersionTest () => Assert.DoesNotThrow (() => options.SetVersion (NWIPVersion.Version6));

		[Test]
		public void SetHopLimitTest () => Assert.DoesNotThrow (() => options.SetHopLimit (1));


		[Test]
		public void SetUseMinimumMtu () => Assert.DoesNotThrow (() => options.SetUseMinimumMtu (true));


		[Test]
		public void SetDisableFragmentation () => Assert.DoesNotThrow (() => options.SetDisableFragmentation (true));


		[Test]
		public void SetCaculateReceiveTimeTest () => Assert.DoesNotThrow (() => options.SetCalculateReceiveTime (true));

		[Test]
		public void SetIPLocalAddressPreference () => Assert.DoesNotThrow (() => options.SetIPLocalAddressPreference (NWIPLocalAddressPreference.Temporary));

		[Test]
		public void DisableMulticastLoopbackTest ()
		{
			TestRuntime.AssertXcodeVersion (13, 0);
			Assert.DoesNotThrow (() => options.DisableMulticastLoopback (false));
		}
	}
}
