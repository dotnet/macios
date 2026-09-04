#nullable enable

#if MONOMAC || __MACCATALYST__

using System;
using System.Runtime.Versioning;

namespace Security {

	/// <summary>Specifies authorization status values.</summary>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public enum AuthorizationStatus {
		/// <summary>The authorization operation succeeded.</summary>
		Success = 0,
		/// <summary>The authorization item set is invalid.</summary>
		InvalidSet = -60001,
		/// <summary>The authorization reference is invalid.</summary>
		InvalidRef = -60002,
		/// <summary>The authorization tag is invalid.</summary>
		InvalidTag = -60003,
		/// <summary>A required pointer is invalid.</summary>
		InvalidPointer = -60004,
		/// <summary>The authorization request was denied.</summary>
		Denied = -60005,
		/// <summary>The authorization request was cancelled.</summary>
		Canceled = -60006,
		/// <summary>The authorization requires interaction that was not allowed.</summary>
		InteractionNotAllowed = -60007,
		/// <summary>An internal authorization error occurred.</summary>
		Internal = -60008,
		/// <summary>The authorization cannot be externalized.</summary>
		ExternalizeNotAllowed = -60009,
		/// <summary>The authorization cannot be internalized.</summary>
		InternalizeNotAllowed = -60010,
		/// <summary>The authorization flags are invalid.</summary>
		InvalidFlags = -60011,
		/// <summary>The privileged tool could not be executed.</summary>
		ToolExecuteFailure = -60031,
		/// <summary>The privileged tool environment is invalid.</summary>
		ToolEnvironmentError = -60032,
		/// <summary>An invalid address was provided.</summary>
		BadAddress = -60033,
	}

	/// <summary>Specifies options for authorization operations.</summary>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[Flags]
	public enum AuthorizationFlags : int {
		/// <summary>No authorization options are set.</summary>
		Defaults,
		/// <summary>Allows the authorization service to interact with the user.</summary>
		InteractionAllowed = 1 << 0,
		/// <summary>Requests additional rights when necessary.</summary>
		ExtendRights = 1 << 1,
		/// <summary>Allows the operation to succeed when only some rights are granted.</summary>
		PartialRights = 1 << 2,
		/// <summary>Prevents acquired rights from persisting after the operation.</summary>
		DestroyRights = 1 << 3,
		/// <summary>Preauthorizes rights for later use.</summary>
		PreAuthorize = 1 << 4,
		/// <summary>Skips internal authorization checks.</summary>
		SkipInternalAuth = 1 << 9,
		/// <summary>Prevents authorization data from being returned.</summary>
		NoData = 1 << 20,
	}
}

#endif
