using System.Collections.Generic;

#nullable enable

namespace Cecil.Tests {
	public partial class SetHandleTest {
		static HashSet<string> knownFailuresNobodyCallsHandleSetter = new HashSet<string> {
			// Nothing should be added here, any issues must be fixed.
		};
	}
}
