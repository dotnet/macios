//
// SslContext
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2014-2016 Xamarin Inc.
//

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace Security {
	/// <summary>Class that encapsulates SSL session state..</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
	[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
	[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
	[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
	public class SslContext : NativeObject {

		SslConnection? connection;
		SslStatus result;

		[DllImport (Constants.SecurityLibrary)]
		extern static /* SSLContextRef */ IntPtr SSLCreateContext (/* CFAllocatorRef */ IntPtr alloc, SslProtocolSide protocolSide, SslConnectionType connectionType);

		/// <param name="protocolSide">To be added.</param>
		///         <param name="connectionType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SslContext (SslProtocolSide protocolSide, SslConnectionType connectionType)
			: base (SSLCreateContext (IntPtr.Zero, protocolSide, connectionType), true)
		{
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static /* OSStatus */ SslStatus SSLClose (/* SSLContextRef */ IntPtr context);

		/// <include file="../../docs/api/Security/SslContext.xml" path="/Documentation/Docs[@DocId='M:Security.SslContext.Dispose(System.Boolean)']/*" />
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero)
				result = SSLClose (Handle);

			// don't remove the read/write delegates before we closed the connection, i.e.
			// the SSLClose will send an Alert for a "close notify"
			if (connection is not null) {
				connection.Dispose ();
				connection = null;
			}

			base.Dispose (disposing);
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SslStatus GetLastStatus ()
		{
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static /* OSStatus */ SslStatus SSLGetProtocolVersionMax (/* SSLContextRef */ IntPtr context, SslProtocol* maxVersion);

		[DllImport (Constants.SecurityLibrary)]
		extern static /* OSStatus */ SslStatus SSLSetProtocolVersionMax (/* SSLContextRef */ IntPtr context, SslProtocol maxVersion);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SslProtocol MaxProtocol {
			get {
				SslProtocol value;
				unsafe {
					result = SSLGetProtocolVersionMax (Handle, &value);
				}
				return value;
			}
			set {
				result = SSLSetProtocolVersionMax (Handle, value);
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static /* OSStatus */ SslStatus SSLGetProtocolVersionMin (/* SSLContextRef */ IntPtr context, SslProtocol* minVersion);

		[DllImport (Constants.SecurityLibrary)]
		extern static /* OSStatus */ SslStatus SSLSetProtocolVersionMin (/* SSLContextRef */ IntPtr context, SslProtocol minVersion);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SslProtocol MinProtocol {
			get {
				SslProtocol value;
				unsafe {
					result = SSLGetProtocolVersionMin (Handle, &value);
				}
				return value;
			}
			set {
				result = SSLSetProtocolVersionMin (Handle, value);
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static /* OSStatus */ SslStatus SSLGetNegotiatedProtocolVersion (/* SSLContextRef */ IntPtr context, SslProtocol* protocol);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SslProtocol NegotiatedProtocol {
			get {
				SslProtocol value;
				unsafe {
					result = SSLGetNegotiatedProtocolVersion (Handle, &value);
				}
				return value;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static /* OSStatus */ SslStatus SSLGetConnection (/* SSLContextRef */ IntPtr context, /* SSLConnectionRef* */ IntPtr* connection);

		[DllImport (Constants.SecurityLibrary)]
		extern static /* OSStatus */ SslStatus SSLSetConnection (/* SSLContextRef */ IntPtr context, /* SSLConnectionRef */ IntPtr connection);

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static /* OSStatus */ SslStatus SSLSetIOFuncs (
			/* SSLContextRef */ IntPtr context,
			/* SSLReadFunc */ delegate* unmanaged<IntPtr, IntPtr, nint*, SslStatus> readFunc,
			/* SSLWriteFunc */ delegate* unmanaged<IntPtr, IntPtr, nint*, SslStatus> writeFunc);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SslConnection? Connection {
			get {
				if (connection is null)
					return null;
				IntPtr value;
				unsafe {
					result = SSLGetConnection (Handle, &value);
				}
				if (value != connection.ConnectionId)
					throw new InvalidOperationException ();
				return connection;
			}
			set {
				// the read/write delegates needs to be set before setting the connection id
				unsafe {
					if (value is null)
						result = SSLSetIOFuncs (Handle, null, null);
					else
						result = SSLSetIOFuncs (Handle, value.ReadFunc, value.WriteFunc);
				}

				if (result == SslStatus.Success) {
					result = SSLSetConnection (Handle, value is null ? IntPtr.Zero : value.ConnectionId);
					connection = value;
				}
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static /* OSStatus */ SslStatus SSLGetSessionOption (/* SSLContextRef */ IntPtr context, SslSessionOption option, byte* value);

		/// <param name="option">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SslStatus GetSessionOption (SslSessionOption option, out bool value)
		{
			byte byteValue;
			unsafe {
				result = SSLGetSessionOption (Handle, option, &byteValue);
			}
			value = byteValue != 0;
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static /* OSStatus */ SslStatus SSLSetSessionOption (/* SSLContextRef */ IntPtr context, SslSessionOption option, byte value);

		/// <param name="option">To be added.</param>
		///         <param name="value">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SslStatus SetSessionOption (SslSessionOption option, bool value)
		{
			result = SSLSetSessionOption (Handle, option, value.AsByte ());
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static /* OSStatus */ SslStatus SSLSetClientSideAuthenticate (/* SSLContextRef */ IntPtr context, SslAuthenticate auth);

		/// <param name="auth">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SslStatus SetClientSideAuthenticate (SslAuthenticate auth)
		{
			result = SSLSetClientSideAuthenticate (Handle, auth);
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static /* OSStatus */ SslStatus SSLHandshake (/* SSLContextRef */ IntPtr context);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SslStatus Handshake ()
		{
			result = SSLHandshake (Handle);
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static /* OSStatus */ SslStatus SSLGetSessionState (/* SSLContextRef */ IntPtr context, SslSessionState* state);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SslSessionState SessionState {
			get {
				var value = SslSessionState.Invalid;
				unsafe {
					result = SSLGetSessionState (Handle, &value);
				}
				return value;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLGetPeerID (/* SSLContextRef */ IntPtr context, /* const void** */ IntPtr* peerID, /* size_t* */ nint* peerIDLen);

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLSetPeerID (/* SSLContextRef */ IntPtr context, /* const void** */ byte* peerID, /* size_t */ nint peerIDLen);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public unsafe byte []? PeerId {
			get {
				nint length;
				IntPtr id;
				unsafe {
					result = SSLGetPeerID (Handle, &id, &length);
				}
				if ((result != SslStatus.Success) || (length == 0))
					return null;
				var data = new byte [length];
				Marshal.Copy (id, data, 0, (int) length);
				return data;
			}
			set {
				nint length = (value is null) ? 0 : value.Length;
				fixed (byte* p = value) {
					result = SSLSetPeerID (Handle, p, length);
				}
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLGetBufferedReadSize (/* SSLContextRef */ IntPtr context, /* size_t* */ nint* bufSize);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint BufferedReadSize {
			get {
				nint value;
				unsafe {
					result = SSLGetBufferedReadSize (Handle, &value);
				}
				return value;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLRead (/* SSLContextRef */ IntPtr context, /* const void* */ byte* data, /* size_t */ nint dataLength, /* size_t* */ nint* processed);

		internal unsafe SslStatus Read (byte [] data, int offset, int size, out nint processed)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			processed = default;
			fixed (byte* d = &data [offset])
				result = SSLRead (Handle, d, size, (nint*) Unsafe.AsPointer<nint> (ref processed));
			return result;
		}

		public unsafe SslStatus Read (byte [] data, out nint processed)
		{
			int size = data is null ? 0 : data.Length;
			processed = default;
			fixed (byte* d = data)
				result = SSLRead (Handle, d, size, (nint*) Unsafe.AsPointer<nint> (ref processed));
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLWrite (/* SSLContextRef */ IntPtr context, /* const void* */ byte* data, /* size_t */ nint dataLength, /* size_t* */ nint* processed);

		internal unsafe SslStatus Write (byte [] data, int offset, int size, out nint processed)
		{
			if (data is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (data));
			processed = default;
			fixed (byte* d = &data [offset])
				result = SSLWrite (Handle, d, size, (nint*) Unsafe.AsPointer<nint> (ref processed));
			return result;
		}

		public unsafe SslStatus Write (byte [] data, out nint processed)
		{
			int size = data is null ? 0 : data.Length;
			processed = default;
			fixed (byte* d = data)
				result = SSLWrite (Handle, d, size, (nint*) Unsafe.AsPointer<nint> (ref processed));
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLGetDatagramWriteSize (/* SSLContextRef */ IntPtr context, /* size_t* */ nint* bufSize);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint DatagramWriteSize {
			get {
				nint value;
				unsafe {
					result = SSLGetDatagramWriteSize (Handle, &value);
				}
				return value;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLGetMaxDatagramRecordSize (/* SSLContextRef */ IntPtr context, /* size_t* */ nint* maxSize);

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLSetMaxDatagramRecordSize (/* SSLContextRef */ IntPtr context, /* size_t */ nint maxSize);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public nint MaxDatagramRecordSize {
			get {
				nint value;
				unsafe {
					result = SSLGetMaxDatagramRecordSize (Handle, &value);
				}
				return value;
			}
			set {
				result = SSLSetMaxDatagramRecordSize (Handle, value);
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLSetDatagramHelloCookie (/* SSLContextRef */ IntPtr context, /* const void* */ byte* cookie, nint cookieLength);

		/// <param name="cookie">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe SslStatus SetDatagramHelloCookie (byte [] cookie)
		{
			nint len = cookie is null ? 0 : cookie.Length;
			fixed (byte* p = cookie)
				result = SSLSetDatagramHelloCookie (Handle, p, len);
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLGetPeerDomainNameLength (/* SSLContextRef */ IntPtr context, /* size_t* */ nint* peerNameLen);

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLGetPeerDomainName (/* SSLContextRef */ IntPtr context, /* char* */ byte* peerName, /* size_t */ nint* peerNameLen);

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLSetPeerDomainName (/* SSLContextRef */ IntPtr context, /* char* */ byte* peerName, /* size_t */ nint peerNameLen);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public string PeerDomainName {
			get {
				nint length;
				unsafe {
					result = SSLGetPeerDomainNameLength (Handle, &length);
				}
				if (result != SslStatus.Success || length == 0)
					return String.Empty;
				var bytes = new byte [length];
				unsafe {
					fixed (byte* bytesPtr = bytes) {
						result = SSLGetPeerDomainName (Handle, bytesPtr, &length);
					}
				}
				return result == SslStatus.Success ? Encoding.UTF8.GetString (bytes) : String.Empty;
			}
			set {
				if (value is null) {
					unsafe {
						result = SSLSetPeerDomainName (Handle, null, 0);
					}
				} else {
					var bytes = Encoding.UTF8.GetBytes (value);
					unsafe {
						fixed (byte* bytesPtr = bytes) {
							result = SSLSetPeerDomainName (Handle, bytesPtr, bytes.Length);
						}
					}
				}
			}
		}

		// SSLAddDistinguishedName
		// Documented as unsupported and unimplemented (both iOS and OSX)
#if false
		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLCopyDistinguishedNames (/* SSLContextRef */ IntPtr context, /* CFArrayRef* */ out IntPtr names);

		// TODO: need to setup a server that requires client-side certificates
		public IList<T>? GetDistinguishedNames<T> ()
		{
			IntPtr p;
			result = SSLCopyDistinguishedNames (Handle, out p);
			if (p == IntPtr.Zero)
				return null; // empty

			if (result != SslStatus.Success) {
				CFObject.CFRelease (p);
				return null; // error
			}
			var array = new CFArray (p, false);
			var list = new List<T> ();
			for (int i = 0; i < array.Count; i++) {
				// CFData -> X500DistinguishedName -> string
				list.Add (Convert <T> (p));
			}
			CFObject.CFRelease (p);
			return list;
		}

		T Convert<T> (IntPtr p)
		{
			object value = null;
			var tt = typeof(T);
			if (tt == typeof (NSData))
				value = new NSData (p);
			else if (tt == typeof (string))
				value = p.ToString ();
			// X500DistinguishedName
			// NSString ?
			return (T) value;
		}
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLSetCertificate (/* SSLContextRef */ IntPtr context, /* _Nullable CFArrayRef */ IntPtr certRefs);

		NSArray Bundle (SecIdentity? identity, IEnumerable<SecCertificate>? certificates)
		{
			// The analyzer cannot deal with arrays, we manually keep alive the whole array below
#pragma warning disable RBI0014
			int i = identity is null ? 0 : 1;
			int n = certificates is null ? 0 : certificates.Count ();
			var ptrs = new NativeHandle [n + i];
			if (i == 1)
				ptrs [0] = identity!.Handle;
			if (certificates is not null) {
				foreach (var certificate in certificates)
					ptrs [i++] = certificate.Handle;
			}
			NSArray result = NSArray.FromIntPtrs (ptrs);
			GC.KeepAlive (identity);
			GC.KeepAlive (certificates);
			return result;
#pragma warning restore RBI0014
		}

		/// <param name="identify">To be added.</param>
		///         <param name="certificates">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SslStatus SetCertificate (SecIdentity identify, IEnumerable<SecCertificate> certificates)
		{
			using (var array = Bundle (identify, certificates)) {
				result = SSLSetCertificate (Handle, array.Handle);
				GC.KeepAlive (array);
				return result;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLGetClientCertificateState (/* SSLContextRef */ IntPtr context, SslClientCertificateState* clientState);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SslClientCertificateState ClientCertificateState {
			get {
				SslClientCertificateState value;
				unsafe {
					result = SSLGetClientCertificateState (Handle, &value);
				}
				return value;
			}
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst", "The use of different RSA certificates for signing and encryption is no longer allowed.")]
		[ObsoletedOSPlatform ("macos", "The use of different RSA certificates for signing and encryption is no longer allowed.")]
		[ObsoletedOSPlatform ("tvos", "The use of different RSA certificates for signing and encryption is no longer allowed.")]
		[ObsoletedOSPlatform ("ios", "The use of different RSA certificates for signing and encryption is no longer allowed.")]
		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLSetEncryptionCertificate (/* SSLContextRef */ IntPtr context, /* CFArrayRef */ IntPtr certRefs);

		/// <param name="identify">To be added.</param>
		///         <param name="certificates">To be added.</param>
		///         <summary>Developers should not use this deprecated method. Export ciphers are not available anymore.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst", "Export ciphers are not available anymore.")]
		[ObsoletedOSPlatform ("macos", "Export ciphers are not available anymore.")]
		[ObsoletedOSPlatform ("tvos", "Export ciphers are not available anymore.")]
		[ObsoletedOSPlatform ("ios", "Export ciphers are not available anymore.")]
		public SslStatus SetEncryptionCertificate (SecIdentity identify, IEnumerable<SecCertificate> certificates)
		{
			using (var array = Bundle (identify, certificates)) {
				result = SSLSetEncryptionCertificate (Handle, array.Handle);
				GC.KeepAlive (array);
				return result;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* OSStatus */ SslStatus SSLCopyPeerTrust (/* SSLContextRef */ IntPtr context, /* SecTrustRef */ IntPtr* trust);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public SecTrust? PeerTrust {
			get {
				IntPtr value;
				unsafe {
					result = SSLCopyPeerTrust (Handle, &value);
				}
				return (value == IntPtr.Zero) ? null : new SecTrust (value, true);
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern unsafe static /* CFType */ IntPtr SSLContextGetTypeID ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static IntPtr GetTypeId ()
		{
			return SSLContextGetTypeID ();
		}

		// TODO: Headers say /* Deprecated, does nothing */ but we are not completly sure about it since there is no deprecation macro
		// Plus they added new members to SslSessionStrengthPolicy enum opened radar://23379052 https://trello.com/c/NbdTLVD3
		// Xcode 8 beta 1: the P/Invoke was removed completely.

#if !XAMCORE_5_0
		/// <param name="policyStrength">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[UnsupportedOSPlatform ("maccatalyst")]
		[UnsupportedOSPlatform ("tvos")]
		[UnsupportedOSPlatform ("ios")]
		[UnsupportedOSPlatform ("macos")]
		[Obsolete ("'SetSessionStrengthPolicy' is not available anymore.")]
		[EditorBrowsable (EditorBrowsableState.Never)]
		public SslStatus SetSessionStrengthPolicy (SslSessionStrengthPolicy policyStrength)
		{
			Runtime.NSLog ("SetSessionStrengthPolicy is not available anymore.");
			return SslStatus.Success;
		}
#endif // !XAMCORE_5_0

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern int SSLSetSessionConfig (IntPtr /* SSLContextRef* */ context, IntPtr /* CFStringRef* */ config);

		/// <param name="config">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		public int SetSessionConfig (NSString config)
		{
			if (config is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (config));

			int result = SSLSetSessionConfig (Handle, config.Handle);
			GC.KeepAlive (config);
			return result;
		}

		/// <param name="config">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public int SetSessionConfig (SslSessionConfig config)
		{
			return SetSessionConfig (config.GetConstant ()!);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern int SSLReHandshake (IntPtr /* SSLContextRef* */ context);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public int ReHandshake ()
		{
			return SSLReHandshake (Handle);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern /* OSStatus */ SslStatus SSLCopyRequestedPeerName (IntPtr /* SSLContextRef* */ context, byte* /* char* */ peerName, nuint* /* size_t */ peerNameLen);

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern /* OSStatus */ SslStatus SSLCopyRequestedPeerNameLength (IntPtr /* SSLContextRef* */ context, nuint* /* size_t */ peerNameLen);

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public string GetRequestedPeerName ()
		{
			var result = String.Empty;
			nuint length = 0;
			unsafe {
				if (SSLCopyRequestedPeerNameLength (Handle, &length) == SslStatus.Success) {
					var bytes = new byte [length];
					fixed (byte* bytesPtr = bytes) {
						if (SSLCopyRequestedPeerName (Handle, bytesPtr, &length) == SslStatus.Success)
							result = Encoding.UTF8.GetString (bytes);
					}
				}
			}
			return result;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern /* OSStatus */ int SSLSetSessionTicketsEnabled (IntPtr /* SSLContextRef */ context, byte /* Boolean */ enabled);

		/// <param name="enabled">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public int SetSessionTickets (bool enabled)
		{
			return SSLSetSessionTicketsEnabled (Handle, enabled.AsByte ());
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern /* OSStatus */ int SSLSetError (IntPtr /* SSLContextRef */ context, SecStatusCode /* OSStatus */ status);

		/// <param name="status">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public int SetError (SecStatusCode status)
		{
			return SSLSetError (Handle, status);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern /* OSStatus */ int SSLSetOCSPResponse (IntPtr /* SSLContextRef */ context, IntPtr /* CFDataRef __nonnull */ response);

		/// <param name="response">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public int SetOcspResponse (NSData response)
		{
			if (response is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (response));
			int result = SSLSetOCSPResponse (Handle, response.Handle);
			GC.KeepAlive (response);
			return result;
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		static extern /* OSStatus */ int SSLSetALPNProtocols (IntPtr /* SSLContextRef */ context, IntPtr /* CFArrayRef */ protocols);

		/// <param name="protocols">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public int SetAlpnProtocols (string [] protocols)
		{
			using (var array = NSArray.FromStrings (protocols))
				return SSLSetALPNProtocols (Handle, array.Handle);
		}

		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern /* OSStatus */ int SSLCopyALPNProtocols (IntPtr /* SSLContextRef */ context, IntPtr* /* CFArrayRef* */ protocols);

		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]

		public string? [] GetAlpnProtocols (out int error)
		{
			IntPtr protocols = IntPtr.Zero; // must be null, CFArray allocated by SSLCopyALPNProtocols
			unsafe {
				error = SSLCopyALPNProtocols (Handle, &protocols);
			}
			if (protocols == IntPtr.Zero)
				return Array.Empty<string> ();
			return CFArray.StringArrayFromHandle (protocols, true)!;
		}

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[ObsoletedOSPlatform ("maccatalyst", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("macos", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Network.framework' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Network.framework' instead.")]
		public string? [] GetAlpnProtocols ()
		{
			int error;
			return GetAlpnProtocols (out error);
		}
	}
}
