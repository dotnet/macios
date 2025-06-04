using System.Collections.Generic;

#nullable enable

namespace Cecil.Tests {
	public partial class SetHandleTest {
		// Runtime.RegisterNSObject shouldn't call InitializeHandle, so just mark this as a known failure.
		static HashSet<string> knownFailuresNobodyCallsHandleSetter = new HashSet<string> {
			"ObjCRuntime.Runtime::RegisterNSObject(Foundation.NSObject,System.IntPtr)",
			"Foundation.NSKeyedUnarchiver::.ctor(Foundation.NSData)",
		};
	}
}
