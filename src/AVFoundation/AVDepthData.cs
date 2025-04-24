//
// AVDepthData.cs
//
// Authors:
//	Alex Soto (alexsoto@microsoft.com)
//
// Copyright 2017 Xamarin Inc.
//

using System;
using Foundation;
using ObjCRuntime;
using CoreVideo;
using ImageIO;

#nullable enable

namespace AVFoundation {
	public partial class AVDepthData {

		/// <param name="dataInfo">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Creates and returns a new AVDepthData object with the specified values.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		public static AVDepthData? Create (CGImageAuxiliaryDataInfo dataInfo, out NSError? error)
		{
			return Create (dataInfo.Dictionary, out error);
		}

		/// <summary>Returns an array of depth data types that are suitable for use with <see cref="AVFoundation.AVDepthData.Create(CGImageAuxiliaryDataInfo, out NSError)" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
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
