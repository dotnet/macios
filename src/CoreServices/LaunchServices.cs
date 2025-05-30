//
// LaunchServices.cs
//
// Author:
//   Aaron Bockover <abock@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//
// NOTE: intentionally passing IntPtr.Zero to all
// 'out NSError' APIs since errors return NULL anyway,
// and the NSError objects are specified to be a
// constant error object (from the docs).
// 
// In other words, A NULL return value implies
// ApplicationNotFoundso we just drop the
// 'out NSError' parameter to make the API nicer.
//
// NOTE: only bound APIs not deprecated in 10.11
//
// NOTE: KEEP IN SYNC WITH TESTS!

#nullable enable

#if MONOMAC

using System;
using System.Runtime.InteropServices;

using CoreFoundation;
using Foundation;
using ObjCRuntime;

namespace CoreServices {
	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum LSRoles/*Mask*/ : uint /* always 32-bit uint */
	{
		/// <summary>To be added.</summary>
		None = 1,
		/// <summary>To be added.</summary>
		Viewer = 2,
		/// <summary>To be added.</summary>
		Editor = 4,
		/// <summary>To be added.</summary>
		Shell = 8,
		/// <summary>To be added.</summary>
		All = 0xffffffff,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[Flags]
	public enum LSAcceptanceFlags : uint /* always 32-bit uint */
	{
		/// <summary>To be added.</summary>
		Default = 1,
		/// <summary>To be added.</summary>
		AllowLoginUI = 2,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	public enum LSResult {
		/// <summary>To be added.</summary>
		Success = 0,
		[SupportedOSPlatform ("macos13.0")]
		MalformedLocErr = -10400,
		/// <summary>To be added.</summary>
		AppInTrash = -10660,
		/// <summary>To be added.</summary>
		ExecutableIncorrectFormat = -10661,
		/// <summary>To be added.</summary>
		AttributeNotFound = -10662,
		/// <summary>To be added.</summary>
		AttributeNotSettable = -10663,
		/// <summary>To be added.</summary>
		IncompatibleApplicationVersion = -10664,
		/// <summary>To be added.</summary>
		NoRosettaEnvironment = -10665,
		/// <summary>To be added.</summary>
		Unknown = -10810,
		/// <summary>To be added.</summary>
		NotAnApplication = -10811,
		/// <summary>To be added.</summary>
		NotInitialized = -10812,
		/// <summary>To be added.</summary>
		DataUnavailable = -10813,
		/// <summary>To be added.</summary>
		ApplicationNotFound = -10814,
		/// <summary>To be added.</summary>
		UnknownType = -10815,
		/// <summary>To be added.</summary>
		DataTooOld = -10816,
		/// <summary>To be added.</summary>
		Data = -10817,
		/// <summary>To be added.</summary>
		LaunchInProgress = -10818,
		/// <summary>To be added.</summary>
		NotRegistered = -10819,
		/// <summary>To be added.</summary>
		AppDoesNotClaimType = -10820,
		/// <summary>To be added.</summary>
		AppDoesNotSupportSchemeWarning = -10821,
		/// <summary>To be added.</summary>
		ServerCommunication = -10822,
		/// <summary>To be added.</summary>
		CannotSetInfo = -10823,
		/// <summary>To be added.</summary>
		NoRegistrationInfo = -10824,
		/// <summary>To be added.</summary>
		IncompatibleSystemVersion = -10825,
		/// <summary>To be added.</summary>
		NoLaunchPermission = -10826,
		/// <summary>To be added.</summary>
		NoExecutable = -10827,
		/// <summary>To be added.</summary>
		NoClassicEnvironment = -10828,
		/// <summary>To be added.</summary>
		MultipleSessionsNotSupported = -10829,
	}

	/// <summary>To be added.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("macos")]
	public static class LaunchServices {
		#region Locating an Application

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultApplicationURLForURL (IntPtr inUrl, LSRoles inRole, /*out*/ IntPtr outError);

		/// <param name="url">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public static NSUrl? GetDefaultApplicationUrlForUrl (NSUrl url, LSRoles roles = LSRoles.All)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			var result = Runtime.GetNSObject<NSUrl> (
				LSCopyDefaultApplicationURLForURL (url.Handle, roles, IntPtr.Zero)
			);
			GC.KeepAlive (url);
			return result;
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultApplicationURLForContentType (IntPtr inContentType, LSRoles inRole, /*out*/ IntPtr outError);

		/// <param name="contentType">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public static NSUrl? GetDefaultApplicationUrlForContentType (string contentType, LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));

			var contentTypeHandle = CFString.CreateNative (contentType);
			try {
				return Runtime.GetNSObject<NSUrl> (
					LSCopyDefaultApplicationURLForContentType (contentTypeHandle, roles, IntPtr.Zero)
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
			}
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyApplicationURLsForURL (IntPtr inUrl, LSRoles inRole);

		/// <param name="url">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public static NSUrl [] GetApplicationUrlsForUrl (NSUrl url, LSRoles roles = LSRoles.All)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			var result = NSArray.ArrayFromHandle<NSUrl> (
				LSCopyApplicationURLsForURL (url.Handle, roles)
			);
			GC.KeepAlive (url);
			return result;
		}

		[DllImport (Constants.CoreServicesLibrary)]
		unsafe static extern LSResult LSCanURLAcceptURL (IntPtr inItemUrl, IntPtr inTargetUrl,
			LSRoles inRole, LSAcceptanceFlags inFlags, byte* outAcceptsItem);

		// NOTE: intentionally inverting the status results (return bool, with an out
		// LSResult vs return LSResult with an out bool) to make the API nicer to use
		/// <param name="itemUrl">To be added.</param>
		///         <param name="targetUrl">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <param name="acceptanceFlags">To be added.</param>
		///         <param name="result">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool CanUrlAcceptUrl (NSUrl itemUrl, NSUrl targetUrl,
			LSRoles roles, LSAcceptanceFlags acceptanceFlags, out LSResult result)
		{
			if (itemUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (itemUrl));
			if (targetUrl is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (targetUrl));

			byte acceptsItem;
			unsafe {
				result = LSCanURLAcceptURL (itemUrl.Handle, targetUrl.Handle, roles, acceptanceFlags, &acceptsItem);
				GC.KeepAlive (itemUrl);
				GC.KeepAlive (targetUrl);
			}
			return acceptsItem != 0;
		}

