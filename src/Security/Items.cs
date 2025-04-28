// 
// Items.cs: Implements the KeyChain query access APIs
//
// We use strong types and a helper SecQuery class to simplify the
// creation of the dictionary used to query the Keychain
// 
// Authors:
//	Miguel de Icaza
//	Sebastien Pouliot
//     
// Copyright 2010 Novell, Inc
// Copyright 2011-2016 Xamarin Inc
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

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using ObjCRuntime;
#if !MONOMAC
using UIKit;
#endif

namespace Security {

	/// <summary>The kind of SecRecord.</summary>
	///     <remarks>A SecRecord can represent one of the following values.</remarks>
	public enum SecKind {
		/// <summary>The SecRecord stores an internet password.</summary>
		InternetPassword,
		/// <summary>The SecRecord stores a password.</summary>
		GenericPassword,
		/// <summary>The SecRecord represents a certificate.</summary>
		Certificate,
		/// <summary>The SecRecord represents a cryptographic key.</summary>
		Key,
		/// <summary>The SecRecord represents an identity</summary>
		Identity,
	}

	// manually mapped to KeysAccessible
	/// <summary>An enumeration whose values specify when a keychain item should be readable.</summary>
	///     <remarks>
	///       <para>There are a number of axis to consider for the accessible settings of an item.</para>
	///       <para>Whether the information should be made accessible without entering a passcode, the device being unlocked or always available.</para>
	///       <para>Another one is whether the information should be locked to this device, or whether the information can migrate to a new device via a backup restore.</para>
	///       <para>This value is used by the <see cref="T:Security.SecAccessControl" /> constructor and surfaced as a property of the <see cref="T:Security.SecRecord" />.</para>
	///     </remarks>
	public enum SecAccessible {
		/// <summary>Invalid value.</summary>
		Invalid = -1,
		/// <summary>The data is only available when the device is unlocked.</summary>
		WhenUnlocked,
		/// <summary>The data is only available after the first time the device has been unlocked after booting.</summary>
		AfterFirstUnlock,
		/// <summary>Always available.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos", "Use 'AfterFirstUnlock' or a better suited option instead.")]
		[ObsoletedOSPlatform ("ios", "Use 'AfterFirstUnlock' or a better suited option instead.")]
		[ObsoletedOSPlatform ("tvos", "Use 'AfterFirstUnlock' or a better suited option instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'AfterFirstUnlock' or a better suited option instead.")]
		Always,
		/// <summary>Limits access to the item to this device and the device being unlocked.</summary>
		WhenUnlockedThisDeviceOnly,
		/// <summary>The data is only available after the first time the device has been unlocked after booting.</summary>
		AfterFirstUnlockThisDeviceOnly,
		/// <summary>Always available.</summary>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos", "Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.")]
		[ObsoletedOSPlatform ("ios", "Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.")]
		[ObsoletedOSPlatform ("tvos", "Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.")]
		AlwaysThisDeviceOnly,
		/// <summary>Limits access to the item to both this device and requires a passcode to be set and the data is only available if the device is currently unlocked.</summary>
		WhenPasscodeSetThisDeviceOnly,
	}

	/// <summary>Protocol used for InternetPasswords</summary>
	///     <remarks>To be added.</remarks>
	public enum SecProtocol {
		/// <summary>Invalid</summary>
		Invalid = -1,
		/// <summary>FTP service</summary>
		Ftp,
		/// <summary>FTP account</summary>
		FtpAccount,
		/// <summary>HTTP server</summary>
		Http,
		/// <summary>Internet Relay Chat</summary>
		Irc,
		/// <summary>NTTP</summary>
		Nntp,
		/// <summary>Post office protocol for email.</summary>
		Pop3,
		/// <summary>Simple Mail Transfer Protocol service.</summary>
		Smtp,
		/// <summary>SOCKS proxy</summary>
		Socks,
		/// <summary>IMAP protocol</summary>
		Imap,
		/// <summary>LDAP</summary>
		Ldap,
		/// <summary>AppleTalk service</summary>
		AppleTalk,
		/// <summary>To be added.</summary>
		Afp,
		/// <summary>Telnet service</summary>
		Telnet,
		/// <summary>Secure Shell</summary>
		Ssh,
		/// <summary>FTP over SSL/TLS.</summary>
		Ftps,
		/// <summary>HTTP over SSL/TLS.</summary>
		Https,
		/// <summary>HTTP Proxy</summary>
		HttpProxy,
		/// <summary>HTTP Proxy over SSL/TLS</summary>
		HttpsProxy,
		/// <summary>FTP proxy</summary>
		FtpProxy,
		/// <summary>CIFS/SMB file sharing or print share.</summary>
		Smb,
		/// <summary>RTSP</summary>
		Rtsp,
		/// <summary>RTSP Proxy</summary>
		RtspProxy,
		/// <summary>To be added.</summary>
		Daap,
		/// <summary>To be added.</summary>
		Eppc,
		/// <summary>To be added.</summary>
		Ipp,
		/// <summary>NTTP over SSL/TLS.</summary>
		Nntps,
		/// <summary>Ldap over SSL/TLS.</summary>
		Ldaps,
		/// <summary>Telnet over SSL/TLS.</summary>
		Telnets,
		/// <summary>Imap over SSL/TLS.</summary>
		Imaps,
		/// <summary>IRC over SSL/TLS.</summary>
		Ircs,
		/// <summary>POP3 over SSL/TLS</summary>
		Pop3s,
	}

	/// <summary>An enumeration whose values specify various types of authentication. Used with the <see cref="P:Security.SecRecord.AuthenticationType" /> property.</summary>
	///     <remarks>
	///       <para />
	///     </remarks>
	public enum SecAuthenticationType {
		/// <summary>Invalid authentication setting</summary>
		Invalid = -1,
		Any = 0,
		/// <summary>NTLM authentication</summary>
		Ntlm = 1835824238,
		/// <summary>Microsoft Network authentication</summary>
		Msn = 1634628461,
		/// <summary>Distributed Password Authentication</summary>
		Dpa = 1633775716,
		/// <summary>Remote password authentication.</summary>
		Rpa = 1633775730,
		/// <summary>HTTP Basic authentication.</summary>
		HttpBasic = 1886680168,
		/// <summary>HTTP Digest authentication</summary>
		HttpDigest = 1685353576,
		/// <summary>HTTP Form authentication.</summary>
		HtmlForm = 1836216166,
		/// <summary>Default authentication type</summary>
		Default = 1953261156,
	}

