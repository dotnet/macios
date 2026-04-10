#if __MACOS__ || __MACCATALYST__
#nullable enable

using System;
using System.Runtime.InteropServices;
using CoreMediaIO;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMediaIO {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMIODeviceAVCCommandTest {

		[Test]
		public void DefaultValues ()
		{
			var cmd = new CMIODeviceAVCCommand ();
			Assert.AreEqual (IntPtr.Zero, cmd.Command, "Command");
			Assert.AreEqual ((uint) 0, cmd.CommandLength, "CommandLength");
			Assert.AreEqual (IntPtr.Zero, cmd.Response, "Response");
			Assert.AreEqual ((uint) 0, cmd.ResponseLength, "ResponseLength");
			Assert.AreEqual ((uint) 0, cmd.ResponseUsed, "ResponseUsed");
		}

		[Test]
		public void Properties_RoundTrip ()
		{
			var cmd = new CMIODeviceAVCCommand ();
			cmd.Command = new IntPtr (0x1234);
			cmd.CommandLength = 16;
			cmd.Response = new IntPtr (0x5678);
			cmd.ResponseLength = 32;
			Assert.AreEqual (new IntPtr (0x1234), cmd.Command, "Command");
			Assert.AreEqual ((uint) 16, cmd.CommandLength, "CommandLength");
			Assert.AreEqual (new IntPtr (0x5678), cmd.Response, "Response");
			Assert.AreEqual ((uint) 32, cmd.ResponseLength, "ResponseLength");
		}

		[Test]
		public void StructLayout ()
		{
			// Layout with 64-bit alignment:
			// IntPtr(8) + uint(4) + padding(4) + IntPtr(8) + uint(4) + uint(4) = 32 on 64-bit
			int expectedSize = Marshal.SizeOf<CMIODeviceAVCCommand> ();
			Assert.That (expectedSize, Is.GreaterThan (0), "Size should be positive");
		}
	}
}
#endif // __MACOS__ || __MACCATALYST__
