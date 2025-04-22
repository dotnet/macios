// 
// Authorization.cs: 
//
// Authors: Miguel de Icaza
//     
// Copyright 2012 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#nullable enable

#if MONOMAC || __MACCATALYST__

using ObjCRuntime;
using Foundation;
using System;
using System.Runtime.InteropServices;

namespace Security {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	// Untyped enum in ObjC
	public enum AuthorizationStatus {
		/// <summary>To be added.</summary>
		Success = 0,
		/// <summary>To be added.</summary>
		InvalidSet = -60001,
		/// <summary>To be added.</summary>
		InvalidRef = -60002,
		/// <summary>To be added.</summary>
		InvalidTag = -60003,
		/// <summary>To be added.</summary>
		InvalidPointer = -60004,
		/// <summary>To be added.</summary>
		Denied = -60005,
		/// <summary>To be added.</summary>
		Canceled = -60006,
		/// <summary>To be added.</summary>
		InteractionNotAllowed = -60007,
		/// <summary>To be added.</summary>
		Internal = -60008,
		/// <summary>To be added.</summary>
		ExternalizeNotAllowed = -60009,
		/// <summary>To be added.</summary>
		InternalizeNotAllowed = -60010,
		/// <summary>To be added.</summary>
		InvalidFlags = -60011,
		/// <summary>To be added.</summary>
		ToolExecuteFailure = -60031,
		/// <summary>To be added.</summary>
		ToolEnvironmentError = -60032,
		/// <summary>To be added.</summary>
		BadAddress = -60033,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	// typedef UInt32 AuthorizationFlags;
	[Flags]
	public enum AuthorizationFlags : int {
		/// <summary>To be added.</summary>
		Defaults,
		/// <summary>To be added.</summary>
		InteractionAllowed = 1 << 0,
		/// <summary>To be added.</summary>
		ExtendRights = 1 << 1,
		/// <summary>To be added.</summary>
		PartialRights = 1 << 2,
		/// <summary>To be added.</summary>
		DestroyRights = 1 << 3,
		/// <summary>To be added.</summary>
		PreAuthorize = 1 << 4,
		[SupportedOSPlatform ("maccatalyst17.0")]
		[SupportedOSPlatform ("macos14.0")]
		SkipInternalAuth = 1 << 9,
		NoData = 1 << 20,
	}

