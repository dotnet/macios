// 
// Trust.cs: Implements the managed SecTrust wrapper.
//
// Authors: 
//	Miguel de Icaza
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2010 Novell, Inc
// Copyright 2012-2014 Xamarin Inc.
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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using ObjCRuntime;
using CoreFoundation;
using Foundation;
#if NET
#else
using NativeHandle = System.IntPtr;
#endif

namespace Security {
	public partial class SecTrust : NativeObject {
#if !NET
		public SecTrust (NativeHandle handle)
			: base (handle, false)
		{
		}
#endif

		[Preserve (Conditional = true)]
		internal SecTrust (NativeHandle handle, bool owns)
			: base (handle, owns)
		{
		}

#if !COREBUILD

		/// <summary>Type identifier for the Security.SecTrust type.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para>The returned token is the CoreFoundation type identifier (CFType) that has been assigned to this class.</para>
		///           <para>This can be used to determine type identity between different CoreFoundation objects.</para>
		///           <para>You can retrieve the type of a CoreFoundation object by invoking the <see cref="M:CoreFoundation.CFType.GetTypeID(System.IntPtr)" /> on the native handle of the object</para>
		///           <example>
		///             <code lang="csharp lang-csharp"><![CDATA[bool isSecTrust = (CFType.GetTypeID (foo.Handle) == SecTrust.GetTypeID ());]]></code>
		///           </example>
		///         </remarks>
		[DllImport (Constants.SecurityLibrary, EntryPoint = "SecTrustGetTypeID")]
		public extern static nint GetTypeID ();

		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode SecTrustCreateWithCertificates (
			/* CFTypeRef */            IntPtr certOrCertArray,
			/* CFTypeRef __nullable */ IntPtr policies,
			/* SecTrustRef *__nonull */ IntPtr* sectrustref);


		/// <param name="certificate">The certificate to be evaluated.</param>
		///         <param name="policy">The policy to be used to evaluate the trust.</param>
		///         <summary>Create a new instance based on the certificate, to be evaluated, and a policy, to be applied.</summary>
		///         <remarks>
		///         </remarks>
		public SecTrust (X509Certificate certificate, SecPolicy? policy)
		{
			if (certificate is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (certificate));

			using (SecCertificate cert = new SecCertificate (certificate)) {
				Initialize (cert.Handle, policy);
				GC.KeepAlive (cert);
			}
		}

		/// <param name="certificate">The certificate to be evaluated.</param>
		///         <param name="policy">The policy to be used to evaluate the trust.</param>
		///         <summary>Create a new instance based on the certificate, to be evaluated, and a policy, to be applied</summary>
		///         <remarks>
		///         </remarks>
		public SecTrust (X509Certificate2 certificate, SecPolicy? policy)
		{
			if (certificate is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (certificate));

			using (SecCertificate cert = new SecCertificate (certificate)) {
				Initialize (cert.Handle, policy);
				GC.KeepAlive (cert);
			}
		}

		/// <param name="certificates">A collection of X.509 certificates</param>
		///         <param name="policy">The policy to be used to evaluate the trust.</param>
		///         <summary>Create a new instance based on the certificate, to be evaluated, and a policy, to be applied.</summary>
		///         <remarks>The first certificate (in the collection) is the one to be evaluated, the others will be used to build a chain of trust.</remarks>
		public SecTrust (X509CertificateCollection certificates, SecPolicy? policy)
		{
			if (certificates is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (certificates));

			SecCertificate [] array = new SecCertificate [certificates.Count];
			int i = 0;
			foreach (var certificate in certificates)
				array [i++] = new SecCertificate (certificate);
			Initialize (array, policy);
		}