	/// <include file="../../docs/api/Security/SecKeyChain.xml" path="/Documentation/Docs[@DocId='T:Security.SecKeyChain']/*" />
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class SecKeyChain : INativeObject {

		internal SecKeyChain (NativeHandle handle)
		{
			Handle = handle;
		}

		/// <summary>Handle (pointer) to the unmanaged object representation.</summary>
		///         <value>A pointer</value>
		///         <remarks>This IntPtr is a handle to the underlying unmanaged representation for this object.</remarks>
		public NativeHandle Handle { get; internal set; }

		static NSNumber? SetLimit (NSMutableDictionary dict, int max)
		{
			NSNumber? n = null;
			IntPtr val;
			if (max == -1)
				val = SecMatchLimit.MatchLimitAll;
			else if (max == 1)
				val = SecMatchLimit.MatchLimitOne;
			else {
				n = NSNumber.FromInt32 (max);
#pragma warning disable RBI0014
				val = n.Handle;
#pragma warning restore RBI0014
			}

			dict.LowlevelSetObject (val, SecItem.MatchLimit);
			GC.KeepAlive (n);
			return n;
		}

		/// <include file="../../docs/api/Security/SecKeyChain.xml" path="/Documentation/Docs[@DocId='M:Security.SecKeyChain.QueryAsData(Security.SecRecord,System.Boolean,Security.SecStatusCode@)']/*" />
		public static NSData? QueryAsData (SecRecord query, bool wantPersistentReference, out SecStatusCode status)
		{
			if (query is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (query));

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)) {
				SetLimit (copy, 1);
				if (wantPersistentReference)
					copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnPersistentRef);
				else
					copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnData);

				status = SecItem.SecItemCopyMatching (copy.Handle, out var ptr);
				if (status == SecStatusCode.Success)
					return Runtime.GetNSObject<NSData> (ptr, true);
				return null;
			}
		}

		/// <include file="../../docs/api/Security/SecKeyChain.xml" path="/Documentation/Docs[@DocId='M:Security.SecKeyChain.QueryAsData(Security.SecRecord,System.Boolean,System.Int32,Security.SecStatusCode@)']/*" />
		public static NSData []? QueryAsData (SecRecord query, bool wantPersistentReference, int max, out SecStatusCode status)
		{
			if (query is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (query));

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)) {
				var n = SetLimit (copy, max);
				if (wantPersistentReference)
					copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnPersistentRef);
				else
					copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnData);

				status = SecItem.SecItemCopyMatching (copy.Handle, out var ptr);
				n = null;
				if (status == SecStatusCode.Success) {
					// From the header docs, it's guaranteed the function will return an array only if we pass max > 1.

					// By default, this function returns only the first match found.
					// To obtain more than one matching item at a time, specify the search key kSecMatchLimit with a value greater than 1.
					// The result will be an object of type CFArrayRef containing up to that number of matching items.
					if (max == 1)
						return new NSData [] { Runtime.GetNSObject<NSData> (ptr, true)! };

					return CFArray.ArrayFromHandle<NSData> (ptr, true)!;
				}
				return null;
			}
		}

		/// <include file="../../docs/api/Security/SecKeyChain.xml" path="/Documentation/Docs[@DocId='M:Security.SecKeyChain.QueryAsData(Security.SecRecord)']/*" />
		public static NSData? QueryAsData (SecRecord query)
		{
			SecStatusCode status;
			return QueryAsData (query, false, out status);
		}

		/// <include file="../../docs/api/Security/SecKeyChain.xml" path="/Documentation/Docs[@DocId='M:Security.SecKeyChain.QueryAsData(Security.SecRecord,System.Int32)']/*" />
		public static NSData []? QueryAsData (SecRecord query, int max)
		{
			SecStatusCode status;
			return QueryAsData (query, false, max, out status);
		}

		/// <param name="query">The query used to lookup the value on the keychain.</param>
		///         <param name="result">Returns the status code from calling SecItemCopyMatching.</param>
		///         <summary>Fetches a single SecRecord.</summary>
		///         <returns>Returns a stronglty typed SecRecord.</returns>
		///         <remarks>
		///           <para>
		/// 	    Unlike the <see cref="M:Security.SecKeyChain.QueryAsData(Security.SecRecord,System.Boolean,System.Int32,Security.SecStatusCode@)" />
		/// 	    methods which return a binary blob inside an NSData, this
		/// 	    returns a strongly typed SecRecord that you can easily
		/// 	    inspect.
		/// 	  </para>
		///           <para>
		/// 	    This is the strongly typed equivalent of calling the
		/// 	    Security's framework SecItemCopyMatching method with the
		/// 	    kSecReturnData set to true, kSecReturnAttributes set to
		/// 	    true and kSecMatchLimit set to 1, forcing a single record
		/// 	    to be returned.
		/// 	  </para>
		///         </remarks>
		public static SecRecord? QueryAsRecord (SecRecord query, out SecStatusCode result)
		{
			if (query is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (query));

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)) {
				SetLimit (copy, 1);
				copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnAttributes);
				copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnData);
				result = SecItem.SecItemCopyMatching (copy.Handle, out var ptr);
				if (result == SecStatusCode.Success)
					return new SecRecord (new NSMutableDictionary (ptr, true));
				return null;
			}
		}

		/// <include file="../../docs/api/Security/SecKeyChain.xml" path="/Documentation/Docs[@DocId='M:Security.SecKeyChain.QueryAsRecord(Security.SecRecord,System.Int32,Security.SecStatusCode@)']/*" />
		public static SecRecord []? QueryAsRecord (SecRecord query, int max, out SecStatusCode result)
		{
			if (query is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (query));

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)) {
				copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnAttributes);
				copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnData);
				var n = SetLimit (copy, max);

				result = SecItem.SecItemCopyMatching (copy.Handle, out var ptr);
				n = null;
				if (result == SecStatusCode.Success)
					return CFArray.ArrayFromHandleFunc<SecRecord> (ptr, (element) => {
						var dictionary = Runtime.GetNSObject<NSMutableDictionary> (element, false)!;
						return new SecRecord (dictionary);
					}, releaseHandle: true)!;
				return null;
			}
		}

		/// <param name="query">To be added.</param>
		///         <param name="max">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static INativeObject []? QueryAsReference (SecRecord query, int max, out SecStatusCode result)
		{
			if (query is null) {
				result = SecStatusCode.Param;
				return null;
			}

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)) {
				copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnRef);
				SetLimit (copy, max);

				result = SecItem.SecItemCopyMatching (copy.Handle, out var ptr);
				if ((result == SecStatusCode.Success) && (ptr != IntPtr.Zero)) {
					var array = CFArray.ArrayFromHandleFunc<INativeObject> (ptr, p => {
						nint cfType = CFType.GetTypeID (p);
						CFObject.CFRetain (p);

						if (cfType == SecCertificate.GetTypeID ())
							return new SecCertificate (p, true);
						else if (cfType == SecKey.GetTypeID ())
							return new SecKey (p, true);
						else if (cfType == SecIdentity.GetTypeID ())
							return new SecIdentity (p, true);
						else {
							CFObject.CFRelease (p);
							throw new Exception (String.Format ("Unexpected type: 0x{0:x}", cfType));
						}
					}, releaseHandle: true)!;
					return array;
				}
				return null;
			}
		}

		/// <param name="record">A populated record.</param>
		///         <summary>Adds the specified record to the keychain.</summary>
		///         <returns>The result of the operation.</returns>
		///         <remarks>To be added.</remarks>
		public static SecStatusCode Add (SecRecord record)
		{
			if (record is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (record));
			return SecItem.SecItemAdd (record.queryDict.Handle, IntPtr.Zero);

		}

		/// <param name="record">Record to be removed from the keychain.</param>
		///         <summary>Removes the specified record from the keychain.</summary>
		///         <returns>The status code from performing the remove operation.</returns>
		///         <remarks>This calls the SecItemDelete method on the keychain.</remarks>
		public static SecStatusCode Remove (SecRecord record)
		{
			if (record is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (record));
			return SecItem.SecItemDelete (record.queryDict.Handle);
		}

		/// <param name="query">The query to use to update the records on the keychain.</param>
		///         <param name="newAttributes">The updated record value to store.</param>
		///         <summary>Updates the record matching the query with the provided data.</summary>
		///         <returns>Status code of calling SecItemUpdate.</returns>
		///         <remarks>
		///           <para>
		/// 	    This performs an update on the keychain.
		/// 	  </para>
		///           <para>
		/// 	    This calls the SecItemUpdate method.
		/// 	  </para>
		///         </remarks>
		public static SecStatusCode Update (SecRecord query, SecRecord newAttributes)
		{
			if (query is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (query));
			if (newAttributes is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (newAttributes));

			return SecItem.SecItemUpdate (query.queryDict.Handle, newAttributes.queryDict.Handle);

		}
