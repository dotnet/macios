#if __MACOS__
#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using CryptoTokenKit;
using Foundation;
using LocalAuthentication;
using ObjCRuntime;

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

	[StructLayout (LayoutKind.Sequential)]
	struct AuthorizationValueNative {
		internal nuint Length;
		internal unsafe void* Data;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AuthorizationValueVectorNative {
		internal uint Count;
		internal unsafe AuthorizationValueNative* Values;
	}

	[StructLayout (LayoutKind.Sequential)]
	struct AuthorizationCallbacksNative {
		internal uint Version;
		internal unsafe delegate* unmanaged<IntPtr, AuthorizationResult, OSStatus> SetResult;
		internal unsafe delegate* unmanaged<IntPtr, OSStatus> RequestInterrupt;
		internal unsafe delegate* unmanaged<IntPtr, OSStatus> DidDeactivate;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags*, IntPtr*, OSStatus> GetContextValue;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags, IntPtr, OSStatus> SetContextValue;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetHintValue;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr, OSStatus> SetHintValue;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetArguments;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetSessionId;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetImmutableHintValue;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetLAContext;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetTokenIdentities;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetTKTokenWatcher;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, OSStatus> RemoveHintValue;
		internal unsafe delegate* unmanaged<IntPtr, IntPtr, OSStatus> RemoveContextValue;
	}

	/// <summary>Provides the unmanaged callbacks used to create an owned <see cref="AuthorizationCallbacks" /> table.</summary>
	[SupportedOSPlatform ("macos")]
	public sealed class AuthorizationCallbacksConfiguration {
		/// <summary>Creates an empty callback configuration.</summary>
		public AuthorizationCallbacksConfiguration ()
		{
		}

		/// <summary>Gets or sets the callback that sets the authorization result.</summary>
		public unsafe delegate* unmanaged<IntPtr, AuthorizationResult, OSStatus> SetResult { get; set; }

		/// <summary>Gets or sets the callback that requests an authorization interrupt.</summary>
		public unsafe delegate* unmanaged<IntPtr, OSStatus> RequestInterrupt { get; set; }

		/// <summary>Gets or sets the callback that reports mechanism deactivation.</summary>
		public unsafe delegate* unmanaged<IntPtr, OSStatus> DidDeactivate { get; set; }

		/// <summary>Gets or sets the callback that gets a context value.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags*, IntPtr*, OSStatus> GetContextValue { get; set; }

		/// <summary>Gets or sets the callback that sets a context value.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags, IntPtr, OSStatus> SetContextValue { get; set; }

		/// <summary>Gets or sets the callback that gets a hint value.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetHintValue { get; set; }

		/// <summary>Gets or sets the callback that sets a hint value.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr, OSStatus> SetHintValue { get; set; }

		/// <summary>Gets or sets the callback that gets the authorization arguments.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetArguments { get; set; }

		/// <summary>Gets or sets the callback that gets the authorization session identifier.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetSessionId { get; set; }

		/// <summary>Gets or sets the callback that gets an immutable hint value.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetImmutableHintValue { get; set; }

		/// <summary>Gets or sets the callback that gets the local authentication context.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetLAContext { get; set; }

		/// <summary>Gets or sets the callback that gets token identities.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, IntPtr*, OSStatus> GetTokenIdentities { get; set; }

		/// <summary>Gets or sets the callback that gets the token watcher.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr*, OSStatus> GetTokenWatcher { get; set; }

		/// <summary>Gets or sets the callback that removes a hint value.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, OSStatus> RemoveHintValue { get; set; }

		/// <summary>Gets or sets the callback that removes a context value.</summary>
		public unsafe delegate* unmanaged<IntPtr, IntPtr, OSStatus> RemoveContextValue { get; set; }
	}

	/// <summary>Wraps the native AuthorizationCallbacks structure provided by the authorization engine host.</summary>
	[SupportedOSPlatform ("macos")]
	public sealed class AuthorizationCallbacks : IDisposable {
		const uint currentVersion = 4;

		unsafe readonly AuthorizationCallbacksNative* callbacks;
		readonly bool owns;
		int disposed;

		unsafe AuthorizationCallbacks (AuthorizationCallbacksNative* callbacks)
		{
			this.callbacks = callbacks;
		}

		unsafe AuthorizationCallbacks (AuthorizationCallbacksNative callbacks)
		{
			var pointer = (AuthorizationCallbacksNative*) Marshal.AllocHGlobal (sizeof (AuthorizationCallbacksNative));
			*pointer = callbacks;
			this.callbacks = pointer;
			owns = true;
		}

		/// <summary>Creates an owned callbacks table from static unmanaged callback pointers.</summary>
		/// <param name="configuration">The callback pointers used to populate the native table.</param>
		/// <remarks>
		/// The callback pointers should reference static C# methods marked with <see cref="UnmanagedCallersOnlyAttribute" /> and must not throw exceptions.
		/// This owned table cannot be used with <c>SFAuthorizationPluginView</c>, which requires the borrowed table supplied by the authorization plugin host.
		/// </remarks>
		public AuthorizationCallbacks (AuthorizationCallbacksConfiguration configuration)
			: this (CreateNative (configuration))
		{
		}

		static unsafe AuthorizationCallbacksNative CreateNative (AuthorizationCallbacksConfiguration configuration)
		{
			ArgumentNullException.ThrowIfNull (configuration);
			if (configuration.SetResult is null)
				throw new ArgumentException ("The SetResult callback is required.", nameof (configuration));
			if (configuration.RequestInterrupt is null)
				throw new ArgumentException ("The RequestInterrupt callback is required.", nameof (configuration));
			if (configuration.DidDeactivate is null)
				throw new ArgumentException ("The DidDeactivate callback is required.", nameof (configuration));
			if (configuration.GetContextValue is null)
				throw new ArgumentException ("The GetContextValue callback is required.", nameof (configuration));
			if (configuration.SetContextValue is null)
				throw new ArgumentException ("The SetContextValue callback is required.", nameof (configuration));
			if (configuration.GetHintValue is null)
				throw new ArgumentException ("The GetHintValue callback is required.", nameof (configuration));
			if (configuration.SetHintValue is null)
				throw new ArgumentException ("The SetHintValue callback is required.", nameof (configuration));
			if (configuration.GetArguments is null)
				throw new ArgumentException ("The GetArguments callback is required.", nameof (configuration));
			if (configuration.GetSessionId is null)
				throw new ArgumentException ("The GetSessionId callback is required.", nameof (configuration));
			if (configuration.GetImmutableHintValue is null)
				throw new ArgumentException ("The GetImmutableHintValue callback is required.", nameof (configuration));
			if (configuration.GetLAContext is null)
				throw new ArgumentException ("The GetLAContext callback is required.", nameof (configuration));
			if (configuration.GetTokenIdentities is null)
				throw new ArgumentException ("The GetTokenIdentities callback is required.", nameof (configuration));
			if (configuration.GetTokenWatcher is null)
				throw new ArgumentException ("The GetTokenWatcher callback is required.", nameof (configuration));
			if (configuration.RemoveHintValue is null)
				throw new ArgumentException ("The RemoveHintValue callback is required.", nameof (configuration));
			if (configuration.RemoveContextValue is null)
				throw new ArgumentException ("The RemoveContextValue callback is required.", nameof (configuration));

			return new AuthorizationCallbacksNative {
				Version = currentVersion,
				SetResult = configuration.SetResult,
				RequestInterrupt = configuration.RequestInterrupt,
				DidDeactivate = configuration.DidDeactivate,
				GetContextValue = configuration.GetContextValue,
				SetContextValue = configuration.SetContextValue,
				GetHintValue = configuration.GetHintValue,
				SetHintValue = configuration.SetHintValue,
				GetArguments = configuration.GetArguments,
				GetSessionId = configuration.GetSessionId,
				GetImmutableHintValue = configuration.GetImmutableHintValue,
				GetLAContext = configuration.GetLAContext,
				GetTokenIdentities = configuration.GetTokenIdentities,
				GetTKTokenWatcher = configuration.GetTokenWatcher,
				RemoveHintValue = configuration.RemoveHintValue,
				RemoveContextValue = configuration.RemoveContextValue,
			};
		}

		/// <summary>Creates an <see cref="AuthorizationCallbacks" /> from a native callbacks pointer.</summary>
		/// <param name="callbacks">The pointer to the native AuthorizationCallbacks structure.</param>
		/// <returns>A managed wrapper, or <see langword="null" /> if the pointer is zero.</returns>
		public static unsafe AuthorizationCallbacks? Create (IntPtr callbacks)
		{
			if (callbacks == IntPtr.Zero)
				return null;
			return new AuthorizationCallbacks ((AuthorizationCallbacksNative*) callbacks);
		}

		/// <summary>Releases the owned native callbacks table if the instance was not disposed.</summary>
		~AuthorizationCallbacks ()
		{
			Dispose (false);
		}

		/// <summary>Releases the owned native callbacks table.</summary>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		unsafe void Dispose (bool disposing)
		{
			if (Interlocked.Exchange (ref disposed, 1) != 0)
				return;
			if (owns)
				Marshal.FreeHGlobal ((IntPtr) callbacks);
		}

		unsafe AuthorizationCallbacksNative* GetCallbacks ()
		{
			if (Volatile.Read (ref disposed) != 0)
				ObjCRuntime.ThrowHelper.ThrowObjectDisposedException (this);
			return callbacks;
		}

		internal bool Owns => owns;

		internal unsafe IntPtr GetCheckedPointer () => (IntPtr) GetCallbacks ();

		/// <summary>Gets the version of the callbacks structure.</summary>
		public unsafe uint Version => GetCallbacks ()->Version;

		void EnsureVersion (uint minimumVersion, string callback)
		{
			if (Version < minimumVersion)
				throw new NotSupportedException ($"The '{callback}' callback requires AuthorizationCallbacks version {minimumVersion} or later.");
		}

		static unsafe byte []? CopyValue (AuthorizationValueNative* value)
		{
			if (value is null)
				return null;
			if (value->Length == 0)
				return [];
			if (value->Data is null)
				throw new InvalidOperationException ("The native authorization value has a non-zero length and a null data pointer.");

			var length = checked((int) value->Length);
			var result = new byte [length];
			Marshal.Copy ((IntPtr) value->Data, result, 0, length);
			return result;
		}

		static unsafe byte [] []? CopyValues (AuthorizationValueVectorNative* vector)
		{
			if (vector is null)
				return null;
			if (vector->Count == 0)
				return [];
			if (vector->Values is null)
				throw new InvalidOperationException ("The native authorization value vector has a non-zero count and a null values pointer.");

			var count = checked((int) vector->Count);
			var result = new byte [count] [];
			for (var i = 0; i < count; i++)
				result [i] = CopyValue (&vector->Values [i]) ?? [];
			return result;
		}

		static InvalidOperationException MissingCallback (string callback)
			=> new InvalidOperationException ($"The native AuthorizationCallbacks table does not provide the '{callback}' callback.");

		/// <summary>Sets the result of the current authorization evaluation.</summary>
		public unsafe OSStatus SetResult (AuthorizationEngine engine, AuthorizationResult result)
		{
			ArgumentNullException.ThrowIfNull (engine);
			var callback = GetCallbacks ()->SetResult;
			if (callback is null)
				throw MissingCallback (nameof (SetResult));
			var status = callback (engine.GetCheckedHandle (), result);
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Requests an interrupt of the current authorization evaluation.</summary>
		public unsafe OSStatus RequestInterrupt (AuthorizationEngine engine)
		{
			ArgumentNullException.ThrowIfNull (engine);
			var callback = GetCallbacks ()->RequestInterrupt;
			if (callback is null)
				throw MissingCallback (nameof (RequestInterrupt));
			var status = callback (engine.GetCheckedHandle ());
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Notifies the engine that the mechanism has deactivated.</summary>
		public unsafe OSStatus DidDeactivate (AuthorizationEngine engine)
		{
			ArgumentNullException.ThrowIfNull (engine);
			var callback = GetCallbacks ()->DidDeactivate;
			if (callback is null)
				throw MissingCallback (nameof (DidDeactivate));
			var status = callback (engine.GetCheckedHandle ());
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Gets a context value for the specified key.</summary>
		public unsafe OSStatus GetContextValue (AuthorizationEngine engine, string key, out AuthorizationContextFlags contextFlags, out byte []? value)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (key);
			var callback = GetCallbacks ()->GetContextValue;
			if (callback is null)
				throw MissingCallback (nameof (GetContextValue));

			using var nativeKey = new TransientString (key);
			AuthorizationContextFlags flags = default;
			IntPtr nativeValue = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), nativeKey, &flags, &nativeValue);
			contextFlags = flags;
			value = status == 0 ? CopyValue ((AuthorizationValueNative*) nativeValue) : null;
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Sets a context value for the specified key.</summary>
		public unsafe OSStatus SetContextValue (AuthorizationEngine engine, string key, AuthorizationContextFlags contextFlags, byte [] value)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (key);
			ArgumentNullException.ThrowIfNull (value);
			var callback = GetCallbacks ()->SetContextValue;
			if (callback is null)
				throw MissingCallback (nameof (SetContextValue));

			using var nativeKey = new TransientString (key);
			fixed (byte* valuePtr = value) {
				var nativeValue = new AuthorizationValueNative {
					Length = (nuint) value.Length,
					Data = valuePtr,
				};
				var status = callback (engine.GetCheckedHandle (), nativeKey, contextFlags, (IntPtr) (&nativeValue));
				GC.KeepAlive (engine);
				return status;
			}
		}

		/// <summary>Gets a hint value for the specified key.</summary>
		public unsafe OSStatus GetHintValue (AuthorizationEngine engine, string key, out byte []? value)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (key);
			var callback = GetCallbacks ()->GetHintValue;
			if (callback is null)
				throw MissingCallback (nameof (GetHintValue));

			using var nativeKey = new TransientString (key);
			IntPtr nativeValue = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), nativeKey, &nativeValue);
			value = status == 0 ? CopyValue ((AuthorizationValueNative*) nativeValue) : null;
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Sets a hint value for the specified key.</summary>
		public unsafe OSStatus SetHintValue (AuthorizationEngine engine, string key, byte [] value)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (key);
			ArgumentNullException.ThrowIfNull (value);
			var callback = GetCallbacks ()->SetHintValue;
			if (callback is null)
				throw MissingCallback (nameof (SetHintValue));

			using var nativeKey = new TransientString (key);
			fixed (byte* valuePtr = value) {
				var nativeValue = new AuthorizationValueNative {
					Length = (nuint) value.Length,
					Data = valuePtr,
				};
				var status = callback (engine.GetCheckedHandle (), nativeKey, (IntPtr) (&nativeValue));
				GC.KeepAlive (engine);
				return status;
			}
		}

		/// <summary>Gets the argument values supplied to the authorization mechanism.</summary>
		public unsafe OSStatus GetArguments (AuthorizationEngine engine, out byte [] []? arguments)
		{
			ArgumentNullException.ThrowIfNull (engine);
			EnsureVersion (1, nameof (GetArguments));
			var callback = GetCallbacks ()->GetArguments;
			if (callback is null)
				throw MissingCallback (nameof (GetArguments));

			IntPtr nativeArguments = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), &nativeArguments);
			arguments = status == 0 ? CopyValues ((AuthorizationValueVectorNative*) nativeArguments) : null;
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Gets the identifier of the authorization session.</summary>
		public unsafe OSStatus GetSessionId (AuthorizationEngine engine, out NativeHandle sessionId)
		{
			ArgumentNullException.ThrowIfNull (engine);
			EnsureVersion (1, nameof (GetSessionId));
			var callback = GetCallbacks ()->GetSessionId;
			if (callback is null)
				throw MissingCallback (nameof (GetSessionId));

			IntPtr nativeSessionId = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), &nativeSessionId);
			sessionId = nativeSessionId;
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Gets an immutable hint value for the specified key.</summary>
		public unsafe OSStatus GetImmutableHintValue (AuthorizationEngine engine, string key, out byte []? value)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (key);
			EnsureVersion (1, nameof (GetImmutableHintValue));
			var callback = GetCallbacks ()->GetImmutableHintValue;
			if (callback is null)
				throw MissingCallback (nameof (GetImmutableHintValue));

			using var nativeKey = new TransientString (key);
			IntPtr nativeValue = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), nativeKey, &nativeValue);
			value = status == 0 ? CopyValue ((AuthorizationValueNative*) nativeValue) : null;
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Gets a local authentication context containing the current user's credentials.</summary>
		[SupportedOSPlatform ("macos")]
		public unsafe OSStatus GetLAContext (AuthorizationEngine engine, out LAContext? context)
		{
			ArgumentNullException.ThrowIfNull (engine);
			EnsureVersion (2, nameof (GetLAContext));
			var callback = GetCallbacks ()->GetLAContext;
			if (callback is null)
				throw MissingCallback (nameof (GetLAContext));

			IntPtr nativeContext = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), &nativeContext);
			context = status == 0 ? Runtime.GetNSObject<LAContext> (nativeContext, owns: true) : null;
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Gets the token identities available to the authorization mechanism.</summary>
		[SupportedOSPlatform ("macos")]
		public unsafe OSStatus GetTokenIdentities (AuthorizationEngine engine, LAContext context, out NSArray? identities)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (context);
			EnsureVersion (2, nameof (GetTokenIdentities));
			var callback = GetCallbacks ()->GetTokenIdentities;
			if (callback is null)
				throw MissingCallback (nameof (GetTokenIdentities));

			IntPtr nativeIdentities = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), context.GetNonNullHandle (nameof (context)), &nativeIdentities);
			identities = status == 0 ? Runtime.GetNSObject<NSArray> (nativeIdentities, owns: true) : null;
			GC.KeepAlive (context);
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Gets the token watcher used by the authorization mechanism.</summary>
		[SupportedOSPlatform ("macos")]
		public unsafe OSStatus GetTokenWatcher (AuthorizationEngine engine, out TKTokenWatcher? tokenWatcher)
		{
			ArgumentNullException.ThrowIfNull (engine);
			EnsureVersion (3, nameof (GetTokenWatcher));
			var callback = GetCallbacks ()->GetTKTokenWatcher;
			if (callback is null)
				throw MissingCallback (nameof (GetTokenWatcher));

			IntPtr nativeTokenWatcher = IntPtr.Zero;
			var status = callback (engine.GetCheckedHandle (), &nativeTokenWatcher);
			tokenWatcher = status == 0 ? Runtime.GetNSObject<TKTokenWatcher> (nativeTokenWatcher, owns: true) : null;
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Removes a hint value for the specified key.</summary>
		public unsafe OSStatus RemoveHintValue (AuthorizationEngine engine, string key)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (key);
			EnsureVersion (4, nameof (RemoveHintValue));
			var callback = GetCallbacks ()->RemoveHintValue;
			if (callback is null)
				throw MissingCallback (nameof (RemoveHintValue));

			using var nativeKey = new TransientString (key);
			var status = callback (engine.GetCheckedHandle (), nativeKey);
			GC.KeepAlive (engine);
			return status;
		}

		/// <summary>Removes a context value for the specified key.</summary>
		public unsafe OSStatus RemoveContextValue (AuthorizationEngine engine, string key)
		{
			ArgumentNullException.ThrowIfNull (engine);
			ArgumentNullException.ThrowIfNull (key);
			EnsureVersion (4, nameof (RemoveContextValue));
			var callback = GetCallbacks ()->RemoveContextValue;
			if (callback is null)
				throw MissingCallback (nameof (RemoveContextValue));

			using var nativeKey = new TransientString (key);
			var status = callback (engine.GetCheckedHandle (), nativeKey);
			GC.KeepAlive (engine);
			return status;
		}
	}
}
#endif // __MACOS__
