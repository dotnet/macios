using CoreFoundation;
using Foundation;

// @interface NSGeometryTest
interface NSGeometryTest {
	// -(NSPoint)getPoint;
	[Export ("getPoint")]
	[Verify (MethodToProperty)]
	CGPoint Point { get; }

	// -(NSSize)getSize;
	[Export ("getSize")]
	[Verify (MethodToProperty)]
	CGSize Size { get; }

	// -(NSRect)getRect;
	[Export ("getRect")]
	[Verify (MethodToProperty)]
	CGRect Rect { get; }
}
