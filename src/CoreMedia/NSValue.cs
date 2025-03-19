// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreMedia;

#nullable enable

namespace Foundation;

public partial class NSValue : NSObject {

	/// <summary>
	/// Converts a native handle to a CMTime.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CMTime.</returns>
	public static CMTimeRange ToCMTimeRange (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CMTimeRangeValue;
	}

	/// <summary>
	/// Converts a native handle to a CMTime.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CMTime.</returns>
	public static CMTime ToCMTime (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CMTimeValue;
	}

	/// <summary>
	/// Converts a native handle to a CMTimeMapping.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CMTimeMapping.</returns>
	public static CMTimeMapping ToCMTimeMapping (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CMTimeMappingValue;
	}

	/// <summary>
	/// Converts a native handle to a CMTimeRange.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CMTimeRange.</returns>
	public static CMVideoDimensions ToCMVideoDimensions (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CMVideoDimensionsValue;
	}

}