	//
	// For ease of use, we let the user pass the AuthorizationParameters, and we
	// create the structure for them with the proper data
	//
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public class AuthorizationParameters {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? PathToSystemPrivilegeTool;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? Prompt;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? IconPath;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public class AuthorizationEnvironment {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? Username;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public string? Password;
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public bool AddToSharedCredentialPool;
	}

	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[StructLayout (LayoutKind.Sequential)]
	struct AuthorizationItem {
		public IntPtr /* AuthorizationString = const char * */ name;
		public nint /* size_t */ valueLen;
		public IntPtr /* void* */ value;
		public int /* UInt32 */ flags;  // zero
	}

	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	unsafe struct AuthorizationItemSet {
		public int /* UInt32 */ count;
		public AuthorizationItem* /* AuthorizationItem* */ ptrToAuthorization;
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	public unsafe class Authorization : DisposableObject {
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static int /* OSStatus = int */ AuthorizationCreate (AuthorizationItemSet* rights, AuthorizationItemSet* environment, AuthorizationFlags flags, IntPtr* auth);

		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("maccatalyst", "Use the Service Management framework or the launchd-launched helper tool instead.")]
		[ObsoletedOSPlatform ("macos", "Use the Service Management framework or the launchd-launched helper tool instead.")]
		[DllImport (Constants.SecurityLibrary)]
		extern static int /* OSStatus = int */ AuthorizationExecuteWithPrivileges (IntPtr handle, IntPtr pathToTool, AuthorizationFlags flags, IntPtr args, IntPtr FILEPtr);

		[DllImport (Constants.SecurityLibrary)]
		extern static int /* OSStatus = int */ AuthorizationFree (IntPtr handle, AuthorizationFlags flags);

		[Preserve (Conditional = true)]
		internal Authorization (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

		/// <param name="pathToTool">To be added.</param>
		///         <param name="flags">To be added.</param>
		///         <param name="args">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("maccatalyst", "Use the Service Management framework or the launchd-launched helper tool instead.")]
		[ObsoletedOSPlatform ("macos", "Use the Service Management framework or the launchd-launched helper tool instead.")]
		public int ExecuteWithPrivileges (string pathToTool, AuthorizationFlags flags, string []? args)
		{
			string? []? arguments = args!;

			if (arguments is not null) {
				// The arguments array must be null-terminated, so make sure that's the case
				if (arguments.Length == 0) {
					arguments = new string? [] { null };
				} else if (arguments [arguments.Length - 1] is not null) {
					var array = new string? [arguments.Length + 1];
					arguments.CopyTo (array, 0);
					arguments = array;
				}
			}
			using var pathToToolStr = new TransientString (pathToTool);
			var argsPtr = TransientString.AllocStringArray (arguments);
			var retval = AuthorizationExecuteWithPrivileges (Handle, pathToToolStr, flags, argsPtr, IntPtr.Zero);
			TransientString.FreeStringArray (argsPtr, args is null ? 0 : args.Length);
			return retval;
		}

		protected override void Dispose (bool disposing)
		{
			Dispose (0, disposing);
		}

		/// <param name="flags">To be added.</param>
		///         <param name="disposing">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public virtual void Dispose (AuthorizationFlags flags, bool disposing)
		{
			if (Handle != IntPtr.Zero && Owns)
				AuthorizationFree (Handle, flags);
			base.Dispose (disposing);
		}

		/// <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static Authorization? Create (AuthorizationFlags flags)
		{
			return Create (null, null, flags);
		}

		static void EncodeString (ref AuthorizationItem item, string key, string? value)
		{
			item.name = Marshal.StringToHGlobalAuto (key);
			if (value is not null) {
				item.value = Marshal.StringToHGlobalAuto (value);
				item.valueLen = value.Length;
			}
		}

		/// <param name="parameters">To be added.</param>
		///         <param name="environment">To be added.</param>
		///         <param name="flags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static Authorization? Create (AuthorizationParameters? parameters, AuthorizationEnvironment? environment, AuthorizationFlags flags)
		{
			AuthorizationItemSet pars = new AuthorizationItemSet ();
			AuthorizationItemSet* ppars = null;
			AuthorizationItem* pitems = null;
			AuthorizationItemSet env = new AuthorizationItemSet ();
			AuthorizationItemSet* penv = null;
			AuthorizationItem* eitems = null;
			int code;
			IntPtr auth;

			try {
				unsafe {
					if (parameters is not null) {
						ppars = &pars;
						pars.ptrToAuthorization = (AuthorizationItem*) Marshal.AllocHGlobal (sizeof (AuthorizationItem) * 2);
						if (parameters.PathToSystemPrivilegeTool is not null)
							EncodeString (ref pars.ptrToAuthorization [pars.count++], "system.privilege.admin", parameters.PathToSystemPrivilegeTool);
						if (parameters.IconPath is not null)
							EncodeString (ref pars.ptrToAuthorization [pars.count++], "icon", parameters.IconPath);
					}
					if (environment is not null || (parameters is not null && parameters.Prompt is not null)) {
						penv = &env;
						env.ptrToAuthorization = (AuthorizationItem*) Marshal.AllocHGlobal (sizeof (AuthorizationItem) * 4);
						if (environment is not null) {
							if (environment.Username is not null)
								EncodeString (ref env.ptrToAuthorization [env.count++], "username", environment.Username);
							if (environment.Password is not null)
								EncodeString (ref env.ptrToAuthorization [env.count++], "password", environment.Password);
							if (environment.AddToSharedCredentialPool)
								EncodeString (ref env.ptrToAuthorization [env.count++], "shared", null);
						}
						if (parameters is not null) {
							if (parameters.Prompt is not null)
								EncodeString (ref env.ptrToAuthorization [env.count++], "prompt", parameters.Prompt);
						}
					}
					code = AuthorizationCreate (ppars, penv, flags, &auth);
					if (code != 0)
						return null;
					return new Authorization (auth, true);
				}
			} finally {
				if (ppars is not null) {
					for (int i = 0; i < pars.count; i++) {
						Marshal.FreeHGlobal (pars.ptrToAuthorization [i].name);
						Marshal.FreeHGlobal (pars.ptrToAuthorization [i].value);
					}
					Marshal.FreeHGlobal ((IntPtr) pars.ptrToAuthorization);
				}
				if (penv is not null) {
					for (int i = 0; i < env.count; i++) {
						Marshal.FreeHGlobal (env.ptrToAuthorization [i].name);
						if (env.ptrToAuthorization [i].value != IntPtr.Zero)
							Marshal.FreeHGlobal (env.ptrToAuthorization [i].value);
					}
					Marshal.FreeHGlobal ((IntPtr) env.ptrToAuthorization);
				}
			}
		}
	}
}

#endif // MONOMAC
