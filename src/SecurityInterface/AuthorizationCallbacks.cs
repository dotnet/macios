#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Foundation;
using ObjCRuntime;
using Security;

namespace SecurityInterface {

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
		/// <summary>The value can be extracted by the client.</summary>
		Extractable = 1 << 0,
		/// <summary>The value is volatile and should not be persisted.</summary>
		Volatile = 1 << 1,
		/// <summary>The value is sticky and persists across mechanism evaluations.</summary>
		Sticky = 1 << 2,
	}

	[StructLayout (LayoutKind.Sequential)]
	unsafe struct AuthorizationValueNative {
		public nuint Length;
		public void* Data;
	}

	[StructLayout (LayoutKind.Sequential)]
	unsafe struct AuthorizationValueVectorNative {
		public uint Count;
		public AuthorizationValueNative* Values;
	}

	[StructLayout (LayoutKind.Sequential)]
	unsafe struct AuthorizationCallbacksNative {
		public uint Version;
		public delegate* unmanaged<IntPtr, AuthorizationResult, int> SetResult;
		public delegate* unmanaged<IntPtr, int> RequestInterrupt;
		public delegate* unmanaged<IntPtr, int> DidDeactivate;
		public delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags*, AuthorizationValueNative**, int> GetContextValue;
		public delegate* unmanaged<IntPtr, IntPtr, AuthorizationContextFlags, AuthorizationValueNative*, int> SetContextValue;
		public delegate* unmanaged<IntPtr, IntPtr, AuthorizationValueNative**, int> GetHintValue;
		public delegate* unmanaged<IntPtr, IntPtr, AuthorizationValueNative*, int> SetHintValue;
		public delegate* unmanaged<IntPtr, AuthorizationValueVectorNative**, int> GetArguments;
		public delegate* unmanaged<IntPtr, IntPtr*, int> GetSessionId;
		public delegate* unmanaged<IntPtr, IntPtr, AuthorizationValueNative**, int> GetImmutableHintValue;
		public delegate* unmanaged<IntPtr, IntPtr*, int> GetLAContext;
		public delegate* unmanaged<IntPtr, IntPtr, IntPtr*, int> GetTokenIdentities;
		public delegate* unmanaged<IntPtr, IntPtr*, int> GetTKTokenWatcher;
		public delegate* unmanaged<IntPtr, IntPtr, int> RemoveHintValue;
		public delegate* unmanaged<IntPtr, IntPtr, int> RemoveContextValue;
	}

	/// <summary>Wraps the native AuthorizationCallbacks structure provided by the authorization engine host for communicating with the Security Server.</summary>
	[SupportedOSPlatform ("macos")]
	public unsafe sealed class AuthorizationCallbacks : INativeObject {
		readonly NativeHandle handle;

		/// <summary>Creates an <see cref="AuthorizationCallbacks" /> from a native callbacks pointer.</summary>
		/// <param name="handle">The pointer to the native AuthorizationCallbacks structure.</param>
		public AuthorizationCallbacks (NativeHandle handle)
		{
			this.handle = handle;
		}

		/// <summary>Gets the native handle for this callbacks structure.</summary>
		public NativeHandle Handle => handle;

		internal static AuthorizationCallbacks? Create (NativeHandle handle)
		{
			if (handle == NativeHandle.Zero)
				return null;
			return new AuthorizationCallbacks (handle);
		}

		AuthorizationCallbacksNative* Native {
			get {
				if (handle == NativeHandle.Zero)
					throw new ObjectDisposedException (nameof (AuthorizationCallbacks));
				return (AuthorizationCallbacksNative*) handle;
			}
		}

		/// <summary>Gets the version of the callbacks structure.</summary>
		public uint Version => Native->Version;

		static byte [] ToNullTerminatedUtf8 (string value)
		{
			var utf8 = Encoding.UTF8.GetBytes (value);
			var rv = new byte [utf8.Length + 1];
			Buffer.BlockCopy (utf8, 0, rv, 0, utf8.Length);
			return rv;
		}

		static byte []? CopyValue (AuthorizationValueNative* value)
		{
			if (value is null || value->Length == 0)
				return null;
			var length = checked((int) value->Length);
			var rv = new byte [length];
			Marshal.Copy ((IntPtr) value->Data, rv, 0, length);
			return rv;
		}

		/// <summary>Sets the result of the current authorization evaluation.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <param name="result">The authorization result.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int SetResult (AuthorizationEngine engine, AuthorizationResult result)
		{
			var rv = Native->SetResult (engine.GetNonNullHandle (nameof (engine)), result);
			GC.KeepAlive (engine);
			return rv;
		}

		/// <summary>Requests an interrupt of the current authorization evaluation.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int RequestInterrupt (AuthorizationEngine engine)
		{
			var rv = Native->RequestInterrupt (engine.GetNonNullHandle (nameof (engine)));
			GC.KeepAlive (engine);
			return rv;
		}

		/// <summary>Notifies the engine that the mechanism has deactivated.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int DidDeactivate (AuthorizationEngine engine)
		{
			var rv = Native->DidDeactivate (engine.GetNonNullHandle (nameof (engine)));
			GC.KeepAlive (engine);
			return rv;
		}

		/// <summary>Gets a context value for the specified key.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <param name="key">The context key name.</param>
		/// <param name="contextFlags">On return, the flags associated with the context value.</param>
		/// <param name="value">On return, the context value as a byte array, or <see langword="null" /> if not found.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int GetContextValue (AuthorizationEngine engine, string key, out AuthorizationContextFlags contextFlags, out byte []? value)
		{
			if (key is null)
				ThrowHelper.ThrowArgumentNullException (nameof (key));
			fixed (byte* keyPtr = ToNullTerminatedUtf8 (key)) {
				AuthorizationContextFlags flags = default;
				AuthorizationValueNative* nativeValue = null;
				var rv = Native->GetContextValue (engine.GetNonNullHandle (nameof (engine)), (IntPtr) keyPtr, &flags, &nativeValue);
				contextFlags = flags;
				value = rv == 0 ? CopyValue (nativeValue) : null;
				GC.KeepAlive (engine);
				return rv;
			}
		}

		/// <summary>Sets a context value for the specified key.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <param name="key">The context key name.</param>
		/// <param name="contextFlags">The flags to associate with the value.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int SetContextValue (AuthorizationEngine engine, string key, AuthorizationContextFlags contextFlags, byte [] value)
		{
			if (key is null)
				ThrowHelper.ThrowArgumentNullException (nameof (key));
			if (value is null)
				ThrowHelper.ThrowArgumentNullException (nameof (value));
			fixed (byte* keyPtr = ToNullTerminatedUtf8 (key))
			fixed (byte* valuePtr = value) {
				var nativeValue = new AuthorizationValueNative { Length = (nuint) value.Length, Data = valuePtr };
				var rv = Native->SetContextValue (engine.GetNonNullHandle (nameof (engine)), (IntPtr) keyPtr, contextFlags, &nativeValue);
				GC.KeepAlive (engine);
				return rv;
			}
		}

		/// <summary>Gets a hint value for the specified key.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <param name="key">The hint key name.</param>
		/// <param name="value">On return, the hint value as a byte array, or <see langword="null" /> if not found.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int GetHintValue (AuthorizationEngine engine, string key, out byte []? value)
		{
			if (key is null)
				ThrowHelper.ThrowArgumentNullException (nameof (key));
			fixed (byte* keyPtr = ToNullTerminatedUtf8 (key)) {
				AuthorizationValueNative* nativeValue = null;
				var rv = Native->GetHintValue (engine.GetNonNullHandle (nameof (engine)), (IntPtr) keyPtr, &nativeValue);
				value = rv == 0 ? CopyValue (nativeValue) : null;
				GC.KeepAlive (engine);
				return rv;
			}
		}

		/// <summary>Sets a hint value for the specified key.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <param name="key">The hint key name.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int SetHintValue (AuthorizationEngine engine, string key, byte [] value)
		{
			if (key is null)
				ThrowHelper.ThrowArgumentNullException (nameof (key));
			if (value is null)
				ThrowHelper.ThrowArgumentNullException (nameof (value));
			fixed (byte* keyPtr = ToNullTerminatedUtf8 (key))
			fixed (byte* valuePtr = value) {
				var nativeValue = new AuthorizationValueNative { Length = (nuint) value.Length, Data = valuePtr };
				var rv = Native->SetHintValue (engine.GetNonNullHandle (nameof (engine)), (IntPtr) keyPtr, &nativeValue);
				GC.KeepAlive (engine);
				return rv;
			}
		}

		/// <summary>Removes a hint value for the specified key.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <param name="key">The hint key name to remove.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int RemoveHintValue (AuthorizationEngine engine, string key)
		{
			if (key is null)
				ThrowHelper.ThrowArgumentNullException (nameof (key));
			fixed (byte* keyPtr = ToNullTerminatedUtf8 (key)) {
				var rv = Native->RemoveHintValue (engine.GetNonNullHandle (nameof (engine)), (IntPtr) keyPtr);
				GC.KeepAlive (engine);
				return rv;
			}
		}

		/// <summary>Removes a context value for the specified key.</summary>
		/// <param name="engine">The authorization engine.</param>
		/// <param name="key">The context key name to remove.</param>
		/// <returns>An OSStatus code; 0 on success.</returns>
		public int RemoveContextValue (AuthorizationEngine engine, string key)
		{
			if (key is null)
				ThrowHelper.ThrowArgumentNullException (nameof (key));
			fixed (byte* keyPtr = ToNullTerminatedUtf8 (key)) {
				var rv = Native->RemoveContextValue (engine.GetNonNullHandle (nameof (engine)), (IntPtr) keyPtr);
				GC.KeepAlive (engine);
				return rv;
			}
		}
	}
}
