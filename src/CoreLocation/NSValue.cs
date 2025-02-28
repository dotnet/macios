// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreLocation;

#nullable enable

namespace Foundation;

public partial class NSValue : NSObject {

	/// <summary>
	/// Converts a native handle to a CLLocationCoordinate2D.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CLLocationCoordinate2D.</returns>
	public static CLLocationCoordinate2D ToCLLocationCoordinate2D (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CoordinateValue;
	}
}
