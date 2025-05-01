#nullable enable

#if IOS || MONOMAC

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace Security {

	public static partial class SecSharedCredential {

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static void SecAddSharedWebCredential (IntPtr /* CFStringRef */ fqdn, IntPtr /* CFStringRef */ account, IntPtr /* CFStringRef */ password,
			BlockLiteral* /* void (^completionHandler)( CFErrorRef error) ) */ completionHandler);

		// This class bridges native block invocations that call into C#
		static internal class ActionTrampoline {
			[UnmanagedCallersOnly]
			internal static unsafe void Invoke (IntPtr block, IntPtr obj)
			{
				var descriptor = (BlockLiteral*) block;
				var del = (global::System.Action<NSError?>) (descriptor->Target);
				if (del is not null) {
					del (Runtime.GetNSObject<NSError> (obj));
				}
			}
		}

		/// <param name="domainName">To be added.</param>
		///         <param name="account">To be added.</param>
		///         <param name="password">To be added.</param>
		///         <param name="handler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void AddSharedWebCredential (string domainName, string account, string password, Action<NSError> handler)
		{
			if (domainName is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (domainName));
			if (account is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (account));
			// we need to create our own block literal. We can reuse the SDActionArity1V12 which is generated and takes a
			// NSError because a CFError is a toll-free bridget to CFError
			unsafe {
				using var nsDomain = new NSString (domainName);
				using var nsAccount = new NSString (account);
				using var nsPassword = (NSString?) password;

				delegate* unmanaged<IntPtr, IntPtr, void> trampoline = &ActionTrampoline.Invoke;
				using var block = new BlockLiteral (trampoline, handler, typeof (ActionTrampoline), nameof (ActionTrampoline.Invoke));
				SecAddSharedWebCredential (nsDomain.Handle, nsAccount.Handle, nsPassword.GetHandle (), &block);
				GC.KeepAlive (nsDomain);
				GC.KeepAlive (nsAccount);
				GC.KeepAlive (nsPassword);
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("ios14.0")]
		[UnsupportedOSPlatform ("tvos")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static void SecRequestSharedWebCredential (IntPtr /* CFStringRef */ fqdn, IntPtr /* CFStringRef */ account,
			BlockLiteral* /* void (^completionHandler)( CFArrayRef credentials, CFErrorRef error) */ completionHandler);

		//
		// This class bridges native block invocations that call into C# because we cannot use the decorator, we have to create
		// it for our use here.
		//
		static internal class ArrayErrorActionTrampoline {
			[UnmanagedCallersOnly]
			internal static unsafe void Invoke (IntPtr block, IntPtr array, IntPtr err)
			{
				var descriptor = (BlockLiteral*) block;
				var del = (global::System.Action<NSArray?, NSError?>) (descriptor->Target);
				if (del is not null)
					del (Runtime.GetNSObject<NSArray> (array), Runtime.GetNSObject<NSError> (err));
			}
		}

		/// <param name="domainName">To be added.</param>
		///         <param name="account">To be added.</param>
		///         <param name="handler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'ASAuthorizationPasswordRequest' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'ASAuthorizationPasswordRequest' instead.")]
		[ObsoletedOSPlatform ("ios14.0", "Use 'ASAuthorizationPasswordRequest' instead.")]
		[UnsupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void RequestSharedWebCredential (string domainName, string account, Action<SecSharedCredentialInfo [], NSError> handler)
		{
			Action<NSArray, NSError> onComplete = (NSArray a, NSError e) => {
				var creds = new SecSharedCredentialInfo [a.Count];
				int i = 0;
				foreach (var dict in NSArray.FromArrayNative<NSDictionary> (a)) {
					creds [i++] = new SecSharedCredentialInfo (dict);
				}
				handler (creds, e);
			};
			// we need to create our own block literal.
			using var nsDomain = (NSString?) domainName;
			using var nsAccount = (NSString?) account;

			unsafe {
				delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> trampoline = &ArrayErrorActionTrampoline.Invoke;
				using var block = new BlockLiteral (trampoline, handler, typeof (ArrayErrorActionTrampoline), nameof (ArrayErrorActionTrampoline.Invoke));
				SecRequestSharedWebCredential (nsDomain.GetHandle (), nsAccount.GetHandle (), &block);
				GC.KeepAlive (nsDomain);
				GC.KeepAlive (nsAccount);
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* CFStringRef */ SecCreateSharedWebCredentialPassword ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static string? CreateSharedWebCredentialPassword ()
		{
			var handle = SecCreateSharedWebCredentialPassword ();
			var str = CFString.FromHandle (handle);
			NSObject.DangerousRelease (handle);
			return str;
		}
	}

}

#endif  // IOS
