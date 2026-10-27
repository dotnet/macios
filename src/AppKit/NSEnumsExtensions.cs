//
// NSEnumsExtensions.cs:
//
// Copyright 2021, Microsoft Corp
//

#nullable enable

namespace AppKit {

#if !__MACCATALYST__
	public static class NSImageResizingModeExtensions {
		public static nint ToNative (NSImageResizingMode value)
		{
#if !XAMCORE_5_0
			// For backwards compat reasons, the values we have in managed code corresponds with the X64 values.
			if (Runtime.IsARM64CallingConvention) {
				// Stretch and Tile are switched on arm64 on macOS
				switch (value) {
				case (NSImageResizingMode) 0:
					value = (NSImageResizingMode) 1;
					break;
				case (NSImageResizingMode) 1:
					value = (NSImageResizingMode) 0;
					break;
				}
			}
#endif
			return (nint) (long) value;
		}

		public static NSImageResizingMode ToManaged (nint value)
		{
#if !XAMCORE_5_0
			// For backwards compat reasons, the values we have in managed code corresponds with the X64 values.
			if (Runtime.IsARM64CallingConvention) {
				// Stretch and Tile are switched on arm64 on macOS
				switch (value) {
				case 0:
					value = (nint) 1;
					break;
				case 1:
					value = (nint) 0;
					break;
				}
			}
#endif
			return (NSImageResizingMode) (long) value;
		}
	}

	public static class NSTextAlignmentExtensions {
		public static nuint ToNative (NSTextAlignment value)
		{
#if !XAMCORE_5_0
			// For backwards compat reasons, the values we have in managed code corresponds with the X64 values.
			if (Runtime.IsARM64CallingConvention) {
				// Center and Right are switched on arm64 on macOS
				switch (value) {
				case (NSTextAlignment) 2:
					value = (NSTextAlignment) 1;
					break;
				case (NSTextAlignment) 1:
					value = (NSTextAlignment) 2;
					break;
				}
			}
#endif
			return (nuint) (ulong) value;
		}

		public static NSTextAlignment ToManaged (nuint value)
		{
#if !XAMCORE_5_0
			// For backwards compat reasons, the values we have in managed code corresponds with the X64 values.
			if (Runtime.IsARM64CallingConvention) {
				// Center and Right are switched on arm64 on macOS
				switch (value) {
				case 1:
					value = (nuint) 2;
					break;
				case 2:
					value = (nuint) 1;
					break;
				}
			}
#endif
			return (NSTextAlignment) (ulong) value;
		}
	}

	/// <summary>Converts image interpolation values between managed and native representations.</summary>
	public static class NSImageInterpolationExtensions {
		/// <summary>Converts an image interpolation value to its native representation.</summary>
		/// <param name="value">The managed interpolation value.</param>
		/// <returns>The native interpolation value.</returns>
		public static nuint ToNative (NSImageInterpolation value)
		{
#if !XAMCORE_5_0
			switch (value) {
			case NSImageInterpolation.Medium:
				return 4;
			case NSImageInterpolation.High:
				return 3;
			}
#endif
			return (nuint) (ulong) value;
		}

		/// <summary>Converts a native image interpolation value to its managed representation.</summary>
		/// <param name="value">The native interpolation value.</param>
		/// <returns>The managed interpolation value.</returns>
		public static NSImageInterpolation ToManaged (nuint value)
		{
#if !XAMCORE_5_0
			switch (value) {
			case 4:
				return NSImageInterpolation.Medium;
			case 3:
				return NSImageInterpolation.High;
			}
#endif
			return (NSImageInterpolation) (ulong) value;
		}
	}

	/// <summary>Converts slider tick-mark positions between managed and native representations.</summary>
	public static class NSTickMarkPositionExtensions {
		/// <summary>Converts a slider tick-mark position to its native representation.</summary>
		/// <param name="value">The managed tick-mark position.</param>
		/// <returns>The native tick-mark position.</returns>
		public static nuint ToNative (NSTickMarkPosition value)
		{
#if !XAMCORE_5_0
			switch (value) {
			case NSTickMarkPosition.Left:
				return 1;
			case NSTickMarkPosition.Right:
				return 0;
			}
#endif
			return (nuint) (ulong) value;
		}

		/// <summary>Converts a native slider tick-mark position to its managed representation.</summary>
		/// <param name="value">The native tick-mark position.</param>
		/// <returns>The managed position, using <see cref="NSTickMarkPosition.Above" /> and <see cref="NSTickMarkPosition.Below" /> for the shared native values.</returns>
		public static NSTickMarkPosition ToManaged (nuint value)
		{
			// Native leading/above and trailing/below are aliases, so return the canonical managed values.
			return (NSTickMarkPosition) (ulong) value;
		}
	}
#endif // !__MACCATALYST__
}

