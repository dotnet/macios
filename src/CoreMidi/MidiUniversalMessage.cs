// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable

#pragma warning disable CS0649 // Field '...' is never assigned to, and will always have its default value
#pragma warning disable CS0169 // The field '...' is never used

namespace CoreMidi {
	/// <summary>A representation of all possible messages stored in a Universal MIDI packet (UMP).</summary>
	/// <remarks>
	///   <para>This is the managed representation of the native <c>MIDIUniversalMessage</c> struct. The active variant is determined by the <see cref="Type" /> property: only the union member matching the message type contains valid data.</para>
	/// </remarks>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Explicit, Size = 28)]
	public struct MidiUniversalMessage {
		[FieldOffset (0)]
		MidiMessageType type;
		[FieldOffset (4)]
		byte group;
		[FieldOffset (8)]
		MidiUniversalMessageUtility utility;
		[FieldOffset (8)]
		MidiUniversalMessageSystem system;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice1 channelVoice1;
		[FieldOffset (8)]
		MidiUniversalMessageSysEx sysEx;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2 channelVoice2;
		[FieldOffset (8)]
		MidiUniversalMessageData128 data128;
		[FieldOffset (8)]
		MidiUniversalMessageUnknown unknown;

		/// <summary>The message type. Determines which variant in the union is valid.</summary>
		public MidiMessageType Type => type;

		/// <summary>The 4-bit MIDI group this message belongs to.</summary>
		public byte Group => group;

		/// <summary>The utility message data. Valid when <see cref="Type" /> is <see cref="MidiMessageType.Utility" />.</summary>
		public MidiUniversalMessageUtility Utility => utility;

		/// <summary>The system message data. Valid when <see cref="Type" /> is <see cref="MidiMessageType.System" />.</summary>
		public MidiUniversalMessageSystem System => system;

		/// <summary>The MIDI 1.0 channel voice message data. Valid when <see cref="Type" /> is <see cref="MidiMessageType.ChannelVoice1" />.</summary>
		public MidiUniversalMessageChannelVoice1 ChannelVoice1 => channelVoice1;

		/// <summary>The system exclusive (SysEx) message data. Valid when <see cref="Type" /> is <see cref="MidiMessageType.SysEx" />.</summary>
		public MidiUniversalMessageSysEx SysEx => sysEx;

		/// <summary>The MIDI 2.0 channel voice message data. Valid when <see cref="Type" /> is <see cref="MidiMessageType.ChannelVoice2" />.</summary>
		public MidiUniversalMessageChannelVoice2 ChannelVoice2 => channelVoice2;

		/// <summary>The 128-bit data message data. Valid when <see cref="Type" /> is <see cref="MidiMessageType.Data128" />.</summary>
		public MidiUniversalMessageData128 Data128 => data128;

		/// <summary>The raw words of an unknown message. Valid when <see cref="Type" /> is not a recognized message type.</summary>
		public MidiUniversalMessageUnknown Unknown => unknown;
	}

	/// <summary>A utility message in a <see cref="MidiUniversalMessage" />.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Explicit, Size = 8)]
	public struct MidiUniversalMessageUtility {
		[FieldOffset (0)]
		MidiUtilityStatus status;
		[FieldOffset (4)]
		ushort jitterReductionClock;
		[FieldOffset (4)]
		ushort jitterReductionTimestamp;

		/// <summary>The status determining which value is valid.</summary>
		public MidiUtilityStatus Status => status;

		/// <summary>The jitter reduction clock. Valid when <see cref="Status" /> is <see cref="MidiUtilityStatus.JitterReductionClock" />.</summary>
		public ushort JitterReductionClock => jitterReductionClock;

		/// <summary>The jitter reduction timestamp. Valid when <see cref="Status" /> is <see cref="MidiUtilityStatus.JitterReductionTimestamp" />.</summary>
		public ushort JitterReductionTimestamp => jitterReductionTimestamp;
	}

	/// <summary>A system message in a <see cref="MidiUniversalMessage" />.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Explicit, Size = 8)]
	public struct MidiUniversalMessageSystem {
		[FieldOffset (0)]
		MidiSystemStatus status;
		[FieldOffset (4)]
		byte timeCode;
		[FieldOffset (4)]
		ushort songPositionPointer;
		[FieldOffset (4)]
		byte songSelect;

		/// <summary>The status determining which value is valid.</summary>
		public MidiSystemStatus Status => status;

		/// <summary>The MIDI time code. Valid when <see cref="Status" /> is <see cref="MidiSystemStatus.Mtc" />.</summary>
		public byte TimeCode => timeCode;

