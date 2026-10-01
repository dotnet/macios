#if __MACOS__
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Security {

	/// <summary>Specifies the result of an authorization operation.</summary>
	[SupportedOSPlatform ("macos")]
	public enum AuthorizationResult : uint {
		/// <summary>The authorization was granted.</summary>
		Allow = 0,
		/// <summary>The authorization was denied.</summary>
		Deny = 1,
		/// <summary>The result is undefined.</summary>
		Undefined = 2,
		/// <summary>The user cancelled the authorization.</summary>
		UserCanceled = 3,
	}

	/// <summary>Flags that describe properties of an authorization context value.</summary>
	[SupportedOSPlatform ("macos")]
	[Flags]
	public enum AuthorizationContextFlags : uint {
		/// <summary>No context flags are set.</summary>
		None = 0,
		/// <summary>The value can be extracted by the client.</summary>
		Extractable = 1 << 0,
		/// <summary>The value is volatile and should not be persisted.</summary>
		Volatile = 1 << 1,
		/// <summary>The value is sticky and persists across mechanism evaluations.</summary>
		Sticky = 1 << 2,
	}

#pragma warning disable 0649 // The authorization plugin host initializes these fields.
	/// <summary>Contains the callbacks provided by the authorization engine host.</summary>
	/// <remarks>
	/// Access this structure through the pointer supplied by the authorization plugin host. The pointer is borrowed and must not be freed,
	/// mutated, or used after the host-defined authorization mechanism lifetime ends. Do not copy the structure by value, because older hosts
	/// may provide a smaller table. Access its properties through the pointer and check version-gated function pointers for null before invoking them.
	/// </remarks>
	[SupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	public readonly unsafe struct AuthorizationCallbacks {
		readonly uint version;
		readonly delegate* unmanaged<IntPtr, AuthorizationResult, OSStatus> setResult;
		readonly delegate* unmanaged<IntPtr, OSStatus> requestInterrupt;
		readonly delegate* unmanaged<IntPtr, OSStatus> didDeactivate;
		readonly delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags*, IntPtr*, OSStatus> getContextValue;
		readonly delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags, IntPtr, OSStatus> setContextValue;
		readonly delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> getHintValue;
		readonly delegate* unmanaged<IntPtr, IntPtr, IntPtr, OSStatus> setHintValue;
		readonly delegate* unmanaged<IntPtr, IntPtr*, OSStatus> getArguments;
		readonly delegate* unmanaged<IntPtr, IntPtr*, OSStatus> getSessionId;
		readonly delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> getImmutableHintValue;
		readonly delegate* unmanaged<IntPtr, IntPtr*, OSStatus> getLAContext;
		readonly delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> getTokenIdentities;
		readonly delegate* unmanaged<IntPtr, IntPtr*, OSStatus> getTKTokenWatcher;
		readonly delegate* unmanaged<IntPtr, IntPtr, OSStatus> removeHintValue;
		readonly delegate* unmanaged<IntPtr, IntPtr, OSStatus> removeContextValue;

		/// <summary>Gets the version of the callback table.</summary>
		public uint Version => version;

		/// <summary>Gets the callback that sets the authorization result.</summary>
		public delegate* unmanaged<IntPtr, AuthorizationResult, OSStatus> SetResult => setResult;

		/// <summary>Gets the callback that requests an authorization interrupt.</summary>
		public delegate* unmanaged<IntPtr, OSStatus> RequestInterrupt => requestInterrupt;

		/// <summary>Gets the callback that reports mechanism deactivation.</summary>
		public delegate* unmanaged<IntPtr, OSStatus> DidDeactivate => didDeactivate;

		/// <summary>Gets the callback that reads a context value.</summary>
		/// <remarks>The returned authorization value and its data are borrowed.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags*, IntPtr*, OSStatus> GetContextValue => getContextValue;

		/// <summary>Gets the callback that writes a context value.</summary>
		/// <remarks>The authorization value and its data are copied by the authorization engine.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags, IntPtr, OSStatus> SetContextValue => setContextValue;

		/// <summary>Gets the callback that reads a hint value.</summary>
		/// <remarks>The returned authorization value and its data are borrowed.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetHintValue => getHintValue;

		/// <summary>Gets the callback that writes a hint value.</summary>
		/// <remarks>The authorization value and its data are copied by the authorization engine.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr, OSStatus> SetHintValue => setHintValue;

		/// <summary>Gets the callback that reads the authorization arguments.</summary>
		/// <remarks>The returned authorization value vector and its data are borrowed.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetArguments => getArguments;

		/// <summary>Gets the callback that reads the authorization session identifier.</summary>
		public delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetSessionId => getSessionId;

		/// <summary>Gets the callback that reads an immutable hint value.</summary>
		/// <remarks>The returned authorization value and its data are borrowed. Returns a null function pointer when <see cref="Version" /> is less than 1.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetImmutableHintValue => version >= 1 ? getImmutableHintValue : null;

		/// <summary>Gets the callback that creates a local authentication context.</summary>
		/// <remarks>Returns a null function pointer when <see cref="Version" /> is less than 2. The caller owns the returned native object.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetLAContext => version >= 2 ? getLAContext : null;

		/// <summary>Gets the callback that returns token identities.</summary>
		/// <remarks>Returns a null function pointer when <see cref="Version" /> is less than 2. The caller owns the returned native array.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetTokenIdentities => version >= 2 ? getTokenIdentities : null;

		/// <summary>Gets the callback that creates a token watcher.</summary>
		/// <remarks>Returns a null function pointer when <see cref="Version" /> is less than 3. The caller owns the returned native object.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetTKTokenWatcher => version >= 3 ? getTKTokenWatcher : null;

		/// <summary>Gets the callback that removes a hint value.</summary>
		/// <remarks>Returns a null function pointer when <see cref="Version" /> is less than 4.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, OSStatus> RemoveHintValue => version >= 4 ? removeHintValue : null;

		/// <summary>Gets the callback that removes a context value.</summary>
		/// <remarks>Returns a null function pointer when <see cref="Version" /> is less than 4.</remarks>
		public delegate* unmanaged<IntPtr, IntPtr, OSStatus> RemoveContextValue => version >= 4 ? removeContextValue : null;
	}
#pragma warning restore 0649
}
#endif // __MACOS__
