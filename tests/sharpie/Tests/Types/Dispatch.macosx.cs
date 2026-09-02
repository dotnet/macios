using CoreFoundation;
using Foundation;
using ObjCRuntime;

// @interface DispatchTests
interface DispatchTests {
	// @property (assign) dispatch_object_t object;
	[Export ("object", ArgumentSemantic.Assign)]
	DispatchObject Object { get; set; }

	// @property (assign) dispatch_queue_t queue;
	[Export ("queue", ArgumentSemantic.Assign)]
	DispatchQueue Queue { get; set; }

	// @property (assign) dispatch_group_t group;
	[Export ("group", ArgumentSemantic.Assign)]
	DispatchGroup Group { get; set; }
}
