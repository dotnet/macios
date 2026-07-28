// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// Let's hope that by .NET 12 we've ironed out all the bugs in the API.
// This can of course be adjusted as needed (until we've released as stable).
#if NET120_0_OR_GREATER
#define STABLE_MIDIDRIVER
#endif


#if !__TVOS__

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;

using CoreFoundation;
using ObjCRuntime;

using MidiObjectRef = System.Int32;
using MidiClientRef = System.Int32;
using MidiDeviceRef = System.Int32;
using MidiDeviceListRef = System.Int32;
using MidiDriverRef = System.IntPtr;
using MidiPortRef = System.Int32;
using MidiEndpointRef = System.Int32;
using MidiEntityRef = System.Int32;

using MidiEventListPointer = System.IntPtr;
using MidiPacketListPointer = System.IntPtr;

using HRESULT = System.Int32;

namespace CoreMidi {
	/// <summary>Abstract base class for implementing custom MIDI drivers. Subclass this to create a driver that communicates with MIDI hardware.</summary>
#if !STABLE_MIDIDRIVER
	[Experimental ("APL0004")]
#endif
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public abstract class MidiDriver {
#if !COREBUILD
		unsafe MidiDriverInterface* driverInterface;

		unsafe internal MidiDriverInterface* DriverInterface { get => driverInterface; }

		unsafe protected MidiDriver ()
		{
			driverInterface = CreateDriver ();
		}

		unsafe MidiDriverInterface* CreateDriver ()
		{
			var iface = (MidiDriverInterface*) Marshal.AllocHGlobal (sizeof (MidiDriverInterface));
			iface->QueryInterface = &QueryInterface;
			iface->AddRef = &AddRef;
			iface->Release = &Release;
			iface->FindDevices = &FindDevices;
			iface->Start = &Start;
			iface->Stop = &Stop;
			iface->Configure = &Configure;
			iface->Send = &Send;
			iface->EnableSource = &EnableSource;
			iface->Flush = &Flush;
			iface->Monitor = &Monitor;
			iface->SendPackets = &SendPackets;
			iface->MonitorEvents = &MonitorEvents;
			iface->gchandle = (IntPtr) GCHandle.Alloc (this, GCHandleType.Weak);
			iface->referenceCount = 1; // managed code has one reference
			return iface;
		}

		~MidiDriver ()
		{
			Release (); // release managed code's reference
		}

		[UnmanagedCallersOnly]
		unsafe static HRESULT QueryInterface (MidiDriverInterface* self, CFUuidBytes iid, void* ppv)
		{
			var driver = self->GetObject ();
			return driver?.QueryInterface (iid, (IntPtr) ppv) ?? 0;
		}

		internal virtual HRESULT QueryInterface (CFUuidBytes iid, IntPtr ppv)
		{
			return 0;
		}

		static List<MidiDriver> strongReferences = new List<MidiDriver> ();

		[UnmanagedCallersOnly]
		unsafe static uint AddRef (MidiDriverInterface* self)
		{
			var driver = self->GetObject ();
			return driver?.AddRef () ?? 0;
		}

		unsafe internal virtual uint AddRef ()
		{
			uint referenceCount;
			lock (strongReferences) {
				referenceCount = Interlocked.Increment (ref driverInterface->referenceCount);
				if (referenceCount == 2) {
					strongReferences.Add (this);
				}
			}
			return referenceCount;
		}

		[UnmanagedCallersOnly]
		unsafe static uint Release (MidiDriverInterface* self)
		{
			var driver = self->GetObject ();
			return driver?.Release () ?? 0;
		}

		unsafe internal virtual uint Release ()
		{
			uint referenceCount = 0;
			lock (strongReferences) {
				if (driverInterface is not null) {
					referenceCount = Interlocked.Decrement (ref driverInterface->referenceCount);
					if (referenceCount == 1) {
						strongReferences.Remove (this);
					} else if (referenceCount == 0) {
						var gchandle = GCHandle.FromIntPtr (driverInterface->gchandle);
						gchandle.Free ();
						driverInterface->gchandle = IntPtr.Zero;
						Marshal.FreeHGlobal ((IntPtr) driverInterface);
						driverInterface = null;
					}
				}
			}
			return referenceCount;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus FindDevices (MidiDriverInterface* self, MidiDeviceListRef devList)
		{
			var driver = self->GetObject ();
			return driver?.FindDevices (devList) ?? 0;
		}

		/// <summary>The server requests that the driver detects any present devices. For each detected device, call <see cref="MidiDevice.Create" /> and <see cref="MidiDevice.Add(string,bool,nuint,nuint,MidiEntity)" />, and then add the device to the supplied <paramref name="deviceList" />.</summary>
		protected virtual OSStatus FindDevices (MidiDeviceListRef deviceList /* FIXME: strongly typed */)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus Start (MidiDriverInterface* self, MidiDeviceListRef devList)
		{
			var driver = self->GetObject ();
			return driver?.Start (devList) ?? 0;
		}

		/// <summary>Start MIDI I/O.</summary>
		protected virtual OSStatus Start (MidiDeviceListRef deviceList /* FIXME: strongly typed */)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus Stop (MidiDriverInterface* self)
		{
			var driver = self->GetObject ();
			return driver?.Stop () ?? 0;
		}

		/// <summary>Stop MIDI I/O.</summary>
		protected virtual OSStatus Stop ()
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus Configure (MidiDriverInterface* self, MidiDeviceRef device)
		{
			var driver = self->GetObject ();
			return driver?.Configure (device) ?? 0;
		}

		/// <summary>Not used at the moment.</summary>
		protected virtual OSStatus Configure (MidiDeviceRef device)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus Send (MidiDriverInterface* self, MidiPacketListPointer pktList, void* destRefCon1, void* destRefCon2)
		{
			var driver = self->GetObject ();
			return driver?.Send (pktList, destRefCon1, destRefCon2) ?? 0;
		}

		/// <summary>Send a MidiPacketList to the destination endpoint.</summary>
		protected unsafe virtual OSStatus Send (MidiPacketListPointer pktList, void* destRefCon1, void* destRefCon2)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus EnableSource (MidiDriverInterface* self, MidiEndpointRef src, byte enabled)
		{
			var driver = self->GetObject ();
			return driver?.EnableSource (src, enabled != 0) ?? 0;
		}

		/// <summary>Lets the driver know if a particular source has any listeners or not.</summary>
		protected unsafe virtual OSStatus EnableSource (MidiEndpointRef src, bool enabled)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus Flush (MidiDriverInterface* self, MidiEndpointRef dest, void* destRefCon1, void* destRefCon2)
		{
			var driver = self->GetObject ();
			return driver?.Flush (dest, destRefCon1, destRefCon2) ?? 0;
		}

		/// <summary>Unschedule all pending output to the specified destination endpoint (or all endpoints if null).</summary>
		protected unsafe virtual OSStatus Flush (MidiEndpointRef src, void* destRefCon1, void* destRefCon2)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus Monitor (MidiDriverInterface* self, MidiEndpointRef dest, MidiPacketListPointer pktList)
		{
			var driver = self->GetObject ();
			return driver?.Monitor (dest, pktList) ?? 0;
		}

		/// <summary>If monitoring is enabled, this method will be called with all outgoing MIDI messages.</summary>
		protected unsafe virtual OSStatus Monitor (MidiEndpointRef src, MidiPacketListPointer packetList)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus SendPackets (MidiDriverInterface* self, MidiEventListPointer pktList, void* destRefCon1, void* destRefCon2)
		{
			var driver = self->GetObject ();
			return driver?.SendPackets (pktList, destRefCon1, destRefCon2) ?? 0;
		}

		/// <summary>Send a <see cref="MidiEventList" /> to the destination endpoint.</summary>
		protected unsafe virtual OSStatus SendPackets (MidiEventListPointer pktList, void* destRefCon1, void* destRefCon2)
		{
			return 0;
		}

		[UnmanagedCallersOnly]
		unsafe static OSStatus MonitorEvents (MidiDriverInterface* self, MidiEndpointRef dest, MidiEventListPointer pktList)
		{
			var driver = self->GetObject ();
			return driver?.MonitorEvents (dest, pktList) ?? 0;
		}

		/// <summary>Same as <see cref="Monitor(MidiEndpointRef,MidiPacketListPointer)" />, but sending a <see cref="MidiEventList" /> instead of a MidiPacketList.</summary>
		protected unsafe virtual OSStatus MonitorEvents (MidiEndpointRef dest, MidiEventListPointer pktList)
		{
			return 0;
		}

		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern MidiDeviceListRef MIDIGetDriverDeviceList (MidiDriverInterface** driver);

		/// <summary>Get the devices this driver owns or created.</summary>
		/// <returns>If successful, a list of the device this driver owns or created. Otherwise null.</returns>
		public unsafe MidiDeviceList? GetDeviceList ()
		{
			fixed (MidiDriverInterface** driverInterfacePtr = &driverInterface) {
				var rv = MIDIGetDriverDeviceList (driverInterfacePtr);
				if (rv == MidiObject.InvalidRef)
					return null;
				return new MidiDeviceList (rv);
			}
		}

		[DllImport (Constants.CoreMidiLibrary)]
		static extern IntPtr /* CFRunLoopRef */ MIDIGetDriverIORunLoop ();

		/// <summary>Get the high (realtime) priority run loop that can be used for asynchronous I/O completion callbacks.</summary>
		/// <returns>If successful, the IO run loop. Otherwise null.</returns>
		public static CFRunLoop? GetIORunLoop ()
		{
			var rv = MIDIGetDriverIORunLoop ();
			if (rv == IntPtr.Zero)
				return null;
			return new CFRunLoop (rv, false);
		}

#if MONOMAC
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern OSStatus MIDIDriverEnableMonitoring (MidiDriverInterface** driver, byte enabled);

		/// <summary>A driver can call this method to receive all the outgoing MIDI packets in the system.</summary>
		/// <param name="enabled">Whether to enable or disable monitoring.</param>
		/// <returns>A status code that describes the result of the operation. This will be <see cref="MidiError.Ok" /> in case of success.</returns>
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public unsafe MidiError EnableMonitoring (bool enabled)
		{
			fixed (MidiDriverInterface **driver = &driverInterface) {
				return (MidiError) MIDIDriverEnableMonitoring (driver, enabled.AsByte ());
			}
		}
#endif // MONOMAC
#endif // COREBUILD
	}

#if !COREBUILD
#if !STABLE_MIDIDRIVER
	[Experimental ("APL0004")]
#endif
	struct MidiDriverInterface {
#pragma warning disable CS0169 // The field 'MidiDriverInterface._reserved' is never used
		IntPtr _reserved;
#pragma warning restore CS0169
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* thisPointer */, CFUuidBytes /* REFIID iid */, void* /* ppv */, int /* HRESULT */> QueryInterface;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* thisPointer */, uint /* ULONG */> AddRef;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* thisPointer */, uint /* ULONG */ > Release;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiDeviceListRef /* devList */, OSStatus> FindDevices;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiDeviceListRef /* devList */, OSStatus> Start;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, OSStatus> Stop;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiDeviceRef /* device */, OSStatus> Configure;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiPacketListPointer /* pktList */, void* /* destRefCon1 */, void* /* destRefCon2 */, OSStatus> Send;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiEndpointRef /* src */, byte /* enabled */, OSStatus> EnableSource;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiEndpointRef /* dest */, void* /* destRefCon1 */, void* /* destRefCon2 */, OSStatus> Flush;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiEndpointRef /* dest */, MidiPacketListPointer /* pktList */, OSStatus> Monitor;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiEventListPointer /* pktList */, void* /* destRefCon1 */, void* /* destRefCon2 */, OSStatus> SendPackets;
		internal unsafe delegate* unmanaged<MidiDriverInterface* /* self */, MidiEndpointRef /* dest */, MidiEventListPointer /* pktList */, OSStatus> MonitorEvents;

		internal IntPtr gchandle; // this is our own
		internal uint referenceCount; // this is our own

		internal MidiDriver? GetObject ()
		{
			var gchandle = this.gchandle;
			if (gchandle == IntPtr.Zero)
				return null;
			return (MidiDriver?) GCHandle.FromIntPtr (gchandle).Target;
		}
	}
#endif // COREBUILD
}

#endif // !__TVOS__

