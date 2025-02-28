// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using UIKit;

#nullable enable

namespace Foundation;

public partial class NSValue : NSObject {

	/// <summary>
	/// Converts a native handle to a UIEdgeInsets.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The UIEdgeInsets.</returns>
	public static UIEdgeInsets ToUIEdgeInsets (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.UIEdgeInsetsValue;
	}

	/// <summary>
	/// Converts a native handle to a UIOffset.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The UIOffset.</returns>
	public static UIOffset ToUIOffset (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.UIOffsetValue;
	}

	/// <summary>
	/// Converts a native handle to a NSDirectionalEdgeInsets.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The NSDirectionalEdgeInsets.</returns>
	public static NSDirectionalEdgeInsets ToNSDirectionalEdgeInsets (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.DirectionalEdgeInsetsValue;
	}
}

