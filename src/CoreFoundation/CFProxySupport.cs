// 
// CFProxySupport.cs: Implements the managed binding for CFProxySupport.h
//
// Authors: Jeffrey Stedfast <jeff@xamarin.com>
//     
// Copyright (C) 2011 Xamarin, Inc.
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
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Versioning;
using ObjCRuntime;
using Foundation;
using CFNetwork;

namespace CoreFoundation {
	/// <summary>Provides information about a proxy.</summary>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public partial class CFProxy {
		NSDictionary settings;

		internal CFProxy (NSDictionary settings)
		{
			this.settings = settings;
		}

		public CFHTTPMessage? AutoConfigurationHttpResponse {
			get {
				var key = AutoConfigurationHttpResponseKey;
				if (key is null)
					return null;

				var rv = settings.LowlevelObjectForKey (key.GetHandle ());
				GC.KeepAlive (key);
				return Runtime.GetINativeObject<CFHTTPMessage> (rv, false);
			}
		}

		/// <summary>JavaScript source code for auto-configuring the proxy.</summary>
		/// <value>A string containing the JavaScript source code.</value>
		/// <remarks>
		///     This value is meant to be used with
		/// 	<see cref="CoreFoundation.CFNetwork.GetProxiesForAutoConfigurationScript(Foundation.NSString,System.Uri)" />
		/// 	and will only be set if <see cref="CoreFoundation.CFProxy.ProxyType" /> is set to
		/// 	AutoConfigurationJavaScript.
		/// </remarks>
		public NSString? AutoConfigurationJavaScript {
			get {
				if (AutoConfigurationJavaScriptKey is null)
					return null;

				return (NSString?) settings [AutoConfigurationJavaScriptKey];
			}
		}

		/// <summary>URL location of a proxy auto-configuration script.</summary>
		/// <value>A URL containing the location of the proxy auto-configuration script file.</value>
		/// <remarks>This property will only set when <see cref="CoreFoundation.CFProxy.ProxyType" /> is set to AutoConfigurationUrl.</remarks>
		public NSUrl? AutoConfigurationUrl {
			get {
				var key = AutoConfigurationUrlKey;
				if (key is null)
					return null;

				return (NSUrl?) settings [key];
			}
		}

		/// <summary>The hostname of the proxy server.</summary>
		/// <value>A string containing the hostname of the proxy server.</value>
		/// <remarks>This property will be null if the <see cref="CoreFoundation.CFProxy.ProxyType" /> is set to AutoConfigurationJavaScript or AutoConfigurationUrl.</remarks>
		public string? HostName {
			get {
				var key = HostNameKey;
				if (key is null)
					return null;

				return settings [key]?.ToString ();
			}
		}

		/// <summary>The password needed to authenticate with the proxy server.</summary>
		/// <value>A string containing the password or null if not needed.</value>
		public string? Password {
			get {
				var key = PasswordKey;
				if (key is null)
					return null;

				return settings [key]?.ToString ();
			}
		}

		/// <summary>The port to connect to on the proxy server.</summary>
		/// <value>The numeric port to connect to.</value>
		/// <remarks>This property is not useful if the <see cref="CoreFoundation.CFProxy.ProxyType" /> is javascript or a url.</remarks>
		public int Port {
			get {
				var key = PortNumberKey;
				if (key is null)
					return 0;

				var v = (NSNumber?) settings [key];

				return v?.Int32Value ?? 0;
			}
		}

		/// <summary>The type of the proxy.</summary>
		/// <value>Any of the <see cref="CoreFoundation.CFProxyType" /> enum values.</value>
		/// <remarks>See <see cref="WeakProxyType" /> for a weakly typed version of this property.</remarks>
		public CFProxyType ProxyType {
			get {
				var key = WeakProxyType;
				var rv = CFProxyTypeExtensions.GetNullableValue (key.GetHandle ()) ?? CFProxyType.None;
				GC.KeepAlive (key);
				return rv;
			}
		}

		/// <summary>The type of the proxy as an <see cref="NSString" />.</summary>
		/// <value>The type of the proxy as an <see cref="NSString" />.</value>
		/// <remarks>See <see cref="ProxyType" /> for a strongly typed version of this property.</remarks>
		public NSString? WeakProxyType {
			get {
				var key = TypeKey;
				if (key is null)
					return null;

				return (NSString?) settings [key];
			}
		}

