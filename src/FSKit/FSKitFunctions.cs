// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using Foundation;
using ObjCRuntime;

#nullable enable

// Let's hope that by .NET 11 we've ironed out all the bugs in the API.
// This can of course be adjusted as needed (until we've released as stable).
#if NET11_0_OR_GREATER
#define STABLE_FSKIT
#endif

namespace FSKit {
#if !STABLE_FSKIT
	[Experimental ("APL0002")]
#endif
	[SupportedOSPlatform ("macos15.4")]
	[UnsupportedOSPlatform ("maccatalyst")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class FSKitFunctions {
		[DllImport (Constants.FSKitLibrary)]
		static extern IntPtr fs_errorForPOSIXError (int errorCode);

		/// <summary>Get an <see cref="NSError" /> for a given POSIX error code.</summary>
		/// <param name="errorCode">The error code to look up.</param>
		/// <returns>A new <see cref="NSError" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static NSError? GetErrorForPosixError (int errorCode)
		{
			return Runtime.GetNSObject<NSError> (fs_errorForPOSIXError (errorCode));
		}

		[DllImport (Constants.FSKitLibrary)]
		static extern IntPtr fs_errorForMachError (int errorCode);

		/// <summary>Get an <see cref="NSError" /> for a given Mach error code.</summary>
		/// <param name="errorCode">The error code to look up.</param>
		/// <returns>A new <see cref="NSError" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static NSError? GetErrorForMachError (int errorCode)
		{
			return Runtime.GetNSObject<NSError> (fs_errorForMachError (errorCode));
		}

		[DllImport (Constants.FSKitLibrary)]
		static extern IntPtr fs_errorForCocoaError (int errorCode);

		/// <summary>Get an <see cref="NSError" /> for a given Cocoa error code.</summary>
		/// <param name="errorCode">The error code to look up.</param>
		/// <returns>A new <see cref="NSError" /> instance if successful, <see langword="null" /> otherwise.</returns>
		public static NSError? GetErrorForCocoaError (int errorCode)
		{
			return Runtime.GetNSObject<NSError> (fs_errorForCocoaError (errorCode));
		}
	}
}
