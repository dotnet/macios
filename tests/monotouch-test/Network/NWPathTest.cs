using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CoreFoundation;
using Foundation;
using Network;
using ObjCRuntime;
using NUnit.Framework;
using MonoTests.System.Net.Http;


namespace MonoTouchFixtures.Network {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class NWPathTest {
		NWPath path;
		List<NWInterface> interfaces = new List<NWInterface> ();
		ConnectionManager manager;
		NWConnection connection;

		[OneTimeSetUp]
		public void Init ()
		{
			// we want to use a single connection, since it is expensive
			interfaces = new List<NWInterface> ();
			// we create a connection which we are going to use to get the availabe
			// interfaces, that way we can later test protperties of the NWParameters class.
			manager = new ConnectionManager ();
			connection = manager.CreateConnection ();
		}

		[OneTimeTearDown]
		public void Dispose ()
		{
			manager?.Dispose ();
			foreach (var i in interfaces)
				i.Dispose ();
		}

		[SetUp]
		public void SetUp ()
		{

			path = connection.CurrentPath;
			path.EnumerateInterfaces (EnumerateInterfacesHandler);
			Assert.That (interfaces.Count, Is.GreaterThan (0), "interfaces.Count");
		}

		void EnumerateInterfacesHandler (NWInterface nwInterface)
		{
			interfaces.Add (nwInterface);
		}

		[TearDown]
		public void TearDown ()
		{
			path?.Dispose ();
		}

		[Test]
		public void StatusPropertyTest ()
		{
			Assert.AreEqual (NWPathStatus.Satisfied, path.Status, $"Unexpected status {path.Status}");
		}

		[Test]
		public void IsExpensivePropertyTest ()
		{
			Assert.False (path.IsExpensive, "Path was not expected to be expensive."); // To be tested as part of NWProtocolStack
		}

		[Test]
		public void HasIPV4PropertyTest ()
		{
#if !MONOMAC && !__MACCATALYST__
			if (Runtime.Arch != Arch.DEVICE)
				Assert.False (path.HasIPV4, "By default the interface does not support IPV4 on the simulator");
			else
#endif
				Assert.True (path.HasIPV4, "By default the interface does support IPV4 on the device");
		}

		[Test]
		public void HasIPV6PropertyTest ()
		{
			Assert.Ignore ("We cannot test the use of IPV6 since it is different per machine configuraton and makes the test flaky.");
			Assert.False (path.HasIPV6, "By default the interface does not support IPV6"); // To be tested as part of NWProtocolStack
		}

		[Test]
		public void HasDnsPropertyTest ()
		{
#if !MONOMAC && !__MACCATALYST__
			if (Runtime.Arch != Arch.DEVICE)
				Assert.False (path.HasDns, "By default the interface does not support DNS on the simulator");
			else
#endif
				Assert.True (path.HasDns, "By default the interface does support DNS on the device");
		}

		[Test]
		public void UsesInterfaceTypeTest ()
		{
			foreach (var i in interfaces) {
				Assert.True (path.UsesInterfaceType (i.InterfaceType), $"Type {i.InterfaceType} should be in use.");
			}
		}

		[Test]
		public void IterateInterfacesTest ()
		{
			// clean interfaces from setup
			foreach (var i in interfaces)
				i.Dispose ();
			interfaces = new List<NWInterface> ();
			path.EnumerateInterfaces (EnumerateInterfacesHandler);
			Assert.That (interfaces.Count, Is.GreaterThan (0), "interfaces.Count");
		}

		[Test]
		public void EnumerateGatewayNullCallbackTest ()
		{
			TestRuntime.AssertXcodeVersion (11, 0);

			Assert.Throws<ArgumentNullException> (() => { path.EnumerateGateways ((Func<NWEndpoint, bool>) null); });
		}

		[Test]
		public void EnumerateGatewayTest ()
		{
			var e1 = new TaskCompletionSource<bool> ();
			var monitor = new NWPathMonitor ();
			try {
				monitor.SetQueue (DispatchQueue.DefaultGlobalQueue);
				monitor.SnapshotHandler += path => {
					path.EnumerateGateways (gateway => {
						e1.TrySetResult (true);
						return true;
					});
				};
				monitor.Start ();
				var rv = TestRuntime.RunAsync (TimeSpan.FromSeconds (5),
						Task.CompletedTask,
						e1.Task);
				if (!rv)
					Assert.Ignore ("No gateways on this machine?"); // no gateways isn't all that uncommon, so just always ignore in this case.
				Assert.IsTrue (rv, "Called back");
			} finally {
				monitor.Cancel ();
				monitor.Dispose ();
			}
		}

		[Test]
		public void GetUnsatisfiedReason ()
		{
			TestRuntime.AssertXcodeVersion (12, 2);
			Assert.That (path.GetUnsatisfiedReason (), Is.EqualTo (NWPathUnsatisfiedReason.NotAvailable));
		}

		[Test]
		public void IsUltraConstrained ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);
			Assert.That (path.IsUltraConstrained, Is.EqualTo (false).Or.EqualTo (true), "IsUltraConstrained");
		}

		[Test]
		public void LinkQuality ()
		{
			TestRuntime.AssertXcodeVersion (26, 0);
			Assert.That (path.LinkQuality, Is.EqualTo (NWLinkQuality.Good).Or.EqualTo (NWLinkQuality.Unknown), "LinkQuality");
		}
	}
}
