// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using MapKit;

#nullable enable

namespace Foundation;

public partial class NSValue : NSObject {

	/// <summary>
	/// Converts a native handle to a MKCoordinateRegion.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The MKCoordinateRegion.</returns>
	public static MKCoordinateSpan ToMKCoordinateSpan (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CoordinateSpanValue;
	}

}
