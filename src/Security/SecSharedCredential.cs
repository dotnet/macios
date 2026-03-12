#nullable enable

#if IOS || MONOMAC

using System.ComponentModel;

using CoreFoundation;

namespace Security {

	public static partial class SecSharedCredential {

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios26.2")]
		[ObsoletedOSPlatform ("maccatalyst26.2")]
		[ObsoletedOSPlatform ("macos26.2")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static void SecAddSharedWebCredential (IntPtr /* CFStringRef */ fqdn, IntPtr /* CFStringRef */ account, IntPtr /* CFStringRef */ password,
			BlockLiteral* /* void (^completionHandler)( CFErrorRef error) ) */ completionHandler);

		// This class bridges native block invocations that call into C#
		static internal class ActionTrampoline {
			[UnmanagedCallersOnly]
			internal static unsafe void Invoke (IntPtr block, IntPtr obj)
			{
				var del = BlockLiteral.GetTarget<Action<NSError?>> (block);
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
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("ios26.2")]
		[ObsoletedOSPlatform ("maccatalyst26.2")]
		[ObsoletedOSPlatform ("macos26.2")]
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
				var del = BlockLiteral.GetTarget<Action<SecSharedCredentialInfo []?, NSError?>> (block);
				if (del is not null) {
					var arr = NSArray.DictionaryArrayFromHandleDropNullElements<SecSharedCredentialInfo> (array, (dict) => new SecSharedCredentialInfo (dict));
					del (arr, Runtime.GetNSObject<NSError> (err));
				}
			}
		}

		/// <summary>Asynchronously requests shared web credentials from the iCloud Keychain for the specified domain and account.</summary>
		/// <param name="domainName">
		///   The fully qualified domain name of the website to request credentials for,
		///   or <see langword="null" /> to search all domains in the app's Associated Domains entitlement.
		/// </param>
		/// <param name="account">
		///   The account name to request credentials for,
		///   or <see langword="null" /> to request credentials for all accounts on the matching domain.
		/// </param>
		/// <param name="handler">
		///   A callback invoked when the request completes, receiving an array of
		///   <see cref="SecSharedCredentialInfo" /> with the matching credentials and an <see cref="NSError" /> if the request failed.
		/// </param>
		/// <remarks>
		///   <para>
		///     This method requires that the app has an Associated Domains entitlement configured
		///     for the requested domain. The request may prompt the user for permission.
		///   </para>
		/// </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'ASAuthorizationPasswordRequest' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'ASAuthorizationPasswordRequest' instead.")]
		[ObsoletedOSPlatform ("ios14.0", "Use 'ASAuthorizationPasswordRequest' instead.")]
		[UnsupportedOSPlatform ("tvos")]
		[BindingImpl (BindingImplOptions.Optimizable)]
		public static void RequestSharedWebCredential (string? domainName, string? account, Action<SecSharedCredentialInfo []?, NSError?> handler)
		{
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
