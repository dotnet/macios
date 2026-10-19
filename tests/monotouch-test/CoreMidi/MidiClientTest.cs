//
// Unit tests for MidiClient
//
// Authors:
//	Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2023 Microsoft Corp. All rights reserved.
//

#if !__TVOS__
using System.Diagnostics;
using System.Runtime.InteropServices;

using CoreMidi;

namespace MonoTouchFixtures.CoreMidi {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiClientTest {
#if !XAMCORE_5_0
#pragma warning disable CS0618
		[Test]
		public unsafe void PacketBytes ()
		{
			var bytes = new byte [] { 1, 2, 3, 4 };
			using var packet = new MidiPacket (0, bytes, 1, 2);
			var pointer = packet.Bytes;
			GC.Collect (GC.MaxGeneration, GCCollectionMode.Forced, true, true);
			fixed (byte* current = &bytes [1])
				Assert.That (pointer, Is.EqualTo ((IntPtr) current), "Pinned array");
			Assert.That (packet.Bytes, Is.EqualTo (pointer), "Stable pointer");
			Assert.That (Marshal.ReadByte (pointer), Is.EqualTo (2), "Offset");
			bytes [1] = 42;
			Assert.That (Marshal.ReadByte (pointer), Is.EqualTo (42), "Array mutation");
			Assert.That (packet.ByteArray, Is.SameAs (bytes), "Original array");
			Assert.That (packet.BytePointer, Is.EqualTo (IntPtr.Zero), "Unmanaged pointer");
			packet.Dispose ();
			packet.Dispose ();
			Assert.Throws<ObjectDisposedException> (() => { _ = packet.Bytes; }, "Disposed");
		}

		[Test]
		public unsafe void PacketBytesNative ()
		{
			byte value = 42;
			var pointer = (IntPtr) (&value);
			using var packet = new MidiPacket (0, 1, pointer);
			Assert.That (packet.Bytes, Is.EqualTo (pointer), "Original pointer");
			Assert.That (packet.BytePointer, Is.EqualTo (pointer), "Unmanaged pointer");
			Assert.That (packet.ByteArray, Is.Null, "No managed array");
		}

		[Test]
		public void PacketBytesEmpty ()
		{
			using var packet = new MidiPacket (0, []);
			Assert.That (packet.Bytes, Is.EqualTo (IntPtr.Zero), "Empty array");
		}

		[System.Runtime.CompilerServices.MethodImpl (System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		static WeakReference CreateUnrootedPacket ()
		{
			var bytes = new byte [1024];
			var packet = new MidiPacket (0, bytes);
			_ = packet.Bytes;
			return new WeakReference (bytes);
		}

		[Test]
		public void PacketBytesFinalization ()
		{
			var bytes = CreateUnrootedPacket ();
			GC.Collect ();
			GC.WaitForPendingFinalizers ();
			GC.Collect ();
			Assert.That (bytes.IsAlive, Is.False, "Array unpinned by finalizer");
		}
#pragma warning restore CS0618
#endif

		[Test]
		public void SendTest ()
		{
			if (Midi.DestinationCount <= 0)
				Assert.Inconclusive ("No Midi Destinations");

			using var ep = MidiEndpoint.GetDestination (0);
			Assert.That (ep, Is.Not.Null, "EndPoint");

			var mevent = new byte [] { 0x90, 0x40, 0x70 };
			using var client = new MidiClient ($"outputclient-{Process.GetCurrentProcess ().Id}");
			using var port = client.CreateOutputPort ($"outputport-{Process.GetCurrentProcess ().Id}");
			unsafe {
				fixed (byte* meventPtr = mevent) {
					using var packet1 = new MidiPacket (0, (ushort) mevent.Length, (IntPtr) meventPtr);
					using var packet2 = new MidiPacket (0, mevent);
					using var packet3 = new MidiPacket (0, mevent, 0, mevent.Length);
					var packets = new MidiPacket [] { packet1, packet2, packet3 };
					port.Send (ep, packets);
				}
			}
		}
	}
}
#endif
