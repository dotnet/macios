// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CoreMediaIO;

namespace MonoTouchFixtures.CoreMediaIO {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ObjectTest {
		[Test]
		public void PropertyAddress ()
		{
			var address = new CMIOObjectPropertyAddress (CMIOObjectPropertySelector.HardwareDevices);
			Assert.That (Marshal.SizeOf<CMIOObjectPropertyAddress> (), Is.EqualTo (12), "Native size");
			Assert.That ((uint) address.Selector, Is.EqualTo (0x64657623), "Selector");
			Assert.That (address.Scope, Is.EqualTo (CMIOObjectPropertyScope.Global), "Scope");
			Assert.That (address.Element, Is.Zero, "Element");
		}

		[Test]
		public void SystemClass ()
		{
			var address = new CMIOObjectPropertyAddress (CMIOObjectPropertySelector.Class);
			Assert.That (CMIOObject.HasProperty (CMIOObject.SystemObject, in address), Is.True, "HasProperty");
			Assert.That (CMIOObject.IsPropertySettable (CMIOObject.SystemObject, in address, out var settable), Is.Zero, "IsPropertySettable");
			Assert.That (settable, Is.False, "Settable");
			Assert.That (CMIOObject.GetPropertyDataSize (CMIOObject.SystemObject, in address, [], out var size), Is.Zero, "GetPropertyDataSize");
			Assert.That (size, Is.EqualTo (4), "Data size");
			var data = new byte [4];
			Assert.That (CMIOObject.GetPropertyData (CMIOObject.SystemObject, in address, [], data, out var used), Is.Zero, "GetPropertyData");
			Assert.That (used, Is.EqualTo (4), "Data used");
			Assert.That (BitConverter.ToUInt32 (data, 0), Is.EqualTo (0x61737973), "System class");
		}

		[Test]
		public void Devices ()
		{
			var address = new CMIOObjectPropertyAddress (CMIOObjectPropertySelector.HardwareDevices);
			Assert.That (CMIOObject.GetPropertyDataSize (CMIOObject.SystemObject, in address, [], out var size), Is.Zero, "Size status");
			Assert.That (size % sizeof (uint), Is.Zero, "Identifier alignment");
			var data = new byte [size];
			Assert.That (CMIOObject.GetPropertyData (CMIOObject.SystemObject, in address, [], data, out var used), Is.Zero, "Read status");
			Assert.That (used, Is.LessThanOrEqualTo (size), "Used size");
			Assert.That (used % sizeof (uint), Is.Zero, "Used alignment");
		}

		[UnmanagedCallersOnly]
		static unsafe int Listener (uint objectId, uint count, CMIOObjectPropertyAddress* addresses, IntPtr context) => 0;

		[Test]
		public unsafe void PropertyListener ()
		{
			var address = new CMIOObjectPropertyAddress (CMIOObjectPropertySelector.HardwareDevices);
			Assert.That (CMIOObject.AddPropertyListener (CMIOObject.SystemObject, in address, &Listener, IntPtr.Zero), Is.Zero, "Add");
			Assert.That (CMIOObject.RemovePropertyListener (CMIOObject.SystemObject, in address, &Listener, IntPtr.Zero), Is.Zero, "Remove");
		}

		[Test]
		public unsafe void InvalidObjects ()
		{
			var address = new CMIOObjectPropertyAddress (CMIOObjectPropertySelector.Class);
			Assert.That (CMIOObject.HasProperty (0, in address), Is.False, "Unknown object");
			Assert.That (CMIOObject.GetPropertyDataSize (0, in address, [], out _), Is.Not.Zero, "Unknown object status");
			Assert.That (CMIOObject.GetPropertyData (0, in address, [], new byte [4], out _), Is.Not.Zero, "Read status");
			Assert.That (CMIOObject.SetPropertyData (0, in address, [], new byte [4]), Is.Not.Zero, "Write status");
			Assert.That (CMIODevice.StartStream (0, 0), Is.Not.Zero, "Start");
			Assert.That (CMIODevice.StopStream (0, 0), Is.Not.Zero, "Stop");
			Assert.That (CMIOStream.CopyBufferQueue (0, null, IntPtr.Zero, out var queue), Is.Not.Zero, "Queue status");
			Assert.That (queue, Is.Null, "Queue");
		}

		[Test]
		public void AttachmentKeys ()
		{
			Assert.That (CMIOSampleBufferAttachmentKey.SequenceNumber, Is.Not.Null, "SequenceNumber");
			Assert.That (CMIOSampleBufferAttachmentKey.HostTime, Is.Not.Null, "HostTime");
			Assert.That (CMIOSampleBufferAttachmentKey.DiscontinuityFlags, Is.Not.Null, "DiscontinuityFlags");
		}
	}
}

#endif
