//
// Unit tests for MidiEventPacket
//
// Copyright 2025 Microsoft Corp. All rights reserved.
//

#if HAS_COREMIDI

using System;
using System.Collections.Generic;
using System.Linq;

using CoreMidi;
using Foundation;

using NUnit.Framework;

namespace MonoTouchFixtures.CoreMidi {
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiEventListTest {
		[Test]
		public void CtorTest ()
		{
			Assert.Multiple (() => {
				var obj = new MidiEventList (MidiProtocolId.Protocol_1_0);
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_1_0), "Protocol");
				Assert.That (obj.PacketCount, Is.EqualTo (0), "PacketCount");
				var packets = obj.ToArray ();
				Assert.That (packets.Length, Is.EqualTo (0), "ToArray ().Length");
			});
		}

		[Test]
		public void CtorTest_Size ()
		{
			Exception ex;

			Assert.Multiple (() => {
				ex = Assert.Throws<ArgumentOutOfRangeException> (() => new MidiEventList (MidiProtocolId.Protocol_1_0, int.MinValue), "AOORE int.MinValue");
				Assert.That (ex.Message, Does.Contain ("size must be at least 276."), "AOORE msg int.MinValue");
				ex = Assert.Throws<ArgumentOutOfRangeException> (() => new MidiEventList (MidiProtocolId.Protocol_1_0, -1), "AOORE -1");
				Assert.That (ex.Message, Does.Contain ("size must be at least 276."), "AOORE msg -1");
				ex = Assert.Throws<ArgumentOutOfRangeException> (() => new MidiEventList (MidiProtocolId.Protocol_1_0, 0), "AOORE 0");
				Assert.That (ex.Message, Does.Contain ("size must be at least 276."), "AOORE msg 0");
				ex = Assert.Throws<ArgumentOutOfRangeException> (() => new MidiEventList (MidiProtocolId.Protocol_1_0, 275), "AOORE 275");
				Assert.That (ex.Message, Does.Contain ("size must be at least 276."), "AOORE msg 275");

				var obj = new MidiEventList (MidiProtocolId.Protocol_1_0, 276);
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_1_0), "Protocol");
				Assert.That (obj.PacketCount, Is.EqualTo (0), "PacketCount");
				var packets = obj.ToArray ();
				Assert.That (packets.Length, Is.EqualTo (0), "ToArray ().Length");
			});
		}

		[Test]
		public void AddTest ()
		{
			Assert.Multiple (() => {
				var obj = new MidiEventList (MidiProtocolId.Protocol_2_0);
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_2_0), "Protocol");
				Assert.That (obj.PacketCount, Is.EqualTo (0), "PacketCount");

				var rv = obj.Add (123, new uint [] { 1, 2, 3 });
				Assert.That (rv, Is.EqualTo (true), "Add B");
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_2_0), "Protocol B");
				Assert.That (obj.PacketCount, Is.EqualTo (1), "PacketCount B");

				var packets = obj.ToArray ();
				Assert.That (packets.Length, Is.EqualTo (1), "ToArray ().Length");
				Assert.That (packets [0].Timestamp, Is.EqualTo (123), "Item[0].Timestamp");
				Assert.That (packets [0].WordCount, Is.EqualTo (3), "Item[0].WordCount");
				Assert.That (packets [0].Words, Is.EqualTo (new uint [] { 1, 2, 3 }), "Item[0].Words");
			});
		}

		[Test]
		public void AddTest_ManyWords ()
		{
			Assert.Multiple (() => {
				var obj = new MidiEventList (MidiProtocolId.Protocol_2_0);
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_2_0), "Protocol");
				Assert.That (obj.PacketCount, Is.EqualTo (0), "PacketCount");

				var manyWords = Enumerable.Range (1, 65).Select (v => (uint) v).ToArray ();
				var rv = obj.Add (123, manyWords);
				Assert.That (rv, Is.EqualTo (false), "Add B");
			});
		}

		[Test]
		public void AddTest_NotEnoughSpace ()
		{
			Assert.Multiple (() => {
				var obj = new MidiEventList (MidiProtocolId.Protocol_2_0);
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_2_0), "Protocol");
				Assert.That (obj.PacketCount, Is.EqualTo (0), "PacketCount");

				var fitsTwice = Enumerable.Range (1, 24).Select (v => (uint) v).ToArray ();
				var rv = obj.Add (123, fitsTwice);
				Assert.That (rv, Is.EqualTo (true), "Add B");
				rv = obj.Add (456, fitsTwice);
				Assert.That (rv, Is.EqualTo (true), "Add C");
				rv = obj.Add (789, fitsTwice);
				Assert.That (rv, Is.EqualTo (false), "Add C");
			});
		}

		[Test]
		public void EnumeratorTest ()
		{
			Assert.Multiple (() => {
				var obj = new MidiEventList (MidiProtocolId.Protocol_2_0);
				var rv = obj.Add (789, new uint [] { 4, 5, 6 });
				Assert.That (rv, Is.EqualTo (true), "Add B");
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_2_0), "Protocol B");
				Assert.That (obj.PacketCount, Is.EqualTo (1), "PacketCount B");

				var packets = obj.ToArray ();
				Assert.That (packets.Length, Is.EqualTo (1), "ToArray ().Length");
				Assert.That (packets [0].Timestamp, Is.EqualTo (789), "Item[0].Timestamp");
				Assert.That (packets [0].WordCount, Is.EqualTo (3), "Item[0].WordCount");
				Assert.That (packets [0].Words, Is.EqualTo (new uint [] { 4, 5, 6 }), "Item[0].Words");
			});
		}

		[Test]
		public void IteratorTest ()
		{
			Assert.Multiple (() => {
				var obj = new MidiEventList (MidiProtocolId.Protocol_2_0);
				var rv = obj.Add (456, new uint [] { 1, 2, 3, 4, 5, 6 });
				Assert.That (rv, Is.EqualTo (true), "Add B");
				Assert.That (obj.Protocol, Is.EqualTo (MidiProtocolId.Protocol_2_0), "Protocol B");
				Assert.That (obj.PacketCount, Is.EqualTo (1), "PacketCount B");

				var packets = obj.ToArray ();
				Assert.That (packets.Length, Is.EqualTo (1), "ToArray ().Length");
				Assert.That (packets [0].Timestamp, Is.EqualTo (456), "Item[0].Timestamp");
				Assert.That (packets [0].WordCount, Is.EqualTo (6), "Item[0].WordCount");
				Assert.That (packets [0].Words, Is.EqualTo (new uint [] { 1, 2, 3, 4, 5, 6 }), "Item[0].Words");

				var packetList = new List<MidiEventPacket> ();
				obj.Iterate ((ref MidiEventPacket packet) => {
					packetList.Add (packet);
				});
				Assert.That (packetList.Count, Is.EqualTo (1), "packetList.Length");
				Assert.That (packetList [0].Timestamp, Is.EqualTo (456), "packetList[0].Timestamp");
				Assert.That (packetList [0].WordCount, Is.EqualTo (6), "packetList[0].WordCount");
				Assert.That (packetList [0].Words, Is.EqualTo (new uint [] { 1, 2, 3, 4, 5, 6 }), "packetList[0].Words");
			});
		}
	}
}

#endif
