// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using CoreAnimation;

#nullable enable

namespace Foundation;

public partial class NSValue : NSObject {

	/// <summary>
	/// Converts a native handle to a CATransform3D.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The CATransform3D.</returns>
	public static CATransform3D ToCATransform3D (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.CATransform3DValue;
	}
}