#if MONOMAC
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainAddGenericPassword (
			IntPtr keychain,
			int serviceNameLength,
			byte []? serviceName,
			int accountNameLength,
			byte []? accountName,
			int passwordLength,
			byte [] passwordData,
			IntPtr itemRef);

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode SecKeychainFindGenericPassword (
			IntPtr keychainOrArray,
			int serviceNameLength,
			byte* serviceName,
			int accountNameLength,
			byte* accountName,
			int* passwordLength,
			IntPtr* passwordData,
			IntPtr itemRef);

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainAddInternetPassword (
			IntPtr keychain,
			int serverNameLength,
			byte []? serverName,
			int securityDomainLength,
			byte []? securityDomain,
			int accountNameLength,
			byte []? accountName,
			int pathLength,
			byte []? path,
			short port,
			IntPtr protocol,
			IntPtr authenticationType,
			int passwordLength,
			byte [] passwordData,
			IntPtr itemRef);

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode SecKeychainFindInternetPassword (
			IntPtr keychain,
			int serverNameLength,
			byte* serverName,
			int securityDomainLength,
			byte* securityDomain,
			int accountNameLength,
			byte* accountName,
			int pathLength,
			byte* path,
			short port,
			IntPtr protocol,
			IntPtr authenticationType,
			int* passwordLength,
			IntPtr* passwordData,
			IntPtr itemRef);

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode SecKeychainItemFreeContent (IntPtr attrList, IntPtr data);

		/// <param name="serverName">To be added.</param>
		///         <param name="accountName">To be added.</param>
		///         <param name="password">To be added.</param>
		///         <param name="protocolType">To be added.</param>
		///         <param name="port">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <param name="authenticationType">To be added.</param>
		///         <param name="securityDomain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static SecStatusCode AddInternetPassword (
			string serverName,
			string accountName,
			byte [] password,
			SecProtocol protocolType = SecProtocol.Http,
			short port = 0,
			string? path = null,
			SecAuthenticationType authenticationType = SecAuthenticationType.Default,
			string? securityDomain = null)
		{
			byte []? serverNameBytes = null;
			byte []? securityDomainBytes = null;
			byte []? accountNameBytes = null;
			byte []? pathBytes = null;

			if (!String.IsNullOrEmpty (serverName))
				serverNameBytes = System.Text.Encoding.UTF8.GetBytes (serverName);

			if (!String.IsNullOrEmpty (securityDomain))
				securityDomainBytes = System.Text.Encoding.UTF8.GetBytes (securityDomain);

			if (!String.IsNullOrEmpty (accountName))
				accountNameBytes = System.Text.Encoding.UTF8.GetBytes (accountName);

			if (!String.IsNullOrEmpty (path))
				pathBytes = System.Text.Encoding.UTF8.GetBytes (path);

			return SecKeychainAddInternetPassword (
				IntPtr.Zero,
				serverNameBytes?.Length ?? 0,
				serverNameBytes,
				securityDomainBytes?.Length ?? 0,
				securityDomainBytes,
				accountNameBytes?.Length ?? 0,
				accountNameBytes,
				pathBytes?.Length ?? 0,
				pathBytes,
				port,
				SecProtocolKeys.FromSecProtocol (protocolType),
				KeysAuthenticationType.FromSecAuthenticationType (authenticationType),
				password?.Length ?? 0,
				password!,
				IntPtr.Zero);
		}


		/// <param name="serverName">To be added.</param>
		///         <param name="accountName">To be added.</param>
		///         <param name="password">To be added.</param>
		///         <param name="protocolType">To be added.</param>
		///         <param name="port">To be added.</param>
		///         <param name="path">To be added.</param>
		///         <param name="authenticationType">To be added.</param>
		///         <param name="securityDomain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static SecStatusCode FindInternetPassword (
			string serverName,
			string accountName,
			out byte []? password,
			SecProtocol protocolType = SecProtocol.Http,
			short port = 0,
			string? path = null,
			SecAuthenticationType authenticationType = SecAuthenticationType.Default,
			string? securityDomain = null)
		{
			password = null;

			byte []? serverBytes = null;
			byte []? securityDomainBytes = null;
			byte []? accountNameBytes = null;
			byte []? pathBytes = null;

			IntPtr passwordPtr = IntPtr.Zero;

			try {
				if (!String.IsNullOrEmpty (serverName))
					serverBytes = System.Text.Encoding.UTF8.GetBytes (serverName);

				if (!String.IsNullOrEmpty (securityDomain))
					securityDomainBytes = System.Text.Encoding.UTF8.GetBytes (securityDomain);

				if (!String.IsNullOrEmpty (accountName))
					accountNameBytes = System.Text.Encoding.UTF8.GetBytes (accountName);

				if (!String.IsNullOrEmpty (path))
					pathBytes = System.Text.Encoding.UTF8.GetBytes (path);

				int passwordLength = 0;

				SecStatusCode code;
				unsafe {
					fixed (byte* serverBytesPtr = serverBytes) {
						fixed (byte* securityDomainBytesPtr = securityDomainBytes) {
							fixed (byte* accountNameBytesPtr = accountNameBytes) {
								fixed (byte* pathBytesPtr = pathBytes) {
									code = SecKeychainFindInternetPassword (
										IntPtr.Zero,
										serverBytes?.Length ?? 0,
										serverBytesPtr,
										securityDomainBytes?.Length ?? 0,
										securityDomainBytesPtr,
										accountNameBytes?.Length ?? 0,
										accountNameBytesPtr,
										pathBytes?.Length ?? 0,
										pathBytesPtr,
										port,
										SecProtocolKeys.FromSecProtocol (protocolType),
										KeysAuthenticationType.FromSecAuthenticationType (authenticationType),
										&passwordLength,
										&passwordPtr,
										IntPtr.Zero);
								}
							}
						}
					}
				}

				if (code == SecStatusCode.Success && passwordLength > 0) {
					password = new byte [passwordLength];
					Marshal.Copy (passwordPtr, password, 0, passwordLength);
				}

				return code;

			} finally {
				if (passwordPtr != IntPtr.Zero)
					SecKeychainItemFreeContent (IntPtr.Zero, passwordPtr);
			}
		}

		/// <param name="serviceName">To be added.</param>
		///         <param name="accountName">To be added.</param>
		///         <param name="password">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static SecStatusCode AddGenericPassword (string serviceName, string accountName, byte [] password)
		{
			byte []? serviceNameBytes = null;
			byte []? accountNameBytes = null;

			if (!String.IsNullOrEmpty (serviceName))
				serviceNameBytes = System.Text.Encoding.UTF8.GetBytes (serviceName);

			if (!String.IsNullOrEmpty (accountName))
				accountNameBytes = System.Text.Encoding.UTF8.GetBytes (accountName);

			return SecKeychainAddGenericPassword (
				IntPtr.Zero,
				serviceNameBytes?.Length ?? 0,
				serviceNameBytes,
				accountNameBytes?.Length ?? 0,
				accountNameBytes,
				password?.Length ?? 0,
				password!,
				IntPtr.Zero
				);
		}

		/// <param name="serviceName">To be added.</param>
		///         <param name="accountName">To be added.</param>
		///         <param name="password">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public static SecStatusCode FindGenericPassword (string serviceName, string accountName, out byte []? password)
		{
			password = null;

			byte []? serviceNameBytes = null;
			byte []? accountNameBytes = null;

			IntPtr passwordPtr = IntPtr.Zero;

			try {

				if (!String.IsNullOrEmpty (serviceName))
					serviceNameBytes = System.Text.Encoding.UTF8.GetBytes (serviceName);

				if (!String.IsNullOrEmpty (accountName))
					accountNameBytes = System.Text.Encoding.UTF8.GetBytes (accountName);

				int passwordLength = 0;

				SecStatusCode code;
				unsafe {
					fixed (byte* serviceNameBytesPtr = serviceNameBytes) {
						fixed (byte* accountNameBytesPtr = accountNameBytes) {
							code = SecKeychainFindGenericPassword (
								IntPtr.Zero,
								serviceNameBytes?.Length ?? 0,
								serviceNameBytesPtr,
								accountNameBytes?.Length ?? 0,
								accountNameBytesPtr,
								&passwordLength,
								&passwordPtr,
								IntPtr.Zero
								);
						}
					}
				}

				if (code == SecStatusCode.Success && passwordLength > 0) {
					password = new byte [passwordLength];
					Marshal.Copy (passwordPtr, password, 0, passwordLength);
				}

				return code;

			} finally {
				if (passwordPtr != IntPtr.Zero)
					SecKeychainItemFreeContent (IntPtr.Zero, passwordPtr);
			}
		}
#else
		/// <include file="../../docs/api/Security/SecKeyChain.xml" path="/Documentation/Docs[@DocId='M:Security.SecKeyChain.QueryAsConcreteType(Security.SecRecord,Security.SecStatusCode@)']/*" />
		public static object? QueryAsConcreteType (SecRecord query, out SecStatusCode result)
		{
			if (query is null) {
				result = SecStatusCode.Param;
				return null;
			}

			using (var copy = NSMutableDictionary.FromDictionary (query.queryDict)) {
				copy.LowlevelSetObject (CFBoolean.TrueHandle, SecItem.ReturnRef);
				SetLimit (copy, 1);

				result = SecItem.SecItemCopyMatching (copy.Handle, out var ptr);
				if ((result == SecStatusCode.Success) && (ptr != IntPtr.Zero)) {
					nint cfType = CFType.GetTypeID (ptr);

					if (cfType == SecCertificate.GetTypeID ())
						return new SecCertificate (ptr, true);
					else if (cfType == SecKey.GetTypeID ())
						return new SecKey (ptr, true);
					else if (cfType == SecIdentity.GetTypeID ())
						return new SecIdentity (ptr, true);
					else
						throw new Exception (String.Format ("Unexpected type: 0x{0:x}", cfType));
				}
				return null;
			}
		}
