//
// NSGradient: Extensions to the API for NSGradient
//
// Author:
//   Regan Sarwas (find me on gmail as rsarwas)
//

#if !__MACCATALYST__

using System;
using Foundation;
using ObjCRuntime;
using System.Runtime.InteropServices;

#nullable enable

namespace AppKit {
	public partial class NSGradient : NSObject {
		// The signature of this ObjC method is
		// - (id)initWithColorsAndLocations:(NSColor *)firstColor, ... NS_REQUIRES_NIL_TERMINATION;
		// where colors and locations (as CGFloats between 0.0 and 1.0) alternate until nil
		// ObjC example: 
		//    NSGradient *gradient = [[NSGradient alloc] initWithColorsAndLocations: [NSColor blackColor], 0.0,
		//                                                                           [NSColor blueColor], 0.33,
		//                                                                           [NSColor redColor], 1.0, nil];
		// which is a very un-C# thing to do.  The best correlation would be
		//   NSGradient (NSColor[] colors, float[] locations)
		// C# example:
		//    NSGradient gradient = new NSGradient(new[] {NSColor.Black, NSColor.Blue, NSColor.Red},
		//                                         new[] { 0.0f, 0.33f, 1.0f});
		// Per NSGradient.h, this initializer calls the designated initializer (below) with a
		// color space of NSColorSpace.GenericRGBColorSpace, so we will do the same.

		/// <summary>Create a new <see cref="NSGradient" /> instance with the specified colors and color locations.</summary>
		/// <param name="colors">The colors for the new gradient.</param>
		/// <param name="locations">The locations for each color in the new gradient. Each location must be a value between 0.0 and 1.0.</param>
		/// <remarks>This gradient is created in the <see cref="NSColorSpace.GenericRGBColorSpace" /> generic RGB color space.</remarks>
		public NSGradient (NSColor [] colors, float [] locations) :
			this (colors, locations, NSColorSpace.GenericRGBColorSpace)
		{
		}

		/// <summary>Create a new <see cref="NSGradient" /> instance with the specified colors and color locations.</summary>
		/// <param name="colors">The colors for the new gradient.</param>
		/// <param name="locations">The locations for each color in the new gradient. Each location must be a value between 0.0 and 1.0.</param>
		/// <remarks>This gradient is created in the <see cref="NSColorSpace.GenericRGBColorSpace" /> generic RGB color space.</remarks>
		public NSGradient (NSColor [] colors, double [] locations) :
			this (colors, locations, NSColorSpace.GenericRGBColorSpace)
		{
		}

		/// <summary>Create a new <see cref="NSGradient" /> instance with the specified colors and color locations.</summary>
		/// <param name="colors">The colors for the new gradient.</param>
		/// <param name="locations">The locations for each color in the new gradient. Each location must be a value between 0.0 and 1.0.</param>
		/// <remarks>This gradient is created in the <see cref="NSColorSpace.GenericRGBColorSpace" /> generic RGB color space.</remarks>
		public NSGradient (NSColor [] colors, nfloat [] locations)
			: this (colors, locations, NSColorSpace.GenericRGBColorSpace)
		{
		}

		/// <summary>Create a new <see cref="NSGradient" /> instance with the specified colors and color locations.</summary>
		/// <param name="colors">The colors for the new gradient.</param>
		/// <param name="locations">The locations for each color in the new gradient. Each location must be a value between 0.0 and 1.0.</param>
		/// <param name="colorSpace">The color space for the new gradient.</param>
		public NSGradient (NSColor [] colors, float [] locations, NSColorSpace colorSpace)
			: this (colors, Array.ConvertAll<float, nfloat> (locations, new Converter<float, nfloat> (a => (nfloat) a)), colorSpace)
		{
		}

		/// <summary>Create a new <see cref="NSGradient" /> instance with the specified colors and color locations.</summary>
		/// <param name="colors">The colors for the new gradient.</param>
		/// <param name="locations">The locations for each color in the new gradient. Each location must be a value between 0.0 and 1.0.</param>
		/// <param name="colorSpace">The color space for the new gradient.</param>
		public NSGradient (NSColor [] colors, double [] locations, NSColorSpace colorSpace) : base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (void* locationPtr = locations) {
					Initialize (colors, locations?.Length, (IntPtr) locationPtr, colorSpace);
				}
			}
		}

		/// <summary>Create a new <see cref="NSGradient" /> instance with the specified colors and color locations.</summary>
		/// <param name="colors">The colors for the new gradient.</param>
		/// <param name="locations">The locations for each color in the new gradient. Each location must be a value between 0.0 and 1.0.</param>
		/// <param name="colorSpace">The color space for the new gradient.</param>
		public NSGradient (NSColor [] colors, nfloat [] locations, NSColorSpace colorSpace)
			: base (NSObjectFlag.Empty)
		{
			unsafe {
				fixed (void* locationPtr = locations) {
					Initialize (colors, locations?.Length, (IntPtr) locationPtr, colorSpace);
				}
			}
		}

		void Initialize (NSColor [] colors, int? locationCount, IntPtr locations, NSColorSpace colorSpace)
		{
			if (colors is null)
				ThrowHelper.ThrowArgumentNullException (nameof (colors));

			if (locationCount is null)
				ThrowHelper.ThrowArgumentNullException (nameof (locations));

			if (colors.Length != locationCount)
				ThrowHelper.ThrowArgumentOutOfRangeException (nameof (locations), string.Format ("The number of colors ({0}) and the number of locations ({1}) must be equal.", colors.Length, locationCount));

			InitializeHandle (_InitWithColorsAtLocationsAndColorSpace (colors, locations, colorSpace), "initWithColors:atLocations:colorSpace:");
		}
	}
}
#endif // !__MACCATALYST__
