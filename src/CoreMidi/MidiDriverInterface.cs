// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if !__TVOS__

using System.Runtime.InteropServices;

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

namespace CoreMidi {
	public class MidiDriver {
#if !COREBUILD
		MidiDriverInterface driverInterface = default;

		internal MidiDriverInterface DriverInterface { get => driverInterface; }

		internal MidiDriver (MidiDriverInterface iface)
		{
			driverInterface = iface;
		}

		[DllImport (Constants.CoreMidiLibrary)]
		unsafe static extern MidiDeviceListRef MIDIGetDriverDeviceList (MidiDriverInterface** driver);

		/// <summary>Get the devices this driver owns or created.</summary>
		/// <returns>If successful, a list of the device this driver owns or created. Otherwise null.</returns>
		public unsafe MidiDeviceList? GetDeviceList ()
		{
			fixed (MidiDriverInterface *driver = &driverInterface) {
				var rv = MIDIGetDriverDeviceList (&driver);
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
#endif // COREBUILD
	}

	struct MidiDriverInterface {
#pragma warning disable CS0649 // Field '...' is never assigned to, and will always have its default value 0
#pragma warning disable CS0169 // The field '...' is never used
		/* IUNKNOWN_C_GUTS;

	    void *_reserved; \
	    HRESULT (STDMETHODCALLTYPE *QueryInterface)(void *thisPointer, REFIID iid, LPVOID *ppv); \
	    ULONG (STDMETHODCALLTYPE *AddRef)(void *thisPointer); \
	    ULONG (STDMETHODCALLTYPE *Release)(void *thisPointer) \

		*/

	    IntPtr _reserved;
	    unsafe delegate* unmanaged<void* /* thisPointer */, CFUuidBytes /* REFIID iid */, void * /* ppv */, int /* HRESULT */> QueryInterface;
	    unsafe delegate* unmanaged<void* /* thisPointer */, uint /* ULONG */> AddRef;
	    unsafe delegate* unmanaged<void* /* thisPointer */, uint /* ULONG */ > Release;

	/*!
		@fn FindDevices
		@discussion
			This is only called for version 1 drivers.  The server is requesting that the driver
			detect the devices which are present.  For each device present, the driver should
			create a MIDIDeviceRef with entities, using MIDIDeviceCreate and
			MIDIDeviceAddEntity, and add the device to the supplied MIDIDeviceListRef, using
			MIDIDeviceListAddDevice.

			The driver should not retain any references to the created devices and entities.
	*/
	// OSStatus	(*FindDevices)(MIDIDriverRef __nonnull self, MIDIDeviceListRef devList);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiDeviceListRef /* devList */, OSStatus> FindDevices;

	/*!
		@fn Start
		@discussion
			The server is telling the driver to begin MIDI I/O.

			The provided device list contains the devices which were previously located by
			FindDevices (in the case of a version 1 driver), or the devices which are owned by
			this driver and are currently in the current MIDISetup (for version 2 drivers).

			The provided devices may or may not still be present.  A version 1 driver should
			attempt to use as many of the devices as are actually present.

			A version 2 driver may make calls such as MIDISetupAddDevice, MIDIDeviceAddEntity,
			MIDIDeviceRemoveEntity to dynamically modify the system's current state. For devices
			in the provided device list which are not present, the driver should set their
			kMIDIPropertyOffline property to 1.  A version 2 driver may also set up
			notifications when the IORegistry changes, to detect connection and disconnection of
			devices it wishes to control.  At these times also, the driver may change the
			devices' kMIDIPropertyOffline, and dynamically modify the system's current state to
			reflect the devices which are present.  When passing a CFRunLoopRef to IOKit for
			notification purposes, the driver must use the server's main runloop, which is
			obtained with CFRunLoopGetCurrent().

			The driver will probably want to iterate through the destination endpoints and
			assign their driver refCons, so as to identify multiple destinations when Send() is
			called.

			The provided device list remains owned by the system and can be assumed to contain
			only devices owned by this driver.  The driver may retain references to the devices
			in this list and any it creates while running.
	*/
	// OSStatus	(*Start)(MIDIDriverRef __nonnull self, MIDIDeviceListRef devList);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiDeviceListRef /* devList */, OSStatus> Start;

	/*!
		@fn Stop
		@discussion
			The server is telling the driver to terminate MIDI I/O.  All I/O operations that
			were begun in Start, or as a result of a subsequent IOKit notification, should be
			terminated.
	*/
	// OSStatus	(*Stop)(MIDIDriverRef __nonnull self);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, OSStatus> Stop;
	
	/*!
		@fn Configure
		@discussion
			not currently used
	*/
	// OSStatus	(*Configure)(MIDIDriverRef __nonnull self, MIDIDeviceRef device);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiDeviceRef /* device */, OSStatus> Configure;

	/*!
		@fn Send
		@discussion
			Send a MIDIPacketList to the destination endpoint whose refCons are being passed as
			arguments.
	*/
	// OSStatus	(*Send)(MIDIDriverRef __nonnull self, const MIDIPacketList *pktlist, void *destRefCon1, void *destRefCon2);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiPacketListPointer /* pktList */, void* /* destRefCon1 */, void* /* destRefCon2 */, OSStatus> Send;
	
	/*!
		@fn EnableSource
		@discussion
			A client has opened or closed a connection, and now the server is telling the driver
			that input from a particular source either does or does not have any listeners in
			the system.  The driver may use this information to decide whether to pass messages
			from the source to the server, and it may even be able to tell the source hardware
			not to generate incoming MIDI I/O for that source.
	*/
	// OSStatus	(*EnableSource)(MIDIDriverRef __nonnull self, MIDIEndpointRef src, Boolean enabled);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiEndpointRef /* src */, byte /* enabled */, OSStatus> EnableSource;
	
	/*!
		@fn Flush
		@discussion
			Only for version 2 drivers (new for CoreMIDI 1.1).

			Drivers which support schedule-ahead, when receiving this message, should unschedule
			all pending output to the specified destination.  If the destination is null/0, the
			driver should unschedule all pending output to all destinations.
	*/
	// OSStatus	(*Flush)(MIDIDriverRef __nonnull self, MIDIEndpointRef dest, void * __nullable destRefCon1, void * __nullable destRefCon2);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiEndpointRef /* dest */, void* /* destRefCon1 */, void* /* destRefCon2 */, OSStatus> Flush;

	/*!
		@fn Monitor
		@discussion
			Only for version 2 drivers (new for CoreMIDI 1.1).

			Some specialized drivers (e.g. a MIDI monitor display) may wish to intercept and
			look at all outgoing MIDI messages.  After a driver calls
			MIDIDriverEnableMonitoring(true) on itself, this function is called with the
			outgoing MIDI packets for all destinations in the system.  The Monitor function
			cannot rely on the MIDI events arriving in order, due to MIDIServer's schedule-ahead
			facilities.
	*/
	// OSStatus	(*Monitor)(MIDIDriverRef __nonnull self, MIDIEndpointRef dest, const MIDIPacketList *pktlist);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiEndpointRef /* dest */, MidiPacketListPointer /* pktList */, OSStatus> Monitor;
    
	/*!
		@fn SendPackets
		@discussion
	 		Only for version 3 drivers (new for macOS 12.0).
	 
			Send a MIDIEventList to the destination endpoint whose refCons are being passed as
			arguments.
	*/
	// OSStatus	(*SendPackets)(MIDIDriverRef __nonnull self, const MIDIEventList *pktlist, void *destRefCon1, void *destRefCon2);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiEventListPointer /* pktList */, void* /* destRefCon1 */, void* /* destRefCon2 */, OSStatus> SendPackets;
    
    /*!
		@fn MonitorEvents
		@discussion
			Only for version 3 drivers (new for macOS 12.0).

			Same as Monitor but uses MIDEventList, whose protocol may vary from MIDI 1.0.
	*/
	// OSStatus	(*MonitorEvents)(MIDIDriverRef __nonnull self, MIDIEndpointRef dest, const MIDIEventList *pktlist);
		unsafe delegate* unmanaged<MidiDriverRef /* self */, MidiEndpointRef /* dest */, MidiEventListPointer /* pktList */, OSStatus> MonitorEvents;
#pragma warning restore CS0169
#pragma warning restore CS0649
	}
}

#endif // !__TVOS__

