//
// AVDepthData.cs
//
// Authors:
//	Alex Soto (alexsoto@microsoft.com)
//
// Copyright 2017 Xamarin Inc.
//

using CoreVideo;
using ImageIO;

#nullable enable

namespace AVFoundation {
	public partial class AVDepthData {

		/// <summary>Creates depth data from the specified auxiliary image data.</summary>
		/// <param name="dataInfo">The auxiliary image data from which to create the depth data.</param>
		/// <param name="error">The error that occurred, or <see langword="null" /> if no error occurred.</param>
		/// <returns>The created depth data, or <see langword="null" /> if it could not be created.</returns>
		public static AVDepthData? Create (CGImageAuxiliaryDataInfo dataInfo, out NSError? error)
		{
			return Create (dataInfo.Dictionary, out error);
		}

		/// <summary>Gets the depth data types that are suitable for use with <see cref="AVFoundation.AVDepthData.Create(CGImageAuxiliaryDataInfo, out NSError)" />.</summary>
		/// <value>The available depth data pixel formats, an empty array if no formats are available, or <see langword="null" /> if the underlying value is unavailable.</value>
		public CVPixelFormatType []? AvailableDepthDataTypes {
			get {
				var values = WeakAvailableDepthDataTypes;
				if (values is null)
					return null;

				var count = values.Length;
				var arr = new CVPixelFormatType [count];
				for (int i = 0; i < count; i++)
					arr [i] = (CVPixelFormatType) values [i].UInt32Value; // CVPixelFormatType is uint.

				return arr;
			}
		}
	}
}
