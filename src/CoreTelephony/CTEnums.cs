using ObjCRuntime;
using System;

#nullable enable

namespace CoreTelephony {

	// untyped enum -> CoreTelephonyDefines.h
	// in header file this is used inside a CTError structure where the domain is a SInt32
	/// <summary>An enumeration whose values specify an error domain.</summary>
	public enum CTErrorDomain {
		/// <summary>To be added.</summary>
		NoError = 0,
		/// <summary>To be added.</summary>
		Posix = 1,
		/// <summary>To be added.</summary>
		Mach = 2,
	}

	/// <summary>Enumerates data restrictions for <see cref="CoreTelephony.CTCellularData.RestrictedState" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CTCellularDataRestrictedState : ulong {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Restricted,
		/// <summary>To be added.</summary>
		NotRestricted,
	}

	[MacCatalyst (13, 1)]
	[Native]
	public enum CTCellularPlanProvisioningAddPlanResult : long {
		/// <summary>To be added.</summary>
		Unknown,
		/// <summary>To be added.</summary>
		Fail,
		/// <summary>To be added.</summary>
		Success,
		[iOS (17, 0)]
		Cancel,
	}
}