#endif

		/// <param name="identity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void AddIdentity (SecIdentity identity)
		{
			if (identity is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (identity));
			using (var record = new SecRecord ()) {
				record.SetValueRef (identity);

				SecStatusCode result = SecKeyChain.Add (record);

				if (result != SecStatusCode.DuplicateItem && result != SecStatusCode.Success)
					throw new InvalidOperationException (result.ToString ());
			}
		}

		/// <param name="identity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public static void RemoveIdentity (SecIdentity identity)
		{
			if (identity is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (identity));
			using (var record = new SecRecord ()) {
				record.SetValueRef (identity);

				SecStatusCode result = SecKeyChain.Remove (record);

				if (result != SecStatusCode.ItemNotFound && result != SecStatusCode.Success)
					throw new InvalidOperationException (result.ToString ());
			}
		}

		/// <param name="certificate">To be added.</param>
		///         <param name="throwOnError">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static SecIdentity? FindIdentity (SecCertificate certificate, bool throwOnError = false)
		{
			if (certificate is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (certificate));
			var identity = FindIdentity (cert => SecCertificate.Equals (certificate, cert));
			if (!throwOnError || identity is not null)
				return identity;

			throw new InvalidOperationException (string.Format ("Could not find SecIdentity for certificate '{0}' in keychain.", certificate.SubjectSummary));
		}

		static SecIdentity? FindIdentity (Predicate<SecCertificate> filter)
		{
			/*
			 * Unfortunately, SecItemCopyMatching() does not allow any search
			 * filters when looking up an identity.
			 * 
			 * The following lookup will return all identities from the keychain -
			 * we then need need to find the right one.
			 */
			using (var record = new SecRecord (SecKind.Identity)) {
				SecStatusCode status;
				var result = SecKeyChain.QueryAsReference (record, -1, out status);
				if (status != SecStatusCode.Success || result is null)
					return null;

				for (int i = 0; i < result.Length; i++) {
					var identity = (SecIdentity?) result [i];
					if (filter (identity?.Certificate!))
						return identity;
				}
			}

			return null;
		}
	}

	/// <summary>Tracks a set of properties from the keychain.</summary>
	///     <remarks>
	///       <para>
	/// 	This represents a set of properties on a keychain record.   It
	/// 	can be used to query the keychain by filling out a few of the
	/// 	properties and calling one of the Query methods on the <see cref="T:Security.SecKeyChain" /> class and it is
	/// 	also used as a result from some of the same Query methods.
	///       </para>
	///       <para>
	/// 	You would typically use it like this:
	///       </para>
	///       <example>
	///         <code lang="csharp lang-csharp"><![CDATA[
	/// var query = new SecRecord (SecKind.InternetPassword) {
	///    Server = "bugzilla.novell.com",
	///    Account = "miguel"
	/// };
	/// var password = SecKeyChain.QueryAsData (query);
	/// Console.WriteLine ("The password for the account is: {0}", password);
	/// ]]></code>
	///       </example>
	///     </remarks>
	///     <related type="sample" href="https://github.com/xamarin/ios-samples/tree/master/LineLayout/">Keychain</related>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class SecRecord : IDisposable {
		// Fix <= iOS 6 Behaviour - Desk #83099
		// NSCFDictionary: mutating method sent to immutable object
		// iOS 6 returns an inmutable NSDictionary handle and when we try to set its values it goes kaboom
		// By explicitly calling `MutableCopy` we ensure we always have a mutable reference we expect that.
		NSMutableDictionary? _queryDict;
		internal NSMutableDictionary queryDict {
			get {
				return _queryDict!;
			}
			set {
				_queryDict = (NSMutableDictionary) value.MutableCopy ();
			}
		}

		internal SecRecord (NSMutableDictionary dict)
		{
			queryDict = dict;
		}

		// it's possible to query something without a class
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecRecord ()
		{
			queryDict = new NSMutableDictionary ();
		}

		/// <include file="../../docs/api/Security/SecRecord.xml" path="/Documentation/Docs[@DocId='M:Security.SecRecord.#ctor(Security.SecKind)']/*" />
		public SecRecord (SecKind secKind)
		{
			var kind = SecClass.FromSecKind (secKind);
#if MONOMAC
			queryDict = NSMutableDictionary.LowlevelFromObjectAndKey (kind, SecClass.SecClassKey);
#else
			// Apple changed/fixed this in iOS7 (not the only change, see comments above)
			// test suite has a test case that needs to work on both pre-7.0 and post-7.0
			if ((kind == SecClass.Identity) && !SystemVersion.CheckiOS (7, 0))
				queryDict = new NSMutableDictionary ();
			else
				queryDict = NSMutableDictionary.LowlevelFromObjectAndKey (kind, SecClass.SecClassKey);
#endif
		}

		/// <param name="certificate">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecRecord (SecCertificate certificate) : this (SecKind.Certificate)
		{
			SetCertificate (certificate);
		}

		/// <param name="identity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecRecord (SecIdentity identity) : this (SecKind.Identity)
		{
			SetIdentity (identity);
		}

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecRecord (SecKey key) : this (SecKind.Key)
		{
			SetKey (key);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SecCertificate? GetCertificate ()
		{
			CheckClass (SecClass.Certificate);
			return GetValueRef<SecCertificate> ();
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SecIdentity? GetIdentity ()
		{
			CheckClass (SecClass.Identity);
			return GetValueRef<SecIdentity> ();
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SecKey? GetKey ()
		{
			CheckClass (SecClass.Key);
			return GetValueRef<SecKey> ();
		}

		void CheckClass (IntPtr secClass)
		{
			var kind = queryDict.LowlevelObjectForKey (SecClass.SecClassKey);
			if (kind != secClass)
				throw new InvalidOperationException ("SecRecord of incompatible SecClass");
		}

		/// <summary>Makes a copy of this SecRecord.</summary>
		///         <returns />
		///         <remarks>To be added.</remarks>
		public SecRecord Clone ()
		{
			return new SecRecord (NSMutableDictionary.FromDictionary (queryDict));
		}

		// some API are unusable without this (e.g. SecKey.GenerateKeyPair) without duplicating much of SecRecord logic
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public NSDictionary ToDictionary ()
		{
			return queryDict;
		}

		/// <summary>Releases the resources used by the SecRecord object.</summary>
		///         <remarks>
		///           <para>The Dispose method releases the resources used by the SecRecord class.</para>
		///           <para>Calling the Dispose method when the application is finished using the SecRecord ensures that all external resources used by this managed object are released as soon as possible.  Once developers have invoked the Dispose method, the object is no longer useful and developers should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at https://msdn.microsoft.com/en-us/library/498928w2.aspx</para>
		///         </remarks>
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		/// <include file="../../docs/api/Security/SecRecord.xml" path="/Documentation/Docs[@DocId='M:Security.SecRecord.Dispose(System.Boolean)']/*" />
		protected virtual void Dispose (bool disposing)
		{
			if (disposing)
				queryDict?.Dispose ();
		}

		~SecRecord ()
		{
			Dispose (false);
		}

		IntPtr Fetch (IntPtr key)
		{
			return queryDict.LowlevelObjectForKey (key);
		}

		NSObject? FetchObject (IntPtr key)
		{
			return Runtime.GetNSObject (Fetch (key));
		}

		string? FetchString (IntPtr key)
		{
			return (NSString?) FetchObject (key);
		}

		int FetchInt (IntPtr key)
		{
			var obj = (NSNumber?) FetchObject (key);
			return obj is null ? -1 : obj.Int32Value;
		}

		bool FetchBool (IntPtr key, bool defaultValue)
		{
			var obj = (NSNumber?) FetchObject (key);
			return obj is null ? defaultValue : obj.Int32Value != 0;
		}

		T? Fetch<T> (IntPtr key) where T : NSObject
		{
			return (T?) FetchObject (key);
		}


		void SetValue (NSObject val, IntPtr key)
		{
			queryDict.LowlevelSetObject (val, key);
		}

		void SetValue (IntPtr val, IntPtr key)
		{
			queryDict.LowlevelSetObject (val, key);
		}

		void SetValue (string value, IntPtr key)
		{
			// FIXME: it's not clear that we should not allow null (i.e. that null should remove entries)
			// but this is compatible with the exiting behaviour of older XI/XM
			if (value is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
			var ptr = CFString.CreateNative (value);
			try {
				queryDict.LowlevelSetObject (ptr, key);
			} finally {
				CFString.ReleaseNative (ptr);
			}
		}

		//
		// Attributes
		//
		/// <summary>When should the keychain information be accessed.</summary>
		///         <value />
		///         <remarks>Applications should use the most restrictive possible value for this property.</remarks>
		public SecAccessible Accessible {
			get {
				return KeysAccessible.ToSecAccessible (Fetch (SecAttributeKey.Accessible));
			}

			set {
				SetValue (KeysAccessible.FromSecAccessible (value), SecAttributeKey.Accessible);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool Synchronizable {
			get {
				return FetchBool (SecAttributeKey.Synchronizable, false);
			}
			set {
				SetValue (new NSNumber (value ? 1 : 0), SecAttributeKey.Synchronizable);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool SynchronizableAny {
			get {
				return FetchBool (SecAttributeKey.SynchronizableAny, false);
			}
			set {
				SetValue (new NSNumber (value ? 1 : 0), SecAttributeKey.SynchronizableAny);
			}
		}

#if !MONOMAC
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("macos")]
		public string? SyncViewHint {
			get {
				return FetchString (SecAttributeKey.SyncViewHint);
			}
			set {
				SetValue (value!, SecAttributeKey.SyncViewHint);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		public SecTokenID TokenID {
			get {
				return SecTokenIDExtensions.GetValue (Fetch<NSString> (SecKeyGenerationAttributeKeys.TokenIDKey.GetHandle ())!);
			}
			set {
				// choose wisely to avoid NSString -> string -> NSString conversion
				SetValue ((NSObject) value.GetConstant ()!, SecKeyGenerationAttributeKeys.TokenIDKey.GetHandle ());
			}
		}
#endif

		/// <summary>Creation date for this item.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public NSDate? CreationDate {
			get {
				return (NSDate?) FetchObject (SecAttributeKey.CreationDate);
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value, SecAttributeKey.CreationDate);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDate? ModificationDate {
			get {
				return (NSDate?) FetchObject (SecAttributeKey.ModificationDate);
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value, SecAttributeKey.ModificationDate);
			}
		}

		/// <summary>User visible description of this item.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Description {
			get {
				return FetchString (SecAttributeKey.Description);
			}

			set {
				SetValue (value!, SecAttributeKey.Description);
			}
		}

		/// <summary>Used editable comment for this record.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Comment {
			get {
				return FetchString (SecAttributeKey.Comment);
			}

			set {
				SetValue (value!, SecAttributeKey.Comment);
			}
		}

		/// <summary>Creator key, a 32-bit value</summary>
		///         <value />
		///         <remarks>A 32 bit value used to flag the entry with the creator key.</remarks>
		public int Creator {
			get {
				return FetchInt (SecAttributeKey.Creator);
			}

			set {
				SetValue (new NSNumber (value), SecAttributeKey.Creator);
			}
		}

		/// <summary>Item's type.   32-bit value.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public int CreatorType {
			get {
				return FetchInt (SecAttributeKey.Type);
			}

			set {
				SetValue (new NSNumber (value), SecAttributeKey.Type);
			}
		}

		/// <summary>User visible label for this item.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Label {
			get {
				return FetchString (SecAttributeKeys.LabelKey.Handle);
			}

			set {
				SetValue (value!, SecAttributeKeys.LabelKey.Handle);
			}
		}

		/// <summary>If set, the item is not displayed to the user.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public bool Invisible {
			get {
				return Fetch (SecAttributeKey.IsInvisible) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKey.IsInvisible);
			}
		}

		/// <summary>Whether there is a valid password associated.</summary>
		///         <value />
		///         <remarks>You can set this flag if you want to force the user to enter the password every time he needs to use the item.</remarks>
		public bool IsNegative {
			get {
				return Fetch (SecAttributeKey.IsNegative) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKey.IsNegative);
			}
		}

		/// <summary>Account name.</summary>
		///         <value />
		///         <remarks>Used by GenericPassword and InternetPassword kinds.</remarks>
		public string? Account {
			get {
				return FetchString (SecAttributeKey.Account);
			}

			set {
				SetValue (value!, SecAttributeKey.Account);
			}
		}

		/// <summary>Service associated with an InternetPassword.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Service {
			get {
				return FetchString (SecAttributeKey.Service);
			}

			set {
				SetValue (value!, SecAttributeKey.Service);
			}
		}

