#if !__MACCATALYST__
using System;
using Foundation;

using System.Runtime.InteropServices;

using ObjCRuntime;

#nullable enable

namespace AppKit {

	public partial class NSWorkspace {

		/// <param name="urls">To be added.</param>
		///         <param name="bundleIdentifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="descriptor">To be added.</param>
		///         <param name="identifiers">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos11.0", "Use 'NSWorkspace.OpenUrls' with completion handler.")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public virtual bool OpenUrls (NSUrl [] urls, string bundleIdentifier, NSWorkspaceLaunchOptions options, NSAppleEventDescriptor descriptor, string [] identifiers)
		{
			// Ignore the passed in argument, because if you pass it in we will crash on cleanup.
			return _OpenUrls (urls, bundleIdentifier, options, descriptor, null);
		}

		/// <param name="urls">To be added.</param>
		///         <param name="bundleIdentifier">To be added.</param>
		///         <param name="options">To be added.</param>
		///         <param name="descriptor">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[SupportedOSPlatform ("macos")]
		[ObsoletedOSPlatform ("macos11.0", "Use 'NSWorkspace.OpenUrls' with completion handler.")]
		[UnsupportedOSPlatform ("maccatalyst")]
		public virtual bool OpenUrls (NSUrl [] urls, string bundleIdentifier, NSWorkspaceLaunchOptions options, NSAppleEventDescriptor descriptor)
		{
			return _OpenUrls (urls, bundleIdentifier, options, descriptor, null);
		}

		/// <param name="fileType">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[ObsoletedOSPlatform ("macos", "Use 'NSWorkspace.GetIcon' instead.")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public virtual NSImage IconForFileType (string fileType)
		{
			var nsFileType = NSString.CreateNative (fileType);
			try {
				return IconForFileType (nsFileType);
			} finally {
				NSString.ReleaseNative (nsFileType);
			}
		}

		[ObsoletedOSPlatform ("macos", "Use 'NSWorkspace.GetIcon' instead.")]
		[UnsupportedOSPlatform ("maccatalyst")]
		[SupportedOSPlatform ("macos")]
		public virtual NSImage IconForFileType (HfsTypeCode typeCode)
		{
			var nsFileType = GetNSFileType ((uint) typeCode);
			return IconForFileType (nsFileType);
		}

		[DllImport (Constants.FoundationLibrary)]
		extern static IntPtr NSFileTypeForHFSTypeCode (uint /* OSType = int32_t */ hfsFileTypeCode);

		private static IntPtr GetNSFileType (uint fourCcTypeCode)
		{
			return NSFileTypeForHFSTypeCode (fourCcTypeCode);
		}

#if !NET
		[Obsolete ("Use the overload that takes 'out NSError' instead.")]
		public virtual NSRunningApplication LaunchApplication (NSUrl url, NSWorkspaceLaunchOptions options, NSDictionary configuration, NSError error)
		{
			return LaunchApplication (url, options, configuration, out error);
		}
#endif
	}
}
#endif // !__MACCATALYST__
