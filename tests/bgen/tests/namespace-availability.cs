// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Foundation;
using ObjCRuntime;

#if MONOMAC
namespace AppKit {
#else
namespace UIKit {
#endif
	[iOS (99, 0), TV (99, 0), Mac (99, 0), MacCatalyst (99, 0)]
	[BaseType (typeof (NSObject))]
	interface NamespaceAvailability : Foundation.NamespaceAvailabilityProtocol {
		[iOS (100, 0), TV (100, 0), Mac (100, 0), MacCatalyst (100, 0)]
		[Deprecated (PlatformName.iOS, 101, 0, message: "Use another property.")]
		[Deprecated (PlatformName.TvOS, 101, 0, message: "Use another property.")]
		[Deprecated (PlatformName.MacOSX, 101, 0, message: "Use another property.")]
		[Deprecated (PlatformName.MacCatalyst, 101, 0, message: "Use another property.")]
		[Export ("value")]
		int Value {
			[iOS (102, 0), TV (102, 0), Mac (102, 0), MacCatalyst (102, 0)]
			[Obsoleted (PlatformName.iOS, 103, 0, message: "Use another getter.")]
			[Obsoleted (PlatformName.TvOS, 103, 0, message: "Use another getter.")]
			[Obsoleted (PlatformName.MacOSX, 103, 0, message: "Use another getter.")]
			[Obsoleted (PlatformName.MacCatalyst, 103, 0, message: "Use another getter.")]
			get;
			[NoTV]
			set;
		}

		[iOS (100, 0), TV (100, 0), Mac (100, 0), MacCatalyst (100, 0)]
		[Async]
		[Export ("performWithCompletionHandler:")]
		void Perform (Action completionHandler);
	}

	[iOS (99, 0), TV (99, 0), Mac (99, 0), MacCatalyst (99, 0)]
	[Native]
	public enum NamespaceAvailabilityEnum : long {
		[iOS (100, 0), TV (100, 0), Mac (100, 0), MacCatalyst (100, 0)]
		Value,
	}

	[iOS (99, 0), TV (99, 0), Mac (99, 0), MacCatalyst (99, 0)]
	[Protocol]
	interface NamespaceAvailabilitySource {
		[iOS (100, 0), TV (100, 0), Mac (100, 0), MacCatalyst (100, 0)]
		[Abstract]
		[Export ("inlinedMethod")]
		void InlinedMethod ();
	}

	[MacCatalyst (99, 0)]
	[BaseType (typeof (NSObject))]
	interface NamespaceAvailabilityImplicit {
		[MacCatalyst (100, 0)]
		[Export ("value")]
		int Value { get; }
	}

	[NoMacCatalyst]
	[BaseType (typeof (NSObject))]
	interface NamespaceAvailabilityRestricted {
		[MacCatalyst (100, 0)]
		[Export ("value")]
		int Value {
			[MacCatalyst (102, 0)]
			get;
		}
	}
}

namespace Foundation {
#if CORE_BINDING
	[BaseType (typeof (NSObject))]
	interface NSNumber {
	}

	[BaseType (typeof (NSObject))]
	interface NSValue {
	}
#endif

	[iOS (99, 0), TV (99, 0), Mac (99, 0), MacCatalyst (99, 0)]
	[Protocol]
	interface NamespaceAvailabilityProtocol {
		[iOS (100, 0), TV (100, 0), Mac (100, 0), MacCatalyst (100, 0)]
		[Abstract]
		[Export ("inlinedValue")]
		int InlinedValue {
			[iOS (102, 0), TV (102, 0), Mac (102, 0), MacCatalyst (102, 0)]
			get;
		}
	}

	[iOS (99, 0), TV (99, 0), Mac (99, 0), MacCatalyst (99, 0)]
	[BaseType (typeof (NSObject))]
#if MONOMAC
	interface NamespaceAvailabilityDestination : AppKit.NamespaceAvailabilitySource {
#else
	interface NamespaceAvailabilityDestination : UIKit.NamespaceAvailabilitySource {
#endif
	}
}

namespace AppKit {
	[NoiOS, NoTV, Mac (99, 0), MacCatalyst (99, 0)]
	[BaseType (typeof (NSObject))]
	interface NamespaceAvailabilityCatalyst {
		[TV (100, 0)]
		[Export ("restrictedValue")]
		int RestrictedValue {
			[TV (102, 0)]
			get;
		}
	}
}
