// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#nullable enable

#if __MACOS__ || __MACCATALYST__

namespace Foundation {
	/// <summary>Provides conversions from four-character strings to Apple event keywords.</summary>
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[UnsupportedOSPlatform ("ios")]
	[UnsupportedOSPlatform ("tvos")]
	public static class AEKeywordExtensions {
		/// <summary>Converts a four-character string to an Apple event keyword.</summary>
		/// <param name="self">Exactly four characters, each with a value between 0 and 255.</param>
		/// <returns>The keyword with the first character in its most significant byte.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="self" /> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="self" /> does not contain exactly four single-byte characters.</exception>
		public static AEKeyword ToAEKeyword (this string self)
		{
			ArgumentNullException.ThrowIfNull (self);
			if (self.Length != 4)
				throw new ArgumentException ("An Apple event keyword must contain exactly four characters.", nameof (self));

			uint value = 0;
			foreach (var character in self) {
				if (character > byte.MaxValue)
					throw new ArgumentException ("Each Apple event keyword character must fit in a single byte.", nameof (self));
				value = (value << 8) | character;
			}
			return (AEKeyword) value;
		}
	}
}

#endif