		/// <summary>The song position pointer. Valid when <see cref="Status" /> is <see cref="MidiSystemStatus.SongPosPointer" />.</summary>
		public ushort SongPositionPointer => songPositionPointer;

		/// <summary>The selected song. Valid when <see cref="Status" /> is <see cref="MidiSystemStatus.SongSelect" />.</summary>
		public byte SongSelect => songSelect;
	}

	/// <summary>The note data of a MIDI 1.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice1Note {
		byte number;
		byte velocity;

		/// <summary>The 7-bit note number.</summary>
		public byte Number => number;

		/// <summary>The 7-bit note velocity.</summary>
		public byte Velocity => velocity;
	}

	/// <summary>The poly pressure data of a MIDI 1.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice1PolyPressure {
		byte noteNumber;
		byte pressure;

		/// <summary>The 7-bit note number.</summary>
		public byte NoteNumber => noteNumber;

		/// <summary>The 7-bit poly pressure data.</summary>
		public byte Pressure => pressure;
	}

	/// <summary>The control change data of a MIDI 1.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice1ControlChange {
		byte index;
		byte data;

		/// <summary>The 7-bit index of the control parameter.</summary>
		public byte Index => index;

		/// <summary>The 7-bit value of the control parameter.</summary>
		public byte Data => data;
	}

	/// <summary>A MIDI 1.0 channel voice message in a <see cref="MidiUniversalMessage" />.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Explicit, Size = 12)]
	public struct MidiUniversalMessageChannelVoice1 {
		[FieldOffset (0)]
		MidiCVStatus status;
		[FieldOffset (4)]
		byte channel;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice1Note note;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice1PolyPressure polyPressure;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice1ControlChange controlChange;
		[FieldOffset (8)]
		byte program;
		[FieldOffset (8)]
		byte channelPressure;
		[FieldOffset (8)]
		ushort pitchBend;

		/// <summary>The status determining which value is valid.</summary>
		public MidiCVStatus Status => status;

		/// <summary>The MIDI channel (0-15).</summary>
		public byte Channel => channel;

		/// <summary>The note data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.NoteOff" /> or <see cref="MidiCVStatus.NoteOn" />.</summary>
		public MidiUniversalMessageChannelVoice1Note Note => note;

		/// <summary>The poly pressure data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.PolyPressure" />.</summary>
		public MidiUniversalMessageChannelVoice1PolyPressure PolyPressure => polyPressure;

		/// <summary>The control change data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.ControlChange" />.</summary>
		public MidiUniversalMessageChannelVoice1ControlChange ControlChange => controlChange;

		/// <summary>The 7-bit program number. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.ProgramChange" />.</summary>
		public byte Program => program;

		/// <summary>The 7-bit channel pressure. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.ChannelPressure" />.</summary>
		public byte ChannelPressure => channelPressure;

		/// <summary>The pitch bend value. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.PitchBend" />.</summary>
		public ushort PitchBend => pitchBend;
	}

	/// <summary>A system exclusive (SysEx) message in a <see cref="MidiUniversalMessage" />.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct MidiUniversalMessageSysEx {
		MidiSysExStatus status;
		byte channel;
		fixed byte data [6];
		byte reserved;

		/// <summary>The status determining how the message should be interpreted.</summary>
		public MidiSysExStatus Status => status;

		/// <summary>The MIDI channel (0-15).</summary>
		public byte Channel => channel;

		/// <summary>The SysEx data (6 bytes, 7-bit values each).</summary>
		/// <returns>A 6-element array with the SysEx data.</returns>
		public byte [] Data {
			get {
				var rv = new byte [6];
				for (var i = 0; i < rv.Length; i++)
					rv [i] = data [i];
				return rv;
			}
		}
	}

	/// <summary>The note data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2Note {
		byte number;
		MidiNoteAttribute attributeType;
		ushort velocity;
		ushort attribute;

		/// <summary>The 7-bit note number.</summary>
		public byte Number => number;

		/// <summary>The attribute type.</summary>
		public MidiNoteAttribute AttributeType => attributeType;

		/// <summary>The note velocity.</summary>
		public ushort Velocity => velocity;

		/// <summary>The attribute data.</summary>
		public ushort Attribute => attribute;
	}

	/// <summary>The poly pressure data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2PolyPressure {
		byte noteNumber;
		byte reserved;
		uint pressure;

		/// <summary>The 7-bit note number.</summary>
		public byte NoteNumber => noteNumber;

		/// <summary>The pressure value.</summary>
		public uint Pressure => pressure;
	}

	/// <summary>The control change data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2ControlChange {
		byte index;
		byte reserved;
		uint data;

		/// <summary>The 7-bit controller number.</summary>
		public byte Index => index;

		/// <summary>The controller value.</summary>
		public uint Data => data;
	}

	/// <summary>The program change data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2ProgramChange {
		MidiProgramChangeOptions options;
		byte program;
		byte reserved0;
		byte reserved1;
		ushort bank;

		/// <summary>The program change options.</summary>
		public MidiProgramChangeOptions Options => options;

		/// <summary>The 7-bit program number.</summary>
		public byte Program => program;

		/// <summary>The 14-bit bank.</summary>
		public ushort Bank => bank;
	}

	/// <summary>The channel pressure data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2ChannelPressure {
		uint data;
		byte reserved0;
		byte reserved1;

		/// <summary>The channel pressure data.</summary>
		public uint Data => data;
	}

	/// <summary>The pitch bend data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2PitchBend {
		uint data;
		byte reserved0;
		byte reserved1;

		/// <summary>The pitch bend data.</summary>
		public uint Data => data;
	}

	/// <summary>The per-note controller data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2PerNoteController {
		byte noteNumber;
		byte index;
		uint data;

		/// <summary>The 7-bit note number.</summary>
		public byte NoteNumber => noteNumber;

		/// <summary>The 7-bit controller number.</summary>
		public byte Index => index;

		/// <summary>The controller data.</summary>
		public uint Data => data;
	}

	/// <summary>The registered/assignable controller data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2Controller {
		byte bank;
		byte index;
		uint data;

		/// <summary>The 7-bit bank.</summary>
		public byte Bank => bank;

		/// <summary>The 7-bit controller number.</summary>
		public byte Index => index;

		/// <summary>The controller data.</summary>
		public uint Data => data;
	}

	/// <summary>The per-note pitch bend data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2PerNotePitchBend {
		byte noteNumber;
		byte reserved;
		uint bend;

		/// <summary>The 7-bit note number.</summary>
		public byte NoteNumber => noteNumber;

		/// <summary>The per-note pitch bend value.</summary>
		public uint Bend => bend;
	}

	/// <summary>The per-note management data of a MIDI 2.0 channel voice message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	public struct MidiUniversalMessageChannelVoice2PerNoteManagement {
		byte note;
		MidiPerNoteManagementOptions options;
		byte reserved0;
		byte reserved1;
		byte reserved2;
		byte reserved3;

		/// <summary>The 7-bit note number.</summary>
		public byte Note => note;

		/// <summary>The per-note management options.</summary>
		public MidiPerNoteManagementOptions Options => options;
	}

	/// <summary>A MIDI 2.0 channel voice message in a <see cref="MidiUniversalMessage" />.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Explicit, Size = 16)]
	public struct MidiUniversalMessageChannelVoice2 {
		[FieldOffset (0)]
		MidiCVStatus status;
		[FieldOffset (4)]
		byte channel;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2Note note;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2PolyPressure polyPressure;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2ControlChange controlChange;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2ProgramChange programChange;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2ChannelPressure channelPressure;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2PitchBend pitchBend;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2PerNoteController perNoteController;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2Controller controller;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2PerNotePitchBend perNotePitchBend;
		[FieldOffset (8)]
		MidiUniversalMessageChannelVoice2PerNoteManagement perNoteManagement;

		/// <summary>The status determining which value is valid.</summary>
		public MidiCVStatus Status => status;

		/// <summary>The MIDI channel.</summary>
		public byte Channel => channel;

		/// <summary>The note data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.NoteOff" /> or <see cref="MidiCVStatus.NoteOn" />.</summary>
		public MidiUniversalMessageChannelVoice2Note Note => note;

		/// <summary>The poly pressure data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.PolyPressure" />.</summary>
		public MidiUniversalMessageChannelVoice2PolyPressure PolyPressure => polyPressure;

		/// <summary>The control change data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.ControlChange" />.</summary>
		public MidiUniversalMessageChannelVoice2ControlChange ControlChange => controlChange;

		/// <summary>The program change data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.ProgramChange" />.</summary>
		public MidiUniversalMessageChannelVoice2ProgramChange ProgramChange => programChange;

		/// <summary>The channel pressure data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.ChannelPressure" />.</summary>
		public MidiUniversalMessageChannelVoice2ChannelPressure ChannelPressure => channelPressure;

		/// <summary>The pitch bend data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.PitchBend" />.</summary>
		public MidiUniversalMessageChannelVoice2PitchBend PitchBend => pitchBend;

		/// <summary>The per-note controller data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.RegisteredPnc" /> or <see cref="MidiCVStatus.AssignablePnc" />.</summary>
		public MidiUniversalMessageChannelVoice2PerNoteController PerNoteController => perNoteController;

		/// <summary>The registered/assignable controller data. Valid when <see cref="Status" /> is one of <see cref="MidiCVStatus.RegisteredControl" />, <see cref="MidiCVStatus.AssignableControl" />, <see cref="MidiCVStatus.RelRegisteredControl" /> or <see cref="MidiCVStatus.RelAssignableControl" />.</summary>
		public MidiUniversalMessageChannelVoice2Controller Controller => controller;

		/// <summary>The per-note pitch bend data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.PerNotePitchBend" />.</summary>
		public MidiUniversalMessageChannelVoice2PerNotePitchBend PerNotePitchBend => perNotePitchBend;

		/// <summary>The per-note management data. Valid when <see cref="Status" /> is <see cref="MidiCVStatus.PerNoteMgmt" />.</summary>
		public MidiUniversalMessageChannelVoice2PerNoteManagement PerNoteManagement => perNoteManagement;
	}

	/// <summary>The 8-bit system exclusive (SysEx8) data of a 128-bit data message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct MidiUniversalMessageSysEx8 {
		byte byteCount;
		byte streamID;
		fixed byte data [13];
		byte reserved;

		/// <summary>The byte count of the data including the stream ID (1-14 bytes).</summary>
		public byte ByteCount => byteCount;

		/// <summary>The stream ID.</summary>
		public byte StreamId => streamID;

		/// <summary>The SysEx8 data (13 bytes).</summary>
		/// <returns>A 13-element array with the SysEx8 data.</returns>
		public byte [] Data {
			get {
				var rv = new byte [13];
				for (var i = 0; i < rv.Length; i++)
					rv [i] = data [i];
				return rv;
			}
		}
	}

	/// <summary>The mixed data set of a 128-bit data message.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct MidiUniversalMessageMixedDataSet {
		byte mdsID;
		fixed byte data [14];
		byte reserved;

		/// <summary>The mixed data set ID.</summary>
		public byte MixedDataSetId => mdsID;

		/// <summary>The mixed data set data (14 bytes).</summary>
		/// <returns>A 14-element array with the mixed data set data.</returns>
		public byte [] Data {
			get {
				var rv = new byte [14];
				for (var i = 0; i < rv.Length; i++)
					rv [i] = data [i];
				return rv;
			}
		}
	}

	/// <summary>A 128-bit data message in a <see cref="MidiUniversalMessage" />.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Explicit, Size = 20)]
	public struct MidiUniversalMessageData128 {
		[FieldOffset (0)]
		MidiSysExStatus status;
		[FieldOffset (4)]
		MidiUniversalMessageSysEx8 sysEx8;
		[FieldOffset (4)]
		MidiUniversalMessageMixedDataSet mixedDataSet;

		/// <summary>The status determining which value is valid.</summary>
		public MidiSysExStatus Status => status;

		/// <summary>The SysEx8 data. Valid when <see cref="Status" /> is one of <see cref="MidiSysExStatus.Complete" />, <see cref="MidiSysExStatus.Start" />, <see cref="MidiSysExStatus.Continue" /> or <see cref="MidiSysExStatus.End" />.</summary>
		public MidiUniversalMessageSysEx8 SysEx8 => sysEx8;

		/// <summary>The mixed data set. Valid when <see cref="Status" /> is <see cref="MidiSysExStatus.MixedDataSetHeader" /> or <see cref="MidiSysExStatus.MixedDataSetPayload" />.</summary>
		public MidiUniversalMessageMixedDataSet MixedDataSet => mixedDataSet;
	}

	/// <summary>The raw words of an unknown message in a <see cref="MidiUniversalMessage" />.</summary>
	[SupportedOSPlatform ("ios15.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	[StructLayout (LayoutKind.Sequential)]
	public unsafe struct MidiUniversalMessageUnknown {
		fixed uint words [4];

		/// <summary>The raw words of the message (up to four 32-bit words).</summary>
		/// <returns>A 4-element array with the raw words.</returns>
		public uint [] Words {
			get {
				var rv = new uint [4];
				for (var i = 0; i < rv.Length; i++)
					rv [i] = words [i];
				return rv;
			}
		}
	}
}

#pragma warning restore CS0649
#pragma warning restore CS0169
