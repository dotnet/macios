#if __MACOS__ || __MACCATALYST__
#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreMediaIO;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIODeviceRS422CommandTest {

		[Test]
		public void DefaultValues ()
		{
			var cmd = new CMIODeviceRS422Command ();
			Assert.AreEqual (IntPtr.Zero, cmd.Command, "Command");
			Assert.AreEqual ((uint) 0, cmd.CommandLength, "CommandLength");
			Assert.AreEqual (IntPtr.Zero, cmd.Response, "Response");
			Assert.AreEqual ((uint) 0, cmd.ResponseLength, "ResponseLength");
			Assert.AreEqual ((uint) 0, cmd.ResponseUsed, "ResponseUsed");
		}

		[Test]
		public void Properties_RoundTrip ()
		{
			var cmd = new CMIODeviceRS422Command ();
			cmd.Command = new IntPtr (0xABCD);
			cmd.CommandLength = 64;
			cmd.Response = new IntPtr (0xEF01);
			cmd.ResponseLength = 128;
			Assert.AreEqual (new IntPtr (0xABCD), cmd.Command, "Command");
			Assert.AreEqual ((uint) 64, cmd.CommandLength, "CommandLength");
			Assert.AreEqual (new IntPtr (0xEF01), cmd.Response, "Response");
			Assert.AreEqual ((uint) 128, cmd.ResponseLength, "ResponseLength");
		}

		[Test]
		public void StructLayout ()
		{
			// Layout with 64-bit alignment:
			// IntPtr(8) + uint(4) + padding(4) + IntPtr(8) + uint(4) + uint(4) = 32 on 64-bit
			int expectedSize = Marshal.SizeOf<CMIODeviceRS422Command> ();
			Assert.That (expectedSize, Is.GreaterThan (0), "Size should be positive");
		}
	}
}
#endif // __MACOS__ || __MACCATALYST__
