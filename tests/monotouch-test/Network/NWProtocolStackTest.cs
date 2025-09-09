using System;
using System.Collections.Generic;
using System.Threading;

using CoreFoundation;
using Foundation;
using Network;

using NUnit.Framework;

namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWProtocolStackTest {
		ConnectionManager manager;
		NWConnection connection;
		NWProtocolStack stack;
		List<NWProtocolOptions> options;

		[OneTimeSetUp]
		public void Init ()
		{
			manager = new ConnectionManager (true, "www.google.com");
			connection = manager.CreateConnection (out var parameters);
			stack = parameters.ProtocolStack;
			using var ipOptions = stack.InternetProtocol;
			if (ipOptions is not null)
				stack.PrependApplicationProtocol (ipOptions);
		}

		[OneTimeTearDown]
		public void Dispose ()
		{
			manager?.Dispose ();
			stack?.Dispose ();
			if (options is not null) {
				foreach (var o in options)
					o.Dispose ();
			}
		}

		[SetUp]
		public void SetUp ()
		{
			options = new List<NWProtocolOptions> ();
		}

		[Test]
		public void PrependApplicationProtocolNullOptionsTest ()
		{
			// not need to test the method with a valid argument since it is part of the setup.
			Assert.Throws<ArgumentNullException> (() => stack.PrependApplicationProtocol (null));
		}

		// handler to iterate over the app protocols
		public void InterateProtocolsHandler (NWProtocolOptions o)
		{
			options.Add (o);
		}

		[Test]
		public void ClearApplicationProtocolsTest ()
		{
			// test the clean and the iterate
			stack.IterateProtocols (InterateProtocolsHandler);
			Assert.That (options.Count, Is.GreaterThan (0), "options.Count");
			// remove present ones
			foreach (var o in options)
				o.Dispose ();
			options = new List<NWProtocolOptions> ();
			stack.ClearApplicationProtocols ();
			stack.IterateProtocols (InterateProtocolsHandler);
			Assert.AreEqual (0, options.Count, "Cleared options");
		}
		/*
		[Test]
		public void TransportProtocolPropertyTest ()
		{
			using (var options = stack.TransportProtocol)
			{
				Assert.IsNotNull (options, "Transport protocol should not be null.");
			}
			using (var options = NWProtocolOptions.CreateUdp ())
			{
				stack.TransportProtocol = options;
				using (var copyOptions = stack.TransportProtocol)
				{
					copyOptions?.IPSetUseMinimumMtu (true); // should not crash
				}
			}
		}

		[Test]
		public void InternetProtocolTest ()
		{
			using (var o = stack.InternetProtocol)
			{
				if (o is not null)
					o.IPSetUseMinimumMtu (true); // should not crash
				Assert.Inconclusive ("stack does not have an IP protocol.");
			}
		}
		*/
	}
}
