#nullable enable

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using Foundation;
using ObjCRuntime;

namespace Photos {
	public partial class PHCloudIdentifier {
		/// <summary>Creates a cloud identifier from its archival string representation.</summary>
		/// <param name="archivalStringValue">The archival string representation of the cloud identifier.</param>
		/// <returns>A new cloud identifier, or <see langword="null" /> if the archival string is invalid.</returns>
		[SupportedOSPlatform ("ios18.2")]
		[SupportedOSPlatform ("tvos18.2")]
		[SupportedOSPlatform ("macos15.2")]
		[SupportedOSPlatform ("maccatalyst18.2")]
		public static PHCloudIdentifier? Create (string archivalStringValue)
		{
			ArgumentNullException.ThrowIfNull (archivalStringValue);

			var rv = new PHCloudIdentifier (NSObjectFlag.Empty);
			rv.InitializeHandle (rv._InitWithArchivalStringValue (archivalStringValue), "initWithArchivalStringValue:", false);
			if (rv.Handle == NativeHandle.Zero) {
				rv.Dispose ();
				return null;
			}
			return rv;
		}
	}
}
