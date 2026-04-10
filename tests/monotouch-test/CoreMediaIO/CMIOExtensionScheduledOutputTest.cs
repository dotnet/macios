#if __MACOS__ || __MACCATALYST__
#nullable enable

using System;
using CoreMediaIO;
using Foundation;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIOExtensionScheduledOutputTest {

		[Test]
		public void Create ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var output = CMIOExtensionScheduledOutput.Create (42, 1000000);
			Assert.IsNotNull (output, "Created output");
			Assert.AreEqual ((ulong) 42, output.SequenceNumber, "SequenceNumber");
			Assert.AreEqual ((ulong) 1000000, output.HostTimeInNanoseconds, "HostTimeInNanoseconds");
		}

		[Test]
		public void Constructor ()
		{
			TestRuntime.AssertXcodeVersion (13, 3);

			var output = new CMIOExtensionScheduledOutput (100, 500000);
			Assert.AreEqual ((ulong) 100, output.SequenceNumber, "SequenceNumber");
			Assert.AreEqual ((ulong) 500000, output.HostTimeInNanoseconds, "HostTimeInNanoseconds");
		}
	}
}
#endif // __MACOS__ || __MACCATALYST__
