// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Runtime.InteropServices;

using ObjCRuntime;
using SceneKit;

#nullable enable

namespace Foundation;

public partial class NSValue : NSObject {

	/// <summary>
	/// Converts a native handle to a SCNMatrix4.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The SCNMatrix4.</returns>	
	public static SCNMatrix4 ToSCNMatrix4 (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.SCNMatrix4Value;
	}

	/// <summary>
	/// Converts a native handle to a NSRange.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The NSRange.</returns>
	public static NSRange ToNSRange (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.RangeValue;
	}

	/// <summary>
	/// Converts a native handle to a SCNVector3.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The SCNVector3.</returns>
	public static SCNVector3 ToSCNVector3 (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.Vector3Value;
	}

	/// <summary>
	/// Converts a native handle to a SCNVector4.
	/// </summary>
	/// <param name="handle">The native handle.</param>
	/// <returns>The SCNVector4.</returns>
	public static SCNVector4 ToSCNVector4 (NativeHandle handle)
	{
		using var nsvalue = Runtime.GetNSObject<NSValue> (handle)!;
		return nsvalue.Vector4Value;
	}

}
