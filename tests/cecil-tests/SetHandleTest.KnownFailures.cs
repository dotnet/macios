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
			"GameplayKit.GKPath::.ctor(System.Numerics.Vector2[],System.Single,System.Boolean)",
			"GameplayKit.GKPath::.ctor(System.Numerics.Vector3[],System.Single,System.Boolean)",
			"ModelIO.MDLNoiseTexture::.ctor(System.Single,System.String,CoreGraphics.NVector2i,ModelIO.MDLTextureChannelEncoding,ModelIO.MDLNoiseTextureType)",
			"ObjCRuntime.Runtime::RegisterNSObject(Foundation.NSObject,System.IntPtr)",
			"ScreenCaptureKit.SCContentFilter::.ctor(ScreenCaptureKit.SCDisplay,ScreenCaptureKit.SCRunningApplication[],ScreenCaptureKit.SCWindow[],ScreenCaptureKit.SCContentFilterOption)",
			"ScreenCaptureKit.SCContentFilter::.ctor(ScreenCaptureKit.SCDisplay,ScreenCaptureKit.SCWindow[],ScreenCaptureKit.SCContentFilterOption)",
			"Security.SecTrust2::.ctor(Security.SecTrust)",
		};
	}
}
