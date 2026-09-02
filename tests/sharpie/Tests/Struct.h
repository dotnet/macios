// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// RUN c: -xc -std=c11
// RUN objc: -xobjective-c
// RUN cxx11: -xc++ -std=c++11

struct ElaboratedNameOnlyStruct {
	int foo;
};

typedef struct {
	int foo;
} AnonStructRenamedByTypedef;

struct ElaboratedNameStructToBeRenamedByTypedef {
	int foo;
};

typedef struct ElaboratedNameStructToBeRenamedByTypedef
	ElaboratedNameStructRenamedByTypedef;

#if defined(__cplusplus)

struct NonPODVirtual {
	int hello;
	virtual void pureVirtual () = 0;
};

struct PODNonC {
	int hello;
	void nonVirtual ();
};

#else
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wmissing-declarations"

struct {
	int fully_anon_struct;
};

#pragma clang diagnostic pop
#endif

struct Parent {
	char first;
	struct ChildStruct {
		int a;
		char b;
	} second;
	unsigned long third;
	struct AnotherChild {
		int foo;
		struct AndAnotherChild {
			char *name;
			int age;
		} andAnotherChild;
		int bar;
	} anotherChild;
};

#include <libkern/OSTypes.h>
typedef struct _NXTabletPointData {
	SInt32  x;  /* absolute x coordinate in tablet space at full tablet resolution */
    SInt32  y;  /* absolute y coordinate in tablet space at full tablet resolution */
	SInt32  z;  /* absolute z coordinate in tablet space at full tablet resolution */
    UInt16  buttons;   /* one bit per button - bit 0 is first button - 1 = closed  */
    UInt16  pressure;  /* scaled pressure value; MAX=(2^16)-1, MIN=0 */
    struct {           /* tilt range is -((2^15)-1) to (2^15)-1 (-32767 to 32767)  */
        SInt16 x;      /* scaled tilt x value */
        SInt16 y;      /* scaled tilt y value */
	} tilt;
	UInt16  rotation;  /* Fixed-point representation of device rotation in a 10.6 format */
	SInt16  tangentialPressure; /* tangential pressure on the device; same range as tilt */
	UInt16  deviceID;  /* system-assigned unique device ID */
	SInt16  vendor1;   /* vendor-defined signed 16-bit integer */
	SInt16  vendor2;   /* vendor-defined signed 16-bit integer */
	SInt16  vendor3;   /* vendor-defined signed 16-bit integer */
} NXTabletPointData, *NXTabletPointDataPtr;

/* TabletProximityData type: defines the tablet data for proximity
 * events included in mouse events created by a tablet driver.
 */

typedef struct _NXTabletProximityData {
    UInt16  vendorID;             /* vendor-defined ID - typically the USB vendor ID */
	UInt16  tabletID;             /* vendor-defined tablet ID - typically the USB product ID */
	UInt16  pointerID;            /* vendor-defined ID of the specific pointing device */
	UInt16  deviceID;             /* system-assigned unique device ID */
	UInt16  systemTabletID;       /* system-assigned unique tablet ID */
	UInt16  vendorPointerType;    /* vendor-defined pointer type */
	UInt32  pointerSerialNumber;  /* vendor-defined serial number */
	UInt64  uniqueID __attribute__ ((packed));             /* vendor-defined unique ID */
	UInt32  capabilityMask;       /* capabilities mask of the device */
	UInt8   pointerType;          /* type of pointing device */
	UInt8   enterProximity;       /* non-zero = entering; zero = leaving */
	SInt16  reserved1;
} NXTabletProximityData, *NXTabletProximityDataPtr;

/* EventData type: defines the data field of an event */

