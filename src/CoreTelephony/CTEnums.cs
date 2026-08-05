
#nullable enable

namespace CoreTelephony {

	// untyped enum -> CoreTelephonyDefines.h
	// in header file this is used inside a CTError structure where the domain is a SInt32
	/// <summary>An enumeration whose values specify an error domain.</summary>
	public enum CTErrorDomain {
		/// <summary>No error occurred.</summary>
		NoError = 0,
		/// <summary>The error is in the POSIX error domain.</summary>
		Posix = 1,
		/// <summary>The error is in the Mach error domain.</summary>
		Mach = 2,
	}

	/// <summary>Enumerates data restrictions for <see cref="CoreTelephony.CTCellularData.RestrictedState" />.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CTCellularDataRestrictedState : ulong {
		/// <summary>The cellular data restriction state is unknown.</summary>
		Unknown,
		/// <summary>Cellular data access is restricted.</summary>
		Restricted,
		/// <summary>Cellular data access is not restricted.</summary>
		NotRestricted,
	}

	/// <summary>Enumerates the results of adding a cellular plan.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	public enum CTCellularPlanProvisioningAddPlanResult : long {
		/// <summary>The result is unknown.</summary>
		Unknown,
		/// <summary>The cellular plan could not be added.</summary>
		Fail,
		/// <summary>The cellular plan was added successfully.</summary>
		Success,
		/// <summary>The operation was canceled.</summary>
		[iOS (17, 0)]
		Cancel,
	}
}