#if !MONOMAC
		/// <summary>User facing description of the kind of authentication that the application is trying to perform</summary>
		///         <value>
		///           <para />
		///         </value>
		///         <remarks>Set this value to a string that will be displayed to the user when the authentication takes place for the item to give the user some context for the request.</remarks>
		[SupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("ios14.0", "Use 'LAContext.InteractionNotAllowed' instead.")]
		[ObsoletedOSPlatform ("tvos14.0", "Use 'LAContext.InteractionNotAllowed' instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'LAContext.InteractionNotAllowed' instead.")]
		public string? UseOperationPrompt {
			get {
				return FetchString (SecItem.UseOperationPrompt);
			}
			set {
				SetValue (value!, SecItem.UseOperationPrompt);
			}
		}

		/// <summary>Developers should not use this deprecated property. Developers should use AuthenticationUI property</summary>
		///         <value>
		///           <para />
		///         </value>
		///         <remarks>Setting this value will return an error condition if the item requires a user interface to authenticate.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos", "Use 'AuthenticationUI' property instead.")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'AuthenticationUI' property instead.")]
		[ObsoletedOSPlatform ("ios", "Use 'AuthenticationUI' property instead.")]
		public bool UseNoAuthenticationUI {
			get {
				return Fetch (SecItem.UseNoAuthenticationUI) == CFBoolean.TrueHandle;
			}
			set {
				SetValue (CFBoolean.ToHandle (value), SecItem.UseNoAuthenticationUI);
			}
		}
#endif
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public SecAuthenticationUI AuthenticationUI {
			get {
				var s = Fetch<NSString> (SecItem.UseAuthenticationUI);
				return s is null ? SecAuthenticationUI.NotSet : SecAuthenticationUIExtensions.GetValue (s);
			}
			set {
				SetValue ((NSObject) value.GetConstant ()!, SecItem.UseAuthenticationUI);
			}
		}

#if !TVOS
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		public LocalAuthentication.LAContext? AuthenticationContext {
			get {
				return Fetch<LocalAuthentication.LAContext> (SecItem.UseAuthenticationContext);
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value.Handle, SecItem.UseAuthenticationContext);
				GC.KeepAlive (value);
			}
		}
