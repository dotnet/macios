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
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct GCPoint2
#if !COREBUILD
		: IEquatable<GCPoint2>
#endif
	{
		float x;
		float y;

		public float X {
			get { return x; }
			set { x = value; }
		}

		public float Y {
			get { return y; }
			set { y = value; }
		}

#if !COREBUILD
		public static readonly GCPoint2 Zero;

		public static bool operator == (GCPoint2 l, GCPoint2 r)
		{
			// the following version of Equals cannot be removed by the linker, while == can be
			return l.Equals (r);
		}

		public static bool operator != (GCPoint2 l, GCPoint2 r)
		{
			return l.x != r.x || l.y != r.y;
		}

		public bool IsEmpty {
			get { return x == 0.0 && y == 0.0; }
		}

		public GCPoint2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public GCPoint2 (GCPoint2 point)
		{
			this.x = point.x;
			this.y = point.y;
		}

		public override bool Equals (object? obj)
		{
			return (obj is GCPoint2 t) && Equals (t);
		}

		public bool Equals (GCPoint2 point)
		{
			return point.x == x && point.y == y;
		}

		public override int GetHashCode ()
		{
			return HashCode.Combine (x, y);
		}

		public void Deconstruct (out nfloat x, out nfloat y)
		{
			x = X;
			y = Y;
		}

		public override string? ToString ()
		{
			if (OperatingSystem.IsMacOSVersionAtLeast (14, 3) ||
				OperatingSystem.IsMacCatalystVersionAtLeast (17, 4) ||
				OperatingSystem.IsIOSVersionAtLeast (17, 4) ||
				OperatingSystem.IsTvOSVersionAtLeast (17, 4))
				return CFString.FromHandle (NSStringFromGCPoint2 (this));
			return $"{{{x}, {y}}}";
		}

		[SupportedOSPlatform ("ios17.4")]
		[SupportedOSPlatform ("tvos17.4")]
		[SupportedOSPlatform ("maccatalyst17.4")]
		[SupportedOSPlatform ("macos14.3")]
		[DllImport (Constants.GameControllerLibrary)]
		extern static /* NSString* */ IntPtr NSStringFromGCPoint2 (/* GCPoint2 */ GCPoint2 point);
#endif // !COREBUILD
	}
}