		/// <param name="itemUrl">To be added.</param>
		///         <param name="targetUrl">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <param name="acceptanceFlags">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static bool CanUrlAcceptUrl (NSUrl itemUrl, NSUrl targetUrl,
			LSRoles roles = LSRoles.All, LSAcceptanceFlags acceptanceFlags = LSAcceptanceFlags.Default)
		{
			LSResult result;
			return CanUrlAcceptUrl (itemUrl, targetUrl, roles, acceptanceFlags, out result);
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyApplicationURLsForBundleIdentifier (IntPtr inBundleIdentifier, /*out*/ IntPtr outError);

		/// <param name="bundleIdentifier">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public static NSUrl [] GetApplicationUrlsForBundleIdentifier (string bundleIdentifier)
		{
			if (bundleIdentifier is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (bundleIdentifier));

			var bundleIdentifierHandle = CFString.CreateNative (bundleIdentifier);
			try {
				return NSArray.ArrayFromHandle<NSUrl> (
					LSCopyApplicationURLsForBundleIdentifier (bundleIdentifierHandle, IntPtr.Zero)
				);
			}
			finally {
				CFString.ReleaseNative (bundleIdentifierHandle);
			}
		}

		#endregion

		#region Opening Items

		[DllImport (Constants.CoreServicesLibrary)]
		unsafe static extern LSResult LSOpenCFURLRef (IntPtr inUrl, void** outLaunchedUrl);

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe static LSResult Open (NSUrl url)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			LSResult result = LSOpenCFURLRef (url.Handle, (void**) 0);
			GC.KeepAlive (url);
			return result;
		}

		/// <param name="url">To be added.</param>
		///         <param name="launchedUrl">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public unsafe static LSResult Open (NSUrl url, out NSUrl? launchedUrl)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			void* launchedUrlHandle;
			var result = LSOpenCFURLRef (url.Handle, &launchedUrlHandle);
			GC.KeepAlive (url);
			launchedUrl = Runtime.GetNSObject<NSUrl> (new IntPtr (launchedUrlHandle));
			return result;
		}

		#endregion

		#region Registering an Application

		[DllImport (Constants.CoreServicesLibrary)]
		static extern LSResult LSRegisterURL (IntPtr inUrl, byte inUpdate);

		/// <param name="url">To be added.</param>
		///         <param name="update">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static LSResult Register (NSUrl url, bool update)
		{
			if (url is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (url));

			LSResult result = LSRegisterURL (url.Handle, (byte) (update ? 1 : 0));
			GC.KeepAlive (url);
			return result;
		}

		#endregion

		#region Working with Role Handlers

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyAllRoleHandlersForContentType (IntPtr inContentType, LSRoles inRole);

		/// <param name="contentType">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public static string? []? GetAllRoleHandlersForContentType (string contentType, LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));

			var contentTypeHandle = CFString.CreateNative (contentType);
			try {
				return CFArray.StringArrayFromHandle (
					LSCopyAllRoleHandlersForContentType (contentTypeHandle, roles)
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
			}
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultRoleHandlerForContentType (IntPtr inContentType, LSRoles inRole);

		/// <param name="contentType">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public static string GetDefaultRoleHandlerForContentType (string contentType, LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));

			var contentTypeHandle = CFString.CreateNative (contentType);
			try {
				return (string) Runtime.GetNSObject<NSString> (
					LSCopyDefaultRoleHandlerForContentType (contentTypeHandle, roles)
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
			}
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern LSResult LSSetDefaultRoleHandlerForContentType (IntPtr inContentType,
			LSRoles inRole, IntPtr inHandlerBundleID);

		/// <param name="contentType">To be added.</param>
		///         <param name="handlerBundleId">To be added.</param>
		///         <param name="roles">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		// NOTE: intentionally swapped handlerBundleId and roles parameters for a nicer API
		public static LSResult SetDefaultRoleHandlerForContentType (string contentType, string handlerBundleId,
			LSRoles roles = LSRoles.All)
		{
			if (contentType is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (contentType));
			if (handlerBundleId is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handlerBundleId));

			var contentTypeHandle = CFString.CreateNative (contentType);
			var handlerBundleIdHandle = CFString.CreateNative (handlerBundleId);
			try {
				return LSSetDefaultRoleHandlerForContentType (
					contentTypeHandle,
					roles,
					handlerBundleIdHandle
				);
			}
			finally {
				CFString.ReleaseNative (contentTypeHandle);
				CFString.ReleaseNative (handlerBundleIdHandle);
			}
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyAllHandlersForURLScheme (IntPtr inUrlScheme);

		/// <param name="urlScheme">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GetApplicationUrlsForUrl' instead.")]
		public static string? []? GetAllHandlersForUrlScheme (string urlScheme)
		{
			if (urlScheme is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (urlScheme));

			var urlSchemeHandle = CFString.CreateNative (urlScheme);
			try {
				return CFArray.StringArrayFromHandle (
					LSCopyAllHandlersForURLScheme (urlSchemeHandle)
				);
			}
			finally {
				CFString.ReleaseNative (urlSchemeHandle);
			}
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern IntPtr LSCopyDefaultHandlerForURLScheme (IntPtr inUrlScheme);

		/// <param name="urlScheme">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos10.15", "Use 'GetDefaultApplicationUrlForUrl' instead.")]
		public static string GetDefaultHandlerForUrlScheme (string urlScheme)
		{
			if (urlScheme is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (urlScheme));

			var urlSchemeHandle = CFString.CreateNative (urlScheme);
			try {
				return (string) Runtime.GetNSObject<NSString> (
					LSCopyDefaultHandlerForURLScheme (urlSchemeHandle)
				);
			}
			finally {
				CFString.ReleaseNative (urlSchemeHandle);
			}
		}

		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		[DllImport (Constants.CoreServicesLibrary)]
		static extern LSResult LSSetDefaultHandlerForURLScheme (IntPtr inUrlScheme, IntPtr inHandlerBundleId);

		/// <param name="urlScheme">To be added.</param>
		///         <param name="handlerBundleId">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos14.0")]
		public static LSResult SetDefaultHandlerForUrlScheme (string urlScheme, string handlerBundleId)
		{
			if (urlScheme is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (urlScheme));
			if (handlerBundleId is null)
				ObjCRuntime.ThrowHelper.ThrowArgumentNullException (nameof (handlerBundleId));

			var urlSchemeHandle = CFString.CreateNative (urlScheme);
			var handlerBundleIdHandle = CFString.CreateNative (handlerBundleId);
			try {
				return LSSetDefaultHandlerForURLScheme (
					urlSchemeHandle,
					handlerBundleIdHandle
				);
			}
			finally {
				CFString.ReleaseNative (urlSchemeHandle);
				CFString.ReleaseNative (handlerBundleIdHandle);
			}
		}

		#endregion
	}
}

#endif // MONOMAC