typedef	union {
    struct {    /* For mouse-down and mouse-up events */
        UInt8   subx;       /* sub-pixel position for x */
        UInt8   suby;       /* sub-pixel position for y */
        SInt16  eventNum;   /* unique identifier for this button */
        SInt32  click;      /* click state of this event */
        UInt8   pressure;   /* pressure value: 0=none, 255=full */
        UInt8   buttonNumber;/* button generating other button event (0-31) */
        UInt8   subType;
        UInt8   reserved2;
        SInt32  reserved3;
        union {
            NXTabletPointData      point;     /* tablet point data */
            NXTabletProximityData  proximity; /* tablet proximity data */
        } tablet;
    } mouse;
    struct {
        SInt32  dx;
        SInt32  dy;
        UInt8   subx;
        UInt8   suby;
        UInt8   subType;
        UInt8   reserved1;
        SInt32  reserved2;
        union {
            NXTabletPointData      point;     /* tablet point data */
            NXTabletProximityData  proximity; /* tablet proximity data */
        } tablet;
    } mouseMove;
    struct {    /* For key-down and key-up events */
        UInt16  origCharSet;    /* unmodified character set code */
        SInt16  repeat;         /* for key-down: nonzero if really a repeat */
        UInt16  charSet;        /* character set code */
        UInt16  charCode;       /* character code in that set */
        UInt16  keyCode;        /* device-dependent key number */
        UInt16  origCharCode;   /* unmodified character code */
        SInt32  reserved1;
        UInt32  keyboardType;
        SInt32  reserved2;
        SInt32  reserved3;
        SInt32  reserved4;
        SInt32  reserved5[4];
    } key;
    struct {    /* For mouse-entered and mouse-exited events */
        SInt16  reserved;
        SInt16  eventNum;       /* unique identifier from mouse down event */
        SInt32  trackingNum;    /* unique identifier from settrackingrect */
        SInt32  userData;       /* uninterpreted integer from settrackingrect */
        SInt32  reserved1;
        SInt32  reserved2;
        SInt32  reserved3;
        SInt32  reserved4;
        SInt32  reserved5;
        SInt32  reserved6[4];
    } tracking;
    struct {
        SInt16  deltaAxis1;
        SInt16  deltaAxis2;
        SInt16  deltaAxis3;
        SInt16  reserved1;
        SInt32  fixedDeltaAxis1;
        SInt32  fixedDeltaAxis2;
        SInt32  fixedDeltaAxis3;
        SInt32  pointDeltaAxis1;
        SInt32  pointDeltaAxis2;
        SInt32  pointDeltaAxis3;
        SInt32  reserved8[4];
    } scrollWheel, zoom;
    struct {    /* For window-changed, sys-defined, and app-defined events */
        SInt16  reserved;
        SInt16  subType;    /* event subtype for compound events */
        union {
            float   F[11];  /* for use in compound events */
            SInt32  L[11];  /* for use in compound events */
            SInt16  S[22];  /* for use in compound events */
            char    C[44];  /* for use in compound events */
        } misc;
    } compound;
    struct {
        SInt32  x;  /* absolute x coordinate in tablet space at full tablet resolution */
        SInt32  y;  /* absolute y coordinate in tablet space at full tablet resolution */
        SInt32  z;  /* absolute z coordinate in tablet space at full tablet resolution */
        UInt16  buttons;   /* one bit per button - bit 0 is first button - 1 = closed  */
        UInt16  pressure;  /* scaled pressure value; MAX=(2^16)-1, MIN=0 */
        struct {           /* tilt range is -((2^15)-1) to (2^15)-1 (-32767 to 32767)  */
            SInt16 x;      /* scaled tilt x value */
            SInt16 y;      /* scaled tilt y value */
        } tilt;
        UInt16  rotation;  /* Fixed-point representation of device rotation in a 10.6 format */
        SInt16  tangentialPressure; /* tangential pressure on the device; same range as tilt */
        UInt16  deviceID;  /* system-assigned unique device ID */
        SInt16  vendor1;   /* vendor-defined signed 16-bit integer */
        SInt16  vendor2;   /* vendor-defined signed 16-bit integer */
        SInt16  vendor3;   /* vendor-defined signed 16-bit integer */
        SInt32  reserved[4];
    } tablet;
    struct {
        UInt16  vendorID;  /* vendor-defined ID - typically the USB vendor ID */
        UInt16  tabletID;  /* vendor-defined tablet ID - typically the USB product ID */
        UInt16  pointerID; /* vendor-defined ID of the specific pointing device */
        UInt16  deviceID;             /* system-assigned unique device ID */
        UInt16  systemTabletID;       /* system-assigned unique tablet ID */
        UInt16  vendorPointerType;    /* vendor-defined pointer type */
        UInt32  pointerSerialNumber;  /* vendor-defined serial number */
        UInt64  uniqueID __attribute__ ((packed));             /* vendor-defined unique ID */
        UInt32  capabilityMask;       /* capabilities mask of the device */
        UInt8   pointerType;          /* type of pointing device */
        UInt8   enterProximity;       /* non-zero = entering; zero = leaving */
        SInt16  reserved1;
        SInt32  reserved2[4];
    } proximity;
} NXEventData;

//typedef struct  BluetoothHCIEventReturnLinkKeysResults  BluetoothHCIEventReturnLinkKeysResults;
struct BluetoothHCIEventReturnLinkKeysResults
{
    int numLinkKeys;
    struct {
        int deviceAddress;
        int linkKey;
    } linkKeys[1];
};
