//
// Unit tests for MidiThruConnection
//
// Authors:
//   Rolf Bjarne Kvinge <rolf@xamarin.com>
//
// Copyright 2024 Microsoft Corp. All rights reserved.
//

#if !__TVOS__
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using CoreMidi;

namespace MonoTouchFixtures.CoreMidi {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class MidiThruConnectionParamsTests {
		static byte [] DefaultStruct {
			get {
				return new byte [] {
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03,
					0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x00, 0x00, 0x00, 0x7f,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
				};
			}
		}

		static byte [] DefaultChannelMap {
			get {
				return new byte [] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
			}
		}

		string [] parameterSuffixes = new string [] {
			" Arg_ParamName_Name, value",
			" (Parameter 'value')",
			"\nParameter name: value",
		};

		void AssertAreEqualWithAnySuffix (string expectedWithoutSuffix, string actual, string message)
		{
			Assert.That (actual, Is.AnyOf (parameterSuffixes.Select (suffix => expectedWithoutSuffix + suffix).ToArray ()), message);
		}

		[Test]
		public void ParamsTest ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.Sources, Is.Null, "Sources");
			Assert.That (p.Destinations, Is.Null, "Destinations");
			AreEqual (DefaultChannelMap, p.ChannelMap, "ChannelMap");
			Assert.That (p.LowVelocity, Is.EqualTo (0), "LowVelocity");
			Assert.That (p.HighVelocity, Is.EqualTo (0), "HighVelocity");
			Assert.That (p.LowNote, Is.EqualTo (0), "LowNote");
			Assert.That (p.HighNote, Is.EqualTo (127), "HighNote");
			var defaultMidiTransform = default (MidiTransform);
			AreEqual (defaultMidiTransform, p.NoteNumber, "NoteNumber");
			AreEqual (defaultMidiTransform, p.Velocity, "Velocity");
			AreEqual (defaultMidiTransform, p.KeyPressure, "KeyPressure");
			AreEqual (defaultMidiTransform, p.ChannelPressure, "ChannelPressure");
			AreEqual (defaultMidiTransform, p.ProgramChange, "ProgramChange");
			AreEqual (defaultMidiTransform, p.PitchBend, "PitchBend");
			Assert.That (p.FilterOutSysEx, Is.EqualTo (false), "FilterOutSysEx");
			Assert.That (p.FilterOutMtc, Is.EqualTo (false), "FilterOutMtc");
			Assert.That (p.FilterOutBeatClock, Is.EqualTo (false), "FilterOutBeatClock");
			Assert.That (p.FilterOutTuneRequest, Is.EqualTo (false), "FilterOutTuneRequest");
			Assert.That (p.FilterOutAllControls, Is.EqualTo (false), "FilterOutAllControls");
			Assert.That (p.Controls, Is.Null, "Controls");
			Assert.That (p.Maps, Is.Null, "Maps");

