using System;
using System.Runtime.InteropServices;

[StructLayout (LayoutKind.Sequential)]
public struct ElaboratedNameOnlyStruct {
	public int foo;
}

[StructLayout (LayoutKind.Sequential)]
public struct AnonStructRenamedByTypedef {
	public int foo;
}

[StructLayout (LayoutKind.Sequential)]
public struct ElaboratedNameStructRenamedByTypedef {
	public int foo;
}

[StructLayout (LayoutKind.Sequential)]
public struct Parent {
	public int fully_anon_struct;
}

[StructLayout (LayoutKind.Sequential)]
public struct Parent {
	public sbyte first;

	[StructLayout (LayoutKind.Sequential)]
	public struct ChildStruct {
		public int a;

		public sbyte b;
	}


	public ChildStruct second;

	public nuint third;

	[StructLayout (LayoutKind.Sequential)]
	public struct AnotherChild {
		public int foo;

		[StructLayout (LayoutKind.Sequential)]
		public struct AndAnotherChild {
			public unsafe sbyte* name;

			public int age;
		}


		public AndAnotherChild andAnotherChild;

		public int bar;
	}


	public AnotherChild anotherChild;
}

[StructLayout (LayoutKind.Sequential)]
public struct NXTabletPointData {
	public int x;

	public int y;

	public int z;

	public ushort buttons;

	public ushort pressure;

	[StructLayout (LayoutKind.Sequential)]
	public struct tilt {
		public short x;

		public short y;
	}


	public tilt tilt;

	public ushort rotation;

	public short tangentialPressure;

	public ushort deviceID;

	public short vendor1;

	public short vendor2;

	public short vendor3;
}

[StructLayout (LayoutKind.Sequential)]
public struct NXTabletProximityData {
	public ushort vendorID;

	public ushort tabletID;

	public ushort pointerID;

	public ushort deviceID;

	public ushort systemTabletID;

	public ushort vendorPointerType;

	public uint pointerSerialNumber;

	public ulong uniqueID;

	public uint capabilityMask;

	public byte pointerType;

	public byte enterProximity;

	public short reserved1;
}

[StructLayout (LayoutKind.Explicit)]
public struct NXEventData {
	[StructLayout (LayoutKind.Sequential)]
	public struct mouse {
		public byte subx;

		public byte suby;

		public short eventNum;

		public int click;

		public byte pressure;

		public byte buttonNumber;

		public byte subType;

		public byte reserved2;

		public int reserved3;

		[StructLayout (LayoutKind.Explicit)]
		public struct tablet {
			[FieldOffset (0)]
			public NXTabletPointData point;

			[FieldOffset (0)]
			public NXTabletProximityData proximity;
		}


		public tablet tablet;
	}


	[FieldOffset (0)]
	public mouse mouse;

	[StructLayout (LayoutKind.Sequential)]
	public struct mouseMove {
		public int dx;

		public int dy;

		public byte subx;

		public byte suby;

		public byte subType;

		public byte reserved1;

		public int reserved2;

		[StructLayout (LayoutKind.Explicit)]
		public struct tablet {
			[FieldOffset (0)]
			public NXTabletPointData point;

			[FieldOffset (0)]
			public NXTabletProximityData proximity;
		}


		public tablet tablet;
	}


	[FieldOffset (0)]
	public mouseMove mouseMove;

	[StructLayout (LayoutKind.Sequential)]
	public struct key {
		public ushort origCharSet;

		public short repeat;

		public ushort charSet;

		public ushort charCode;

		public ushort keyCode;

		public ushort origCharCode;

		public int reserved1;

		public uint keyboardType;

		public int reserved2;

		public int reserved3;

		public int reserved4;

		public int [] reserved5;
	}


	[FieldOffset (0)]
	public key key;

	[StructLayout (LayoutKind.Sequential)]
	public struct tracking {
		public short reserved;

		public short eventNum;

		public int trackingNum;

		public int userData;

		public int reserved1;

		public int reserved2;

		public int reserved3;

		public int reserved4;

		public int reserved5;

		public int [] reserved6;
	}


	[FieldOffset (0)]
	public tracking tracking;

	[StructLayout (LayoutKind.Sequential)]
	public struct scrollWheel {
		public short deltaAxis1;

		public short deltaAxis2;

		public short deltaAxis3;

		public short reserved1;

		public int fixedDeltaAxis1;

		public int fixedDeltaAxis2;

		public int fixedDeltaAxis3;

		public int pointDeltaAxis1;

		public int pointDeltaAxis2;

		public int pointDeltaAxis3;

		public int [] reserved8;
	}


	[FieldOffset (0)]
	public scrollWheel scrollWheel;

	[FieldOffset (0)]
	public scrollWheel zoom;

	[StructLayout (LayoutKind.Sequential)]
	public struct compound {
		public short reserved;

		public short subType;

		[StructLayout (LayoutKind.Explicit)]
		public struct misc {
			[FieldOffset (0)]
			public float [] F;

			[FieldOffset (0)]
			public int [] L;

			[FieldOffset (0)]
			public short [] S;

			[FieldOffset (0)]
			public sbyte [] C;
		}


		public misc misc;
	}


	[FieldOffset (0)]
	public compound compound;

	[StructLayout (LayoutKind.Sequential)]
	public struct tablet {
		public int x;

		public int y;

		public int z;

		public ushort buttons;

		public ushort pressure;

		[StructLayout (LayoutKind.Sequential)]
		public struct tilt {
			public short x;

			public short y;
		}


		public tilt tilt;

		public ushort rotation;

		public short tangentialPressure;

		public ushort deviceID;

		public short vendor1;

		public short vendor2;

		public short vendor3;

		public int [] reserved;
	}


	[FieldOffset (0)]
	public tablet tablet;

	[StructLayout (LayoutKind.Sequential)]
	public struct proximity {
		public ushort vendorID;

		public ushort tabletID;

		public ushort pointerID;

		public ushort deviceID;

		public ushort systemTabletID;

		public ushort vendorPointerType;

		public uint pointerSerialNumber;

		public ulong uniqueID;

		public uint capabilityMask;

		public byte pointerType;

		public byte enterProximity;

		public short reserved1;

		public int [] reserved2;
	}


	[FieldOffset (0)]
	public proximity proximity;
}

[StructLayout (LayoutKind.Sequential)]
public struct BluetoothHCIEventReturnLinkKeysResults {
	public int numLinkKeys;

	[StructLayout (LayoutKind.Sequential)]
	public struct linkKeys {
		public int deviceAddress;

		public int linkKey;
	}


	public linkKeys [] linkKeys;
}
