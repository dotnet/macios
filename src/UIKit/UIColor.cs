//
// UIColor.cs: Extensions to UIColor
//
// Authors:
//   Geoff Norton
//
// Copyright 2009, Novell, Inc.
//

using System;
using ObjCRuntime;
#if !COREBUILD
using CoreGraphics;
#endif

namespace UIKit {

	public partial class UIColor {

		/// <param name="red">Red component, 0.0 to 1.0f.</param>
		/// <param name="green">Green component 0.0 to 1.0f.</param>
		/// <param name="blue">Blue component value 0.0 to 1.0f.</param>
		/// <summary>Creates a solid color using the red, green and blue components specified.</summary>
		/// <returns>
		///         </returns>
		/// <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIColor FromRGB (nfloat red, nfloat green, nfloat blue)
		{
			return FromRGBA (red, green, blue, 1.0f);
		}

		/// <param name="red">Red component, 0 to 255.</param>
		///         <param name="green">Green component 0 to 255.</param>
		///         <param name="blue">Blue component value 0 to 255.</param>
		///         <summary>Creates a solid color using the red, green and blue components specified.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIColor FromRGB (byte red, byte green, byte blue)
		{
			return FromRGBA (red / 255.0f, green / 255.0f, blue / 255.0f, 1.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <summary>Creates a color from the specified combinated of red, green, and blue components.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIColor FromRGB (int red, int green, int blue)
		{
			return FromRGB ((byte) red, (byte) green, (byte) blue);
		}

		/// <param name="red">Red component, 0 to 255.</param>
		///         <param name="green">Green component 0 to 255.</param>
		///         <param name="blue">Blue component value 0 to 255.</param>
		///         <param name="alpha">Alpha (transparency) value 0 to 255.</param>
		///         <summary>Creates a color with the specified alpha transparency using the red, green and blue components specified.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIColor FromRGBA (byte red, byte green, byte blue, byte alpha)
		{
			return FromRGBA (red / 255.0f, green / 255.0f, blue / 255.0f, alpha / 255.0f);
		}

		/// <param name="red">To be added.</param>
		///         <param name="green">To be added.</param>
		///         <param name="blue">To be added.</param>
		///         <param name="alpha">To be added.</param>
		///         <summary>Creates a color from the specified combination of red, green, and blue elements, with the specified transparency.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIColor FromRGBA (int red, int green, int blue, int alpha)
		{
			return FromRGBA ((byte) red, (byte) green, (byte) blue, (byte) alpha);
		}

		/// <param name="hue">Hue component value from 0.0 to 1.0f.</param>
		/// <param name="saturation">Saturation component value from 0.0 to 1.0f</param>
		/// <param name="brightness">Brightness component value from 0.0 to 1.0f.</param>
		/// <summary>Creates a color from using the hue, saturation and brightness components.</summary>
		/// <returns>
		///         </returns>
		/// <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public static UIColor FromHSB (nfloat hue, nfloat saturation, nfloat brightness)
		{
			return FromHSBA (hue, saturation, brightness, 1.0f);
		}

		// note: replacing this managed code with "getRed:green:blue:alpha:" would break RGB methods
		/// <param name="red">Red component, 0.0 to 1.0f.</param>
		/// <param name="green">Green component 0.0 to 1.0f.</param>
		/// <param name="blue">Blue component value 0.0 to 1.0f.</param>
		/// <param name="alpha">Alpha (transparency) value from 0.0 to 1.0f.</param>
		/// <summary>Returns the red, green, blue and alpha components of this color.</summary>
		/// <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public void GetRGBA (out nfloat red, out nfloat green, out nfloat blue, out nfloat alpha)
		{
			using (var cv = CGColor) {
				nfloat [] result = cv.Components;

				switch (result.Length) {
				case 2:
					red = result [0];
					green = result [0];
					blue = result [0];
					alpha = result [1];
					break;
				case 3:
					red = result [0];
					green = result [1];
					blue = result [2];
					alpha = 1.0f;
					break;
				case 4:
					red = result [0];
					green = result [1];
					blue = result [2];
					alpha = result [3];
					break;
				default:
					throw new Exception ("Unsupported colorspace component length: " + result.Length);
				}
			}
		}

		static nfloat Max (nfloat a, nfloat b)
		{
			return a > b ? a : b;
		}

		static nfloat Min (nfloat a, nfloat b)
		{
			return a < b ? a : b;
		}

		// note: replacing this managed code with "getHue:saturation:brightness:alpha:" would break HSB methods
		/// <param name="hue">Hue component value from 0.0 to 1.0f.</param>
		/// <param name="saturation">Saturation component value from 0.0 to 1.0f</param>
		/// <param name="brightness">Brightness component value from 0.0 to 1.0f.</param>
		/// <param name="alpha">Alpha (transparency) value from 0.0 to 1.0f.</param>
		/// <summary>Returns the hue, saturation, brightness and alpha components of the color.</summary>
		/// <remarks>
		///           <para>
		///           </para>
		///           <para tool="threads">This can be used from a background thread.</para>
		///         </remarks>
		public void GetHSBA (out nfloat hue, out nfloat saturation, out nfloat brightness, out nfloat alpha)
		{
			using (var cv = CGColor) {
				nfloat [] result = cv.Components;
				nfloat red, green, blue;

				switch (result.Length) {
				case 2:
					red = result [0];
					green = result [0];
					blue = result [0];
					alpha = result [1];
					break;
				case 3:
					red = result [0];
					green = result [1];
					blue = result [2];
					alpha = 1.0f;
					break;
				case 4:
					red = result [0];
					green = result [1];
					blue = result [2];
					alpha = result [3];
					break;
				default:
					throw new Exception ("Unsupported colorspace component length: " + result.Length);
				}


				var maxv = Max (red, Max (green, blue));
				var minv = Min (red, Min (green, blue));

				brightness = maxv;

				var delta = maxv - minv;
				if (maxv != 0.0)
					saturation = delta / maxv;
				else
					saturation = 0;

				if (saturation == 0)
					hue = 0;
				else {
					if (red == brightness)
						hue = (green - blue) / delta;
					else if (green == maxv)
						hue = 2.0f + (blue - red) / delta;
					else
						hue = 4.0f + (red - green) / delta;

					hue = hue / 6.0f;
					if (hue <= 0.0f)
						hue += 1.0f;
				}
			}
		}

		/// <summary>Returns a string representation of the value of the current instance.</summary>
		///         <returns>
		///         </returns>
		///         <remarks>
		///         </remarks>
		public override string ToString ()
		{
			nfloat r, g, b, a;
			try {
				GetRGBA (out r, out g, out b, out a);
				return String.Format ("UIColor [A={0}, R={1}, G={2}, B={3}]",
					(byte) (a * 255), (byte) (r * 255), (byte) (g * 255), (byte) (b * 255));
			} catch (Exception) {
				// e.g. patterns will return "kCGColorSpaceModelPattern 1", see bug #7362
				return Description;
			}
		}

#if false
		public static UIColor FromColor (System.Drawing.Color color)
		{
			return new UIColor (color.R/255.0f, color.G/255.0f, color.B/255.0f, color.A/255.0f);
		}
#endif
	}
}