			var bytes = GetData (p);
			AreEqual (DefaultStruct, bytes, "Bytes");
		}

		[Test]
		public void PropertiesTest_Sources ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.Sources, Is.Null, "Sources 1");

			// Set to some array
			var array = new MidiThruConnectionEndpoint [] { new MidiThruConnectionEndpoint (0, 0), new MidiThruConnectionEndpoint (3, 4) };
			p.Sources = array;
			Assert.That (p.Sources, Is.EqualTo (array), "Sources 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [4] = 2;
			expectedStruct2b [16] = 3;
			expectedStruct2b [20] = 4;
			AreEqual (expectedStruct2b, GetData (p), "Sources 2b");

			// Set back to default value
			p.Sources = null;
			Assert.That (p.Sources, Is.Null, "Sources 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "Sources 3b");

			// Set to more than 8 sources
			var ex = Assert.Throws<ArgumentOutOfRangeException> (() => { p.Sources = new MidiThruConnectionEndpoint [9]; }, "Sources 4");
			AssertAreEqualWithAnySuffix ($"A maximum of 8 endpoints are allowed", ex.Message, "Sources 4b");
		}

		[Test]
		public void PropertiesTest_Destinations ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.Destinations, Is.Null, "Destinations 1");

			// Set to some array
			var array = new MidiThruConnectionEndpoint [] { new MidiThruConnectionEndpoint (0, 0), new MidiThruConnectionEndpoint (3, 4) };
			p.Destinations = array;
			Assert.That (p.Destinations, Is.EqualTo (array), "Destinations 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [72] = 2;
			expectedStruct2b [84] = 3;
			expectedStruct2b [88] = 4;
			AreEqual (expectedStruct2b, GetData (p), "Destinations 2b");

			// Set back to default value
			p.Destinations = null;
			Assert.That (p.Destinations, Is.Null, "Destinations 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "Destinations 3b");

			// Set to more than 8 destinations
			var ex = Assert.Throws<ArgumentOutOfRangeException> (() => { p.Destinations = new MidiThruConnectionEndpoint [9]; }, "Sources 4");
			AssertAreEqualWithAnySuffix ($"A maximum of 8 endpoints are allowed", ex.Message, "Destinations 4b");
		}

		[Test]
		public void PropertiesTest_ChannelMap ()
		{
			var p = new MidiThruConnectionParams ();
			AreEqual (DefaultChannelMap, p.ChannelMap, "ChannelMap 1");

			// Set to some array
			var array = new byte [] { 1, 2 };
			p.ChannelMap = array;
			var expectedChannelMap2 = new byte [16];
			expectedChannelMap2 [0] = 1;
			expectedChannelMap2 [1] = 2;
			AreEqual (expectedChannelMap2, p.ChannelMap, "ChannelMap 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [140] = 0x01;
			expectedStruct2b [141] = 0x02;
			expectedStruct2b [142] = 0x00;
			expectedStruct2b [143] = 0x00;
			expectedStruct2b [144] = 0x00;
			expectedStruct2b [145] = 0x00;
			expectedStruct2b [146] = 0x00;
			expectedStruct2b [147] = 0x00;
			expectedStruct2b [148] = 0x00;
			expectedStruct2b [149] = 0x00;
			expectedStruct2b [150] = 0x00;
			expectedStruct2b [151] = 0x00;
			expectedStruct2b [152] = 0x00;
			expectedStruct2b [153] = 0x00;
			expectedStruct2b [154] = 0x00;
			expectedStruct2b [155] = 0x00;
			AreEqual (expectedStruct2b, GetData (p), "ChannelMap 2b");

			// Set back to default value
			// This one is a bit weird: the default channel map is all zeroes, it's 'new byte [] { 1, 2, 3, ..., 15, 16 }'
			// So when we set it to null here (which will set to an array of all zeros),
			// we have to update the expected data as well accordingly (to all zeroes).
			p.ChannelMap = null;
			expectedStruct2b [140] = 0x00;
			expectedStruct2b [141] = 0x00;
			expectedStruct2b [142] = 0x00;
			expectedStruct2b [143] = 0x00;
			expectedStruct2b [144] = 0x00;
			expectedStruct2b [145] = 0x00;
			expectedStruct2b [146] = 0x00;
			expectedStruct2b [147] = 0x00;
			expectedStruct2b [148] = 0x00;
			expectedStruct2b [149] = 0x00;
			expectedStruct2b [150] = 0x00;
			expectedStruct2b [151] = 0x00;
			expectedStruct2b [152] = 0x00;
			expectedStruct2b [153] = 0x00;
			expectedStruct2b [154] = 0x00;
			expectedStruct2b [155] = 0x00;
			AreEqual (new byte [16], p.ChannelMap, "ChannelMap 3");
			AreEqual (expectedStruct2b, GetData (p), "ChannelMap 3b");

			// Set to more than 16 channels
			var ex = Assert.Throws<ArgumentOutOfRangeException> (() => { p.ChannelMap = new byte [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }; }, "ChannelMap 4");
			AssertAreEqualWithAnySuffix ($"A maximum of 16 channels are allowed", ex.Message, "ChannelMap 4b");
		}

		[Test]
		public void PropertiesTest_LowVelocity ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.LowVelocity, Is.EqualTo (0), "LowVelocity 1");

			// Set to some value
			p.LowVelocity = 42;
			Assert.That (p.LowVelocity, Is.EqualTo (42), "LowVelocity 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [156] = 42;
			AreEqual (expectedStruct2b, GetData (p), "LowVelocity 2b");

			// Set back to default value
			p.LowVelocity = 0;
			Assert.That (p.LowVelocity, Is.EqualTo (0), "LowVelocity 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "LowVelocity 3b");
		}

		[Test]
		public void PropertiesTest_HighVelocity ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.HighVelocity, Is.EqualTo (0), "HighVelocity 1");

			// Set to some value
			p.HighVelocity = 42;
			Assert.That (p.HighVelocity, Is.EqualTo (42), "HighVelocity 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [157] = 42;
			AreEqual (expectedStruct2b, GetData (p), "HighVelocity 2b");

			// Set back to default value
			p.HighVelocity = 0;
			Assert.That (p.HighVelocity, Is.EqualTo (0), "HighVelocity 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "HighVelocity 3b");
		}

		[Test]
		public void PropertiesTest_LowNote ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.LowNote, Is.EqualTo (0), "LowNote 1");

			// Set to some value
			p.LowNote = 42;
			Assert.That (p.LowNote, Is.EqualTo (42), "LowNote 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [158] = 42;
			AreEqual (expectedStruct2b, GetData (p), "LowNote 2b");

			// Set back to default value
			p.LowNote = 0;
			Assert.That (p.LowNote, Is.EqualTo (0), "LowNote 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "LowNote 3b");
		}

		[Test]
		public void PropertiesTest_HighNote ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.HighNote, Is.EqualTo (127), "HighNote 1");

			// Set to some value
			p.HighNote = 42;
			Assert.That (p.HighNote, Is.EqualTo (42), "HighNote 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [159] = 42;
			AreEqual (expectedStruct2b, GetData (p), "HighNote 2b");

			// Set back to default value
			p.HighNote = 127;
			Assert.That (p.HighNote, Is.EqualTo (127), "HighNote 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "HighNote 3b");
		}

		[Test]
		public void PropertiesTest_NoteNumber ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.NoteNumber, Is.EqualTo (default (MidiTransform)), "NoteNumber 1");

			// Set to some value
			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			p.NoteNumber = someMidiTransformType;
			AreEqual (someMidiTransformType, p.NoteNumber, "NoteNumber 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [160] = 0x01;
			expectedStruct2b [162] = 0x02;
			AreEqual (expectedStruct2b, GetData (p), "NoteNumber 2b");

			// Set back to default value
			p.NoteNumber = default (MidiTransform);
			AreEqual (default (MidiTransform), p.NoteNumber, "NoteNumber 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "NoteNumber 3b");
		}

		[Test]
		public void PropertiesTest_Velocity ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.Velocity, Is.EqualTo (default (MidiTransform)), "Velocity 1");

			// Set to some value
			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			p.Velocity = someMidiTransformType;
			AreEqual (someMidiTransformType, p.Velocity, "Velocity 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [164] = 0x01;
			expectedStruct2b [166] = 0x02;
			AreEqual (expectedStruct2b, GetData (p), "Velocity 2b");

			// Set back to default value
			p.Velocity = default (MidiTransform);
			AreEqual (default (MidiTransform), p.Velocity, "Velocity 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "Velocity 3b");
		}

		[Test]
		public void PropertiesTest_KeyPressure ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.KeyPressure, Is.EqualTo (default (MidiTransform)), "KeyPressure 1");

			// Set to some value
			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			p.KeyPressure = someMidiTransformType;
			AreEqual (someMidiTransformType, p.KeyPressure, "KeyPressure 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [168] = 0x01;
			expectedStruct2b [170] = 0x02;
			AreEqual (expectedStruct2b, GetData (p), "KeyPressure 2b");

			// Set back to default value
			p.KeyPressure = default (MidiTransform);
			AreEqual (default (MidiTransform), p.KeyPressure, "KeyPressure 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "KeyPressure 3b");
		}

		[Test]
		public void PropertiesTest_ChannelPressure ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.ChannelPressure, Is.EqualTo (default (MidiTransform)), "ChannelPressure 1");

			// Set to some value
			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			p.ChannelPressure = someMidiTransformType;
			AreEqual (someMidiTransformType, p.ChannelPressure, "ChannelPressure 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [172] = 0x01;
			expectedStruct2b [174] = 0x02;
			AreEqual (expectedStruct2b, GetData (p), "ChannelPressure 2b");

			// Set back to default value
			p.ChannelPressure = default (MidiTransform);
			AreEqual (default (MidiTransform), p.ChannelPressure, "ChannelPressure 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "ChannelPressure 3b");
		}

		[Test]
		public void PropertiesTest_ProgramChange ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.ProgramChange, Is.EqualTo (default (MidiTransform)), "ProgramChange 1");

			// Set to some value
			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			p.ProgramChange = someMidiTransformType;
			AreEqual (someMidiTransformType, p.ProgramChange, "ProgramChange 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [176] = 0x01;
			expectedStruct2b [178] = 0x02;
			AreEqual (expectedStruct2b, GetData (p), "ProgramChange 2b");

			// Set back to default value
			p.ProgramChange = default (MidiTransform);
			AreEqual (default (MidiTransform), p.ProgramChange, "ProgramChange 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "ProgramChange 3b");
		}

		[Test]
		public void PropertiesTest_PitchBend ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.PitchBend, Is.EqualTo (default (MidiTransform)), "PitchBend 1");

			// Set to some value
			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			p.PitchBend = someMidiTransformType;
			AreEqual (someMidiTransformType, p.PitchBend, "PitchBend 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [180] = 0x01;
			expectedStruct2b [182] = 0x02;
			AreEqual (expectedStruct2b, GetData (p), "PitchBend 2b");

			// Set back to default value
			p.PitchBend = default (MidiTransform);
			AreEqual (default (MidiTransform), p.PitchBend, "PitchBend 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "PitchBend 3b");
		}

		[Test]
		public void PropertiesTest_FilterOutSysEx ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.FilterOutSysEx, Is.EqualTo (false), "FilterOutSysEx 1");

			// Set to some value
			p.FilterOutSysEx = true;
			Assert.That (p.FilterOutSysEx, Is.EqualTo (true), "FilterOutSysEx 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [184] = 0x01;
			AreEqual (expectedStruct2b, GetData (p), "FilterOutSysEx 2b");

			// Set back to default value
			p.FilterOutSysEx = false;
			Assert.That (p.FilterOutSysEx, Is.EqualTo (false), "FilterOutSysEx 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "FilterOutSysEx 3b");
		}

		[Test]
		public void PropertiesTest_FilterOutMtc ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.FilterOutMtc, Is.EqualTo (false), "FilterOutMtc 1");

			// Set to some value
			p.FilterOutMtc = true;
			Assert.That (p.FilterOutMtc, Is.EqualTo (true), "FilterOutMtc 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [185] = 0x01;
			AreEqual (expectedStruct2b, GetData (p), "FilterOutMtc 2b");

			// Set back to default value
			p.FilterOutMtc = false;
			Assert.That (p.FilterOutMtc, Is.EqualTo (false), "FilterOutMtc 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "FilterOutMtc 3b");
		}

		[Test]
		public void PropertiesTest_FilterOutBeatClock ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.FilterOutBeatClock, Is.EqualTo (false), "FilterOutBeatClock 1");

			// Set to some value
			p.FilterOutBeatClock = true;
			Assert.That (p.FilterOutBeatClock, Is.EqualTo (true), "FilterOutBeatClock 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [186] = 0x01;
			AreEqual (expectedStruct2b, GetData (p), "FilterOutBeatClock 2b");

			// Set back to default value
			p.FilterOutBeatClock = false;
			Assert.That (p.FilterOutBeatClock, Is.EqualTo (false), "FilterOutBeatClock 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "FilterOutBeatClock 3b");
		}

		[Test]
		public void PropertiesTest_FilterOutTuneRequest ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.FilterOutTuneRequest, Is.EqualTo (false), "FilterOutTuneRequest 1");

			// Set to some value
			p.FilterOutTuneRequest = true;
			Assert.That (p.FilterOutTuneRequest, Is.EqualTo (true), "FilterOutTuneRequest 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [187] = 0x01;
			AreEqual (expectedStruct2b, GetData (p), "FilterOutTuneRequest 2b");

			// Set back to default value
			p.FilterOutTuneRequest = false;
			Assert.That (p.FilterOutTuneRequest, Is.EqualTo (false), "FilterOutTuneRequest 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "FilterOutTuneRequest 3b");
		}

		[Test]
		public void PropertiesTest_FilterOutAllControls ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.FilterOutAllControls, Is.EqualTo (false), "FilterOutAllControls 1");

			// Set to some value
			p.FilterOutAllControls = true;
			Assert.That (p.FilterOutAllControls, Is.EqualTo (true), "FilterOutAllControls 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [191] = 0x01;
			AreEqual (expectedStruct2b, GetData (p), "FilterOutAllControls 2b");

			// Set back to default value
			p.FilterOutAllControls = false;
			Assert.That (p.FilterOutAllControls, Is.EqualTo (false), "FilterOutAllControls 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "FilterOutAllControls 3b");
		}

		[Test]
		public void PropertiesTest_Controls ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.Controls, Is.Null, "Controls 1");

			// Set to some array
			var transform1 = new MidiControlTransform (MidiTransformControlType.FourteenBit /* 1 */, MidiTransformControlType.FourteenBitRpn /* 3 */, 4242, MidiTransformType.MinValue /* 10 */, short.MaxValue);
			var transform2 = new MidiControlTransform (MidiTransformControlType.SevenBitRpn /* 2 */, MidiTransformControlType.SevenBitNRpn /* 4 */, ushort.MaxValue, MidiTransformType.MaxValue /* 11 */, 136);
			var array = new MidiControlTransform [] { transform1, transform2 };
			p.Controls = array;
			Assert.That (p.Controls, Is.EqualTo (array), "Controls 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [192] = 0x02;
			Array.Resize (ref expectedStruct2b, 220);
			expectedStruct2b [204] = 0x01; // MidiTransformControlType.FourteenBit
			expectedStruct2b [205] = 0x03; // MidiTransformControlType.FourteenBitRpn
			expectedStruct2b [206] = 0x92; expectedStruct2b [207] = 0x10; // 4242
			expectedStruct2b [208] = 0x0a; expectedStruct2b [209] = 0x00; // MidiTransformType.MinValue
			expectedStruct2b [210] = 0xFF; expectedStruct2b [211] = 0x7f; // short.MaxValue
			expectedStruct2b [212] = 0x02; // MidiTransformControlType.SevenBitRpn
			expectedStruct2b [213] = 0x04; // MidiTransformControlType.SevenBitNRpn
			expectedStruct2b [214] = 0xFF; expectedStruct2b [215] = 0xFF; // ushort.MaxValue
			expectedStruct2b [216] = 0x0b; expectedStruct2b [217] = 0x00; // MidiTransformType.MaxValue
			expectedStruct2b [218] = 0x88; expectedStruct2b [219] = 0x00; // 136
			AreEqual (expectedStruct2b, GetData (p), "Controls 2b");

			// Set back to default value
			p.Controls = null;
			Assert.That (p.Controls, Is.Null, "Controls 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "Controls 3b");

			// Set to a big array; the field with the number of controls is a UInt16, so overflow by one
			var toobigArray = new MidiControlTransform [1 + (int) ushort.MaxValue];
			var ex = Assert.Throws<ArgumentOutOfRangeException> (() => { p.Controls = toobigArray; }, "Controls 4");
			AssertAreEqualWithAnySuffix ($"A maximum of {ushort.MaxValue} controls are allowed", ex?.Message, "Controls 4b");

			// Set to the maximum sized array; the field with the number of maps is a UInt16, so create exactly that
			var bigArray = new MidiControlTransform [ushort.MaxValue];
			p.Controls = bigArray;
			Assert.That (p.Controls, Is.EqualTo (bigArray), "Controls 5");
			var expectedStruct5b = DefaultStruct;
			expectedStruct5b [192] = 0xFF; expectedStruct5b [193] = 0xFF; // ushort.MaxValue
			Array.Resize (ref expectedStruct5b, 524484);
			AreEqual (expectedStruct5b, GetData (p), "Controls 5b");
		}

		[Test]
		public void PropertiesTest_Maps ()
		{
			var p = new MidiThruConnectionParams ();
			Assert.That (p.Maps, Is.Null, "Maps 1");

			// Set to some array
			var valueMap1 = new byte [128];
			valueMap1 [42] = 36;
			var valueMap2 = new byte [128];
			valueMap2 [124] = 136;
			var array = new MidiValueMap [] { new MidiValueMap { Value = valueMap1 }, new MidiValueMap { Value = valueMap2 } };
			p.Maps = array;
			Assert.That (p.Maps, Is.EqualTo (array), "Maps 2");
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [194] = 2;
			Array.Resize (ref expectedStruct2b, 460);
			expectedStruct2b [246] = 36;
			expectedStruct2b [456] = 136;
			AreEqual (expectedStruct2b, GetData (p), "Maps 2b");

			// Set back to default value
			p.Maps = null;
			Assert.That (p.Maps, Is.Null, "Maps 3");
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "Maps 3b");

			// Set to a too big array; the field with the number of maps is a UInt16, so overflow by one
			var toobigArray = new MidiValueMap [1 + (int) ushort.MaxValue];
			var ex = Assert.Throws<ArgumentOutOfRangeException> (() => { p.Maps = toobigArray; }, "Maps 4");
			AssertAreEqualWithAnySuffix ($"A maximum of {ushort.MaxValue} maps are allowed", ex?.Message, "Maps 4b");

			// Set to the maximum sized array; the field with the number of maps is a UInt16, so create exactly that
			var bigArray = new MidiValueMap [ushort.MaxValue];
			p.Maps = bigArray;
			Assert.That (p.Maps, Is.EqualTo (bigArray), "Maps 5");
			var expectedStruct5b = DefaultStruct;
			expectedStruct5b [194] = 0xff; expectedStruct5b [195] = 0xff; // ushort.MaxValue
			Array.Resize (ref expectedStruct5b, 8388684);
			AreEqual (expectedStruct5b, GetData (p), "Maps 5b");
		}

		[Test]
		public void ReadStructTest_Default ()
		{
			var p = SetData (DefaultStruct);
			Assert.That (GetData (p), Is.EqualTo (DefaultStruct), "Default");
		}

		[Test]
		public void ReadStructTest_Sources ()
		{
			// Set to some array
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [4] = 2;
			expectedStruct2b [16] = 3;
			expectedStruct2b [20] = 4;
			var p = SetData (expectedStruct2b);

			var array = new MidiThruConnectionEndpoint [] { new MidiThruConnectionEndpoint (0, 0), new MidiThruConnectionEndpoint (3, 4) };
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct Sources 2b");
			AreEqual (array, p.Sources, "ReadStruct Sources 2");
		}

		[Test]
		public void ReadStructTest_Destinations ()
		{
			// Set to some array
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [72] = 2;
			expectedStruct2b [84] = 3;
			expectedStruct2b [88] = 4;
			var p = SetData (expectedStruct2b);

			var array = new MidiThruConnectionEndpoint [] { new MidiThruConnectionEndpoint (0, 0), new MidiThruConnectionEndpoint (3, 4) };
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct Destinations 2b");
			AreEqual (array, p.Destinations, "ReadStruct Destinations 2");
		}

		[Test]
		public void ReadStructTest_ChannelMap ()
		{
			// Set to some array
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [140] = 0x01;
			expectedStruct2b [141] = 0x02;
			expectedStruct2b [142] = 0x00;
			expectedStruct2b [143] = 0x00;
			expectedStruct2b [144] = 0x00;
			expectedStruct2b [145] = 0x00;
			expectedStruct2b [146] = 0x00;
			expectedStruct2b [147] = 0x00;
			expectedStruct2b [148] = 0x00;
			expectedStruct2b [149] = 0x00;
			expectedStruct2b [150] = 0x00;
			expectedStruct2b [151] = 0x00;
			expectedStruct2b [152] = 0x00;
			expectedStruct2b [153] = 0x00;
			expectedStruct2b [154] = 0x00;
			expectedStruct2b [155] = 0x00;
			var p = SetData (expectedStruct2b);

			var array = new byte [] { 1, 2 };
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct ChannelMap 2b");
			var expectedChannelMap2 = new byte [16];
			expectedChannelMap2 [0] = 1;
			expectedChannelMap2 [1] = 2;
			AreEqual (expectedChannelMap2, p.ChannelMap, "ReadStruct ChannelMap 2");
		}

		[Test]
		public void ReadStructTest_LowVelocity ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [156] = 42;
			var p = SetData (expectedStruct2b);

			Assert.That (p.LowVelocity, Is.EqualTo (42), "ReadStruct LowVelocity 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct LowVelocity 2b");
		}

		[Test]
		public void ReadStructTest_HighVelocity ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [157] = 42;
			var p = SetData (expectedStruct2b);

			Assert.That (p.HighVelocity, Is.EqualTo (42), "ReadStruct HighVelocity 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct HighVelocity 2b");
		}

		[Test]
		public void ReadStructTest_LowNote ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [158] = 42;
			var p = SetData (expectedStruct2b);

			Assert.That (p.LowNote, Is.EqualTo (42), "ReadStruct LowNote 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct LowNote 2b");
		}

		[Test]
		public void ReadStructTest_HighNode ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [159] = 42;
			var p = SetData (expectedStruct2b);

			Assert.That (p.HighNote, Is.EqualTo (42), "ReadStruct HighNote 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct HighNote 2b");
		}

		[Test]
		public void ReadStructTest_NoteNumber ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [160] = 0x01;
			expectedStruct2b [162] = 0x02;
			var p = SetData (expectedStruct2b);

			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			AreEqual (someMidiTransformType, p.NoteNumber, "ReadStruct NoteNumber 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct NoteNumber 2b");
		}

		[Test]
		public void ReadStructTest_Velocity ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [164] = 0x01;
			expectedStruct2b [166] = 0x02;
			var p = SetData (expectedStruct2b);

			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			AreEqual (someMidiTransformType, p.Velocity, "ReadStruct Velocity 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct Velocity 2b");
		}

		[Test]
		public void ReadStructTest_KeyPressure ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [168] = 0x01;
			expectedStruct2b [170] = 0x02;
			var p = SetData (expectedStruct2b);

			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			AreEqual (someMidiTransformType, p.KeyPressure, "ReadStruct KeyPressure 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct KeyPressure 2b");
		}

		[Test]
		public void ReadStructTest_ChannelPressure ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [172] = 0x01;
			expectedStruct2b [174] = 0x02;
			var p = SetData (expectedStruct2b);

			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			AreEqual (someMidiTransformType, p.ChannelPressure, "ReadStruct ChannelPressure 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct ChannelPressure 2b");
		}

		[Test]
		public void ReadStructTest_ProgramChange ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [176] = 0x01;
			expectedStruct2b [178] = 0x02;
			var p = SetData (expectedStruct2b);

			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			AreEqual (someMidiTransformType, p.ProgramChange, "ReadStruct ProgramChange 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct ProgramChange 2b");
		}

		[Test]
		public void ReadStructTest_PitchBend ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [180] = 0x01;
			expectedStruct2b [182] = 0x02;
			var p = SetData (expectedStruct2b);

			var someMidiTransformType = new MidiTransform (MidiTransformType.FilterOut /* 1 */, 2);
			AreEqual (someMidiTransformType, p.PitchBend, "ReadStruct PitchBend 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct PitchBend 2b");
		}

		[Test]
		public void ReadStructTest_FilterOutSysEx ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [184] = 0x01;
			var p = SetData (expectedStruct2b);

			Assert.That (p.FilterOutSysEx, Is.EqualTo (true), "ReadStruct FilterOutSysEx 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct FilterOutSysEx 2b");
		}

		[Test]
		public void ReadStructTest_FilterOutMtc ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [185] = 0x01;
			var p = SetData (expectedStruct2b);

			Assert.That (p.FilterOutMtc, Is.EqualTo (true), "ReadStruct FilterOutMtc 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct FilterOutMtc 2b");
		}

		[Test]
		public void ReadStructTest_FilterOutBeatClock ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [186] = 0x01;
			var p = SetData (expectedStruct2b);

			Assert.That (p.FilterOutBeatClock, Is.EqualTo (true), "ReadStruct FilterOutBeatClock 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct FilterOutBeatClock 2b");
		}

		[Test]
		public void ReadStructTest_FilterOutTuneRequest ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [187] = 0x01;
			var p = SetData (expectedStruct2b);

			Assert.That (p.FilterOutTuneRequest, Is.EqualTo (true), "ReadStruct FilterOutTuneRequest 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct FilterOutTuneRequest 2b");
		}

		[Test]
		public void ReadStructTest_FilterOutAllControls ()
		{
			// Set to some value
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [191] = 0x01;
			var p = SetData (expectedStruct2b);

			Assert.That (p.FilterOutAllControls, Is.EqualTo (true), "ReadStruct FilterOutAllControls 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct FilterOutAllControls 2b");
		}

		[Test]
		public void ReadStructTest_Controls ()
		{
			// Set to some array
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [192] = 0x02;
			Array.Resize (ref expectedStruct2b, 220);
			expectedStruct2b [204] = 0x01; // MidiTransformControlType.FourteenBit
			expectedStruct2b [205] = 0x03; // MidiTransformControlType.FourteenBitRpn
			expectedStruct2b [206] = 0x92; expectedStruct2b [207] = 0x10; // 4242
			expectedStruct2b [208] = 0x0a; expectedStruct2b [209] = 0x00; // MidiTransformType.MinValue
			expectedStruct2b [210] = 0xFF; expectedStruct2b [211] = 0x7f; // short.MaxValue
			expectedStruct2b [212] = 0x02; // MidiTransformControlType.SevenBitRpn
			expectedStruct2b [213] = 0x04; // MidiTransformControlType.SevenBitNRpn
			expectedStruct2b [214] = 0xFF; expectedStruct2b [215] = 0xFF; // ushort.MaxValue
			expectedStruct2b [216] = 0x0b; expectedStruct2b [217] = 0x00; // MidiTransformType.MaxValue
			expectedStruct2b [218] = 0x88; expectedStruct2b [219] = 0x00; // 136

			var p = SetData (expectedStruct2b);

			var transform1 = new MidiControlTransform (MidiTransformControlType.FourteenBit /* 1 */, MidiTransformControlType.FourteenBitRpn /* 3 */, 4242, MidiTransformType.MinValue /* 10 */, short.MaxValue);
			var transform2 = new MidiControlTransform (MidiTransformControlType.SevenBitRpn /* 2 */, MidiTransformControlType.SevenBitNRpn /* 4 */, ushort.MaxValue, MidiTransformType.MaxValue /* 11 */, 136);
			var array = new MidiControlTransform [] { transform1, transform2 };
			Assert.That (p.Controls, Is.EqualTo (array), "ReadStruct Controls 2");

			AreEqual (expectedStruct2b, GetData (p), "ReadStruct Controls 2b");

			// Set to the maximum sized array; the field with the number of maps is a UInt16, so create exactly that
			var expectedStruct5b = DefaultStruct;
			expectedStruct5b [192] = 0xFF; expectedStruct5b [193] = 0xFF; // ushort.MaxValue
			Array.Resize (ref expectedStruct5b, 524484);

			p = SetData (expectedStruct5b);

			var bigArray = new MidiControlTransform [ushort.MaxValue];
			Assert.That (p.Controls, Is.EqualTo (bigArray), "ReadStruct Controls 5");
			AreEqual (expectedStruct5b, GetData (p), "ReadStruct Controls 5b");
		}

		[Test]
		public void ReadStructTest_Maps ()
		{
			// Set to some array
			var expectedStruct2b = DefaultStruct;
			expectedStruct2b [194] = 2;
			Array.Resize (ref expectedStruct2b, 460);
			expectedStruct2b [246] = 36;
			expectedStruct2b [456] = 136;
			var p = SetData (expectedStruct2b);

			var valueMap1 = new byte [128];
			valueMap1 [42] = 36;
			var valueMap2 = new byte [128];
			valueMap2 [124] = 136;
			var array = new MidiValueMap [] { new MidiValueMap { Value = valueMap1 }, new MidiValueMap { Value = valueMap2 } };
			AreEqual (array, p.Maps, "ReadStruct Maps 2");
			AreEqual (expectedStruct2b, GetData (p), "ReadStruct Maps 2b");

			// Set to the maximum sized array; the field with the number of maps is a UInt16, so create exactly that
			var expectedStruct5b = DefaultStruct;
			expectedStruct5b [194] = 0xff; expectedStruct5b [195] = 0xff; // ushort.MaxValue
			Array.Resize (ref expectedStruct5b, 8388684);

			p = SetData (expectedStruct5b);

			var bigArray = new MidiValueMap [ushort.MaxValue];
			AreEqual (bigArray, p.Maps, "ReadStruct Maps 5");
			AreEqual (expectedStruct5b, GetData (p), "ReadStruct Maps 5b");
		}

		static void AreEqual (MidiThruConnectionEndpoint [] expected, MidiThruConnectionEndpoint [] actual, string message)
		{
			if (expected is null && actual is null)
				return;
			Assert.That (actual.Length, Is.EqualTo (expected.Length), $"Length: {message}");
			for (var i = 0; i < expected.Length; i++) {
				AreEqual (expected [i], actual [i], $"Item[{i}]: {message}");
			}
		}

		static void AreEqual (MidiThruConnectionEndpoint expected, MidiThruConnectionEndpoint actual, string message)
		{
			if (expected.UniqueID == actual.UniqueID && expected.EndpointRef == actual.EndpointRef)
				return;
			Assert.Fail ($"{message}\n" +
						 $"Expected: [MidiThruConnectionEndpoint (EndpointRef = {expected.EndpointRef}; UniqueID = {expected.UniqueID}\n" +
						 $"Actual: [MidiThruConnectionEndpoint (EndpointRef = {actual.EndpointRef}; UniqueID = {actual.UniqueID}");
		}

		static void AreEqual (MidiTransform expected, MidiTransform actual, string message)
		{
			if (expected.Param == actual.Param && expected.Transform == actual.Transform)
				return;
			Assert.Fail ($"{message}\n" +
						 $"Expected: [MidiTransform (Transform = {expected.Transform}; Param = {expected.Param}\n" +
						 $"Actual: [MidiTransform (Transform = {actual.Transform}; Param = {actual.Param}");
		}

		static void AreEqual (MidiValueMap [] expected, MidiValueMap [] actual, string message)
		{
			if (expected is null && actual is null)
				return;
			Assert.That (actual.Length, Is.EqualTo (expected.Length), $"Length: {message}");
			for (var i = 0; i < expected.Length; i++) {
				AreEqual (expected [i].Value, actual [i].Value, $"Item[{i}]: {message}");
			}
		}

		static void AreEqual (byte [] expected, byte [] actual, string message)
		{
			if (expected is null && actual is null)
				return;
			if (expected is not null && actual is not null) {
				if (expected.Length == actual.Length) {
					var equal = true;
					for (var i = 0; i < expected.Length; i++) {
						if (expected [i] != actual [i]) {
							equal = false;
							break;
						}
					}
					if (equal)
						return;
				}
			}
			if (expected is null ^ actual is null) {
				Assert.Fail ($"{message}\n" +
							 $"Expected: {(expected is null ? "is null" : "is not null")}\n" +
							 $"Actual:   {(actual is null ? "is null" : "is not null")}\n");
				return;
			}

			var expectedString = string.Join (", ", expected.Take (1024).Select (v => $"0x{v:x2}"));
			var actualString = string.Join (", ", actual.Take (1024).Select (v => $"0x{v:x2}"));
			var failureCount = 0;
			for (var i = 0; i < Math.Max (expected.Length, actual.Length) && failureCount < 100; i++) {
				if (i >= expected.Length) {
					actualString += $"\n\texpected[{i}] = out of range; actual[{i}] = 0x{actual [i]:x2}";
					failureCount++;
				} else if (i >= actual.Length) {
					actualString += $"\n\texpected[{i}] = 0x{expected [i]:x2}; actual[{i}] = out of range";
					failureCount++;
				} else if (actual [i] != expected [i]) {
					actualString += $"\n\texpected[{i}] = 0x{expected [i]:x2}; actual[{i}] = 0x{actual [i]:x2}";
					failureCount++;
				}
			}
			if (expected.Length != actual.Length) {
				Assert.Fail ($"{message}\n" +
							 $"Expected: Length = {expected.Length} {expectedString}\n" +
							 $"Actual:   Length = {actual.Length} {actualString}\n");
				return;
			}

			Assert.Fail ($"{message}\n" +
						 $"Expected: {expectedString}\n" +
						 $"Actual:   {actualString}\n");
		}

		byte [] GetData (MidiThruConnectionParams p)
		{
			var obj = typeof (MidiThruConnectionParams).GetMethod ("WriteStruct", BindingFlags.NonPublic | BindingFlags.Instance).Invoke (p, new object [] { });
			var data = (NSData) obj;
			return data.ToArray ();
		}

		MidiThruConnectionParams SetData (byte [] data)
		{
			object boxedParams = new MidiThruConnectionParams ();
			using var obj = NSData.FromArray (data);
			typeof (MidiThruConnectionParams).GetMethod ("ReadStruct", BindingFlags.NonPublic | BindingFlags.Instance).Invoke (boxedParams, new object [] { obj });
			return (MidiThruConnectionParams) boxedParams;
		}

		[Test]
		public void MidiValueMapTest ()
		{
			var map = default (MidiValueMap);
			Assert.That (map.Value, Is.EqualTo (new byte [128]), "Default");

			var bytes = new byte [42];
			var ex = Assert.Throws<ArgumentOutOfRangeException> (() => { map.Value = bytes; }, "Invalid byte array");
			AssertAreEqualWithAnySuffix ($"The length of the Value array must be 128", ex.Message, "Ex Message");

			bytes = new byte [128];
			bytes [42] = 36;
			map.Value = bytes;
			Assert.That (map.Value, Is.EqualTo (bytes), "Bytes");
		}
	}
}
#endif // !__TVOS__
