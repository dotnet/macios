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
	/// <summary>Represents an ordered pair of floating-point x- and y-coordinates that defines a point in a two-dimensional plane.</summary>
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

		/// <summary>Gets or sets the x-coordinate of this <see cref="GCPoint2" />.</summary>
		/// <value>The x-coordinate of this <see cref="GCPoint2" />.</value>
		public float X {
			get { return x; }
			set { x = value; }
		}

		/// <summary>Gets or sets the y-coordinate of this <see cref="GCPoint2" />.</summary>
		/// <value>The y-coordinate of this <see cref="GCPoint2" />.</value>
		public float Y {
			get { return y; }
			set { y = value; }
		}

#if !COREBUILD
		/// <summary>Represents a new instance of the <see cref="GCPoint2" /> where both the <see cref="X" /> and <see cref="Y"/> coordinates are 0.</summary>
		public static readonly GCPoint2 Zero;

		/// <summary>Compares two <see cref="GCPoint2" /> values for equality.</summary>
		/// <param name="l">The left-hand side of the comparison.</param>
		/// <param name="r">The right-hand side of the comparison.</param>
		/// <returns><see langword="true" /> if <paramref name="l" /> and <paramref name="r" /> are equal, otherwise <see langword="false" />.</returns>
		public static bool operator == (GCPoint2 l, GCPoint2 r)
		{
			return l.Equals (r);
		}

		/// <summary>Compares two <see cref="GCPoint2" /> values for inequality.</summary>
		/// <param name="l">The left-hand side of the comparison.</param>
		/// <param name="r">The right-hand side of the comparison.</param>
		/// <returns><see langword="true" /> if <paramref name="l" /> and <paramref name="r" /> are not equal, otherwise <see langword="false" />.</returns>
		public static bool operator != (GCPoint2 l, GCPoint2 r)
		{
			return !l.Equals (r);
		}

		/// <summary>Gets a value indicating whether this <see cref="GCPoint2" /> is empty.</summary>
		/// <value><see langword="true" /> if both <see cref="X" /> and <see cref="Y" /> are 0,, otherwise <see langword="false" />.</value>
		public bool IsEmpty {
			get { return x == 0.0 && y == 0.0; }
		}

		/// <summary>Initializes a new instance of the <see cref="GCPoint2" /> struct with the specified coordinates.</summary>
		/// <param name="x">The X coordinate of the point.</param>
		/// <param name="y">The X coordinate of the point.</param>
		public GCPoint2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>Initializes a new instance of the <see cref="GCPoint2" /> struct from the specified <see cref="GCPoint2" />.</summary>
		/// <param name="point">The source point.</param>
		public GCPoint2 (GCPoint2 point)
		{
			this.x = point.x;
			this.y = point.y;
		}

		/// <summary>Specifies whether this <see cref="GCPoint2" /> contains the same coordinates as the specified <see cref="object" />.</summary>
		/// <param name="obj">The <see cref="object" /> to compare with this object.</param>
		/// <returns><see langword="true" /> if <paramref name="obj" /> is a <see cref="GCPoint2" /> with the same coordinates as this point, otherwise <see langword="false" />.</returns>
		public override bool Equals (object? obj)
		{
			return (obj is GCPoint2 t) && Equals (t);
		}

		/// <summary>Specifies whether this <see cref="GCPoint2" /> contains the same coordinates as the specified <see cref="GCPoint2" />.</summary>
		/// <param name="point">The <see cref="GCPoint2" /> to compare with this object.</param>
		/// <returns><see langword="true" /> if <paramref name="point" /> has the same coordinates as this point, otherwise <see langword="false" />.</returns>
		public bool Equals (GCPoint2 point)
		{
			return point.x == x && point.y == y;
		}

		/// <summary>Returns a hash code for this <see cref="GCPoint2" /> structure.</summary>
		/// <returns>An integer value that specifies a hash value for this <see cref="GCPoint2" /> structure.</returns>
		public override int GetHashCode ()
		{
			return HashCode.Combine (x, y);
		}

		/// <summary>Deconstructs <see cref="GCPoint2"/> by <see cref="X"/> and <see cref="Y"/>.</summary>
		/// <param name="x">Deconstructed parameter for <see cref="X"/>.</param>
		/// <param name="y">Deconstructed parameter for <see cref="Y"/>.</param>
		public void Deconstruct (out nfloat x, out nfloat y)
		{
			x = X;
			y = Y;
		}

		/// <summary>Converts this <see cref="GCPoint2" /> to a human readable string.</summary>
		/// <returns>A string that represents this <see cref="GCPoint2" />.</returns>
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