#endif

		// Must store the _secAccessControl here, since we have no way of inspecting its values if
		// it is ever returned from a dictionary, so return what we cached.
		SecAccessControl? _secAccessControl;
		/// <summary>Access control for the item.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecAccessControl? AccessControl {
			get {
				return _secAccessControl;
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				_secAccessControl = value;
				SetValue (value.Handle, SecAttributeKeys.AccessControlKey.Handle);
				GC.KeepAlive (value);
			}
		}

		/// <summary>Generic password's NSData storage.</summary>
		///         <value>Items of kind GenericPassword use this field to store item-specific data.</value>
		///         <remarks>To be added.</remarks>
		public NSData? Generic {
			get {
				return Fetch<NSData> (SecAttributeKey.Generic);
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value, SecAttributeKey.Generic);
			}
		}

		/// <summary>Security domain for InternetPassword items.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? SecurityDomain {
			get {
				return FetchString (SecAttributeKey.SecurityDomain);
			}

			set {
				SetValue (value!, SecAttributeKey.SecurityDomain);
			}
		}

		/// <summary>Server component for an InternetPassword</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? Server {
			get {
				return FetchString (SecAttributeKey.Server);
			}

			set {
				SetValue (value!, SecAttributeKey.Server);
			}
		}

		/// <summary>Protocol component of an InternetPassword.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public SecProtocol Protocol {
			get {
				return SecProtocolKeys.ToSecProtocol (Fetch (SecAttributeKey.Protocol));
			}

			set {
				SetValue (SecProtocolKeys.FromSecProtocol (value), SecAttributeKey.Protocol);
			}
		}

		/// <summary>The authentication type.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public SecAuthenticationType AuthenticationType {
			get {
				var at = Fetch (SecAttributeKey.AuthenticationType);
				if (at == IntPtr.Zero)
					return SecAuthenticationType.Default;
				return KeysAuthenticationType.ToSecAuthenticationType (at);
			}

			set {
				SetValue (KeysAuthenticationType.FromSecAuthenticationType (value),
								 SecAttributeKey.AuthenticationType);
			}
		}

		/// <summary>Port component of an InternetPassword</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public int Port {
			get {
				return FetchInt (SecAttributeKey.Port);
			}

			set {
				SetValue (new NSNumber (value), SecAttributeKey.Port);
			}
		}

		/// <summary>Path component of an InternetPassword.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Path {
			get {
				return FetchString (SecAttributeKey.Path);
			}

			set {
				SetValue (value!, SecAttributeKey.Path);
			}
		}

		// read only
		/// <summary>X.500 Subject name stored as an NSData.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public string? Subject {
			get {
				return FetchString (SecAttributeKey.Subject);
			}
		}

		// read only
		/// <summary>X.500 Issuer certificate name as an NSData block.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public NSData? Issuer {
			get {
				return Fetch<NSData> (SecAttributeKey.Issuer);
			}
		}

		// read only
		/// <summary>Serial number for the certificate.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public NSData? SerialNumber {
			get {
				return Fetch<NSData> (SecAttributeKey.SerialNumber);
			}
		}

		// read only
		/// <summary>SubjectKeyID of the certificate.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public NSData? SubjectKeyID {
			get {
				return Fetch<NSData> (SecAttributeKey.SubjectKeyID);
			}
		}

		// read only
		/// <summary>Public key hash</summary>
		///         <value />
		///         <remarks>The public key hash, used by certificates.</remarks>
		public NSData? PublicKeyHash {
			get {
				return Fetch<NSData> (SecAttributeKey.PublicKeyHash);
			}
		}

		// read only
		/// <summary>A certificate type.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public NSNumber? CertificateType {
			get {
				return Fetch<NSNumber> (SecAttributeKey.CertificateType);
			}
		}

		// read only
		/// <summary>The encoding used for the certificate.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public NSNumber? CertificateEncoding {
			get {
				return Fetch<NSNumber> (SecAttributeKey.CertificateEncoding);
			}
		}

		/// <summary>The key class.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public SecKeyClass KeyClass {
			get {
				var k = Fetch (SecAttributeKey.KeyClass);
				if (k == IntPtr.Zero)
					return SecKeyClass.Invalid;
				using (var s = new NSString (k))
					return SecKeyClassExtensions.GetValue (s);
			}
			set {
				var k = value.GetConstant ();
				if (k is null)
					throw new ArgumentException (nameof (value));
				SetValue ((NSObject) k, SecAttributeKey.KeyClass);
			}
		}

		/// <summary>An application-level tag, used to identify this key.</summary>
		///         <value />
		///         <remarks>Intended for your program to use as an identifier that you can lookup.</remarks>
		public string? ApplicationLabel {
			get {
				return FetchString (SecAttributeKey.ApplicationLabel);
			}

			set {
				SetValue (value!, SecAttributeKey.ApplicationLabel);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsPermanent {
			get {
				return Fetch (SecAttributeKeys.IsPermanentKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKeys.IsPermanentKey.Handle);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsSensitive {
			get {
				return Fetch (SecAttributeKey.IsSensitive) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKey.IsSensitive);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool IsExtractable {
			get {
				return Fetch (SecAttributeKey.IsExtractable) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKey.IsExtractable);
			}
		}

		/// <summary>To store your application data.</summary>
		///         <value />
		///         <remarks>You can use this to store application-level binary data in the form of an NSData source.</remarks>
		public NSData? ApplicationTag {
			get {
				return Fetch<NSData> (SecAttributeKeys.ApplicationTagKey.Handle);
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value, SecAttributeKeys.ApplicationTagKey.Handle);
			}
		}

		/// <summary>The key type.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public SecKeyType KeyType {
			get {
				var k = Fetch (SecKeyGenerationAttributeKeys.KeyTypeKey.Handle);
				if (k == IntPtr.Zero)
					return SecKeyType.Invalid;
				using (var s = new NSString (k))
					return SecKeyTypeExtensions.GetValue (s);
			}

			set {
				var k = value.GetConstant ();
				if (k is null)
					throw new ArgumentException (nameof (value));
				SetValue ((NSObject) k, SecKeyGenerationAttributeKeys.KeyTypeKey.Handle);
			}
		}

		/// <summary>Bitsize for the key, contrast this with EffectiveKeySize.</summary>
		///         <value />
		///         <remarks>This determines the number of bits in the key.   This can contain padding, contrast this with EffectiveKeySize.</remarks>
		public int KeySizeInBits {
			get {
				return FetchInt (SecKeyGenerationAttributeKeys.KeySizeInBitsKey.Handle);
			}

			set {
				SetValue (new NSNumber (value), SecKeyGenerationAttributeKeys.KeySizeInBitsKey.Handle);
			}
		}

		/// <summary>Number of effective bits on the key.</summary>
		///         <value />
		///         <remarks>The number of effective bits on the key.   Contrast this with the KeySize that might be larger, but contains padding.</remarks>
		public int EffectiveKeySize {
			get {
				return FetchInt (SecAttributeKeys.EffectiveKeySizeKey.Handle);
			}

			set {
				SetValue (new NSNumber (value), SecAttributeKeys.EffectiveKeySizeKey.Handle);
			}
		}

		/// <summary>Whether this cryptographic key can be used to encrypt data.</summary>
		///         <value />
		///         <remarks>For keys, this determines whether the key can be used to encrypt data.</remarks>
		public bool CanEncrypt {
			get {
				return Fetch (SecAttributeKeys.CanEncryptKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKeys.CanEncryptKey.Handle);
			}
		}

		/// <summary>Whether this cryptographic key can be used to decrypt data.</summary>
		///         <value />
		///         <remarks>For keys, whether this can be used to decrypt data.</remarks>
		public bool CanDecrypt {
			get {
				return Fetch (SecAttributeKeys.CanDecryptKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKeys.CanDecryptKey.Handle);
			}
		}

		/// <summary>Whether this key can be used to derive another key.</summary>
		///         <value />
		///         <remarks>For keys, whether this can be used to derive another key.</remarks>
		public bool CanDerive {
			get {
				return Fetch (SecAttributeKeys.CanDeriveKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKeys.CanDeriveKey.Handle);
			}
		}

		/// <summary>Whether this key can be used to sign data.</summary>
		///         <value />
		///         <remarks>For keys, whether this can be used to sign.</remarks>
		public bool CanSign {
			get {
				return Fetch (SecAttributeKeys.CanSignKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKeys.CanSignKey.Handle);
			}
		}

		/// <summary>Whether this key can be used to verify a digital signature.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public bool CanVerify {
			get {
				return Fetch (SecAttributeKeys.CanVerifyKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKeys.CanVerifyKey.Handle);
			}
		}

		/// <summary>Whether this key can be used to wrap another key.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public bool CanWrap {
			get {
				return Fetch (SecKeyGenerationAttributeKeys.CanWrapKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecKeyGenerationAttributeKeys.CanWrapKey.Handle);
			}
		}

		/// <summary>Whether this key can be used to unwrap another key.</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public bool CanUnwrap {
			get {
				return Fetch (SecAttributeKeys.CanUnwrapKey.Handle) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKeys.CanUnwrapKey.Handle);
			}
		}

		/// <summary>Access group name. </summary>
		///         <value />
		///         <remarks>Access groups are used to share information between applications that share the same access group.    Applications that wish to do this, need to register the access group on their keychain-access-group entitlement.   This value must be set when the item is added to the keychain for the second application to be able to look it up.</remarks>
		public string? AccessGroup {
			get {
				return FetchString (SecAttributeKey.AccessGroup);
			}

			set {
				SetValue (value!, SecAttributeKey.AccessGroup);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool PersistentReference {
			get {
				return Fetch (SecAttributeKey.PersistentReference) == CFBoolean.TrueHandle;
			}
			set {
				SetValue (CFBoolean.ToHandle (value), SecAttributeKey.PersistentReference);
			}
		}

		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		public bool UseDataProtectionKeychain {
			get {
				return Fetch (SecItem.UseDataProtectionKeychain) == CFBoolean.TrueHandle;
			}
			set {
				SetValue (CFBoolean.ToHandle (value), SecItem.UseDataProtectionKeychain);
			}
		}

		//
		// Matches
		//

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecPolicy? MatchPolicy {
			get {
				var pol = Fetch (SecItem.MatchPolicy);
				return (pol == IntPtr.Zero) ? null : new SecPolicy (pol, false);
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value.Handle, SecItem.MatchPolicy);
				GC.KeepAlive (value);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecKeyChain? []? MatchItemList {
			get {
				return NSArray.ArrayFromHandle<SecKeyChain> (Fetch (SecItem.MatchItemList));
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				using (var array = NSArray.FromNativeObjects (value))
					SetValue (array, SecItem.MatchItemList);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSData? []? MatchIssuers {
			get {
				return NSArray.ArrayFromHandle<NSData> (Fetch (SecItem.MatchIssuers));
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));

				SetValue (NSArray.FromNSObjects (value), SecItem.MatchIssuers);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? MatchEmailAddressIfPresent {
			get {
				return FetchString (SecItem.MatchEmailAddressIfPresent);
			}

			set {
				SetValue (value!, SecItem.MatchEmailAddressIfPresent);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string? MatchSubjectContains {
			get {
				return FetchString (SecItem.MatchSubjectContains);
			}

			set {
				SetValue (value!, SecItem.MatchSubjectContains);
			}
		}

		/// <summary>Whether matches should be case insensitive</summary>
		///         <value />
		///         <remarks>To be added.</remarks>
		public bool MatchCaseInsensitive {
			get {
				return Fetch (SecItem.MatchCaseInsensitive) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecItem.MatchCaseInsensitive);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public bool MatchTrustedOnly {
			get {
				return Fetch (SecItem.MatchTrustedOnly) == CFBoolean.TrueHandle;
			}

			set {
				SetValue (CFBoolean.ToHandle (value), SecItem.MatchTrustedOnly);
			}
		}

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSDate? MatchValidOnDate {
			get {
				return Runtime.GetNSObject<NSDate> (Fetch (SecItem.MatchValidOnDate));
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value, SecItem.MatchValidOnDate);
			}
		}

		/// <summary>The value data to store.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public NSData? ValueData {
			get {
				return Fetch<NSData> (SecItem.ValueData);
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				SetValue (value, SecItem.ValueData);
			}
		}

		/// <typeparam name="T">The desired strong type of the value to
		/// 	get, one of <see cref="T:Security.SecCertificate" /><see cref="T:Security.SecIdentity" /> or <see cref="T:Security.SecKey" />.</typeparam>
		///         <summary>Returns the associated Certificate, Identity, or Key stored in this record.</summary>
		///         <returns>The return value, if present shoudl be one of the
		/// 	allowed types <see cref="T:Security.SecCertificate" /><see cref="T:Security.SecIdentity" /> or <see cref="T:Security.SecKey" />.</returns>
		///         <remarks>
		///         </remarks>
		public T? GetValueRef<T> () where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (queryDict.LowlevelObjectForKey (SecItem.ValueRef), false);
		}

		// This can be used to store SecKey, SecCertificate, SecIdentity and SecKeyChainItem (not bound yet, and not availble on iOS)
		/// <param name="value">An object of type <see cref="T:Security.SecCertificate" /><see cref="T:Security.SecIdentity" /> or <see cref="T:Security.SecKey" />.</param>
		///         <summary>Use this to add a certificate, identity or key to the record.</summary>
		///         <remarks>
		///         </remarks>
		public void SetValueRef (INativeObject value)
		{
			SetValue (value.GetHandle (), SecItem.ValueRef);
			GC.KeepAlive (value);
		}

		/// <param name="cert">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetCertificate (SecCertificate cert) => SetValueRef (cert);
		/// <param name="identity">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetIdentity (SecIdentity identity) => SetValueRef (identity);
		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetKey (SecKey key) => SetValueRef (key);

	}

	internal partial class SecItem {

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode SecItemCopyMatching (/* CFDictionaryRef */ IntPtr query, /* CFTypeRef* */ IntPtr* result);

		internal static SecStatusCode SecItemCopyMatching (/* CFDictionaryRef */ IntPtr query, /* CFTypeRef* */ out IntPtr result)
		{
			result = default;
			unsafe {
				return SecItemCopyMatching (query, (IntPtr*) Unsafe.AsPointer<IntPtr> (ref result));
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		internal extern static SecStatusCode SecItemAdd (/* CFDictionaryRef */ IntPtr attributes, /* CFTypeRef* */ IntPtr result);

		[DllImport (Constants.SecurityLibrary)]
		internal extern static SecStatusCode SecItemDelete (/* CFDictionaryRef */ IntPtr query);

		[DllImport (Constants.SecurityLibrary)]
		internal extern static SecStatusCode SecItemUpdate (/* CFDictionaryRef */ IntPtr query, /* CFDictionaryRef */ IntPtr attributesToUpdate);
	}

	internal static partial class SecClass {

		public static IntPtr FromSecKind (SecKind secKind)
		{
			switch (secKind) {
			case SecKind.InternetPassword:
				return InternetPassword;
			case SecKind.GenericPassword:
				return GenericPassword;
			case SecKind.Certificate:
				return Certificate;
			case SecKind.Key:
				return Key;
			case SecKind.Identity:
				return Identity;
			default:
				throw new ArgumentException (nameof (secKind));
			}
		}
	}

	internal static partial class KeysAccessible {
		public static IntPtr FromSecAccessible (SecAccessible accessible)
		{
			switch (accessible) {
			case SecAccessible.WhenUnlocked:
				return WhenUnlocked;
			case SecAccessible.AfterFirstUnlock:
				return AfterFirstUnlock;
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'SecAccessible.Always' is obsoleted on: 'ios' 12.0 and later (Use 'AfterFirstUnlock' or a better suited option instead.), 'maccatalyst' 12.0 and later (Use 'AfterFirstUnlock' or a better suited option instead.), 'macOS/OSX' 10.14 and later (Use 'AfterFirstUnlock' or a better suited option instead.).
			case SecAccessible.Always:
				return Always;
#pragma warning restore CA1422
			case SecAccessible.WhenUnlockedThisDeviceOnly:
				return WhenUnlockedThisDeviceOnly;
			case SecAccessible.AfterFirstUnlockThisDeviceOnly:
				return AfterFirstUnlockThisDeviceOnly;
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'SecAccessible.AlwaysThisDeviceOnly' is obsoleted on: 'ios' 12.0 and later (Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.), 'maccatalyst' 12.0 and later (Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.), 'macOS/OSX' 10.14 and later (Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.).
			case SecAccessible.AlwaysThisDeviceOnly:
				return AlwaysThisDeviceOnly;
#pragma warning restore CA1422
			case SecAccessible.WhenPasscodeSetThisDeviceOnly:
				return WhenPasscodeSetThisDeviceOnly;
			default:
				throw new ArgumentException (nameof (accessible));
			}
		}

		// note: we're comparing pointers - but it's an (even if opaque) CFType
		// and it turns out to be using CFString - so we need to use CFTypeEqual
		public static SecAccessible ToSecAccessible (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return SecAccessible.Invalid;
			if (CFType.Equal (handle, WhenUnlocked))
				return SecAccessible.WhenUnlocked;
			if (CFType.Equal (handle, AfterFirstUnlock))
				return SecAccessible.AfterFirstUnlock;
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'SecAccessible.Always' is obsoleted on: 'ios' 12.0 and later (Use 'AfterFirstUnlock' or a better suited option instead.), 'maccatalyst' 12.0 and later (Use 'AfterFirstUnlock' or a better suited option instead.), 'macOS/OSX' 10.14 and later (Use 'AfterFirstUnlock' or a better suited option instead.).
			if (CFType.Equal (handle, Always))
				return SecAccessible.Always;
#pragma warning restore CA1422
			if (CFType.Equal (handle, WhenUnlockedThisDeviceOnly))
				return SecAccessible.WhenUnlockedThisDeviceOnly;
			if (CFType.Equal (handle, AfterFirstUnlockThisDeviceOnly))
				return SecAccessible.AfterFirstUnlockThisDeviceOnly;
#pragma warning disable CA1422 // This call site is reachable on: 'ios' 12.2 and later, 'maccatalyst' 12.2 and later, 'macOS/OSX' 12.0 and later, 'tvos' 12.2 and later. 'SecAccessible.AlwaysThisDeviceOnly' is obsoleted on: 'ios' 12.0 and later (Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.), 'maccatalyst' 12.0 and later (Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.), 'macOS/OSX' 10.14 and later (Use 'AfterFirstUnlockThisDeviceOnly' or a better suited option instead.).
			if (CFType.Equal (handle, AlwaysThisDeviceOnly))
				return SecAccessible.AlwaysThisDeviceOnly;
#pragma warning restore CA1422
			if (CFType.Equal (handle, WhenUnlockedThisDeviceOnly))
				return SecAccessible.WhenUnlockedThisDeviceOnly;
			return SecAccessible.Invalid;
		}
	}

	internal static partial class SecProtocolKeys {
		public static IntPtr FromSecProtocol (SecProtocol protocol)
		{
			switch (protocol) {
			case SecProtocol.Ftp: return FTP;
			case SecProtocol.FtpAccount: return FTPAccount;
			case SecProtocol.Http: return HTTP;
			case SecProtocol.Irc: return IRC;
			case SecProtocol.Nntp: return NNTP;
			case SecProtocol.Pop3: return POP3;
			case SecProtocol.Smtp: return SMTP;
			case SecProtocol.Socks: return SOCKS;
			case SecProtocol.Imap: return IMAP;
			case SecProtocol.Ldap: return LDAP;
			case SecProtocol.AppleTalk: return AppleTalk;
			case SecProtocol.Afp: return AFP;
			case SecProtocol.Telnet: return Telnet;
			case SecProtocol.Ssh: return SSH;
			case SecProtocol.Ftps: return FTPS;
			case SecProtocol.Https: return HTTPS;
			case SecProtocol.HttpProxy: return HTTPProxy;
			case SecProtocol.HttpsProxy: return HTTPSProxy;
			case SecProtocol.FtpProxy: return FTPProxy;
			case SecProtocol.Smb: return SMB;
			case SecProtocol.Rtsp: return RTSP;
			case SecProtocol.RtspProxy: return RTSPProxy;
			case SecProtocol.Daap: return DAAP;
			case SecProtocol.Eppc: return EPPC;
			case SecProtocol.Ipp: return IPP;
			case SecProtocol.Nntps: return NNTPS;
			case SecProtocol.Ldaps: return LDAPS;
			case SecProtocol.Telnets: return TelnetS;
			case SecProtocol.Imaps: return IMAPS;
			case SecProtocol.Ircs: return IRCS;
			case SecProtocol.Pop3s: return POP3S;
			}
			throw new ArgumentException (nameof (protocol));
		}

		public static SecProtocol ToSecProtocol (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return SecProtocol.Invalid;
			if (CFType.Equal (handle, FTP))
				return SecProtocol.Ftp;
			if (CFType.Equal (handle, FTPAccount))
				return SecProtocol.FtpAccount;
			if (CFType.Equal (handle, HTTP))
				return SecProtocol.Http;
			if (CFType.Equal (handle, IRC))
				return SecProtocol.Irc;
			if (CFType.Equal (handle, NNTP))
				return SecProtocol.Nntp;
			if (CFType.Equal (handle, POP3))
				return SecProtocol.Pop3;
			if (CFType.Equal (handle, SMTP))
				return SecProtocol.Smtp;
			if (CFType.Equal (handle, SOCKS))
				return SecProtocol.Socks;
			if (CFType.Equal (handle, IMAP))
				return SecProtocol.Imap;
			if (CFType.Equal (handle, LDAP))
				return SecProtocol.Ldap;
			if (CFType.Equal (handle, AppleTalk))
				return SecProtocol.AppleTalk;
			if (CFType.Equal (handle, AFP))
				return SecProtocol.Afp;
			if (CFType.Equal (handle, Telnet))
				return SecProtocol.Telnet;
			if (CFType.Equal (handle, SSH))
				return SecProtocol.Ssh;
			if (CFType.Equal (handle, FTPS))
				return SecProtocol.Ftps;
			if (CFType.Equal (handle, HTTPS))
				return SecProtocol.Https;
			if (CFType.Equal (handle, HTTPProxy))
				return SecProtocol.HttpProxy;
			if (CFType.Equal (handle, HTTPSProxy))
				return SecProtocol.HttpsProxy;
			if (CFType.Equal (handle, FTPProxy))
				return SecProtocol.FtpProxy;
			if (CFType.Equal (handle, SMB))
				return SecProtocol.Smb;
			if (CFType.Equal (handle, RTSP))
				return SecProtocol.Rtsp;
			if (CFType.Equal (handle, RTSPProxy))
				return SecProtocol.RtspProxy;
			if (CFType.Equal (handle, DAAP))
				return SecProtocol.Daap;
			if (CFType.Equal (handle, EPPC))
				return SecProtocol.Eppc;
			if (CFType.Equal (handle, IPP))
				return SecProtocol.Ipp;
			if (CFType.Equal (handle, NNTPS))
				return SecProtocol.Nntps;
			if (CFType.Equal (handle, LDAPS))
				return SecProtocol.Ldaps;
			if (CFType.Equal (handle, TelnetS))
				return SecProtocol.Telnets;
			if (CFType.Equal (handle, IMAPS))
				return SecProtocol.Imaps;
			if (CFType.Equal (handle, IRCS))
				return SecProtocol.Ircs;
			if (CFType.Equal (handle, POP3S))
				return SecProtocol.Pop3s;
			return SecProtocol.Invalid;
		}
	}

	internal static partial class KeysAuthenticationType {
		public static SecAuthenticationType ToSecAuthenticationType (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return SecAuthenticationType.Invalid;
			if (CFType.Equal (handle, NTLM))
				return SecAuthenticationType.Ntlm;
			if (CFType.Equal (handle, MSN))
				return SecAuthenticationType.Msn;
			if (CFType.Equal (handle, DPA))
				return SecAuthenticationType.Dpa;
			if (CFType.Equal (handle, RPA))
				return SecAuthenticationType.Rpa;
			if (CFType.Equal (handle, HTTPBasic))
				return SecAuthenticationType.HttpBasic;
			if (CFType.Equal (handle, HTTPDigest))
				return SecAuthenticationType.HttpDigest;
			if (CFType.Equal (handle, HTMLForm))
				return SecAuthenticationType.HtmlForm;
			if (CFType.Equal (handle, Default))
				return SecAuthenticationType.Default;
			return SecAuthenticationType.Invalid;
		}

		public static IntPtr FromSecAuthenticationType (SecAuthenticationType type)
		{
			switch (type) {
			case SecAuthenticationType.Ntlm:
				return NTLM;
			case SecAuthenticationType.Msn:
				return MSN;
			case SecAuthenticationType.Dpa:
				return DPA;
			case SecAuthenticationType.Rpa:
				return RPA;
			case SecAuthenticationType.HttpBasic:
				return HTTPBasic;
			case SecAuthenticationType.HttpDigest:
				return HTTPDigest;
			case SecAuthenticationType.HtmlForm:
				return HTMLForm;
			case SecAuthenticationType.Default:
				return Default;
			default:
				throw new ArgumentException (nameof (type));
			}
		}
	}

	/// <summary>An exception based on a <see cref="T:Security.SecStatusCode" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class SecurityException : Exception {
		static string ToMessage (SecStatusCode code)
		{

			switch (code) {
			case SecStatusCode.Success:
			case SecStatusCode.Unimplemented:
			case SecStatusCode.Param:
			case SecStatusCode.Allocate:
			case SecStatusCode.NotAvailable:
			case SecStatusCode.DuplicateItem:
			case SecStatusCode.ItemNotFound:
			case SecStatusCode.InteractionNotAllowed:
			case SecStatusCode.Decode:
				return code.ToString ();
			}
			return String.Format ("Unknown error: 0x{0:x}", code);
		}

		/// <param name="code" />
		///         <summary>Creates an exception from a status code.</summary>
		///         <remarks>To be added.</remarks>
		public SecurityException (SecStatusCode code) : base (ToMessage (code))
		{
		}
	}

	/// <summary>Contains parameters for use with <see cref="M:Security.SecKey.CreateRandomKey(Security.SecKeyType,System.Int32,Foundation.NSDictionary,Foundation.NSError@)" />.</summary>
	///     <remarks>To be added.</remarks>
	public partial class SecKeyParameters : DictionaryContainer {
		// For caching, as we can't reverse it easily.
		SecAccessControl? _secAccessControl;

		/// <summary>Gets or sets the access control for the new key.</summary>
		///         <value>The access control for the new key.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public SecAccessControl AccessControl {
			get {
				return _secAccessControl!;
			}
			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				_secAccessControl = value;
				SetNativeValue (SecAttributeKeys.AccessControlKey, value);
			}
		}
	}

	/// <summary>Contains parameters for key generation.</summary>
	///     <remarks>To be added.</remarks>
	public partial class SecKeyGenerationParameters : DictionaryContainer {
		/// <summary>Gets or sets the type of key to create.</summary>
		///         <value>The type of key to create.</value>
		///         <remarks>To be added.</remarks>
		public SecKeyType KeyType {
			get {
				var type = GetNSStringValue (SecKeyGenerationAttributeKeys.KeyTypeKey);
				if (type is null)
					return SecKeyType.Invalid;
				return SecKeyTypeExtensions.GetValue (type);
			}

			set {
				var k = value.GetConstant ();
				if (k is null)
					throw new ArgumentException (nameof (value));
				SetStringValue (SecKeyGenerationAttributeKeys.KeyTypeKey, k);
			}
		}

		// For caching, as we can't reverse it easily.
		SecAccessControl? _secAccessControl;

		/// <summary>Gets or sets the access control for the new key.</summary>
		///         <value>The access control for the new key.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public SecAccessControl AccessControl {
			get {
				return _secAccessControl!;
			}

			set {
				if (value is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (value));
				_secAccessControl = value;
				SetNativeValue (SecAttributeKeys.AccessControlKey, value);
			}
		}

		/// <summary>Gets or sets the token ID.</summary>
		///         <value>The token ID.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		public SecTokenID TokenID {
			get {
				return SecTokenIDExtensions.GetValue (GetNSStringValue (SecKeyGenerationAttributeKeys.TokenIDKey)!);
			}

			set {
				SetStringValue (SecKeyGenerationAttributeKeys.TokenIDKey, value.GetConstant ());
			}
		}
	}
}
