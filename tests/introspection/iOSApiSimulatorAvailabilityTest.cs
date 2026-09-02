// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Reflection;

namespace Introspection {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class iOSApiSimulatorAvailabilityTest : ApiSimulatorAvailabilityTest {

		protected override bool SkipPInvoke (MethodInfo mi)
		{
			switch (mi.DeclaringType?.Namespace) {
			case "CoreNFC":
				if (Class.GetHandle ("NFCNDEFReaderSession") == IntPtr.Zero)
					return true;
				break;
			}

			return base.SkipPInvoke (mi);
		}

		protected override bool SkipField (PropertyInfo pi)
		{
			switch (pi.DeclaringType?.Namespace) {
			case "CoreNFC":
				if (Class.GetHandle ("NFCNDEFReaderSession") == IntPtr.Zero)
					return true;
				break;
			case "DeviceCheck":
				return true;
			case "Phase":
				return true;
			}

			return base.SkipField (pi);
		}
	}
}
