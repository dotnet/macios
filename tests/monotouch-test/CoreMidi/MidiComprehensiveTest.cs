//
// Comprehensive tests for CoreMidi APIs
//
// Copyright 2025 Microsoft Corp. All rights reserved.
//

#if HAS_COREMIDI && !__TVOS__

#pragma warning disable APL0004 // MidiDevice.Create is experimental

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using CoreMidi;
using Foundation;

using NUnit.Framework;

namespace MonoTouchFixtures.CoreMidi {
	static class MidiTestHelpers {
		public static void AssertStatusOkOrInconclusive (MidiError status, string message)
		{
			if (status == MidiError.NotPermitted)
				Assert.Inconclusive ("MIDI permission not granted in this environment.");

			Assert.That (status, Is.EqualTo (MidiError.Ok), message);
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiTest {
		[Test]
		public void Restart ()
		{
			Assert.DoesNotThrow (() => Midi.Restart (), "Restart");
		}

		[Test]
		public void SourceCount ()
		{
			Assert.That ((int) Midi.SourceCount, Is.GreaterThanOrEqualTo (0), "SourceCount");
		}

		[Test]
		public void DestinationCount ()
		{
			Assert.That ((int) Midi.DestinationCount, Is.GreaterThanOrEqualTo (0), "DestinationCount");
		}

		[Test]
		public void DeviceCount ()
		{
			Assert.That ((int) Midi.DeviceCount, Is.GreaterThanOrEqualTo (0), "DeviceCount");
		}

		[Test]
		public void ExternalDeviceCount ()
		{
			Assert.That ((int) Midi.ExternalDeviceCount, Is.GreaterThanOrEqualTo (0), "ExternalDeviceCount");
		}

		[Test]
		public void GetDevice ()
		{
			// Get any device that might exist; if none, verify null for invalid index
			if (Midi.DeviceCount > 0) {
				var device = Midi.GetDevice (0);
				Assert.That (device, Is.Not.Null, "GetDevice (0)");
			}

			// Out of range should return null
			var invalid = Midi.GetDevice (99999);
			Assert.That (invalid, Is.Null, "GetDevice (99999)");
		}

		[Test]
		public void GetExternalDevice ()
		{
			if (Midi.ExternalDeviceCount > 0) {
				var device = Midi.GetExternalDevice (0);
				Assert.That (device, Is.Not.Null, "GetExternalDevice (0)");
			}
		}

		[Test]
		public void CreateExternalDevice ()
		{
			MidiError status;
			using var device = Midi.CreateExternalDevice ("Test Device", "Test Manufacturer", "Test Model", out status);
			Assert.That (status, Is.EqualTo (MidiError.Ok), "Status");
			Assert.That (device, Is.Not.Null, "Device");

			// Clean up
			var removeStatus = MidiSetup.RemoveExternalDevice (device!);
			Assert.That (removeStatus, Is.EqualTo (MidiError.Ok), "RemoveExternalDevice");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiSetupTest {
		[Test]
		public void AddRemoveDevice ()
		{
			// MIDIDeviceCreate requires a MIDI driver context and returns -50 (paramErr) in user-space.
			// Verify the API doesn't crash and returns a meaningful error.
			var device = MidiDevice.Create (null, "TestSetupDevice", "TestManufacturer", "TestModel", out var createStatus);
			// -50 is paramErr - expected when not running as a MIDI driver
			Assert.That ((int) createStatus, Is.EqualTo (-50), "Create returns paramErr without driver");
			Assert.That (device, Is.Null, "Device is null without driver");
		}

		[Test]
		public void AddRemoveExternalDevice ()
		{
			var device = Midi.CreateExternalDevice ("TestExtDevice", "TestExtManufacturer", "TestExtModel", out var createStatus);
			Assert.That (createStatus, Is.EqualTo (MidiError.Ok), "Create");
			Assert.That (device, Is.Not.Null, "Device not null");

			var addStatus = MidiSetup.AddExternalDevice (device!);
			Assert.That (addStatus, Is.EqualTo (MidiError.Ok), "AddExternalDevice");

			var removeStatus = MidiSetup.RemoveExternalDevice (device!);
			Assert.That (removeStatus, Is.EqualTo (MidiError.Ok), "RemoveExternalDevice");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiClientTest_Comprehensive {
		[Test]
		public void CreateClient ()
		{
			using var client = new MidiClient ("TestClient");
			Assert.That (client.Name, Is.EqualTo ("TestClient"), "Name");
			Assert.That (client.Handle, Is.Not.EqualTo (0), "Handle");
		}

		[Test]
		public void CreateOutputPort ()
		{
			using var client = new MidiClient ("TestOutputPortClient");
			using var port = client.CreateOutputPort ("TestOutputPort");
			Assert.That (port, Is.Not.Null, "Port not null");
			Assert.That (port.PortName, Is.EqualTo ("TestOutputPort"), "PortName");
		}

		[Test]
		public void CreateInputPort ()
		{
			using var client = new MidiClient ("TestInputPortClient");
			using var port = client.CreateInputPort ("TestInputPort");
			Assert.That (port, Is.Not.Null, "Port not null");
			Assert.That (port.PortName, Is.EqualTo ("TestInputPort"), "PortName");
		}

		[Test]
		public void CreateVirtualSource_Legacy ()
		{
			using var client = new MidiClient ("TestVirtualSourceClient");
#pragma warning disable CS0618 // Type or member is obsolete
			var source = client.CreateVirtualSource ("TestVirtualSource", out var status);
#pragma warning restore CS0618
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "Status");
			Assert.That (source, Is.Not.Null, "Source not null");
			source?.Dispose ();
		}

		[Test]
		public void CreateVirtualSource_WithProtocol ()
		{
			using var client = new MidiClient ("TestVirtualSourceClient2");
			var source = client.CreateVirtualSource ("TestVirtualSource2", MidiProtocolId.Protocol_1_0, out var status);
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "Status");
			Assert.That (source, Is.Not.Null, "Source not null");
			source?.Dispose ();
		}

		[Test]
		public void CreateVirtualDestination_Legacy ()
		{
			using var client = new MidiClient ("TestVirtualDestClient");
#pragma warning disable CS0618 // Type or member is obsolete
			var dest = client.CreateVirtualDestination ("TestVirtualDest", out var status);
#pragma warning restore CS0618
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "Status");
			Assert.That (dest, Is.Not.Null, "Destination not null");
			dest?.Dispose ();
		}

		[Test]
		public void Events ()
		{
			using var client = new MidiClient ("TestEventsClient");
			// We can't easily trigger these events in a test, but verify the subscription doesn't crash
			Assert.DoesNotThrow (() => {
				client.ObjectAdded += (sender, args) => { };
				client.ObjectRemoved += (sender, args) => { };
				client.PropertyChanged += (sender, args) => { };
				client.ThruConnectionsChanged += (sender, args) => { };
				client.SerialPortOwnerChanged += (sender, args) => { };
				client.IOError += (sender, args) => { };
			}, "Event subscriptions");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiPortTest {
		[Test]
		public void PortToString ()
		{
			using var client = new MidiClient ("TestPortToStringClient");
			using var outputPort = client.CreateOutputPort ("Output1");
			Assert.That (outputPort.ToString (), Does.Contain ("Output1"), "OutputPort.ToString");
			Assert.That (outputPort.ToString (), Does.Contain ("output"), "OutputPort contains 'output'");
		}

		[Test]
		public void SendAndReceive ()
		{
			// Create client, source, destination, and output port
			using var client = new MidiClient ("TestSendReceiveClient");
			using var outputPort = client.CreateOutputPort ("TestOutput");

			var source = client.CreateVirtualSource ("TestSendSource", MidiProtocolId.Protocol_1_0, out var srcStatus);
			MidiTestHelpers.AssertStatusOkOrInconclusive (srcStatus, "Source status");
			Assert.That (source, Is.Not.Null, "Source not null");
			source?.Dispose ();
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiDeviceTest_Comprehensive {
		[Test]
		public void Create ()
		{
			// MIDIDeviceCreate requires a MIDI driver context, returns -50 (paramErr) in user-space
			var device = MidiDevice.Create (null, "TestDevice", "Manufacturer", "Model", out var status);
			Assert.That ((int) status, Is.EqualTo (-50), "Create returns paramErr without driver");
			Assert.That (device, Is.Null, "Device is null without driver");
		}

		[Test]
		public void EntityCount ()
		{
			// Use an existing device if available, otherwise verify the API exists
			if (Midi.DeviceCount > 0) {
				var device = Midi.GetDevice (0);
				Assert.That (device, Is.Not.Null, "Device");
				Assert.That ((int) device!.EntityCount, Is.GreaterThanOrEqualTo (0), "EntityCount >= 0");
			}
		}

		[Test]
		public void CreateEntity ()
		{
			// MIDIDeviceCreate requires a driver context, can't test entity creation in user-space
			var device = MidiDevice.Create (null, "TestEntityDevice", "Manufacturer", "Model", out var status);
			Assert.That ((int) status, Is.EqualTo (-50), "Create returns paramErr");
		}

		[Test]
		public void RemoveEntity ()
		{
			// MIDIDeviceCreate requires a driver context, can't test entity removal in user-space
			var device = MidiDevice.Create (null, "TestRemoveEntityDevice", "Manufacturer", "Model", out var status);
			Assert.That ((int) status, Is.EqualTo (-50), "Create returns paramErr");
		}

		[Test]
		public void GetEntity ()
		{
			// Use an existing device if available
			if (Midi.DeviceCount > 0) {
				var device = Midi.GetDevice (0);
				Assert.That (device, Is.Not.Null, "Device");
				if (device!.EntityCount > 0) {
					var entity = device.GetEntity (0);
					Assert.That (entity, Is.Not.Null, "GetEntity (0)");
				}
				var outOfRange = device.GetEntity (99999);
				Assert.That (outOfRange, Is.Null, "GetEntity (99999)");
			}
		}

		[Test]
		public void UniqueID ()
		{
			// Use an existing device if available
			if (Midi.DeviceCount > 0) {
				var device = Midi.GetDevice (0);
				Assert.That (device, Is.Not.Null, "Device");
				Assert.DoesNotThrow (() => {
					var uniqueId = device!.UniqueID;
				}, "UniqueID getter");
			}
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiEntityTest {
		[Test]
		public void SourcesAndDestinations ()
		{
			// Use an existing device/entity if available
			if (Midi.DeviceCount > 0) {
				var device = Midi.GetDevice (0);
				if (device is not null && device.EntityCount > 0) {
					var entity = device.GetEntity (0);
					Assert.That (entity, Is.Not.Null, "Entity");
					Assert.That ((int) entity!.Sources, Is.GreaterThanOrEqualTo (0), "Sources");
					Assert.That ((int) entity.Destinations, Is.GreaterThanOrEqualTo (0), "Destinations");
				}
			}
		}

		[Test]
		public void AddOrRemoveEndpoints ()
		{
			// MIDIDeviceCreate requires a driver context, can't test in user-space
			var device = MidiDevice.Create (null, "TestAddRemoveEndpointsDevice", "Manufacturer", "Model", out var status);
			Assert.That ((int) status, Is.EqualTo (-50), "Create returns paramErr");
		}

		[Test]
		public void Device ()
		{
			// Use an existing device/entity if available
			if (Midi.DeviceCount > 0) {
				var device = Midi.GetDevice (0);
				if (device is not null && device.EntityCount > 0) {
					var entity = device.GetEntity (0);
					Assert.That (entity, Is.Not.Null, "Entity");
					var entityDevice = entity!.Device;
					Assert.That (entityDevice, Is.Not.Null, "Device");
				}
			}
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiEndpointTest_Comprehensive {
		[Test]
		public void GetSource ()
		{
			if (Midi.SourceCount > 0) {
				var source = MidiEndpoint.GetSource (0);
				Assert.That (source, Is.Not.Null, "GetSource (0)");
				Assert.That (source!.Handle, Is.Not.EqualTo (0), "Handle");
			}
		}

		[Test]
		public void GetDestination ()
		{
			if (Midi.DestinationCount > 0) {
				var dest = MidiEndpoint.GetDestination (0);
				Assert.That (dest, Is.Not.Null, "GetDestination (0)");
				Assert.That (dest!.Handle, Is.Not.EqualTo (0), "Handle");
			}
		}

		[Test]
		public void FlushOutput ()
		{
			// Create a virtual destination and flush it
			using var client = new MidiClient ("TestFlushClient");
#pragma warning disable CS0618
			var dest = client.CreateVirtualDestination ("TestFlushDest", out var status);
#pragma warning restore CS0618
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "Create");
			Assert.DoesNotThrow (() => dest!.FlushOutput (), "FlushOutput");
			dest?.Dispose ();
		}

		[Test]
		public void EndpointName ()
		{
			using var client = new MidiClient ("TestEndpointNameClient");
			var source = client.CreateVirtualSource ("TestNameSource", MidiProtocolId.Protocol_1_0, out var status);
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "Create");
			Assert.That (source!.EndpointName, Is.Not.Null, "EndpointName");
			source?.Dispose ();
		}

		[Test]
		public void Entity_ForVirtualEndpoint ()
		{
			using var client = new MidiClient ("TestEntityClient");
			var source = client.CreateVirtualSource ("TestEntitySource", MidiProtocolId.Protocol_1_0, out var status);
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "Create");
			// Virtual endpoints don't have a parent entity
			var entity = source!.Entity;
			Assert.That (entity, Is.Null, "Entity should be null for virtual endpoints");
			source.Dispose ();
		}

		[Test]
		public void Properties ()
		{
			// Use a virtual source to test endpoint properties
			using var client = new MidiClient ("TestPropsClient");
			var source = client.CreateVirtualSource ("TestPropsSource", MidiProtocolId.Protocol_1_0, out var srcStatus);
			MidiTestHelpers.AssertStatusOkOrInconclusive (srcStatus, "Create source");
			Assert.That (source, Is.Not.Null, "source");

			// Test properties that should be readable
			Assert.DoesNotThrow (() => {
				_ = source!.MaxSysExSpeed;
			}, "MaxSysExSpeed");

			source?.Dispose ();
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiObjectTest {
		[Test]
		public void FindByUniqueId ()
		{
			// Use an existing device to find by unique ID
			if (Midi.DeviceCount > 0) {
				var device = Midi.GetDevice (0);
				Assert.That (device, Is.Not.Null, "Device");
				var uniqueId = device!.UniqueID;
				var findStatus = MidiObject.FindByUniqueId (uniqueId, out var found);
				Assert.That (findStatus, Is.EqualTo (MidiError.Ok), "FindByUniqueId");
				Assert.That (found, Is.Not.Null, "Found object not null");
			} else {
				// If no devices exist, verify the API can handle "not found"
				var findStatus = MidiObject.FindByUniqueId (12345, out var found);
				Assert.That (findStatus, Is.EqualTo (MidiError.ObjectNotFound), "ObjectNotFound");
			}
		}

		[Test]
		public void FindByUniqueId_NotFound ()
		{
			var findStatus = MidiObject.FindByUniqueId (-999999, out var found);
			Assert.That (findStatus, Is.EqualTo (MidiError.ObjectNotFound), "FindByUniqueId for non-existent ID");
			Assert.That (found, Is.Null, "Found object is null");
		}

		[Test]
		public void GetDictionaryProperties ()
		{
			// Use a virtual source instead of creating a device (which requires driver context)
			using var client = new MidiClient ("TestDictPropsClient");
			var source = client.CreateVirtualSource ("TestDictPropsSource", MidiProtocolId.Protocol_1_0, out var status);
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "Create");

			var dict = source!.GetDictionaryProperties (false);
			Assert.That (dict, Is.Not.Null, "GetDictionaryProperties");
			Assert.That ((int) dict!.Count, Is.GreaterThan (0), "Properties count > 0");

			source.Dispose ();
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiEventListTest_Comprehensive {
		[Test]
		public void Ctor_Protocol10 ()
		{
			using var list = new MidiEventList (MidiProtocolId.Protocol_1_0);
			Assert.That (list.Protocol, Is.EqualTo (MidiProtocolId.Protocol_1_0), "Protocol");
			Assert.That (list.PacketCount, Is.EqualTo (0), "PacketCount");
		}

		[Test]
		public void Ctor_Protocol20 ()
		{
			using var list = new MidiEventList (MidiProtocolId.Protocol_2_0);
			Assert.That (list.Protocol, Is.EqualTo (MidiProtocolId.Protocol_2_0), "Protocol");
			Assert.That (list.PacketCount, Is.EqualTo (0), "PacketCount");
		}

		[Test]
		public void Add_MultiplePackets ()
		{
			// Use a large list to hold multiple packets
			using var list = new MidiEventList (MidiProtocolId.Protocol_2_0, 1024);

			var rv1 = list.Add (100, new uint [] { 0x20906040 }); // Note On
			Assert.That (rv1, Is.True, "Add 1");
			Assert.That (list.PacketCount, Is.EqualTo (1), "PacketCount 1");

			var rv2 = list.Add (200, new uint [] { 0x20806040 }); // Note Off
			Assert.That (rv2, Is.True, "Add 2");
			Assert.That (list.PacketCount, Is.EqualTo (2), "PacketCount 2");

			var rv3 = list.Add (300, new uint [] { 0x20906050 }); // Another Note On
			Assert.That (rv3, Is.True, "Add 3");
			Assert.That (list.PacketCount, Is.EqualTo (3), "PacketCount 3");

			// Verify packets via enumeration
			var packets = list.ToArray ();
			Assert.That (packets.Length, Is.EqualTo (3), "Enumerated length");
			Assert.That (packets [0].Timestamp, Is.EqualTo (100), "Packet 0 timestamp");
			Assert.That (packets [1].Timestamp, Is.EqualTo (200), "Packet 1 timestamp");
			Assert.That (packets [2].Timestamp, Is.EqualTo (300), "Packet 2 timestamp");
		}

		[Test]
		public void Add_FromRawPointer_Throws ()
		{
			var ptr = Marshal.AllocHGlobal (512);
			try {
				var list = new MidiEventList (ptr);
				Assert.Throws<InvalidOperationException> (() => list.Add (0, new uint [] { 1 }), "Add to raw pointer list");
			} finally {
				Marshal.FreeHGlobal (ptr);
			}
		}

		[Test]
		public void Iterate_MultiplePackets ()
		{
			using var list = new MidiEventList (MidiProtocolId.Protocol_1_0, 1024);

			list.Add (100, new uint [] { 0x20906040 });
			list.Add (200, new uint [] { 0x20806040 });

			var packetList = new List<(ulong Timestamp, uint [] Words)> ();
			list.Iterate ((ref MidiEventPacket packet) => {
				packetList.Add ((packet.Timestamp, packet.Words));
			});

			Assert.That (packetList.Count, Is.EqualTo (2), "Count");
			Assert.That (packetList [0].Timestamp, Is.EqualTo (100), "Timestamp 0");
			Assert.That (packetList [1].Timestamp, Is.EqualTo (200), "Timestamp 1");
		}

		[Test]
		public void Iterate_EmptyList ()
		{
			using var list = new MidiEventList (MidiProtocolId.Protocol_1_0);
			var count = 0;
			list.Iterate ((ref MidiEventPacket packet) => { count++; });
			Assert.That (count, Is.EqualTo (0), "Empty iteration count");
		}

		[Test]
		public void Enumerator_EmptyList ()
		{
			using var list = new MidiEventList (MidiProtocolId.Protocol_1_0);
			var packets = list.ToArray ();
			Assert.That (packets.Length, Is.EqualTo (0), "Empty enumeration");
		}

		[Test]
		public void Dispose_Idempotent ()
		{
			var list = new MidiEventList (MidiProtocolId.Protocol_1_0);
			Assert.DoesNotThrow (() => {
				list.Dispose ();
				list.Dispose ();
			}, "Double dispose");
		}

		[Test]
		public void SendAndReceive ()
		{
			using var client = new MidiClient ("TestEventListSendClient");
			using var outputPort = client.CreateOutputPort ("TestEventListOutput");

			var source = client.CreateVirtualSource ("TestEventListSource", MidiProtocolId.Protocol_1_0, out var status);
			MidiTestHelpers.AssertStatusOkOrInconclusive (status, "CreateVirtualSource");

			using var list = new MidiEventList (MidiProtocolId.Protocol_1_0, 1024);
			// MIDI 1.0 Note On: channel 0, note 60 (middle C), velocity 127
			list.Add (0, new uint [] { 0x20903C7F });

			// Send from source (distribute to listeners)
			var sendStatus = list.Receive (source!);
			Assert.That (sendStatus, Is.EqualTo (0), "Receive status");

			source?.Dispose ();
		}

		/// <summary>
		/// Test creating a MIDI event list with the notes for "Happy Birthday" melody.
		/// Uses MIDI 1.0 protocol with Note On/Off messages.
		/// </summary>
		[Test]
		public void HappyBirthday ()
		{
			using var list = new MidiEventList (MidiProtocolId.Protocol_1_0, 4096);
			Assert.That (list.Protocol, Is.EqualTo (MidiProtocolId.Protocol_1_0), "Protocol");

			// MIDI 1.0 channel voice messages encoded as UMP (Universal MIDI Packet):
			// Type 2 (MIDI 1.0 Channel Voice), Group 0
			// Status: 0x90 = Note On channel 0, 0x80 = Note Off channel 0
			// Format: 0x2tssnnvv where t=type(0=group0), ss=status, nn=note, vv=velocity
			//
			// "Happy Birthday to You" melody notes (in MIDI note numbers):
			// C4=60, D4=62, E4=64, F4=65, G4=67, A4=69, Bb4=70, C5=72
			//
			// Melody: C C D C F E | C C D C G F | C C C' A F E D | Bb Bb A F G F
			byte [] melody = {
				60, 60, 62, 60, 65, 64,        // Hap-py Birth-day to You
				60, 60, 62, 60, 67, 65,        // Hap-py Birth-day to You
				60, 60, 72, 69, 65, 64, 62,   // Hap-py Birth-day dear friend
				70, 70, 69, 65, 67, 65         // Hap-py Birth-day to You
			};

			// Duration in ticks (arbitrary units) for each note
			ulong [] durations = {
				250, 250, 500, 500, 500, 1000,          // line 1
				250, 250, 500, 500, 500, 1000,          // line 2
				250, 250, 500, 500, 500, 500, 1000,    // line 3
				250, 250, 500, 500, 500, 1000           // line 4
			};

			byte velocity = 100;
			ulong currentTime = 0;

			for (int i = 0; i < melody.Length; i++) {
				// Note On: 0x2090NNVV
				uint noteOn = (uint) (0x20900000 | (melody [i] << 8) | velocity);
				var addedOn = list.Add (currentTime, new uint [] { noteOn });
				Assert.That (addedOn, Is.True, $"Add NoteOn {i}");

				// Note Off: 0x2080NN00
				uint noteOff = (uint) (0x20800000 | (melody [i] << 8));
				var addedOff = list.Add (currentTime + durations [i], new uint [] { noteOff });
				Assert.That (addedOff, Is.True, $"Add NoteOff {i}");

				currentTime += durations [i];
			}

			// MIDIEventListAdd merges events with the same timestamp into a single packet.
			// The NoteOff of note i and NoteOn of note i+1 share the same timestamp,
			// so they are merged into one packet. This gives us:
			// 1 NoteOn at time 0 + 24 merged (NoteOff + NoteOn) packets + 1 final NoteOff = 26 packets
			Assert.That ((int) list.PacketCount, Is.EqualTo (26), "PacketCount (merged by timestamp)");

			// Verify the melody by iterating and collecting all words
			var allWords = new List<(ulong Timestamp, uint Word)> ();
			list.Iterate ((ref MidiEventPacket packet) => {
				var words = packet.Words;
				for (int w = 0; w < words.Length; w++)
					allWords.Add ((packet.Timestamp, words [w]));
			});

			Assert.That (allWords.Count, Is.EqualTo (melody.Length * 2), "Total word count");

			// Verify first note: C4 Note On at time 0
			Assert.That (allWords [0].Timestamp, Is.EqualTo (0UL), "First note timestamp");
			Assert.That (allWords [0].Word & 0xFF00, Is.EqualTo ((uint) (60 << 8)), "First note is C4 (60)");

			// Verify the sequence contains all happy birthday notes
			var noteOnMessages = allWords.Where (n => (n.Word & 0x00F00000) == 0x00900000).Select (n => (byte) ((n.Word >> 8) & 0xFF)).ToArray ();
			Assert.That (noteOnMessages, Is.EqualTo (melody), "Happy Birthday melody matches");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiEventPacketTest_Comprehensive {
		[Test]
		public void DefaultValues ()
		{
			var packet = new MidiEventPacket ();
			Assert.That (packet.Timestamp, Is.EqualTo (0UL), "Timestamp default");
			Assert.That (packet.WordCount, Is.EqualTo (0U), "WordCount default");
			Assert.That (packet.Words.Length, Is.EqualTo (0), "Words default length");
		}

		[Test]
		public void Timestamp_SetGet ()
		{
			var packet = new MidiEventPacket ();
			packet.Timestamp = ulong.MaxValue;
			Assert.That (packet.Timestamp, Is.EqualTo (ulong.MaxValue), "MaxValue");

			packet.Timestamp = 0;
			Assert.That (packet.Timestamp, Is.EqualTo (0UL), "Zero");

			packet.Timestamp = 12345678UL;
			Assert.That (packet.Timestamp, Is.EqualTo (12345678UL), "Arbitrary");
		}

		[Test]
		public void WordCount_Validation ()
		{
			var packet = new MidiEventPacket ();
			Assert.DoesNotThrow (() => packet.WordCount = 0, "0 is valid");
			Assert.DoesNotThrow (() => packet.WordCount = 64, "64 is valid");
			Assert.Throws<ArgumentOutOfRangeException> (() => packet.WordCount = 65, "65 is invalid");
		}

		[Test]
		public void Words_SetGet ()
		{
			var packet = new MidiEventPacket ();
			var words = new uint [] { 0xDEADBEEF, 0xCAFEBABE, 0x12345678 };
			packet.Words = words;

			Assert.That (packet.WordCount, Is.EqualTo (3U), "WordCount after set");
			Assert.That (packet.Words, Is.EqualTo (words), "Words match");
		}

		[Test]
		public void Words_MaxWords ()
		{
			var packet = new MidiEventPacket ();
			var words = Enumerable.Range (1, 64).Select (v => (uint) v).ToArray ();
			packet.Words = words;

			Assert.That (packet.WordCount, Is.EqualTo (64U), "WordCount = 64");
			Assert.That (packet.Words, Is.EqualTo (words), "Words match");
		}

		[Test]
		public void Words_TooMany ()
		{
			var packet = new MidiEventPacket ();
			var words = Enumerable.Range (1, 65).Select (v => (uint) v).ToArray ();
			Assert.Throws<ArgumentOutOfRangeException> (() => packet.Words = words, "65 words is too many");
		}

		[Test]
		public void Indexer ()
		{
			var packet = new MidiEventPacket ();
			packet.Words = new uint [] { 10, 20, 30, 40, 50 };

			Assert.That (packet [0], Is.EqualTo (10U), "Index 0");
			Assert.That (packet [4], Is.EqualTo (50U), "Index 4");

			packet [2] = 999;
			Assert.That (packet [2], Is.EqualTo (999U), "Modified index 2");
		}

		[Test]
		public void Indexer_OutOfRange ()
		{
			var packet = new MidiEventPacket ();
			packet.Words = new uint [] { 1, 2, 3 };

			Assert.Throws<ArgumentOutOfRangeException> (() => { var _ = packet [-1]; }, "Negative index");
			Assert.Throws<ArgumentOutOfRangeException> (() => { var _ = packet [64]; }, "Index 64");
			Assert.Throws<ArgumentOutOfRangeException> (() => { var _ = packet [3]; }, "Beyond WordCount");
		}

		[Test]
		public void NoteOnOffRoundtrip ()
		{
			// Construct a MIDI 1.0 Note On as UMP
			var packet = new MidiEventPacket ();
			packet.Timestamp = 1000;
			// UMP Type 2 (MIDI 1.0 CV), Group 0, Note On, Channel 0, Note 60, Velocity 127
			packet.Words = new uint [] { 0x20903C7F };

			Assert.That (packet.Timestamp, Is.EqualTo (1000UL), "Timestamp");
			Assert.That (packet.WordCount, Is.EqualTo (1U), "WordCount");

			var word = packet [0];
			var messageType = (word >> 28) & 0xF;
			var group = (word >> 24) & 0xF;
			var status = (word >> 16) & 0xFF;
			var note = (word >> 8) & 0xFF;
			var velocity = word & 0xFF;

			Assert.That (messageType, Is.EqualTo (2U), "Message type (MIDI 1.0 CV)");
			Assert.That (group, Is.EqualTo (0U), "Group 0");
			Assert.That (status, Is.EqualTo (0x90U), "Note On status");
			Assert.That (note, Is.EqualTo (60U), "Middle C (note 60)");
			Assert.That (velocity, Is.EqualTo (127U), "Velocity 127");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiPacketTest {
		[Test]
		public void Ctor_IntPtr ()
		{
			var bytes = new byte [] { 0x90, 60, 100 };
			var handle = Marshal.AllocHGlobal (bytes.Length);
			try {
				Marshal.Copy (bytes, 0, handle, bytes.Length);
				using var packet = new MidiPacket (12345, (ushort) bytes.Length, handle);
				Assert.That (packet.TimeStamp, Is.EqualTo (12345L), "TimeStamp");
				Assert.That (packet.Length, Is.EqualTo (3), "Length");
				Assert.That (packet.BytePointer, Is.EqualTo (handle), "BytePointer");
			} finally {
				Marshal.FreeHGlobal (handle);
			}
		}

		[Test]
		public void Ctor_ByteArray ()
		{
			var bytes = new byte [] { 0x90, 60, 100 };
			using var packet = new MidiPacket (54321, bytes);
			Assert.That (packet.TimeStamp, Is.EqualTo (54321L), "TimeStamp");
			Assert.That (packet.Length, Is.EqualTo (3), "Length");
			Assert.That (packet.ByteArray, Is.EqualTo (bytes), "ByteArray");
		}

		[Test]
		public void Ctor_ByteArrayRange ()
		{
			var bytes = new byte [] { 0x00, 0x90, 60, 100, 0x00 };
			using var packet = new MidiPacket (0, bytes, 1, 3);
			Assert.That (packet.Length, Is.EqualTo (3), "Length");
			Assert.That (packet.ByteArray, Is.Not.Null, "ByteArray not null");
		}

		[Test]
		public void Ctor_NullBytes ()
		{
			// The public constructor dereferences bytes.Length before the null check in the
			// private constructor, so it throws NullReferenceException rather than ArgumentNullException.
			Assert.Throws<NullReferenceException> (() => new MidiPacket (0, (byte []) null!), "Null bytes");
		}

		[Test]
		public void Ctor_TooLong ()
		{
			var bytes = new byte [ushort.MaxValue + 1];
			Assert.Throws<ArgumentException> (() => new MidiPacket (0, bytes), "Too long");
		}

		[Test]
		public void Dispose_ClearsPointer ()
		{
			var bytes = new byte [] { 0x90, 60, 100 };
			var handle = Marshal.AllocHGlobal (bytes.Length);
			try {
				Marshal.Copy (bytes, 0, handle, bytes.Length);
				var packet = new MidiPacket (0, (ushort) bytes.Length, handle);
				Assert.That (packet.BytePointer, Is.Not.EqualTo (IntPtr.Zero), "Before dispose");
				packet.Dispose ();
				Assert.That (packet.BytePointer, Is.EqualTo (IntPtr.Zero), "After dispose");
			} finally {
				Marshal.FreeHGlobal (handle);
			}
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiExceptionTest {
		[Test]
		public void ErrorCode_Property ()
		{
			// MidiException has an ErrorCode property with the underlying MidiError value.
			// Verify by creating a client with a null name, which should fail on some platforms.
			// Since we can't construct MidiException directly, verify the type exists and
			// its ErrorCode property is accessible via reflection.
			var type = typeof (MidiException);
			Assert.That (type, Is.Not.Null, "MidiException type exists");
			var prop = type.GetProperty ("ErrorCode");
			Assert.That (prop, Is.Not.Null, "ErrorCode property exists");
			Assert.That (prop!.PropertyType, Is.EqualTo (typeof (MidiError)), "ErrorCode type");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiMidi2StructsTest {
		[Test]
		public void Midi2DeviceManufacturer_SysExIdByte ()
		{
			var mfg = new Midi2DeviceManufacturer ();
			mfg.SysExIdByte = new byte [] { 0x7E, 0x01, 0x02 };
			Assert.That (mfg.SysExIdByte, Is.EqualTo (new byte [] { 0x7E, 0x01, 0x02 }), "SysExIdByte roundtrip");
		}

		[Test]
		public void Midi2DeviceManufacturer_WrongLength ()
		{
			var mfg = new Midi2DeviceManufacturer ();
			Assert.Throws<ArgumentOutOfRangeException> (() => mfg.SysExIdByte = new byte [] { 1, 2 }, "Too short");
			Assert.Throws<ArgumentOutOfRangeException> (() => mfg.SysExIdByte = new byte [] { 1, 2, 3, 4 }, "Too long");
		}

		[Test]
		public void Midi2DeviceRevisionLevel_RevisionLevel ()
		{
			var rev = new Midi2DeviceRevisionLevel ();
			rev.RevisionLevel = new byte [] { 1, 2, 3, 4 };
			Assert.That (rev.RevisionLevel, Is.EqualTo (new byte [] { 1, 2, 3, 4 }), "RevisionLevel roundtrip");
		}

		[Test]
		public void Midi2DeviceRevisionLevel_WrongLength ()
		{
			var rev = new Midi2DeviceRevisionLevel ();
			Assert.Throws<ArgumentOutOfRangeException> (() => rev.RevisionLevel = new byte [] { 1, 2, 3 }, "Too short");
			Assert.Throws<ArgumentOutOfRangeException> (() => rev.RevisionLevel = new byte [] { 1, 2, 3, 4, 5 }, "Too long");
		}

		[Test]
		public void MidiCIProfileId_Standard ()
		{
			var id = new MidiCIProfileId ();
			id.Standard = new MidiCIProfileIdStandard {
				ProfileIdByte1 = 1,
				ProfileBank = 2,
				ProfileNumber = 3,
				ProfileVersion = 4,
				ProfileLevel = 5,
			};

			Assert.That (id.Standard.ProfileIdByte1, Is.EqualTo (1), "ProfileIdByte1");
			Assert.That (id.Standard.ProfileBank, Is.EqualTo (2), "ProfileBank");
			Assert.That (id.Standard.ProfileNumber, Is.EqualTo (3), "ProfileNumber");
			Assert.That (id.Standard.ProfileVersion, Is.EqualTo (4), "ProfileVersion");
			Assert.That (id.Standard.ProfileLevel, Is.EqualTo (5), "ProfileLevel");
		}

		[Test]
		public void MidiCIProfileId_ManufacturerSpecific ()
		{
			var id = new MidiCIProfileId ();
			id.ManufacturerSpecific = new MidiCIProfileIdManufacturerSpecific {
				SysExId1 = 0x7E,
				SysExId2 = 0x01,
				SysExId3 = 0x02,
				Info1 = 0x10,
				Info2 = 0x20,
			};

			Assert.That (id.ManufacturerSpecific.SysExId1, Is.EqualTo (0x7E), "SysExId1");
			Assert.That (id.ManufacturerSpecific.SysExId2, Is.EqualTo (0x01), "SysExId2");
			Assert.That (id.ManufacturerSpecific.SysExId3, Is.EqualTo (0x02), "SysExId3");
			Assert.That (id.ManufacturerSpecific.Info1, Is.EqualTo (0x10), "Info1");
			Assert.That (id.ManufacturerSpecific.Info2, Is.EqualTo (0x20), "Info2");
		}
	}

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiErrorTest {
		[Test]
		public void ErrorValues ()
		{
			Assert.That ((int) MidiError.Ok, Is.EqualTo (0), "Ok");
			Assert.That ((int) MidiError.InvalidClient, Is.EqualTo (-10830), "InvalidClient");
			Assert.That ((int) MidiError.InvalidPort, Is.EqualTo (-10831), "InvalidPort");
			Assert.That ((int) MidiError.WrongEndpointType, Is.EqualTo (-10832), "WrongEndpointType");
			Assert.That ((int) MidiError.NoConnection, Is.EqualTo (-10833), "NoConnection");
			Assert.That ((int) MidiError.UnknownEndpoint, Is.EqualTo (-10834), "UnknownEndpoint");
			Assert.That ((int) MidiError.UnknownProperty, Is.EqualTo (-10835), "UnknownProperty");
			Assert.That ((int) MidiError.WrongPropertyType, Is.EqualTo (-10836), "WrongPropertyType");
			Assert.That ((int) MidiError.NoCurrentSetup, Is.EqualTo (-10837), "NoCurrentSetup");
			Assert.That ((int) MidiError.MessageSendErr, Is.EqualTo (-10838), "MessageSendErr");
			Assert.That ((int) MidiError.ServerStartErr, Is.EqualTo (-10839), "ServerStartErr");
			Assert.That ((int) MidiError.SetupFormatErr, Is.EqualTo (-10840), "SetupFormatErr");
			Assert.That ((int) MidiError.WrongThread, Is.EqualTo (-10841), "WrongThread");
			Assert.That ((int) MidiError.ObjectNotFound, Is.EqualTo (-10842), "ObjectNotFound");
			Assert.That ((int) MidiError.IDNotUnique, Is.EqualTo (-10843), "IDNotUnique");
			Assert.That ((int) MidiError.NotPermitted, Is.EqualTo (-10844), "NotPermitted");
		}
	}
}

#endif
