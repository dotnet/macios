// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using CoreFoundation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace GameController {
	public partial class GCInput {
		[SupportedOSPlatform ("ios17.4")]
		[SupportedOSPlatform ("macos14.4")]
		[SupportedOSPlatform ("tvos17.4")]
		[SupportedOSPlatform ("maccatalyst17.4")]
		[DllImport (Constants.GameControllerLibrary)]
		static extern IntPtr /* GCButtonElementName */ GCInputBackLeftButton (nint position);

		// A strongly-typed API (the GCInputButtonName enum) is not possible, because it's not an exhaustive enum,
		// this method may return strings that aren't in the enum.
		/// <summary>Get the name of the back left button on the controller for the specified position.</summary>
		/// <param name="position">Zero-based position of the button.</param>
		/// <returns>The name of the back left button on the controller for the specified position.</returns>
		[SupportedOSPlatform ("ios17.4")]
		[SupportedOSPlatform ("macos14.4")]
		[SupportedOSPlatform ("tvos17.4")]
		[SupportedOSPlatform ("maccatalyst17.4")]
		public static NSString? GetBackLeftButtonName (nint position)
		{
			return Runtime.GetNSObject<NSString> (GCInputBackLeftButton (position));
		}

		[SupportedOSPlatform ("ios17.4")]
		[SupportedOSPlatform ("macos14.4")]
		[SupportedOSPlatform ("tvos17.4")]
		[SupportedOSPlatform ("maccatalyst17.4")]
		[DllImport (Constants.GameControllerLibrary)]
		static extern IntPtr /* GCButtonElementName */ GCInputBackRightButton (nint position);

		// A strongly-typed API (the GCInputButtonName enum) is not possible, because it's not an exhaustive enum,
		// this method may return strings that aren't in the enum.
		/// <summary>Get the name of the back right button on the controller for the specified position.</summary>
		/// <param name="position">Zero-based position of the button.</param>
		/// <returns>The name of the back rught button on the controller for the specified position.</returns>
		[SupportedOSPlatform ("ios17.4")]
		[SupportedOSPlatform ("macos14.4")]
		[SupportedOSPlatform ("tvos17.4")]
		[SupportedOSPlatform ("maccatalyst17.4")]
		public static NSString? GetBackRightButtonName (nint position)
		{
			return Runtime.GetNSObject<NSString> (GCInputBackRightButton (position));
		}

		// headers claim macOS 13.0 / iOS 16.0, but introspection says macOS 14.0 / iOS 17.0, so use that.
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		[DllImport (Constants.GameControllerLibrary)]
		static extern IntPtr /* GCButtonElementName */ GCInputArcadeButtonName (nint row, nint column);

		// A strongly-typed API (the GCInputButtonName enum) is not possible, because it's not an exhaustive enum,
		// this method may return strings that aren't in the enum.
		/// <summary>Get the name of the arcade button for the specified position.</summary>
		/// <param name="row">The row of the arcade button.</param>
		/// <param name="column">The column of the arcade button.</param>
		/// <returns>The name of the arcade button on the controller for the specified position.</returns>
		[SupportedOSPlatform ("ios17.0")]
		[SupportedOSPlatform ("macos14.0")]
		[SupportedOSPlatform ("tvos17.0")]
		[SupportedOSPlatform ("maccatalyst17.0")]
		public static NSString? GetArcadeButtonName (nint row, nint column)
		{
			return Runtime.GetNSObject<NSString> (GCInputArcadeButtonName (row, column));
		}
	}
}
