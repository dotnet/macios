//
// VideoToolbox core types
//
// Authors: 
// 		Miguel de Icaza (miguel@xamarin.com)
//		Alex Soto (alex.soto@xamarin.com)
//
// Copyright 2014 Xamarin Inc
//

using System;
using Foundation;
using ObjCRuntime;

namespace VideoToolbox {

	public struct VTDataRateLimit
	{
		public uint NumberOfBytes { get; set; }
		public double Seconds { get; set; }

		public VTDataRateLimit (uint numberOfBytes, double seconds) : this ()
		{
			NumberOfBytes = numberOfBytes;
			Seconds = seconds;
		}
	}
}
