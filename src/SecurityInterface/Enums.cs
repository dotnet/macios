#nullable enable

using System;
using System.Runtime.Versioning;
using ObjCRuntime;

namespace SecurityInterface {

	/// <summary>Defines the authorization states of an <see cref="SFAuthorizationView" />.</summary>
	[SupportedOSPlatform ("macos")]
	public enum SFAuthorizationViewState : uint {
		/// <summary>The initial state before the first status update.</summary>
		Startup = 0,
		/// <summary>The view is locked, indicating the user is not authorized.</summary>
		Locked,
		/// <summary>An authorization operation is in progress.</summary>
		InProgress,
		/// <summary>The view is unlocked, indicating the user is authorized.</summary>
		Unlocked,
	}

	/// <summary>Identifies button types in an authorization plugin view.</summary>
	[SupportedOSPlatform ("macos")]
	public enum SFButtonType : uint {
		/// <summary>The cancel button.</summary>
		Cancel = 0,
		/// <summary>The OK button.</summary>
		Ok = 1,
		/// <summary>The back button. This has the same value as <see cref="Cancel" />.</summary>
		Back = 0,
		/// <summary>The login button. This has the same value as <see cref="Ok" />.</summary>
		Login = 1,
	}

	/// <summary>Specifies the type of view requested from an authorization plugin view.</summary>
	[SupportedOSPlatform ("macos")]
	public enum SFViewType : uint {
		/// <summary>A view showing both identity and credentials fields.</summary>
		IdentityAndCredentials = 0,
		/// <summary>A view showing only credentials fields.</summary>
		Credentials,
	}
}
