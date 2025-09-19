// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using ObjCRuntime;

namespace CoreGraphics {
	/// <summary>This struct contains values used when creating an adaptive bitmap context.</summary>
	[SupportedOSPlatform ("ios26.0")]
	[SupportedOSPlatform ("maccatalyst26.0")]
	[SupportedOSPlatform ("macos26.0")]
	[SupportedOSPlatform ("tvos26.0")]
	[StructLayout (LayoutKind.Sequential)]
	public struct CGContentInfo {
		CGComponent deepestImageComponent;  /* deepest image component */
		CGColorModel contentColorModels;     /* sum of all color models drawn */
		byte /* bool */ hasWideGamut;           /* there is content in wide gamut color space */
		byte /* bool */ hasTransparency;        /* there is transparent content */
		float largestContentHeadroom;

		/// <summary>The deepest image component.</summary>
		public CGComponent DeepestImageComponent {
			get => deepestImageComponent;
			set => deepestImageComponent = value;
		}

		/// <summary>The sum of all drawn color models.</summary>
		public CGColorModel ContentColorModels {
			get => contentColorModels;
			set => contentColorModels = value;
		}

		/// <summary>Whether there's content in the wide gamut colorspace or not.</summary>
		public bool HasWideGamut {
			get => hasWideGamut != 0;
			set => hasWideGamut = value.AsByte ();
		}

		/// <summary>Whether there's transparent content or not.</summary>
		public bool HasTransparency {
			get => hasTransparency != 0;
			set => hasTransparency = value.AsByte ();
		}

		/// <summary>The largest content headroom value.</summary>
		public float LargestContentHeadroom {
			get => largestContentHeadroom;
			set => largestContentHeadroom = value;
		}

		/// <summary>Returns a string representation of this <see cref="CGContentInfo" />.</summary>
		public override string ToString ()
		{
			return $"CGContentInfo [DeepestImageComponent={DeepestImageComponent};ContentColorModels={ContentColorModels};HasWideGamut={HasWideGamut};HasTransparency={HasTransparency};LargestContentHeadroom={LargestContentHeadroom}]";
		}
	}
}
