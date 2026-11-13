using System;
using System.Runtime.InteropServices;
using Foundation;

static class CFunctions {
	// extern void * _void_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe void* _void_ptr ();

	// extern int * _int_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe int* _int_ptr ();

	// extern int ** _int_ptr_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe int** _int_ptr_ptr ();

	// extern int *** _int_ptr_ptr_ptr ();
	[DllImport ("__Internal")]
	[Verify (PlatformInvoke)]
	static extern unsafe int*** _int_ptr_ptr_ptr ();
}

// @interface ObjCObjectOutPointers
interface ObjCObjectOutPointers {
	// -(void)getError:(NSError **)error;
	[Export ("getError:")]
	void GetError (out NSError error);
}

[StructLayout (LayoutKind.Sequential)]
public struct CLLocationCoordinate2D {
	public double latitude;

	public double longitude;
}

// @interface MAPolygon : NSObject
[BaseType (typeof (NSObject))]
interface MAPolygon {
	// +(instancetype)polygonWithCoordinates:(CLLocationCoordinate2D *)coords count:(NSUInteger)count;
	[Static]
	[Export ("polygonWithCoordinates:count:")]
	unsafe MAPolygon PolygonWithCoordinates (CLLocationCoordinate2D* coords, nuint count);

	// -(void)getCoordinates:(CLLocationCoordinate2D *)coords count:(NSUInteger)count;
	[Export ("getCoordinates:count:")]
	unsafe void GetCoordinates (CLLocationCoordinate2D* coords, nuint count);

	// -(void)setCoordinates:(const CLLocationCoordinate2D *)coords count:(NSUInteger)count;
	[Export ("setCoordinates:count:")]
	unsafe void SetCoordinates (CLLocationCoordinate2D* coords, nuint count);

	// -(CLLocationCoordinate2D *)coordinates;
	[Export ("coordinates")]
	[Verify (MethodToProperty)]
	unsafe CLLocationCoordinate2D* Coordinates { get; }

	// -(void)getCoordinatesPointer:(CLLocationCoordinate2D **)coords;
	[Export ("getCoordinatesPointer:")]
	unsafe void GetCoordinatesPointer (CLLocationCoordinate2D** coords);
}
