//
// VideoToolbox core types
//
// Authors: 
// 		Miguel de Icaza (miguel@xamarin.com)
//		Alex Soto (alex.soto@xamarin.com)
//
// Copyright 2014 Xamarin Inc
//

#nullable enable

using System;
using Foundation;
using ObjCRuntime;
using System.Runtime.Versioning;

namespace VideoToolbox {
	/// <summary>Strongly typed representation of bytes and seconds used in <see cref="VideoToolbox.VTCompressionProperties.DataRateLimits" />.</summary>
	///     <remarks>To be added.</remarks>
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("maccatalyst")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("tvos")]
	public struct VTDataRateLimit {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public uint NumberOfBytes { get; set; }
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		public double Seconds { get; set; }

		/// <param name="numberOfBytes">To be added.</param>
		///         <param name="seconds">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public VTDataRateLimit (uint numberOfBytes, double seconds) : this ()
		{
			NumberOfBytes = numberOfBytes;
			Seconds = seconds;
		}
	}
}
