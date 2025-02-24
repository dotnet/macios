// 
// SecTrust.cs: Implements the managed SecTrust wrapper.
//
// Authors: 
//  Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2013-2014 Xamarin Inc.
// Copyright 2019 Microsoft Corporation
//

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using ObjCRuntime;
using CoreFoundation;
using Foundation;

namespace Security {

	public delegate void SecTrustCallback (SecTrust? trust, SecTrustResult trustResult);
	public delegate void SecTrustWithErrorCallback (SecTrust? trust, bool result, NSError? /* CFErrorRef _Nullable */ error);

	public partial class SecTrust {

		/// <param name="certificate">To be added.</param>
		///         <param name="policy">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public SecTrust (SecCertificate certificate, SecPolicy policy)
		{
			if (certificate is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (certificate));

			Initialize (certificate.Handle, policy);
			GC.KeepAlive (certificate);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode /* OSStatus */ SecTrustCopyPolicies (IntPtr /* SecTrustRef */ trust, IntPtr* /* CFArrayRef* */ policies);

#if NET
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public SecPolicy [] GetPolicies ()
		{
			IntPtr p = IntPtr.Zero;
			SecStatusCode result;
			unsafe {
				result = SecTrustCopyPolicies (Handle, &p);
			}
			if (result != SecStatusCode.Success)
				throw new InvalidOperationException (result.ToString ());
			return NSArray.ArrayFromHandle<SecPolicy> (p);
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode /* OSStatus */ SecTrustSetPolicies (IntPtr /* SecTrustRef */ trust, IntPtr /* CFTypeRef */ policies);

		// the API accept the handle for a single policy or an array of them
		void SetPolicies (IntPtr policy)
		{
			SecStatusCode result = SecTrustSetPolicies (Handle, policy);
			if (result != SecStatusCode.Success)
				throw new InvalidOperationException (result.ToString ());
		}

		/// <param name="policy">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetPolicy (SecPolicy policy)
		{
			if (policy is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (policy));

			SetPolicies (policy.Handle);
			GC.KeepAlive (policy);
		}

		/// <param name="policies">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetPolicies (IEnumerable<SecPolicy> policies)
		{
			if (policies is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (policies));

			using (var array = NSArray.FromNSObjects (policies.ToArray ()))
				SetPolicies (array.Handle);
		}

		/// <param name="policies">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public void SetPolicies (NSArray policies)
		{
			if (policies is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (policies));

			SetPolicies (policies.Handle);
			GC.KeepAlive (policies);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode /* OSStatus */ SecTrustGetNetworkFetchAllowed (IntPtr /* SecTrustRef */ trust, byte* /* Boolean* */ allowFetch);

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode /* OSStatus */ SecTrustSetNetworkFetchAllowed (IntPtr /* SecTrustRef */ trust, byte /* Boolean */ allowFetch);

#if NET
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public bool NetworkFetchAllowed {
			get {
				byte value;
				SecStatusCode result;
				unsafe {
					result = SecTrustGetNetworkFetchAllowed (Handle, &value);
				}
				if (result != SecStatusCode.Success)
					throw new InvalidOperationException (result.ToString ());
				return value != 0;
			}
			set {
				SecStatusCode result = SecTrustSetNetworkFetchAllowed (Handle, value.AsByte ());
				if (result != SecStatusCode.Success)
					throw new InvalidOperationException (result.ToString ());
			}
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode /* OSStatus */ SecTrustCopyCustomAnchorCertificates (IntPtr /* SecTrustRef */ trust, IntPtr* /* CFArrayRef* */ anchors);

#if NET
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public SecCertificate [] GetCustomAnchorCertificates ()
		{
			IntPtr p;
			SecStatusCode result;
			unsafe {
				result = SecTrustCopyCustomAnchorCertificates (Handle, &p);
			}
			if (result != SecStatusCode.Success)
				throw new InvalidOperationException (result.ToString ());
			return NSArray.ArrayFromHandle<SecCertificate> (p);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode /* OSStatus */ SecTrustEvaluateAsync (IntPtr /* SecTrustRef */ trust, IntPtr /* dispatch_queue_t */ queue, BlockLiteral* block);

#if !NET
		internal delegate void TrustEvaluateHandler (IntPtr block, IntPtr trust, SecTrustResult trustResult);
		static readonly TrustEvaluateHandler evaluate = TrampolineEvaluate;

		[MonoPInvokeCallback (typeof (TrustEvaluateHandler))]
#else
		[UnmanagedCallersOnly]
#endif
		static void TrampolineEvaluate (IntPtr block, IntPtr trust, SecTrustResult trustResult)
		{
			var del = BlockLiteral.GetTarget<SecTrustCallback> (block);
			if (del is not null) {
				var t = trust == IntPtr.Zero ? null : new SecTrust (trust, false);
				del (t, trustResult);
			}
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[ObsoletedOSPlatform ("tvos13.0", "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[ObsoletedOSPlatform ("ios13.0", "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
#else
		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use 'Evaluate (DispatchQueue, SecTrustWithErrorCallback)' instead.")]
#endif
		[BindingImpl (BindingImplOptions.Optimizable)]
		public SecStatusCode Evaluate (DispatchQueue queue, SecTrustCallback handler)
		{
			// headers have `dispatch_queue_t _Nullable queue` but it crashes... don't trust headers, even for SecTrust
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
#if NET
				delegate* unmanaged<IntPtr, IntPtr, SecTrustResult, void> trampoline = &TrampolineEvaluate;
				using var block = new BlockLiteral (trampoline, handler, typeof (SecTrust), nameof (TrampolineEvaluate));
#else
				using var block = new BlockLiteral ();
				block.SetupBlockUnsafe (evaluate, handler);
#endif
				SecStatusCode status = SecTrustEvaluateAsync (Handle, queue.Handle, &block);
				GC.KeepAlive (queue);
				return status;
			}
		}

#if NET
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[TV (13, 0)]
		[iOS (13, 0)]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern SecStatusCode SecTrustEvaluateAsyncWithError (IntPtr /* SecTrustRef */ trust, IntPtr /* dispatch_queue_t */ queue, BlockLiteral* block);

#if !NET
		internal delegate void TrustEvaluateErrorHandler (IntPtr block, IntPtr trust, byte result, IntPtr /* CFErrorRef _Nullable */  error);
		static readonly TrustEvaluateErrorHandler evaluate_error = TrampolineEvaluateError;

		[MonoPInvokeCallback (typeof (TrustEvaluateErrorHandler))]
#else
		[UnmanagedCallersOnly]
#endif
		static void TrampolineEvaluateError (IntPtr block, IntPtr trust, byte result, IntPtr /* CFErrorRef _Nullable */  error)
		{
			var del = BlockLiteral.GetTarget<SecTrustWithErrorCallback> (block);
			if (del is not null) {
				var t = trust == IntPtr.Zero ? null : new SecTrust (trust, false);
				var e = error == IntPtr.Zero ? null : new NSError (error);
				del (t, result != 0, e);
			}
		}

#if NET
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("maccatalyst")]
#else
		[TV (13, 0)]
		[iOS (13, 0)]
#endif
		[BindingImpl (BindingImplOptions.Optimizable)]
		public SecStatusCode Evaluate (DispatchQueue queue, SecTrustWithErrorCallback handler)
		{
			if (queue is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (queue));
			if (handler is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handler));

			unsafe {
#if NET
				delegate* unmanaged<IntPtr, IntPtr, byte, IntPtr, void> trampoline = &TrampolineEvaluateError;
				using var block = new BlockLiteral (trampoline, handler, typeof (SecTrust), nameof (TrampolineEvaluateError));
#else
				using var block = new BlockLiteral ();
				block.SetupBlockUnsafe (evaluate_error, handler);
#endif
				SecStatusCode status = SecTrustEvaluateAsyncWithError (Handle, queue.Handle, &block);
				GC.KeepAlive (queue);
				return status;
			}
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe extern static SecStatusCode /* OSStatus */ SecTrustGetTrustResult (IntPtr /* SecTrustRef */ trust, SecTrustResult* /* SecTrustResultType */ result);

#if NET
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public SecTrustResult GetTrustResult ()
		{
			SecTrustResult trust_result;
			SecStatusCode result;
			unsafe {
				result = SecTrustGetTrustResult (Handle, &trust_result);
			}
			if (result != SecStatusCode.Success)
				throw new InvalidOperationException (result.ToString ());
			return trust_result;
		}

#if NET
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		unsafe static extern byte SecTrustEvaluateWithError (/* SecTrustRef */ IntPtr trust, /* CFErrorRef** */ IntPtr* error);

#if NET
		/// <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public bool Evaluate (out NSError? error)
		{
			IntPtr err;
			bool result;
			unsafe {
				result = SecTrustEvaluateWithError (Handle, &err) != 0;
			}
			error = err == IntPtr.Zero ? null : new NSError (err);
			return result;
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr /* CFDictionaryRef */ SecTrustCopyResult (IntPtr /* SecTrustRef */ trust);

#if NET
		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		public NSDictionary GetResult ()
		{
			return new NSDictionary (SecTrustCopyResult (Handle), true);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		extern static SecStatusCode /* OSStatus */ SecTrustSetOCSPResponse (IntPtr /* SecTrustRef */ trust, IntPtr /* CFTypeRef */ responseData);

		// the API accept the handle for a single policy or an array of them
#if NET
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("tvos")]
#endif
		void SetOCSPResponse (IntPtr ocsp)
		{
			SecStatusCode result = SecTrustSetOCSPResponse (Handle, ocsp);
			if (result != SecStatusCode.Success)
				throw new InvalidOperationException (result.ToString ());
		}

#if NET
		/// <param name="ocspResponse">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public void SetOCSPResponse (NSData ocspResponse)
		{
			if (ocspResponse is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ocspResponse));

			SetOCSPResponse (ocspResponse.Handle);
			GC.KeepAlive (ocspResponse);
		}

#if NET
		/// <param name="ocspResponses">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public void SetOCSPResponse (IEnumerable<NSData> ocspResponses)
		{
			if (ocspResponses is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ocspResponses));

			using (var array = NSArray.FromNSObjects (ocspResponses.ToArray ()))
				SetOCSPResponse (array.Handle);
		}

#if NET
		/// <param name="ocspResponses">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("tvos")]
#endif
		public void SetOCSPResponse (NSArray ocspResponses)
		{
			if (ocspResponses is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (ocspResponses));

			SetOCSPResponse (ocspResponses.Handle);
			GC.KeepAlive (ocspResponses);
		}

#if NET
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		static extern SecStatusCode /* OSStatus */ SecTrustSetSignedCertificateTimestamps (/* SecTrustRef* */ IntPtr trust, /* CFArrayRef* */ IntPtr sctArray);

#if NET
		/// <param name="sct">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public SecStatusCode SetSignedCertificateTimestamps (IEnumerable<NSData> sct)
		{
			if (sct is null)
				return SecTrustSetSignedCertificateTimestamps (Handle, IntPtr.Zero);

			using (var array = NSArray.FromNSObjects (sct.ToArray ()))
				return SecTrustSetSignedCertificateTimestamps (Handle, array.Handle);
		}

#if NET
		/// <param name="sct">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("maccatalyst")]
#endif
		public SecStatusCode SetSignedCertificateTimestamps (NSArray<NSData> sct)
		{
			SecStatusCode status = SecTrustSetSignedCertificateTimestamps (Handle, sct.GetHandle ());
			GC.KeepAlive (sct);
			return status;
		}
	}
}
