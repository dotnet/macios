//
// corefoundation.cs: Definitions for CoreFoundation
//
// Copyright 2014-2015 Xamarin Inc. All rights reserved.
//

using System;
using XamCore.Foundation;
using XamCore.ObjCRuntime;

namespace XamCore.CoreFoundation {

	[Partial]
	interface CFAllocator {

		[Internal][Field ("kCFAllocatorDefault")]
		IntPtr default_ptr { get; }

		[Internal][Field ("kCFAllocatorSystemDefault")]
		IntPtr system_default_ptr { get; }

		[Internal][Field ("kCFAllocatorMalloc")]
		IntPtr malloc_ptr { get; }

		[Internal][Field ("kCFAllocatorMallocZone")]
		IntPtr malloc_zone_ptr { get; }

		[Internal][Field ("kCFAllocatorNull")]
		IntPtr null_ptr { get; }
	}

	[Partial]
	interface CFRunLoop {

		[Field ("kCFRunLoopDefaultMode")]
		NSString ModeDefault { get; }

		[Field ("kCFRunLoopCommonModes")]
		NSString ModeCommon { get; }
	}

#if !WATCH
	[Partial]
	interface CFNetwork {

		[Field ("kCFErrorDomainCFNetwork", "CFNetwork")]
		NSString ErrorDomain { get; }
	}
#endif
}