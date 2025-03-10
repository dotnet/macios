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

		public static AVDepthData? Create (CGImageAuxiliaryDataInfo dataInfo, out NSError? error)
		{
			return Create (dataInfo.Dictionary, out error);
		}

		/// <summary>Returns an array of depth data types that are suitable for use with <format type="text/html"><a href="https://docs.microsoft.com/en-us/search/index?search=M:AVFoundation.AVDepthData.Create(CoreVideo.CVPixelFormatType)&amp;scope=Xamarin" title="M:AVFoundation.AVDepthData.Create(CoreVideo.CVPixelFormatType)">M:AVFoundation.AVDepthData.Create(CoreVideo.CVPixelFormatType)</a></format>.</summary>
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
