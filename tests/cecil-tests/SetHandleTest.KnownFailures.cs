using System.Collections.Generic;

#nullable enable

namespace Cecil.Tests {
	public partial class SetHandleTest {
		static HashSet<string> knownFailuresNobodyCallsHandleSetter = new HashSet<string> {
			"AddressBook.ABGroup::.ctor(AddressBook.ABRecord)",
			"CoreFoundation.CFMutableString::.ctor(CoreFoundation.CFString,System.IntPtr)",
			"CoreFoundation.CFMutableString::.ctor(System.String,System.IntPtr)",
			"CoreFoundation.CFSocket::Initialize(CoreFoundation.CFRunLoop,CoreFoundation.CFSocket/CreateSocket)",
			"CoreFoundation.CFString::.ctor(System.String)",
			"CoreFoundation.DispatchBlock::Retain()",
			"CoreFoundation.OSLog::.ctor(System.String,System.String)",
			"CoreGraphics.CGPattern::.ctor(CoreGraphics.CGRect,CoreGraphics.CGAffineTransform,System.Runtime.InteropServices.NFloat,System.Runtime.InteropServices.NFloat,CoreGraphics.CGPatternTiling,System.Boolean,CoreGraphics.CGPattern/DrawPattern)",
			"Foundation.NSKeyedUnarchiver::.ctor(Foundation.NSData)",
			"Foundation.NSUuid::.ctor(System.Byte[])",
			"ObjCRuntime.Runtime::RegisterNSObject(Foundation.NSObject,System.IntPtr)",
			"ScreenCaptureKit.SCContentFilter::.ctor(ScreenCaptureKit.SCDisplay,ScreenCaptureKit.SCRunningApplication[],ScreenCaptureKit.SCWindow[],ScreenCaptureKit.SCContentFilterOption)",
			"ScreenCaptureKit.SCContentFilter::.ctor(ScreenCaptureKit.SCDisplay,ScreenCaptureKit.SCWindow[],ScreenCaptureKit.SCContentFilterOption)",
			"Security.SecTrust2::.ctor(Security.SecTrust)",
		};
	}
}
