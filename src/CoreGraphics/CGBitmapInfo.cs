// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace CoreGraphics {
	public static class CGBitmapInfoExtensions {
		/// <summary>Get the alpha info flags for this <see cref="CGBitmapInfo" /> value.</summary>
		/// <param name="value">The <see cref="CGBitmapInfo" /> value with the flags to get.</param>
		/// <returns>The alpha info flags for this <see cref="CGBitmapInfo" /> value.</returns>
		public static CGImageAlphaInfo GetAlphaInfo (this CGBitmapInfo value)
		{
			return (CGImageAlphaInfo) (value & CGBitmapInfo.AlphaInfoMask);
		}

		/// <summary>Get the component info flags for this <see cref="CGBitmapInfo" /> value.</summary>
		/// <param name="value">The <see cref="CGBitmapInfo" /> value with the flags to get.</param>
		/// <returns>The component info flags for this <see cref="CGBitmapInfo" /> value.</returns>
		public static CGImageComponentInfo GetComponentInfo (this CGBitmapInfo value)
		{
			return (CGImageComponentInfo) (value & CGBitmapInfo.ComponentInfoMask);
		}

		/// <summary>Get the byte order info flags for this <see cref="CGBitmapInfo" /> value.</summary>
		/// <param name="value">The <see cref="CGBitmapInfo" /> value with the flags to get.</param>
		/// <returns>The byte order info flags for this <see cref="CGBitmapInfo" /> value.</returns>
		public static CGImageByteOrderInfo GetByteOrderInfo (this CGBitmapInfo value)
		{
			return (CGImageByteOrderInfo) (value & CGBitmapInfo.ByteOrderInfoMask);
		}

		/// <summary>Get the pixel format info flags for this <see cref="CGBitmapInfo" /> value.</summary>
		/// <param name="value">The <see cref="CGBitmapInfo" /> value with the flags to get.</param>
		/// <returns>The pixel formatinfo flags for this <see cref="CGBitmapInfo" /> value.</returns>
		public static CGImagePixelFormatInfo GetPixelFormatInfo (this CGBitmapInfo value)
		{
			return (CGImagePixelFormatInfo) (value & CGBitmapInfo.PixelFormatInfoMask);
		}
	}
}
