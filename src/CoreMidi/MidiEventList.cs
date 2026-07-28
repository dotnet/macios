// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Foundation;
using ObjCRuntime;

using MidiEndpointRef = System.Int32;
using MidiPortRef = System.Int32;

#nullable enable

namespace CoreMidi {
	/// <summary>This class represents the Objective-C struct MIDIEventList, which is a list of <see cref="MidiEventPacket" /> packets.</summary>
	[SupportedOSPlatform ("ios14.0")]
	[SupportedOSPlatform ("tvos15.0")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst")]
	// [NativeName ("MIDIEventList")]
	public sealed class MidiEventList : IEnumerable<MidiEventPacket>, IDisposable {
		/* This is a variable sized struct, so store all the data in a byte array.
		 * struct MIDIEventList
		 * {
		 *    MIDIProtocolID protocol;
		 *    UInt32 numPackets;
		 *    MIDIEventPacket packet[1];
		 * };
		 */

		// this struct is just used internally to avoid some manual pointer math
		struct MIDIEventList {
#pragma warning disable CS0649 // Field '...' is never assigned to, and will always have its default value
#pragma warning disable CS0169 // The field '...' is never used
			internal MidiProtocolId protocol;
			internal uint numPackets;
			internal MidiEventPacket packet;
#pragma warning restore CS0169
#pragma warning restore CS0649
		}

		unsafe MIDIEventList* midiDataPointer;
		int midiDataSize;
		bool owns;
		unsafe MidiEventPacket* currentPacket;

		const int MinimumSize = 276; /* 4 + 4 + sizeof (MidiEventPacket) */

		/// <summary>The <see cref="MidiProtocolId" /> protocol for the packets in this list of packets.</summary>
		/// <returns>The <see cref="MidiProtocolId" /> protocol for the packets in this list of packets.</returns>
		public unsafe MidiProtocolId Protocol {
			get {
				return midiDataPointer->protocol;
			}
		}

		/// <summary>The number of packets in this list.</summary>
		/// <returns>The number of packets in this list.</returns>
		public unsafe uint PacketCount {
			get {
				return midiDataPointer->numPackets;
			}
		}

		unsafe internal void* MidiData { get => midiDataPointer; }

		/// <summary>Create a new <see cref="MidiEventList" /> list with the minimum size.</summary>
		/// <param name="protocol">The protocol for the packets in the created list.</param>
		/// <returns>A newly created <see cref="MidiEventList" />, or an exception in case of failure.</returns>
		public MidiEventList (MidiProtocolId protocol)
			: this (protocol, MinimumSize)
		{
		}

		/// <summary>Create a new <see cref="MidiEventList" /> for the specified protocol and size.</summary>
		/// <param name="protocol">The protocol for the event list.</param>
		/// <param name="size">The size, in number of bytes, of the event list. Minimum size is 276 bytes.</param>
		/// <returns>A newly created <see cref="MidiEventList" />, or an exception in case of failure.</returns>
		public MidiEventList (MidiProtocolId protocol, int size)
		{
			if (size < MinimumSize)
				throw new ArgumentOutOfRangeException (nameof (size), $"{nameof (size)} must be at least {MinimumSize}.");

			midiDataSize = size;
			owns = true;
			unsafe {
				midiDataPointer = (MIDIEventList*) Marshal.AllocHGlobal (midiDataSize);
				currentPacket = MIDIEventListInit (midiDataPointer, protocol);
				if (currentPacket is null) {
					Marshal.FreeHGlobal ((IntPtr) midiDataPointer);
					midiDataPointer = null;
					throw new Exception ($"Failed to create midi event list.");
				}
			}
		}

		/// <summary>Create a new <see cref="MidiEventList" /> for a given block of memory.</summary>
		/// <param name="eventListPointer">A pointer to a block of memory with the event list.</param>
		/// <returns>A newly created <see cref="MidiEventList" />, or an exception in case of failure.</returns>
		public MidiEventList (IntPtr eventListPointer)
		{
			if (eventListPointer == IntPtr.Zero)
				throw new ArgumentOutOfRangeException (nameof (eventListPointer));

			unsafe {
				midiDataPointer = (MIDIEventList*) eventListPointer;
				owns = false;
				midiDataSize = -1;
			}
		}

		/// <summary>Releases the resources associated with this <see cref="MidiEventList" />.</summary>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void Dispose (bool disposing)
		{
			if (owns) {
				unsafe {
					Marshal.FreeHGlobal ((IntPtr) midiDataPointer);
					midiDataPointer = null;
				}
			}
		}

		~MidiEventList ()
		{
			Dispose (false);
		}

#if !__TVOS__
		/// <summary>Send the packets in this list to the specified <paramref name="destination" />.</summary>
		/// <param name="port">The port through which the packets are sent.</param>
		/// <param name="destination">The destination where the packets are sent.</param>
		/// <returns>A non-zero error code in case of failure, otherwise zero (which indicates success).</returns>
		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe int /* OSStatus */ Send (MidiPort port, MidiEndpoint destination)
		{
			var rv = MIDISendEventList (port.Handle, destination.Handle, midiDataPointer);
			GC.KeepAlive (port);
			GC.KeepAlive (destination);
			return rv;
		}

		/// <summary>Distribute the packets from the specified <paramref name="source" />.</summary>
		/// <param name="source">The endpoint where the packates come from.</param>
		/// <returns>A non-zero error code in case of failure, otherwise zero (which indicates success).</returns>
		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe int /* OSStatus */ Receive (MidiEndpoint source)
		{
			var rv = MIDIReceivedEventList (source.Handle, midiDataPointer);
			GC.KeepAlive (source);
			return rv;
		}
#endif

		/// <summary>Add a new <see cref="MidiEventPacket" /> to this lis.</summary>
		/// <param name="time">The timestamp for the new packet.</param>
		/// <param name="words">The data for the midi event to add.</param>
		/// <returns>True if successful, otherwise false (which typically means there's not enough space for the new packet).</returns>
		public unsafe bool Add (ulong time, uint [] words)
		{
			if (midiDataSize < 0)
				throw new InvalidOperationException ($"Can't add to a MidiEventList initialized from a raw pointer.");

			ArgumentNullException.ThrowIfNull (words);

			fixed (uint* wordsPtr = words) {
				var rv = MIDIEventListAdd (midiDataPointer, (ulong) midiDataSize, currentPacket, time, (ulong) words.Length, wordsPtr);
				if (rv is not null) {
					currentPacket = rv;
					return true;
				}
				return false;
			}
		}

		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern MidiEventPacket* MIDIEventListInit (MIDIEventList* evtlist, MidiProtocolId /* MIDIProtocolID */ protocol);

		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern MidiEventPacket* MIDIEventListAdd (
			MIDIEventList* evtlist,
			ulong /* ByteCount = unsigned long */ listSize,
			MidiEventPacket* curPacket,
			ulong /* MIDITimeStamp */ time,
			ulong /* ByteCount = unsigned long */ wordCount,
			uint* /* const UInt32 * */ words);

#if !__TVOS__
		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern int /* OSStatus */ MIDISendEventList (MidiPortRef port, MidiEndpointRef dest, MIDIEventList* evtList);

		[SupportedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern int /* OSStatus */ MIDIReceivedEventList (MidiEndpointRef src, MIDIEventList* evtlist);
#endif // !__TVOS__

		IEnumerator<MidiEventPacket> IEnumerable<MidiEventPacket>.GetEnumerator ()
		{
			MidiEventPacket packetToYield;
			IntPtr packetPtr;

			if (PacketCount == 0)
				yield break;

			unsafe {
				MidiEventPacket* packet = &midiDataPointer->packet;
				packetToYield = *packet;
				packetPtr = (IntPtr) packet;
			}
			yield return packetToYield;

			for (var i = 1; i < PacketCount; i++) {
				unsafe {
					MidiEventPacket* packet = (MidiEventPacket*) packetPtr;
					uint* wordPointer = &packet->word_00;
					packet = (MidiEventPacket*) (wordPointer + packet->WordCount);
					packetToYield = *packet;
					packetPtr = (IntPtr) packet;
				}
				yield return packetToYield;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<MidiEventPacket>) this).GetEnumerator ();
		}

		/// <summary>Iterate over each <see cref="MidiEventPacket" /> in this list without allocating or copying memory.</summary>
		/// <param name="callback">The function to call for each packet.</param>
		public unsafe void Iterate (MidiEventListIterator callback)
		{
			if (PacketCount == 0)
				return;

			MidiEventPacket* packet = &midiDataPointer->packet;
			callback (ref Unsafe.AsRef<MidiEventPacket> (packet));
			for (var i = 1; i < PacketCount; i++) {
				uint* wordPointer = &packet->word_00;
				packet = (MidiEventPacket*) (wordPointer + packet->WordCount);
				callback (ref Unsafe.AsRef<MidiEventPacket> (packet));
			}
		}

		/// <summary>Parse each Universal MIDI Packet (UMP) in this list, invoking the specified <paramref name="visitor" /> for each parsed message.</summary>
		/// <param name="visitor">The function to call for each parsed message. Unknown messages are reported with their raw words.</param>
		/// <remarks>This is a binding for the native <c>MIDIEventListForEachEvent</c> function, which parses each UMP and fills a <see cref="MidiUniversalMessage" /> struct.</remarks>
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public unsafe void ForEachEvent (MidiUniversalMessageVisitor visitor)
		{
			ArgumentNullException.ThrowIfNull (visitor);

			var gch = GCHandle.Alloc (visitor);
			try {
				MIDIEventListForEachEvent (midiDataPointer, &TrampolineForEachEvent, (void*) GCHandle.ToIntPtr (gch));
			} finally {
				gch.Free ();
			}
		}

		[UnmanagedCallersOnly]
		static unsafe void TrampolineForEachEvent (void* context, ulong timeStamp, MidiUniversalMessage message)
		{
			var gch = GCHandle.FromIntPtr ((IntPtr) context);
			if (gch.Target is MidiUniversalMessageVisitor visitor)
				visitor (timeStamp, message);
		}

		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern void MIDIEventListForEachEvent (MIDIEventList* evtlist, delegate* unmanaged<void*, ulong, MidiUniversalMessage, void> visitor, void* visitorContext);
	}

	/// <summary>The delegate type used by <see cref="MidiEventList.ForEachEvent" />.</summary>
	/// <param name="timeStamp">The timestamp of the parsed message.</param>
	/// <param name="message">The parsed message.</param>
	public delegate void MidiUniversalMessageVisitor (ulong timeStamp, MidiUniversalMessage message);

	/// <summary>The delegate type used by <see cref="MidiEventList.Iterate" />.</summary>
	/// <param name="packet">The current packet found when iterating.</param>
	public delegate void MidiEventListIterator (ref MidiEventPacket packet);
}