		/// <summary>The user name needed for authentication with the proxy server.</summary>
		/// <value>A string containing the user name or null if not needed.</value>
		public string? Username {
			get {
				var key = UsernameKey;
				if (key is null)
					return null;

				return settings [key]?.ToString ();
			}
		}
	}

	/// <summary>Configuration settings used by <see cref="CoreFoundation.CFNetwork.GetProxiesForURL(Foundation.NSUrl,CoreFoundation.CFProxySettings)" />.</summary>
	/// <remarks>Returned by <see cref="CoreFoundation.CFNetwork.GetSystemProxySettings" />.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public class CFProxySettings {
		NSDictionary settings;

		internal CFProxySettings (NSDictionary settings)
		{
			this.settings = settings;
		}

		/// <summary>The dictionary holding the settings.</summary>
		public NSDictionary Dictionary {
			get { return settings; }
		}

		public bool HTTPEnable {
			get {
				var key = CFNetworkProxies.HttpEnable;
				if (key is null)
					return false;

				var v = (NSNumber?) settings [key];

				return v?.BoolValue ?? false;
			}
		}

		public int HTTPPort {
			get {
				var key = CFNetworkProxies.HttpPort;
				if (key is null)
					return 0;

				var v = (NSNumber?) settings [key];

				return v?.Int32Value ?? 0;
			}
		}

		public string? HTTPProxy {
			get {
				var key = CFNetworkProxies.HttpProxy;
				if (key is null)
					return null;

				return settings [key]?.ToString ();
			}
		}

		public bool ProxyAutoConfigEnable {
			get {
				var key = CFNetworkProxies.ProxyAutoConfigEnable;
				if (key is null)
					return false;

				var v = (NSNumber?) settings [key];

				return v?.BoolValue ?? false;
			}
		}

		public string? ProxyAutoConfigJavaScript {
			get {
				var key = CFNetworkProxies.ProxyAutoConfigJavaScript;
				if (key is null)
					return null;

				return settings [key]?.ToString ();
			}
		}

		public string? ProxyAutoConfigURLString {
			get {
				var key = CFNetworkProxies.ProxyAutoConfigUrlString;
				if (key is null)
					return null;

				return settings [key]?.ToString ();
			}
		}
	}

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public static partial class CFNetwork {
		[DllImport (Constants.CFNetworkLibrary)]
		unsafe extern static /* CFArrayRef __nullable */ IntPtr CFNetworkCopyProxiesForAutoConfigurationScript (
			/* CFStringRef __nonnull */ IntPtr proxyAutoConfigurationScript,
			/* CFURLRef __nonnull */ IntPtr targetURL, /* CFErrorRef  __nullable * __nullable */ IntPtr* error);

		static NSArray? CopyProxiesForAutoConfigurationScript (NSString proxyAutoConfigurationScript, NSUrl targetURL)
		{
			IntPtr err;
			IntPtr native;
			unsafe {
				native = CFNetworkCopyProxiesForAutoConfigurationScript (proxyAutoConfigurationScript.Handle, targetURL.Handle, &err);
				GC.KeepAlive (proxyAutoConfigurationScript);
				GC.KeepAlive (targetURL);
			}
			return native == IntPtr.Zero ? null : new NSArray (native);
		}

		/// <param name="proxyAutoConfigurationScript">JavaScript source to be executed to obtain a list of proxies to use.</param>
		///         <param name="targetURL">The target URL to connect to.</param>
		///         <summary>Executes the provided javascript source to determine a list of proxies to use for connecting to the target URL.</summary>
		///         <returns>Returns an array of <see cref="CoreFoundation.CFProxy" /> objects suitable to use for connecting to <paramref name="targetURL" />.</returns>
		///         <remarks>See also: <see cref="CoreFoundation.CFNetwork.GetProxiesForURL(Foundation.NSUrl,CoreFoundation.CFProxySettings)" /></remarks>
		public static CFProxy []? GetProxiesForAutoConfigurationScript (NSString proxyAutoConfigurationScript, NSUrl targetURL)
		{
			if (proxyAutoConfigurationScript is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (proxyAutoConfigurationScript));

			if (targetURL is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetURL));

			using (var array = CopyProxiesForAutoConfigurationScript (proxyAutoConfigurationScript, targetURL)) {
				if (array is null)
					return null;

				NSDictionary [] dictionaries = NSArray.ArrayFromHandle<NSDictionary> (array.Handle);
				GC.KeepAlive (array);
				if (dictionaries is null)
					return null;

				CFProxy [] proxies = new CFProxy [dictionaries.Length];
				for (int i = 0; i < dictionaries.Length; i++)
					proxies [i] = new CFProxy (dictionaries [i]);

				return proxies;
			}
		}

		/// <include file="../../docs/api/CoreFoundation/CFNetwork.xml" path="/Documentation/Docs[@DocId='M:CoreFoundation.CFNetwork.GetProxiesForAutoConfigurationScript(Foundation.NSString,System.Uri)']/*" />
		public static CFProxy []? GetProxiesForAutoConfigurationScript (NSString proxyAutoConfigurationScript, Uri targetUri)
		{
			// proxyAutoConfigurationScript checked later
			if (targetUri is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUri));

			using (var targetURL = NSUrl.FromString (targetUri.AbsoluteUri)) {
				if (targetURL is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetURL));
				return GetProxiesForAutoConfigurationScript (proxyAutoConfigurationScript, targetURL);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		// CFArrayRef CFNetworkCopyProxiesForURL (CFURLRef url, CFDictionaryRef proxySettings);
		extern static /* CFArrayRef __nonnull */ IntPtr CFNetworkCopyProxiesForURL (
			/* CFURLRef __nonnull */ IntPtr url,
			/* CFDictionaryRef __nonnull */ IntPtr proxySettings);

		static NSArray? CopyProxiesForURL (NSUrl url, NSDictionary proxySettings)
		{
			IntPtr native = CFNetworkCopyProxiesForURL (url.Handle, proxySettings.Handle);
			GC.KeepAlive (url);
			GC.KeepAlive (proxySettings);
			return native == IntPtr.Zero ? null : new NSArray (native);
		}

		/// <param name="url">The target URL to connect to.</param>
		///         <param name="proxySettings">The proxy settings as returned by <see cref="CoreFoundation.CFNetwork.GetSystemProxySettings" />.</param>
		///         <summary>Gets an array of <see cref="CoreFoundation.CFProxy" /> objects suitable to use for connecting to <paramref name="url" />.</summary>
		///         <returns>Returns an array of <see cref="CoreFoundation.CFProxy" /> objects suitable to use for connecting to <paramref name="url" />.</returns>
		///         <remarks>See also: <see cref="CoreFoundation.CFNetwork.GetProxiesForUri(System.Uri,CoreFoundation.CFProxySettings)" /></remarks>
		public static CFProxy []? GetProxiesForURL (NSUrl url, CFProxySettings? proxySettings)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			if (proxySettings is null)
				proxySettings = GetSystemProxySettings ();

			if (proxySettings is null)
				return null;

			using (NSArray? array = CopyProxiesForURL (url, proxySettings.Dictionary)) {
				if (array is null)
					return null;

				NSDictionary [] dictionaries = NSArray.ArrayFromHandle<NSDictionary> (array.Handle);
				GC.KeepAlive (array);
				if (dictionaries is null)
					return null;

				CFProxy [] proxies = new CFProxy [dictionaries.Length];
				for (int i = 0; i < dictionaries.Length; i++)
					proxies [i] = new CFProxy (dictionaries [i]);

				return proxies;
			}
		}

		/// <param name="uri">The target Uri to connect to.</param>
		///         <param name="proxySettings">The proxy settings as returned by <see cref="CoreFoundation.CFNetwork.GetSystemProxySettings" />.</param>
		///         <summary>Gets an array of <see cref="CoreFoundation.CFProxy" /> objects suitable to use for connecting to <paramref name="uri" />.</summary>
		///         <returns>Returns an array of <see cref="CoreFoundation.CFProxy" /> objects suitable to use for connecting to <paramref name="uri" />.</returns>
		///         <remarks>This method serves as a convenience wrapper for <see cref="CoreFoundation.CFNetwork.GetProxiesForURL(Foundation.NSUrl,CoreFoundation.CFProxySettings)" />.</remarks>
		public static CFProxy []? GetProxiesForUri (Uri uri, CFProxySettings? proxySettings)
		{
			if (uri is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (uri));

			using (NSUrl? url = NSUrl.FromString (uri.AbsoluteUri)) {
				if (url is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));
				return GetProxiesForURL (url, proxySettings);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFDictionaryRef __nullable */ IntPtr CFNetworkCopySystemProxySettings ();

		/// <summary>Gets the system's proxy configuration settings.</summary>
		///         <returns>A <see cref="CoreFoundation.CFProxySettings" /> with the system's proxy settings.</returns>
		///         <remarks>These settings are used by <see cref="CoreFoundation.CFNetwork.GetProxiesForURL(Foundation.NSUrl,CoreFoundation.CFProxySettings)" /> and 
		/// 	<see cref="GetProxiesForUri" />.</remarks>
		public static CFProxySettings? GetSystemProxySettings ()
		{
			IntPtr native = CFNetworkCopySystemProxySettings ();

			if (native == IntPtr.Zero)
				return null;

			var dict = new NSDictionary (native);
			// Must release since the IntPtr constructor calls Retain and
			// CFNetworkCopySystemProxySettings return value is already retained
			dict.DangerousRelease ();
			return new CFProxySettings (dict);
		}

		unsafe delegate IntPtr CreatePACCFRunLoopSource (delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> cb, ref CFStreamClientContext context);

		static CFProxy []? ParseProxies (IntPtr proxyList)
		{
			CFProxy []? proxies = null;
			if (proxyList != IntPtr.Zero) {
				// it was retained in the cbs.
				using (var array = new CFArray (proxyList, false)) {
					proxies = new CFProxy [array.Count];
					for (int i = 0; i < proxies.Length; i++) {
						var dict = Runtime.GetNSObject<NSDictionary> (array.GetValue (i));
						proxies [i] = new CFProxy (dict!);
					}
				}
			}
			return proxies;
		}

		// helper struct to contain all the data that was used in the callback
		struct PACProxyCallbackData {
			public IntPtr ProxyListPtr; // Pointer to a CFArray to later be parsed
			public IntPtr ErrorPtr; // Pointer to the Error
			public IntPtr CFRunLoopPtr; // Pointer to the runloop, needed to be stopped

			public CFProxy []? ProxyList {
				get {
					if (ProxyListPtr != IntPtr.Zero)
						return ParseProxies (ProxyListPtr);
					return null;
				}
			}

			public NSError? Error {
				get {
					if (ErrorPtr != IntPtr.Zero)
						return Runtime.GetNSObject<NSError> (ErrorPtr);
					return null;
				}
			}
		}

		// callback that will sent the client info
		[UnmanagedCallersOnly]
		static void ExecutePacCallback (IntPtr client, IntPtr proxyList, IntPtr error)
		{
			// grab the required structure and set the data, according apple docs:
			// client
			// The client reference originally passed in the clientContext parameter of the 
			// CFNetworkExecuteProxyAutoConfigurationScript or CFNetworkExecuteProxyAutoConfigurationURL call
			// that triggered this callback.
			// Well, that is NOT TRUE, the client passed is the client.Info pointer not the client.
			var pacCbData = Marshal.PtrToStructure<PACProxyCallbackData> (client)!;
			// make sure is not released, will be released by the parsing method.
			if (proxyList != IntPtr.Zero) {
				CFObject.CFRetain (proxyList);
				pacCbData.ProxyListPtr = proxyList;
			}
			if (error != IntPtr.Zero) {
				NSObject.DangerousRetain (error);
				pacCbData.ErrorPtr = error;
			}
			// stop the CFRunLoop
			var runLoop = new CFRunLoop (pacCbData.CFRunLoopPtr, false);
			Marshal.StructureToPtr<PACProxyCallbackData> (pacCbData, client, false);
			runLoop.Stop ();
		}

		static async Task<(CFProxy []? proxies, NSError? error)> ExecutePacCFRunLoopSourceAsync (CreatePACCFRunLoopSource factory, CancellationToken cancellationToken)
		{
			CFProxy []? proxies = null;
			NSError? outError = null;
			if (cancellationToken.IsCancellationRequested)
				throw new OperationCanceledException ("Operation was cancelled.");

			await Task.Run (() => {
				// we need the runloop of THIS thread, so it is important to get it in the correct context
				var runLoop = CFRunLoop.Current;

				// build a struct that will have all the needed info for the callback
				var pacCbData = new PACProxyCallbackData ();
				pacCbData.CFRunLoopPtr = runLoop.Handle;
				var pacDataPtr = Marshal.AllocHGlobal (Marshal.SizeOf<PACProxyCallbackData> ());
				try {
					Marshal.StructureToPtr<PACProxyCallbackData> (pacCbData, pacDataPtr, false);

					var clientContext = new CFStreamClientContext ();
					clientContext.Info = pacDataPtr;

					unsafe {
						using (var loopSource = new CFRunLoopSource (factory (&ExecutePacCallback, ref clientContext), true))
						using (var mode = new NSString ("Xamarin.iOS.Proxy")) {

							if (cancellationToken.IsCancellationRequested)
								throw new OperationCanceledException ("Operation was cancelled.");

							cancellationToken.Register (() => {
								//if user cancels, we invalidte the source, stop the runloop and remove the source
								loopSource.Invalidate ();
								runLoop.RemoveSource (loopSource, mode);
								runLoop.Stop ();
							});
							runLoop.AddSource (loopSource, mode);
							// blocks until stop is called, will be done in the cb set previously
							runLoop.RunInMode (mode, double.MaxValue, false);
							// does not raise an error if source is not longer present, so no need to worry
							runLoop.RemoveSource (loopSource, mode);
						}
					} // matches the unsafe block

					if (cancellationToken.IsCancellationRequested)
						throw new OperationCanceledException ("Operation was cancelled.");

					pacCbData = Marshal.PtrToStructure<PACProxyCallbackData> (pacDataPtr)!;
					// get data from the struct
					proxies = pacCbData.ProxyList;
					outError = pacCbData.Error;
				} finally {
					// clean resources
					if (pacCbData.ProxyListPtr != IntPtr.Zero)
						CFObject.CFRelease (pacCbData.ProxyListPtr);
					if (pacCbData.ErrorPtr != IntPtr.Zero)
						NSObject.DangerousRelease (pacCbData.ErrorPtr);
					Marshal.FreeHGlobal (pacDataPtr);
				}
			}, cancellationToken).ConfigureAwait (false);

			if (cancellationToken.IsCancellationRequested)
				throw new OperationCanceledException ("Operation was cancelled.");
			return (proxies: proxies, error: outError);
		}

		static CFProxy []? ExecutePacCFRunLoopSourceBlocking (CreatePACCFRunLoopSource factory, out NSError? outError)
		{
			var runLoop = CFRunLoop.Current;
			outError = null;

			// build a struct that will have all the needed info for the callback
			var pacCbData = new PACProxyCallbackData ();
			pacCbData.CFRunLoopPtr = runLoop.Handle;
			var pacDataPtr = Marshal.AllocHGlobal (Marshal.SizeOf<PACProxyCallbackData> ());
			try {
				Marshal.StructureToPtr<PACProxyCallbackData> (pacCbData, pacDataPtr, false);

				var clientContext = new CFStreamClientContext ();
				clientContext.Info = pacDataPtr;

				unsafe {
					using (var loopSource = new CFRunLoopSource (factory (&ExecutePacCallback, ref clientContext), true))
					using (var mode = new NSString ("Xamarin.iOS.Proxy")) {
						runLoop.AddSource (loopSource, mode);
						runLoop.RunInMode (mode, double.MaxValue, false);
						runLoop.RemoveSource (loopSource, mode);
					}
					pacCbData = Marshal.PtrToStructure<PACProxyCallbackData> (pacDataPtr)!;
					// get data from the struct
					outError = pacCbData.Error;
				} // unsafe
				return pacCbData.ProxyList;
			} finally {
				if (pacCbData.ProxyListPtr != IntPtr.Zero)
					CFObject.CFRelease (pacCbData.ProxyListPtr);
				if (pacCbData.ErrorPtr != IntPtr.Zero)
					NSObject.DangerousRelease (pacCbData.ErrorPtr);
				Marshal.FreeHGlobal (pacDataPtr);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern unsafe static /* CFRunLoopSourceRef __nonnull */ IntPtr CFNetworkExecuteProxyAutoConfigurationScript (
			/* CFStringRef __nonnull */ IntPtr proxyAutoConfigurationScript,
			/* CFURLRef __nonnull */ IntPtr targetURL,
			/* CFProxyAutoConfigurationResultCallback __nonnull */ delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> cb,
			/* CFStreamClientContext * __nonnull */ CFStreamClientContext* clientContext);

		public static CFProxy []? ExecuteProxyAutoConfigurationScript (string proxyAutoConfigurationScript, Uri targetUrl, out NSError? outError)
		{
			outError = null;
			if (proxyAutoConfigurationScript is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (proxyAutoConfigurationScript));

			if (targetUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUrl));

			using (var pacScript = new NSString (proxyAutoConfigurationScript))
			using (var url = new NSUrl (targetUrl.AbsoluteUri)) {
				CreatePACCFRunLoopSource factory;
				unsafe {
					factory = delegate (delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> cb, ref CFStreamClientContext context)
					{
						fixed (CFStreamClientContext* contextPtr = &context) {
							var result = CFNetworkExecuteProxyAutoConfigurationScript (pacScript.Handle, url.Handle, cb, contextPtr);
							GC.KeepAlive (pacScript);
							GC.KeepAlive (url);
							return result;
						}
					};
				}
				return ExecutePacCFRunLoopSourceBlocking (factory, out outError);
			}
		}

		public static async Task<(CFProxy []? proxies, NSError? error)> ExecuteProxyAutoConfigurationScriptAsync (string proxyAutoConfigurationScript, Uri targetUrl, CancellationToken cancellationToken)
		{
			if (proxyAutoConfigurationScript is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (proxyAutoConfigurationScript));

			if (targetUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUrl));

			using (var pacScript = new NSString (proxyAutoConfigurationScript))
			using (var url = new NSUrl (targetUrl.AbsoluteUri)) {
				CreatePACCFRunLoopSource factory;
				unsafe {
					factory = delegate (delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> cb, ref CFStreamClientContext context)
					{
						fixed (CFStreamClientContext* contextPtr = &context) {
							var result = CFNetworkExecuteProxyAutoConfigurationScript (pacScript.Handle, url.Handle, cb, contextPtr);
							GC.KeepAlive (pacScript);
							GC.KeepAlive (url);
							return result;
						}
					};
				}
				// use the helper task with a factory for this method
				return await ExecutePacCFRunLoopSourceAsync (factory, cancellationToken).ConfigureAwait (false);
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern unsafe static /* CFRunLoopSourceRef __nonnull */ IntPtr CFNetworkExecuteProxyAutoConfigurationURL (
			/* CFURLRef __nonnull */ IntPtr proxyAutoConfigurationURL,
			/* CFURLRef __nonnull */ IntPtr targetURL,
			/* CFProxyAutoConfigurationResultCallback __nonnull */ delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> cb,
			/* CFStreamClientContext * __nonnull */ CFStreamClientContext* clientContext);

		public static CFProxy []? ExecuteProxyAutoConfigurationUrl (Uri proxyAutoConfigurationUrl, Uri targetUrl, out NSError? outError)
		{
			outError = null;
			if (proxyAutoConfigurationUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (proxyAutoConfigurationUrl));

			if (targetUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUrl));

			using (var pacUrl = new NSUrl (proxyAutoConfigurationUrl.AbsoluteUri)) // toll free bridge to CFUrl
			using (var url = new NSUrl (targetUrl.AbsoluteUri)) {
				CreatePACCFRunLoopSource factory;
				unsafe {
					factory = delegate (delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> cb, ref CFStreamClientContext context)
					{
						fixed (CFStreamClientContext* contextPtr = &context) {
							var result = CFNetworkExecuteProxyAutoConfigurationURL (pacUrl.Handle, url.Handle, cb, contextPtr);
							GC.KeepAlive (pacUrl);
							GC.KeepAlive (url);
							return result;
						}
					};
				}
				return ExecutePacCFRunLoopSourceBlocking (factory, out outError);
			}
		}

		public static async Task<(CFProxy []? proxies, NSError? error)> ExecuteProxyAutoConfigurationUrlAsync (Uri proxyAutoConfigurationUrl, Uri targetUrl, CancellationToken cancellationToken)
		{
			// similar to the sync method, but we will spawn a thread and wait in an async manner to an autoreset event to be fired
			if (proxyAutoConfigurationUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (proxyAutoConfigurationUrl));

			if (targetUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUrl));

			using (var pacUrl = new NSUrl (proxyAutoConfigurationUrl.AbsoluteUri)) // toll free bridge to CFUrl
			using (var url = new NSUrl (targetUrl.AbsoluteUri)) {
				CreatePACCFRunLoopSource factory;
				unsafe {
					factory = delegate (delegate* unmanaged<IntPtr, IntPtr, IntPtr, void> cb, ref CFStreamClientContext context)
					{
						fixed (CFStreamClientContext* contextPtr = &context) {
							var result = CFNetworkExecuteProxyAutoConfigurationURL (pacUrl.Handle, url.Handle, cb, contextPtr);
							GC.KeepAlive (pacUrl);
							GC.KeepAlive (url);
							return result;
						}
					};
				}
				// use the helper task with a factory for this method
				return await ExecutePacCFRunLoopSourceAsync (factory, cancellationToken).ConfigureAwait (false);
			}
		}

		class CFWebProxy : IWebProxy {
			ICredentials? credentials;
			bool userSpecified;

			public CFWebProxy ()
			{
			}

			public ICredentials? Credentials {
				get { return credentials; }
				set {
					userSpecified = true;
					credentials = value;
				}
			}

			static Uri? GetProxyUri (CFProxy proxy, out NetworkCredential? credentials)
			{
				string protocol;

				switch (proxy.ProxyType) {
				case CFProxyType.FTP:
					protocol = "ftp://";
					break;
				case CFProxyType.HTTP:
				case CFProxyType.HTTPS:
					protocol = "http://";
					break;
				default:
					credentials = null;
					return null;
				}

				//[SuppressMessage ("Microsoft.Security", "CS002:SecretInNextLine", Justification="No credentials are stored, they are retrived from the OS Proxy settings.")]
				var username = proxy?.Username;
				var password = proxy?.Password;
				var hostname = proxy?.HostName;
				var port = proxy?.Port;
				string uri;

				if (username is not null)
					credentials = new NetworkCredential (username, password);
				else
					credentials = null;

				uri = protocol + hostname + (port != 0 ? ':' + port.ToString () : string.Empty);

				return new Uri (uri, UriKind.Absolute);
			}

			static Uri? GetProxyUriFromScript (NSString script, Uri targetUri, out NetworkCredential? credentials)
			{
				CFProxy []? proxies = CFNetwork.GetProxiesForAutoConfigurationScript (script, targetUri);

				if (proxies is null) {
					credentials = null;
					return targetUri;
				}

				for (int i = 0; i < proxies.Length; i++) {
					switch (proxies [i].ProxyType) {
					case CFProxyType.HTTPS:
					case CFProxyType.HTTP:
					case CFProxyType.FTP:
						// create a Uri based on the hostname/port/etc info
						return GetProxyUri (proxies [i], out credentials);
					case CFProxyType.SOCKS:
					default:
						// unsupported proxy type, try the next one
						break;
					case CFProxyType.None:
						// no proxy should be used
						credentials = null;
						return targetUri;
					}
				}

				credentials = null;

				return null;
			}

			public Uri GetProxy (Uri targetUri)
			{
				NetworkCredential? credentials = null;
				Uri? proxy = null;

				if (targetUri is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUri));

				try {
					CFProxySettings? settings = CFNetwork.GetSystemProxySettings ();
					CFProxy []? proxies = CFNetwork.GetProxiesForUri (targetUri, settings);

					if (proxies is not null) {
						for (int i = 0; i < proxies.Length && proxy is null; i++) {
							switch (proxies [i].ProxyType) {
							case CFProxyType.AutoConfigurationJavaScript:
								proxy = GetProxyUriFromScript (proxies [i].AutoConfigurationJavaScript!, targetUri, out credentials);
								break;
							case CFProxyType.AutoConfigurationUrl:
								// unsupported proxy type (requires fetching script from remote url)
								break;
							case CFProxyType.HTTPS:
							case CFProxyType.HTTP:
							case CFProxyType.FTP:
								// create a Uri based on the hostname/port/etc info
								proxy = GetProxyUri (proxies [i], out credentials);
								break;
							case CFProxyType.SOCKS:
								// unsupported proxy type, try the next one
								break;
							case CFProxyType.None:
								// no proxy should be used
								proxy = targetUri;
								break;
							}
						}

						if (proxy is null) {
							// no supported proxies for this Uri, fall back to trying to connect to targetUri directly
							proxy = targetUri;
						}
					} else {
						proxy = targetUri;
					}
				} catch {
					// ignore errors while retrieving proxy data
					proxy = targetUri;
				}

				if (!userSpecified)
					this.credentials = credentials;

				return proxy;
			}

			public bool IsBypassed (Uri targetUri)
			{
				if (targetUri is null)
					ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUri));

				return GetProxy (targetUri) == targetUri;
			}
		}

		/// <include file="../../docs/api/CoreFoundation/CFNetwork.xml" path="/Documentation/Docs[@DocId='M:CoreFoundation.CFNetwork.GetDefaultProxy']/*" />
		public static IWebProxy GetDefaultProxy ()
		{
			return new CFWebProxy ();
		}
	}
}
