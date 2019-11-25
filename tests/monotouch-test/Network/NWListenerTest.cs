#if !__WATCHOS__
using System;
using System.Collections.Generic;
using System.Threading;
#if XAMCORE_2_0
using CoreFoundation;
using Foundation;
using Network;
using ObjCRuntime;
using Security;
#else
using MonoTouch.CoreFoundation;
using MonoTouch.Foundation;
using MonoTouch.Network;
using MonoTouch.Security;
#endif

using NUnit.Framework;

namespace MonoTouchFixtures.Network {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWListenerTest {

		NWListener listener;

		[TestFixtureSetUp]
		public void Init () => TestRuntime.AssertXcodeVersion (11, 0);

		[SetUp]
		public void SetUp ()
		{
			using (var tcpOptions = NWProtocolOptions.CreateTcp ())
			using (var tlsOptions = NWProtocolOptions.CreateTls ())
			using (var parameters = NWParameters.CreateTcp ()) {
				parameters.ProtocolStack.PrependApplicationProtocol (tlsOptions);
				parameters.ProtocolStack.PrependApplicationProtocol (tcpOptions);
				parameters.IncludePeerToPeer = true;
				listener = NWListener.Create ("1234", parameters);
			}
		}

		[TearDown]
		public void TearDown ()
		{
			listener?.Dispose ();
		}

		[Test]
		public void TestConnectionLimit ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			var defaultValue = 4294967295; // got it from running the code, if changes we will have an error.
			Assert.AreEqual (defaultValue, listener.ConnectionLimit);
			listener.ConnectionLimit = 10;
			Assert.AreEqual (10, listener.ConnectionLimit, "New value was not stored.");
		}
	}
}
#endif