		/// <param name="certificates">A collection of X.509 certificates</param>
		///         <param name="policy">The policy to be used to evaluate the trust.</param>
		///         <summary>Create a new instance based on the certificate, to be evaluated, and a policy, to be applied.</summary>
		///         <remarks>The first certificate (in the collection) is the one to be evaluated, the others will be used to build a chain of trust.</remarks>
		public SecTrust (X509Certificate2Collection certificates, SecPolicy? policy)
		{
			if (certificates is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (certificates));

			SecCertificate [] array = new SecCertificate [certificates.Count];
			int i = 0;
			foreach (var certificate in certificates)
				array [i++] = new SecCertificate (certificate);
			Initialize (array, policy);
		}

		void Initialize (SecCertificate [] array, SecPolicy? policy)
		{
			using (var certs = CFArray.FromNativeObjects (array)) {
				Initialize (certs.Handle, policy);
				GC.KeepAlive (certs);
			}
		}

		void Initialize (IntPtr certHandle, SecPolicy? policy)
		{
			IntPtr handle;
			SecStatusCode result;
			unsafe {
				result = SecTrustCreateWithCertificates (certHandle, policy.GetHandle (), &handle);
				GC.KeepAlive (policy);
			}
			if (result != SecStatusCode.Success)
				throw new ArgumentException (result.ToString ());
			InitializeHandle (handle);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos12.1", "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[ObsoletedOSPlatform ("macos10.14.1", "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[ObsoletedOSPlatform ("ios12.1", "Use 'SecTrust.Evaluate (out NSError)' instead.")]
#else
		[Deprecated (PlatformName.iOS, 12, 1, message: "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[Deprecated (PlatformName.TvOS, 12, 1, message: "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 14, 1, message: "Use 'SecTrust.Evaluate (out NSError)' instead.")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode /* OSStatus */ SecTrustEvaluate (IntPtr /* SecTrustRef */ trust, /* SecTrustResultType */ SecTrustResult* result);

#if NET
		/// <summary>Evaluate the trust of the certificate using the policy.</summary>
		///         <returns>A code that describe if the certificate can be trusted and, if so, under which circumstances.</returns>
		///         <remarks>In general both Proceed and Unspecified means you can trust the certificate, other values means it should not be trusted.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("tvos12.1", "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[ObsoletedOSPlatform ("macos10.14.1", "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[ObsoletedOSPlatform ("ios12.1", "Use 'SecTrust.Evaluate (out NSError)' instead.")]
#else
		[Deprecated (PlatformName.iOS, 12, 1, message: "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[Deprecated (PlatformName.TvOS, 12, 1, message: "Use 'SecTrust.Evaluate (out NSError)' instead.")]
		[Deprecated (PlatformName.MacOSX, 10, 14, 1, message: "Use 'SecTrust.Evaluate (out NSError)' instead.")]
#endif
		public SecTrustResult Evaluate ()
		{
			SecTrustResult trust;
			SecStatusCode result;
			unsafe {
				result = SecTrustEvaluate (GetCheckedHandle (), &trust);
			}
			if (result != SecStatusCode.Success)
				throw new InvalidOperationException (result.ToString ());
			return trust;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static nint /* CFIndex */ SecTrustGetCertificateCount (IntPtr /* SecTrustRef */ trust);

		/// <summary>Return the number of certificates used for evaluation.</summary>
		///         <value>The number of certificates.</value>
		///         <remarks>There can be more and different certificates than the one provided to the constructor.</remarks>
		public int Count {
			get {
				if (Handle == IntPtr.Zero)
					return 0;
				return (int) SecTrustGetCertificateCount (Handle);
			}
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos12.0")]
		[ObsoletedOSPlatform ("maccatalyst15.0")]
		[ObsoletedOSPlatform ("tvos15.0")]
		[ObsoletedOSPlatform ("ios15.0")]
#else
		[Deprecated (PlatformName.MacOSX, 12, 0)]
		[Deprecated (PlatformName.iOS, 15, 0)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0)]
		[Deprecated (PlatformName.TvOS, 15, 0)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* SecCertificateRef */ SecTrustGetCertificateAtIndex (IntPtr /* SecTrustRef */ trust, nint /* CFIndex */ ix);

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos12.0", "Use the 'GetCertificateChain' method instead.")]
		[ObsoletedOSPlatform ("maccatalyst15.0", "Use the 'GetCertificateChain' method instead.")]
		[ObsoletedOSPlatform ("tvos15.0", "Use the 'GetCertificateChain' method instead.")]
		[ObsoletedOSPlatform ("ios15.0", "Use the 'GetCertificateChain' method instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 12, 0, message: "Use the 'GetCertificateChain' method instead.")]
		[Deprecated (PlatformName.iOS, 15, 0, message: "Use the 'GetCertificateChain' method instead.")]
		[Deprecated (PlatformName.MacCatalyst, 15, 0, message: "Use the 'GetCertificateChain' method instead.")]
		[Deprecated (PlatformName.TvOS, 15, 0, message: "Use the 'GetCertificateChain' method instead.")]
#endif
		public SecCertificate this [nint index] {
			get {
				if ((index < 0) || (index >= Count))
					throw new ArgumentOutOfRangeException (nameof (index));

				return new SecCertificate (SecTrustGetCertificateAtIndex (GetCheckedHandle (), index), false);
			}
		}

#if NET
		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[TV (15, 0)]
		[iOS (15, 0)]
		[MacCatalyst (15, 0)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		static extern /* CFArrayRef */ IntPtr SecTrustCopyCertificateChain (/* SecTrustRef */ IntPtr trust);

#if NET
		[SupportedOSPlatform ("tvos15.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios15.0")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[TV (15, 0)]
		[iOS (15, 0)]
		[MacCatalyst (15, 0)]
#endif
		public SecCertificate [] GetCertificateChain ()
			=> NSArray.ArrayFromHandle<SecCertificate> (SecTrustCopyCertificateChain (Handle));

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos11.0")]
		[ObsoletedOSPlatform ("tvos14.0")]
		[ObsoletedOSPlatform ("ios14.0")]
		[ObsoletedOSPlatform ("maccatalyst14.0")]
#else
		[Deprecated (PlatformName.iOS, 14, 0)]
		[Deprecated (PlatformName.MacOSX, 11, 0)]
		[Deprecated (PlatformName.TvOS, 14, 0)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* SecKeyRef */ SecTrustCopyPublicKey (IntPtr /* SecTrustRef */ trust);

#if NET
		/// <summary>Get the public key of the evaluated certificate.</summary>
		///         <returns>A SecKey instance of the certificate's public key.</returns>
		///         <remarks>
		///         </remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos11.0", "Use 'GetKey' instead.")]
		[ObsoletedOSPlatform ("tvos14.0", "Use 'GetKey' instead.")]
		[ObsoletedOSPlatform ("ios14.0", "Use 'GetKey' instead.")]
#else
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'GetKey' instead.")]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'GetKey' instead.")]
		[Deprecated (PlatformName.TvOS, 14, 0, message: "Use 'GetKey' instead.")]
#endif
		public SecKey GetPublicKey ()
		{
			return new SecKey (SecTrustCopyPublicKey (GetCheckedHandle ()), true);
		}

#if NET
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* SecKeyRef */ SecTrustCopyKey (IntPtr /* SecTrustRef */ trust);

#if NET
		[SupportedOSPlatform ("ios14.0")]
		[SupportedOSPlatform ("tvos14.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[iOS (14, 0)]
		[TV (14, 0)]
		[MacCatalyst (14, 0)]
#endif
		public SecKey GetKey ()
		{
			return new SecKey (SecTrustCopyKey (GetCheckedHandle ()), true);
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* CFDataRef */ SecTrustCopyExceptions (IntPtr /* SecTrustRef */ trust);

#if NET
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public NSData? GetExceptions ()
		{
			return Runtime.GetNSObject<NSData> (SecTrustCopyExceptions (GetCheckedHandle ()), true);
		}

#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static byte SecTrustSetExceptions (IntPtr /* SecTrustRef */ trust, IntPtr /* __nullable CFDataRef */ exceptions);

#if NET
		/// <param name="data">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public bool SetExceptions (NSData data)
		{
			bool result = SecTrustSetExceptions (GetCheckedHandle (), data.GetHandle ()) != 0;
			GC.KeepAlive (data);
			return result;
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static double /* CFAbsoluteTime */ SecTrustGetVerifyTime (IntPtr /* SecTrustRef */ trust);

		/// <summary>Get the verification time.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>This is often used for digital signatures.</remarks>
		public double GetVerifyTime ()
		{
			return SecTrustGetVerifyTime (GetCheckedHandle ());
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode /* OSStatus */ SecTrustSetVerifyDate (IntPtr /* SecTrustRef */ trust, IntPtr /* CFDateRef */ verifyDate);

		/// <param name="date">Date for which the verification should made.</param>
		///         <summary>Set the date at which the trust is to be evaluated.</summary>
		///         <returns>An error code.</returns>
		///         <remarks>This is often used for digital signatures.</remarks>
		public SecStatusCode SetVerifyDate (DateTime date)
		{
			// CFDateRef amd NSDate are toll-freee bridged
			using (NSDate d = (NSDate) date) {
				SecStatusCode statusCode = SecTrustSetVerifyDate (GetCheckedHandle (), d.Handle);
				return statusCode;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode /* OSStatus */ SecTrustSetAnchorCertificates (IntPtr /* SecTrustRef */ trust, IntPtr /* CFArrayRef */ anchorCertificates);

		/// <param name="certificates">A collection of anchor (trusted root) certificates.</param>
		///         <summary>Provide your own collection of trusted certificate for the evaluation.</summary>
		///         <returns>An error code.</returns>
		///         <remarks>
		///         </remarks>
		public SecStatusCode SetAnchorCertificates (X509CertificateCollection certificates)
		{
			if (certificates is null)
				return SecTrustSetAnchorCertificates (GetCheckedHandle (), IntPtr.Zero);

			SecCertificate [] array = new SecCertificate [certificates.Count];
			int i = 0;
			foreach (var certificate in certificates)
				array [i++] = new SecCertificate (certificate);
			return SetAnchorCertificates (array);
		}

		/// <param name="certificates">A collection of anchor (trusted root) certificates.</param>
		///         <summary>Provide your own collection of trusted certificate for the evaluation.</summary>
		///         <returns>An error code.</returns>
		///         <remarks>
		///         </remarks>
		public SecStatusCode SetAnchorCertificates (X509Certificate2Collection certificates)
		{
			if (certificates is null)
				return SecTrustSetAnchorCertificates (GetCheckedHandle (), IntPtr.Zero);

			SecCertificate [] array = new SecCertificate [certificates.Count];
			int i = 0;
			foreach (var certificate in certificates)
				array [i++] = new SecCertificate (certificate);
			return SetAnchorCertificates (array);
		}

		/// <param name="array">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public SecStatusCode SetAnchorCertificates (SecCertificate [] array)
		{
			if (array is null)
				return SecTrustSetAnchorCertificates (Handle, IntPtr.Zero);
			using (var certs = CFArray.FromNativeObjects (array)) {
				SecStatusCode statusCode = SecTrustSetAnchorCertificates (Handle, certs.Handle);
				GC.KeepAlive (certs);
				return statusCode;
			}
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode /* OSStatus */ SecTrustSetAnchorCertificatesOnly (IntPtr /* SecTrustRef */ trust, byte anchorCertificatesOnly);

		/// <param name="anchorCertificatesOnly">true if only the supplied anchors should be used, false if the system anchors should also be used</param>
		///         <summary>Specify if only the supplied anchor certificates should be used.</summary>
		///         <returns>An error code.</returns>
		///         <remarks>
		///         </remarks>
		public SecStatusCode SetAnchorCertificatesOnly (bool anchorCertificatesOnly)
		{
			return SecTrustSetAnchorCertificatesOnly (GetCheckedHandle (), anchorCertificatesOnly.AsByte ());
		}
#endif
	}
}
